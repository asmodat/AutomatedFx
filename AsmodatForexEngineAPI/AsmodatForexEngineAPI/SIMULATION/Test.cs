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
        public void TestUnknownPosition(ref ChartPointsPredition CPsPOriginal)
        {
            string product = CPsPOriginal.Product;
            ChartPointsPredition CPsPToAnalise = CPsPOriginal;
            this.Checkup(ref CPsPOriginal);
        }

        public void TestPosition(string product, double gainMultiplayer, int analiseMultiplayer, int index)
        {
            ChartPointsPredition CPsPToAnalise = DATA[product][index];
            CPsPToAnalise.TestID = 1;
            int position = CPsPToAnalise.Position;
            if (position >= (DLSCPoints[product].Count - CPsPToAnalise.Ahead * analiseMultiplayer)) 
                return;

            /*Rates RATE = ORBlotter.Get(product);
            double spread = ANALYSIS.Spread(RATE) * gainMultiplayer; //spread = 0.0003;

            if (CPsPToAnalise.TestSpread == spread)
                return;

            CPsPToAnalise.TestSpread = spread;*/
            this.CheckupTest(ref CPsPToAnalise);//;, DLSCPoints[product].GetRange(CPsPToAnalise.Position, CPsPToAnalise.Ahead), spread);

        }
        int iDataPosition = 0;
        public void Test(List<string> LSProducts)
        {

            for (int i = 0; i < LSProducts.Count; i++)
            {
                string product = LSProducts[i];

                int iDCount = this.DATACount(product);

                if (iDataPosition == 0)
                    iDataPosition = 30;

                if (iDCount > 11 && iDataPosition < (iDCount - 10))
                    ++iDataPosition;
                else continue;

              //  if (iDataPosition == (iDCount - 10)) {   int i22 = 0;  int i33 = i22;  }

               // if (DATA[product][iDataPosition].TestID >= 0) continue;




                if (iDataPosition >= (iDCount - 20))
                {
                    for (int i2 = iDataPosition; i2 >= 0; i2--)
                        if (DATA[product][i2].TestID != 1)
                            this.TestPosition(product, 1, 1, i2);

                    this.ResolutePositionTest(product, iDataPosition);
                }


            }

        }



    }
}
