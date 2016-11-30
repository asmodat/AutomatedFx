using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public class OpenDealsBlotter
    {
        AsmodatForexEngineAPI.DataBase DATABASE;
        public OpenDealsBlotter(string TOKEN, string USERID)
        {
            this.TOKEN = TOKEN;
            this.USERID = USERID;
            DATABASE = new DataBase(USERID);

           string[] SArray =  Directory.GetFiles(DATABASE.Directory_Deals);

            foreach(string s in SArray)
            {
                string DealID = Path.GetFileNameWithoutExtension(s);
                Deals Ds = DATABASE.Load_Deals(DealID);

                if (Ds != null && Ds.DealReference == DealID)
                    DATA.Add(Ds);
            }

        }

        public void Restart(string TOKEN)
        {
            this.TOKEN = TOKEN;
        }

        private string TOKEN, USERID;
        com.efxnow.demoweb.tradingservice.TradingService CEDT_TreadingService = new com.efxnow.demoweb.tradingservice.TradingService();

        private List<Deals> DATA = new List<Deals>();

        public List<Deals> GetData
        {
            get
            {
                return DATA;
            }
        }

        public int COUNT
        {
            get
            {
                return DATA.Count();
            }
        }

        public int Count(string Product)
        {
            int counter = 0;
            foreach(Deals Ds in DATA)
                if (Ds.PRODUCT == Product) ++counter;

            return counter;
        }

        /// <summary>
        /// Adds or Replaces DEAL with specified DealID (DealReference).
        /// </summary>
        /// <param name="DEAL">Deals that have to be instert or replaced.</param>
        public void Add(Deals DEAL)
        {
            bool found = false;
            for(int i = 0; i < DATA.Count; i++)
            {
                if(DATA[i].DealReference == DEAL.DealReference)
                {
                    DATA[i] = DEAL;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                DATA.Add(DEAL);
                DATABASE.Save_Deals(DEAL);
            }
        }
        public Deals Get(string DealReference)
        {
            Deals DEAL = null;
            foreach (Deals D in DATA)
                if (D.DealReference.ToString() == DealReference) 
                    DEAL = D;

            return DEAL;
        }

        /// <summary>
        /// Removes Deals from class and from DATABASE
        /// </summary>
        /// <param name="DealReference">Reference or DealID of deal that will be removed.</param>
        public void Remove(string DealReference)
        {
            foreach (Deals D in DATA)
                if (D.DealReference.ToString() == DealReference) 
                {
                    DATABASE.Delete_Deals(D.DealReference);
                    DATA.Remove(D); 
                    return; 
                }
            
        }


        public bool Contains(string DealReference)
        {
            bool found = false;
            foreach (Deals D in DATA)
                if (D.DealReference.ToString() == DealReference) found = true;

            return found;
        }



    }
}
