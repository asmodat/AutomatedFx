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

using Asmodat.Types;

namespace AsmodatForex
{
    public partial class ServiceAuthentication : AbstractServices
    {
        private int _ExceptionsCount = 0;
        private string _LastException = "";
        /// <summary>
        /// This property contains string instance of last ocurred error with date and number
        /// </summary>
        public string LastException { get { return _LastException; } private set { _LastException = "[ ServiceAuthentication Exception nr. " + ++_ExceptionsCount + " ] [ " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " ] \n" + value; } }

        ///// <summary>
        ///// This property contains data recived from ServerSocket
        ///// </summary>
        //public string Data { get; private set; }
        ////public string SetData { private get; set; }

        ThreadedDictionary<TickTime, string> Data = new ThreadedDictionary<TickTime, string>();


        public string Token
        {
            get
            {
                if (AuthenticationResult == null) return null;
                else return AuthenticationResult.token;
            }
        }

        /// <summary>
        /// This property is a disconnecions counter, that defines how many times user was disconnected from server
        /// </summary>
        public int Disconnections { get; private set; }
        
    }
}
