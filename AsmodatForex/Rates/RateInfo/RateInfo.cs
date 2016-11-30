using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Asmodat.Abbreviate;

using System.Text.RegularExpressions;

using Asmodat.Serialization;

using AsmodatMath;

namespace AsmodatForex
{
    public partial class RateInfo
    {
        public enum DataType
        {
            Null = 0,
            ChartPoint = 1

        }

        /// <summary>
        /// This method compares pair in frame 
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        public static bool EqualsOrigin(Rate r1, Rate r2)
        {
            if (r1 == null || r2 == null) return false;

            if (r1.Pair == r2.Pair && r1.Frame == r2.Frame)
                return true;
            else return false;
        }


        

        /// <summary>
        /// Creates new instance using string serialized chartpoint data
        /// </summary>
        /// <param name="data"></param>
        public static Rate ToRate(string data, RateInfo.DataType type)
        {
            Rate rate = new Rate();
            if (type == RateInfo.DataType.ChartPoint)
            {
                rate.ChartData = ChartPointInfo.Deserialize(data);//.Deserialize(data);
                return rate;
            }
            else throw new Exception("Rate() Deserializing Unknown Format !");
        }






        /// <summary>
        /// This method returns percenatge change (x) where
        /// max -> 100%
        /// min -> x
        /// </summary>
        /// <param name="arr"></param>
        public static double ChangePercentage(params double[] arr)
        {
            double max = AMath.Max(arr);
            double min = AMath.Min(arr);

            if (max == 0 && min != 0)
                return Math.Sign(min) * double.MaxValue;

            return (double)(min * 100) / max;
        }


    }
}
