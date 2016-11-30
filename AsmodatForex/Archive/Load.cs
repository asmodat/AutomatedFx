using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;

using System.IO;

namespace AsmodatForex
{

    public partial class Archive
    {
        private ThreadedStopWatch StopWatch = new ThreadedStopWatch();

        private bool InitializeData()
        {

            List<string> Pairs = this.GetAvailablePairs();
            List<ServiceConfiguration.TimeFrame> Frames = Enums.ToList<ServiceConfiguration.TimeFrame>();

            if (Pairs == null || Frames == null || Pairs.Count <= 0 || Frames.Count <= 0) return false;

            foreach (string pair in Pairs)
            {
                if (!this.Data.ContainsKey(pair))
                    this.Data.Add(pair, new ThreadedDictionary<ServiceConfiguration.TimeFrame, ThreadedDictionary<DateTime, Rate>>());

                foreach (ServiceConfiguration.TimeFrame frame in Frames)
                    if (!this.Data[pair].ContainsKey(frame))
                        this.Data[pair].Add(frame, new ThreadedDictionary<DateTime, Rate>());
            }

            return true;
        }


        public void LoadAll()
        {
            StopWatch.DateSet("LoadAll");

            Loaded = false;
            if (!InitializeData())
            {
                Loaded = true;
                return;
            }

            Loading = true;
            List<string> Pairs = this.GetAvailablePairs();
            List<ServiceConfiguration.TimeFrame> Frames = Enums.ToList<ServiceConfiguration.TimeFrame>();
            if (Pairs.Count <= 0 || Frames == null) { Loaded = true; return; }// 

            UpToDate = new Dictionary<string, DateTime>();

            this.LoadData();

            StopWatch.Run();

            foreach (string pair in Pairs) //for (int p = 0; p < Pairs.Count; p++)//foreach (ServiceConfiguration.TimeFrame frame in Frames)//for (int f = 0; f < Frames.Count; f++)
                //this.LoadFrames(pair);
                ThreadsLoad.Run(() => this.LoadFrames(pair, Frames), pair , true, true);//ThreadsLoad.Run(() => this.Load(pair, frame), pair + frame, true, true);

            ThreadsLoad.JoinAll();

            DataAssembler.Clear();

            var dimespan = StopWatch.ms();
            double us = StopWatch.us();

            
            Loaded = true;
            Loading = false;
            LoadTime = StopWatch.DateGet("LoadAll");
        }

        //foreach (string pair in Pairs) //for (int p = 0; p < Pairs.Count; p++)//
        //    foreach (ServiceConfiguration.TimeFrame frame in Frames)//for (int f = 0; f < Frames.Count; f++)
        //        this.Load(pair, frame);

        
        private Dictionary<ServiceConfiguration.TimeFrame, Dictionary<string, string>> DataAssembler;
        private void LoadData()
        {
            DataAssembler = new Dictionary<ServiceConfiguration.TimeFrame, Dictionary<string, string>>();
            List<ServiceConfiguration.TimeFrame> Frames = Enums.ToList<ServiceConfiguration.TimeFrame>();

            foreach (ServiceConfiguration.TimeFrame frame in Frames)
            {
                string[] Pathes = Directory.GetFiles(this.GetFolder(frame), "*" + FileDataExtention, SearchOption.AllDirectories);
                if (Pathes == null || Pathes.Length <= 0) continue;

                lock (Locker.Get("DataAssembler")) DataAssembler.Add(frame, new Dictionary<string, string>());

                foreach (string path in Pathes)
                {
                    string pair = Path.GetFileNameWithoutExtension(path).Replace("-", "/");


                    lock (Locker.Get("DataAssembler"))
                        if (!DataAssembler[frame].ContainsKey(pair)) DataAssembler[frame].Add(pair, string.Empty);


                    byte[] bytes = File.ReadAllBytes(path);
                    string data = Asmodat.IO.Compression.UnZipString(bytes);

                    lock (Locker.Get("DataAssembler")) DataAssembler[frame][pair] += data;// .Append(data);
                }
            }
        }

        private void LoadFrames(string pair, List<ServiceConfiguration.TimeFrame> Frames)
        {
            foreach (ServiceConfiguration.TimeFrame frame in Frames)//for (int f = 0; f < Frames.Count; f++)
                    this.LoadRates(pair, frame);
        }
        

        /// <summary>
        /// This method loads specified pair-frame data from file in hard drive
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        private void LoadRates(string pair, ServiceConfiguration.TimeFrame frame)
        {
            if (DataAssembler.ContainsKey(frame) && DataAssembler[frame].ContainsKey(pair))
            {
                Dictionary<DateTime, Rate> Rates = RateInfo.ParseRates(DataAssembler[frame][pair]);

                if (Rates == null || Rates.Count <= 0)
                    return;

                this.Data[pair][frame].AddRange(Rates);
                lock (Locker.Get("DataAssembler")) SetUpToDate(pair, frame, this.Data[pair][frame].Keys.Last());
                /*
                 * string data;
                lock (Locker.Get("DataAssembler")) data = DataAssembler[frame][pair];//.ToString();
                Dictionary<DateTime, Rate> Rates = RateInfo.ParseRates(data);
                this.Data[pair][frame].AddRange(Rates);
                lock (Locker.Get("DataAssembler")) SetUpToDate(pair, frame, this.Data[pair][frame].Keys.Last());
                 */
            }
        }


        /// <summary>
        /// Returns Pair names from Data folder
        /// </summary>
        /// <returns></returns>
        private List<string> GetAvailablePairs()
        {
            List<string> Pairs = new List<string>();
            if (!Directory.Exists(DirectoryData)) return Pairs;

            string[] Pathes = Directory.GetFiles(DirectoryData, "*-*" + FileDataExtention, SearchOption.AllDirectories);

            foreach (string path in Pathes)
            {
                string info = Path.GetFileNameWithoutExtension(path);
                if (System.String.IsNullOrEmpty(info) || info.Length != 7) 
                    continue;// || !info.Contains("-") - not neaded search pattern already included
                string pair = info.Replace("-", "/");

                if (!Pairs.Contains(pair))
                    Pairs.Add(pair);
            }

            return Pairs;
        }


        
    }
}
