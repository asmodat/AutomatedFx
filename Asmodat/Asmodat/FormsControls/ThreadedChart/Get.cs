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

        public SeriesCollection SeriesCollection
        {
            get
            { return this.ChartMain.Series;
            }
        }

        public ChartAreaCollection ChartAreaCollection
        {
            get
            {
                return this.ChartMain.ChartAreas;
            }
        }

        /// <summary>
        /// This method counts noumber of point in chart series
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="Invoker"></param>
        /// <returns></returns>
        public int CountPoints<TInvoker>(string name, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<int, TInvoker>(Invoker, () => CountPoints<TInvoker>(name, null));

            return ChartMain.Series[name].Points.Count;
        }


        public Series GetSeries<TInvoker>(string name, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<Series, TInvoker>(Invoker, () => GetSeries<TInvoker>(name, null));

      
            return ChartMain.Series[name];
        }

        public ChartArea GetChartArea<TInvoker>(int index, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<ChartArea, TInvoker>(Invoker, () => GetChartArea<TInvoker>(index, null));

            return ChartMain.ChartAreas[index];
        }


        public Axis GetAreaAxisX<TInvoker>(int index, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<Axis, TInvoker>(Invoker, () => GetAreaAxisX<TInvoker>(index, null));

            return Objects.CloneJson(ChartMain.ChartAreas[index].AxisX, Newtonsoft.Json.ReferenceLoopHandling.Ignore);//ChartMain.ChartAreas[index].AxisX;
        }



        public ThreadedChart Clone<TInvoker>(TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<ThreadedChart, TInvoker>(Invoker, () => Clone<TInvoker>(null));


            ThreadedChart copy = Objects.CloneJson(this, Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //ThreadedChart copy = (ThreadedChart)this.MemberwiseClone();
            return copy;
        }
        

    }
}
