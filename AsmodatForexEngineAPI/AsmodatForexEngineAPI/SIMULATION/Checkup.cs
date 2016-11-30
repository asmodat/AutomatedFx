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

        private bool Checkup(ref ChartPointsPredition CPsP)
        {
            CPsP.TestID = 2;

            if (CPsP.Type == ChartPointsPredition.Kind.Uncertain)
                return false;

            //this.CheckupContinuity(ref CPsP, position);
            //this.CheckupContinuityAlternative(ref CPsP, position);
            //this.CheckupContinuityTendencyChange(ref CPsP, position);
            this.CheckupTendencyToday(ref CPsP);
            this.CheckupTendencyComplementary(ref CPsP);
            this.CheckupStrengthFactor(ref CPsP);

            CPsP.Strike = false;
            CPsP.Accuracity = 0;

            return true;
        }


        private bool CheckupTest(ref ChartPointsPredition CPsP)//, List<ChartPoint> LCPointsToPredict, double spread)
        {
            if (CPsP.Type == ChartPointsPredition.Kind.Uncertain)
                return false;

            List<ChartPoint> LCPointsToPredict = DLSCPoints[CPsP.Product].GetRange(CPsP.Position, CPsP.Ahead);


            this.CheckupTendencyToday(ref CPsP);
           // this.CheckupTendencyComplementary(ref CPsP);
            this.CheckupStrengthFactor(ref CPsP);
            

            bool bStrike = false;

            double dRealTendency = 50;

            if (CPsP.Type == ChartPointsPredition.Kind.Down)
            {
                dRealTendency = ANALYSIS.Tendency(LCPointsToPredict, false);

                if (CPsP.Tendency < 50 && dRealTendency < 50)
                    bStrike = true;
            }
            else if (CPsP.Type == ChartPointsPredition.Kind.Up)
            {
                dRealTendency = ANALYSIS.Tendency(LCPointsToPredict, true);

                if (CPsP.Tendency > 50 && dRealTendency > 50)
                    bStrike = true;
            }

            CPsP.Strike = bStrike;
            //CPsP.Accuracity = Accuracity;
            return true;
        }


        private void CheckupContinuity(ref ChartPointsPredition CPsP, int position)
        {
            CPsP.Continuity = 0;
            int iSeconds = (ABBREVIATIONS.ToMinutes(CPsP.TimeFrame) * 60);

            

                 for(int i = position - 1; i >= 0; i--)
                 {
                     ChartPointsPredition CPsPPrevious = DATA[CPsP.Product][i];
                     if (CPsPPrevious.Type == ChartPointsPredition.Kind.Uncertain) 
                         break;

                     double dSecondsDifference = (CPsP.DTOriginal - CPsPPrevious.DTOriginal).TotalSeconds;
                     double dSecondsDifferenceAllowed = (iSeconds * (position - i)) + 6;

                     if (dSecondsDifference > dSecondsDifferenceAllowed)
                         break;
                     else ++CPsP.Continuity;

                     if (CPsP.Continuity >= CPsP.Deep)
                         break;
                 }


        }
        /// <summary>
        /// Must appear after CheckupContinuity is finished, sets cout of similar alternative types in reference to its original
        /// </summary>
        /// <param name="CPsP">Original ChartPoints Predicion</param>
        /// <param name="position">Position of Original in DATA</param>
        private void CheckupContinuityAlternative(ref ChartPointsPredition CPsP, int position)
        {
            CPsP.ContinuityAlternative = 0;

            List<string> LSOptions = this.Options(CPsP.Product);
            string alternative = LSOptions[1];

            int iAShift = iAShift = this.PositionShift(CPsP.Product, alternative, 0);
            int iACount = DATA[alternative].Count;
            int iPCount = DATA[CPsP.Product].Count;

            for (int i = position; i >= (position - CPsP.Continuity); i--)
            {
                if (iACount <= (i + iAShift) || iACount <= i || (i + iAShift) < 0 || i < 0 || (iPCount <= i))
                    break;

                iAShift = this.PositionShift(CPsP.Product, alternative, i);

                if (iACount <= (i + iAShift) || iACount <= i || (i + iAShift) < 0 || i < 0 || (iPCount <= i))
                    break;

                ChartPointsPredition CPsPOrigins = DATA[CPsP.Product][i];
                ChartPointsPredition CPsPAlternative = DATA[alternative][i + iAShift];

                if (CPsPOrigins.Type != ChartPointsPredition.Kind.Uncertain && CPsPAlternative.Type == CPsPOrigins.Type && CPsPOrigins.DTOriginal == CPsPAlternative.DTOriginal)
                    ++CPsP.ContinuityAlternative;
                else break;

                
                
            }

        }
        private void CheckupContinuityTendencyChange(ref ChartPointsPredition CPsP, int position)
        {
            CPsP.ContinuityTendencyChange = 0;
            double dPreviousTendency = CPsP.Tendency;
            for (int i = position - 1; i >= (position - CPsP.Continuity); i--)
            {
                ChartPointsPredition CPsPPrevious = DATA[CPsP.Product][i];

                if (CPsP.Type == ChartPointsPredition.Kind.Up)
                {
                    if (CPsPPrevious.Tendency < dPreviousTendency)
                    {
                        ++CPsP.ContinuityTendencyChange;
                        dPreviousTendency = CPsPPrevious.Tendency;
                    }
                    else break;
                }
                else if (CPsP.Type == ChartPointsPredition.Kind.Down)
                {
                    if (CPsPPrevious.Tendency > dPreviousTendency)
                    {
                        ++CPsP.ContinuityTendencyChange;
                        dPreviousTendency = CPsPPrevious.Tendency;
                    }
                    else break;
                }
            }

        }
        private void CheckupTendencyToday(ref ChartPointsPredition CPsP)
        {
            List<ChartPoint> LCPToday = ARCHIVE.GetDATA(CPsP.TimeFrame, CPsP.Product, CPsP.DTOriginal.Date, CPsP.DTOriginal);
            //List<double> LDChanges = (from CP in LCPToday select CP.Change).ToList();

            double dExtremumTodayDistance;
            CPsP.TendencyToday = ANALYSIS.TendencyLastExtremum(LCPToday, out dExtremumTodayDistance);//ANALYSIS.Tendency(LDChanges, LDChanges.Count);
            CPsP.ExtremumTodayDistance = dExtremumTodayDistance;
        }
        private void CheckupStrengthFactor(ref ChartPointsPredition CPsP)
        {
            List<ChartPoint> LCPOrigin = ARCHIVE.GetDATA(CPsP.TimeFrame, CPsP.Product, CPsP.Position - CPsP.Deep, CPsP.Deep);
            List<double> LDOPeak = (from CP in LCPOrigin select CP.Peak).ToList();
            List<double> LDOBase = (from CP in LCPOrigin select CP.Peak).ToList();

            CPsP.SetStrengthFactor(LDOPeak, LDOBase);
        }
        private void CheckupTendencyComplementary(ref ChartPointsPredition CPsP)
        {
            List<ChartPoint> LCPToday = ARCHIVE.GetDATA(CPsP.TimeFrame, CPsP.Product, CPsP.DTOriginal.Date, CPsP.DTOriginal);

            int iPosition = ANALYSIS.ExtremumComplementaryPosition(LCPToday, LCPToday.Count, (double)CPsP.Deep / 2);
            //int iPositionBefore = ANALYSIS.ExtremumComplementaryPosition(LCPToday, iPosition + 1, (double)CPsP.Deep / 2);
            int iShiftCount = LCPToday.Count - iPosition;

            if (iShiftCount <= 0 || iPosition >= LCPToday.Count || iPosition - iShiftCount < 0)
            {
                CPsP.ExtremumConplementaryDistance = -1;
                CPsP.TendencyComplementary = 50;
                return;
            }
            else
            {
                //List<double> LDChanges = (from CP in LCPToday select CP.Change).ToList(); int iCount = LDChanges.Count - iPosition; CPsP.TendencyComplementary = ANALYSIS.Tendency(LDChanges, iPosition, iCount);
               
                List<ChartPoint> LDCPRanged = LCPToday.GetRange(iPosition, iShiftCount);
                List<ChartPoint> LDCPRangedBefore = LCPToday.GetRange(iPosition - iShiftCount, iShiftCount);


                double dTendency = ANALYSIS.Tendency(LDCPRanged);
                double dTendencyBefore = ANALYSIS.Tendency(LDCPRangedBefore);

                if (dTendency < 50 && dTendencyBefore > 50 || dTendency > 50 && dTendencyBefore < 50)
                {
                    CPsP.ExtremumConplementaryDistance = -1;
                    CPsP.TendencyComplementary = 50;
                    return;
                }

                CPsP.ExtremumConplementaryDistance = (LDCPRanged.Last().Time - LDCPRanged[0].Time).TotalMinutes;
                CPsP.TendencyComplementary = (dTendency / dTendencyBefore) * 100;
            }
        }

        private int PositionShift(string pPrimary, string pSecondary, int position)
        {
            double dMinutes = (DATA[pPrimary][position].DTOriginal - DATA[pSecondary][position].DTOriginal).TotalMinutes;
            int iShiftMinutes = (int)(dMinutes / ABBREVIATIONS.ToMinutes(DATA[pPrimary][position].TimeFrame));

            return iShiftMinutes;
        }

    }
}


/*
private bool CheckupTest(ref ChartPointsPredition CPsP, List<ChartPoint> LCPointsToPredict, double spread)
        {
            if (CPsP.Type == ChartPointsPredition.Kind.Uncertain)
                return false;

            //this.CheckupContinuity(ref CPsP, position);
            //this.CheckupContinuityAlternative(ref CPsP, position);
            //this.CheckupContinuityTendencyChange(ref CPsP, position);
            this.CheckupTendencyToday(ref CPsP);
            this.CheckupTendencyComplementary(ref CPsP);
            this.CheckupStrengthFactor(ref CPsP);
            

            int iCount = LCPointsToPredict.Count;
            double dChangePoint = 0;
            int ahead = CPsP.Ahead;
            double Accuracity = -1;

            double dLocalPeak = double.MinValue;
            int iLocalPeakPosition = 0;
            double dLocalBase = double.MaxValue;
            int iLocalBasePosition = 0;
            int iExtremum = 0;

            bool bStrike = false;
            bool bStart = false;

            double dRealTendency = ANALYSIS.Tendency(LCPointsToPredict);

            for (int i = 0; i < iCount; i++)
            {

                double dPeak = dChangePoint + LCPointsToPredict[i].Peak;
                double dBase = dChangePoint - LCPointsToPredict[i].Base;

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

                double dExtremumSpread = (dLocalPeak - dLocalBase);
                if (dExtremumSpread > spread &&
                    ((CPsP.Type == ChartPointsPredition.Kind.Down && iLocalPeakPosition < iLocalBasePosition) ||
                    (CPsP.Type == ChartPointsPredition.Kind.Up && iLocalPeakPosition > iLocalBasePosition)))
                {
                    bStrike = true;
                    break;
                }


                dChangePoint += LCPointsToPredict[i].Change;
            }


            if (CPsP.Type == ChartPointsPredition.Kind.Down)
            {
                iExtremum = iLocalBasePosition;
                if (iLocalPeakPosition >= ahead)
                    bStrike = false;

            }
            else if (CPsP.Type == ChartPointsPredition.Kind.Up)
            {
                iExtremum = iLocalPeakPosition;
                if (iLocalBasePosition >= ahead)
                    bStrike = false;
            }




            if (bStrike)
            {
                double dDiff = Math.Abs(CPsP.IndexExtremum - iExtremum);
                Accuracity = (double)(1 - (dDiff / iCount)) * 100;

            }

            CPsP.Strike = bStrike;
            CPsP.Accuracity = Accuracity;

            return true;
        }
*/