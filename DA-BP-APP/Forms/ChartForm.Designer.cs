namespace AgricultureAnalytics
{
    partial class ChartForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnSaveChart;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnSaveChart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(776, 396);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // btnSaveChart
            // 
            this.btnSaveChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChart.Location = new System.Drawing.Point(713, 414);
            this.btnSaveChart.Name = "btnSaveChart";
            this.btnSaveChart.Size = new System.Drawing.Size(75, 23);
            this.btnSaveChart.TabIndex = 1;
            this.btnSaveChart.Text = "Save Report";
            this.btnSaveChart.UseVisualStyleBackColor = true;
            this.btnSaveChart.Click += new System.EventHandler(this.btnSaveChart_Click);
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSaveChart);
            this.Controls.Add(this.chart1);
            this.Name = "ChartForm";
            this.Text = "Harvest Distribution Chart";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
