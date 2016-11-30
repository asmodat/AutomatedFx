using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;
using Asmodat.IO;

using Asmodat.Types;

namespace AsmodatForex
{

    public partial class Archive
    {
        public ThreadedMethod ThreadsLoad;
        public ThreadedMethod ThreadsSave;
        public ThreadedMethod ThreadsMain;


        public int Updates = 0;

        public Archive()
        {
            ThreadsLoad = new ThreadedMethod(25, System.Threading.ThreadPriority.Highest, 1);
            ThreadsMain = new ThreadedMethod(25, System.Threading.ThreadPriority.Highest, 1);
            ThreadsSave = new ThreadedMethod(25, System.Threading.ThreadPriority.Normal, 1);

            //FileDictionary<TickTime, DealRequest> IODictionary = new FileDictionary<TickTime, DealRequest>(@"C:\k.k", -10);
            //DateTime nnn = (DateTime)IODictionary.Data.First().Value.CreationTime; if (nnn == DateTime.Now) return;
            //StopWatch.Run("IO");
            //for (int i = 0; i < 1000; i++)
            //{
            //    DealRequest DR = new DealRequest();
            //    DR.ASK = 1;
            //    DR.BID = -1;
            //    DR.TFBuySell = true;
            //    DR.CreationTime = (TickTime)TickTime.Now;
            //    IODictionary.Add(DR.CreationTime, DR);


            //    //
            //}
            //double msss = StopWatch.ms("IO");
            //IODictionary.SaveData();
            //if (msss > 100000000)
            //    Asmodat.Foo.nop();

            ThreadsMain.Run(() => this.LoadAll(), null, true, true);
        }

        /// <summary>
        /// This field contains data of all cuttency pairs in all posible time frames configurations
        /// </summary>
        public ThreadedDictionary<string, ThreadedDictionary<ServiceConfiguration.TimeFrame, ThreadedDictionary<DateTime, Rate>>> Data = new ThreadedDictionary<string, ThreadedDictionary<ServiceConfiguration.TimeFrame, ThreadedDictionary<DateTime, Rate>>>();


        private bool _Loaded = false; //Loaded initialization
        /// <summary>
        /// This property defines wgeter or not Data was fully loaded from all availible sources
        /// </summary>
        public bool Loaded { get { return _Loaded; } private set { _Loaded = value; } }


        private bool _Loading = false; //Loading initialization
        /// <summary>
        /// This property indicates taht data is being loaded
        /// </summary>
        public bool Loading { get { return _Loading; } private set { _Loading = value; } }

        private DateTime _LoadTime = DateTime.MinValue;
        /// <summary>
        /// this property defines what is the last time data was saved from availible source, so saving does not have to repeat that
        /// </summary>
        public DateTime LoadTime { get { return _LoadTime; } private set { _LoadTime = value; } }


        //private DateTime _SavedUpToDate = DateTime.MinValue;
        //public DateTime SavedUpToDate { get { return _SavedUpToDate; } private set { _SavedUpToDate = value; } }


        public Dictionary<string, DateTime> UpToDate = new Dictionary<string, DateTime>();

        public void SetUpToDate(string pair, ServiceConfiguration.TimeFrame frame, DateTime date)
        {
            if (!UpToDate.ContainsKey(pair + frame)) UpToDate.Add(pair + frame, date);
            else UpToDate[pair + frame] = date;
        }

        public DateTime GetUpToDate(string pair, ServiceConfiguration.TimeFrame frame)
        {
            if (!UpToDate.ContainsKey(pair + frame)) 
                UpToDate.Add(pair + frame, DateTime.MinValue);

            return UpToDate[pair + frame];
        }


        private bool _Saving = false; //Loaded initialization
        /// <summary>
        /// This property defines wheter or not Archive is saving data
        /// </summary>
        public bool Saving { get { return _Saving; } private set { _Saving = value; } }


        private bool _Reapairing = false; //Repairing initialization
        /// <summary>
        /// This property defines wheter or not Archive is reapiring data
        /// </summary>
        public bool Repairing { get { return _Reapairing; } private set { _Reapairing = value; } }


        /// <summary>
        /// This property determines if no special action is being used on archive
        /// </summary>
        public bool Ready
        {
            get
            {
                if (Loaded && !Loading && !Saving && !Repairing)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// This object stores locks for multithread operations
        /// </summary>
        private ThreadedLocker Locker = new ThreadedLocker(10000);
    }
}
