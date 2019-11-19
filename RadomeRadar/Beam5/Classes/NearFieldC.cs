using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Apparat
{
    unsafe public class NearFieldC
    {
        readonly NearFieldElement[] field;

        public NearFieldElement this[int index]
        {
            get
            {
                return field[index];
            }
            set
            {
                field[index] = value;
            }
        }

        public int Count
        {
            get { return this.field.Length; }
        }
        public NearFieldC(int size)
        {
            field = new NearFieldElement[size];
            for (int i = 0; i < size; i++)
            {
                field[i] = new NearFieldElement();
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);       

        //@"C:\Users\Denis\YandexDisk\Work\RadomeRadar_Beta\RadomeRadar\NearFieldLibrary\x64\Release\Solver.dll"
        [DllImport("Solver.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern CComplex* SurfaceCurrentToGeometry(double[] ApX, double[] ApY, double[] ApZ, double[] ApS,
                                    Complex[] Ix, Complex[] Iy, Complex[] Iz,
                                    Complex[] Mx, Complex[] My, Complex[] Mz,
                                    double[] GeoX, double[] GeoY, double[] GeoZ,
                                    double freq, int ApSize, int GeoSize, ref int pthread);

        [DllImport("Solver.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern CComplex* ReflactedFromRadomeNF(Complex[] incEx, Complex[] incEy, Complex[] incEz, Complex[] incHx, Complex[] incHy, Complex[] incHz,
                                double[] Nx, double[] Ny, double[] Nz, double[] px, double[] py, double[] pz,
                                Complex[] eps_a, Complex[] mu_a, double[] tickness, double[] gtickness, int[] layersCount, int[] stenkaIndexer,
                                int numberElements, double freq, ref int proc);

        [DllImport("Solver.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern CComplex* InducedNF(Complex[] Ix, Complex[] Iy, Complex[] Iz, Complex[] Mx, Complex[] My, Complex[] Mz,
                                             double[] Nx, double[] Ny, double[] Nz, double[] Area, int size,
                                             double[] pXO, double[] pYO, double[] pZO,
                                             double[] pXC, double[] pYC, double[] pZC,
                                             double[] pX1, double[] pY1, double[] pZ1,
                                             double[] pX2, double[] pY2, double[] pZ2,
                                             double[] pX3, double[] pY3, double[] pZ3,
                                             double freq, ref int pthread);

        //Расчёт ближнего поля, возбуждаемого токами на апертуре 
        unsafe public static NearFieldC SurfaceCurToGeometryCpp(Aperture app, Radome geom, double freq, float dim, int proc)
        {

            int ApSize = app.ApertureCurrent.Count;
            double[] ApX = new double[ApSize];
            double[] ApY = new double[ApSize];
            double[] ApZ = new double[ApSize];
            double[] ApS = new double[ApSize];

            for (int i = 0; i < ApSize; i++)
            {
                ApX[i] = app.ApertureCurrent[i].P.X;
                ApY[i] = app.ApertureCurrent[i].P.Y;
                ApZ[i] = app.ApertureCurrent[i].P.Z;
                ApS[i] = app.ApertureCurrent[i].S;
            }


            Complex[] Ix = new Complex[ApSize];
            Complex[] Iy = new Complex[ApSize];
            Complex[] Iz = new Complex[ApSize];

            Complex[] Mx = new Complex[ApSize];
            Complex[] My = new Complex[ApSize];
            Complex[] Mz = new Complex[ApSize];

            for (int j = 0; j < ApSize; j++)
            {
                Ix[j] = app.ApertureCurrent[j].I.X;
                Iy[j] = app.ApertureCurrent[j].I.Y;
                Iz[j] = app.ApertureCurrent[j].I.Z;

                Mx[j] = app.ApertureCurrent[j].M.X;
                My[j] = app.ApertureCurrent[j].M.Y;
                Mz[j] = app.ApertureCurrent[j].M.Z;
            }

            int GeoSize = geom.CountElements;
            double[] GeoX = new double[GeoSize];
            double[] GeoY = new double[GeoSize];
            double[] GeoZ = new double[GeoSize];

            int h = 0;
            for (int r = 0; r < geom.Count; r++)
            {
                RadomeElement finalGeom = geom[r];

                for (int k = 0; k < finalGeom.Count; k++)
                {
                    GeoX[h] = finalGeom[k].Center.X;
                    GeoY[h] = finalGeom[k].Center.Y;
                    GeoZ[h] = finalGeom[k].Center.Z;
                    h++;
                }
            }



            CComplex* sol = SurfaceCurrentToGeometry(ApX, ApY, ApZ, ApS, Ix, Iy, Iz, Mx, My, Mz, GeoX, GeoY, GeoZ, freq, ApSize, GeoSize, ref proc);

            NearFieldC ans = new NearFieldC(GeoSize);

            for (int i = 0; i < GeoSize; i++)
            {
                ans[i].E = new CVector(new Complex(sol[i * 6 + 0].re, sol[i * 6 + 0].im), new Complex(sol[i * 6 + 1].re, sol[i * 6 + 1].im), new Complex(sol[i * 6 + 2].re, sol[i * 6 + 2].im));
                ans[i].H = new CVector(new Complex(sol[i * 6 + 3].re, sol[i * 6 + 3].im), new Complex(sol[i * 6 + 4].re, sol[i * 6 + 4].im), new Complex(sol[i * 6 + 5].re, sol[i * 6 + 5].im));
                ans[i].Place = new Point3D(GeoX[i], GeoY[i], GeoZ[i]);
            }

            return ans;
        }
        public static NearFieldC SurfaceCurToGeometryCs(Aperture app, Radome geom, double freq, float dim, int proc)
        {
            Complex imOneMin = new Complex(0, -1);      // минус мнимая единица 
            double pi = CV.pi;
            Complex imOne = new Complex(0, 1);


            double Lambda = CV.c_0 / freq;
            double Omega = 2 * pi * freq;
            double K_0 = 2 * pi / Lambda;      // волновое число 2pi/lambda
            double K2 = K_0 * K_0;
            Complex iomega = imOne * Omega;
            Complex Ekoeff = (-1.0) / (iomega * CV.E_0);          // 1/i*omega*E_0
            Complex Mukoeff = (-1.0) / (iomega * CV.Mu_0);         // -1/i*omega*Mu_0

            int ApSize = app.Count;
            double[] ApX = new double[ApSize];
            double[] ApY = new double[ApSize];
            double[] ApZ = new double[ApSize];
            double[] ApS = new double[ApSize];
            float dim2 = dim * dim;
            for (int i = 0; i < ApSize; i++)
            {
                ApX[i] = dim * app[i].Center.X;
                ApY[i] = dim * app[i].Center.Y;
                ApZ[i] = dim * app[i].Center.Z;
                ApS[i] = dim2 * app[i].Area;
            }


            Complex[] Ix = new Complex[ApSize];
            Complex[] Iy = new Complex[ApSize];
            Complex[] Iz = new Complex[ApSize];

            Complex[] Mx = new Complex[ApSize];
            Complex[] My = new Complex[ApSize];
            Complex[] Mz = new Complex[ApSize];

            for (int j = 0; j < ApSize; j++)
            {
                Ix[j] = app.ApertureCurrent[j].I.X;
                Iy[j] = app.ApertureCurrent[j].I.Y;
                Iz[j] = app.ApertureCurrent[j].I.Z;

                Mx[j] = app.ApertureCurrent[j].M.X;
                My[j] = app.ApertureCurrent[j].M.Y;
                Mz[j] = app.ApertureCurrent[j].M.Z;
            }

            int GeoSize = geom.CountElements;
            double[] GeoX = new double[GeoSize];
            double[] GeoY = new double[GeoSize];
            double[] GeoZ = new double[GeoSize];

            int h = 0;
            for (int r = 0; r < geom.Count; r++)
            {
                RadomeElement finalGeom = geom[r];

                for (int k = 0; k < finalGeom.Count; k++)
                {
                    GeoX[h] = finalGeom[h].Center.X * dim;
                    GeoY[h] = finalGeom[h].Center.Y * dim;
                    GeoZ[h] = finalGeom[h].Center.Z * dim;
                    h++;
                }
            }


            Complex ex, ey, ez, hx, hy, hz;
            NearFieldC outField = new NearFieldC(GeoSize);


            for (int j = 0; j < GeoSize; j++)
            {
                //st.Start();
                ex = new System.Numerics.Complex(0, 0);
                ey = new System.Numerics.Complex(0, 0);
                ez = new System.Numerics.Complex(0, 0);

                hx = new System.Numerics.Complex(0, 0);
                hy = new System.Numerics.Complex(0, 0);
                hz = new System.Numerics.Complex(0, 0);

                float xsecond = Convert.ToSingle(GeoX[j] * dim);
                float ysecond = Convert.ToSingle(GeoY[j] * dim);
                float zsecond = Convert.ToSingle(GeoZ[j] * dim);

                Point3D pointsecond = new Point3D(xsecond, ysecond, zsecond);


                for (int p = 0; p < ApSize; p++)
                {
                    //
                    //	Координаты 
                    //
                    float xprime = Convert.ToSingle(ApX[p] * dim);
                    float yprime = Convert.ToSingle(ApY[p] * dim);
                    float zprime = Convert.ToSingle(ApZ[p] * dim);

                    Point3D pointprime = new Point3D(xprime, yprime, zprime);

                    CVector i = new CVector(Ix[p], Iy[p], Iz[p]);
                    CVector m = new CVector(Mx[p], My[p], Mz[p]);
                    float square = Convert.ToSingle(ApS[p] * dim2);

                    //Field FieldC = ElementFieldCalcP(icur, mcur, pointprime, pointsecond, square);

                    //
                    //  Переменные
                    //          
                    double x_x0 = xprime - xsecond;
                    double y_y0 = yprime - ysecond;
                    double z_z0 = zprime - zsecond;
                    double x2 = x_x0 * x_x0;
                    double y2 = y_y0 * y_y0;
                    double z2 = z_z0 * z_z0;

                    double r = Math.Sqrt(x2 + y2 + z2);
                    Complex exp_ikr = new Complex(Math.Cos(K_0 * r), -Math.Sin(K_0 * r));
                    //Complex exp_ikr = Complex.Exp(imOneMin * K_0 * r);

                    Complex funG = exp_ikr / (4 * pi * r);
                    double r2 = r * r;
                    double r4 = r2 * r2;

                    //
                    //НАЧАЛО КОСТИНЫ ФОРМУЛЫ
                    //
                    Complex coeffA = imOneMin * K_0 / r - 1.0 / r2;
                    Complex coeffB = (3.0 + 3.0 * imOne * K_0 * r - K2 * r2) / r4;

                    Complex dx = x_x0 * coeffA * funG;
                    Complex dy = y_y0 * coeffA * funG;
                    Complex dz = z_z0 * coeffA * funG;

                    Complex dxx = (x2 * coeffB + coeffA) * funG;
                    Complex dyy = (y2 * coeffB + coeffA) * funG;
                    Complex dzz = (z2 * coeffB + coeffA) * funG;

                    Complex dxy = (x_x0 * y_y0 * coeffB) * funG;
                    Complex dxz = (x_x0 * z_z0 * coeffB) * funG;
                    Complex dyz = (y_y0 * z_z0 * coeffB) * funG;
                    //
                    // КОНЕЦ
                    //

                    ex += (-1) * (Ekoeff * (K2 * i.X * funG + i.X * dxx + i.Y * dxy + i.Z * dxz) * square - (m.Z * dy - m.Y * dz) * square); //-
                    ey += (-1) * (Ekoeff * (K2 * i.Y * funG + i.Y * dyy + i.Z * dyz + i.X * dxy) * square - (m.X * dz - m.Z * dx) * square); //-
                    ez += (-1) * (Ekoeff * (K2 * i.Z * funG + i.Z * dzz + i.X * dxz + i.Y * dyz) * square - (m.Y * dx - m.X * dy) * square); //-

                    hx += (-1) * (Mukoeff * (K2 * m.X * funG + m.X * dxx + m.Y * dxy + m.Z * dxz) * square + (i.Z * dy - i.Y * dz) * square); //+
                    hy += (-1) * (Mukoeff * (K2 * m.Y * funG + m.X * dxy + m.Y * dyy + m.Z * dyz) * square + (i.X * dz - i.Z * dx) * square); //+
                    hz += (-1) * (Mukoeff * (K2 * m.Z * funG + m.X * dxz + m.Y * dyz + m.Z * dzz) * square + (i.Y * dx - i.X * dy) * square); //+
                }
                outField[j].E = new CVector(ex, ey, ez);
                outField[j].H = new CVector(hx, hy, hz);
                outField[j].Place = new Point3D(GeoX[j], GeoY[j], GeoZ[j]);
            }
            return outField;
        }

        unsafe public static NearFieldC ReflactedNearFieldCpp(Radome radUnion, NearFieldC incidentField, double freq, int proc)
        {
            int radomeUnionSize = radUnion.CountElements;
            double[] Nx = new double[radomeUnionSize];
            double[] Ny = new double[radomeUnionSize];
            double[] Nz = new double[radomeUnionSize];

            Complex[] incEx = new Complex[radomeUnionSize];
            Complex[] incEy = new Complex[radomeUnionSize];
            Complex[] incEz = new Complex[radomeUnionSize];

            Complex[] incHx = new Complex[radomeUnionSize];
            Complex[] incHy = new Complex[radomeUnionSize];
            Complex[] incHz = new Complex[radomeUnionSize];

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

            double[] px = new double[radomeUnionSize];
            double[] py = new double[radomeUnionSize];
            double[] pz = new double[radomeUnionSize];
            int h = 0;
            for (int w = 0; w < radUnion.Count; w++)
            {
                RadomeElement rad = radUnion[w];
                for (int i = 0; i < rad.Count; i++)
                {
                    Nx[h] = (-1) * rad[i].Norma.X;
                    Ny[h] = (-1) * rad[i].Norma.Y;
                    Nz[h] = (-1) * rad[i].Norma.Z;

                    incEx[h] = incidentField[h].E.X;
                    incEy[h] = incidentField[h].E.Y;
                    incEz[h] = incidentField[h].E.Z;

                    incHx[h] = incidentField[h].H.X;
                    incHy[h] = incidentField[h].H.Y;
                    incHz[h] = incidentField[h].H.Z;

                    px[h] = rad[i].Center.X;
                    py[h] = rad[i].Center.Y;
                    pz[h] = rad[i].Center.Z;

                    stenkaIndexer[h] = w;
                    h++;
                }
            }

            int numberElements = incidentField.Count;
            NearFieldC reflectedNearField = new NearFieldC(numberElements);

            CComplex* sol = ReflactedFromRadomeNF(incEx, incEy, incEz, incHx, incHy, incHz,
                                Nx, Ny, Nz, px, py, pz,
                                eps_a, mu_a, tickness, gtickness, layersCount, stenkaIndexer,
                                numberElements, freq, ref proc);


            for (int i = 0; i < numberElements; i++)
            {
                reflectedNearField[i].E = new CVector(new Complex(sol[i * 6 + 0].re, sol[i * 6 + 0].im), new Complex(sol[i * 6 + 1].re, sol[i * 6 + 1].im), new Complex(sol[i * 6 + 2].re, sol[i * 6 + 2].im));
                reflectedNearField[i].H = new CVector(new Complex(sol[i * 6 + 3].re, sol[i * 6 + 3].im), new Complex(sol[i * 6 + 4].re, sol[i * 6 + 4].im), new Complex(sol[i * 6 + 5].re, sol[i * 6 + 5].im));
                reflectedNearField[i].Place = new Point3D(px[i], py[i], pz[i]);
            }
            return reflectedNearField;
        }
        public static NearFieldC ReflactedNearFieldCs(Radome radUnion, NearFieldC incidentField, double freq, int proc)
        {
            double pi = CV.pi;
            Complex imOne = new Complex(0, 1);

            double Omega = CV.Omega;
            double K_0 = CV.K_0;
            double K2 = CV.K2;
            Complex iOmega = CV.iOmega;
            Complex Ekoeff = CV.Ekoeff;

            Complex Z0m = CV.Z0m;
            Complex Y0e = CV.Y0e;

            double cutoff_angle = CV.cutoff_angle;

            int radomeUnionSize = radUnion.CountElements;
            double[] Nx = new double[radomeUnionSize];
            double[] Ny = new double[radomeUnionSize];
            double[] Nz = new double[radomeUnionSize];

            Complex[] incEx = new Complex[radomeUnionSize];
            Complex[] incEy = new Complex[radomeUnionSize];
            Complex[] incEz = new Complex[radomeUnionSize];

            Complex[] incHx = new Complex[radomeUnionSize];
            Complex[] incHy = new Complex[radomeUnionSize];
            Complex[] incHz = new Complex[radomeUnionSize];

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

            Point3D[] pointsCenter = new Point3D[radomeUnionSize];
            int h = 0;
            for (int w = 0; w < radUnion.Count; w++)
            {
                RadomeElement rad = radUnion[w];
                for (int i = 0; i < rad.Count; i++)
                {
                    Nx[h] = (-1) * rad[i].Norma.X;
                    Ny[h] = (-1) * rad[i].Norma.Y;
                    Nz[h] = (-1) * rad[i].Norma.Z;

                    incEx[h] = incidentField[h].E.X;
                    incEy[h] = incidentField[h].E.Y;
                    incEz[h] = incidentField[h].E.Z;

                    incHx[h] = incidentField[h].H.X;
                    incHy[h] = incidentField[h].H.Y;
                    incHz[h] = incidentField[h].H.Z;

                    pointsCenter[h] = rad[i].Center;

                    stenkaIndexer[h] = w;
                    h++;
                }
            }

            int numberElements = incidentField.Count;
            NearFieldC reflectedNearField = new NearFieldC(numberElements);

            Task[] tasks = new Task[proc];
            for (int g = 0; g < proc; g++)
            {
                tasks[g] = Task.Factory.StartNew((Object obj) =>
                {
                    int cur_proc = (int)obj;
                    int start = GetStartIndex(cur_proc, proc, numberElements);
                    int end = GetEndIndex(cur_proc, proc, numberElements);


                    for (int j = start; j <= end; j++)
                    {
                        double thetaLoc = 0;

                        double nx = Nx[j];
                        double ny = Ny[j];
                        double nz = Nz[j];

                        Complex Eincx = incEx[j];
                        Complex Eincy = incEy[j];
                        Complex Eincz = incEz[j];

                        Complex Hincx = incHx[j];
                        Complex Hincy = incHy[j];
                        Complex Hincz = incHz[j];

                        Complex HincxCoj = Complex.Conjugate(incHx[j]);
                        Complex HincyCoj = Complex.Conjugate(incHy[j]);
                        Complex HinczCoj = Complex.Conjugate(incHz[j]);

                        Complex JincComplexx = Eincy * HinczCoj - Eincz * HincyCoj;
                        Complex JincComplexy = Eincz * HincxCoj - Eincx * HinczCoj;
                        Complex JincComplexz = Eincx * HincyCoj - Eincy * HincxCoj;

                        double kx = JincComplexx.Real;
                        double ky = JincComplexy.Real;
                        double kz = JincComplexz.Real;

                        double modulus = Math.Sqrt(kx * kx + ky * ky + kz * kz);
                        kx /= modulus;
                        ky /= modulus;
                        kz /= modulus;

                        double scalNK = kx * nx + ky * ny + kz * nz;                        // скалярное произведение вектора нормали n и вектора k;                        
                        //
                        // Определение thetaLoc
                        //    
                        if (Math.Abs(scalNK) >= 1)
                        {
                            thetaLoc = 0;
                        }
                        else if (Math.Abs(scalNK) >= 0.035)
                        {
                            thetaLoc = Math.Acos(scalNK);                // угол theta в локальной системе координат, в радианах      
                        }
                        else if (Math.Abs(scalNK) >= 0)
                        {
                            thetaLoc = cutoff_angle;
                        }

                        double VincRx, VincRy, VincRz;
                        if (thetaLoc > 0.001)
                        {
                            VincRx = ky * nz - kz * ny;
                            VincRy = kz * nx - kx * nz;
                            VincRz = kx * ny - ky * nx;
                            double modulus2 = Math.Sqrt(VincRx * VincRx + VincRy * VincRy + VincRz * VincRz);
                            VincRx /= modulus2;
                            VincRy /= modulus2;
                            VincRz /= modulus2;
                        }
                        else
                        {
                            if (Math.Abs(nx) < 0.98)
                            {
                                VincRx = 0;
                                VincRy = nz;
                                VincRz = (-1) * ny;
                                double modulus2 = Math.Sqrt(VincRy * VincRy + VincRz * VincRz);

                                VincRy /= modulus2;
                                VincRz /= modulus2;
                            }
                            else
                            {
                                VincRx = (-1) * nz;
                                VincRy = 0;
                                VincRz = nx;
                                double modulus2 = Math.Sqrt(VincRx * VincRx + VincRz * VincRz);
                                VincRx /= modulus2;

                                VincRz /= modulus2;
                            }
                        }
                        double VincLx = ky * VincRz - kz * VincRy;
                        double VincLy = kz * VincRx - kx * VincRz;
                        double VincLz = kx * VincRy - ky * VincRx;

                        double modulus3 = Math.Sqrt(VincLx * VincLx + VincLy * VincLy + VincLz * VincLz);
                        VincLx /= modulus3;
                        VincLy /= modulus3;
                        VincLz /= modulus3;

                        double kp1 = K_0 * Math.Sin(thetaLoc);   // kp1, kp2 - пространственные частоты             


                        double kappa2 = kp1 * kp1;
                        Stenka st = radUnion[stenkaIndexer[j]].Structure;
                        Tuple<Complex, Complex, Complex, Complex> reflect = TRCoeffitients.ReflectionCoefficientCalc2(st, kappa2, thetaLoc, Omega, K2, Y0e, Z0m);
                        Complex refE = reflect.Item1;
                        Complex refH = reflect.Item2;
                        Complex tE = reflect.Item3;
                        Complex tH = reflect.Item4;

                        Complex EincL = Eincx * VincLx + Eincy * VincLy + Eincz * VincLz;
                        Complex EincR = Eincx * VincRx + Eincy * VincRy + Eincz * VincRz;

                        Complex HincL = Hincx * VincLx + Hincy * VincLy + Hincz * VincLz;
                        Complex HincR = Hincx * VincRx + Hincy * VincRy + Hincz * VincRz;


                        double VrefRx = VincRx;
                        double VrefRy = VincRy;
                        double VrefRz = VincRz;

                        //double doubleVincLByNScal = Math.Abs(VincLx * nx + VincLy * ny + VincLz * nz);                        
                        double kref_x = kx - 2 * nx * Math.Cos(thetaLoc);
                        double kref_y = ky - 2 * ny * Math.Cos(thetaLoc);
                        double kref_z = kz - 2 * nz * Math.Cos(thetaLoc);

                        //double VrefLx = (2 * doubleVincLByNScal * nx) + VincLx;
                        //double VrefLy = (2 * doubleVincLByNScal * ny) + VincLy;
                        //double VrefLz = (2 * doubleVincLByNScal * nz) + VincLz;

                        double VrefLx = -(kref_y * VincRz - kref_z * VincRy);
                        double VrefLy = -(kref_z * VincRx - kref_x * VincRz);
                        double VrefLz = -(kref_x * VincRy - kref_y * VincRx);

                        double modulus4 = Math.Sqrt(VrefLx * VrefLx + VrefLy * VrefLy + VrefLz * VrefLz);
                        VrefLx /= modulus4;
                        VrefLy /= modulus4;
                        VrefLz /= modulus4;

                        Complex ErefLx = refE * EincL * VrefLx; //(2 * (SCVector.Scal(EincL, n) * n) - EincL)
                        Complex ErefLy = refE * EincL * VrefLy;
                        Complex ErefLz = refE * EincL * VrefLz;

                        Complex ErefRx = refH * EincR * VrefRx;
                        Complex ErefRy = refH * EincR * VrefRy;
                        Complex ErefRz = refH * EincR * VrefRz;

                        Complex HrefLx = refH * HincL * VrefLx;
                        Complex HrefLy = refH * HincL * VrefLy;
                        Complex HrefLz = refH * HincL * VrefLz;

                        Complex HrefRx = refE * HincR * VrefRx;
                        Complex HrefRy = refE * HincR * VrefRy;
                        Complex HrefRz = refE * HincR * VrefRz;


                        Complex Erefx = (-1) * (ErefLx + ErefRx);
                        Complex Erefy = (-1) * (ErefLy + ErefRy);
                        Complex Erefz = (-1) * (ErefLz + ErefRz);

                        Complex Hrefx = HrefLx + HrefRx;
                        Complex Hrefy = HrefLy + HrefRy;
                        Complex Hrefz = HrefLz + HrefRz;

                        reflectedNearField[j].E = new CVector(Erefx, Erefy, Erefz);// 
                        reflectedNearField[j].H = new CVector(Hrefx, Hrefy, Hrefz);//
                        reflectedNearField[j].Place = pointsCenter[j];
                    }
                }, g);
            }
            Task.WaitAll(tasks);
            return reflectedNearField;
        }

        public static NearFieldC InducedNearFieldCs(Current reflactedCurrents, Radome radUnion, Direction inside)
        {
            double pi = CV.pi;
            Complex imOne = new Complex(0, 1);

            double Omega = CV.Omega;
            double K_0 = CV.K_0;
            double K2 = CV.K2;
            Complex iOmega = CV.iOmega;
            Complex Ekoeff = CV.Ekoeff;

            Complex Z0m = CV.Z0m;
            Complex Y0e = CV.Y0e;
            double magCoeff = 1.07d;
            double cutoff_angle = CV.cutoff_angle;

            int radomeUnionSize = radUnion.CountElements;
            double[] Nx = new double[radomeUnionSize];
            double[] Ny = new double[radomeUnionSize];
            double[] Nz = new double[radomeUnionSize];

            Complex[] Ix = new Complex[radomeUnionSize];
            Complex[] Iy = new Complex[radomeUnionSize];
            Complex[] Iz = new Complex[radomeUnionSize];

            Complex[] Mx = new Complex[radomeUnionSize];
            Complex[] My = new Complex[radomeUnionSize];
            Complex[] Mz = new Complex[radomeUnionSize];

            double[] Area = new double[radomeUnionSize];

            double[] pXO = new double[radomeUnionSize];
            double[] pYO = new double[radomeUnionSize];
            double[] pZO = new double[radomeUnionSize];

            double[] pXC = new double[radomeUnionSize];
            double[] pYC = new double[radomeUnionSize];
            double[] pZC = new double[radomeUnionSize];

            double[] pX1 = new double[radomeUnionSize];
            double[] pY1 = new double[radomeUnionSize];
            double[] pZ1 = new double[radomeUnionSize];

            double[] pX2 = new double[radomeUnionSize];
            double[] pY2 = new double[radomeUnionSize];
            double[] pZ2 = new double[radomeUnionSize];

            double[] pX3 = new double[radomeUnionSize];
            double[] pY3 = new double[radomeUnionSize];
            double[] pZ3 = new double[radomeUnionSize];

            int h = 0;
            for (int w = 0; w < radUnion.Count; w++)
            {
                RadomeElement rad = radUnion[w];
                for (int i = 0; i < rad.Count; i++)
                {
                    Nx[h] = (-1) * rad[i].Norma.X;
                    Ny[h] = (-1) * rad[i].Norma.Y;
                    Nz[h] = (-1) * rad[i].Norma.Z;

                    Ix[h] = reflactedCurrents[h].I.X;
                    Iy[h] = reflactedCurrents[h].I.Y;
                    Iz[h] = reflactedCurrents[h].I.Z;

                    Mx[h] = reflactedCurrents[h].M.X;
                    My[h] = reflactedCurrents[h].M.Y;
                    Mz[h] = reflactedCurrents[h].M.Z;

                    Area[h] = rad[i].Area;

                    pXC[h] = rad[i].Center.X;
                    pYC[h] = rad[i].Center.Y;
                    pZC[h] = rad[i].Center.Z;

                    pXO[h] = magCoeff * rad[i].Center.X;
                    pYO[h] = magCoeff * rad[i].Center.Y;
                    pZO[h] = magCoeff * rad[i].Center.Z;

                    pX1[h] = rad[i].Triangle.V1.X;
                    pY1[h] = rad[i].Triangle.V1.Y;
                    pZ1[h] = rad[i].Triangle.V1.Z;

                    pX2[h] = rad[i].Triangle.V2.X;
                    pY2[h] = rad[i].Triangle.V2.Y;
                    pZ2[h] = rad[i].Triangle.V2.Z;

                    pX3[h] = rad[i].Triangle.V3.X;
                    pY3[h] = rad[i].Triangle.V3.Y;
                    pZ3[h] = rad[i].Triangle.V3.Z;
                    h++;
                }
            }

            NearFieldC reflectedNearField = new NearFieldC(radomeUnionSize);

            ///

            for (int j = 0; j < radomeUnionSize; j++)
            {
                Complex ex = new Complex(0, 0);
                Complex ey = new Complex(0, 0);
                Complex ez = new Complex(0, 0);

                Complex hx = new Complex(0, 0);
                Complex hy = new Complex(0, 0);
                Complex hz = new Complex(0, 0);

                

                for (int i = 0; i < radomeUnionSize; i++)
                {
                    double dS = Area[i];

                    Complex ix = Ix[i];
                    Complex iy = Ix[i];
                    Complex iz = Ix[i];

                    Complex mx = Mx[i];
                    Complex my = My[i];
                    Complex mz = Mz[i];


                    double x_x0 = pXC[i] - pXO[j];
                    double y_y0 = pYC[i] - pYO[j];
                    double z_z0 = pZC[i] - pZO[j];
                    double x2 = x_x0 * x_x0;
                    double y2 = y_y0 * y_y0;
                    double z2 = z_z0 * z_z0;

                    double r = Math.Sqrt(x2 + y2 + z2);

                    if (r < 0.09f)//r < 0.03f
                    {
                        double xq, yq, zq;
                        //
                        //   Расчет при условии близкого расположения точки интергрирования и точки наблюдения по правилу 4-х точечной квадратуры
                        //
                        //Точка 1
                        float a = 0.33333333f;      //
                        float b = 0.33333333f;      // координаты
                        float g = 0.33333333f;      //
                        float w = -0.56250000f;     // вес

                        xq = g * pX1[i] + a * pX2[i] + b * pX3[i];
                        yq = g * pY1[i] + a * pY2[i] + b * pY3[i];
                        zq = g * pZ1[i] + a * pZ2[i] + b * pZ3[i];

                        x_x0 = xq - pXO[j];
                        y_y0 = yq - pYO[j];
                        z_z0 = zq - pZO[j];

                        x2 = x_x0 * x_x0;
                        y2 = y_y0 * y_y0;
                        z2 = z_z0 * z_z0;

                        r = Math.Sqrt(x2 + y2 + z2);

                        Tuple<Complex, Complex, Complex, Complex, Complex, Complex> tuple = ElementFieldCalcR(ix, iy, iz, mx, my, mz, x_x0, y_y0, z_z0, x2, y2, z2, r, dS);

                        ex += w * tuple.Item1;
                        ey += w * tuple.Item2;
                        ez += w * tuple.Item3;

                        hx += w * tuple.Item4;
                        hy += w * tuple.Item5;
                        hz += w * tuple.Item6;

                        //Точка 2
                        a = 0.60000000f;        //
                        b = 0.20000000f;        // координаты
                        g = 0.20000000f;        //
                        w = 0.52083333f;       // вес

                        xq = g * pX1[i] + a * pX2[i] + b * pX3[i];
                        yq = g * pY1[i] + a * pY2[i] + b * pY3[i];
                        zq = g * pZ1[i] + a * pZ2[i] + b * pZ3[i];

                        x_x0 = xq - pXO[j];
                        y_y0 = yq - pYO[j];
                        z_z0 = zq - pZO[j];

                        x2 = x_x0 * x_x0;
                        y2 = y_y0 * y_y0;
                        z2 = z_z0 * z_z0;

                        r = Math.Sqrt(x2 + y2 + z2);

                        tuple = ElementFieldCalcR(ix, iy, iz, mx, my, mz, x_x0, y_y0, z_z0, x2, y2, z2, r, dS);

                        ex += w * tuple.Item1;
                        ey += w * tuple.Item2;
                        ez += w * tuple.Item3;

                        hx += w * tuple.Item4;
                        hy += w * tuple.Item5;
                        hz += w * tuple.Item6;

                        //Точка 3
                        a = 0.20000000f;        //
                        b = 0.60000000f;        // координаты
                        g = 0.20000000f;        //
                        w = 0.52083333f;       // вес

                        xq = g * pX1[i] + a * pX2[i] + b * pX3[i];
                        yq = g * pY1[i] + a * pY2[i] + b * pY3[i];
                        zq = g * pZ1[i] + a * pZ2[i] + b * pZ3[i];

                        x_x0 = xq - pXO[j];
                        y_y0 = yq - pYO[j];
                        z_z0 = zq - pZO[j];

                        x2 = x_x0 * x_x0;
                        y2 = y_y0 * y_y0;
                        z2 = z_z0 * z_z0;

                        r = Math.Sqrt(x2 + y2 + z2);

                        tuple = ElementFieldCalcR(ix, iy, iz, mx, my, mz, x_x0, y_y0, z_z0, x2, y2, z2, r, dS);

                        ex += w * tuple.Item1;
                        ey += w * tuple.Item2;
                        ez += w * tuple.Item3;

                        hx += w * tuple.Item4;
                        hy += w * tuple.Item5;
                        hz += w * tuple.Item6;


                        //Точка 4
                        a = 0.20000000f;        //
                        b = 0.20000000f;        // координаты
                        g = 0.60000000f;        //
                        w = 0.52083333f;        // вес

                        xq = g * pX1[i] + a * pX2[i] + b * pX3[i];
                        yq = g * pY1[i] + a * pY2[i] + b * pY3[i];
                        zq = g * pZ1[i] + a * pZ2[i] + b * pZ3[i];

                        x_x0 = xq - pXO[j];
                        y_y0 = yq - pYO[j];
                        z_z0 = zq - pZO[j];

                        x2 = x_x0 * x_x0;
                        y2 = y_y0 * y_y0;
                        z2 = z_z0 * z_z0;

                        r = Math.Sqrt(x2 + y2 + z2);

                        tuple = ElementFieldCalcR(ix, iy, iz, mx, my, mz, x_x0, y_y0, z_z0, x2, y2, z2, r, dS);

                        ex += w * tuple.Item1;
                        ey += w * tuple.Item2;
                        ez += w * tuple.Item3;

                        hx += w * tuple.Item4;
                        hy += w * tuple.Item5;
                        hz += w * tuple.Item6;
                    }
                    else
                    {
                        Tuple<Complex, Complex, Complex, Complex, Complex, Complex> tuple = ElementFieldCalcR(ix, iy, iz, mx, my, mz, x_x0, y_y0, z_z0, x2, y2, z2, r, dS);

                        ex += tuple.Item1; //-
                        ey += tuple.Item2; //-
                        ez += tuple.Item3; //-

                        hx += tuple.Item4; //-
                        hy += tuple.Item5; //-
                        hz += tuple.Item6; //-
                    }

                }
                reflectedNearField[j].E = new CVector(ex, ey, ez);
                reflectedNearField[j].H = new CVector(hx, hy, hz);
                reflectedNearField[j].Place = new Point3D(pXC[j], pYC[j], pZC[j]);
            }
            ///
            return reflectedNearField;
        }

        public static NearFieldC InducedNearFieldCpp(Current reflactedCurrents, Radome radUnion, double freq, Direction inside, int proc)
        {
            double pi = CV.pi;
            Complex imOne = new Complex(0, 1);

            double Omega = CV.Omega;
            double K_0 = CV.K_0;
            double K2 = CV.K2;
            Complex iOmega = CV.iOmega;
            Complex Ekoeff = CV.Ekoeff;

            Complex Z0m = CV.Z0m;
            Complex Y0e = CV.Y0e;
            double magCoeff = 1.07d;
            double cutoff_angle = CV.cutoff_angle;

            int radomeUnionSize = radUnion.CountElements;
            double[] Nx = new double[radomeUnionSize];
            double[] Ny = new double[radomeUnionSize];
            double[] Nz = new double[radomeUnionSize];

            Complex[] Ix = new Complex[radomeUnionSize];
            Complex[] Iy = new Complex[radomeUnionSize];
            Complex[] Iz = new Complex[radomeUnionSize];

            Complex[] Mx = new Complex[radomeUnionSize];
            Complex[] My = new Complex[radomeUnionSize];
            Complex[] Mz = new Complex[radomeUnionSize];

            double[] Area = new double[radomeUnionSize];

            double[] pXO = new double[radomeUnionSize];
            double[] pYO = new double[radomeUnionSize];
            double[] pZO = new double[radomeUnionSize];

            double[] pXC = new double[radomeUnionSize];
            double[] pYC = new double[radomeUnionSize];
            double[] pZC = new double[radomeUnionSize];

            double[] pX1 = new double[radomeUnionSize];
            double[] pY1 = new double[radomeUnionSize];
            double[] pZ1 = new double[radomeUnionSize];

            double[] pX2 = new double[radomeUnionSize];
            double[] pY2 = new double[radomeUnionSize];
            double[] pZ2 = new double[radomeUnionSize];

            double[] pX3 = new double[radomeUnionSize];
            double[] pY3 = new double[radomeUnionSize];
            double[] pZ3 = new double[radomeUnionSize];

            int h = 0;
            for (int w = 0; w < radUnion.Count; w++)
            {
                RadomeElement rad = radUnion[w];
                for (int i = 0; i < rad.Count; i++)
                {
                    Nx[h] = (-1) * rad[i].Norma.X;
                    Ny[h] = (-1) * rad[i].Norma.Y;
                    Nz[h] = (-1) * rad[i].Norma.Z;

                    Ix[h] = reflactedCurrents[h].I.X;
                    Iy[h] = reflactedCurrents[h].I.Y;
                    Iz[h] = reflactedCurrents[h].I.Z;

                    Mx[h] = reflactedCurrents[h].M.X;
                    My[h] = reflactedCurrents[h].M.Y;
                    Mz[h] = reflactedCurrents[h].M.Z;

                    Area[h] = rad[i].Area;

                    pXC[h] = rad[i].Center.X;
                    pYC[h] = rad[i].Center.Y;
                    pZC[h] = rad[i].Center.Z;

                    pXO[h] = magCoeff * rad[i].Center.X;
                    pYO[h] = magCoeff * rad[i].Center.Y;
                    pZO[h] = magCoeff * rad[i].Center.Z;

                    pX1[h] = rad[i].Triangle.V1.X;
                    pY1[h] = rad[i].Triangle.V1.Y;
                    pZ1[h] = rad[i].Triangle.V1.Z;

                    pX2[h] = rad[i].Triangle.V2.X;
                    pY2[h] = rad[i].Triangle.V2.Y;
                    pZ2[h] = rad[i].Triangle.V2.Z;

                    pX3[h] = rad[i].Triangle.V3.X;
                    pY3[h] = rad[i].Triangle.V3.Y;
                    pZ3[h] = rad[i].Triangle.V3.Z;
                    h++;
                }
            }

            NearFieldC reflectedNearField = new NearFieldC(radomeUnionSize);

            ///

            CComplex* sol = InducedNF(Ix, Iy, Iz, Mx, My, Mz, Nx, Ny, Nz, Area, radomeUnionSize, pXO, pYO, pZO, pXC, pYC, pZC, pX1, pY1, pZ1, pX2, pY2, pZ2, pX3, pY3, pZ3, freq, ref proc);

            for (int i = 0; i < radomeUnionSize; i++)
            {
                reflectedNearField[i].E = new CVector(new Complex(sol[i * 6 + 0].re, sol[i * 6 + 0].im), new Complex(sol[i * 6 + 1].re, sol[i * 6 + 1].im), new Complex(sol[i * 6 + 2].re, sol[i * 6 + 2].im));
                reflectedNearField[i].H = new CVector(new Complex(sol[i * 6 + 3].re, sol[i * 6 + 3].im), new Complex(sol[i * 6 + 4].re, sol[i * 6 + 4].im), new Complex(sol[i * 6 + 5].re, sol[i * 6 + 5].im));
                reflectedNearField[i].Place = new Point3D(pXC[i], pYC[i], pZC[i]);
            }

            return reflectedNearField;
        }

        private static Tuple<Complex, Complex, Complex, Complex, Complex, Complex> ElementFieldCalcR(Complex ix, Complex iy, Complex iz, Complex mx, Complex my, Complex mz, double x_x0, double y_y0, double z_z0, double x2, double y2, double z2, double r, double dS)
        {
            double K_0 = CV.K_0;
            double K2 = CV.K2;
            Complex Ekoeff = CV.Ekoeff;
            Complex Mukoeff = CV.Mukoeff;

            Complex imOneMin = new Complex(0, -1);
            Complex imOne = new Complex(0, 1);

            Complex exp_ikr = new Complex(Math.Cos(K_0 * r), -Math.Sin(K_0 * r));

            Complex funG = exp_ikr / (4 * CV.pi * r);
            double r2 = r * r;
            double r4 = r2 * r2;

            //
            //НАЧАЛО КОСТИНЫ ФОРМУЛЫ
            //
            Complex coeffA = imOneMin * K_0 / r - 1.0 / r2;
            Complex coeffB = (3.0 + 3.0 * imOne * K_0 * r - K2 * r2) / r4;

            Complex dx = x_x0 * coeffA * funG;
            Complex dy = y_y0 * coeffA * funG;
            Complex dz = z_z0 * coeffA * funG;

            Complex dxx = (x2 * coeffB + coeffA) * funG;
            Complex dyy = (y2 * coeffB + coeffA) * funG;
            Complex dzz = (z2 * coeffB + coeffA) * funG;

            Complex dxy = (x_x0 * y_y0 * coeffB) * funG;
            Complex dxz = (x_x0 * z_z0 * coeffB) * funG;
            Complex dyz = (y_y0 * z_z0 * coeffB) * funG;
            //
            // КОНЕЦ
            //

            Complex ex = (-1) * (Ekoeff * (K2 * ix * funG + ix * dxx + iy * dxy + iz * dxz) * dS - (mz * dy - my * dz) * dS); //-
            Complex ey = (-1) * (Ekoeff * (K2 * iy * funG + iy * dyy + iz * dyz + ix * dxy) * dS - (mx * dz - mz * dx) * dS); //-
            Complex ez = (-1) * (Ekoeff * (K2 * iz * funG + iz * dzz + ix * dxz + iy * dyz) * dS - (my * dx - mx * dy) * dS); //-

            Complex hx = (-1) * (Mukoeff * (K2 * mx * funG + mx * dxx + my * dxy + mz * dxz) * dS + (iz * dy - iy * dz) * dS); //+
            Complex hy = (-1) * (Mukoeff * (K2 * my * funG + mx * dxy + my * dyy + mz * dyz) * dS + (ix * dz - iz * dx) * dS); //+
            Complex hz = (-1) * (Mukoeff * (K2 * mz * funG + mx * dxz + my * dyz + mz * dzz) * dS + (iy * dx - ix * dy) * dS); //+

            return new Tuple<Complex, Complex, Complex, Complex, Complex, Complex>(ex, ey, ez, hx, hy, hz);
        }

        public static NearFieldC GetExitationField(Current current, Radome radUnion, Direction direction)
        {
            double nx, ny, nz;
            Complex ix, iy, iz, mx, my, mz;

            Complex ex;
            Complex ey;
            Complex ez;

            Complex hx;
            Complex hy;
            Complex hz;

            int generalTrianglesNumber = radUnion.CountElements;
            NearFieldC outField = new NearFieldC(generalTrianglesNumber);


            for (int i = 0; i < radUnion.Count; i++)
            {
                RadomeElement geometry = radUnion[i];

                for (int j = 0; j < generalTrianglesNumber; j++)
                {
                    //
                    // Выбор нормали
                    //  
                    //Triangle element = geometry.triangles[j];

                    double upDown = 1;

                    if (direction == Direction.Inside)
                    {
                        upDown = -1;
                    }
                    //
                    // Загрузка полей
                    //  
                    nx = upDown * geometry[j].Norma.X;
                    ny = upDown * geometry[j].Norma.Y;
                    nz = upDown * geometry[j].Norma.Z;


                    ix = current.I[j].X;
                    iy = current.I[j].Y;
                    iz = current.I[j].Z;

                    mx = current.M[j].X;
                    my = current.M[j].Y;
                    mz = current.M[j].Z;

                    //
                    // Расчет возбуждаемых токов
                    // 
                    ex = mz * ny - my * nz;        // x - компонента электрического тока на внутренней стороне укрытия
                    ey = mx * nz - mz * nx;        // y - компонента электрического тока на внутренней стороне укрытия
                    ez = my * nx - mx * ny;        // z - компонента электрического тока на внутренней стороне укрытия

                    hx = nz * iy - ny * iz;        // x - компонента магнитного тока на внутренней стороне укрытия
                    hy = nx * iz - nz * ix;        // y - компонента магнитного тока на внутренней стороне укрытия
                    hz = ny * ix - nx * iy;        // z - компонента магнитного тока на внутренней стороне укрытия 


                    outField[j].E = new CVector(ex, ey, ez);
                    outField[j].H = new CVector(hx, hy, hz);
                    outField[j].Place = geometry[j].Center;
                }
            }
            return outField;
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

    }

    struct CComplex
    {
        public double re;
        public double im;
    };
    public class NearFieldElement
    {
        public CVector E { get; set; }
        public CVector H { get; set; }

        public Point3D Place { get; set; }

        public NearFieldElement()
        {
            E = new CVector();
            H = new CVector();
            Place = new Point3D(0, 0, 0);
        }
        public NearFieldElement(CVector e, CVector h, Point3D loc)
        {
            E = e;
            H = h;
            Place = loc;
        }
    }
    public enum Direction
    {
        Inside,
        Outside
    }
}
