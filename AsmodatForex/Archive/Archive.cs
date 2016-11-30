using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;

namespace AsmodatForex
{

    /// <summary>
    /// Stores and manages historical and current currency rates data
    /// </summary>
    public partial class Archive
    {

        


        private DateTime _LastUpdate = DateTime.MinValue;
        public DateTime LastUpdate
        {
            get
            {
                return _LastUpdate;
            }
            private set
            {
                _LastUpdate = value;
            }
        }

        public void Update(ServiceConfiguration.TimeFrame TimeFrame, List<Rate> Rates)
        {
            if (Rates == null || Rates.Count <= 0) 
                return;

            string pair;
            foreach(Rate Rate in Rates)
            {
                pair = Rate.Pair;
                if (!Data.ContainsKey(pair)) Data.Add(pair, new ThreadedDictionary<ServiceConfiguration.TimeFrame, ThreadedDictionary<DateTime, Rate>>());


                if (!Data[pair].ContainsKey(TimeFrame)) 
                    Data[pair].Add(TimeFrame, new ThreadedDictionary<DateTime, Rate>());

                Data[pair][TimeFrame].Add(Rate.DateTime, Rate);

                

            }

            LastUpdate = DateTime.Now;
        }

        public int Records
        {
            get
            {
                int sum = 0;

               // lock (Data)
               // {
                foreach (var v1 in Data.ValuesArray)
                    foreach (var v2 in v1.ValuesArray)
                        sum += v2.Count;
                //}
                
                return sum;
            }
        }

    }
}
