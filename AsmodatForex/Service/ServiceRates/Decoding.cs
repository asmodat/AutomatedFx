using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using System.Threading;

using Asmodat.Types;

namespace AsmodatForex
{

    public partial class ServiceRates
    {

        private List<Rate> DecodeRates(string packet, ref TickTime origin)
        {

            List<Rate> Rates = new List<Rate>();
            string[] DelimitedData = Asmodat.Abbreviate.String.ToList(packet, "$");
            if (DelimitedData == null) return Rates;



            foreach (string data in DelimitedData)
            {
                Rate Rate = this.ToRate(data, ref origin);
                if (Rate != null) Rates.Add(Rate);
            }

            return Rates;
        }

        /// <summary>
        /// Converts string data into Rate property
        /// Example rate: "EUR/USD\\1.17684\\1.17701\\D\\1.18485\\1.17619\\5\\A\\1.18385\\EUR/USD\\EUR/USD$"
        /// Example data: "Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR$"
        ///              @"Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR\DateTime\Token\OPEN\CLOSE", @"\");
        /// </summary>
        /// <param name="getRateString">Sting formattes as: Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR$ without $ char</param>
        /// <returns>Rate based on string input.</returns>
        private Rate ToRate(string data, ref TickTime origin)
        {
            if (System.String.IsNullOrEmpty(data) || data.Length < 11)
                return null;

            data = data.Replace("$", "");


            string[] properties = Asmodat.Abbreviate.String.ToList(data, "\\");
            if (properties.Length != 11)
                return null;

            string status = properties[3];
            string notation = properties[7];

            if (status != "D" && status != "R")
                return null;

            if (notation != "E" && notation != "A")
                return null;


            Rate rate = new Rate();
            try
            {

                rate.Pair = properties[0];
                rate.DECIMALS = int.Parse(properties[6]);

                if (rate.DECIMALS != ForexConfiguration.GetDecimals(rate.Pair))
                    return null;

                rate.BID = Doubles.Parse(properties[1], rate.DECIMALS);
                rate.OFFER = Doubles.Parse(properties[2], rate.DECIMALS);
                rate.STATUS = status;
                rate.NOTATION = notation;
                //rate.HIGH = Doubles.Parse(properties[4], rate.DECIMALS);
                //rate.LOW = Doubles.Parse(properties[5], rate.DECIMALS);
                //rate.CLOSINGBID = Doubles.Parse(properties[8], rate.DECIMALS);
                //rate.CONTRACTPAIR = properties[9];
                //rate.COUNTERPAIR = properties[10];

                rate.ChartData.TickTime = (origin += 1);

                //backtest
               double pchange = RateInfo.ChangePercentage(rate.BID, rate.OFFER);
               if (pchange < 25)
                   return null;

            }
            catch (Exception e)
            {
                Exceptions.Add(e);
                return null;
            }
            return rate;
        }
        

    }
}
