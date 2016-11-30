using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using Asmodat.Abbreviate;

namespace AsmodatForex
{
    public partial class Manager
    {
        public ThreadedBuffer<Exception> Exceptions
        {
            get
            {
                if (Service == null || Service.Forex == null) return null;
                return Service.Forex.Exceptions;
            }
        }


        
        public ServiceAuthentication ForexAuthentication
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.Authentication;
            }
        }
        public ServiceConfiguration ForexConfiguration
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.Configuration;
            }
        }
        public ServiceCharting ForexCharting
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.Charting;
            }
        }
        public ServiceRates ForexRates
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.Rates;
            }
        }
        public TreadingAPI ForexAPI
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.API;
            }
        }
        public ServiceTrading ForexTrading
        {
            get
            {
                if (Service == null) return null; else
                return Service.Trading;
            }
        }
        public Archive ForexArchive
        {
            get
            {
                if (Service == null) return null;
                else
                return Service.Archive;
            }
        }
        //public Credentials UserCredentials
        //{
        //    get
        //    {
        //        return Service.UserCredentials;
        //    }
        //}

        public Analysis ForexAnalysis
        {
            get
            {
                if (Service == null) return null;
                else
                    return Service.Analysis;
            }
        }

        public bool UserCreditalsUpdated { get; private set; }

        private Credentials _UserCredentials;

        public Credentials UserCredentials
        {
            get
            {
                return _UserCredentials;
            }
            set
            {
                _UserCredentials = value;
                UserCreditalsUpdated = true;
            }
        }

        public bool IsLoadedRate
        {
            get
            {
                if (this.Service == null || this.Service.Rates == null || !this.Service.Rates.Loaded) return false;
                else return true;
            }
        }

        public bool IsLoadedConfiguration
        {
            get
            {
                if (this.Service == null || this.Service.Configuration == null || !this.Service.Configuration.Loaded) return false;
                else return true;
            }
        }
         
    }
}
