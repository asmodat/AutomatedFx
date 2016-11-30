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
using Asmodat;

namespace AsmodatForexDataManager.UserControls
{

    [TypeDescriptionProvider(typeof(AbstractDescriptionProvider<ChartFeedUserControl, UserControl>))]
    public abstract class ChartFeedUserControl : UserControl//abstract
    {


        public ChartControl Chart;
        public Manager Manager;
        public ThreadedTimers Timers = new ThreadedTimers();
        public ThreadedMethod Methods = new ThreadedMethod();

       // public ThreadedTimers Timers = new ThreadedTimers();

        //public void Feed(ref Manager Manager)
        //{
        //    this.Manager = Manager;
        //    Methods.Run(() => Init(), null);
        //}

        //public void Feed(ref ChartControl Chart)
        //{
        //    this.Chart = Chart;
        //    Methods.Run(() => Init(), null);
        //}


        public void Feed(ref Manager Manager, ref ChartControl Chart)
        {
            this.Manager = Manager;
            this.Chart = Chart;
            Methods.Run(() => Init(), null);
        }

        public abstract void Init();
        //{
        //    //throw new Exception("Init method must be overrided with new keyword !");
        //}


        public bool IsFeed
        {
            get
            {
                if (IsChartFeed && IsManagerFeed) return true;
                else return false;
            }
        }
        public bool IsChartFeed
        {
            get
            {
                if (Chart != null)
                    return true;
                else return false;
            }
        }
        public bool IsManagerFeed
        {
            get
            {
                if (Manager != null)
                    return true;
                else return false;
            }
        }


    }
}
