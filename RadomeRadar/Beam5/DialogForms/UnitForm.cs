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
    public partial class UnitForm : Form
    {
        public double Dim { get; set; }
        Form1 parent;
        public UnitForm(Form1 p)
        {
            InitializeComponent();
            Dim = 0;
            parent = p;            
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;            
            switch (rb.Name.ToString())
            {
                case "radioButtonMM":
                    Dim = 1e-3f;
                    break;
                case "radioButtonSM":
                    Dim = 1e-2f;
                    break;
                case "radioButtonDM":
                    Dim = 1e-1f;
                    break;
                case "radioButtonM":
                    Dim = 1f;
                    break;
                default:
                    break;
            }
            
            Close();
        }
        
    }
}
