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
        public double Compare(List<double> LDSet, List<double> LDSetToCompare, int position)
        {
            int iDeep = LDSetToCompare.Count;
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

            List<double> LDComparision = new List<double>();
            for (int i = 0; i < iDeep; i++)
            {
                LDComparision.Add(this.CompareSorted2(DDIPositions, DAKeys, LDSetToCompare[i], LDSet[position + i]));
            }

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += LDComparision[i];

            return dSum / iDeep;
        }





        

        public ChartPointsPredition PredictNextSpecified(string product, List<ChartPoint> LCPoints, List<ChartPoint> LCPointsToCompare, TimeFrame TFrame, int iRound, int iMinMatches, double dMinResolution, int ahead, double[] DAWeightFactors)
        {
            List<DateTime> LDTSetTime = (from CP in LCPoints select CP.Time).ToList();
            double[][] DJSets = new double[5][];

            int iDeep = LCPointsToCompare.Count;
            int iSetCount = LCPoints.Count;

            LCPointsToCompare = LCPointsToCompare.OrderByDescending(CP => CP.Time).ToList();

            for (int id = 0; id < DJSets.Length; id++)
            {
                DJSets[id] = new double[iSetCount];

                for (int i = 0; i < LCPoints.Count; i++)
                    DJSets[id][i] = LCPoints[i].GetParam(id, iRound);
                
            }

            //DateTime DTStart = DateTime.Now;
            ConcurrentDictionary<int, List<double>> CDILDMatches = new ConcurrentDictionary<int, List<double>>();
            List<Thread> LThread = new List<Thread>();
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[0], LCPointsToCompare, 0, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
          /*  LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[1], LCPointsToCompare, 1, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[2], LCPointsToCompare, 2, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[3], LCPointsToCompare, 3, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            */
            foreach (Thread Thrd in LThread)  {  Thrd.Priority = ThreadPriority.Lowest; Thrd.Start(); }
            foreach (Thread Thrd in LThread) Thrd.Join();


            int iCDILDMCount = CDILDMatches[0].Count;
            List<int> LISMatches = new List<int>();
            List<double> LDSimilarities = new List<double>();

                double dSimil = 50;


                List<int> LIMatches = new List<int>();
                for (int i = 0; i < iCDILDMCount; i++)
                {
                    double dSimilarity = Math.Min(CDILDMatches[0][i], CDILDMatches[1][i]);//(CDILDMatches[0][i] * CDILDMatches[1][i] * CDILDMatches[2][i] * CDILDMatches[3][i]) / Math.Pow(100,4-1);
                    dSimilarity = Math.Min(dSimilarity, CDILDMatches[2][i]);
                    dSimilarity = Math.Min(dSimilarity, CDILDMatches[3][i]);
                    if (dSimilarity > dSimil)// && this.CompareTendency(LCPoints, LCPointsToCompare, i, dSimil))
                    {
                        LIMatches.Add(i);
                        LDSimilarities.Add(dSimilarity);
                        i += iDeep - 1;
                    }

                }





                // DateTime DTStop = DateTime.Now;  double dMS = (DTStop - DTStart).TotalMilliseconds; double dDisplay = 1 + dMS;//return new ChartPointsPredition(LISMatches, DJSets, TFrame, product, LCPointsToCompare[0], iDeep, ahead, LDSimilarities);
                return new ChartPointsPredition(LIMatches, DJSets, TFrame, product, LCPointsToCompare[0], iDeep, ahead, LDSimilarities);
        }

        //TODO: evry deep step involves greater change and error - don't compare separate values - you will not need to compare tendency


        private bool CompareTendency(List<ChartPoint> LCPSet, List<ChartPoint> LCPOrigin, int position, double similarity)
        {
            int deep = LCPOrigin.Count;
            List<ChartPoint> LCPPosition = LCPSet.GetRange(position - deep, deep);

            double dTOHigh = this.Tendency(LCPOrigin, true);
            double dTOLow = this.Tendency(LCPOrigin, false);

            double dTPHigh = this.Tendency(LCPPosition, true);
            double dTPLow = this.Tendency(LCPPosition, false);

            if (dTOHigh == 50 || dTOLow == 50 || dTPHigh == 50 || dTPLow == 50 ||
                dTOHigh == double.NaN || dTOLow == double.NaN || dTPHigh == double.NaN || dTPLow == double.NaN)
                return false;

            double dSmimilarity = similarity;// ((double)(similarity + 100) / 2);

            bool bInRangeHigh = this.InRangePercentage(dTOHigh, dTPHigh, dSmimilarity);
            bool bInRangeLow = this.InRangePercentage(dTOLow, dTPLow, dSmimilarity);

            if (bInRangeHigh && bInRangeLow)
                return true;

            return false;
        }


        private void AddPredictionSpecified( double[] LDSet, List<ChartPoint> LCPointsToCompare, int setID, ref ConcurrentDictionary<int, List<double>> CDILDMatches, List<DateTime> LDTSTimes, TimeFrame TFrame, int ahead, double[] DAWeightFactors, int iRound)
        {
            

            int iDeep = LCPointsToCompare.Count;
            double[] DASubSet = new double[iDeep];
            for (int i = 0; i < iDeep; i++)
                DASubSet[i] = DASubSet[i] = LCPointsToCompare[i].GetParam(setID, iRound);//LCPointsToCompare[i].GetParamQuick(setID);//

            Dictionary<double, int> DDIPositions = this.PositionsQuick(LDSet, DASubSet);
            int iDDIPCount = DDIPositions.Keys.Count;

            int iPeriod = ABBREVIATIONS.ToMinutes(TFrame);
            int iSubSetStrength = DDIPositions[DASubSet.Max()] - DDIPositions[DASubSet.Min()];


            double[][] DJComparision = new double[iDeep][];
            int dComparisionCount = (LDSet.Length - iDeep);
            for (int i = 0; i < iDeep; i++)
            {
                double[] DAComparision = new double[dComparisionCount];//(1 - ((double)(Math.Abs(DDIPositions[DASubSet[i]] - DDIPositions[LDSet[i2]])) / iDDIPCount)) * 100;

                for (int i2 = 0; i2 < dComparisionCount; i2++)
                    DAComparision[i2] = (1 - ((double)(Math.Abs(DDIPositions[DASubSet[i]] - DDIPositions[LDSet[i2]])) / iSubSetStrength)) * 100;// this.CompareSortedQuick(DDIPositions, iCount, DASubSet[i], LDSet[i2], iRound);// DAComparision[i2] = this.CompareSorted2(DDIPositions, DAKeys, DASubSet[i], LDSet[i2]);

                DJComparision[i] = DAComparision;
            }

            List<double> LDMatch = new List<double>();
            for (int i = 0; i < dComparisionCount; i++)
                if (this.ExpressionContinuous(LDTSTimes, iPeriod, iDeep, ahead, i))
                    LDMatch.Add(this.ExpessionDeppSimilarityQuick(DJComparision, i, setID));//ExpessionDeppSimilarityQuickMultiply
                else LDMatch.Add(0);
            //


            while (!CDILDMatches.TryAdd(setID, LDMatch)) ;
        }


        private bool ExpressionContinuous(List<DateTime> LDTSTimes, int iPeriod, int iDeep, int iAhead, int iPosition)
        {
            if (LDTSTimes.Count <= (iPosition + iAhead) || (iPosition - iDeep) < 0) return false;

            DateTime DTPositinonStart = LDTSTimes[iPosition - iDeep];
            DateTime DTPositionEnd = LDTSTimes[iPosition + iAhead];

            //if (DTPositinonStart.Date != DTPositionEnd.Date)
            //    return false;

            if (DTPositinonStart.AddMinutes((iAhead + iDeep) * iPeriod) == DTPositionEnd)
                return true;
            else return false;
        }

        private bool ExpessionDeppSpecifiedQuick(double[][] DJSet, double similarity, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            for (int i = 0; i < iDeep; i++)
                if (DJSet[i][position - i] < similarity)
                    return false;

            return true;
        }

        private bool ExpessionDeppSpecifiedQuick2(double[][] DJSet, double similarity, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += DJSet[i][position - i];

            if ((double)dSum / iDeep < similarity)
                return false;

            return true;
        }
        private double ExpessionDeppSimilarityQuick(double[][] DJSet, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return 0;

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += DJSet[i][position - i];

            return (double)dSum / iDeep;
        }
        private double ExpessionDeppSimilarityQuickMultiply(double[][] DJSet, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return 0;

            double dMul = 1;
            for (int i = 0; i < iDeep; i++)
                dMul *= DJSet[i][position - i];

            double dResolution = dMul / Math.Pow(100, iDeep -1);

            return dResolution;
        }



        private bool ExpessionDeppSpecified(double[][] DJSet, double symilarity, int position, double[] DAWeightFactors, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            double dMatches = 0;
            double dSymilarity = 0;
            double dWeightSum = 0;
            double dS21 = symilarity / 2 + 1;
            double dWCurrent = 0;
            double dPSymilarity = 0;
            for (int i = 0; i < iDeep; i++)
            {
                dPSymilarity = DJSet[i][position - i];

                

                dWCurrent = 1;
                if (DAWeightFactors[setID] > 0)
                    dWCurrent = Math.Pow((iDeep - i), DAWeightFactors[setID]);
                else if (DAWeightFactors[setID] < 0)
                    dWCurrent = Math.Pow(i + 1, Math.Abs(DAWeightFactors[setID]));

                dSymilarity += dPSymilarity * dWCurrent;

                if (dPSymilarity >= dS21)
                    dMatches += dWCurrent;

                dWeightSum += dWCurrent;


            }

            double dAverange = dSymilarity / dWeightSum;
            double dAverangeMatch = dMatches / dWeightSum;

            if (dAverangeMatch * dAverange >= symilarity)
                return true;
            else return false;
        }

    
    }
}

/*
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
        public double Compare(List<double> LDSet, List<double> LDSetToCompare, int position)
        {
            int iDeep = LDSetToCompare.Count;
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

            List<double> LDComparision = new List<double>();
            for (int i = 0; i < iDeep; i++)
            {
                LDComparision.Add(this.CompareSorted2(DDIPositions, DAKeys, LDSetToCompare[i], LDSet[position + i]));
            }

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += LDComparision[i];

            return dSum / iDeep;
        }





        

        public ChartPointsPredition PredictNextSpecified(string product, List<ChartPoint> LCPoints, List<ChartPoint> LCPointsToCompare, TimeFrame TFrame, int iRound, int iMinMatches, double dMinResolution, int ahead, double[] DAWeightFactors)
        {
            List<DateTime> LDTSetTime = (from CP in LCPoints select CP.Time).ToList();
            double[][] DJSets = new double[5][];

            int iDeep = LCPointsToCompare.Count;
            int iSetCount = LCPoints.Count;

            LCPointsToCompare = LCPointsToCompare.OrderByDescending(CP => CP.Time).ToList();

            for (int id = 0; id < DJSets.Length; id++)
            {
                DJSets[id] = new double[iSetCount];

                for (int i = 0; i < LCPoints.Count; i++)
                    DJSets[id][i] = LCPoints[i].GetParam(id, iRound);
                
            }

            //DateTime DTStart = DateTime.Now;
            ConcurrentDictionary<int, List<double>> CDILDMatches = new ConcurrentDictionary<int, List<double>>();
            List<Thread> LThread = new List<Thread>();
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[0], LCPointsToCompare, 0, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[1], LCPointsToCompare, 1, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[2], LCPointsToCompare, 2, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));
            LThread.Add(new Thread(() => AddPredictionSpecified(DJSets[3], LCPointsToCompare, 3, ref CDILDMatches, LDTSetTime, TFrame, ahead, DAWeightFactors, iRound)));

            foreach (Thread Thrd in LThread)  {  Thrd.Priority = ThreadPriority.Lowest; Thrd.Start(); }
            foreach (Thread Thrd in LThread) Thrd.Join();


            int iCDILDMCount = CDILDMatches[0].Count;
            List<int> LISMatches = new List<int>();
            double dSimilMax = 100;
            double diymilMin = 0;
            double dResolution = double.MaxValue;
            double dSimilarity = 0;

            do
            {
                double dSimil = (dSimilMax + diymilMin) / 2;


                List<int> LIMatches = new List<int>();
                for(int i = 0; i < iCDILDMCount; i++)
                {
                    if (CDILDMatches[0][i] >= dSimil &&
                        CDILDMatches[1][i] >= dSimil &&
                        CDILDMatches[2][i] >= dSimil &&
                        CDILDMatches[3][i] >= dSimil)// && this.CompareTendency(LCPoints, LCPointsToCompare, i, dSimil))
                    {
                        LIMatches.Add(i);
                        dSimilarity = dSimil;
                        i += iDeep - 1;
                    }

                }

                int iLIMCount = LIMatches.Count;

                if (iLIMCount >= iMinMatches)
                {
                    LISMatches = LIMatches;
                    //CPsPredictionNow.Similarity = dSimil;
                }

                if (iLIMCount < iMinMatches)
                    dSimilMax = dSimil;
                else
                    diymilMin = dSimil;

                dResolution = dSimilMax - diymilMin;

            } while (dResolution >= dMinResolution);







           // DateTime DTStop = DateTime.Now;  double dMS = (DTStop - DTStart).TotalMilliseconds; double dDisplay = 1 + dMS;
            return new ChartPointsPredition(LISMatches, DJSets, TFrame, product, LCPointsToCompare[0], iDeep, ahead, dSimilarity);
        }

        //TODO: evry deep step involves greater change and error - don't compare separate values - you will not need to compare tendency


        private bool CompareTendency(List<ChartPoint> LCPSet, List<ChartPoint> LCPOrigin, int position, double similarity)
        {
            int deep = LCPOrigin.Count;
            List<ChartPoint> LCPPosition = LCPSet.GetRange(position - deep, deep);

            double dTOHigh = this.Tendency(LCPOrigin, true);
            double dTOLow = this.Tendency(LCPOrigin, false);

            double dTPHigh = this.Tendency(LCPPosition, true);
            double dTPLow = this.Tendency(LCPPosition, false);

            if (dTOHigh == 50 || dTOLow == 50 || dTPHigh == 50 || dTPLow == 50 ||
                dTOHigh == double.NaN || dTOLow == double.NaN || dTPHigh == double.NaN || dTPLow == double.NaN)
                return false;

            double dSmimilarity = similarity;// ((double)(similarity + 100) / 2);

            bool bInRangeHigh = this.InRangePercentage(dTOHigh, dTPHigh, dSmimilarity);
            bool bInRangeLow = this.InRangePercentage(dTOLow, dTPLow, dSmimilarity);

            if (bInRangeHigh && bInRangeLow)
                return true;

            return false;
        }


        private void AddPredictionSpecified( double[] LDSet, List<ChartPoint> LCPointsToCompare, int setID, ref ConcurrentDictionary<int, List<double>> CDILDMatches, List<DateTime> LDTSTimes, TimeFrame TFrame, int ahead, double[] DAWeightFactors, int iRound)
        {
            

            int iDeep = LCPointsToCompare.Count;
            double[] DASubSet = new double[iDeep];
            for (int i = 0; i < iDeep; i++)
                DASubSet[i] = DASubSet[i] = LCPointsToCompare[i].GetParam(setID, iRound);//LCPointsToCompare[i].GetParamQuick(setID);//

            Dictionary<double, int> DDIPositions = this.PositionsQuick(LDSet, DASubSet);
            int iDDIPCount = DDIPositions.Keys.Count;

            int iPeriod = ABBREVIATIONS.ToMinutes(TFrame);
            int iSetLength = LDSet.Length;



            double[][] DJComparision = new double[iDeep][];
            int dComparisionCount = (iSetLength - iDeep);
            for (int i = 0; i < iDeep; i++)
            {
                double[] DAComparision = new double[dComparisionCount];//(1 - ((double)(Math.Abs(DDIPositions[DASubSet[i]] - DDIPositions[LDSet[i2]])) / iDDIPCount)) * 100;

                for (int i2 = 0; i2 < dComparisionCount; i2++)
                    DAComparision[i2] = (1 - ((double)(Math.Abs(DDIPositions[DASubSet[i]] - DDIPositions[LDSet[i2]])) / iSetLength)) * 100;// this.CompareSortedQuick(DDIPositions, iCount, DASubSet[i], LDSet[i2], iRound);// DAComparision[i2] = this.CompareSorted2(DDIPositions, DAKeys, DASubSet[i], LDSet[i2]);

                DJComparision[i] = DAComparision;
            }

            List<double> LDMatch = new List<double>();
            for (int i = 0; i < dComparisionCount; i++)
                if (this.ExpressionContinuous(LDTSTimes, iPeriod, iDeep, ahead, i))
                    LDMatch.Add(this.ExpessionDeppSimilarityQuickMultiply(DJComparision, i, setID));
                else LDMatch.Add(0);



            while (!CDILDMatches.TryAdd(setID, LDMatch)) ;
        }


        private bool ExpressionContinuous(List<DateTime> LDTSTimes, int iPeriod, int iDeep, int iAhead, int iPosition)
        {
            if (LDTSTimes.Count <= (iPosition + iAhead) || (iPosition - iDeep) < 0) return false;

            DateTime DTPositinonStart = LDTSTimes[iPosition - iDeep];
            DateTime DTPositionEnd = LDTSTimes[iPosition + iAhead];

            //if (DTPositinonStart.Date != DTPositionEnd.Date)
            //    return false;

            if (DTPositinonStart.AddMinutes((iAhead + iDeep) * iPeriod) == DTPositionEnd)
                return true;
            else return false;
        }

        private bool ExpessionDeppSpecifiedQuick(double[][] DJSet, double similarity, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            for (int i = 0; i < iDeep; i++)
                if (DJSet[i][position - i] < similarity)
                    return false;

            return true;
        }

        private bool ExpessionDeppSpecifiedQuick2(double[][] DJSet, double similarity, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += DJSet[i][position - i];

            if ((double)dSum / iDeep < similarity)
                return false;

            return true;
        }
        private double ExpessionDeppSimilarityQuick(double[][] DJSet, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return 0;

            double dSum = 0;
            for (int i = 0; i < iDeep; i++)
                dSum += DJSet[i][position - i];

            return (double)dSum / iDeep;
        }
        private double ExpessionDeppSimilarityQuickMultiply(double[][] DJSet, int position, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return 0;

            double dMul = 1;
            for (int i = 0; i < iDeep; i++)
                dMul *= DJSet[i][position - i];

            double dResolution = dMul / Math.Pow(100, iDeep -1);

            return dResolution;
        }



        private bool ExpessionDeppSpecified(double[][] DJSet, double symilarity, int position, double[] DAWeightFactors, int setID)
        {
            int iDeep = DJSet.Length;

            if (position - iDeep < 0)
                return false;

            double dMatches = 0;
            double dSymilarity = 0;
            double dWeightSum = 0;
            double dS21 = symilarity / 2 + 1;
            double dWCurrent = 0;
            double dPSymilarity = 0;
            for (int i = 0; i < iDeep; i++)
            {
                dPSymilarity = DJSet[i][position - i];

                

                dWCurrent = 1;
                if (DAWeightFactors[setID] > 0)
                    dWCurrent = Math.Pow((iDeep - i), DAWeightFactors[setID]);
                else if (DAWeightFactors[setID] < 0)
                    dWCurrent = Math.Pow(i + 1, Math.Abs(DAWeightFactors[setID]));

                dSymilarity += dPSymilarity * dWCurrent;

                if (dPSymilarity >= dS21)
                    dMatches += dWCurrent;

                dWeightSum += dWCurrent;


            }

            double dAverange = dSymilarity / dWeightSum;
            double dAverangeMatch = dMatches / dWeightSum;

            if (dAverangeMatch * dAverange >= symilarity)
                return true;
            else return false;
        }

    
    }
}

/*
private List<int> PredictSpecified(List<double> LDSet, List<double> LDSetToCompare, List<DateTime> LDTSTimes, TimeFrame TFrame, double symilarity, int ahead, double[] DAWeightFactors, int setID)
        {
            List<List<double>> LLDComparision = new List<List<double>>();
            int iPeriod = ABBREVIATIONS.ToMinutes(TFrame);
            int iDeep = LDSetToCompare.Count;


            Dictionary<double, int> DDIPositions = new Dictionary<double, int>();
            for (int i = 0; i < LDSet.Count; i++)
            {
                double dKey = LDSet[i];
                if (!DDIPositions.ContainsKey(dKey))
                    DDIPositions.Add(dKey, 1);
                else ++DDIPositions[dKey];
            }

            DDIPositions = DDIPositions.OrderBy(v => v.Key).ToDictionary(d => d.Key, i => i.Value);

            double[] DAKeys = DDIPositions.Keys.ToArray();
            for (int i = 1; i < DAKeys.Length; i++)
                DDIPositions[DAKeys[i]] += DDIPositions[DAKeys[i - 1]];


            

            for (int i = 0; i < iDeep; i++)
            {
                List<double> LDComparision = new List<double>();

                for (int i2 = 0; i2 < (LDSet.Count - iDeep); i2++)
                    LDComparision.Add(this.CompareSorted(DDIPositions, DAKeys, LDSetToCompare[i], LDSet[i2]));

                LLDComparision.Add(LDComparision);
            }

            List<int> LIMatch = new List<int>();
            for(int i = 0; i < LLDComparision[0].Count; i++)
            {
                if (this.ExpressionContinuous(LDTSTimes, iPeriod, iDeep, ahead, i) &&
                    this.ExpessionDeppSpecified(LLDComparision, symilarity, i, DAWeightFactors, setID))
                    LIMatch.Add(i);
            }



            return LIMatch;
        }
*/
