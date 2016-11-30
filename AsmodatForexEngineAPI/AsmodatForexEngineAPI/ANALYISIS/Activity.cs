using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public partial class Analysis
    {

        public int SpreadPips(Deals DEAL, double decimals, bool hazard)
        {
            
            double dSpread = DEAL.ASK - DEAL.BID;
            double pipValue = Math.Pow(10, -decimals);

            if (hazard)
            {
                dSpread += ACCOUNT.OpeningHazard * pipValue;
                dSpread += ACCOUNT.SqueringHazard * pipValue;
            }

            return (int)(dSpread / pipValue);
        }

        public int SpreadPips(Rates RATE, bool hazard)
        {
            double dSpread = RATE.Spread;
            double pipValue = Math.Pow(10, -RATE.Decimals);

            if (hazard)
            {
                dSpread += ACCOUNT.OpeningHazard * pipValue;
                dSpread += ACCOUNT.SqueringHazard * pipValue;
            }

            return (int)(dSpread / pipValue);
        }
        public double Spread(Rates RATE)
        {
            double dSpread = RATE.Spread;
            double pipValue = Math.Pow(10, -RATE.Decimals);
            dSpread += ACCOUNT.OpeningHazard * pipValue;
            dSpread += ACCOUNT.SqueringHazard * pipValue;

            return dSpread;// (int)(dSpread / pipValue);
        }

        public double Spread(Rates RATE, int pipsGain, bool hazard)
        {
            double dSpread = RATE.Spread;
            double pipValue = Math.Pow(10, -RATE.Decimals);

            if (hazard)
            {
                dSpread += ACCOUNT.OpeningHazard * pipValue;
                dSpread += ACCOUNT.SqueringHazard * pipValue;
            }

            if (pipsGain > 0)
            {
                dSpread += pipsGain * pipValue;
            }

            return dSpread;// (int)(dSpread / pipValue);
        }

        public double LiquidPercentage(List<ChartPoint> LCPoints, Rates RATE, double factor)
        {
            double dSpread = this.Spread(RATE) * factor;
            double dMatches = LCPoints.Count(CP => CP.Activity >= dSpread);

            return (dMatches / LCPoints.Count) * 100;
        }



        public List<string> GetLiquids(List<string> LSProducts, OpenRatesBlotter ORBlotter, double minPercentage, int deep, double spreadFactor, TimeFrame TFrame)
        {
            List<string> LSLiquidProducts = new List<string>();
            foreach (string product in ARCHIVE.GetProducts())
            {
                Rates RATE = ORBlotter.Get(product);
                int iMinutesFrame = ABBREVIATIONS.ToMinutes(TFrame);
                List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TFrame, product);


                DateTime DTLast100 = ABBREVIATIONS.GreenwichMeanTime.AddMinutes(- deep * iMinutesFrame);

                List<ChartPoint> LCPLast100 = (from CP in LCPoints where CP.Time > DTLast100 select CP).ToList();

                if (this.LiquidPercentage(LCPLast100, RATE, spreadFactor) > minPercentage)
                    LSLiquidProducts.Add(product);
            }


            return LSLiquidProducts;
        }

        /*public List<string> GetLiquidsLive(OpenRatesBlotter ORBlotter, double minPercentage, int deep, double spreadFactor, TimeFrame TFrame)
        {
            
            List<string> LSLiquidProducts = new List<string>();
            foreach (string product in ORBlotter.GetProducts)
            {
                Rates RATE = ORBlotter.Get(product);
                int iMinutesFrame = ABBREVIATIONS.ToMinutes(TFrame);
                List<ChartPoint> LCPoints = ORBlotter.Archive.Get(product, TFrame, deep);
                if (LCPoints == null) continue;

                DateTime DTLast100 = ABBREVIATIONS.GreenwichMeanTime.AddMinutes(-deep * iMinutesFrame);

                List<ChartPoint> LCPPrevious = (from CP in LCPoints where CP.Time > DTLast100 select CP).ToList();

                if (this.LiquidPercentage(LCPPrevious, RATE, spreadFactor) > minPercentage)
                    LSLiquidProducts.Add(product);
            }


            return LSLiquidProducts;
        }*/


    }
}
