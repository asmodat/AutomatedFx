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
        public DealRequest()
        {

        }

        public DealRequest(int expiration)
        {
            this.CreationTime = TickTime.Now;
            this.ExpirationTime = this.CreationTime + TickTime.ms * expiration;
        }
    }
}
