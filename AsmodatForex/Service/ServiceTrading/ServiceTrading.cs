using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.IO;

namespace AsmodatForex
{
    /// <summary>
    /// This class that is based upon com.efxnow.demoweb.tradingservice.TradingService, that
    /// Provides interface to Forex trading System
    /// https://demoweb.efxnow.com/gaincapitalwebservices/trading/tradingservice.asmx
    /// </summary>
    public partial class ServiceTrading : AbstractServices
    {
        
        private com.efxnow.demoweb.tradingservice.TradingService CEDTS_TradingService = new com.efxnow.demoweb.tradingservice.TradingService();
        private ThreadedDictionary<string, Deal> DataDeals = new ThreadedDictionary<string, Deal>();

        


        public ServiceTrading(ref ForexService ForexService) : base(ref ForexService) 
        {
            DataDeals.IsValid = false;
            Threads.Run(() => this.InitRequestManager(), null, true, true);

            //UpdateProperties();
            Timers.Run(() => this.PeacemakerUpdate(), 100, null, true, true);
            Timers.Run(() => this.PeacemakerRequest(), 1, null, true, true);
            Timers.Run(() => this.PeacemakerAccount(), 1000, null, true, true);
        }




        private void PeacemakerUpdate()
        {
            this.GetDeals();
        }


        //public Settings Settings { get; private set; }

        public void GetDeals()
        {
            //Wait at least one minute to check
            if (!ForexAuthentication.Connected || (DataDeals.IsValid && DataDeals.UpdateSpan < 10 * 1000)) 
                return;

            Deal[] ADeals;
            //CEDTS_TradingService.get
            try
            {
                ADeals = CEDTS_TradingService.GetOpenDealBlotter(ForexAuthentication.Token).Output;
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
                return;
            }

            DataDeals.IsValid = false;

            lock (DataDeals)
            {
                DataDeals.Clear();

                foreach (Deal deal in ADeals)
                    if (deal != null)
                        DataDeals.Add(deal.ConfirmationNumber, deal);
                    
            }
           
            DataDeals.IsValid = true;

            if (DataDeals.Count == 0)
            {
                DataDeals.IsValid = true;
                return;
            }
        }

        
        


    }
}
