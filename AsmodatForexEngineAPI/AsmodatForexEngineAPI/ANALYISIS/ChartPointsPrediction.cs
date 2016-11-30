
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
using AsmodatSerialization;
#endregion

namespace AsmodatForexEngineAPI
{
    public class ChartPointsPredition
    {

        #region Variables & declarations
        Abbreviations ABBREVIATION = new Abbreviations();
        Analysis ANALISIS = new Analysis();
        //private List<int> LIMatches;
        //private ConcurrentDictionary<int, List<int>> CDILIMatches;
        private double[][] DJSets;
        private List<int> LDMatchCommons = null;
        private List<double> LDSimilarity = null;

        private DateTime DTOrigin;
        private int iMinutes;
        private string sProduct;
        private TimeFrame TFrame;
        private bool bAnalised = false;
        private int iStrength = 0;
        private ChartPoint CPOrigin;
        private Kind KType = Kind.Uncertain;
        private int iMatches;
        private int iDeep;
        private int iAhead;

        public List<double> LDChange = new List<double>();
        public List<double> LDPeak = new List<double>();
        public List<double> LDBase = new List<double>();

        private int iStart;
        private int iExtremum;
        private double dTendency, dTendencyToday, dTendencyComplementary, dExtremumTodayDistance, dExtremumConplementaryDistance;
        private double dSymilarity = -1;
        private int iPosition = -1;

        private bool bStrike;
        private double dAccuracity;
        private int iContinuity = 0, iContinuityAlternative = 0, iContinuityTendencyChange = 0;
        private int iTestID = -1;

        private double dResolution = 0;
        public double Resolution
        {
            get
            {
                return Math.Round(dResolution, 1);
            }
            set
            {
                dResolution = value;
            }
        }

        private double dTestSpread = double.MaxValue;
        public double TestSpread
        {
            get
            {
                return dTestSpread;
            }
            set
            {
                dTestSpread = value;
            }
        }


      
        public double[] Parameters
        {
            get
            {
                if (this.TestID < 0)
                    return null;

                return new double[] 
                { 
                    //this.dStrengthFactor,
                    //this.Similarity,
                    //this.iStart,
                    //this.iExtremum,
                    //this.TendencyComplementary,
                    //this.TendencyToday,
                    this.Tendency,
                    //this.ExtremumConplementaryDistance,
                    //this.ExtremumTodayDistance,
                };
            }

        }

        public DateTime DTOriginal
        {
            get
            {
                return this.DTOrigin;
            }
        }
        public int TestID
        {
            get
            {
                return iTestID;
            }
            set
            {
                iTestID = value;
            }
        }
        public int Continuity
        {
            get
            {
                return iContinuity;
            }
            set
            {
                    iContinuity = value;
            }
        }
        public int ContinuityAlternative
        {
            get
            {
                return iContinuityAlternative;
            }
            set
            {
                iContinuityAlternative = value;
            }
        }
        public int ContinuityTendencyChange
        {
            get
            {
                return iContinuityTendencyChange;
            }
            set
            {
                iContinuityTendencyChange = value;
            }
        }

        /// <summary>
        /// Accuracity of strike
        /// </summary>
        public double Accuracity
        {
            get
            {
                if (bStrike)
                    return dAccuracity;
                else return -1;
            }
            set
            {
                dAccuracity = value;
            }
        }
       
        /// <summary>
        /// Successful prediction
        /// </summary>
        public bool Strike
        {
            get
            {
                return bStrike;
            }
            set
            {
                    bStrike = value;
            }
        }
        /// <summary>
        /// Defines position in ChartPoints Set
        /// </summary>
        public int Position
        {
            get
            {
                return iPosition;
            }
            set
            {
                if (iPosition == -1 && value > 0)
                    iPosition = value;
            }
        }
        /// <summary>
        /// Symilarity param of ANALISIS matches search
        /// </summary>
        public double Similarity
        {
            get
            {
                return Math.Round(dSymilarity, 1);
            }
            set
            {
                if (dSymilarity == -1 && value > 0)
                    dSymilarity = value;
            }
        }
        public int IndexStart
        {
            get
            {
                return iStart;
            }
        }
        public int IndexExtremum
        {
            get
            {
                return iExtremum;
            }
        }
        public double Tendency
        {
            get
            {
                return Math.Round(dTendency,1);
            }
        }
        public double TendencyToday
        {
            get
            {
                    return Math.Round(dTendencyToday,1);
            }
            set
            {
                dTendencyToday = value;
            }
        }
        public double ExtremumTodayDistance
        {
            get
            {
                return Math.Round(dExtremumTodayDistance, 0);
            }
            set
            {
                dExtremumTodayDistance = value;
            }
        }
        public double TendencyComplementary
        {
            get
            {
                return Math.Round(dTendencyComplementary,1);
            }
            set
            {
                dTendencyComplementary = value;
            }
        }
        public double ExtremumConplementaryDistance
        {
            get
            {
                return Math.Round(dExtremumConplementaryDistance, 0);
            }
            set
            {
                dExtremumConplementaryDistance = value;
            }
        }

        public TimeFrame TimeFrame
        {
            get
            {
                return TFrame;
            }
        }

        public string Product
        {
            get
            {
                return sProduct;
            }
        }

        public bool Analised
        {
            get
            {
                return bAnalised;
            }
        }
        
        /*public ChartPoint Origin
        {
            get
            {
                return CPOrigin;
            }
        }*/
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
                return iMatches;
            }
        }
        public int Ahead
        {
            get
            {
                return iAhead;
            }
        }
        public int Deep
        {
            get
            {
                return iDeep;
            }
        }


        private string sID = null;
        /// <summary>
        /// Unical ID of properly constructed prediction
        /// </summary>
        public string ID
        {
            get
            {
                return sID;
            }
        }


        public enum Kind
        {
            Up = 0,
            Average = 1,
            Down = 2,
            Uncertain = 3
        }

        public double dStrengthFactor = 0;
        private double dStrength = 0;
        private double dOriginStrength = 0;
        public double Strength
        {
            get
            {
                return dStrength;
            }
        }
        public double OriginStrength
        {
            get
            {
                return dOriginStrength;
            }
        }
        private void SetStrength()
        {
            if (LDPeak.Count < iAhead || LDBase.Count < iAhead)
            {
                dStrength = 0;
                return;
            }

            dStrength = 0;
            for (int i = 0; i < iAhead; i++)
                dStrength += Math.Pow(LDPeak[i], 2) + Math.Pow(LDBase[i], 2);

        }
        public void SetStrengthFactor(List<double> LDOPeak, List<double> LDOBase)
        {
            if (LDOPeak.Count < iDeep || LDOBase.Count < iDeep)
            {
                dOriginStrength = 0;
                return;
            }

            dOriginStrength = 0;
            for (int i = 0; i < iAhead; i++)
                dOriginStrength += Math.Pow(LDOPeak[i], 2) + Math.Pow(LDOBase[i], 2);

            dStrengthFactor = Math.Round(((double)((double)dStrength / iAhead) / ((double)dOriginStrength / iDeep)) * 100,0);

        }
        #endregion

        public ChartPointsPredition()
        {

        }
        public ChartPointsPredition(List<int> LIMatches, double[][] DJSets, TimeFrame TFrame, string product, ChartPoint CPOrigin, int deep, int ahead, List<double> LDSimilarity)
        {
            this.LDSimilarity = LDSimilarity;
            //this.dSymilarity = similarity;
            this.LDMatchCommons = LIMatches;
            this.DJSets = DJSets;
            this.sProduct = product;
            this.TFrame = TFrame;
            this.CPOrigin = CPOrigin;
            this.iDeep = deep;
            this.iAhead = ahead;

            iMinutes = ABBREVIATION.ToMinutes(TFrame);
            DTOrigin = CPOrigin.Time;


            DateTime DTNow = DateTime.Now;
            sID = "ID#" + product + "#" + DTNow.Year + "#" + DTNow.Month + "#" + DTNow.Day + "#" + DTNow.Hour + "#" + DTNow.Minute + "#" + DTNow.Second + "#" + DTNow.Millisecond + "#" + new Random().Next(int.MaxValue);
        }
        public ChartPointsPredition(DataBase_ChartPointsPrediction DBCPsPrediction)
        {
            
            this.bAnalised = true;
            this.dSymilarity = DBCPsPrediction.SIMILARITY;
            this.sProduct = DBCPsPrediction.PRODUCT;
            this.iAhead = DBCPsPrediction.AHEAD;
            this.bAnalised = DBCPsPrediction.ANALISED;
            this.iDeep = DBCPsPrediction.DEEP;
            this.DTOrigin = DBCPsPrediction.DTORIGINAL;
            this.sID = DBCPsPrediction.ID;
            this.LDBase = DBCPsPrediction.LDBASE;
            this.LDChange = DBCPsPrediction.LDCHANGE;
            this.LDPeak = DBCPsPrediction.LDPEAK;
            this.iMatches = DBCPsPrediction.MATCHES;
            this.iPosition = DBCPsPrediction.POSITION;
            this.TFrame = ABBREVIATION.ToTimeFrame(DBCPsPrediction.TIMEFRAME);
        }
        public DataBase_ChartPointsPrediction ToDataBase()
        {


            DataBase_ChartPointsPrediction DBCPsPrediction = new DataBase_ChartPointsPrediction();

            DBCPsPrediction.SIMILARITY = this.dSymilarity;
            DBCPsPrediction.PRODUCT = this.Product;
            DBCPsPrediction.AHEAD = this.iAhead;
            DBCPsPrediction.ANALISED = this.bAnalised;
            DBCPsPrediction.DEEP = this.iDeep;
            DBCPsPrediction.DTORIGINAL = this.DTOrigin;
            DBCPsPrediction.ID = this.ID;
            DBCPsPrediction.LDBASE = this.LDBase;
            DBCPsPrediction.LDCHANGE = this.LDChange;
            DBCPsPrediction.LDPEAK = this.LDPeak;
            DBCPsPrediction.MATCHES = this.Matches;
            DBCPsPrediction.POSITION = this.Position;
            DBCPsPrediction.TIMEFRAME = ABBREVIATION.ToString(this.TimeFrame);

            return DBCPsPrediction;
        }


        private List<double> Analise(int setID, int selectSetID)
        {
            List<double> LDSubSet = new List<double>();
            for (int i = 1; i <= iAhead; i++)
            {
                List<double> LDSub = (from i2 in LDMatchCommons select (DJSets[setID][i2 + i])).ToList();
                double dAverange = ANALISIS.Average(LDSub);
                double dRange = Math.Abs(dAverange);

                List<double> LDDown = (from i2 in LDMatchCommons where ((DJSets[setID][i2 + i]) < -dRange) select (DJSets[selectSetID][i2 + i])).ToList();
                List<double> LDAverange = (from i2 in LDMatchCommons where ((DJSets[setID][i2 + i]) <= dRange && (DJSets[selectSetID][i2 + i]) >= -dRange) select (DJSets[selectSetID][i2 + i])).ToList();
                List<double> LDCUp = (from i2 in LDMatchCommons where ((DJSets[setID][i2 + i]) > dRange) select (DJSets[selectSetID][i2 + i])).ToList();

                double dPropabilityDown = ((double)LDDown.Count() / LDMatchCommons.Count()) * 100;
                double dPropabilityAverange = ((double)LDAverange.Count() / LDMatchCommons.Count()) * 100;
                double dPropabilityUp = ((double)LDCUp.Count() / LDMatchCommons.Count()) * 100;

                if (dPropabilityDown == double.NaN)
                    dPropabilityDown = 0;
                if (dPropabilityAverange == double.NaN)
                    dPropabilityAverange = 0;
                if (dPropabilityUp == double.NaN)
                    dPropabilityUp = 0;

                List<double> LDSelected = new List<double>();
                if (dPropabilityDown > dPropabilityUp && dPropabilityDown >= dPropabilityAverange)
                {
                    LDSelected = LDDown;
                }
                else if (dPropabilityUp > dPropabilityDown && dPropabilityUp >= dPropabilityAverange)
                {
                    LDSelected = LDCUp;
                }
                else
                {
                    LDSelected = LDAverange;
                }

                double dValieSelected = 0;
                if (LDSelected.Count != 0)
                    dValieSelected = ANALISIS.Average(LDSelected);

                LDSubSet.Add(dValieSelected);
            }

            return LDSubSet;
        }

        private List<double> AnaliseQuick(int setID, int selectSetID)
        {
            List<double> LDSubSet = new List<double>();
            for (int i = 1; i <= iAhead; i++)
            {
                List<double> LDSelected = (from i2 in LDMatchCommons select (DJSets[setID][i2 + i])).ToList();
                double dValieSelected = 0;
                if (LDSelected.Count != 0)
                    dValieSelected = ANALISIS.Average(LDSelected);

                LDSubSet.Add(dValieSelected);
            }

            return LDSubSet;
        }


        private List<int> CleanUp(List<int> LISet, int space)
        {
            List<int> LISCleaned = new List<int>();
            int iCount = LISet.Count;
            int iPrevious = int.MinValue/2;
            for(int i = 0; i < iCount; i++)
            {
                int iValue = LISet[i];
                int iSpace = iValue - iPrevious;

                if (iSpace > space)
                    LISCleaned.Add(iValue);

                iPrevious = iValue;
            }

            return LISCleaned;
        }


        private bool Analise()
        {
            if (LDMatchCommons == null)
            {
                DJSets = null;
                bAnalised = true;
                return false;
            }

            iMatches = LDMatchCommons.Count;


            if (iMatches < 1 || bAnalised)
            {
                DJSets = null;
                bAnalised = true;
                return false;
            }

            /*LDChange = this.Analise(4,4);
            LDPeak = this.Analise(4, 2);
            LDBase = this.Analise(4, 3);

            
             LDChange = this.AnaliseQuick(4,4);
            LDPeak = this.AnaliseQuick(4, 2);
            LDBase = this.AnaliseQuick(4, 3);
             */
            

            for (int i = 1; i <= iAhead; i++)
            {
                
                List<double> LDSChange = (from i2 in LDMatchCommons select (DJSets[1][i2 + i] - DJSets[3][i2 + i])).ToList();
                List<double> LDSPeak = (from i2 in LDMatchCommons select DJSets[2][i2 + i]).ToList();
                List<double> LDSBase = (from i2 in LDMatchCommons select DJSets[3][i2 + i]).ToList();
                
                int iLDSCount = LDSimilarity.Count;

                for(int i2 = 0; i2 < iLDSCount; i2++)
                {
                    double dSimilarityFactor = (double)LDSimilarity[i2] / 100;
                    LDSChange[i2] *= dSimilarityFactor;
                    LDSPeak[i2] *= dSimilarityFactor;
                    LDSBase[i2] *= dSimilarityFactor;
                }


                double dChange = ANALISIS.Average(LDSChange);
                double dPeak = ANALISIS.Average(LDSPeak);
                double dBase = ANALISIS.Average(LDSBase);
                LDChange.Add(dChange);
                LDPeak.Add(dPeak);
                LDBase.Add(dBase);
            }
          

            bAnalised = true;
            DJSets = null;
            return true;
        }


        public bool IsActual
        {
            get
            {
                
                double iSecondsShift = 0;

                double dUPTime = (double)iAhead / 2;
                if (dUPTime < (iStart + 1))
                    dUPTime = (iStart + 1);

                double dSecondsPredicted = (double)(iMinutes * dUPTime * 60); 

                iSecondsShift = (DTOrigin.AddSeconds(dSecondsPredicted) - ABBREVIATION.GreenwichMeanTime).TotalSeconds;


                if (iSecondsShift > 0)
                    return true;
                else return false;
            }

        }


        public Kind Prognosis(double tendencyDeviation)
        {
            if (!Analised)
                this.Analise();

            if (!bAnalised || this.sID == null)
            {
                KType = Kind.Uncertain;
                return KType;
            }

            this.SetStrength();

            int iCount = LDChange.Count;
            double dChangePoint = 0;
            double dLocalPeak = double.MinValue;
            int iLocalPeakPosition = 0;
            double dLocalBase = double.MaxValue;
            int iLocalBasePosition = 0;
            
            iExtremum = 0;

            List<double> LDSetHigh = new List<double>();
            List<double> LDSetLow = new List<double>();

            for (int i = 0; i < iCount; i++)
            {
                

                double dPeak = dChangePoint + LDPeak[i];
                double dBase = dChangePoint - LDBase[i];
                LDSetHigh.Add(dPeak);
                LDSetLow.Add(dBase);

                if (dPeak >= dLocalPeak)
                {
                    dLocalPeak = dPeak;
                    iLocalPeakPosition = i;
                }

                if (dBase <= dLocalBase)
                {
                    dLocalBase = dBase;
                    iLocalBasePosition = i;
                }

                dChangePoint += LDChange[i];
            }


            if (iLocalPeakPosition > iLocalBasePosition)
            {
                KType = Kind.Up;
                iStart = iLocalBasePosition;
                iExtremum = iLocalPeakPosition;
            }
            else if (iLocalPeakPosition < iLocalBasePosition)
            {
                KType = Kind.Down;
                iStart = iLocalPeakPosition;
                iExtremum = iLocalBasePosition;
            } 
            else 
            {
                KType = Kind.Average;
                return KType;
            }

            //dTendency = ANALISIS.Tendency(LDSetHigh, LDSetLow);  //LDChange,0,(iExtremum + 1));

            double dTendencyHigh = ANALISIS.Tendency(LDSetHigh);
            double dTendencyLow = ANALISIS.Tendency(LDSetLow);
            double dTAverange = (double)(dTendencyHigh + dTendencyLow) / 2;

            if ((dTendencyHigh > 50 && dTendencyLow > 50) || (dTendencyHigh < 50 && dTendencyLow < 50))
                dTendency = dTAverange;
            else  dTendency = 50;



            if (dTendency > (50 + tendencyDeviation) && KType == Kind.Up)
                KType = Kind.Up;
            else if (dTendency < (50 - tendencyDeviation) && KType == Kind.Down)
                KType = Kind.Down;
            else if (ANALISIS.InRange(dTendency, 50, tendencyDeviation) && KType == Kind.Average)
                KType = Kind.Average;
            else
            {
                KType = Kind.Uncertain;
                return KType;
            }



            return KType;
        }



        public void StartPoint(double dLastExtremumBase, double dLastExtremumPeak)
        {
            double dChangePoint = 0;
            double dDifference = 0;

            for (int i = 0; i < (iExtremum + 1); i++)
            {
                double dPeak = dChangePoint + LDPeak[i];
                double dBase = dChangePoint - LDBase[i];

                double dDiffPeak = (dLastExtremumBase - dPeak); //loss//(dLastExtremum - dPeak);
                double dDiffBase = (dLastExtremumPeak - dBase); //gain//(dLastExtremum - dBase);

                if (KType == Kind.Up && dDiffBase >= dDifference)
                {
                    iStart = i;
                    dDifference = dDiffBase;
                }
                else if (KType == Kind.Down && dDiffPeak <= dDifference)
                {
                    iStart = i;
                    dDifference = dDiffPeak;
                }

                dChangePoint += LDChange[i];
            }
        }


        

    }
}

