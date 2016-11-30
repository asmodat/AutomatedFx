using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;



namespace Asmodat.Abbreviate
{
    public static class Strings
    {

        /// <summary>
        /// Tests similarity between occurance of separate characters count's 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static double Similarity(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;

            
            Dictionary<char,int> dci1 = new Dictionary<char,int>();
            Dictionary<char,int> dci2 = new Dictionary<char,int>();
            Dictionary<char, int> dciTotal = new Dictionary<char, int>();

            int i1 = 0, i2 = 0, i1max = s1.Length, i2max = s2.Length;
            char c;
            int sum = i1max + i2max;

            for (; i1 < i1max; i1++)
            {
                c = s1[i1];
                if (!dci1.ContainsKey(c)) 
                    dci1.Add(c, 1);
                else ++dci1[c];


                if (!dciTotal.ContainsKey(s1[i1]))
                    dciTotal.Add(s1[i1], 1);
                else ++dciTotal[s1[i1]];
            }

            for (; i2 < i2max; i2++)
            {
                c = s2[i2];
                if (!dci2.ContainsKey(c))
                    dci2.Add(c, 1);
                else ++dci2[c];

                if (!dciTotal.ContainsKey(c))
                    dciTotal.Add(c, 1);
                else ++dciTotal[c];
            }

            if (dciTotal.Count <= 0)
                return 0;

            double sum_element = 0;
            double sum_weight = 0;
            foreach (char key in dciTotal.Keys)
            {
                double weight = dciTotal[key];
                sum_weight += weight;

                if (dci1.ContainsKey(key) && dci2.ContainsKey(key))
                {
                    double median = (double)weight / 2.0;
                    double min = Math.Min(dci1[key], dci2[key]);
                    double similarity = min / median; ///min * 100 / max
                    sum_element += similarity * weight;
                }//else sum_element += weight*0
            }


            return sum_element / sum_weight;
        }

    }
}
