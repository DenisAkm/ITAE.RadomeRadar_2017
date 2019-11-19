using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apparat
{
    public partial class FarFieldRequestForm : Form
    {
        public static double ThetaStart = 0;        
        public static double ThetaFinish = 180;
        public static double PhiStart = 0;        
        public static double Delta = 0.1;
        public static string Direction = "Азимутальная (XY)";
        public static double BodyAngle = 5;
        public static double BodyAngleStep = 0.01;
        public static int SystemOfCoordinates = 0;
        public const string initialTitle = "Поле дальней зоны ";
        public static bool AntenaField = true;
        public static bool RadomeField = true;
        public static bool ReflactedField = false;
        public static bool ReflactedItField = false;
        public static int FarFieldType = 0;
        bool reducting = false;
        bool loading = false;
        
        
        string templateLable = "";
        string Title { get; set; }
        FarFieldRequestTemplate parentTemplate;        
        Form1 parentForm;

        public FarFieldRequestForm(Form1 ParentForm)
        {
            InitializeComponent();
            parentForm = ParentForm;            
            LoadDefaultParameters();
            templateLable = textBoxTitle.Text;
            if (FarFieldType == 0)
            {
                DrawFarFieldArc(templateLable);
            }
            
            
            Show();
        }
        public FarFieldRequestForm(Form1 ParentForm, FarFieldRequestTemplate ParentTemplate)
        {
            InitializeComponent();
            parentForm = ParentForm;                        
            parentTemplate = ParentTemplate;
            LoadParameters(ParentTemplate);
            
            templateLable = ParentTemplate.Lable;
            reducting = true;
            
            Show();            
        }
        private void button1_Click(object sender, EventArgs e)
        {            
            if (CheckAllEnteredValues())
            {
                if (!FarFieldRequestNameMatching(textBoxTitle.Text)|| reducting)
                {
                    if (radioButtonType1.Checked)
                    {
                        parentForm.renderControl1.removingArc(templateLable);
                        DrawFarFieldArc(Title);
                    }
                    
                    SaveCurrentParameters();
                    
                    FarFieldRequestTemplate fft = new FarFieldRequestTemplate(this);                    
                    if (reducting)
                    {
                        parentForm.ChangeFarFieldRequest(parentTemplate.Lable, fft);
                    }
                    else
                    {
                        parentForm.AddFarFieldRequest(fft);
                    }
                    Close();
                }
                else
                {
                    textBoxTitle.BackColor = Color.Red;
                }
            }           
        }
        private void SaveCurrentParameters()
        {
            ThetaFinish = Convert.ToDouble(textBoxThetaFinish.Text.Replace(".", ","));
            ThetaStart = Convert.ToDouble(textBoxThetaStart.Text.Replace(".", ","));
            PhiStart = Convert.ToDouble(textBoxPhiStart.Text.Replace(".", ","));
            Delta = Convert.ToDouble(textBoxStep.Text.Replace(".", ","));
            
            Direction = comboBoxDirection.Text;
            BodyAngle = Convert.ToDouble(textBoxBodyAngle.Text);
            BodyAngleStep = Convert.ToDouble(textBoxBodyAngleStep.Text.Replace(".", ","));

            SystemOfCoordinates = comboBoxFarFieldSystem.SelectedIndex;

            if (radioButtonType1.Checked)
            {
                FarFieldType = 0;    
            }
            else
            {
                FarFieldType = 1;
            }
            
            AntenaField = checkBoxSolutionA.Checked;
            RadomeField = checkBoxSolutionR.Checked;
            ReflactedField = checkBoxSolutionRef.Checked;
            ReflactedItField = checkBoxSolutionRefIteration.Checked;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!reducting)
            {
                parentForm.renderControl1.removingArc(templateLable);    
            }
            else
            {
                FarFieldRequestTemplate fft = Logic.Instance.Requests.Find(x => x.Lable == templateLable);
                if (fft.FarFieldType == 0)
                {
                    parentForm.renderControl1.removingArc(templateLable);                    
                    parentForm.renderControl1.Draw(fft.Lable, fft.ThetaStart, fft.ThetaFinish, fft.PhiStart, fft.Delta, fft.SystemOfCoordinates);    
                }
                else
                {
                    parentForm.renderControl1.removingArc(templateLable);                    
                }
            }
            
            
            Close();
        }
        private void LoadDefaultParameters()
        {
            loading = true;
            textBoxThetaFinish.Text = FarFieldRequestForm.ThetaFinish.ToString();
            textBoxThetaStart.Text = FarFieldRequestForm.ThetaStart.ToString();            
            textBoxPhiStart.Text = FarFieldRequestForm.PhiStart.ToString();
            textBoxStep.Text = FarFieldRequestForm.Delta.ToString();
            comboBoxDirection.SelectedIndex = comboBoxDirection.FindString(FarFieldRequestForm.Direction);
            textBoxBodyAngle.Text = FarFieldRequestForm.BodyAngle.ToString();
            textBoxBodyAngleStep.Text = FarFieldRequestForm.BodyAngleStep.ToString();
            comboBoxFarFieldSystem.SelectedIndex = FarFieldRequestForm.SystemOfCoordinates;
            checkBoxSolutionA.Checked = FarFieldRequestForm.AntenaField;
            checkBoxSolutionR.Checked = FarFieldRequestForm.RadomeField;
            checkBoxSolutionRef.Checked = FarFieldRequestForm.ReflactedField;            
            checkBoxSolutionRefIteration.Checked = FarFieldRequestForm.ReflactedItField;
            textBoxTitle.Text = GenerateUniqueName();
            Title = textBoxTitle.Text;
            loading = false;
            if (FarFieldType == 0)
            {
                radioButtonType1.Checked = true;
            }
            else
            {
                radioButtonType2.Checked = true;
            }
            
        }
        private void LoadParameters(FarFieldRequestTemplate template)
        {
            string format = "0.####";
            loading = true;
            textBoxThetaFinish.Text = Convert.ToString(template.ThetaFinish).Replace(",", ".");
            textBoxThetaStart.Text = Convert.ToString(template.ThetaStart).Replace(",", ".");
            textBoxPhiStart.Text = Convert.ToString(template.PhiStart).Replace(",", ".");
            textBoxStep.Text = Convert.ToString(template.Delta).Replace(",", ".");

            comboBoxDirection.SelectedIndex = comboBoxDirection.FindString(template.Direction);
            textBoxBodyAngle.Text = template.BodyAngle.ToString(format);
            textBoxBodyAngleStep.Text = template.BodyAngleStep.ToString(format);

            comboBoxFarFieldSystem.SelectedIndex = template.SystemOfCoordinates;

            checkBoxSolutionA.Checked = template.AntenaField;
            checkBoxSolutionR.Checked = template.RadomeField;
            checkBoxSolutionRef.Checked = template.ReflactedField;
            checkBoxSolutionRefIteration.Checked = template.ReflactedItField;

            textBoxTitle.Text = template.Lable;
            Title = textBoxTitle.Text;
            loading = false;

            if (template.FarFieldType == 0)
            {
                radioButtonType1.Checked = true;
            }
            else
            {
                radioButtonType2.Checked = true;
            }
        }
        private string GenerateUniqueName()
        {
            string answer = initialTitle;
            int k = 0;
            do
            {
                k++;
                answer = String.Concat(FarFieldRequestForm.initialTitle, k);
            } while (FarFieldRequestNameMatching(answer));
            return answer;
        }              
        public bool FarFieldRequestNameMatching(string title)
        {
            TreeNode[] SearceResult = parentForm.treeViewConfiguration.Nodes.Find("Request", false);
            TreeNode farFiledRequestParentNode = SearceResult[0];

            bool match = false;

            foreach (TreeNode node in farFiledRequestParentNode.Nodes)
            {
                if (node.Text == title)
                {
                    match = true;
                    break;
                }
            }
            return match;
        }
        void DrawFarFieldArc(string title)
        {
            if (!loading)
            {
                double thetaFinish = Convert.ToDouble(textBoxThetaFinish.Text.Replace(".", ","));
                double thetaStart = Convert.ToDouble(textBoxThetaStart.Text.Replace(".", ","));
                double phiStart = Convert.ToDouble(textBoxPhiStart.Text.Replace(".", ","));                
                int systemOfCoordinates = comboBoxFarFieldSystem.SelectedIndex;
                double delta = Convert.ToDouble(textBoxStep.Text.Replace(".", ","));
                parentForm.renderControl1.Draw(title, thetaStart, thetaFinish, phiStart, delta, systemOfCoordinates);
            }
        }
        private void RerenderArc_TextChanged(object sender, EventArgs e)
        {
            if (CheckAllEnteredValues())
            {
                DrawFarFieldArc(templateLable);
            }
            else
            {
                parentForm.renderControl1.removingArc(templateLable);
            }                
        }       
        private bool CheckConvertToDoubleAll()
        {
            bool convertable = true;
            TextBox[] list = new TextBox[] { textBoxThetaStart, textBoxThetaFinish, textBoxPhiStart, textBoxStep };

            foreach (TextBox box in list)
            {
                try
                {
                    double d = Convert.ToDouble(box.Text.Replace(".", ","));
                    box.BackColor = SystemColors.Window;
                }
                catch (Exception)
                {
                    box.BackColor = Color.Red;
                    convertable = false;
                }
            }
            return convertable;
        }
        private bool CheckAllEnteredValues()
        {
            bool answer = CheckConvertToDoubleAll();
            if (answer)
            {
                answer = Check_360();                
            }
            if (answer)
            {
                answer = CheckStartAndFinish();
            }
            if (answer)
            {
                answer = CheckStep();
            }

            return answer;
        }

        bool CheckStep()
        {
            bool answer = true;

            if (Convert.ToDouble(textBoxStep.Text.Replace(".", ",")) < 0.0001)
            {
                answer = false;
            }

            return answer;
        }

        bool CheckStartAndFinish()
        {
            bool answer = true;
            if (Convert.ToDouble(textBoxThetaFinish.Text.Replace(".", ",")) < Convert.ToDouble(textBoxThetaStart.Text.Replace(".", ",")))
            {
                answer = false;
            }
            return answer;
        }
        private bool Check_360()
        {
            bool check = true;

            double start = Convert.ToDouble(textBoxThetaStart.Text.Replace(".", ","));
            double finish = Convert.ToDouble(textBoxThetaFinish.Text.Replace(".", ","));

            if (start > finish )
            {
                check = false;
                
            }
            if (start > 360 || start < -360 || finish > 360 || finish < -360)
            {
                check = false;
            }
            if (!check)
            {
                parentForm.textBox1.Text += "Ошибка ввода диапазона расчёта" + Environment.NewLine;
            }
            
            return true;
        }
        private void onlyNumbers(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 45 && number != 46) // только цифры, клавиша BackSpace
            {
                e.Handled = true;
            }
            if (number == 46)   //нажатие точки
            {
                e.KeyChar = ',';
            }
            if (number == 13)    //нажатие Enter
            {
                button1_Click(sender, e);
            }
        }
        private void Check_360(object sender, EventArgs e)
        {            
            //if (!(Loading))
            //{
            //    try
            //    {
            //        Single numb = Convert.ToSingle((sender as TextBox).Text);
            //        if (numb < -360 || numb > 360)
            //        {
            //            (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //            textBox1.Text += "Ошибка ввода диапазона сканирования" + Environment.NewLine;
            //            renderControl1.removingArc();
            //            Error = true;
            //        }
            //        else
            //        {
            //            (sender as TextBox).ForeColor = Color.Black;
            //            StartAngle = Convert.ToSingle(textBoxStart.Text);
            //            FinishAngle = Convert.ToSingle(textBoxFinish.Text);
            //            if ((sender as TextBox).Name == textBoxStart.Name)
            //            {
            //                if (StartAngle >= FinishAngle)
            //                {
            //                    (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //                    textBox1.Text += "Начальное значение угла сканирование должно быть меньше конечного значения угла сканирования" + Environment.NewLine;
            //                    renderControl1.removingArc();
            //                    Error = true;
            //                }
            //                else             //все ок через эту ветку
            //                {
            //                    textBoxStart.ForeColor = Color.Black;
            //                    textBoxFinish.ForeColor = Color.Black;
            //                    ArcRefresh();
            //                    Error = false;
            //                }
            //            }
            //            if ((sender as TextBox).Name == textBoxFinish.Name)
            //            {
            //                if (FinishAngle <= StartAngle)
            //                {
            //                    (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //                    textBox1.Text += "Конечное значение угла сканирование должно быть больше начально значения угла сканирования" + Environment.NewLine;
            //                    renderControl1.removingArc();
            //                    Error = true;
            //                }
            //                else             // или через эту ветку
            //                {
            //                    textBoxStart.ForeColor = Color.Black;
            //                    textBoxFinish.ForeColor = Color.Black;
            //                    ArcRefresh();
            //                    Error = false;
            //                }
            //            }
            //        }
            //    }
            //    catch (System.FormatException)
            //    {
            //        (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //        textBox1.Text += "Ошибка ввода диапазона сканирования" + Environment.NewLine;
            //        renderControl1.removingArc();
            //        Error = true;
            //    }
            //}
        }
        private void Check_180(object sender, EventArgs e)
        {
            //if (!(Loading))
            //{
            //    try
            //    {
            //        Single numb = Convert.ToSingle((sender as TextBox).Text);
            //        if (numb < -180 || numb > 180)
            //        {
            //            (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //            textBox1.Text += "Ошибка ввода угла наклона" + Environment.NewLine;
            //            renderControl1.removingArc();
            //            Error = true;
            //        }
            //        else
            //        {
            //            (sender as TextBox).ForeColor = Color.Black;
            //            InclineAngle = numb;
            //            ArcRefresh();
            //            Error = false;
            //        }
            //    }
            //    catch (System.FormatException)
            //    {
            //        (sender as TextBox).ForeColor = Color.FromArgb(220, 40, 20);
            //        textBox1.Text += "Ошибка ввода угла наклона" + Environment.NewLine;
            //        renderControl1.removingArc();
            //        Error = true;
            //    }
            //}
        }
        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            textBoxTitle.BackColor = SystemColors.Window;
        }

        private void radioButtonType_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                if (radioButtonType1.Checked)
                {
                    FarFieldType = 0;
                    DrawFarFieldArc(templateLable);
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = true;

                }
                else
                {
                    FarFieldType = 1;
                    parentForm.renderControl1.removingArc(templateLable);  
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = false;
                }
            }
        }

        
    }    
}
