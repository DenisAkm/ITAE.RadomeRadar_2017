using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat.Classes
{
    public class CComplex
    {
        double Re { get; set; }
        double Im { get; set; }

        public double Phase
        {
            get
            {
                return Math.Atan2(this.Im, this.Re);
            }
        }
        public double Mod 
        {
            get 
            {
                return Math.Sqrt(this.Re * this.Re + this.Im * this.Im);
            }
        }


        public CComplex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        public static CComplex operator *(CComplex a, CComplex b)
        {
            return new CComplex(a.Re*b.Re - a.Im*b.Im, a.Re*b.Im + b.Im*a.Re);
        }
        public static CComplex operator -(CComplex a, CComplex b)
        {
            return new CComplex(a.Re - b.Re, a.Im - b.Im);
        }
        public static CComplex operator +(CComplex a, CComplex b)
        {
            return new CComplex(a.Re + b.Re, a.Im + b.Im);
        }
    }
}
