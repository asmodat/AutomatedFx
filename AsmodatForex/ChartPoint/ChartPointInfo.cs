using System;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;

using System.Runtime.Serialization;

using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization;

namespace AsmodatForex
{

    public partial struct ChartPointInfo
    {
        /// <summary>
        /// Serializes chart point to string
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static string Serialize(ChartPoint point)
        {
            StringWriter SWriter = new StringWriter();
            SWriter.WriteLine(point.Pair);
            SWriter.WriteLine(point.ASK);
            SWriter.WriteLine(point.BID);
            SWriter.WriteLine(point.Open);
            SWriter.WriteLine(point.Close);
            SWriter.WriteLine(point.High);
            SWriter.WriteLine(point.Low);
            SWriter.WriteLine(point.TickTime.Ticks);
            return SWriter.ToString();
        }


        /// <summary>
        /// deserializes string to chart point serialized with Serialize(ChartPoint point) method from ChartPointInfo class
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ChartPoint Deserialize(string data)
        {
            ChartPoint point = new ChartPoint();
            StringReader SReader = new StringReader(data);
            point.Pair = SReader.ReadLine();
            point.ASK = System.Double.Parse(SReader.ReadLine());
            point.BID = System.Double.Parse(SReader.ReadLine());
            point.Open = System.Double.Parse(SReader.ReadLine());
            point.Close = System.Double.Parse(SReader.ReadLine());
            point.High = System.Double.Parse(SReader.ReadLine());
            point.Low = System.Double.Parse(SReader.ReadLine());
            point.TickTime = new TickTime(System.Int64.Parse(SReader.ReadLine()));
            return point;
        }


        /// <summary>
        /// This data pont can be used as candle stick inside chart
        /// </summary>
        /// <param name="CPoint"></param>
        public static DataPoint ToCandle(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();//DateT
            DPoint.SetValueXY((DateTime)point.TickTime, point.High, point.Low, point.Open, point.Close);
            return DPoint;
        }

        /// <summary>
        /// This data pont can be used as HiLo AskBid stick inside chart
        /// </summary>
        /// <param name="CPoint"></param>
        public static DataPoint ToAskBid(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();
            DPoint.SetValueXY((DateTime)point.TickTime, point.ASK, point.BID);
            return DPoint;
        }

        public static DataPoint ToAskBidErrorBar(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();
            DPoint.SetValueXY((DateTime)point.TickTime, point.ASK, point.BID, point.ASK);
            return DPoint;
        }
        public static DataPoint ToAskLine(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();
            DPoint.SetValueXY((DateTime)point.TickTime, point.ASK);
            return DPoint;
        }

        /// <summary>
        /// This data pont can be used as HighLow High Low stick inside chart
        /// </summary>
        /// <param name="CPoint"></param>
        public static DataPoint ToHihgLow(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();
            DPoint.SetValueXY((DateTime)point.TickTime, point.High, point.Low);
            return DPoint;
        }

        /// <summary>
        /// This data pont can be used as HighLow AskBid stick inside chart
        /// </summary>
        /// <param name="CPoint"></param>
        public static DataPoint ToOpenClose(ChartPoint point)
        {
            DataPoint DPoint = new DataPoint();
            DPoint.SetValueXY((DateTime)point.TickTime, point.Open, point.Close);
            return DPoint;
        }

        /// <summary>
        /// This method coverts chart points to data points
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static DataPoint[] ToDataPoints(ChartPoint[] points, DataPointType type)
        {
            DataPoint[] ADPoints = new DataPoint[points.Length];

            Parallel.For(0, points.Length, i =>
            {
                if (type == DataPointType.Candle)
                    ADPoints[i] = ChartPointInfo.ToCandle(points[i]);
                else if (type == DataPointType.AskBid)
                    ADPoints[i] = ChartPointInfo.ToAskBid(points[i]);
                else if (type == DataPointType.HiLo)
                    ADPoints[i] = ChartPointInfo.ToHihgLow(points[i]);
                else if (type == DataPointType.OpenClose)
                    ADPoints[i] = ChartPointInfo.ToOpenClose(points[i]);
                else if (type == DataPointType.AskBidErrorBar)
                    ADPoints[i] = ChartPointInfo.ToAskBidErrorBar(points[i]);
                else if (type == DataPointType.AskLine)
                    ADPoints[i] = ChartPointInfo.ToAskLine(points[i]);
                else throw new ArgumentException("ChartPointInfo ToDataPoints Unknown data point type !");
            });

            return ADPoints;
        }


        /// <summary>
        /// This method can combine data with tim as data point and extrapolate date if span is set.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static DataPoint[] ToDataPoints(double[] data, DateTime[] dates, TimeSpan? span = null)
        {
            DataPoint[] points = new DataPoint[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                points[i] = new DataPoint();

                if (dates.Length <= i)
                {
                    DateTime date = dates[i - 1].Add((TimeSpan)span);

                    //if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                    //    date = date.AddDays(2);

                    points[i].SetValueXY(date, data[i]);
                }
                else points[i].SetValueXY(dates[i], data[i]);
            }


           return points;
        }


        public static DataPoint[] ToDataPoints(double[] data, DateTime StartDate, TimeSpan span)
        {
            DataPoint[] points = new DataPoint[data.Length];


            for (int i = 0; i < data.Length; i++)
            {
                points[i] = new DataPoint();

                StartDate = StartDate.Add(span);
                //if (date.DayOfWeek == DayOfWeek.Friday)
                //   date = date.AddDays(2);

                points[i].SetValueXY(StartDate, data[i]);
            }


            return points;
        }



        public enum DataPointType
        {
            Null = 0,
            Candle = 1,
            AskBid = 2,
            HiLo = 3,
            OpenClose = 4,
            AskBidErrorBar = 5,
            AskLine = 6,
        }
    }
}
