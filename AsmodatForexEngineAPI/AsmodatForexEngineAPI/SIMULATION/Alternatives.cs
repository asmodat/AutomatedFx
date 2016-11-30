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
        public List<string> Options(string product)
        {
            if (!this.HaveOptions(product))
                return new List<string>();

            return new List<string>(new string[] {product, DSSAlternatives[product], DSSCounterAlternatives[product] });
        }

        public bool HaveOptions(string product)
        {
            if (DSSAlternatives.ContainsKey(product) && DSSCounterAlternatives.ContainsKey(product))
                return true;
            else return false;
        }

        Dictionary<string, string> DSSAlternatives = new Dictionary<string, string>();
        Dictionary<string, string> DSSCounterAlternatives = new Dictionary<string, string>();
        public void LoadOptions(TimeFrame TFrame, List<string> LSProducts, int count)
        {
            for (int i = 0; i < LSProducts.Count; i++)
            {
                string product = LSProducts[i];

                if (this.HaveOptions(product))
                    continue;

                int iShift = 0;
                int iShiftAlternative = 0;
                string alternative = "";
                string alternativeCounter = "";
                bool bFound = ANALYSIS.AlternateProduct(ORBlotter, TFrame, count, product, ref iShift, ref iShiftAlternative, 2, ref alternative, ref alternativeCounter);

                if (!bFound)
                    return;

                DSSAlternatives.Add(product, alternative);
                DSSCounterAlternatives.Add(product, alternativeCounter);
            }

        }

    }
}
