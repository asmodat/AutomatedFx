using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

using System.Collections.Concurrent;

namespace AsmodatForexEngineAPI
{
    public partial class AutoTreader
    {
        private Account ACCOUNT;
        private Archive ARCHIVE;
        private OpenRatesBlotter ORBlotter;
        private OpenDealsBlotter ODBlotter;
        private OpenSettingsBlotter OSBlotter;
        private Analysis ANALYSIS;
        private Abbreviations ABBREVIATIONS = new Abbreviations();

        public AutoTreader(ref Archive ARCHIVE, ref OpenRatesBlotter ORBlotter, ref OpenDealsBlotter ODBlotter, ref OpenSettingsBlotter OSBlotter,ref Account ACCOUNT)
        {
            this.ARCHIVE = ARCHIVE;
            this.ODBlotter = ODBlotter;
            this.ORBlotter = ORBlotter;
            this.OSBlotter = OSBlotter;
            this.ACCOUNT = ACCOUNT;
            this.ANALYSIS = new Analysis(ref ARCHIVE, ref ACCOUNT);

            foreach (TimeFrame TFrame in ARCHIVE.LTFrames)
                DATA.Add(TFrame, new ConcurrentDictionary<string, ChartPointPredition>());
        }




        public List<ChartPointPredition> BestPredictions(TimeFrame TFrame, int minMatches, double minPropability)
        {
            List<ChartPointPredition> LCPBestPredictions = new List<ChartPointPredition>();
            List<ChartPointPredition> LCPPredictions = this.GetList(TFrame);
            if (LCPPredictions.Count == 0) return LCPBestPredictions;

            List<string> LSInUse = (from Ds in ODBlotter.GetData select Ds.PRODUCT).ToList();
            

            for (int i = 0; i < LCPPredictions.Count; i++ )
            {
                ChartPointPredition CPP = LCPPredictions[i];

                if (!LSInUse.Contains(CPP.Product) 
                    && CPP.IsActual && 
                    CPP.Matches >= minMatches && 
                    CPP.Propability >= minPropability &&
                    CPP.Type != ChartPointPredition.Kind.Uncertain &&
                    CPP.Type != ChartPointPredition.Kind.Average)
                    LCPBestPredictions.Add(CPP);
            }

                
            return LCPBestPredictions;
        }










        public double AvalaibleMeans()
        {
            double dMeans = ACCOUNT.Leverage * ACCOUNT.ClosedBalance / 2;

            foreach(Deals Ds in ODBlotter.GetData)
            {
                dMeans -= this.ConvertToUSD(Ds);
            }

            return dMeans;
        }
        public double LotToUSD(string product, bool BUY)
        {
            var SETTINGS = OSBlotter.Get(product);
            return this.ConvertToUSD(product, BUY, int.Parse(SETTINGS.OrderSize));
        }
        public double ConvertToUSD(Deals DEAL)
        {
            return this.ConvertToUSD(DEAL.PRODUCT, DEAL.BUY, DEAL.CONTRACT);
        }
        public double ConvertToUSD(string PRODUCT, bool BUY, double amount)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var SETTINGS = OSBlotter.Get(PRODUCT);
            string sPrimary = PRODUCT.Substring(0, 3);
            string sSecondary = PRODUCT.Substring(4, 3);

            string sContract = SETTINGS.ContractProduct;
            string sCounter = SETTINGS.CounterProduct;
            double dContractRate = 0;
            double dCost;

            if (sContract == sCounter && sSecondary == "USD") return Math.Round(amount, 2);

            Rates RATE_Counter = ORBlotter.Get(sCounter);

            if (RATE_Counter == null)
                return double.NaN;

            if (BUY)
                dContractRate = RATE_Counter.ASK;
            else
                dContractRate = RATE_Counter.BID;


            dCost = amount / dContractRate;


            return Math.Round(dCost, 2);
        }


        



    }
}
