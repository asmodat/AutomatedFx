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


namespace AsmodatForexEngineAPI
{   

    public partial class Form1 : Form
    {
        Abbreviations ABBREVIATIONS = new Abbreviations();
        AutoTreader AUTOTREADER;
        Analysis ANALYSIS;
        Archive ARCHIVE = null;
        Account ACCOUNT;
        OpenSettingsBlotter OSBlotter = null;
        OpenDealsBlotter ODBlotter = null;
        OpenRatesBlotter ORBlotter = null;
        ProductsSettingsBlotter PSBlotter;
        AsmodatForexEngineAPI.Decoding AFEAPIDecoding = new Decoding();
        AsmodatForexEngineAPI.DataBase DATABASE;
        AsmodatForexEngineAPI.Simulation SIMULATION, SIMULATION1, SIMULATION2;//, SIMULATION4, SIMULATION5, SIMULATION6;

       com.efxnow.demoweb.authentyficationservice.Authenticator CEDA_Authentyficator = new com.efxnow.demoweb.authentyficationservice.Authenticator();
       com.efxnow.demoweb.authentyficationservice.AuthenticationService CEDA_Authentification = new com.efxnow.demoweb.authentyficationservice.AuthenticationService();
       com.efxnow.demoweb.configurationservice.Service CEDC_Service = new com.efxnow.demoweb.configurationservice.Service();
       //com.efxnow.demoweb.configurationservice.ConfigurationService CEDC_Configuration = new com.efxnow.demoweb.configurationservice.ConfigurationService();
       com.efxnow.demoweb.authentyficationservice.Connection CEDC_Connection = new com.efxnow.demoweb.authentyficationservice.Connection();
       com.efxnow.demoweb.tradingservice.TradingService CEDT_TreadingService = new com.efxnow.demoweb.tradingservice.TradingService();
       com.efxnow.api.GainCapitalAutoExTradingAPI GCAETAPI = new com.efxnow.api.GainCapitalAutoExTradingAPI();


       TradingService TS_TradingService = new TradingService();

       const string HOST = "demorates.efxnow.com";
       const int PORT = 443, PORT2 = 3020;
       const string APPNAME = "AsmodatForexEngineAPI";
       const string USERID = "apiasmodat@gmail.com";
       const string PASSWORD = "mateusz";
       const string BRANDCODE = "GCUK";//"FRXC";//
       string TOKEN = "";
        public Form1()
       {
           DATABASE = new DataBase(USERID);
           
           ACCOUNT = DATABASE.Load_Account(USERID);
           if (ACCOUNT == null) ACCOUNT = new Account();

           CEDA_Authentyficator.ApplicationName = APPNAME;
           CEDA_Authentyficator.IPAddress = "demorates.efxnow.com";// "87.205.102.39";
           CEDA_Authentyficator.Language = "English";
           CEDA_Authentyficator.MachineName = "EUROCOM";


           this.StartConnection();
           

            

            InitializeComponent();
            

            
            
            
            PSBlotter = new ProductsSettingsBlotter(TOKEN);
            ODBlotter = new OpenDealsBlotter(TOKEN, USERID);
            ORBlotter = new OpenRatesBlotter();


            this.SendString(TOKEN+"\r");
            this.UpdateDeals();
            this.UpdateAccount();


            

            TimrStartup.Enabled = true;
        }

       
        private void TimrStartup_Tick(object sender, EventArgs e)
        {
            if (ORBlotter.Count != 0 && OSBlotter == null)
            {
                List<string> LSProducts = ORBlotter.GetProducts;
                OSBlotter = new OpenSettingsBlotter(TOKEN, USERID);
                OSBlotter.Load(LSProducts);
                ARCHIVE = new Archive(TOKEN, USERID, LSProducts);
                ANALYSIS = new Analysis(ref ARCHIVE, ref ACCOUNT);
                AUTOTREADER = new AutoTreader(ref ARCHIVE, ref ORBlotter, ref ODBlotter, ref OSBlotter, ref ACCOUNT);
                SIMULATION = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);
                SIMULATION1 = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);
                SIMULATION2 = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);
               /* SIMULATION4 = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);
                SIMULATION5 = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);
                SIMULATION6 = new AsmodatForexEngineAPI.Simulation(ref ORBlotter, ref OSBlotter, ref ARCHIVE, ref ACCOUNT, ref DATABASE);*/
            }
            else if (OSBlotter != null && OSBlotter.IsLoaded)
            {
                TimrArchive.Interval = 1;
                TimrArchive.Enabled = true;
                TimrControls.Enabled = true;
                //TimrSimulation.Enabled = true;
                //TimrSimulationTest.Enabled = true;//TODO: DISABLE

                TimrStartup.Enabled = false;
                TsslInfo2.Text = "Thank you, for your patience.";
            }
        }



        

        

        private void BtnBuy_Click(object sender, EventArgs e)
        {
            if (LbxCCYPairs.SelectedItem == null) return;
            this.Action(LbxCCYPairs.SelectedItem.ToString(), true);
        }
        private void BtnSell_Click(object sender, EventArgs e)
        {
            if (LbxCCYPairs.SelectedItem == null) return;
            this.Action(LbxCCYPairs.SelectedItem.ToString(), false);
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (LbxDealsOpened.SelectedItem == null) return;
            this.CloseDeal(ODBlotter.Get(LbxDealsOpened.SelectedItem.ToString()));//  DLDDeals[long.Parse(LbxDealsOpened.SelectedItem.ToString())]);
        }

        
        
        private bool Action(string PRODUCT, bool BUY)
        {
            if (!OSBlotter.IsLoaded) return false;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var SETTINGS = OSBlotter.Get(PRODUCT); // CEDC_Configuration.GetProductSetting(TOKEN, PRODUCT); //Product settings - minimum lot etc //var b5 = CEDT_TreadingService.GetPositionBlotter(TOKEN); //Summary position for all currencies - all positions
            int OrderSize = int.Parse(SETTINGS.OrderSize);

            if (PRODUCT == "XAU/USD")
                OrderSize *= 10;

            DealResponse INFO;

           // double cost = this.ToUSD(PRODUCT, BUY, OrderSize);

            string Rate;
            string BuySell;
            if (BUY) BuySell = "B";
            else BuySell = "S";

            bool bDealSucced = true;
            int timeout;
            Rates RATE = ORBlotter.Get(PRODUCT);
            do
            {
                timeout = 0;
                if (BUY) Rate = ABBREVIATIONS.ToString(RATE.ASK,RATE.Decimals);// BID.ToString();
                else Rate = ABBREVIATIONS.ToString(RATE.BID,RATE.Decimals);//ASK.ToString();

                INFO = CEDT_TreadingService.InstantExecution(TOKEN, PRODUCT, BuySell, OrderSize.ToString(), Rate, ACCOUNT.OpeningHazard); // var INFO = CEDT_TreadingService.DealRequest(TOKEN, PRODUCT, BuySell, OrderSize.ToString(), Rate); //quick // CEDT_TreadingService.DealRequestAtBest(TOKEN, PRODUCT, BuySell, "100000"); //execute whatever but slow
                
                if(INFO.ErrorNo != "")
                {
                    timeout = RATE.WaitForUpdate(2000);
                    if (timeout < 0) { 
                        TsslInfo.Text = "Rate does not changed in 2s, Timeout Error !"; bDealSucced = false; break; }
                    else { 
                        TsslInfo.Text = "Time needed for rate to update: " + timeout + " [ms]"; }
                }

            } while (timeout != 0);


            
            if (bDealSucced)
            {
                this.UpdateDeals();
                Deals DEAL = ODBlotter.Get(INFO.dealId);
                if (DEAL != null)
                {
                    double dPipValue = Math.Pow(10, -RATE.Decimals);
                    DEAL.BID = RATE.BID;
                    DEAL.ASK = RATE.ASK;
                    ODBlotter.Add(DEAL);
                }

                
                this.UpdateAccount();
            }


            return bDealSucced;
        }
        private void CloseDeal(Deals DEAL) 
        {
            if (!OSBlotter.IsLoaded) return;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var SETTINGS = OSBlotter.Get(DEAL.PRODUCT);
            
            string Rate;
            string BuySell;
            string PRODUCT = DEAL.PRODUCT;
            string Amount = Math.Abs(DEAL.CONTRACT).ToString();
            int productsCount = ODBlotter.Count(PRODUCT);
            DealResponse INFO = null;
            if (DEAL.BUY) BuySell = "S"; 
            else BuySell = "B";



            
            int timeout = 0;
            Rates RATE = ORBlotter.Get(PRODUCT);
            do
            {
                timeout = 0;

                if (DEAL.BUY)
                Rate = ABBREVIATIONS.ToString(RATE.BID,RATE.Decimals); //BID.ToString();
                else Rate = ABBREVIATIONS.ToString(RATE.ASK, RATE.Decimals); //ASK.ToString();

                INFO = CEDT_TreadingService.InstantExecution(TOKEN, PRODUCT, BuySell, Amount, Rate, ACCOUNT.SqueringHazard);

                if (INFO.ErrorNo != "")
                {
                    timeout = RATE.WaitForUpdate(2000);
                    if (timeout < 0) { TsslInfo.Text = "Rate does not changed in 2s"; return; }
                    else TsslInfo.Text = "Time needed for rate to update: " + timeout + " [ms]";
                }
            } while (timeout != 0);



            double dPL = INFO.OutgoingMarginRealizedInBase;
            ACCOUNT.ClosedBalance += dPL;
            PLSum += dPL;

            Margin BALANCE = CEDT_TreadingService.GetMarginBlotter(TOKEN).Output[0];
            TsslInfo.Text = "Profit Or Loss: Session [" + PLSum + "] USD | Current: Real " +(double.Parse(BALANCE.MarginBalance) - ACCOUNT.ClosedBalanceOld) + ", Predicted " + dPL;

            this.UpdateDeals();
            DATABASE.Save_Account(ACCOUNT);
            this.UpdateAccount();
        }

        double PLSum = 0;



        //-------------------------------------------------------------------- UPDATE SECTION
        private void UpdateDeals()
        {
            

            Deal[] ADeal = CEDT_TreadingService.GetOpenDealBlotter(TOKEN).Output;

            LbxDealsOpened.Items.Clear();
            foreach (Deal D in ADeal)//Pending Deals - not squered out positions
            {
                Deals Ds = new Deals(D);
                if(!ODBlotter.Contains(D.DealReference))
                    ODBlotter.Add(Ds);
                
                LbxDealsOpened.Items.Add(Ds.DealReference);
            }

            List<string> LSDsGarbage = new List<string>();
            foreach (Deals Ds in ODBlotter.GetData)
            {
                bool found = false;
                foreach (Deal D in ADeal)
                    if (D.DealReference == Ds.DealReference) { found = true; break; }
                    
                if (!found) LSDsGarbage.Add(Ds.DealReference);
            }

            foreach (string s in LSDsGarbage)
                ODBlotter.Remove(s);

            if (LbxDealsOpened.Items.Count > 0)
                LbxDealsOpened.SelectedIndex = 0;

        }
        private void UpdateMargin()
        {


            if (ACCOUNT != null)
            {
                TbxMargin.Text = ABBREVIATIONS.ToString(ACCOUNT.ClosedBalance, 2);

                List<Deals> LDeals = ODBlotter.GetData;
                double ProfitOrLoss = 0;
                for(int i = 0; i < ODBlotter.COUNT; i++)
                {
                    Deals DEAL = LDeals[i];
                    ProfitOrLoss += AUTOTREADER.ConvertToUSD(DEAL.PRODUCT, DEAL.BUY, ACCOUNT.CalculateProfitOrLoss(DEAL, ORBlotter.Get(DEAL.PRODUCT)));
                }

                TbxProfitOrLoss.Text = ABBREVIATIONS.ToString(ProfitOrLoss, 2);

                if (ProfitOrLoss > 0) TbxProfitOrLoss.ForeColor = Color.Green;
                else if (ProfitOrLoss == 0) TbxProfitOrLoss.ForeColor = Color.Black;
                else if (ProfitOrLoss < 0) TbxProfitOrLoss.ForeColor = Color.Red;
            }
        }
        private void UpdateAction()
        {
            if (LbxCCYPairs.SelectedItem == null) return;

            string PRODUCT = LbxCCYPairs.SelectedItem.ToString();
            Rates RATE = ORBlotter.Get(PRODUCT);


            TbxASK.Text = ABBREVIATIONS.ToString(RATE.ASK, RATE.Decimals);
            TbxBID.Text = ABBREVIATIONS.ToString(RATE.BID,RATE.Decimals);
        }
        private void UpdateAccount()
        {
            if (ODBlotter.COUNT == 0)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

                Margin BALANCE = CEDT_TreadingService.GetMarginBlotter(TOKEN).Output[0]; // Margin Info - account size
                double MarginBalance = double.Parse(BALANCE.MarginBalance);

                if (ACCOUNT != null && ACCOUNT.ClosedBalance != MarginBalance)
                {
                    ACCOUNT.ClosedBalance = MarginBalance;
                    DATABASE.Save_Account(ACCOUNT);
                }
            }
        }

        private void TbxMargin_TextChanged(object sender, EventArgs e)
        {

        }


        

        




    }
}


/*

string ERRORS =
@"
0 	   No Error
1001       Authentication Failed
2001       Invalid Product
2002       BuySell not B or S
2003       Rate must be numeric
2004       Rate out of range
2005       Amount must be numeric
2006       Amount must be greater than 0
2007       Basis not T or S
2008       Expiry not EOD or GTC
2009       Trailing points must be numeric
2010       Trailing Points must be greater than 0
2011       Trailing points value out of range
2012       Reference number must be numeric
2013       Reference number must be greater than 0
2014       Invalid reference number
2015       Amount must be divisible by Size
2016       Amount must be less than or equal to (Size * MaxLots)
2017       Amount must be greater than or equal to Size
2029       Cannot subscribe to following product(s)
3001       Associated position is 0
3002       New Order activity suspended
3003       Associated Position Order already exists
3004       Pair not found
3005       Order rate must be within $$ point
3006       Stop Loss Order rate must be > current offer rate
3007       Stop Loss Order rate must be < current bid rate
3008       Limit Order rate must be < current offer rate
3009       Limit Order rate must be > current bid rate
3010       Stop Loss Order rate must be > first leg order rate
3011       Stop Loss Order rate must be < first leg order rate
3012       Limit Order rate must be < first leg order rate
3013       Limit Order rate must be > first leg order rate
3014       Customer Locked Out
3015       Record not found
3016       Order cannot be cancelled as order rate is within CancelOrderPips points
3017       Order failed - unable to get confirmation number
3018       When modifying an Order, the Order Type cannot be modified
3019       When modifying an Order, the Pair cannot be modified
3020       Order Failed
3021       Order already dealt
3022       Order already expired
3023       Order Pending
3024       Order deal pending
3025       Order being processed
3026       Insufficient Margin
3027       Order Stopped - Insufficient Margin
3028       Order rate must be within 1000 points
3029       Stop Loss Order rate must be > current offer rate
3030       Stop Loss Order rate must be < current bid rate
3031       Limit Order rate must be <= current offer rate
3032       Limit Order rate must be >= current bid rate
3033       Limit Order rate must be < current offer rate
3034       Limit Order rate must be > current bid rate
3035       Invalid rate
3036       No rate entered No order submitted
3037       Invalid Order Type
3038       Order Failed : Customer Locked Out
3039       Order Failed : Associated Position Order Already exists
3040       Order Failed : Associated Position is 0
3041       New order activity suspended
3042       Connection to database failed
3050       Order Manager Hub Offline
4001       Request failed
5001       Invalid deal sequence
5002       Confirmation not received, please call customer support
5003       Unable to complete request, the trading system is down for maintenance
5004       Rates changed
5005       Insufficient Margin
5006       An error occurred processing your request, please check all positions and or 
		contact the trading desk to confirm or deny the execution of this trade.
5007       Deal Execution failed
5008       Invalid Position for Point and Shoot
5009       Dealing temporarily suspended. No deals submitted
5010       Customer Locked Out - Check Position
5011       Rates Off-line
5012       Pair not dealable
5013       Trading Suspended
5014       Customer Locked out - Check Position
6001       Failed to get Config settings
9001       Empty User settings collection
9101       Empty Subscription Product Collection
9102       Subscription product collection count cannot exceed 
9103       Subscription product collection count cannot be less than

";
*/