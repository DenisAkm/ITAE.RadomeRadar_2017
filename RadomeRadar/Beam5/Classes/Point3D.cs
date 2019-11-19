using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    //
    // Точка в трёхмерном пространстве
    //
    public class Point3D
    {
        public double X;
        public double Y;
        public double Z;
        public int Index;
        public Point3D(double x, double y, double z, int index = 0)
        {
            X = x;
            Y = y;
            Z = z;
            Index = index;
        }
        public Point3D(Point3D _p)
        {
            this.X = _p.X;
            this.Y = _p.Y;
            this.Z = _p.Z;
            this.Index = _p.Index;
        }
        public static double Distance(Point3D a, Point3D b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2) + Math.Pow((a.Z - b.Z), 2));
        }
        public void Move(DVector vector)
        {
            this.X += vector.X;
            this.Y += vector.Y;
            this.Z += vector.Z;
        }

        public void Scale(double factor)
        {
            this.X = this.X * factor;
            this.Y = this.Y * factor;
            this.Z = this.Z * factor;
        }
        public static Point3D operator *(double a, Point3D P)
        {
            return new Point3D(P.X * a, P.Y * a, P.Z * a);
        }
    }
}
