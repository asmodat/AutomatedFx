using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Security.Cryptography;
using System.Web;
using Asmodat.Types;
using AsmodatMath;
using Asmodat.Abbreviate;

using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Asmodat.Networking
{
    public class NetTester
    {
        public NetTester()
        {
            bNetworkAvailable = IsNetworkAvailable();
            System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            bNetworkAddresChanged = true;
        }

        void NetworkChange_NetworkAvailabilityChanged(object sender, System.Net.NetworkInformation.NetworkAvailabilityEventArgs e)
        {
            bNetworkAvailable = IsNetworkAvailable();
        }


        private bool bNetworkAvailable = false;
        private bool bNetworkAddresChanged = false;

        public static bool IsNetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        public bool NetworkAvailable { get { return bNetworkAvailable; } }
        public bool NetworkAddresChanged 
        { 
            get 
            { 
                if (bNetworkAddresChanged) 
                { 
                    bNetworkAddresChanged = false; 
                    return true; 
                }

                return false;
            } 
        
        }


        public static string GetRandomUrlSafeToken(int strength = 16)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            byte[] randomBytes = new byte[strength];
            random.NextBytes(randomBytes);
            string token = HttpServerUtility.UrlTokenEncode(randomBytes);
            return token;
        }


        /// <summary>
        /// TcpClient based ping test
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PingHost(string ip, int port)
        {
            try
            {
                TcpClient client = new TcpClient(ip, port);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*try
                {



                    WebClient WC = new System.Net.WebClient();

                    if (ip != ProxyManager.default_proxy_IP || port != ProxyManager.default_proxy_port) //change proxy if not default
                        WC.Proxy = new WebProxy(ip, port);

                  

                    WC.DownloadString("bc.vc");//http://google.com/ncr
                  
                    passed = true;
                }
                catch { }*/
        

    }
}
