using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;

using System.Runtime.Serialization;

using AsmodatMath;

namespace AsmodatForex
{
    [DataContract(Name = "account")]
    public partial class Account
    {


        #region Margin Data
        [DataMember(Name = "margin_origin")]
        [XmlElement("margin_origin")]
        public TickTime MarginOrigin = (TickTime)0;

        [DataMember(Name = "availible_position")]
        [XmlElement("availible_position")]
        public double AvailablePosition;

        [DataMember(Name = "margin_balance")]
        [XmlElement("margin_balance")]
        public double MarginBalance;

        [DataMember(Name = "margin_factor")]
        [XmlElement("margin_factor")]
        public double MarginFactor;

        [DataMember(Name = "max_deal_availible")]
        [XmlElement("max_deal_availible")]
        public double MaxDealAvailable;

        [DataMember(Name = "open_position")]
        [XmlElement("open_position")]
        public double OpenPosition;

        [DataMember(Name = "posted_margin")]
        [XmlElement("posted_margin")]
        public double PostedMargin;

        [DataMember(Name = "realized_profit")]
        [XmlElement("realized_profit")]
        public double RealizedProfit;

        [DataMember(Name = "unrealized_profit")]
        [XmlElement("unrealized_profit")]
        public double UnrealizedProfit;

        [DataMember(Name = "usd_posted_margin")]
        [XmlElement("usd_posted_margin")]
        public double USDPostedMargin;

        [DataMember(Name = "usd_realized_profit")]
        [XmlElement("usd_realized_profit")]
        public double USDRealizedProfit;
        #endregion

        


        /// <summary>
        /// this fileld contains previous margin balance while all deals are closed
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double ClosedBalance;

        /// <summary>
        /// this fileld contains first - all time margin balance while all deals were closed
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double OriginBalance;

        /// <summary>
        /// Unrealized live balance if all deals would be closed at this point
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double LiveBalance
        {
            get
            {
                return RealizedBalance + LiveProfit;
            }
        }

        /// <summary>
        /// At specified time this balance value can be trusted to be an availible minimum  
        /// </summary>
        public double TrustBalance
        {
            get
            {
                return AMath.Min(LiveBalance, ClosedBalance, RealizedBalance);
            }
        }


        /// <summary>
        /// this filed contains realized balance, that is safely availible for dealing (current balance with unrealized profit)
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double RealizedBalance
        {
            get
            {
                return MarginBalance + UnrealizedProfit;
            }
        }

        /// <summary>
        /// this filed contains profit if all oppened deals would be instantly closed
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double LiveProfit;

        /// <summary>
        /// margin multiplayer of gain and losses
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public double Leverage
        {
            get
            {
                return this.MaxDealAvailable / this.MarginBalance;
            }
        }


    }
}


///// <summary>
///// this filed contains all times margin balance if all oppened deals would be instantly closed
///// </summary>
//[IgnoreDataMember]
//[XmlIgnore]
//public double OriginLiveMargin
//{
//    get
//    {
//        return OriginBalance + OriginProfit;
//    }
//}

///// <summary>
///// this filed contains all times profit if all oppened deals would be instantly closed
///// </summary>
//[IgnoreDataMember]
//[XmlIgnore]
//public double OriginProfit
//{
//    get
//    {
//        return (ClosedBalance - OriginBalance) + _LiveProfit;
//    }
//}

///// <summary>
///// this field contains origin tick time of live margin balance
///// </summary>
//[IgnoreDataMember]
//[XmlIgnore]
//public TickTime LiveOrigin = (TickTime)0;