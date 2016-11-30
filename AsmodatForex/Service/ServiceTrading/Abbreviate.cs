using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

using Asmodat.IO;

namespace AsmodatForex
{
    public partial class ServiceTrading
    {
        public static bool BlotterSuccess(BlotterOfDealResponse BODResponse)
        {
            if (BODResponse == null)
                return false;

            DealResponse[] output = BODResponse.Output;

            if (output == null || output.Length <= 0)
                return false;

            foreach (DealResponse DResponse in output)
                if (!DResponse.success)
                    return false;



            return true;
        }

    }
}
