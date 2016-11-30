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
    public partial class ServiceAuthentication
    {
        


        /// <summary>
        /// Keep-alive traffic 
        /// The server will send a keep-alive ‘carriage-return’ character on a regular basis if rate-data is not sent. 
        /// The client should send a return keep-alive character once a minute if it does not receive any data. 
        /// This is to ensure that the socket connection is valid without swamping the rates server with keep-alive messages. 
        /// Typically, the socket interface will throw a ‘reset by peer’ message if the socket fails to deliver the keep-alive. 
        /// Additionally, if you, the client, do not receive a message send a keep-alive to test the link.
        /// </summary>
        private void KeepAliveTimer()
        {
            if (Connected || Connecting || Stopped) 
                return;

            if (Data.UpdateTime == DateTime.MinValue || (DateTime.Now - Data.UpdateTime).TotalMilliseconds <= 5000) 
                return;


            this.Send("\r");
        }


        /// <summary>
        /// This method allos to reconnect with server if KeepConnected property is set.
        /// Connection can be restarted if socket is not already connected and force to 'Reconnect' property is not set.
        /// This method also increases Disconnections property counter
        /// </summary>
        private void ReconnectTimer()
        {
            if (this.Connected || this.Connecting || this.Stopped) 
                return;

            
            this._Start();
        }


        private void DataTimer()
        {
            if (Data.Count <= 0 || !ForexArchive.Ready || !ForexConfiguration.Loaded) 
                return;

            this.DecodeData();
        }
        
    }
}
