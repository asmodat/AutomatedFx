using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Asmodat.Abbreviate;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        /// <summary>
        /// Clears all points and keeps series attributes
        /// </summary>
        public void ClearPoints<TInvoker>(TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
               // lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => ClearPoints<TInvoker>(null));
                UpdateTime.SetNow();
                return;
            }

            foreach (Series series in ChartMain.Series)
                series.Points.Clear();

            Reset();
        }

        /// <summary>
        /// Clears all series
        /// </summary>
        /// <param name="invoked"></param>
        public void ClearSeries<TInvoker>(TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => ClearSeries<TInvoker>(null));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.Series.Clear();
            Reset();
        }



        public void Reset()
        {
            ScaleX.ZoomReset();
            ScaleY.ZoomReset();
            MinX = null;
            MaxX = null;
        }

    }
}
