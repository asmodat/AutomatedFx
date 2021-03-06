﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;
using System.Runtime.Serialization;
using System.Globalization;



namespace Asmodat.Types
{
    //TODO: IFormattable,

    /// <summary>
    /// This class stores and manages DateTime as UTC tick long value, it is safe and efficient to use with serialization instead of DateTime
    /// </summary>
    [Serializable]
    [DataContract(Name = "tick_time")]
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {
        public TickTime(long Ticks)
        {
            this.Ticks = Ticks;
        }
        public TickTime(DateTime DateTime)
        {
            this.Ticks = DateTime.ToUniversalTime().Ticks;
        }

        [DataMember(Name = "ticks")]
        [XmlElement("ticks")]
        public long Ticks;


        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime UTC
        {
            get
            {
                return new DateTime(Ticks, DateTimeKind.Utc);
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc) throw new ArgumentException("Passed Date is not UTC");
                Ticks = value.ToUniversalTime().Ticks;//you cannot be ceratin that passes value is UTC
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public const string FormatLongDateTime = "yyyy-MM-dd HH:mm:ss.fff";


        /// <summary>
        /// Converts UTC tick time into LongFormat string: "yy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <returns>Returns formated string "yy-MM-dd HH:mm:ss.fff"</returns>
        public string ToLongDateTimeString()
        {
            return this.UTC.ToString(FormatLongDateTime, CultureInfo.InvariantCulture);
        }

        public string ToStringExact(string format)
        {
            return this.UTC.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses string date using specified format from const object's of TickTime class
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static TickTime ParseExact(string date, string format)
        {
            return new TickTime(DateTime.ParseExact(date, format, CultureInfo.InvariantCulture));
        }


        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime Local
        {
            get
            {
                return new DateTime(Ticks, DateTimeKind.Utc).ToLocalTime();
            }
            set
            {
                if (value.Kind != DateTimeKind.Local) throw new ArgumentException("Passed Date is not a local time");
                Ticks = value.ToUniversalTime().Ticks;
            }
        }

        /// <summary>
        /// This property returns current span between now and stored tick value
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public long Span
        {
            get
            {
                return (TickTime.Now.Ticks - this.Ticks);
            }
        }

        


        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime MinValue { get { return new TickTime(0); } }

        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime MaxValue { get { return new TickTime(long.MaxValue); } }

        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime us { get { return new TickTime(10); } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime ms { get { return TickTime.us * 1000; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime s { get { return TickTime.ms * 1000; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime m { get { return TickTime.s * 60; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime h { get { return TickTime.m * 60; ; } }

        


        public int CompareTo(object obj)
        {
            TickTime that = (TickTime)obj;

            return this.Ticks.CompareTo(that.Ticks);
        }

        public int CompareTo(TickTime that)
        {
            return this.Ticks.CompareTo(that.Ticks);
        }

        public int CompareTo(long that)
        {
            return this.Ticks.CompareTo(that);
        }

        public override string ToString()
        {
            return Ticks.ToString();
        }
        public static TickTime Parse(string value)
        {
            return new TickTime(long.Parse(value));
        }

        public bool Equals(TickTime that)
        {
            return this.Ticks == that.Ticks;
        }
        public bool Equals(long that)
        {
            return this.Ticks == that;
        }
        public override bool Equals(object obj)
        {
            if (obj is TickTime) return Equals((TickTime)obj);
            else if (!Objects.IsNumber(obj)) return false;

            return Ticks == (long)obj;
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

    }
}
///// <summary>
///// Returns UTC current tick time
///// </summary>
//[XmlIgnore]
//public static long Now
//{
//    get
//    {
//        return Asmodat.Abbreviate.UTC.Now; //System.Diagnostics.Stopwatch.GetTimestamp(); //DateTime.Now.ToUniversalTime().Ticks;
//    }
//}


///// <summary>
///// Returns UTC date time
///// </summary>
//[XmlIgnore]
//public static DateTime Now
//{
//    get
//    {
//        return new DateTime(TickTime.Stamp, DateTimeKind.Utc);
//    }
//}



        //[XmlIgnore]
        //public DateTime Unspecified
        //{
        //    set
        //    {
        //        Ticks = value.ToUniversalTime().Ticks;//you cannot be ceratin that passes value is UTC
        //    }
        //}

//public long Total(DateTime DT)
//{
//    return (DT.ToUniversalTime().Ticks - this.Ticks);
//}


//


//public int CompareTo(object obj)
//{
//    return 1;
//}

//public int CompareTo(TickTime TT)
//{
//    if (this.Ticks < TT.Ticks) return 1;
//    else if (this.Ticks > TT.Ticks) return -1;
//    else return 0;
//}

//public static bool operator ==(TickTime x, TickTime y)
//{
//    return x.Ticks == y.Ticks ? true : false;
//}
//public static TickTime operator !=(TickTime x, TickTime y)
//{
//    if (x.Value == 0 || y.Value == 0) return Null;
//    return x.Value != y.Value ? True : False;
//}