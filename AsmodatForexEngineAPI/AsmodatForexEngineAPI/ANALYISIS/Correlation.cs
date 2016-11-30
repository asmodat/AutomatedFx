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

        public bool AlternateProduct(OpenRatesBlotter ORBlotter, TimeFrame TFrame, int count, string product, ref int shift, ref int shiftAlternative, int maxShift, ref string alternative, ref string counterAlternative)
        {
            List<string> LSProd = ARCHIVE.GetProducts();
            int iLCPCount = ARCHIVE.GetDATACount(TFrame, product);
            int iLastPoints = count;
            int iStartIdx = iLCPCount - iLastPoints;
            Rates RATE = ORBlotter.Get(product);
            int iDecimals = RATE.Decimals;

            double dBestCompare = double.MinValue;
            double dWorstCompare = double.MaxValue;
            alternative = counterAlternative = "";

            foreach (string sP in LSProd)
            {
                if (sP == product) continue;

                Rates RATESecond = ORBlotter.Get(sP);
                int iDecimalsSecond = RATESecond.Decimals;

                int iLCPCountSecondary = ARCHIVE.GetDATACount(TFrame, sP);
                List<ChartPoint> LCPPrimary = ARCHIVE.GetDATA(TFrame, product, iStartIdx, iLastPoints);
                List<ChartPoint> LCPSecondary = ARCHIVE.GetDATA(TFrame, sP, iLCPCountSecondary - iLastPoints, iLastPoints);
                int iShift = 0;

                double dCompaison = this.CompareCharts(LCPPrimary, LCPSecondary, iDecimals, iDecimalsSecond, maxShift, ref iShift);

                if (dCompaison > dBestCompare)
                {
                    dBestCompare = dCompaison;
                    alternative = sP;
                    shift = iShift;
                }

                if(dCompaison < dWorstCompare)
                {
                    dWorstCompare = dCompaison;
                    counterAlternative = sP;
                    shiftAlternative = iShift;
                }
            }


            if (shift != 0 || alternative == "" || counterAlternative == "")
                return false;
            else return true;
        }

        public double CompareCharts(List<ChartPoint> LCPPrimary, List<ChartPoint> LCPSecondary, int iPrimaryDecimals, int iSecondaryDecimals, int maxShift, ref int shift)
        {

            int iMaxCount = Math.Max(LCPPrimary.Count, LCPSecondary.Count);


            List<List<double>> LDPSPrimary = new List<List<double>>();
            LDPSPrimary.Add(new List<double>());
            LDPSPrimary.Add(new List<double>());
            LDPSPrimary.Add(new List<double>());
            LDPSPrimary.Add(new List<double>());

            List<List<double>> LDPSSecondary = new List<List<double>>();
            LDPSSecondary.Add(new List<double>());
            LDPSSecondary.Add(new List<double>());
            LDPSSecondary.Add(new List<double>());
            LDPSSecondary.Add(new List<double>());

            int iPIndexer = 0;
            int iSIndexer = 0;
            for (int i = 0; i < iMaxCount; i++)
            {
                if (LCPPrimary.Count <= i + iPIndexer || LCPSecondary.Count <= i + iSIndexer)
                    break;

                ChartPoint CPPrimary = LCPPrimary[i + iPIndexer];
                ChartPoint CPSecondary = LCPSecondary[i + iSIndexer];

                if (CPPrimary.Time == CPSecondary.Time)
                {
                    for (int id = 0; id < 4; id++)
                    {
                        LDPSPrimary[id].Add(CPPrimary.GetParam(id, iPrimaryDecimals));
                        LDPSSecondary[id].Add(CPSecondary.GetParam(id, iSecondaryDecimals));
                    }
                    continue;
                }



                while (CPPrimary.Time > CPSecondary.Time)
                {
                    ++iSIndexer;

                    if (LCPSecondary.Count <= i + iSIndexer)
                        break;

                    CPSecondary = LCPSecondary[i + iSIndexer];

                    if (CPPrimary.Time == CPSecondary.Time)
                    {
                        for (int id = 0; id < 4; id++)
                        {
                            LDPSPrimary[id].Add(CPPrimary.GetParam(id, iPrimaryDecimals));
                            LDPSSecondary[id].Add(CPSecondary.GetParam(id, iSecondaryDecimals));
                        }
                        continue;
                    }

                }

                while (CPPrimary.Time < CPSecondary.Time)
                {
                    ++iPIndexer;

                    if (LCPPrimary.Count <= i + iPIndexer)
                        break;

                    CPPrimary = LCPPrimary[i + iPIndexer];

                    if (CPPrimary.Time == CPSecondary.Time)
                    {
                        for (int id = 0; id < 4; id++)
                        {
                            LDPSPrimary[id].Add(CPPrimary.GetParam(id, iPrimaryDecimals));
                            LDPSSecondary[id].Add(CPSecondary.GetParam(id, iSecondaryDecimals));
                        }
                        continue;
                    }
                }
            }


            List<double> LDFactors = new List<double>();
            for (int id = 0; id < 4; id++)
            {
                double dAverangePrimary = this.Average(LDPSPrimary[id]);
                double dAverangeSecondary = this.Average(LDPSSecondary[id]);

                double dFactor = dAverangePrimary / dAverangeSecondary;

                LDFactors.Add(dFactor);
            }

            for (int id = 0; id < 4; id++)
            {
                for (int i = 0; i < LDPSSecondary[id].Count; i++)
                {
                    LDPSSecondary[id][i] = Math.Round(LDPSSecondary[id][i] * LDFactors[id], iPrimaryDecimals);
                }
            }


            List<double> LDShiftSimilarity = new List<double>();

            for (int i = -maxShift; i <= maxShift; i++)
            {
                List<double> LDSimilarities = new List<double>();

                for (int id = 0; id < 4; id++)
                {
                    if (i < 0)
                    {
                        List<double> LDSNew = LDPSSecondary[id].GetRange(Math.Abs(i), LDPSSecondary[id].Count - Math.Abs(i));
                        LDSimilarities.Add(this.Compare(LDPSPrimary[id], LDSNew, 0));
                    }
                    else
                    {
                        List<double> LDSNew = LDPSSecondary[id].GetRange(0, LDPSSecondary[id].Count - Math.Abs(i));
                        LDSimilarities.Add(this.Compare(LDPSPrimary[id], LDSNew, i));
                    }

                }

                double dProd = this.Product(LDSimilarities) / Math.Pow(100, LDSimilarities.Count - 1);

                //double dAver = this.Average(LDSimilarities);
                LDShiftSimilarity.Add(dProd);
            }


            double dMaxSimilarity = 0;
            shift = 0;

            for (int i = 0; i < LDShiftSimilarity.Count; i++)
            {
                if (LDShiftSimilarity[i] > dMaxSimilarity)
                {
                    dMaxSimilarity = LDShiftSimilarity[i];
                    shift = i - maxShift;
                }
            }



            return dMaxSimilarity;
        }

    }
}
