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
        List<Thread> LThreadLoad = new List<Thread>();
        private bool bIsLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return bIsLoaded;
            }
        }

        public void LoadAll()
        {
            List<string> LSProducts = this.GetProducts();

            foreach (TimeFrame TF in LTFrames)
                this.Load(LSProducts, TF);

            foreach (Thread Thrd in LThreadLoad)
                Thrd.Join();

            bIsLoaded = true;
        }
        public void Load(List<string> LSProducts, TimeFrame TFrame)
        {
            string sTFrame = ABBREVIATIONS.ToString(TFrame);

            Thread Thrd = new Thread(delegate()
            {
                Record DATA = new Record(DTStart, TFrame);
                foreach (string product in LSProducts)
                {
                    DateTime DTNow = DateTime.Now.AddMonths(1);
                    DateTime DTCurrent = DTStart;

                    do
                    {
                        List<ChartPoint> LCPoints = DATABASE.Load_ChartPoints(product, DTCurrent, sTFrame);
                        DATA.Set(product, LCPoints);

                        DTCurrent = DTCurrent.AddMonths(1);
                    } while (DTNow.Year >= DTCurrent.Year || DTNow.Month >= DTCurrent.Month);

                }
                this.Set(TFrame, DATA);
            });

            Thrd.Priority = ThreadPriority.Highest;
            Thrd.Start();
            LThreadLoad.Add(Thrd);
        }
    }
}
