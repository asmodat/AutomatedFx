#region References
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Added references
using AsmodatSerialization;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;
#endregion

namespace AsmodatForexEngineAPI
{
    public class ChartPoint
    {

        #region Variables & Initialization
        Abbreviations ABBREVIATIONS = new Abbreviations();
        public double HIGH, LOW, OPEN, CLOSE;
        public DateTime Time;
        #endregion

        #region Main Properties
        //To define chart point: Raise & Fall & Peak & Base
        public double Raise
        {
            get//rfpb
            {
                return HIGH - CLOSE;
            }
        }
        public double Fall
        {
            get
            {
                return CLOSE - LOW;
            }
        }
        public double Peak
        {
            get
            {
                return HIGH - OPEN;
            }
        }
        public double Base
        {
            get
            {
                return OPEN - LOW;
                //return LOW - OPEN;
            }
        }
        public double Activity
        {
            get
            {
                return HIGH - LOW;
            }

        }
        public double Change
        {
            get
            {
                return (CLOSE - OPEN);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// The default constuctor with no need of initialisqtion - is not safe to use !
        /// </summary>
        public ChartPoint( )
        {
        }
        /// <summary>
        /// The proper constructor that can be used to create ChartPoint with use of ChartData
        /// </summary>
        /// <param name="CLOSE">Closing Price of TimeFrame (Bid)</param>
        /// <param name="OPEN">Opening Price of TimeFrame (Bid)</param>
        /// <param name="HIGH">Highest Price in TimeFrame (Bid)</param>
        /// <param name="LOW">Lowest Price in TimeFrame (Bid)</param>
        /// <param name="Time">Time of begining of ChartData (GMT)</param>
        public ChartPoint(double CLOSE, double OPEN, double HIGH, double LOW, DateTime Time)
        {
            this.CLOSE = CLOSE;
            this.OPEN = OPEN;
            this.HIGH = HIGH;
            this.LOW = LOW;
            this.Time = Time;
        }
        /// <summary>
        /// Constructor that can be use to create restore ChartPoint of its DataBase equivalent
        /// </summary>
        /// <param name="DBCPoint"></param>
        public ChartPoint(DataBase_ChartPoint DBCPoint)
        {
            CLOSE = DBCPoint.CLOSE;
            OPEN = DBCPoint.OPEN;
            HIGH = DBCPoint.HIGH;
            LOW = DBCPoint.LOW;
            Time = DBCPoint.Time;
        }
        #endregion



        /// <summary>
        /// Allows to convert chart point to its DataBase equivelent that can be store lates in the Hard Drive
        /// </summary>
        /// <returns>Returns DataBase equivelent of ChartPoint </returns>
        public DataBase_ChartPoint ToDataBase()
        {
            DataBase_ChartPoint DBCPoint = new DataBase_ChartPoint();


            DBCPoint.CLOSE = CLOSE;
            DBCPoint.OPEN = OPEN;
            DBCPoint.HIGH = HIGH;
            DBCPoint.LOW = LOW;
            DBCPoint.Time = Time;

            return DBCPoint;
        }

        /// <summary>
        /// Specifies if ChartPoint is up to date in respect to specified parameters
        /// </summary>
        /// <param name="TFrame">TimeFrame of chart point</param>
        /// <param name="percentageTimeLeft">Percent of time left to expire</param>
        /// <returns>True if ChartPoint is uo to date, else false</returns>
        public bool IsActual(TimeFrame TFrame, double percentageTimeLeft)
        {
            int iSeconds = ABBREVIATIONS.ToMinutes(TFrame) * 60;
            DateTime DTNextFrame = this.Time.AddSeconds(iSeconds);
            double iSecondsAhead = (DTNextFrame - ABBREVIATIONS.GreenwichMeanTime).TotalSeconds;

            double dFactor = (percentageTimeLeft / 100);


            if (iSecondsAhead >= (iSeconds * dFactor))
                return true;
            else return false;
        }


        public double GetParam(int paramID, int decimals)
        {
            switch (paramID)
            {
                case 0: return Math.Round(this.Raise, decimals);

                case 1: return Math.Round(this.Fall, decimals);

                case 2: return Math.Round(this.Peak, decimals);

                case 3: return Math.Round(this.Base, decimals);

                case 4: return Math.Round(this.Change, decimals);

                default: return -1;

            }
        }

        public double GetParamQuick(int paramID)
        {
            switch (paramID)
            {
                case 0: return this.Raise;

                case 1: return this.Fall;

                case 2: return this.Peak;

                case 3: return this.Base;

                case 4: return this.Change;

                default: return -1;

            }
        }


    }
}
