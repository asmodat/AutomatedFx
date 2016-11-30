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
using System.Windows.Forms.DataVisualization.Charting;

namespace AsmodatForexDataManager.UserControls
{

    public partial class RatesIndicator 

    {

        private void PeacemakerChart()
        {
            if (!IsChartFeed) return;

            DataPoint DPoint = Chart.Main.Collision;
            if (DPoint == null) return;

            DateTime DateTime = DateTime.FromOADate(DPoint.XValue);

            if (DateTime != null && DateTime > DateTime.MinValue)
                this.DateTime = DateTime;
        }
    }
}
