using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

namespace AsmodatForex
{

    public partial class ServiceRates
    {

        

        private void UpdateRatesTimer()
        {
            if (ForexArchive == null || !ForexArchive.Loaded || !ForexAuthentication.Connected || !ForexConfiguration.Loaded) return;

            this.UpdateRates();
        }

    }
}
