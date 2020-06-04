using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apparat
{
    unsafe public class FarFieldC
    {
        #region Definition of dll methods        

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);       
        


        [DllImport("Solver.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern CComplex* ApertureFF(Complex[] Ix, Complex[] Iy, Complex[] Iz, Complex[] Mx, Complex[] My, Complex[] Mz,
            int appSize, double[] trX, double[] trY, double[] trZ, double[] trS,
            double freq, double thetaStart, double thetaFinish, double thetaStep, double phiStart, double phiFinish, double phiStep, int systemOfCoord, int proc);

        
        [DllImport("Solver.dll", CallingConvention = CallingConvention.Cdecl)]        
        static extern CComplex* ApertureRadomeFF(Complex[] Ix, Complex[] Iy, Complex[] Iz, Complex[] Mx, Complex[] My, Complex[] Mz,
                                                                    double[] trX, double[] trY, double[] trZ, double[] trS, double[] Nx, double[] Ny, double[] Nz, int geoSize,
                                                                    Complex[] eps, Complex[] mu, double[] tickness, double[] gtickness, int[] layersCount, int[] layersIndexer,
                                                                    double freq, double thetaStart, double thetaFinish, double phiStart, double phiFinish, double thetaStep, double phiStep,
                                                                    int systemOfCoord, int proc);
        
             
        
        FarFieldElementC[] elements;
        #endregion

        #region Properties
        public string Title { get; set; }
        public FarFieldElementC this[int index]
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
        public int Count
        {
            get
            {
                return elements.Length;
            }
        }
        #endregion

        #region Constructor
        private FarFieldC(string title, int count)
        {
            Title = title;
            elements = new FarFieldElementC[count];
        }
        #endregion






        /// <summary>
        /// Вычисление поля в дальней зоне от апертуры (используя класс Task)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="freq"></param>
        /// <param name="thetaStart"></param>
        /// <param name="thetaFinish"></param>
        /// <param name="phiStart"></param>
        /// <param name="phiFinish"></param>
        /// <param name="step"></param>
        /// <param name="systemOfCoord"></param>
        /// <param name="dimention"></param>
        /// <param name="title"></param>
        /// <param name="proc"></param>
        public static FarFieldC FarFieldFromSurfaceCurrentCs(Current current, double freq, double thetaStart, double thetaFinish, double phiStart, double phiFinish, double thetaStep, double phiStep, int systemOfCoord, string title, int proc)
        {
            double pi = CV.pi;
            double Mu_0 = CV.Mu_0;                                    // 1.0 / (c * c * E_0) Гн/м     magnetic constant    магнитная постоянная        
            double E_0 = CV.E_0;                                      // 8.85e-12 Ф/м        electric constant     электрическая постоянная
            double c0 = CV.c_0;        // м/с скорость света    
            Complex imOne = Complex.ImaginaryOne;

            int numberPhi = Convert.ToInt32((phiFinish - phiStart) / phiStep) + 1;
            int numberTheta = Convert.ToInt32((thetaFinish - thetaStart) / thetaStep) + 1;
            int numberTotal = numberPhi * numberTheta;

            FarFieldC ffantenna = new FarFieldC(title, numberTotal);

            int appSize = current.Count;

            double[] trX = new double[appSize];
            double[] trY = new double[appSize];
            double[] trZ = new double[appSize];
            double[] trS = new double[appSize];

            Complex[] Ix = new Complex[appSize];
            Complex[] Iy = new Complex[appSize];
            Complex[] Iz = new Complex[appSize];

            Complex[] Mx = new Complex[appSize];
            Complex[] My = new Complex[appSize];
            Complex[] Mz = new Complex[appSize];

            for (int i = 0; i < appSize; i++)
            {
                trX[i] = current[i].P.X;
                trY[i] = current[i].P.Y;
                trZ[i] = current[i].P.Z;
                trS[i] = current[i].S;

                Ix[i] = current.I[i].X;
                Iy[i] = current.I[i].Y;
                Iz[i] = current.I[i].Z;

                Mx[i] = current.M[i].X;
                My[i] = current.M[i].Y;
                Mz[i] = current.M[i].Z;
            }


            ////////////////////////////////////////
            //////////////Параметры/////////////////
            ////////////////////////////////////////


            double Omega = CV.Omega;//2 * pi * freq;
            double K_0 = CV.K_0;// Omega / c0;
            double K2 = CV.K2;// K_0 * K_0;
            Complex Ekoeff = CV.Ekoeff;// (1.0) / (4 * pi * imOne * Omega * E_0); //fourComx*piComx*imOne*omegaComx*E_0Comx                                        

            Task[] tasks = new Task[proc];//proc
            for (int p = 0; p < proc; p++)
            {
                tasks[p] = Task.Factory.StartNew((Object obj) =>
                {
                    int cur_proc = (int)obj;
                    int start = GetStartIndex(cur_proc, proc, numberTotal);
                    int end = GetEndIndex(cur_proc, proc, numberTotal);

                    for (int j = start; j <= end; j++)
                    {
                        double phi = phiStart + (j % numberPhi) * phiStep;
                        double theta = thetaStart + j / numberPhi * thetaStep;

                        double thetaGlobal = GetThetaGlobal(phi, theta, systemOfCoord);
                        double phiGlobal = GetPhiGlobal(phi, theta, systemOfCoord);

                        Complex ix, iy, iz, mx, my, mz;

                        Complex ex = new Complex(0, 0);
                        Complex ey = new Complex(0, 0);
                        Complex ez = new Complex(0, 0);

                        double a = K_0 * Math.Sin(thetaGlobal * pi / 180) * Math.Cos(phiGlobal * pi / 180);
                        double b = K_0 * Math.Sin(thetaGlobal * pi / 180) * Math.Sin(phiGlobal * pi / 180);
                        double c = K_0 * Math.Cos(thetaGlobal * pi / 180);

                        double ab = a * b;
                        double bc = b * c;
                        double ac = a * c;

                        for (int k = 0; k < appSize; k++)
                        {

                            double square = trS[k];

                            double arg = a * trX[k] + b * trY[k] + c * trZ[k];
                            //Complex cexp = new Complex(Math.Cos(arg), Math.Sin(arg));
                            Complex cexp = Complex.Exp(new Complex(0, arg));

                            ix = Ix[k];
                            iy = Iy[k];
                            iz = Iz[k];

                            mx = Mx[k];
                            my = My[k];
                            mz = Mz[k];

                            ex += (-1) * (Ekoeff * (K2 * ix - a * a * ix - ab * iy - ac * iz) * cexp * square + imOne * (b * mz - c * my) * cexp * square / (4 * pi));//-
                            ey += (-1) * (Ekoeff * (K2 * iy - b * b * iy - bc * iz - ab * ix) * cexp * square + imOne * (c * mx - a * mz) * cexp * square / (4 * pi));//-
                            ez += (-1) * (Ekoeff * (K2 * iz - c * c * iz - ac * ix - bc * iy) * cexp * square + imOne * (a * my - b * mx) * cexp * square / (4 * pi));//-
                        }
                        ffantenna.elements[j] = new FarFieldElementC(freq, thetaGlobal, phiGlobal, theta, phi, ex, ey, ez);
                    }
                }, p);
            }
            Task.WaitAll(tasks);
            return ffantenna;
        }

        
        public static FarFieldC FarFieldFromSurfaceCurrentCpp(Current current, double freq, double thetaStart, double thetaFinish, double phiStart, double phiFinish, double thetaStep, double phiStep, int systemOfCoord, string title, int proc)
        {
            int numberPhi = Convert.ToInt32((phiFinish - phiStart) / phiStep) + 1;
            int numberTheta = Convert.ToInt32((thetaFinish - thetaStart) / thetaStep) + 1;
            int numberTotal = numberPhi * numberTheta;

            FarFieldC ffantenna = new FarFieldC(title, numberTotal);

            int appSize = current.Count;

            double[] trX = new double[appSize];
            double[] trY = new double[appSize];
            double[] trZ = new double[appSize];
            double[] trS = new double[appSize];

            Complex[] Ix = new Complex[appSize];
            Complex[] Iy = new Complex[appSize];
            Complex[] Iz = new Complex[appSize];

            Complex[] Mx = new Complex[appSize];
            Complex[] My = new Complex[appSize];
            Complex[] Mz = new Complex[appSize];

            for (int i = 0; i < appSize; i++)
            {
                trX[i] = current[i].P.X;
                trY[i] = current[i].P.Y;
                trZ[i] = current[i].P.Z;
                trS[i] = current[i].S;

                Ix[i] = current.I[i].X;
                Iy[i] = current.I[i].Y;
                Iz[i] = current.I[i].Z;

                Mx[i] = current.M[i].X;
                My[i] = current.M[i].Y;
                Mz[i] = current.M[i].Z;
            }
            
             CComplex* sol = ApertureFF(Ix, Iy, Iz, Mx, My, Mz,
                                        appSize, trX, trY, trZ, trS,
                                        freq, thetaStart, thetaFinish, thetaStep, phiStart, phiFinish, phiStep, systemOfCoord, proc);
            
            
            int j = 0; 
            for (int n = 0; n < numberPhi; n++)
            {
                for (int m = 0; m < numberTheta; m++)
                {
                    double phi = phiStart + n * phiStep;
                    double theta = thetaStart + m * thetaStep;
                    double thetaGlobal = GetThetaGlobal(phi, theta, systemOfCoord);
                    double phiGlobal = GetPhiGlobal(phi, theta, systemOfCoord);
                    Complex ex = new Complex(sol[3 * (m * numberPhi + n) + 0].re, sol[3 * (m * numberPhi + n) + 0].im);
                    Complex ey = new Complex(sol[3 * (m * numberPhi + n) + 1].re, sol[3 * (m * numberPhi + n) + 1].im);
                    Complex ez = new Complex(sol[3 * (m * numberPhi + n) + 2].re, sol[3 * (m * numberPhi + n) + 2].im);
                    ffantenna.elements[j] = new FarFieldElementC(freq, thetaGlobal, phiGlobal, theta, phi, ex, ey, ez);
                    j++;
                }
            }
            return ffantenna;            
        }


        /// <summary>
        /// Вычисление поля в дальней зоне от токов через стенку (используя С++, OpenMP)
        /// </summary>
        /// <param name="rad"></param>
        /// <param name="cur"></param>
        /// <param name="layers"></param>
        /// <param name="freq"></param>
        /// <param name="thetaStart"></param>
        /// <param name="thetaFinish"></param>
        /// <param name="phiStart"></param>
        /// <param name="phiFinish"></param>
        /// <param name="step"></param>
        /// <param name="systemOfCoord"></param>
        /// <param name="title"></param>
        /// <param name="dim"></param>
        /// <param name="proc"></param>
        public static FarFieldC FarFieldScatteredCpp(Radome radUnion, Current cur, double freq, double thetaStart, double thetaFinish, double phiStart, double phiFinish, double thetaStep, double phiStep, int systemOfCoord, string title, double dim, int proc)
        {
            int numberPhi = Convert.ToInt32((phiFinish - phiStart) / phiStep) + 1;
            int numberTheta = Convert.ToInt32((thetaFinish - thetaStart) / thetaStep) + 1;
            int numberTotal = numberPhi * numberTheta;
            FarFieldC ffradome = new FarFieldC(title, numberTotal);

            int radomeUnionSize = radUnion.CountElements;

            double[] trX = new double[radomeUnionSize];
            double[] trY = new double[radomeUnionSize];
            double[] trZ = new double[radomeUnionSize];
            double[] trS = new double[radomeUnionSize];

            Complex[] Ix = new Complex[radomeUnionSize];
            Complex[] Iy = new Complex[radomeUnionSize];
            Complex[] Iz = new Complex[radomeUnionSize];

            Complex[] Mx = new Complex[radomeUnionSize];
            Complex[] My = new Complex[radomeUnionSize];
            Complex[] Mz = new Complex[radomeUnionSize];

            double[] Nx = new double[radomeUnionSize];
            double[] Ny = new double[radomeUnionSize];
            double[] Nz = new double[radomeUnionSize];


            int[] stenkaIndexer = new int[radomeUnionSize];

            int layersSummary = 0;
            for (int i = 0; i < radUnion.Count; i++)
            {
                layersSummary += radUnion[i].Structure.Count;
            }
            Complex[] eps_a = new Complex[layersSummary];
            Complex[] mu_a = new Complex[layersSummary];
            double[] tickness = new double[layersSummary];

            //указывает сколько слоёв в каждом элементе обтекателя
            int[] layersCount = new int[radUnion.Count];
            double[] gtickness = new double[radUnion.Count];

            int s = 0;
            for (int i = 0; i < radUnion.Count; i++)
            {
                layersCount[i] = radUnion[i].Structure.Count;
                gtickness[i] = radUnion[i].Structure.GeneralThickness;
                for (int r = 0; r < radUnion[i].Structure.Count; r++)
                {
                    eps_a[s] = radUnion[i].Structure[r].Ea;
                    mu_a[s] = radUnion[i].Structure[r].Mua;
                    tickness[s] = radUnion[i].Structure[r].Tickness;
                    s++;
                }
            }


            int g = 0;
            for (int k = 0; k < radUnion.Count; k++)
            {
                for (int i = 0; i < radUnion[k].Count; i++)
                {
                    MeshElement me = radUnion[k].elements[i];
                    Nx[g] = me.Norma.X;
                    Ny[g] = me.Norma.Y;
                    Nz[g] = me.Norma.Z;

                    Point3D Center = me.Center;
                    trX[g] = Center.X;
                    trY[g] = Center.Y;
                    trZ[g] = Center.Z;

                    trS[g] = me.Area;

                    Ix[g] = cur.I[g].X;
                    Iy[g] = cur.I[g].Y;
                    Iz[g] = cur.I[g].Z;

                    Mx[g] = cur.M[g].X;
                    My[g] = cur.M[g].Y;
                    Mz[g] = cur.M[g].Z;

                    stenkaIndexer[g] = k;
                    g++;
                }
            }


            CComplex* sol = ApertureRadomeFF(Ix, Iy, Iz, Mx, My, Mz, trX, trY, trZ, trS, Nx, Ny, Nz, radomeUnionSize,
                                             eps_a, mu_a, tickness, gtickness, layersCount, stenkaIndexer,
                                             freq, thetaStart, thetaFinish, phiStart, phiFinish, phiStep, thetaStep, systemOfCoord, proc);
            int j = 0;
            for (int n = 0; n < numberPhi; n++)
            {
                for (int m = 0; m < numberTheta; m++)
                {
                    double phi = phiStart + n * phiStep;
                    double theta = thetaStart + m * thetaStep;
                    double thetaGlobal = GetThetaGlobal(phi, theta, systemOfCoord);
                    double phiGlobal = GetPhiGlobal(phi, theta, systemOfCoord);
                    Complex ex = new Complex(sol[3 * (m * numberPhi + n) + 0].re, sol[3 * (m * numberPhi + n) + 0].im);
                    Complex ey = new Complex(sol[3 * (m * numberPhi + n) + 1].re, sol[3 * (m * numberPhi + n) + 1].im);
                    Complex ez = new Complex(sol[3 * (m * numberPhi + n) + 2].re, sol[3 * (m * numberPhi + n) + 2].im);
                    ffradome.elements[j] = new FarFieldElementC(freq, thetaGlobal, phiGlobal, theta, phi, ex, ey, ez);
                    j++;
                }
            }
            return ffradome;
        }
        

        /// <summary>
        /// Расчёт поля в дальней зоне пройдённого через обтекатель с MultiRadome (с изменяемыми параметрами стенки) на c# (копия FarFieldScattered на с#)
        /// </summary>
        /// <param name="radUnion"></param>
        /// <param name="cur"></param>
        /// <param name="freq"></param>
        /// <param name="thetaStart"></param>
        /// <param name="thetaFinish"></param>
        /// <param name="phiStart"></param>
        /// <param name="phiFinish"></param>
        /// <param name="thetaStep"></param>
        /// <param name="phiStep"></param>
        /// <param name="systemOfCoord"></param>
        /// <param name="title"></param>
        /// <param name="dim"></param>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static FarFieldC FarFieldScatteredCs(Radome radUnion, Current cur, double freq, double thetaStart, double thetaFinish, double phiStart, double phiFinish, double thetaStep, double phiStep, int systemOfCoord, string title, double dim, int proc)
        {
            Complex imOne = new Complex(0, 1);

            double pi = CV.pi;
            double Mu_0 = CV.Mu_0;                                    // 1.0 / (c * c * E_0) Гн/м     magnetic constant    магнитная постоянная        
            double E_0 = CV.E_0;                                      // 8.85e-12 Ф/м        electric constant     электрическая постоянная
            double c0 = CV.c_0;        // м/с скорость света    

            double Omega = 2 * pi * freq;
            double Omega2 = Omega * Omega;
            double K_0 = Omega / c0;
            double K2 = K_0 * K_0;

            Complex iOmega = imOne * Omega;
            Complex Ekoeff = (1.0) / (4 * pi * imOne * Omega * E_0); //fourComx*piComx*imOne*omegaComx*E_0Comx                            
            double pi4 = 4 * pi;
            double pi_2 = pi / 2;
            double pi180 = pi / 180;
            Complex Z0m = imOne * Omega * Mu_0;
            Complex Y0e = imOne * Omega * E_0;

            //Максимально допустимый угол падения
            double cutoff_angle = CV.cutoff_angle;                                   // в радианах 88 градусов



            int numberPhi = Convert.ToInt32((phiFinish - phiStart) / phiStep) + 1;
            int numberTheta = Convert.ToInt32((thetaFinish - thetaStart) / thetaStep) + 1;
            int numberTotal = numberPhi * numberTheta;

            FarFieldC ffradome = new FarFieldC(title, numberTotal);
            //Tuple<Complex, Complex> transition = Tuple.Create<Complex, Complex>(new Complex(1, 0), new Complex(1, 0));
            int radomeSize = radUnion.CountElements;

            double[] trX = new double[radomeSize];
            double[] trY = new double[radomeSize];
            double[] trZ = new double[radomeSize];
            double[] trS = new double[radomeSize];

            Complex[] Ix = new Complex[radomeSize];
            Complex[] Iy = new Complex[radomeSize];
            Complex[] Iz = new Complex[radomeSize];

            Complex[] Mx = new Complex[radomeSize];
            Complex[] My = new Complex[radomeSize];
            Complex[] Mz = new Complex[radomeSize];

            double[] Nx = new double[radomeSize];
            double[] Ny = new double[radomeSize];
            double[] Nz = new double[radomeSize];

            int[] stenkaIndexer = new int[radomeSize];

            int layersSummary = 0;
            for (int i = 0; i < radUnion.Count; i++)
            {
                layersSummary += radUnion[i].Structure.Count;
            }
            Complex[] eps_a = new Complex[layersSummary];
            Complex[] mu_a = new Complex[layersSummary];
            double[] tickness = new double[layersSummary];

            //указывает сколько слоёв в каждом элементе обтекателя
            int[] layersCount = new int[radUnion.Count];

            int s = 0;
            for (int i = 0; i < radUnion.Count; i++)
            {
                layersCount[i] = radUnion[i].Structure.Count;
                for (int j = 0; j < radUnion[i].Structure.Count; j++)
                {
                    eps_a[s] = radUnion[i].Structure[j].Ea;
                    mu_a[s] = radUnion[i].Structure[j].Mua;
                    tickness[s] = radUnion[i].Structure[j].Tickness;
                    s++;
                }
            }

            int t = 0;
            for (int k = 0; k < radUnion.Count; k++)
            {
                for (int i = 0; i < radUnion[k].Count; i++)
                {
                    RadomeElement rel = radUnion[k];
                    Nx[t] = rel[i].Norma.X;
                    Ny[t] = rel[i].Norma.Y;
                    Nz[t] = rel[i].Norma.Z;

                    Point3D Center = rel[i].Center;
                    trX[t] = Center.X;
                    trY[t] = Center.Y;
                    trZ[t] = Center.Z;

                    trS[t] = rel[i].Area;

                    Ix[t] = cur.I[t].X;
                    Iy[t] = cur.I[t].Y;
                    Iz[t] = cur.I[t].Z;

                    Mx[t] = cur.M[t].X;
                    My[t] = cur.M[t].Y;
                    Mz[t] = cur.M[t].Z;

                    stenkaIndexer[t] = k;
                    t++;
                }
            }
            DVector ran1 = new DVector();
            ran1.X = 1;
            ran1.Y = 0;
            ran1.Z = 0;
            DVector ran2 = new DVector();
            ran2.X = 0;
            ran2.Y = 1;
            ran2.Z = 0;

            Task[] tasks = new Task[proc];
            for (int g = 0; g < proc; g++)
            {
                tasks[g] = Task.Factory.StartNew((Object obj) =>
                {
                    int cur_proc = (int)obj;
                    int start = GetStartIndex(cur_proc, proc, numberTotal);
                    int end = GetEndIndex(cur_proc, proc, numberTotal);

                    DVector loc1 = new DVector();
                    DVector loc2 = new DVector();

                    DVector n = null;

                    Complex ex;
                    Complex ey;
                    Complex ez;

                    DVector k = new DVector();
                    CVector jc = new CVector();
                    CVector mc = new CVector();

                    //Tuple<Complex, Complex> transition = new Tuple<Complex, Complex>(new Complex(1, 0), new Complex(1, 0));

                    for (int j = start; j <= end; j++)
                    {
                        double phi = phiStart + (j % numberPhi) * phiStep;
                        double theta = thetaStart + j / numberPhi * thetaStep;

                        double thetaGlobal = GetThetaGlobal(phi, theta, systemOfCoord);      //  в градусах
                        double phiGlobal = GetPhiGlobal(phi, theta, systemOfCoord);          //  в градусах

                        double thetaGlobalRad = thetaGlobal * pi180;      //  в радианах
                        double phiGlobalRad = phiGlobal * pi180;          //  в радианах

                        ex = new Complex();
                        ey = new Complex();
                        ez = new Complex();


                        //
                        // расчет направление вектора распространения волны
                        //
                        k.X = Math.Sin(thetaGlobalRad) * Math.Cos(phiGlobalRad);  // вектор k = {kx, ky, kz}
                        k.Y = Math.Sin(thetaGlobalRad) * Math.Sin(phiGlobalRad);  // направление распространиения
                        k.Z = Math.Cos(thetaGlobalRad);                           // волны в глобальных координатах                        

                        int h = 0;
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////                            
                        for (int b = 0; b < radUnion.Count; b++)
                        {
                            double generalTickness = radUnion[b].Structure.GeneralThickness;
                            for (int i = 0; i < radUnion[b].Count; i++)
                            {
                                n = new DVector();
                                n.X = Nx[h];    // нормаль наружу                                                                                                                                            
                                n.Y = Ny[h];    // нормаль наружу                                                                                                                                            
                                n.Z = Nz[h];    // нормаль наружу                                                                                                                                            
                                //
                                //   Определение осей локальной системы координат
                                //   
                                loc1.X = n.Y * ran1.Z - n.Z * ran1.Y;
                                loc1.Y = n.Z * ran1.X - n.X * ran1.Z;
                                loc1.Z = n.X * ran1.Y - n.Y * ran1.X;

                                if (loc1.Module < 0.5)
                                {
                                    loc1.X = n.Y * ran2.Z - n.Z * ran2.Y;
                                    loc1.Y = n.Z * ran2.X - n.X * ran2.Z;
                                    loc1.Z = n.X * ran2.Y - n.Y * ran2.X;
                                }
                                loc1.Normalize();


                                loc2.X = n.Y * loc1.Z - n.Z * loc1.Y;
                                loc2.Y = n.Z * loc1.X - n.X * loc1.Z;
                                loc2.Z = n.X * loc1.Y - n.Y * loc1.X;
                                loc2.Normalize();

                                double scalNK = DVector.Scal(n, k);     // скалярное произведение вектора нормали n и вектора k;                                

                                if (scalNK > 1)
                                {
                                    scalNK = 1;
                                }
                                if (scalNK < -1)
                                {
                                    scalNK = -1;
                                }

                                //
                                // Определение локальных сферических координат
                                //                                    
                                double thetaLoc = Math.Acos(scalNK);                // угол theta в локальной системе координат, в радианах                                                                     

                                if (scalNK < 0)
                                {
                                    loc2 = (-1) * loc2;
                                    n = (-1) * n;
                                    thetaLoc = pi - thetaLoc;                               // угол theta в локальной системе координат, в радианах
                                }

                                if (thetaLoc > cutoff_angle && thetaLoc <= pi_2)
                                {
                                    thetaLoc = cutoff_angle;
                                }
                                else if (thetaLoc < 0.001)
                                {
                                    thetaLoc = 0.001;
                                }

                                //
                                // расчет каппа 1 и каппа 2
                                //
                                double scalPLoc1 = DVector.Scal(k, loc1);               // скалярное произведение вектора p и вектора loc1                                
                                if (scalPLoc1 > 1)
                                {
                                    scalPLoc1 = 1;
                                }


                                double scalPLoc2 = DVector.Scal(k, loc2);
                                if (scalPLoc2 > 1)
                                {
                                    scalPLoc2 = 1;
                                }

                                double phiLoc;
                                if (scalPLoc2 >= 0)
                                {
                                    phiLoc = Math.Acos(scalPLoc1);          // угол phi в локальной системе координат, в радианах
                                }
                                else
                                {
                                    phiLoc = (-1) * Math.Acos(scalPLoc1);       // угол phi в локальной системе координат, в радианах
                                }

                                double kp1 = K_0 * Math.Sin(thetaLoc) * Math.Cos(phiLoc);   // kp1, kp2 - пространственные частоты
                                double kp2 = K_0 * Math.Sin(thetaLoc) * Math.Sin(phiLoc);   // или период изменения фазы вдоль выделенноего направления (углы в радианах)

                                double kappa2 = kp1 * kp1 + kp2 * kp2;
                                Complex ikappa2 = imOne * kappa2;
                                double kp1dS = kp1 * trS[h];
                                double kp2dS = kp2 * trS[h];

                                //
                                // Расчет локальных токов
                                //               
                                jc.X = Ix[h];
                                jc.Y = Iy[h];
                                jc.Z = Iz[h];

                                mc.X = Mx[h];
                                mc.Y = My[h];
                                mc.Z = Mz[h];

                                Complex iLoc1 = CVector.Scal(jc, loc1); // локальные электрические токи
                                Complex iLoc2 = CVector.Scal(jc, loc2);

                                Complex mLoc1 = CVector.Scal(mc, loc1); // локальные магнитные токи
                                Complex mLoc2 = CVector.Scal(mc, loc2);

                                //
                                // расчет гамма в свободном пространстве
                                //               
                                Complex gamma0 = Complex.Sqrt(kappa2 - K2);
                                //
                                //   Волновое сопротивление в свободном пространстве
                                //
                                Complex W0e = gamma0 / Y0e;
                                Complex W0m = Z0m / gamma0;

                                //
                                // матрица передачи диэлектрического слоя
                                //
                                //Tuple<Complex, Complex> transition = TRCoeffitients.TransmissionCoefficientCalc(radUnion[b].Structure, kappa2, Omega, K2, Y0e, Z0m);
                                //Stenka st = radUnion[b].Structure;


                                Matrix<Complex> generalMatrixEtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
                                Matrix<Complex> generalMatrixMtype = Matrix<Complex>.Build.Dense(2, 2, new Complex[] { 1, 0, 0, 1 });
                                Complex iomega = imOne * Omega;

                                int indexOfStenka = stenkaIndexer[h];
                                int layerCount = layersCount[indexOfStenka];

                                int numberoflayersbefore = 0;
                                for (int q = 0; q < indexOfStenka; q++)
                                {
                                    numberoflayersbefore += layersCount[q];
                                }
                                for (int l = 0; l < layerCount; l++)
                                {

                                    Complex Ea = eps_a[numberoflayersbefore + l];
                                    Complex Mua = mu_a[numberoflayersbefore + l];
                                    double d = tickness[numberoflayersbefore + l];

                                    //Complex Zm = iomega * Mua;
                                    //Complex Ye = iomega * Ea;

                                    //Complex a_e, b_e, c_e, d_e;
                                    //Complex a_m, b_m, c_m, d_m;
                                    //
                                    //   определение волновое число в диэлектрике
                                    //
                                    //Complex k_a = Omega * Complex.Sqrt(Mua * Ea);

                                    //
                                    // расчет гамма в диэлектрике и в свободном пространстве
                                    //
                                    Complex gamma = Complex.Sqrt(kappa2 - Omega2 * Mua * Ea);
                                    //
                                    //   Волновое сопротивление в диэлектрике
                                    //
                                    Complex we = gamma / (iomega * Ea);
                                    Complex wm = iomega * Mua / gamma;
                                    Complex gammaL = gamma * d;


                                    Complex sinhGT = Complex.Sinh(gammaL);
                                    Complex coshGT = Complex.Cosh(gammaL);


                                    //a_e = coshGT;
                                    //a_m = coshGT;
                                    //b_e = we * sinhGT;
                                    //b_m = wm * sinhGT;
                                    //c_e = 1 / we * sinhGT;
                                    //c_m = 1 / wm * sinhGT;
                                    //d_e = coshGT;
                                    //d_m = coshGT;


                                    Matrix<Complex> matrixEtype = Matrix<Complex>.Build.Dense(2, 2);//, new Complex[] { a_e, b_e, c_e, d_e });
                                    matrixEtype[0, 0] = coshGT;
                                    matrixEtype[1, 1] = coshGT;
                                    matrixEtype[1, 0] = we * sinhGT;
                                    matrixEtype[0, 1] = 1 / we * sinhGT;


                                    generalMatrixEtype *= matrixEtype;

                                    Matrix<Complex> matrixMtype = Matrix<Complex>.Build.Dense(2, 2); //, new Complex[] { a_m, b_m, c_m, d_m });
                                    matrixMtype[0, 0] = coshGT;
                                    matrixMtype[1, 1] = coshGT;
                                    matrixMtype[1, 0] = wm * sinhGT;
                                    matrixMtype[0, 1] = 1 / wm * sinhGT;
                                    generalMatrixMtype *= matrixMtype;
                                }


                                Complex A_e = generalMatrixEtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
                                Complex A_m = generalMatrixMtype[0, 0];    // а элемент матрицы передачи сквозь весь диэлектрик для полей типа H
                                Complex C_e = generalMatrixEtype[0, 1];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
                                Complex C_m = generalMatrixMtype[0, 1];    // с элемент матрицы передачи сквозь весь диэлектрик для полей типа H
                                Complex B_e = generalMatrixEtype[1, 0];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
                                Complex B_m = generalMatrixMtype[1, 0];    // b элемент матрицы передачи сквозь весь диэлектрик для полей типа H                                
                                Complex D_e = generalMatrixEtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа Е
                                Complex D_m = generalMatrixMtype[1, 1];    // d элемент матрицы передачи сквозь весь диэлектрик для полей типа H

                                //
                                //   входное сопротивление длинной линии со стороны слоя
                                //


                                //Complex inputZe = (A_e * W0e + B_e) / (C_e * W0e + D_e);
                                //Complex inputZm = (A_m * W0m + B_m) / (C_m * W0m + D_m);

                                Complex tE = 1 / (A_e + B_e / W0e + C_e * W0e + D_e);

                                Complex tH = 1 / (A_m + B_m / W0m + C_m * W0m + D_m);
                                //
                                // расчет коэффициента прохождения
                                //

                                //Complex tE = inputZe * W0e / (inputZe + W0e) / (A_e * W0e + B_e);
                                //Complex tH = inputZm * W0m / (inputZm + W0m) / (A_m * W0m + B_m);

                                //if (thetaLoc == 0.001)
                                //{
                                //    MessageBox.Show("Fv");
                                //}

                                //
                                //   Расчет спектров входных источников напряжения и токов
                                //
                                Complex spectorFe = (kp1dS * iLoc1 + kp2dS * iLoc2) / ikappa2;
                                Complex spectorFm = (kp2dS * iLoc1 - kp1dS * iLoc2) / ikappa2;
                                Complex spectorEe = (-1) * (kp2dS * mLoc1 - kp1dS * mLoc2) / ikappa2;
                                Complex spectorEm = (kp1dS * mLoc1 + kp2dS * mLoc2) / ikappa2;
                                //
                                // Входные источники напряжения электрического и магнитного полей
                                //
                                Complex ue1 = spectorEe + spectorFe * W0e;
                                Complex um1 = spectorEm + spectorFm * W0m;
                                //
                                // Выходные источники напряжения электрического и магнитного полей
                                //
                                Complex ue2 = ue1 * tE;
                                Complex um2 = um1 * tH;
                                //
                                // расчет спектра полей
                                //                                
                                //x, y, z компоненты спектора 
                                //электричекого поля в локальной 
                                //системе координат         
                                Complex spExLoc = (-1) * imOne * (kp1 * ue2 + kp2 * um2);
                                Complex spEyLoc = (-1) * imOne * (kp2 * ue2 - kp1 * um2);
                                Complex spEzLoc = (-1) * kappa2 * ue2 / gamma0;


                                //x, y, z компоненты спектора 
                                //электричекого поля в глобальной 
                                //системе координат
                                Complex spEx = (spExLoc * loc1.X + spEyLoc * loc2.X + spEzLoc * n.X) / pi4;
                                Complex spEy = (spExLoc * loc1.Y + spEyLoc * loc2.Y + spEzLoc * n.Y) / pi4;
                                Complex spEz = (spExLoc * loc1.Z + spEyLoc * loc2.Z + spEzLoc * n.Z) / pi4;
                                //
                                // расчет поля в дальней зоне
                                //
                                double gammaLoc = K_0 * Math.Cos(thetaLoc);
                                Complex i2gammaLoc = 2 * gammaLoc * imOne;


                                double argument = K_0 * (trX[h] * k.X + trY[h] * k.Y + trZ[h] * k.Z) + generalTickness * gammaLoc;

                                Complex exp = Complex.Exp(Complex.One * argument);
                                Complex i2gammaLocEXP = 2 * gammaLoc * imOne * Complex.Exp(new Complex(0, argument));

                                ex += i2gammaLocEXP * spEx;
                                ey += i2gammaLocEXP * spEy;
                                ez += i2gammaLocEXP * spEz;
                                h++;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ffradome.elements[j] = new FarFieldElementC(freq, thetaGlobal, phiGlobal, theta, phi, ex, ey, ez);
                    }
                }, g);
            }
            Task.WaitAll(tasks);
            return ffradome;
        }


        public static FarFieldC Add(FarFieldC a, FarFieldC b)
        {
            int count = a.Count;
            FarFieldC ff = new FarFieldC(a.Title, count);
            for (int i = 0; i < count; i++)
            {
                FarFieldElementC el_a = a.elements[i];
                FarFieldElementC el_b = b.elements[i];
                ff.elements[i] = new FarFieldElementC(el_a.Frequency, el_a.Theta, el_a.Phi, el_a.LocalTheta, el_a.LocalPhi, el_a.Ex + el_b.Ex, el_a.Ey + el_b.Ey, el_a.Ez + el_b.Ez);
            }
            return ff;
        }

        static int GetStartIndex(int cur_proc, int proc, int matrix_size)
        {
            int eq_number = matrix_size / proc;
            int overflow_eq = matrix_size % proc;
            if (overflow_eq != 0)
            {
                eq_number++;
            }
            int cur_cor_order = 0;
            if (overflow_eq != 0 && cur_proc > overflow_eq)
            {
                cur_cor_order = cur_proc - overflow_eq;
            }
            int start = cur_proc * eq_number - cur_cor_order;
            return start;
        }
        static int GetEndIndex(int cur_proc, int proc, int matrix_size)
        {
            cur_proc++;
            int eq_number = matrix_size / proc;
            int overflow_eq = matrix_size % proc;
            if (overflow_eq != 0)
            {
                eq_number++;
            }
            int cur_cor_order = 0;
            if (overflow_eq != 0 && cur_proc > overflow_eq)
            {
                cur_cor_order = cur_proc - overflow_eq;
            }
            int end = cur_proc * eq_number - 1 - cur_cor_order;
            return end;
        }
        static double GetThetaGlobal(double phi, double theta, int system)
        {
            double a = 0; double b = 0; double c = 0;

            if (system == 0)
            {
                a = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                b = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                c = Math.Cos(theta * Math.PI / 180);
            }
            else if (system == 1)
            {
                c = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                a = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                b = Math.Cos(theta * Math.PI / 180);
            }
            else if (system == 2)
            {
                b = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                c = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                a = Math.Cos(theta * Math.PI / 180);
            }

            double m = c / Math.Sqrt(a * a + b * b + c * c);
            if (m < -1)
            {
                m = -1;
            }
            else if (m > 1)
            {
                m = 1;
            }

            return Math.Acos(m) / Math.PI * 180;
        }
        static double GetPhiGlobal(double phi, double theta, int system)
        {
            double a = 0; double b = 0; double c = 0;

            if (system == 0)
            {
                a = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                b = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                c = Math.Cos(theta * Math.PI / 180);
            }
            else if (system == 1)
            {
                c = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                a = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                b = Math.Cos(theta * Math.PI / 180);
            }
            else if (system == 2)
            {
                b = Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180);
                c = Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180);
                a = Math.Cos(theta * Math.PI / 180);
            }
            double m = c / Math.Sqrt(a * a + b * b + c * c);
            if (m < -1)
            {
                m = -1;
            }
            else if (m > 1)
            {
                m = 1;
            }

            double thetaGlobal = Math.Acos(m) / Math.PI * 180;
            if (thetaGlobal == 0)
            {
                return 0;
            }
            else
            {
                double v = a / Math.Sin(thetaGlobal * Math.PI / 180);
                if (v > 1)
                {
                    v = 1;
                }
                else if (v < -1)
                {
                    v = -1;
                }

                double phiGlobal = Math.Acos(v) / Math.PI * 180;
                //if (theta > 90 && theta < 270)
                //{
                //    return 360 - phiGlobal;
                //}
                //else
                //{
                //    return phiGlobal;
                //}

                if (b < 0)
                {
                    return 360 - phiGlobal;
                }
                else
                {
                    return phiGlobal;
                }
            }

        }


    }


    public class FarFieldElementC
    {        
        const double pi = Math.PI;        

        public double LocalPhi = 0;
        public double LocalTheta = 0;
        public Complex Ex { get; set; }
        public Complex Ey { get; set; }
        public Complex Ez { get; set; }

        public double Phi { get; set; }
        public double Theta { get; set; }
        public double Frequency { get; set; }


        //конструктор
        public FarFieldElementC(double freq, double thetaG, double phiG, double thetaL, double phiL, Complex ex, Complex ey, Complex ez)
        {
            Phi = phiG;
            Theta = thetaG;
            Frequency = freq;
            LocalTheta = thetaL;
            LocalPhi = phiL;
            Ex = ex; Ey = ey; Ez = ez;
        }

        //Свойства
        public double Ephi
        {
            get
            {
                double phi = Phi * pi / 180;
                double theta = Theta * pi / 180;

                DVector v_phi = new DVector(-Math.Sin(phi), Math.Cos(phi), 0);
                v_phi.Normalize();
                Complex phiProjectionE = Projection(v_phi);
                return 20 * Math.Log10(phiProjectionE.Magnitude);
            }
        }
        public double Etheta
        {
            get
            {
                double phi = Phi * pi / 180;
                double theta = Theta * pi / 180;

                DVector v_theta = new DVector(-Math.Cos(theta) * Math.Cos(phi), -Math.Cos(theta) * Math.Sin(phi), Math.Sin(theta));
                v_theta.Normalize();
                Complex thetaProjectionE = Projection(v_theta);
                return 20 * Math.Log10(thetaProjectionE.Magnitude);
            }
        }
        public double Etotal
        {
            get
            {
                double phi = Phi * pi / 180;
                double theta = Theta * pi / 180;

                DVector v_phi = new DVector(-Math.Sin(phi), Math.Cos(phi), 0);
                DVector v_theta = new DVector(-Math.Cos(theta) * Math.Cos(phi), -Math.Cos(theta) * Math.Sin(phi), Math.Sin(theta));
                v_phi.Normalize();
                v_theta.Normalize();
                Complex phiProjectionE = Projection(v_phi);
                Complex thetaProjectionE = Projection(v_theta);
                Complex totalProjectionE = Math.Sqrt(thetaProjectionE.Magnitude * thetaProjectionE.Magnitude + phiProjectionE.Magnitude * phiProjectionE.Magnitude);

                return 20 * Math.Log10(totalProjectionE.Magnitude);
            }
        }
        public double EthetaPhase
        {
            get
            {
                double phi = Phi * pi / 180;
                double theta = Theta * pi / 180;

                DVector v_theta = new DVector(-Math.Cos(theta) * Math.Cos(phi), -Math.Cos(theta) * Math.Sin(phi), Math.Sin(theta));
                v_theta.Normalize();
                Complex thetaProjectionE = Projection(v_theta);
                return thetaProjectionE.Phase / pi * 180;
            }
        }
        public double EphiPhase
        {
            get
            {
                double phi = Phi * pi / 180;
                double theta = Theta * pi / 180;

                DVector v_phi = new DVector(-Math.Sin(phi), Math.Cos(phi), 0);
                v_phi.Normalize();
                Complex phiProjectionE = Projection(v_phi);
                return phiProjectionE.Phase / pi * 180;
            }
        }


        /// <summary>
        /// Амплитуда поля дальней зоны прямой составляющей поля 
        /// </summary>
        /// <param name="p">Вектор собственной поляризации</param>
        /// <returns></returns>
        public double EdirectMagnitude(Aperture ap)
        {
            DVector p = ap.GetPolarization(Theta, Phi);
            Complex direct = CVector.Scal(new CVector(this.Ex, this.Ey, this.Ez), p);

            return 20 * Math.Log10(direct.Magnitude);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">Вектор собственной поляризации</param>
        /// <param name="J">Вектор направления распространения волны</param>
        /// <returns></returns>
        public double EcrossMagnitude(Aperture ap)
        {
            DVector p = ap.GetPolarization(Theta, Phi);

            double x = Math.Sin(Theta * Math.PI / 180) * Math.Cos(Phi * Math.PI / 180);
            double y = Math.Sin(Theta * Math.PI / 180) * Math.Sin(Phi * Math.PI / 180);
            double z = Math.Cos(Theta * Math.PI / 180);

            DVector J = new DVector(x - ap.Center.X, y - ap.Center.Y, z - ap.Center.Z);
            DVector crossVector = DVector.Cross(p, J);

            Complex cross = CVector.Scal(new CVector(this.Ex, this.Ey, this.Ez), crossVector);
            return 20 * Math.Log10(cross.Magnitude);
        }
        public double EdirectPhase(Aperture ap)
        {
            DVector p = ap.GetPolarization(Theta, Phi);
            Complex direct = CVector.Scal(new CVector(this.Ex, this.Ey, this.Ez), p);

            return direct.Phase;
        }
        public double EcrossPhase(Aperture ap)
        {
            DVector p;
            DVector crossVector;

            p = ap.GetPolarization(Theta, Phi);

            double x = Math.Sin(Theta * Math.PI / 180) * Math.Cos(Phi * Math.PI / 180);
            double y = Math.Sin(Theta * Math.PI / 180) * Math.Sin(Phi * Math.PI / 180);
            double z = Math.Cos(Theta * Math.PI / 180);

            DVector J = new DVector(x - ap.Center.X, y - ap.Center.Y, z - ap.Center.Z);
            crossVector = DVector.Cross(p, J);

            Complex cross = CVector.Scal(new CVector(this.Ex, this.Ey, this.Ez), crossVector);
            return cross.Phase;
        }


        private Complex Projection(DVector vector)
        {
            Complex projVectorE = (this.Ex * vector.X + this.Ey * vector.Y + this.Ez * vector.Z);
            if (projVectorE.Magnitude <= 0.00001)
            {
                projVectorE = 0.00001;
            }
            return projVectorE;
        }
    }
}
