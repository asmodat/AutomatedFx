using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;


using System.Collections.Concurrent;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{
    public class Record
    {
        //since 2001 until today
        private DateTime DTStart;
        public TimeFrame TFrame;

        public Record(DateTime DTStart, TimeFrame TFrame)
        {
            this.DTStart = DTStart;
            this.TFrame = TFrame;
        }


        private ConcurrentDictionary<string, List<ChartPoint>> DATA = new ConcurrentDictionary<string, List<ChartPoint>>();

        public void Set(string product, List<ChartPoint> LCPoints)
        {
            if (DATA.ContainsKey(product))
            {
                if (LCPoints.Count == 0) return;
                List<ChartPoint> LRData = DATA[product];
                if (LRData == null) LRData = new List<ChartPoint>();



                int LRDataCount = LRData.Count;
                DateTime DTRFirst = LCPoints.First().Time;
                DateTime DTRLast = LCPoints.Last().Time;


                var leftSide = (from R in LRData where R.Time < DTRFirst select R);
                var rightSide = (from R in LRData where R.Time > DTRLast select R);

                List<ChartPoint> LRDataNew = new List<ChartPoint>();

                LRDataNew.AddRange(leftSide);
                LRDataNew.AddRange(LCPoints);
                LRDataNew.AddRange(rightSide);

                DATA[product] = LRDataNew;
            }
            else
            {
                if (LCPoints == null) LCPoints = new List<ChartPoint>();

                while (!DATA.ContainsKey(product) && !DATA.TryAdd(product, LCPoints)) ;
            }
        }


        public List<ChartPoint> Get(string product)
        {
            if (!DATA.ContainsKey(product))
                return new List<ChartPoint>();
            else return DATA[product];

        }

        public int Count(string product)
        {
            if (!DATA.ContainsKey(product))
                return -1;
            else return DATA[product].Count;

        }

        public List<string> GetProducts()
        {
            return new List<string>(DATA.Keys.ToArray());
        }
        public List<ChartPoint> GetYear(string product, DateTime Date)
        {
            List<ChartPoint> LRData = DATA[product];
            var LCPYear = (from CP in LRData where CP.Time.Year == Date.Year select CP);

            return new List<ChartPoint>(LCPYear.ToArray());
        }
        public List<ChartPoint> GetMonth(string product, DateTime Date)
        {
            List<ChartPoint> LRData = DATA[product];
            var LCPYear = (from CP in LRData where (CP.Time.Year == Date.Year && CP.Time.Month == Date.Month) select CP);

            return new List<ChartPoint>(LCPYear.ToArray());
        }
        public bool Contains(string product, DateTime DateTime, bool onlyDate)
        {

            if (onlyDate)
                return DATA[product].Any(R => R.Time.Date == DateTime);
            else return DATA[product].Any(R => R.Time == DateTime);
        }
    }
}
