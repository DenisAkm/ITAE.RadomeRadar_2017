using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apparat
{
    public partial class CreateRadomeForm : Form
    {
        Form1 parentForm = null;
        public Radome tempRadome = new Radome();
        bool loading = false;

        public static Dictionary<string, Color> colorDic = new Dictionary<string, Color>
        {
            {"Белый", Color.Transparent},
            {"Синий", Color.Blue},
            {"Красный", Color.Red},
            {"Зелёный", Color.Green},
            {"Жёлтый", Color.Yellow},
            {"Серый", Color.Gray},
            {"Оранжевый", Color.Orange}
        };

        public CreateRadomeForm(Form1 parent)
        {
            loading = true;
            parentForm = parent;            
            InitializeComponent();            
            LoadRadomeParameters();
            loading = false;
        }

        private void LoadRadomeParameters()
        {
            for (int i = 0; i < Logic.Instance.RadomeComposition.Count; i++)
			{
                // копирует элемент обтекателя и добавляет соответствующую строчку
                RadomeElement el = Logic.Instance.RadomeComposition[i];
                //RadomeElement rel = new RadomeElement(el.Lable, el.Color, el.ListX, el.ListY, el.ListZ, el.ListI1, el.ListI2, el.ListI3, el.Include);
                RadomeElement rel = RadomeElement.Copy(el);
                rel.Tag = el.Tag;
                rel.Structure = el.Structure;
                tempRadome.Add(rel);                
                dataGridView1.Rows.Add(rel.Include, rel.Lable, rel.Structure.Lable, colorDic.FirstOrDefault(x => x.Value == rel.Color).Key);

                // Загружает доступные вариантыы стенок
                DataGridViewCell cb = dataGridView1[2, i];
                DataGridViewComboBoxCell cbc = cb as DataGridViewComboBoxCell;
                for (int j = 0; j < Logic.Instance.RadomeLayers.Count; j++)
                {
                    cbc.Items.Add(Logic.Instance.RadomeLayers[j].Lable);
                }            
			}
        }

        
        private void button2_Click(object sender, EventArgs e)
        {                      
            Logic.Instance.LoadRadomeElementMesh(this);
            buttonOK.Focus();
        }
        
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loading)
            {
                if (e.RowIndex > -1)
                {
                    DataGridViewCheckBoxCell includeCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[0];
                    DataGridViewTextBoxCell lableCell = (DataGridViewTextBoxCell)dataGridView1.Rows[e.RowIndex].Cells[1];

                    DataGridViewComboBoxCell colorCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[3];

                    // стенка
                    if (e.ColumnIndex == 2)
                    {
                        RadomeElement radElement = tempRadome[e.RowIndex];
                        DataGridViewComboBoxCell structureCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[2];

                        //Stenka st = Logic.Instance.RadomeLayers.Find(x => x.Lable == structureCell.Value.ToString());
                        if (structureCell.Value.ToString() != "[Пусто]")
                        {
                            int indexStenka = Logic.Instance.RadomeLayers.FindIndex(x => x.Lable == structureCell.Value.ToString());
                            radElement.Structure = Logic.Instance.RadomeLayers[indexStenka];
                        }
                        else
                        {
                            radElement.Structure = new Stenka("[Пусто]", new List<Complex>() { new Complex(1, 0) }, new List<Complex>() { new Complex(1, 0) }, new List<float>() { 0.001f });
                        }
                        
                    }
                    //цвет и параметр включения
                    if (e.ColumnIndex == 3 || e.ColumnIndex == 0)
                    {
                        if (includeCell.Value.ToString().ToLower() == "true")
                        {
                            Color col = colorDic[colorCell.Value.ToString()];
                            RadomeElement radElement = tempRadome[e.RowIndex];
                            radElement.Include = true;
                            radElement.Color = col;
                            parentForm.renderControl1.Draw(radElement);                            
                        }
                        else
                        {
                            RadomeElement radElement = tempRadome[e.RowIndex];
                            radElement.Include = false;
                            parentForm.renderControl1.RemoveRadome(radElement);
                        }
                    }
                }
            }
        }
        void dataGridView1_CurrentCellDirtyStateChanged(object sender,
        EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int curIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewTextBoxCell lableCell = (DataGridViewTextBoxCell)dataGridView1.Rows[curIndex].Cells[1];
                RadomeElement radElement = tempRadome[curIndex];
                parentForm.renderControl1.RemoveRadome(radElement);
                tempRadome.Exclude(lableCell.Value.ToString());
                dataGridView1.Rows.RemoveAt(curIndex);                
            }
            buttonOK.Focus();
        }

        

        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            TreeNode[] nodes = parentForm.treeViewConfiguration.Nodes.Find("Radome", false);
            nodes[0].Text = "Геометрия обтекателя";
            nodes[0].Nodes.Clear();
            for (int i = 0; i < tempRadome.Count; i++)
            {
                tempRadome[i].Lable = dataGridView1[1, i].Value.ToString();
                parentForm.AddRadomeGeometryNameToTreeView(tempRadome[i]);    
            }
            Logic.Instance.RadomeComposition = tempRadome;
            
            if (Logic.Instance.RadomeComposition.Count > 0)
            {
                CameraManager.Instance.CameraAt(0);

                SharpDX.Vector3 eye = CameraManager.Instance.returnCamera(0).eye;
                SharpDX.Vector3 target = CameraManager.Instance.returnCamera(0).target;
                SharpDX.Vector3 up = CameraManager.Instance.returnCamera(0).up;

                SharpDX.Vector3 newEye = (eye - target);
                newEye.Normalize();
                newEye = SharpDX.Vector3.Multiply(newEye, (float)Logic.Instance.RadomeComposition.DiagonalSize * 3f) + target;
                CameraManager.Instance.returnCamera(0).eye = newEye; ;
                CameraManager.Instance.returnCamera(0).SetView(newEye, target, up);
            }
            this.Close();
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            parentForm.renderControl1.ClearRadome();
            for (int i = 0; i < Logic.Instance.RadomeComposition.Count; i++)
            {
                if (Logic.Instance.RadomeComposition[i].Include)
                {
                    parentForm.renderControl1.Draw(Logic.Instance.RadomeComposition[i]);    
                }                
            }
            this.Close();
        }
    }
}
