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
    public partial class AccountIndicator : UserControl
    {
        public AccountIndicator()
        {
            InitializeComponent();
        }


        private Manager Manager;
        private ThreadedTimers Timers = new ThreadedTimers();

        public void Feed(ref Manager Manager)
        {
            this.Manager = Manager;
            Timers.Run(() => PeacemakerUpdate(), 1000, null, true, true, true);
        }


        private void PeacemakerUpdate()
        {
            if (Manager == null || Manager.ForexTrading == null || !Manager.ForexTrading.AccountLogs.IsValid) return;

            Account account = Manager.ForexTrading.Account;
            char punctuation = ',';
            int decimals = 2;

            double origin = account.OriginBalance;
            double closed = account.ClosedBalance;
            double realized = account.RealizedBalance;
            double live = account.LiveProfit;

            this.Invoke((MethodInvoker)(() =>
            {
                
                TbxOriginBalance.Text = Doubles.ToString(account.OriginBalance, "???", decimals, 0, double.MaxValue, punctuation);
                FormsControls.ColourDouble(ref TbxClosedBalance, account.ClosedBalance, account.OriginBalance, "???", decimals, 0, double.MaxValue, punctuation);
                FormsControls.ColourDouble(ref TbxRealizedBalance, account.RealizedBalance, account.ClosedBalance, "???", decimals, 0, double.MaxValue, punctuation, 0.001);
                FormsControls.ColourDouble(ref TbxLiveProfit, account.LiveProfit, "???", decimals, double.MinValue, double.MaxValue, punctuation);

            }));
        }
    }
}




        //private void ColourDouble(ref TextBox Tbx, double value, string exception, int decimals, double min, double max, char punctuation)
        //{
        //    Tbx.Text = Doubles.ToString(value, exception, decimals, min, max, punctuation);

        //    if (Tbx.Text == exception)
        //        Tbx.ForeColor = Color.Black;
        //    else if (value < 0)
        //        Tbx.ForeColor = Color.DarkRed;
        //    else if (value == 0)
        //        Tbx.ForeColor = Color.DarkBlue;
        //    else if (value > 0)
        //        Tbx.ForeColor = Color.DarkGreen;
        //}

        //private void ColourComparer(ref TextBox Tbx, double v1, double v2, string exception, int decimals, double min, double max, char punctuation)
        //{
        //    Tbx.Text = Doubles.ToString(v1, exception, decimals, min, max, punctuation);

        //    if (Tbx.Text == exception)
        //        Tbx.ForeColor = Color.Black;
        //    else if (v1 < v2)
        //        Tbx.ForeColor = Color.DarkRed;
        //    else if (v1 == v2)
        //        Tbx.ForeColor = Color.DarkBlue;
        //    else if (v1 > v2)
        //        Tbx.ForeColor = Color.DarkGreen;
        //}