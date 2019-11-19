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

namespace Apparat
{
    public partial class CreateStenkaForm : Form
    {
        const string Format1 = "0.#####";
        Form1 parentForm;
        string initialTitle = "Стенка ";
        bool reducting = false;        
        string currentStenkaLable;
        public CreateStenkaForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
            dataGridView1.Rows.Add("1", "1", "0", "1", "0", "1");
            textBoxName.Text = GenerateUniqueStenkaName();
            Show();
        }

        public CreateStenkaForm(Form1 parent, Stenka stenka)
        {
            InitializeComponent();
            reducting = true;
            parentForm = parent;
            ShowCurrentStenkaParameters(stenka);
            textBoxName.Text = stenka.Lable;
            currentStenkaLable = stenka.Lable;
            Show();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(dataGridView1.Rows.Count + 1, "1", "0", "1", "0", "1");
            buttonOK.Focus();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool correct = CheckValues();
            
            if (correct)
            {
                List<Complex> eps = new List<Complex>(dataGridView1.RowCount);
                List<Complex> mu = new List<Complex>(dataGridView1.RowCount);
                List<float> tickness = new List<float>(dataGridView1.RowCount);

                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    eps.Add(new Complex(Convert.ToDouble(dataGridView1[1, j].Value), Convert.ToDouble(dataGridView1[2, j].Value)));
                    mu.Add(new Complex(Convert.ToDouble(dataGridView1[3, j].Value), Convert.ToDouble(dataGridView1[4, j].Value)));
                    tickness.Add(Convert.ToSingle(dataGridView1[5, j].Value) * 0.001f);
                }                
                if (reducting)
                {                    
                    parentForm.ChangeStenka(currentStenkaLable, new Stenka(textBoxName.Text, eps, mu, tickness));                         
                }
                else
                {
                    parentForm.AddStenka(new Stenka(textBoxName.Text, eps, mu, tickness));
                }
                Close();
            }
        }
        private bool CheckValues()

        {
            bool answer = true;
            int countColumns = dataGridView1.ColumnCount;
            int countRows = dataGridView1.RowCount;
            if (countRows == 0)
            {
                answer = false;
            }
            else
            {
                for (int i = 0; i < countRows; i++)
                {
                    for (int j = 1; j < countColumns; j++)
                    {
                        bool error = false;
                        try
                        {
                            double val = Convert.ToDouble(dataGridView1[j, i].Value);
                        }
                        catch (Exception)
                        {
                            error = true;
                        }
                        if (dataGridView1[j, i].Value.ToString() == "" || error)
                        {
                            dataGridView1[j, i].Style.BackColor = Color.Red;
                            answer = false;
                        }
                        else
                        {
                            dataGridView1[j, i].Style.BackColor = SystemColors.Window;
                        }
                    }
                }                
            }
            dataGridView1.ClearSelection();
            return answer;
        }        
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                CorrectOrder();
            }
            buttonOK.Focus();
        }

        private void CorrectOrder()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1[0, i].Value = i + 1;
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.FilterIndex = 5;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<Complex> eps = new List<Complex>();
                List<Complex> mu = new List<Complex>();
                List<float> tickness = new List<float>();
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    do
                    {
                        string line = sr.ReadLine();
                        string[] arr = line.Split('\t');
                        eps.Add(new Complex(Convert.ToDouble(arr[1].Replace(".", ",")), Convert.ToDouble(arr[3].Replace(".", ","))));
                        mu.Add(new Complex(Convert.ToDouble(arr[5].Replace(".", ",")), Convert.ToDouble(arr[7].Replace(".", ","))));
                        tickness.Add(Convert.ToSingle(arr[9].Replace(".", ",")));
                    } while (!(sr.EndOfStream));

                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < eps.Count; i++)
                    {                        
                        //dataGridView1[1, i].Value = eps[i].Real.ToString(Format1);
                        //dataGridView1[2, i].Value = eps[i].Imaginary.ToString(Format1);
                        //dataGridView1[3, i].Value = mu[i].Real.ToString(Format1);
                        //dataGridView1[4, i].Value = mu[i].Imaginary.ToString(Format1);
                        //dataGridView1[5, i].Value = (tickness[i]*1e3).ToString(Format1);
                        string eps1 = eps[i].Real.ToString(Format1);
                        string eps2 = eps[i].Imaginary.ToString(Format1);
                        string mu1 = mu[i].Real.ToString(Format1);
                        string mu2 = mu[i].Imaginary.ToString(Format1);
                        string d = tickness[i].ToString(Format1);
                        dataGridView1.Rows.Add(i + 1, eps1, eps2, mu1, mu2, d);
                    }
                    CorrectOrder();
                    sr.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Неверный формат файла", Logic.ProgramName);
                }
            }
            buttonOK.Focus();
        }

        private void ShowCurrentStenkaParameters(Stenka s)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
			{
                dataGridView1.Rows.RemoveAt(i);
			}
        
            for (int k = 0; k < s.Count; k++)
			{
                string eps1 = s[k].Permittivity.Real.ToString(Format1);
                string eps2 = s[k].Permittivity.Imaginary.ToString(Format1);
                string mu1 = s[k].Permeability.Real.ToString(Format1);
                string mu2 = s[k].Permeability.Imaginary.ToString(Format1);
                string t = (s[k].Tickness * 1e3).ToString(Format1);

                dataGridView1.Rows.Add(dataGridView1.RowCount + 1, eps1, eps2, mu1, mu2, t);
			}            
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            int currentRow = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.Insert(currentRow, "", "1", "0", "1", "0", "1");
            CorrectOrder();
            buttonOK.Focus();
        }

        private string GenerateUniqueStenkaName()
        {
            int k = 0;
            string answer;
            do
            {
                k++;
                answer = initialTitle + k;
            }
            while (CheckStenkaNameMatching(answer));
            return answer;
        }
        private bool CheckStenkaNameMatching(string name)
        {
            TreeNode[] SearceResult = parentForm.treeViewConfiguration.Nodes.Find("Stenka", false);
            TreeNode stenkaParentNode = SearceResult[0];

            bool match = false;

            foreach (TreeNode node in stenkaParentNode.Nodes)
            {
                if (node.Text == name)
                {
                    match = true;
                    break;
                }
            }
            return match;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (CheckValues())
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string savefile = saveFileDialog1.FileName;
                        StreamWriter sw = new StreamWriter(savefile);

                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            double eps1 = Convert.ToDouble(dataGridView1[1, i].Value);
                            double eps2 = Convert.ToDouble(dataGridView1[2, i].Value);
                            double mu1 = Convert.ToDouble(dataGridView1[3, i].Value);
                            double mu2 = Convert.ToDouble(dataGridView1[4, i].Value);
                            double t = Convert.ToDouble(dataGridView1[5, i].Value);
                            string line = String.Format("Слой {0}: E'=\t{1}\tE''=\t{2}\tmu'=\t{3}\tmu''=\t{4}\tt=\t{5}", i + 1, eps1, eps2, mu1, mu2, t);
                            sw.WriteLine(line);
                        }
                        sw.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось записать в указанный файл", Logic.ProgramName);
                    }
                }
            }
        }
        

        private void TextBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar == '\r')
            {
                buttonOK.PerformClick();
            }
        }
    }
}
