using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

using System.Windows.Forms;

namespace AsmodatForexEngineAPI
{
    public partial class Form1
    {
        private Socket SOCMain = null;
        private byte[] BUFFER = new byte[1 * 1024 * 1024];
        private byte[] BUFFER_OUT = new byte[1 * 1024 * 1024];
        int DATA_OUT = 0;
        string DATA = "";
        bool Server_KEEPALIVE = false;

        private void StartConnection()
        {
            try
            {
                if (SOCMain != null && SOCMain.Connected)
                {
                    SOCMain.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(100);
                    SOCMain.Close();
                }

                IPAddress[] AIPAdresses = Dns.GetHostEntry(HOST).AddressList;
                var ADRESS = AIPAdresses[0];
                SOCMain = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IPEPMain = new IPEndPoint(ADRESS, PORT);

                SOCMain.Blocking = false;
                AsyncCallback ACallbackConnected = new AsyncCallback(OcConnect);
                //Thread.Sleep(1000);
                SOCMain.BeginConnect(IPEPMain, ACallbackConnected, SOCMain);

                var CEDAA_CreditalsWithBrandCode = CEDA_Authentification.AuthenticateCredentialsWithBrandCode(USERID, PASSWORD, BRANDCODE); //var CEDAA_Creditals = CEDA_Authentification.AuthenticateCredentials(USERID, PASSWORD); //LOGIN FOR TOKEN
                TOKEN = CEDAA_CreditalsWithBrandCode.token; // TOKEN  = CEDAA_Creditals.token;

                if (ORBlotter != null || PSBlotter != null || ARCHIVE != null || OSBlotter != null)
                {
                    ODBlotter.Restart(TOKEN);
                    PSBlotter.Restart(TOKEN);
                    ARCHIVE.Restart(TOKEN);
                    OSBlotter.Restart(TOKEN);
                    this.SendString(TOKEN + "\r");
                }

       

                
                //this.UpdateDeals();
                //this.UpdateAccount();

            }
            catch
            {
                try { TsslInfo.Text = "Restart Failed [" + ++iRestartFails + "]x "; }
                catch { }
                this.StartConnection();
            }
        }

        int iRestartFails = 0;

        public void OcConnect(IAsyncResult IAR)
        {
            Socket SOCCurrent = (Socket)IAR.AsyncState;

            if (SOCCurrent != null && SOCCurrent.Connected)
                this.StartReciving(SOCCurrent);
            //SOCCurrent.BeginReceive(BUFFER, 0, BUFFER.Length, 0, new AsyncCallback(OnBeginRecieve), SOCCurrent);
        }
        private void StartReciving(Socket SOC)
        {
            AsyncCallback ACallabck = new AsyncCallback(OnRecieved);
            SOC.BeginReceive(BUFFER, 0, BUFFER.Length, SocketFlags.None, ACallabck, SOC);
        }
        public void OnRecieved(IAsyncResult IAR)
        {
            try
            {
                Socket SOCCurrent = (Socket)IAR.AsyncState;


                int iDataToRecieve = SOCCurrent.EndReceive(IAR);


                if (iDataToRecieve > 0)
                {
                    //Thread Thrd = new Thread(delegate()
                    // {
                    DATA = Encoding.ASCII.GetString(BUFFER, 0, iDataToRecieve);

                    this.ProcessData();
                    //});
                    //Thrd.Start();

                    this.StartReciving(SOCCurrent);
                }
                else
                {

                }
            }
            catch
            {
                try { TsslInfo.Text = "Connection Broken !!!"; } catch {  MessageBox.Show("Connection Broken !!!, and couldn't display info on frame"); }

                this.StartConnection();   
            }


        }
        private int SendString(string input)
        {
            try
            {
                byte[] BArray = Encoding.ASCII.GetBytes(input);
                DATA_OUT = BArray.Length;


                if (DATA_OUT <= BUFFER_OUT.Length)
                {
                    for (int i = 0; i < DATA_OUT; i++)
                        BUFFER_OUT[i] = BArray[i];



                    SOCMain.Send(BUFFER_OUT, 0, DATA_OUT, SocketFlags.None);

                    return DATA_OUT;
                }
                else return -1;
            }
            catch
            {
                this.StartConnection();
                return -1;
            }
        }
        private bool IsConnected()
        {

            if (SOCMain != null && SOCMain.Connected) return true;

            return false;
        }

        private void TimrKAConnection_Tick(object sender, EventArgs e)
        {
            if (IsConnected() && Server_KEEPALIVE)
            {
                this.SendString("\r");
                Server_KEEPALIVE = false;
                // MessageBox.Show("[" + DATA.Length + "]" + DATA);
            }
        }

        
        private void ProcessData()
        {
            List<string> SLDecodedData = AFEAPIDecoding.DataRates(DATA);
            if (AFEAPIDecoding.Rates(SLDecodedData, ref ORBlotter))
            {
                Server_KEEPALIVE = true;
            }
        }
    }
}
