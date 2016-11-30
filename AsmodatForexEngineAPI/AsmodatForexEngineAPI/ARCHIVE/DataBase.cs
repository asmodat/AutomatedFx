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
    public partial class Archive
    {
        private ConcurrentDictionary<TimeFrame, Record> DSRData = new ConcurrentDictionary<TimeFrame, Record>();

        public Record Get(TimeFrame TFrame)
        {
            if (DSRData.ContainsKey(TFrame)) return DSRData[TFrame];
            else return new Record(DTStart, TFrame);
        }
        public void Set(TimeFrame TFrame, Record R)
        {
            if (DSRData.ContainsKey(TFrame))
                DSRData[TFrame] = R;
            else while (!DSRData.TryAdd(TFrame, R)) ;

        }
        public void SetDATA(TimeFrame TFrame, string product, List<ChartPoint> LCPoints)
        {
            Record DATA = this.Get(TFrame);
            DATA.Set(product, LCPoints);
            this.Set(TFrame, DATA);
        }
        public List<ChartPoint> GetDATA(TimeFrame TFrame, string product)
        {
            Record DATA = this.Get(TFrame);
            return DATA.Get(product);
        }
        public int GetDATACount(TimeFrame TFrame, string product)
        {
            Record DATA = this.Get(TFrame);
            return DATA.Count(product);
        }


        public List<ChartPoint> GetDATA(TimeFrame TFrame, string product, DateTime DTLast)
        {
            Record DATA = this.Get(TFrame);

            List<ChartPoint> LCPoints = DATA.Get(product);
            List<ChartPoint> LCPInRange = new List<ChartPoint>();
            for (int i = LCPoints.Count - 1; i >= 0; i--)
            {
                if (LCPoints[i].Time > DTLast)
                    LCPInRange.Add(LCPoints[i]);
            }


            LCPInRange.Reverse();


            return LCPInRange;
        }


        public List<ChartPoint> GetDATA(TimeFrame TFrame, string product, DateTime DTStart, DateTime DTStop)
        {
            Record DATA = this.Get(TFrame);

            List<ChartPoint> LCPoints = DATA.Get(product);
            List<ChartPoint> LCPInRange = new List<ChartPoint>();
            for (int i = LCPoints.Count - 1; i >= 0; i--)
            {
                if (LCPoints[i].Time > DTStart && LCPoints[i].Time < DTStop)
                    LCPInRange.Add(LCPoints[i]);
                else if (LCPoints[i].Time < DTStart)
                    break;
            }


            LCPInRange.Reverse();


            return LCPInRange;
        }

        public List<ChartPoint> GetDATA(TimeFrame TFrame, string product, int iStartIndex, int count)
        {
            

            Record DATA = this.Get(TFrame);
            List<ChartPoint> LCPoints = DATA.Get(product);

            List<ChartPoint> LCPInRange = new List<ChartPoint>();
            for (int i = iStartIndex; i < (iStartIndex + count); i++)
                    LCPInRange.Add(LCPoints[i]);
            
            return LCPInRange;
        }



    }
}
