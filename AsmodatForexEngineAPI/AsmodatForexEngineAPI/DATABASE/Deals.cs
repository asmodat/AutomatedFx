using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{
    public class Deals
    {
        public Deals()
        {

        }

        public Deals(com.efxnow.demoweb.tradingservice.Deal D)
        {
           Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

           if (D.BuySell == "B") BUY = true;
           else if (D.BuySell == "S") BUY = false;
           else throw new Exception();

            this.ConfirmationNumber = long.Parse(D.ConfirmationNumber);
            this.CONTRACT = int.Parse(D.Contract);
            this.COUNTER = double.Parse(D.Counter);
            this.TIME = DateTime.ParseExact(D.DealDate, "yyyy-MM-dd HH:mm:ss.fff", null); //"2014-02-07 16:14:34.000"

            this.DealReference = D.DealReference;
            this.DealSequence = D.DealSequence;

            this.PRODUCT = D.Product;
            this.RATE = double.Parse(D.Rate);

            if (D.Status == "O") OPENED = true;
            else if (D.Status == "C") OPENED = false;
            else throw new Exception();
        }

        private bool Buy;
        private bool Sell;
        public bool BUY
        {
            get
            {
                return Buy;
            }
            set
            {
                this.Buy = value;
                this.Sell = !value;
            }
        }
        public bool SELL
        {
            get
            {
                return Sell;
            }
            set
            {
                this.Sell = value;
                this.Buy = !value;
            }
        }


        public DateTime TIME;
        public long ConfirmationNumber;
        public string DealReference;
        public string DealSequence;
        public string PRODUCT;
        public int CONTRACT;
        public double COUNTER;
        public double RATE;
       
        public bool OPENED;

        public bool LooseWaring = false;


        public double Spread
        {
            get
            {
                return (ASK - BID);
            }
        }

        public double BID, ASK;


    }


    



}
