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

    public partial class Form1 : Form
    {

        private void BtnSavePredictions_Click(object sender, EventArgs e)
        {
            bSavePrediction = true;
            BtnSavePredictions.Enabled = false;
            BtnSavePredictions.Text = " LOADING ... ";
            SIMULATION.Saving = true;
        }

        private bool bSavePrediction = false;
        Thread ThrdPREDICTION;

        List<ChartPointsPredition> LCPsPTestSelected = new List<ChartPointsPredition>();
        List<ChartPointsPredition> LCPsPTestSelected1 = new List<ChartPointsPredition>();

        DateTime DTPredict = DateTime.Now;
        int TradeCurrentExecutionDelay = 0;

        List<Thread> LTSimulation = new List<Thread>();

        private void TimrSimulation_Tick(object sender, EventArgs e)
        {
            if (!ARCHIVE.IsLoaded || !ARCHIVE.Updated)
                return;

            //
            {
                if (!SIMULATION.Saving && !SIMULATION.Saved)
                {
                    BtnSavePredictions.Text = "SAVE PREDICTIONS";
                    BtnSavePredictions.Enabled = true;

                }
                else if (SIMULATION.Saving)
                {
                    BtnSavePredictions.Enabled = false;
                    BtnSavePredictions.Text = "SAVING ... ";
                }

                if (SIMULATION.Saved)
                {
                    bSavePrediction = false;
                }
            }

            if ((ThrdPREDICTION != null && ThrdPREDICTION.IsAlive) || !ARCHIVE.IsLoaded)
                return;

            ThrdPREDICTION = new Thread(delegate()
            {

                List<string> LSProducts = ARCHIVE.GetProducts();//LSProducts[27]
                List<string> LSSubProducts = new List<string>(new string[] { "EUR/USD", "XAU/USD" });//, "USD/JPY", "GBP/USD", "XAU/USD" });// , "USD/CAD" , "XAG/USD" ", "AUD/USD", , "USD/CHF"

                foreach (string product in LSSubProducts)
                    if (!LSProducts.Contains(product)) throw new Exception("Currency: " + product + " misssing in archive !");

              //  LSSubProducts = LSProducts;


                /*SIMULATION.Start(TimeFrame.ONE_MINUTE, 14 * 1440, 20, 10, 10, LSSubProducts, bSavePrediction);
                SIMULATION1.Start(TimeFrame.FIVE_MINUTE, 14 * 1440, 20, 10, 10, LSSubProducts, bSavePrediction);
                SIMULATION2.Start(TimeFrame.FIFTEEN_MINUTE, 14 * 1440, 20, 10, 10, LSSubProducts, bSavePrediction);*/

                for (int i = 0; i < LTSimulation.Count; i++)  LTSimulation[i].Join();

                

                LTSimulation = new List<Thread>();
                LTSimulation.Add(new Thread(delegate() { SIMULATION.Start(TimeFrame.THIRTY_MINUTE, 60 * 24, 4, 2, LSSubProducts, bSavePrediction); }));//30-15,60-30,120-60
                //LTSimulation.Add(new Thread(delegate() { SIMULATION1.Start(TimeFrame.TEN_MINUTE, 42 * 1440, 30, 5, LSSubProducts, bSavePrediction); }));
                //LTSimulation.Add(new Thread(delegate() { SIMULATION2.Start(TimeFrame.FIFTEEN_MINUTE, 42 * 1440, 30, 5, LSSubProducts, bSavePrediction); }));

               /* LTSimulation.Add(new Thread(delegate() {  
                    List<ChartPointsPredition> LCPsPNew = SIMULATION.Select(LSSubProducts);
                    this.Update(ref LCPsPTestSelected, LCPsPNew);
                }));

                LTSimulation.Add(new Thread(delegate()
                {
                    List<ChartPointsPredition> LCPsPNew = SIMULATION1.Select(LSSubProducts);
                    this.Update(ref LCPsPTestSelected1, LCPsPNew);
                }));*/

                 for (int i = 0; i < LTSimulation.Count; i++)
                 {
                     LTSimulation[i].Start();
                     LTSimulation[i].Priority = ThreadPriority.Normal;
                 }

                


                // for (int i = 0; i < LTSimulation.Count; i++ ) LTSimulation[i].Join();

               

            });

            ThrdPREDICTION.Priority = ThreadPriority.Normal;// ThreadPriority.Highest;
            TradeCurrentExecutionDelay = (int)((DateTime.Now - DTPredict).TotalSeconds + 1);
            ThrdPREDICTION.Start();
            TsslInfo2.Text = "Prediction Execution Time: " + TradeCurrentExecutionDelay + " [s]";
            DTPredict = DateTime.Now;



            double dRatio = ((double)(SIMULATION.Saves + 1) / (SIMULATION.Predictions + 1)) * 100;
            TSSPBPredictions.Value = (int)dRatio;
        }

        public void Update(ref List<ChartPointsPredition> LCPsPOld, List<ChartPointsPredition> LCPsPNew)
        {
            

            List<ChartPointsPredition> LCPsPUpdated = new List<ChartPointsPredition>();
            for (int i = 0; i < LCPsPNew.Count; i++)
            {
                bool found = false;
                for (int i2 = 0; i2 < LCPsPOld.Count; i2++)
                {
                    if (LCPsPNew[i].Product == LCPsPOld[i2].Product)
                    {
                        LCPsPOld[i2] = LCPsPNew[i];
                        found = true;
                        break;
                    }
                }

                if (!found)
                    LCPsPOld.Add(LCPsPNew[i]);

            }
        }



        Thread ThrdPREDICTIONTEST;
        private void TimrSimulationTest_Tick(object sender, EventArgs e)
        {
            if (!ARCHIVE.IsLoaded || !ARCHIVE.Updated || !SIMULATION.Updated)
                return;

            if (ThrdPREDICTIONTEST != null && ThrdPREDICTIONTEST.IsAlive)
                return;

            ThrdPREDICTIONTEST = new Thread(delegate()
            {
                if (ARCHIVE.IsLoaded)
                {
                    List<string> LSProducts = ARCHIVE.GetProducts();
                    List<string> LSSubProducts = new List<string>(new string[] { "EUR/USD" });//, "USD/JPY", "GBP/USD", "NZD/USD", "AUD/USD" 
                    
               
                    SIMULATION.Test(LSSubProducts);

                }
            });

            ThrdPREDICTIONTEST.Priority = ThreadPriority.Normal;// ThreadPriority.Highest;
            ThrdPREDICTIONTEST.Start();


        }
    }
}