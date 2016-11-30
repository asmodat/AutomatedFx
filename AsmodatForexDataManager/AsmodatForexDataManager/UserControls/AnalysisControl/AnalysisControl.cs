using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

using AsmodatForex;
using AsmodatMath;
using MathNet.Numerics;
using Asmodat.Abbreviate;

namespace AsmodatForexDataManager.UserControls
{
    public partial class AnalysisControl : ChartFeedUserControl
    {
        ThreadedStopWatch Watch = new ThreadedStopWatch();
        public AnalysisControl()
        {
            InitializeComponent();

            TTbxSpan.SetIntegerMode(1, 10000, 1);
            TTbxMin.SetDoubleMode(double.MinValue, double.MaxValue, 0, 5, "");
            TTbxAverage.SetDoubleMode(double.MinValue, double.MaxValue, 0, 5, "");
            TTbxMax.SetDoubleMode(double.MinValue, double.MaxValue, 0, 5, "");

            TTbxBacktests.SetIntegerMode(0, 10000, 25);
            TTbxBacktestSamples.SetIntegerMode(int.MinValue, int.MaxValue, 0);
            TTbxBacktestConfidence.SetDoubleMode(double.MinValue, double.MaxValue, 0, 2, "%");


            //double[] dat = new double[10000000];
            //double[] dat1 = new double[10000000];
            //double[] dat2 = new double[10000000];

            //for (int i = 0; i < dat.Length; i++)
            //    dat[i] = AMath.Random();
            //Watch.Run("1");
            
            //for (int i = 0; i < dat.Length; i++)
            //    dat1[i] = AMath.Ln(dat[i]);

            //double v1 = AMath.Average(dat1);
            //double sp1 = Watch.ms("1");

            //Watch.Run("2");
            //for (int i = 0; i < dat.Length; i++)
            //    dat2[i] = Math.Log(dat[i]);

            //double v2 = AMath.Average(dat2);
            //double sp2 = Watch.ms("2");

            //if (sp2 > sp1 || v2 != v1)
            //    return;

        }

        private void SetOutputStatistic(string pair, double max, double avg, double min)
        {
            int decimals = Manager.ForexConfiguration.GetDecimals(pair);
            TTbxMin.Decimals = decimals;
            TTbxAverage.Decimals = decimals;
            TTbxMax.Decimals = decimals;

            TTbxMax.SetValue(this, max);
            TTbxAverage.SetValue(this, avg);
            TTbxMin.SetValue(this, min);

        }


        System.Windows.Forms.Control Invoker = new System.Windows.Forms.Control();

        public override void Init()
        {
            Timers.Run(() => Peacemaker(), 10, null, true, true, true);
            Timers.Run(() => PeacemakerChart(), 100, null, true, true, true);

            CmbxsPair.MouseEnter += CmbxPair_MouseEnter;
            CmbxsFrame.MouseEnter += CmbxsFrame_MouseEnter;

            TCmbxConfidence.SetDoubleMode(0.01, 99.99, 68, 2, " %");
            TCmbxConfidence.AddItems(this, false, 6, 8, 17, 25, 34, 45, 51, 68, 75, 85, 95, 99.7);

            Chart.Main.Start();
        }

        void CmbxsFrame_MouseEnter(object sender, EventArgs e)
        {
            if (!IsFeed || !Manager.IsLoadedRate) return;
            CmbxsFrame.AddItemsEnum<ServiceConfiguration.TimeFrame, AnalysisControl>(this, true, 0);
        }

        void CmbxPair_MouseEnter(object sender, EventArgs e)
        {
            if (!IsFeed || !Manager.IsLoadedRate) return;
            CmbxsPair.AddItems(this, true, 0, Manager.ForexArchive.Data.KeysArray);
            
        }


        public ServiceConfiguration.TimeFrame Frame
        {
            get
            {
                return CmbxsFrame.GetEnum<ServiceConfiguration.TimeFrame, AnalysisControl>(this);
            }
        }

        public string Pair
        {
            get
            {
                return CmbxsPair.GetText(this);
            }
        }

        public int Span
        {
            get
            {
                return TTbxSpan.GetInt(this);
            }
        }

        public double Confidecne
        {
            get
            {
                return TCmbxConfidence.GetDouble(this);
            }
        }


        private void BtnTest_Click(object sender, EventArgs e)
        {
            Manager.ForexAnalysis.TerminateCaluculations();
        }



        private void Peacemaker()
        {
            if (!IsFeed || !Manager.IsLoadedRate) return;


            string pair = this.Pair;
            ServiceConfiguration.TimeFrame frame = this.Frame;
            int span = this.Span;
            double confidence = this.Confidecne;
            int tests = TTbxBacktests.GetInt(this);

            Rate[] Rates = Manager.ForexArchive.Data.GetValuesArray<ServiceConfiguration.TimeFrame, DateTime, Rate>(pair, frame); //Manager.ForexArchive.Data[pair][ServiceConfiguration.TimeFrame.DAILY].ValuesArray;// //  


           // TTbxMax.dec


            if (Objects.IsNullOrEmpty(Rates) || !ServiceConfiguration.IsSpan(frame)) 
                return;


     
            Manager.ForexAnalysis.MonteCarlo(pair, frame, span, tests);
            

            double[][] MonteCarloData = Manager.ForexAnalysis.GetMonteCarlo(pair, frame, span, -1);
            if (Objects.IsNullOrEmpty(MonteCarloData))
            {
                Chart.Main.ClearSeries(this);
                return;
            }

            double[] data = Objects.ToArray<double>(RateInfo.Extract(Rates, RateInfo.Properties.CLOSE));
            DateTime[] dates = Objects.ToArray<DateTime>(RateInfo.Extract(Rates, RateInfo.Properties.DateTime));

            
            double Min = 0;
            double Avg = 0;
            double Max = 0;
            double Test = 0;
            Manager.ForexAnalysis.GetStatistics(pair, frame, span, confidence, ref Max, ref Avg, ref Min, ref Test);

            this.SetOutputStatistic(pair, Max, Avg, Min);
            TTbxBacktestConfidence.SetValue(this, Test);
            TTbxBacktestSamples.SetValue(this, Manager.ForexAnalysis.MonteCarloBacktestSamples);

            double[] next = MonteCarloData.Last();// MonteCarloSimulation.Next(data, 50).ToArray();
            if (next == null)
                return;

            TTbxTestSamples.SetValue(this, MonteCarloData.Length);

            List<double> combine = new List<double>();
            combine.AddRange(data);
            combine.AddRange(next);

            if (Chart.Main.Series.Count <= 0 || Chart.Main.Series[0].Name != pair || Chart.Main.ID != frame + pair + span + tests)
            {
                Chart.Main.ID = frame + pair + span + tests;
                Chart.Main.ClearSeries(this);
                Chart.Main.Add(pair, SeriesChartType.Line, this);
                DataPoint[] points = ChartPointInfo.ToDataPoints(data, dates, ServiceConfiguration.Span(frame));
                Chart.Main.AddRange(this, pair, points, ChartValueType.DateTime, ChartValueType.Double);
            }
            else
            {
                DataPoint[] points = ChartPointInfo.ToDataPoints(next, dates.Last(), ServiceConfiguration.Span(frame));
                Chart.Main.UpdateRange(this, pair, points, ChartValueType.DateTime, ChartValueType.Double);
            }
            
        }


    }
}
