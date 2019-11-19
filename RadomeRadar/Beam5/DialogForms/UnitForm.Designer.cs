namespace Apparat
{
    partial class UnitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonM = new System.Windows.Forms.RadioButton();
            this.radioButtonDM = new System.Windows.Forms.RadioButton();
            this.radioButtonSM = new System.Windows.Forms.RadioButton();
            this.radioButtonMM = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.0229F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 197);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(20, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 191);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Единица размерности";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.radioButtonM, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonDM, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonSM, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonMM, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(154, 172);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // radioButtonM
            // 
            this.radioButtonM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonM.AutoSize = true;
            this.radioButtonM.Location = new System.Drawing.Point(15, 142);
            this.radioButtonM.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.radioButtonM.Name = "radioButtonM";
            this.radioButtonM.Size = new System.Drawing.Size(76, 17);
            this.radioButtonM.TabIndex = 3;
            this.radioButtonM.TabStop = true;
            this.radioButtonM.Text = "Метры (м)";
            this.radioButtonM.UseVisualStyleBackColor = true;
            this.radioButtonM.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonDM
            // 
            this.radioButtonDM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonDM.AutoSize = true;
            this.radioButtonDM.Location = new System.Drawing.Point(15, 99);
            this.radioButtonDM.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.radioButtonDM.Name = "radioButtonDM";
            this.radioButtonDM.Size = new System.Drawing.Size(108, 17);
            this.radioButtonDM.TabIndex = 2;
            this.radioButtonDM.TabStop = true;
            this.radioButtonDM.Text = "Дециметры (дм)";
            this.radioButtonDM.UseVisualStyleBackColor = true;
            this.radioButtonDM.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonSM
            // 
            this.radioButtonSM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonSM.AutoSize = true;
            this.radioButtonSM.Location = new System.Drawing.Point(15, 56);
            this.radioButtonSM.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.radioButtonSM.Name = "radioButtonSM";
            this.radioButtonSM.Size = new System.Drawing.Size(111, 17);
            this.radioButtonSM.TabIndex = 1;
            this.radioButtonSM.TabStop = true;
            this.radioButtonSM.Text = "Сантиметры (см)";
            this.radioButtonSM.UseVisualStyleBackColor = true;
            this.radioButtonSM.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonMM
            // 
            this.radioButtonMM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonMM.AutoSize = true;
            this.radioButtonMM.Location = new System.Drawing.Point(15, 13);
            this.radioButtonMM.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.radioButtonMM.Name = "radioButtonMM";
            this.radioButtonMM.Size = new System.Drawing.Size(116, 17);
            this.radioButtonMM.TabIndex = 0;
            this.radioButtonMM.TabStop = true;
            this.radioButtonMM.Text = "Миллиметры (мм)";
            this.radioButtonMM.UseVisualStyleBackColor = true;
            this.radioButtonMM.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // UnitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 197);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UnitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Размерность";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButtonM;
        private System.Windows.Forms.RadioButton radioButtonDM;
        private System.Windows.Forms.RadioButton radioButtonSM;
        private System.Windows.Forms.RadioButton radioButtonMM;
    }
}