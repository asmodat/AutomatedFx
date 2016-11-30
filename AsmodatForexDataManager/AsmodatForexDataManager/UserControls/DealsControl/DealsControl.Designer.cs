namespace AsmodatForexDataManager.UserControls
{
    partial class DealsControl
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
            this.GpbxDeals = new System.Windows.Forms.GroupBox();
            this.BtnClosePosition = new System.Windows.Forms.Button();
            this.BtnLiquidateAll = new System.Windows.Forms.Button();
            this.BtnCloseSelected = new System.Windows.Forms.Button();
            this.TdgvDeals = new Asmodat.FormsControls.ThreadedDataGridView();
            this.GpbxDeals.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpbxDeals
            // 
            this.GpbxDeals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GpbxDeals.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.GpbxDeals.Controls.Add(this.BtnClosePosition);
            this.GpbxDeals.Controls.Add(this.BtnLiquidateAll);
            this.GpbxDeals.Controls.Add(this.TdgvDeals);
            this.GpbxDeals.Controls.Add(this.BtnCloseSelected);
            this.GpbxDeals.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GpbxDeals.Location = new System.Drawing.Point(5, 5);
            this.GpbxDeals.Name = "GpbxDeals";
            this.GpbxDeals.Size = new System.Drawing.Size(816, 525);
            this.GpbxDeals.TabIndex = 100;
            this.GpbxDeals.TabStop = false;
            this.GpbxDeals.Text = "Open Deals";
            // 
            // BtnClosePosition
            // 
            this.BtnClosePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClosePosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnClosePosition.Location = new System.Drawing.Point(279, 441);
            this.BtnClosePosition.Name = "BtnClosePosition";
            this.BtnClosePosition.Size = new System.Drawing.Size(257, 68);
            this.BtnClosePosition.TabIndex = 98;
            this.BtnClosePosition.Text = "Close Positions";
            this.BtnClosePosition.UseVisualStyleBackColor = true;
            // 
            // BtnLiquidateAll
            // 
            this.BtnLiquidateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLiquidateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnLiquidateAll.Location = new System.Drawing.Point(553, 441);
            this.BtnLiquidateAll.Name = "BtnLiquidateAll";
            this.BtnLiquidateAll.Size = new System.Drawing.Size(257, 68);
            this.BtnLiquidateAll.TabIndex = 97;
            this.BtnLiquidateAll.Text = "Liquidate All";
            this.BtnLiquidateAll.UseVisualStyleBackColor = true;
            // 
            // BtnCloseSelected
            // 
            this.BtnCloseSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCloseSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnCloseSelected.Location = new System.Drawing.Point(6, 441);
            this.BtnCloseSelected.Name = "BtnCloseSelected";
            this.BtnCloseSelected.Size = new System.Drawing.Size(257, 68);
            this.BtnCloseSelected.TabIndex = 96;
            this.BtnCloseSelected.Text = "Close Selected";
            this.BtnCloseSelected.UseVisualStyleBackColor = true;
            // 
            // TdgvDeals
            // 
            this.TdgvDeals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TdgvDeals.AutoScroll = true;
            this.TdgvDeals.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TdgvDeals.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TdgvDeals.Location = new System.Drawing.Point(6, 19);
            this.TdgvDeals.Margin = new System.Windows.Forms.Padding(0);
            this.TdgvDeals.Name = "TdgvDeals";
            this.TdgvDeals.RowsEnumaration = true;
            this.TdgvDeals.Size = new System.Drawing.Size(804, 410);
            this.TdgvDeals.TabIndex = 0;
            // 
            // DealsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.GpbxDeals);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DealsControl";
            this.Size = new System.Drawing.Size(825, 533);
            this.GpbxDeals.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GpbxDeals;
        private System.Windows.Forms.Button BtnCloseSelected;
        private Asmodat.FormsControls.ThreadedDataGridView TdgvDeals;
        private System.Windows.Forms.Button BtnClosePosition;
        private System.Windows.Forms.Button BtnLiquidateAll;
    }
}
