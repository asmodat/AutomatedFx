using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.democharting.chartingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

namespace AsmodatForex
{
    public partial class ServiceRates
    {

        public string UpdateRate(string pair)
        {
            if (ForexAuthentication.Connected)
                try
                {
                    return CEDR_RatesService.getRate(ForexAuthentication.Token, pair);
                }
                catch { }

            return null;
        }

        public bool UpdateDataRates(Rate[] Rates)
        {
            if (Rates == null) return true;

            bool bSuccess = true;
            foreach (Rate rate in Rates)
                if (!this.UpdateDataRate(rate))
                    bSuccess = false;

            return bSuccess;
        }

        public bool UpdateDataRate(Rate rate)
        {
            string pair = rate.Pair;

            if (!Data.ContainsKey(pair))
            {
                Data.Add(pair, rate);
                return false;
            }
            else if (Data[pair].DateTime > rate.DateTime) //Update only new Rates
                return false;

            Dictionary<object, object> Properties = Objects.GetProperties(rate, true, true);
            Dictionary<string, object> PropertiesNew = new Dictionary<string, object>();
            bool bSuccess = true;
            foreach (KeyValuePair<object, object> KVP in Properties)
            {
                object oValue = KVP.Value;
                object oKey = KVP.Key;


                if (!(oKey is string))
                    continue;

                if (oValue == null)
                    continue;
                if (oValue is int || oValue is double)
                {
                    if (System.Convert.ToDouble(oValue) < 0)
                        continue;
                }
                else if (oValue is string)
                {
                    if (System.String.IsNullOrEmpty((string)oValue))
                        continue;
                }
                else if (oValue is DateTime)
                {
                    if (((DateTime)oValue) < Data[pair].DateTime)
                    {
                        bSuccess = false;
                        break;
                    }
                }

                PropertiesNew.Add((string)oKey, oValue);
            }

            if (!bSuccess)
                return false;

            Rate rateCurrent = Data[pair];
            if (Objects.SetProperties(ref rateCurrent, PropertiesNew, true))
                Data[pair] = rateCurrent;
            else
            {
                return false;
            }

            DateUpdateData = DateTime.Now;
            return true;
        }

        private void UpdateRates()
        {
            //ProductSetting PS = new ProductSetting();
            string packet = GetRateBlotter();
            TickTime origin = TickTime.Now;

            if (System.String.IsNullOrEmpty(packet)) 
                return;

            List<Rate> Rates = this.DecodeRates(packet, ref origin);
            this.UpdateDataRates(Rates.ToArray());
        }


    }
}
