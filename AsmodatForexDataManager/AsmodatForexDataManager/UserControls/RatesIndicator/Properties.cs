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

    public partial class RatesIndicator : ChartFeedUserControl  {

        /// <summary>
        /// This property returns currently displayed Pair
        /// </summary>
        public string Pair
        {
            get
            {
                if (Manager == null || Manager.ForexArchive == null) return null;
                string pair = FormsControls.Invoke(this, () => CmbxPair.Text);

                if (Manager.ForexArchive.Data.ContainsKey(pair))
                    return pair;
                return null;
            }
        }

        /// <summary>
        /// This property currently displayed frame name
        /// </summary>
        public string Frame
        {
            get
            {
                string pair = this.Pair;

                if (System.String.IsNullOrEmpty(pair) || !Manager.IsLoadedConfiguration) return null;

                string sFrame = FormsControls.Invoke(this, () => CmbxTimeFrame.Text);

                if (System.String.IsNullOrEmpty(sFrame)) return null;

                ServiceConfiguration.TimeFrame frame = (ServiceConfiguration.TimeFrame)Enum.Parse(typeof(ServiceConfiguration.TimeFrame), sFrame);

                if (!Manager.ForexArchive.Data[pair].Keys.Contains(frame))
                    return null;

                return sFrame;
            }
        }

        public ServiceConfiguration.TimeFrame TimeFrame
        {
            get
            {
                string frame = this.Frame;
                return (ServiceConfiguration.TimeFrame)Enum.Parse(typeof(ServiceConfiguration.TimeFrame), frame);
            }
        }

        /// <summary>
        /// This property currently displayed rate, this rate contains also information about frame
        /// </summary>
        public Rate Rate
        {
            get
            {
                string pair = this.Pair;
                string frame = this.Frame;
               

                if (System.String.IsNullOrEmpty(pair) || System.String.IsNullOrEmpty(frame))
                    return null;

                Rate rate = null;
                DateTime date = this.DateTime;
                ServiceConfiguration.TimeFrame timeframe = this.TimeFrame;

                if (date != DateTime.MinValue && Manager.ForexArchive.Data[pair][timeframe].ContainsKey(date))
                    rate = Manager.ForexArchive.Data[pair][timeframe][date];
                else
                {
                    if (frame == ServiceConfiguration.TimeFrame.LIVE.ToString())
                        rate = Manager.ForexRates.GetRate(pair);
                    else
                        rate = Manager.ForexArchive.GetRate(pair, frame);
                }

                rate.Frame = timeframe;

                return rate;
            }
        }

        private DateTime _DateTime = DateTime.MinValue;
        public DateTime DateTime
        {
            get
            {
                return _DateTime;
            }
            private set
            {
                _DateTime = value;
            }
        }
        
    }
}
