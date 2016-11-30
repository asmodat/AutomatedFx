using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public partial class Analysis
    {
        private Abbreviations ABBREVIATIONS = new Abbreviations();
        private Archive ARCHIVE;
        private Account ACCOUNT;
        
        public Analysis(ref Archive ARCHIVE, ref Account ACCOUNT)
        {
            this.ARCHIVE = ARCHIVE;
            this.ACCOUNT = ACCOUNT;
        }
        public Analysis()
        {
        }


        public double Product(List<double> LDouble)
        {
            if (LDouble.Count == 0) return 0;

            double dProduct = 1;

            for (int i = 0; i < LDouble.Count; i++)
                dProduct *= LDouble[i];

            return dProduct;
        }

        public double Average(List<double> LDouble)
        {
            double sum = 0;
            foreach (double D in LDouble)
                sum += D;

            return sum / LDouble.Count;
        }
        public double StandardDeviation(List<double> LDouble, double dAverange)
        {
            double dMeanSquareSum = 0;
            foreach (Double D in LDouble)
                dMeanSquareSum += Math.Pow(D - dAverange, 2);

            return Math.Pow(dMeanSquareSum / (LDouble.Count - 1), 0.5);
        }
        public double Percentage(List<double> LDouble, double dAverange, double dError)
        {
            double dCounterInRange = 0;
            double dCounterSamples = LDouble.Count;
            double max = dAverange + dError;
            double min = dAverange - dError;
            foreach (double D in LDouble)
            {

                if (D <= max && D >= min)
                    ++dCounterInRange;
            }

            return (dCounterInRange / dCounterSamples) * 100;
        }

        public double ExpectedValue(List<double> LDSet)
        {
            Dictionary<double, int> DDISet = new Dictionary<double, int>();

            foreach(double dElement in LDSet)
            {
                if (DDISet.ContainsKey(dElement))
                    ++DDISet[dElement];
                else DDISet.Add(dElement, 1);
            }

            double dExpectedValue = 0;
            int iLDSCount = LDSet.Count;
            foreach(var vPair in DDISet)
            {
                double dPropability = (double)vPair.Value / iLDSCount;
                dExpectedValue += dPropability * vPair.Key;
            }

            return dExpectedValue;
        }

        /*
         public void ExactDeviation(List<double> LDSet, double dPercentage)
        {
            double dPError = 0.5;
            double dPCurrent = 0;
            int iMaxSteps = 100;

            double dMin = LDSet.Min();
            double dMax = LDSet.Max();
            double dCurrentError = (Math.Abs(dMax) + Math.Abs(dMin)) / 2;
            double dCurrentValue = (dMax + dMin) / 2;
            do
            {
                    




                --iMaxSteps;
            } while (dPCurrent < (dPercentage - dPError) || dPCurrent > (dPercentage + dPError) || iMaxSteps <= 0);
        }
         */

        public bool InRange(double value, double avereage, double deviation)
        {
            if (value <= avereage + deviation && value >= avereage - deviation) 
                return true;
            else return false;
        }

        public bool InRangePercentage(double value1, double value2, double similarity)
        {
            double dSDifference = 100 - similarity;

            double dRSimilarity = (value1 / value2) * 100;

            if (dRSimilarity < 100 - dSDifference || dRSimilarity > 100 + dSDifference)
                return false;
            
            return true;
        }



        public void DystrybutionRanges(List<double> LDSet, double percentage)
        {
            double min = LDSet.Min();
            double max = LDSet.Max();
            // 100% Data of Set between min and max range.

            List<double> LDRemoved = new List<double>();
            int iSetCount = LDSet.Count;
            double dValuesToRemove = iSetCount * (1 - (percentage / 100));

            SortedDictionary<double, int> DDICounter = new SortedDictionary<double, int>();

            foreach(double dElement in LDSet)
            {
                if (!DDICounter.ContainsKey(dElement))
                    DDICounter.Add(dElement, 1);
                else ++DDICounter[dElement];
            }
        }



        


    }
}
