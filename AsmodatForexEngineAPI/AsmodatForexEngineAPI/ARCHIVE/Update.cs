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

        private List<ChartPoint> ProcessData(string ChartPointsData)
        {
            List<ChartPoint> LCPoint = new List<ChartPoint>();
            List<string> SLDecodedData = AFEAPIDecoding.DataChartPoints(ChartPointsData);
            AFEAPIDecoding.ChartPoints(SLDecodedData, ref LCPoint);

            return LCPoint;
        }
        public int UpdateAll(bool AutoSave)
        {
            if (AutoSave)
            {
                bArchiveSaving = true;
                bArchiveSaved = false;
            }
            else
            {
                bArchiveSaving = false;
            }

            int counter = 0;

            List<string> LSProducts = this.GetProducts();

            for (int i = 0; i < LTFrames.Count; i++)
            {
                counter += this.Update(AutoSave, LTFrames[i]);

                if (LTFrames[i] == TimeFrame.ONE_MINUTE)
                    bUpdated = true;
            }

            if (AutoSave)
            {
                bArchiveSaving = false;
                bArchiveSaved = true;
            }

            

            return counter;
        }

        bool bUpdated = false;
        public bool Updated
        {
            get
            {
                return bUpdated;
            }
        }
        public bool Saved
        {
            get
            {
                return bArchiveSaved;
            }
        }
        public bool Saving
        {
            get
            {
                return bArchiveSaving;
            }
        }

        bool bArchiveSaved = false;
        bool bArchiveSaving = false;
        public int Update(bool AutoSave, TimeFrame TFrame)
        {
           Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string sTFrame = ABBREVIATIONS.ToString(TFrame);
            int iTFMinutes = ABBREVIATIONS.ToMinutes(TFrame);
            List<string> LSProducts = this.GetProducts();
            //LThreadUpdate = new List<Thread>();
            int counter = 0;

            foreach (string product in LSProducts)
            {
                

                if (this.GetDATA(TFrame, product).Last().IsActual(TFrame, 0))
                    continue;


                //LThreadUpdate.Add(new Thread(delegate() {
               
                DateTime DTCurrent = ABBREVIATIONS.GreenwichMeanTime.AddDays(1); 
                Record DATA = this.Get(TFrame);

                DateTime startDateTime = DTStart;
                List<ChartPoint> LCPoints = DATA.Get(product);

                if (LCPoints != null && LCPoints.Count > 0)
                    startDateTime = LCPoints.Last().Time.AddMinutes(iTFMinutes);

                List<DateTime> LDTUpdates = new List<DateTime>();
                ChartData CData = null;
                do
                {
                    try
                    {
                        CData = CService.GetChartData(TOKEN, product, TFrame, startDateTime, DTCurrent); //Product\Date and Time\Open\High price\Low price\Closing Price
                    }
                    catch
                    {

                    }


                    if (CData == null || CData.Data == "") break;


                    List<ChartPoint> LCPointsNew = this.ProcessData(CData.Data);
                    LCPointsNew = new List<ChartPoint>(LCPointsNew.OrderBy(CP => CP.Time).ToArray());

                    


                    this.SetDATA(TFrame, product, LCPointsNew); counter += LCPointsNew.Count();
                    DTCurrent = LCPointsNew[0].Time.AddMinutes(-iTFMinutes);


                    if (AutoSave)
                    {
                        this.Save(product, DTCurrent, TFrame);
                    }

                } while (startDateTime <= DTCurrent);

                

                // }));

                //LThreadUpdate.Last().Start();
            }

            //foreach (Thread Thrd in LThreadUpdate)
            //    Thrd.Join();

            return counter;
        }


    }
}
