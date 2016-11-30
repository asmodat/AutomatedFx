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

        public bool Load(string product, TimeFrame TFrame, int deep, int ahead, int count)
        {

            if (DLSCPoints.ContainsKey(product)) //if up to date, refresh data base and return
            {
                DLSCPoints[product] = ARCHIVE.GetDATA(TFrame, product);
                return false;
            }
            else  DLSCPoints.Add(product, ARCHIVE.GetDATA(TFrame, product));


            List<ChartPointsPredition> LDCPsPoints = DATABASE.Load_ChartPointsPrediction(product, ABBREVIATIONS.ToString(TFrame), deep, ahead);

            
            int iPosition = DLSCPoints[product].Count - count;

            if (LDCPsPoints == null || LDCPsPoints.Count == 0)
                DATA.Add(product, new List<ChartPointsPredition>());
            else
            {
                if (LDCPsPoints.Count > count)
                    LDCPsPoints.RemoveRange(0, LDCPsPoints.Count - count);

                for (int i = 0; i < LDCPsPoints.Count; i++)
                    LDCPsPoints[i].Prognosis(0);

                DATA.Add(product, LDCPsPoints);
            }

            return true;
        }


        
    }
}
