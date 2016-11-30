using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public partial class Analysis
    {
        /// <summary>
        /// Allow to quick search best value in sorted double Array Set,
        /// </summary>
        /// <param name="DAKeys">Set of sorted values to search</param>
        /// <param name="value">Value to search</param>
        /// <returns>Index in Set of best match to value, -1 if below index 0, or -2 if abow Set Length</returns>
        

        public int BinarySearchBest2(double[] DASet, double value)
        {
            int iMinIndex = 0;
            int iMaxIndex = DASet.Length - 1;
            int iMiddleIndex = 0;
            int iCompare;

            if (value >= DASet[iMaxIndex]) return iMaxIndex;
            if (value <= DASet[iMinIndex]) return iMinIndex;

            while (iMinIndex < iMaxIndex)
            {
                iMiddleIndex = (iMaxIndex + iMinIndex) / 2;
                iCompare = value.CompareTo(DASet[iMiddleIndex]);

                if (iCompare < 0)
                    iMaxIndex = iMiddleIndex - 1;
                else if (iCompare > 0)
                    iMinIndex = iMiddleIndex + 1;
                else return iMiddleIndex;

            }

            return iMiddleIndex;
        }

    }
}

/*

public int BinarySearchBest(double[] DASet, double value)
        {
            int iMinIndex = 0;
            int iMaxIndex = DASet.Length;
            int iMiddleIndex;
            int iCompare;

            while(iMinIndex < iMaxIndex)
            {
                iMiddleIndex = (iMaxIndex + iMinIndex) / 2;
                iCompare = value.CompareTo(DASet[iMiddleIndex]);

                if (iCompare < 0)
                    iMaxIndex = iMiddleIndex - 1;
                else if (iCompare > 0)
                    iMinIndex = iMiddleIndex + 1;
                else return iMiddleIndex;
                
            }

            if (iMinIndex >= DASet.Length) 
                return -2;
            else if (iMaxIndex < 0)
                return -1;

            if (iMinIndex == iMaxIndex)
                return iMinIndex;
            else if (iMinIndex > iMaxIndex)
                return iMaxIndex;


            throw new Exception("Unexpected search result :/ !");
        }*/