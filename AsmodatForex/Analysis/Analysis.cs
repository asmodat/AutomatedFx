using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForex;
using Asmodat.Abbreviate;
using AsmodatMath;
using Asmodat;
using Asmodat.Types;
using System.Threading;

namespace AsmodatForex
{
    public partial class Analysis : AbstractServices
    {
        public Analysis(ref ForexService ForexService)
            : base(ref ForexService)
        {
         
        }

        public bool StopSimulations { get; private set; }

        public void TerminateCaluculations()
        {
            StopSimulations = true;
            Methods.JoinAll(5000);
            Methods.TerminateAll();
            MonteCarloData = new ThreadedDictionary<string, ThreadedList<double[]>>();
        }

        private ThreadedMethod Methods = new ThreadedMethod(1000, ThreadPriority.Lowest, 1);

        public Rate[] RatesUnderTest = null;

        ThreadedDictionary<string, ThreadedList<double[]>> MonteCarloData = new ThreadedDictionary<string, ThreadedList<double[]>>();

        public double[][] GetMonteCarlo(string pair, ServiceConfiguration.TimeFrame frame, int span, int index = -1)
        {
            if (index < 0)
                index = Index;

            string key = pair + frame + span + "idx" + index;


            var data = MonteCarloData.GetValue(key);
            if (Objects.IsNullOrEmpty(data)) return null;
            return data.ValuesArray;
        }

        public void GetStatistics(string pair, ServiceConfiguration.TimeFrame frame, int span, double confidence, ref double max, ref double average, ref double min, ref double test)
        {
            
            double[][] data = GetMonteCarlo(pair, frame, span, Index);
            double[] values;

            if (Objects.IsNullOrEmpty( data)) return;

            values = Doubles.ToArray(data.ToArray(), span-1, true, false);

            if (Objects.IsNullOrEmpty(values)) return;

            average = values.Average();
            double change = AMath.StandarConfidence(values, average, confidence, false);
            max = average + change;
            min = average - change;

            double success = 0;
            for (int i = 1; i < Indexes.Length; i++)
            {
                var pack = GetMonteCarlo(pair, frame, span, Indexes[i]);
                if(pack == null)
                {
                    success = 0;
                    break;
                }

                double[][] packet = pack.ToArray();
                values = Doubles.ToArray(packet, span - 1, true, false);


                if (Objects.IsNullOrEmpty(values))
                {
                    success = 0;
                    break;
                }

                double avg = values.Average();
                double ch = AMath.StandarConfidence(values, avg, confidence, false);
                double mx = avg + ch;
                double mn = avg - ch;


                double value = TestData[Indexes[i] + span];

                if (value >= mn && value <= mx)
                    ++success;
            }

            test = ((double)success / (Indexes.Length - 1)) * 100;

        }

        private int[] Indexes = null;
        public int Index
        {
            get
            {
                return Indexes[0];
            }
        }

        double[] TestData = null;

        public string Key { get; private set; }

        public void MonteCarlo(string pair, ServiceConfiguration.TimeFrame frame, int span, int tests)
        {

            if (Methods.Count >= tests + 1)
                return;


            Rate[] Rates = ForexArchive.Data.GetValuesArray<ServiceConfiguration.TimeFrame, DateTime, Rate>(pair, frame); //Manager.ForexArchive.Data[pair][ServiceConfiguration.TimeFrame.DAILY].ValuesArray;// //  
            if (Objects.IsNullOrEmpty(Rates) || !ServiceConfiguration.IsSpan(frame)) return;

            double[] data = Objects.ToArray<double>(RateInfo.Extract(Rates, RateInfo.Properties.CLOSE));
            TestData = data;

            Key = pair + frame + span;
            int[] points = AMath.Random(Rates.Length / 2, Rates.Length - 2 - span, tests);

            if (Objects.IsNullOrEmpty(points) && tests > 0) return;

            List<int> indexes = new List<int>();

            indexes.Add(data.Length - 1);
            if (points != null) indexes.AddRange(points);
            Indexes = indexes.ToArray();

            if (Objects.IsNullOrEmpty(Indexes))
                return;

            StopSimulations = false;
            

            foreach (int i in Indexes)
                Methods.Run(() => this.MonteCarlo(data, Key + "idx" + i, span, i), Key + "idx" + i, true, false);
        }

        public int MonteCarloBacktestSamples
        {
            get
            {
                if (Indexes.Length <= 1)
                    return 0;

                List<int> Count = new List<int>();
                for (int i = 1; i < Indexes.Length; i++)
                {
                    string key = Key + "idx" + Indexes[i];
                    int? count = MonteCarloData.CountValues<double[]>(key);

                    if (count == null)
                        return 0;

                    Count.Add(MonteCarloData[key].Count);
                    
                }

                return Count.Min();
            }
        }

        //public void MonteCarloBackTest(string pair, ServiceConfiguration.TimeFrame frame, int span, int tests, string key, double[] data)
        //{
        //    foreach (int i in Indexes)
        //    {


        //    }
        //        Methods.Run(() => this.MonteCarlo(data, key + "idx" + i, span, i), key + "idx" + i, true, false);
        //}

        Performance Performance = new Performance(1);

        public void MonteCarlo(double[] data, string key, int span, int index)
        {
            double[] daSubData = data;

            if (index != data.Length - 1)
                daSubData = data.Take(index + 1).ToArray();


            //if (!MonteCarloData.ContainsKey(key))
            MonteCarloData.Add(key, new ThreadedList<double[]>());

            try
            {
                for (int i = 0; i < 1000000; i++)
                {

                    MonteCarloData[key].Add(MonteCarloSimulation.Next(daSubData, span).ToArray());


                    while (Performance.CPU > 50)
                    {
                        Thread.Sleep(1);
                        if (StopSimulations) return;

                    } 

                    //while (Performance.CPU > 50 && Performance.CPU < 100)
                    //    if (!this.Sleep(Indexes.Length)) return;

                    if (StopSimulations) return;
                    //
                    
                }
            }
            catch { }
        }


        private bool Sleep(int ms)
        {
            for (int i = 0; i < ms && Performance.CPU > 1; i++)
            {
                if (StopSimulations) return false;
                Thread.Sleep(1);
            }

            return true;
        }
        


        //public void Test(string pair, ServiceConfiguration.TimeFrame frame)
        //{
        //    Rate[] Rates = ForexArchive.Data[pair][frame].ValuesArray;
        //    double[] data = Doubles.ToArray(RateInfo.Extract(Rates, RateInfo.Properties.CLOSE));

            

        //    var v = MonteCarloSimulation.Next(data, 10, 10000, true);




        //    if (v == null)
        //        return;

        //}








    }
}
