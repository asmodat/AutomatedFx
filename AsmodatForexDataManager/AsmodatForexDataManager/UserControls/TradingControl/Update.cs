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
        public void UpdateItemsProducts()
        {
            if (Manager == null || !Manager.IsLoadedConfiguration) return;

            Dictionary<string, ProductSetting> Settings = Manager.ForexConfiguration.ProductSettings;
            List<string> Products = new List<string>();
            bool active = false;
            bool popular = false;

            this.Invoke((MethodInvoker)(() =>
            {
                active = ChbxActive.Checked;
                popular = ChbxPopular.Checked;
            }));

            foreach (KeyValuePair<string, ProductSetting> KVP in Settings)
            {
                string product = KVP.Key;
                ProductSetting settings = KVP.Value;

                if (active && !settings.Active)
                    continue;

                if (popular && !settings.IsPopularMarket)
                    continue;

                //if (!settings.QuickPair || !settings.Subscribed)  continue;

                Products.Add(product);
            }

            if (Products.Count <= 0)
                return;

            this.Invoke((MethodInvoker)(() =>
            {
                string sProduct = CmbxProduct.Text;
                CmbxProduct.Items.Clear();
                CmbxProduct.Items.AddRange(Products.ToArray());
                if (!CmbxProduct.Items.Contains(sProduct))
                    CmbxProduct.SelectedIndex = 0;
                else CmbxProduct.Text = sProduct;


            
            }));
        }

        public void UpdateDealControls()
        {
            Rate rate = this.GetRate;
            if (rate == null) return;

            
            int iDecimals = Manager.ForexConfiguration.GetDecimals(rate.Pair);// rate.DECIMALS;
            int iTolerance = (Manager.ForexConfiguration.GetTolerance(rate.Pair) * 5);

            this.Invoke((MethodInvoker)(() =>
            {
                TbxDateTime.Text = rate.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if (iDecimals >= 0)
                {
                    TbxASK.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OFFER, "???", iDecimals, 0, double.MaxValue);
                    TbxBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.BID, "???", iDecimals, 0, double.MaxValue);
                }
                else
                {
                    TbxASK.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OFFER, "???", 0, double.MaxValue);
                    TbxBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.BID, "???", 0, double.MaxValue);
                }

                TbxTolerance.Text =  iTolerance + "";
            }));

        }


        /// <summary>
        /// This property returns Rate selected from Gpbx, or null if its not loded yet or manager is not ready
        /// </summary>
        public Rate GetRate
        {
            get
            {
                string product = null;
                GpbxRateProperties.Invoke((MethodInvoker)(() => { product = CmbxProduct.Text; }));

                if (System.String.IsNullOrEmpty(product) ||
                    !Manager.IsLoadedConfiguration ||
                    !Manager.IsLoadedRate ||
                    !Manager.ForexRates.Data.ContainsKey(product) ||
                    !Manager.ForexConfiguration.ProductSettings.ContainsKey(product)) return null;

                //ProductSetting settings = Manager.ForexConfiguration.ProductSettings[product];
                return Manager.ForexRates.Data[product];
            }
        }
    }
}
