using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

using Asmodat.IO;

namespace AsmodatForex
{
    public partial class ServiceTrading
    {
        private ThreadedDictionary<TickTime, Account> DataAccount = new ThreadedDictionary<TickTime, Account>("AccountLog", true);

        private Account _Account = null;
        public Account Account
        {
            get
            {
                return _Account;
            }
            private set
            {
                _Account = value;
            }
        }

        public ThreadedDictionary<TickTime, Account> AccountLogs
        {
            get
            {
                lock (DataAccount)
                {
                    return DataAccount;
                } 
            }
        }

        public  Account AccountLog
        {
            get
            {
                lock (DataAccount)
                {
                    return DataAccount.Last().Value;
                }
            }
            private set
            {
                lock (DataAccount)
                {
                    TickTime key = DataAccount.Last().Key;
                    DataAccount[key] = value;
                }
            }
        }

        //public ThreadedDictionary<TickTime, Account> AccountLog
        //{
        //    get
        //    {
        //        lock (DataAccount)
        //        {
        //            return DataAccount;
        //        }
        //    }
        //}


        public void PeacemakerAccount()
        {
            this.UpdateAccount();

        }

        public void UpdateAccount()
        {
            if (!ForexAuthentication.Connected || !ForexRates.Loaded || (DataAccount.IsValid && DataAccount.UpdateSpan < 1000))
                return;

            Margin marg = null;
            try
            {
                marg = CEDTS_TradingService.GetMarginBlotter(ForexAuthentication.Token).Output[0];
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
                return;
            }

            Account current = AccountInfo.ToAccount(marg);

            current.MarginOrigin = TickTime.Now;

            if (DataAccount.Count <= 0)
            {
                if (current.OpenPosition == 0)
                {
                    current.ClosedBalance = current.MarginBalance;
                    current.OriginBalance = current.MarginBalance;
                }
                else throw new Exception("Archive log must contain at least one closed record !");
            }
            else
            {
                current.ClosedBalance = this.GetClosedMarginBalance();
                current.OriginBalance = this.GetOriginMarginBalance();
            }

            if (DataAccount.Count <= 0 || (current.OpenPosition == 0 && !AccountInfo.EqualsPosition(current, AccountLog)))
            lock (DataAccount)
            {
                DataAccount.IsValid = false;
                DataAccount.Add(current.MarginOrigin, current); //only closed position should be saved
                DataAccount.SaveFile(false);
            }

            current.LiveProfit = this.GetLiveProfit();
            Account = current; //Account.LiveOrigin = TickTime.Now;
            DataAccount.IsValid = true;
        }

        public double GetClosedMarginBalance()
        {
            Account[] Accounts = DataAccount.Values.ToArray();

            if (Accounts != null)
            for(int i = (Accounts.Length - 1); i >= 0; i--)
            {
                if (Accounts[i].OpenPosition == 0)
                    return Accounts[i].MarginBalance;
            }

            throw new Exception("GetClosedMarginBalance, margin balance for 0 position not found.");
        }

        public double GetOriginMarginBalance()
        {
            Account[] Accounts = DataAccount.Values.ToArray();

            if (Accounts != null)
            for (int i = 0; i < Accounts.Length; i++)
            {
                if (Accounts[i].OpenPosition == 0)
                    return Accounts[i].MarginBalance;
            }

            throw new Exception("GetOriginMarginBalance, margin balance for 0 position not found.");
        }

        public double GetLiveProfit()
        {
            string[] confirmations = DataDeals.Keys.ToArray();

            double profit = 0;
            foreach (string confirmation in confirmations)
                profit += this.GetLiveProfit(confirmation);

            return profit;
        }
        public double GetLiveProfit(string confirmation)
        {
            Deal deal = DataDeals[confirmation];
            Rate current = ForexRates.Data[deal.Product];

            double profit = 0;
            double previous = Doubles.ParseAny(deal.Rate);

            if (deal.BuySell == "B")
            {
                profit += current.BID - previous;
            }
            else if (deal.BuySell == "S")
            {
                profit += current.OFFER - previous;
            }
            else throw new Exception("GetLiveProfit unknown deal BuySell format !");

            profit *= int.Parse(deal.Contract);

            return profit;
        }

    }
}
