namespace Symu.SysDyn.App
{
    partial class Home
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGlobalProcess = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbResults = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStop = new System.Windows.Forms.TextBox();
            this.tbStart = new System.Windows.Forms.TextBox();
            this.tbDt = new System.Windows.Forms.TextBox();
            this.Simulation = new System.Windows.Forms.GroupBox();
            this.btnSubModelProcess = new System.Windows.Forms.Button();
            this.cbOptimized = new System.Windows.Forms.CheckBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPause = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbModels = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbEquation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbVariables = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chartControl1 = new Syncfusion.Windows.Forms.Chart.ChartControl();
            this.Simulation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGlobalProcess
            // 
            this.btnGlobalProcess.Location = new System.Drawing.Point(199, 28);
            this.btnGlobalProcess.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGlobalProcess.Name = "btnGlobalProcess";
            this.btnGlobalProcess.Size = new System.Drawing.Size(130, 56);
            this.btnGlobalProcess.TabIndex = 0;
            this.btnGlobalProcess.Text = "Process global model";
            this.btnGlobalProcess.UseVisualStyleBackColor = true;
            this.btnGlobalProcess.Click += new System.EventHandler(this.btnGlobalProcess_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(124, 20);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "Open xmile File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbResults
            // 
            this.cbResults.Enabled = false;
            this.cbResults.FormattingEnabled = true;
            this.cbResults.Location = new System.Drawing.Point(123, 210);
            this.cbResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbResults.Name = "cbResults";
            this.cbResults.Size = new System.Drawing.Size(206, 28);
            this.cbResults.TabIndex = 4;
            this.cbResults.SelectedIndexChanged += new System.EventHandler(this.cbVariables_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Results";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Delta time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Stop";
            // 
            // tbStop
            // 
            this.tbStop.Location = new System.Drawing.Point(109, 55);
            this.tbStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbStop.Name = "tbStop";
            this.tbStop.Size = new System.Drawing.Size(47, 27);
            this.tbStop.TabIndex = 10;
            this.tbStop.TextChanged += new System.EventHandler(this.tbStop_TextChanged);
            // 
            // tbStart
            // 
            this.tbStart.Location = new System.Drawing.Point(109, 24);
            this.tbStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbStart.Name = "tbStart";
            this.tbStart.Size = new System.Drawing.Size(47, 27);
            this.tbStart.TabIndex = 11;
            this.tbStart.TextChanged += new System.EventHandler(this.tbStart_TextChanged);
            // 
            // tbDt
            // 
            this.tbDt.Location = new System.Drawing.Point(109, 86);
            this.tbDt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDt.Name = "tbDt";
            this.tbDt.Size = new System.Drawing.Size(47, 27);
            this.tbDt.TabIndex = 12;
            this.tbDt.TextChanged += new System.EventHandler(this.tbDt_TextChanged);
            // 
            // Simulation
            // 
            this.Simulation.Controls.Add(this.btnSubModelProcess);
            this.Simulation.Controls.Add(this.cbOptimized);
            this.Simulation.Controls.Add(this.lblTime);
            this.Simulation.Controls.Add(this.label6);
            this.Simulation.Controls.Add(this.tbPause);
            this.Simulation.Controls.Add(this.label5);
            this.Simulation.Controls.Add(this.tbStart);
            this.Simulation.Controls.Add(this.tbDt);
            this.Simulation.Controls.Add(this.label3);
            this.Simulation.Controls.Add(this.label1);
            this.Simulation.Controls.Add(this.label2);
            this.Simulation.Controls.Add(this.cbResults);
            this.Simulation.Controls.Add(this.tbStop);
            this.Simulation.Controls.Add(this.label4);
            this.Simulation.Controls.Add(this.btnGlobalProcess);
            this.Simulation.Location = new System.Drawing.Point(356, 10);
            this.Simulation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Simulation.Name = "Simulation";
            this.Simulation.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Simulation.Size = new System.Drawing.Size(338, 250);
            this.Simulation.TabIndex = 13;
            this.Simulation.TabStop = false;
            this.Simulation.Text = "Simulation";
            // 
            // btnSubModelProcess
            // 
            this.btnSubModelProcess.Location = new System.Drawing.Point(199, 90);
            this.btnSubModelProcess.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubModelProcess.Name = "btnSubModelProcess";
            this.btnSubModelProcess.Size = new System.Drawing.Size(130, 56);
            this.btnSubModelProcess.TabIndex = 19;
            this.btnSubModelProcess.Text = "Process sub model";
            this.btnSubModelProcess.UseVisualStyleBackColor = true;
            this.btnSubModelProcess.Click += new System.EventHandler(this.btnSubModelProcess_Click);
            // 
            // cbOptimized
            // 
            this.cbOptimized.AutoSize = true;
            this.cbOptimized.Location = new System.Drawing.Point(14, 155);
            this.cbOptimized.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbOptimized.Name = "cbOptimized";
            this.cbOptimized.Size = new System.Drawing.Size(101, 24);
            this.cbOptimized.TabIndex = 17;
            this.cbOptimized.Text = "Optimized";
            this.cbOptimized.UseVisualStyleBackColor = true;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(286, 166);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(17, 20);
            this.lblTime.TabIndex = 16;
            this.lblTime.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(220, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Time";
            // 
            // tbPause
            // 
            this.tbPause.Location = new System.Drawing.Point(109, 119);
            this.tbPause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPause.Name = "tbPause";
            this.tbPause.Size = new System.Drawing.Size(47, 27);
            this.tbPause.TabIndex = 14;
            this.tbPause.TextChanged += new System.EventHandler(this.tbPause_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Pause";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xmile";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "xmile files (*.xmile)|*.xmile|stmx files (*.stmx)|*.stmx|itmx files (*.itmx)|*.it" +
    "mx|xml files (*.xml)|*.xml|All files (*.*)|*.*";
            this.openFileDialog1.InitialDirectory = "C:\\Users\\laure\\Dropbox\\Symu\\SourceCode\\Symu.SysDyn\\Github\\SourceCode\\SymuSysDyn\\T" +
    "emplates\\";
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "Xmile file";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "Groups";
            // 
            // cbGroups
            // 
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new System.Drawing.Point(107, 90);
            this.cbGroups.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(195, 28);
            this.cbGroups.TabIndex = 18;
            this.cbGroups.SelectedIndexChanged += new System.EventHandler(this.cbGroups_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbModels);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbEquation);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbVariables);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.cbGroups);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(23, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(324, 250);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model";
            // 
            // cbModels
            // 
            this.cbModels.FormattingEnabled = true;
            this.cbModels.Location = new System.Drawing.Point(107, 55);
            this.cbModels.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbModels.Name = "cbModels";
            this.cbModels.Size = new System.Drawing.Size(195, 28);
            this.cbModels.TabIndex = 22;
            this.cbModels.SelectedIndexChanged += new System.EventHandler(this.cbModels_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "SubModels";
            // 
            // tbEquation
            // 
            this.tbEquation.Enabled = false;
            this.tbEquation.Location = new System.Drawing.Point(107, 162);
            this.tbEquation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbEquation.Multiline = true;
            this.tbEquation.Name = "tbEquation";
            this.tbEquation.Size = new System.Drawing.Size(195, 76);
            this.tbEquation.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "Equation";
            // 
            // cbVariables
            // 
            this.cbVariables.FormattingEnabled = true;
            this.cbVariables.Location = new System.Drawing.Point(107, 126);
            this.cbVariables.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbVariables.Name = "cbVariables";
            this.cbVariables.Size = new System.Drawing.Size(195, 28);
            this.cbVariables.TabIndex = 20;
            this.cbVariables.SelectedIndexChanged += new System.EventHandler(this.cbVariables_SelectedIndexChanged_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 20);
            this.label8.TabIndex = 19;
            this.label8.Text = "Variables";
            // 
            // chartControl1
            // 
            this.chartControl1.ChartArea.CursorLocation = new System.Drawing.Point(0, 0);
            this.chartControl1.ChartArea.CursorReDraw = false;
            this.chartControl1.DataSourceName = "[none]";
            this.chartControl1.IsWindowLess = false;
            // 
            // 
            // 
            this.chartControl1.Legend.Location = new System.Drawing.Point(539, 81);
            this.chartControl1.Legend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chartControl1.Localize = null;
            this.chartControl1.Location = new System.Drawing.Point(25, 267);
            this.chartControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.PrimaryXAxis.LogLabelsDisplayMode = Syncfusion.Windows.Forms.Chart.LogLabelsDisplayMode.Default;
            this.chartControl1.PrimaryXAxis.Margin = true;
            this.chartControl1.PrimaryYAxis.LogLabelsDisplayMode = Syncfusion.Windows.Forms.Chart.LogLabelsDisplayMode.Default;
            this.chartControl1.PrimaryYAxis.Margin = true;
            this.chartControl1.Size = new System.Drawing.Size(660, 582);
            this.chartControl1.TabIndex = 20;
            this.chartControl1.Text = "chartControl1";
            // 
            // 
            // 
            this.chartControl1.Title.Name = "Default";
            this.chartControl1.Titles.Add(this.chartControl1.Title);
            this.chartControl1.VisualTheme = "";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 885);
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Simulation);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Home";
            this.Text = "Symu.SysDyn : system dynamics";
            this.Simulation.ResumeLayout(false);
            this.Simulation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGlobalProcess;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cbResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStop;
        private System.Windows.Forms.TextBox tbStart;
        private System.Windows.Forms.TextBox tbDt;
        private System.Windows.Forms.GroupBox Simulation;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbPause;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbVariables;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbEquation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbOptimized;
        private System.Windows.Forms.ComboBox cbModels;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSubModelProcess;
        private Syncfusion.Windows.Forms.Chart.ChartControl chartControl1;
    }
}

