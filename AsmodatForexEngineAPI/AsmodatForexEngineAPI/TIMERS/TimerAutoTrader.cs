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
        private void BtnAutoTrader_Click(object sender, EventArgs e)
        {
            if (!OSBlotter.IsLoaded) return;

            if(TimrAutoTrader.Enabled == true)
            {
                TimrAutoTrader.Enabled = false;
                TimrAutoTraderSquare.Enabled = false;
                BtnAutoTrader.Text = "START AUTO TRADING";
            }
            else
            {
                TimrAutoTrader.Enabled = true;
                TimrAutoTraderSquare.Enabled = true;
                BtnAutoTrader.Text = "STOP AUTO TRADING";
            }
        }



        private void TimrAutoTraderSquare_Tick(object sender, EventArgs e)
        {



            if (ODBlotter.COUNT == 0) return;
            else
            {
                TimeFrame TFrame = TimeFrame.ONE_MINUTE;
                List<Deals> LDeals = ODBlotter.GetData.ToList();

                for (int i = 0; i < LDeals.Count; i++)
                {
                    Deals DEAL = LDeals[i];
                    string product = DEAL.PRODUCT;
                    List<ChartPointsPredition> LCPsPSimulated = this.LCPsPTestSelected.OrderByDescending(CPsP => CPsP.Resolution).ToList();
                    ChartPointsPredition CPsPrediction = null;
                    List<Rates> LRAll = ORBlotter.Archive.TryGet(product);

                    for(int i2 = 0; i2 < LCPsPSimulated.Count; i2++)
                        if (LCPsPSimulated[i2].Product == product)
                        {
                            CPsPrediction = LCPsPSimulated[i2];
                            break;
                        }

                    


                    double dMinDeal = 0, dMaxDeal = 0;
                    if (!this.Extrema(DEAL, 10, ref dMinDeal, ref dMaxDeal))
                        return;

                    bool bBUY = DEAL.BUY;
                    Rates RATE = ORBlotter.Get(DEAL.PRODUCT);
                    double onePip = Math.Pow(10, -RATE.Decimals);
                    int iSpread = ANALYSIS.SpreadPips(DEAL, RATE.Decimals, true) + 2;
                    double dLevel = (double)iSpread;
                    bool bLooseWarning = false;
                    double dGainLoosePercentage = 75;
                    double dStartSpread = (double)(DEAL.ASK - DEAL.BID) / onePip;

                    if (CPsPrediction != null && CPsPrediction.IsActual)
                    {

                        if ((bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Down) ||
                            (!bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Up))
                        {
                            DEAL.LooseWaring = true;
                            dGainLoosePercentage = 85;
                        }
                        else if ((!bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Down) ||
                            (bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Up))
                        {
                            dGainLoosePercentage = 60;
                        }
                    }

                    if (bBUY)
                    {
                        
                        double dPLMax = (double)(dMaxDeal - DEAL.ASK) / onePip;
                        double dPLNow = (double)(RATE.BID - DEAL.ASK) / onePip;

                        

                       if (dPLMax > 0 && dPLNow > 0)
                        {
                            double dGainPercentage = ((double)(dPLNow + dStartSpread) / (dPLMax + dStartSpread)) * 100;

                            if (dGainPercentage < dGainLoosePercentage || DEAL.LooseWaring)
                                bLooseWarning = true;
                        }
                    }
                    else//sell for bid, buy for ask
                    {
                        double dPLMin = (double)(DEAL.BID - dMinDeal) / onePip;
                        double dPLNow = (double)(DEAL.BID - RATE.ASK) / onePip;


                        if (dPLMin > 0 && dPLNow > 0)
                        {
                            double dGainPercentage = ((double)(dPLNow + dStartSpread) / (dPLMin + dStartSpread)) * 100;

                            if (dGainPercentage < dGainLoosePercentage || DEAL.LooseWaring)
                                bLooseWarning = true;

                        }
                    }

                    if (bLooseWarning)
                    {
                        this.CloseDeal(DEAL);
                        Thread ThrdBeep = new Thread(delegate() { Console.Beep(200, 50); Console.Beep(200, 50); }); ThrdBeep.Start();
                    }
                    return;



                }



            }
        }



        public bool Extrema(string product, int lastMinutes, ref double min, ref double max)
        {
            List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TimeFrame.ONE_MINUTE, product, ABBREVIATIONS.GreenwichMeanTime.AddMinutes(-lastMinutes));

            min = double.MaxValue;
            max = double.MinValue;
            for (int i = 0; i < LCPoints.Count; i++)
            {
                if (LCPoints[i].LOW <= min) min = LCPoints[i].LOW;
                else if (LCPoints[i].HIGH >= max) max = LCPoints[i].HIGH;
            }


            if (min < double.MaxValue && max > double.MinValue)
                return true;
            else return false;
        }
        public bool Extrema(Deals DEAL, int lastSeconds, ref double min, ref double max)
        {
            DateTime DTDealGMT = ABBREVIATIONS.ToGMT(Abbreviations.TimeZone.EasternTime, DEAL.TIME);

            if ((ABBREVIATIONS.GreenwichMeanTime - DTDealGMT).TotalSeconds < lastSeconds)
                return false;


            List<Rates> LRDeal = ORBlotter.Archive.Get(DEAL.PRODUCT, DTDealGMT);
            int iRLCount = LRDeal.Count;

            min = double.MaxValue;
            max = double.MinValue;

            if (DEAL.BUY)
            {

                for (int i = 0; i < iRLCount; i++)
                    if (LRDeal[i].BID <= min) min = LRDeal[i].BID;
                    else if (LRDeal[i].BID >= max) max = LRDeal[i].BID;
            }
            else
            {

                for (int i = 0; i < iRLCount; i++)
                    if (LRDeal[i].ASK <= min) min = LRDeal[i].ASK;
                    else if (LRDeal[i].ASK >= max) max = LRDeal[i].ASK;
            }




            if (min < double.MaxValue && max > double.MinValue)
                return true;
            else return false;
        }



        Thread ThrdAutoTread = null;
        private void TimrAutoTrader_Tick(object sender, EventArgs e)
        {
            //if (ThrdAutoTread != null && ThrdAutoTread.IsAlive) return;
            
            List<ChartPointsPredition> LCPsPSimulated = this.LCPsPTestSelected.OrderByDescending(CPsP => CPsP.Resolution).ToList();
            

            if (LCPsPSimulated.Count < 1) return;

            ChartPointsPredition CPsPBest = null;

            List<Deals> LDeals = ODBlotter.GetData;

            for (int i = 0; i < LCPsPSimulated.Count; i++)
            {
                if(!LCPsPSimulated[i].IsActual) continue;
                //if ((LCPsPSimulated[i].Tendency <= 50 && LCPsPSimulated[i].TendencyToday >= 50) ||  (LCPsPSimulated[i].Tendency >= 50 && LCPsPSimulated[i].TendencyToday <= 50)) continue;

                bool found = false;
                for (int i2 = 0; i2 < LDeals.Count; i++)
                {
                    try
                    {
                        if (LDeals[i2].PRODUCT == LCPsPSimulated[i].Product)
                        {
                            found = true;
                            break;
                        }
                    }
                    catch
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    CPsPBest = LCPsPSimulated[i];
                    break;
                }
            }

            if (CPsPBest == null) return;
            
            string product = CPsPBest.Product;
            DateTime DTLastGMT = CPsPBest.DTOriginal.AddMinutes(ABBREVIATIONS.ToMinutes(TimeFrame.ONE_MINUTE));
            List<Rates> LRAll = ORBlotter.Archive.Get(product, DTLastGMT);

           // double dExecutionDataCount = (double)(ABBREVIATIONS.GreenwichMeanTime - DTLastGMT).TotalSeconds / 5;

            if (LRAll == null || LRAll.Count < 7)//dExecutionDataCount)
            {
                if (LRAll != null) TsslInfo2.Text = "Missing Execution Data ";// +LRAll.Count + " / " + (int)dExecutionDataCount;
                return;
            }



            //double dMin = 0, dMax = 0;  if (!this.Extrema(product, 60, ref dMin, ref dMax)) return;
            //List<ChartPoint> LCPAll = ARCHIVE.GetDATA(TFrame, product, ABBREVIATIONS.GreenwichMeanTime.Date);
            //double dTendencyGlobal = ANALYSIS.Tendency(LCPAll, -1, false);
            //double dTendencyLoacl = ANALYSIS.Tendency(LCPAll, 15, true);

            bool bBUY = false;
            if(CPsPBest.Type == ChartPointsPredition.Kind.Up)
                bBUY = true;

            int iLRCount = LRAll.Count;

            bool bMatch = true;  
            double dTendencyNow, dTendencyPrev;


            if (bBUY)
                dTendencyPrev = 0;
            else dTendencyPrev = 100;

            int iOFFTendencies = 0; int iOKTendencies = 0;
            int iSteps = 2;

            for (int i = iLRCount - 1; i > 2 && iSteps-- > 0; i /= 2)
            {
                dTendencyNow = ANALYSIS.Tendency(LRAll, i);

                if (bBUY && dTendencyNow < dTendencyPrev)
                {
                    bMatch = false;
                    break;
                }
                else if (!bBUY && dTendencyNow > dTendencyPrev)
                {
                    bMatch = false;
                    break;
                }

                dTendencyPrev = dTendencyNow;
                if ((dTendencyPrev >= 50 && bBUY) || (dTendencyPrev <= 50 && !bBUY)) ++iOFFTendencies;
                else ++iOKTendencies;
            }

            double dOPRatio = ((double)iOFFTendencies / (iOFFTendencies + iOKTendencies)) * 100;

            //if (dOPRatio >= 40) return;

            if (!bMatch) return;// && dOPRatio > 40



            this.Action(product, bBUY);



            Thread ThrdBeep = new Thread(delegate() { Console.Beep(250, 100); }); ThrdBeep.Start();
        }

        
        

        
    }
}


/*
private void TimrAutoTraderSquare_Tick(object sender, EventArgs e)
        {



            if (ODBlotter.COUNT == 0) return;
            else
            {
                TimeFrame TFrame = TimeFrame.ONE_MINUTE;
                List<Deals> LDeals = ODBlotter.GetData.ToList();

                for (int i = 0; i < LDeals.Count; i++)
                {
                    Deals DEAL = LDeals[i];
                    string product = DEAL.PRODUCT;
                    List<ChartPointsPredition> LCPsPSimulated = this.LCPsPTestSelected.OrderByDescending(CPsP => CPsP.Resolution).ToList();
                    ChartPointsPredition CPsPrediction = null;
                    List<Rates> LRAll = ORBlotter.Archive.TryGet(product);

                    for(int i2 = 0; i2 < LCPsPSimulated.Count; i2++)
                        if (LCPsPSimulated[i2].Product == product)
                        {
                            CPsPrediction = LCPsPSimulated[i2];
                            break;
                        }

                    double dMinDeal = 0, dMaxDeal = 0;
                    if (!this.Extrema(DEAL, 10, ref dMinDeal, ref dMaxDeal))
                        return;

                    bool bBUY = DEAL.BUY;
                    Rates RATE = ORBlotter.Get(DEAL.PRODUCT);
                    double onePip = Math.Pow(10, -RATE.Decimals);
                    int iSpread = ANALYSIS.SpreadPips(DEAL, RATE.Decimals, true) + 2;
                    double dLevel = (double)iSpread;
                    bool bLooseWarning = false;
                    double dGainLoosePercentage = 75;
                    double dStartSpread = (double)(DEAL.ASK - DEAL.BID) / onePip;

                    if (CPsPrediction != null && CPsPrediction.IsActual)
                    {

                        if ((bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Down) ||
                            (!bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Up))
                        {
                            dLevel /= 2;
                            dGainLoosePercentage = 85;
                        }
                        else if ((!bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Down) ||
                            (bBUY && CPsPrediction.Type == ChartPointsPredition.Kind.Up))
                        {
                            dLevel *= 2;
                            dGainLoosePercentage = 60;
                        }
                    }

                    if (bBUY)
                    {
                        
                        double dPLMax = (double)(dMaxDeal - DEAL.ASK) / onePip;
                        double dPLNow = (double)(RATE.BID - DEAL.ASK) / onePip;

                        

                        if (dPLNow < -dLevel)
                        {
                            //if (dPLMax > 0 && dPLNow < 0)
                                bLooseWarning = true;
                        }
                        else if (dPLMax > dLevel)//dPLNow > dLevel)
                        {
                            double dGainPercentage = ((double)(dPLNow + dStartSpread) / (dPLMax + dStartSpread)) * 100;

                            if (dGainPercentage < dGainLoosePercentage)
                                bLooseWarning = true;
                        }
                    }
                    else//sell for bid, buy for ask
                    {
                        double dPLMin = (double)(DEAL.BID - dMinDeal) / onePip;
                        double dPLNow = (double)(DEAL.BID - RATE.ASK) / onePip;


                        if (dPLNow < -dLevel)
                        {
                            //if (dPLMin > 0 && dPLNow < 0)
                                bLooseWarning = true;
                        }
                        else if (dPLMin > dLevel)//(dPLNow > dLevel)
                        {
                            double dGainPercentage = ((double)(dPLNow + dStartSpread) / (dPLMin + dStartSpread)) * 100;

                            if (dGainPercentage < dGainLoosePercentage)
                                bLooseWarning = true;

                        }
                    }

                    if (bLooseWarning)
                    {
                        this.CloseDeal(DEAL);
                        Thread ThrdBeep = new Thread(delegate() { Console.Beep(200, 50); Console.Beep(200, 50); }); ThrdBeep.Start();
                    }
                    return;



                }



            }
        }
*/