using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;

using System.IO;


namespace AsmodatForex
{

    public partial class Archive
    {

        public bool RemoveAll()
        {
            if (Directory.Exists(DirectoryData))
            {
                Directory.Delete(DirectoryData, true);
                this.UpToDate = new Dictionary<string, DateTime>();
                return true;
            }
            else return false;
        }
    }
}
