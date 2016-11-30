using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;

namespace Asmodat.Types
{
    //TODO: IFormattable,

    /// <summary>
    /// This class stores and manages DateTime as UTC tick long value, it is safe and efficient to use with serialization instead of DateTime
    /// </summary>
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {
        [IgnoreDataMember]
        [XmlIgnore]
        private static long _LastTimeStamp = DateTime.UtcNow.Ticks;

        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime Now
        {
            get
            {
                long orig, newval;
                do
                {
                    orig = _LastTimeStamp;
                    long now = DateTime.UtcNow.Ticks;
                    newval = Math.Max(now, orig + 1);
                } while (Interlocked.CompareExchange(ref _LastTimeStamp, newval, orig) != orig);

                return new TickTime(newval);
            }
        }


        /// <summary>
        /// Set ticts to TickTime.Now
        /// </summary>
        public void SetNow()
        {
            this.Ticks = TickTime.Now.Ticks;
        }

        public void SetMin()
        {
            this.Ticks = TickTime.MinValue.Ticks;
        }

        public void SetMax()
        {
            this.Ticks = TickTime.MaxValue.Ticks;
        }
    }
}
