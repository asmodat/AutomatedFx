using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using AsmodatSerialization;

namespace AsmodatForexEngineAPI
{
    public class Rates
    {

        public Rates()  { }

        public Rates(DataBase_Rates DBRates)
        {
            American = DBRates.American;
            ASK = DBRates.ASK;
            BID = DBRates.BID;
            CCY_Token = DBRates.CCY_Token;
            CLOSE = DBRates.CLOSE;
            Dealable = DBRates.Dealable;
            Decimals = DBRates.Decimals;
            HIGH = DBRates.HIGH;
            LOW = DBRates.LOW;
            Time = DBRates.Time;
        }
        public DataBase_Rates ToDataBase()
        {
            DataBase_Rates DBRates = new DataBase_Rates();
            DBRates.American = American;
            DBRates.ASK = ASK;
            DBRates.BID = BID;
            DBRates.CCY_Token = CCY_Token;
            DBRates.CLOSE = CLOSE;
            DBRates.Dealable = Dealable;
            DBRates.Decimals = Decimals;
            DBRates.HIGH = HIGH;
            DBRates.LOW = LOW;
            DBRates.Time = Time;

            return DBRates;
        }
        public Rates Clone()
        {
            Rates RATE = new Rates();
            RATE.American = American;
            RATE.ASK = ASK;
            RATE.BID = BID;
            RATE.CCY_Token = CCY_Token;
            RATE.CCY_Pair = CCY_Pair;
            RATE.CLOSE = CLOSE;
            RATE.Dealable = Dealable;
            RATE.Decimals = Decimals;
            RATE.HIGH = HIGH;
            RATE.LOW = LOW;
            RATE.Time = Time;


            return RATE;
        }


        public string CCY_Pair;
        public double BID; //Market buy price.
        public double ASK; //Market sell price.
        public double HIGH;
        public double LOW;
        public double CLOSE; //End price from prior session.
        public DateTime Time;
        public int CCY_Token;
        public bool Dealable; //If False -> Referred;
        public bool American; //If False -> European;
        public int Decimals;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milisecons">Defines maximum time to wait for update</param>
        /// <returns>Defines time of execution in milisecons</returns>
        public int WaitForUpdate(int milisecons)
        {
            int count = 0;
            DateTime past = Time;
            while (past == Time)
            {
                Thread.Sleep(5);
                ++count;

                if (count * 5 >= milisecons) return -1;
            }

            return count * 5;
        }

        public double Spread
        {
            get
            {
                return ASK - BID;
            }

        }

        public DateTime TimeGMT
        {
            get
            {
                TimeZoneInfo TZIEST = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                DateTime DTEST = Time.AddHours(5);
                if (TZIEST.IsDaylightSavingTime(DTEST))
                    DTEST = Time.AddHours(4);

                return DTEST;
            }
        }
    
    
    }
}
