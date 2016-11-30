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

using Asmodat.Types;

//using AsmodatForex.com.efxnow.demoweb.tradingservice;
//using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

namespace AsmodatForexDataManager.UserControls
{
    public partial class TradingControl : UserControl
    {
        private void BtnBuy_Click(object sender, EventArgs e)
        {
            this.BuySell(true);
        }

        private void BtnSell_Click(object sender, EventArgs e)
        {
            this.BuySell(false);
        }

        public void BuySell(bool TFBuySell)
        {
                int amount = -1;
                int tolerance = -1;
                int expiration = -1;

                this.Invoke((MethodInvoker)(() =>
                {
                    amount = int.Parse(TbxAmount.Text);
                    tolerance = int.Parse(TbxTolerance.Text);
                    expiration = int.Parse(TbxExpiration.Text) * 1000; //Expiration is passed as [s] but DealRequest is using ms
                }));

                Manager.ForexTrading.DealRequest(TFBuySell, this.GetRate, amount, tolerance, expiration);
        }




    }
}
