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
    public partial class DealRequest
    {


        [IgnoreDataMember]
        [XmlIgnore]
        public string BuySell
        {
            get
            {
                //if (!Close)
                //{
                    if (Buy) return "B";
                    else if (Sell) return "S";
                //}
                //else
                //{
                //    if (Sell) return "B";
                //    else if (Buy) return "S";
                //}

                return null;
            }
            set
            {
                string val = value.ToUpper();
                if (val == "B")
                {
                    Buy = true;
                    return;
                }
                else if (val == "S")
                {
                    Sell = true;
                    return;
                }

                throw new Exception("Unknown BuySell format ('" + value + "'), only 'B' or 'C' char's allowed  !");
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsAlive
        {
            get
            {
                if 
                    (

                    this.Executed || //Its is not alive it is already executed
                    this.Expired || //time expired
                    this.Abort 
                    )
                    return false;
                else return true;
            }
        }


        /// <summary>
        /// This property defines wheter or not request is still "fresh" and can be executed
        /// </summary>
        public bool Expired
        {
            get
            {
                return this.ExpirationTime.Span > 0;
            }
        }
    }
}


//||//must be abandoned
//                    (!ClosePosition && !LiquidateAll && this.Amount == 0) || //value exception
//                    (this.Amount != 0 && (_Buy.IsNull || _Sell.IsNull || _Close.IsNull))
