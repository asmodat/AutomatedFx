namespace AsmodatForexDataManager.UserControls
{
    partial class ChartControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GpbxChart = new System.Windows.Forms.GroupBox();
            this.ChartMain = new Asmodat.FormsControls.ThreadedChart();
            this.GpbxChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpbxChart
            // 
            this.GpbxChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GpbxChart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GpbxChart.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.GpbxChart.Controls.Add(this.ChartMain);
            this.GpbxChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GpbxChart.Location = new System.Drawing.Point(5, 5);
            this.GpbxChart.Margin = new System.Windows.Forms.Padding(0);
            this.GpbxChart.Name = "GpbxChart";
            this.GpbxChart.Padding = new System.Windows.Forms.Padding(0);
            this.GpbxChart.Size = new System.Drawing.Size(938, 558);
            this.GpbxChart.TabIndex = 100;
            this.GpbxChart.TabStop = false;
            this.GpbxChart.Text = "Chart";
            // 
            // ChartMain
            // 
            this.ChartMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ChartMain.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ChartMain.Location = new System.Drawing.Point(9, 24);
            this.ChartMain.Margin = new System.Windows.Forms.Padding(0);
            this.ChartMain.Name = "ChartMain";
            this.ChartMain.Size = new System.Drawing.Size(917, 524);
            this.ChartMain.TabIndex = 0;
            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.GpbxChart);
            this.Location = new System.Drawing.Point(5, 5);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(948, 568);
            this.GpbxChart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GpbxChart;
        private Asmodat.FormsControls.ThreadedChart ChartMain;
    }
}
