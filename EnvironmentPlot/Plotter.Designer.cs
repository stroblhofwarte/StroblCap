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
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plotter));
            this.chartChannel1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartChannel2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBoxHumidityChannel1 = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerCh1 = new System.Windows.Forms.CheckBox();
            this.checkBoxHumidityChannel2 = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerCh2 = new System.Windows.Forms.CheckBox();
            this.timerDataRead = new System.Windows.Forms.Timer(this.components);
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartChannel2)).BeginInit();
            this.SuspendLayout();
            // 
            // chartChannel1
            // 
            this.chartChannel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
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
            this.chartChannel1.Location = new System.Drawing.Point(0, -3);
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
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.LegendText = "Humidity  [%]";
            series3.Name = "HumCh1";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series4.Legend = "Legend1";
            series4.LegendText = "Power [%]";
            series4.Name = "PowerCh1";
            this.chartChannel1.Series.Add(series1);
            this.chartChannel1.Series.Add(series2);
            this.chartChannel1.Series.Add(series3);
            this.chartChannel1.Series.Add(series4);
            this.chartChannel1.Size = new System.Drawing.Size(801, 223);
            this.chartChannel1.TabIndex = 0;
            this.chartChannel1.Text = "Channel 1";
            // 
            // chartChannel2
            // 
            this.chartChannel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
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
            this.chartChannel2.Location = new System.Drawing.Point(0, 217);
            this.chartChannel2.Name = "chartChannel2";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.LegendText = "Temperature  [°C]";
            series5.Name = "TempCh2";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.LegendText = "Dewpoint  [°C]";
            series6.Name = "DewCh2";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.LegendText = "Humidity  [%]";
            series7.Name = "HumCh2";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series8.Legend = "Legend1";
            series8.LegendText = "Power [%]";
            series8.Name = "PowerCh2";
            this.chartChannel2.Series.Add(series5);
            this.chartChannel2.Series.Add(series6);
            this.chartChannel2.Series.Add(series7);
            this.chartChannel2.Series.Add(series8);
            this.chartChannel2.Size = new System.Drawing.Size(801, 235);
            this.chartChannel2.TabIndex = 1;
            this.chartChannel2.Text = "Channel 2";
            // 
            // checkBoxHumidityChannel1
            // 
            this.checkBoxHumidityChannel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxHumidityChannel1.AutoSize = true;
            this.checkBoxHumidityChannel1.BackColor = System.Drawing.Color.Black;
            this.checkBoxHumidityChannel1.ForeColor = System.Drawing.Color.DimGray;
            this.checkBoxHumidityChannel1.Location = new System.Drawing.Point(648, 76);
            this.checkBoxHumidityChannel1.Name = "checkBoxHumidityChannel1";
            this.checkBoxHumidityChannel1.Size = new System.Drawing.Size(66, 17);
            this.checkBoxHumidityChannel1.TabIndex = 2;
            this.checkBoxHumidityChannel1.Text = "Humidity";
            this.checkBoxHumidityChannel1.UseVisualStyleBackColor = false;
            this.checkBoxHumidityChannel1.CheckedChanged += new System.EventHandler(this.checkBoxHumidityChannel1_CheckedChanged);
            // 
            // checkBoxPowerCh1
            // 
            this.checkBoxPowerCh1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPowerCh1.AutoSize = true;
            this.checkBoxPowerCh1.BackColor = System.Drawing.Color.Black;
            this.checkBoxPowerCh1.ForeColor = System.Drawing.Color.DimGray;
            this.checkBoxPowerCh1.Location = new System.Drawing.Point(648, 99);
            this.checkBoxPowerCh1.Name = "checkBoxPowerCh1";
            this.checkBoxPowerCh1.Size = new System.Drawing.Size(56, 17);
            this.checkBoxPowerCh1.TabIndex = 3;
            this.checkBoxPowerCh1.Text = "Power";
            this.checkBoxPowerCh1.UseVisualStyleBackColor = false;
            this.checkBoxPowerCh1.CheckedChanged += new System.EventHandler(this.checkBoxPowerCh1_CheckedChanged);
            // 
            // checkBoxHumidityChannel2
            // 
            this.checkBoxHumidityChannel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxHumidityChannel2.AutoSize = true;
            this.checkBoxHumidityChannel2.BackColor = System.Drawing.Color.Black;
            this.checkBoxHumidityChannel2.ForeColor = System.Drawing.Color.DimGray;
            this.checkBoxHumidityChannel2.Location = new System.Drawing.Point(648, 294);
            this.checkBoxHumidityChannel2.Name = "checkBoxHumidityChannel2";
            this.checkBoxHumidityChannel2.Size = new System.Drawing.Size(66, 17);
            this.checkBoxHumidityChannel2.TabIndex = 4;
            this.checkBoxHumidityChannel2.Text = "Humidity";
            this.checkBoxHumidityChannel2.UseVisualStyleBackColor = false;
            this.checkBoxHumidityChannel2.CheckedChanged += new System.EventHandler(this.checkBoxHumidityChannel2_CheckedChanged);
            // 
            // checkBoxPowerCh2
            // 
            this.checkBoxPowerCh2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPowerCh2.AutoSize = true;
            this.checkBoxPowerCh2.BackColor = System.Drawing.Color.Black;
            this.checkBoxPowerCh2.ForeColor = System.Drawing.Color.DimGray;
            this.checkBoxPowerCh2.Location = new System.Drawing.Point(648, 317);
            this.checkBoxPowerCh2.Name = "checkBoxPowerCh2";
            this.checkBoxPowerCh2.Size = new System.Drawing.Size(56, 17);
            this.checkBoxPowerCh2.TabIndex = 5;
            this.checkBoxPowerCh2.Text = "Power";
            this.checkBoxPowerCh2.UseVisualStyleBackColor = false;
            this.checkBoxPowerCh2.CheckedChanged += new System.EventHandler(this.checkBoxPowerCh2_CheckedChanged);
            // 
            // timerDataRead
            // 
            this.timerDataRead.Enabled = true;
            this.timerDataRead.Interval = 10000;
            this.timerDataRead.Tick += new System.EventHandler(this.timerDataRead_Tick);
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPort.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxPort.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(722, 190);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(66, 21);
            this.comboBoxPort.TabIndex = 6;
            this.comboBoxPort.SelectedIndexChanged += new System.EventHandler(this.comboBoxPort_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(660, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "COM Port:";
            // 
            // Plotter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPort);
            this.Controls.Add(this.checkBoxPowerCh2);
            this.Controls.Add(this.checkBoxHumidityChannel2);
            this.Controls.Add(this.checkBoxPowerCh1);
            this.Controls.Add(this.checkBoxHumidityChannel1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartChannel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartChannel2;
        private System.Windows.Forms.CheckBox checkBoxHumidityChannel1;
        private System.Windows.Forms.CheckBox checkBoxPowerCh1;
        private System.Windows.Forms.CheckBox checkBoxHumidityChannel2;
        private System.Windows.Forms.CheckBox checkBoxPowerCh2;
        private System.Windows.Forms.Timer timerDataRead;
        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Label label1;
    }
}

