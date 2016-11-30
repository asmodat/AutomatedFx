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
        /// Adds new series
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="Invoker"></param>
        public void Add<TInvoker>(string name, SeriesChartType type, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => Add<TInvoker>(name, type, null));
                UpdateTime.SetNow();
                return;
            }

            Series series = new Series();
            series.Name = name;
            series.ChartType = type;
       
            //series.IsVisibleInLegend = false;

            //
            ChartMain.Series.Add(series);
            
        }

        /// <summary>
        /// adds new data point to series specified by name
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="point"></param>
        /// <param name="Invoker"></param>
        public void Add<TInvoker>(string name, DataPoint point, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => Add<TInvoker>(name, point, null));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.Series[name].Points.Add(point);
            
        }

        /// <summary>
        /// adds range of new data points to series specified by name
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="points"></param>
        /// <param name="Invoker"></param>
        public void AddRange<TInvoker>(TInvoker Invoker, string name, DataPoint[] points, ChartValueType? XValueType = null, ChartValueType? YValueType = null) where TInvoker : Control
        {

            if (points.Length <= 0)
                return;

            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => AddRange<TInvoker>(null, name, points, XValueType, YValueType));
                UpdateTime.SetNow();
                return;
            }



            double xMin = ScaleX.ViewMinimum;
            double xMax = ScaleX.ViewMaximum;


            foreach (DataPoint point in points)
            {
                ChartMain.Series[name].Points.Add(point);


                if (MinX == null || MinX.XValue > point.XValue)
                    MinX = point;
                if (MaxX == null || MaxX.XValue < point.XValue)
                    MaxX = point;
            }

            if (XValueType != null)
                ChartMain.Series[name].XValueType = (ChartValueType)XValueType;
            if (YValueType != null)
                ChartMain.Series[name].YValueType = (ChartValueType)YValueType;

            Area.AxisX.IsStartedFromZero = false;
            Area.AxisY.IsStartedFromZero = false;
            Area.RecalculateAxesScale();
            Area.RecalculateAxesScale();


            DataPoint last = ChartMain.Series[name].Points.Last();
            if (!Doubles.IsNaN(xMax, xMin) && last.XValue > xMax)
            {
                double change = (xMax - last.XValue);
                ScaleX.Zoom(xMin + change, last.XValue);
            }


            this.RescaleY();
        }

        public DataPoint MinX { get; private set; }
        public DataPoint MaxX { get; private set; }
    }
}
