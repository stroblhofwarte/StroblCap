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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series25 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series26 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series27 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series28 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series29 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series30 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series31 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series32 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Plotter));
            this.chartChannel1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartChannel2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBoxHumidityChannel1 = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerCh1 = new System.Windows.Forms.CheckBox();
            this.checkBoxHumidityChannel2 = new System.Windows.Forms.CheckBox();
            this.checkBoxPowerCh2 = new System.Windows.Forms.CheckBox();
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
            chartArea7.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gray;
            chartArea7.AxisX.LineColor = System.Drawing.Color.Gray;
            chartArea7.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea7.AxisX.Maximum = 720D;
            chartArea7.AxisX.Minimum = 0D;
            chartArea7.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea7.AxisX.TitleForeColor = System.Drawing.Color.Gray;
            chartArea7.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DimGray;
            chartArea7.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea7.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea7.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea7.BackColor = System.Drawing.Color.Black;
            chartArea7.BorderColor = System.Drawing.Color.DimGray;
            chartArea7.Name = "ChartArea1";
            this.chartChannel1.ChartAreas.Add(chartArea7);
            legend7.BackColor = System.Drawing.Color.Black;
            legend7.ForeColor = System.Drawing.Color.DimGray;
            legend7.Name = "Legend1";
            this.chartChannel1.Legends.Add(legend7);
            this.chartChannel1.Location = new System.Drawing.Point(0, -3);
            this.chartChannel1.Name = "chartChannel1";
            series25.ChartArea = "ChartArea1";
            series25.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series25.Legend = "Legend1";
            series25.LegendText = "Temperature  [°C]";
            series25.Name = "TempCh1";
            series26.ChartArea = "ChartArea1";
            series26.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series26.Legend = "Legend1";
            series26.LegendText = "Dewpoint  [°C]";
            series26.Name = "DewCh1";
            series27.ChartArea = "ChartArea1";
            series27.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series27.Legend = "Legend1";
            series27.LegendText = "Humidity  [%]";
            series27.Name = "HumCh1";
            series28.ChartArea = "ChartArea1";
            series28.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series28.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series28.Legend = "Legend1";
            series28.LegendText = "Power [%]";
            series28.Name = "PowerCh1";
            this.chartChannel1.Series.Add(series25);
            this.chartChannel1.Series.Add(series26);
            this.chartChannel1.Series.Add(series27);
            this.chartChannel1.Series.Add(series28);
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
            chartArea8.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Gray;
            chartArea8.AxisX.LineColor = System.Drawing.Color.Gray;
            chartArea8.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisX.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea8.AxisX.Maximum = 720D;
            chartArea8.AxisX.Minimum = 0D;
            chartArea8.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisX.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea8.AxisX.TitleForeColor = System.Drawing.Color.Gray;
            chartArea8.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DimGray;
            chartArea8.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea8.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea8.AxisY.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea8.BackColor = System.Drawing.Color.Black;
            chartArea8.BorderColor = System.Drawing.Color.DimGray;
            chartArea8.Name = "ChartArea1";
            this.chartChannel2.ChartAreas.Add(chartArea8);
            legend8.BackColor = System.Drawing.Color.Black;
            legend8.ForeColor = System.Drawing.Color.DimGray;
            legend8.Name = "Legend1";
            this.chartChannel2.Legends.Add(legend8);
            this.chartChannel2.Location = new System.Drawing.Point(0, 217);
            this.chartChannel2.Name = "chartChannel2";
            series29.ChartArea = "ChartArea1";
            series29.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series29.Legend = "Legend1";
            series29.LegendText = "Temperature  [°C]";
            series29.Name = "TempCh2";
            series30.ChartArea = "ChartArea1";
            series30.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series30.Legend = "Legend1";
            series30.LegendText = "Dewpoint  [°C]";
            series30.Name = "DewCh2";
            series31.ChartArea = "ChartArea1";
            series31.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series31.Legend = "Legend1";
            series31.LegendText = "Humidity  [%]";
            series31.Name = "HumCh2";
            series32.ChartArea = "ChartArea1";
            series32.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series32.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series32.Legend = "Legend1";
            series32.LegendText = "Power [%]";
            series32.Name = "PowerCh2";
            this.chartChannel2.Series.Add(series29);
            this.chartChannel2.Series.Add(series30);
            this.chartChannel2.Series.Add(series31);
            this.chartChannel2.Series.Add(series32);
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
            // Plotter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

