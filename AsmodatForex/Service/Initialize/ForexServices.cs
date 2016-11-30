using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatForex
{
    public class ForexService
    {
        public ForexService(ref Credentials UserCredentials)
        {
            this.UserCredentials = UserCredentials;
            if(Archive == null) 
                Archive = new Archive();
        }

        public Credentials UserCredentials;
        public ServiceAuthentication Authentication;
        public ServiceConfiguration Configuration;
        public ServiceCharting Charting;
        public ServiceRates Rates;
        public TreadingAPI API;
        public Archive Archive;
        public ServiceTrading Trading;
        public Forex Forex;
        public Analysis Analysis;
    }
}
