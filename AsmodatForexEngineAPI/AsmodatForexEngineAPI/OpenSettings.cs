using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;
using AsmodatForexEngineAPI.com.efxnow.demoweb.configurationservice;

using System.Collections.Concurrent;
using System.Threading;

namespace AsmodatForexEngineAPI
{
    public class OpenSettingsBlotter
    {
        private com.efxnow.demoweb.configurationservice.ConfigurationService CEDC_Configuration = new com.efxnow.demoweb.configurationservice.ConfigurationService();
        private ConcurrentDictionary<string, ProductSetting> DATA = new ConcurrentDictionary<string, ProductSetting>();
        private string TOKEN, USERID;

        public void Restart(string TOKEN)
        {
            this.TOKEN = TOKEN;
        }
        public OpenSettingsBlotter(string TOKEN, string USERID)
        {
            this.TOKEN = TOKEN;
            this.USERID = USERID;
        }

        public ProductSetting Get(string product)
        {
            if (DATA.ContainsKey(product))
                return DATA[product];
            else return null;
        }

        private void Update(string product)
        {
            ProductSetting PSettings = CEDC_Configuration.GetProductSetting(TOKEN, product).Output[0];

            if (DATA.ContainsKey(product))
                while (!DATA.TryUpdate(product, PSettings, DATA[product])) ;
            else
                while (!DATA.TryAdd(product, PSettings)) ;
        }

        private bool bIsLoaded = false;

        public bool IsLoaded
        {
            get
            {
                return bIsLoaded;
            }
        }

       List<Thread> LTread;
        public void Load(List<string> LSProducts)
        {
            Thread ThrdMain = new Thread(delegate()
                {
                    LTread = new List<Thread>();
                    foreach (string product in LSProducts)
                    {
                        LTread.Add(new Thread(delegate() { this.Update(product); }));
                        LTread.Last().Priority = ThreadPriority.Highest;
                        LTread.Last().Start();
                    }

                    foreach (Thread Thrd in LTread)
                        Thrd.Join();

                    bIsLoaded = true;
                });

            ThrdMain.Priority = ThreadPriority.Highest;
            ThrdMain.Start();
        }

    }
}
