using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.configurationservice;

namespace AsmodatForexEngineAPI
{
    public class ProductsSettingsBlotter
    {
        com.efxnow.demoweb.configurationservice.ConfigurationService CEDC_Configuration = new com.efxnow.demoweb.configurationservice.ConfigurationService();

        public void Restart(string TOKEN)
        {
            this.TOKEN = TOKEN;
        }
        public ProductsSettingsBlotter(string TOKEN)
        {
            this.TOKEN = TOKEN;
        }

        private string TOKEN;

        Dictionary<string, BlotterOfProductSetting> DATA = new Dictionary<string, BlotterOfProductSetting>();
        public void Update(OpenRatesBlotter ORBlotter)
        {
            List<Rates> LRATES = ORBlotter.GetData;
            int count = LRATES.Count;

            DATA.Clear();

            for (int i = 0; i < count; i++)
                DATA.Add(LRATES[i].CCY_Pair, CEDC_Configuration.GetProductSetting(TOKEN, LRATES[i].CCY_Pair));
            
        }

        public double GetOrderSize(string CCY_Pair)
        {
           return double.Parse(DATA[CCY_Pair].Output[0].OrderSize);
        }


        public List<string> GetProducts
        {
            get
            {
                return DATA.Keys.ToList();
            }
        }


    }

   
}
