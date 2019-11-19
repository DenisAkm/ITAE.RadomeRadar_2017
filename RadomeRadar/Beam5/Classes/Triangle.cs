using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apparat
{
    public class Triangle
    {
        //Поля класса Triangle
        public Point3D V1;
        public Point3D V2;
        public Point3D V3;
        public bool AllowToSetParameters { get; set; }

        public int index;        

        //Конструктор
        public Triangle(Point3D p1, Point3D p2, Point3D p3, int i = 0)
        {
            V1 = p1;
            V2 = p2;
            V3 = p3;
            index = i;
            AllowToSetParameters = true;
            SquareCalc();
            CenterCalc();
            AllowToSetParameters = false;
        }

        private void CenterCalc()
        {
            Center = new Point3D((V1.X + V2.X + V3.X) / 3, (V1.Y + V2.Y + V3.Y) / 3, (V1.Z + V2.Z + V3.Z) / 3);
        }

        private void SquareCalc()
        {
            //double a = Math.Sqrt(Math.Pow(V2.X - V1.X, 2) + Math.Pow(V2.Y - V1.Y, 2) + Math.Pow(V2.Z - V1.Z, 2));
            //double b = Math.Sqrt(Math.Pow(V3.X - V2.X, 2) + Math.Pow(V3.Y - V2.Y, 2) + Math.Pow(V3.Z - V2.Z, 2));
            //double c = Math.Sqrt(Math.Pow(V1.X - V3.X, 2) + Math.Pow(V1.Y - V3.Y, 2) + Math.Pow(V1.Z - V3.Z, 2));
            //double p = (a + b + c) / 2;

            DVector ab = new DVector(V2.X - V1.X, V2.Y - V1.Y, V2.Z - V1.Z);
            DVector ac = new DVector(V3.X - V1.X, V3.Y - V1.Y, V3.Z - V1.Z);

            //DVector val = 
            Square = 0.5d * DVector.Cross(ab, ac).Module;
            //Square = Math.Sqrt((p - a) * (p - b) * (p - c) * p);
            //double diff = Square - sq;
            //MessageBox.Show("" + diff);
        }
        public Triangle(Triangle tr)
        {
            V1 = new Point3D(tr.V1);
            V2 = new Point3D(tr.V2);
            V3 = new Point3D(tr.V3);
            index = tr.index;
        }
        public Triangle()
        {
            V1 = new Point3D(0, 0, 0);
            V2 = new Point3D(0, 0, 0);
            V3 = new Point3D(0, 0, 0);
            index = 0;
        }
        //Свойства класса Triangle  
        Point3D center = new Point3D(0, 0, 0);
        public Point3D Center
        {
            get
            {
                return center;
            }
            set
            {
                if (AllowToSetParameters)
                {
                    center = value;    
                }                
            }
        }

        double square = 0;
        public double Square
        {
            get
            {
                return square;
            }
            set
            {
                if (AllowToSetParameters)
                {
                    square = value;    
                }                
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



        //Методы класса Triangle
        public void Move(DVector vector)
        {            
            V1.Move(vector);
            V2.Move(vector);
            V3.Move(vector);
        }
        public void Scale(double factor)
        {
            V1.Scale(factor);
            V2.Scale(factor);
            V3.Scale(factor);            
        }


        //public void ReverseNorma()
        //{
        //    updown = (-1) * updown;
        //}
    }
}
