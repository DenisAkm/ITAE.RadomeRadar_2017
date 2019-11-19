using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class CVector
    {
        public Complex X;
        public Complex Y;
        public Complex Z;

        public CVector()
        {
            X = new Complex(0, 0);
            Y = new Complex(0, 0);
            Z = new Complex(0, 0);   
        }
        
        public CVector(Complex x, Complex y, Complex z)
        {
            X = x;
            Y = y;
            Z = z;        
        }
        public CVector(CVector vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z; 
        }
        public void Set(Complex fx, Complex fy, Complex fz)
        {
            X = fx;
            Y = fy;
            Z = fz;
        }
        public static CVector Cross(CVector v1, CVector v3)
        {            
            CVector v2 = new CVector(Complex.Conjugate(v3.X), Complex.Conjugate(v3.Y), Complex.Conjugate(v3.Z));
            
            Complex x = v1.Y * v2.Z - v1.Z * v2.Y;
            Complex y = v1.Z * v2.X - v1.X * v2.Z;
            Complex z = v1.X * v2.Y - v1.Y * v2.X;

            return new CVector(x, y, z);
        }
        public static CVector Cross(CVector v1, DVector v2)
        {
            Complex x = v1.Y * v2.Z - v1.Z * v2.Y;
            Complex y = v1.Z * v2.X - v1.X * v2.Z;
            Complex z = v1.X * v2.Y - v1.Y * v2.X;

            return new CVector(x, y, z);
        }
        public double Modulus
        {
            get
            {
                return Math.Sqrt(X.Real * X.Real + X.Imaginary * X.Imaginary + Y.Real * Y.Real + Y.Imaginary * Y.Imaginary + Z.Real * Z.Real + Z.Imaginary * Z.Imaginary);
            }
        }
        public static CVector operator +(CVector v1, CVector v2)
        {
            return new CVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static CVector operator -(CVector v1, CVector v2)
        {
            return new CVector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static CVector operator *(Complex val, CVector v2)
        {
            return new CVector(val * v2.X, val * v2.Y, val * v2.Z);
        }
        public static Complex Scal(CVector v1, DVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public static Complex Scal(Complex v1x, Complex v1y, Complex v1z, double v2x, double v2y, double v2z)
        {
            return v1x * v2x + v1y * v2y + v1z * v2z;
        }
        public static Complex Scal(CVector v1, CVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public DVector Real()
        {
            return new DVector(this.X.Real, this.Y.Real, this.Z.Real);
        }
        public void Normalize()
        {
            double length = this.Modulus;
            X /= length;
            Y /= length;
            Z /= length;
        }
    }
}
