namespace AsmodatForexDataManager.UserControls
{
    partial class TradingControl
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
            this.GpbxRateProperties = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TbxExpiration = new System.Windows.Forms.TextBox();
            this.TbxAmount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ChbxPopular = new System.Windows.Forms.CheckBox();
            this.ChbxActive = new System.Windows.Forms.CheckBox();
            this.BtnSell = new System.Windows.Forms.Button();
            this.BtnBuy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbxBID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TbxASK = new System.Windows.Forms.TextBox();
            this.CmbxProduct = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.TbxDateTime = new System.Windows.Forms.TextBox();
            this.TbxLots = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TbxTolerance = new System.Windows.Forms.TextBox();
            this.GpbxRateProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpbxRateProperties
            // 
            this.GpbxRateProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GpbxRateProperties.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.GpbxRateProperties.Controls.Add(this.label5);
            this.GpbxRateProperties.Controls.Add(this.TbxExpiration);
            this.GpbxRateProperties.Controls.Add(this.TbxAmount);
            this.GpbxRateProperties.Controls.Add(this.label4);
            this.GpbxRateProperties.Controls.Add(this.ChbxPopular);
            this.GpbxRateProperties.Controls.Add(this.ChbxActive);
            this.GpbxRateProperties.Controls.Add(this.BtnSell);
            this.GpbxRateProperties.Controls.Add(this.BtnBuy);
            this.GpbxRateProperties.Controls.Add(this.label1);
            this.GpbxRateProperties.Controls.Add(this.TbxBID);
            this.GpbxRateProperties.Controls.Add(this.label2);
            this.GpbxRateProperties.Controls.Add(this.TbxASK);
            this.GpbxRateProperties.Controls.Add(this.CmbxProduct);
            this.GpbxRateProperties.Controls.Add(this.label3);
            this.GpbxRateProperties.Controls.Add(this.label15);
            this.GpbxRateProperties.Controls.Add(this.TbxDateTime);
            this.GpbxRateProperties.Controls.Add(this.TbxLots);
            this.GpbxRateProperties.Controls.Add(this.label13);
            this.GpbxRateProperties.Controls.Add(this.label6);
            this.GpbxRateProperties.Controls.Add(this.TbxTolerance);
            this.GpbxRateProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GpbxRateProperties.Location = new System.Drawing.Point(3, 3);
            this.GpbxRateProperties.Name = "GpbxRateProperties";
            this.GpbxRateProperties.Size = new System.Drawing.Size(450, 492);
            this.GpbxRateProperties.TabIndex = 99;
            this.GpbxRateProperties.TabStop = false;
            this.GpbxRateProperties.Text = "Trade Request";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(4, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 25);
            this.label5.TabIndex = 102;
            this.label5.Text = "Expiration [s]";
            // 
            // TbxExpiration
            // 
            this.TbxExpiration.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxExpiration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxExpiration.Location = new System.Drawing.Point(156, 89);
            this.TbxExpiration.Name = "TbxExpiration";
            this.TbxExpiration.Size = new System.Drawing.Size(279, 31);
            this.TbxExpiration.TabIndex = 101;
            this.TbxExpiration.Text = "1";
            this.TbxExpiration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbxExpiration.TextChanged += new System.EventHandler(this.TbxExpiration_TextChanged);
            // 
            // TbxAmount
            // 
            this.TbxAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxAmount.Location = new System.Drawing.Point(156, 274);
            this.TbxAmount.Name = "TbxAmount";
            this.TbxAmount.ReadOnly = true;
            this.TbxAmount.Size = new System.Drawing.Size(279, 31);
            this.TbxAmount.TabIndex = 99;
            this.TbxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(56, 280);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 25);
            this.label4.TabIndex = 100;
            this.label4.Text = "Amount";
            // 
            // ChbxPopular
            // 
            this.ChbxPopular.AutoSize = true;
            this.ChbxPopular.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChbxPopular.Location = new System.Drawing.Point(36, 402);
            this.ChbxPopular.Name = "ChbxPopular";
            this.ChbxPopular.Size = new System.Drawing.Size(94, 28);
            this.ChbxPopular.TabIndex = 98;
            this.ChbxPopular.Text = "Popular";
            this.ChbxPopular.UseVisualStyleBackColor = true;
            // 
            // ChbxActive
            // 
            this.ChbxActive.AutoSize = true;
            this.ChbxActive.Checked = true;
            this.ChbxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChbxActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChbxActive.Location = new System.Drawing.Point(37, 362);
            this.ChbxActive.Name = "ChbxActive";
            this.ChbxActive.Size = new System.Drawing.Size(80, 28);
            this.ChbxActive.TabIndex = 97;
            this.ChbxActive.Text = "Active";
            this.ChbxActive.UseVisualStyleBackColor = true;
            // 
            // BtnSell
            // 
            this.BtnSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnSell.Location = new System.Drawing.Point(156, 402);
            this.BtnSell.Name = "BtnSell";
            this.BtnSell.Size = new System.Drawing.Size(279, 68);
            this.BtnSell.TabIndex = 96;
            this.BtnSell.Text = "SELL";
            this.BtnSell.UseVisualStyleBackColor = true;
            this.BtnSell.Click += new System.EventHandler(this.BtnSell_Click);
            // 
            // BtnBuy
            // 
            this.BtnBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnBuy.Location = new System.Drawing.Point(156, 322);
            this.BtnBuy.Name = "BtnBuy";
            this.BtnBuy.Size = new System.Drawing.Size(279, 68);
            this.BtnBuy.TabIndex = 95;
            this.BtnBuy.Text = "BUY";
            this.BtnBuy.UseVisualStyleBackColor = true;
            this.BtnBuy.Click += new System.EventHandler(this.BtnBuy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(55, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 66;
            this.label1.Text = "Product";
            // 
            // TbxBID
            // 
            this.TbxBID.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxBID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxBID.Location = new System.Drawing.Point(156, 163);
            this.TbxBID.Name = "TbxBID";
            this.TbxBID.Size = new System.Drawing.Size(279, 31);
            this.TbxBID.TabIndex = 67;
            this.TbxBID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(95, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 25);
            this.label2.TabIndex = 68;
            this.label2.Text = "BID";
            // 
            // TbxASK
            // 
            this.TbxASK.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxASK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxASK.Location = new System.Drawing.Point(156, 126);
            this.TbxASK.Name = "TbxASK";
            this.TbxASK.Size = new System.Drawing.Size(279, 31);
            this.TbxASK.TabIndex = 69;
            this.TbxASK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CmbxProduct
            // 
            this.CmbxProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbxProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CmbxProduct.FormattingEnabled = true;
            this.CmbxProduct.Location = new System.Drawing.Point(156, 13);
            this.CmbxProduct.Name = "CmbxProduct";
            this.CmbxProduct.Size = new System.Drawing.Size(279, 33);
            this.CmbxProduct.TabIndex = 94;
            this.CmbxProduct.SelectedIndexChanged += new System.EventHandler(this.CmbxProduct_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(87, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 25);
            this.label3.TabIndex = 70;
            this.label3.Text = "ASK";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label15.Location = new System.Drawing.Point(31, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(110, 25);
            this.label15.TabIndex = 93;
            this.label15.Text = "Date Time";
            // 
            // TbxDateTime
            // 
            this.TbxDateTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxDateTime.Location = new System.Drawing.Point(156, 52);
            this.TbxDateTime.Name = "TbxDateTime";
            this.TbxDateTime.Size = new System.Drawing.Size(279, 31);
            this.TbxDateTime.TabIndex = 92;
            this.TbxDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbxLots
            // 
            this.TbxLots.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxLots.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxLots.Location = new System.Drawing.Point(156, 237);
            this.TbxLots.Name = "TbxLots";
            this.TbxLots.Size = new System.Drawing.Size(279, 31);
            this.TbxLots.TabIndex = 75;
            this.TbxLots.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbxLots.TextChanged += new System.EventHandler(this.TbxLots_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(33, 206);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 25);
            this.label13.TabIndex = 88;
            this.label13.Text = "Tolerance";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(88, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 25);
            this.label6.TabIndex = 76;
            this.label6.Text = "Lots";
            // 
            // TbxTolerance
            // 
            this.TbxTolerance.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TbxTolerance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TbxTolerance.Location = new System.Drawing.Point(156, 200);
            this.TbxTolerance.Name = "TbxTolerance";
            this.TbxTolerance.Size = new System.Drawing.Size(279, 31);
            this.TbxTolerance.TabIndex = 87;
            this.TbxTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbxTolerance.TextChanged += new System.EventHandler(this.TbxTolerance_TextChanged);
            // 
            // TradingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.GpbxRateProperties);
            this.Name = "TradingControl";
            this.Size = new System.Drawing.Size(456, 498);
            this.GpbxRateProperties.ResumeLayout(false);
            this.GpbxRateProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GpbxRateProperties;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbxBID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbxASK;
        private System.Windows.Forms.ComboBox CmbxProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox TbxDateTime;
        private System.Windows.Forms.TextBox TbxLots;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TbxTolerance;
        private System.Windows.Forms.Button BtnSell;
        private System.Windows.Forms.Button BtnBuy;
        private System.Windows.Forms.CheckBox ChbxPopular;
        private System.Windows.Forms.CheckBox ChbxActive;
        private System.Windows.Forms.TextBox TbxAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbxExpiration;
    }
}
