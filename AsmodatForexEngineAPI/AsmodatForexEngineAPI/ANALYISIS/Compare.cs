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

        private void AddPredictionSpecified(List<List<double>> LLDSets, List<List<double>> LDSetToCompare, int iSetId, ref ConcurrentDictionary<int, List<int>> CDILIMatches, List<DateTime> LDTSetTime, double[] IASymiliarities, TimeFrame TFrame)
        {
            List<int> LIMatch = this.PredictSpecified(LLDSets[iSetId], LDSetToCompare[iSetId], LDTSetTime, TFrame, IASymiliarities);
            while (!CDILIMatches.TryAdd(iSetId, LIMatch)) ;
        }

        public ChartPointPredition PredictNextSpecified(string product, List<ChartPoint> LCPoints, List<ChartPoint> LCPointsToCompare, TimeFrame TFrame, int iRound, double[] IASymiliarities)
        {
            List<DateTime> LDTSetTime = (from CP in LCPoints select CP.Time).ToList();
            List<List<double>> LLDSets = new List<List<double>>();
            List<List<double>> LLDSetsToCampare = new List<List<double>>();
            int iDeep = IASymiliarities.Length;
            //int[] IADeeps = new int[4] { iDeep, iDeep, iDeep, iDeep };

            LLDSets.Add((from CP in LCPoints select Math.Round(CP.Raise, iRound)).ToList());
            LLDSets.Add((from CP in LCPoints select Math.Round(CP.Fall, iRound)).ToList());
            LLDSets.Add((from CP in LCPoints select Math.Round(CP.Peak, iRound)).ToList());
            LLDSets.Add((from CP in LCPoints select Math.Round(CP.Base, iRound)).ToList());
            LLDSetsToCampare.Add((from CP in LCPointsToCompare select Math.Round(CP.Raise, iRound)).ToList());
            LLDSetsToCampare.Add((from CP in LCPointsToCompare select Math.Round(CP.Fall, iRound)).ToList());
            LLDSetsToCampare.Add((from CP in LCPointsToCompare select Math.Round(CP.Peak, iRound)).ToList());
            LLDSetsToCampare.Add((from CP in LCPointsToCompare select Math.Round(CP.Base, iRound)).ToList());

            ConcurrentDictionary<int, List<int>> CDILIMatches = new ConcurrentDictionary<int, List<int>>();
            List<Thread> LThread = new List<Thread>();

            /*for (int i = 0; i < LLDSetsToCampare.Count; i++)
                this.AddPredictionSpecified(LLDSets, LLDSetsToCampare, i, ref CDILIMatches, LDTSetTime, IASymiliarities, TFrame);*/

            LThread.Add(new Thread(() => AddPredictionSpecified(LLDSets, LLDSetsToCampare, 0, ref CDILIMatches, LDTSetTime, IASymiliarities, TFrame)));
            LThread.Add(new Thread(() => AddPredictionSpecified(LLDSets,LLDSetsToCampare, 1, ref CDILIMatches, LDTSetTime, IASymiliarities, TFrame)));
            LThread.Add(new Thread(() => AddPredictionSpecified(LLDSets,LLDSetsToCampare, 2, ref CDILIMatches, LDTSetTime, IASymiliarities, TFrame)));
            LThread.Add(new Thread(() => AddPredictionSpecified(LLDSets,LLDSetsToCampare, 3, ref CDILIMatches, LDTSetTime, IASymiliarities, TFrame)));

            foreach (Thread Thrd in LThread)
            {
                Thrd.Priority = ThreadPriority.Highest;
                Thrd.Start();
            }

            foreach (Thread Thrd in LThread)
                Thrd.Join();

            return new ChartPointPredition(CDILIMatches, LLDSets, TFrame, product, LCPointsToCompare[0]);
        }


        private bool ExpressionContinuous(List<DateTime> LDTSTimes, int iPeriod, int iDeep, int iPosition)
        {
            if (LDTSTimes.Count <= iPosition + 1) return false;

            DateTime DTPositinon = LDTSTimes[iPosition];

            if (LDTSTimes[iPosition + 1] != DTPositinon.AddMinutes(iPeriod)) 
                return false;

            for (int i = 1; i < iDeep; i++)
                if (LDTSTimes[iPosition - i].AddMinutes(iPeriod*i) != DTPositinon) 
                    return false;

            return true;
        }


        private bool ExpessionDeppSpecified(List<List<double>> LLDSet, double[] IASymiliarities, int position)
        {

            int iDeep = LLDSet.Count;

            for (int i = 0; i < iDeep; i++)
            {
                if (LLDSet[i].Count <= position - i ||
                    position - i < 0)
                    return false;

                if (LLDSet[i][position - i] < IASymiliarities[i]) return false;
            }

            return true;
        }

        private List<int> PredictSpecified(List<double> LDSet, List<double> LDSetToCompare, List<DateTime> LDTSTimes, TimeFrame TFrame, double[] IASymiliarities)
        {
            List<List<double>> LLDComparision = new List<List<double>>();
            int iPeriod = ABBREVIATIONS.ToMinutes(TFrame);
            int iDeep = LDSetToCompare.Count;

            int iDecimals = 5;
            double iQuantization = Math.Pow(10, -iDecimals);

            List<double> LDSetSorted = LDSet.OrderBy(D => D).ToList();
            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < LDSetSorted.Count; i++)
            {
                double dKey = LDSetSorted[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            double[] DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];


            for (int i = 0; i < iDeep; i++)
            {
                List<double> LDComparision = new List<double>();

                for (int i2 = 0; i2 < LDSet.Count; i2++)
                    LDComparision.Add(this.CompareSorted2(DDIPositions, DAKeys, LDSetToCompare[i], LDSet[i2]));

                LLDComparision.Add(LDComparision);
            }

            List<int> LIMatch = Enumerable.Range(0, LLDComparision[0].Count)
                .Where(i => this.ExpessionDeppSpecified(LLDComparision, IASymiliarities, i))
                .ToList();

            return Enumerable.Range(0, LIMatch.Count)
                .Where(i => this.ExpressionContinuous(LDTSTimes, iPeriod, iDeep, LIMatch[i]))
                .Select(i => LIMatch[i])
                .ToList();
        }


        

        public double CompareSorted2(Dictionary<double, int> DDIPositions, double[] DAKeys, double dV1, double dV2)
        {
            if (dV1 == dV2) return 100;

            double dRange = Math.Abs(dV1 - dV2);
            int iMinIndex = this.BinarySearchBest2(DAKeys, (dV1 - dRange));
            int iMaxIndex = this.BinarySearchBest2(DAKeys, (dV1 + dRange));

            if (iMaxIndex < iMinIndex)
                return 0;

            return (1 - ((double)(DDIPositions[DAKeys[iMaxIndex]] - DDIPositions[DAKeys[iMinIndex]]) / DDIPositions[DAKeys.Last()])) * 100;
        }


        public double CompareSortedQuick(Dictionary<double, int> DDIPositions, int setCount, double dV1, double dV2, int round)
        {
            if (dV1 == dV2) return 100;

            double dRange = Math.Abs(dV1 - dV2);
            double dMin = Math.Round(dV1 - dRange, round);
            double dMax = Math.Round(dV1 + dRange, round);

            if (dMin < dMax)
                throw new Exception("Something went wrong !");

            return (1 - ((double)(DDIPositions[dMax] - DDIPositions[dMin]) / setCount)) * 100;
        }

        public double CompareSortedQuick2(Dictionary<double, int> DDIPositions, int setCount, double dV1, double dV2)
        {
           

            return (1 - ((double)(Math.Abs(DDIPositions[dV1] - DDIPositions[dV2])) / setCount)) * 100;
        }

     
    }
}


/*
public double CompareSorted(Dictionary<double, int> DDIPositions, double[] DAKeys, double dV1, double dV2)
        {
            if (dV1 == dV2) return 100;

            double dRange = Math.Abs(dV1 - dV2);
            int iMinIndex = this.BinarySearchBest(DAKeys, (dV1 - dRange));
            int iMaxIndex = this.BinarySearchBest(DAKeys, (dV1 + dRange));

            if (iMaxIndex < iMinIndex) 
                return 0;

            int iLeftBorder;
            int iRightBorder;

            if (iMinIndex == -1)
                iLeftBorder = 0;
            //else if (iMinIndex == -2) iLeftBorder = DDIPositions[DAKeys[iMinIndex]];
            else iLeftBorder = DDIPositions[DAKeys[iMinIndex]];

            if (iMaxIndex == -2)
                iRightBorder = DDIPositions[DAKeys.Last()];
            else iRightBorder = DDIPositions[DAKeys[iMaxIndex]];


            return (1 - ((double)(iRightBorder - iLeftBorder) / DDIPositions[DAKeys.Last()])) * 100;
        }
*/