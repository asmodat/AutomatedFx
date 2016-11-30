using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForex.com.efxnow.demoweb.tradingservice;
using AsmodatForex.com.efxnow.demoweb.configurationservice;

using Asmodat.Abbreviate;

using Asmodat.Types;

using Asmodat.IO;

namespace AsmodatForex
{
    public partial class ServiceTrading
    {
        private File FileDealRequest;
        ThreadedMethod ThreadRequest;
        ThreadedDictionary<TickTime, DealRequest> DataDealRequest;

        private void InitRequestManager()
        {
            FileDealRequest = new File("DealRequest", true);
            ThreadRequest = new ThreadedMethod(100, System.Threading.ThreadPriority.Highest, 1);
            DataDealRequest = new ThreadedDictionary<TickTime, DealRequest>();

            string data = FileDealRequest.Load();
            if (System.String.IsNullOrEmpty(data))
                return;

            DataDealRequest.XmlDeserialize(data);

        }



        public void SaveDataRequest(string ID = null)
        {
            ThreadRequest.Run(() => FileDealRequest.Save(DataDealRequest.XmlSerialize()), "Request_IO_" + ID, true, true);
        }

        public void Save(DealRequest DRequest)
        {
            lock (DataDealRequest)
            {
                if (DataDealRequest.ContainsKey(DRequest.CreationTime))
                    DataDealRequest[DRequest.CreationTime] = DRequest;
                else DataDealRequest.Add(DRequest.CreationTime, DRequest);
            }
            
            this.SaveDataRequest(DRequest.CreationTime.ToString());
        }


        public void DealRequest(bool TFBuySell, Rate rate, int amount, int tolerance, int expiration = 1000)
        {
            string product = rate.Pair;

            if (!IsReady || !ForexConfiguration.ProductSettings.ContainsKey(product) || !ForexRates.Data.ContainsKey(product)) return;

            ProductSetting Settings = ForexConfiguration.ProductSettings[product];
            DealRequest DRequest = new DealRequest(expiration);

            DRequest.Product = product;
            DRequest.Executed = false;
            DRequest.Buy = TFBuySell;
            DRequest.Close = false;
            DRequest.ASK = rate.OFFER;
            DRequest.BID = rate.BID;
            DRequest.Amount = amount; //DRequest.Lots = (lots * int.Parse(Settings.OrderSize)).ToString();
            DRequest.Tolerance = tolerance;


            this.Save(DRequest);
        }

        public Deal GetDealByConfirmationNumber(string confirmation)
        {
            Deal deal = null;

            foreach (Deal d in Deals.Values)
                if (d != null && d.ConfirmationNumber == confirmation)
                    deal = d;

            return deal;
        }

        public void DealClose(string confirmation, int tolerance, int expiration)
        {
            if (System.String.IsNullOrEmpty(confirmation)) return;

            Deal deal = this.GetDealByConfirmationNumber(confirmation);


            if (deal == null) return;

            


            string product = deal.Product;
            

             if (!IsReady || !ForexConfiguration.ProductSettings.ContainsKey(product) || !ForexRates.Data.ContainsKey(product)) return;

            ProductSetting Settings = ForexConfiguration.ProductSettings[product];
            Rate rate = ForexRates.GetRate(product);

            DealRequest DRequest = new DealRequest(expiration);

            DRequest.Product = deal.Product;
            DRequest.Amount = Math.Abs(int.Parse(deal.Contract)); //It can be negative if positionis short
            DRequest.ConfirmationNumber = deal.ConfirmationNumber;
            DRequest.DealSequence = deal.DealSequence;
            DRequest.DealReference = deal.DealReference;
            DRequest.Contract = deal.Contract;
            DRequest.ASK = rate.OFFER;
            DRequest.BID = rate.BID;
            DRequest.BuySell = deal.BuySell;// DRequest.Buy = !DRequest.Buy; //Reverse Order for closing purpouses
            DRequest.Buy = !DRequest.Buy;
            DRequest.Close = true;
            DRequest.Rate = deal.Rate;
            DRequest.Tolerance = tolerance;

            this.Save(DRequest);
        }

        public void PeacemakerRequest()
        {
            if (!this.IsReady) return;

            IsBusy = true;
            List<DealRequest> LDRequests = null;
            lock (DataDealRequest)
                LDRequests = DataDealRequest.Values.ToList();
            LDRequests.Reverse();

            bool requested = false;

            foreach (DealRequest DRequest in LDRequests)
                if (DRequest.IsAlive) //execute all alive requests at once
                {
                    ThreadRequest.Run(() => this.DealInstantExecute(DRequest), "Request_Exe_" + DRequest.CreationTime, true, true);
                    requested = true;
                }

            ThreadRequest.JoinAll();

            if (requested)
                DataDeals.IsValid = false;

        }


        

        /// <summary>
        /// This method tries to instantly execute passed request
        /// </summary>
        /// <param name="DRequest">DealRequest containing information how to communicate with server.</param>
        public void DealInstantExecute(DealRequest DRequest)
        {
            if (!IsReady) return;

            #region Liquidate All
            if (DRequest.LiquidateAll)
            {
                
                BlotterOfDealResponse blotter = CEDTS_TradingService.LiquidateAll(ForexAuthentication.Token);

                if(ServiceTrading.BlotterSuccess(blotter))
                {
                    DataDealRequest[DRequest.CreationTime].Executed = true;
                    this.Save(DRequest);
                }

                AccountLogs.IsValid = false;
                return;
            }
            #endregion

            string product = DRequest.Product;
            if (!ForexRates.Data.ContainsKey(product) ) return;

            #region Close Position
            if (DRequest.ClosePosition)
            {
                DealResponse DResponse = CEDTS_TradingService.ClosePosition(ForexAuthentication.Token, product);

                if (DResponse.success)
                {
                    DataDealRequest[DRequest.CreationTime].Executed = true;
                    this.Save(DRequest);
                }
                
                AccountLogs.IsValid = false;
                return;
            }
            #endregion



            int iDecimals = ForexConfiguration.GetDecimals(product);// ForexRates.Data[product].DECIMALS;
            string sRate = DRequest.Buy ? Doubles.ToString(DRequest.ASK, null, iDecimals, 0, double.MaxValue, '.') : Doubles.ToString(DRequest.BID, null, iDecimals, 0, double.MaxValue, '.');
            string amount = DRequest.Amount.ToString();

            #region Close
            if (DRequest.Close)
            {
                //double profit = this.GetLiveProfit(DRequest.ConfirmationNumber);

                DealResponse DResponse = CEDTS_TradingService.InstantExecution(
                    ForexAuthentication.Token,
                    product,
                    DRequest.BuySell,
                    amount,
                    sRate,
                     (int)DRequest.Tolerance);

                if (DResponse.success)
                {
                    //Account.ClosedBalance += profit;
                    DataDealRequest[DRequest.CreationTime].Executed = true;
                    this.Save(DRequest);
                }

                AccountLogs.IsValid = false;
                return;
            }
            #endregion

            #region Buy Sell
            if (DRequest.Buy || DRequest.Sell)
            {
                

                DealResponse DResponse = CEDTS_TradingService.InstantExecution(
                    ForexAuthentication.Token,
                    product,
                    DRequest.BuySell,
                    amount,
                    sRate,
                     (int)DRequest.Tolerance);

                if (DResponse.success)
                {
                    DataDealRequest[DRequest.CreationTime].Executed = true;
                    this.Save(DRequest);
                }


                AccountLogs.IsValid = false;
                return;
            }
            #endregion

            throw new Exception("DealInstantExecute Undefined request !");
        }



    }
}
