using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Threading;
using System.Globalization;


using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public class Abbreviations
    {

        public List<Type> GetLast<Type>(List<Type> LType, int deep)
        {
            List<Type> LTSub = new List<Type>();
            for (int i = LType.Count - deep; i < LType.Count; i++)
                LTSub.Add(LType[i]);

            return LTSub;
        }


        public DateTime GreenwichMeanTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public int ToMinutes(TimeFrame TFrame)
        {

            switch (TFrame)
            {
                case TimeFrame.ONE_MINUTE: return 1;
                case TimeFrame.FIVE_MINUTE: return 5;
                case TimeFrame.TEN_MINUTE: return 10;
                case TimeFrame.FIFTEEN_MINUTE: return 15;
                case TimeFrame.THIRTY_MINUTE: return 30;
                case TimeFrame.ONE_HOUR: return 60;
                case TimeFrame.TWO_HOUR: return 120;
                case TimeFrame.FOUR_HOUR: return 240;
                case TimeFrame.DAILY: return 1440;
                case TimeFrame.WEEKLY: return 10080;
            }

            return 0;
        }
        public string ToString(TimeFrame TFrame)
        {
         
            switch (TFrame)
            {
                case TimeFrame.ONE_MINUTE: return "ONE_MINUTE";
                case TimeFrame.FIVE_MINUTE: return "FIVE_MINUTE";
                case TimeFrame.TEN_MINUTE: return "TEN_MINUTE";
                case TimeFrame.FIFTEEN_MINUTE: return "FIFTEEN_MINUTE";
                case TimeFrame.THIRTY_MINUTE: return "THIRTY_MINUTE";
                case TimeFrame.ONE_HOUR: return "ONE_HOUR";
                case TimeFrame.TWO_HOUR: return "TWO_HOUR";
                case TimeFrame.FOUR_HOUR: return "FOUR_HOUR";
                case TimeFrame.DAILY: return "DAILY";
                case TimeFrame.WEEKLY: return "WEEKLY";
            }

            return "";
        }
        public TimeFrame ToTimeFrame(string TFrame)
        {

            switch (TFrame)
            {
                case "ONE_MINUTE": return TimeFrame.ONE_MINUTE;
                case "FIVE_MINUTE": return TimeFrame.FIVE_MINUTE;
                case "TEN_MINUTE": return TimeFrame.TEN_MINUTE;
                case "FIFTEEN_MINUTE": return TimeFrame.FIFTEEN_MINUTE;
                case "THIRTY_MINUTE": return TimeFrame.THIRTY_MINUTE;
                case "ONE_HOUR": return TimeFrame.ONE_HOUR;
                case "TWO_HOUR": return TimeFrame.TWO_HOUR;
                case "FOUR_HOUR": return TimeFrame.FOUR_HOUR;
                case "DAILY": return TimeFrame.DAILY;
                case "WEEKLY": return TimeFrame.WEEKLY;
            }

            return TimeFrame.MONTHLY;
        }

        public DateTime ToGMT(TimeZone TZone, DateTime DTime)
        {
            switch(TZone)
            {
                case TimeZone.EasternTime:
                    {
                        TimeZoneInfo TZIEST = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                        DateTime DTEST = DTime.AddHours(5);
                        if (TZIEST.IsDaylightSavingTime(DTEST))
                            DTEST = DTime.AddHours(4);

                        return DTEST;
                    }
               /* case TimeZone.UnitedKingdom: return DTime;
                case TimeZone.CentralEuropeanTime: return DTime.AddHours(1);
                case TimeZone.PacificTime: return DTime.AddHours(-8);
                case TimeZone.ChinaStandardTime: return DTime.AddHours(8);*/
                default: throw new Exception("Wrong Time Zone !");
            }
        }

        public enum TimeZone
        {
            EasternTime = 0,
            /*UnitedKingdom = 1,
            CentralEuropeanTime = 2,
            PacificTime = 3,
            ChinaStandardTime = 4*/

        }

        public DateTime DateNow()
        {
            DateTime DNow = DateTime.Now.Date;
            if (DNow.Hour != 0) DNow = DNow.AddHours(DNow.Hour);
            return DNow;
        }
        public string ToString(double D, int ForceDecimalPlaces)
        {
            string output = D.ToString();

            int LENGTH = output.Count();
            int DECIMAL = output.IndexOf('.');
            string right = "";

            if (DECIMAL < 0)
            {
                if (ForceDecimalPlaces == 0) return output;

                while (right.Length < ForceDecimalPlaces)
                    right += "0";
                

                return output + "." + right;
            }
            else
            {
                string left = output.Substring(0, DECIMAL);
                if (DECIMAL == LENGTH - 1) right = "";
                else right = output.Substring(DECIMAL + 1, LENGTH - (DECIMAL + 1));

                if (right.Count() < ForceDecimalPlaces) while (right.Count() < ForceDecimalPlaces) right += "0";
                else if (right.Count() > ForceDecimalPlaces) right.Substring(0, ForceDecimalPlaces);

                return left + "." + right;
            }
        }

    }
}
