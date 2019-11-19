using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public static class TRCoeffitients
    {
        const double pi = Math.PI;
        private static Complex imOne = new Complex(0, 1);
        //const double Mu_0 = 4.0f * pi * 1.0e-7f;                                    // 1.0 / (c * c * E_0) Гн/м     magnetic constant    магнитная постоянная
        //const double E_0 = 8.85418781761e-12f;                                      // 8.85e-12 Ф/м        electric constant     электрическая постоянная

        public static Tuple<Complex, Complex> TransmissionCoefficientCalc(Stenka st, double kappaTo2, double Omega, double K2, Complex Y0e, Complex Z0m)
        {

            Matrix<Complex> generalMatrixEtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Matrix<Complex> generalMatrixMtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Complex iomega = imOne * Omega;


            int layerCount = st.Count;

            for (int i = 0; i < layerCount; i++)
            {
                Complex Ea = st[i].Ea;
                Complex Mua = st[i].Mua;

                Complex Zm = iomega * Mua;
                Complex Ye = iomega * Ea;

                Complex a_e, b_e, c_e, d_e;
                Complex a_m, b_m, c_m, d_m;
                //
                //   определение волновое число в диэлектрике
                //
                Complex k_a = Omega * Complex.Sqrt(Mua * Ea);

                //
                // расчет гамма в диэлектрике и в свободном пространстве
                //
                Complex gamma = Complex.Sqrt(kappaTo2 - k_a * k_a);
                //
                //   Волновое сопротивление в диэлектрике
                //
                Complex we = gamma / Ye;
                Complex wm = Zm / gamma;
                Complex gammaL = gamma * st[i].Tickness;


                Complex sinhGT = Complex.Sinh(gammaL);
                Complex coshGT = Complex.Cosh(gammaL);


                a_e = coshGT;
                a_m = coshGT;
                b_e = we * sinhGT;
                b_m = wm * sinhGT;
                c_e = 1 / we * sinhGT;
                c_m = 1 / wm * sinhGT;
                d_e = coshGT;
                d_m = coshGT;


                Matrix<Complex> matrixEtype = Matrix<Complex>.Build.Dense(2, 2);//, new Complex[] { a_e, b_e, c_e, d_e });
                matrixEtype[0, 0] = a_e;
                matrixEtype[1, 0] = b_e;
                matrixEtype[0, 1] = c_e;
                matrixEtype[1, 1] = d_e;

                generalMatrixEtype = generalMatrixEtype * matrixEtype;

                Matrix<Complex> matrixMtype = Matrix<Complex>.Build.Dense(2, 2); //, new Complex[] { a_m, b_m, c_m, d_m });
                matrixMtype[0, 0] = a_m;
                matrixMtype[1, 0] = b_m;
                matrixMtype[0, 1] = c_m;
                matrixMtype[1, 1] = d_m;
                generalMatrixMtype = generalMatrixMtype * matrixMtype;
            }

            //
            // расчет гамма в свободном пространстве
            //               
            Complex gamma0 = Complex.Sqrt(kappaTo2 - K2);
            //
            //   Волновое сопротивление в свободном пространстве
            //
            Complex w0e = gamma0 / Y0e;
            Complex w0m = Z0m / gamma0;


            Complex A_e = generalMatrixEtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex A_m = generalMatrixMtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex B_e = generalMatrixEtype[1, 0];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex B_m = generalMatrixMtype[1, 0];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex C_e = generalMatrixEtype[0, 1];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex C_m = generalMatrixMtype[0, 1];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex D_e = generalMatrixEtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex D_m = generalMatrixMtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            //
            //   входное сопротивление длинной линии со стороны слоя
            //
            Complex inputZe = (A_e * w0e + B_e) / (C_e * w0e + D_e);
            Complex inputZm = (A_m * w0m + B_m) / (C_m * w0m + D_m);
            //
            // расчет коэффициента прохождения
            //

            Complex tE = inputZe * w0e / (inputZe + w0e) / (A_e * w0e + B_e);
            Complex tH = inputZm * w0m / (inputZm + w0m) / (A_m * w0m + B_m);

            return Tuple.Create(tE, tH);
        }

        public static Tuple<Complex, Complex> ReflectionCoefficientCalc(Stenka st, double kappaTo2, double theta, double Omega, double K2, Complex Y0e, Complex Z0m)
        {
            Matrix<Complex> generalMatrixEtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Matrix<Complex> generalMatrixMtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Complex iomega = imOne * Omega;

            int layerCount = st.Count;
            for (int i = 0; i < layerCount; i++)
            {
                Complex Ea = st[i].Ea;
                Complex Mua = st[i].Mua;

                Complex Zm = iomega * Mua;
                Complex Ye = iomega * Ea;

                Complex a_e, b_e, c_e, d_e;
                Complex a_m, b_m, c_m, d_m;
                //
                //   определение волновое число в диэлектрике
                //
                Complex k_a = Omega * Complex.Sqrt(Mua * Ea);

                //
                // расчет гамма в диэлектрике и в свободном пространстве
                //
                Complex gamma = Complex.Sqrt(kappaTo2 - k_a * k_a);
                //
                //   Волновое сопротивление в диэлектрике
                //
                Complex we = gamma / Ye;
                Complex wm = Zm / gamma;
                Complex gammaL = gamma * st[i].Tickness;


                Complex sinhGT = Complex.Sinh(gammaL);
                Complex coshGT = Complex.Cosh(gammaL);


                a_e = coshGT;
                a_m = coshGT;
                b_e = we * sinhGT;
                b_m = wm * sinhGT;
                c_e = 1 / we * sinhGT;
                c_m = 1 / wm * sinhGT;
                d_e = coshGT;
                d_m = coshGT;


                Matrix<Complex> matrixEtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { a_e, b_e, c_e, d_e });
                generalMatrixEtype = generalMatrixEtype * matrixEtype;

                Matrix<Complex> matrixMtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { a_m, b_m, c_m, d_m });
                generalMatrixMtype = generalMatrixMtype * matrixMtype;
            }

            //
            // расчет гамма в свободном пространстве
            //               
            Complex gamma0 = Complex.Sqrt(kappaTo2 - K2);
            //
            //   Волновое сопротивление в свободном пространстве
            //
            Complex w0e = gamma0 / Y0e;
            Complex w0m = Z0m / gamma0;

            Complex A_e = new Complex(generalMatrixEtype[0, 0].Real, generalMatrixEtype[0, 0].Imaginary);    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex A_m = new Complex(generalMatrixMtype[0, 0].Real, generalMatrixMtype[0, 0].Imaginary);    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex B_e = new Complex(generalMatrixEtype[1, 0].Real, generalMatrixEtype[1, 0].Imaginary);    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex B_m = new Complex(generalMatrixMtype[1, 0].Real, generalMatrixMtype[1, 0].Imaginary);    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex C_e = new Complex(generalMatrixEtype[0, 1].Real, generalMatrixEtype[0, 1].Imaginary);    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex C_m = new Complex(generalMatrixMtype[0, 1].Real, generalMatrixMtype[0, 1].Imaginary);    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex D_e = new Complex(generalMatrixEtype[1, 1].Real, generalMatrixEtype[1, 1].Imaginary);    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex D_m = new Complex(generalMatrixMtype[1, 1].Real, generalMatrixMtype[1, 1].Imaginary);    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            //
            //   входное сопротивление длинной линии со стороны слоя
            //
            Complex inputZe = (A_e * w0e + B_e) / (C_e * w0e + D_e);
            Complex inputZm = (A_m * w0m + B_m) / (C_m * w0m + D_m);

            Complex refE = (inputZe - w0e) / (inputZe + w0e);
            Complex refH = (inputZm - w0m) / (inputZm + w0m);

            return Tuple.Create(refE, refH);
        }

        public static Tuple<Complex, Complex, Complex, Complex> ReflectionCoefficientCalc2(Stenka st, double kappaTo2, double theta, double Omega, double K2, Complex Y0e, Complex Z0m)
        {
            Matrix<Complex> generalMatrixEtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Matrix<Complex> generalMatrixMtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
            Complex iomega = imOne * Omega;


            int layerCount = st.Count;

            for (int i = 0 ; i < layerCount; i++)
            {
                Complex Ea = st[i].Ea;
                Complex Mua = st[i].Mua;

                Complex Zm = iomega * Mua;
                Complex Ye = iomega * Ea;

                Complex a_e, b_e, c_e, d_e;
                Complex a_m, b_m, c_m, d_m;
                //
                //   определение волновое число в диэлектрике
                //
                Complex k_a = Omega * Complex.Sqrt(Mua * Ea);

                //
                // расчет гамма в диэлектрике и в свободном пространстве
                //
                Complex gamma = Complex.Sqrt(kappaTo2 - k_a * k_a);
                //
                //   Волновое сопротивление в диэлектрике
                //
                Complex we = gamma / Ye;
                Complex wm = Zm / gamma;
                Complex gammaL = gamma * st[i].Tickness;


                Complex sinhGT = Complex.Sinh(gammaL);
                Complex coshGT = Complex.Cosh(gammaL);


                a_e = coshGT;
                a_m = coshGT;
                b_e = we * sinhGT;
                b_m = wm * sinhGT;
                c_e = 1 / we * sinhGT;
                c_m = 1 / wm * sinhGT;
                d_e = coshGT;
                d_m = coshGT;


                Matrix<Complex> matrixEtype = Matrix<Complex>.Build.Dense(2, 2);//, new Complex[] { a_e, b_e, c_e, d_e });
                matrixEtype[0, 0] = a_e;
                matrixEtype[0, 1] = b_e;
                matrixEtype[1, 0] = c_e;
                matrixEtype[1, 1] = d_e;

                generalMatrixEtype = generalMatrixEtype * matrixEtype;
                //generalMatrixEtype = Multyply(generalMatrixEtype, matrixEtype);

                Matrix<Complex> matrixMtype = Matrix<Complex>.Build.Dense(2, 2); //, new Complex[] { a_m, b_m, c_m, d_m });
                matrixMtype[0, 0] = a_m;
                matrixMtype[0, 1] = b_m;
                matrixMtype[1, 0] = c_m;
                matrixMtype[1, 1] = d_m;

                generalMatrixMtype = generalMatrixMtype * matrixMtype;
                //generalMatrixMtype = Multyply(generalMatrixMtype, matrixMtype);
            }

            //
            // расчет гамма в свободном пространстве
            //               
            Complex gamma0 = Complex.Sqrt(kappaTo2 - K2);
            //
            //   Волновое сопротивление в свободном пространстве
            //
            Complex w0e = gamma0 / Y0e;
            Complex w0m = Z0m / gamma0;


            Complex A_e = generalMatrixEtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex A_m = generalMatrixMtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex B_e = generalMatrixEtype[0, 1];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex B_m = generalMatrixMtype[0, 1];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex C_e = generalMatrixEtype[1, 0];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex C_m = generalMatrixMtype[1, 0];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            Complex D_e = generalMatrixEtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
            Complex D_m = generalMatrixMtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа H
            //
            //   входное сопротивление длинной линии со стороны слоя
            //
            Complex inputZe = (A_e * w0e + B_e) / (C_e * w0e + D_e);
            Complex inputZm = (A_m * w0m + B_m) / (C_m * w0m + D_m);
            //
            // расчет коэффициента прохождения
            //

            Complex refE = (inputZe - w0e) / (inputZe + w0e);
            Complex refH = (inputZm - w0m) / (inputZm + w0m);

            Complex tE = inputZe * w0e / (inputZe + w0e) / (A_e * w0e + B_e);
            Complex tH = inputZm * w0m / (inputZm + w0m) / (A_m * w0m + B_m);

            return Tuple.Create(refE, refH, tE, tH);
        }

        public static Matrix<Complex> Multyply(Matrix<Complex> m1, Matrix<Complex> m2)
        {
            int N = m1.RowCount;
            Matrix<Complex> res = Matrix<Complex>.Build.Dense(N, N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < N; k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }                    
                }
            }
            return res;
        }
    }
}
