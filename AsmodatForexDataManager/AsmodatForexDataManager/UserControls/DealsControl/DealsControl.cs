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

namespace AsmodatForexDataManager.UserControls
{
    public partial class DealsControl : UserControl
    {
        public DealsControl()
        {
            InitializeComponent();

            object[,] TextColumns = new object[,] 
            { 
                {"Product", "Product", ThreadedDataGridView.Tags.Product}, 
                {"Position", "Position", null},
                {"Contract", "Contract", null},
                {"Rate", "Rate", null},
                {"Now", "Now", null},
                {"Profit", "Profit", null},
                {"Date", "Date", null},
                {"Confirmation", "Confirmation", ThreadedDataGridView.Tags.Key}
            };

            TdgvDeals.AddTextColumns(TextColumns, false);
            //TdgvDeals.AddButtonColumns("Close", "Close", ThreadedDataGridView.KeyTag, false);
            TdgvDeals.ClearRows(false);

            TdgvDeals.VisibleColumn("Confirmation", false, false);
            //TdgvDeals.VisibleColumn(" ", false, false);

            TdgvDeals.DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            TdgvDeals.AutosizeColumnMode("Date", DataGridViewAutoSizeColumnMode.Fill, false);
         

            this.BtnCloseSelected.Click += BtnCloseSelected_Click;
            this.BtnLiquidateAll.Click += BtnLiquidateAll_Click;
            this.BtnClosePosition.Click += BtnClosePosition_Click;
            
        }



        
        private Manager Manager = null;
        private ThreadedTimers Timers = new ThreadedTimers();

        public void Feed(ref Manager Manager)
        {
            this.Manager = Manager;
            Timers.Run(() => PeacemakerUpdate(), 1000, null, true, true, true);
        }


        

        



        

        public void PeacemakerUpdate()
        {
            //if (!Manager.IsLoadedConfiguration) return;
            if (Manager == null || Manager.ForexTrading == null || !Manager.IsLoadedConfiguration || !Manager.ForexTrading.Deals.IsValid || !Manager.ForexRates.Loaded) return;


            //TdgvDeals.ClearRows();

            List<object[]> Rows = new List<object[]>();
            
            foreach(Deal deal in Manager.ForexTrading.Deals.Values.ToArray())
                Rows.Add(ToObjectList(deal));

            TdgvDeals.AddOrUpdateRows(Rows, false);
        }

        /// <summary>
        ///         {"Product", "Product"}, 
        ///        {"Amount", "Amount"},
        ///        {"Position", "Position"},
        ///        {"Change", "Change"},
        ///        {"Date", "Date"},
        /// </summary>
        /// <param name="deal"></param>
        private object[] ToObjectList(Deal deal)
        {
            if (deal == null) return null;

            string BuySell = (deal.BuySell + "").ToUpper();
            double profit = Manager.ForexTrading.GetLiveProfit(deal.ConfirmationNumber);

            List<object> objects = new List<object>();
            objects.Add(deal.Product);
            

            double now = -1;
            if (BuySell == "B")
            {
                objects.Add(" Long ");
                now = Manager.ForexRates.Data[deal.Product].BID;
            }
            else if (BuySell == "S")
            {
                objects.Add(" Short ");
                now = Manager.ForexRates.Data[deal.Product].OFFER;
            }
            else objects.Add(" ??? ");

            objects.Add(deal.Contract);
            objects.Add(deal.Rate);


            //Manager.ForexRates.Data[deal.Product].DECIMALS
            if (now <= 0) objects.Add(" ??? ");
            else objects.Add(Doubles.ToString(now, "???", Manager.ForexConfiguration.GetDecimals(deal.Product), double.MinValue, double.MaxValue, ','));
            
           
            objects.Add(Doubles.ToString(profit, "???", 2, double.MinValue, double.MaxValue, ','));
            objects.Add(deal.DealDate);// objects.Add(deal.DealDate.Substring(0, deal.DealDate.Length - 4));
            objects.Add(deal.ConfirmationNumber);


            return objects.ToArray();
        }



    }
}
