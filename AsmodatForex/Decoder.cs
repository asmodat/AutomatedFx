//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using System.Threading;
//using System.Globalization;

//namespace AsmodatForex
//{
//    //Data exmple
//    /*
//     * "S22\\AUD/CAD\\0.95542\\0.95568\\0.95869\\0.95125\\D\\A\\5\\0.95658\\$35\\AUD/CHF\\0.81879\\0.81914\\0.82049\\0.81495\\D\\A\\5\\0.81632\\$15\\AUD/JPY\\96.113\\96.134\\96.458\\95.629\\D\\A\\3\\95.672\\$24\\AUD/NZD\\1.03807\\1.03834\\1.04370\\1.03525\\D\\A\\5\\1.04106\\$8\\AUD/USD\\0.80822\\0.80838\\0.80912\\0.80321\\D\\A\\5\\0.80804\\$56\\AUX/AUD\\5331\\5333\\5348\\5270\\D\\A\\0\\5289\\$6\\BCO/USD\\50.95\\51.00\\51.86\\49.65\\D\\A\\2\\51.05\\$36\\CAD/CHF\\0.85692\\0.85731\\0.85946\\0.85202\\D\\A\\5\\0.85304\\$21\\CAD/JPY\\100.590\\100.612\\101.191\\99.985\\D\\A\\3\\99.999\\$29\\CHF/JPY\\117.360\\117.384\\117.977\\116.965\\D\\A\\3\\117.160\\$76\\COR/USD\\3.9575\\3.9650\\4.0750\\3.9575\\R\\A\\4\\4.0450\\$77\\COT/USD\\0.6022\\0.6047\\0.6062\\0.5971\\R\\A\\4\\0.6000\\$75\\CUU/USD\\2.7610\\2.7635\\2.7775\\2.7470\\D\\A\\4\\2.7620\\$61\\ETX/EUR\\3045\\3049\\3059\\2994\\D\\A\\0\\2994\\$19\\EUR/AUD\\1.46624\\1.46651\\1.47350\\1.46388\\D\\A\\5\\1.47028\\$16\\EUR/CAD\\1.40103\\1.40133\\1.40946\\1.39753\\D\\A\\5\\1.40692\\$10\\EUR/CHF\\1.20086\\1.20107\\1.20143\\1.20067\\D\\A\\5\\1.20094\\$73\\EUR/CZK\\27.8800\\27.8840\\27.8908\\27.6065\\D\\A\\4\\27.6460\\$20\\EUR/DKK\\7.44067\\7.44117\\7.44245\\7.44015\\D\\A\\5\\7.44117\\$12\\EUR/GBP\\0.78377\\0.78389\\0.78544\\0.78142\\D\\A\\5\\0.78453\\$71\\EUR/HUF\\318.286\\318.681\\320.972\\318.263\\D\\A\\3\\319.156\\$5\\EUR/JPY\\140.944\\140.969\\141.682\\140.549\\D\\A\\3\\140.758\\$37\\EUR/NOK\\9.10651\\9.12291\\9.30538\\9.08770\\D\\A\\5\\9.20374\\$30\\EUR/NZD\\1.52246\\1.52321\\1.53474\\1.52045\\D\\A\\5\\1.53115\\$67\\EUR/PLN\\4.29860\\4.30012\\4.32495\\4.29830\\D\\A\\5\\4.31610\\$38\\EUR/SEK\\9.42490\\9.43610\\9.44039\\9.38504\\D\\A\\5\\9.41044\\$69\\EUR/TRY\\2.74987\\2.75248\\2.76995\\2.74661\\D\\A\\5\\2.75791\\$1\\EUR/USD\\1.18516\\1.18537\\1.18975\\1.18014\\D\\A\\5\\1.18882\\$52\\FRX/EUR\\4140.50\\4142.00\\4156.00\\4079.00\\D\\A\\2\\4083.50\\$14\\GBP/AUD\\1.87059\\1.87096\\1.87839\\1.86804\\D\\A\\5\\1.87362\\$25\\GBP/CAD\\1.78735\\1.78780\\1.79676\\1.78373\\D\\A\\5\\1.79284\\$26\\GBP/CHF\\1.53192\\1.53228\\1.53714\\1.52855\\D\\A\\5\\1.52998\\$9\\GBP/JPY\\179.811\\179.850\\180.877\\179.074\\D\\A\\3\\179.303\\$31\\GBP/NZD\\1.94231\\1.94295\\1.95600\\1.93994\\D\\A\\5\\1.95120\\$2\\GBP/USD\\1.51202\\1.51225\\1.51566\\1.50537\\D\\A\\5\\1.51488\\$51\\GRX/EUR\\9582.50\\9585.00\\9612.50\\9463.50\\D\\A\\2\\9463.25\\$78\\HGO/USD\\1.6973\\1.7003\\1.7271\\1.6697\\D\\A\\4\\1.7061\\$60\\HKX/HKD\\23721\\23746\\23804\\23290\\D\\E\\0\\23373\\$57\\JPX/JPY\\16980\\16995\\17175\\16670\\D\\E\\0\\16685\\$59\\NSX/USD\\4143.00\\4144.50\\4164.00\\4098.23\\D\\A\\2\\4099.23\\$79\\NTG/USD\\2.880\\2.897\\3.021\\2.818\\D\\A\\3\\2.936\\$39\\NZD/CAD\\0.91982\\0.92057\\0.92226\\0.91347\\D\\A\\5\\0.91805\\$40\\NZD/CHF\\0.78836\\0.78894\\0.79003\\0.78241\\D\\A\\5\\0.78348\\$27\\NZD/JPY\\92.535\\92.571\\92.806\\91.791\\D\\A\\3\\91.841\\$17\\NZD/USD\\0.77828\\0.77851\\0.77961\\0.77128\\D\\A\\5\\0.77574\\$43\\SGD/JPY\\88.860\\88.911\\89.346\\88.679\\D\\A\\3\\88.753\\$80\\SOY/USD\\10.5550\\10.5625\\10.6125\\10.4800\\R\\A\\4\\10.5575\\$50\\SPX/USD\\2014.38\\2014.88\\2023.88\\1994.38\\D\\A\\2\\1994.63\\$81\\SUG/USD\\0.1467\\0.1472\\0.1501\\0.1460\\R\\A\\4\\0.1483\\$58\\UDX/USD\\17459\\17463\\17524\\17287\\D\\A\\0\\17289\\$49\\UKX/GBP\\6377.00\\6378.50\\6399.50\\6302.00\\D\\A\\2\\6302.00\\$11\\USD/CAD\\1.18203\\1.18224\\1.18766\\1.18124\\D\\E\\5\\1.18352\\$13\\USD/CHF\\1.01313\\1.01332\\1.01764\\1.00940\\D\\E\\5\\1.01008\\$107\\USD/CNH\\6.2147\\6.2176\\6.2220\\6.2078\\D\\E\\4\\6.2101\\$72\\USD/CZK\\23.5205\\23.5295\\23.6145\\23.2380\\D\\E\\4\\23.2475\\$18\\USD/DKK\\6.27753\\6.27821\\6.30483\\6.25470\\D\\E\\5\\6.25838\\$23\\USD/HKD\\7.75530\\7.75560\\7.75574\\7.75360\\D\\E\\5\\7.75410\\$70\\USD/HUF\\268.609\\268.936\\270.787\\268.257\\D\\E\\3\\268.435\\$3\\USD/JPY\\118.918\\118.934\\119.655\\118.401\\D\\E\\3\\118.390\\$46\\USD/MXN\\14.71923\\14.72320\\14.91828\\14.68550\\D\\E\\5\\14.88810\\$32\\USD/NOK\\7.68221\\7.69767\\7.84675\\7.66365\\D\\E\\5\\7.74075\\$66\\USD/PLN\\3.62615\\3.62788\\3.64951\\3.62304\\D\\E\\5\\3.63075\\$33\\USD/SEK\\7.95264\\7.95795\\7.99365\\7.91252\\D\\E\\5\\7.91632\\$34\\USD/SGD\\1.33768\\1.33828\\1.34206\\1.33249\\D\\E\\5\\1.33272\\$68\\USD/TRY\\2.31968\\2.32157\\2.33261\\2.31811\\D\\E\\5\\2.32188\\$47\\USD/ZAR\\11.67850\\11.68813\\11.74656\\11.67260\\D\\E\\5\\11.70150\\$82\\WHT/USD\\5.7875\\5.7950\\5.9475\\5.7825\\R\\A\\4\\5.9175\\$7\\WTI/USD\\48.61\\48.66\\49.33\\46.81\\D\\A\\2\\48.01\\$28\\XAG/USD\\16.581\\16.611\\16.670\\16.280\\D\\A\\3\\16.495\\$42\\XAU/AUD\\1503.81\\1504.42\\1512.87\\1499.25\\D\\A\\2\\1506.77\\$44\\XAU/CHF\\1231.51\\1232.12\\1237.40\\1225.49\\D\\A\\2\\1230.47\\$41\\XAU/EUR\\1025.39\\1026.00\\1030.36\\1020.26\\D\\A\\2\\1024.62\\$45\\XAU/GBP\\803.69\\804.30\\807.74\\799.09\\D\\A\\2\\803.92\\$4\\XAU/USD\\1215.51\\1215.96\\1218.98\\1208.90\\D\\A\\2\\1218.24\\$74\\ZAR/JPY\\10.167\\10.193\\10.237\\10.089\\D\\A\\3\\10.084\\$\r"
//     */
//    public class Decoder
//    {
//        public bool Rates(List<string> SLDecodedData)//, ref OpenRatesBlotter ORBlotter)
//        {

//            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
//            bool ResponseRecived = false;

//            int count = SLDecodedData.Count;
//            int KEY = -1;
//            bool BlockFound = false;
//            bool UpdateFound = false;
//            int position = -1;

//            int i;
//            for (i = 0; i < count; i++)
//            {
//                string PART = SLDecodedData[i];
//                int length = PART.Length;
//                BlockFound = false;
//                UpdateFound = false;

//                if (PART.Contains("BUP") || PART.Contains("DEAL") || PART.Contains("ORD"))
//                {

//                }


//                if ((position = PART.IndexOf("\r")) >= 0)
//                {
//                    PART = PART.Substring(position + 1, length - (position + 1));
//                    length = PART.Length;
//                    ResponseRecived = true;
//                }



//                if (length == 0) continue;

//                if (!(PART.Contains("/") && length == 7) && (PART[0] == 'S' || PART[0] == '$' || PART[0] == 'R')) //Delimiters !PART.Contains("/") &&
//                {

//                    string SUB = PART.Substring(1, length - 1);
//                    int CCY_Token = int.Parse(SUB);

//                    if (PART.Contains("R")) UpdateFound = true; else BlockFound = true;

//                    KEY = CCY_Token;
//                }


//                if (BlockFound)
//                {
//                    if (SLDecodedData.Count <= (i + 9)) break;

//                    Rate RATE = new Rate();

//                    RATE.CCY_Token = KEY;
//                    RATE.CCY_Pair = SLDecodedData[i + 1];
//                    RATE.BID = double.Parse(SLDecodedData[i + 2]);
//                    RATE.ASK = double.Parse(SLDecodedData[i + 3]);
//                    RATE.HIGH = double.Parse(SLDecodedData[i + 4]);
//                    RATE.LOW = double.Parse(SLDecodedData[i + 5]);
//                    RATE.Dealable = false;
//                    RATE.American = false;
//                    if (SLDecodedData[i + 6] == "D") RATE.Dealable = true;
//                    if (SLDecodedData[i + 7] == "A") RATE.American = true;
//                    RATE.Decimals = int.Parse(SLDecodedData[i + 8]);
//                    RATE.CLOSE = double.Parse(SLDecodedData[i + 9]);

//                    ORBlotter.Add(RATE);

//                    i += 9;
//                }
//                else if (UpdateFound)
//                {
//                    if (SLDecodedData.Count <= (i + 4)) break;

//                    double BID = double.Parse(SLDecodedData[i + 1]);
//                    double ASK = double.Parse(SLDecodedData[i + 2]);
//                    bool Dealable = false;
//                    if (SLDecodedData[i + 3] == "D") Dealable = true;
//                    DateTime Time;
//                    try
//                    {
//                        Time = DateTime.ParseExact(SLDecodedData[i + 4], "MM/dd/yyyy HH:mm:ss", null);// "02/06/2014 11:39:01"
//                    }
//                    catch
//                    {
//                        try
//                        {
//                            Time = DateTime.ParseExact(SLDecodedData[i + 9], "MM/dd/yyyy HH:mm:ss", null);// "02/06/2014 11:39:01"
//                        }
//                        catch
//                        {
//                            i += 1;
//                            break;
//                        }
//                    }

//                    Rates RATE = ORBlotter.Get(KEY);

//                    RATE.BID = BID;
//                    RATE.ASK = ASK;
//                    RATE.Dealable = Dealable;
//                    RATE.Time = Time;

//                    ORBlotter.Add(RATE);

//                    i += 4;
//                }
//            }

//            return ResponseRecived;
//        }
//    }
//}
