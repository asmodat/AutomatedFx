using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Collections.Concurrent;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

using MathNet.Numerics.LinearAlgebra.Double;

namespace AsmodatForexEngineAPI
{
    public partial class Analysis
    {
        
        public double Tendency(List<Rates> LRates, int deep)
        {
            int iCount = LRates.Count;
            List<Rates> LRSub = LRates.GetRange(iCount - deep, deep);

            List<DateTime> LDTime = (from R in LRates select R.Time).ToList();
            List<double> LDSetHigh = (from R in LRates select R.ASK).ToList();
            List<double> LDSetLow = (from R in LRates select R.BID).ToList();

            return this.Tendency2(LDSetHigh, LDSetLow, LDTime); //TODO: FIX
        }


        public double Tendency(List<ChartPoint> LCPoints, bool highOnly)
        {
            double dChangePoint = 0;
            int iCount = LCPoints.Count;
            List<double> LDSetHigh = new List<double>();
            List<double> LDSetLow = new List<double>();
            List<DateTime> LDTime = new List<DateTime>();

            for (int i = 0; i < iCount; i++)
            {
                double dPeak = dChangePoint + LCPoints[i].Peak;
                double dBase = dChangePoint - LCPoints[i].Base;
                LDSetHigh.Add(dPeak);
                LDSetLow.Add(dBase);
                LDTime.Add(LCPoints[i].Time);

                dChangePoint += LCPoints[i].Change;
            }

            double dTendency = 50;

            if (highOnly)
                dTendency = this.Tendency(LDSetHigh, LDTime);
            else dTendency = this.Tendency(LDSetLow, LDTime);

            if(dTendency == double.NaN)
            {
                return 50;
            }

            return dTendency;
        }

        public double Tendency(List<ChartPoint> LCPoints)
        {
            double dChangePoint = 0;
            int iCount = LCPoints.Count;
            List<double> LDSetHigh = new List<double>();
            List<double> LDSetLow = new List<double>();
            List<DateTime> LDTime = new List<DateTime>();

            for (int i = 0; i < iCount; i++)
            {
                double dPeak = dChangePoint + LCPoints[i].Peak;
                double dBase = dChangePoint - LCPoints[i].Base;
                LDSetHigh.Add(dPeak);
                LDSetLow.Add(dBase);
                LDTime.Add(LCPoints[i].Time);

                dChangePoint += LCPoints[i].Change;
            }

            return this.Tendency2(LDSetHigh, LDSetLow, LDTime);
        }
        public double Tendency(List<double> LDHigh, List<double> LDLow)
        {
            int iCount = LDHigh.Count;
            List<DateTime> LDTime = new List<DateTime>();
            DateTime DTStart = DateTime.Now;

            for (int i = 0; i < iCount; i++)
                LDTime.Add(DTStart.AddMinutes(i));

            return this.Tendency2(LDHigh, LDLow, LDTime);
        }
        public double Tendency(List<double> LDSet)
        {
            int iCount = LDSet.Count;
            List<DateTime> LDTime = new List<DateTime>();
            DateTime DTStart = DateTime.Now;

            for (int i = 0; i < iCount; i++)
                LDTime.Add(DTStart.AddMinutes(i));

            return this.Tendency(LDSet, LDTime);
        }
        public double TendencyLastExtremum(List<ChartPoint> LCPoints, out double distance)//it must be a set of changes
        {
            List<double> LDChanges = (from CP in LCPoints select CP.Change).ToList();


            int[] IAExtrema = this.ExtremaPositions(LCPoints, 0, LCPoints.Count);

            int iPosition = Math.Max(IAExtrema[0], IAExtrema[1]);

            if (iPosition <= 0 || iPosition >= (LDChanges.Count - 20))
            {
                distance = -1;
                return 50;
            }

            distance = (LCPoints.Last().Time - LCPoints[iPosition].Time).TotalMinutes;
        
            List<ChartPoint> LDCPRanged = LCPoints.GetRange(iPosition, LCPoints.Count - iPosition);
            return this.Tendency(LDCPRanged);
        }


        public double Tendency(List<double> LDHigh, List<double> LDLow, List<DateTime> LDTime)
        {
            int iCount = LDHigh.Count;

            if (iCount < 2 || iCount != LDLow.Count || iCount != LDTime.Count) return 50;

            double[] DAX = new double[iCount];
            double[] DAY = new double[iCount];

            double dOriginY = ((double)(LDHigh[0] + LDLow[0]) / 2); // Start of XY Axis
            DateTime DTOriginX = LDTime[0];
            double dMaxX = (LDTime[iCount - 1] - DTOriginX).TotalSeconds;
            double dMaxY = this.MaxTendencyValue(LDHigh, LDLow);

            for (int i = 0; i < iCount; i++)
            {
                DAX[i] = (double)(LDTime[i] - DTOriginX).TotalSeconds / dMaxX;
                DAY[i] = (double)(((double)(LDHigh[i] + LDLow[i]) / 2) - dOriginY) / dMaxY;
            }


            var DMY = new DenseMatrix(iCount, 1, DAY);
            var DMX = new DenseMatrix(iCount, 1, DAX);
            var LRMA = DMX.QR().Solve(DMY);


            double dAngle = Math.Atan(LRMA[0, 0]) * 180 / Math.PI;
            double dTendency = dTendency = ((double)((dAngle / 90) * 100) / 2) + 50;


            if (dAngle >= 90 || dTendency >= 100 || dAngle <= -90 || dTendency <= 0)
            {
                int error = 0;
                int e2 = error;
            }

            return dTendency;
        }
        public double Tendency2(List<double> LDHigh, List<double> LDLow, List<DateTime> LDTime)
        {
            double dTendencyHigh = this.Tendency(LDHigh, LDTime);
            double dTendencyLow = this.Tendency(LDLow, LDTime);
            double dTendency = 50;

            if (dTendencyHigh == 50 || dTendencyLow == 50) return 50;

            if(dTendencyHigh > 50 && dTendencyLow > 50 && dTendencyLow <= dTendencyHigh)
                dTendency = (double)(dTendencyHigh + dTendencyLow) / 2;
            else if (dTendencyHigh < 50 && dTendencyLow < 50 && dTendencyLow >= dTendencyHigh)
                dTendency = (double)(dTendencyHigh + dTendencyLow) / 2;
            

            return dTendency;
        }


        public double Tendency(List<double> LDSet, List<DateTime> LDTime)
        {
            int iCount = LDSet.Count;

            if (iCount < 2 || iCount != LDTime.Count) return 50;

            double[] DAX = new double[iCount];
            double[] DAY = new double[iCount];

            double dOriginY = 0;// LDSet[0]; // Start of XY Axis
            DateTime DTOriginX = LDTime[0];
            double dMaxX = (LDTime[iCount - 1] - DTOriginX).TotalSeconds;
            double dMaxY = this.MaxTendencyValue(LDSet);

            for (int i = 0; i < iCount; i++)
            {
                DAX[i] = (double)(LDTime[i] - DTOriginX).TotalSeconds / dMaxX;

                double dValue = (double)(LDSet[i] - dOriginY) / dMaxY;

                if (dValue != double.NaN && dValue != double.PositiveInfinity && dValue != double.NegativeInfinity)
                    DAY[i] = (double)(LDSet[i] - dOriginY) / dMaxY;
                else DAY[i] = 0;
            }


            var DMY = new DenseMatrix(iCount, 1, DAY);
            var DMX = new DenseMatrix(iCount, 1, DAX);
            var LRMA = DMX.QR().Solve(DMY);


            double dAngle = Math.Atan(LRMA[0, 0]) * 180 / Math.PI;
            double dTendency = dTendency = ((double)((dAngle / 90) * 100) / 2) + 50;


            if (dAngle >= 90 || dTendency >= 100 || dAngle <= -90 || dTendency <= 0)
            {
                int error = 0;
                int e2 = error;
            }

            return dTendency;
        }



        public double MaxTendencyValue(List<double> LDHigh, List<double> LDLow)
        {
            int iCount = LDHigh.Count;

            if (iCount < 2 || iCount != LDLow.Count) throw new Exception("Error in finding max value.");

            double dOriginY = ((double)(LDHigh[0] + LDLow[0]) / 2); // Start of XY Axis

            double dMaxValue = double.MinValue;
            for (int i = 0; i < iCount; i++)
            {

                double dValueNow = Math.Abs(((double)(LDHigh[i] + LDLow[i]) / 2) - dOriginY);
                if (dValueNow > dMaxValue)
                    dMaxValue = dValueNow;
            }

            return dMaxValue;
        }
        public double MaxTendencyValue(List<double> LDSet)
        {
            int iCount = LDSet.Count;

            if (iCount < 2) throw new Exception("Error in finding max value.");

            double dOriginY = 0;//LDSet[0]; // Start of XY Axis

            double dMaxValue = double.MinValue;
            for (int i = 0; i < iCount; i++)
            {

                double dValueNow = Math.Abs(LDSet[i] - dOriginY);
                if (dValueNow > dMaxValue)
                    dMaxValue = dValueNow;
            }

            return dMaxValue;
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

using MathNet.Numerics.LinearAlgebra.Double;

namespace AsmodatForexEngineAPI
{
    public partial class Analysis
    {
        
        public double Tendency(List<Rates> LRates, int deep)
        {
            int iCount = LRates.Count;
            List<Rates> LRSub = LRates.GetRange(iCount - deep, deep);

            List<DateTime> LDTime = (from R in LRates select R.Time).ToList();
            List<double> LDSetHigh = (from R in LRates select R.ASK).ToList();
            List<double> LDSetLow = (from R in LRates select R.BID).ToList();

            return this.Tendency2(LDSetHigh, LDSetLow, LDTime); //TODO: FIX
        }


        public double Tendency(List<ChartPoint> LCPoints, bool highOnly)
        {
            double dChangePoint = 0;
            int iCount = LCPoints.Count;
            List<double> LDSetHigh = new List<double>();
            List<double> LDSetLow = new List<double>();
            List<DateTime> LDTime = new List<DateTime>();

            for (int i = 0; i < iCount; i++)
            {
                double dPeak = dChangePoint + LCPoints[i].Peak;
                double dBase = dChangePoint - LCPoints[i].Base;
                LDSetHigh.Add(dPeak);
                LDSetLow.Add(dBase);
                LDTime.Add(LCPoints[i].Time);

                dChangePoint += LCPoints[i].Change;
            }

            double dTendency = 50;

            if (highOnly)
                dTendency = this.Tendency(LDSetHigh, LDTime);
            else dTendency = this.Tendency(LDSetLow, LDTime);

            if(dTendency == double.NaN)
            {
                return 50;
            }

            return dTendency;
        }

        public double Tendency(List<ChartPoint> LCPoints)
        {
            double dChangePoint = 0;
            int iCount = LCPoints.Count;
            List<double> LDSetHigh = new List<double>();
            List<double> LDSetLow = new List<double>();
            List<DateTime> LDTime = new List<DateTime>();

            for (int i = 0; i < iCount; i++)
            {
                double dPeak = dChangePoint + LCPoints[i].Peak;
                double dBase = dChangePoint - LCPoints[i].Base;
                LDSetHigh.Add(dPeak);
                LDSetLow.Add(dBase);
                LDTime.Add(LCPoints[i].Time);

                dChangePoint += LCPoints[i].Change;
            }

            return this.Tendency2(LDSetHigh, LDSetLow, LDTime);
        }
        public double Tendency(List<double> LDHigh, List<double> LDLow)
        {
            int iCount = LDHigh.Count;
            List<DateTime> LDTime = new List<DateTime>();
            DateTime DTStart = DateTime.Now;

            for (int i = 0; i < iCount; i++)
                LDTime.Add(DTStart.AddMinutes(i));

            return this.Tendency2(LDHigh, LDLow, LDTime);
        }
        public double Tendency(List<double> LDSet)
        {
            int iCount = LDSet.Count;
            List<DateTime> LDTime = new List<DateTime>();
            DateTime DTStart = DateTime.Now;

            for (int i = 0; i < iCount; i++)
                LDTime.Add(DTStart.AddMinutes(i));

            return this.Tendency(LDSet, LDTime);
        }
        public double TendencyLastExtremum(List<ChartPoint> LCPoints, out double distance)//it must be a set of changes
        {
            List<double> LDChanges = (from CP in LCPoints select CP.Change).ToList();


            int[] IAExtrema = this.ExtremaPositions(LCPoints, 0, LCPoints.Count);

            int iPosition = Math.Max(IAExtrema[0], IAExtrema[1]);

            if (iPosition <= 0 || iPosition >= (LDChanges.Count - 20))
            {
                distance = -1;
                return 50;
            }

            distance = (LCPoints.Last().Time - LCPoints[iPosition].Time).TotalMinutes;
        
            List<ChartPoint> LDCPRanged = LCPoints.GetRange(iPosition, LCPoints.Count - iPosition);
            return this.Tendency(LDCPRanged);
        }


        public double Tendency(List<double> LDHigh, List<double> LDLow, List<DateTime> LDTime)
        {
            int iCount = LDHigh.Count;

            if (iCount < 2 || iCount != LDLow.Count || iCount != LDTime.Count) return 50;

            double[] DAX = new double[iCount];
            double[] DAY = new double[iCount];

            double dOriginY = ((double)(LDHigh[0] + LDLow[0]) / 2); // Start of XY Axis
            DateTime DTOriginX = LDTime[0];
            double dMaxX = (LDTime[iCount - 1] - DTOriginX).TotalSeconds;
            double dMaxY = this.MaxTendencyValue(LDHigh, LDLow);

            for (int i = 0; i < iCount; i++)
            {
                DAX[i] = (double)(LDTime[i] - DTOriginX).TotalSeconds / dMaxX;
                DAY[i] = (double)(((double)(LDHigh[i] + LDLow[i]) / 2) - dOriginY) / dMaxY;
            }


            var DMY = new DenseMatrix(iCount, 1, DAY);
            var DMX = new DenseMatrix(iCount, 1, DAX);
            var LRMA = DMX.QR().Solve(DMY);


            double dAngle = Math.Atan(LRMA[0, 0]) * 180 / Math.PI;
            double dTendency = dTendency = ((double)((dAngle / 90) * 100) / 2) + 50;


            if (dAngle >= 90 || dTendency >= 100 || dAngle <= -90 || dTendency <= 0)
            {
                int error = 0;
                int e2 = error;
            }

            return dTendency;
        }
        public double Tendency2(List<double> LDHigh, List<double> LDLow, List<DateTime> LDTime)
        {
            double dTendencyHigh = this.Tendency(LDHigh, LDTime);
            double dTendencyLow = this.Tendency(LDLow, LDTime);
            double dTendency = 50;

            if (dTendencyHigh == 50 || dTendencyLow == 50) return 50;

            if(dTendencyHigh > 50 && dTendencyLow > 50 && dTendencyLow <= dTendencyHigh)
                dTendency = (double)(dTendencyHigh + dTendencyLow) / 2;
            else if (dTendencyHigh < 50 && dTendencyLow < 50 && dTendencyLow >= dTendencyHigh)
                dTendency = (double)(dTendencyHigh + dTendencyLow) / 2;
            

            return dTendency;
        }


        public double Tendency(List<double> LDSet, List<DateTime> LDTime)
        {
            int iCount = LDSet.Count;

            if (iCount < 2 || iCount != LDTime.Count) return 50;

            double[] DAX = new double[iCount];
            double[] DAY = new double[iCount];

            double dOriginY = LDSet[0]; // Start of XY Axis
            DateTime DTOriginX = LDTime[0];
            double dMaxX = (LDTime[iCount - 1] - DTOriginX).TotalSeconds;
            double dMaxY = this.MaxTendencyValue(LDSet);

            for (int i = 0; i < iCount; i++)
            {
                DAX[i] = (double)(LDTime[i] - DTOriginX).TotalSeconds / dMaxX;

                double dValue = (double)(LDSet[i] - dOriginY) / dMaxY;

                if (dValue != double.NaN && dValue != double.PositiveInfinity && dValue != double.NegativeInfinity)
                    DAY[i] = (double)(LDSet[i] - dOriginY) / dMaxY;
                else DAY[i] = 0;
            }


            var DMY = new DenseMatrix(iCount, 1, DAY);
            var DMX = new DenseMatrix(iCount, 1, DAX);
            var LRMA = DMX.QR().Solve(DMY);


            double dAngle = Math.Atan(LRMA[0, 0]) * 180 / Math.PI;
            double dTendency = dTendency = ((double)((dAngle / 90) * 100) / 2) + 50;


            if (dAngle >= 90 || dTendency >= 100 || dAngle <= -90 || dTendency <= 0)
            {
                int error = 0;
                int e2 = error;
            }

            return dTendency;
        }



        public double MaxTendencyValue(List<double> LDHigh, List<double> LDLow)
        {
            int iCount = LDHigh.Count;

            if (iCount < 2 || iCount != LDLow.Count) throw new Exception("Error in finding max value.");

            double dOriginY = ((double)(LDHigh[0] + LDLow[0]) / 2); // Start of XY Axis

            double dMaxValue = double.MinValue;
            for (int i = 0; i < iCount; i++)
            {

                double dValueNow = Math.Abs(((double)(LDHigh[i] + LDLow[i]) / 2) - dOriginY);
                if (dValueNow > dMaxValue)
                    dMaxValue = dValueNow;
            }

            return dMaxValue;
        }
        public double MaxTendencyValue(List<double> LDSet)
        {
            int iCount = LDSet.Count;

            if (iCount < 2) throw new Exception("Error in finding max value.");

            double dOriginY = LDSet[0]; // Start of XY Axis

            double dMaxValue = double.MinValue;
            for (int i = 0; i < iCount; i++)
            {

                double dValueNow = Math.Abs(LDSet[i] - dOriginY);
                if (dValueNow > dMaxValue)
                    dMaxValue = dValueNow;
            }

            return dMaxValue;
        }
    
    }
}


*/

