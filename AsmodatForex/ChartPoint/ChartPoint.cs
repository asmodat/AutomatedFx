using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;

using System.Runtime.Serialization;

namespace AsmodatForex
{

    /// <summary>
    /// Stores Rate Data
    /// </summary>
    [DataContract(Name="chart_point")]
    public partial struct ChartPoint
    {
        [DataMember(Name = "pair")]
        [XmlElement("pair")]
        public string Pair;

        [DataMember(Name = "ask")]
        [XmlElement("ask")]
        public double ASK;

        [DataMember(Name = "bid")]
        [XmlElement("bid")]
        public double BID;

        [DataMember(Name = "open")]
        [XmlElement("open")]
        public double Open;

        [DataMember(Name = "close")]
        [XmlElement("close")]
        public double Close;

        [DataMember(Name = "high")]
        [XmlElement("high")]
        public double High;

        [DataMember(Name = "low")]
        [XmlElement("low")]
        public double Low;

        [DataMember(Name = "tick_time")]
        [XmlElement("tick_time")]
        public TickTime TickTime;
    }
}

//public string Serialize()
//{
//    StringWriter SWriter = new StringWriter();
//    SWriter.WriteLine(Pair);
//    SWriter.WriteLine(ASK);
//    SWriter.WriteLine(BID);
//    SWriter.WriteLine(Open);
//    SWriter.WriteLine(Close);
//    SWriter.WriteLine(High);
//    SWriter.WriteLine(Low);
//    SWriter.WriteLine(TickTime.Ticks);
//    return SWriter.ToString();
//}

//public void Deserialize(string data)
//{
//    StringReader SReader = new StringReader(data);
//    Pair = SReader.ReadLine();
//    ASK = System.Double.Parse(SReader.ReadLine());
//    BID = System.Double.Parse(SReader.ReadLine());
//    Open = System.Double.Parse(SReader.ReadLine());
//    Close = System.Double.Parse(SReader.ReadLine());
//    High = System.Double.Parse(SReader.ReadLine());
//    Low = System.Double.Parse(SReader.ReadLine());
//    TickTime = new TickTime(System.Int64.Parse(SReader.ReadLine()));
//}

//Rate.Pair = LSRateProperties[0];
//                Rate.BID = System.Double.Parse(LSRateProperties[1]);
//                Rate.OFFER = System.Double.Parse(LSRateProperties[2]);
//                Rate.OPEN = System.Double.Parse(LSRateProperties[3]);
//                Rate.CLOSE = System.Double.Parse(LSRateProperties[4]);
//                Rate.HIGH = System.Double.Parse(LSRateProperties[5]);
//                Rate.LOW = System.Double.Parse(LSRateProperties[6]);
//                Rate.DateTime = DateTime.ParseExact(LSRateProperties[7], "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);