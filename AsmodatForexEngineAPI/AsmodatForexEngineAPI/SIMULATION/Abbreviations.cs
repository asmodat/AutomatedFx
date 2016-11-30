using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{
    public partial class Simulation
    {
        private void ExtractChartPoints(List<ChartPointsPredition> LCPsPSet)
        {
            int iLCPsPSCount = LCPsPSet.Count;
            List<ChartPoint> LCPAll = new List<ChartPoint>();

            for(int i = 0; i < iLCPsPSCount; i++)
            {
                ChartPointsPredition CPsPNow = LCPsPSet[i];
                int iDeep = CPsPNow.Deep;
                LCPAll.AddRange(DLSCPoints[CPsPNow.Product].GetRange(CPsPNow.Position - iDeep, iDeep));
            }

        }


    }
}
