using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class Rectangle
    {
        public Point3D V1 { get; set; }
        public Point3D V2 { get; set; }
        public Point3D V3 { get; set; }
        public Point3D V4 { get; set; }
        public int Index { get; set; }

        

        public List<Point3D> pointList;

        public Point3D this[int k]
        {
            get
            {
                return pointList[k];
            }
            set
            {
                pointList[k] = value;
            }
        }

        public Rectangle(Point3D p1, Point3D p2, Point3D p3, Point3D p4, int i =-1)
        {
            V1 = p1;
            V2 = p2;
            V3 = p3;
            V4 = p4;
            pointList = new List<Point3D>() { V1, V2, V3, V4 };            
            Index = i;                   
        }
        Point3D center;
        /// <summary>
        /// Точка центра масс
        /// </summary>
        public Point3D Center
        {
            get
            {
                if (center == null)
                {
                    //центр масс
                    center = new Point3D((V1.X + V2.X + V3.X + V4.X) / 4, (V1.Y + V2.Y + V3.Y + V4.Y) / 4, (V1.Z + V2.Z + V3.Z + V4.Z) / 4);

                }                
                return center;
            }
            set
            {

                center = value;                
            }
        }

        double square = 0;
        public double Square
        {
            get
            {
                if (square == 0)
                {
                    DVector ab = new DVector(V1, V2);
                    DVector bc = new DVector(V2, V3);
                    DVector cd = new DVector(V3, V4);
                    DVector da = new DVector(V4, V1);

                    double s1 = 0.5d * DVector.Cross(ab, bc).Module;
                    double s2 = 0.5d * DVector.Cross(cd, da).Module;

                    square = s1 + s2;                    
                }
                return square;
            }
            set
            {                
                square = value;             
            }
        }
        public List<Tuple<Point3D, Point3D>> ReturnDiagonalPoints
        {
            get
            {
                List<Tuple<Point3D, Point3D>> dlist = new List<Tuple<Point3D, Point3D>>();

                for (int i = 0; i < pointList.Count - 1; i++)
                {
                    for (int j = i + 1; j < pointList.Count; j++)
                    {
                        Point3D p1 = pointList[i];
                        Point3D p2 = pointList[j];
                        dlist.Add(new Tuple<Point3D, Point3D>(p1, p2));                        
                    }
                }

                for (int m = 0; m < dlist.Count-1; m++)
                {
                    for (int k = m + 1; k < dlist.Count; k++)
                    {
                        Point3D A = dlist[m].Item1;
                        Point3D B = dlist[m].Item2;
                        double p = B.X - A.X;
                        double q = B.Y - A.Y;
                        double r = B.Z - A.Z;

                        Point3D A1 = dlist[k].Item1;
                        Point3D B1 = dlist[k].Item2;

                        double p1 = B1.X - A1.X;
                        double q1 = B1.Y - A1.Y;
                        double r1 = B1.Z - A1.Z;

                        double x0 = A.X;
                        double x1 = A1.X;
                        double y0 = A.Y;
                        double y1 = A1.Y;
                        double z0 = A.Z;
                        double z1 = A1.Z;

                        double x = (x0 * q * p1 - x1 * q1 * p - y0 * p * p1 + y1 * p * p1) / (q * p1 - q1 * p);
                        double y = (y0 * p * q1 - y1 * p1 * q - x0 * q * q1 + x1 * q * q1) / (p * q1 - p1 * q);
                        double z = (z0 * q * r1 - z1 * q1 * r - y0 * r * r1 + y1 * r * r1) / (q * r1 - q1 * r);

                        bool match = false;
                        foreach (var point in pointList)
                        {                            
                            if (point.X - x < 0.000001 && point.Y - y < 0.000001 && point.Z - z < 0.000001)
                            {
                                match = true;
                                break;
                            }
                        }

                        if (!match)
                        {
                            return new List<Tuple<Point3D, Point3D>>() { new Tuple<Point3D, Point3D>(A, B), new Tuple<Point3D, Point3D>(A1, B1) };
                        }
                    }
                }

                return null;
                //List<DVector> dlist = new List<DVector>();
                //for (int i = 0; i < pointList.Count - 1; i++)
                //{
                //    for (int j = i+1; j < pointList.Count; j++)
                //    {
                //        Point3D p1 = pointList[i];
                //        Point3D p2 = pointList[j];
                //        Point3D pC = new Point3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
                //        double difx = Math.Abs(pC.X - Center.X);
                //        double dify = Math.Abs(pC.Y - Center.Y);
                //        double difz = Math.Abs(pC.Z - Center.Z);
                //        double dif = Math.Sqrt(difx * difx + dify * dify + difz * difz);
                //        if (dif < 0.0001 )
                //        {
                //            dlist.Add(new DVector(p1, p2));
                //        }
                //    }
                //}
                //if (dlist.Count == 2)
                //{
                //    return new Tuple<DVector, DVector>(dlist[0], dlist[1]);    
                //}
                //else
                //{
                //    return null;
                //}               
                
            }            
        }        

        public DVector Norma    //  Вычисление нормали треуголька
        {
            get
            {
                double a = (V2.Y - V1.Y) * (V3.Z - V1.Z) - (V3.Y - V1.Y) * (V2.Z - V1.Z);                                                                  // коэффициенты плоскости
                double b = (V3.X - V1.X) * (V2.Z - V1.Z) - (V2.X - V1.X) * (V3.Z - V1.Z);                                                                  // Ax+By+Cz+D=0
                double c = (V2.X - V1.X) * (V3.Y - V1.Y) - (V2.Y - V1.Y) * (V3.X - V1.X);
                //double d = (V3.X - V1.X) * (V2.Y * V1.Z - V1.Y * V2.Z) + (V3.Y - V1.Y) * (V1.X * V2.Z - V2.X * V1.Z) + (V3.Z - V1.Z) * (V2.X * V1.Y - V1.X * V2.Y);
                double length = Math.Sqrt(a * a + b * b + c * c);
                //double var = a * this.Center.X + b * Center.Y + c * Center.Z + d;

                DVector norma = new DVector
                {
                    X = a / length,
                    Y = b / length,
                    Z = c / length
                };
                DVector centerV = new DVector(Center.X, Center.Y, Center.Z);
                if (DVector.Scal(norma, centerV) < 0) 
                {
                    norma = (-1) * norma;
                }
                
                return norma;                
            }
        }       
    }
}
