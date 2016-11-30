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
        DataBase DATABASE;
        Account ACCOUNT;
        Analysis ANALYSIS;
        OpenSettingsBlotter OSBlotter;
        OpenRatesBlotter ORBlotter;
        Archive ARCHIVE;
        Abbreviations ABBREVIATIONS = new Abbreviations();
        Dictionary<string, List<ChartPoint>> DLSCPoints = new Dictionary<string, List<ChartPoint>>();
        Dictionary<string, List<ChartPointsPredition>> DATA = new Dictionary<string, List<ChartPointsPredition>>();
        private Thread ThrdMain;
        private Thread ThrdTest;
        bool bSaving = false;
        int iPredictions = 0;
        int iSaved = 0;
        private bool bUpdated = false;
        List<Thread> LTSimulation = new List<Thread>();

        public bool Updated
        {
            get
            {
                return bUpdated;
            }
        }
        public bool Saving
        {
            get
            {
                return bSaving;
            }
            set
            {
                bSaving = value;
            }
        }
        public bool Saved
        {
            get
            {
                if (iPredictions == iSaved)
                    return true;
                else return false;
            }
        }
        public int Predictions
        {
            get
            {
                return iPredictions;
            }
        }
        public int Saves
        {
            get
            {
                return iSaved;
            }
        }

        public int DATAFindIndex(ChartPointsPredition CPsP)
        {
            for (int i = DATA[CPsP.Product].Count - 1; i >= 0; i--)
            {
                if (DATA[CPsP.Product][i] == CPsP)
                    return i;
            }

            return -1;
        }
        public void DATASort(string product)
        {
            DATA[product] = DATA[product].OrderBy(CPsP => CPsP.Position).ToList();
        }
        public int DATANextPosition(string product, int maxCount)
        {
            int iStartPositon = DLSCPoints[product].Count - 1;//Position of latest (fresh) data in archive
            int iCount = DATA[product].Count;
            if (!DATA.ContainsKey(product) || iCount == 0 || DATA[product].Last().Position < iStartPositon)
                return iStartPositon;

            int iPositionGap = -1;
            for (int i = iCount - 1; i > 0; i--)
                if ((DATA[product][i].Position - DATA[product][i - 1].Position) > 1)//Position gap found
                {
                    iPositionGap = DATA[product][i].Position - 1;
                    break;
                }

            

            if (iPositionGap < 0 && DATA[product][0].Position > 0)
                iPositionGap = DATA[product][0].Position - 1;

            if (iPositionGap > iStartPositon || iCount >= maxCount)
                iPositionGap = -1;

            return iPositionGap;
        }
        public int DATACount(string product)
        {
            if (!DATA.ContainsKey(product))
                return 0;
            else return DATA[product].Count();
        }


        

        public Simulation(ref OpenRatesBlotter ORBlotter, ref OpenSettingsBlotter OSBlotter, ref Archive ARCHIVE, ref Account ACCOUNT, ref DataBase DATABASE)
        {
            ANALYSIS = new Analysis(ref ARCHIVE, ref ACCOUNT);
            this.ARCHIVE = ARCHIVE;
            this.OSBlotter = OSBlotter;
            this.ORBlotter = ORBlotter;
            this.ACCOUNT = ACCOUNT;
            this.DATABASE = DATABASE;
        }

        


        private void StartNext(string product, TimeFrame TFrame, int count, int deep, int ahead, bool AutoSave, int position)
        {
            ChartPointsPredition CPsPNew = this.Predict(TFrame, product, deep, ahead, position, DLSCPoints[product]);

            if(CPsPNew == null)
            {
                return;
            }

            DATA[product].Add(CPsPNew);
            ++iPredictions;
            this.DATASort(product);
            if (AutoSave)
                this.Save(DATA[product]);
        }
        public void Start(TimeFrame TFrame, int count, int deep, int ahead, List<string> LSProducts, bool AutoSave)
        {

            if (AutoSave)
                bSaving = true;

            
            //this.LoadOptions(TFrame, LSProducts, (5 * 1440));

            List<string> LSTProducts = new List<string>();
            DateTime DTTimr0 = DateTime.Now;

            for (int i2 = 0; i2 < LSProducts.Count; i2++)
            {
                string product = LSProducts[i2];

                if (!LSTProducts.Contains(product)) // Dont repeat simulation
                    LSTProducts.Add(product);
                else continue;


                this.Load(product, TFrame, deep, ahead, count);

                int iPosition = DATANextPosition(product, count);

                if (iPosition < deep)
                    continue;

                LTSimulation.Add(new Thread(() => this.StartNext(product, TFrame, count, deep, ahead, AutoSave, iPosition)));
                LTSimulation.Last().Priority = ThreadPriority.Normal;//Highest;
                LTSimulation.Last().Start();
            }

            for (int i = 0; i < LTSimulation.Count; i++)
                LTSimulation[i].Join();


                if (AutoSave)
                {
                    iSaved = iPredictions;
                    bSaving = false;
                    AutoSave = false;
                }



            double dMS = (DateTime.Now - DTTimr0).TotalMilliseconds;
            int nope = (int)dMS;

            bUpdated = true;

        }


        

    }


}
