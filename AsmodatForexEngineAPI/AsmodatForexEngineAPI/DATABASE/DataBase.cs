using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AsmodatSerialization;

namespace AsmodatForexEngineAPI
{
    public class DataBase
    {
        Abbreviations ABBREVIATION = new Abbreviations();

        public DataBase(string UserName)
        {
            Directory_Main = Directory.GetCurrentDirectory() + @"\Asmodat_ForexEngineAPI\DATA_BASE";
            Directory_User = Directory_Main + @"\" + UserName;
            Directory_Deals = Directory_User + @"\DEALS\";
            Directory_Rates = Directory_User + @"\RATES\";
            Directory_ChartPoints = Directory_User + @"\CHARTING\";
            Directory_ChartPointsPrediction = Directory_User + @"\PREDICTION\";
            Patch_Account = Directory_User + @"\" + UserName + ".asa";

            if (!Directory.Exists(Directory_Main))
                Directory.CreateDirectory(Directory_Main);
            if (!Directory.Exists(Directory_User))
                Directory.CreateDirectory(Directory_User);
            if (!Directory.Exists(Directory_Deals))
                Directory.CreateDirectory(Directory_Deals);
            if (!Directory.Exists(Directory_Rates))
                Directory.CreateDirectory(Directory_Rates);
            if (!Directory.Exists(Directory_ChartPoints))
                Directory.CreateDirectory(Directory_ChartPoints);
            if (!Directory.Exists(Directory_ChartPointsPrediction))
                Directory.CreateDirectory(Directory_ChartPointsPrediction);

        }

        public string Directory_Main, Directory_User, Directory_Deals, Directory_Rates, Directory_ChartPoints, Directory_ChartPointsPrediction;
        public string Patch_Account;
        static string Extention_Deals = @".asds";
        static string Extention_Rates = @".aslr";
        static string Extention_ChartPoints = @".ascp";
        static string Extention_ChartPointsPrediction = @".ascpp";

        public void Save_Deals(Deals DEALS)
        {
            AsmodatSerialization.DataBase_Deals ASDB_Deals = new AsmodatSerialization.DataBase_Deals();
            ASDB_Deals.ASK = DEALS.ASK;
            ASDB_Deals.BID = DEALS.BID;
            ASDB_Deals.BUY = DEALS.BUY;
            ASDB_Deals.CONFIRMATIONNUMBER = DEALS.ConfirmationNumber;
            ASDB_Deals.CONTRACT = DEALS.CONTRACT;
            ASDB_Deals.COUNTER = DEALS.COUNTER;
            ASDB_Deals.DEALREFERENCE = DEALS.DealReference;
            ASDB_Deals.DEALSEQUENCE = DEALS.DealSequence;
            ASDB_Deals.OPENED = DEALS.OPENED;
            ASDB_Deals.PRODUCT = DEALS.PRODUCT;
            ASDB_Deals.RATE = DEALS.RATE;
            ASDB_Deals.SELL = DEALS.SELL;
            ASDB_Deals.TIME = DEALS.TIME;

            string PATCH = Directory_Deals + DEALS.DealReference + Extention_Deals;
            this.Delete_Deals(DEALS.DealReference);
            AsmodatSerialization.Operations.Save<AsmodatSerialization.DataBase_Deals>(ASDB_Deals, PATCH);
        }
        public void Delete_Deals(string DealReference)
        {
            string PATCH = Directory_Deals + DealReference + Extention_Deals;
            if (File.Exists(PATCH)) File.Delete(PATCH);
        }
        public Deals Load_Deals(string DealReference)
        {
            string PATCH = Directory_Deals + DealReference + Extention_Deals;
            AsmodatSerialization.DataBase_Deals ASDB_Deals = AsmodatSerialization.Operations.Load<DataBase_Deals>(PATCH);


            if (ASDB_Deals == null)
            {
                return null;
            }

            Deals DEALS = new Deals();

            DEALS.ASK = ASDB_Deals.ASK;
            DEALS.BID = ASDB_Deals.BID;
            DEALS.BUY = ASDB_Deals.BUY;
            DEALS.ConfirmationNumber = ASDB_Deals.CONFIRMATIONNUMBER;
            DEALS.CONTRACT = ASDB_Deals.CONTRACT;
            DEALS.COUNTER = ASDB_Deals.COUNTER;
            DEALS.DealReference = ASDB_Deals.DEALREFERENCE;
            DEALS.DealSequence = ASDB_Deals.DEALSEQUENCE;
            DEALS.OPENED = ASDB_Deals.OPENED;
            DEALS.PRODUCT = ASDB_Deals.PRODUCT;
            DEALS.RATE = ASDB_Deals.RATE;
            DEALS.SELL = ASDB_Deals.SELL;
            DEALS.TIME = ASDB_Deals.TIME;

            return DEALS;
        }

        public void Save_Account(Account ACCOUNT)
        {
            AsmodatSerialization.DataBase_Account ASDB_Account = new AsmodatSerialization.DataBase_Account();

            ASDB_Account.CLOSEDBALANCE = ACCOUNT.ClosedBalance;

            this.Delete_Account();
            AsmodatSerialization.Operations.Save<AsmodatSerialization.DataBase_Account>(ASDB_Account, Patch_Account);
        }
        public void Delete_Account()
        {
            if (File.Exists(Patch_Account)) File.Delete(Patch_Account);
        }
        public Account Load_Account(string USERID)
        {
            AsmodatSerialization.DataBase_Account ASDB_Account = AsmodatSerialization.Operations.Load<DataBase_Account>(Patch_Account);


            if (ASDB_Account == null)
            {
                return null;
            }

            Account ACCOUNT = new Account();

            ACCOUNT.ClosedBalance = ASDB_Account.CLOSEDBALANCE;

            return ACCOUNT;
        }

        public void Save_LRates(List<Rates> LRates, string product, DateTime Date)
        {
            List<AsmodatSerialization.DataBase_Rates> LASDB_Rates = new List<DataBase_Rates>();

            foreach(Rates RATE in LRates)
                LASDB_Rates.Add(RATE.ToDataBase());

            product = product.Replace("/", "-");

            string directory = Directory_Rates + Date.Year + "+" + Date.Month + @"\";
            string PATCH = directory + product + Extention_Rates;
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            this.Delete_LRates(product, Date);

            AsmodatSerialization.Operations.Save<List<AsmodatSerialization.DataBase_Rates>>(LASDB_Rates, PATCH);
        }
        public void Delete_LRates(string product, DateTime Date)
        {
            product = product.Replace("/", "-");

            string folder = Date.Year + "+" + Date.Month + @"\";
            string PATCH = Directory_Rates + folder + product + Extention_Rates;
            if (File.Exists(PATCH)) File.Delete(PATCH);
        }
        public List<Rates> Load_LRates(string product, DateTime Date)
        {
            product = product.Replace("/", "-");

            string folder = Date.Year + "+" + Date.Month + @"\";
            string PATCH = Directory_Rates + folder + product + Extention_Rates;
            List<AsmodatSerialization.DataBase_Rates> LASDB_Rates = AsmodatSerialization.Operations.Load<List<AsmodatSerialization.DataBase_Rates>>(PATCH);
            List<Rates> LRates = new List<Rates>();

            if (LASDB_Rates == null) return LRates;

            foreach (DataBase_Rates DBRates in LASDB_Rates)
                LRates.Add(new Rates(DBRates));

            return LRates;
        }


        public void Save_ChartPoints(List<ChartPoint> LCPoints, string product, DateTime Date, string TimeFrame)
        {

            product = product.Replace("/","-");

            List<AsmodatSerialization.DataBase_ChartPoint> LASDB_CPoints = new List<DataBase_ChartPoint>();


            foreach (ChartPoint CPoint in LCPoints)
                LASDB_CPoints.Add(CPoint.ToDataBase());

            string directory = Directory_ChartPoints + TimeFrame + @"\" +Date.Year + "+" + Date.Month + @"\";
            string PATCH = directory + product + Extention_Rates;
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            this.Delete_ChartPoints(product, Date, TimeFrame);

            AsmodatSerialization.Operations.Save<List<AsmodatSerialization.DataBase_ChartPoint>>(LASDB_CPoints, PATCH);
        }
        public void Delete_ChartPoints(string product, DateTime Date, string TimeFrame)
        {
            product = product.Replace("/", "-");

            string folder = TimeFrame + @"\" + Date.Year + "+" + Date.Month + @"\";
            string PATCH = Directory_ChartPoints + folder + product + Extention_Rates;
            if (File.Exists(PATCH)) File.Delete(PATCH);
        }
        public List<ChartPoint> Load_ChartPoints(string product, DateTime Date, string TimeFrame)
        {
            product = product.Replace("/", "-");

            string folder = TimeFrame + @"\" + Date.Year + "+" + Date.Month + @"\";
            string PATCH = Directory_ChartPoints + folder + product + Extention_Rates;
            List<AsmodatSerialization.DataBase_ChartPoint> LASDB_CPoints = AsmodatSerialization.Operations.Load<List<AsmodatSerialization.DataBase_ChartPoint>>(PATCH);
            List<ChartPoint> LCPoint = new List<ChartPoint>();

            if (LASDB_CPoints == null) return LCPoint;

            foreach (DataBase_ChartPoint DBCPoint in LASDB_CPoints)
                LCPoint.Add(new ChartPoint(DBCPoint));

            return LCPoint;
        }


        public void Save_ChartPointsPrediction(List<ChartPointsPredition> LCPsPrediction)
        {
            ChartPointsPredition CPsPSource = LCPsPrediction.First(CPsP => CPsP != null && CPsP.Analised);
            string TimeFrame = ABBREVIATION.ToString(CPsPSource.TimeFrame);

            string product = CPsPSource.Product.Replace("/", "-");
            int deep = CPsPSource.Deep;
            int ahead = CPsPSource.Ahead;

            List<AsmodatSerialization.DataBase_ChartPointsPrediction> LASDB_CPsPredictions = new List<DataBase_ChartPointsPrediction>();


            foreach (ChartPointsPredition CPsPrediction in LCPsPrediction)
                LASDB_CPsPredictions.Add(CPsPrediction.ToDataBase());

            string directory = Directory_ChartPointsPrediction + TimeFrame + @"\" + CPsPSource.Deep + "+" + CPsPSource.Ahead + @"\";
            string PATCH = directory + product + Extention_ChartPointsPrediction;
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            this.Delete_ChartPointsPrediction(product, TimeFrame, deep, ahead);

            AsmodatSerialization.Operations.Save<List<AsmodatSerialization.DataBase_ChartPointsPrediction>>(LASDB_CPsPredictions, PATCH);
        }
        public void Delete_ChartPointsPrediction(string product, string TimeFrame, int deep, int ahead)
        {
            product = product.Replace("/", "-");

            string directory = Directory_ChartPointsPrediction + TimeFrame + @"\" + deep + "+" + ahead + @"\";
            string PATCH = directory + product + Extention_ChartPointsPrediction;
            if (File.Exists(PATCH)) File.Delete(PATCH);
        }
        public List<ChartPointsPredition> Load_ChartPointsPrediction(string product, string TimeFrame, int deep, int ahead)
        {
            product = product.Replace("/", "-");

            string directory = Directory_ChartPointsPrediction + TimeFrame + @"\" + deep + "+" + ahead + @"\";
            string PATCH = directory + product + Extention_ChartPointsPrediction;

            List<AsmodatSerialization.DataBase_ChartPointsPrediction> LASDB_CPsPredictions = AsmodatSerialization.Operations.Load<List<AsmodatSerialization.DataBase_ChartPointsPrediction>>(PATCH);
            List<ChartPointsPredition> LCPsPredictions = new List<ChartPointsPredition>();

            if (LASDB_CPsPredictions == null) return LCPsPredictions;

            foreach (DataBase_ChartPointsPrediction DBCPsPrediction in LASDB_CPsPredictions)
                LCPsPredictions.Add(new ChartPointsPredition(DBCPsPrediction));

            return LCPsPredictions;
        }
    }
}

