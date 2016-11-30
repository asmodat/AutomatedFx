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
        private ChartPointsPredition Predict(TimeFrame TFrame, string product, int deep, int ahead, int position, List<ChartPoint> LCPoints)
        {

            

            int iDeep = deep;
            int iAhead = ahead;

            if (position < iDeep)
                return null;


            int iDecimals = int.Parse(OSBlotter.Get(product).DecimalPlaces);
            int iMinMatches = 1;
            double dMinResolution = 1;
            List<ChartPoint> LCPSub = LCPoints.GetRange(0, position);
            List<ChartPoint> LCPSubSpecified = LCPoints.GetRange(position - iDeep, iDeep);
            //List<ChartPoint> LCPSubPredicted = LCPoints.GetRange(position, iAhead);//TODO comment

            

            ChartPointsPredition CPsP = ANALYSIS.PredictNextSpecified(product, LCPSub, LCPSubSpecified, TFrame, iDecimals, iMinMatches, dMinResolution, iAhead, new double[] { 0, 0, 0, 0, 0 });
            CPsP.Prognosis(0);
            CPsP.Position = position;
            return CPsP;
        }


    }
}
