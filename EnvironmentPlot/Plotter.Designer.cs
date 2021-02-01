namespace EnvironmentPlot
{
    partial class Plotter
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plotter));
            this.chartChannel1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartChannel2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timerDataRead = new System.Windows.Forms.Timer(this.components);
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelSystem = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.labelChannel1 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonPanelSystem = new System.Windows.Forms.Button();
            this.buttonPanelRight = new System.Windows.Forms.Button();
            this.buttonCh1OnOff = new System.Windows.Forms.Button();
            this.buttonPanelLeft = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonAuto1 = new System.Windows.Forms.Button();
            this.numericUpDownChannel1 = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelTemp1 = new System.Windows.Forms.Label();
            this.labelDew1 = new System.Windows.Forms.Label();
            this.labelHum1 = new System.Windows.Forms.Label();
            this.labelPower1 = new System.Windows.Forms.Label();
            this.labelDesc1 = new System.Windows.Forms.Label();
            this.labelDesc2 = new System.Windows.Forms.Label();
            this.labelPower2 = new System.Windows.Forms.Label();
            this.labelHum2 = new System.Windows.Forms.Label();
            this.labelDew2 = new System.Windows.Forms.Label();
            this.labelTemp2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.numericUpDownChannel2 = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.buttonAuto2 = new System.Windows.Forms.Button();
            this.labelChannel2 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.buttonCh2OnOff = new System.Windows.Forms.Button();
            this.labelAge1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelAge2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel2)).BeginInit();
            this.panelRight.SuspendLayout();
            this.panelSystem.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChannel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChannel2)).BeginInit();
            this.SuspendLayout();
            // 
            // chartChannel1
            // 
            this.chartChannel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartChannel1.BackColor = System.Drawing.Color.Black;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.Maximum = 720D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.BorderColor = System.Drawing.Color.DimGray;
            chartArea1.Name = "ChartArea1";
            this.chartChannel1.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Black;
            legend1.ForeColor = System.Drawing.Color.DimGray;
            legend1.Name = "Legend1";
            this.chartChannel1.Legends.Add(legend1);
            this.chartChannel1.Location = new System.Drawing.Point(0, 1);
            this.chartChannel1.Name = "chartChannel1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.LegendText = "Temperature  [°C]";
            series1.Name = "TempCh1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.LegendText = "Dewpoint  [°C]";
            series2.Name = "DewCh1";
            this.chartChannel1.Series.Add(series1);
            this.chartChannel1.Series.Add(series2);
            this.chartChannel1.Size = new System.Drawing.Size(801, 250);
            this.chartChannel1.TabIndex = 0;
            this.chartChannel1.Text = "Channel 1";
            // 
            // chartChannel2
            // 
            this.chartChannel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartChannel2.BackColor = System.Drawing.Color.Black;
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gray;
            chartArea2.AxisX.LineColor = System.Drawing.Color.Gray;
            chartArea2.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.Maximum = 720D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.TitleForeColor = System.Drawing.Color.Gray;
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DimGray;
            chartArea2.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea2.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.BorderColor = System.Drawing.Color.DimGray;
            chartArea2.Name = "ChartArea1";
            this.chartChannel2.ChartAreas.Add(chartArea2);
            legend2.BackColor = System.Drawing.Color.Black;
            legend2.ForeColor = System.Drawing.Color.DimGray;
            legend2.Name = "Legend1";
            this.chartChannel2.Legends.Add(legend2);
            this.chartChannel2.Location = new System.Drawing.Point(0, 251);
            this.chartChannel2.Name = "chartChannel2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.LegendText = "Temperature  [°C]";
            series3.Name = "TempCh2";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.LegendText = "Dewpoint  [°C]";
            series4.Name = "DewCh2";
            this.chartChannel2.Series.Add(series3);
            this.chartChannel2.Series.Add(series4);
            this.chartChannel2.Size = new System.Drawing.Size(801, 250);
            this.chartChannel2.TabIndex = 1;
            this.chartChannel2.Text = "Channel 2";
            // 
            // timerDataRead
            // 
            this.timerDataRead.Enabled = true;
            this.timerDataRead.Interval = 10000;
            this.timerDataRead.Tick += new System.EventHandler(this.timerDataRead_Tick);
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.BackColor = System.Drawing.Color.Teal;
            this.panelRight.Controls.Add(this.labelAge2);
            this.panelRight.Controls.Add(this.labelDesc2);
            this.panelRight.Controls.Add(this.label6);
            this.panelRight.Controls.Add(this.buttonPanelRight);
            this.panelRight.Controls.Add(this.labelPower2);
            this.panelRight.Controls.Add(this.numericUpDownChannel2);
            this.panelRight.Controls.Add(this.labelHum2);
            this.panelRight.Controls.Add(this.buttonCh2OnOff);
            this.panelRight.Controls.Add(this.labelDew2);
            this.panelRight.Controls.Add(this.label21);
            this.panelRight.Controls.Add(this.labelTemp2);
            this.panelRight.Controls.Add(this.labelChannel2);
            this.panelRight.Controls.Add(this.label9);
            this.panelRight.Controls.Add(this.buttonAuto2);
            this.panelRight.Controls.Add(this.label10);
            this.panelRight.Controls.Add(this.label19);
            this.panelRight.Controls.Add(this.label11);
            this.panelRight.Controls.Add(this.label18);
            this.panelRight.Controls.Add(this.label12);
            this.panelRight.Location = new System.Drawing.Point(601, 1);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(200, 443);
            this.panelRight.TabIndex = 9;
            // 
            // panelSystem
            // 
            this.panelSystem.BackColor = System.Drawing.Color.Teal;
            this.panelSystem.Controls.Add(this.label13);
            this.panelSystem.Controls.Add(this.buttonConnect);
            this.panelSystem.Controls.Add(this.buttonPanelSystem);
            this.panelSystem.Controls.Add(this.buttonSetup);
            this.panelSystem.Location = new System.Drawing.Point(221, 1);
            this.panelSystem.Name = "panelSystem";
            this.panelSystem.Size = new System.Drawing.Size(357, 100);
            this.panelSystem.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label13.Location = new System.Drawing.Point(46, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(182, 16);
            this.label13.TabIndex = 4;
            this.label13.Text = "ASCOM.StroblCap.Switch";
            // 
            // buttonSetup
            // 
            this.buttonSetup.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetup.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonSetup.Location = new System.Drawing.Point(234, 35);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(44, 26);
            this.buttonSetup.TabIndex = 1;
            this.buttonSetup.Text = "...";
            this.buttonSetup.UseVisualStyleBackColor = false;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Teal;
            this.panelLeft.Controls.Add(this.labelAge1);
            this.panelLeft.Controls.Add(this.label5);
            this.panelLeft.Controls.Add(this.labelDesc1);
            this.panelLeft.Controls.Add(this.labelPower1);
            this.panelLeft.Controls.Add(this.labelHum1);
            this.panelLeft.Controls.Add(this.labelDew1);
            this.panelLeft.Controls.Add(this.labelTemp1);
            this.panelLeft.Controls.Add(this.label3);
            this.panelLeft.Controls.Add(this.label2);
            this.panelLeft.Controls.Add(this.label1);
            this.panelLeft.Controls.Add(this.label17);
            this.panelLeft.Controls.Add(this.label16);
            this.panelLeft.Controls.Add(this.numericUpDownChannel1);
            this.panelLeft.Controls.Add(this.label15);
            this.panelLeft.Controls.Add(this.buttonAuto1);
            this.panelLeft.Controls.Add(this.labelChannel1);
            this.panelLeft.Controls.Add(this.label14);
            this.panelLeft.Controls.Add(this.buttonCh1OnOff);
            this.panelLeft.Controls.Add(this.buttonPanelLeft);
            this.panelLeft.Location = new System.Drawing.Point(0, 1);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(200, 443);
            this.panelLeft.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label14.Location = new System.Drawing.Point(12, 75);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 16);
            this.label14.TabIndex = 5;
            this.label14.Text = "Output active:";
            // 
            // labelChannel1
            // 
            this.labelChannel1.AutoSize = true;
            this.labelChannel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChannel1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelChannel1.Location = new System.Drawing.Point(12, 24);
            this.labelChannel1.Name = "labelChannel1";
            this.labelChannel1.Size = new System.Drawing.Size(80, 16);
            this.labelChannel1.TabIndex = 11;
            this.labelChannel1.Text = "Channel 1:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonConnect.Image = global::EnvironmentPlot.Properties.Resources.Off;
            this.buttonConnect.Location = new System.Drawing.Point(284, 35);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(59, 26);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonPanelSystem
            // 
            this.buttonPanelSystem.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonPanelSystem.FlatAppearance.BorderSize = 0;
            this.buttonPanelSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPanelSystem.Image = global::EnvironmentPlot.Properties.Resources.horizDots;
            this.buttonPanelSystem.Location = new System.Drawing.Point(0, 80);
            this.buttonPanelSystem.Name = "buttonPanelSystem";
            this.buttonPanelSystem.Size = new System.Drawing.Size(357, 20);
            this.buttonPanelSystem.TabIndex = 2;
            this.buttonPanelSystem.UseVisualStyleBackColor = false;
            this.buttonPanelSystem.Click += new System.EventHandler(this.buttonPanelSystem_Click);
            // 
            // buttonPanelRight
            // 
            this.buttonPanelRight.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonPanelRight.FlatAppearance.BorderSize = 0;
            this.buttonPanelRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPanelRight.Image = global::EnvironmentPlot.Properties.Resources.vertikalDots;
            this.buttonPanelRight.Location = new System.Drawing.Point(0, 0);
            this.buttonPanelRight.Name = "buttonPanelRight";
            this.buttonPanelRight.Size = new System.Drawing.Size(20, 443);
            this.buttonPanelRight.TabIndex = 9;
            this.buttonPanelRight.UseVisualStyleBackColor = false;
            this.buttonPanelRight.Click += new System.EventHandler(this.buttonPanelRight_Click);
            // 
            // buttonCh1OnOff
            // 
            this.buttonCh1OnOff.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonCh1OnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCh1OnOff.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonCh1OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;
            this.buttonCh1OnOff.Location = new System.Drawing.Point(120, 72);
            this.buttonCh1OnOff.Name = "buttonCh1OnOff";
            this.buttonCh1OnOff.Size = new System.Drawing.Size(54, 23);
            this.buttonCh1OnOff.TabIndex = 10;
            this.buttonCh1OnOff.UseVisualStyleBackColor = false;
            this.buttonCh1OnOff.Click += new System.EventHandler(this.buttonCh1OnOff_Click);
            // 
            // buttonPanelLeft
            // 
            this.buttonPanelLeft.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonPanelLeft.FlatAppearance.BorderSize = 0;
            this.buttonPanelLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPanelLeft.Image = global::EnvironmentPlot.Properties.Resources.vertikalDots;
            this.buttonPanelLeft.Location = new System.Drawing.Point(180, 0);
            this.buttonPanelLeft.Name = "buttonPanelLeft";
            this.buttonPanelLeft.Size = new System.Drawing.Size(20, 443);
            this.buttonPanelLeft.TabIndex = 9;
            this.buttonPanelLeft.UseVisualStyleBackColor = false;
            this.buttonPanelLeft.Click += new System.EventHandler(this.buttonPanelLeft_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label15.Location = new System.Drawing.Point(12, 121);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 16);
            this.label15.TabIndex = 12;
            this.label15.Text = "Automatic:";
            // 
            // buttonAuto1
            // 
            this.buttonAuto1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonAuto1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAuto1.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonAuto1.Image = global::EnvironmentPlot.Properties.Resources.Off;
            this.buttonAuto1.Location = new System.Drawing.Point(120, 118);
            this.buttonAuto1.Name = "buttonAuto1";
            this.buttonAuto1.Size = new System.Drawing.Size(54, 23);
            this.buttonAuto1.TabIndex = 13;
            this.buttonAuto1.UseVisualStyleBackColor = false;
            this.buttonAuto1.Click += new System.EventHandler(this.buttonAuto1_Click);
            // 
            // numericUpDownChannel1
            // 
            this.numericUpDownChannel1.BackColor = System.Drawing.Color.CadetBlue;
            this.numericUpDownChannel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownChannel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownChannel1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.numericUpDownChannel1.Location = new System.Drawing.Point(97, 165);
            this.numericUpDownChannel1.Name = "numericUpDownChannel1";
            this.numericUpDownChannel1.Size = new System.Drawing.Size(77, 26);
            this.numericUpDownChannel1.TabIndex = 15;
            this.numericUpDownChannel1.ValueChanged += new System.EventHandler(this.numericUpDownChannel1_ValueChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label16.Location = new System.Drawing.Point(12, 170);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 16);
            this.label16.TabIndex = 16;
            this.label16.Text = "Power [%]:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label17.Location = new System.Drawing.Point(14, 227);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 16);
            this.label17.TabIndex = 17;
            this.label17.Text = "Temp.:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(14, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Dewpoint:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label2.Location = new System.Drawing.Point(14, 287);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 19;
            this.label2.Text = "Humidity:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label3.Location = new System.Drawing.Point(14, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Power:";
            // 
            // labelTemp1
            // 
            this.labelTemp1.AutoSize = true;
            this.labelTemp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemp1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelTemp1.Location = new System.Drawing.Point(94, 227);
            this.labelTemp1.Name = "labelTemp1";
            this.labelTemp1.Size = new System.Drawing.Size(35, 16);
            this.labelTemp1.TabIndex = 21;
            this.labelTemp1.Text = "...°C";
            // 
            // labelDew1
            // 
            this.labelDew1.AutoSize = true;
            this.labelDew1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDew1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelDew1.Location = new System.Drawing.Point(94, 257);
            this.labelDew1.Name = "labelDew1";
            this.labelDew1.Size = new System.Drawing.Size(35, 16);
            this.labelDew1.TabIndex = 22;
            this.labelDew1.Text = "...°C";
            // 
            // labelHum1
            // 
            this.labelHum1.AutoSize = true;
            this.labelHum1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHum1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelHum1.Location = new System.Drawing.Point(92, 287);
            this.labelHum1.Name = "labelHum1";
            this.labelHum1.Size = new System.Drawing.Size(33, 16);
            this.labelHum1.TabIndex = 23;
            this.labelHum1.Text = "...%";
            // 
            // labelPower1
            // 
            this.labelPower1.AutoSize = true;
            this.labelPower1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPower1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelPower1.Location = new System.Drawing.Point(92, 317);
            this.labelPower1.Name = "labelPower1";
            this.labelPower1.Size = new System.Drawing.Size(20, 16);
            this.labelPower1.TabIndex = 24;
            this.labelPower1.Text = "...";
            // 
            // labelDesc1
            // 
            this.labelDesc1.AutoSize = true;
            this.labelDesc1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDesc1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelDesc1.Location = new System.Drawing.Point(12, 40);
            this.labelDesc1.Name = "labelDesc1";
            this.labelDesc1.Size = new System.Drawing.Size(16, 13);
            this.labelDesc1.TabIndex = 25;
            this.labelDesc1.Text = "...";
            // 
            // labelDesc2
            // 
            this.labelDesc2.AutoSize = true;
            this.labelDesc2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDesc2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelDesc2.Location = new System.Drawing.Point(25, 40);
            this.labelDesc2.Name = "labelDesc2";
            this.labelDesc2.Size = new System.Drawing.Size(16, 13);
            this.labelDesc2.TabIndex = 41;
            this.labelDesc2.Text = "...";
            // 
            // labelPower2
            // 
            this.labelPower2.AutoSize = true;
            this.labelPower2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPower2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelPower2.Location = new System.Drawing.Point(105, 317);
            this.labelPower2.Name = "labelPower2";
            this.labelPower2.Size = new System.Drawing.Size(20, 16);
            this.labelPower2.TabIndex = 40;
            this.labelPower2.Text = "...";
            // 
            // labelHum2
            // 
            this.labelHum2.AutoSize = true;
            this.labelHum2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHum2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelHum2.Location = new System.Drawing.Point(105, 287);
            this.labelHum2.Name = "labelHum2";
            this.labelHum2.Size = new System.Drawing.Size(33, 16);
            this.labelHum2.TabIndex = 39;
            this.labelHum2.Text = "...%";
            // 
            // labelDew2
            // 
            this.labelDew2.AutoSize = true;
            this.labelDew2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDew2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelDew2.Location = new System.Drawing.Point(107, 257);
            this.labelDew2.Name = "labelDew2";
            this.labelDew2.Size = new System.Drawing.Size(35, 16);
            this.labelDew2.TabIndex = 38;
            this.labelDew2.Text = "...°C";
            // 
            // labelTemp2
            // 
            this.labelTemp2.AutoSize = true;
            this.labelTemp2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemp2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelTemp2.Location = new System.Drawing.Point(107, 227);
            this.labelTemp2.Name = "labelTemp2";
            this.labelTemp2.Size = new System.Drawing.Size(35, 16);
            this.labelTemp2.TabIndex = 37;
            this.labelTemp2.Text = "...°C";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label9.Location = new System.Drawing.Point(27, 317);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 16);
            this.label9.TabIndex = 36;
            this.label9.Text = "Power:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label10.Location = new System.Drawing.Point(27, 287);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 35;
            this.label10.Text = "Humidity:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label11.Location = new System.Drawing.Point(27, 257);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 16);
            this.label11.TabIndex = 34;
            this.label11.Text = "Dewpoint:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label12.Location = new System.Drawing.Point(27, 227);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 16);
            this.label12.TabIndex = 33;
            this.label12.Text = "Temp.:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label18.Location = new System.Drawing.Point(25, 170);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 16);
            this.label18.TabIndex = 32;
            this.label18.Text = "Power [%]:";
            // 
            // numericUpDownChannel2
            // 
            this.numericUpDownChannel2.BackColor = System.Drawing.Color.CadetBlue;
            this.numericUpDownChannel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownChannel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownChannel2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.numericUpDownChannel2.Location = new System.Drawing.Point(110, 165);
            this.numericUpDownChannel2.Name = "numericUpDownChannel2";
            this.numericUpDownChannel2.Size = new System.Drawing.Size(77, 26);
            this.numericUpDownChannel2.TabIndex = 31;
            this.numericUpDownChannel2.ValueChanged += new System.EventHandler(this.numericUpDownChannel2_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label19.Location = new System.Drawing.Point(25, 121);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 16);
            this.label19.TabIndex = 29;
            this.label19.Text = "Automatic:";
            // 
            // buttonAuto2
            // 
            this.buttonAuto2.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonAuto2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAuto2.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonAuto2.Image = global::EnvironmentPlot.Properties.Resources.Off;
            this.buttonAuto2.Location = new System.Drawing.Point(133, 118);
            this.buttonAuto2.Name = "buttonAuto2";
            this.buttonAuto2.Size = new System.Drawing.Size(54, 23);
            this.buttonAuto2.TabIndex = 30;
            this.buttonAuto2.UseVisualStyleBackColor = false;
            this.buttonAuto2.Click += new System.EventHandler(this.buttonAuto2_Click);
            // 
            // labelChannel2
            // 
            this.labelChannel2.AutoSize = true;
            this.labelChannel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChannel2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelChannel2.Location = new System.Drawing.Point(25, 24);
            this.labelChannel2.Name = "labelChannel2";
            this.labelChannel2.Size = new System.Drawing.Size(80, 16);
            this.labelChannel2.TabIndex = 28;
            this.labelChannel2.Text = "Channel 2:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label21.Location = new System.Drawing.Point(25, 75);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(102, 16);
            this.label21.TabIndex = 26;
            this.label21.Text = "Output active:";
            // 
            // buttonCh2OnOff
            // 
            this.buttonCh2OnOff.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonCh2OnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCh2OnOff.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.buttonCh2OnOff.Image = global::EnvironmentPlot.Properties.Resources.Off;
            this.buttonCh2OnOff.Location = new System.Drawing.Point(133, 72);
            this.buttonCh2OnOff.Name = "buttonCh2OnOff";
            this.buttonCh2OnOff.Size = new System.Drawing.Size(54, 23);
            this.buttonCh2OnOff.TabIndex = 27;
            this.buttonCh2OnOff.UseVisualStyleBackColor = false;
            this.buttonCh2OnOff.Click += new System.EventHandler(this.buttonCh2OnOff_Click);
            // 
            // labelAge1
            // 
            this.labelAge1.AutoSize = true;
            this.labelAge1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAge1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelAge1.Location = new System.Drawing.Point(92, 366);
            this.labelAge1.Name = "labelAge1";
            this.labelAge1.Size = new System.Drawing.Size(29, 13);
            this.labelAge1.TabIndex = 27;
            this.labelAge1.Text = "... s";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label5.Location = new System.Drawing.Point(14, 366);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Data age:";
            // 
            // labelAge2
            // 
            this.labelAge2.AutoSize = true;
            this.labelAge2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAge2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.labelAge2.Location = new System.Drawing.Point(105, 366);
            this.labelAge2.Name = "labelAge2";
            this.labelAge2.Size = new System.Drawing.Size(29, 13);
            this.labelAge2.TabIndex = 29;
            this.labelAge2.Text = "... s";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label6.Location = new System.Drawing.Point(27, 366);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Data age:";
            // 
            // Plotter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 501);
            this.Controls.Add(this.panelSystem);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.chartChannel2);
            this.Controls.Add(this.chartChannel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Plotter";
            this.Text = "StroblCap environment plot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Plotter_FormClosing);
            this.Load += new System.EventHandler(this.Plotter_Load);
            this.SizeChanged += new System.EventHandler(this.Plotter_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel2)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelSystem.ResumeLayout(false);
            this.panelSystem.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChannel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownChannel2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartChannel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartChannel2;
        private System.Windows.Forms.Timer timerDataRead;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelSystem;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button buttonPanelRight;
        private System.Windows.Forms.Button buttonPanelSystem;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonPanelLeft;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label labelChannel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button buttonCh1OnOff;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numericUpDownChannel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonAuto1;
        private System.Windows.Forms.Label labelPower1;
        private System.Windows.Forms.Label labelHum1;
        private System.Windows.Forms.Label labelDew1;
        private System.Windows.Forms.Label labelTemp1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label labelDesc1;
        private System.Windows.Forms.Label labelDesc2;
        private System.Windows.Forms.Label labelPower2;
        private System.Windows.Forms.NumericUpDown numericUpDownChannel2;
        private System.Windows.Forms.Label labelHum2;
        private System.Windows.Forms.Button buttonCh2OnOff;
        private System.Windows.Forms.Label labelDew2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labelTemp2;
        private System.Windows.Forms.Label labelChannel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonAuto2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelAge2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelAge1;
        private System.Windows.Forms.Label label5;
    }
}

