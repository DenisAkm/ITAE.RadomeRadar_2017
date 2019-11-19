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
    public partial class CreateRadomeForm : Form
    {
        Form1 Parent;
        Color[] uniqueColors = new Color[58];
        public CreateRadomeForm(Form1 parent)
        {
            InitializeComponent();
            InitializeUniqueColors();
            Parent = parent;
        }

        private void InitializeUniqueColors()
        {
            string[] setOfcolors = new string[]{"#e5194a","#27509e","#20252a","#fbbe18","#29b297","#26295a","#4ebbbc","#ec6593","#a79258",
                "#ef7c52","#e4524f","#32bce9","#3b3b3b","#f3e737","#e83b6e","#3baa36","#e6322a","#19365f","#fadd13","#202230","#eb6626",
                "#e41d32","#1ba1a5","#1a3c55","#040506","#6bba7c","#d72125","#c6ccd2","#f8af42","#715291","#e95254","#1a65ae","#b8ce36",
                "#bc7b37","#fdd816","#784f1e","#2c73b8","#e8534f","#daa43f","#d4cc3c","#f3cc5a","#9059a0","#11a9dd","#e41c2a","#fade1f",
                "#238b94","#39bcdd","#f4cf39","#109338","#e67a5c","#fbf0a3","#22b6ea","#82bb26","#212a31","#8ac4cd","#ede439","#e94a54","#1c9675"};

            for (int i = 0; i < setOfcolors.Length; i++)
            {
                uniqueColors[i] = ColorTranslator.FromHtml(setOfcolors[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {                      
            Parent.LoadRadomeMesh();

            //dataGridView1.Rows.Add(true, Parent.RadomeComposition[]);  
            int currentRow = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed);
            //dataGridView1[0, currentRow].Value = true;
            //dataGridView1[1, currentRow].Value = 
            DataGridViewCell cb = dataGridView1[2, dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Displayed)];
            
            
            DataGridViewComboBoxCell cbc = cb as DataGridViewComboBoxCell;


            cbc.Items.Add("Белый");
            cbc.Items.Add("Синий");
            cbc.Items.Add("Красный"); 
            cbc.Items.Add("Зелёный");
            cbc.Items.Add("Жёлтый");
            cbc.Items.Add("Серый");
            cbc.Items.Add("Оранжевый");               
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(0);
        }

        private void DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                e.DrawBackground();
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.Color.Red), e.Bounds);
                Font f = new Font("Arial", 24, FontStyle.Bold);
                e.Graphics.DrawString("", f, new SolidBrush(System.Drawing.Color.Red), e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        
    }
}
