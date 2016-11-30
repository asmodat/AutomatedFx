using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace AsmodatForex
{
    public abstract class AbstractServices
    {
        public ServiceAuthentication ForexAuthentication
        {
            get
            {
                return ForexService.Authentication;
            }
        }
        public ServiceConfiguration ForexConfiguration
        {
            get
            {
                return ForexService.Configuration;
            }
        }
        public ServiceCharting ForexCharting
        {
            get
            {
                return ForexService.Charting;
            }
        }
        public ServiceRates ForexRates
        {
            get
            {
                return ForexService.Rates;
            }
        }
        public TreadingAPI ForexAPI
        {
            get
            {
                return ForexService.API;
            }
        }
        public ServiceTrading ForexTrading
        {
            get
            {
                return ForexService.Trading;
            }
        }
        public Archive ForexArchive
        {
            get
            {
                return ForexService.Archive;
            }
        }
        public Credentials UserCredentials
        {
            get
            {
                return ForexService.UserCredentials;
            }
        }
        public Forex Forex
        {
            get
            {
                return Forex;
            }
        }


        public ForexService ForexService { get; set; }


        public ThreadedTimers Timers;
        public ThreadedMethod Threads;
        public ThreadedLocker Locks;
        public ThreadedBuffer<Exception> Exceptions;


        public AbstractServices(ref ForexService ForexService)
        {
            this.ForexService = ForexService;
            Timers = new ThreadedTimers(100);
            Threads = new ThreadedMethod(100);
            Locks = new ThreadedLocker(100);
            Exceptions = new ThreadedBuffer<Exception>(100);
        }

        public AbstractServices()
        {

        }


    
      

    }
}
