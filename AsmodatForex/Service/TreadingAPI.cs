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
    /// This class is based upon com.efxnow.api.GainCapitalAutoExTradingAPI, that
    /// Core Web services offered by GAIN Capital. These services are offered to customers of GAIN Capital for the purposes of carrying out business with GAIN Capital. 
    /// All data supplied remains copyright GAIN Capital inc and may not be reproduced without the prior written permission of GAIN Capital inc. 
    /// USAGE: The web functions have been developed with Microsoft .NET WebServices. Best results can be found when used with Visual Studio.NET. For further information, see http://api.efxnow.com/Docs
    /// http://api.efxnow.com/DEMOWebServices2.8/Service.asmx
    /// </summary>
    public class TreadingAPI : AbstractServices
    {
        private com.efxnow.api.GainCapitalAutoExTradingAPI CEA_TradingAPI = new com.efxnow.api.GainCapitalAutoExTradingAPI();
       

        public TreadingAPI(ref ForexService ForexService)  : base(ref ForexService) 
        {
           //CEA_TradingAPI.get
        }

      
        



    }
}
