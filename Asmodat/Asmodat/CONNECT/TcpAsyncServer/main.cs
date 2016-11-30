using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using System.Collections.Concurrent;

using System.Timers;
using System.IO;

using System.Net;
using System.Net.Sockets;

using Asmodat.Abbreviate;

namespace Asmodat.Connect
{
    
    public partial class TcpAsyncServer : IDisposable
    {

        public void Dispose()
        {
            this.StopAll();
        }


        public void StopAll()
        {
            TimrMain.Enabled = false;

            foreach (string sKey in D2Sockets.Keys)
                Method.Run(() => this.Stop(sKey), "stop" + sKey, true, false);

            Method.Terminate(() => StartListening());
        }

        public TcpAsyncServer(int iPort)
        {
            this.Port = iPort;

            if (TimrMain == null)
            {
                TimrMain = new System.Timers.Timer();
                TimrMain.Elapsed += TimrMain_Elapsed;
                TimrMain.Enabled = false;
                TimrMain.Interval = 1;
            }
        }

        ThreadedMethod Method = new ThreadedMethod(100, ThreadPriority.Normal, 1);
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        

        
        public void Stop(string key)
        {
            StateObject state = D2Sockets.Get(key);
            Socket handler = state.workSocket;

            try
            {
                handler.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), state);
            }
            catch(SocketException se)
            {
                ExceptionBuffer.Add(se);
            }
        }

        private static void DisconnectCallback(IAsyncResult IAR)
        {
            StateObject state = (StateObject)IAR.AsyncState;
            Socket handler = state.workSocket;
            string key = state.key;

            

            handler.EndDisconnect(IAR);

            

            handler.Shutdown(SocketShutdown.Both);
            handler.Disconnect(true);
            handler.Close();

            handler.Dispose();

            
            


            D2Sockets.Remove(key);
            D2TReceive.Remove(key);
            D2TSend.Remove(key);
            D3BReceive.Remove(key);
            D3BSend.Remove(key);
        }
        //cm sis
        
        void TimrMain_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Method.Run(() => this.Accept(TcpAsyncCommon.DefaultUID + iAcceptCounter), "Accept", true, false);
            Method.Run(() => Communication(), "Communication", true, false);
        }


        private void Communication()
        {
            foreach (string sKey in D2Sockets.Keys)
            {
                this.ReceiveThread(sKey);
                this.SendThread(sKey);
            }
        }


        //public void ChangeKey(string sOldKey, string sNewKey)
        //{
        //    D2Sockets.ChangeKey(sOldKey, sNewKey);
        //    D2TReceive.ChangeKey(sOldKey, sNewKey);
        //    D2TSend.ChangeKey(sOldKey, sNewKey);
        //    D3BReceive.ChangeKey(sOldKey, sNewKey);
        //    D3BSend.ChangeKey(sOldKey, sNewKey);
        //}

       


    }
}


/*
Thread ThrdMain; //Main Timer Thread

if (ThrdMain != null && ThrdMain.IsAlive) return;

            ThrdMain = new Thread(() =>
                {


                    
                });

            ThrdMain.Start();

*/
