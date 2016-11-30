using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Asmodat.Abbreviate;
using AsmodatForex;
using System.Windows.Forms.DataVisualization.Charting;

using Asmodat.FormsControls;

namespace AsmodatForexDataManager.UserControls
{
    public partial class ChartControl : UserControl
    {
        public ChartControl()
        {
            InitializeComponent();
            
        }


        public ThreadedChart Main
        {
            get
            {
                return ChartMain;
            }
        }

        private RatesIndicator RateIndicator = null;
        private Manager Manager = null;
        private ThreadedTimers Timers = new ThreadedTimers();

        public void Feed(ref Manager Manager, ref RatesIndicator RateIndicator)
        {
            this.Manager = Manager;
            this.RateIndicator = RateIndicator;
            this.ChartMain.Start();
            Timers.Run(() => PeacemakerUpdate(), 250, null, true, true, true);
        }


        private Rate RateOld = null;
        public void PeacemakerUpdate()
        {
            if (Manager == null) return;

            Rate rate = this.Rate;

            if (rate == null) return;// || rate.Frame == ServiceConfiguration.TimeFrame.LIVE) return;

            ChartPoint[] ACPints;
            DataPoint[] ADPoints;
            ChartPointInfo.DataPointType CPIDType = ChartPointInfo.DataPointType.Candle;
            SeriesChartType SCType = SeriesChartType.Candlestick;
            if (rate.Frame == ServiceConfiguration.TimeFrame.LIVE)
            {
                CPIDType = ChartPointInfo.DataPointType.AskLine;
                SCType = SeriesChartType.Line;
            }

            

            if (RateInfo.EqualsOrigin(rate, RateOld))
            {
                int count = ChartMain.CountPoints(rate.Pair, this);
                ACPints = Manager.ForexArchive.GetChartPoints(rate, count);

                if (ACPints.Length <= 0) return;

                ADPoints = ChartPointInfo.ToDataPoints(ACPints, CPIDType);
            }
            else
            {
                RateOld = rate;

                ChartMain.ClearSeries(this);
                ChartMain.Add(rate.Pair, SCType, this);

                if (SCType == SeriesChartType.Candlestick)
                    ChartMain.SetCandlestickSettings(this, rate.Pair, Color.Green, Color.Red, Color.Black, Color.Transparent);
                
                

                ACPints = Manager.ForexArchive.GetChartPoints(rate);
                ADPoints = ChartPointInfo.ToDataPoints(ACPints, CPIDType);
            }

            ChartMain.AddRange(this, rate.Pair, ADPoints, ChartValueType.DateTime, ChartValueType.Double);

        }
    }
}
