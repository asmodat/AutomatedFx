using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatForexEngineAPI
{
    public class Account
    {
        private int iLeverage = 200;
        public int Leverage
        {
            get
            {
                return iLeverage;
            }
        }



        public int OpeningHazard = 5;
        public int SqueringHazard = 5;
        //AsmodatForexEngineAPI.DataBase DATABASE;

        private double AllDealsClosedBalanceNew;
        private double AllDealsClosedBalanceOld;

        //To set value OpenDealBlotter Count should be zero
        public double ClosedBalance
        {
            get
            {
                return AllDealsClosedBalanceNew;
            }
            set
            {
                AllDealsClosedBalanceOld = AllDealsClosedBalanceNew;
                AllDealsClosedBalanceNew = value;
            }
        }
        public double ClosedBalanceOld
        {
            get
            {
                return AllDealsClosedBalanceOld;
            }
        }
        public double CalculateProfitOrLoss(Deals DEAL, Rates RATE)
        {
            double MarginBalance = 0;
            if (RATE == null || DEAL == null)
            {
                return MarginBalance;
            }

            

            double rateOld = DEAL.RATE;
            double rateNew;
            if (DEAL.BUY)
                rateNew = RATE.BID;
            else
                rateNew = RATE.ASK;

            //double OnePip = Math.Pow(10, -RATE.Decimals);
            //double PipValue = (OnePip / rateCurrent) * CONTRACT;
            //double SpreadRatePips = Math.Round((rateCurrent - rateOld) / OnePip, 0);
            //double ProfitOrLost = SpreadRatePips * PipValue;// *rateCurrent; WTF ?


            double spread = Math.Round((rateNew - rateOld),RATE.Decimals);
            double ProfitOrLost = DEAL.CONTRACT * spread;


            MarginBalance = ProfitOrLost;

            return MarginBalance;
        }

    }
}
