using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{



    
    public partial class Simulation
    {


        int iCounter = 0, iCounterTotal = 0;
        int iStrikes = 0, iFails = 0;
        int iFailsNotImportangt = 0;

        public double Resolute(string product, int minResolutions, int index)
        {
            ChartPointsPredition CPsPOriginal = DATA[product][index];

            if (CPsPOriginal == null || CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average || CPsPOriginal.Tendency == 50 || CPsPOriginal.TendencyToday == 50 || CPsPOriginal.TendencyComplementary == 50)
                return 0;

            List<ChartPointsPredition> LCPsPAll = this.ExtractType(DATA[product].GetRange(0, index), CPsPOriginal); //All of type


            //1 - 67%//ExtractTendencyToTendencyComplementary ExtractTendenciesDirectted ExtractTendencyUncertainity
            //1-3 - 67%
            LCPsPAll = this.ExtractTendencyToTendencyComplementary(LCPsPAll, CPsPOriginal);

            //0.87 ratio - ExtractTendencyToTendencyComplementary

            double[] DAParametersOryginal = CPsPOriginal.Parameters;

            List<double[]> LDAPStrikes = this.ExtractParameters(LCPsPAll, CPsPOriginal, true, false);
            List<double[]> LDAPFails = this.ExtractParameters(LCPsPAll, CPsPOriginal, false, false);

            double dRatio = ((double)LDAPStrikes.Count / LCPsPAll.Count()) * 100;

            if (LDAPStrikes.Count < minResolutions || LDAPFails.Count < minResolutions || dRatio < 25)
                return 0;

            double dSFS = ANALYSIS.SimilarityFactor(LDAPStrikes, DAParametersOryginal) ;
            double dSFF = ANALYSIS.SimilarityFactor(LDAPFails, DAParametersOryginal);// *((double)LDAPFails.Count() / LCPsPAll.Count());


            if (dSFS > dSFF)
                return ((double)dSFS / dSFF) * 100 * ((double)LDAPStrikes.Count() / LCPsPAll.Count());
            else return 0;
        }



        public double ResoluteTest(string product, int minResolutions, int index)
        {
            ChartPointsPredition CPsPOriginal = DATA[product][index];

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return 0;

            //ExtractTypeExtremumTendencyTest
            List<ChartPointsPredition> LCPsPAll = this.ExtractTypeTest(DATA[product].GetRange(0, index), CPsPOriginal,false); //All of type
            List<ChartPointsPredition> LCPsPAllReversed = this.ExtractTypeTest(DATA[product].GetRange(0, index), CPsPOriginal, true); //All of type

           // LCPsPAll = this.ExtractSimilarityTest(LCPsPAll); LCPsPAllReversed = this.ExtractSimilarityTest(LCPsPAllReversed);
           // LCPsPAll = this.ExtractTypeExtremumTendencyTest(LCPsPAll); LCPsPAllReversed = this.ExtractTypeExtremumTendencyTest(LCPsPAllReversed);
            //LCPsPAll = this.ExtractTestContinuous(LCPsPAll); LCPsPAllReversed = this.ExtractTestContinuous(LCPsPAllReversed);
            //LCPsPAll = this.ExtractTestContinuousTendency(LCPsPAll); LCPsPAllReversed = this.ExtractTestContinuousTendency(LCPsPAllReversed);

            double[] DAParametersOryginal = CPsPOriginal.Parameters;

            List<double[]> LDAPStrikes = this.ExtractParameters(LCPsPAll, CPsPOriginal, true, false);
            List<double[]> LDAPFails = this.ExtractParameters(LCPsPAll, CPsPOriginal, false, false);

            List<double[]> LDAPStrikesReversed = this.ExtractParameters(LCPsPAllReversed, CPsPOriginal, true, true);
            List<double[]> LDAPFailsReversed = this.ExtractParameters(LCPsPAllReversed, CPsPOriginal, false, true);

            double dRatio = ((double)LDAPStrikes.Count / LCPsPAll.Count()) * 100;
            double dRatioReversed = ((double)LDAPStrikesReversed.Count / LCPsPAllReversed.Count()) * 100;
            //50.14-50.85 - clear

            if (LDAPStrikesReversed.Count < minResolutions || LDAPFailsReversed.Count < minResolutions)
                return 0;

            if (LDAPStrikes.Count < minResolutions || LDAPFails.Count < minResolutions)
                return 0;

            if (dRatio < 45 || dRatioReversed < 45)
            {
                //throw new Exception("Something is fucked up !");
            }

            for (int i = 0; i < LDAPStrikes.Count; i++ )
            {
                if(LDAPStrikes[i] == null)
                {
                    var v = 0;
                    var P = 1 + v;
                }
            }
            for (int i = 0; i < LDAPFails.Count; i++)
            {
                if (LDAPFails[i] == null)
                {
                    var v = 0;
                    var P = 1 + v;
                }
            }

            return 1;

            double dSFS = ANALYSIS.SimilarityFactor(LDAPStrikes, DAParametersOryginal);
            double dSFF = ANALYSIS.SimilarityFactor(LDAPFails, DAParametersOryginal);// *((double)LDAPFails.Count() / LCPsPAll.Count());

            return 1;

            if (dSFS > dSFF)
                return ((double)dSFS / dSFF) * 100 * ((double)LDAPStrikes.Count() / LCPsPAll.Count());
            else return 0;
        }
        public void ResolutePositionTest(string product, int index)
        {
            ++iCounterTotal;
            ChartPointsPredition CPsPOriginal = DATA[product][index];

            double dResolution = this.ResoluteTest(product, 100, index);
            


            bool bStike = false;

            if (dResolution > 0) 
                bStike = true;

            ++iCounter;



            if (bStike == true && CPsPOriginal.Strike == false)
            {
                ++iFails;

            }
            else if (bStike != CPsPOriginal.Strike)
            {
                ++iFailsNotImportangt;
            }
            else if (bStike == true && CPsPOriginal.Strike == true)
            {
                ++iStrikes;

            }

            if (iCounterTotal == 9000)
            {

                int breakpoint = 0;
                int ii = breakpoint;
            }
        }


        public List<ChartPointsPredition> ExtractTendencyToTendencyComplementary(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.ExtremumConplementaryDistance == CPsPOriginal.ExtremumTodayDistance || CPsPOriginal.TendencyToday == 50 || CPsPOriginal.ExtremumConplementaryDistance <= 0) return LCPsPExtracted;


            if (CPsPOriginal.Tendency < 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].ExtremumConplementaryDistance == LCPsPAll[i].ExtremumTodayDistance || LCPsPAll[i].TendencyToday == 50 || LCPsPAll[i].ExtremumConplementaryDistance <= 0) continue;

                    if (LCPsPAll[i].TendencyComplementary > 100)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }
            else if (CPsPOriginal.Tendency > 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].ExtremumConplementaryDistance == LCPsPAll[i].ExtremumTodayDistance || LCPsPAll[i].TendencyToday == 50 || LCPsPAll[i].ExtremumConplementaryDistance <= 0) continue;

                    if (LCPsPAll[i].TendencyComplementary < 100)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }

            return LCPsPExtracted;
        }

        public List<ChartPointsPredition> ExtractTendenciesDirectted(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.ExtremumTodayDistance == CPsPOriginal.ExtremumConplementaryDistance) return LCPsPExtracted;

            if (CPsPOriginal.TendencyComplementary < 50 && CPsPOriginal.Tendency < 50 && CPsPOriginal.TendencyToday < 50)
            {
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if (LCPsPAll[i].TendencyComplementary < 50 && LCPsPAll[i].Tendency < 50 && LCPsPAll[i].TendencyToday < 50 && LCPsPAll[i].ExtremumTodayDistance != LCPsPAll[i].ExtremumConplementaryDistance) 
                        LCPsPExtracted.Add(LCPsPAll[i]);
            }
            else if (CPsPOriginal.TendencyComplementary > 50 && CPsPOriginal.Tendency > 50 && CPsPOriginal.TendencyToday > 50)
            {
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if (LCPsPAll[i].TendencyComplementary > 50 && LCPsPAll[i].Tendency > 50 && LCPsPAll[i].TendencyToday > 50 && LCPsPAll[i].ExtremumTodayDistance != LCPsPAll[i].ExtremumConplementaryDistance)
                        LCPsPExtracted.Add(LCPsPAll[i]);
            }

            return LCPsPExtracted;
        }

        public List<double[]> ExtractParameters(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal, bool strikes, bool reversed)
        {
            List<double[]> LDAParams = new List<double[]>();
            string sID = CPsPOriginal.ID;

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return LDAParams;

            for (int i = 0; i < LCPsPAll.Count; i++)
            {
                ChartPointsPredition CPsP = LCPsPAll[i];
                if (CPsP.ID == sID || CPsP.Type == ChartPointsPredition.Kind.Uncertain || CPsP.Type == ChartPointsPredition.Kind.Average || strikes != CPsP.Strike)
                    continue;

                if (reversed && CPsP.Type != CPsPOriginal.Type)
                    LDAParams.Add(CPsP.Parameters);
                else if (!reversed && CPsP.Type == CPsPOriginal.Type)
                    LDAParams.Add(CPsP.Parameters);

            }

            return LDAParams;
        }
        public List<ChartPointsPredition> ExtractTendencyUncertainity(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return LCPsPExtracted;

            for (int i = 0; i < LCPsPAll.Count; i++)
                if (LCPsPAll[i].Tendency != 50 && LCPsPAll[i].TendencyComplementary != 50 && LCPsPAll[i].TendencyToday != 50)
                    LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractTendencyComplementary(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.TendencyComplementary < 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].TendencyComplementary < 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }
            else if (CPsPOriginal.TendencyComplementary > 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if (LCPsPAll[i].TendencyComplementary > 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractTendencyToday(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.TendencyToday < 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].TendencyToday < 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }
            else if (CPsPOriginal.TendencyToday > 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if (LCPsPAll[i].TendencyToday > 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractTendency(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.Tendency < 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].Tendency < 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }
            else if (CPsPOriginal.Tendency > 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if ( LCPsPAll[i].Tendency > 50)
                        LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractType(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return LCPsPExtracted;

            for (int i = 0; i < LCPsPAll.Count; i++)
                if (CPsPOriginal.Type == LCPsPAll[i].Type)
                    LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractCounterType(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return LCPsPExtracted;

            for (int i = 0; i < LCPsPAll.Count; i++)
                if (CPsPOriginal.Type != LCPsPAll[i].Type)
                    LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }


        public List<ChartPointsPredition> ExtractTypeExtremumTendencyTest(List<ChartPointsPredition> LCPsPAll)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();


            for (int i = 0; i < LCPsPAll.Count; i++)
            {
                ChartPointsPredition CPsPNow = LCPsPAll[i];
                if (CPsPNow.ExtremumTodayDistance > ((CPsPNow.Deep)))// / 2) + 1))
                    LCPsPExtracted.Add(CPsPNow);
            }

            return LCPsPExtracted;
        }

        public List<ChartPointsPredition> ExtractSimilarityTest(List<ChartPointsPredition> LCPsPAll)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();


            for (int i = 0; i < LCPsPAll.Count; i++)
            {
                ChartPointsPredition CPsPNow = LCPsPAll[i];
                if (CPsPNow.Similarity > 80)// / 2) + 1))
                    LCPsPExtracted.Add(CPsPNow);
            }

            return LCPsPExtracted;
        }


      
        public List<ChartPointsPredition> ExtractTypeTest(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal, bool reversed)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.Type == ChartPointsPredition.Kind.Uncertain || CPsPOriginal.Type == ChartPointsPredition.Kind.Average)
                return LCPsPExtracted;

            for (int i = 0; i < LCPsPAll.Count; i++)
            {
                ChartPointsPredition CPsP = LCPsPAll[i];

                if (CPsP.Type == ChartPointsPredition.Kind.Uncertain || CPsP.Type == ChartPointsPredition.Kind.Average)
                    continue;


               if(!reversed && CPsPOriginal.Type == CPsP.Type)
                    LCPsPExtracted.Add(CPsP);
               else if (reversed && CPsPOriginal.Type != CPsP.Type)
                   LCPsPExtracted.Add(CPsP);
            }

            return LCPsPExtracted;
        }



        public List<ChartPointsPredition> ExtractTestContinuous(List<ChartPointsPredition> LCPsPAll)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            int iDeepMax = 1;
            
            for (int i = iDeepMax + 1; i < LCPsPAll.Count; i++)
            {
                int iDeep = -1;
                bool bPass = true;

                while (++iDeep < iDeepMax && bPass != false)
                {

                    ChartPointsPredition CPsPPrevious = LCPsPAll[i - 1 - iDeep];
                    ChartPointsPredition CPsPNow = LCPsPAll[i - iDeep];

                    double dTimeSpread = (CPsPNow.DTOriginal - CPsPPrevious.DTOriginal).TotalMinutes;
                    int iTFMinutes = ABBREVIATIONS.ToMinutes(CPsPNow.TimeFrame);

                    if (CPsPNow.Type != CPsPPrevious.Type || dTimeSpread > iTFMinutes)//CPsPNow.Ahead)
                        bPass = false;
                }

                if(bPass)
                    LCPsPExtracted.Add(LCPsPAll[i]);
            }

            return LCPsPExtracted;
        }
        public List<ChartPointsPredition> ExtractTestContinuousTendency(List<ChartPointsPredition> LCPsPAll)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            int iDeepMax = 6;

            for (int i = iDeepMax + 1; i < LCPsPAll.Count; i++)
            {
                int iDeep = -1;
                bool bPass = true;

                while (++iDeep < iDeepMax && bPass != false)
                {

                    ChartPointsPredition CPsPPrevious = LCPsPAll[i - 1 - iDeep];
                    ChartPointsPredition CPsPNow = LCPsPAll[i - iDeep];

                    double dTimeSpread = (CPsPNow.DTOriginal - CPsPPrevious.DTOriginal).TotalMinutes;
                    int iTFMinutes = ABBREVIATIONS.ToMinutes(CPsPNow.TimeFrame);


                    if (dTimeSpread > iTFMinutes)
                        bPass = false;
                    else
                        if ((CPsPNow.Type == ChartPointsPredition.Kind.Down && CPsPNow.Tendency >= CPsPPrevious.Tendency) ||
                        (CPsPNow.Type == ChartPointsPredition.Kind.Up && CPsPNow.Tendency <= CPsPPrevious.Tendency))
                            bPass = false;
                }

                if (bPass)
                    LCPsPExtracted.Add(LCPsPAll[i]);
            }

            return LCPsPExtracted;



                

        }


    }
}


/*
public List<ChartPointsPredition> ExtractTendencyToTendencyComplementary(List<ChartPointsPredition> LCPsPAll, ChartPointsPredition CPsPOriginal)
        {
            List<ChartPointsPredition> LCPsPExtracted = new List<ChartPointsPredition>();

            if (CPsPOriginal.ExtremumTodayDistance == CPsPOriginal.ExtremumConplementaryDistance) return LCPsPExtracted;


            if (CPsPOriginal.TendencyComplementary < 50 && CPsPOriginal.Tendency < 50)
                for (int i = 0; i < LCPsPAll.Count; i++)
                {
                    if (LCPsPAll[i].TendencyComplementary < 50 && LCPsPAll[i].Tendency < 50 && LCPsPAll[i].ExtremumTodayDistance != LCPsPAll[i].ExtremumConplementaryDistance)
                        LCPsPExtracted.Add(LCPsPAll[i]);
                }
            else if (CPsPOriginal.TendencyComplementary > 50 && CPsPOriginal.Tendency > 50 )
                for (int i = 0; i < LCPsPAll.Count; i++)
                    if (LCPsPAll[i].TendencyComplementary > 50 && LCPsPAll[i].Tendency > 50 && LCPsPAll[i].ExtremumTodayDistance != LCPsPAll[i].ExtremumConplementaryDistance)
                        LCPsPExtracted.Add(LCPsPAll[i]);

            return LCPsPExtracted;
        }
*/