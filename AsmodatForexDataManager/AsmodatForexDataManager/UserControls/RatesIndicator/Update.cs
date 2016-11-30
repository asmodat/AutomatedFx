using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForex;
using Asmodat.Abbreviate;

namespace AsmodatForexDataManager.UserControls
{

    public partial class RatesIndicator : ChartFeedUserControl {

        private bool UpdateRateControls()
        {
            Rate rate = this.Rate;

            if (rate == null || !Manager.ForexConfiguration.Loaded)
                return false;

            int iDecimals = Manager.ForexConfiguration.GetDecimals(rate.Pair);

            int iToken = Rate.Token;
            if (iToken <= 0)
                iToken = Manager.ForexConfiguration.GetToken(rate.Pair);

            this.Invoke((MethodInvoker)(() =>
            {
                TbxCONRACTPAIR.Text = Asmodat.Abbreviate.String.ToString(rate.CONTRACTPAIR, "???");
                TbxCOUNTERPAIR.Text = Asmodat.Abbreviate.String.ToString(rate.COUNTERPAIR, "???");
                TbxDateTime.Text = rate.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                TbxDECIMALS.Text = Asmodat.Abbreviate.Integer.ToString(iDecimals, "???", 0, int.MaxValue);
                TbxNOTATION.Text = Asmodat.Abbreviate.String.ToString(rate.NOTATION, "???");
                TbxSTATUS.Text = Asmodat.Abbreviate.String.ToString(rate.STATUS, "???");
                TbxToken.Text = Asmodat.Abbreviate.Integer.ToString(iToken, "???", 0, int.MaxValue);

                if (iDecimals >= 0)
                {
                    TbxHIGH.Text = Asmodat.Abbreviate.Doubles.ToString(rate.HIGH, "???", iDecimals, 0, double.MaxValue);
                    TbxLOW.Text = Asmodat.Abbreviate.Doubles.ToString(rate.LOW, "???", iDecimals, 0, double.MaxValue);
                    TbxOPEN.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OPEN, "???", iDecimals, 0, double.MaxValue);
                    TbxASK.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OFFER, "???", iDecimals, 0, double.MaxValue);
                    TbxBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.BID, "???", iDecimals, 0, double.MaxValue);
                    TbxCLOSE.Text = Asmodat.Abbreviate.Doubles.ToString(rate.CLOSE, "???", iDecimals, 0, double.MaxValue);
                    TbxCLOSINGBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.CLOSINGBID, "???", iDecimals, 0, double.MaxValue);
                }
                else
                {
                    TbxHIGH.Text = Asmodat.Abbreviate.Doubles.ToString(rate.HIGH, "???", 0, double.MaxValue);
                    TbxLOW.Text = Asmodat.Abbreviate.Doubles.ToString(rate.LOW, "???", 0, double.MaxValue);
                    TbxOPEN.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OPEN, "???", 0, double.MaxValue);
                    TbxASK.Text = Asmodat.Abbreviate.Doubles.ToString(rate.OFFER, "???", 0, double.MaxValue);
                    TbxBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.BID, "???", 0, double.MaxValue);
                    TbxCLOSE.Text = Asmodat.Abbreviate.Doubles.ToString(rate.CLOSE, "???", 0, double.MaxValue);
                    TbxCLOSINGBID.Text = Asmodat.Abbreviate.Doubles.ToString(rate.CLOSINGBID, "???", 0, double.MaxValue);
                }

            }));

            return true;
        }


        private void UpdateItemsPair()
        {
            if (Manager == null) return;

            string pair = CmbxPair.Text;
            string sframe = CmbxTimeFrame.Text;
            
            if (System.String.IsNullOrEmpty(sframe) || !Manager.IsLoadedRate) return;


            ServiceConfiguration.TimeFrame frame = (ServiceConfiguration.TimeFrame)Enum.Parse(typeof(ServiceConfiguration.TimeFrame), sframe);

            List<string> Pairs = new List<string>();

            if (frame == ServiceConfiguration.TimeFrame.LIVE)
                Pairs.AddRange(Manager.Service.Rates.Data.Keys);
            else
                Pairs.AddRange(Manager.Service.Archive.Data.Keys);

            CmbxPair.Items.Clear();
            CmbxPair.Items.AddRange(Pairs.ToArray());

            if (System.String.IsNullOrEmpty(pair) || !Pairs.Contains(pair))
                CmbxPair.SelectedIndex = 0;
            else CmbxPair.Text = pair;
        }

        private void UpdateItemsTimeFrame()
        {
            string sframe = CmbxTimeFrame.Text;
            string pair = CmbxPair.Text;
            if (System.String.IsNullOrEmpty(pair) || !Manager.IsLoadedRate) return;

            List<ServiceConfiguration.TimeFrame> Frames = new List<ServiceConfiguration.TimeFrame>();

            if (Manager.ForexArchive.Data.Keys.Contains(pair))
                Frames.AddRange(Manager.ForexArchive.Data[pair].Keys);

            if (!Frames.Contains(ServiceConfiguration.TimeFrame.LIVE) && Manager.Service.Rates.Data.ContainsKey(pair))
                Frames.Add(ServiceConfiguration.TimeFrame.LIVE);

            List<string> sFrames = new List<string>();

            foreach (ServiceConfiguration.TimeFrame frame in Frames)
                sFrames.Add(frame.ToString());

            CmbxTimeFrame.Items.Clear();
            CmbxTimeFrame.Items.AddRange(sFrames.ToArray());

            if (sFrames.Count > 0)
                if (System.String.IsNullOrEmpty(sframe) || !sFrames.Contains(sframe))
                    CmbxTimeFrame.SelectedIndex = 0;
                else CmbxTimeFrame.Text = sframe;
        }
    }
}
