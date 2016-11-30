using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForex;
using Asmodat.Abbreviate;

//using AsmodatForex.com.efxnow.demoweb.tradingservice;
//using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

namespace AsmodatForexDataManager.UserControls
{
    public partial class TradingControl : UserControl
    {
        public TradingControl()
        {
            InitializeComponent();

            //this.CmbxPair.MouseEnter += CmbxPair_MouseEnter;

            this.CmbxProduct.MouseEnter += CmbxProduct_MouseEnter;
            TbxLots.Text = 1 + "";
            TbxTolerance.Text = 5 + "";

            
        }

        


        private Manager Manager;
        private ThreadedTimers Timers = new ThreadedTimers();

        public void Feed(ref Manager Manager)
        {
            this.Manager = Manager;
            Timers.Run(() => MainTimer(), 200, null, true, true, true);
        }

        void CmbxProduct_MouseEnter(object sender, EventArgs e)
        {
            this.UpdateItemsProducts();
        }


        
        public void MainTimer()
        {
            if (Manager == null || !Manager.IsLoadedConfiguration) return;

            UpdateDealControls();

            if (FormsControls.Invoke(() => CmbxProduct.Items.Count) <= 0)//if (CmbxProduct.Items.Count <= 0)//
                this.UpdateItemsProducts();

        }


        

        private void CmbxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string product = CmbxProduct.Text;
            ProductSetting settings = Manager.ForexConfiguration.ProductSettings[product];

            this.UpdateAmount();
        }

        private void TbxLots_TextChanged(object sender, EventArgs e)
        {
           TbxLots.Text = Asmodat.Abbreviate.Integer.Parse(TbxLots.Text, 1, 1, int.MaxValue).ToString();

           this.UpdateAmount();
        }

        private void TbxTolerance_TextChanged(object sender, EventArgs e)
        {
            TbxTolerance.Text = Asmodat.Abbreviate.Integer.Parse(TbxTolerance.Text, 1, 1, int.MaxValue).ToString();
        }

        private void TbxExpiration_TextChanged(object sender, EventArgs e)
        {
            TbxExpiration.Text = Asmodat.Abbreviate.Integer.Parse(TbxExpiration.Text, 1, 1, int.MaxValue).ToString();
        }


        /// <summary>
        /// This function updates amount textbox (TbxAmount.Text) on TradingControl based on current product settings
        /// </summary>
        private void UpdateAmount()
        {
            string product = CmbxProduct.Text;

            if (System.String.IsNullOrEmpty(product) || Manager == null || !Manager.IsLoadedConfiguration) return;

            ProductSetting settings = Manager.ForexConfiguration.ProductSettings[product];
            int lots = Asmodat.Abbreviate.Integer.Parse(TbxLots.Text, 1, 1, int.MaxValue);

            TbxAmount.Text = (lots * int.Parse(settings.OrderSize)).ToString();
        }

        
        
    }
}
