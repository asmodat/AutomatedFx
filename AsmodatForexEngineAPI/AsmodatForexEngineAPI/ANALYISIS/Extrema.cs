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
        
        public int ExtremumComplementaryPosition(List<ChartPoint> LCPoints, int count, double minShift)
        {
            List<int[]> LIAExtrema = new List<int[]>();

            int iPosition = 0;
            int[] IAExtremaNow;
            while(true)
            {
                IAExtremaNow = new int[2]; int iCount = count - iPosition;
                IAExtremaNow = this.ExtremaPositions(LCPoints, iPosition, iCount);

                double iShift = count - Math.Max(IAExtremaNow[0], IAExtremaNow[1]);

                if (IAExtremaNow[0] != IAExtremaNow[1] && iShift > minShift)
                {
                    iPosition = Math.Max(IAExtremaNow[0], IAExtremaNow[1]);
                    LIAExtrema.Add(IAExtremaNow);
                }
                else break;
            }

            int iLIAECount = LIAExtrema.Count();
            if (iLIAECount <= 1)
            {
                return -1;
            }
            else
            {

                bool bMaximum = false;
                if (LIAExtrema.Last()[0] < LIAExtrema.Last()[1])
                    bMaximum = true;

                if (bMaximum)
                    return LIAExtrema[iLIAECount - 2][1];
                else return LIAExtrema[iLIAECount - 2][0];

            }
        }

        public int[] ExtremaPositions(List<ChartPoint> LCPoints, int startPosition, int count)
        {
            double dMin = double.MaxValue;
            double dMax = double.MinValue;
            int iPositionMax = -1;
            int iPositionMin = -1;
            int iCount = (startPosition + count);

            ChartPoint CPoint;

            //Find last index of global minumum or maximum
            for (int i = startPosition; i < iCount; i++)
            {
                CPoint = LCPoints[i];
                double valueL = CPoint.LOW;
                double valueH = CPoint.HIGH;
                if (valueL <= dMin) { dMin = valueL; iPositionMin = i; }
                if (valueH >= dMax) { dMax = valueH; iPositionMax = i;}
            }

            return new int[2] { iPositionMin, iPositionMax };
        }

       
    
    }
}
