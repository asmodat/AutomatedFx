using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace AsmodatMath
{
    public partial class AMath
    {
        public static double Average(List<double[]> data, int index, bool SkipExceptions = false, bool ThrowExceptions = true) 
        { return AMath.Average(data.ToArray(), index, SkipExceptions, ThrowExceptions); }
        public static double Average(double[][] data, int index, bool SkipExceptions = false, bool ThrowExceptions = true)//if (TestLength && data[i].Length <= index) return double.NaN;
        {
            double sum = 0;
            int count = 0;
            for(int i = 0; i < data.Length; i++)
            {
                if(data == null || data[i].Length <= index)
                {
                    if (SkipExceptions)
                        continue;
                    else if(!ThrowExceptions) 
                        return double.NaN;
                }

                sum += data[i][index];
                ++count;
            }

            return (double)sum / count;
        }


        /// <summary>
        /// this method is 3x faster then array.Average()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Average(double[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }

        /// <summary>
        /// Calculates average using last one, if last is NaN then it is calculated with standard algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static double Average(double[] data, double last)
        {
            if (data == null) return double.NaN;

            double average;
            int LN1 = data.Length - 1;
            if (double.IsNaN(last))
                average = AMath.Average(data);
            else
                average = (double)((last * LN1) + data[LN1]) / (LN1 + 1);

            return average;
        }


        public static double Average(int[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }

        public static double Average(UInt64[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }
        /*
         double[] dat = new double[100000000];
            for (int i = 0; i < dat.Length; i++)
                dat[i] = AMath.Random() * 1000;
            Watch.Run("1");
            double v1 = dat.Average();
            double sp1 = Watch.ms("1");

            Watch.Run("2");
            double v2 = AMath.Average(dat);
            double sp2 = Watch.ms("2");

            if (sp2 > sp1 || v2 != v1)
                return;*/


        /// <summary>
        /// Returns minimum value from array
        /// This overkill is faster then standard arr.min()
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double Min(params double[] arr)
        {
            if (arr == null || arr.Length <= 0)
                throw new ArgumentException("AMath.Min, array cannot be null or empty !");

            int i = 1;
            double res = arr[0];
            for (; i < arr.Length; i++)
            {
                if (res > arr[i])
                    res = arr[i];
            }

            return res; //return Math.Min(Math.Min(d1, d2), d3);
        }
        /// <summary>
        /// Returns maximum value from array
        /// This overkill is faster then standard arr.max()
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double Max(params double[] arr)
        {

            if (arr == null || arr.Length <= 0)
                throw new ArgumentException("AMath.Min, array cannot be null or empty !");

            int i = 1;
            double res = arr[0];
            for (; i < arr.Length; i++)
            {
                if (res < arr[i])
                    res = arr[i];
            }

            return res; //return Math.Min(Math.Min(d1, d2), d3);
        }


        

        /// <summary>
        /// Copares two doubles with specified precision
        /// Example:
        /// precision = 0.1
        /// 0.9 == 1 -> true
        /// 0.8 == 1 -> false
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="precision"></param>
        /// <returns>returns true if |v1 - v2| is les or equal to precision, else false </returns>
        public static bool Equ(double v1, double v2, double precision)
        {
            if (Math.Abs(v1 - v2) <= precision) return true;
            return false;
        }


        


        //public static double Percentage(double value, double percentage, double xvalue)
        //{
        //    if (max == 0 && min != 0)
        //        return Math.Sign(min) * double.MaxValue;

        //    return (double)(min * 100) / max;
        //}
    }
}


//public static double Min(double d1, double d2, double d3 = double.MaxValue)
//{
//    return Math.Min(Math.Min(d1, d2), d3);
//}