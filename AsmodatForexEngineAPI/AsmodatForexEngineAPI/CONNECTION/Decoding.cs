using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Globalization;

namespace AsmodatForexEngineAPI
{
    public class Decoding
    {
        
        public Decoding()
        {

        }


        public List<string> DataRates(string DATA)
        {
            List<string> SLInfo = new List<string>();
            int index = 0;

            while (DATA.Length > 0 && index >= 0)
            {
                index = DATA.IndexOf("\\");
                string PART = "";
                if (index > 0)
                {
                    PART = DATA.Substring(0, index);
                    DATA = DATA.Substring(index + 1, DATA.Length - (index + 1));
                    SLInfo.Add(PART);
                }
                else if (index == 0)
                {
                    DATA = DATA.Substring(1, DATA.Length - 1);
                }
            }
            return SLInfo;
        }

        public List<string> DataChartPoints(string DATA)
        {
            List<string> SLInfo = new List<string>();
            int index = 0;

            while (DATA.Length > 0 && index >= 0)
            {
                index = DATA.IndexOf("\\");
                string PART = "";
                if (index > 0)
                {
                    PART = DATA.Substring(0, index);
                    DATA = DATA.Substring(index + 1, DATA.Length - (index + 1));

                    if (PART.Contains("$"))
                    {
                        int index2 = PART.IndexOf("$");
                        string PART1 = PART.Substring(0, index2);
                        string PART2 = PART.Substring(index2, PART.Length - index2);

                        if (PART1 != null && PART1 != "")
                            SLInfo.Add(PART1);
                        if (PART2 != null && PART2 != "")
                            SLInfo.Add(PART2);
                    }
                    else
                    {
                        SLInfo.Add(PART);
                    }
                }
                else if (index == 0)
                {
                    DATA = DATA.Substring(1, DATA.Length - 1);
                }
                else if(index < 1)
                {
                      SLInfo.Add(DATA);
                      break;
                }
            }
            return SLInfo;
        }


        public bool ChartPoints(List<string> SLDecodedData, ref List<ChartPoint> LCPoints)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            bool ResponseRecived = false;

            int count = SLDecodedData.Count;
            bool BlockFound = false;

            int i;
            for (i = 0; i < count; i++)
            {
                string PART = SLDecodedData[i];
                int length = PART.Length;
                BlockFound = false;


                if (length == 0) continue;

                if ((PART.Contains("/") && i == 0) || (PART[0] == '$')) //Delimiters 
                {
                    SLDecodedData[i] = SLDecodedData[i].Replace("$","");
                    BlockFound = true;
                    ResponseRecived = true;
                }
                


                if (BlockFound)
                {
                    if (SLDecodedData.Count <= (i + 4)) break;

                    ChartPoint CPoint = new ChartPoint();

                    //"2/06/2014 1:39:01 AM"
                    CPoint.Time = DateTime.ParseExact(SLDecodedData[i], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);// "02/06/2014 11:39:01"//"M'/'d'/'yyyy h':'mm':'ss tt""MM/dd/yyyy h:mm:ss tt"
                    CPoint.OPEN = double.Parse(SLDecodedData[i + 1]);
                    CPoint.HIGH = double.Parse(SLDecodedData[i + 2]);
                    CPoint.LOW = double.Parse(SLDecodedData[i + 3]);
                    CPoint.CLOSE = double.Parse(SLDecodedData[i + 4]);
                    
                    

                    LCPoints.Add(CPoint);

                    i += 4;
                }
            }

            return ResponseRecived;
        }



        public bool Rates(List<string> SLDecodedData, ref OpenRatesBlotter ORBlotter)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            bool ResponseRecived = false;

            int count = SLDecodedData.Count;
            int KEY = -1;
            bool BlockFound = false;
            bool UpdateFound = false;
            int position = -1;

            int i;
            for (i = 0; i < count; i++)
            {
                string PART = SLDecodedData[i];
                int length = PART.Length;
                BlockFound = false;
                UpdateFound = false;

                if (PART.Contains("BUP") || PART.Contains("DEAL") || PART.Contains("ORD"))
                {

                }


                if ((position = PART.IndexOf("\r")) >= 0)
                {
                    PART = PART.Substring(position + 1, length - (position + 1));
                    length = PART.Length;
                    ResponseRecived = true;
                }

                

                if (length == 0) continue;

                if (!(PART.Contains("/") && length == 7) && (PART[0] == 'S' || PART[0] == '$' || PART[0] == 'R')) //Delimiters !PART.Contains("/") &&
                {

                    string SUB = PART.Substring(1, length - 1);
                    int CCY_Token = int.Parse(SUB);

                    if (PART.Contains("R")) UpdateFound = true; else BlockFound = true;

                    KEY = CCY_Token;
                }


                if (BlockFound)
                {
                    if (SLDecodedData.Count <= (i + 9)) break;

                    Rates RATE = new AsmodatForexEngineAPI.Rates();

                    RATE.CCY_Token = KEY;
                    RATE.CCY_Pair = SLDecodedData[i + 1];
                    RATE.BID = double.Parse(SLDecodedData[i + 2]);
                    RATE.ASK = double.Parse(SLDecodedData[i + 3]);
                    RATE.HIGH = double.Parse(SLDecodedData[i + 4]);
                    RATE.LOW = double.Parse(SLDecodedData[i + 5]);
                    RATE.Dealable = false; 
                    RATE.American = false;
                    if (SLDecodedData[i + 6] == "D") RATE.Dealable = true;
                    if (SLDecodedData[i + 7] == "A") RATE.American = true;
                    RATE.Decimals = int.Parse(SLDecodedData[i + 8]);
                    RATE.CLOSE = double.Parse(SLDecodedData[i + 9]);

                    ORBlotter.Add(RATE);

                    i += 9;
                }
                else if (UpdateFound)
                {
                    if (SLDecodedData.Count <= (i + 4)) break;

                    double BID = double.Parse(SLDecodedData[i + 1]);
                    double ASK = double.Parse(SLDecodedData[i + 2]);
                    bool Dealable = false;
                    if (SLDecodedData[i + 3] == "D") Dealable = true;
                    DateTime Time;
                    try
                    {
                        Time = DateTime.ParseExact(SLDecodedData[i + 4], "MM/dd/yyyy HH:mm:ss", null);// "02/06/2014 11:39:01"
                    }
                    catch
                    {
                        try
                        {
                            Time = DateTime.ParseExact(SLDecodedData[i + 9], "MM/dd/yyyy HH:mm:ss", null);// "02/06/2014 11:39:01"
                        }
                        catch 
                        {
                            i += 1;
                            break;
                        }
                    }

                    Rates RATE = ORBlotter.Get(KEY);

                    RATE.BID = BID;
                    RATE.ASK = ASK;
                    RATE.Dealable = Dealable;
                    RATE.Time = Time;

                    ORBlotter.Add(RATE);
                    
                    i += 4;
                }
            }

            return ResponseRecived;
        }



    }
}
