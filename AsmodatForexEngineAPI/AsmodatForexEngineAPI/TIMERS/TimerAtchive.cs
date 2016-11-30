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

    public partial class Form1 : Form
    {
        private void BtnSaveArchive_Click(object sender, EventArgs e)
        {
            bSaveArchive = true;
            BtnSaveArchive.Enabled = false;
            BtnSaveArchive.Text = " LOADING ... ";
            
        }

        private bool bSaveArchive = false;
        Thread ThrdARCHIVE;
        int counterCurrentArchiveUpdates = 0;
        int counterCurrentArchiveSaved = 0;
        private void TimrArchive_Tick(object sender, EventArgs e)
        {
            if (ThrdARCHIVE == null || !ThrdARCHIVE.IsAlive)
            {
                int DataKind = ORBlotter.Count;

                if (ARCHIVE.IsLoaded)
                {
                    if (!ARCHIVE.Saving && bSaveArchive == false)
                    {
                        BtnSaveArchive.Text = "SAVE ARCHIVE";
                        BtnSaveArchive.Enabled = true;
                    }
                    else if (bSaveArchive)
                    {
                        BtnSaveArchive.Enabled = false;
                        BtnSaveArchive.Text = "SAVING ... ";
                    }
                }



                ThrdARCHIVE = new Thread(delegate()
                {


                    if (!ARCHIVE.IsLoaded) ARCHIVE.LoadAll();

                    counterCurrentArchiveUpdates += ARCHIVE.UpdateAll(bSaveArchive);


                    if (bSaveArchive && ARCHIVE.Saved)
                    {
                        counterCurrentArchiveSaved = counterCurrentArchiveUpdates;
                        bSaveArchive = false;
                    }


                    //ARCHIVE.RepairAll();

                    ThrdARCHIVE.Abort();
                });

                ThrdARCHIVE.Start();

                double dRatio = ((double)(counterCurrentArchiveSaved + 1) / (counterCurrentArchiveUpdates + 1)) * 100;
                TSSPBArchives.Value = (int)dRatio;
            }
        }


    }
}
