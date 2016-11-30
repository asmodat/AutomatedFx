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

namespace AsmodatForexDataManager.UserControls
{
    public partial class ChartControl : UserControl
    {
        /// <summary>
        /// This property returns currently feeded Pair
        /// </summary>
        public string Pair
        {
            get
            {
                if (RateIndicator == null) return null;

                return RateIndicator.Pair;
            }
        }

        /// <summary>
        /// This property currently feeded frame name
        /// </summary>
        public string Frame
        {
            get
            {
                if (RateIndicator == null) return null;

                return RateIndicator.Frame;
            }
        }

        /// <summary>
        /// This property currently feeded rate, this rate contains also information about feeded frame
        /// </summary>
        public Rate Rate
        {
            get
            {
                if (RateIndicator == null) return null;

                return RateIndicator.Rate;
            }
        }
    }
}
