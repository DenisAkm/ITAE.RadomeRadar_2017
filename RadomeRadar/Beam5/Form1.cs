using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Apparat
{
    public partial class Form1 : Form
    {
        //****************************//
        //********Переменные**********//
        //****************************//      
        #region Singleton Pattern
        private static Form1 instance = null;
        public static Form1 Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new Form1();
                }
                return instance;
            }
        }
        #endregion
        
        

        public bool Loading;           
        public bool Error;


        //написать что блокирует!!!
        public bool lockdata = false;

        public TreeNode selectedNode {get; set;}
        public RenderControl renderControl1;
        public Graph GraphControl;
        public ExTreeView treeViewResults;
        public ExTreeView treeViewConfiguration;


        //***************************//
        //*******Функции формы*******//
        //***************************//
        public Form1()
        {
            InitializeComponent();
            CustomInitialization();
            instance = this;            
        }

        private void CustomInitialization()
        {
            //
            //  renderControl1
            //
            renderControl1 = new RenderControl();
            tableLayoutPanel7.Controls.Add(renderControl1, 1, 0);
            renderControl1.BackColor = System.Drawing.Color.LightGray;
            renderControl1.ContextMenuStrip = contextMenuStripCamera;
            renderControl1.Dock = DockStyle.Fill;
            renderControl1.Location = new System.Drawing.Point(558, 3);
            renderControl1.Name = "renderControl1";
            tableLayoutPanel7.SetRowSpan(renderControl1, 2);
            renderControl1.ScaleFactor = 1F;
            renderControl1.Size = new System.Drawing.Size(1023, 570);
            renderControl1.TabIndex = 0;
            renderControl1.TabStop = false;
            ActiveControl = renderControl1;
            //
            //  treeViewConfiguration
            //
            treeViewConfiguration = new ExTreeView();
            treeViewConfiguration.Dock = DockStyle.Fill;
            treeViewConfiguration.Location = new System.Drawing.Point(3, 18);
            treeViewConfiguration.Name = "treeViewConfiguration";
            TreeNode treeNode1 = new TreeNode("Параметры стенок");
            TreeNode treeNode2 = new TreeNode("Частота[]");
            TreeNode treeNode3 = new TreeNode("Геометрия обтекателя");
            TreeNode treeNode4 = new TreeNode("Источник");
            TreeNode treeNode5 = new TreeNode("Поле в обтекателе");
            TreeNode treeNode6 = new TreeNode("Задача");
            treeNode1.ContextMenuStrip = contextMenuStripCreateStenka;
            treeNode1.Name = "Stenka";
            treeNode1.Text = "Параметры стенок";
            treeNode2.ContextMenuStrip = contextMenuStripFrequency;
            treeNode2.Name = "frequency";
            treeNode2.Text = "Частота[]";
            treeNode3.Name = "Radome";
            treeNode3.Text = "Геометрия обтекателя";
            treeNode4.ContextMenuStrip = contextMenuStripSource;
            treeNode4.Name = "Source";
            treeNode4.Text = "Источник";
            treeNode5.ContextMenuStrip = contextMenuStripNearField;
            treeNode5.Name = "NearField";
            treeNode5.Text = "Поле в обтекателе";
            treeNode6.Name = "Request";
            treeNode6.Text = "Задача";
            treeViewConfiguration.Nodes.AddRange(new TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            treeViewConfiguration.Size = new System.Drawing.Size(543, 549);
            treeViewConfiguration.TabIndex = 0;
            treeViewConfiguration.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeViewConfiguration_NodeMouseClick);
            treeViewConfiguration.DoubleClick += new System.EventHandler(treeViewConfiguration_DoubleClick);
            groupBox18.Controls.Add(treeViewConfiguration);
            //
            //  GraphControl
            //
            GraphControl = new Graph();
            GraphControl.Dock = DockStyle.Fill;
            GraphControl.IsEnableSelection = true;
            GraphControl.Location = new System.Drawing.Point(558, 3);
            GraphControl.Name = "GraphControl";
            GraphControl.ScrollGrace = 0D;
            GraphControl.ScrollMaxX = 0D;
            GraphControl.ScrollMaxY = 0D;
            GraphControl.ScrollMaxY2 = 0D;
            GraphControl.ScrollMinX = 0D;
            GraphControl.ScrollMinY = 0D;
            GraphControl.ScrollMinY2 = 0D;
            GraphControl.Size = new System.Drawing.Size(1023, 570);
            GraphControl.TabIndex = 0;
            GraphControl.UseExtendedPrintDialog = true;
            GraphControl.ZoomStepFraction = 0.2D;
            tableLayoutPanel6.Controls.Add(GraphControl, 1, 0);
            tableLayoutPanel6.SetRowSpan(GraphControl, 2);
            //
            //  treeViewResults
            //
            treeViewResults = new ExTreeView();
            treeViewResults.CheckBoxes = true;
            treeViewResults.Dock = DockStyle.Fill;
            treeViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeViewResults.HideSelection = false;
            treeViewResults.Location = new System.Drawing.Point(0, 0);
            treeViewResults.Margin = new Padding(0, 0, 1, 0);
            treeViewResults.Name = "treeViewResults";
            treeViewResults.Size = new System.Drawing.Size(542, 469);
            treeViewResults.TabIndex = 0;
            treeViewResults.AfterCheck += new TreeViewEventHandler(treeViewResults_AfterCheck);
            treeViewResults.BeforeSelect += new TreeViewCancelEventHandler(treeViewResults_BeforeSelect);
            treeViewResults.AfterSelect += new TreeViewEventHandler(treeViewResults_AfterSelect);
            tableLayoutPanel2.Controls.Add(treeViewResults, 0, 0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            TopMost = true;
            if (MessageBox.Show("Вы хотите сохранить проект перед выходом?", Logic.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Logic.Instance.FastSaveScenario();
            }
            renderControl1.ShutDown();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            renderControl1.Init();                 
            //
            //   Загрузка нового проекта
            //   
            Logic.Instance.ReturnNewProjectParam();
            
            FarFieldC.SetDllDirectory(path);
            NearFieldC.SetDllDirectory(path);
        }

        

        //***************************//
        //******Кнопки на форме******//
        //***************************//


        private void button2_Click(object sender, EventArgs e)
        {
            Logic.Instance.FastSaveScenario();
        }      // cохранение проекта
        private void button3_Click_1(object sender, EventArgs e)
        {
            // Logic.Instance.GraphWithdrawal();            
            Logic.Instance.DiagramWithdrawal();            
            //RegionDataWithdrawal();            
        }
        
        

        
        
        // выгрузка графиков
        private void button4_Click(object sender, EventArgs e)
        {            
            //renderControl1.pause();

            //if (RadomeCurrent == null)
            //{
            //    textBox1.Text += "Токи не рассчитаны" + Environment.NewLine;
            //}
            //else
            //{
            //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        textBoxUseCurrents.Text = saveFileDialog1.FileName;
            //        CurrentSaving(saveFileDialog1.FileName, RadomeCurrent);
            //    }
            //}
        }      // Выгрузка токов                
        private void button6_Click(object sender, EventArgs e)
        {
            if (!(Logic.Instance.Antenna == null))
            {
                if (!(Error))
                {
                    if (Logic.Instance.FastSaveScenario())
                    {
                        tabControl3.SelectedIndex = 1;
                        tabControl3.Refresh();
                        Logic.Instance.Start();
                        Error = false;
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, проверьте окно сообщений");
                }
            }
            else
            {
                textBox1.Text += "Геометрия антенны не загружена" + Environment.NewLine;
            }

        }      // Start        
        private void button8_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadNearFiled();
        }      // загрузка токов        
        
        private void button15_Click(object sender, EventArgs e)
        {
            Logic.Instance.FastSaveScenario();
            Logic.Instance.ReturnNewProjectParam();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadProject();
        }     // загрузка проекта

        private void buttonLoadAntennaMesh_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadAntennaMesh();
        }
        private void buttonLoadRadomeMesh_Click(object sender, EventArgs e)
        {
            CreateRadomeForm newform = new CreateRadomeForm(this);
            newform.Show();            
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            Logic.Instance.CheckMeshSize();
        }

        //*******************************************//
        //******Cобытия на форме в разделе меню******//
        //*******************************************//
        private void новыйПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logic.Instance.FastSaveScenario();
            Logic.Instance.ReturnNewProjectParam();
        }
        private void сохранитьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logic.Instance.FastSaveScenario();
        }
        private void сохранитьМодельКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Logic.Instance.SaveProject(saveFileDialog1.FileName);
            }
        }
        private void загрузкаМоделиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadProject();
        }       //OpenFileDialog      
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть программу?", Logic.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Close();
            }
        }


        //***********************************//
        //******Прочие события на форме******//
        //***********************************//
        
        

        private string GetTypeRadioButton()
        {
            string answer = "";
            if (radioButtonModulus.Checked)
            {
                answer = radioButtonModulus.Text;
            }
            if (radioButtonPhase.Checked)
            {
                answer = radioButtonPhase.Text;
            }
            return answer;
        }

        private string GetComponentRadioButton()
        {
            string answer = "";
            if (radioButtonTotal.Checked)
            {
                answer = radioButtonTotal.Text;
            }
            if (radioButtonTheta.Checked)
            {
                answer = radioButtonTheta.Text;
            }
            if (radioButtonPhi.Checked)
            {
                answer = radioButtonPhi.Text;
            }
            return answer;
        }

        
        
        private void ммToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!Loading)
            //{
            //    ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            //    foreach (ToolStripMenuItem item in размерToolStripMenuItem.DropDownItems)
            //    {
            //        item.Checked = false;
            //    }
            //    clickedItem.Checked = true;
            //    Logic.Instance.SizeUnit = clickedItem.Text;
            //    textBox1.AppendText("Выбрана размерность \"" + Logic.Instance.SizeUnit + "\"" + Environment.NewLine);
                
            //    //CheckMechSize();
            //}
        }
        private void Check_CurrentValue(object sender, EventArgs e)
        {
            if (!(Loading))
            {
                try
                {
                    Single numb = Convert.ToSingle((sender as TextBox).Text);
                    //RefrechApertureCurrents();
                }
                catch (System.FormatException)
                {

                }
            }
        }
       
        
        private void ChangeCameraView(object sender, EventArgs e)
        {
            var obj = (sender as RadioButton);
            if (obj.Name == "radioButtonFreeRotate" && obj.Checked)
            {
                renderControl1.ChangeCamera(0);
            }
            else if (obj.Name == "radioButtonEgoX" && obj.Checked)
            {
                renderControl1.ChangeCamera(1);
            }
            else if (obj.Name == "radioButtonEgoY" && obj.Checked)
            {
                renderControl1.ChangeCamera(2);
            }
            else if (obj.Name == "radioButtonEgoZ" && obj.Checked)
            {
                renderControl1.ChangeCamera(3);
            }
        }

        //***************************//
        //********Операторы**********//
        //***************************//
        
        private void RegionDataWithdrawal()
        {
            try
            {
                int s = 0;

                StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Logic.Instance.ProjectAdress), "Results", SourceTemplate2.Lable + ".txt"));

                int thetaCount = Convert.ToInt32((SourceTemplate2.ThetaScanEFinish - SourceTemplate2.ThetaScanEStart) / SourceTemplate2.ThetaScanEStep) + 1;
                int phiCount = Convert.ToInt32((SourceTemplate2.PhiScanEFinish - SourceTemplate2.PhiScanEStart) / SourceTemplate2.PhiScanEStep) + 1;

                for (int k = 0; k < phiCount; k++)
                {
                    SolutionElement ffc = Logic.Instance.solutions[s];
                    for (int j = 0; j < ffc.ffantenna.Count; j++)
                    {
                        Decimal phi = (decimal)Math.Round(SourceTemplate2.PhiScanEStart + SourceTemplate2.PhiScanEStep * k, 2);
                        if (phi != 0)
                        {
                            string line = String.Format("{0}\t{1}\t{2}\t{3}", phi + 90, ffc.ffradome[j].Etotal - ffc.ffantenna[j].Etotal, ffc.ffantenna[j].Etotal, ffc.ffradome[j].Etotal);
                            sw.WriteLine(line.Replace(",", "."));
                        }
                    }
                    s++;
                }
                sw.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            

        }
        

        
        /// <summary>
        /// Возвращает имя нитки, в которой было измененно состояние Check/Uncheked
        /// </summary>
        /// <returns>Ответ в виде (название родительского элемента, название дочернего элемента)</returns>
        private Tuple<string, string> WhosCheckBoxClicked() 
        {
            string parentName = "";
            string childName = "";

            foreach (TreeNode item1 in treeViewResults.Nodes)
            {
                if (item1.Checked && item1.Tag == null)
                {
                    parentName = item1.Text;
                    item1.Tag = "true";
                    break;
                }
                if (!item1.Checked && item1.Tag == "true")
                {
                    parentName = item1.Text;
                    item1.Tag = null;
                    break;
                }
                foreach (TreeNode item2 in item1.Nodes)
                {
                    if (item2.Checked && item2.Tag == null)
                    {
                        parentName = item1.Text;
                        childName = item2.Text;
                        item2.Tag = "true";
                        break;
                    }
                    if (!item2.Checked && item2.Tag == "true")
                    {
                        parentName = item1.Text;
                        childName = item2.Text;
                        item2.Tag = null;
                        break;
                    }
                }
            }
            return Tuple.Create(parentName, childName);
        }


        private void FarFieldRequestButton_Click(object sender, EventArgs e)
        {
            FarFieldRequestForm form = new FarFieldRequestForm(this);
        }
        private void buttonFrequency_Click(object sender, EventArgs e)
        {
            SetFrequencyForm freq = new SetFrequencyForm(this);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            CreateApertureForm form = new CreateApertureForm(this, true);
        }
        private void buttonStenka_Click(object sender, EventArgs e)
        {
            CreateStenkaForm form = new CreateStenkaForm(this);            
        }
       

        //treeViewConfiguration методы
        private void treeViewConfiguration_DoubleClick(object sender, EventArgs e)
        {
            if (treeViewConfiguration.SelectedNode != null)
            {
                TreeNode ParentNode, ChildNode;
                string parentName = "", childName = "";

                if (treeViewConfiguration.SelectedNode.Level == 0)
                {
                    ParentNode = treeViewConfiguration.SelectedNode;
                    ChildNode = null;
                }
                else
                {
                    ParentNode = treeViewConfiguration.SelectedNode.Parent;
                    ChildNode = treeViewConfiguration.SelectedNode;
                    childName = ChildNode.Name;
                }
                parentName = ParentNode.Name;


                switch (parentName)
                {
                    case "Radome":                                                
                        CreateRadomeForm newform = new CreateRadomeForm(this);
                        newform.Show();                        
                        break;
                    case "Aperture":
                        //LoadAntennaMesh();
                        break;
                    case "Stenka":
                        if (childName == "")
                        {
                            CreateStenkaForm form = new CreateStenkaForm(this);
                        }
                        else
                        {
                            Stenka s = Logic.Instance.RadomeLayers.Find(x => x.Lable == childName);
                            CreateStenkaForm form = new CreateStenkaForm(this, s);
                        }
                        break;
                    case "frequency":
                        SetFrequencyForm freq = new SetFrequencyForm(this);
                        break;
                    case "Source":
                        if (childName == "")
                        {
                            Logic.Instance.LoadAntennaMesh();                                                                               
                        }
                        else
                        {                            
                            CreateApertureForm form = new CreateApertureForm(this, false);
                        }
                        break;
                    case "NearField":
                        Logic.Instance.LoadNearFiled();
                        break;
                    case "Request":
                        if (childName == "")
                        {
                            FarFieldRequestForm form = new FarFieldRequestForm(this);
                        }
                        else
                        {
                            FarFieldRequestTemplate s = Logic.Instance.Requests.Find(x => x.Lable == childName);
                            FarFieldRequestForm form = new FarFieldRequestForm(this, s);
                        }
                        break;
                    default:
                        break;
                }                
            }
        }
        public void ChangeSource()
        {

            TreeNode parentNodeSource = GetParentNode("Source");
            parentNodeSource.Text = SourceTemplate2.Lable;
            //parentNodeSource. = SourceTemplate2.Lable;

            parentNodeSource.Nodes.Clear();

            for (int i = 0; i < SourceTemplate2.Sources.Length; i++)
            {
                parentNodeSource.Nodes.Add(SourceTemplate2.Sources[i].name, SourceTemplate2.Sources[i].name);
            }
            //TreeNode childNodeSource = GetChildNode("Source", formerName);

            //childNodeSource.ForeColor = Color.Black;            

            //childNodeSource.Text = SourceTemplate2.Lable;
            //childNodeSource.Name = SourceTemplate2.Lable;
            //if (SourceTemplate2.Scanning == 0)
            //{
            //    childNodeSource.Nodes.Clear();
            //}
            //if (SourceTemplate2.Scanning == 1)
            //{
            //    childNodeSource.Nodes.Clear();
            //    int pointsThetaCount = Convert.ToInt32((SourceTemplate2.ThetaScanEFinish - SourceTemplate2.ThetaScanEStart) / SourceTemplate2.ThetaScanEStep) + 1;
            //    int pointsPhiCount = Convert.ToInt32((SourceTemplate2.PhiScanEFinish - SourceTemplate2.PhiScanEStart) / SourceTemplate2.PhiScanEStep) + 1;                

            //    for (int i = 0; i < pointsThetaCount; i++)
            //    {
            //        double theta = SourceTemplate2.ThetaScanEStart + i * SourceTemplate2.ThetaScanEStep;
            //        for (int j = 0; j < pointsPhiCount; j++)
            //        {
            //            double phi = SourceTemplate2.PhiScanEStart + j * SourceTemplate2.PhiScanEStep;
            //            string name = String.Concat(SourceTemplate2.Lable, "_T", theta.ToString("0.##"), "_P", phi.ToString("0.##"));
            //            childNodeSource.Nodes.Add(name, name);
            //        }
            //    }    
            //}
            //if (SourceTemplate2.Scanning == 2)
            //{
            //    childNodeSource.Nodes.Clear();
            //    int pointsThetaCount = Convert.ToInt32((SourceTemplate2.ThetaScanMFinish - SourceTemplate2.ThetaScanMStart) / SourceTemplate2.ThetaScanMStep) + 1;
            //    int pointsPhiCount = Convert.ToInt32((SourceTemplate2.PhiScanMFinish - SourceTemplate2.PhiScanMStart) / SourceTemplate2.PhiScanMStep) + 1;

            //    for (int i = 0; i < pointsThetaCount; i++)
            //    {
            //        double theta = SourceTemplate2.ThetaScanMStart + i * SourceTemplate2.ThetaScanMStep;
            //        for (int j = 0; j < pointsPhiCount; j++)
            //        {
            //            double phi = SourceTemplate2.PhiScanMStart + j * SourceTemplate2.PhiScanMStep;
            //            string name = String.Concat(SourceTemplate2.Lable, "_T", theta.ToString("0.##"), "_P", phi.ToString("0.##"));
            //            childNodeSource.Nodes.Add(name, name);
            //        }
            //    }
            //}            
            //Logic.Instance.Sources = template;
            parentNodeSource.Expand();
        }
        public void ChangeStenka(string formerName, Stenka newStenka)
        {
            TreeNode parentNodeStenka = GetParentNode("Stenka");
            TreeNode childNodeStenka = GetChildNode("Stenka", formerName);
            childNodeStenka.Text = newStenka.Lable;
            childNodeStenka.Name = newStenka.Lable;

            int index = Logic.Instance.RadomeLayers.FindIndex(x => x.Lable == formerName);
            //Logic.Instance.RadomeLayers[index] = newStenka;
            Logic.Instance.RadomeLayers[index].Clear();
            Logic.Instance.RadomeLayers[index].Lable = newStenka.Lable;
            for (int i = 0; i < newStenka.Count; i++)
            {
                Logic.Instance.RadomeLayers[index].Add(newStenka.Layers[i]);
            }
            parentNodeStenka.Expand();
        }

        public void ChangeNearFiledName(string newName)
        {
            TreeNode parentNodeNearField = Form1.Instance.GetParentNode("NearField");
            parentNodeNearField.Text = "Поле в обтекателе [" + newName + "]";            
        }
        public TreeNode GetParentNode(string nodeName)
        {
            TreeNode[] SearchResult = treeViewConfiguration.Nodes.Find(nodeName, false);
            int index = 0;
            for (int i = 0; i < SearchResult.Length; i++)
            {
                if (SearchResult[i].Level == 0)
                {
                    index = i;
                    break;
                }
            }
            return SearchResult[index];
        }
        public TreeNode GetChildNode(string parentName, string childName)
        {
            TreeNode[] SearchResult1 = treeViewConfiguration.Nodes.Find(parentName, false);
            int k = 0;
            for (int i = 0; i < SearchResult1.Length; i++)
            {
                if (SearchResult1[i].Level == 0)
                {
                    k = i;
                    break;
                }
            }
            int indexParent = SearchResult1[k].Index;

            TreeNode[] SearchResult2 = treeViewConfiguration.Nodes[indexParent].Nodes.Find(childName, true);

            int p = 0;
            for (int i = 0; i < SearchResult2.Length; i++)
            {
                if (SearchResult2[i].Level == 1)
                {
                    p = i;
                    break;
                }
            }
            int indexChild = SearchResult2[p].Index;
            return treeViewConfiguration.Nodes[indexParent].Nodes[indexChild];
        }
        public void ChangeFarFieldRequest(string formerName, FarFieldRequestTemplate fft)
        {
            TreeNode parentNodeFarFieldRequest = GetParentNode("Request");
            TreeNode childNodeFarFieldRequest = GetChildNode("Request", formerName);
            childNodeFarFieldRequest.Text = fft.Lable;
            childNodeFarFieldRequest.Name = fft.Lable;

            int index = Logic.Instance.Requests.FindIndex(x => x.Lable == formerName);
            Logic.Instance.Requests[index] = fft;
            parentNodeFarFieldRequest.Expand();
        }
        public void DeleteFarFieldRequest(FarFieldRequestTemplate fft)
        {
            Logic.Instance.Requests.Remove(fft);
            TreeNode parentNodeRequest = GetParentNode("Request");
            parentNodeRequest.Nodes.RemoveByKey(fft.Lable);            
        }
        public void DeleteSource()
        {
            //SourceTemplate2.Sources = null;
            //TreeNode parentSourceNode = GetParentNode("Source");
            //parentSourceNode.Nodes.Clear();
        }
        public void CleareSource()
        {
            SourceTemplate2.Sources = null;
            TreeNode parentSourceNode = GetParentNode("Source");
            parentSourceNode.Nodes.Clear();
        }
        public void DeleteStenka(Stenka st)
        {
            Logic.Instance.RadomeLayers.Remove(st);
            TreeNode parentStenkaNode = GetParentNode("Stenka");
            parentStenkaNode.Nodes.RemoveByKey(st.Lable);
        }
        private void DeleteRadomeGeometry(RadomeElement rgt)
        {
            Logic.Instance.RadomeComposition.Remove(rgt);                        

            TreeNode parentNodeRequest = GetParentNode("Radome");
            parentNodeRequest.Nodes.RemoveByKey(rgt.Lable);
        }
        public void AddFarFieldRequest(FarFieldRequestTemplate fft)
        {
            Logic.Instance.Requests.Add(fft);
            TreeNode parentNodeRequest = GetParentNode("Request");
            parentNodeRequest.Nodes.Add(fft.Lable, fft.Lable);
            TreeNode newnode = parentNodeRequest.Nodes[fft.Lable];
            newnode.ContextMenuStrip = CreateContextMenuStrip(newnode);
            parentNodeRequest.Expand();
        }
      
        public void AddStenka(Stenka st)
        {
            Logic.Instance.RadomeLayers.Add(st);
            TreeNode parentNodeStenka = GetParentNode("Stenka");
            parentNodeStenka.Nodes.Add(st.Lable, st.Lable);
            TreeNode newnode = parentNodeStenka.Nodes[st.Lable];
            newnode.ContextMenuStrip = CreateContextMenuStrip(newnode);
            parentNodeStenka.Expand();
        }
        public void AddRadomeGeometryNameToTreeView(RadomeElement radomeTemplate)
        {
            TreeNode[] nodes = treeViewConfiguration.Nodes.Find("Radome", false);
            TreeNode parentNodeRadome = nodes[0];
            string radomeLable = radomeTemplate.Lable;;
            parentNodeRadome.Nodes.Add(radomeLable, radomeLable);
            TreeNode newNode = parentNodeRadome.Nodes[radomeLable];
            if (radomeTemplate.Include)
            {
                newNode.ForeColor = Color.Black;
            }
            else
            {
                newNode.ForeColor = Color.LightGray;
            }           
            
            parentNodeRadome.Expand();
        }
        public void AddSource()
        {            
            TreeNode parentNodeSource = GetParentNode("Source");
            parentNodeSource.Nodes.Add(SourceTemplate2.Lable, SourceTemplate2.Lable);
            TreeNode newnode = parentNodeSource.Nodes[SourceTemplate2.Lable];

            for (int i = 0; i < SourceTemplate2.Sources.Length; i++)
            {
                newnode.Nodes.Add(SourceTemplate2.Sources[i].name, SourceTemplate2.Sources[i].name);
            }
           
            newnode.ContextMenuStrip = CreateContextMenuStrip(newnode);
            parentNodeSource.Expand();
        }
        public void AddFrequenciesToTreeView(List<double> f)
        {
            string name = "Частота [";
            for (int i = 0; i < Logic.Instance.Frequencies.Count; i++)
            {
                name += Logic.Instance.Frequencies[i] / 1e9;
                name += " ГГц";
                if (i != Logic.Instance.Frequencies.Count - 1)
                {
                    name += ", ";
                }
            }
            name += "]";
            TreeNode[] nodes = treeViewConfiguration.Nodes.Find("frequency", false);
            nodes[0].Text = name;      

            treeViewConfiguration.Nodes.Find("frequency", false)[0].Text = name;               
        }
        public void ClearTreeViewConfiguration()
        {
            TreeNode[] nodes = treeViewConfiguration.Nodes.Find("Radome", false);
            nodes[0].Text = "Геометрия обтекателя";
            nodes[0].Nodes.Clear();
            //nodes = treeViewConfiguration.Nodes.Find("Aperture", false);
            //nodes[0].Text = "Геометрия апертуры";
            nodes = treeViewConfiguration.Nodes.Find("Stenka", false);
            nodes[0].Nodes.Clear();
            nodes = treeViewConfiguration.Nodes.Find("Frequency", false);
            nodes[0].Text = "Частота []";
            nodes = treeViewConfiguration.Nodes.Find("Source", false);
            nodes[0].Nodes.Clear();
            nodes = treeViewConfiguration.Nodes.Find("NearField", false);
            nodes[0].Text = "Поле в обтекателе";
            nodes = treeViewConfiguration.Nodes.Find("Request", false);
            nodes[0].Nodes.Clear();
            treeViewConfiguration.Refresh();
        }
        public void RefreshTreeViewConfiguration()
        {
            ClearTreeViewConfiguration();

            for (int i = 0; i < Logic.Instance.RadomeComposition.Count; i++)
            {
                AddRadomeGeometryNameToTreeView(Logic.Instance.RadomeComposition[i]);                
            }
            
            
          
            if (Logic.Instance.RadomeLayers != null)
            {
                for (int i = 0; i < Logic.Instance.RadomeLayers.Count; i++)
                {
                    TreeNode parentNodeStenka = GetParentNode("Stenka");
                    parentNodeStenka.Nodes.Add(Logic.Instance.RadomeLayers[i].Lable, Logic.Instance.RadomeLayers[i].Lable);
                    TreeNode newnode = parentNodeStenka.Nodes[Logic.Instance.RadomeLayers[i].Lable];
                    newnode.ContextMenuStrip = CreateContextMenuStrip_stenka(newnode);
                   
                    parentNodeStenka.Expand();
                }
            }
            AddFrequenciesToTreeView(Logic.Instance.Frequencies);

            if (SourceTemplate2.Lable != null)
            {
                TreeNode parentNodeSource = GetParentNode("Source");
                parentNodeSource.Text = String.Concat("Источник: ",SourceTemplate2.Lable);


                for (int m = 0; m < SourceTemplate2.Sources.Length; m++)
                {
                    parentNodeSource.Nodes.Add(SourceTemplate2.Sources[m].name, SourceTemplate2.Sources[m].name);
                }
                parentNodeSource.Expand();

            }
            else
            {
                TreeNode parentNodeSource = GetParentNode("Source");
                parentNodeSource.Text = "Источник";
            }
            if (Logic.Instance.LoadFieldName != "")
            {
                Form1.Instance.ChangeNearFiledName(Logic.Instance.LoadFieldName);    
            }


            if (Logic.Instance.Requests != null)
            {
                for (int i = 0; i < Logic.Instance.Requests.Count; i++)
                {
                    TreeNode parentNodeSource = GetParentNode("Request");
                    parentNodeSource.Nodes.Add(Logic.Instance.Requests[i].Lable, Logic.Instance.Requests[i].Lable);
                    TreeNode newnode = parentNodeSource.Nodes[Logic.Instance.Requests[i].Lable];
                    newnode.ContextMenuStrip = CreateContextMenuStrip(newnode);
                    if (!Logic.Instance.Requests[i].Include)
                    {
                        newnode.ForeColor = Color.LightGray;
                    }
                    else
                    {
                        newnode.ForeColor = Color.Black;
                    }    
                    parentNodeSource.Expand();
                }
            }
        }
        private ContextMenuStrip CreateContextMenuStrip(TreeNode node)
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            ToolStripMenuItem Change = new ToolStripMenuItem();
            ToolStripMenuItem Exclude = new ToolStripMenuItem();
            ToolStripMenuItem Delete = new ToolStripMenuItem();
            ToolStripMenuItem Copy = new ToolStripMenuItem();
            // 
            // contextMenuStripConfiguration
            // 
            cms.BackColor = System.Drawing.SystemColors.Control;
            cms.Items.AddRange(new ToolStripItem[] {
            Change,
            Copy,
            Exclude,
            Delete});
            cms.Name = node.Text;
            cms.Size = new System.Drawing.Size(139, 70);            
            // 
            // Change
            // 
            Change.Name = "Change";
            Change.Size = new System.Drawing.Size(138, 22);
            Change.Text = "Изменить";
            Change.Tag = node;
            Change.Click += new System.EventHandler(Change_Click);
            // 
            // Exclude
            // 
            Exclude.Name = "Exclude";
            Exclude.Size = new System.Drawing.Size(138, 22);
            Exclude.Text = "Вкл/Выкл";
            Exclude.Tag = node;
            Exclude.Click += new System.EventHandler(Exclude_Click);
            // 
            // Copy
            // 
            Copy.Name = "Copy";
            Copy.Size = new System.Drawing.Size(138, 22);
            Copy.Text = "Копировать";
            Copy.Tag = node;
            Copy.Click += new System.EventHandler(Copy_Click);
            // 
            // Delete
            // 
            Delete.Name = "Delete";
            Delete.Size = new System.Drawing.Size(138, 22);
            Delete.Text = "Удалить";
            Delete.Tag = node;
            Delete.Click += new System.EventHandler(Delete_Click);

            return cms;       
        }

        private ContextMenuStrip CreateContextMenuStrip_radome(TreeNode node)
        {
            ContextMenuStrip cms = new ContextMenuStrip();            
            ToolStripMenuItem Delete = new ToolStripMenuItem();            
            // 
            // contextMenuStripConfiguration
            // 
            cms.BackColor = System.Drawing.SystemColors.Control;
            cms.Items.AddRange(new ToolStripItem[] {            
            Delete});
            cms.Name = node.Text;
            cms.Size = new System.Drawing.Size(139, 70);            
            // 
            // Delete
            // 
            Delete.Name = "Delete";
            Delete.Size = new System.Drawing.Size(138, 22);
            Delete.Text = "Удалить";
            Delete.Tag = node;
            Delete.Click += new System.EventHandler(Delete_Click);

            return cms;
        }
        private ContextMenuStrip CreateContextMenuStrip_stenka(TreeNode node)
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            ToolStripMenuItem Change = new ToolStripMenuItem();            
            ToolStripMenuItem Delete = new ToolStripMenuItem();
            ToolStripMenuItem Copy = new ToolStripMenuItem();
            // 
            // contextMenuStripConfiguration
            // 
            cms.BackColor = System.Drawing.SystemColors.Control;
            cms.Items.AddRange(new ToolStripItem[] {
            Change,
            Copy,
            Delete});
            cms.Name = node.Text;
            cms.Size = new System.Drawing.Size(139, 70);
            // 
            // Change
            // 
            Change.Name = "Change";
            Change.Size = new System.Drawing.Size(138, 22);
            Change.Text = "Изменить";
            Change.Tag = node;
            Change.Click += new System.EventHandler(Change_Click);
            // 
            // Copy
            // 
            Copy.Name = "Copy";
            Copy.Size = new System.Drawing.Size(138, 22);
            Copy.Text = "Копировать";
            Copy.Tag = node;
            Copy.Click += new System.EventHandler(Copy_Click);
            // 
            // Delete
            // 
            Delete.Name = "Delete";
            Delete.Size = new System.Drawing.Size(138, 22);
            Delete.Text = "Удалить";
            Delete.Tag = node;
            Delete.Click += new System.EventHandler(Delete_Click);

            return cms;     
        }

        private void setFrequency_Click(object sender, EventArgs e)
        {
            SetFrequencyForm freq = new SetFrequencyForm(this);
        }

        private void createSource_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadAntennaMesh();
        }
        private void createNewStenka_Click(object sender, EventArgs e)
        {
            CreateStenkaForm form = new CreateStenkaForm(this);  
        }

        private void changeSource_Click(object sender, EventArgs e)
        {
            CreateApertureForm form = new CreateApertureForm(this, false);
        }
        private void Change_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem contextItem = sender as ToolStripMenuItem;
            TreeNode node = contextItem.Tag as TreeNode;

            string parentName = node.Parent.Name;
            string childName = node.Text;
            if (parentName == "Stenka")
            {
                Stenka s = Logic.Instance.RadomeLayers.Find(x => x.Lable == childName);
                CreateStenkaForm form = new CreateStenkaForm(this, s);
            }
            if (parentName == "Source")
            {                
                CreateApertureForm form = new CreateApertureForm(this, false);
            }
            if (parentName == "Request")
            {
                FarFieldRequestTemplate s = Logic.Instance.Requests.Find(x => x.Lable == childName);
                FarFieldRequestForm form = new FarFieldRequestForm(this, s);
            }
        }
        private void Exclude_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem contextItem = sender as ToolStripMenuItem;
            TreeNode node = contextItem.Tag as TreeNode;

            string parentName = node.Parent.Name;
            string childName = node.Text;
            //if (parentName == "Stenka")
            //{
            //    Stenka s = RadomeLayers.Find(x => x.Lable == childName);
            //    if (s.Include)
            //    {
            //        s.Include = false;
            //        node.ForeColor = Color.LightGray;                    
            //    }
            //    else
            //    {
            //        s.Include = true;
            //        node.ForeColor = Color.Black;
            //    }      
            //}            
            if (parentName == "Source")
            {
                ////SourceTemplate s = Logic.Instance.Sources;
                //if (s.Include)
                //{
                //    s.Include = false;
                //    node.ForeColor = Color.LightGray;
                //    if (node.Nodes.Count != 0)
                //    {
                //        foreach (TreeNode child in node.Nodes)
                //        {
                //            child.ForeColor = Color.LightGray;
                //        }
                //    }
                //    QuickRemoveRegion(s);
                //}                    
                //else
                //{
                //    s.Include = true;
                //    node.ForeColor = Color.Black;
                //    if (node.Nodes.Count != 0)
                //    {
                //        foreach (TreeNode child in node.Nodes)
                //        {
                //            child.ForeColor = Color.Black;
                //        }
                //    }
                //    QuickDrawRegion(s);
                //}                 
            }
            if (parentName == "Request")
            {
                FarFieldRequestTemplate s = Logic.Instance.Requests.Find(x => x.Lable == childName);
                if (s.Include)
                {
                    s.Include = false;
                    node.ForeColor = Color.LightGray;
                    QuickRemoveFarField(s);
                }
                else
                {
                    s.Include = true;
                    node.ForeColor = Color.Black;
                    QuickDrawFarField(s);
                }      
            }
            treeViewConfiguration.SelectedNode = null;            
        }

        
        private void Delete_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem contextItem = sender as ToolStripMenuItem;
            TreeNode node = contextItem.Tag as TreeNode;

            string parentName = node.Parent.Name;
            string childName = node.Text;
            if (parentName == "Stenka")
            {
                Stenka st = Logic.Instance.RadomeLayers.Find(x => x.Lable == childName);
                DeleteStenka(st);
            }
            if (parentName == "Source")
            {
                //SourceTemplate s = Logic.Instance.Sources;
                DeleteSource();
                renderControl1.removingscanRegion(SourceTemplate2.Lable);
                renderControl1.removingAntenna();
            }
            if (parentName == "Request")
            {
                FarFieldRequestTemplate s = Logic.Instance.Requests.Find(x => x.Lable == childName);
                DeleteFarFieldRequest(s);
                renderControl1.removingArc(s.Lable);
            }            
        }
        private void Copy_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem contextItem = sender as ToolStripMenuItem;
            TreeNode node = contextItem.Tag as TreeNode;

            string parentName = node.Parent.Name;
            string childName = node.Text;
            if (parentName == "Stenka")
            {
                Stenka st = Logic.Instance.RadomeLayers.Find(x => x.Lable == childName);                
                Stenka st_copy = st.Copy();
                AddStenka(st_copy);                
            }
            if (parentName == "Source")
            {
                //SourceTemplate2 s = Logic.Instance.Sources;
                //SourceTemplate s_copy = s.Copy();
                //AddSource(s_copy);
            }
            if (parentName == "Request")
            {
                FarFieldRequestTemplate s = Logic.Instance.Requests.Find(x => x.Lable == childName);
                FarFieldRequestTemplate fft = s.Copy();
                AddFarFieldRequest(fft);
                if (fft.FarFieldType == 0)
                {
                    renderControl1.Draw(fft.Lable, fft.ThetaStart, fft.ThetaFinish, fft.PhiStart, fft.Delta, fft.SystemOfCoordinates);    
                }                
            }
        }


        //treeViewResults методы
        private void treeViewResults_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null && !lockdata)
            {
                string name = e.Node.Parent.Text + "_" + e.Node.Text;
                GraphData gd = GraphControl.Curves.Find(x => x.Title == name);
                if (gd != null)
                {
                    lockdata = true;
                    string component = gd.Component;
                    string type = gd.Type;
                    ChangeComponent(component);
                    ChangeType(type);
                    lockdata = false;
                }
            }
            if (e.Node.Parent == null)
            {
                treeViewResults.SelectedNode = selectedNode;
            }
        }
        private void treeViewResults_AfterCheck(object sender, TreeViewEventArgs e)
        {
            string name = "";
            bool checkstate = e.Node.Checked;
            string Component = GetComponentRadioButton();
            string Type = GetTypeRadioButton();


            if (e.Node.Parent != null)
            {
                name = e.Node.Parent.Text + "_" + e.Node.Text;
                if (checkstate)
                {
                    FarFieldC ff = Logic.Instance.ConnectFafField(name);
                    GraphControl.Add(name, ff, Logic.Instance.Antenna, Component, Type);
                }
                else
                {
                    GraphControl.Remove(name);
                }
            }
            else
            {
                foreach (TreeNode item in e.Node.Nodes)
                {
                    if (item.Checked != checkstate)
                    {
                        item.Checked = checkstate;
                    }
                }
            }
            treeViewResults.SelectedNode = e.Node;
        }
        private void treeViewResults_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                selectedNode = treeViewResults.SelectedNode;
            }
        }
        private void radioButtonComponent_CheckedChanged(object sender, EventArgs e)
        {
            if (treeViewResults.SelectedNode != null)
            {
                RadioButton rb = sender as RadioButton;
                TreeNode tn = treeViewResults.SelectedNode;

                if (tn.Parent != null && tn.Checked && rb.Checked)
                {
                    string component = rb.Text;
                    string type = GetTypeRadioButton();
                    string name = tn.Parent.Text + "_" + tn.Text;
                    GraphControl.Remove(name);
                    FarFieldC ff = Logic.Instance.ConnectFafField(name);
                    GraphControl.Add(name, ff, Logic.Instance.Antenna, component, type);
                }
            }
            if (GetComponentRadioButton() != "Total")
            {
                radioButtonPhase.Enabled = true;                
            }
            else
            {
                radioButtonPhase.Enabled = false;
                radioButtonModulus.Checked = true;
            }
        }
        private void radioButtonType_CheckedChanged(object sender, EventArgs e)
        {
            if (treeViewResults.SelectedNode != null)
            {
                RadioButton rb = sender as RadioButton;
                TreeNode tn = treeViewResults.SelectedNode;

                if (tn.Parent != null && tn.Checked && rb.Checked)
                {
                    string component = GetComponentRadioButton();
                    string type = rb.Text;
                    string name = tn.Parent.Text + "_" + tn.Text;
                    GraphControl.Remove(name);
                    FarFieldC ff = Logic.Instance.ConnectFafField(name);
                    GraphControl.Add(name, ff, Logic.Instance.Antenna, component, type);
                }
            }
        }
        private void ChangeType(string type)
        {
            if (radioButtonModulus.Text == type)
            {
                radioButtonModulus.Checked = true;
            }
            if (radioButtonPhase.Text == type)
            {
                radioButtonPhase.Checked = true;
            }
        }
        private void ChangeComponent(string component)
        {
            if (radioButtonTotal.Text == component)
            {
                radioButtonTotal.Checked = true;
            }
            if (radioButtonTheta.Text == component)
            {
                radioButtonTheta.Checked = true;
            }
            if (radioButtonPhi.Text == component)
            {
                radioButtonPhi.Checked = true;
            }
        }
        public void ClearResults()
        {
            treeViewResults.Nodes.Clear();
            Logic.Instance.solutions = null;
            treeViewResults.Refresh();
        }


        // render методы 
        //private void QuickDrawRegion()
        //{
        //    double startTheta = SourceTemplate2.ThetaScanEStart;
        //    double finishTheta = SourceTemplate2.ThetaScanEFinish;
        //    double stepTheta = SourceTemplate2.ThetaScanEStep;
        //    double startPhi = SourceTemplate2.PhiScanEStart;
        //    double finishPhi = SourceTemplate2.PhiScanEFinish;
        //    double stepPhi = SourceTemplate2.PhiScanEStep;
        //    int sys = SourceTemplate2.SystemOfCoordinatesScan;
        //    string lable = SourceTemplate2.Lable;

        //    renderControl1.Draw(lable, startTheta, finishTheta, stepTheta, startPhi, finishPhi, stepPhi, sys);
        //}
        //private void QuickRemoveRegion()
        //{
        //    string lable = SourceTemplate2.Lable;

        //    renderControl1.removingscanRegion(lable);
        //}
        private void QuickRemoveFarField(FarFieldRequestTemplate s)
        {
            string lable = s.Lable;

            renderControl1.removingFarField(lable);
        }

        private void QuickDrawFarField(FarFieldRequestTemplate s)
        {
            string title = s.Lable;
            double start = s.ThetaStart;
            double finish = s.ThetaFinish;
            double inclineAngle = s.PhiStart;
            double delta = s.Delta;
            int scantype = s.SystemOfCoordinates;            

            renderControl1.Draw(title, start, finish, inclineAngle, delta, scantype);            
        }
        //
        // Черновые варианты и устаревшие функции
        //        


        private void dxfLoader(string var, string DxfDirectory)
        {
            //DxfDirectory не помню что за переменная
            int RingsCounter = 0;
            string line = "";
            String RingName;
            StreamReader sr = new StreamReader(var);

            List<Single> BufferX = new List<Single>();
            List<Single> BufferY = new List<Single>();
            List<Single> BufferZ = new List<Single>();

            while (!(sr.EndOfStream))
            {

                if (sr.ReadLine() == "SECTION")
                {
                    if (sr.ReadLine() == "  2")
                    {
                        if (sr.ReadLine() == "ENTITIES")
                        {
                            line = sr.ReadLine();
                            while (!(line == "ENDSEC"))
                            {
                                if (line == "POLYLINE")
                                {
                                    BufferX.Clear();
                                    BufferY.Clear();
                                    BufferZ.Clear();
                                    RingsCounter++;
                                    RingName = String.Concat(DxfDirectory, "\\\\", "Ring ", Convert.ToString(RingsCounter), ".txt");
                                    while (!(line == "SEQEND"))
                                    {
                                        if (line == "AcDb3dPolylineVertex")
                                        {
                                            line = sr.ReadLine();
                                            BufferX.Add(Convert.ToSingle(sr.ReadLine().Replace(".", ",")));
                                            line = sr.ReadLine();
                                            BufferY.Add(Convert.ToSingle(sr.ReadLine().Replace(".", ",")));
                                            line = sr.ReadLine();
                                            BufferZ.Add(Convert.ToSingle(sr.ReadLine().Replace(".", ",")));
                                        }
                                        line = sr.ReadLine();
                                    }
                                    //dxfWritter(RingName, BufferX, BufferY, BufferZ);                                    
                                }
                                line = sr.ReadLine();
                            }
                            if (RingsCounter == 0)
                            {
                                MessageBox.Show("3D Полилиний не обнаружено.");
                                goto OUT;
                            }
                        }
                    }
                }
            }
        OUT:
            sr.Close();
            MessageBox.Show("ОК");
        }
        private void dxfWritter(string var, ArrayList X, ArrayList Y, ArrayList Z)
        {
            StreamWriter sw = new StreamWriter(var);
            if (X.Count > 1200)
            {
                int Colibration = Convert.ToInt32(Decimal.Round(Decimal.Divide(Convert.ToDecimal(X.Count), 1000), 0));
                for (int i = 0; i < X.Count / Colibration; i++)
                {
                    sw.Write(Convert.ToString(X[i * Colibration]).Replace(",", "."));
                    sw.Write(",");
                    sw.Write(Convert.ToString(Y[i * Colibration]).Replace(",", "."));
                    sw.Write(",");
                    sw.Write(Convert.ToString(Z[i * Colibration]).Replace(",", "."));
                    sw.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < X.Count; i++)
                {
                    sw.Write(Convert.ToString(X[i]).Replace(",", "."));
                    sw.Write(",");
                    sw.Write(Convert.ToString(Y[i]).Replace(",", "."));
                    sw.Write(",");
                    sw.Write(Convert.ToString(Z[i]).Replace(",", "."));
                    sw.WriteLine();
                }
            }
            sw.Close();
        }



        private void GraphContextRebuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, ZedGraphControl.ContextMenuObjectState objState)
        {
            // Некоторые пункты удалим
            menuStrip.Items.RemoveAt(2);
            menuStrip.Items.RemoveAt(2);
            ToolStripItem showAllCurves = new ToolStripMenuItem("Показать все кривые");

            menuStrip.Items.Add(showAllCurves);
            showAllCurves.Click += new EventHandler(showAllCurves_Click);
        }
        private void showAllCurves_Click(object sender, EventArgs e)
        {

        }
        private void contextMenuStripTextBox1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in contextMenuStripTextBox1.Items)
            {
                if (item.Text == "Очистить")
                {
                    textBox1.Text = "";
                }
            }
        }

        private void treeViewConfiguration_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewConfiguration.SelectedNode = e.Node;
        }

        public void toolStripMenuItemTurnOn_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (!menu.Checked)
            {                
                TreeNode parentNode = GetParentNode("NearField");
                parentNode.ForeColor = Color.Gray;
                treeViewConfiguration.SelectedNode = null;
            }
            else
            {                
                TreeNode parentNode = GetParentNode("NearField");
                parentNode.ForeColor = Color.Black;
                treeViewConfiguration.SelectedNode = null;
            }
            
        }

        private void toolStripMenuItemLoadNearField_Click(object sender, EventArgs e)
        {
            Logic.Instance.LoadNearFiled();
        }

        private void toolStripMenuItemRemoveNearField_Click(object sender, EventArgs e)
        {
            Logic.Instance.RadomeNearField = null;
            TreeNode parentNode = GetParentNode("NearField");
            parentNode.Text = "Поле в обтекателе";
            parentNode.ForeColor = Color.Black;
            treeViewConfiguration.SelectedNode = null;
            toolStripMenuItemTurnOn.Enabled = false;
            toolStripMenuItemTurnOn.Checked = false;
            textBox1.AppendText("Поле в обтекателе выгружено" + Environment.NewLine);
        }

        private void режимПрезентацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderControl1.RunPresentation();
        }

        private void вернутьПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renderControl1.SetPriviousView();
        }
    }   
}
