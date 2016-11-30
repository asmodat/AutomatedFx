namespace AsmodatForexDataManager
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
            this.TbxHostName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TbxPorts = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TbxPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TbxUserID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TbxBrandCode = new System.Windows.Forms.TextBox();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.TbCntrlMain = new System.Windows.Forms.TabControl();
            this.TabPageCreditals = new System.Windows.Forms.TabPage();
            this.GpbxAuthentification = new System.Windows.Forms.GroupBox();
            this.TabPageArchives = new System.Windows.Forms.TabPage();
            this.ArchiveCntrlMain = new AsmodatForexDataManager.UserControls.ArchiveControl();
            this.TabPageRates = new System.Windows.Forms.TabPage();
            this.ChartControlRates = new AsmodatForexDataManager.UserControls.ChartControl();
            this.RatesIndicatorMain = new AsmodatForexDataManager.UserControls.RatesIndicator();
            this.TabPageTrading = new System.Windows.Forms.TabPage();
            this.AccountIndIicatorInfo = new AsmodatForexDataManager.UserControls.AccountIndicator();
            this.DealsCntrlTradinng = new AsmodatForexDataManager.UserControls.DealsControl();
            this.TradeCntrlProperties = new AsmodatForexDataManager.UserControls.TradingControl();
            this.TabPageAnalysis = new System.Windows.Forms.TabPage();
            this.ChartControlAnalysis = new AsmodatForexDataManager.UserControls.ChartControl();
            this.AnalysisCntrlMain = new AsmodatForexDataManager.UserControls.AnalysisControl();
            this.TbCntrlMain.SuspendLayout();
            this.TabPageCreditals.SuspendLayout();
            this.GpbxAuthentification.SuspendLayout();
            this.TabPageArchives.SuspendLayout();
            this.TabPageRates.SuspendLayout();
            this.TabPageTrading.SuspendLayout();
            this.TabPageAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbxHostName
            // 
            this.TbxHostName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxHostName.Location = new System.Drawing.Point(163, 39);
            this.TbxHostName.Name = "TbxHostName";
            this.TbxHostName.Size = new System.Drawing.Size(263, 31);
            this.TbxHostName.TabIndex = 2;
            this.TbxHostName.TextChanged += new System.EventHandler(this.TbxHostName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(31, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Host Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(31, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ports";
            // 
            // TbxPorts
            // 
            this.TbxPorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxPorts.Location = new System.Drawing.Point(163, 76);
            this.TbxPorts.Name = "TbxPorts";
            this.TbxPorts.Size = new System.Drawing.Size(263, 31);
            this.TbxPorts.TabIndex = 4;
            this.TbxPorts.TextChanged += new System.EventHandler(this.TbxPorts_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(31, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // TbxPassword
            // 
            this.TbxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxPassword.Location = new System.Drawing.Point(163, 150);
            this.TbxPassword.Name = "TbxPassword";
            this.TbxPassword.Size = new System.Drawing.Size(263, 31);
            this.TbxPassword.TabIndex = 8;
            this.TbxPassword.UseSystemPasswordChar = true;
            this.TbxPassword.TextChanged += new System.EventHandler(this.TbxPassword_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(31, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "User ID";
            // 
            // TbxUserID
            // 
            this.TbxUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxUserID.Location = new System.Drawing.Point(163, 113);
            this.TbxUserID.Name = "TbxUserID";
            this.TbxUserID.Size = new System.Drawing.Size(263, 31);
            this.TbxUserID.TabIndex = 6;
            this.TbxUserID.TextChanged += new System.EventHandler(this.TbxUserID_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(31, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "Brand Code";
            // 
            // TbxBrandCode
            // 
            this.TbxBrandCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxBrandCode.Location = new System.Drawing.Point(163, 187);
            this.TbxBrandCode.Name = "TbxBrandCode";
            this.TbxBrandCode.Size = new System.Drawing.Size(263, 31);
            this.TbxBrandCode.TabIndex = 10;
            this.TbxBrandCode.TextChanged += new System.EventHandler(this.TbxBrandCode_TextChanged);
            // 
            // BtnConnect
            // 
            this.BtnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnConnect.Location = new System.Drawing.Point(163, 224);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(263, 68);
            this.BtnConnect.TabIndex = 12;
            this.BtnConnect.Text = "CONNECT";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // TbCntrlMain
            // 
            this.TbCntrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbCntrlMain.Controls.Add(this.TabPageCreditals);
            this.TbCntrlMain.Controls.Add(this.TabPageArchives);
            this.TbCntrlMain.Controls.Add(this.TabPageRates);
            this.TbCntrlMain.Controls.Add(this.TabPageTrading);
            this.TbCntrlMain.Controls.Add(this.TabPageAnalysis);
            this.TbCntrlMain.Location = new System.Drawing.Point(12, 12);
            this.TbCntrlMain.Name = "TbCntrlMain";
            this.TbCntrlMain.SelectedIndex = 0;
            this.TbCntrlMain.Size = new System.Drawing.Size(1363, 737);
            this.TbCntrlMain.TabIndex = 13;
            // 
            // TabPageCreditals
            // 
            this.TabPageCreditals.Controls.Add(this.GpbxAuthentification);
            this.TabPageCreditals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TabPageCreditals.Location = new System.Drawing.Point(4, 22);
            this.TabPageCreditals.Name = "TabPageCreditals";
            this.TabPageCreditals.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageCreditals.Size = new System.Drawing.Size(1355, 711);
            this.TabPageCreditals.TabIndex = 0;
            this.TabPageCreditals.Text = "Creditals";
            this.TabPageCreditals.UseVisualStyleBackColor = true;
            // 
            // GpbxAuthentification
            // 
            this.GpbxAuthentification.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GpbxAuthentification.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.GpbxAuthentification.Controls.Add(this.label2);
            this.GpbxAuthentification.Controls.Add(this.label5);
            this.GpbxAuthentification.Controls.Add(this.TbxPassword);
            this.GpbxAuthentification.Controls.Add(this.BtnConnect);
            this.GpbxAuthentification.Controls.Add(this.TbxUserID);
            this.GpbxAuthentification.Controls.Add(this.label4);
            this.GpbxAuthentification.Controls.Add(this.TbxHostName);
            this.GpbxAuthentification.Controls.Add(this.label3);
            this.GpbxAuthentification.Controls.Add(this.label6);
            this.GpbxAuthentification.Controls.Add(this.TbxBrandCode);
            this.GpbxAuthentification.Controls.Add(this.TbxPorts);
            this.GpbxAuthentification.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GpbxAuthentification.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GpbxAuthentification.Location = new System.Drawing.Point(23, 41);
            this.GpbxAuthentification.Name = "GpbxAuthentification";
            this.GpbxAuthentification.Size = new System.Drawing.Size(467, 327);
            this.GpbxAuthentification.TabIndex = 103;
            this.GpbxAuthentification.TabStop = false;
            this.GpbxAuthentification.Text = "Authentification";
            // 
            // TabPageArchives
            // 
            this.TabPageArchives.Controls.Add(this.ArchiveCntrlMain);
            this.TabPageArchives.Location = new System.Drawing.Point(4, 22);
            this.TabPageArchives.Name = "TabPageArchives";
            this.TabPageArchives.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageArchives.Size = new System.Drawing.Size(1355, 711);
            this.TabPageArchives.TabIndex = 4;
            this.TabPageArchives.Text = "Archives";
            this.TabPageArchives.UseVisualStyleBackColor = true;
            // 
            // ArchiveCntrlMain
            // 
            this.ArchiveCntrlMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ArchiveCntrlMain.Location = new System.Drawing.Point(31, 30);
            this.ArchiveCntrlMain.Name = "ArchiveCntrlMain";
            this.ArchiveCntrlMain.Size = new System.Drawing.Size(336, 492);
            this.ArchiveCntrlMain.TabIndex = 0;
            // 
            // TabPageRates
            // 
            this.TabPageRates.Controls.Add(this.ChartControlRates);
            this.TabPageRates.Controls.Add(this.RatesIndicatorMain);
            this.TabPageRates.Location = new System.Drawing.Point(4, 22);
            this.TabPageRates.Margin = new System.Windows.Forms.Padding(0);
            this.TabPageRates.Name = "TabPageRates";
            this.TabPageRates.Size = new System.Drawing.Size(1355, 711);
            this.TabPageRates.TabIndex = 2;
            this.TabPageRates.Text = "Rates";
            this.TabPageRates.UseVisualStyleBackColor = true;
            // 
            // ChartControlRates
            // 
            this.ChartControlRates.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ChartControlRates.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ChartControlRates.Location = new System.Drawing.Point(481, 19);
            this.ChartControlRates.Margin = new System.Windows.Forms.Padding(0);
            this.ChartControlRates.Name = "ChartControlRates";
            this.ChartControlRates.Size = new System.Drawing.Size(844, 629);
            this.ChartControlRates.TabIndex = 1;
            // 
            // RatesIndicatorMain
            // 
            this.RatesIndicatorMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.RatesIndicatorMain.Location = new System.Drawing.Point(14, 19);
            this.RatesIndicatorMain.Name = "RatesIndicatorMain";
            this.RatesIndicatorMain.Size = new System.Drawing.Size(461, 629);
            this.RatesIndicatorMain.TabIndex = 0;
            // 
            // TabPageTrading
            // 
            this.TabPageTrading.BackColor = System.Drawing.Color.White;
            this.TabPageTrading.Controls.Add(this.AccountIndIicatorInfo);
            this.TabPageTrading.Controls.Add(this.DealsCntrlTradinng);
            this.TabPageTrading.Controls.Add(this.TradeCntrlProperties);
            this.TabPageTrading.Location = new System.Drawing.Point(4, 22);
            this.TabPageTrading.Name = "TabPageTrading";
            this.TabPageTrading.Size = new System.Drawing.Size(1355, 711);
            this.TabPageTrading.TabIndex = 3;
            this.TabPageTrading.Text = "Trading";
            // 
            // AccountIndIicatorInfo
            // 
            this.AccountIndIicatorInfo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.AccountIndIicatorInfo.Location = new System.Drawing.Point(18, 521);
            this.AccountIndIicatorInfo.Name = "AccountIndIicatorInfo";
            this.AccountIndIicatorInfo.Size = new System.Drawing.Size(458, 184);
            this.AccountIndIicatorInfo.TabIndex = 2;
            // 
            // DealsCntrlTradinng
            // 
            this.DealsCntrlTradinng.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DealsCntrlTradinng.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DealsCntrlTradinng.Location = new System.Drawing.Point(499, 17);
            this.DealsCntrlTradinng.Margin = new System.Windows.Forms.Padding(0);
            this.DealsCntrlTradinng.Name = "DealsCntrlTradinng";
            this.DealsCntrlTradinng.Size = new System.Drawing.Size(825, 688);
            this.DealsCntrlTradinng.TabIndex = 1;
            // 
            // TradeCntrlProperties
            // 
            this.TradeCntrlProperties.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.TradeCntrlProperties.Location = new System.Drawing.Point(18, 17);
            this.TradeCntrlProperties.Name = "TradeCntrlProperties";
            this.TradeCntrlProperties.Size = new System.Drawing.Size(458, 498);
            this.TradeCntrlProperties.TabIndex = 0;
            // 
            // TabPageAnalysis
            // 
            this.TabPageAnalysis.Controls.Add(this.ChartControlAnalysis);
            this.TabPageAnalysis.Controls.Add(this.AnalysisCntrlMain);
            this.TabPageAnalysis.Location = new System.Drawing.Point(4, 22);
            this.TabPageAnalysis.Name = "TabPageAnalysis";
            this.TabPageAnalysis.Size = new System.Drawing.Size(1355, 711);
            this.TabPageAnalysis.TabIndex = 5;
            this.TabPageAnalysis.Text = "Analysis";
            this.TabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // ChartControlAnalysis
            // 
            this.ChartControlAnalysis.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ChartControlAnalysis.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ChartControlAnalysis.Location = new System.Drawing.Point(491, 17);
            this.ChartControlAnalysis.Margin = new System.Windows.Forms.Padding(0);
            this.ChartControlAnalysis.Name = "ChartControlAnalysis";
            this.ChartControlAnalysis.Size = new System.Drawing.Size(844, 629);
            this.ChartControlAnalysis.TabIndex = 2;
            // 
            // AnalysisCntrlMain
            // 
            this.AnalysisCntrlMain.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.AnalysisCntrlMain.Location = new System.Drawing.Point(13, 17);
            this.AnalysisCntrlMain.Name = "AnalysisCntrlMain";
            this.AnalysisCntrlMain.Size = new System.Drawing.Size(475, 663);
            this.AnalysisCntrlMain.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 761);
            this.Controls.Add(this.TbCntrlMain);
            this.Name = "Form1";
            this.Text = "Asmodat Forex Data Manager";
            this.TbCntrlMain.ResumeLayout(false);
            this.TabPageCreditals.ResumeLayout(false);
            this.GpbxAuthentification.ResumeLayout(false);
            this.GpbxAuthentification.PerformLayout();
            this.TabPageArchives.ResumeLayout(false);
            this.TabPageRates.ResumeLayout(false);
            this.TabPageTrading.ResumeLayout(false);
            this.TabPageAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TbxHostName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbxPorts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbxPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbxUserID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TbxBrandCode;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.TabControl TbCntrlMain;
        private System.Windows.Forms.TabPage TabPageCreditals;
        private System.Windows.Forms.TabPage TabPageRates;
        private System.Windows.Forms.TabPage TabPageTrading;
        private UserControls.TradingControl TradeCntrlProperties;
        private UserControls.DealsControl DealsCntrlTradinng;
        private UserControls.AccountIndicator AccountIndIicatorInfo;
        private UserControls.RatesIndicator RatesIndicatorMain;
        private System.Windows.Forms.TabPage TabPageArchives;
        private UserControls.ArchiveControl ArchiveCntrlMain;
        private System.Windows.Forms.GroupBox GpbxAuthentification;
        private UserControls.ChartControl ChartControlRates;
        private System.Windows.Forms.TabPage TabPageAnalysis;
        private UserControls.AnalysisControl AnalysisCntrlMain;
        private UserControls.ChartControl ChartControlAnalysis;
    }
}

