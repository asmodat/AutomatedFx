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

using System.Collections.Concurrent;

namespace AsmodatForexEngineAPI
{
    public partial class Simulation
    {



        public List<ChartPointsPredition> Select(List<string> LSProducts)
        {
            List<ChartPointsPredition> LCPsPSelected = new List<ChartPointsPredition>();

            for(int i = 0; i < LSProducts.Count; i++)
            {
                string product = LSProducts[i];
                if (!DATA.ContainsKey(product) || DATA[product].Count == 0) continue;

                ChartPointsPredition CPsPOriginal = DATA[product].Last();

                if (!CPsPOriginal.IsActual) 
                    continue;

                for (int i2 = 0; i2 < DATA[product].Count; i2++)
                    this.TestPosition(product, 1.25, 1, i2);



                    this.TestUnknownPosition(ref CPsPOriginal);

                    CPsPOriginal.Resolution = this.Resolute(product, 100, this.DATAFindIndex(CPsPOriginal));

                if (CPsPOriginal.Resolution > 0)
                    LCPsPSelected.Add(CPsPOriginal);
            }

            return LCPsPSelected;
        }
    }
}
