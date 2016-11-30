using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

using Asmodat.IO;

namespace AsmodatForex
{
    public partial class ServiceTrading
    {

        public bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            private set
            {
                _IsBusy = value;
            }
        }


        public bool IsReady
        {
            get
            {
                if (ForexConfiguration.Loaded && ForexRates.Loaded && ForexAuthentication.Connected && ForexConfiguration.Loaded) return true;
                else return false;
            }
        }

        public ThreadedDictionary<string, Deal> Deals
        {
            get
            {
                lock (DataDeals)
                    return DataDeals;

            }
            private set
            {
                DataDeals = value;
            }
        }
    }
}
