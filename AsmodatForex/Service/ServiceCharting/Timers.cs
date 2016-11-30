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

    public partial class ServiceCharting
    {
        private void UpdateTimer()
        {
            if (ForexConfiguration == null || !ForexConfiguration.Loaded) return;  //Chart Properites Avalaible
            if (ForexArchive == null || !ForexArchive.Loaded) return; //Chart data loaded from hard drive

            if (ForexArchive.Saving) 
                return;

            this.UpdateAll(false);
        }
    }
}
