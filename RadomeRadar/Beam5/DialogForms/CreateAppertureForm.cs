using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = SharpDX.Color;

namespace Apparat
{
    public partial class CreateApertureForm : Form
    {
        const string Format1 = "0.#####";
        string initialTitle = "Апертура ";
        public const double c_0 = 299792458.00017589410202102027464;        
        public const double Z_0 = 120 * Math.PI;                            // sqrt(Mu_0/E_0) = 120pi Ом    impedance of free space   волновое сопротивление свободного пространства
        //Параметры по умолчанию
        //public static CVector I = new CVector(1, 0, 0); //-0.1598934596, 0, 0.3663091297
        //public static CVector M = new CVector(0, Z_0, 0); //0, 0.4912473491, 0]
        public static int PolarizationType = 0;         // эл.ток направлен: 0 - вдоль длинной стороны, 1 - вдоль короткой, 2 - для круговой поляризации, 3 - пользовательский тип
        public static bool Difference = true;
        public static string Axis = "Плоскость XZ";        
        public static string DistributionType = "Косинус на пьедестале";//"Постоянное поле";        
        public static int Scanning = 0;
        public static int SysOfCoord = 0;        
        public static double scanEPhiStart = 0;
        public static double scanEPhiFinish = 0;
        public static double scanEPhiStep = 1;
        public static double scanEThetaStart = 0;
        public static double scanEThetaFinish = 0;
        public static double scanEThetaStep = 1;
                
        public static bool  includeAxis1 = false;
        public static double axis1x1 = 0;
        public static double axis1y1 = 0;
        public static double axis1z1 = 0;
        public static double axis1x2 = 0;
        public static double axis1y2 = 0;
        public static double axis1z2 = 1;

        public static bool includeAxis2 = false;
        public static double axis2x1 = 0;
        public static double axis2y1 = 0;
        public static double axis2z1 = 0;
        public static double axis2x2 = 1;
        public static double axis2y2 = 0;
        public static double axis2z2 = 0;

        public static double scanMPhiStart = 0;     //вращение вокруг оси 2
        public static double scanMPhiFinish = 0;
        public static double scanMPhiStep = 1;
        public static double scanMThetaStart = 0;   //вращение вокруг оси 1
        public static double scanMThetaFinish = 0;
        public static double scanMThetaStep = 1;

        //static Dictionary<string, bool[]> polarization = new Dictionary<string, bool[]>() { { "Поляризация А", new bool[4] }, { "Поляризация Б", new bool[4] }, { "Круговая поляризация", new bool[4] }, { "Пользовательская", new bool[4] } };
        

        //Поля
        Form1 parent;
        string templateLable = "";
          
        bool reducting = false;
        bool loading = false;

        public CVector[] I = null;
        public CVector[] M = null;

        //public CVector[] icurInfo = null;
        //public CVector[] mcurInfo = null;        

        //Конструкторы
        //public CreateApertureForm(Form1 ParentForm)
        //{
        //    InitializeComponent();            
        //    parent = ParentForm;
        //    LoadParameters();
        //    templateLable = textBoxApertureTitle.Text;
        //    if (Scanning == 1)
        //    {
        //        ShowScanningPoints(templateLable);    
        //    }            
        //    Show();
        //}
        public CreateApertureForm(Form1 ParentForm, bool openNew)
        {
            InitializeComponent();

            // Не получается подписаться на ConvertToDouble и RefreshScanningRegion одновременно через форму
            textBoxScanPhiStart.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxScanPhiFinish.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxScanPhiStep.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxScanThetaStart.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxScanThetaFinish.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxScanThetaStep.TextChanged += new System.EventHandler(RefreshScanningRegion);

            textBoxMAngle1Start.TextChanged += new System.EventHandler(RefreshScanningRegion);            
            textBoxMAngle1Finish.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxMAngle1Step.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxMAngle2Start.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxMAngle2Finish.TextChanged += new System.EventHandler(RefreshScanningRegion);
            textBoxMAngle2Step.TextChanged += new System.EventHandler(RefreshScanningRegion);


            parent = ParentForm;
            if (openNew)
            {
                LoadParametersFromByDefault();
            }
            else
            {
                reducting = true;
                LoadParametersFromSourceTemplate();
            }            

            templateLable = SourceTemplate2.Lable;
            if (comboBoxScanning.SelectedIndex == 1)
            {
                ShowEScanningPoints(templateLable);    
            }
            if (comboBoxScanning.SelectedIndex == 2)
            {
                ShowMScanningPoints(templateLable);    
            }
            
            Show();
        }

        private void ShowMScanningPoints(string templateLable)
        {
            if (CheckConvertToDoubleAll())
            {
                double axis1x1 = Convert.ToDouble(textBoxRotAxis1X1.Text);
                double axis1y1 = Convert.ToDouble(textBoxRotAxis1Y1.Text);
                double axis1z1 = Convert.ToDouble(textBoxRotAxis1Z1.Text);

                double axis1x2 = Convert.ToDouble(textBoxRotAxis1X2.Text);
                double axis1y2 = Convert.ToDouble(textBoxRotAxis1Y2.Text);
                double axis1z2 = Convert.ToDouble(textBoxRotAxis1Z2.Text);

                double axis2x1 = Convert.ToDouble(textBoxRotAxis2X1.Text);
                double axis2y1 = Convert.ToDouble(textBoxRotAxis2Y1.Text);
                double axis2z1 = Convert.ToDouble(textBoxRotAxis2Z1.Text);

                double axis2x2 = Convert.ToDouble(textBoxRotAxis2X2.Text);
                double axis2y2 = Convert.ToDouble(textBoxRotAxis2Y2.Text);
                double axis2z2 = Convert.ToDouble(textBoxRotAxis2Z2.Text);

                double scanMPhiStart = Convert.ToDouble(textBoxMAngle2Start.Text);
                double scanMPhiFinish = Convert.ToDouble(textBoxMAngle2Finish.Text);
                double scanMPhiStep = Convert.ToDouble(textBoxMAngle2Step.Text);

                double scanMThetaStart = Convert.ToDouble(textBoxMAngle1Start.Text);
                double scanMThetaFinish = Convert.ToDouble(textBoxMAngle1Finish.Text);
                double scanMThetaStep = Convert.ToDouble(textBoxMAngle1Step.Text);

                bool include1 = false;
                if (comboBoxAxis1.SelectedIndex == 1)
                {
                    include1 = true;
                }
                bool include2 = false;
                if (comboBoxAxis2.SelectedIndex == 1)
                {
                    include2 = true;
                }
                DVector n = Logic.Instance.Antenna.elements[0].Norma;
                Point3D center = Logic.Instance.Antenna.Center;


                if (scanMThetaStart <= scanMThetaFinish && scanMThetaStep != 0 || !include1)
                {
                    if (scanMPhiStart <= scanMPhiFinish && scanMPhiStep != 0 || !include2)
                    {
                        Color xCol = new Color(255, 0, 128);
                        Point3D p1a1 = new Point3D(axis1x1, axis1y1, axis1z1);
                        Point3D p2a1 = new Point3D(axis1x2, axis1y2, axis1z2);
                        Point3D p1a2 = new Point3D(axis2x1, axis2y1, axis2z1);
                        Point3D p2a2 = new Point3D(axis2x2, axis2y2, axis2z2);

                        parent.renderControl1.Draw(templateLable, center, n, include1, p1a1, p2a1, scanMThetaStart, scanMThetaFinish, scanMThetaStep, include2, p1a2, p2a2, scanMPhiStart, scanMPhiFinish, scanMPhiStep, xCol);
                    }
                    
                    
                }
                else
                {
                    RemoveScanningRegion(templateLable);
                }
            }
            else
            {
                RemoveScanningRegion(templateLable);
            }
        }
        private void ShowEScanningPoints(string lable)
        {
            if (CheckConvertToDoubleAll())
            {
                double startTheta = Convert.ToDouble(textBoxScanThetaStart.Text);
                double finishTheta = Convert.ToDouble(textBoxScanThetaFinish.Text);
                double stepTheta = Convert.ToDouble(textBoxScanThetaStep.Text);
                double startPhi = Convert.ToDouble(textBoxScanPhiStart.Text);
                double finishPhi = Convert.ToDouble(textBoxScanPhiFinish.Text);
                double stepPhi = Convert.ToDouble(textBoxScanPhiStep.Text);
                int sys = comboBoxSysOfCoordScan.SelectedIndex;

                if (startTheta <= finishTheta && startPhi <= finishPhi && stepTheta != 0 && stepPhi != 0)
                {
                    Color blueCol = new Color(30, 225, 225);
                    parent.renderControl1.Draw(lable, startTheta, finishTheta, stepTheta, startPhi, finishPhi, stepPhi, sys, blueCol);
                }
                else
                {
                    RemoveScanningRegion(lable);
                }
            }
            else
            {
                RemoveScanningRegion(lable);
            }
        }
      

        //События
        private void button2_Click(object sender, EventArgs e)
        {
            if (!reducting)
            {
                parent.renderControl1.removingscanRegion(templateLable);
                parent.renderControl1.removingAntenna();
            }
            else
            {
                parent.renderControl1.removingscanRegion(templateLable);
                //SourceTemplate st = Logic.Instance.Sources;
                if (SourceTemplate2.Scanning == 1)
                {
                    Logic.Instance.LoadEScanningPoints(SourceTemplate2.Lable);
                    //int blueCol = Color.FromArgb(30, 225, 225).ToArgb();
                    //parent.renderControl1.Draw(SourceTemplate2.Lable, SourceTemplate2.ThetaScanEStart, SourceTemplate2.ThetaScanEFinish, SourceTemplate2.ThetaScanEStep, SourceTemplate2.PhiScanEStart, SourceTemplate2.PhiScanEFinish, SourceTemplate2.PhiScanEStep, SourceTemplate2.SystemOfCoordinatesScan, blueCol);
                }
                if (SourceTemplate2.Scanning == 2)
                {
                    Logic.Instance.LoadMScanningPoints(SourceTemplate2.Lable);
                    //int xCol = Color.FromArgb(30, 225, 225).ToArgb();
                    //parent.renderControl1.Draw(SourceTemplate2.Lable, SourceTemplate2.ThetaScanMStart, SourceTemplate2.ThetaScanMFinish, SourceTemplate2.ThetaScanMStep, SourceTemplate2.PhiScanMStart, SourceTemplate2.PhiScanMFinish, SourceTemplate2.PhiScanMStep, 0, xCol);
                }
            }           
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckConvertToDoubleAll())
            {
                if (!CheckApertureNameMatching(textBoxApertureTitle.Name))
                {
                    if (CheckEnteredValues())
                    {
                        SaveCurrentParameters();
                        SourceTemplate2.SetStaticSourceParameters(this);
                        SourceTemplate2.CreateSources();
                        if (reducting)
                        {                            
                            parent.ChangeSource();
                        }
                        else
                        {
                            parent.CleareSource();
                            //parent.AddSource(template);
                        }
                        Close();
                    }                   
                }
                else
                {
                    textBoxApertureTitle.BackColor = System.Drawing.Color.Red;
                }
            }            
        }    
        private bool CheckEnteredValues()
        {
            bool answer = true;
            
            if (comboBoxScanning.SelectedIndex == 1)
            {
                double thetastart = Convert.ToDouble(textBoxScanThetaStart.Text);
                double thetaFinish = Convert.ToDouble(textBoxScanThetaFinish.Text);
                double step = Convert.ToDouble(textBoxScanThetaStep.Text);
                
                if (thetastart > thetaFinish)
                {
                    answer = false;
                }
                if (step == 0)
                {
                    answer = false;
                }
            }
            
            return answer;
        }
        private void radioButtonChannel2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonChannel2.Checked)
            {
                comboBoxDiffAxis.Enabled = true;
            }
            else
            {
                comboBoxDiffAxis.Enabled = false;
            }
        }

        public void RefreshScanningRegion(object sender, EventArgs e)
        {
            if (!loading)
            {
                if (comboBoxScanning.SelectedIndex == 1)
                {
                    ShowEScanningPoints(templateLable);    
                }
                else if (comboBoxScanning.SelectedIndex == 2)
                {
                    ShowMScanningPoints(templateLable);    
                }
                
            }            
        }
        
        private void RemoveScanningRegion(string lable)
        {
            parent.renderControl1.removingscanRegion(lable);
        }
        private void onlyNumbers(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45 && number != 46) // цифры, клавиша BackSpace
            {
                e.Handled = true;
            }
            if (!(number != 46))
            {
                e.KeyChar = ',';
            }
            if (number == 13)    //нажатие Enter
            {
                button1_Click(sender, e);
            }
        }
        private void CheckConvertToDouble(object sender, EventArgs e)
        {
            CheckConvertToDoubleAll();
        }     
        //методы
        private bool CheckConvertToDoubleAll()
        {
            bool convertable = true;
            TextBox[] list1 = new TextBox[] { textBoxAppertureIxRe, textBoxAppertureIxIm, textBoxAppertureIyRe, textBoxAppertureIyIm, textBoxAppertureIzRe, textBoxAppertureIzIm,
                textBoxAppertureMxRe, textBoxAppertureMxIm, textBoxAppertureMyRe, textBoxAppertureMyIm, textBoxAppertureMzRe, textBoxAppertureMzIm };

            TextBox[] list2 = new TextBox[] { textBoxScanThetaStart, textBoxScanThetaFinish, textBoxScanThetaStep, textBoxScanPhiStart, textBoxScanPhiFinish , textBoxScanPhiStep};

            TextBox[] list3 = new TextBox[] { textBoxMAngle2Start, textBoxMAngle2Finish, textBoxMAngle2Step, textBoxMAngle1Start, textBoxMAngle1Finish, textBoxMAngle1Step, 
                                                textBoxRotAxis1X1, textBoxRotAxis1X2, textBoxRotAxis1Y1, textBoxRotAxis1Y2, textBoxRotAxis1Z1, textBoxRotAxis1Z2, 
                                                    textBoxRotAxis2X1, textBoxRotAxis2X2, textBoxRotAxis2Y1, textBoxRotAxis2Y2, textBoxRotAxis2Z1, textBoxRotAxis2Z2};

            foreach (TextBox box in list1)
            {
                try
                {
                    double d = Convert.ToDouble(box.Text);
                    box.BackColor = SystemColors.Window;
                }
                catch (Exception)
                {
                    box.BackColor = System.Drawing.Color.Red;
                    convertable = false;                    
                }
            }

            if (comboBoxScanning.SelectedIndex == 1)
            {
                foreach (TextBox box in list2)
                {
                    try
                    {
                        double d = Convert.ToDouble(box.Text);                        
                        box.BackColor = SystemColors.Window;
                    }
                    catch (Exception)
                    {
                        box.BackColor = System.Drawing.Color.Red;
                        convertable = false;
                    }                    
                }
            }

            if (comboBoxScanning.SelectedIndex == 2)
            {
                foreach (TextBox box in list3)
                {
                    try
                    {
                        double d = Convert.ToDouble(box.Text);
                        box.BackColor = SystemColors.Window;
                    }
                    catch (Exception)
                    {
                        box.BackColor = System.Drawing.Color.Red;
                        convertable = false;
                    }
                }
            }
            return convertable;
        }
        private void LoadParametersFromByDefault()
        {
            loading = true;

            textBoxScanPhiStart.Text = Convert.ToString(scanEPhiStart);
            textBoxScanPhiFinish.Text = Convert.ToString(scanEPhiFinish);
            textBoxScanPhiStep.Text = Convert.ToString(scanEPhiStep);
            textBoxScanThetaStart.Text = Convert.ToString(scanEThetaStart);
            textBoxScanThetaFinish.Text = Convert.ToString(scanEThetaFinish);
            textBoxScanThetaStep.Text = Convert.ToString(scanEThetaStep);

            radioButtonChannel2.Checked = CreateApertureForm.Difference;
            comboBoxScanning.SelectedIndex = CreateApertureForm.Scanning;
            comboBoxSysOfCoordScan.SelectedIndex = CreateApertureForm.SysOfCoord;


            for (int i = 0; i < comboBoxDistribution.Items.Count; i++)
            {
                if (comboBoxDistribution.Items[i].ToString() == CreateApertureForm.DistributionType)
                {
                    comboBoxDistribution.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxDiffAxis.Items.Count; i++)
            {
                if (comboBoxDiffAxis.Items[i].ToString() == CreateApertureForm.Axis)
                {
                    comboBoxDiffAxis.SelectedIndex = i;
                    break;
                }
            }

            textBoxApertureTitle.Text = String.Format("{0} [{1}]", GenerateUniqueName(), Logic.Instance.Antenna.Count);

            if (CreateApertureForm.includeAxis1)
            {
                comboBoxAxis1.SelectedIndex = 1;
            }
            else
            {
                comboBoxAxis1.SelectedIndex = 0;
            }
            textBoxRotAxis1X1.Text = Convert.ToString(CreateApertureForm.axis1x1);
            textBoxRotAxis1Y1.Text = Convert.ToString(CreateApertureForm.axis1y1);
            textBoxRotAxis1Z1.Text = Convert.ToString(CreateApertureForm.axis1z1);

            textBoxRotAxis1X2.Text = Convert.ToString(CreateApertureForm.axis1x2);
            textBoxRotAxis1Y2.Text = Convert.ToString(CreateApertureForm.axis1y2);
            textBoxRotAxis1Z2.Text = Convert.ToString(CreateApertureForm.axis1z2);

            if (CreateApertureForm.includeAxis2)
            {
                comboBoxAxis2.SelectedIndex = 1;
            }
            else
            {
                comboBoxAxis2.SelectedIndex = 0;
            }
            textBoxRotAxis2X1.Text = Convert.ToString(CreateApertureForm.axis2x1);
            textBoxRotAxis2Y1.Text = Convert.ToString(CreateApertureForm.axis2y1);
            textBoxRotAxis2Z1.Text = Convert.ToString(CreateApertureForm.axis2z1);

            textBoxRotAxis2X2.Text = Convert.ToString(CreateApertureForm.axis2x2);
            textBoxRotAxis2Y2.Text = Convert.ToString(CreateApertureForm.axis2y2);
            textBoxRotAxis2Z2.Text = Convert.ToString(CreateApertureForm.axis2z2);

            textBoxMAngle2Start.Text = Convert.ToString(CreateApertureForm.scanMPhiStart);
            textBoxMAngle2Finish.Text = Convert.ToString(CreateApertureForm.scanMPhiFinish);
            textBoxMAngle2Step.Text = Convert.ToString(CreateApertureForm.scanMPhiStep);

            textBoxMAngle1Start.Text = Convert.ToString(CreateApertureForm.scanMThetaStart);
            textBoxMAngle1Finish.Text = Convert.ToString(CreateApertureForm.scanMThetaFinish);
            textBoxMAngle1Step.Text = Convert.ToString(CreateApertureForm.scanMThetaStep);

            checkedListBoxPolarization.Items.Clear();

            checkedListBoxPolarization.Items.Add("Поляризация А", true);
            checkedListBoxPolarization.Items.Add("Поляризация Б", false);
            checkedListBoxPolarization.Items.Add("Круговая поляризация А", false);
            checkedListBoxPolarization.Items.Add("Круговая поляризация Б", false);
            checkedListBoxPolarization.Items.Add("Пользовательская", false);

            loading = false;
            
            checkedListBoxPolarization.SelectedIndex = CreateApertureForm.PolarizationType;
        }
        private void LoadParametersFromSourceTemplate()
        {
            loading = true;

            textBoxScanPhiStart.Text = Convert.ToString(SourceTemplate2.PhiScanEStart);
            textBoxScanPhiFinish.Text = Convert.ToString(SourceTemplate2.PhiScanEFinish);
            textBoxScanPhiStep.Text = Convert.ToString(SourceTemplate2.PhiScanEStep);

            textBoxScanThetaStart.Text = Convert.ToString(SourceTemplate2.ThetaScanEStart);
            textBoxScanThetaFinish.Text = Convert.ToString(SourceTemplate2.ThetaScanEFinish);
            textBoxScanThetaStep.Text = Convert.ToString(SourceTemplate2.ThetaScanEStep);

            radioButtonChannel2.Checked = SourceTemplate2.DifferenceChanel;
            comboBoxScanning.SelectedIndex = SourceTemplate2.Scanning;
            comboBoxSysOfCoordScan.SelectedIndex = SourceTemplate2.SystemOfCoordinatesScan;

            for (int i = 0; i < comboBoxDistribution.Items.Count; i++)
            {
                if (comboBoxDistribution.Items[i].ToString() == SourceTemplate2.Distribution)
                {
                    comboBoxDistribution.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxDiffAxis.Items.Count; i++)
            {
                if (comboBoxDiffAxis.Items[i].ToString() == SourceTemplate2.DifferenceAxis)
                {
                    comboBoxDiffAxis.SelectedIndex = i;
                    break;
                }
            }

            textBoxApertureTitle.Text = SourceTemplate2.Lable;

            
            textBoxRotAxis1X1.Text = Convert.ToString(SourceTemplate2.Axis1x1);
            textBoxRotAxis1Y1.Text = Convert.ToString(SourceTemplate2.Axis1y1);
            textBoxRotAxis1Z1.Text = Convert.ToString(SourceTemplate2.Axis1z1);

            textBoxRotAxis1X2.Text = Convert.ToString(SourceTemplate2.Axis1x2);
            textBoxRotAxis1Y2.Text = Convert.ToString(SourceTemplate2.Axis1y2);
            textBoxRotAxis1Z2.Text = Convert.ToString(SourceTemplate2.Axis1z2);

            
            textBoxRotAxis2X1.Text = Convert.ToString(SourceTemplate2.Axis2x1);
            textBoxRotAxis2Y1.Text = Convert.ToString(SourceTemplate2.Axis2y1);
            textBoxRotAxis2Z1.Text = Convert.ToString(SourceTemplate2.Axis2z1);

            textBoxRotAxis2X2.Text = Convert.ToString(SourceTemplate2.Axis2x2);
            textBoxRotAxis2Y2.Text = Convert.ToString(SourceTemplate2.Axis2y2);
            textBoxRotAxis2Z2.Text = Convert.ToString(SourceTemplate2.Axis2z2);

            textBoxMAngle2Start.Text = Convert.ToString(SourceTemplate2.PhiScanMStart);
            textBoxMAngle2Finish.Text = Convert.ToString(SourceTemplate2.PhiScanMFinish);
            textBoxMAngle2Step.Text = Convert.ToString(SourceTemplate2.PhiScanMStep);

            textBoxMAngle1Start.Text = Convert.ToString(SourceTemplate2.ThetaScanMStart);
            textBoxMAngle1Finish.Text = Convert.ToString(SourceTemplate2.ThetaScanMFinish);
            textBoxMAngle1Step.Text = Convert.ToString(SourceTemplate2.ThetaScanMStep);

            if (comboBoxDistribution.SelectedIndex == 2)
            {
                I = SourceTemplate2.I;
                M = SourceTemplate2.M;

                checkedListBoxPolarization.Items.Clear();

                checkedListBoxPolarization.Items.Add("Поляризация А", SourceTemplate2.Polarization["Поляризация А"]);   
                checkedListBoxPolarization.Items.Add("Поляризация Б", SourceTemplate2.Polarization["Поляризация Б"]);   
            }
            else
            {
                checkedListBoxPolarization.Items.Clear();

                checkedListBoxPolarization.Items.Add("Поляризация А", SourceTemplate2.Polarization["Поляризация А"]);   
                checkedListBoxPolarization.Items.Add("Поляризация Б", SourceTemplate2.Polarization["Поляризация Б"]);   
                checkedListBoxPolarization.Items.Add("Круговая поляризация А", SourceTemplate2.Polarization["Круговая поляризация А"]);   
                checkedListBoxPolarization.Items.Add("Круговая поляризация Б", SourceTemplate2.Polarization["Круговая поляризация Б"]);   
                checkedListBoxPolarization.Items.Add("Пользовательская", SourceTemplate2.Polarization["Пользовательская"]);   
            }            

            if (SourceTemplate2.Axis1Include)
            {
                comboBoxAxis1.SelectedIndex = 1;
            }
            else
            {
                comboBoxAxis1.SelectedIndex = 0;

            }
            if (SourceTemplate2.Axis2Include)
            {
                comboBoxAxis2.SelectedIndex = 1;
            }
            else
            {
                comboBoxAxis2.SelectedIndex = 0;
            }
            
            loading = false;

            if (checkedListBoxPolarization.Items.Count > 1)
            {
                checkedListBoxPolarization.SelectedIndex = 0;                
            }
        }
        private string GenerateUniqueName()
        {
            int k = 0;
            string answer;   
            do
            {
                k++;                
                answer = initialTitle + k;                
            }
            while (CheckApertureNameMatching(answer));
            return answer;
        }

        private bool CheckApertureNameMatching(string name)
        {
            TreeNode[] SearceResult = parent.treeViewConfiguration.Nodes.Find("Source", false);
            TreeNode apertureParentNode = SearceResult[0];
            
            bool match = false;

            foreach (TreeNode node in apertureParentNode.Nodes)
            {
                if (node.Text == name)
                {
                    match = true;
                    break;
                }
            }            
            return match;
        }
        
        private void SaveCurrentParameters()
        {
            Complex Ix = new Complex(Convert.ToDouble(textBoxAppertureIxRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureIxIm.Text.Replace(".", ",")));
            Complex Iy = new Complex(Convert.ToDouble(textBoxAppertureIyRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureIyIm.Text.Replace(".", ",")));
            Complex Iz = new Complex(Convert.ToDouble(textBoxAppertureIzRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureIzIm.Text.Replace(".", ",")));

            Complex Mx = new Complex(Convert.ToDouble(textBoxAppertureMxRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureMxIm.Text.Replace(".", ",")));
            Complex My = new Complex(Convert.ToDouble(textBoxAppertureMyRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureMyIm.Text.Replace(".", ",")));
            Complex Mz = new Complex(Convert.ToDouble(textBoxAppertureMzRe.Text.Replace(".", ",")), Convert.ToDouble(textBoxAppertureMzIm.Text.Replace(".", ",")));

            if (comboBoxAxis1.SelectedIndex == 0)
            {
                includeAxis1 = false;
            }
            else
            {
                includeAxis1 = true;
            }

            if (comboBoxAxis2.SelectedIndex == 0)
            {
                includeAxis2 = false;
            }
            else
            {
                includeAxis2 = true;
            }

            CreateApertureForm.PolarizationType = checkedListBoxPolarization.SelectedIndex;
            CreateApertureForm.Difference = radioButtonChannel2.Checked;

            if (radioButtonChannel2.Checked)
            {
                CreateApertureForm.Axis = comboBoxDiffAxis.Items[comboBoxDiffAxis.SelectedIndex].ToString();
            }

            CreateApertureForm.DistributionType = comboBoxDistribution.Items[comboBoxDistribution.SelectedIndex].ToString();        
            CreateApertureForm.Scanning = comboBoxScanning.SelectedIndex;

            CreateApertureForm.scanEPhiStart = Convert.ToDouble(textBoxScanPhiStart.Text);
            CreateApertureForm.scanEPhiFinish = Convert.ToDouble(textBoxScanPhiFinish.Text);
            CreateApertureForm.scanEPhiStep = Convert.ToDouble(textBoxScanPhiStep.Text);
            CreateApertureForm.scanEThetaStart = Convert.ToDouble(textBoxScanThetaStart.Text);
            CreateApertureForm.scanEThetaFinish = Convert.ToDouble(textBoxScanThetaFinish.Text);
            CreateApertureForm.scanEThetaStep = Convert.ToDouble(textBoxScanThetaStep.Text);
            CreateApertureForm.SysOfCoord = comboBoxSysOfCoordScan.SelectedIndex;

            CreateApertureForm.axis1x1 = Convert.ToDouble(textBoxRotAxis1X1.Text);
            CreateApertureForm.axis1y1 = Convert.ToDouble(textBoxRotAxis1Y1.Text);
            CreateApertureForm.axis1z1 = Convert.ToDouble(textBoxRotAxis1Z1.Text);

            CreateApertureForm.axis1x2 = Convert.ToDouble(textBoxRotAxis1X2.Text);
            CreateApertureForm.axis1y2 = Convert.ToDouble(textBoxRotAxis1Y2.Text);
            CreateApertureForm.axis1z2 = Convert.ToDouble(textBoxRotAxis1Z2.Text);

            CreateApertureForm.axis2x1 = Convert.ToDouble(textBoxRotAxis2X1.Text);
            CreateApertureForm.axis2y1 = Convert.ToDouble(textBoxRotAxis2Y1.Text);
            CreateApertureForm.axis2z1 = Convert.ToDouble(textBoxRotAxis2Z1.Text);

            CreateApertureForm.axis2x2 = Convert.ToDouble(textBoxRotAxis2X2.Text);
            CreateApertureForm.axis2y2 = Convert.ToDouble(textBoxRotAxis2Y2.Text);
            CreateApertureForm.axis2z2 = Convert.ToDouble(textBoxRotAxis2Z2.Text);

            CreateApertureForm.scanMPhiStart = Convert.ToDouble(textBoxMAngle2Start.Text);
            CreateApertureForm.scanMPhiFinish = Convert.ToDouble(textBoxMAngle2Finish.Text);
            CreateApertureForm.scanMPhiStep = Convert.ToDouble(textBoxMAngle2Step.Text);

            CreateApertureForm.scanMThetaStart = Convert.ToDouble(textBoxMAngle1Start.Text);
            CreateApertureForm.scanMThetaFinish = Convert.ToDouble(textBoxMAngle1Finish.Text);
            CreateApertureForm.scanMThetaStep = Convert.ToDouble(textBoxMAngle1Step.Text);   
        }
        private void textBoxApertureTitle_TextChanged(object sender, EventArgs e)
        {
            textBoxApertureTitle.BackColor = SystemColors.Window;
        }

        private void comboBoxScanning_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                tabPageElectroScan.Parent = null;
                tabPageMechanicScan.Parent = null;
                RemoveScanningRegion(templateLable);
            }
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                tabPageElectroScan.Parent = tabControl1;
                tabPageMechanicScan.Parent = null;
                RefreshScanningRegion(sender, e);
            }
            if ((sender as ComboBox).SelectedIndex == 2)
            {
                tabPageMechanicScan.Parent = tabControl1;
                tabPageElectroScan.Parent = null;
                RefreshScanningRegion(sender, e);
            }
        }
        private void comboBoxDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                if(comboBoxDistribution.SelectedIndex == 0 || comboBoxDistribution.SelectedIndex == 1)
                {
                    //Dictionary<string, bool> polSaveDic = new Dictionary<string, bool>();
                    //for (int i = 0; i < checkedListBoxPolarization.Items.Count; i++)
                    //{
                    //    polSaveDic.Add(checkedListBoxPolarization.Items[i].ToString(), checkedListBoxPolarization.GetItemChecked(i));
                    //}
                    Logic.Instance.Antenna.GeneratePolarizationCurrents("Постоянное поле");

                    checkedListBoxPolarization.Items.Clear();
                    checkedListBoxPolarization.Items.AddRange(new string[] { "Поляризация А", "Поляризация Б", "Круговая поляризация А", "Круговая поляризация Б", "Пользовательская" });
                    checkedListBoxPolarization.SetItemChecked(0, true);
                    checkedListBoxPolarization.SelectedIndex = 0;

                    //int count = checkedListBoxPolarization.Items.Count;
                    //if (polSaveDic.Count < checkedListBoxPolarization.Items.Count)
                    //{
                    //    count = polSaveDic.Count;
                    //}
                    //for (int i = 0; i < count; i++)
                    //{
                    //    checkedListBoxPolarization.SetItemChecked(i, polSaveDic[checkedListBoxPolarization.Items[i].ToString()]);
                    //}

                    //EnableChangeableParameters(true);
                    
                }
                else if (comboBoxDistribution.SelectedIndex == 2)
                {
                    
                    //EnableChangeableParameters(false);
                    //Dictionary<string, bool> polSaveDic = new Dictionary<string, bool>();
                    //for (int i = 0; i < checkedListBoxPolarization.Items.Count; i++)
                    //{
                    //    polSaveDic.Add(checkedListBoxPolarization.Items[i].ToString(), checkedListBoxPolarization.GetItemChecked(i));
                    //}

                    //for (int i = 0; i < checkedListBoxPolarization.Items.Count; i++)
                    //{
                    //    checkedListBoxPolarization.SetItemChecked(i, polSaveDic[checkedListBoxPolarization.Items[i].ToString()]);
                    //}

                    try
                    {
                        CVector[] i = Logic.Instance.ReadCurrentFromFile(openFileDialog1, "Загрузить электрические эквивалентные токи");
                        int countI = i.Length;
                        CVector[] m = Logic.Instance.ReadCurrentFromFile(openFileDialog1, "Загрузить магнитные эквивалентные токи");
                        
                        int countM = m.Length;
                        for (int n = 0; n < countI; n++)
                        {
                            m[n] = Z_0 * m[n];
                        }
                        int countElements = Logic.Instance.Antenna.Count;

                        if (countI == countM && countI == countElements)
                        {                            
                            I = i;
                            M = m;

                            SourceTemplate2.I = I;
                            SourceTemplate2.M = M;
                            checkedListBoxPolarization.Items.Clear();
                            checkedListBoxPolarization.Items.AddRange(new string[] { "Поляризация А", "Поляризация Б" });
                            checkedListBoxPolarization.SetItemChecked(0, true);
                            checkedListBoxPolarization.SelectedIndex = 0;
                            Logic.Instance.Antenna.GeneratePolarizationCurrents("Загрузить из файла");
                        }
                        else
                        {
                            MessageBox.Show(String.Format("Количество элементов геометрии апертуры {0}, элементов электрического тока {1}, и магнитного тока {2} не совпадают", countElements, countI, countM));
                            comboBoxDistribution.SelectedIndex = 0;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Токи на апертуре не загружены \n");
                        comboBoxDistribution.SelectedIndex = 0;
                    }
                }
                
            }
        }

        private void EnableChangeableParameters(bool b)
        {
            TextBox[] list1 = new TextBox[] { textBoxAppertureIxRe, textBoxAppertureIxIm, textBoxAppertureIyRe, textBoxAppertureIyIm, textBoxAppertureIzRe, textBoxAppertureIzIm,
                textBoxAppertureMxRe, textBoxAppertureMxIm, textBoxAppertureMyRe, textBoxAppertureMyIm, textBoxAppertureMzRe, textBoxAppertureMzIm };

            foreach (var el in list1)
            {
                el.Enabled = b;
            }
        }
        
        private void Enter_Press(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (number == 13)    //нажатие Enter
            {
                button1_Click(sender, e);
            }
        }

        private void comboBoxPolarization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                loading = true;
                //string changedPolarizatioin = comboBoxPolarization.Items[comboBoxPolarization.SelectedIndex].ToString();
                //ChangePolarizationTo(changedPolarizatioin);
                loading = false;    
            }            
        }

        //private Tuple<CVector, CVector> GetPolarizationCurrents(string changedPolarizatioin)
        //{
        //    //polarization detection
        //    //var d = DictionaryLibrary.PolarizationNames;
        //    //int polarization = d.FirstOrDefault(x => x.Value == changedPolarizatioin).Key;

        //    //set currents
        //    CVector electricCurrent = null;
        //    CVector magneticCurrent = null;

        //    if (comboBoxDistribution.SelectedIndex != 2)
        //    {
        //        //ortogonal and parallel vectors detection
        //        double xlength = Logic.Instance.Antenna.XMax - Logic.Instance.Antenna.XMin;
        //        double ylength = Logic.Instance.Antenna.YMax - Logic.Instance.Antenna.YMin;
        //        double zlength = Logic.Instance.Antenna.ZMax - Logic.Instance.Antenna.ZMin;

        //        DVector probeVector;
        //        if (xlength > ylength)
        //        {
        //            if (xlength > zlength)
        //            {
        //                probeVector = new DVector(1, 0, 0);
        //            }
        //            else
        //            {
        //                probeVector = new DVector(0, 0, 1);
        //            }
        //        }
        //        else
        //        {
        //            if (ylength > zlength)
        //            {
        //                probeVector = new DVector(0, 1, 0);
        //            }
        //            else
        //            {
        //                probeVector = new DVector(0, 0, 1);
        //            }
        //        }

        //        DVector normaVector = Logic.Instance.Antenna[0].Norma;
        //        DVector ortogonalVector = DVector.Cross(probeVector, normaVector);
        //        ortogonalVector.Normalize();
        //        DVector parallelVector = DVector.Cross(ortogonalVector, normaVector);
        //        parallelVector.Normalize();




        //        if (changedPolarizatioin == "Поляризация А")
        //        {
        //            electricCurrent = new CVector(new Complex(parallelVector.X, 0), new Complex(parallelVector.Y, 0), new Complex(parallelVector.Z, 0));
        //            magneticCurrent = Z_0 * new CVector(new Complex(ortogonalVector.X, 0), new Complex(ortogonalVector.Y, 0), new Complex(ortogonalVector.Z, 0));
        //        }
        //        else if (changedPolarizatioin == "Поляризация Б")
        //        {
        //            electricCurrent = new CVector(new Complex(-ortogonalVector.X, 0), new Complex(-ortogonalVector.Y, 0), new Complex(-ortogonalVector.Z, 0));
        //            magneticCurrent = Z_0 * new CVector(new Complex(parallelVector.X, 0), new Complex(parallelVector.Y, 0), new Complex(parallelVector.Z, 0));
        //        }
        //        else if (changedPolarizatioin == "Круговая поляризация А")
        //        {
        //            //!!!! запланировано, не работает
        //            electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //            magneticCurrent = Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //        }
        //        else if (changedPolarizatioin == "Круговая поляризация Б")
        //        {
        //            //!!!!запланировано, не работает
        //            electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //            magneticCurrent = Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //        }
        //        else if (changedPolarizatioin == "Пользовательская")
        //        {
        //            //!!!!запланировано, не работает
        //            electricCurrent = new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //            magneticCurrent = Z_0 * new CVector(new Complex(0, 0), new Complex(0, 0), new Complex(0, 0));
        //        }
        //    }
        //    else if (comboBoxDistribution.SelectedIndex == 2)
        //    {
        //        int count = this.I.Length;
        //        double max = this.I[0].Modulus;
        //        int maxIndex = 0;
        //        for (int i = 1; i < count; i++)
        //        {
        //            if (this.I[i].Modulus > max)
        //            {
        //                maxIndex = i;
        //                max = this.I[i].Modulus;
        //            }
        //        }


        //        if (changedPolarizatioin == "Поляризация А")
        //        {
        //            electricCurrent = this.I[maxIndex];
        //            magneticCurrent = this.M[maxIndex];
        //        }
        //        else if (changedPolarizatioin == "Поляризация Б")
        //        {
        //            electricCurrent = this.I[maxIndex];
        //            magneticCurrent = this.M[maxIndex];

        //            electricCurrent = Current.ChangeLoadedCurrentAnlorithm(electricCurrent, Logic.Instance.Antenna[maxIndex].Norma, 90);
        //            magneticCurrent = Current.ChangeLoadedCurrentAnlorithm(magneticCurrent, Logic.Instance.Antenna[maxIndex].Norma, 90);
        //        }
        //    }
        //    return new Tuple<CVector, CVector>(electricCurrent, magneticCurrent);
        //}

        private void SetTextBoxCurrentValues(CVector electricCurrent, CVector magneticCurrent)
        {
            if (electricCurrent != null && magneticCurrent != null)
            {
                textBoxAppertureIxRe.Text = electricCurrent.X.Real.ToString(Format1);
                textBoxAppertureIxIm.Text = electricCurrent.X.Imaginary.ToString(Format1);
                textBoxAppertureIyRe.Text = electricCurrent.Y.Real.ToString(Format1);
                textBoxAppertureIyIm.Text = electricCurrent.Y.Imaginary.ToString(Format1);
                textBoxAppertureIzRe.Text = electricCurrent.Z.Real.ToString(Format1);
                textBoxAppertureIzIm.Text = electricCurrent.Z.Imaginary.ToString(Format1);

                textBoxAppertureMxRe.Text = magneticCurrent.X.Real.ToString(Format1);
                textBoxAppertureMxIm.Text = magneticCurrent.X.Imaginary.ToString(Format1);
                textBoxAppertureMyRe.Text = magneticCurrent.Y.Real.ToString(Format1);
                textBoxAppertureMyIm.Text = magneticCurrent.Y.Imaginary.ToString(Format1);
                textBoxAppertureMzRe.Text = magneticCurrent.Z.Real.ToString(Format1);
                textBoxAppertureMzIm.Text = magneticCurrent.Z.Imaginary.ToString(Format1);                
            }
            else
            {
                textBoxAppertureIxRe.Text = "NAN";
                textBoxAppertureIxIm.Text = "NAN";
                textBoxAppertureIyRe.Text = "NAN";
                textBoxAppertureIyIm.Text = "NAN";
                textBoxAppertureIzRe.Text = "NAN";
                textBoxAppertureIzIm.Text = "NAN";

                textBoxAppertureMxRe.Text = "NAN";
                textBoxAppertureMxIm.Text = "NAN";
                textBoxAppertureMyRe.Text = "NAN";
                textBoxAppertureMyIm.Text = "NAN";
                textBoxAppertureMzRe.Text = "NAN";
                textBoxAppertureMzIm.Text = "NAN";
            }
        }

        private void CheckedListBoxPolarization_SelectedIndexChanged(object sender, EventArgs e)
        {
            string polarization = checkedListBoxPolarization.SelectedItem.ToString();

            int polIndex = DictionaryLibrary.PolarizationNames.FirstOrDefault(x => x.Value == polarization).Key;
            SetTextBoxCurrentValues(SourceTemplate2.I0[polIndex], SourceTemplate2.M0[polIndex]);

            if (polarization == "Пользовательская")
            {
                EnableChangeableParameters(true);
            }
            else
            {
                EnableChangeableParameters(false);
            }

            //string name = clb.Items[clb.SelectedIndex].ToString();
            //PreviousSelectedIndex = clb.SelectedIndex;
            //var cur = GetPolarizationCurrents(name);
            //
        }

        private void comboBoxAxis2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                groupBox6.Enabled = true;
                groupBox10.Enabled = true;
            }
            else
            {
                groupBox6.Enabled = false;
                groupBox10.Enabled = false;
            }
            if (!loading)
            {                
                ShowMScanningPoints(templateLable);    
            }            
        }
        private void comboBoxAxis1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                groupBox9.Enabled = true;
                groupBox11.Enabled = true;
            }
            else
            {
                groupBox9.Enabled = false;
                groupBox11.Enabled = false;
            }

            if (!loading)
            {                
                ShowMScanningPoints(templateLable);
            }            
        }       
        //void SaveCurrent_0(string pol)
        //{
        //    int index_p = DictionaryLibrary.PolarizationNames.FirstOrDefault(x => x.Value == pol).Key;

        //    icurInfo[index_p] = new CVector(new Complex(Convert.ToDouble(textBoxAppertureIxRe), Convert.ToDouble(textBoxAppertureIxIm)), new Complex(Convert.ToDouble(textBoxAppertureIyRe), Convert.ToDouble(textBoxAppertureIyIm)), new Complex(Convert.ToDouble(textBoxAppertureIzRe), Convert.ToDouble(textBoxAppertureIzIm)));
        //}
        
    }


}
