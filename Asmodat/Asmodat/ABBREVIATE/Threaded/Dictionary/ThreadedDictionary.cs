using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {

        public Dictionary<TKey, TValue> ToDictionary()
        {
            lock (this)
                return new Dictionary<TKey, TValue>(this);
        }

        public ThreadedDictionary(Dictionary<TKey, TValue> dictrionary)
        {
            lock(this)
            {
                this.Clear();
                if(dictrionary != null)
                foreach(var v in dictrionary)
                    base.Add(v.Key, v.Value);
            }
        }

        
#if (DEBUG)
        [XmlIgnore]
        public new TValue this[TKey key]
        {
            get
            {
                lock (this)
                {
                    if (!base.ContainsKey(key))
                        return default(TValue);

                    return base[key];

                }
            }
            set { this.Add(key, value); }
        }
#else
        [XmlIgnore]
        public new TValue this[TKey key]
        {
            get { lock (this) return base[key]; }
            set { this.Add(key, value); }
        }
#endif
        public int? CountValues<T>(TKey key)
        {
                lock (this)
                {
                    if (!base.ContainsKey(key))
                        return null;

                    var val = base[key] as IEnumerable<T>;
                    return val.Count();
                }
        }


        //[XmlIgnore]
        //public new TValue this[TKey key]
        //{
        //    get { lock (this) return base[key]; }
        //    set { this.Add(key, value); }
        //}
        [XmlIgnore]
        public new KeyCollection Keys { get { lock (this) return base.Keys; } }
        [XmlIgnore]
        public new ValueCollection Values { get { lock (this) return base.Values; } }
        [XmlIgnore]
        public new int Count { get { lock (this) return base.Count; } }


        [XmlIgnore]
        public TValue[] ValuesArray { get { lock (this) return base.Values.ToArray(); } }


        [XmlIgnore]
        public TKey[] KeysArray { get { lock (this) return base.Keys.ToArray(); } }

        [XmlIgnore]
        public TValue FirstValue { get { lock (this) return base.Values.First(); } }
        [XmlIgnore]
        public TKey FirstKey { get { lock (this) return base.Keys.First(); } }

        //public KeyValuePair<TKey, TValue> ElementAt(int i)
        //{

        //    lock (this) return base.ElementAt<KeyValuePair<TKey, TValue>>(i);
        //}

        //public int CountNested<K, V>() //base.Values.Sum(x => ((ThreadedDictionary<K, V>)(object)x).Count);
        //{
        //    int sum = 0;
        //    lock (this)
        //    {

        //        foreach (TValue v in base.Values)
        //        {
        //            if (v == null) continue;
        //            ThreadedDictionary<K, V> data = (ThreadedDictionary<K, V>)(object)v;
                   
        //            sum += data.Count;
        //        }

        //    }
        //    return sum;
        //}
        //public KeyValuePair<TKey, TValue>[] ToArray() { lock (this) return base.ToArray(); }

        /// <summary>
        /// This property defines wheter or not data inside dixionary is in specied state, 
        /// This propery can be set by user in order to determine if dictionary data is valid and up to date, or needs to be updated
        /// </summary>
        [XmlIgnore]
        public bool IsValid{ get; set; }



        /// <summary>
        /// this property adds or updates value inside dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="update">true if value can be updated regardles if key exists</param>
        public new void Add(TKey key, TValue value, bool update = true)
        {
            if (this.ContainsKey(key)) { if (update) lock (this) { base[key] = value; } }
            else lock (this) { base.Add(key, value); }


            UpdateTime = DateTime.Now;
        }


        public void AddRange(Dictionary<TKey, TValue> data)
        {

            lock(this)
            {
                foreach(KeyValuePair<TKey, TValue> kvp in data)
                {
                    base.Add(kvp.Key, kvp.Value);
                    //if (kv.Value != null && kv.Key != null)
                    //if (!base.ContainsKey(kv.Key))
                    //    base.Add(kv.Key, kv.Value);
                    //else base[kv.Key] = kv.Value;
                }
            }

            UpdateTime = DateTime.Now;
        }


        //public new void AddRange(ThreadedDictionary<TKey, TValue> data)
        //{

        //    UpdateTime = DateTime.Now;
        //}

        /// <summary>
        /// Thi method is threadsafe allows to check if dictionary contains specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(TKey key)
        {
            //if (key == null) return false;
            lock (this) { return base.ContainsKey(key); }
        }

        /// <summary>
        /// Removes key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returns fale if kay was not removed, else if removed returns true. </returns>
        public new bool Remove(TKey key)
        {
            DateTime DTStart = DateTime.Now;

            lock (this)
                if (!base.Remove(key))
                {
                    //WTF ?
                }


            if (this.ContainsKey(key)) return false;
            else
            {
                UpdateTime = DateTime.Now;
                return true;
            }
        }
    }
}
