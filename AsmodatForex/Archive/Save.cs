using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using AsmodatForex.com.efxnow.democharting.chartingservice;

using Asmodat.Abbreviate;

using System.IO;

using Asmodat.IO;

            //StopWatch.Run("SaveAll");
            //var dimespan = StopWatch.ms("SaveAll");
            //StopWatch.Run("SaveAll2");
            //var dimespan2 = StopWatch.ms("SaveAll2");
namespace AsmodatForex
{

    public partial class Archive
    {
        public string DirectoryData = Directory.GetCurrentDirectory() + @"\AsmodatForexArchive\Data";
        public string FileDataExtention = @".aad";

        public bool SaveAll(bool upToDate)
        {
            upToDate = false;
            if (!this.Loaded) return false;

            Saving = true;

            this.DisassemblyData();


            this.SaveData();

            Saving = false;

            DataDisassembler.Clear();
            return true;
        }


        
        



        private Dictionary<string, byte[]> DataDisassembler;
        private void DisassemblyData()
        {
            DataDisassembler = new Dictionary<string, byte[]>();
            List<ServiceConfiguration.TimeFrame> Frames = Enums.ToList<ServiceConfiguration.TimeFrame>();

            foreach (string pair in this.Data.Keys)
                foreach (ServiceConfiguration.TimeFrame frame in this.Data[pair].Keys)
                    ThreadsSave.Run(() => this.UnloadRates(pair, frame), pair + frame, true, true);//ThreadsLoad.Run(() => this.Load(pair, frame), pair + frame, true, true);
                    //this.UnloadRates(pair, frame);
            ThreadsSave.JoinAll();
        }

        private void SaveData()
        {
            foreach (KeyValuePair<string, byte[]> kvp in DataDisassembler)
            {
                //string data = Compression.ZipString(kvp.Value.ToString());
                ThreadsSave.Run(() => System.IO.File.WriteAllBytes(kvp.Key, kvp.Value), kvp.Key, true, true); //Tis was previously done
                //ThreadsSave.Run(() => File.WriteAllText(kvp.Key, data), kvp.Key, true, true);
            }
                //File.WriteAllText(kvp.Key, kvp.Value.ToString());

            ThreadsSave.JoinAll();
        }


        private void UnloadRates(string pair, ServiceConfiguration.TimeFrame frame)
        {
            string path;
            lock(Locker.Get("GetPath")) path = this.GetPath(frame, pair);

            Rate[] Rates = Data[pair][frame].Values.ToArray();

            //Do not disassembly if no data is availible
            if (Rates == null || Rates.Length <= 0) return; 

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Rates.Length; i++)
            {
                Rate rate = Rates[i];
                builder.Append(ChartPointInfo.Serialize(rate.ChartData) + "$");//ToStringQuick()
            }

            byte[] bytes = Asmodat.IO.Compression.Zip(builder.ToString());

            lock (Locker.Get("DataDisassembler")) DataDisassembler.Add(path, bytes);
        }


        //----------------------------------------------------------------------------------------------------
    }
}

/*
 public bool SaveAll(bool upToDate)
        {
            upToDate = false;

            if (!this.Loaded) 
                return false;
            //if(upToDate && )

            string[] Pairs = this.Data.Keys.ToArray();
            if (Pairs == null || Pairs.Length <= 0) return false;


            Saving = true;

            for (int p = 0; p < Pairs.Length; p++)
            {
                string pair = Pairs[p];
                ServiceConfiguration.TimeFrame[] Frames = this.Data[pair].Keys.ToArray();
                if (Frames == null) continue;

                for (int f = 0; f < Frames.Length; f++)
                {
                    ServiceConfiguration.TimeFrame frame = Frames[f];
                    this.Save(pair, frame, upToDate);
                    //ThreadsSave.Run(() => this.Save(pair, frame, upToDate), pair + frame, true, true);
                }

                //ThreadsSave.JoinAll();
            }

            //
            Saving = false;
            return true;
        }
public void Save(string pair, ServiceConfiguration.TimeFrame frame, bool upToDate = false)
        {
            //DateTime[] Dates = this.Data[pair][frame].SortedKeys.ToArray();
            Rate[] Rates = this.Data[pair][frame].Values.ToArray();

            if (Rates == null || Rates.Length <= 0) return;

            //Dictionary<string, string> Packets = SplitData(pair, frame, Dates, upToDate);
            //if (Packets.Count <= 0) return;

            string path = this.GetPath(frame, pair);

            string data = "";
            string subData = "";
            for (int i = 0; i < Rates.Length; i++)
            {
                subData = Rates[i].ToStringQuick() + "$";
                if (!System.String.IsNullOrEmpty(subData) && subData.Length > 20)
                    data += subData;
            }


            File.WriteAllText(path, data);

            //Parallel.ForEach(Packets.Keys, file => 
            //{
            //    string path = folder + @"\" + file + FileDataExtention;
            //    File.WriteAllText(path, Packets[file]);
            //});

            //foreach (string file in Packets.Keys)
            //{
            //    path = folder + @"\" + file + FileDataExtention;
            //    File.WriteAllText(path, Packets[file]);
            //}
        }


        /// <summary>
        /// This method splits rates into file name and strig data, ready to save into hard drive.
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        /// <param name="Split"></param>
        /// <param name="Dates"></param>
        /// <returns></returns>
        private Dictionary<string, string> SplitData(string pair, ServiceConfiguration.TimeFrame frame, DateTime[] Dates, bool upToDate = false)
        {
            SplitOptions Split = this.ToSplitOption(frame);
            Dictionary<string, string> Packets = new Dictionary<string, string>();
            DateTime LastDate = DateTime.MinValue;
            DateTime DTUpToDate = this.GetUpToDate(pair, frame);
            string data = "";
            bool bSave = false;
            for (int i = 0; i < Dates.Length; i++)
            {
                DateTime date = Dates[i];

                if (i == 0) LastDate = date;

                if ((date.Year != LastDate.Year && Split == SplitOptions.Yearly) ||//If date changes or dates end's
                  (date.Month != LastDate.Month && Split == SplitOptions.Monthly) ||
                  (date.Day != LastDate.Day && Split == SplitOptions.Daily))
                {
                    if (upToDate && LastDate >= DTUpToDate)
                        bSave = true;

                    Packets.Add(CodeFolderName(Split, LastDate), data);
                    data = "";
                    LastDate = date;

                }

                
               
                data += this.Data[pair][frame][date].ToStringQuick() + "$"; //serialize data
                //data += this.Data[pair][frame][date].ToString() + "$"; //serialize data - old format
            }

            if (upToDate && !bSave)
                data = "";

            if (data != "")
            {
                string FolderName = CodeFolderName(Split, LastDate);
                if (Packets.ContainsKey(FolderName)) Packets[FolderName] += data;
                else Packets.Add(FolderName, data);
            }

            return Packets;
        }

*/