using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;

using System.Runtime.Serialization;

using AsmodatForex.com.efxnow.demoweb.tradingservice;

using Asmodat.Abbreviate;

namespace AsmodatForex
{
    public static class AccountInfo
    {
        public static Account ToAccount(Margin margin)
        {
            return AccountInfo.Merge(new Account(), margin);
        }

        public static Account Merge(Account account, Margin margin)
        {
            account.AvailablePosition = Doubles.ParseAny(margin.AvailablePosition, 0);
            account.MarginBalance = Doubles.ParseAny(margin.MarginBalance, 0);
            account.MarginFactor = Doubles.ParseAny(margin.MarginFactor, 0);
            account.MaxDealAvailable = Doubles.ParseAny(margin.MaxDealAvailable, 0);
            account.OpenPosition = Doubles.ParseAny(margin.OpenPosition, 0);
            account.PostedMargin = Doubles.ParseAny(margin.PostedMargin, 0);
            account.RealizedProfit = Doubles.ParseAny(margin.RealizedProfit, 0);
            account.UnrealizedProfit = Doubles.ParseAny(margin.UnrealizedProfit, 0);
            account.USDPostedMargin = Doubles.ParseAny(margin.USDPostedMargin, 0);
            account.USDRealizedProfit = Doubles.ParseAny(margin.USDRealizedProfit, 0);

            return account;
        }

        public static Account Merge(Account account, List<Deal> deals)
        {



            return account;
        }

        public static bool EqualsMargin(Account acc1, Account acc2)
        {
            if (acc1.AvailablePosition != acc2.AvailablePosition) return false;
            if (acc1.MarginBalance != acc2.MarginBalance) return false;
            if (acc1.MarginFactor != acc2.MarginFactor) return false;
            if (acc1.MaxDealAvailable != acc2.MaxDealAvailable) return false;
            if (acc1.OpenPosition != acc2.OpenPosition) return false;
            if (acc1.PostedMargin != acc2.PostedMargin) return false;
            if (acc1.RealizedProfit != acc2.RealizedProfit) return false;
            if (acc1.UnrealizedProfit != acc2.UnrealizedProfit) return false;
            if (acc1.USDPostedMargin != acc2.USDPostedMargin) return false;
            if (acc1.USDRealizedProfit != acc2.USDRealizedProfit) return false;

            return true;
        }

        public static bool EqualsPosition(Account acc1, Account acc2)
        {
            //if (acc1 == null && acc2 == null) throw new Exception
            //if (acc1 == null || acc2 == null) return false;
            if (acc1.MarginBalance != acc2.MarginBalance) return false;
            if (acc1.OpenPosition != acc2.OpenPosition) return false;
            
            return true;
        }
    }
}
