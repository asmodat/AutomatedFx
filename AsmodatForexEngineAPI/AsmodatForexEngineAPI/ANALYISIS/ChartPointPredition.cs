
#region References
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Added references
using System.Threading;
using System.Collections.Concurrent;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;
#endregion

namespace AsmodatForexEngineAPI
{
    public class ChartPointPredition
    {

        #region Variables & declarations
        Abbreviations ABBREVIATION = new Abbreviations();
        Analysis ANALISIS = new Analysis();
        private ConcurrentDictionary<int, List<int>> CDILIMatches;
        private List<List<double>> LLDSets;
        private List<int> LDMatchCommons = null;

        private double dPropabilityDown;
        private double dPropabilityEqual;
        private double dPropabilityUp;

        private List<double> LDChangesDown, LDChangesAverage, LDChangesUp;
        private List<double> LDPeaksChangesDown, LDPeaksChangesAverage, LDPeaksChangesUp;
        private List<double> LDBasesChangesDown, LDBasesChangesAverage, LDBasesChangesUp;

        private DateTime DTPredictedTime;
        private int iMinutesPredicted;
        private string sProduct;
        private TimeFrame TFrame;
        private bool bAnalised = false;
        private int iStrength = 0;
        private ChartPoint CPOrigin;
        private Kind KType;
        private int dMatches;


        private double dPeak;
        private double dPeakDeviation;
        private double dBase;
        private double dBaseDeviation;
        private double dChange;
        private double dChangeDeviation;
        private double dPropability;

        public string Product
        {
            get
            {
                return sProduct;
            }
        }
        public TimeFrame Frame
        {
            get
            {
                return TFrame;
            }

        }
        public bool Analised
        {
            get
            {
                return bAnalised;
            }
        }
        public int Strength
        {
            get
            {
                return iStrength;
            }
        }
        public ChartPoint Origin
        {
            get
            {
                return CPOrigin;
            }
        }
        public Kind Type
        {
            get
            {
                return KType;
            }
        }
        public int Matches
        {
            get
            {
                return dMatches;
            }
        }
        public double PropabilityEual
        {
            get
            {
                return dPropabilityEqual;
            }
        }


        public double Peak
        {
            get
            {
                return dPeak;
            }
        }
        public double PeakDeviation
        {
            get
            {
                return dPeakDeviation;
            }
        }
        public double Base
        {
            get
            {
                return dBase;
            }
        }
        public double BaseDeviation
        {
            get
            {
                return dBaseDeviation;
            }
        }
        public double Change
        {
            get
            {
                return dChange;
            }
        }
        public double ChangeDeviation
        {
            get
            {
                return dChangeDeviation;
            }
        }
        public double Propability
        {
            get
            {
                return dPropability;
            }
        }

        

        public enum Kind
        {
            Up = 0,
            Average = 1,
            Down = 2,
            Uncertain = 3
        }
        #endregion

        /// <summary>
        /// Private constructor, that can be used to clone object.
        /// </summary>
        private ChartPointPredition()
        {

        }
        /// <summary>
        /// Constructor that initializes data used in Analisis and Prediction, sldo sets up time, that it can be determined if prediction is up to date
        /// </summary>
        /// <param name="CDILIMatches">List (4 elements) (indexes) of matches to before specified symilarity between them and compared value in specified deep</param>
        /// <param name="LLDSets">Set of data used to analisis with help of indexes of Matches Dictionary</param>
        /// <param name="DTimeOfPrediction">Time of chart on from whih prediction time will be ciunted</param>
        /// <param name="product">Name of good that will be predicted</param>
        /// <param name="CPOrigin">Origin of specified ChartoPoints list - previous to predicted.</param>
        public ChartPointPredition(ConcurrentDictionary<int, List<int>> CDILIMatches, List<List<double>> LLDSets, TimeFrame TFrame, string product, ChartPoint CPOrigin)
        {
            this.CDILIMatches = CDILIMatches;
            this.LLDSets = LLDSets;
            this.sProduct = product;
            this.TFrame = TFrame;
            this.CPOrigin = CPOrigin;

            iMinutesPredicted = ABBREVIATION.ToMinutes(TFrame);
            DTPredictedTime = CPOrigin.Time.AddMinutes(iMinutesPredicted);
            
        }

        /// <summary>
        /// Analises data passed in constructor to set up propability of Acceleraction (Raise - Fall) and its direction, as good as fills lists of specified changes (Up/Down/Averange)
        /// </summary>
        /// <returns>Returns true if at least 4 matches of ChartrPoints have been passed to compare.</returns>
        private bool Analise()
        {
            //Selects ony matches for all Lists
            if (LDMatchCommons == null)
                LDMatchCommons = (from i2 in CDILIMatches[0] where CDILIMatches[1].Contains(i2) && CDILIMatches[2].Contains(i2) && CDILIMatches[3].Contains(i2) select i2).ToList();

            dMatches = LDMatchCommons.Count;

            if (dMatches < 2 || bAnalised)
                return false;

            List<double> LDSub = (from i2 in LDMatchCommons select (LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1])).ToList(); 
            double dAverange = ANALISIS.Average(LDSub);
            double dRange = Math.Abs(dAverange / 4);


            LDChangesDown = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) < -dRange) select LLDSets[0][i2 + 1]).ToList();
            LDPeaksChangesDown = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) < -dRange) select LLDSets[1][i2 + 1]).ToList();
            LDBasesChangesDown = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) < -dRange) select LLDSets[2][i2 + 1]).ToList();

            LDChangesAverage = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) <= dRange && (LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) >= -dRange) select LLDSets[0][i2 + 1]).ToList();
            LDPeaksChangesAverage = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) <= dRange && (LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) >= -dRange) select LLDSets[1][i2 + 1]).ToList();
            LDBasesChangesAverage = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) <= dRange && (LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) >= -dRange) select LLDSets[2][i2 + 1]).ToList();

            LDChangesUp = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) > dRange) select LLDSets[0][i2 + 1]).ToList();
            LDPeaksChangesUp = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) > dRange) select LLDSets[1][i2 + 1]).ToList();
            LDBasesChangesUp = (from i2 in LDMatchCommons where ((LLDSets[0][i2 + 1] - LLDSets[1][i2 + 1]) > dRange) select LLDSets[2][i2 + 1]).ToList();

            dPropabilityDown = ((double)LDChangesDown.Count() / LDMatchCommons.Count()) * 100;
            dPropabilityEqual = ((double)LDChangesAverage.Count() / LDMatchCommons.Count()) * 100;
            dPropabilityUp = ((double)LDChangesUp.Count() / LDMatchCommons.Count()) * 100;

            

            bAnalised = true;
            return true;
        }

        /// <summary>
        /// Defines if prediction is up to date
        /// </summary>
        /// <returns>Returns true if more than 50% of time is remaining else false</returns>
        public bool IsActual
        {
            get
            {
                double iSecondsPredicted = (DTPredictedTime - ABBREVIATION.GreenwichMeanTime).TotalSeconds;
                double dSecondsToPredict = ((iMinutesPredicted * 60) / 4); // 0.25% left

                if (iSecondsPredicted >= dSecondsToPredict)
                    return true;
                else return false;
            }

        }

        /// <summary>
        /// Sets up prognosis if data is Analised properly, also sets up predicted values of acceleration from previous value, peak and base of future events with specified propability
        /// </summary>
        /// <returns>Reurns kind of acceleration prognosis</returns>
        public Kind Prognosis()
        {
            this.Analise();

            if (!bAnalised)
                KType = Kind.Uncertain;

            else if (dPropabilityDown > dPropabilityEqual &&
                dPropabilityDown > dPropabilityUp)
            {
                dChange = ANALISIS.Average(LDChangesDown);
                dChangeDeviation = ANALISIS.StandardDeviation(LDChangesDown, dChange);
                dPeak = ANALISIS.Average(LDPeaksChangesDown);
                dPeakDeviation = ANALISIS.StandardDeviation(LDPeaksChangesDown, dPeak);
                dBase = ANALISIS.Average(LDBasesChangesDown);
                dBaseDeviation = ANALISIS.StandardDeviation(LDBasesChangesDown, dBase);

                iStrength = (int)((dPropabilityDown - dPropabilityUp) * LDChangesDown.Count);
                dPropability = dPropabilityDown;
                KType = Kind.Down;
            }
            else if (dPropabilityEqual > dPropabilityDown &&
                dPropabilityEqual > dPropabilityUp)
            {

                dChange = ANALISIS.Average(LDChangesAverage);
                dChangeDeviation = ANALISIS.StandardDeviation(LDChangesAverage, dChange);
                dPeak = ANALISIS.Average(LDPeaksChangesAverage);
                dPeakDeviation = ANALISIS.StandardDeviation(LDPeaksChangesAverage, dPeak);
                dBase = ANALISIS.Average(LDBasesChangesAverage);
                dBaseDeviation = ANALISIS.StandardDeviation(LDBasesChangesAverage, dBase);

                iStrength = 0;// (int)(dPropabilityEqual * LDChangesAverage.Count);
                dPropability = dPropabilityEqual;
                KType = Kind.Average;
            }
            else if (dPropabilityUp > dPropabilityDown &&
                dPropabilityUp > dPropabilityEqual)
            {

                dChange = ANALISIS.Average(LDChangesUp);
                dChangeDeviation = ANALISIS.StandardDeviation(LDChangesUp, dChange);
                dPeak = ANALISIS.Average(LDPeaksChangesUp);
                dPeakDeviation = ANALISIS.StandardDeviation(LDPeaksChangesUp, dPeak);
                dBase = ANALISIS.Average(LDBasesChangesUp);
                dBaseDeviation = ANALISIS.StandardDeviation(LDBasesChangesUp, dBase);


                iStrength = (int)(LDChangesUp.Count * (dPropabilityUp - dPropabilityDown));
                dPropability = dPropabilityUp;
                KType = Kind.Up;
            }
            else KType = Kind.Uncertain;

            return KType;
        }

        /// <summary>
        /// Clones object without list and sub classes
        /// </summary>
        /// <returns>Clonned dumb object.</returns>
        public ChartPointPredition Clone()
        {
            ChartPointPredition CPP = new ChartPointPredition();
            /*CPP.ABBREVIATION = ABBREVIATION;
            CPP.ANALISIS = ANALISIS;*/
            CPP.bAnalised = bAnalised;
            //CPP.CPOrigin = CPOrigin;
            CPP.dBase = dBase;
            CPP.dBaseDeviation = dBaseDeviation;
            CPP.dChange = dChange;
            CPP.dChangeDeviation = dChangeDeviation;
            CPP.dPeak = dPeak;
            CPP.dPeakDeviation = dPeakDeviation;
            CPP.dPropability = dPropability;
            CPP.dPropabilityDown = dPropabilityDown;
            CPP.dPropabilityEqual = dPropabilityEqual;
            CPP.dPropabilityUp = dPropabilityEqual;
            CPP.dPropabilityUp = dPropabilityUp;
            CPP.DTPredictedTime = DTPredictedTime;
            CPP.iMinutesPredicted = iMinutesPredicted;
            CPP.iStrength = iStrength;
            CPP.KType = KType;
            CPP.dMatches = dMatches;
            /*CPP.LDBasesChangesAverage = LDBasesChangesAverage;
            CPP.LDBasesChangesDown = LDBasesChangesDown;ss
            CPP.LDBasesChangesUp = LDBasesChangesUp;
            CPP.LDChangesAverage = LDChangesAverage;
            CPP.LDChangesDown = LDChangesUp;
            CPP.LDMatchCommons = LDMatchCommons;
            CPP.LDPeaksChangesAverage = LDPeaksChangesDown;
            CPP.LDPeaksChangesUp = LDPeaksChangesUp;
            CPP.LLDSets = LLDSets;*/
            CPP.sProduct = sProduct;
            CPP.TFrame = TFrame;

            return CPP;
        }



    }
}
