using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using System.Threading;

namespace AsmodatForex
{

    public partial class ServiceRates : AbstractServices
    {
        private com.efxnow.demoweb.ratesservice.RatesService CEDR_RatesService = new com.efxnow.demoweb.ratesservice.RatesService();

        private ThreadedDictionary<string, Rate> _Data = new ThreadedDictionary<string, Rate>();

        public ThreadedDictionary<string, Rate> Data
        {   get
            {
                return _Data;
            }
            private set
            {
                _Data = value;
            }
        }


        public bool Loaded
        {
            get
            {
                if (DateUpdateData == DateTime.MinValue) 
                    return false;
                else return true; 
            }
        }


        private DateTime _DateUpdateData = DateTime.MinValue;
        public DateTime DateUpdateData { get { return _DateUpdateData; } private set { _DateUpdateData = value; } }




        public ServiceRates(ref ForexService ForexService) : base(ref ForexService) 
        {
            RateUpdateInterval = 1000;
            Timers.Run(() => UpdateRatesTimer(), RateUpdateInterval, null, true, true);
        }

        public int RateUpdateInterval { get; private set; }

    }
}
