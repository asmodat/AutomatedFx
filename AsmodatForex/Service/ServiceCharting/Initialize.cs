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

    public partial class ServiceCharting : AbstractServices
    {
        private com.efxnow.democharting.chartingservice.ChartingService CED_ChartingService = new com.efxnow.democharting.chartingservice.ChartingService();

        public ThreadedMethod ThreadsUpdate;
        public ThreadedMethod ThreadsSave;


        public ServiceCharting(ref ForexService ForexService) : base(ref ForexService) 
        {
            ThreadsUpdate = new ThreadedMethod(25, System.Threading.ThreadPriority.Lowest, 1);
            ThreadsSave = new ThreadedMethod(25, System.Threading.ThreadPriority.Lowest, 1);

            Timers.Run(() => this.UpdateTimer(), 1000, null, true, false);
        }

    }
}
