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


using AsmodatForex.com.efxnow.demoweb.authentyficationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

namespace AsmodatForex
{
    public partial class ServiceAuthentication : IDisposable
    {


        /// <summary>
        /// This method starts thread that connects with server, by setting KeepConnected property and restarting Reconnect Timer instantly. 
        /// </summary>
        public void Start()
        {
            this.Stopped = false;
        }




        /// <summary>
        /// This method stops connection tisth server, shutdown's server socket ass well as prevent reconnection by setting Reconnect and KeepConnected properties to false.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (ServerSocket != null)
                {
                    ServerSocket.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(100);
                    ServerSocket.Close();
                    ServerSocket.Dispose();
                    ServerSocket = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// This method connects or reconnects with forex server server as well as initializes OnRead events in order to receive data from socket
        /// </summary>
        /// <returns>Returns true if connection was establised else returns false.</returns>
        private bool _Start()
        {
            //com.efxnow.api.GainCapitalAutoExTradingAPI g = new com.efxnow.api.GainCapitalAutoExTradingAPI();
            if (System.String.IsNullOrEmpty(UserCredentials.HostName) || UserCredentials.Ports.Count < 1) return false;

            Connecting = true;
            ++Disconnections; //Increase disconnections counter
            this.Stop(); //Stop before restart
            

            try
            {
                IPAddress[] AIPAdresses = Dns.GetHostEntry(UserCredentials.HostName).AddressList;
                IPAddress ADRESS = AIPAdresses[0];
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IPEPMain = new IPEndPoint(ADRESS, UserCredentials.Ports[0]);

                ServerSocket.Blocking = false;

                AsyncCallback ACallbackConnected = new AsyncCallback(OcConnect);
                ServerSocket.BeginConnect(IPEPMain, ACallbackConnected, ServerSocket);
            
           
                //var v = //AuthenticateCredentials(UserCredentials.UserID, UserCredentials.UserPassword);
               
                //Thread.Sleep(100);
                AuthenticationResult = CEDA_AuthenticationService.AuthenticateCredentialsWithBrandCode(UserCredentials.UserID, UserCredentials.UserPassword, UserCredentials.UserBrandcode);

                
                

                if (System.String.IsNullOrEmpty(this.Token))
                    goto exitFalse;
                //
                
                // SConfiguration = new ServiceConfiguration(Token);
                this.Send(this.Token + "\r");
                //System.Console.Beep(666,25);
                //ConnectionStart = DateTime.Now;
            }
            catch
            {
                goto exitFalse;
            }



            Connecting = false;

            return true;

            exitFalse:
            Connecting = false;
            return false;
        }

        public void OcConnect(IAsyncResult IAR)
        {
            Socket SOCCurrent = (Socket)IAR.AsyncState;

            if (SOCCurrent != null && SOCCurrent.Connected)
                this.StartReciving(SOCCurrent);
        }
        private void StartReciving(Socket SOC)
        {
            try
            {
                AsyncCallback ACallabck = new AsyncCallback(OnRecieved);
                SOC.BeginReceive(BUFFER_IN, 0, BUFFER_IN.Length, SocketFlags.None, ACallabck, SOC);
            }
            catch(Exception e)
            {
                this.LastException = "|StartReciving| " + e.Message;
                this.Stop();
            }
        }

        /// <summary>
        /// This method recives data from ServerSocket and stores it inside GetData property by appending it.
        /// If exeption occurs in this method, Reconnect property is set to true, and this can trigger reconnect timer event.
        /// </summary>
        /// <param name="IAR">Default call IAsyncResult parameter</param>
        public void OnRecieved(IAsyncResult IAR)
        {
            try
            {
                Socket SOCCurrent = (Socket)IAR.AsyncState;
                int iDataToRecieve = SOCCurrent.EndReceive(IAR);


                if (iDataToRecieve > 0) 
                {
                    string data = Encoding.ASCII.GetString(BUFFER_IN, 0, iDataToRecieve); //this.ProcessData();

                    if (!System.String.IsNullOrEmpty(data))
                        Data.Add(TickTime.Now, data);

                    this.StartReciving(SOCCurrent);
                }
            }
            catch(Exception e)
            {
                this.LastException = "|OnRecieved| " + e.Message;
                this.Stop();
            }


        }

        /// <summary>
        /// This method allows to send string data into server via ServerSocket Send property
        /// If exeption occurs in this method, Reconnect property is set to true, and this can trigger reconnect timer event.
        /// </summary>
        /// <param name="data">Data that schould be sent to server</param>
        /// <returns></returns>
        private bool Send(string data)
        {
            if (!this.Connected && !this.Connecting) return false;
            

            try
            {
                byte[] BArray = Encoding.ASCII.GetBytes(data);
                DATA_OUT = BArray.Length;


                if (DATA_OUT <= BUFFER_OUT.Length)
                {
                    for (int i = 0; i < DATA_OUT; i++)
                        BUFFER_OUT[i] = BArray[i];

                    ServerSocket.Send(BUFFER_OUT, 0, DATA_OUT, SocketFlags.None);

                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                this.LastException = "|Send| " + e.Message;
                this.Stop();
                return false;
            }
        }

        /// <summary>
        /// This property indicates wheter or not ServerSocket is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (!Connecting && ServerSocket != null && ServerSocket.Connected) return true;
                else return false;
            }
        }

        public bool _Connecting = false;
        /// <summary>
        /// this property is true if program is trying to establist connection with the server
        /// </summary>
        public bool Connecting { get { return _Connecting; } set { _Connecting = value; } }


        private bool _Stopped = true;
        /// <summary>
        /// This property indicates wheter or not ServerSocket is or will be disconnected
        /// </summary>
        public bool Stopped { get  { return _Stopped; } 
            set 
            {
                _Stopped = value; 

                if(_Stopped)
                    this.Stop();
            } 
        }
       

    }
}
