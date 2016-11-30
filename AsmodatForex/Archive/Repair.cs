using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using System.IO;

using Asmodat.IO;

using Microsoft.VisualBasic;

namespace AsmodatForex
{

    public partial class Archive
    {
        public void RepairAll(ref Manager manager)
        {
            Repairing = true;

            //string[] pairs = this.Data.KeysArray;
            

            //foreach (string pair in pairs)
            //{
            //   ServiceConfiguration.TimeFrame[] frames = this.Data[pair].KeysArray;
            //   int decimals = manager.ForexConfiguration.GetDecimals(pair);

            //    foreach(ServiceConfiguration.TimeFrame frame in frames)
            //    {
            //        DateTime[] keys = this.Data[pair][frame].KeysArray;

            //        for (int i = 1; i < keys.Length; i++)
            //        {
            //            Rate previous = this.Data[pair][frame][keys[i - 1]];
            //            Rate current = this.Data[pair][frame][keys[i]];
            //            current.Frame = frame;

            //            this.RepairDecimals(ref previous, ref current, decimals);

            //            this.Data[pair][frame][keys[i]] = current;
            //        }
            //    }
            //}



            Repairing = false;
        }


        private void RepairDecimals(ref Rate previous, ref Rate current, int decimals)
        {
            current.DECIMALS = decimals;

            if (current.Frame == ServiceConfiguration.TimeFrame.LIVE)
            {
                current.BID = this.FixIt(current, previous.BID, current.BID);
                current.OFFER = this.FixIt(current, previous.OFFER, current.OFFER);
            }
            else
            {
                current.HIGH = this.FixIt(current, previous.HIGH, current.HIGH);
                current.LOW = this.FixIt(current, previous.LOW, current.LOW);
                current.OPEN = this.FixIt(current, previous.OPEN, current.OPEN);
                current.CLOSE = this.FixIt(current, previous.CLOSE, current.CLOSE);
            }
        }


        private double FixIt(Rate current_rate, double previous, double current)
        {
            //int ptl = Math.Truncate(previous).ToString().Length;
            //int ctl = Math.Truncate(current).ToString().Length;
            int decimals = current_rate.DECIMALS;
            double pchange = RateInfo.ChangePercentage(previous, current);

            

            if (pchange < 25)//ptl > ctl + 1 || 
            {
                //double parse = Doubles.Parse(value, decimals);

                return current;
            }

            return current;
        }

    }
}

//string value = Interaction.InputBox(
//                "Do you want to fix " + current_rate.Pair +
//                "\n Date: " + current_rate.DateTime.ToLongDateString() + " " + current_rate.DateTime.ToLongTimeString() +
//                "\n Frame: " + current_rate.Frame +
//                "\n Decimals: "  + decimals +
//                "\n previous: " + previous +
//                "\n current: " + current +
//                "\n change: " + pchange + "%"
//                , "FixIt", current.ToString()
//                );