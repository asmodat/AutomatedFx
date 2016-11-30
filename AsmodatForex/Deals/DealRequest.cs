using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;

using System.Runtime.Serialization;

namespace AsmodatForex
{
    /// <summary>
    /// This class consist of memebers that are storing info about current request, 
    /// wheter to execute it, abort as well as information about Deal Response
    /// </summary>
    [DataContract(Name="deal_request")]
    public partial class DealRequest
    {
        [DataMember(Name = "product")]
        [XmlElement("product")]
        public string Product = null;

        [DataMember(Name = "creation")]
        [XmlElement("creation")]
        public TickTime CreationTime = (TickTime)0;

        [DataMember(Name = "expiration")]
        [XmlElement("expiration")]
        public TickTime ExpirationTime = (TickTime)0;

        [DataMember(Name = "bid")]
        [XmlElement("bid")]
        public double BID = -1;

        [DataMember(Name = "ask")]
        [XmlElement("ask")]
        public double ASK = -1;

        [DataMember(Name = "amount")]
        [XmlElement("amount")]
        public double Amount = -1;

        [DataMember(Name = "tolerance")]
        [XmlElement("tolerance")]
        public double Tolerance = -1;

        [DataMember(Name = "buy")]
        [XmlElement("buy")]
        private ThreeState _Buy = ThreeState.Null;

        [DataMember(Name = "sell")]
        [XmlElement("sell")]
        private ThreeState _Sell = ThreeState.Null;

        [DataMember(Name = "close")]
        [XmlElement("close")]
        private ThreeState _Close = ThreeState.Null;

        [DataMember(Name = "close_position")]
        [XmlElement("close_position")]
        private ThreeState _ClosePosition = ThreeState.Null;

        [DataMember(Name = "liquidate_all")]
        [XmlElement("liquidate_all")]
        private ThreeState _LiquidateAll = ThreeState.Null;

        [DataMember(Name = "executed")]
        [XmlElement("executed")]
        public bool Executed = false;

        #region Deal Members
        [DataMember(Name = "deal_sequence")]
        [XmlElement("deal_sequence")]
        public string DealSequence = null;

        [DataMember(Name = "deal_reference")]
        [XmlElement("deal_reference")]
        public string DealReference = null;

        [DataMember(Name = "confirmation_number")]
        [XmlElement("confirmation_number")]
        public string ConfirmationNumber = null;

        [DataMember(Name = "contract")]
        [XmlElement("contract")]
        public string Contract = null;

        [DataMember(Name = "rate")]
        [XmlElement("rate")]
        public string Rate = null;
        #endregion

        [IgnoreDataMember]
        [XmlIgnore]
        public bool Abort = false;

        [IgnoreDataMember]
        [XmlIgnore]
        public bool Buy
        {
            get
            {
                if (_Buy.IsNull) return false;
                return (bool)_Buy;
            }
            set
            {
                _Buy = value;
                _Sell = !value;
            }
        }
        
        [IgnoreDataMember]
        [XmlIgnore]
        public bool Sell
        {
            get
            {
                if (_Sell.IsNull) return false;
                return (bool)_Sell;
            }
            set
            {
                _Sell = value;
                _Buy = !value;
            }
        }
        
        [IgnoreDataMember]
        [XmlIgnore]
        public bool Close
        {
            get
            {
                if (_Close.IsNull) return false;
                return (bool)_Close;
            }
            set
            {
                _Close = value;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool ClosePosition
        {
            get
            {
                if (_ClosePosition.IsNull) return false;
                return (bool)_ClosePosition;
            }
            set
            {
                _ClosePosition = value;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool LiquidateAll
        {
            get
            {
                if (_LiquidateAll.IsNull) return false;
                return (bool)_LiquidateAll;
            }
            set
            {
                _LiquidateAll = value;
            }
        }
        
        
    }
}
