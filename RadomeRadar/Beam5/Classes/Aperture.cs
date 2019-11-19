using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class Aperture : Mesh
    {
        /// <summary>
        /// Скорость света (м/с) = 1 / Math.Sqrt(E_0 * Mu_0)
        /// </summary>

        //public const double Z_0 = 120 * Math.PI;                            // sqrt(Mu_0/E_0) = 120pi Ом    impedance of free space   волновое сопротивление свободного пространства        

        //Current electricCurrent;
        //Current magneticCurrent;
        public Current ApertureCurrent;

        public string workPlane = "";
        //public Mesh antennaMesh;        
        double R { get; set; }

        public Aperture(List<double> x, List<double> y, List<double> z, List<int> i1, List<int> i2, List<int> i3)
            : base(ref x, ref y, ref z, ref i1, ref i2, ref i3)
        {
            FindParametersForRoundApperture();
        }

        public Aperture(Point3D[] points, double[] area, DVector[] norma)
            : base(points, area, norma)
        {

        }

        public Aperture(ref Mesh antennaMesh)
        {
            // TODO: Complete member initialization
            this.elements = antennaMesh.elements;
        }

        public double DiagonalSize
        {
            get
            {
                return Math.Sqrt((XMax - XMin) * (XMax - XMin) + (YMax - YMin) * (YMax - YMin) + (ZMax - ZMin) * (ZMax - ZMin));
            }
        }
        public void FindParametersForRoundApperture()
        {

            //double answer = 0;
            List<double> max = new List<double>() { this[0].Center.X, this[0].Center.Y, this[0].Center.Z };
            List<double> min = new List<double>() { this[0].Center.X, this[0].Center.Y, this[0].Center.Z };


            for (int i = 1; i < this.Count; i++)
            {
                List<double> value = new List<double>() { this[0].Center.X, this[0].Center.Y, this[0].Center.Z };

                for (int k = 0; k < 3; k++)
                {
                    if (value[k] > max[k])
                    {
                        max[k] = value[k];
                    }
                    if (value[k] < min[k])
                    {
                        min[k] = value[k];
                    }
                }
            }

            double Rx = (max[0] - min[0]) / 2;
            double Ry = (max[1] - min[1]) / 2;
            double Rz = (max[2] - min[2]) / 2;


            if (Rx > Ry && Rx > Rz)
            {
                R = Rx;
                if (Ry > Rz)
                {
                    workPlane = "XY";
                }
                else
                {
                    workPlane = "XZ";
                }
            }
            else if (Rx > Ry && Rx < Rz)
            {
                R = Rz;
                workPlane = "XZ";
            }
            else if (Rx < Ry && Ry > Rz)
            {
                R = Ry;
                if (Rx > Rz)
                {
                    workPlane = "XY";
                }
                else
                {
                    workPlane = "YZ";
                }
            }
            else if (Rx < Ry && Ry < Rz)
            {
                R = Rz;
                workPlane = "XZ";
            }
            else if (Rx == Ry && Rx > Rz)
            {
                R = Rx;
                workPlane = "XY";
            }
            else if (Rx == Ry && Rx < Rz)
            {
                R = Rz;
                workPlane = "";
            }
        }


        public DVector GetPolarization(double thetaG, double phiG)
        {
            double x = Math.Sin(thetaG * Math.PI / 180) * Math.Cos(phiG * Math.PI / 180);
            double y = Math.Sin(thetaG * Math.PI / 180) * Math.Sin(phiG * Math.PI / 180);
            double z = Math.Cos(thetaG * Math.PI / 180);

            DVector J = new DVector(x - Center.X, y - Center.Y, z - Center.Z);

            J.Normalize();
            DVector n = new DVector(this[0].Norma);
            n.Normalize();

            DVector e_u = Aperture.GetUVector(n);
            DVector e_v = Aperture.GetVVector(n);

            Complex i_u = CVector.Scal(ApertureCurrent.I[0].CVector, e_u);
            Complex i_v = CVector.Scal(ApertureCurrent.I[0].CVector, e_v);

            DVector e_y = i_v.Magnitude * e_v + i_u.Magnitude * e_u;
            DVector e_x = DVector.Cross(e_y, n);

            e_x.Normalize();
            e_y.Normalize();

            double J_zLoc = DVector.Scal(J, n);
            if (J_zLoc > 1)
            {
                J_zLoc = 1;
            }
            if (J_zLoc < -1)
            {
                J_zLoc = -1;
            }

            double thetaLoc = Math.Acos(J_zLoc);               // угол theta в локальной системе координат, в радианах            

            double J_xLoc = DVector.Scal(e_x, J);
            if (J_xLoc > 1)
            {
                J_xLoc = 1;
            }
            double J_yLoc = DVector.Scal(e_y, J);
            if (J_yLoc > 1)
            {
                J_yLoc = 1;
            }

            double phiLoc = Math.Atan2(J_yLoc, J_xLoc);

            DVector e_phiLoc = DVector.Cross(n, J_xLoc * e_x + J_yLoc * e_y);
            e_phiLoc.Normalize();
            DVector e_thetaLoc = DVector.Cross(e_phiLoc, J);
            e_thetaLoc.Normalize();

            DVector p = (Math.Cos(thetaLoc) + 1) * Math.Sin(phiLoc) * e_thetaLoc + (1 + Math.Cos(thetaLoc)) * Math.Cos(phiLoc) * e_phiLoc;
            p.Normalize();
            return p;
        }




        //динамические методы класса
        public void Generate(double f, SourceTemplate2 source)
        {
            switch (SourceTemplate2.Distribution)
            {
                case "Постоянное поле":
                    GenerateConstantCurrentApperture(source.i, source.m);
                    break;
                case "Косинус на пьедестале":
                    GenerateCosOnStepApperture(source.i, source.m, 1);
                    break;
                case "Загрузить из файла":
                    GenerateLoadedCurrents(SourceTemplate2.I, SourceTemplate2.M);

                    if (source.polarization == "Поляризация Б")
                    {
                        for (int i = 0; i < ApertureCurrent.Count; i++)
                        {
                            ApertureCurrent.I[i].CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent.I[i].CVector, this[i].Norma, 90);
                            ApertureCurrent.M[i].CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent.M[i].CVector, this[i].Norma, 90);
                        }
                    }
                    break;
                default:
                    break;
            }

            if (SourceTemplate2.DifferenceChanel)
            {
                DifferenceRadiationPattern(SourceTemplate2.DifferenceAxis);
            }

            if (SourceTemplate2.Scanning == 1)
            {
                ApplyScanning(f, source.thetaScanE, source.phiScanE, 1f);
            }
            else if (SourceTemplate2.Scanning == 2)
            {
                //Механическое сканирование
                ApplyRotation(source.thetaScanM, source.phiScanM, 
                    source.axis1Include, 
                    source.axis1x1, source.axis1y1, source.axis1z1, 
                    source.axis1x2, source.axis1y2, source.axis1z2, 
                    source.axis2Include, 
                    source.axis2x1, source.axis2y1, source.axis2z1, 
                    source.axis2x2, source.axis2y2, source.axis2z2);
            }

            
        }

        private void ApplyRotation(double rotAngle1, double rotAngle2, 
            bool axis1Include, double axis1x1, double axis1y1, double axis1z1,
                               double axis1x2, double axis1y2, double axis1z2,
            bool axis2Include, double axis2x1, double axis2y1, double axis2z1,
                               double axis2x2, double axis2y2, double axis2z2)
        {
            if (axis1Include)
            {
                for (int i = 0; i < ApertureCurrent.Count; i++)
                {
                    ApertureCurrent[i].I.CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent[i].I.CVector, new DVector(axis1x2 - axis1x1, axis1y2 - axis1y1, axis1z2 - axis1z1), rotAngle1);
                    ApertureCurrent[i].M.CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent[i].M.CVector, new DVector(axis1x2 - axis1x1, axis1y2 - axis1y1, axis1z2 - axis1z1), rotAngle1);
                    ApertureCurrent[i].P = Logic.Instance.RotateElement(ApertureCurrent[i].P, rotAngle1, axis1x1, axis1y1, axis1z1, axis1x2, axis1y2, axis1z2);
                }
            }
            if (axis2Include)
            {
                for (int i = 0; i < ApertureCurrent.Count; i++)
                {                    
                    Point3D a2p1 = new Point3D(axis2x1, axis2y1, axis2z1);
                    Point3D a2p2 = new Point3D(axis2x2, axis2y2, axis2z2);

                    if (axis1Include)
                    {
                        a2p1 = Logic.Instance.RotateElement(a2p1, rotAngle1, axis1x1, axis1y1, axis1z1, axis1x2, axis1y2, axis1z2);
                        a2p2 = Logic.Instance.RotateElement(a2p2, rotAngle1, axis1x1, axis1y1, axis1z1, axis1x2, axis1y2, axis1z2);    
                    }                    

                    ApertureCurrent[i].I.CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent[i].I.CVector, new DVector(a2p2.X - a2p1.X, a2p2.Y - a2p1.Y, a2p2.Z - a2p1.Z), rotAngle2);
                    ApertureCurrent[i].M.CVector = Current.ChangeLoadedCurrentAnlorithm(ApertureCurrent[i].M.CVector, new DVector(a2p2.X - a2p1.X, a2p2.Y - a2p1.Y, a2p2.Z - a2p1.Z), rotAngle2);
                    ApertureCurrent[i].P = Logic.Instance.RotateElement(ApertureCurrent[i].P, rotAngle2, a2p1.X, a2p1.Y, a2p1.Z, a2p2.X, a2p2.Y, a2p2.Z);
                }
            }            
        }

        private void GenerateLoadedCurrents(CVector[] i, CVector[] m)
        {
            CVector[] VectorI = new CVector[Count];
            CVector[] VectorM = new CVector[Count];
            Point3D[] Segments = new Point3D[Count];
            double[] Area = new double[Count];

            for (int k = 0; k < Count; k++)
            {
                VectorI[k] = i[k];
                VectorM[k] = m[k];
                Segments[k] = this[k].Center;
                Area[k] = this[k].Area;
            }

            ApertureCurrent = new Current(VectorI, VectorM, Segments, Area);
        }

        private void GenerateConstantCurrentApperture(CVector i, CVector m)
        {
            CVector[] VectorI = new CVector[Count];
            CVector[] VectorM = new CVector[Count];
            Point3D[] Segments = new Point3D[Count];
            double[] Area = new double[Count];

            for (int k = 0; k < Count; k++)
            {
                VectorI[k] = i;
                VectorM[k] = m;
                Segments[k] = this[k].Center;
                Area[k] = this[k].Area;
            }

            ApertureCurrent = new Current(VectorI, VectorM, Segments, Area);
            //            magneticCurrent = new Current(VectorM, Segments);
        }
        private void GenerateCosOnStepApperture(CVector i_a, CVector m_a, double delta) //i_a, m_a - двумерные вектора, связанные с плоскостью апертуры)
        {
            CVector[] VectorI = new CVector[Count];
            CVector[] VectorM = new CVector[Count];
            Point3D[] Segments = new Point3D[Count];
            double[] Square = new double[Count];

            double xlen = XMax - XMin;
            double ylen = YMax - YMin;
            double zlen = ZMax - ZMin;

            double Ra = 0;

            if (xlen >= ylen && xlen >= zlen)
            {
                Ra = xlen / 2;
            }
            else if (ylen >= zlen)
            {
                Ra = ylen / 2;
            }
            else
            {
                Ra = zlen / 2;
            }


            for (int p = 0; p < Count; p++)
            {
                Point3D position = this[p].Center;
                DVector n = this[p].Norma;

                DVector ran = new DVector(1, 0, 0);
                if (DVector.IsEqual(n, ran, 4) || DVector.IsEqual((-1) * n, ran, 4))
                {
                    ran = new DVector(0, 1, 0);
                }
                //базисные вектора в плоскости апертуры
                DVector e_u = DVector.Cross(n, ran);
                e_u.Normalize();
                DVector e_v = DVector.Cross(n, e_u);
                e_v.Normalize();



                double c1 = (1 + delta * Math.Cos(Math.PI * Point3D.Distance(position, this.Center) / Ra)) / (1 + delta);
                CVector i = c1 * (i_a.X * e_u + i_a.Y * e_v);
                CVector m = c1 * (m_a.X * e_u + m_a.Y * e_v);

                VectorI[p] = i;
                VectorM[p] = m;
                Segments[p] = this[p].Center;
                Square[p] = this[p].Area;
            }

            ApertureCurrent = new Current(VectorI, VectorM, Segments, Square);
            //electricCurrent = new Current(VectorI, Segments);
            //magneticCurrent = new Current(VectorM, Segments);
        }
        private void DifferenceRadiationPattern(string axis)
        {
            int axisNumber = 1;
            switch (axis)
            {
                case "Плоскость XY":
                    axisNumber = 1;
                    break;
                case "Плоскость YZ":
                    axisNumber = 2;
                    break;
                case "Плоскость XZ":
                    axisNumber = 3;
                    break;
                default:
                    break;
            }

            double c = 1;
            Point3D center = Center;

            CVector[] VectorI = new CVector[Count];
            CVector[] VectorM = new CVector[Count];
            //Point3D[] Position = new Point3D[Count];
            //Triangle[] Segments = new Triangle[Count];
            //double[] square = new double[Count];

            for (int n = 0; n < Count; n++)
            {
                c = 1;
                Point3D position = this[n].Center;

                if (axisNumber == 2 && position.X < center.X)
                {
                    c = -1;
                }
                if (axisNumber == 3 && position.Y < center.Y)
                {
                    c = -1;
                }
                if (axisNumber == 1 && position.Z < center.Z)
                {
                    c = -1;
                }
                if (c == -1)
                {
                    ApertureCurrent[n].I.CVector = c * ApertureCurrent[n].I.CVector;
                    ApertureCurrent[n].M.CVector = c * ApertureCurrent[n].M.CVector;
                }
            }
        }
        public void ApplyScanning(double f, double scanTheta, double scanPhi, double dimention)
        {
            double K_0 = 2 * Math.PI * f / CV.c_0;      // волновое число 2pi/lambda            
            Point3D center = Center;


            for (int n = 0; n < Count; n++)
            {
                Point3D p = this[n].Center;

                double cx = K_0 * Math.Sin(scanTheta * Math.PI / 180) * Math.Cos(scanPhi * Math.PI / 180) * (p.X - center.X) * dimention;
                double cy = K_0 * Math.Sin(scanTheta * Math.PI / 180) * Math.Sin(scanPhi * Math.PI / 180) * (p.Y - center.Y) * dimention;
                double cz = K_0 * Math.Cos(scanTheta * Math.PI / 180) * (p.Z - center.Z) * dimention;
                Complex c = exp(cx) * exp(cy) * exp(cz);

                ApertureCurrent[n].I.CVector = c * ApertureCurrent[n].I.CVector;
                ApertureCurrent[n].M.CVector = c * ApertureCurrent[n].M.CVector;
            }
        }

        //статические методы класса
        public static Complex exp(double x)
        {
            return new Complex(Math.Cos(x), -Math.Sin(x));
        }
        public static DVector GetUVector(DVector n)
        {
            DVector ran = new DVector(1, 0, 0);
            if (DVector.IsEqual(n, ran, 4) || DVector.IsEqual((-1) * n, ran, 4))
            {
                ran = new DVector(0, 1, 0);
            }

            DVector e_u = DVector.Cross(n, ran);
            e_u.Normalize();
            return e_u;
        }
        public static DVector GetVVector(DVector n)
        {
            DVector ran = new DVector(1, 0, 0);
            if (DVector.IsEqual(n, ran, 4) || DVector.IsEqual((-1) * n, ran, 4))
            {
                ran = new DVector(0, 1, 0);
            }

            DVector e_u = DVector.Cross(n, ran);
            e_u.Normalize();
            DVector e_v = DVector.Cross(n, e_u);
            e_v.Normalize();
            return e_v;
        }

        internal void GeneratePolarizationCurrents(string distribution)
        {
            CVector[] polI = new CVector[DictionaryLibrary.PolarizationNames.Count];
            CVector[] polM = new CVector[DictionaryLibrary.PolarizationNames.Count];

            for (int p = 0; p < DictionaryLibrary.PolarizationNames.Count; p++)
            {
                var currentTuple = SourceTemplate2.GetPolarizationCurrents(DictionaryLibrary.PolarizationNames[p], distribution);
                polI[p] = currentTuple.Item1;
                polM[p] = currentTuple.Item2;
            }
            SourceTemplate2.I0 = polI;
            SourceTemplate2.M0 = polM;
        }
    }
}
