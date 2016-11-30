using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatForex
{
    public class Credentials
    {
        public Credentials(string host, List<int> ports, string userID, string password, string brandcode)
        {
            this.HostName = host;
            this.Ports = ports;
            this.UserID = userID;
            this.UserPassword = password;
            this.UserBrandcode = brandcode;
            this.AppName = "AsmodatForexManager";
        }

        #region User Settings
        public string HostName { get; private set; }
        public List<int> Ports { get; private set; }
        public string AppName { get; private set; }
        public string UserID { get; private set; }
        public string UserPassword { get; private set; }
        public string UserBrandcode { get; private set; }
        #endregion

    }
}
