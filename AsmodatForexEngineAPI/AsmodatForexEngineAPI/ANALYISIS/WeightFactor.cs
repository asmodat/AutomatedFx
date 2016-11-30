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


        public double WeightFactor(Rates RATE, int position, int deep, int ahead, bool averange, double step, double range, int setID) //symilarity must be above 60%
        {

            string product = RATE.CCY_Pair;
            TimeFrame TFrame = TimeFrame.ONE_MINUTE;
            int iDecimals = RATE.Decimals;
            double dPipValue = Math.Pow(10, -iDecimals);


            List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TFrame, product, 0, position);
            List<ChartPoint> LCPointsSpecified = ARCHIVE.GetDATA(TFrame, product, position - deep, deep);
            List<ChartPoint> LCPointsAll = ARCHIVE.GetDATA(TFrame, product, 0, position + ahead);

            List<double> LDSetChange = (from CP in LCPointsAll select Math.Round(CP.Change, iDecimals)).ToList();
            List<double> LDSetPeak = (from CP in LCPointsAll select Math.Round(CP.Peak, iDecimals)).ToList();
            List<double> LDSetBase = (from CP in LCPointsAll select Math.Round(CP.Base, iDecimals)).ToList();

            //double dTopSubSim = 0;
            //

            List<double> LDWFactors = new List<double>();
            List<double> LDSymilarities = new List<double>();
            double[] DAWeightFactor = new double[5];

            for (double dWF = -step * range; dWF <= step * range; dWF += step)
            {
                DAWeightFactor[setID] = dWF;

                ChartPointsPredition CPsPrediction = new ChartPointsPredition();
                ChartPointsPredition CPsPredictionNow = new ChartPointsPredition();
                double dSymilMax = 90;
                double dSymilMin = 50;

                do
                {

                    double dSymil = (dSymilMax + dSymilMin) / 2;

                    ChartPointsPredition CPsP = null;// this.PredictNextSpecified(product, LCPoints, LCPointsSpecified, TFrame, iDecimals, dSymil, ahead, DAWeightFactor);
                    if (CPsP.Prognosis(1) != ChartPointsPredition.Kind.Uncertain)
                        CPsPredictionNow = CPsP;

                    if (CPsP.Matches < 10)
                        dSymilMax = dSymil;
                    else
                        dSymilMin = dSymil;


                    if ((dSymilMax - dSymilMin <= 1) || (CPsPredictionNow.Matches >= 10 && CPsPredictionNow.Matches < 50 && CPsPredictionNow.Analised))
                    {
                        CPsPrediction = CPsPredictionNow;
                        break;
                    }



                } while (true);

                if (CPsPrediction.Matches == 0)
                    continue;

                double dSimChange = this.Compare(LDSetChange, CPsPrediction.LDChange, position);
                double dSimPeak = this.Compare(LDSetPeak, CPsPrediction.LDPeak, position);
                double dSimBase = this.Compare(LDSetBase, CPsPrediction.LDBase, position);
                double dSubSim = (dSimChange * dSimPeak * dSimBase) / (100 * 100);

                LDWFactors.Add(dWF);
                LDSymilarities.Add(dSubSim);
            }

            double dLESum = 0;
            double dGESum = 0;
            int iGECount = 0;
            int iLECount = 0;
            for (int i = 0; i < LDWFactors.Count; i++)
            {
                if (LDWFactors[i] > 0)
                {
                    dGESum += LDSymilarities[i];
                    ++iGECount;
                }
                else if (LDWFactors[i] < 0)
                {
                    dLESum += LDSymilarities[i];
                    ++iLECount;
                }
            }

            double dGES = dGESum / iGECount;
            double dLES = dLESum / iLECount;

            double dTobWF = 0;
            double dTopSubSim = 0;

            if (!averange)
            {

                for (int i = 0; i < LDWFactors.Count; i++)
                {
                    if (((dGES > dLES) && LDWFactors[i] > 0 && LDSymilarities[i] > dTopSubSim) ||
                    ((dGES < dLES) && LDWFactors[i] < 0 && LDSymilarities[i] > dTopSubSim))
                    {
                        dTopSubSim = LDSymilarities[i];
                        dTobWF = LDWFactors[i];
                    }
                }
            }
            else
            {
                double dSumWF = 0;
                double dSumWeightWF = 0;
                for (int i = 0; i < LDWFactors.Count; i++)
                {
                    if ((dGES > dLES && LDWFactors[i] > 0) || (dGES < dLES && LDWFactors[i] < 0))
                    {
                        dSumWF += LDWFactors[i] * LDSymilarities[i];
                        dSumWeightWF += LDSymilarities[i];
                        dTobWF = dSumWF / dSumWeightWF;
                    }
                }
            }



            return dTobWF;
        }


        public double CheckSimilarity(Rates RATE, int position, int deep, int ahead, double[] DAWeightFacotrs) //symilarity must be above 60%
        {

            string product = RATE.CCY_Pair;
            TimeFrame TFrame = TimeFrame.ONE_MINUTE;
            int iDecimals = RATE.Decimals;
            double dPipValue = Math.Pow(10, -iDecimals);


            List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TFrame, product, 0, position);
            List<ChartPoint> LCPointsSpecified = ARCHIVE.GetDATA(TFrame, product, position - deep, deep);
            List<ChartPoint> LCPointsAll = ARCHIVE.GetDATA(TFrame, product, 0, position + ahead);

            List<double> LDSetChange = (from CP in LCPointsAll select Math.Round(CP.Change, iDecimals)).ToList();
            List<double> LDSetPeak = (from CP in LCPointsAll select Math.Round(CP.Peak, iDecimals)).ToList();
            List<double> LDSetBase = (from CP in LCPointsAll select Math.Round(CP.Base, iDecimals)).ToList();


                ChartPointsPredition CPsPrediction = new ChartPointsPredition();
                ChartPointsPredition CPsPredictionNow = new ChartPointsPredition();
                double dSymilMax = 90;
                double dSymilMin = 50;

                do
                {

                    double dSymil = (dSymilMax + dSymilMin) / 2;

                    ChartPointsPredition CPsP = null;// this.PredictNextSpecified(product, LCPoints, LCPointsSpecified, TFrame, iDecimals, dSymil, ahead, DAWeightFacotrs);
                    if (CPsP.Prognosis(1) != ChartPointsPredition.Kind.Uncertain)
                        CPsPredictionNow = CPsP;

                    if (CPsP.Matches < 10)
                        dSymilMax = dSymil;
                    else
                        dSymilMin = dSymil;


                    if ((dSymilMax - dSymilMin <= 1) || (CPsPredictionNow.Matches >= 10 && CPsPredictionNow.Matches < 50 && CPsPredictionNow.Analised))
                    {
                        CPsPrediction = CPsPredictionNow;
                        break;
                    }



                } while (true);

                if (CPsPrediction.Matches == 0)
                    return 0;

                double dSimChange = this.Compare(LDSetChange, CPsPrediction.LDChange, position);
                double dSimPeak = this.Compare(LDSetPeak, CPsPrediction.LDPeak, position);
                double dSimBase = this.Compare(LDSetBase, CPsPrediction.LDBase, position);
                double dSubSim = (dSimChange * dSimPeak * dSimBase) / (100 * 100);


         
            return dSubSim;
        }




        



    }
}
