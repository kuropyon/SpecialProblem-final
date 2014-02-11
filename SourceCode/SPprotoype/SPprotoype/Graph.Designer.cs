using System.Windows;
using System.IO;
namespace SPprotoype
{
    partial class Graph
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "BasedPath";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "ExpectedValue";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "RealValue";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(549, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.ClientSize = new System.Drawing.Size(577, 332);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Graph";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Graph";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Lime;
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
        private int counter = 0;
        public void setDataPoint(Point[] baseCoords , Point[] expectedValue, Point[] realValue)
        {

            if (chart1.Series.Count > 0)
            {
                chart1.Series.Clear();
                chart1.Series.Add("BasedPath");
                chart1.Series["BasedPath"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series.Add("ExpectedValue");
                chart1.Series["ExpectedValue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series.Add("RealValue");
                chart1.Series["RealValue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            for (int i = 0; i < baseCoords.Length; i++)
            {
                chart1.Series["BasedPath"].Points.AddXY(baseCoords[i].X,baseCoords[i].Y);
            }
            for (int i = 0; i < expectedValue.Length; i++)
            {
                chart1.Series["ExpectedValue"].Points.AddXY(expectedValue[i].X, expectedValue[i].Y);
            }
            for (int i = 0; i < realValue.Length; i++)
            {
                chart1.Series["RealValue"].Points.AddXY(realValue[i].X,realValue[i].Y);
            }

            if(File.Exists(@"D:\acads\CMSC190-2\SP\textfiles\graphs\graph"+counter.ToString()+".png"))
                counter++;
            chart1.SaveImage(@"D:\acads\CMSC190-2\SP\textfiles\graphs\graph"+counter.ToString()+".png", System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            
        }
    }
}