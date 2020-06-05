using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Apparat
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStripCreateStenka = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createNewStenka = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFrequency = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setFrequency = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createSource = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSource = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripNearField = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTurnOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadNearField = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveNearField = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStenka = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonSource = new System.Windows.Forms.Button();
            this.FarFieldRequestButton = new System.Windows.Forms.Button();
            this.buttonFrequency = new System.Windows.Forms.Button();
            this.buttonLoadRadomeMesh = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.FarFieldRequest = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonLoadAntennaMesh = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonFreeRotate = new System.Windows.Forms.RadioButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.radioButtonEgoZ = new System.Windows.Forms.RadioButton();
            this.radioButtonEgoY = new System.Windows.Forms.RadioButton();
            this.radioButtonEgoX = new System.Windows.Forms.RadioButton();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonPhase = new System.Windows.Forms.RadioButton();
            this.radioButtonModulus = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonPhi = new System.Windows.Forms.RadioButton();
            this.radioButtonTheta = new System.Windows.Forms.RadioButton();
            this.radioButtonTotal = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonX2 = new System.Windows.Forms.RadioButton();
            this.radioButtonX1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStripTextBox1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripCamera = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вернутьПоУмолчаниюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.режимПрезентацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйПроектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузкаМоделиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьПроектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьМодельКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStripCreateStenka.SuspendLayout();
            this.contextMenuStripFrequency.SuspendLayout();
            this.contextMenuStripSource.SuspendLayout();
            this.contextMenuStripNearField.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStripTextBox1.SuspendLayout();
            this.contextMenuStripCamera.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripCreateStenka
            // 
            this.contextMenuStripCreateStenka.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewStenka});
            this.contextMenuStripCreateStenka.Name = "contextMenuStripCreateStenka";
            this.contextMenuStripCreateStenka.Size = new System.Drawing.Size(148, 26);
            // 
            // createNewStenka
            // 
            this.createNewStenka.Name = "createNewStenka";
            this.createNewStenka.Size = new System.Drawing.Size(147, 22);
            this.createNewStenka.Text = "Новая стенка";
            this.createNewStenka.Click += new System.EventHandler(this.createNewStenka_Click);
            // 
            // contextMenuStripFrequency
            // 
            this.contextMenuStripFrequency.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFrequency});
            this.contextMenuStripFrequency.Name = "contextMenuStripFrequency";
            this.contextMenuStripFrequency.Size = new System.Drawing.Size(182, 26);
            // 
            // setFrequency
            // 
            this.setFrequency.Name = "setFrequency";
            this.setFrequency.Size = new System.Drawing.Size(181, 22);
            this.setFrequency.Text = "Установить частоту";
            this.setFrequency.Click += new System.EventHandler(this.setFrequency_Click);
            // 
            // contextMenuStripSource
            // 
            this.contextMenuStripSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createSource,
            this.loadSource});
            this.contextMenuStripSource.Name = "contextMenuStripSource";
            this.contextMenuStripSource.Size = new System.Drawing.Size(184, 48);
            // 
            // createSource
            // 
            this.createSource.Name = "createSource";
            this.createSource.Size = new System.Drawing.Size(183, 22);
            this.createSource.Text = "Создать источник";
            this.createSource.Click += new System.EventHandler(this.createSource_Click);
            // 
            // loadSource
            // 
            this.loadSource.Name = "loadSource";
            this.loadSource.Size = new System.Drawing.Size(183, 22);
            this.loadSource.Text = "Изменить источник";
            this.loadSource.Click += new System.EventHandler(this.changeSource_Click);
            // 
            // contextMenuStripNearField
            // 
            this.contextMenuStripNearField.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTurnOn,
            this.toolStripMenuItemLoadNearField,
            this.toolStripMenuItemRemoveNearField});
            this.contextMenuStripNearField.Name = "contextMenuStripNearField";
            this.contextMenuStripNearField.Size = new System.Drawing.Size(129, 70);
            // 
            // toolStripMenuItemTurnOn
            // 
            this.toolStripMenuItemTurnOn.CheckOnClick = true;
            this.toolStripMenuItemTurnOn.Enabled = false;
            this.toolStripMenuItemTurnOn.Name = "toolStripMenuItemTurnOn";
            this.toolStripMenuItemTurnOn.Size = new System.Drawing.Size(128, 22);
            this.toolStripMenuItemTurnOn.Text = "Вкл/Выкл";
            this.toolStripMenuItemTurnOn.Click += new System.EventHandler(this.toolStripMenuItemTurnOn_Click);
            // 
            // toolStripMenuItemLoadNearField
            // 
            this.toolStripMenuItemLoadNearField.Name = "toolStripMenuItemLoadNearField";
            this.toolStripMenuItemLoadNearField.Size = new System.Drawing.Size(128, 22);
            this.toolStripMenuItemLoadNearField.Text = "Загрузить";
            this.toolStripMenuItemLoadNearField.Click += new System.EventHandler(this.toolStripMenuItemLoadNearField_Click);
            // 
            // toolStripMenuItemRemoveNearField
            // 
            this.toolStripMenuItemRemoveNearField.Name = "toolStripMenuItemRemoveNearField";
            this.toolStripMenuItemRemoveNearField.Size = new System.Drawing.Size(128, 22);
            this.toolStripMenuItemRemoveNearField.Text = "Удалить";
            this.toolStripMenuItemRemoveNearField.Click += new System.EventHandler(this.toolStripMenuItemRemoveNearField_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 565F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1604, 858);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1601, 100);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1593, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Главная";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 21;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.buttonStenka, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSource, 9, 0);
            this.tableLayoutPanel3.Controls.Add(this.FarFieldRequestButton, 10, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonFrequency, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonLoadRadomeMesh, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.button4, 13, 0);
            this.tableLayoutPanel3.Controls.Add(this.FarFieldRequest, 12, 0);
            this.tableLayoutPanel3.Controls.Add(this.button8, 14, 0);
            this.tableLayoutPanel3.Controls.Add(this.button15, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.button3, 15, 0);
            this.tableLayoutPanel3.Controls.Add(this.button16, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.button2, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonLoadAntennaMesh, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.button1, 11, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1587, 68);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // buttonStenka
            // 
            this.buttonStenka.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonStenka.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStenka.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStenka.ImageKey = "149841.png";
            this.buttonStenka.ImageList = this.imageList1;
            this.buttonStenka.Location = new System.Drawing.Point(398, 0);
            this.buttonStenka.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStenka.Name = "buttonStenka";
            this.buttonStenka.Size = new System.Drawing.Size(64, 68);
            this.buttonStenka.TabIndex = 15;
            this.buttonStenka.Text = "Создать стенку";
            this.buttonStenka.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonStenka.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonStenka.UseVisualStyleBackColor = true;
            this.buttonStenka.Click += new System.EventHandler(this.buttonStenka_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "open.png");
            this.imageList1.Images.SetKeyName(1, "start2.png");
            this.imageList1.Images.SetKeyName(2, "save project.png");
            this.imageList1.Images.SetKeyName(3, "save cur.png");
            this.imageList1.Images.SetKeyName(4, "load cur.png");
            this.imageList1.Images.SetKeyName(5, "new project.png");
            this.imageList1.Images.SetKeyName(6, "import.png");
            this.imageList1.Images.SetKeyName(7, "graph.png");
            this.imageList1.Images.SetKeyName(8, "radome.png");
            this.imageList1.Images.SetKeyName(9, "antenna.png");
            this.imageList1.Images.SetKeyName(10, "norma.png");
            this.imageList1.Images.SetKeyName(11, "284604.png");
            this.imageList1.Images.SetKeyName(12, "refresh_mesh.png");
            this.imageList1.Images.SetKeyName(13, "blinklist_48_9017.png");
            this.imageList1.Images.SetKeyName(14, "69077.png");
            this.imageList1.Images.SetKeyName(15, "284588.png");
            this.imageList1.Images.SetKeyName(16, "149841.png");
            // 
            // buttonSource
            // 
            this.buttonSource.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSource.ImageKey = "284588.png";
            this.buttonSource.ImageList = this.imageList1;
            this.buttonSource.Location = new System.Drawing.Point(526, 0);
            this.buttonSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSource.Name = "buttonSource";
            this.buttonSource.Size = new System.Drawing.Size(64, 68);
            this.buttonSource.TabIndex = 16;
            this.buttonSource.Text = "Выбор источника";
            this.buttonSource.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonSource.UseVisualStyleBackColor = true;
            this.buttonSource.Click += new System.EventHandler(this.button5_Click);
            // 
            // FarFieldRequestButton
            // 
            this.FarFieldRequestButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.FarFieldRequestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FarFieldRequestButton.ImageKey = "blinklist_48_9017.png";
            this.FarFieldRequestButton.ImageList = this.imageList1;
            this.FarFieldRequestButton.Location = new System.Drawing.Point(590, 0);
            this.FarFieldRequestButton.Margin = new System.Windows.Forms.Padding(0);
            this.FarFieldRequestButton.Name = "FarFieldRequestButton";
            this.FarFieldRequestButton.Size = new System.Drawing.Size(64, 68);
            this.FarFieldRequestButton.TabIndex = 15;
            this.FarFieldRequestButton.Text = "Дальняя зона";
            this.FarFieldRequestButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FarFieldRequestButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.FarFieldRequestButton.UseVisualStyleBackColor = true;
            this.FarFieldRequestButton.Click += new System.EventHandler(this.FarFieldRequestButton_Click);
            // 
            // buttonFrequency
            // 
            this.buttonFrequency.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFrequency.ImageKey = "69077.png";
            this.buttonFrequency.ImageList = this.imageList1;
            this.buttonFrequency.Location = new System.Drawing.Point(462, 0);
            this.buttonFrequency.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFrequency.Name = "buttonFrequency";
            this.buttonFrequency.Size = new System.Drawing.Size(64, 68);
            this.buttonFrequency.TabIndex = 15;
            this.buttonFrequency.Text = "Выбор частоты";
            this.buttonFrequency.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonFrequency.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonFrequency.UseVisualStyleBackColor = true;
            this.buttonFrequency.Click += new System.EventHandler(this.buttonFrequency_Click);
            // 
            // buttonLoadRadomeMesh
            // 
            this.buttonLoadRadomeMesh.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonLoadRadomeMesh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadRadomeMesh.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadRadomeMesh.ImageKey = "radome.png";
            this.buttonLoadRadomeMesh.ImageList = this.imageList1;
            this.buttonLoadRadomeMesh.Location = new System.Drawing.Point(334, 0);
            this.buttonLoadRadomeMesh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadRadomeMesh.Name = "buttonLoadRadomeMesh";
            this.buttonLoadRadomeMesh.Size = new System.Drawing.Size(64, 68);
            this.buttonLoadRadomeMesh.TabIndex = 14;
            this.buttonLoadRadomeMesh.Text = "Загрузка обтекателя";
            this.buttonLoadRadomeMesh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLoadRadomeMesh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonLoadRadomeMesh.UseVisualStyleBackColor = true;
            this.buttonLoadRadomeMesh.Click += new System.EventHandler(this.buttonLoadRadomeMesh_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.ImageKey = "save cur.png";
            this.button4.ImageList = this.imageList1;
            this.button4.Location = new System.Drawing.Point(782, 0);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 68);
            this.button4.TabIndex = 3;
            this.button4.Text = "Сохранить поле";
            this.button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FarFieldRequest
            // 
            this.FarFieldRequest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FarFieldRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FarFieldRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FarFieldRequest.ImageKey = "start2.png";
            this.FarFieldRequest.ImageList = this.imageList1;
            this.FarFieldRequest.Location = new System.Drawing.Point(718, 0);
            this.FarFieldRequest.Margin = new System.Windows.Forms.Padding(0);
            this.FarFieldRequest.Name = "FarFieldRequest";
            this.FarFieldRequest.Size = new System.Drawing.Size(64, 68);
            this.FarFieldRequest.TabIndex = 9;
            this.FarFieldRequest.Text = "Начать расчет";
            this.FarFieldRequest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FarFieldRequest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.FarFieldRequest.UseVisualStyleBackColor = true;
            this.FarFieldRequest.Click += new System.EventHandler(this.button6_Click);
            // 
            // button8
            // 
            this.button8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.ImageKey = "load cur.png";
            this.button8.ImageList = this.imageList1;
            this.button8.Location = new System.Drawing.Point(846, 0);
            this.button8.Margin = new System.Windows.Forms.Padding(0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(64, 68);
            this.button8.TabIndex = 7;
            this.button8.Text = "Загрузить поле";
            this.button8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button15
            // 
            this.button15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button15.ImageKey = "new project.png";
            this.button15.ImageList = this.imageList1;
            this.button15.Location = new System.Drawing.Point(0, 0);
            this.button15.Margin = new System.Windows.Forms.Padding(0);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(64, 68);
            this.button15.TabIndex = 10;
            this.button15.Text = "Новый проект";
            this.button15.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.ImageKey = "graph.png";
            this.button3.ImageList = this.imageList1;
            this.button3.Location = new System.Drawing.Point(910, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 68);
            this.button3.TabIndex = 13;
            this.button3.Text = "Сохранить результаты";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button16
            // 
            this.button16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button16.ImageKey = "open.png";
            this.button16.ImageList = this.imageList1;
            this.button16.Location = new System.Drawing.Point(64, 0);
            this.button16.Margin = new System.Windows.Forms.Padding(0);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(64, 68);
            this.button16.TabIndex = 11;
            this.button16.Text = "Открыть проект";
            this.button16.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ImageKey = "save project.png";
            this.button2.ImageList = this.imageList1;
            this.button2.Location = new System.Drawing.Point(128, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 68);
            this.button2.TabIndex = 1;
            this.button2.Text = "Сохранить проект";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonLoadAntennaMesh
            // 
            this.buttonLoadAntennaMesh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadAntennaMesh.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadAntennaMesh.ImageKey = "antenna.png";
            this.buttonLoadAntennaMesh.ImageList = this.imageList1;
            this.buttonLoadAntennaMesh.Location = new System.Drawing.Point(270, 0);
            this.buttonLoadAntennaMesh.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadAntennaMesh.Name = "buttonLoadAntennaMesh";
            this.buttonLoadAntennaMesh.Size = new System.Drawing.Size(64, 68);
            this.buttonLoadAntennaMesh.TabIndex = 12;
            this.buttonLoadAntennaMesh.Text = "Загрузка антенны";
            this.buttonLoadAntennaMesh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLoadAntennaMesh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonLoadAntennaMesh.UseVisualStyleBackColor = true;
            this.buttonLoadAntennaMesh.Click += new System.EventHandler(this.buttonLoadAntennaMesh_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ImageKey = "refresh_mesh.png";
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(654, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 68);
            this.button1.TabIndex = 17;
            this.button1.Text = "Проверка сетки";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1593, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Вид";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 6;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1057F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel18, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.checkBox2, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.checkBox1, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1587, 68);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Controls.Add(this.radioButtonFreeRotate, 0, 0);
            this.tableLayoutPanel18.Controls.Add(this.radioButtonEgoZ, 1, 1);
            this.tableLayoutPanel18.Controls.Add(this.radioButtonEgoY, 1, 0);
            this.tableLayoutPanel18.Controls.Add(this.radioButtonEgoX, 0, 1);
            this.tableLayoutPanel18.Location = new System.Drawing.Point(136, 0);
            this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 2;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(69, 68);
            this.tableLayoutPanel18.TabIndex = 17;
            // 
            // radioButtonFreeRotate
            // 
            this.radioButtonFreeRotate.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonFreeRotate.AutoSize = true;
            this.radioButtonFreeRotate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonFreeRotate.ImageKey = "3d.png";
            this.radioButtonFreeRotate.ImageList = this.imageList2;
            this.radioButtonFreeRotate.Location = new System.Drawing.Point(0, 0);
            this.radioButtonFreeRotate.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonFreeRotate.Name = "radioButtonFreeRotate";
            this.radioButtonFreeRotate.Size = new System.Drawing.Size(34, 34);
            this.radioButtonFreeRotate.TabIndex = 18;
            this.radioButtonFreeRotate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonFreeRotate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButtonFreeRotate.UseVisualStyleBackColor = true;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "3d.png");
            this.imageList2.Images.SetKeyName(1, "LeftView.png");
            this.imageList2.Images.SetKeyName(2, "RightView.png");
            this.imageList2.Images.SetKeyName(3, "TopView.png");
            this.imageList2.Images.SetKeyName(4, "73906.png");
            this.imageList2.Images.SetKeyName(5, "136824.png");
            this.imageList2.Images.SetKeyName(6, "179753.png");
            this.imageList2.Images.SetKeyName(7, "149841.png");
            this.imageList2.Images.SetKeyName(8, "delLayer.png");
            this.imageList2.Images.SetKeyName(9, "loadLayers.png");
            this.imageList2.Images.SetKeyName(10, "loadLayers2.png");
            this.imageList2.Images.SetKeyName(11, "loadLayers3.png");
            // 
            // radioButtonEgoZ
            // 
            this.radioButtonEgoZ.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEgoZ.AutoSize = true;
            this.radioButtonEgoZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonEgoZ.ImageKey = "TopView.png";
            this.radioButtonEgoZ.ImageList = this.imageList2;
            this.radioButtonEgoZ.Location = new System.Drawing.Point(34, 34);
            this.radioButtonEgoZ.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonEgoZ.Name = "radioButtonEgoZ";
            this.radioButtonEgoZ.Size = new System.Drawing.Size(35, 34);
            this.radioButtonEgoZ.TabIndex = 12;
            this.radioButtonEgoZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEgoZ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButtonEgoZ.UseVisualStyleBackColor = true;
            // 
            // radioButtonEgoY
            // 
            this.radioButtonEgoY.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEgoY.AutoSize = true;
            this.radioButtonEgoY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonEgoY.ImageKey = "LeftView.png";
            this.radioButtonEgoY.ImageList = this.imageList2;
            this.radioButtonEgoY.Location = new System.Drawing.Point(34, 0);
            this.radioButtonEgoY.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonEgoY.Name = "radioButtonEgoY";
            this.radioButtonEgoY.Size = new System.Drawing.Size(35, 34);
            this.radioButtonEgoY.TabIndex = 11;
            this.radioButtonEgoY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEgoY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButtonEgoY.UseVisualStyleBackColor = true;
            // 
            // radioButtonEgoX
            // 
            this.radioButtonEgoX.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonEgoX.AutoSize = true;
            this.radioButtonEgoX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonEgoX.ImageKey = "RightView.png";
            this.radioButtonEgoX.ImageList = this.imageList2;
            this.radioButtonEgoX.Location = new System.Drawing.Point(0, 34);
            this.radioButtonEgoX.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonEgoX.Name = "radioButtonEgoX";
            this.radioButtonEgoX.Size = new System.Drawing.Size(34, 34);
            this.radioButtonEgoX.TabIndex = 10;
            this.radioButtonEgoX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonEgoX.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButtonEgoX.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox2.ImageKey = "norma.png";
            this.checkBox2.ImageList = this.imageList1;
            this.checkBox2.Location = new System.Drawing.Point(67, 0);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(69, 68);
            this.checkBox2.TabIndex = 16;
            this.checkBox2.Text = "Показать антенну";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.ImageKey = "284604.png";
            this.checkBox1.ImageList = this.imageList1;
            this.checkBox1.Location = new System.Drawing.Point(0, 0);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(67, 68);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Показать обтекатель";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBox1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl3, 2);
            this.tabControl3.Controls.Add(this.tabPage9);
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 103);
            this.tabControl3.Name = "tabControl3";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl3, 3);
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1598, 608);
            this.tabControl3.TabIndex = 11;
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage9.Controls.Add(this.tableLayoutPanel7);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1590, 582);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "                    Расчётная модель                    ";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.groupBox18, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1584, 576);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // groupBox18
            // 
            this.groupBox18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox18.Location = new System.Drawing.Point(3, 3);
            this.groupBox18.Name = "groupBox18";
            this.tableLayoutPanel7.SetRowSpan(this.groupBox18, 2);
            this.groupBox18.Size = new System.Drawing.Size(549, 570);
            this.groupBox18.TabIndex = 13;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Параметры расчёта";
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage8.Controls.Add(this.tableLayoutPanel6);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1590, 582);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "                    Результаты расчёта                    ";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 555F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox6, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1584, 576);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tableLayoutPanel2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.tableLayoutPanel6.SetRowSpan(this.groupBox6, 2);
            this.groupBox6.Size = new System.Drawing.Size(549, 570);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Обзор решений";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(543, 549);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.radioButtonPhase, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.radioButtonModulus, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 509);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(543, 40);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // radioButtonPhase
            // 
            this.radioButtonPhase.AutoSize = true;
            this.radioButtonPhase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonPhase.Enabled = false;
            this.radioButtonPhase.Location = new System.Drawing.Point(128, 5);
            this.radioButtonPhase.Name = "radioButtonPhase";
            this.radioButtonPhase.Size = new System.Drawing.Size(115, 30);
            this.radioButtonPhase.TabIndex = 2;
            this.radioButtonPhase.Text = "Фаза";
            this.radioButtonPhase.UseVisualStyleBackColor = true;
            this.radioButtonPhase.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
            // 
            // radioButtonModulus
            // 
            this.radioButtonModulus.AutoSize = true;
            this.radioButtonModulus.Checked = true;
            this.radioButtonModulus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonModulus.Location = new System.Drawing.Point(5, 5);
            this.radioButtonModulus.Name = "radioButtonModulus";
            this.radioButtonModulus.Size = new System.Drawing.Size(115, 30);
            this.radioButtonModulus.TabIndex = 1;
            this.radioButtonModulus.TabStop = true;
            this.radioButtonModulus.Text = "Модуль";
            this.radioButtonModulus.UseVisualStyleBackColor = true;
            this.radioButtonModulus.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.radioButtonPhi, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.radioButtonTheta, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.radioButtonTotal, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel9, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 469);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(543, 40);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // radioButtonPhi
            // 
            this.radioButtonPhi.AutoSize = true;
            this.radioButtonPhi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonPhi.Location = new System.Drawing.Point(169, 5);
            this.radioButtonPhi.Name = "radioButtonPhi";
            this.radioButtonPhi.Size = new System.Drawing.Size(74, 30);
            this.radioButtonPhi.TabIndex = 2;
            this.radioButtonPhi.Text = "Phi";
            this.radioButtonPhi.UseVisualStyleBackColor = true;
            this.radioButtonPhi.CheckedChanged += new System.EventHandler(this.radioButtonComponent_CheckedChanged);
            // 
            // radioButtonTheta
            // 
            this.radioButtonTheta.AutoSize = true;
            this.radioButtonTheta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonTheta.Location = new System.Drawing.Point(87, 5);
            this.radioButtonTheta.Name = "radioButtonTheta";
            this.radioButtonTheta.Size = new System.Drawing.Size(74, 30);
            this.radioButtonTheta.TabIndex = 1;
            this.radioButtonTheta.Text = "Theta";
            this.radioButtonTheta.UseVisualStyleBackColor = true;
            this.radioButtonTheta.CheckedChanged += new System.EventHandler(this.radioButtonComponent_CheckedChanged);
            // 
            // radioButtonTotal
            // 
            this.radioButtonTotal.AutoSize = true;
            this.radioButtonTotal.Checked = true;
            this.radioButtonTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonTotal.Location = new System.Drawing.Point(5, 5);
            this.radioButtonTotal.Name = "radioButtonTotal";
            this.radioButtonTotal.Size = new System.Drawing.Size(74, 30);
            this.radioButtonTotal.TabIndex = 0;
            this.radioButtonTotal.TabStop = true;
            this.radioButtonTotal.Text = "Total";
            this.radioButtonTotal.UseVisualStyleBackColor = true;
            this.radioButtonTotal.CheckedChanged += new System.EventHandler(this.radioButtonComponent_CheckedChanged);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.48781F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.5122F));
            this.tableLayoutPanel9.Controls.Add(this.radioButtonX2, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.radioButtonX1, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(251, 5);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(287, 30);
            this.tableLayoutPanel9.TabIndex = 3;
            // 
            // radioButtonX2
            // 
            this.radioButtonX2.AutoSize = true;
            this.radioButtonX2.Location = new System.Drawing.Point(233, 3);
            this.radioButtonX2.Name = "radioButtonX2";
            this.radioButtonX2.Size = new System.Drawing.Size(41, 20);
            this.radioButtonX2.TabIndex = 1;
            this.radioButtonX2.Text = "X2";
            this.radioButtonX2.UseVisualStyleBackColor = true;
            // 
            // radioButtonX1
            // 
            this.radioButtonX1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioButtonX1.AutoSize = true;
            this.radioButtonX1.Checked = true;
            this.radioButtonX1.Location = new System.Drawing.Point(186, 5);
            this.radioButtonX1.Name = "radioButtonX1";
            this.radioButtonX1.Size = new System.Drawing.Size(41, 20);
            this.radioButtonX1.TabIndex = 0;
            this.radioButtonX1.TabStop = true;
            this.radioButtonX1.Text = "X1";
            this.radioButtonX1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(3, 717);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1598, 138);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Информация";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ContextMenuStrip = this.contextMenuStripTextBox1;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 18);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1592, 117);
            this.textBox1.TabIndex = 9;
            // 
            // contextMenuStripTextBox1
            // 
            this.contextMenuStripTextBox1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStripTextBox1.Name = "contextMenuStripTextBox1";
            this.contextMenuStripTextBox1.Size = new System.Drawing.Size(127, 26);
            this.contextMenuStripTextBox1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripTextBox1_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem1.Text = "Очистить";
            // 
            // contextMenuStripCamera
            // 
            this.contextMenuStripCamera.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вернутьПоУмолчаниюToolStripMenuItem,
            this.режимПрезентацииToolStripMenuItem});
            this.contextMenuStripCamera.Name = "contextMenuStripCamera";
            this.contextMenuStripCamera.Size = new System.Drawing.Size(205, 48);
            // 
            // вернутьПоУмолчаниюToolStripMenuItem
            // 
            this.вернутьПоУмолчаниюToolStripMenuItem.Name = "вернутьПоУмолчаниюToolStripMenuItem";
            this.вернутьПоУмолчаниюToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.вернутьПоУмолчаниюToolStripMenuItem.Text = "Вернуть по умолчанию";
            this.вернутьПоУмолчаниюToolStripMenuItem.Click += new System.EventHandler(this.вернутьПоУмолчаниюToolStripMenuItem_Click);
            // 
            // режимПрезентацииToolStripMenuItem
            // 
            this.режимПрезентацииToolStripMenuItem.CheckOnClick = true;
            this.режимПрезентацииToolStripMenuItem.Name = "режимПрезентацииToolStripMenuItem";
            this.режимПрезентацииToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.режимПрезентацииToolStripMenuItem.Text = "Режим презентации";
            this.режимПрезентацииToolStripMenuItem.Click += new System.EventHandler(this.режимПрезентацииToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1604, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйПроектToolStripMenuItem,
            this.загрузкаМоделиToolStripMenuItem,
            this.сохранитьПроектToolStripMenuItem,
            this.сохранитьМодельКакToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // новыйПроектToolStripMenuItem
            // 
            this.новыйПроектToolStripMenuItem.Name = "новыйПроектToolStripMenuItem";
            this.новыйПроектToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.новыйПроектToolStripMenuItem.Text = "Новый проект";
            this.новыйПроектToolStripMenuItem.Click += new System.EventHandler(this.новыйПроектToolStripMenuItem_Click);
            // 
            // загрузкаМоделиToolStripMenuItem
            // 
            this.загрузкаМоделиToolStripMenuItem.Name = "загрузкаМоделиToolStripMenuItem";
            this.загрузкаМоделиToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.загрузкаМоделиToolStripMenuItem.Text = "Открыть проект";
            this.загрузкаМоделиToolStripMenuItem.Click += new System.EventHandler(this.загрузкаМоделиToolStripMenuItem_Click);
            // 
            // сохранитьПроектToolStripMenuItem
            // 
            this.сохранитьПроектToolStripMenuItem.Name = "сохранитьПроектToolStripMenuItem";
            this.сохранитьПроектToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.сохранитьПроектToolStripMenuItem.Text = "Сохранить";
            this.сохранитьПроектToolStripMenuItem.Click += new System.EventHandler(this.сохранитьПроектToolStripMenuItem_Click);
            // 
            // сохранитьМодельКакToolStripMenuItem
            // 
            this.сохранитьМодельКакToolStripMenuItem.Name = "сохранитьМодельКакToolStripMenuItem";
            this.сохранитьМодельКакToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.сохранитьМодельКакToolStripMenuItem.Text = "Сохранить как";
            this.сохранитьМодельКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьМодельКакToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "NASRAN|*.nas|GMSH|*.msh|Cad files|*.dxf|Dat files|*.dat|Text files|*.txt|All file" +
    "s|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 882);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Новый проект";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStripCreateStenka.ResumeLayout(false);
            this.contextMenuStripFrequency.ResumeLayout(false);
            this.contextMenuStripSource.ResumeLayout(false);
            this.contextMenuStripNearField.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel18.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStripTextBox1.ResumeLayout(false);
            this.contextMenuStripCamera.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem загрузкаМоделиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;


        //************************//
        //****Double Constants****//
        //************************//
        private System.Windows.Forms.TabControl tabControl1;        
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        public System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button FarFieldRequest;
        private System.Windows.Forms.ToolStripMenuItem новыйПроектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьПроектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьМодельКакToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;        
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCamera;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox18;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton radioButtonPhase;
        private System.Windows.Forms.RadioButton radioButtonModulus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton radioButtonPhi;
        private System.Windows.Forms.RadioButton radioButtonTheta;
        private System.Windows.Forms.RadioButton radioButtonTotal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSource;
        private System.Windows.Forms.ToolStripMenuItem createSource;
        private System.Windows.Forms.ToolStripMenuItem loadSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFrequency;
        private System.Windows.Forms.ToolStripMenuItem setFrequency;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCreateStenka;
        private System.Windows.Forms.ToolStripMenuItem createNewStenka;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        public System.Windows.Forms.RadioButton radioButtonFreeRotate;
        private System.Windows.Forms.RadioButton radioButtonEgoZ;
        private System.Windows.Forms.RadioButton radioButtonEgoY;
        private System.Windows.Forms.RadioButton radioButtonEgoX;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonStenka;
        private System.Windows.Forms.Button buttonSource;
        private System.Windows.Forms.Button FarFieldRequestButton;
        private System.Windows.Forms.Button buttonFrequency;
        private System.Windows.Forms.Button buttonLoadRadomeMesh;
        private System.Windows.Forms.Button buttonLoadAntennaMesh;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripNearField;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadNearField;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveNearField;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTurnOn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        public System.Windows.Forms.RadioButton radioButtonX2;
        public System.Windows.Forms.RadioButton radioButtonX1;
        private System.Windows.Forms.ToolStripMenuItem вернутьПоУмолчаниюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem режимПрезентацииToolStripMenuItem;
    }
}

