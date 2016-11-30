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
        //private List<Thread> LThrdLiquid;
        private Dictionary<TimeFrame, ConcurrentDictionary<string, ChartPointPredition>> DATA = new Dictionary<TimeFrame, ConcurrentDictionary<string, ChartPointPredition>>();


        public List<ChartPointPredition> GetList(TimeFrame TFrame)
        {
            

                if (!DATA.ContainsKey(TFrame))
                    return null;

                List<ChartPointPredition> LCPPredictions = new List<ChartPointPredition>();

                string[] SAKeys = DATA[TFrame].Keys.ToArray();
                for (int i = 0; i < SAKeys.Length; i++)
                {
                    ChartPointPredition CPPrediction;
                    while (!DATA[TFrame].TryGetValue(SAKeys[i], out CPPrediction)) ;
                    ChartPointPredition CPP = CPPrediction.Clone();

                    LCPPredictions.Add(CPP);
                }
                

                return LCPPredictions;
            

            
        }

        public ChartPointPredition TryGet(string product, TimeFrame TFrame)
        {
            lock (DATA)
            {

                if (!DATA[TFrame].ContainsKey(product))
                    return null;

                ChartPointPredition CPPrediction;
                while (!DATA[TFrame].TryGetValue(product, out CPPrediction)) ;


                if (CPPrediction.IsActual)
                    return CPPrediction;
                else return null;
            }
        }

        private bool TryUpdate(ChartPointPredition CPPrediction)
        {
            lock (DATA)
            {

                if (!CPPrediction.IsActual)
                    return false;

                if (DATA[CPPrediction.Frame].ContainsKey(CPPrediction.Product))
                    while (!DATA[CPPrediction.Frame].TryUpdate(CPPrediction.Product, CPPrediction, DATA[CPPrediction.Frame][CPPrediction.Product])) ;
                else
                    while (!DATA[CPPrediction.Frame].TryAdd(CPPrediction.Product, CPPrediction)) ;

                return true;
            }
        }

        double dSpreadFactor = 1;
        public void Predict(TimeFrame TFrame, double[] IASymiliarities)
        {
            if (this.AvalaibleMeans() <= 0)
                return;

            //int iMinutes = ABBREVIATIONS.ToMinutes(TFrame);

            List<string> LSLiquids = ANALYSIS.GetLiquids(ARCHIVE.GetProducts(), ORBlotter, 60, 30, dSpreadFactor, TFrame); //Chooce only those products whose Activity in last 5 minutes is above 0.5 of spread //List<string> LSLiquidsLive = ANALYSIS.GetLiquidsLive(ORBlotter, 50, 3, 0.2, TFrame);

            if (LSLiquids.Count < 10 && dSpreadFactor > 0.35)
                dSpreadFactor -= 0.0025;
            else if (LSLiquids.Count > 20)
                dSpreadFactor += 0.0025;



            //List<Thread> LThrdLiquid = new List<Thread>();
            foreach (string product in LSLiquids)//ORBlotter.GetProducts)
            {


                //LThrdLiquid.Add(new Thread(delegate() {

                List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TFrame, product);
                ChartPoint CPLAst = LCPoints.Last();
                List<ChartPoint> LCPointsSpecified = ORBlotter.Archive.Get(product, TFrame, IASymiliarities.Length);

                if (LCPointsSpecified != null)
                {
                    ChartPointPredition CPPrediction = ANALYSIS.PredictNextSpecified(product, LCPoints, LCPointsSpecified, TFrame, ORBlotter.Get(product).Decimals, IASymiliarities);
                    ChartPointPredition.Kind CPPKind = CPPrediction.Prognosis();


                    if (CPPKind != ChartPointPredition.Kind.Uncertain)
                        this.TryUpdate(CPPrediction);

                }
               //  }));
               // LThrdLiquid.Last().Priority = ThreadPriority.BelowNormal;
               // LThrdLiquid.Last().Start();
            }

             //foreach (Thread Thrd in LThrdLiquid)  Thrd.Join();

        }

    }
}
