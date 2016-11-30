using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

namespace AsmodatForex
{
    public partial class ServiceConfiguration : AbstractServices
    {
        /// <summary>
        /// TimeFrame override for com.efxnow.democharting.chartingservice.TimeFrame
        /// </summary>
        public enum TimeFrame
        {
            LIVE = 1,
            ONE_MINUTE = 2,
            FIVE_MINUTE = 3,
            TEN_MINUTE = 4,
            FIFTEEN_MINUTE = 5,
            THIRTY_MINUTE = 6,
            ONE_HOUR = 7,
            TWO_HOUR = 8,
            FOUR_HOUR = 9,
            DAILY = 10,
            WEEKLY = 11,
            MONTHLY = 12,
        }

        public static bool IsSpan(TimeFrame frame)
        {
            switch (frame)
            {
                case TimeFrame.LIVE: return false;
                case TimeFrame.MONTHLY: return false;
                default: return true;
            }
        }

        public static TimeSpan Span(TimeFrame frame)
        {
            switch (frame)
            {
                case TimeFrame.ONE_MINUTE: return new TimeSpan(0, 1, 0);
                case TimeFrame.FIVE_MINUTE: return new TimeSpan(0, 5, 0);
                case TimeFrame.TEN_MINUTE: return new TimeSpan(0, 10, 0);
                case TimeFrame.FIFTEEN_MINUTE: return new TimeSpan(0, 15, 0);
                case TimeFrame.THIRTY_MINUTE: return new TimeSpan(0, 30, 0);
                case TimeFrame.ONE_HOUR: return new TimeSpan(1, 0, 0);
                case TimeFrame.TWO_HOUR: return new TimeSpan(2, 0, 0);
                case TimeFrame.FOUR_HOUR: return new TimeSpan(4, 0, 0);
                case TimeFrame.DAILY: return new TimeSpan(24, 0, 0);
                case TimeFrame.WEEKLY: return new TimeSpan(168, 0, 0);
            }


            throw new Exception("This time fram does not chave precise span. ");
        }


        //Enums.ToEnum<ServiceConfiguration.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame>(frame);

        
        //AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame _OriginTimeFrames =

        Dictionary<ServiceConfiguration.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame> _TimeFrames = new Dictionary<TimeFrame, com.efxnow.democharting.chartingservice.TimeFrame>();
        public Dictionary<ServiceConfiguration.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame> TimeFrames
        {
            get
            {
                if (_TimeFrames.Count <= 0)
                   _TimeFrames = Enums.ToDictionary<ServiceConfiguration.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame>();

                return _TimeFrames;
            }
        }


        public static double ToMinutes(TimeFrame TimeFrame)
        {
            switch (TimeFrame)
            {
                case TimeFrame.LIVE: return 0;
                case TimeFrame.ONE_MINUTE: return 1;
                case TimeFrame.FIVE_MINUTE: return 5;
                case TimeFrame.TEN_MINUTE: return 10;
                case TimeFrame.FIFTEEN_MINUTE: return 15;
                case TimeFrame.THIRTY_MINUTE: return 30;
                case TimeFrame.ONE_HOUR: return 60;
                case TimeFrame.TWO_HOUR: return 60*2;
                case TimeFrame.FOUR_HOUR: return 60*4;
                case TimeFrame.DAILY: return  60 * 24;
                case TimeFrame.WEEKLY: return 60 * 24 * 7;
                case TimeFrame.MONTHLY: return 60 * 24 * 30.4368499;
                default: return -1;
            }
        }

        public static int ToPeriodsAccess(ServiceConfiguration.TimeFrame TimeFrame)
        {
            switch (TimeFrame)
            {
                case TimeFrame.LIVE: return 0;
                case TimeFrame.ONE_MINUTE: return 15750;//45*350
                case TimeFrame.FIVE_MINUTE: return 10150; //29*350 ~35 days
                case TimeFrame.TEN_MINUTE: return 8750; //25*350 ~60 days
                case TimeFrame.FIFTEEN_MINUTE: return 8400; //24*350 ~85 days
                case TimeFrame.THIRTY_MINUTE: return 7700;//22*350 ~160 days
                case TimeFrame.ONE_HOUR: return 7350;//21*350 ~305 days
                case TimeFrame.TWO_HOUR: return 7350;//21*350 ~605 days
                case TimeFrame.FOUR_HOUR: return 7350;
                case TimeFrame.DAILY: return 3850;//11*350 ~10years
                case TimeFrame.WEEKLY: return 1050;//3*350 ~15years
                case TimeFrame.MONTHLY: return 350;//1*350~15years
                default: return -1;
            }
        }

    }
}
