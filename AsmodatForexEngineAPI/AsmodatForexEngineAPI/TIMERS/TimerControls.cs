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
        private void TimrControls_Tick(object sender, EventArgs e)
        {
            int InLbxPairs = LbxCCYPairs.Items.Count;
            int InLbxDeals = LbxDealsOpened.Items.Count;

            List<Rates> LRATES = ORBlotter.GetData;
            List<Deals> LDEALS = ODBlotter.GetData;
            int LRCount = LRATES.Count;
            

            if (InLbxPairs < LRCount)
            {
                if (InLbxPairs > 0)
                    LbxCCYPairs.Items.Clear();


                for (int i = 0; i < LRCount; i++)
                    LbxCCYPairs.Items.Add(LRATES[i].CCY_Pair);

            }

            if (LbxDealsOpened.SelectedIndex >= 0)
            {
                Deals DEAL = ODBlotter.Get(LbxDealsOpened.SelectedItem.ToString());

                TbxDealPair.Text = DEAL.PRODUCT;

                if (DEAL.BUY)
                    TbxDealKind.Text = "BUY ";
                else TbxDealKind.Text = "SELL";

                TbxDealProfitOrLoss.Text = AUTOTREADER.ConvertToUSD(DEAL.PRODUCT, DEAL.BUY, ACCOUNT.CalculateProfitOrLoss(DEAL, ORBlotter.Get(DEAL.PRODUCT))) + "";

                ChartPointPredition CPP = AUTOTREADER.TryGet(DEAL.PRODUCT, TimeFrame.ONE_MINUTE);


                if (CPP == null || CPP.Type == ChartPointPredition.Kind.Uncertain || !CPP.IsActual)
                {
                    TbxDealPredictionKind.Text = "Uncertain";
                    TbxDealPredictionPercentage.Text = "";
                    TbxDealPredictionValue.Text = "";
                }
                else
                {

                    if (CPP.Type == ChartPointPredition.Kind.Average)
                        TbxDealPredictionKind.Text = "Average";
                    else if (CPP.Type == ChartPointPredition.Kind.Down)
                        TbxDealPredictionKind.Text = "Down";
                    else if (CPP.Type == ChartPointPredition.Kind.Up)
                        TbxDealPredictionKind.Text = "Up";

                    TbxDealPredictionPercentage.Text = Math.Round(CPP.Propability, 2).ToString();
                    TbxDealPredictionValue.Text = Math.Round(CPP.Change, 5).ToString();
                }

            }





            if (this.IsConnected())
            {
                this.UpdateMargin();
                this.UpdateAction();
            }

            if (!OSBlotter.IsLoaded)
            {
                BtnAutoTrader.Enabled = false;
                BtnBuy.Enabled = false;
                BtnClose.Enabled = false;
                BtnSell.Enabled = false;
            }
            else
            {
                BtnAutoTrader.Enabled = true;
                BtnBuy.Enabled = true;
                BtnClose.Enabled = true;
                BtnSell.Enabled = true;
            }
            

        }
        

    }
}


/*
  List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TimeFrame.ONE_MINUTE, product);
                   // List<double> LDSet = new List<double>(from CP in LCPoints select CP.Activity);
                    // ANALYSIS.DystrybutionRanges(LDSet, 90);

                    Rates RATE = ORBlotter.Get(product);

                    double dSpread = RATE.Spread;
                    double pipValue = Math.Pow(10, -RATE.Decimals);
                    dSpread += ACCOUNT.OpeningHazard * pipValue;
                    dSpread += ACCOUNT.SqueringHazard * pipValue;
                    dSpread += pipValue * 10; //MinimumEarning

                    List<ChartPoint> LCPUsable = new List<ChartPoint>(ANALYSIS.AboveMinimum(LCPoints, dSpread));

                    double dAP = ANALYSIS.AverageActivityTime(LCPoints);
                    double dSDP = ANALYSIS.SampleStandardDeviationActivityTime(LCPoints);
                    double dPP = ANALYSIS.PercentageActivityTime(LCPoints, dAP, dSDP);

                    double dAP2 = ANALYSIS.AverageActivityTime(LCPUsable);
                    double dSDP2 = ANALYSIS.SampleStandardDeviationActivityTime(LCPUsable);
                    double dPP2 = ANALYSIS.PercentageActivityTime(LCPUsable, dAP2, dSDP2);
 */

/*
double dT = ANALYSIS.TopPeak(LCPoints);
double dB = ANALYSIS.BottomBase(LCPoints);

double dAP = ANALYSIS.AveragePeak(LCPoints);
double dSDP = ANALYSIS.SampleStandardDeviationPeak(LCPoints);
double dSEP = ANALYSIS.StandardErrorPeak(LCPoints);

double dAB = ANALYSIS.AverageBottom(LCPoints);
double dSDB = ANALYSIS.SampleStandardDeviationBase(LCPoints);
double dSEB = ANALYSIS.StandardErrorBase(LCPoints);

double dPP = ANALYSIS.PercentagePeak(LCPoints, dAP, dSDP);
double dPB = ANALYSIS.PercentageBase(LCPoints, dAB, dSDB);
*/