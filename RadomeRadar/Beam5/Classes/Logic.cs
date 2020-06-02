using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Apparat
{
    public class Logic
    {
        #region Singleton Pattern
        private static Logic instance = null;
        public static Logic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logic();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private Logic() { }
        #endregion

        #region Global Variables
        const char Eps = '\u03B5';
        const char Mu = '\u03BC';
        const string Format1 = "0.#####";
        public static string ProgramName = "RadomeRadar v.1 Beta";

        readonly float meshSizeParam = 7.2f;       //meshLength * K_0 < meshSizeParam
        public string ProjectAdress;
        public Solution solutions;
        public List<FarFieldRequestTemplate> Requests = new List<FarFieldRequestTemplate>();
        //public SourceTemplate Sources;
        public List<Stenka> RadomeLayers = new List<Stenka>();
        public Aperture Antenna;
        public NearFieldC RadomeNearField;
        public Radome RadomeComposition = new Radome();
        public string LoadFieldName = "";

        //private string sizeUnit = "";
        public bool SaveField = true;
        //public bool LoadField = false;
        public List<double> Frequencies = new List<double>(); //Частота(Гц)
        readonly int Proc = Environment.ProcessorCount;
        #endregion

        #region Static Methods
        public static double GetThetaGlobal(double phi, double theta, int system)
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
        public static double GetPhiGlobal(double phi, double theta, int system)
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

        public static double GetPhiLocal(double thetaGlobal, double phiGlobal, int newSystem)
        {
            double a = Math.Sin(thetaGlobal * Math.PI / 180) * Math.Cos(phiGlobal * Math.PI / 180);
            double b = Math.Sin(thetaGlobal * Math.PI / 180) * Math.Sin(phiGlobal * Math.PI / 180);
            double c = Math.Cos(thetaGlobal * Math.PI / 180);
            double phiLocal = 0;
            double x = 0, y = 0, z = 0;

            if (newSystem == 1)
            {
                x = c;
                y = a;
                z = b;
            }
            else if (newSystem == 2)
            {
                x = b;
                y = c;
                z = a;
            }

            double m = z / Math.Sqrt(x * x + y * y + z * z);
            if (m < -1)
            {
                m = -1;
            }
            else if (m > 1)
            {
                m = 1;
            }

            double thetaLocal = Math.Acos(m) / Math.PI * 180;

            if (thetaLocal == 0)
            {
                phiLocal = 0;
            }
            else
            {
                double v = x / Math.Sin(thetaLocal * Math.PI / 180);
                if (v > 1)
                {
                    v = 1;
                }
                else if (v < -1)
                {
                    v = -1;
                }

                phiLocal = Math.Acos(v) / Math.PI * 180;

                if (c < 0)
                {
                    phiLocal = 360 - phiLocal;
                }
            }

            return phiLocal;            
        }
        public static double GetThetaLocal(double thetaGlobal, double phiGlobal, int newSystem)
        {
            double a = Math.Sin(thetaGlobal * Math.PI / 180) * Math.Cos(phiGlobal * Math.PI / 180);
            double b = Math.Sin(thetaGlobal * Math.PI / 180) * Math.Sin(phiGlobal * Math.PI / 180);
            double c = Math.Cos(thetaGlobal * Math.PI / 180);            
            double x = 0, y = 0, z = 0;

            if (newSystem == 1)
            {
                x = c;
                y = a;
                z = b;
            }
            else if (newSystem == 2)
            {
                x = b;
                y = c;
                z = a;
            }

            double m = z / Math.Sqrt(x * x + y * y + z * z);
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
        public static string GetTime(Stopwatch stopwatch)
        {
            TimeSpan ts = stopwatch.Elapsed;
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
        }
        public static string NowTime
        {
            get
            {
                DateTime time = DateTime.Now;
                return String.Format("{0:00}:{1:00}:{2:00}", time.Hour, time.Minute, time.Second);
            }
        }
        #endregion



        public float RenderScale
        {
            get
            {
                if (RadomeComposition.Count > 0)
                {
                    return (float)RadomeComposition.DiagonalSize;
                }
                else if (Antenna != null)
                {
                    return (float)Antenna.DiagonalSize;
                }
                else
                {
                    return 1;
                }
            }
        }
        private bool CheckEnteredValuesBeforeStart()
        {
            bool ans = true;

            //if (!CheckMechSize())
            //{
            //    ans = false;
            //}
            //if (radioButtonCurrentUse.Checked)
            //{
            //    try
            //    {
            //        StreamReader sr = new StreamReader(textBoxUseCurrents.Text);
            //    }
            //    catch (Exception)
            //    {
            //        textBox1.AppendText("Указан неверный путь к файлу с токами" + Environment.NewLine);
            //        ans = false;
            //    }
            //}            
            return ans;
        }

        private bool CheckMechSize()
        {
            return true;
        }

        private void Initialcondition()
        {
            //
            // очистка
            //            
            Form1.Instance.ClearResults();
            Form1.Instance.GraphControl.Clear();
            //GraphControl.ListClear();
            //
            // Создание объекта решений
            //
            int solutionNumber = 0;
            //SourceTemplate s = Logic.Instance.Sources;
            //SourceTemplate2.CreateSources();



            foreach (var s in SourceTemplate2.Sources)
            {
                foreach (var r in Requests)
                {
                    if (r.Include)
                    {
                        solutionNumber++;
                    }
                }
            }


            solutions = new Solution(solutionNumber * Frequencies.Count);
            //
            // График
            //

            //GraphControl.myPane.XAxis.Scale.Min = StartAngle;
            //GraphControl.myPane.XAxis.Scale.Max = FinishAngle;


            // !!!

            //if ((FinishAngle - StartAngle) < 90)
            //{
            //    GraphControl.myPane.XAxis.Scale.MajorStep = 15.0;
            //}
            //else
            //{
            //    GraphControl.myPane.XAxis.Scale.MajorStep = 30.0;// Крупные риски по оси X будут идти с периодом 5
            //}


            // Мелкие риски будут идти с периодом 1
            // Таким образом, между крупными рисками будет 5 делений или 4 риски
            Form1.Instance.GraphControl.myPane.XAxis.Scale.MinorStep = 0.0 / 5.0;


            // По оси Y установим автоматический подбор масштаба
            Form1.Instance.GraphControl.myPane.YAxis.Scale.MinAuto = true;
            Form1.Instance.GraphControl.myPane.YAxis.Scale.MaxAuto = true;

            // !!! Установим значение параметра IsBoundedRanges как true.
            // !!! Это означает, что при автоматическом подборе масштаба 
            // !!! нужно учитывать только видимый интервал графика
            Form1.Instance.GraphControl.myPane.IsBoundedRanges = true;

            Form1.Instance.GraphControl.AxisChange();
            Form1.Instance.GraphControl.Refresh();
            Form1.Instance.GraphControl.Invalidate();

        }
        public void Start()
        {
            if (CheckEnteredValuesBeforeStart())
            {
                Form1.Instance.renderControl1.Pause();
                Stopwatch gtime = Stopwatch.StartNew();
                Initialcondition();
                Form1.Instance.textBox1.AppendText("**************************START**************************" + Environment.NewLine);
                int i = 0;

                foreach (double frequency in Frequencies)
                {
                    CV.F0 = frequency;
                    foreach (SourceTemplate2 source in SourceTemplate2.Sources)
                    {
                        Antenna.Generate(frequency, source);

                        foreach (FarFieldRequestTemplate request in Requests)
                        {
                            if (request.Include)
                            {
                                solutions[i].frequency = frequency;
                                solutions[i].sourceName = source.name;
                                solutions[i].requestName = request.Lable;

                                double startTheta = 0, finishTheta = 0, stepTheta = 0, startPhi = 0, finishPhi = 0, stepPhi = 0;
                                int sys = request.SystemOfCoordinates;

                                if (request.FarFieldType == 0)
                                {
                                    startTheta = request.ThetaStart;
                                    finishTheta = request.ThetaFinish;
                                    stepTheta = request.Delta;
                                    startPhi = request.PhiStart;
                                    finishPhi = request.PhiStart;
                                    stepPhi = request.Delta;
                                }
                                else
                                {
                                    double bodyAngle = request.BodyAngle;             //, bodyAnglePhi = 0;                                                                        
                                    double thetascan = 0, phiScan = 0;
                                    sys = 0;

                                    if (source.scanning == 1)
                                    {
                                        thetascan = source.thetaScanE;
                                        phiScan = source.phiScanE;
                                    }
                                    else if (source.scanning == 2)
                                    {
                                        Point3D pN = new Point3D(Antenna[0].Norma.X, Antenna[0].Norma.Y, Antenna[0].Norma.Z);
                                        pN = RotateElement(pN, source.thetaScanM, source.axis1x1, source.axis1y1, source.axis1z1, source.axis1x2, source.axis1y2, source.axis1z2);
                                        if (source.axis2Include)
                                        {
                                            pN = RotateElement(pN, source.phiScanM, source.axis2x1, source.axis2y1, source.axis2z1, source.axis2x2, source.axis2y2, source.axis2z2);
                                        }
                                        thetascan = Math.Acos(pN.Z / Math.Sqrt(pN.X * pN.X + pN.Y * pN.Y + pN.Z * pN.Z)) / Math.PI * 180;
                                        phiScan = Math.Atan2(pN.Y, pN.X) / Math.PI * 180;
                                    }
                                    else
                                    {
                                        Point3D pN = new Point3D(Antenna[0].Norma.X, Antenna[0].Norma.Y, Antenna[0].Norma.Z);
                                        thetascan = Math.Acos(pN.Z / Math.Sqrt(pN.X * pN.X + pN.Y * pN.Y + pN.Z * pN.Z)) / Math.PI * 180;
                                        phiScan = Math.Atan2(pN.Y, pN.X) / Math.PI * 180;
                                    }


                                    if (request.Direction == "Азимутальная (XY)")
                                    {
                                        startTheta = thetascan;
                                        finishTheta = thetascan;
                                        stepTheta = request.BodyAngleStep;
                                        startPhi = phiScan - bodyAngle / 2;
                                        finishPhi = phiScan + bodyAngle / 2;
                                        stepPhi = request.BodyAngleStep;
                                    }
                                    else if (request.Direction == "Угломестная (YZ)")
                                    {
                                        startTheta = thetascan - bodyAngle / 2;
                                        finishTheta = thetascan + bodyAngle / 2;
                                        stepTheta = request.BodyAngleStep;
                                        startPhi = phiScan;
                                        finishPhi = phiScan;
                                        stepPhi = request.BodyAngleStep;
                                    }

                                }
                                if (Form1.Instance.toolStripMenuItemTurnOn.Checked)
                                {
                                    try
                                    {
                                        TreeNode parentNodeCurrent = Form1.Instance.GetParentNode("NearField");
                                        string text = parentNodeCurrent.Text;
                                        string FileName = text.Substring(text.IndexOf("[") + 1);
                                        FileName = FileName.Remove(FileName.Length - 1);
                                        RadomeNearField = ReadNearField(RadomeComposition, FileName);
                                    }
                                    catch (Exception)
                                    {
                                        Form1.Instance.textBox1.AppendText("Не удалось загрузить поле внутри обтекателя");
                                    }
                                }

                                if (request.AntenaField)
                                {
                                    Form1.Instance.textBox1.AppendText(NowTime + " - Расчёт поля (" + request.Lable + ") в дальней зоне апертуры (" + source.name + ") в отсутствие обтекателя при частоте " + frequency / 1e9 + " ГГц" + Environment.NewLine);
                                    solutions[i].ffantenna = FarFieldC.FarFieldFromSurfaceCurrentCpp(Antenna.ApertureCurrent, frequency, startTheta, finishTheta, startPhi, finishPhi,
                                        stepTheta, stepPhi, sys, source.name, Proc);
                                }
                                if (request.RadomeField)
                                {
                                    if (RadomeNearField == null || !Form1.Instance.toolStripMenuItemTurnOn.Checked)
                                    {
                                        NearFieldCalculation(frequency);
                                    }
                                    Form1.Instance.textBox1.AppendText(NowTime + " - Расчёт поля (" + request.Lable + ") в дальней зоне от апертуры (" + source.name + ") c обтекателем при частоте " + frequency / 1e9 + " ГГц" + Environment.NewLine);
                                    Current RadomeCurrent = Current.CurrentsExitetion(RadomeNearField, RadomeComposition, Direction.Inside);
                                    solutions[i].ffradome = FarFieldC.FarFieldScatteredCpp(RadomeComposition, RadomeCurrent, frequency, startTheta, finishTheta, startPhi, finishPhi,
                                        stepTheta, stepPhi, sys, request.Lable, 1f, Proc);
                                }
                                Current reflactedCurrents = null;
                                if (request.ReflactedField)
                                {
                                    if (RadomeNearField == null)
                                    {
                                        NearFieldCalculation(frequency);
                                    }

                                    Form1.Instance.textBox1.AppendText(String.Format("{0} - Расчёт отражённого от стенки обтекателя поля (без прохождения) в дальней зоне ({1}) апертуры ({2}) при частоте {3} ГГц" + Environment.NewLine, NowTime, request.Lable, source.name, frequency / 1e9));
                                    NearFieldC reflactedNearField = NearFieldC.ReflactedNearFieldCpp(RadomeComposition, RadomeNearField, frequency, Proc);
                                    reflactedCurrents = Current.CurrentsExitetion(reflactedNearField, RadomeComposition, Direction.Inside);
                                    solutions[i].ffreflactionWithoutTransition = FarFieldC.FarFieldFromSurfaceCurrentCpp(reflactedCurrents, frequency, startTheta, finishTheta, startPhi, finishPhi,
                                        stepTheta, stepPhi, sys, source.name, Proc);

                                    if (solutions[i].ffradome != null)
                                    {
                                        solutions[i].ffradomeAndReflactionWithoutTransSum = FarFieldC.Add(solutions[i].ffradome, solutions[i].ffreflactionWithoutTransition);
                                    }
                                }
                                if (request.ReflactedItField)
                                {
                                    if (RadomeNearField == null)
                                    {
                                        NearFieldCalculation(frequency);
                                    }
                                    Form1.Instance.textBox1.AppendText(String.Format("{0} - Расчёт отражённого поля ({1}) в дальней зоне от апертуры ({2}) c обтекателем при частоте {3} ГГц" + Environment.NewLine, NowTime, request.Lable, source.name, frequency / 1e9));
                                    if (reflactedCurrents == null)
                                    {
                                        NearFieldC reflactedNearField = NearFieldC.ReflactedNearFieldCpp(RadomeComposition, RadomeNearField, frequency, Proc);
                                        reflactedCurrents = Current.CurrentsExitetion(reflactedNearField, RadomeComposition, Direction.Inside);
                                    }
                                    NearFieldC reflactedIncNearField = NearFieldC.InducedNearFieldCpp(reflactedCurrents, RadomeComposition, CV.F0, Direction.Inside, Proc);
                                    Current inducedCurrent = Current.CurrentsExitetion(reflactedIncNearField, RadomeComposition, Direction.Inside);
                                    solutions[i].ffreflactionWithTransition = FarFieldC.FarFieldScatteredCpp(RadomeComposition, inducedCurrent, frequency, startTheta, finishTheta, startPhi, finishPhi,
                                        stepTheta, stepPhi, sys, request.Lable, 1f, Proc);
                                    solutions[i].ffradomeAndReflactionWithTransSum = FarFieldC.Add(solutions[i].ffradome, solutions[i].ffreflactionWithTransition);
                                }
                                i++;
                            }
                        }
                        RadomeNearField = null;
                    }
                }
                FullTreeViewResults();
                DiagramWithdrawal();
                gtime.Stop();
                if (!Form1.Instance.Error)
                {
                    Form1.Instance.textBox1.AppendText(NowTime + " - Готово. Общее время расчета " + gtime.Elapsed.Minutes + " мин " + gtime.Elapsed.Seconds + " cек" + Environment.NewLine);
                }
                Form1.Instance.renderControl1.Play();
            }

        }
        public bool FastSaveScenario()
        {
            if (ProjectAdress == "" || ProjectAdress == null)
            {
                if (Form1.Instance.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ProjectAdress = Form1.Instance.saveFileDialog1.FileName;
                    SaveProject(ProjectAdress);
                    Form1.Instance.textBox1.AppendText(NowTime + " - Текущий проект сохранён в " + ProjectAdress + Environment.NewLine);
                    return true;
                }
                else
                {
                    return false;//ProjectAdress = Environment.CurrentDirectory + "\\tempBeamProject.txt";
                }

            }
            else
            {
                SaveProject(ProjectAdress);
                Form1.Instance.textBox1.AppendText(NowTime + " - Текущий проект сохранён в " + ProjectAdress + Environment.NewLine);
                return true;
            }
        }

        private void GraphWithdrawal()
        {
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
            //    string sep = "\t";

            //    int lines = GraphControl.myPane.CurveList[DictionaryLibrary.CurveNames[0]].Points.Count;


            //    string firstline = "Угол";
            //    string cname = "";


            //    for (int n = 0; n < treeViewResults.Nodes.Count; n++)
            //    {
            //        cname = treeViewResults.Nodes[n].Tag.ToString();
            //        firstline += sep + cname;
            //        for (int nn = 0; nn < treeViewResults.Nodes[n].Nodes.Count; nn++)
            //        {
            //            cname = treeViewResults.Nodes[n].Nodes[nn].Tag.ToString();
            //            firstline += sep + cname;
            //        }
            //    }
            //    sw.WriteLine(firstline);


            //    for (int ls = 0; ls < lines; ls++)
            //    {
            //        cname = treeViewResults.Nodes[0].Tag.ToString();
            //        int curvenumb = DictionaryLibrary.CurveNames.FirstOrDefault(x => x.Value == cname).Key;
            //        double val = GraphControl.ListPointPairs[curvenumb].ElementAt(ls).X;
            //        string line = val.ToString();

            //        for (int n = 0; n < treeViewResults.Nodes.Count; n++)
            //        {
            //            cname = treeViewResults.Nodes[n].Tag.ToString();
            //            curvenumb = CurveNameDict.FirstOrDefault(x => x.Value == cname).Key;
            //            val = GraphControl.ListPointPairs[curvenumb].ElementAt(ls).Y;
            //            line += sep + val.ToString();

            //            for (int nn = 0; nn < treeViewResults.Nodes[n].Nodes.Count; nn++)
            //            {
            //                cname = treeViewResults.Nodes[n].Nodes[nn].Tag.ToString();
            //                curvenumb = CurveNameDict.FirstOrDefault(x => x.Value == cname).Key;
            //                val = GraphControl.ListPointPairs[curvenumb].ElementAt(ls).Y;
            //                line += sep + val.ToString();
            //            }
            //        }
            //        sw.WriteLine(line.Replace(",", "."));
            //    }
            //    sw.Close();
            //}
        }
        public void DiagramWithdrawal()
        {
            try
            {
                string path = Path.GetDirectoryName(ProjectAdress);
                path = Path.Combine(path, "Results");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                for (int i = 0; i < solutions.Count; i++)
                {
                    StreamWriter sw = new StreamWriter(Path.Combine(path, solutions[i].Name + ".txt"));
                    
                    FarFieldC ffr = solutions[i].ffradome;
                    FarFieldC ffa = solutions[i].ffantenna;                    
                    FarFieldC ffref = solutions[i].ffreflactionWithoutTransition;
                    FarFieldC ffradRef = solutions[i].ffradomeAndReflactionWithoutTransSum;
                    FarFieldC ffrefTr= solutions[i].ffreflactionWithTransition;
                    FarFieldC ffrefTrSum = solutions[i].ffradomeAndReflactionWithTransSum;

                    double max = FindMaxNormaValue(ffa);

                    sw.Write("Theta, deg\tPhi, deg\t");

                    if (ffa != null)
                    {
                        sw.Write("Ant [Mag,dB]\tAnt [Phase,deg]\t");
                    }
                    if (ffr != null)
                    {
                        sw.Write("AntRad [Mag,dB]\tAntRad [Phase,deg]\t");
                    }
                    if (ffref != null)
                    {
                        sw.Write("RadRef [Mag,dB]\tRadRef [Phase,deg]\t");
                    }
                    if (ffradRef != null)
                    {
                        sw.Write("RadRefSum [Mag,dB]\tRadRefSum [Phase,deg]\t");
                    }
                    if (ffrefTr != null)
                    {
                        sw.Write("RadRefTr [Mag,dB]\tRadRefTr [Phase,deg]\t");
                    }
                    if (ffrefTrSum != null)
                    {
                        sw.Write("RadRefTrSum [Mag,dB]\tRadRefTrSum [Phase,deg]\t");
                    }

                    string line = String.Format("Angle, deg\t deg\tAntenna with Radome Field Amplitude, dB\tAntenna with Radome Field Phase, deg\tReflacted Field Amplitude, dB\tReflacted Field Phase, deg\tSummary Antenna with Radome Field Amplitude, dB\tSummary Antenna with Radome Field Phase, deg");
                    sw.WriteLine(line);
                    if (ffr != null)
                    {
                        for (int k = 0; k < ffr.Count; k++)
                        {
                            sw.Write(String.Format("{0}\t{1}\t", ffr[k].LocalTheta, ffr[k].LocalPhi));

                            if (ffa != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffa[k].Etotal, ffa[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            if (ffr != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffr[k].Etotal, ffr[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            if (ffref != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffref[k].Etotal, ffref[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            if (ffradRef != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffradRef[k].Etotal, ffradRef[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            if (ffrefTr != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffrefTr[k].Etotal, ffrefTr[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            if (ffrefTrSum != null)
                            {
                                sw.Write(String.Format("{0}\t{1}\t", ffrefTrSum[k].Etotal, ffrefTrSum[k].EdirectPhase(Antenna) / CV.pi * 180).Replace(",", "."));
                            }
                            sw.Write(Environment.NewLine);
                        }
                    }
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void ReturnNewProjectParam()
        {
            Form1.Instance.renderControl1.ClearAll();
            Form1.Instance.renderControl1.InitialLook();

            RadomeComposition = new Radome();
            Antenna = null;
            RadomeNearField = null;
            Form1.Instance.selectedNode = null;
            Form1.Instance.lockdata = false;
            solutions = null;

            Requests.Clear();
            SourceTemplate2.Lable = null;
            LoadFieldName = "";
            RadomeLayers.Clear();
            Frequencies.Clear();

            Form1.Instance.Loading = true;

            ProjectAdress = "";
            Form1.Instance.Text = "Новый проект";

            Form1.Instance.Error = false;

            Form1.Instance.Loading = true;
            Form1.Instance.radioButtonFreeRotate.Checked = true;

            Form1.Instance.RefreshTreeViewConfiguration();
            Form1.Instance.ClearResults();

            Form1.Instance.Loading = false;
        }

        #region Load data from files
        public void LoadProject()
        {
            Form1.Instance.openFileDialog1.Title = "Загрузка проекта";
            Form1.Instance.openFileDialog1.Filter = "Project Files (*.txt)|*.txt|All files (*.*)|*.*";
            Form1.Instance.openFileDialog1.FilterIndex = 1;
            Form1.Instance.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (Form1.Instance.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool exception = false;
                try
                {
                    exception = !ReadProjectFile(Form1.Instance.openFileDialog1.FileName);
                }
                catch (Exception)
                {
                    exception = true;
                }


                if (!exception)
                {
                    SourceTemplate2.CreateSources();
                    Form1.Instance.renderControl1.ClearAll();
                    Form1.Instance.RefreshTreeViewConfiguration();
                    Form1.Instance.ClearResults();

                    int apCount = 0;
                    int radCount = 0;
                    if (!(Antenna == null) && !(RadomeComposition.Count == 0))
                    {
                        Form1.Instance.renderControl1.LookAt(Antenna, RadomeComposition);
                        for (int ir = 0; ir < RadomeComposition.Count; ir++)
                        {
                            Form1.Instance.renderControl1.Draw(RadomeComposition[ir]);
                            radCount += RadomeComposition[ir].Count;
                        }

                        Form1.Instance.renderControl1.Draw(Antenna);
                        apCount = Antenna.Count;
                    }
                    else
                    {
                        if (!(Antenna == null))
                        {
                            Form1.Instance.renderControl1.LookAt(Antenna);
                            Form1.Instance.renderControl1.Draw(Antenna);
                            apCount = Antenna.Count;
                        }
                        if (RadomeComposition.Count != 0)
                        {
                            Form1.Instance.renderControl1.LookAt(RadomeComposition);
                            for (int ir = 0; ir < RadomeComposition.Count; ir++)
                            {
                                Form1.Instance.renderControl1.Draw(RadomeComposition[ir]);
                                radCount += RadomeComposition[ir].Count;
                            }
                        }
                    }

                    foreach (FarFieldRequestTemplate req in Requests)
                    {
                        if (req.FarFieldType == 0)
                        {
                            if (req.Include)
                            {
                                double thetaFinish = req.ThetaFinish;
                                double thetaStart = req.ThetaStart;
                                double phiStart = req.PhiStart;
                                double delta = req.Delta;
                                int systemOfCoordinates = req.SystemOfCoordinates;
                                string title = req.Lable;
                                Form1.Instance.renderControl1.Draw(title, thetaStart, thetaFinish, phiStart, delta, systemOfCoordinates);
                            }
                        }
                    }

                    if (SourceTemplate2.Scanning == 1)
                    {
                        LoadEScanningPoints(SourceTemplate2.Lable);
                    }
                    else if(SourceTemplate2.Scanning == 2)
                    {
                        LoadMScanningPoints(SourceTemplate2.Lable);
                    }

                    

                    Form1.Instance.textBox1.AppendText("Проект загружен: на апертуре " + apCount + " элементов, на обтекателе " + radCount + " элементов" + Environment.NewLine);
                }
                else
                {
                    MessageBox.Show("Файл неподходящего формата или повреждён", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public void LoadEScanningPoints(string lable)
        {
            double startTheta = SourceTemplate2.ThetaScanEStart;
            double finishTheta = SourceTemplate2.ThetaScanEFinish;
            double stepTheta = SourceTemplate2.ThetaScanEStep;
            double startPhi = SourceTemplate2.PhiScanEStart;
            double finishPhi = SourceTemplate2.PhiScanEFinish;
            double stepPhi = SourceTemplate2.PhiScanEStep;
            int sys = SourceTemplate2.SystemOfCoordinatesScan;

            if (startTheta <= finishTheta && startPhi <= finishPhi && stepTheta != 0 && stepPhi != 0)
            {
                int blueCol = new Color(30, 225, 225).ToRgba();
                Form1.Instance.renderControl1.Draw(lable, startTheta, finishTheta, stepTheta, startPhi, finishPhi, stepPhi, sys, blueCol);
            }
        }

        public void LoadMScanningPoints(string lable)
        {
            double axis1x1 = SourceTemplate2.Axis1x1;
            double axis1y1 = SourceTemplate2.Axis1y1;
            double axis1z1 = SourceTemplate2.Axis1z1;

            double axis1x2 = SourceTemplate2.Axis1x2;
            double axis1y2 = SourceTemplate2.Axis1y2;
            double axis1z2 = SourceTemplate2.Axis1z2;

            double axis2x1 = SourceTemplate2.Axis2x1;
            double axis2y1 = SourceTemplate2.Axis2y1;
            double axis2z1 = SourceTemplate2.Axis2z1;

            double axis2x2 = SourceTemplate2.Axis2x2;
            double axis2y2 = SourceTemplate2.Axis2y2;
            double axis2z2 = SourceTemplate2.Axis2z2;

            double scanMPhiStart = SourceTemplate2.PhiScanMStart;
            double scanMPhiFinish = SourceTemplate2.PhiScanMFinish;
            double scanMPhiStep = SourceTemplate2.PhiScanMStep;

            double scanMThetaStart = SourceTemplate2.ThetaScanMStart;
            double scanMThetaFinish = SourceTemplate2.ThetaScanMFinish;
            double scanMThetaStep = SourceTemplate2.ThetaScanMStep;

            bool include1 = SourceTemplate2.Axis1Include;
            bool include2 = SourceTemplate2.Axis2Include;

            DVector n = Logic.Instance.Antenna.elements[0].Norma;
            Point3D center = Logic.Instance.Antenna.Center;


            if (scanMThetaStart <= scanMThetaFinish && scanMThetaStep != 0 || !include1)
            {
                if (scanMPhiStart <= scanMPhiFinish && scanMPhiStep != 0 || !include2)
                {
                    int xCol = new Color(255, 0, 128).ToBgra();
                    Point3D p1a1 = new Point3D(axis1x1, axis1y1, axis1z1);
                    Point3D p2a1 = new Point3D(axis1x2, axis1y2, axis1z2);
                    Point3D p1a2 = new Point3D(axis2x1, axis2y1, axis2z1);
                    Point3D p2a2 = new Point3D(axis2x2, axis2y2, axis2z2);

                    Form1.Instance.renderControl1.Draw(lable, center, n, include1, p1a1, p2a1, scanMThetaStart, scanMThetaFinish, scanMThetaStep, include2, p1a2, p2a2, scanMPhiStart, scanMPhiFinish, scanMPhiStep, xCol);
                }


            }            
        }
        public void LoadRadomeElementMesh(CreateRadomeForm crf)
        {
            OpenFileDialog openFileDialog1 = Form1.Instance.openFileDialog1;
            RenderControl renderControl1 = Form1.Instance.renderControl1;

            Form1.Instance.openFileDialog1.Filter = "Mesh Files (*.nas, *.msh)|*.nas;*.msh|Special Files (*.msh3)|*.msh3|All files|*.*";
            Form1.Instance.openFileDialog1.FilterIndex = 3;
            Form1.Instance.openFileDialog1.Title = "Загрузка сетки обтекателя";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    double dim = 0;
                    using (UnitForm form = new UnitForm(Form1.Instance))
                    {
                        crf.TopMost = false;
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                        dim = form.Dim;
                        //radomeTemplate.MagnifyToMeter(dim);
                        crf.TopMost = true;
                    }
                    

                    string radomeFileName = openFileDialog1.FileName;
                    string a = openFileDialog1.FileName.Substring(openFileDialog1.FileName.IndexOf(".") + 1);

                    Mesh radome = null;
                    if (a == "nas")
                    {
                        radome = Mesh.ReadingMeshNastran(radomeFileName, dim);
                    }
                    else if (a == "msh")
                    {
                        radome = Mesh.ReadingMeshGMSH(radomeFileName, dim);
                    }
                    else if (a == "msh3")
                    {
                        radome = Mesh.ReadingMeshMsh3(radomeFileName, dim);
                    }



                    string uniqueLable = crf.tempRadome.GetUniqueLable(radome.Count);
                    RadomeElement radomeTemplate = new RadomeElement(radome)
                    {
                        Lable = uniqueLable,
                        Color = Color.Transparent,
                        Include = true
                    };
                    crf.tempRadome.Add(radomeTemplate);

                    Aperture Antenna = Instance.Antenna;

                    if (Antenna != null)
                    {
                        renderControl1.LookAt(Antenna, crf.tempRadome);
                        renderControl1.Draw(radomeTemplate);
                        renderControl1.Draw(Antenna);
                    }
                    else
                    {
                        renderControl1.LookAt(crf.tempRadome);
                        renderControl1.Draw(radomeTemplate);
                    }


                    crf.dataGridView1.Rows.Add(true, crf.tempRadome[crf.tempRadome.Count - 1].Lable, "[Пусто]", "Белый");

                    DataGridViewCell cb = crf.dataGridView1[2, crf.dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed)];
                    DataGridViewComboBoxCell cbc = cb as DataGridViewComboBoxCell;
                    for (int i = 0; i < Logic.Instance.RadomeLayers.Count; i++)
                    {
                        cbc.Items.Add(Logic.Instance.RadomeLayers[i].Lable);
                    }


                    Form1.Instance.textBox1.AppendText("Загружена геометрия обтекателя из " + radome.Count + " элементов" + Environment.NewLine);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Файл неподходящего формата или повреждён \n" + e.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }
        public void LoadAntennaMesh()
        {
            Form1.Instance.openFileDialog1.Filter = "Mesh Files (*.nas, *.msh)|*.nas;*.msh|Special Files (*.msh3, *.msh4)|*.msh3;*.msh4|All files|*.*";
            Form1.Instance.openFileDialog1.FilterIndex = 3;
            Form1.Instance.openFileDialog1.Title = "Загрузка треугольной сетки апертуры";
            if (Form1.Instance.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string antennaFileName = Form1.Instance.openFileDialog1.FileName;
                    string a = Form1.Instance.openFileDialog1.FileName.Substring(Form1.Instance.openFileDialog1.FileName.IndexOf(".") + 1);
                    double dim = 0;
                    using (UnitForm form = new UnitForm(Form1.Instance))
                    {
                        form.StartPosition = FormStartPosition.Manual;
                        form.Location = new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y);
                        form.ShowDialog();
                        dim = form.Dim;                        
                    }
                    Mesh antennaMesh = null;
                    if (a == "nas")
                    {
                        antennaMesh = Mesh.ReadingMeshNastran(antennaFileName, dim);
                        Antenna = new Aperture(antennaMesh.ListX, antennaMesh.ListY, antennaMesh.ListZ, antennaMesh.ListI1, antennaMesh.ListI2, antennaMesh.ListI3);
                    }
                    if (a == "msh")
                    {
                        antennaMesh = Mesh.ReadingMeshGMSH(antennaFileName, dim);
                        Antenna = new Aperture(antennaMesh.ListX, antennaMesh.ListY, antennaMesh.ListZ, antennaMesh.ListI1, antennaMesh.ListI2, antennaMesh.ListI3);
                    }
                    //else if (a == "txt")
                    //{
                    //    antennaMesh = Mesh.ReadingMeshTxt(antennaFileName, dim);
                    //}
                    else if (a == "msh3")
                    {
                        antennaMesh = Mesh.ReadingMeshMsh3(antennaFileName, dim);
                    }

                    if (antennaMesh != null)
                    {
                        Antenna = new Aperture(ref antennaMesh);                        
                        if (RadomeComposition.Count != 0)
                        {
                            Form1.Instance.renderControl1.LookAt(RadomeComposition);
                        }
                        else
                        {
                            Form1.Instance.renderControl1.LookAt(Antenna);
                        }
                        Form1.Instance.renderControl1.Draw(Antenna);
                        Form1.Instance.textBox1.AppendText("Загружена геометрия антенны из " + Antenna.Count + " элементов" + Environment.NewLine);
                    }
                    if (Antenna != null)
                    {
                        Antenna.GeneratePolarizationCurrents(CreateApertureForm.DistributionType);
                        CreateApertureForm form = new CreateApertureForm(Form1.Instance, true);
                    }

                    else
                    {
                        Form1.Instance.textBox1.AppendText("Геометрия антенны не загружена" + Environment.NewLine);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Файл неподходящего формата или повреждён", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void LoadNearFiled()
        {
            if (RadomeComposition.Count != 0)
            {
                Form1.Instance.openFileDialog1.FilterIndex = 4;
                if (Form1.Instance.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string FileName = Form1.Instance.openFileDialog1.FileName;
                    RadomeNearField = ReadNearField(RadomeComposition, FileName);
                    if (RadomeNearField != null)
                    {
                        TreeNode parentNodeCurrent = Form1.Instance.GetParentNode("NearField");
                        parentNodeCurrent.Text = "Поле в обтекателе [" + FileName + "]";
                        Form1.Instance.toolStripMenuItemTurnOn.Checked = true;
                        Form1.Instance.toolStripMenuItemTurnOn.Enabled = true;
                    }
                }
            }
            else
            {
                Form1.Instance.textBox1.Text += "Необходимо загрузить геометрию обтекателя" + Environment.NewLine;
            }
        }
        public NearFieldC ReadNearField(Radome radome, String adress)
        {
            StreamReader sr = new StreamReader(adress);
            //CVector[] E = new CVector[radome.Count];
            //CVector[] H = new CVector[radome.Count];
            //Point3D[] P = new Point3D[radome.Count];


            List<Complex> ex = new List<Complex>();
            List<Complex> ey = new List<Complex>();
            List<Complex> ez = new List<Complex>();
            List<Complex> hx = new List<Complex>();
            List<Complex> hy = new List<Complex>();
            List<Complex> hz = new List<Complex>();
            List<double> px = new List<double>();
            List<double> py = new List<double>();
            List<double> pz = new List<double>();

            String line;

            while (!(sr.EndOfStream))
            {
                line = sr.ReadLine();

                String[] substrings = line.Split(new[] { "\t" }, StringSplitOptions.None);

                ex.Add(new Complex(Convert.ToDouble(substrings[0]), Convert.ToDouble(substrings[1])));
                ey.Add(new Complex(Convert.ToDouble(substrings[2]), Convert.ToDouble(substrings[3])));
                ez.Add(new Complex(Convert.ToDouble(substrings[4]), Convert.ToDouble(substrings[5])));
                hx.Add(new Complex(Convert.ToDouble(substrings[6]), Convert.ToDouble(substrings[7])));
                hy.Add(new Complex(Convert.ToDouble(substrings[8]), Convert.ToDouble(substrings[9])));
                hz.Add(new Complex(Convert.ToDouble(substrings[10]), Convert.ToDouble(substrings[11])));
                px.Add((Convert.ToDouble(substrings[12])));
                py.Add((Convert.ToDouble(substrings[13])));
                pz.Add((Convert.ToDouble(substrings[14])));

            }
            sr.Close();


            if (radome.CountElements == ex.Count)
            {
                NearFieldC nfc = new NearFieldC(ex.Count);
                for (int i = 0; i < ex.Count; i++)
                {
                    nfc[i].E = new CVector(ex[i], ey[i], ez[i]);
                    nfc[i].H = new CVector(hx[i], hy[i], hz[i]);
                    nfc[i].Place = new Point3D(px[i], py[i], pz[i]);
                }
                Form1.Instance.textBox1.AppendText("Поле внутри обтекателя загружено" + Environment.NewLine);
                Form1.Instance.toolStripMenuItemTurnOn.Checked = true;
                Form1.Instance.toolStripMenuItemTurnOn.Enabled = true;
                return nfc;
            }
            else
            {
                Form1.Instance.textBox1.AppendText("Ошибка загрузки поля внутри обтекателя" + Environment.NewLine);
                return null;
            }
        }

        private bool ReadProjectFile(string projectAdress)
        {
            Form1.Instance.Loading = true;
            String line = "";
            bool ans = false;
            CVector I = new CVector();
            CVector M = new CVector();
            CVector[] bicur = null;
            CVector[] bmcur = null;

            List<Stenka> buffStenka = new List<Stenka>();
            StreamReader sr = new StreamReader(projectAdress);
            Radome buffUnion = new Radome();
            string buffcurrentFileName = "";
            bool buffUseField = false;
            List<double> BFrequences = new List<double>();
            List<FarFieldRequestTemplate> BRequests = new List<FarFieldRequestTemplate>();

            do
            {
                line = sr.ReadLine();
                if (line == "Раздел 1")
                {
                    Char delimiter = '\t';
                    String[] arr;

                    sr.ReadLine();                                  //Дата сохраниения: 	
                    arr = sr.ReadLine().Split(delimiter);           //Частота сканирования:        
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        BFrequences.Add(Convert.ToDouble(arr[1 + i]));
                    }
                    sr.ReadLine();                                  //Задачи вычисления дальней зоны:
                    line = sr.ReadLine();                           //Название: 
                    while (line != "")
                    {
                        arr = line.Split(delimiter);
                        string lable = arr[1];

                        arr = sr.ReadLine().Split(delimiter);       //Начальный угол Theta:
                        double thetastart = Convert.ToDouble(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Конечный угол Theta:
                        double thetafinish = Convert.ToDouble(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Угол наклона:
                        double startphi = Convert.ToDouble(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Шаг сканирования:
                        double step = Convert.ToDouble(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Траектория сканирования (XYZ, ZXY, YZX):
                        int systemOfCoordinates = Convert.ToInt32(arr[1]);

                        arr = sr.ReadLine().Split(delimiter);       //Расчёт поля в дальней зоне от апертуры:
                        bool antenaField = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Расчёт поля в дальней зоне прошедшего через обтекатель:
                        bool radomeField = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Расчёт поля в дальней зоне отражённого от обтекателя:
                        bool reflactedField = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Расчёт поля в дальней зоне прошедшего через обтекатель с учётом переотражения от стенок:
                        bool reflactedItField = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Включение в модель:
                        bool include = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Тип поля:
                        int farFieldType = Convert.ToInt32(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //В направлнении сканирования
                        //string dir = "Азимутальная (XY)";
                        //double bodyangle = 5f;
                        //double bodyangleStep = 0.1f;
                        string dir = Convert.ToString(arr[1]);
                        double bodyangle = Convert.ToDouble(arr[2]);
                        double bodyangleStep = Convert.ToDouble(arr[3]);
                        BRequests.Add(new FarFieldRequestTemplate(thetastart, thetafinish, startphi, step, dir, bodyangle, bodyangleStep, systemOfCoordinates, lable, antenaField, radomeField, reflactedField, reflactedItField, include, farFieldType));
                        sr.ReadLine();                              // ////
                        line = sr.ReadLine();
                    }
                    line = sr.ReadLine();                           //Источники:
                    line = sr.ReadLine();                           //Название:
                    while (line != "")
                    {
                        arr = line.Split(delimiter);
                        string lable = arr[1];
                        arr = sr.ReadLine().Split(delimiter);       //Распределние токов:
                        string distribution = arr[1];
                        arr = sr.ReadLine().Split(delimiter);       //Разностный канал:
                        bool difference = Convert.ToBoolean(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Ось разностного канала:
                        string axis = arr[1];
                        arr = sr.ReadLine().Split(delimiter);       //Поляризация:
                        Dictionary<string, bool> polarization = new Dictionary<string, bool>();
                        for (int i = 0; i < (arr.Length - 1) / 2; i++)
                        {
                            polarization.Add(arr[2 * i + 1], Convert.ToBoolean(arr[2 * i + 2]));
                        }
                        //bool[] polarization = new bool[4] { true, false, false, false }; //Convert.ToInt32(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Электрические токи на поверхности апертуры:
                        //CVector curI = new CVector(new Complex(Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2])), new Complex(Convert.ToDouble(arr[3]), Convert.ToDouble(arr[4])), new Complex(Convert.ToDouble(arr[5]), Convert.ToDouble(arr[6])));
                        arr = sr.ReadLine().Split(delimiter);       //Магнитные токи на поверхности апертуры:
                        //CVector curM = new CVector(new Complex(Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2])), new Complex(Convert.ToDouble(arr[3]), Convert.ToDouble(arr[4])), new Complex(Convert.ToDouble(arr[5]), Convert.ToDouble(arr[6])));
                        arr = sr.ReadLine().Split(delimiter);       //Активировать сканирование:
                        int scan = Convert.ToInt32(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Параметры электоронного сканирования по Theta:
                        double scanThetaStart = Convert.ToDouble(arr[1]);
                        double scanThetaFinish = Convert.ToDouble(arr[2]);
                        double scanThetaStep = Convert.ToDouble(arr[3]);
                        arr = sr.ReadLine().Split(delimiter);       //Параметры электоронного сканирования по Phi:
                        double scanPhiStart = Convert.ToDouble(arr[1]);
                        double scanPhiFinish = Convert.ToDouble(arr[2]);
                        double scanPhiStep = Convert.ToDouble(arr[3]);
                        arr = sr.ReadLine().Split(delimiter);       //Система координат при сканировании:
                        int systemOfCoordScan = Convert.ToInt32(arr[1]);
                        arr = sr.ReadLine().Split(delimiter);       //Включить в модель:
                        bool axis1Include = Convert.ToBoolean(arr[1]);
                        bool axis2Include = Convert.ToBoolean(arr[2]);
                        arr = sr.ReadLine().Split(delimiter);       //Ось 1, точка 1:
                        double axis1x1 = Convert.ToDouble(arr[1]);
                        double axis1y1 = Convert.ToDouble(arr[2]);
                        double axis1z1 = Convert.ToDouble(arr[3]);

                        arr = sr.ReadLine().Split(delimiter);       //Ось 1, точка 2:
                        double axis1x2 = Convert.ToDouble(arr[1]);
                        double axis1y2 = Convert.ToDouble(arr[2]);
                        double axis1z2 = Convert.ToDouble(arr[3]);

                        arr = sr.ReadLine().Split(delimiter);       //Ось 2, точка 1:
                        double axis2x1 = Convert.ToDouble(arr[1]);
                        double axis2y1 = Convert.ToDouble(arr[2]);
                        double axis2z1 = Convert.ToDouble(arr[3]);

                        arr = sr.ReadLine().Split(delimiter);       //Ось 2, точка 2:
                        double axis2x2 = Convert.ToDouble(arr[1]);
                        double axis2y2 = Convert.ToDouble(arr[2]);
                        double axis2z2 = Convert.ToDouble(arr[3]);

                        arr = sr.ReadLine().Split(delimiter);       //Угол Phi механиxанического сканирования:
                        double scanMPhiStart = Convert.ToDouble(arr[1]);
                        double scanMPhiFinish = Convert.ToDouble(arr[2]);
                        double scanMPhiStep = Convert.ToDouble(arr[3]);

                        arr = sr.ReadLine().Split(delimiter);       //Угол Theta механиxанического сканирования:
                        double scanMThetaStart = Convert.ToDouble(arr[1]);
                        double scanMThetaFinish = Convert.ToDouble(arr[2]);
                        double scanMThetaStep = Convert.ToDouble(arr[3]);

                        SourceTemplate2.SetStaticSourceParameters(lable, null, null, polarization, distribution, difference, axis, scan, systemOfCoordScan, scanThetaStart, scanThetaFinish, scanThetaStep, scanPhiStart, scanPhiFinish, scanPhiStep, axis1Include, axis1x1, axis1y1, axis1z1, axis1x2, axis1y2, axis1z2, axis2Include, axis2x1, axis2y1, axis2z1, axis2x2, axis2y2, axis2z2, scanMPhiStart, scanMPhiFinish, scanMPhiStep, scanMThetaStart, scanMThetaFinish, scanMThetaStep);
                        sr.ReadLine();                              // //// 
                        line = sr.ReadLine();
                    }
                    arr = sr.ReadLine().Split(delimiter);           //Путь:
                    buffcurrentFileName = arr[1];

                    arr = sr.ReadLine().Split(delimiter);           //Использовать поля:               
                    buffUseField = Convert.ToBoolean(arr[1]);

                    ans = true;
                    //Конец раздела 1
                }
                else if (line == "Раздел 2")
                {

                    Char delimiter1 = ' ';
                    Char delimiter2 = '\t';
                    String[] arr1;
                    String[] arr2;
                    String[] arr3;
                    sr.ReadLine();                                  //Структура стенки обтекателя:
                    line = sr.ReadLine();                           //Конец раздела 2

                    while (line != "Конец раздела 2")
                    {
                        buffStenka.Add(new Stenka(line.Split(delimiter2)[1]));
                        buffStenka[buffStenka.Count - 1].Clear();
                        line = sr.ReadLine();
                        while (line.Split(delimiter1)[0] == "Слой")
                        {
                            arr1 = sr.ReadLine().Split(delimiter2); //Диэлектрическая проницаемость:
                            arr2 = sr.ReadLine().Split(delimiter2); //Магнитная проницаемость:
                            arr3 = sr.ReadLine().Split(delimiter2); //Толщина слоя:
                            sr.ReadLine();                          // ////
                            line = sr.ReadLine();
                            buffStenka[buffStenka.Count - 1].Add(new Complex(Convert.ToSingle(arr1[1]), Convert.ToSingle(arr1[2])),
                                new Complex(Convert.ToSingle(arr2[1]), Convert.ToSingle(arr2[2])), Convert.ToSingle(arr3[1]));
                        }
                    }
                    ans = true;
                    //Конец раздела 2
                }
                else if (line == "Раздел 3")
                {
                    Char delimiter = '\t';
                    String[] arr;
                    line = sr.ReadLine();
                    arr = line.Split(delimiter);                    //Количество частей:
                    int radomeParts = Convert.ToInt32(arr[1]);


                    for (int b = 0; b < radomeParts; b++)
                    {
                        //Название
                        string radomeLable = sr.ReadLine();         //Часть ..

                        //Параметры части обтекателя
                        line = sr.ReadLine();                       //Параметры части обтекателя (цвет, название стенки, параметр включения):
                        arr = line.Split(delimiter);
                        Color colorPart = CreateRadomeForm.colorDic[arr[1]];
                        string stenkaLable = arr[2];
                        bool includeParameter = Convert.ToBoolean(arr[3]);

                        //считывание координат
                        line = sr.ReadLine();                       //Координаты части обтекателя:                       

                        List<List<double>> buffPoints = new List<List<double>>();
                        List<List<int>> buffIndexes = new List<List<int>>();
                        for (int k = 0; k < 3; k++)
                        {
                            buffPoints.Add(new List<double>());
                            buffIndexes.Add(new List<int>());
                        }


                        if (line == "Координаты части обтекателя: \t")
                        {
                            line = sr.ReadLine();

                            while (!(line == "Конец координат части обтекателя \t"))
                            {
                                arr = line.Split(delimiter);
                                for (int c = 0; c < 3; c++)
                                {
                                    buffPoints[c].Add(Convert.ToDouble(arr[c]));
                                }
                                line = sr.ReadLine();
                            }
                        }
                        line = sr.ReadLine();
                        if (line == "Индексы части обтекателя: \t")
                        {
                            line = sr.ReadLine();

                            while (!(line == "Конец индексов части обтекателя \t"))
                            {
                                arr = line.Split(delimiter);
                                for (int c = 0; c < 3; c++)
                                {
                                    buffIndexes[c].Add(Convert.ToInt32(arr[c]));
                                }
                                line = sr.ReadLine();
                            }
                        }
                        int countRadEl = buffIndexes[0].Count;
                        Point3D[] p = new Point3D[countRadEl];
                        double[] s = new double[countRadEl];
                        DVector[] n = new DVector[countRadEl];

                        RadomeElement rElement = new RadomeElement(radomeLable, colorPart, buffPoints[0], buffPoints[1], buffPoints[2], buffIndexes[0], buffIndexes[1], buffIndexes[2], includeParameter);

                        if (buffStenka.Exists(x => x.Lable == stenkaLable))
                        {
                            int indexStenka = buffStenka.FindIndex(x => x.Lable == stenkaLable);
                            //Stenka getStenka = buffStenka.Find(x => x.Lable == stenkaLable);
                            rElement.Structure = buffStenka[indexStenka];
                        }

                        buffUnion.Add(rElement);
                        line = sr.ReadLine();
                    }
                    ans = true;
                    //Конец индексов антенны
                    //Конец раздела 4
                }
                else if (line == "Раздел 4")
                {
                    List<List<double>> buffData = new List<List<double>>();
                    for (int k = 0; k < 7; k++)
                    {
                        buffData.Add(new List<double>());
                    }

                    Char delimiter = '\t';
                    String[] arr;
                    line = sr.ReadLine();
                    if (line == "Координаты антенны: \t")
                    {
                        line = sr.ReadLine();
                        while (!(line == "Конец координат антенны"))
                        {
                            arr = line.Split(delimiter);
                            for (int c = 0; c < 7; c++)
                            {
                                buffData[c].Add(Convert.ToDouble(arr[c]));
                            }

                            line = sr.ReadLine();
                        }
                    }
                    line = sr.ReadLine();

                    int countApp = buffData[0].Count;
                    if (buffData[0].Count > 2)
                    {
                        Point3D[] p = new Point3D[countApp];
                        double[] s = new double[countApp];
                        DVector[] n = new DVector[countApp];

                        for (int b = 0; b < buffData[0].Count; b++)
                        {
                            p[b] = new Point3D(buffData[0][b], buffData[1][b], buffData[2][b]);
                            s[b] = buffData[3][b];
                            n[b] = new DVector(buffData[4][b], buffData[5][b], buffData[6][b]);
                        }
                        Antenna = new Aperture(p, s, n);
                    }
                    ans = true;
                }
                else if (line == "Раздел 5")
                {

                    line = sr.ReadLine();                              //Загруженные токи на апертуре антенны
                    if (line == "Загруженные токи на апертуре антенны")
                    {
                        Char delimiter = '\t';

                        line = sr.ReadLine();
                        int count = Convert.ToInt32(line); //{кол-во элементов}
                        bicur = new CVector[count];
                        bmcur = new CVector[count];
                        Point3D[] positions = new Point3D[count];
                        double[] areas = new double[count];

                        for (int q = 0; q < count; q++)
                        {
                            line = sr.ReadLine().Replace(".", ",");
                            string[] arr = line.Split(delimiter);

                            bicur[q] = new CVector(new Complex(Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1])), new Complex(Convert.ToDouble(arr[2]), Convert.ToDouble(arr[3])), new Complex(Convert.ToDouble(arr[4]), Convert.ToDouble(arr[5])));
                            bmcur[q] = new CVector(new Complex(Convert.ToDouble(arr[6]), Convert.ToDouble(arr[7])), new Complex(Convert.ToDouble(arr[8]), Convert.ToDouble(arr[9])), new Complex(Convert.ToDouble(arr[10]), Convert.ToDouble(arr[11])));
                        }
                    }
                }
            } while (!(sr.EndOfStream));
            sr.Close();

            if (buffcurrentFileName != "")
            {
                LoadFieldName = buffcurrentFileName;
                Form1.Instance.toolStripMenuItemTurnOn.Enabled = true;
                Form1.Instance.toolStripMenuItemTurnOn.Checked = buffUseField;
                Form1.Instance.toolStripMenuItemTurnOn_Click(Form1.Instance.toolStripMenuItemTurnOn, null);
            }


            Frequencies = new List<double>(BFrequences.Count);
            for (int k = 0; k < BFrequences.Count; k++)
            {
                Frequencies.Add(BFrequences[k]);
            }
            Requests = new List<FarFieldRequestTemplate>(BRequests.Count);
            for (int k = 0; k < BRequests.Count; k++)
            {
                Requests.Add(BRequests[k]);
            }
            //Sources = BSources;


            RadomeComposition = buffUnion;
            if (bicur != null)
            {
                SourceTemplate2.I = new CVector[bicur.Length];
                SourceTemplate2.M = new CVector[bmcur.Length];
                for (int s = 0; s < bicur.Length; s++)
                {
                    SourceTemplate2.I[s] = bicur[s];
                    SourceTemplate2.M[s] = bmcur[s];
                }
            }
            RadomeLayers.Clear();

            for (int i = 0; i < buffStenka.Count; i++)
            {
                RadomeLayers.Add(buffStenka[i]);
            }

            Antenna.GeneratePolarizationCurrents(SourceTemplate2.Distribution);

            Form1.Instance.Text = projectAdress;
            ProjectAdress = projectAdress;

            Form1.Instance.Loading = false;
            return ans;
        }
        public CVector[] ReadCurrentFromFile(OpenFileDialog openFileDialog, string titleText)
        {
            openFileDialog.Title = titleText;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog.FileName);
                int elmentsCount = Logic.Instance.Antenna.Count;
                double[,] datastream = new double[elmentsCount, 6];
                for (int i = 0; i < elmentsCount; i++)
                {
                    string line = sr.ReadLine();

                    string[] sline1 = line.Split('(');
                    string[] sline2 = sline1[1].Split(',');

                    datastream[i, 0] = Single.Parse(sline2[0].Replace(".", ","));
                    datastream[i, 1] = Single.Parse(sline2[1].Replace(".", ",").Replace(")", ""));

                    sline2 = sline1[2].Split(',');

                    datastream[i, 2] = Single.Parse(sline2[0].Replace(".", ","));
                    datastream[i, 3] = Single.Parse(sline2[1].Replace(".", ",").Replace(")", ""));

                    sline2 = sline1[3].Split(',');

                    datastream[i, 4] = Single.Parse(sline2[0].Replace(".", ","));
                    datastream[i, 5] = Single.Parse(sline2[1].Replace(".", ",").Replace(")", ""));
                }
                sr.Close();

                CVector[] cur = new CVector[elmentsCount];

                for (int i = 0; i < elmentsCount; i++)
                {
                    cur[i] = new CVector(new Complex(datastream[i, 0], datastream[i, 1]), new Complex(datastream[i, 2], datastream[i, 3]), new Complex(datastream[i, 4], datastream[i, 5]));
                }
                return cur;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region Writing to files
        public void SaveProject(string saveFileName)
        {
            string text;

            //Ix - компонента электрического тока на апертуре антенны
            //Iy - компонента электрического тока на апертуре антенны            

            //Mx - компонента магнитного тока на апертуре антенны
            //My - компонента магнитного тока на апертуре антенны
            StreamWriter sw = null;
            //try
            //{
            sw = new StreamWriter(saveFileName);



            text = "********************************************************************" + Environment.NewLine;
            text += "**************************ПРОТОКОЛ РАСЧЕТА**************************" + Environment.NewLine;
            text += "********************************************************************" + Environment.NewLine;
            text += "Раздел 1" + Environment.NewLine;
            text += "Дата сохраниения: \t" + DateTime.Today.ToString("d") + Environment.NewLine;
            text += "Частота сканирования: ";
            for (int i = 0; i < Frequencies.Count; i++)
            {
                text += "\t" + Frequencies[i];
            }
            text += Environment.NewLine;
            text += "Задачи вычисления дальней зоны:" + Environment.NewLine;
            for (int i = 0; i < Requests.Count; i++)
            {
                text += "Название: \t" + Requests[i].Lable + Environment.NewLine;
                text += "Начальный угол Theta: \t" + Requests[i].ThetaStart + Environment.NewLine;
                text += "Конечный угол Theta: \t" + Requests[i].ThetaFinish + Environment.NewLine;
                text += "Угол наклона: \t" + Requests[i].PhiStart + Environment.NewLine;
                text += "Шаг сканирования: \t" + Requests[i].Delta + Environment.NewLine;
                text += "Траектория сканирования (XYZ, ZXY, YZX): \t" + Requests[i].SystemOfCoordinates + Environment.NewLine;

                text += "Расчёт поля в дальней зоне от апертуры: \t" + Requests[i].AntenaField + Environment.NewLine;
                text += "Расчёт поля в дальней зоне прошедшего через обтекатель: \t" + Requests[i].RadomeField + Environment.NewLine;
                text += "Расчёт поля в дальней зоне отражённого от обтекателя: \t" + Requests[i].ReflactedField + Environment.NewLine;
                text += "Расчёт поля в дальней зоне прошедшего через обтекатель с учётом переотражения от стенок: \t" + Requests[i].ReflactedItField + Environment.NewLine;
                text += "Включение в модель: \t" + Requests[i].Include + Environment.NewLine;
                text += "Тип поля: \t" + Requests[i].FarFieldType + Environment.NewLine;
                text += "В направлении сканирования: \t" + Requests[i].Direction + "\t" + Requests[i].BodyAngle + "\t" + Requests[i].BodyAngleStep + "\t" + Environment.NewLine;
                text += "////" + Environment.NewLine;
            }
            text += Environment.NewLine;
            text += "Источники:" + Environment.NewLine;

            if (SourceTemplate2.Sources != null)
            {
                text += "Название: \t" + SourceTemplate2.Lable + Environment.NewLine;
                text += "Распределние токов: \t" + SourceTemplate2.Distribution + Environment.NewLine;
                text += "Разностный канал: \t" + SourceTemplate2.DifferenceChanel + Environment.NewLine;
                text += "Ось разностного канала: \t" + SourceTemplate2.DifferenceAxis + Environment.NewLine;

                text += "Поляризация:";
                foreach (var item in SourceTemplate2.Polarization)
                {
                    text += "\t" + item.Key + "\t" + item.Value;
                }
                text += Environment.NewLine;

                text += "Электрические токи на поверхности апертуры: \t\n";// + SourceTemplate2.I[0].X.Real + "\t" + SourceTemplate2.I[0].X.Imaginary + "\t" + SourceTemplate2.I[0].Y.Real + "\t" + SourceTemplate2.I[0].Y.Imaginary + "\t" + SourceTemplate2.I[0].Z.Real + "\t" + SourceTemplate2.I[0].Z.Imaginary + Environment.NewLine;
                text += "Магнитные токи на поверхности апертуры: \t\n";// + SourceTemplate2.M[0].X.Real + "\t" + SourceTemplate2.M[0].X.Imaginary + "\t" + SourceTemplate2.M[0].Y.Real + "\t" + SourceTemplate2.M[0].Y.Imaginary + "\t" + SourceTemplate2.M[0].Z.Real + "\t" + SourceTemplate2.M[0].Z.Imaginary + Environment.NewLine;
                text += "Активировать сканирование: \t" + SourceTemplate2.Scanning + Environment.NewLine;
                text += "Параметры электоронного сканирования по Theta: \t" + SourceTemplate2.ThetaScanEStart + "\t" + SourceTemplate2.ThetaScanEFinish + "\t" + SourceTemplate2.ThetaScanEStep + Environment.NewLine;
                text += "Параметры электоронного сканирования по Phi: \t" + SourceTemplate2.PhiScanEStart + "\t" + SourceTemplate2.PhiScanEFinish + "\t" + SourceTemplate2.PhiScanEStep + Environment.NewLine;

                text += "Система координат при сканировании: \t" + SourceTemplate2.SystemOfCoordinatesScan + Environment.NewLine;
                text += "Включить в модель: \t" + SourceTemplate2.Axis1Include + "\t" + SourceTemplate2.Axis2Include + Environment.NewLine;

                text += "Ось 1, точка 1: \t" + SourceTemplate2.Axis1x1 + "\t" + SourceTemplate2.Axis1y1 + "\t" + SourceTemplate2.Axis1z1 + Environment.NewLine;
                text += "Ось 1, точка 2: \t" + SourceTemplate2.Axis1x2 + "\t" + SourceTemplate2.Axis1y2 + "\t" + SourceTemplate2.Axis1z2 + Environment.NewLine;

                text += "Ось 2, точка 1: \t" + SourceTemplate2.Axis2x1 + "\t" + SourceTemplate2.Axis2y1 + "\t" + SourceTemplate2.Axis2z1 + Environment.NewLine;
                text += "Ось 2, точка 2: \t" + SourceTemplate2.Axis2x2 + "\t" + SourceTemplate2.Axis2y2 + "\t" + SourceTemplate2.Axis2z2 + Environment.NewLine;

                text += "Угол Phi механиxанического сканирования: \t" + SourceTemplate2.PhiScanMStart + "\t" + SourceTemplate2.PhiScanMFinish + "\t" + SourceTemplate2.PhiScanMStep + "\t" + Environment.NewLine;
                text += "Угол Theta механиxанического сканирования: \t" + SourceTemplate2.ThetaScanMStart + "\t" + SourceTemplate2.ThetaScanMFinish + "\t" + SourceTemplate2.ThetaScanMStep + "\t" + Environment.NewLine;
                text += "////" + Environment.NewLine;
            }
            text += Environment.NewLine;
            string currentsFile = "";

            string ct = Form1.Instance.treeViewConfiguration.Nodes.Find("NearField", false).ElementAt(0).Text;
            string[] arr = ct.Split('[');
            if (arr.Length > 1)
            {
                currentsFile = arr[1].Remove(arr[1].IndexOf(']'));
            }

            text += "Путь: \t" + currentsFile + Environment.NewLine;
            text += "Использовать поля: \t" + Form1.Instance.toolStripMenuItemTurnOn.Checked + Environment.NewLine;
            text += "Конец раздела 1" + Environment.NewLine;
            text += "Раздел 2" + Environment.NewLine;
            text += "Структура стенки обтекателя:" + Environment.NewLine;
            for (int j = 0; j < RadomeLayers.Count; j++)
            {
                text += "Название: \t" + RadomeLayers[j].Lable + Environment.NewLine;
                for (int i = 0; i < RadomeLayers[j].Count; i++)
                {
                    text += "Слой " + (i + 1) + Environment.NewLine;
                    text += "Диэлектрическая проницаемость: \t" + RadomeLayers[j].Eps(i).Real.ToString(Format1) + "\t" + RadomeLayers[j].Eps(i).Imaginary.ToString(Format1) + Environment.NewLine;
                    text += "Магнитная проницаемость: \t" + RadomeLayers[j].Mu(i).Real.ToString(Format1) + "\t" + RadomeLayers[j].Mu(i).Imaginary.ToString(Format1) + Environment.NewLine;
                    text += "Толщина слоя: \t" + RadomeLayers[j].Size(i).ToString(Format1) + Environment.NewLine;
                    text += "////" + Environment.NewLine;
                }
            }
            text += "Конец раздела 2" + Environment.NewLine;
            sw.Write(text);

            //
            // Выгрузка координат обтекателя
            //
            text = "Раздел 3" + Environment.NewLine;
            text += "Количество частей: \t " + RadomeComposition.Count + Environment.NewLine;

            sw.Write(text);


            for (int e = 0; e < RadomeComposition.Count; e++)
            {
                sw.WriteLine(RadomeComposition[e].Lable);
                sw.WriteLine(String.Format("Параметры части обтекателя (цвет, название стенки, параметр включения): \t{0}\t{1}\t{2}",
                    CreateRadomeForm.colorDic.FirstOrDefault(x => x.Value == RadomeComposition[e].Color).Key, RadomeComposition[e].Structure.Lable, RadomeComposition[e].Include));

                sw.WriteLine("Координаты части обтекателя: \t");

                RadomeElement rel = RadomeComposition[e];
                for (int j = 0; j < rel.ListX.Count; j++)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}", rel.ListX[j], rel.ListY[j], rel.ListZ[j]));
                }
                sw.WriteLine("Конец координат части обтекателя \t");

                sw.WriteLine("Индексы части обтекателя: \t");
                for (int j = 0; j < rel.ListI1.Count; j++)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}", rel.ListI1[j], rel.ListI2[j], rel.ListI3[j]));
                }
                sw.WriteLine("Конец индексов части обтекателя \t");

                sw.Write("Конец " + RadomeComposition[e].Lable + Environment.NewLine);
            }
            text = "Конец раздела 3" + Environment.NewLine;
            sw.Write(text);

            //
            // Выгрузка координат антенны
            //
            text = "Раздел 4" + Environment.NewLine;
            text += "Координаты антенны: \t" + Environment.NewLine;
            sw.Write(text);

            if (Antenna != null)
            {
                for (int m = 0; m < Antenna.Count; m++)
                {
                    text = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", Antenna[m].Center.X, Antenna[m].Center.Y, Antenna[m].Center.Z, Antenna[m].Area, Antenna[m].Norma.X, Antenna[m].Norma.Y, Antenna[m].Norma.Z);// antennaPoints[0][m] + "\t" + antennaPoints[1][m] + "\t" + antennaPoints[2][m] + Environment.NewLine;
                    sw.WriteLine(text);
                }
            }

            text = "Конец координат антенны" + Environment.NewLine;
            sw.Write(text);


            text = "Конец раздела 4" + Environment.NewLine;
            sw.Write(text);

            text = "Раздел 5" + Environment.NewLine;
            sw.Write(text);
            if (SourceTemplate2.Distribution == "Загрузить из файла")
            {
                text = "Загруженные токи на апертуре антенны" + Environment.NewLine;
                sw.Write(text);

                sw.WriteLine(SourceTemplate2.I.Length);
                for (int i = 0; i < SourceTemplate2.I.Length; i++)
                {
                    text = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}",
                        SourceTemplate2.I[i].X.Real, SourceTemplate2.I[i].X.Imaginary,
                        SourceTemplate2.I[i].Y.Real, SourceTemplate2.I[i].Y.Imaginary,
                        SourceTemplate2.I[i].Z.Real, SourceTemplate2.I[i].Z.Imaginary,
                        SourceTemplate2.M[i].X.Real, SourceTemplate2.M[i].X.Imaginary,
                        SourceTemplate2.M[i].Y.Real, SourceTemplate2.M[i].Y.Imaginary,
                        SourceTemplate2.M[i].Z.Real, SourceTemplate2.M[i].Z.Imaginary);
                    sw.WriteLine(text);
                }
            }
            text = "Конец раздела 5" + Environment.NewLine;
            sw.Write(text);

            text = "*************************************************************************" + Environment.NewLine;
            text += "**************************КОНЕЦ ПРОТОКОЛА РАСЧЕТА************************" + Environment.NewLine;
            text += "*************************************************************************" + Environment.NewLine;

            sw.Write(text);
            sw.Close();
            Form1.Instance.Text = saveFileName;
            ProjectAdress = saveFileName;
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
        }
        private void NearFieledSaving(String adress, NearFieldC radomeFieled)
        {
            StreamWriter sw = new StreamWriter(adress);

            int numb = radomeFieled.Count;

            for (int i = 0; i < numb; i++)
            {
                NearFieldElement nfe = radomeFieled[i];
                double ExRe = nfe.E.X.Real;
                double ExIm = nfe.E.X.Imaginary;
                double EyRe = nfe.E.Y.Real;
                double EyIm = nfe.E.Y.Imaginary;
                double EzRe = nfe.E.Z.Real;
                double EzIm = nfe.E.Z.Imaginary;

                double HxRe = nfe.H.X.Real;
                double HxIm = nfe.H.X.Imaginary;
                double HyRe = nfe.H.Y.Real;
                double HyIm = nfe.H.Y.Imaginary;
                double HzRe = nfe.H.Z.Real;
                double HzIm = nfe.H.Z.Imaginary;

                double px = nfe.Place.X;
                double py = nfe.Place.Y;
                double pz = nfe.Place.Z;


                sw.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}", ExRe, ExIm, EyRe, EyIm, EzRe, EzIm, HxRe, HxIm, HyRe, HyIm, HzRe, HzIm, px, py, pz));
            }

            sw.Close();
            Form1.Instance.textBox1.Text += "Токи сохранены в файле " + adress + "." + Environment.NewLine;
        }
        #endregion


        private void NearFieldCalculation(double frequency)
        {
            if (RadomeNearField == null)
            {
                Form1.Instance.textBox1.AppendText(NowTime + " - Расчитываю поле на внутренней стороне стенки обтекателя" + Environment.NewLine);

                RadomeNearField = NearFieldC.SurfaceCurToGeometryCpp(Antenna, RadomeComposition, frequency, 1f, Proc);

                if (SaveField)
                {
                    string projectName = Path.GetFileNameWithoutExtension(ProjectAdress);
                    string saveAdress = Path.Combine(Path.GetDirectoryName(ProjectAdress), projectName + "_currents.txt");
                    NearFieledSaving(saveAdress, RadomeNearField);
                }
            }
        }


        public void ShowMeshSize()
        {
            double maxfr = 0;
            if (Frequencies.Count != 0)
            {
                foreach (var f in Frequencies)
                {
                    if (f > maxfr)
                    {
                        maxfr = f;
                    }
                }
            }
            double maxSize = 0;
            for (int i = 0; i < RadomeComposition.Count; i++)
            {

                double current = RadomeComposition[i].MeshSize * 1f;

                if (current > maxSize)
                {
                    maxSize = current;
                }
            }

            double K_0 = 2 * CV.pi * maxfr / CV.c_0;      // волновое число 2pi/lambda
            double optimaL = meshSizeParam / K_0;

            if (maxSize < optimaL)
            {
                Form1.Instance.textBox1.AppendText("Размер элемента геометрии обтекателя " + maxSize + " ( < " + optimaL + ")" + Environment.NewLine);
            }
            else
            {
                Form1.Instance.textBox1.AppendText("Превышен максимальный размер элемента геометрии обтекателя (" + maxSize + " > " + optimaL + ")" + Environment.NewLine);
            }
        }


        private void FullTreeViewResults()
        {
            Form1.Instance.treeViewResults.BeginUpdate();
            for (int i = 0; i < solutions.Count; i++)
            {

                TreeNode[] children = new TreeNode[solutions[i].CountRequest];

                int j = 0;
                if (solutions[i].ffantenna != null)
                {
                    children[j] = new TreeNode("Поле от апертуры");
                    j++;
                }
                if (solutions[i].ffradome != null)
                {
                    children[j] = new TreeNode("Поле от апертуры через обтекатель");
                    j++;
                }
                if (solutions[i].ffreflactionWithoutTransition != null)
                {
                    children[j] = new TreeNode("Отражённое поле (без прохождения)");
                    j++;
                }
                if (solutions[i].ffradomeAndReflactionWithoutTransSum != null && solutions[i].ffradome != null)
                {
                    children[j] = new TreeNode("Сумма поля через обтекатель и отражённого поля (без прохождения)");
                    j++;
                }
                if (solutions[i].ffreflactionWithTransition != null)
                {
                    children[j] = new TreeNode("Отражённое поле");
                    j++;
                }
                if (solutions[i].ffradomeAndReflactionWithTransSum != null)
                {
                    children[j] = new TreeNode("Сумма поля через обтекатель и отражённого поля");
                    j++;
                }

                TreeNode tn = new TreeNode(solutions[i].Name, children);

                Form1.Instance.treeViewResults.Nodes.Add(tn);
            }
            Form1.Instance.treeViewResults.ExpandAll();
            Form1.Instance.treeViewResults.EndUpdate();
        }

        public unsafe FarFieldC ConnectFafField(string name)
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                if (solutions[i].FFantenaName == name)
                {
                    return solutions[i].ffantenna;
                }
                if (solutions[i].FFradomeName == name)
                {
                    return solutions[i].ffradome;
                }
                if (solutions[i].FFreflactionWithoutTransitionName == name)
                {
                    return solutions[i].ffreflactionWithoutTransition;
                }
                if (solutions[i].FFradomeAndReflactionWithoutTransSumName == name)
                {
                    return solutions[i].ffradomeAndReflactionWithoutTransSum;
                }
                if (solutions[i].FFreflactionWithTransSumName == name)
                {
                    return solutions[i].ffreflactionWithTransition;
                }
                if (solutions[i].FFradomeAndReflactionWithTransSumName == name)
                {
                    return solutions[i].ffradomeAndReflactionWithTransSum;
                }
            }
            return null;
        }   //SolutionExplorer
        public unsafe double FindMaxNormaValue(FarFieldC ffa)
        {
            double max = ffa[0].Etotal;
            for (int i = 1; i < ffa.Count; i++)
            {
                if (ffa[i].Etotal > max)
                {
                    max = ffa[i].Etotal;
                }
            }
            return max;
        }

        public Point3D RotateElement(Point3D p, double angle, double axisx1, double axisy1, double axisz1, double axisx2, double axisy2, double axisz2)
        {
            float rangle = (float)(angle / 180 * CV.pi);
            
            SharpDX.Vector3 rotAxis = new SharpDX.Vector3((float)(axisx2 - axisx1), (float)(axisy2 - axisy1), (float)(axisz2 - axisz1));
            SharpDX.Matrix m = SharpDX.Matrix.RotationAxis(rotAxis, rangle);
            
            SharpDX.Vector3 shiftedCoord = new SharpDX.Vector3((float)(p.X - axisx1), (float)(p.Y - axisy1), (float)(p.Z - axisz1));
            SharpDX.Vector3 rotCoord = SharpDX.Vector3.TransformCoordinate(shiftedCoord, m);


            return new Point3D((rotCoord.X + axisx1), (rotCoord.Y + axisy1), (rotCoord.Z + axisz1));
            
        }
    }
}
