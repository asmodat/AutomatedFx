using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatMath
{
    public partial class AMath
    {
        /// <summary>
        /// if (exp less then 0) throw new Exception("Pow only positive exp. can be used");
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double Pow(double value, uint exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= 1;
                exp >>= 1;
                value *= value;
            }

            return value;
        }


        public static double Pow2(double value) { return value * value; }
        public static double Pow3(double value) { return value * value * value; }
        public static double Pow4(double value) { return value * value * value * value; }
        public static double Pow5(double value) { return value * value * value * value * value; }
        public static double Pow6(double value) { return value * value * value * value * value * value; }
        public static double Pow7(double value) { return value * value * value * value * value * value * value; }
    }
}
