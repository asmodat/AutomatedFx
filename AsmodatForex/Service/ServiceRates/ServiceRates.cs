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
    /// <summary>
    /// This class that is based upon com.efxnow.demoweb.ratesservice.RatesService, that
    /// Provides rates Data for all the products or specific product
    /// https://demoweb.efxnow.com/gaincapitalwebservices/rates/ratesservice.asmx
    /// It privides live rates from getRateBlotter and ServiceAuthentification Data
    /// </summary>
    public partial class ServiceRates
    {


        /// <summary>
        /// This property returns Rate selected from Gpbx, or null if its not loded yet or manager is not ready
        /// </summary>
        public Rate GetRate(string product)
        {

            if (!Data.ContainsKey(product)) 
                    return null;

            Rate rate = this.Data[product];
            rate.Frame = ServiceConfiguration.TimeFrame.LIVE;

            return rate;
        }

      


        //public Settings Settings { get; private set; }

        
        private string GetRateBlotter()
        {
            string packet = null;

            if (ForexAuthentication.Connected)
            try
            {
                packet = CEDR_RatesService.getRateBlotter(ForexAuthentication.Token);
            }
            catch
            {

            }

            return packet;
        }

    }
}


/*

          foreach(ProductSetting PSetting in  ForexConfiguration.ProductSettings.Values.ToArray())
          {
              var v = CEDR_RatesService.getRate(Token, PSetting.Product);
             Rate R = Rate.ToRate(v);
          }
*/