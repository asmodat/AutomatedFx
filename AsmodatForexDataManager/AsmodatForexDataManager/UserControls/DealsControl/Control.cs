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


using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.FormsControls;

using Asmodat.Types;

namespace AsmodatForexDataManager.UserControls
{
    public partial class DealsControl : UserControl
    {
        void BtnCloseSelected_Click(object sender, EventArgs e)
        {
            List<object> Values = TdgvDeals.GetSelectedRowsValues(ThreadedDataGridView.Tags.Key);
            if (Values == null ? false : Values.Count <= 0)
                return;

            foreach (object obj in Values)
            {
                Manager.ForexTrading.DealClose(obj.ToString(), int.MaxValue, 1000);

            }

        }

        void BtnLiquidateAll_Click(object sender, EventArgs e)
        {
            DealRequest DRequest = new DealRequest(1000);

            DRequest.LiquidateAll = true;

            Manager.ForexTrading.Save(DRequest);
        }

        void BtnClosePosition_Click(object sender, EventArgs e)
        {


            List<object> Values = TdgvDeals.GetSelectedRowsValues(ThreadedDataGridView.Tags.Product);
            if (Values == null ? false : Values.Count <= 0)
                return;

            Values = Values.Distinct().ToList();

            foreach (object obj in Values)
            {
                DealRequest DRequest = new DealRequest(1000);
                DRequest.Product = obj.ToString();
                DRequest.ClosePosition = true;
                Manager.ForexTrading.Save(DRequest);
            }


            

            
        }


    }
}
