using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

using Asmodat.Abbreviate;

using Asmodat.Types;

namespace AsmodatForex
{
    public partial class ServiceAuthentication
    {
        private void DecodeData()
        {
            TickTime TimeOrigin = Data.FirstKey;
            string data = Data[TimeOrigin];
            Data.Remove(TimeOrigin); //Remove data after reading

            string[] Packets = Asmodat.Abbreviate.String.ToList(data, "\r");


            if (Packets == null || Packets.Length <= 0)
                return;

            for (int i = 0; i < Packets.Length; i++)
            {
                string packet = Packets[i];
                TimeOrigin += i;

                if (System.String.IsNullOrEmpty(packet))
                    continue;

                List<Rate> Rates = this.DecodeRates(packet, ref TimeOrigin);

                if (Rates.Count <= 0)
                {
                    Rate Rate = this.ToRateOnChange(packet, ref TimeOrigin);

                    if (Rate != null && Rate.Token >= 0)
                    {
                        Rate.Pair = ForexConfiguration.OrderPair[Rate.Token];
                        Rate.ChartData.TickTime = TimeOrigin;
                        Rates.Add(Rate);
                    }
                    else
                    {
                        //"UP\\DEAL\\EUR/USD\\1000\\1000\\0\\0\\0\\256044115\\121334263\\1137.75\\1.13775000\\49040.82\\0\\"
                    }
                }

                if (ForexRates != null && ForexRates.Loaded)
                    ForexRates.UpdateDataRates(Rates.ToArray());

                ForexArchive.Update(ServiceConfiguration.TimeFrame.LIVE, Rates);
                ForexArchive.Updates += Rates.Count;
            }
        }

        /// <summary>
        /// Decodes data and sets reciving time if origin != TickTime.MinValue
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        private List<Rate> DecodeRates(string packet, ref TickTime origin)
        {
            List<Rate> Rates = new List<Rate>();
            string[] DelimitedData = Asmodat.Abbreviate.String.ToList(packet, "$");
            if (DelimitedData == null) return Rates;


            for (int i = 0; i < DelimitedData.Length; i++)
            {
                Rate Rate = this.ToRateOnConnect(DelimitedData[i], ref origin);

                if (Rate != null)
                    Rates.Add(Rate);
                
            }

            return Rates;
        }

        /// <summary>
        /// @"Token\Pair\BID\OFFER\HIGH\LOW\STATUS\NOTATION\DECIMALS\CLOSINGBID", @"\");
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dateTimeFormat"></param>
        /// <returns></returns>
        public Rate ToRateOnConnect(string data, ref TickTime origin)
        {
            if (System.String.IsNullOrEmpty(data) || data.Length < 10) 
                return null;

            //Ignore token, might be corrupted
            string[] properties = Asmodat.Abbreviate.String.ToList(data, "\\");
            if (properties.Length != 10)
                return null;


            string status = properties[6];
            string notation = properties[7];

            if (status != "D" && status != "R")
                return null;

            if (notation != "E" && notation != "A")
                return null;

            Rate rate = new Rate();

            try
            {
                rate.Pair = properties[1];
                rate.DECIMALS = ForexConfiguration.GetDecimals(rate.Pair);
                rate.BID = Doubles.Parse(properties[2], rate.DECIMALS);
                rate.OFFER = Doubles.Parse(properties[3], rate.DECIMALS);
                rate.HIGH = Doubles.Parse(properties[4], rate.DECIMALS);
                rate.LOW = Doubles.Parse(properties[5], rate.DECIMALS);
                rate.STATUS = status;
                rate.NOTATION = notation;
                rate.CLOSINGBID = Doubles.Parse(properties[8], rate.DECIMALS);

                rate.ChartData.TickTime = (origin += 1);


                //backtest
                double pchange = RateInfo.ChangePercentage(rate.BID, rate.OFFER, rate.HIGH, rate.LOW);
                if (pchange < 25)
                    return null;


            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return null;
            }

            return rate;
        }



        /// <summary>
        /// @"Token\BID\OFFER\\\\\"
        /// "R27\\89.514\\89.549\\D\\\\\\02/19/2015 09:25:55\\"
        /// TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dateTimeFormat"></param>
        /// <returns></returns>
        public Rate ToRateOnChange(string data, ref TickTime origin)
        {
            if (System.String.IsNullOrEmpty(data) || data.Length < 7) 
                return null;

            char cMessageType = data[0];
            if (!Char.IsLetter(cMessageType))
                return null; //This is not RateChange frame
            data = data.Substring(1, data.Length - 1); //Remove Message Type Rate

            string[] properties = Asmodat.Abbreviate.String.ToList(data, "\\");
            if (properties.Length != 7)
                return null;

            string status = properties[3];

            if (status != "D" && status != "R")
                return null;

            Rate rate = new Rate();

            try
            {
                rate.Token = int.Parse(properties[0]);
                rate.Pair = ForexConfiguration.OrderPair[rate.Token];
                rate.DECIMALS = ForexConfiguration.GetDecimals(rate.Pair);



                rate.BID = Doubles.Parse(properties[1], rate.DECIMALS);
                rate.OFFER = Doubles.Parse(properties[2], rate.DECIMALS);
                rate.STATUS = status;

                rate.ChartData.TickTime = (origin += 1);

                //backtest
                double pchange = RateInfo.ChangePercentage(rate.BID, rate.OFFER);
                if (pchange < 50)
                    return null;


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
