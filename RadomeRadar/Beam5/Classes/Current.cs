using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    /// <summary>
    /// Распределение электрических и магнитных токов
    /// </summary>
    public class Current
    {
        //Поля         
        public CurrentElement[] I;
        public CurrentElement[] M;
        public Point3D[] P;
        public double[] S;    
        
        CurrentStructure[] currentStructure;
        public CurrentStructure this[int index]
        {
            get
            {
                return currentStructure[index];
            }
            set
            {
                currentStructure[index] = value;
            }
        }
        
        
        //Конструкторы
        /// <summary>
        /// Создаёт экземпляр класса Current
        /// </summary>
        public Current(CVector[] VectorI,CVector[] VectorM, Point3D[] Segment, double[] Square)
        {
            int length = Segment.Length;
            I = new CurrentElement[length];
            M = new CurrentElement[length];
            P = new Point3D[length];
            S = new double[length];

            currentStructure = new CurrentStructure[length];

            for (int i = 0; i < length; i++)
            {
                I[i] = new CurrentElement(VectorI[i]);
                M[i] = new CurrentElement(VectorM[i]);
                P[i] = Segment[i];
                S[i] = Square[i];
                currentStructure[i] = new CurrentStructure() { I = I[i], M = M[i], P = P[i], S = S[i] };
            }
        }
        
        //Свойства
        public int Count
        {
            get
            {
                return P.Length;
            }
        }

        //Статические методы класса        
        public static Current CurrentsExitetion(NearFieldC incidentfield, Radome obj, Direction direction)
        {
            double nx, ny, nz;
            Complex ex, ey, ez, hx, hy, hz;
            Complex ix, iy, iz, mx, my, mz;            

            int trianglesNumber = incidentfield.Count;

            CVector[] I = new CVector[trianglesNumber];
            CVector[] M = new CVector[trianglesNumber];            
            Point3D[] Place = new Point3D[trianglesNumber];
            double[] Area = new double[trianglesNumber];

            int h = 0;
            for (int r = 0; r < obj.Count; r++)
            {
                RadomeElement geomObj = obj[r];
                int countElements = geomObj.Count;
                for (int j = 0; j < countElements; j++)
                {
                    //
                    // Выбор нормали
                    //  
                    //Triangle element = geomObj.triangles[j];

                    double upDown = 1;

                    if (direction == Direction.Inside)
                    {
                        upDown = -1;
                    }

                    nx = upDown * geomObj[j].Norma.X;
                    ny = upDown * geomObj[j].Norma.Y;
                    nz = upDown * geomObj[j].Norma.Z;


                    //
                    // Загрузка полей
                    //  
                    ex = incidentfield[h].E.X;
                    ey = incidentfield[h].E.Y;
                    ez = incidentfield[h].E.Z;

                    hx = incidentfield[h].H.X;
                    hy = incidentfield[h].H.Y;
                    hz = incidentfield[h].H.Z;

                    //
                    // Расчет возбуждаемых токов
                    // 
                    ix = hz * ny - hy * nz;        //x - компонента электрического тока на внутренней стороне укрытия
                    iy = hx * nz - hz * nx;        //y - компонента электрического тока на внутренней стороне укрытия
                    iz = hy * nx - hx * ny;        //z - компонента электрического тока на внутренней стороне укрытия

                    mx = nz * ey - ny * ez;        //x - компонента магнитного тока на внутренней стороне укрытия
                    my = nx * ez - nz * ex;        //y - компонента магнитного тока на внутренней стороне укрытия
                    mz = ny * ex - nx * ey;        //z - компонента магнитного тока на внутренней стороне укрытия 

                    I[h] = new CVector(ix, iy, iz);
                    M[h] = new CVector(mx, my, mz);
                    Place[h] = geomObj[j].Center;
                    Area[h] = geomObj[j].Area;
                    h++;
                }
            }
            return new Current(I, M, Place, Area);
        }

        public static CVector ChangeLoadedCurrentAnlorithm(CVector v, DVector n, double angle)
        {
            float rangle = (float)(angle / 180 * CV.pi);
            SlimDX.Matrix m = SlimDX.Matrix.RotationAxis(new SlimDX.Vector3((float)n.X, (float)n.Y, (float)n.Z), rangle);
            SlimDX.Vector3 reV = new SlimDX.Vector3((float)v.X.Real, (float)v.Y.Real, (float)v.Z.Real);
            SlimDX.Vector3 imV = new SlimDX.Vector3((float)v.X.Imaginary, (float)v.Y.Imaginary, (float)v.Z.Imaginary);

            SlimDX.Vector3 reVrot = SlimDX.Vector3.TransformCoordinate(reV, m);
            SlimDX.Vector3 imVrot = SlimDX.Vector3.TransformCoordinate(imV, m);

            return new CVector(new Complex(reVrot.X, imVrot.X), new Complex(reVrot.Y, imVrot.Y), new Complex(reVrot.Z, imVrot.Z));
        }        
    }
    
    /// <summary>
    /// Элементарная ячейка распределения со значением электрического и магнитного тока
    /// </summary>
    public class CurrentElement
    {
        public Complex X { get; set; }
        public Complex Y { get; set; }
        public Complex Z { get; set; }

       
        public CVector CVector
        {
            get
            {
                return new CVector(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        
        public CurrentElement(CVector cvec)
        {
            this.X = cvec.X;
            this.Y = cvec.Y;
            this.Z = cvec.Z;            
        }
    }

    public class CurrentStructure
    {
        /// <summary>
        /// Электрический ток
        /// </summary>
        public CurrentElement I;
        /// <summary>
        /// Магнитный ток
        /// </summary>
        public CurrentElement M;
        /// <summary>
        /// Положение поверхности тока
        /// </summary>
        public Point3D P;
        /// <summary>
        /// Площадь поверхностного тока
        /// </summary>
        public double S;
    }
}
