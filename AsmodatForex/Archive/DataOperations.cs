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
        private enum SplitOptions
        {
            Daily = 1,
            Monthly = 3,
            Yearly = 4,
            AllTimes = 5
        }

        private SplitOptions ToSplitOption(ServiceConfiguration.TimeFrame frame)
        {
            if (frame <= ServiceConfiguration.TimeFrame.LIVE)
            {
                return SplitOptions.Daily;
            }
            else if (frame <= ServiceConfiguration.TimeFrame.THIRTY_MINUTE)
            {
                return SplitOptions.Monthly;
            }
            else if (frame <= ServiceConfiguration.TimeFrame.FOUR_HOUR)
            {
                return SplitOptions.Yearly;
            }
            else if (frame > ServiceConfiguration.TimeFrame.FOUR_HOUR)
            {
                return SplitOptions.AllTimes;
            }


            throw new Exception("Unknown frame option");
        }


        private string CodeFolderName(SplitOptions Split, DateTime date)
        {
            if (Split == SplitOptions.Yearly)
            {
                return date.Year + "";
            }
            else if (Split == SplitOptions.Monthly)
            {
                return date.Year + "-" + date.Month;
            }
            else if (Split == SplitOptions.Daily)
            {
                return date.Year + "-" + date.Month + "-" + date.Day;
            }
            else if (Split == SplitOptions.AllTimes)
            {
                return "AllTimesData";
            }

            return null;
        }



        /// <summary>
        /// Returns folder localisation for specified currency pair and time frame
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private string GetFolder(ServiceConfiguration.TimeFrame frame)
        {
            string FolderFrame = DirectoryData + @"\" + frame;

            if (!Directory.Exists(FolderFrame))
                Directory.CreateDirectory(FolderFrame);

            return FolderFrame;
        }

        private string GetPath(ServiceConfiguration.TimeFrame frame, string pair)
        {
            string FolderFrame = DirectoryData + @"\" + frame;
            string path = FolderFrame + @"\" + pair.Replace("/", "-") + FileDataExtention;
            if (!Directory.Exists(FolderFrame))
                Directory.CreateDirectory(FolderFrame);

            if (!File.Exists(path))
                File.CreateText(path).Close();



            return path;
        }

    }
}
