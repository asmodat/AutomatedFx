using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;


using System.Collections.Concurrent;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{
   

    public partial class Archive
    {
        com.efxnow.demoweb.authentyficationservice.AuthenticationService CEDA_Authentification = new com.efxnow.demoweb.authentyficationservice.AuthenticationService();
        TradingService CEDT_TreadingService = new TradingService();
        ChartingService CService = new ChartingService();

        AsmodatForexEngineAPI.Decoding AFEAPIDecoding = new Decoding();
        AsmodatForexEngineAPI.DataBase DATABASE;
        AsmodatForexEngineAPI.Abbreviations ABBREVIATIONS = new Abbreviations();
       
        private string TOKEN;

        static DateTime DTStart = new DateTime(2010, 1, 1, 0, 0, 0);
        
        List<string> LSProducts = new List<string>();

        public void Restart(string TOKEN)
        {
            this.TOKEN = TOKEN;
        }
        public Archive(string TOKEN, string USERID, List<string> LSProducts)
        {
            this.TOKEN = TOKEN;
            DATABASE = new DataBase(USERID);
            this.LSProducts = LSProducts;
        }

        public List<string> GetProducts()
        {
            return LSProducts;
            /*Position[] APositions = CEDT_TreadingService.GetPositionBlotter(TOKEN).Output;
            List<string> LSProducts = new List<string>();
            foreach (Position P in APositions)
                LSProducts.Add(P.Product);

            return LSProducts;*/
        }
    }


    
}
