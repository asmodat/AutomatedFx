using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;

using Asmodat.Types;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

namespace AsmodatForex
{

    public partial class ServiceCharting
    {
        private List<Rate> DecodeRates(string pair, string packet, string dateTimeFormat = "M/d/yyyy h:mm:ss tt")
        {
            List<Rate> Rates = new List<Rate>();
            string[] DelimitedData = Asmodat.Abbreviate.String.ToList(packet, "$");
            if (DelimitedData == null) return Rates;

            //Decimals must be backchecked with number of decimals in order to set correct value;
            int decimals = ForexConfiguration.GetDecimals(pair);

            foreach (string data in DelimitedData)
            {
                if (System.String.IsNullOrEmpty(data)) 
                    continue;

                Rate rate = this.ToRateChartData(pair, data, decimals);

                if (rate == null) 
                    continue;

                Rates.Add(rate);
            }

            return Rates;
        }


        /// <summary>
        /// @"DateTime\OPEN\HIGH\LOW\CLOSE"
        /// Example: "2/18/2015 5:45:00 PM\\119.220\\119.228\\119.220\\119.227"
        /// DateTime.ParseExact(SLDecodedData[i], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        /// separator - \\
        /// </summary>
        /// <param name="data"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public Rate ToRateChartData(string pair, string data, int decimals)
        {
            if (System.String.IsNullOrEmpty(data) || data.Length < 5)
                return null;

            string[] properties = Asmodat.Abbreviate.String.ToList(data, "\\");

            if (properties.Length != 5)
                throw new Exception("ServiceCharting.DecodeRates Amount of separators don't match Rate Format !");

            if (Asmodat.Abbreviate.String.IsNullOrEmpty(properties))
                throw new Exception("ServiceCharting.DecodeRates Amount properties cannot be null !");

            Rate rate = new Rate();

            try
            {
                rate.Pair = pair;
                rate.DateTime = DateTime.ParseExact(properties[0], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                //"4/3/2015 12:00:00 AM\\16.76\\17.058\\16.683\\17.033"
                rate.OPEN = Doubles.Parse(properties[1], decimals);
                rate.HIGH = Doubles.Parse(properties[2], decimals);
                rate.LOW = Doubles.Parse(properties[3], decimals);
                rate.CLOSE = Doubles.Parse(properties[4], decimals);

                //backtest
                double pchange = RateInfo.ChangePercentage(rate.OPEN, rate.HIGH, rate.LOW, rate.CLOSE);


                if (pchange < 1.5)
                    return null;

                if (pchange < 25)
                    pchange = 25;

            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return null;
            }
            return rate;
        }


        
    }
}

//public static double Parse(string input, int decimals)
//        {
//            input = input.Replace(" ", ""); //Remove white spaces
            
//            string sign = "";
//            if (input[0] == '-')
//            {
//                sign = "-";
//                input = input.Substring(1, input.Length - 1);
//            }

//            int position = input.Length - (decimals + 1);

//            string correction = "";
//            for (int i = 0; i < input.Length; i++)
//            {
//                char c = input[i];

//                //if char is a digit, add it and simply continue
//                if (char.IsDigit(c))
//                {
//                    correction += c;
//                    continue;
//                }

//                if (c == '.' || c == ',')
//                {
//                    //ignore excess formating 
//                    if (i == position)
//                        correction += c;

//                    continue;
//                }


//                throw new Exception("Unknown Format !");
//            }

//            correction = correction.Replace(",", ".");
//            correction = sign + correction;
//            double value = double.Parse(correction, CultureInfo.InvariantCulture);


            

//            return value;
//        }



                //Rate Rate = RateInfo.ToRateChartData(data, dateTimeFormat);
                //if (Rate != null)
                //{
                //    Rate.Pair = pair;
                //    Rates.Add(Rate);
                //}
                //else
                //    continue;