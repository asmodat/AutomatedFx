namespace AsmodatForexEngineAPI
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
            this.TimrKAConnection = new System.Windows.Forms.Timer(this.components);
            this.LbxCCYPairs = new System.Windows.Forms.ListBox();
            this.TimrControls = new System.Windows.Forms.Timer(this.components);
            this.BtnBuy = new System.Windows.Forms.Button();
            this.LbxDealsOpened = new System.Windows.Forms.ListBox();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSell = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbxMargin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TbxProfitOrLoss = new System.Windows.Forms.TextBox();
            this.LabelBid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TbxASK = new System.Windows.Forms.TextBox();
            this.TbxBID = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSSPBArchives = new System.Windows.Forms.ToolStripProgressBar();
            this.TSSPBPredictions = new System.Windows.Forms.ToolStripProgressBar();
            this.TsslInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsslInfo2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TimrArchive = new System.Windows.Forms.Timer(this.components);
            this.BtnAutoTrader = new System.Windows.Forms.Button();
            this.TimrAutoTrader = new System.Windows.Forms.Timer(this.components);
            this.TimrAutoTraderSquare = new System.Windows.Forms.Timer(this.components);
            this.LblDeal = new System.Windows.Forms.Label();
            this.TbxDealPair = new System.Windows.Forms.TextBox();
            this.TbxDealKind = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TbxDealProfitOrLoss = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TbxDealPredictionKind = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TbxDealPredictionPercentage = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TbxDealPredictionValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TimrSimulation = new System.Windows.Forms.Timer(this.components);
            this.TimrStartup = new System.Windows.Forms.Timer(this.components);
            this.BtnSaveArchive = new System.Windows.Forms.Button();
            this.BtnSavePredictions = new System.Windows.Forms.Button();
            this.TimrSimulationTest = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimrKAConnection
            // 
            this.TimrKAConnection.Enabled = true;
            this.TimrKAConnection.Interval = 15000;
            this.TimrKAConnection.Tick += new System.EventHandler(this.TimrKAConnection_Tick);
            // 
            // LbxCCYPairs
            // 
            this.LbxCCYPairs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbxCCYPairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LbxCCYPairs.FormattingEnabled = true;
            this.LbxCCYPairs.ItemHeight = 20;
            this.LbxCCYPairs.Location = new System.Drawing.Point(283, 17);
            this.LbxCCYPairs.Name = "LbxCCYPairs";
            this.LbxCCYPairs.Size = new System.Drawing.Size(234, 364);
            this.LbxCCYPairs.TabIndex = 0;
            // 
            // TimrControls
            // 
            this.TimrControls.Interval = 200;
            this.TimrControls.Tick += new System.EventHandler(this.TimrControls_Tick);
            // 
            // BtnBuy
            // 
            this.BtnBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnBuy.Location = new System.Drawing.Point(283, 447);
            this.BtnBuy.Name = "BtnBuy";
            this.BtnBuy.Size = new System.Drawing.Size(106, 49);
            this.BtnBuy.TabIndex = 1;
            this.BtnBuy.Text = "BUY";
            this.BtnBuy.UseVisualStyleBackColor = true;
            this.BtnBuy.Click += new System.EventHandler(this.BtnBuy_Click);
            // 
            // LbxDealsOpened
            // 
            this.LbxDealsOpened.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LbxDealsOpened.FormattingEnabled = true;
            this.LbxDealsOpened.ItemHeight = 20;
            this.LbxDealsOpened.Location = new System.Drawing.Point(528, 17);
            this.LbxDealsOpened.Name = "LbxDealsOpened";
            this.LbxDealsOpened.Size = new System.Drawing.Size(239, 424);
            this.LbxDealsOpened.TabIndex = 2;
            // 
            // BtnClose
            // 
            this.BtnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnClose.Location = new System.Drawing.Point(528, 447);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(239, 49);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "CLOSE";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSell
            // 
            this.BtnSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnSell.Location = new System.Drawing.Point(411, 447);
            this.BtnSell.Name = "BtnSell";
            this.BtnSell.Size = new System.Drawing.Size(106, 49);
            this.BtnSell.TabIndex = 4;
            this.BtnSell.Text = "SELL";
            this.BtnSell.UseVisualStyleBackColor = true;
            this.BtnSell.Click += new System.EventHandler(this.BtnSell_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "CLOSED MARGIN BALANCE";
            // 
            // TbxMargin
            // 
            this.TbxMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxMargin.Location = new System.Drawing.Point(32, 40);
            this.TbxMargin.Name = "TbxMargin";
            this.TbxMargin.ReadOnly = true;
            this.TbxMargin.Size = new System.Drawing.Size(216, 31);
            this.TbxMargin.TabIndex = 6;
            this.TbxMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbxMargin.TextChanged += new System.EventHandler(this.TbxMargin_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(58, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "PROFIT OR LOSS";
            // 
            // TbxProfitOrLoss
            // 
            this.TbxProfitOrLoss.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxProfitOrLoss.Location = new System.Drawing.Point(32, 107);
            this.TbxProfitOrLoss.Name = "TbxProfitOrLoss";
            this.TbxProfitOrLoss.ReadOnly = true;
            this.TbxProfitOrLoss.Size = new System.Drawing.Size(216, 31);
            this.TbxProfitOrLoss.TabIndex = 8;
            this.TbxProfitOrLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelBid
            // 
            this.LabelBid.AutoSize = true;
            this.LabelBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LabelBid.Location = new System.Drawing.Point(444, 387);
            this.LabelBid.Name = "LabelBid";
            this.LabelBid.Size = new System.Drawing.Size(40, 20);
            this.LabelBid.TabIndex = 9;
            this.LabelBid.Text = "BID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(314, 387);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "ASK";
            // 
            // TbxASK
            // 
            this.TbxASK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxASK.Location = new System.Drawing.Point(283, 410);
            this.TbxASK.Name = "TbxASK";
            this.TbxASK.ReadOnly = true;
            this.TbxASK.Size = new System.Drawing.Size(106, 31);
            this.TbxASK.TabIndex = 11;
            this.TbxASK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbxBID
            // 
            this.TbxBID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxBID.Location = new System.Drawing.Point(411, 410);
            this.TbxBID.Name = "TbxBID";
            this.TbxBID.ReadOnly = true;
            this.TbxBID.Size = new System.Drawing.Size(106, 31);
            this.TbxBID.TabIndex = 12;
            this.TbxBID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSPBArchives,
            this.TSSPBPredictions,
            this.TsslInfo,
            this.TsslInfo2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1058, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSSPBArchives
            // 
            this.TSSPBArchives.Name = "TSSPBArchives";
            this.TSSPBArchives.Size = new System.Drawing.Size(100, 16);
            this.TSSPBArchives.Step = 1;
            this.TSSPBArchives.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // TSSPBPredictions
            // 
            this.TSSPBPredictions.Name = "TSSPBPredictions";
            this.TSSPBPredictions.Size = new System.Drawing.Size(100, 16);
            this.TSSPBPredictions.Step = 1;
            this.TSSPBPredictions.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // TsslInfo
            // 
            this.TsslInfo.Name = "TsslInfo";
            this.TsslInfo.Size = new System.Drawing.Size(41, 17);
            this.TsslInfo.Text = "Hello !";
            // 
            // TsslInfo2
            // 
            this.TsslInfo2.Name = "TsslInfo2";
            this.TsslInfo2.Size = new System.Drawing.Size(295, 17);
            this.TsslInfo2.Text = "Please waint, Rates and Settings blotters are loading  : )";
            // 
            // TimrArchive
            // 
            this.TimrArchive.Interval = 30000;
            this.TimrArchive.Tick += new System.EventHandler(this.TimrArchive_Tick);
            // 
            // BtnAutoTrader
            // 
            this.BtnAutoTrader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnAutoTrader.Location = new System.Drawing.Point(21, 410);
            this.BtnAutoTrader.Name = "BtnAutoTrader";
            this.BtnAutoTrader.Size = new System.Drawing.Size(239, 86);
            this.BtnAutoTrader.TabIndex = 14;
            this.BtnAutoTrader.Text = "START AUTO TRADING";
            this.BtnAutoTrader.UseVisualStyleBackColor = true;
            this.BtnAutoTrader.Click += new System.EventHandler(this.BtnAutoTrader_Click);
            // 
            // TimrAutoTrader
            // 
            this.TimrAutoTrader.Interval = 1;
            this.TimrAutoTrader.Tick += new System.EventHandler(this.TimrAutoTrader_Tick);
            // 
            // TimrAutoTraderSquare
            // 
            this.TimrAutoTraderSquare.Interval = 1;
            this.TimrAutoTraderSquare.Tick += new System.EventHandler(this.TimrAutoTraderSquare_Tick);
            // 
            // LblDeal
            // 
            this.LblDeal.AutoSize = true;
            this.LblDeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LblDeal.Location = new System.Drawing.Point(976, 47);
            this.LblDeal.Name = "LblDeal";
            this.LblDeal.Size = new System.Drawing.Size(56, 20);
            this.LblDeal.TabIndex = 15;
            this.LblDeal.Text = " PAIR";
            // 
            // TbxDealPair
            // 
            this.TbxDealPair.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealPair.Location = new System.Drawing.Point(782, 40);
            this.TbxDealPair.Name = "TbxDealPair";
            this.TbxDealPair.ReadOnly = true;
            this.TbxDealPair.Size = new System.Drawing.Size(188, 31);
            this.TbxDealPair.TabIndex = 16;
            this.TbxDealPair.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbxDealKind
            // 
            this.TbxDealKind.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealKind.Location = new System.Drawing.Point(782, 77);
            this.TbxDealKind.Name = "TbxDealKind";
            this.TbxDealKind.ReadOnly = true;
            this.TbxDealKind.Size = new System.Drawing.Size(188, 31);
            this.TbxDealKind.TabIndex = 17;
            this.TbxDealKind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(981, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "KIND";
            // 
            // TbxDealProfitOrLoss
            // 
            this.TbxDealProfitOrLoss.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealProfitOrLoss.Location = new System.Drawing.Point(782, 114);
            this.TbxDealProfitOrLoss.Name = "TbxDealProfitOrLoss";
            this.TbxDealProfitOrLoss.ReadOnly = true;
            this.TbxDealProfitOrLoss.Size = new System.Drawing.Size(188, 31);
            this.TbxDealProfitOrLoss.TabIndex = 19;
            this.TbxDealProfitOrLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(981, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "P or L";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(794, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "PREDICTION [1 min]";
            // 
            // TbxDealPredictionKind
            // 
            this.TbxDealPredictionKind.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealPredictionKind.Location = new System.Drawing.Point(781, 183);
            this.TbxDealPredictionKind.Name = "TbxDealPredictionKind";
            this.TbxDealPredictionKind.ReadOnly = true;
            this.TbxDealPredictionKind.Size = new System.Drawing.Size(188, 31);
            this.TbxDealPredictionKind.TabIndex = 22;
            this.TbxDealPredictionKind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(985, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 20);
            this.label7.TabIndex = 23;
            this.label7.Text = "KIND";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(849, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 20);
            this.label8.TabIndex = 24;
            this.label8.Text = "DEAL";
            // 
            // TbxDealPredictionPercentage
            // 
            this.TbxDealPredictionPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealPredictionPercentage.Location = new System.Drawing.Point(781, 257);
            this.TbxDealPredictionPercentage.Name = "TbxDealPredictionPercentage";
            this.TbxDealPredictionPercentage.ReadOnly = true;
            this.TbxDealPredictionPercentage.Size = new System.Drawing.Size(188, 31);
            this.TbxDealPredictionPercentage.TabIndex = 25;
            this.TbxDealPredictionPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(985, 264);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 20);
            this.label9.TabIndex = 26;
            this.label9.Text = "P. [%]";
            // 
            // TbxDealPredictionValue
            // 
            this.TbxDealPredictionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDealPredictionValue.Location = new System.Drawing.Point(781, 220);
            this.TbxDealPredictionValue.Name = "TbxDealPredictionValue";
            this.TbxDealPredictionValue.ReadOnly = true;
            this.TbxDealPredictionValue.Size = new System.Drawing.Size(188, 31);
            this.TbxDealPredictionValue.TabIndex = 27;
            this.TbxDealPredictionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(974, 227);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "CHANGE";
            // 
            // TimrSimulation
            // 
            this.TimrSimulation.Interval = 1;
            this.TimrSimulation.Tick += new System.EventHandler(this.TimrSimulation_Tick);
            // 
            // TimrStartup
            // 
            this.TimrStartup.Interval = 1000;
            this.TimrStartup.Tick += new System.EventHandler(this.TimrStartup_Tick);
            // 
            // BtnSaveArchive
            // 
            this.BtnSaveArchive.Enabled = false;
            this.BtnSaveArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnSaveArchive.Location = new System.Drawing.Point(32, 183);
            this.BtnSaveArchive.Name = "BtnSaveArchive";
            this.BtnSaveArchive.Size = new System.Drawing.Size(216, 49);
            this.BtnSaveArchive.TabIndex = 29;
            this.BtnSaveArchive.Text = "SAVE ARCHIVE";
            this.BtnSaveArchive.UseVisualStyleBackColor = true;
            this.BtnSaveArchive.Click += new System.EventHandler(this.BtnSaveArchive_Click);
            // 
            // BtnSavePredictions
            // 
            this.BtnSavePredictions.Enabled = false;
            this.BtnSavePredictions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnSavePredictions.Location = new System.Drawing.Point(32, 239);
            this.BtnSavePredictions.Name = "BtnSavePredictions";
            this.BtnSavePredictions.Size = new System.Drawing.Size(216, 49);
            this.BtnSavePredictions.TabIndex = 30;
            this.BtnSavePredictions.Text = "SAVE PREDICTIONS";
            this.BtnSavePredictions.UseVisualStyleBackColor = true;
            this.BtnSavePredictions.Click += new System.EventHandler(this.BtnSavePredictions_Click);
            // 
            // TimrSimulationTest
            // 
            this.TimrSimulationTest.Interval = 1;
            this.TimrSimulationTest.Tick += new System.EventHandler(this.TimrSimulationTest_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 529);
            this.Controls.Add(this.BtnSavePredictions);
            this.Controls.Add(this.BtnSaveArchive);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.TbxDealPredictionValue);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.TbxDealPredictionPercentage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TbxDealPredictionKind);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TbxDealProfitOrLoss);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TbxDealKind);
            this.Controls.Add(this.TbxDealPair);
            this.Controls.Add(this.LblDeal);
            this.Controls.Add(this.BtnAutoTrader);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.TbxBID);
            this.Controls.Add(this.TbxASK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LabelBid);
            this.Controls.Add(this.TbxProfitOrLoss);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TbxMargin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSell);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.LbxDealsOpened);
            this.Controls.Add(this.BtnBuy);
            this.Controls.Add(this.LbxCCYPairs);
            this.Name = "Form1";
            this.Text = "Asmodat Forex API Engine (Alpha)";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer TimrKAConnection;
        private System.Windows.Forms.ListBox LbxCCYPairs;
        private System.Windows.Forms.Timer TimrControls;
        private System.Windows.Forms.Button BtnBuy;
        private System.Windows.Forms.ListBox LbxDealsOpened;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnSell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbxMargin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbxProfitOrLoss;
        private System.Windows.Forms.Label LabelBid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbxASK;
        private System.Windows.Forms.TextBox TbxBID;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer TimrArchive;
        private System.Windows.Forms.Button BtnAutoTrader;
        private System.Windows.Forms.Timer TimrAutoTrader;
        private System.Windows.Forms.Timer TimrAutoTraderSquare;
        private System.Windows.Forms.ToolStripStatusLabel TsslInfo2;
        private System.Windows.Forms.Label LblDeal;
        private System.Windows.Forms.TextBox TbxDealPair;
        private System.Windows.Forms.TextBox TbxDealKind;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbxDealProfitOrLoss;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TbxDealPredictionKind;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TbxDealPredictionPercentage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TbxDealPredictionValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer TimrSimulation;
        private System.Windows.Forms.Timer TimrStartup;
        private System.Windows.Forms.Button BtnSaveArchive;
        private System.Windows.Forms.Button BtnSavePredictions;
        private System.Windows.Forms.ToolStripProgressBar TSSPBPredictions;
        private System.Windows.Forms.ToolStripProgressBar TSSPBArchives;
        private System.Windows.Forms.ToolStripStatusLabel TsslInfo;
        private System.Windows.Forms.Timer TimrSimulationTest;
    }
}

