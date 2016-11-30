using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

using Asmodat.Abbreviate;


namespace AsmodatForex
{
    /// <summary>
    /// This class is based upon com.efxnow.demoweb.authentyficationservice.AuthenticationService, that
    /// Authenticates the User Credentials and returns an object containg the security token and Service url
    /// https://demoweb.efxnow.com/gaincapitalwebservices/authenticate/authenticationservice.asmx
    /// </summary>
    public partial class ServiceAuthentication : AbstractServices
    {
        
 
        /// <summary>
        /// This constructor creates instance of managed connection with forex server for specified user defined inside ForexService class
        /// </summary>
        /// <param name="ForexService"></param>
        public ServiceAuthentication(ref ForexService ForexService) : base(ref ForexService) 
        {
            this.Disconnections = -1;

            Timers.Run(() => KeepAliveTimer(), 500, null, true, true);
            Timers.Run(() => ReconnectTimer(), 500, null, true, true);
            Timers.Run(() => DataTimer(), 10, null, true, true);
        }

        
        com.efxnow.demoweb.authentyficationservice.AuthenticationService CEDA_AuthenticationService = new com.efxnow.demoweb.authentyficationservice.AuthenticationService();
        com.efxnow.demoweb.authentyficationservice.AuthenticationResult AuthenticationResult;

        private ThreadedMethod ThreadConnection = new ThreadedMethod(10, ThreadPriority.Normal);

 

        #region Socket
        private Socket ServerSocket = null;
        private byte[] BUFFER_IN = new byte[1 * 1024 * 1024];
        private byte[] BUFFER_OUT = new byte[1 * 1024 * 1024];
        private int DATA_OUT = 0;
        #endregion

        

        /// <summary>
        /// This method closes connetion with server and stops ServerSocket
        /// </summary>
        public void Dispose()
        {
            

            this.Stop(); //TTimers.TerminateAll();

            try
            {
                CEDA_AuthenticationService.Abort();
            }
            catch
            {

            }

        }


        ///// <summary>
        ///// This property specifiesthime that keep-allive message sould be send to server
        ///// </summary>
        //public int KeepAliveInterval { get; private set; }

        ///// <summary>
        ///// This property specifies time that Reconnection schould occur if it is neaded
        ///// </summary>
        //public int ReconnectInterval { get; private set; }



        //public DateTime DataReceived = DateTime.MinValue;

    }
}


//private static volatile Connect instance;
//private static object syncRoot = new Object();

//public static Connect Instance
//{
//    get
//    {
//        if (instance == null)
//        {
//            lock (syncRoot)
//            {
//                if (instance == null)
//                    instance = new Connect(UserCredentials.);
//            }
//        }

//        return instance;
//    }
//}