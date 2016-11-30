using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;
using System.Collections.Concurrent;

namespace AsmodatForexEngineAPI
{
    public class ArchiveRates
    {


        private Abbreviations ABBREVIATIONS = new Abbreviations();
        private ConcurrentDictionary<string, List<Rates>> DATA = new ConcurrentDictionary<string, List<Rates>>();

        private int iAddCounter = 0;
        public bool Add(Rates RATE)
        {
            if (RATE.Time == new DateTime()) 
                return false;

            string sKey = RATE.CCY_Pair;
            if (DATA.ContainsKey(sKey))
            {
                if (DATA[sKey].Last().Time < RATE.Time)
                {
                    DATA[sKey].Add(RATE.Clone());
                    ++iAddCounter;
                    return true;
                }
                else return false;
            }
            else
            {
                while(!DATA.TryAdd(sKey, new List<Rates>(new Rates[] { RATE.Clone() })));
                return true;
            }
        }

        public List<Rates> TryGetClone(string product)
        {
            if (!DATA.ContainsKey(product))
                return null;

            List<Rates> LRates = new List<Rates>();
            List<Rates> LRatesNew = new List<Rates>();

            while(!DATA.TryGetValue(product, out LRates));

            for (int i = 0; i < LRates.Count; i++)
            {
                Rates RATE = LRates[i];
                if(RATE != null)
                LRatesNew.Add(RATE.Clone());
                else
                {

                }
            }
            


            return LRates;
        }

        public List<Rates> TryGet(string product)
        {
            if (!DATA.ContainsKey(product))
                return null;

            List<Rates> LRates = new List<Rates>();
            //List<Rates> LRatesNew = new List<Rates>();

            while (!DATA.TryGetValue(product, out LRates)) ;

            /*for (int i = 0; i < LRates.Count; i++)
            {
                Rates RATE = LRates[i];
                if (RATE != null)
                    LRatesNew.Add(RATE);
                else
                {

                }
            }*/



            return LRates;
        }

        public List<ChartPoint> Get(string product, TimeFrame TFrame, int deep)
        {
            if (!DATA.ContainsKey(product))
                return null;

            int iMinutes = ABBREVIATIONS.ToMinutes(TFrame);
            List<Rates> LRData = this.TryGet(product);
            if (LRData.Count < 2) return null;


            Rates RATE = LRData[LRData.Count - 1];
            if (RATE == null) RATE = LRData[LRData.Count - 2]; 

            DateTime DTNow = RATE.TimeGMT;
            List<ChartPoint> LCPoints = new List<ChartPoint>();
            for (int i = 0; i < deep; i++)
            {
                DateTime DTEnd = DTNow.AddMinutes(-i);
                DateTime DTStart = DTNow.AddMinutes(-(i + 1));

                List<Rates> LRChartData = this.Get(product, DTStart, DTEnd, 5);


                if (LRChartData == null || LRChartData.Count <= (iMinutes * 12)) //12 points per minute / 1 pint per second
                    return null;

                LRChartData = LRChartData.OrderBy(R => R.Time).ToList();

                double OPEN = LRChartData.First().BID;
                double CLOSE = LRChartData.Last().BID;
                double HIGH = LRChartData.Max(R => R.BID);
                double LOW = LRChartData.Min(R => R.BID);
                DateTime DTime = LRChartData.Last().TimeGMT;

                LCPoints.Add(new ChartPoint(CLOSE, OPEN, HIGH, LOW, DTime));
            }

            return LCPoints;
        }

        

        public List<Rates> Get(string product, int deep)
        {
            if (!DATA.ContainsKey(product))
                return null;

            List<Rates> LRData = this.TryGet(product);

            if (LRData.Count <= deep)
                return null;

            List<Rates> LRClones = new List<Rates>();
            for (int i = LRData.Count - deep; i < LRData.Count; i++)
                LRClones.Add(LRData[i]);

            return LRClones;
        }


        public List<Rates> Get(string product, DateTime DTLastGMT)
        {
            List<Rates> LRMatches = new List<Rates>();
            if (!DATA.ContainsKey(product))
                return LRMatches;

            List<Rates> LRData = this.TryGet(product);

                for (int i = LRData.Count - 1; i >= 0; i--)
                {
                    Rates RATE = LRData[i];

                    if (RATE.TimeGMT >= DTLastGMT)
                        LRMatches.Add(RATE);
                    else break;
                }

                LRMatches.Reverse();

            return LRMatches;
        }

        public List<Rates> Get(string product, DateTime DTStartGMT, DateTime DTEndGMT, int secondsDeviation)
        {
            List<Rates> LRMatches = new List<Rates>();
            if (!DATA.ContainsKey(product))
                return LRMatches;

            List<Rates> LRData = this.TryGet(product);
            DateTime DTMin = DateTime.Now.AddYears(100);
            DateTime DTMax = DateTime.Now.AddYears(-100);

            for (int i = LRData.Count - 1; i >= 0; i--)
            {
                Rates RATE = LRData[i];
                DateTime DTCurrentGMT = RATE.TimeGMT;
                if (DTCurrentGMT >= DTStartGMT && DTCurrentGMT <= DTEndGMT)
                {
                    LRMatches.Add(RATE);

                    if (DTCurrentGMT > DTMax) DTMax = DTCurrentGMT;
                    else if (DTCurrentGMT < DTMin) DTMin = DTCurrentGMT;
                }

                if (DTCurrentGMT < DTStartGMT)
                      break;
            }

            if (DTMin.AddSeconds(-secondsDeviation) > DTStartGMT)
                return null;
            else if (DTMax.AddSeconds(secondsDeviation) < DTEndGMT)
                return null;

            LRMatches.Reverse();

            return LRMatches;
        }

    }
}
