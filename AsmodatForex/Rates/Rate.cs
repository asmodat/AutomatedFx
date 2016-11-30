using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using Asmodat.Abbreviate;

using System.Text.RegularExpressions;

using Asmodat.Serialization;

using Asmodat.Types;

namespace AsmodatForex
{
    public partial class Rate
    {

       
        private string _STATUS;
        private int _DECIMALS;
        private string _NOTATION;
        private double _CLOSINGBID;
        private string _CONTRACTPAIR;
        private string _COUNTERPAIR;
        private int _Token = -1;
        public ChartPoint ChartData = new ChartPoint();



        //[NonSerialized]
        //private string _NULL;
        ////private string _RawData;

        public string Pair { get { return ChartData.Pair; } set { ChartData.Pair = value; } }
        public double OPEN { get { return ChartData.Open; } set { ChartData.Open = value; } }
        public double CLOSE { get { return ChartData.Close; } set { ChartData.Close = value; } }
        /// <summary>
        ///Bid means Sell Price
        /// </summary>
        public double BID { get { return ChartData.BID; } set { ChartData.BID = value; } }
        public double OFFER { get { return ChartData.ASK; } set { ChartData.ASK = value; } }
        public double HIGH { get { return ChartData.High; } set { ChartData.High = value; } }
        public double LOW { get { return ChartData.Low; } set { ChartData.Low = value; } }
        public DateTime DateTime { get { return (DateTime)ChartData.TickTime; } set { ChartData.TickTime = (TickTime)value; } }
        public TickTime TickTime { get { return ChartData.TickTime; } set { ChartData.TickTime = value; } }
        
        public string STATUS { get { return _STATUS; } set { _STATUS = value; } }
        
        public int DECIMALS { get { return _DECIMALS; } set { _DECIMALS = value; } }
        public string NOTATION { get { return _NOTATION; } set { _NOTATION = value; } }
        public double CLOSINGBID { get { return _CLOSINGBID; } set { _CLOSINGBID = value; } }
        public string CONTRACTPAIR { get { return _CONTRACTPAIR; } set { _CONTRACTPAIR = value; } }
        public string COUNTERPAIR { get { return _COUNTERPAIR; } set { _COUNTERPAIR = value; } }
        public int Token { get { return _Token; } set { _Token = value;  } }
        

        


        /// <summary>
        /// This property is set if rate was extracted whitg GetRate methon using archive or rate service
        /// </summary>
        public ServiceConfiguration.TimeFrame Frame { get; set; }


    }
}

///// <summary>
///// This property must be implemented in order to deserialize undefined properties 
///// </summary>
//public string NULL { get { return _NULL; } set { _NULL = value; } }

/*
 * 
 * 
public Rate()
        {
            _BID = -1;
            _OFFER = -1;
            _HIGH = -1;
            _LOW = -1;
            _DECIMALS = -1;
            _CLOSINGBID = -1;
            _DateTime = DateTime.MinValue;
            _Token = -1;
            _OPEN = -1;
            _CLOSE = -1;
            _NULL = null;
        }

        private string _Pair;
        private double _BID;
        private double _OFFER;
        private string _STATUS;
        private double _HIGH;
        private double _LOW;
        private int _DECIMALS;
        private string _NOTATION;
        private double _CLOSINGBID;
        private string _CONTRACTPAIR;
        private string _COUNTERPAIR;
        private DateTime _DateTime;
        private int _Token;
        private double _OPEN;
        private double _CLOSE;
 * 

 */

///// <summary>
//        /// Converts Rate properties into data format "Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR\DateTime$"
//        /// DateTime format properties can be customized
//        /// </summary>
//        /// <param name="dateTimeFormat">Custom date time format.</param>
//        /// <returns>Formatted Rate properties.</returns>
//        public string ToString(string dateTimeFormat)
//        {
//            return SerializeProperties.Serialize(this, Format, @"\", dateTimeFormat);
//        }

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using AsmodatForex;
using Asmodat.Abbreviate;

namespace AsmodatForex
{

    /// <summary>
    /// This class contains 
    /// </summary>
    public class RateInformation
    {
        private static List<string> _Format = new List<string>();
        public static List<string> Format
        {
            get
            {
                if (_Format == null || _Format.Count <= 0)
                {
                    _Format = new List<string>();
                    _Format = Asmodat.Abbreviate.String.ToList(@"");
                }
                return _Format;
            }
        }


        public RateInformation() { }

        public RateInformation(string getRateString)
        {

        }


        /// <summary>
        /// This property returns all accesible as read and write properties of this class in form of objects dictionary
        /// </summary>
        public Dictionary<object, object> GetProperties
        {
            get
            {
                return Objects.GetProperties(this, true, true);
            }
        }

        /// <summary>
        /// This property sets all accesible as read and write properties of this class with use of objects dictionary
        /// </summary>
        public Dictionary<object, object> SetProperties
        {
            set
            {
                Dictionary<object, object> DO2Properties = value;
                if (value == null) return;

                foreach (object oKey in DO2Properties.Keys)
                {
                    if (!(oKey is string)) return;
                    else Objects.SetProperty(this, (string)oKey, DO2Properties[oKey], false, false);
                }
            }

        }
        

       

        


       

        public RateInformation Clone()
        {
            RateInformation RInformation = new RateInformation();
            RInformation.SetProperties = this.GetProperties;

            return RInformation;
        }


        /// <summary>
        /// Converts string data into Rate Informations property
        /// Example rate: ""
        /// Example data: "Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR"
        /// </summary>
        /// <param name="getRateString">Sting formattes as: Pair\BID\OFFER\STATUS\HIGH\LOW\DECIMALS\NOTATION\CLOSINGBID\CONTRACTPAIR\COUNTERPAIR</param>
        /// <returns>RateInformation based on string input.</returns>
        public static RateInformation ToRateInformation(string data)
        {
            List<string> LSRateProperties = Asmodat.Abbreviate.String.ToList(data, "\\");

            if (LSRateProperties.Count() < 11) return null;




            return null;
        }
    
    }
}

 private double _ASK;
        /// <summary>
        /// Market buy price.
        /// </summary>
        public double ASK
        {
            get
            {
                return _ASK;
            }
            set
            {
                _ASK = value;
                _Spread = ASK - BID;
            }
        }

        private double _BID;
        /// <summary>
        /// Market sell price.
        /// </summary>
        public double BID
        {
            get
            {
                return _BID;
            }
            set
            {
                _BID = value;
                _Spread = ASK - BID;
            }
        }

        private double _Spread;
        /// <summary>
        /// Difference bitween ASK & BID, Spread = ASK - BID;
        /// </summary>
        public double Spread
        {
            get
            {
                return _Spread;
            }
        }
 * 
 /// <summary>
        /// CCY Pair
        /// </summary>
        public string CCY_Pair { get; set; }

        /// <summary>
        /// High value for the CCY pair
        /// </summary>
        public double HIGH { get;  set; }
        /// <summary>
        /// Low value for the CCY pair
        /// </summary>
        public double LOW { get; set; }

        /// <summary>
        /// CCY Token
        /// </summary>
        public int CCY_Token { get; set; }

        public bool _Dealable;
        /// <summary>
        /// If true dealable if false referred
        /// </summary>
        public bool Dealable
        {
            get
            {
                return _Dealable;
            }
            set
            {
                _Dealable = value;
                _Reffered = !value;
            }
        }

        public bool _Reffered;
        /// <summary>
        /// If true reffered if false deleable ; this value can be set by Deleable property
        /// </summary>
        public bool Reffered
        {
            get
            {
                return _Reffered;
            }
        }

        private bool _American;
        /// <summary>
        /// //If true American, If false -> European;
        /// </summary>
        public bool American
        {
            get
            {
                return _American;
            }
            set
            {
                _American = value;
                _European = !value;
            }
        }

        private bool _European;
        /// <summary>
        /// //If true European, If false -> American; this value can be set by American property
        /// </summary>
        public bool European
        {
            get
            {
                return _European;
            }
        }
        
        /// <summary>
        /// Decimals for the CCYPair (typically 3 or 5)
        /// </summary>
        public int Decimals
        {
            get;
            set;
        }

        /// <summary>
        /// Close price from prior day's session at 17:00 ET
        /// </summary>
        public double CLOSE
        {
            get;
            set;
        }
 */







//public Rates(DataBase_Rates DBRates)
//{
//    American = DBRates.American;
//    ASK = DBRates.ASK;
//    BID = DBRates.BID;
//    CCY_Token = DBRates.CCY_Token;
//    CLOSE = DBRates.CLOSE;
//    Dealable = DBRates.Dealable;
//    Decimals = DBRates.Decimals;
//    HIGH = DBRates.HIGH;
//    LOW = DBRates.LOW;
//    Time = DBRates.Time;
//}
//public DataBase_Rates ToDataBase()
//{
//    DataBase_Rates DBRates = new DataBase_Rates();
//    DBRates.American = American;
//    DBRates.ASK = ASK;
//    DBRates.BID = BID;
//    DBRates.CCY_Token = CCY_Token;
//    DBRates.CLOSE = CLOSE;
//    DBRates.Dealable = Dealable;
//    DBRates.Decimals = Decimals;
//    DBRates.HIGH = HIGH;
//    DBRates.LOW = LOW;
//    DBRates.Time = Time;

//    return DBRates;
//}
/*

        private DateTime _Time;
        /// <summary>
        /// Eastern Standard Time of Rate Values
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                _Time = value;
                _TimeGMT = this.ToGMT(value);
            }
        }

        private DateTime _TimeGMT;
        /// <summary>
        /// Greenege Mean Time of Rate Value; this value can be set by Time property
        /// </summary>
        public DateTime TimeGMT
        {
            get
            {
                return this._TimeGMT;
            }
        }

        public static DateTime ToGMT(DateTime DTESTime)
        {
            TimeZoneInfo TZIEST = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime DTEST = Time.AddHours(5);
            if (TZIEST.IsDaylightSavingTime(DTEST))
                DTEST = Time.AddHours(4);

            return DTEST;
        }
*/