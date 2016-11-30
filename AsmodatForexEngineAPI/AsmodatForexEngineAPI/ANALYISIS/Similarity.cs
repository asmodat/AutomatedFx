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
        /// Counts amount of evry value that appears in set and sums it up for evry index. 
        /// </summary>
        /// <param name="LDSet">Set of all values.</param>
        /// <returns>Dictionary of value and its sum count.</returns>
        public Dictionary<double, int> Positions(double[] DASet, out double[] DAKeys)
        {
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < DASet.Length; i++)
            {
                double dKey = DASet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];

            return DDIPositions;
        }
        public Dictionary<double, int> PositionsQuick(double[] DASet, double[] DASubSet)
        {
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < DASet.Length; i++)
            {
                double dKey = DASet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            for (int i = 0; i < DASubSet.Length; i++)
            {
                double dKey = DASubSet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 0);
            }

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            double[] DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];

            return DDIPositions;
        }
        public Dictionary<double, int> PositionsQuick2(double[] DASet, double[] DASubSet)
        {
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < DASet.Length; i++)
            {
                double dKey = DASet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            for (int i = 0; i < DASubSet.Length; i++)
            {
                double dKey = DASubSet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 0);
            }

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            double[] DAKeys = DDIPositions.Keys.ToArray();

            for (int i = 0; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] = i;

            return DDIPositions;
        }
        public Dictionary<double, int> PositionsQuick3(double[] DASet, double[] DASubSet)
        {
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < DASet.Length; i++)
            {
                double dKey = DASet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            for (int i = 0; i < DASubSet.Length; i++)
            {
                double dKey = DASubSet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 0);
            }

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            double[] DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];
            

            int iValue, iNValue;
            for (int i = 0; i < DAKeys.Length; i++)
            {
                iValue = DDIPositions[DAKeys[i]]; //divide vy 2 becouse of using split values
                iNValue = iValue / 2;
                if (iNValue <= 0)
                    DDIPositions[DAKeys[i]] = 1;
                else DDIPositions[DAKeys[i]] = iNValue;
            }

            return DDIPositions;
        }

        public Dictionary<double, int> PositionsSlow(double[] DASet, double[] DASubSet, int round, out double[] DAKeys)
        {
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < DASet.Length; i++)
            {
                double dKey = DASet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];


                for (int i2 = 0; i2 < DASubSet.Length; i2++)
                {
                    double dKey2 = DASubSet[2];

                    double dRange = Math.Abs(dKey2 - dKey);
                    double dMin = Math.Round(dKey2 - dRange, round);
                    double dMax = Math.Round(dKey2 + dRange, round);


                    
                    if (!DDIPositions.ContainsKey(dMin))
                        DDIPositions.Add(dMin, 0);

                    if (!DDIPositions.ContainsKey(dMax))
                        DDIPositions.Add(dMax, 0);
                }
            }

            

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];

            return DDIPositions;
        }



        public double[] GetColumn(List<double[]> LDASAll, int columnID)
        {
            int iCount = LDASAll.Count;
            double[] LDSet = new double[iCount];
            for(int i = 0; i < iCount; i++)
                LDSet[i] = LDASAll[i][columnID];

            return LDSet;
        }

        public double SimilarityFactor(List<double[]> LDASAll, double[] DAOrigin)
        {
            double dFactor = 0;
            int iDeep = DAOrigin.Length;
            int iCount = LDASAll.Count;

            

            double[][] DA2Similarities = new double[iDeep][];

            List<int> LIExceptions = new List<int>();
            for (int i = 0; i < iDeep; i++)
            {
                DA2Similarities[i] = new double[iCount];
                double[] DASet = this.GetColumn(LDASAll, i);
                double[] DAKeys = null;
                Dictionary<double, int> DDIPositions = this.Positions(DASet, out DAKeys);

                double dKey = double.MinValue;
                double dMaxPercentage = double.MinValue;
                for (int i2 = 0; i2 < iCount; i2++)
                {
                    DA2Similarities[i][i2] = this.CompareSorted2(DDIPositions, DAKeys, DAOrigin[i], DASet[i2]);
                    if(DA2Similarities[i][i2] > dMaxPercentage)
                    {
                        dMaxPercentage = DA2Similarities[i][i2];
                        dKey = DASet[i2];
                    }
                    else if (dMaxPercentage == 100) break;
                }

                int iCountSet = DASet.Count();
                int iCountKeys = this.CountKeys(DASet, dKey);

                dFactor += ((double)dMaxPercentage / 100) * ((double)iCountKeys / iCountSet);
            }


            return dFactor;
        }

        




        private int CountKeys(double[] DASet, double key)
        {
            int iCntr = 0;
            foreach (double d in DASet)
                if (d == key) ++iCntr;

            return iCntr;
        }

    }
}
