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

namespace AsmodatForex
{
    /// <summary>
    /// This class is based upon com.efxnow.democharting.chartingservice.ChartingService, that
    /// Provides chart plot Data for specific products for a predefined timeinterval
    /// http://democharting.efxnow.com/Charting/ChartingService.asmx
    /// It provides historical chart data,
    /// WARNING !!! GetChartData returns GMT Time only, not ET and only returns BID prices
    /// trading desk is open 24 hours daily from 17:00 ET Sunday (Monday morning in Sydney) through 17:00 ET on Friday (the end of the NY trading day). 
    /// only 400 rates can be downloaded at once and there is 1440 minutes in one hour to download
    /// </summary>
    public partial class ServiceCharting
    {

        //DateTime.ParseExact(SLDecodedData[i], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

        
        public void UpdateAll(bool save = false)
        {
            
            string[] Pairs = ForexConfiguration.Products.ToArray();
            ServiceConfiguration.TimeFrame[] Frames = Enums.ToList<ServiceConfiguration.TimeFrame>().ToArray();


            for (int p = 0; p < Pairs.Length; p++)
            {
                string pair = Pairs[p];

                if (!ForexArchive.Data.ContainsKey(pair)) ForexArchive.Data.Add(pair, new ThreadedDictionary<ServiceConfiguration.TimeFrame, ThreadedDictionary<DateTime, Rate>>()); //Add missing Key before adding new values


                for (int f = 0; f < Frames.Length; f++)
                {
                    ServiceConfiguration.TimeFrame frame = Frames[f];

                    if (frame == ServiceConfiguration.TimeFrame.LIVE) continue;
                    if (!ForexArchive.Data[pair].ContainsKey(frame)) ForexArchive.Data[pair].Add(frame, new ThreadedDictionary<DateTime, Rate>()); //Add Missing Key before adding values


                    ThreadsUpdate.Run(() => this.Update(pair, frame), "UpdatePair" + pair + "Frame" + frame.ToString(), true, true);
                }

                ThreadsUpdate.JoinAll();
            }

           
            

            

            
        }


        public string GetChartData(string tokem, string pair, ServiceConfiguration.TimeFrame frame, DateTime DateStart, DateTime DateEnd, bool wait = false)
        {
            string packet = "";
            bool exception = false;
            AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame OriginFrame = ForexConfiguration.TimeFrames[frame];


            do
            {
                if (wait)
                {
                    Thread.Sleep(1);
                    if (!ForexAuthentication.Connected) continue;
                }

                try
                {
                    packet = CED_ChartingService.GetChartData(ForexAuthentication.Token, pair, OriginFrame, DateStart, DateEnd).Data;
                    exception = false;
                }
                catch(Exception Ex)
                {
                    exception = true;
                    if (!wait) throw Ex;
                }

            } while (exception);


            return packet;
        }


        public void Update(string pair, ServiceConfiguration.TimeFrame frame)
        {
            List<DateTime> frames = this.GetMissingDates(pair, frame);
            if (frames.Count <= 1)
                return;

            //frames.Reverse();
            double dTimePeriod = ServiceConfiguration.ToMinutes(frame);

            //Decimals must be backchecked with number of decimals in order to set correct value;
            int decimals = ForexConfiguration.GetDecimals(pair);

            string data = "";
            for (int i = 0; i < frames.Count - 1; i++)
            {
                DateTime DateStart = frames[i + 1];
                DateTime DateEnd = frames[i];

                Thread.Sleep(1); //Wait not to spam the server with requests
                string packet = this.GetChartData(ForexAuthentication.Token, pair, frame, DateStart, DateEnd, true); //fresh data cames first

                if (!System.String.IsNullOrEmpty(packet))
                    data = data + "$" + packet;
            }

            List<Rate> Rates;
            if (!System.String.IsNullOrEmpty(data))
                Rates = this.DecodeRates(pair, data); //Data is sorted by threaded dictionary
            else Rates = new List<Rate>();


            foreach (Rate rate in Rates)
            {
                if (ForexArchive.Data[pair][frame].ContainsKey(rate.DateTime)) continue;
                
                ForexArchive.Data[pair][frame].Add(rate.DateTime, rate);
                ++ForexArchive.Updates;
            }
        }



        /// <summary>
        /// This method searches and returns missing time periods in DateTime list in archive data.
        /// This data is stored in UTC format
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="TimeFrame"></param>
        public List<DateTime> GetMissingDates(string pair, ServiceConfiguration.TimeFrame TimeFrame)
        {
            double dTimePeriod = ServiceConfiguration.ToMinutes(TimeFrame);
            int iPeriodMultiplayer = 350; //This value defines time span between values inside list
            int iMaxPeriods = ServiceConfiguration.ToPeriodsAccess(TimeFrame); //Maximum periods of data that are avalaible for download
            List<DateTime> Data = ForexArchive.Data[pair][TimeFrame].Keys.ToList();// .SortedKeys;
            List<DateTime> DataMissing = new List<DateTime>();
            int iDataCount = Data.Count;

            if (dTimePeriod <= 0) throw new Exception("This time period is not vaild.");

            DateTime CurrentDate = LatestMarketOppened;
            DateTime EndDate;
            if (Data.Count <= 0)
                EndDate = DateTime.UtcNow.AddMinutes(-dTimePeriod * iMaxPeriods);
            else EndDate = Data.Max();

            DataMissing.Add(CurrentDate);

            do
            {
                DateTime DateChangeStart = CurrentDate;

                do
                    CurrentDate = CurrentDate.AddMinutes(-1);
                while ((DateChangeStart - CurrentDate).TotalMinutes < iPeriodMultiplayer * dTimePeriod);//Continue while period change is less then multiplayer

                if (CurrentDate >= EndDate)
                {
                    //CurrentDate = FutureMarketOppened(CurrentDate);
                    DataMissing.Add(CurrentDate);
                }
                else
                {
                    DataMissing.Add(EndDate);
                    break;
                }


            } while (CurrentDate > EndDate);


            DateTime[] DateArray = DataMissing.ToArray();

            //remove spare data
            if (DateArray != null)
            for (int i = 1; i < DateArray.Length - 1; )
            {
                DateTime last = DateArray[i - 1];
                DateTime current = DateArray[i];
                DateTime future = DateArray[i + 1];
                bool lastClosed = !IsMarketOppened(last);
                bool currentClosed = !IsMarketOppened(current);
                bool futureCloased = !IsMarketOppened(future);


                if (lastClosed && currentClosed && futureCloased)
                    DataMissing.Remove(current);
                if ((currentClosed && futureCloased && i + 1 == DateArray.Length - 1))
                    DataMissing.Remove(future);

                ++i;
            }

            if (DataMissing.Count <= 1 || DataMissing.Last() >= DataMissing.First())
                return new List<DateTime>();

            return DataMissing;
        }


        //trading desk is open 24 hours daily from 17:00 ET Sunday (Monday morning in Sydney) through 17:00 ET on Friday (the end of the NY trading day).  

        /// <summary>
        /// This method fefines wheter or not passed UTC date is an Open or Closed market day.
        /// </summary>
        /// <param name="UTC">GMT aka UTC Date</param>
        /// <returns>True if market is oppened, False if Market is closed.</returns>
        public bool IsMarketOppened(DateTime UTC)
        {
            //5PM stop EST - 5PM start EST
            if ((UTC.DayOfWeek == DayOfWeek.Friday && UTC.Hour > 22) || 
                UTC.DayOfWeek == DayOfWeek.Saturday || 
                (UTC.DayOfWeek == DayOfWeek.Sunday && UTC.Hour < 22))
                return false;
            else 
                return true;
        }


        /// <summary>
        /// this property returns latest utc data that market was oppened
        /// </summary>
        public DateTime LatestMarketOppened
        {
            get
            {
                DateTime CurrentDate = DateTime.UtcNow;

                if (IsMarketOppened(CurrentDate))
                    return CurrentDate;

                CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 22,0,0, DateTimeKind.Utc);

                if (CurrentDate.DayOfWeek == DayOfWeek.Saturday)
                    CurrentDate = CurrentDate.AddDays(-1);
                else if (CurrentDate.DayOfWeek == DayOfWeek.Sunday)
                    CurrentDate = CurrentDate.AddDays(-2);

                return CurrentDate;
            }
        }

        public DateTime FutureMarketOppened(DateTime CurrentDate)
        {
            if (IsMarketOppened(CurrentDate))
                return CurrentDate;

            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 22, 0, 0, DateTimeKind.Utc);

            if (CurrentDate.DayOfWeek == DayOfWeek.Friday)
                CurrentDate = CurrentDate.AddDays(2);
            else if (CurrentDate.DayOfWeek == DayOfWeek.Saturday)
                CurrentDate = CurrentDate.AddDays(1);

            return CurrentDate;
        }
        


    }
}
/*
        /// <summary>
        /// This method searches and returns missing time periods in DateTime list in archive data.
        /// This data is stored in UTC format
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="TimeFrame"></param>
        public List<DateTime> GetMissingDates(string pair, ServiceConfiguration.TimeFrame TimeFrame)
        {
            double dTimePeriod = ServiceConfiguration.ToMinutes(TimeFrame);
            int iPeriodMultiplayer = 350; //This value defines time span between values inside list
            int iMaxPeriods = ServiceConfiguration.ToPeriodsAccess(TimeFrame); //Maximum periods of data that are avalaible for download
            List<DateTime> Data = ForexArchive.Data[pair][TimeFrame].SortedKeys;
            List<DateTime> DataMissing = new List<DateTime>();
            int iDataCount = Data.Count;

            if (dTimePeriod <= 0) throw new Exception("This time period is not vaild.");

            DateTime CurrentDate = LatestMarketOppened;
            DateTime EndDate;
            if (Data.Count <= 0)
                EndDate = DateTime.UtcNow.AddMinutes(-dTimePeriod * iMaxPeriods);
            else EndDate = Data.Max().AddMinutes(dTimePeriod);//else EndDate = Data.Max();

            //EndDate = FutureMarketOppened(EndDate);

            DataMissing.Add(CurrentDate);

            do
            {
                DateTime DateChangeStart = CurrentDate;

                do
                {
                    CurrentDate = CurrentDate.AddMinutes(-1);

                }
                while (
                    !IsMarketOppened(CurrentDate) || //Continue while market is closed OR
                    (DateChangeStart - CurrentDate).TotalMinutes < iPeriodMultiplayer * dTimePeriod);//Continue while period change is less then multiplayer

                if (CurrentDate >= EndDate)
                {
                    //CurrentDate = FutureMarketOppened(CurrentDate);
                    DataMissing.Add(CurrentDate);
                }
                else
                {
                    DataMissing.Add(EndDate);
                    break;
                }


            } while (CurrentDate > EndDate);

            if (DataMissing.Count <= 1 || DataMissing.Last() >= DataMissing.First()) 
                return new List<DateTime>();


            DateTime[] DateArray = DataMissing.ToArray();

            for (int i = 1; i < DateArray.Length - 1; i++)
            {
                DateTime last = DateArray[i - 1];
                DateTime current = DateArray[i];
                DateTime future = DateArray[i + 1];
                bool lastOppened = IsMarketOppened(last);
                bool currentOppened = IsMarketOppened(current);
                bool futureOppened = IsMarketOppened(future);


                if(!lastOppened && !currentOppened && !futureOppened)
                {

                }


            }



                return DataMissing;
        }
*/