using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatForexEngineAPI.com.efxnow.demoweb.tradingservice;
using AsmodatForexEngineAPI.com.efxnow.democharting.chartingservice;

namespace AsmodatForexEngineAPI
{
    public partial class  Analysis
    {
        //should return expecting gains percentage time and duration
        public void Gains()
        {
            foreach (string product in ARCHIVE.GetProducts()) 
            {
                TimeFrame TFrame = TimeFrame.FIFTEEN_MINUTE;
                List<ChartPoint> LCPoints = ARCHIVE.GetDATA(TFrame, product);
                List<ChartPoint> LCPSelected = new List<ChartPoint>(from CP in LCPoints where CP.Change > 0 select CP);

                double v = this.AverageGainStreak(LCPSelected, TFrame);
                double v2 = this.SampleStandardDeviationGainStreak(LCPSelected, TFrame);
            }
        }


        public double AverageGainStreak(List<ChartPoint> LCPoints, TimeFrame TFrame)
        {
            double sum = 0;
            int iTimeShift = ABBREVIATIONS.ToMinutes(TFrame);

            int counter = 0;
            int antyCounter = 0;
            for (int i = 0; i < LCPoints.Count - 1; i++)
            {

                if(LCPoints[i].Time.AddMinutes(iTimeShift) == LCPoints[i+1].Time)
                    ++counter;
                else if (counter != 0)
                {
                    sum += counter;
                    counter = 0;
                    ++antyCounter;
                }
                else
                    ++antyCounter;
                
                ++i;
            }

            return sum / antyCounter;
        }

        public double SampleStandardDeviationGainStreak(List<ChartPoint> LCPoints, TimeFrame TFrame)
        {
            int iTimeShift = ABBREVIATIONS.ToMinutes(TFrame);
            double dAverangePeak = this.AverageGainStreak(LCPoints, TFrame);
            double dMeanSquareSum = 0;

            int counter = 0;
            int antyCounter = 0;
            for (int i = 0; i < LCPoints.Count - 1; i++)
            {

                if (LCPoints[i].Time.AddMinutes(iTimeShift) == LCPoints[i + 1].Time)
                    ++counter;
                else if (counter != 0)
                {
                    dMeanSquareSum += Math.Pow(counter - dAverangePeak, 2);
                    counter = 0;
                    ++antyCounter;
                }
                else
                {
                    dMeanSquareSum += Math.Pow(counter - dAverangePeak, 2);
                    ++antyCounter;
                }

                ++i;
            }

            return Math.Pow(dMeanSquareSum / (antyCounter - 1), 0.5);
        }

    }
}
