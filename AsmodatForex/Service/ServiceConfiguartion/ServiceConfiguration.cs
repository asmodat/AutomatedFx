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
    /// This class is based upon com.efxnow.demoweb.configurationservice.ConfigurationService, that
    /// Provides configuration parameters for various features to be made available to the application
    /// https://demoweb.efxnow.com/GainCapitalWebServices/Configuration/ConfigurationService.asmx
    /// </summary>
    public partial class ServiceConfiguration
    {

        public ServiceConfiguration(ref ForexService ForexService) : base(ref ForexService) 
        {
            Timers.Run(() => UpdateProperties(), 1000, null, true, true);
        }

       

        

        /// <summary>
        /// This method updates setting properties
        /// </summary>
        public void UpdateProperties()
        {
            if (!ForexAuthentication.Connected || Loaded) 
                return;

            Settings = CEDC_ConfigurationService.GetAccountTradeSettings(ForexAuthentication.Token);
            BlotterOfTimeZones = CEDC_ConfigurationService.GetTimeZones(ForexAuthentication.Token, Languages.English);

            Products = new List<string>();
            ProductSettings = new Dictionary<string, ProductSetting>();
            OrderPair = new Dictionary<int,string>();

            ProductSetting[] ProductsArray = Settings.ProductSettings;
            foreach (ProductSetting PS in ProductsArray)
            {
                string product = PS.Product;
                if (!Products.Contains(product))
                {
                    Products.Add(product);
                    ProductSettings.Add(product, PS);
                    OrderPair.Add(int.Parse(PS.Order), product);
                }
            }

            TimeZones = new List<string>();
            TimeZoneSettings = new Dictionary<string, com.efxnow.demoweb.configurationservice.TimeZones>();
            TimeZones[] TimeZonesArray = BlotterOfTimeZones.Output.ToArray();
            foreach (TimeZones TZ in TimeZonesArray)
            {
                if (!TimeZones.Contains(TZ.TimeZoneStandardName))
                {
                    TimeZones.Add(TZ.TimeZoneStandardName);
                    TimeZoneSettings.Add(TZ.TimeZoneStandardName, TZ);
                }
            }

            Loaded = true;
        }


        public int GetDecimals(string pair)
        {
            if (!ProductSettings.ContainsKey(pair)) 
                return -1;
            string data = ProductSettings[pair].DecimalPlaces;

            //if (pair == "JPX/JPY" || pair == "UDX/USD") data = "3"; //correct JPX mistake

            if (System.String.IsNullOrEmpty(data)) 
                return -1;

            return int.Parse(data);
        }

        public int GetTolerance(string pair)
        {
            if (!ProductSettings.ContainsKey(pair))
                return -1;
            string data = ProductSettings[pair].Tolerance;

            if (System.String.IsNullOrEmpty(data))
                return -1;

            return int.Parse(data);
        }

        /// <summary>
        /// Searches and returns pair based on token otherwise null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetPair(int token)
        {
            if (!OrderPair.ContainsKey(token))
                return null;

            return this.OrderPair[token];
        }


        /// <summary>
        /// Searches and returns token based on oterwise -1
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public int GetToken(string pair)
        {
            if (!OrderPair.ContainsValue(pair))
                return -1;

            foreach (KeyValuePair<int,string> KVP in OrderPair)
                if (KVP.Value == pair)
                    return KVP.Key;
            

            return -1;
        }


    }
}

//public BlotterOfPlatformSettings BlotterOfPlatformSettings { get; private set; }
//public BlotterOfSubAccount BlotterOfSubAccount { get; private set; }
//public ConfigurationSettings ConfigurationSettings { get; private set; }
//BlotterOfPlatformSettings = CEDC_ConfigurationService.GetPlatformSettings(Token).Output[0];
//BlotterOfSubAccount = CEDC_ConfigurationService.GetSubAccounts(Token);
//ConfigurationSettings = CEDC_ConfigurationService.GetConfigurationSettings(Token);


/*
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
    /// This class is based upon com.efxnow.demoweb.configurationservice.ConfigurationService, that
    /// Provides configuration parameters for various features to be made available to the application
    /// https://demoweb.efxnow.com/GainCapitalWebServices/Configuration/ConfigurationService.asmx
    /// </summary>
    public class ServiceConfiguration
    {
        
        private com.efxnow.demoweb.configurationservice.ConfigurationService CEDC_ConfigurationService = new com.efxnow.demoweb.configurationservice.ConfigurationService();
        private ThreadedDictionary<string, ProductSetting> TDSProductSettings = new ThreadedDictionary<string, ProductSetting>();
        private ThreadedMethod TMProductSettings;


        public string Token 
        { 
            get
            {
                return ForexAuthentication.Token;
            }
        }

        

        private ServiceAuthentication ForexAuthentication { get; set; }
        public ServiceConfiguration(ref ServiceAuthentication ForexAuthentication)
        {
            this.ForexAuthentication = ForexAuthentication;
            TMProductSettings = new ThreadedMethod(512, System.Threading.ThreadPriority.Highest);

            UpdateProperties();
        }

        public bool LoadedProductsSettings
        {
            get
            {
                if (TDSProductSettings == null || TMProductSettings == null || TDSProductSettings.Count <= 0) return false;
                return TMProductSettings.AllTerminated();
            }
        }
        private void UpdateProductSettings(string product)
        {
            if (Token == null || Token.Length < 10) return; //Invalid Token
            ProductSetting PSettings = CEDC_ConfigurationService.GetProductSetting(Token, product).Output[0];
            TDSProductSettings.AddOrReplace(product, PSettings);
        }
        public bool LoadProductsSettings(List<string> Products, bool forceUpdate = false)
        {
            if (Products == null || Products.Count <= 0) return false;
            string[] SAProducts = Products.ToArray();

            foreach (string sProduct in SAProducts)
                if (!forceUpdate && TDSProductSettings.ContainsKey(sProduct)) continue;
                else TMProductSettings.Run(() => UpdateProductSettings(sProduct), "UpdateProductSettings" + sProduct, true, true);

            return true;
        }
        public void JoinLoadingProductsSettings()
        {
            TMProductSettings.JoinAll();
            TMProductSettings.TerminateAll();
        }


        public Settings Settings { get; private set; }
        public ConfigurationSettings ConfigurationSettings { get; private set; }
        public BlotterOfTimeZones BlotterOfTimeZones { get; private set; }

        //public BlotterOfPlatformSettings BlotterOfPlatformSettings { get; private set; }
        //public BlotterOfSubAccount BlotterOfSubAccount { get; private set; }

        public void UpdateProperties()
        {
            if (Token == null || Token.Length == 10) return;

            //BlotterOfPlatformSettings = CEDC_ConfigurationService.GetPlatformSettings(Token).Output[0];
            //BlotterOfSubAccount = CEDC_ConfigurationService.GetSubAccounts(Token);
            Settings = CEDC_ConfigurationService.GetAccountTradeSettings(Token);
            ConfigurationSettings = CEDC_ConfigurationService.GetConfigurationSettings(Token);
            BlotterOfTimeZones = CEDC_ConfigurationService.GetTimeZones(Token, Languages.English);
        }


        private List<string> _ContractProducts = new List<string>();
        public List<string> ContractProducts
        {
            get
            {
                int iProductsCount = Settings.ProductSettings.ToArray().Length;


                //if (_ContractProducts.Count <= 0 || Settings.ProductSettings.Count != _ContractProducts.Count)
                {
                    ProductSetting[] PSettings = Settings.ProductSettings.ToArray();
                    foreach(ProductSetting PS in PSettings)
                    {
                        if (!_ContractProducts.Contains(PS.ContractProduct))
                            _ContractProducts.Add(PS.ContractProduct);
                    }
                }

                return _ContractProducts;
            }
            set
            {
                _ContractProducts = value;
            }
        }

        public ThreadedDictionary<string, ProductSetting> ProductsSettings
        {
            get
            {


                return TDSProductSettings;
            }
        }

    }
}

*/