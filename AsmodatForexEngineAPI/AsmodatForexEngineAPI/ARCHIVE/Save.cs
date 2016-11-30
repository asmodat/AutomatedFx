using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;


using System.Collections.Concurrent;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{


    public partial class Archive
    {
        public void Save(string product, DateTime DTime, TimeFrame TFrame)
        {
            string sTFrame = ABBREVIATIONS.ToString(TFrame);
            Record DATA = this.Get(TFrame);
            List<ChartPoint> LCPoints = DATA.GetMonth(product, DTime);
            DATABASE.Save_ChartPoints(LCPoints, product, DTime, sTFrame);
        }

    }
}
