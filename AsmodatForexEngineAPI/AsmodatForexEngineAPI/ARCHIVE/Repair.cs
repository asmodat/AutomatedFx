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
        public List<TimeFrame> LTFrames = new List<TimeFrame>(new TimeFrame[] { TimeFrame.ONE_MINUTE, TimeFrame.FIVE_MINUTE, TimeFrame.TEN_MINUTE, TimeFrame.FIFTEEN_MINUTE, TimeFrame.THIRTY_MINUTE, TimeFrame.ONE_HOUR });
        public void RepairAll()
        {
            List<string> LSProducts = this.GetProducts();

            foreach (TimeFrame TF in LTFrames)
            {
                foreach (string product in LSProducts)
                {
                    List<DateTime[]> LADTPairs = this.GetMissing(TF, product);

                    if (LADTPairs.Count > 0)
                        throw new Exception("Missing data in DATABASE !");
                }
            }
        }
        public List<DateTime[]> GetMissing(TimeFrame TFrame, string product)
        {

            List<DateTime[]> LADTPairs = new List<DateTime[]>();
            List<ChartPoint> LCPoints = this.GetDATA(TFrame, product);
            if (LCPoints.Count < 2) return LADTPairs;
            List<ChartPoint> LCPSorted = new List<ChartPoint>(LCPoints.OrderBy(CP => CP.Time).ToArray());
            int iTFrame = ABBREVIATIONS.ToMinutes(TFrame);


            DateTime DTCurrent, DTNextExpected;
            DateTime DTNext;

            for (int i = 0; i < LCPSorted.Count - 1; i++)
            {
                DTCurrent = LCPSorted[i].Time;
                DTNextExpected = DTCurrent.AddMinutes(iTFrame);
                DTNext = LCPSorted[i + 1].Time;
                DayOfWeek DOfWeek = DTCurrent.DayOfWeek;

                if (DTNextExpected != DTNext &&
                    DOfWeek != DayOfWeek.Saturday)
                {
                    if (DOfWeek == DayOfWeek.Friday && DTNextExpected.Hour > 22 && DTNextExpected.Minute > 0 ||
                        DOfWeek == DayOfWeek.Sunday && DTCurrent.Hour < 22 && DTCurrent.Minute < 0)
                    {
                        LADTPairs.Add(new DateTime[2] { DTCurrent, DTNext });
                    }
                }

                i += 1;
            }

            return LADTPairs;
        }
    
    
    }
}
