using System;
using System.Numerics;

namespace Apparat
{
    public class CV
    {

        //public static double pi = Math.PI;                          //                                          Ratio of a circle's circumference to its diameter
        //public static double E_0 = 8.85418781761e-12;               // 8.85e-12 Ф/м                             Permittivity of free space in F/m                                                
        //public static double Mu_0 = 4.0f * pi * 1.0e-7;             // pi*4e-7 Гн/м = 1.25663706143592e-06      Permeability of free space in H/m
        //public static double c_0 = 1 / Math.Sqrt(E_0 * Mu_0);       // 1/sqrt(eps0*mu0) = 299792458.000176;     Speed of light in free space in m/sec
        //public static double Z_0 = Math.Sqrt(Mu_0 / E_0);           // sqrt(Mu_0 / E_0) = 376.730313461992      Characteristic impedance of free space in Ohm

        

        //Костины значения
        public static double pi = 3.1415926535897932384626433832795d;
        public static double c_0 = 3.0e8;
        public static double E_0 = 1.0 / c_0 / 60.0 / 2.0 / pi;
        public static double Mu_0 = 1.0 / (c_0 * c_0 * E_0);
        public static double Z_0 = Math.Sqrt(Mu_0 / E_0);



        private static double f0;
        public static double Omega;
        public static double K_0;
        public static double K2;
        public static Complex Ekoeff;
        public static Complex Mukoeff;
        public static Complex iOmega;
        public static Complex Z0m;
        public static Complex Y0e;

        //Максимально допустимый угол падения
        public static double cutoff_angle = 88.0 * pi / 180;                                   // в радианах 88 градусов
        public static double F0
        {
            get
            {
                return f0;
            }
            set
            {
                f0 = value;
                Omega = 2 * pi * f0;
                K_0 = Omega / c_0;
                K2 = K_0 * K_0;
                Ekoeff = (1.0) / (4 * pi * Complex.ImaginaryOne * Omega * E_0);
                Mukoeff = (-1.0) / (Complex.ImaginaryOne * Omega * CV.Mu_0);         // -1/i*omega*Mu_0
                iOmega = Complex.ImaginaryOne * Omega;
                Z0m = Complex.ImaginaryOne * Omega * Mu_0;
                Y0e = Complex.ImaginaryOne * Omega * E_0;
            }
        }
        
        
        
    }
}
