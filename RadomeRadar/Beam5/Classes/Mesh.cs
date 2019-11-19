using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class Mesh
    {
        //internal fields
        public MeshElement[] elements;

        //public Fields and Properties
        public Point3D Center
        {
            get
            {
                double xSum = 0;
                double ySum = 0;
                double zSum = 0;

                for (int i = 0; i < Count; i++)
                {
                    xSum = elements[i].Center.X;
                    ySum = elements[i].Center.Y;
                    zSum = elements[i].Center.Z;
                }
                return new Point3D(xSum / elements.Length, ySum / elements.Length, zSum / elements.Length);
            }
        }
        /// <summary>
        /// Число элементов разбиения
        /// </summary>
        public int Count
        {
            get
            {
                return elements.Length;
            }
        }
        public MeshElement this[int index]
        {
            get
            {
                return elements[index];
            }
            set
            {
                elements[index] = value;
            }
        }

        Point3D Left
        {
            get
            {
                Point3D leftpoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (leftpoint.Y > probe.Y)
                    {
                        leftpoint = probe;
                    }
                }
                return leftpoint;
            }
        }
        Point3D Right
        {
            get
            {
                Point3D rightpoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (rightpoint.Y < probe.Y)
                    {
                        rightpoint = probe;
                    }
                }
                return rightpoint;
            }
        }
        Point3D Top
        {
            get
            {
                Point3D toppoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (toppoint.Z < probe.Z)
                    {
                        toppoint = probe;
                    }
                }
                return toppoint;
            }
        }
        Point3D Buttom
        {
            get
            {
                Point3D buttompoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (buttompoint.Z > probe.Z)
                    {
                        buttompoint = probe;
                    }
                }
                return buttompoint;
            }
        }
        Point3D Front
        {
            get
            {
                Point3D frontpoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (frontpoint.X < probe.X)
                    {
                        frontpoint = probe;
                    }
                }
                return frontpoint;
            }
        }
        Point3D Back
        {
            get
            {
                Point3D backpoint = this[0].Center;
                for (int i = 1; i < Count; i++)
                {
                    Point3D probe = this[i].Center;

                    if (backpoint.X > probe.X)
                    {
                        backpoint = probe;
                    }
                }
                return backpoint;
            }
        }



        public double XMax { get { return Front.X; } }
        public double XMin { get { return Back.X; } }
        public double YMax { get { return Right.Y; } }
        public double YMin { get { return Left.Y; } }
        public double ZMax { get { return Top.Z; } }
        public double ZMin { get { return Buttom.Z; } }


        public List<double> ListX { get; set; }
        public List<double> ListY { get; set; }
        public List<double> ListZ { get; set; }


        public List<Int32> ListI1 { get; set; }

        public List<Int32> ListI2 { get; set; }

        public List<Int32> ListI3 { get; set; }

        //public void MagnifyToMeter(double m)
        //{
        //    int count = this.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        elements[i].Area = elements[i].Area * m * m;                
        //        elements[i].Center.Scale(m);                
        //    }
        //}
        public double MeshSize
        {
            get
            {
                double meshSize = this[0].Area;
                for (int i = 1; i < Count; i++)
                {
                    if (meshSize < this[i].Area)
                    {
                        meshSize = this[i].Area;
                    }
                }
                meshSize = Math.Sqrt(meshSize / Math.PI);
                return meshSize;
            }
        }
        //constructors
        public Mesh(ref List<double> _x, ref List<double> _y, ref List<double> _z, ref List<Int32> _ind1, ref List<Int32> _ind2, ref List<Int32> _ind3)
        {
            int countPoints = _x.Count;
            Point3D[] vertices = new Point3D[countPoints];
            for (int i = 0; i < countPoints; i++)
            {
                vertices[i] = new Point3D(_x[i], _y[i], _z[i]);
            }

            int countElements = _ind1.Count;
            elements = new MeshElement[countElements];

            for (int i = 0; i < countElements; i++)
            {
                Point3D p1 = vertices[_ind1[i] - 1];
                Point3D p2 = vertices[_ind2[i] - 1];
                Point3D p3 = vertices[_ind3[i] - 1];
                Triangle tr = new Triangle(p1, p2, p3, i);

                elements[i] = new MeshElement() { Center = tr.Center, Area = tr.Square, Norma = tr.Norma, Triangle = tr };
            }

            ListX = _x;
            ListY = _y;
            ListZ = _z;

            ListI1 = _ind1;
            ListI2 = _ind2;
            ListI3 = _ind3;
        }


        public Mesh(Point3D[] points, double[] areas, DVector[] norma)
        {
            int countPoints = points.Length;
            elements = new MeshElement[countPoints];


            for (int i = 0; i < countPoints; i++)
            {
                elements[i] = new MeshElement() { Center = points[i], Area = areas[i], Norma = norma[i] };
            }
        }

        public Mesh()
        { }




        //static methods
        public static Mesh ReadingMeshNastran(string var, double dimention)
        {
            List<double> vertexX = new List<double>();
            List<double> vertexY = new List<double>();
            List<double> vertexZ = new List<double>();
            List<Int32> indexP1 = new List<Int32>();
            List<Int32> indexP2 = new List<Int32>();
            List<Int32> indexP3 = new List<Int32>();
            string line = "";
            string bufflineX = "";
            string bufflineY = "";
            string bufflineZ = "";


            StreamReader sr = new StreamReader(var);

            for (int i = 0; i < 10; i++)
            {
                line = sr.ReadLine();
            }
            while (!(line.Remove(6) == "CTRIA3"))
            {
                bufflineX = line.Substring(40).Remove(16);
                bufflineY = line.Substring(56).Remove(16);
                line = sr.ReadLine();
                bufflineZ = line.Substring(8);
                vertexX.Add(dimention * DoubleRecognition(bufflineX));
                vertexY.Add(dimention * DoubleRecognition(bufflineY));
                vertexZ.Add(dimention * DoubleRecognition(bufflineZ));
                line = sr.ReadLine();
            }

            while (!(sr.EndOfStream))
            {
                indexP1.Add(Convert.ToInt32(line.Substring(24).Remove(8)));
                indexP2.Add(Convert.ToInt32(line.Substring(32).Remove(8)));
                indexP3.Add(Convert.ToInt32(line.Substring(40)));

                line = sr.ReadLine();
            }
            sr.Close();

            return new Mesh(ref vertexX, ref vertexY, ref vertexZ, ref indexP1, ref indexP2, ref indexP3);
        }
        public static Mesh ReadingMeshGMSH(string adress, double dimention)
        {
            //********************//
            //****StreamReader****//
            //********************//
            List<string> MeshNotes = new List<string>();
            List<string> MeshElements = new List<string>();
            bool Nodes = false;
            bool Elements = false;
            StreamReader sr = new StreamReader(adress);
            String line;
            while (!(sr.EndOfStream))
            {
                line = sr.ReadLine();

                if (line == "$Nodes")
                {
                    Nodes = true;
                    line = sr.ReadLine();
                    while (Nodes)
                    {
                        line = sr.ReadLine();
                        if (line == "$EndNodes")
                        {
                            Nodes = false;
                            break;
                        }
                        MeshNotes.Add(line);
                    }
                }
                else if (line == "$Elements")
                {
                    Elements = true;
                    line = sr.ReadLine();
                    while (Elements)
                    {
                        line = sr.ReadLine();
                        if (line == "$EndElements")
                        {
                            Elements = false;
                            break;
                        }
                        MeshElements.Add(line);
                    }
                }
            }
            sr.Close();

            //********************//
            //*******Split********//
            //********************//

            List<double> buffX = new List<double>();
            List<double> buffY = new List<double>();
            List<double> buffZ = new List<double>();
            List<Int32> buffI1 = new List<Int32>();
            List<Int32> buffI2 = new List<Int32>();
            List<Int32> buffI3 = new List<Int32>();
            string[] result;

            for (int i = 0; i < MeshNotes.Count; i++)
            {
                line = MeshNotes[i];
                result = line.Split(' ');

                buffX.Add(dimention * Convert.ToDouble(result[1].Replace(".", ",")));
                buffY.Add(dimention * Convert.ToDouble(result[2].Replace(".", ",")));
                buffZ.Add(dimention * Convert.ToDouble(result[3].Replace(".", ",")));
            }

            for (int i = 0; i < MeshElements.Count; i++)
            {
                line = MeshElements[i];
                result = line.Split(' ');

                if (result[1] == "2")
                {
                    buffI1.Add(Convert.ToInt32(result[5].Replace(".", ",")));
                    buffI2.Add(Convert.ToInt32(result[6].Replace(".", ",")));
                    buffI3.Add(Convert.ToInt32(result[7].Replace(".", ",")));
                }
            }

            return new Mesh(ref buffX, ref buffY, ref buffZ, ref buffI1, ref buffI2, ref buffI3);
        }

        /// <summary>
        /// Формат: координаты/площади/вектор нормали
        /// </summary>
        /// <param name="antennaFileName"></param>
        /// <returns></returns>
        public static Mesh ReadingMeshTxt(string antennaFileName)
        {
            StreamReader sr = new StreamReader(antennaFileName);

            int count = 0;
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                count++;
            }

            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

            string line = "";
            int columns = 4;
            double[,] datastream = new double[count, columns];
            for (int i = 0; i < count; i++)
            {
                line = sr.ReadLine();

                for (int k = 0; k < columns; k++)
                {
                    float x = Single.Parse(line.Substring(15 * k, 15).Replace(".", ","));
                    datastream[i, k] = x;
                }
            }
            sr.Close();

            Point3D[] points = new Point3D[count];
            double[] squares = new double[count];
            DVector[] norma = new DVector[count];

            for (int n = 0; n < count; n++)
            {
                points[n] = new Point3D(datastream[n, 0], datastream[n, 1], datastream[n, 2]);
                squares[n] = datastream[n, 3];
                //norma[n] = new DVector(datastream[n, 4], datastream[n, 5], datastream[n, 6]);
                norma[n] = new DVector(0, 1, 0);
            }
            Form1.Instance.textBox1.AppendText("Внимание, не считывается вектор нормали. По умолчанию n = [0, 1, 0]" + Environment.NewLine);
            return new Mesh(points, squares, norma);
        }

        /// <summary>
        /// Выдуманный формат треугольной сетки:
        /// \t{число точек}\n
        /// \t{число треугольников}\n
        /// {p.x}\t{p.y}\t{p.z}\n
        /// {i1}\t{i2}\t{i3}
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Mesh ReadingMeshMsh3(string fileName, double dimention)
        {
            StreamReader sr = new StreamReader(fileName);
            string line = sr.ReadLine();
            int pointCount = Convert.ToInt32(line.Split('\t')[1]);
            line = sr.ReadLine();
            int triangleCount = Convert.ToInt32(line.Split('\t')[1]);

            List<double> buffX = new List<double>();
            List<double> buffY = new List<double>();
            List<double> buffZ = new List<double>();
            List<Int32> buffI1 = new List<Int32>();
            List<Int32> buffI2 = new List<Int32>();
            List<Int32> buffI3 = new List<Int32>();

            for (int i = 0; i < pointCount; i++)
            {
                line = sr.ReadLine();
                string[] arr = line.Split('\t');
                buffX.Add(dimention * Convert.ToDouble(arr[0]));
                buffY.Add(dimention * Convert.ToDouble(arr[1]));
                buffZ.Add(dimention * Convert.ToDouble(arr[2]));
            }


            for (int i = 0; i < triangleCount; i++)
            {
                line = sr.ReadLine();
                string[] arr = line.Split('\t');
                buffI1.Add(Convert.ToInt32(arr[0]));
                buffI2.Add(Convert.ToInt32(arr[1]));
                buffI3.Add(Convert.ToInt32(arr[2]));
            }

            return new Mesh(ref buffX, ref buffY, ref buffZ, ref buffI1, ref buffI2, ref buffI3);
        }

        static double DoubleRecognition(string l)
        {
            double ans;
            double digit = Convert.ToDouble(l.Remove(12).Replace(".", ","));
            double pow = Convert.ToInt32(l.Substring(13));
            ans = digit * Math.Pow(10, pow);
            return ans;
        }

        public static Mesh Copy(RadomeElement rel)
        {
            MeshElement[] mel = new MeshElement[rel.Count];

            for (int i = 0; i < rel.Count; i++)
            {
                mel[i] = rel.elements[i];
            }
            return new Mesh() { elements = mel };
        }


    }


    public class MeshElement
    {
        public Point3D Center { get; set; }
        public double Area { get; set; }
        public DVector Norma { get; set; }

        public Triangle Triangle { get; set; }

        public MeshElement()
        {
            Triangle = null;
            Center = new Point3D(0, 0, 0);
            Area = 0;
            Norma = new DVector(0, 0, 0);
        }
    }
}
