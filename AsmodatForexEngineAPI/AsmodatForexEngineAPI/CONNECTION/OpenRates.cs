using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Threading;

namespace AsmodatForexEngineAPI
{
    public class OpenRatesBlotter
    {


        private List<Rates> DATA = new List<Rates>();
        private ArchiveRates ArchiveDATA = new ArchiveRates();
        private List<string> LSProducts = new List<string>();

        public int Count
        {
            get
            {
                return DATA.Count;
            }
        }

        public ArchiveRates Archive
        {
            get
            {
                return ArchiveDATA;
            }
        }

       



        public List<string> GetProducts
        {
            get
            {
                return LSProducts; // List<string> LSProducts = (from R in DATA select R.CCY_Pair).ToList(); 
                
            }
        }

        public List<Rates> GetData
        {
            get
            {
               

                    //List<Rates> LRData = DATA.Select(R => R.Clone()).ToList();

                List<Rates> LRData = new List<Rates>();


                for (int i = 0; i < DATA.Count; i++ )
                    LRData.Add(DATA[i].Clone());
                



                    return LRData;
                
            }
        }
        

        /// <summary>
        /// Adds or Replaces RATE with specified CCY_Pair name
        /// </summary>
        /// <param name="RATE">Rates that have to be instert or replaced.</param>
        public void Add(Rates RATE)
        {
            

            int index = DATA.FindIndex(R => R.CCY_Pair == RATE.CCY_Pair);

            if (index < 0 || index > DATA.Count)
            {
                DATA.Add(RATE);
                LSProducts.Add(RATE.CCY_Pair);
            }
            else
            {
                DATA[index] = RATE;
            }

            ArchiveDATA.Add(RATE);
        }
        public Rates Get(string CCY_Pair)
        {

            Rates RATE = null;// DATA.FirstOrDefault(R => R.CCY_Pair == CCY_Pair);

            for (int i = 0; i < this.Count; i++)
            {

                if (DATA[i].CCY_Pair == CCY_Pair)
                    RATE = DATA[i];//.Clone();
                
            }
            return RATE;
        }

        public Rates Get(int CCY_Token)
        {
           
            //Rates RATE = DATA.FirstOrDefault(R => R.CCY_Token == CCY_Token);

            Rates RATE = null;// DATA.FirstOrDefault(R => R.CCY_Pair == CCY_Pair);

            for (int i = 0; i < this.Count; i++)
            {

                if (DATA[i].CCY_Token == CCY_Token)
                    RATE = DATA[i].Clone();
            }
            return RATE;
  
        }

        /// <summary>
        /// Removes Deals from class and from DATABASE
        /// </summary>
        /// <param name="DealReference">Reference or DealID of deal that will be removed.</param>
        public void Remove(string CCY_Pair)
        {
            Rates RATE = this.Get(CCY_Pair);

           

            DATA.Remove(RATE);
            LSProducts.Remove(RATE.CCY_Pair);
          

           // while (!DATA.TryTake(out RATE)) ;
        }
        public bool Contains(string CCY_Pair)
        {


                return LSProducts.Contains(CCY_Pair);// DATA.Any(R => R.CCY_Pair == CCY_Pair);
            
        }

    }
}
