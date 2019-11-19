using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class SourceTemplate2
    {
        #region Static Fields

        public static SourceTemplate2[] Sources;

        public static string Lable { get; set; }

        public static CVector[] I;
        public static CVector[] M;

        public static CVector[] I0; //= new CVector[4] { new CVector(1, 0, 0), new CVector(1, 0, 0), new CVector(1, 0, 0), new CVector(1, 0, 0) };
        public static CVector[] M0; //= new CVector[4] { new CVector(1, 0, 0), new CVector(1, 0, 0), new CVector(1, 0, 0), new CVector(1, 0, 0) };

        public static Dictionary<string, bool> Polarization;// = new Dictionary<string, bool>() { { "Поляризация А", false }, { "Поляризация Б", false }, { "Круговая поляризация A", false }, { "Круговая поляризация Б", false }, { "Пользовательская", false } };
        public static string Distribution { get; set; }// = "Постоянное поле";
        public static bool DifferenceChanel { get; set; }// = false;
        public static string DifferenceAxis { get; set; }// = "Плоскость XZ";
        public static int Scanning { get; set; }// = 0;
        public static int SystemOfCoordinatesScan { get; set; }// = 0;
        public static double ThetaScanEStart { get; set; }// = 0;
        public static double ThetaScanEFinish { get; set; }// = 0;
        public static double ThetaScanEStep { get; set; }// = 1;
        public static double PhiScanEStart { get; set; } //= 0;
        public static double PhiScanEFinish { get; set; } //= 0;
        public static double PhiScanEStep { get; set; } //= 1;

        public static bool Axis1Include { get; set; } //= false;
        public static double Axis1x1 { get; set; } //= 0;
        public static double Axis1z1 { get; set; } //= 0;
        public static double Axis1y1 { get; set; } //= 0;

        public static double Axis1x2 { get; set; } //= 0;
        public static double Axis1y2 { get; set; } //= 0;
        public static double Axis1z2 { get; set; }// = 0;

        public static bool Axis2Include { get; set; } //= false;
        public static double Axis2x1 { get; set; }// = 0;
        public static double Axis2y1 { get; set; }// = 0;
        public static double Axis2z1 { get; set; }// = 0;

        public static double Axis2x2 { get; set; }// = 0;
        public static double Axis2y2 { get; set; } //= 0;
        public static double Axis2z2 { get; set; } //= 0;

        public static double PhiScanMStart { get; set; } //= 0;угол поворота в вокруг оси 2
        public static double PhiScanMFinish { get; set; } //= 0;
        public static double PhiScanMStep { get; set; } //= 0;

        public static double ThetaScanMStart { get; set; }// = 0;угол поворота вокруг оси 1
        public static double ThetaScanMFinish { get; set; } //= 0;
        public static double ThetaScanMStep { get; set; } //= 0;


        #endregion


        #region Dynamic Fields
        public string name { get; set; }

        public string polarization { get; set; }
        public string distribution { get; set; }
        public bool differenceChanel { get; set; }
        public string differenceAxis { get; set; }

        public int scanning { get; set; }
        public double thetaScanE { get; set; }
        public double phiScanE { get; set; }

        public bool axis1Include { get; set; }
        public double axis1x1 { get; set; }
        public double axis1z1 { get; set; }
        public double axis1y1 { get; set; }
        public double axis1x2 { get; set; }
        public double axis1y2 { get; set; }
        public double axis1z2 { get; set; }

        public bool axis2Include { get; set; }
        public double axis2x1 { get; set; }
        public double axis2y1 { get; set; }
        public double axis2z1 { get; set; }
        public double axis2x2 { get; set; }
        public double axis2y2 { get; set; }
        public double axis2z2 { get; set; }

        public double phiScanM { get; set; }
        public double thetaScanM { get; set; }        

        public CVector i;
        public CVector m;
        #endregion

        public SourceTemplate2(string _Name, string _polarization, string _distribution, bool _differenceChanel, string _differenceAxis, 
            int _scanning, 
            double _thetaScanE, double _phiScanE, 
            bool _axis1Include, double _axis1x1, double _axis1y1, double _axis1z1, double _axis1x2, double _axis1y2, double _axis1z2, 
            bool _axis2Include, double _axis2x1, double _axis2y1, double _axis2z1, double _axis2x2, double _axis2y2, double _axis2z2,
            double _thetaScanM, double _phiScanM,
            CVector i0, CVector m0)
        {
            name = _Name;
            polarization = _polarization;
            distribution = _distribution;
            differenceChanel = _differenceChanel;
            differenceAxis = _differenceAxis;
            scanning = _scanning;
            thetaScanE = _thetaScanE;
            phiScanE = _phiScanE;
            axis1Include = _axis1Include;
            axis1x1 = _axis1x1;            
            axis1y1 = _axis1y1;
            axis1z1 = _axis1z1;
            axis1x2 = _axis1x2;
            axis1y2 = _axis1y2;
            axis1z2 = _axis1z2;
            axis2Include = _axis2Include;
            axis2x1 = _axis2x1;
            axis2y1 = _axis2y1;
            axis2z1 = _axis2z1;
            axis2x2 = _axis2x2;
            axis2y2 = _axis2y2;
            axis2z2 = _axis2z2;
            phiScanM = _phiScanM;
            thetaScanM = _thetaScanM;
            i = i0;
            m = m0;
        }

        
        public static void SetStaticSourceParameters(string lable, CVector[] i, CVector[] m, Dictionary<string, bool> polariztion, string distribution, bool differenceChanel, string differenceAxis,
            int scanning, int systemOfCoordinatesScan, double thetaScanStart, double thetaScanFinish, double thetaScanStep, double phiScanStart, double phiScanFinish, double phiScanStep,
            bool _axis1include, double _axis1x1, double _axis1y1, double _axis1z1, double _axis1x2, double _axis1y2, double _axis1z2,
            bool _axis2include, double _axis2x1, double _axis2y1, double _axis2z1, double _axis2x2, double _axis2y2, double _axis2z2,
            double _scanMPhiStart, double _scanMPhiFinish, double _scanMPhiStep, double _scanMThetaStart, double _scanMThetaFinish, double _scanMThetaStep)
        {
            Lable = lable;
            I = i;
            M = m;            
            Polarization = polariztion;
            Distribution = distribution;
            DifferenceChanel = differenceChanel;
            DifferenceAxis = differenceAxis;
            Scanning = scanning;
            SystemOfCoordinatesScan = systemOfCoordinatesScan;
            ThetaScanEStart = thetaScanStart;
            ThetaScanEFinish = thetaScanFinish;
            ThetaScanEStep = thetaScanStep;

            PhiScanEStart = phiScanStart;
            PhiScanEFinish = phiScanFinish;
            PhiScanEStep = phiScanStep;

            Axis1Include = _axis1include;
            Axis1x1 = _axis1x1;
            Axis1y1 = _axis1y1;
            Axis1z1 = _axis1z1;
            Axis1x2 = _axis1x2;
            Axis1y2 = _axis1y2;
            Axis1z2 = _axis1z2;

            Axis2Include = _axis2include;
            Axis2x1 = _axis2x1;
            Axis2y1 = _axis2y1;
            Axis2z1 = _axis2z1;
            Axis2x2 = _axis2x2;
            Axis2y2 = _axis2y2;
            Axis2z2 = _axis2z2;

            PhiScanMStart = _scanMPhiStart;
            PhiScanMFinish = _scanMPhiFinish;
            PhiScanMStep = _scanMPhiStep;
            ThetaScanMStart = _scanMThetaStart;
            ThetaScanMFinish = _scanMThetaFinish;
            ThetaScanMStep = _scanMThetaStep;            
        }


        public static void SetStaticSourceParameters(CreateApertureForm form)
        {
            string distribution = form.comboBoxDistribution.SelectedItem.ToString();
            string lable = form.textBoxApertureTitle.Text;

            CVector[] picur = new CVector[DictionaryLibrary.PolarizationNames.Count];
            CVector[] pmcur = new CVector[DictionaryLibrary.PolarizationNames.Count];

            //foreach (var pol in DictionaryLibrary.PolarizationNames)
            //{
            //    string name = pol.Value;
            //    if (form.comboBoxDistribution.SelectedIndex == 0 || form.comboBoxDistribution.SelectedIndex == 1)    //постоянное поле или спадающий косинус
            //    {

            //    }
            //    else if (form.comboBoxDistribution.SelectedIndex == 2)   //загрузка из файла
            //    {

            //    }
            //} 

            //Polarization = form.comboBoxPolarization.SelectedIndex;

            bool difference = true;
            if (form.radioButtonChannel1.Checked)
            {
                difference = false;
            }

            Dictionary<string, bool> polariz = new Dictionary<string, bool>();   //new bool[form.checkedListBoxPolarization.Items.Count];
            for (int i = 0; i < form.checkedListBoxPolarization.Items.Count; i++)
            {
                polariz.Add(form.checkedListBoxPolarization.Items[i].ToString(), form.checkedListBoxPolarization.GetItemChecked(i));                
            }

            string axis = form.comboBoxDiffAxis.SelectedItem.ToString();
            int scan = form.comboBoxScanning.SelectedIndex;
            int systemOfCoordScan = form.comboBoxSysOfCoordScan.SelectedIndex;
            double scanThetaStart = Convert.ToDouble(form.textBoxScanThetaStart.Text);
            double scanThetaFinish = Convert.ToDouble(form.textBoxScanThetaFinish.Text);
            double scanThetaStep = Convert.ToDouble(form.textBoxScanThetaStep.Text);

            double phiScanEStart = Convert.ToDouble(form.textBoxScanPhiStart.Text);
            double phiScanEFinish = Convert.ToDouble(form.textBoxScanPhiFinish.Text);
            double phiScanEStep = Convert.ToDouble(form.textBoxScanPhiStep.Text);

            bool includeAxis1 = false;
            if (form.comboBoxAxis1.SelectedIndex == 1)
            {
                includeAxis1 = true;
            }            

            double axis1x1 = Convert.ToDouble(form.textBoxRotAxis1X1.Text);
            double axis1y1 = Convert.ToDouble(form.textBoxRotAxis1Y1.Text);
            double axis1z1 = Convert.ToDouble(form.textBoxRotAxis1Z1.Text);

            bool includeAxis2 = false;
            if (form.comboBoxAxis2.SelectedIndex == 1)
            {
                includeAxis2 = true;
            }
            double axis1x2 = Convert.ToDouble(form.textBoxRotAxis1X2.Text);
            double axis1y2 = Convert.ToDouble(form.textBoxRotAxis1Y2.Text);
            double axis1z2 = Convert.ToDouble(form.textBoxRotAxis1Z2.Text);

            double axis2x1 = Convert.ToDouble(form.textBoxRotAxis2X1.Text);
            double axis2y1 = Convert.ToDouble(form.textBoxRotAxis2Y1.Text);
            double axis2z1 = Convert.ToDouble(form.textBoxRotAxis2Z1.Text);

            double axis2x2 = Convert.ToDouble(form.textBoxRotAxis2X2.Text);
            double axis2y2 = Convert.ToDouble(form.textBoxRotAxis2Y2.Text);
            double axis2z2 = Convert.ToDouble(form.textBoxRotAxis2Z2.Text);

            double phiScanMStart = Convert.ToDouble(form.textBoxMAngle2Start.Text);
            double phiScanMFinish = Convert.ToDouble(form.textBoxMAngle2Finish.Text);
            double phiScanMStep = Convert.ToDouble(form.textBoxMAngle2Step.Text);

            double thetaScanMStart = Convert.ToDouble(form.textBoxMAngle1Start.Text);
            double thetaScanMFinish = Convert.ToDouble(form.textBoxMAngle1Finish.Text);
            double thetaScanMStep = Convert.ToDouble(form.textBoxMAngle1Step.Text);


            SetStaticSourceParameters(lable, form.I, form.M, polariz, distribution, difference, axis, scan, systemOfCoordScan, scanThetaStart, scanThetaFinish, scanThetaStep, phiScanEStart, phiScanEFinish, phiScanEStep, includeAxis1, axis1x1, axis1y1, axis1z1, axis1x2, axis1y2, axis1z2, includeAxis2, axis2x1, axis2y1, axis2z1, axis2x2, axis2y2, axis2z2, phiScanMStart, phiScanMFinish, phiScanMStep, thetaScanMStart, thetaScanMFinish, thetaScanMStep);
        }

        public static void CreateSources()
        {
            int scancount = 1;

            if (Scanning == 1)
            {
                scancount = 0;
                int pointsThetaCount = Convert.ToInt32((ThetaScanEFinish - ThetaScanEStart) / ThetaScanEStep) + 1;
                int pointsPhiCount = Convert.ToInt32((PhiScanEFinish - PhiScanEStart) / PhiScanEStep) + 1;

                for (int iTheta = 0; iTheta < pointsThetaCount; iTheta++)
                {
                    for (int iPhi = 0; iPhi < pointsPhiCount; iPhi++)
                    {
                        scancount++;
                    }
                }
            }
            else if (Scanning == 2)
            {
                scancount = 0;
                int pointsThetaCount = 1;
                int pointsPhiCount = 1;
                if (Axis1Include)
                {
                    pointsThetaCount = Convert.ToInt32((ThetaScanMFinish - ThetaScanMStart) / ThetaScanMStep) + 1;
                }
                if (Axis2Include)
                {
                    pointsPhiCount = Convert.ToInt32((PhiScanMFinish - PhiScanMStart) / PhiScanMStep) + 1;
                }


                for (int iTheta = 0; iTheta < pointsThetaCount; iTheta++)
                {
                    for (int iPhi = 0; iPhi < pointsPhiCount; iPhi++)
                    {
                        scancount++;
                    }
                }
            }

            int polcount = 0;


            foreach (var item in Polarization)
            {
                if (item.Value)
                {
                    polcount++;
                }
            }



            int countSorces = scancount * polcount;

            Sources = new SourceTemplate2[countSorces];
            int indexSources = 0;
            if (Scanning == 0)
            {
                foreach (var pol in Polarization)
                {
                    if (pol.Value)
                    {
                        int key = DictionaryLibrary.PolarizationNames.FirstOrDefault(x => x.Value == pol.Key).Key;
                        string sourceName = String.Concat(Lable, "_", DictionaryLibrary.PolarizationNamesShort[key]);
                        Sources[indexSources] = new SourceTemplate2(sourceName, pol.Key, Distribution, DifferenceChanel, DifferenceAxis, Scanning, 0, 0, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, 0, 0, I0[key], M0[key]);
                        indexSources++;
                    }
                }
            }
            else if (Scanning == 1)
            {
                int pointsThetaCount = Convert.ToInt32((ThetaScanEFinish - ThetaScanEStart) / ThetaScanEStep) + 1;
                int pointsPhiCount = Convert.ToInt32((PhiScanEFinish - PhiScanEStart) / PhiScanEStep) + 1;

                int index = 0;

                foreach (var pol in Polarization)
                {
                    if (pol.Value)
                    {
                        int key = DictionaryLibrary.PolarizationNames.FirstOrDefault(x => x.Value == pol.Key).Key;
                        for (int iTheta = 0; iTheta < pointsThetaCount; iTheta++)
                        {
                            for (int iPhi = 0; iPhi < pointsPhiCount; iPhi++)
                            {
                                double scanThetaLocal = ThetaScanEStart + iTheta * ThetaScanEStep;
                                double scanPhiLocal = PhiScanEStart + iPhi * PhiScanEStep;

                                string sourceName = String.Concat(Lable, "_E", "_T", scanThetaLocal.ToString("0.##"), "_P", scanPhiLocal.ToString("0.##"), "_", DictionaryLibrary.PolarizationNamesShort[key]);
                                double scanTheta = Logic.GetThetaGlobal(scanPhiLocal, scanThetaLocal, SystemOfCoordinatesScan);
                                double scanPhi = Logic.GetPhiGlobal(scanPhiLocal, scanThetaLocal, SystemOfCoordinatesScan);

                                Sources[index] = new SourceTemplate2(sourceName, pol.Key, Distribution, DifferenceChanel, DifferenceAxis, Scanning, scanTheta, scanPhi, false, 0, 0, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, 0, 0, I0[key], M0[key]);
                                index++;
                            }
                        }
                    }
                }
            }
            else if (Scanning == 2)
            {
                //Механическое сканирование
                int pointsThetaCount = Convert.ToInt32((ThetaScanMFinish - ThetaScanMStart) / ThetaScanMStep) + 1;
                int pointsPhiCount = Convert.ToInt32((PhiScanMFinish - PhiScanMStart) / PhiScanMStep) + 1;
                if (!Axis1Include)
                {
                    pointsThetaCount = 1;
                    ThetaScanMStart = 0;
                }
                if (!Axis2Include)
                {
                    pointsPhiCount = 1;
                    PhiScanMStart = 0;
                }

                int index = 0;

                foreach (var pol in Polarization)
                {
                    if (pol.Value)
                    {
                        int key = DictionaryLibrary.PolarizationNames.FirstOrDefault(x => x.Value == pol.Key).Key;
                        for (int iTheta = 0; iTheta < pointsThetaCount; iTheta++)
                        {
                            for (int iPhi = 0; iPhi < pointsPhiCount; iPhi++)
                            {
                                double scanThetaLocal = ThetaScanMStart + iTheta * ThetaScanMStep;
                                double scanPhiLocal = PhiScanMStart + iPhi * PhiScanMStep;

                                string sourceName = String.Concat(Lable, "_M", "_T", scanThetaLocal.ToString("0.##"), "_P", scanPhiLocal.ToString("0.##"), "_", DictionaryLibrary.PolarizationNamesShort[key]);

                                Sources[index] = new SourceTemplate2(sourceName, pol.Key, Distribution, DifferenceChanel, DifferenceAxis, Scanning, 0, 0, Axis1Include, Axis1x1, Axis1y1, Axis1z1, Axis1x2, Axis1y2, Axis1z2, Axis2Include, Axis2x1, Axis2y1, Axis2z1, Axis2x2, Axis2y2, Axis2z2, scanThetaLocal, scanPhiLocal, I0[key], M0[key]);
                                index++;
                            }
                        }
                    }
                }
            }
        }

        public static Tuple<CVector, CVector> GetPolarizationCurrents(string changedPolarizatioin, string distribution)
        {
            //polarization detection
            //var d = DictionaryLibrary.PolarizationNames;
            //int polarization = d.FirstOrDefault(x => x.Value == changedPolarizatioin).Key;

            //set currents
            CVector electricCurrent = null;
            CVector magneticCurrent = null;

            if (distribution == "Постоянное поле"|| distribution == "Косинус на пьедестале")
            {
                //ortogonal and parallel vectors detection
                double xlength = Logic.Instance.Antenna.XMax - Logic.Instance.Antenna.XMin;
                double ylength = Logic.Instance.Antenna.YMax - Logic.Instance.Antenna.YMin;
                double zlength = Logic.Instance.Antenna.ZMax - Logic.Instance.Antenna.ZMin;

                DVector probeVector;
                if (xlength > ylength)
                {
                    if (xlength > zlength)
                    {
                        probeVector = new DVector(1, 0, 0);
                    }
                    else
                    {
                        probeVector = new DVector(0, 0, 1);
                    }
                }
                else
                {
                    if (ylength > zlength)
                    {
                        probeVector = new DVector(0, 1, 0);
                    }
                    else
                    {
                        probeVector = new DVector(0, 0, 1);
                    }
                }

                DVector normaVector = Logic.Instance.Antenna[0].Norma;
                DVector ortogonalVector = DVector.Cross(probeVector, normaVector);
                ortogonalVector.Normalize();
                DVector parallelVector = DVector.Cross(ortogonalVector, normaVector);
                parallelVector.Normalize();




                if (changedPolarizatioin == "Поляризация А")
                {
                    electricCurrent = new CVector(new Complex(parallelVector.X, 0), new Complex(parallelVector.Y, 0), new Complex(parallelVector.Z, 0));
                    magneticCurrent = CV.Z_0 * new CVector(new Complex(ortogonalVector.X, 0), new Complex(ortogonalVector.Y, 0), new Complex(ortogonalVector.Z, 0));
                }
                else if (changedPolarizatioin == "Поляризация Б")
                {
                    electricCurrent = new CVector(new Complex(-ortogonalVector.X, 0), new Complex(-ortogonalVector.Y, 0), new Complex(-ortogonalVector.Z, 0));
                    magneticCurrent = CV.Z_0 * new CVector(new Complex(parallelVector.X, 0), new Complex(parallelVector.Y, 0), new Complex(parallelVector.Z, 0));
                }
                else if (changedPolarizatioin == "Круговая поляризация А")
                {
                    //!!!! запланировано, не работает
                    electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                    magneticCurrent = CV.Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                }
                else if (changedPolarizatioin == "Круговая поляризация Б")
                {
                    //!!!!запланировано, не работает
                    electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                    magneticCurrent = CV.Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                }
                else if (changedPolarizatioin == "Пользовательская")
                {
                    //!!!!запланировано, не работает
                    electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                    magneticCurrent = CV.Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
                }
            }
            else if (distribution == "Загрузить из файла")
            {
                int count = I.Length;
                double max = I[0].Modulus;
                int maxIndex = 0;
                for (int i = 1; i < count; i++)
                {
                    if (I[i].Modulus > max)
                    {
                        maxIndex = i;
                        max = I[i].Modulus;
                    }
                }


                if (changedPolarizatioin == "Поляризация А")
                {
                    electricCurrent = I[maxIndex];
                    magneticCurrent = M[maxIndex];
                }
                else if (changedPolarizatioin == "Поляризация Б")
                {
                    electricCurrent = I[maxIndex];
                    magneticCurrent = M[maxIndex];

                    electricCurrent = Current.ChangeLoadedCurrentAnlorithm(electricCurrent, Logic.Instance.Antenna[maxIndex].Norma, 90);
                    magneticCurrent = Current.ChangeLoadedCurrentAnlorithm(magneticCurrent, Logic.Instance.Antenna[maxIndex].Norma, 90);
                }
            }
            return new Tuple<CVector, CVector>(electricCurrent, magneticCurrent);
        }
    }
}
