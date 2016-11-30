using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;

using System.IO;

namespace AsmodatForex
{

    public partial class Archive
    {
        /// <summary>
        /// This method returns last rate based on pair nad frame values
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public Rate GetRate(string pair, string frame)
        {
            if (System.String.IsNullOrEmpty(pair) || System.String.IsNullOrEmpty(frame)) return null;



            return this.GetRate(pair, (ServiceConfiguration.TimeFrame)Enum.Parse(typeof(ServiceConfiguration.TimeFrame), frame));
        }

        /// <summary>
        /// This method returns last rate based on pair nad frame values
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public Rate GetRate(string pair, ServiceConfiguration.TimeFrame frame)
        {
            if (System.String.IsNullOrEmpty(pair) || !Data.ContainsKey(pair) || !Data[pair].ContainsKey(frame) || Data[pair][frame].Count <= 0) return null;

            Rate rate = Data[pair][frame].Last().Value;
            rate.Frame = frame;

            return rate;
        }



        /// <summary>
        /// This method returns all chartpoints based on rate informations
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public ChartPoint[] GetChartPoints(Rate rate, int start = 0)
        {
            if (rate == null) return new ChartPoint[0];

            return this.GetChartPoints(rate.Pair, rate.Frame, start);

        }


        /// <summary>
        /// This method returns all chartpoints based on pair nad frame values
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public ChartPoint[] GetChartPoints(string pair, ServiceConfiguration.TimeFrame frame, int start = 0)
        {
            if (System.String.IsNullOrEmpty(pair) || !Data.ContainsKey(pair) || !Data[pair].ContainsKey(frame) || Data[pair][frame].Count <= 0) 
                return new ChartPoint[0];


            Rate[] data = Data[pair][frame].ValuesArray;
            int count = data.Length;

            if(start >= count)
                return new ChartPoint[0];

            ChartPoint[] ACPoints = new ChartPoint[count - start];


            Parallel.For(start, count, i =>//KeyValuePair<DateTime, Rate> KVP = Data[pair][frame].ElementAt(i);
            {
                ACPoints[i - start] = data[i].ChartData;
            });

            return ACPoints;
        }

    }
}
