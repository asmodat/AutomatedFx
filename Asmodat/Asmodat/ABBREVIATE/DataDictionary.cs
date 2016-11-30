using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{

    /// <summary>
    /// This is String-Type Dictionary class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataDictionary<T>
    {
        //private static volatile DataDictionary instance;
        //private static object syncRoot = new Object();
        //public static DataDictionary Instance
        //{
        //    get
        //    {
        //        if(instance == null)
        //        {
        //            lock(syncRoot)
        //            {
        //                if (instance == null)
        //                    instance = new DataDictionary();
        //            }
        //        }

        //        return instance;
        //    }
        //}

        private Dictionary<string, T> DSOData = new Dictionary<string, T>();

        public bool ChangeKey(string sOldKey, string sNewKey)
        {
            if (!this.Contains(sOldKey) || this.Contains(sNewKey)) return false;

            T TSave = this.Get(sOldKey);

            if (!this.Remove(sOldKey)) return false;

            return this.Set(sNewKey, TSave);
        }



        public bool Contains(string sKey)
        {
            if (DSOData.ContainsKey(sKey))
                return true;
            else return false;
        }

        public bool Set(string sKey, T tValue, bool bUpdate = true)
        {
            if(sKey == null) return false;

            if (!this.Contains(sKey))
            {
                DSOData.Add(sKey, tValue);
                return true;
            }

            if (bUpdate)
            {
                DSOData[sKey] = tValue;
                return true;
            }

            return false;
        }

        public T Get(string sKey)
        {
            if (sKey == null || !DSOData.ContainsKey(sKey)) return default(T);

            return DSOData[sKey];
        }

        public bool Remove(string sKey)
        {
            if (!this.Contains(sKey)) return true;

            DSOData.Remove(sKey);

            if (!this.Contains(sKey)) return true;
            else return false;
        }


        public string[] Keys
        {
            get
            {
                return DSOData.Keys.ToArray();
            }
        }

    }
}
