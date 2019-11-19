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
    public partial class  SetFrequencyForm : Form
    {
        Form1 parent;
        public SetFrequencyForm(Form1 form)
        {
            InitializeComponent();
            parent = form;
            Show();
        }

        private void buttonAddFrequency_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("");
        }

        private void buttonRemoveFrequency_Click(object sender, EventArgs e)
        {            
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                if (dataGridView1.Rows.Count != item.RowIndex + 1)
                {
                    DataGridViewRow row = dataGridView1.Rows[item.RowIndex];
                    dataGridView1.Rows.Remove(row);
                }                          
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool error = false;
            Logic.Instance.Frequencies.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
                object obj = dataGridView1[0, i].Value;
                if (obj != null)
                {
                    string val = obj.ToString();
                    //double variable = Convert.ToDouble(val);
                    if (val != "0" && val != null)  //Convert.ToDouble(.ToString().Replace(".",","))
                    {
                        try
                        {
                            double numb = Convert.ToDouble(val); // dataGridView1[0, i].Value.ToString().Replace(".", ",")
                            Logic.Instance.Frequencies.Add(Convert.ToDouble(val) * 1e9); //dataGridView1[0, i].Value.ToString().Replace(",", ".")
                            dataGridView1[0, i].Style.ForeColor = System.Drawing.SystemColors.WindowText;
                            error = false;
                        }
                        catch (Exception)
                        {
                            dataGridView1[0, i].Style.ForeColor = Color.FromArgb(220, 40, 20);
                            dataGridView1.ClearSelection();
                            error = true;
                        }
                    }                    
                }                
			}
            if (!error)
            {
                parent.AddFrequenciesToTreeView(Logic.Instance.Frequencies);
                Close();    
            }            
        }

        private void SetFrequencyForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Logic.Instance.Frequencies.Count; i++)
            {
                dataGridView1.Rows.Add(Logic.Instance.Frequencies[i] / 1e9);
            }
        }
    }
}
