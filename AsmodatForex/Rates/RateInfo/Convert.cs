using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Asmodat.Abbreviate;

using System.Text.RegularExpressions;

using Asmodat.Serialization;

namespace AsmodatForex
{
    public partial class RateInfo
    {
        ///// <summary>
        ///// Converts Rate properties into data format "Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR\DateTime\Token\OPEN\CLOSE", 
        ///// </summary>
        ///// <returns>Formatted Rate properties.</returns>
        //public static string ToString(Rate rate)
        //{
        //    string[] Format = Asmodat.Abbreviate.String.ToList(@"Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR\DateTime\Token\OPEN\CLOSE", @"\");
        //    return SerializeProperties.Serialize(rate, Format, @"\", "yyyy-MM-ddTHH:mm:ss.fff");
        //}

        ///// <summary>
        ///// @"Pair\BID\OFFER\HIGH\LOW\OPEN\CLOSE\DateTime", @"\");
        ///// </summary>
        ///// <returns></returns>
        //public static string ToStringQuick(Rate rate)
        //{

        //    return rate.ChartData.Serialize();

        //    //return
        //    //    rate.Pair + "\\" +
        //    //    rate.BID + "\\" +
        //    //    rate.OFFER + "\\" +
        //    //    rate.OPEN + "\\" +
        //    //    rate.CLOSE + "\\" +
        //    //    rate.HIGH + "\\" +
        //    //    rate.LOW + "\\" +
        //    //    rate.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            
        //}


        /// <summary>
        /// This method allows to parse data packets into separate Rate List
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, Rate> ParseRates(string packet)
        {

            string[] DelimitedData = Asmodat.Abbreviate.String.ToList(packet, "$");
            if (DelimitedData == null) return null;


           
            Dictionary<DateTime, Rate> Rates = new Dictionary<DateTime, Rate>();


            foreach (string data in DelimitedData)
            {
                
                if (System.String.IsNullOrEmpty(data)) continue;
                Rate rate = RateInfo.ToRate(data, DataType.ChartPoint); //rate.ChartData.Deserialize(data);
                Rates.Add(rate.DateTime, rate);
            }

            //for (int i = 0; i < DelimitedData.Length; i++)
            //{
                
            //    Rate rate = RateInfo.ParseQuick(DelimitedData[i]);
            //    if (rate != null && !Rates.ContainsKey(rate.DateTime))
            //        Rates.Add(rate.DateTime, rate);
            //}

            return Rates;
        }


        


    }
}
// if (!System.String.IsNullOrEmpty(_RawData))   return _RawData;

//2011-10-05T00:00:00.000
//string data = "";
//string separator = "\\";
//data += _Pair + separator;
//data += _BID + separator;
//data += _OPEN + separator;
//data += _CLOSE + separator;
//data += _HIGH + separator;
//data += _LOW + separator;
//data += _DateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
//return packet;


/*
 * 
 */

//ConcurrentDictionary<DateTime, Rate> Rates = new ConcurrentDictionary<DateTime, Rate>();
//Parallel.For(0, DelimitedData.Length, i => 
//{
//    Rate rate = Rate.ParseQuick(DelimitedData[i]);
//    if (rate != null && !Rates.ContainsKey(rate.DateTime))
//        Rates.TryAdd(rate.DateTime, rate);
//});

//public static List<List<Rate>> ParseRates(string packet, int items)
//{

//    string[] DelimitedData = Asmodat.Abbreviate.String.ToList(packet, "$");
//    if (DelimitedData == null) return null;


//    List<List<Rate>> Rates = new List<List<Rate>>();

//    //Parallel.For(0, iCount, r => { Rates[r] = Rate.ParseQuick(DelimitedData[r]); });
//    List<Rate> SubRates = new List<Rate>();
//    for (int i = 0; i < DelimitedData.Length; i++)
//    {
//        SubRates.Add(RateInfo.ParseQuick(DelimitedData[i]));

//        if (SubRates.Count >= items)
//        {
//            Rates.Add(SubRates);
//            SubRates = new List<Rate>();
//        }
//    }

//    if (SubRates.Count < items)
//        Rates.Add(SubRates);

//    return Rates;
//}

///// <summary>
//        /// Quicly parses "Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR\DateTime\Token\OPEN\CLOSE", into rate
//        /// </summary>
//        /// <param name="data"></param>
//        /// <returns></returns>
//public static Rate ParseQuick(string data)
//        {
//            if (System.String.IsNullOrEmpty(data)) return null;
//            Rate Rate = new Rate();

//            string[] LSRateProperties = Asmodat.Abbreviate.String.ToList(data, "\\");

//            if (LSRateProperties.Length != 15) return null;

//            try
//            {
//                Rate.Pair = LSRateProperties[0];
//                Rate.BID = Double.Parse(LSRateProperties[1]);
//                Rate.OFFER = Double.Parse(LSRateProperties[2]);
//                Rate.STATUS = LSRateProperties[3];
//                Rate.HIGH = Double.Parse(LSRateProperties[4]);
//                Rate.LOW = Double.Parse(LSRateProperties[5]);
//                Rate.DECIMALS = int.Parse(LSRateProperties[6]);
//                Rate.NOTATION = LSRateProperties[7];
//                Rate.CLOSINGBID = Double.Parse(LSRateProperties[8]);
//                Rate.CONTRACTPAIR = LSRateProperties[9];
//                Rate.COUNTERPAIR = LSRateProperties[10];
//                Rate.DateTime = DateTime.ParseExact(LSRateProperties[11], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture); ;
//                Rate.Token = int.Parse(LSRateProperties[12]);
//                Rate.OPEN = Double.Parse(LSRateProperties[13]);
//                Rate.CLOSE = Double.Parse(LSRateProperties[14]);
//            }
//            catch
//            {
//                return null;
//            }

//            return Rate;
//        }