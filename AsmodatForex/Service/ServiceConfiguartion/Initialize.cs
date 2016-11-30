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
 
    public partial class ServiceConfiguration : AbstractServices 
    {

        private com.efxnow.demoweb.configurationservice.ConfigurationService CEDC_ConfigurationService = new com.efxnow.demoweb.configurationservice.ConfigurationService();


       

        public Settings Settings { get; private set; }
        public BlotterOfTimeZones BlotterOfTimeZones { get; private set; }

        private List<string> _Products = new List<string>();
        /// <summary>
        /// This property contains CCY pairs
        /// </summary>
        public List<string> Products
        {
            get
            {
                return _Products;
            }
            private set
            {
                _Products = value;
            }
        }


        private Dictionary<string, ProductSetting> _ProductSettings = new Dictionary<string, ProductSetting>();
        /// <summary>
        /// This property contains products settings organized by Products - CCY pairs
        /// </summary>
        public Dictionary<string, ProductSetting> ProductSettings
        {
            get
            {
                return _ProductSettings;
            }
            private set
            {
                _ProductSettings = value;
            }
        }


        private List<string> _TimeZones = new List<string>();
        /// <summary>
        /// This property contains time zones standard names
        /// </summary>
        public List<string> TimeZones
        {
            get
            {
                return _TimeZones;
            }
            private set
            {
                _TimeZones = value;
            }
        }


        private Dictionary<string, TimeZones> _TimeZoneSettings = new Dictionary<string, TimeZones>();
        /// <summary>
        /// This property contains TimeZones settings organized by TimeZone standar names
        /// </summary>
        public Dictionary<string, TimeZones> TimeZoneSettings
        {
            get
            {
                return _TimeZoneSettings;
            }
            private set
            {
                _TimeZoneSettings = value;
            }
        }

        private Dictionary<int, string> _OrderPair = new Dictionary<int, string>();
        public Dictionary<int, string> OrderPair
        {
            get
            {
                return _OrderPair;
            }
            private set
            {
                _OrderPair = value;
            }
        }

        /// <summary>
        /// This field enables or disables settings update
        /// </summary>
        public bool Loaded = false;

    }
}
