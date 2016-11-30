using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Asmodat.Abbreviate;
using AsmodatForex;

namespace AsmodatForexDataManager
{
    public partial class Form1
    {
        

        private void TbxHostName_TextChanged(object sender, EventArgs e)
        {
            if (TbxHostName.Text.Length <= 6) return;

            HostName = TbxHostName.Text;
            SettingsChanged = true;
        }

        private void TbxPorts_TextChanged(object sender, EventArgs e)
        {
            if (TbxPorts.Text.Length <= 1) return;

           List<int> liParts = Asmodat.Abbreviate.Integer.ParseToList(TbxPorts.Text, ",");
           if (liParts.Count > 0)
           {
               Ports = liParts;
               SettingsChanged = true;
           }
        }

        private void TbxUserID_TextChanged(object sender, EventArgs e)
        {
            if (TbxUserID.Text.Length <= 3) return;

            UserID = TbxUserID.Text;
            SettingsChanged = true;
        }

        private void TbxPassword_TextChanged(object sender, EventArgs e)
        {
            if (TbxPassword.Text.Length <= 3) return;

            UserPassword = TbxPassword.Text;
            SettingsChanged = true;
        }

        private void TbxBrandCode_TextChanged(object sender, EventArgs e)
        {
            if (TbxBrandCode.Text.Length <= 3) return;

            UserBrandcode = TbxBrandCode.Text;
            SettingsChanged = true;
        }


        private void BtnConnect_Click(object sender, EventArgs e)
        {
            BtnConnect.Enabled = false;

            if (Manager != null && Manager.ForexAuthentication != null)
            {
                Manager.ForexAuthentication.Stopped = !Manager.ForexAuthentication.Stopped;
                return;
            }


            if (Manager == null)
            {
                Manager = new Manager(new Credentials(HostName, Ports, UserID, UserPassword, UserBrandcode));
                //ForexManager.Start();
                Methods.Run(() => Manager.Start(), "BtnConnect_Click_ForexManager.Start()", true, true);
            }
            else Manager.UserCredentials = new Credentials(HostName, Ports, UserID, UserPassword, UserBrandcode);



            TradeCntrlProperties.Feed(ref Manager);
            DealsCntrlTradinng.Feed(ref Manager);
            AccountIndIicatorInfo.Feed(ref Manager);
            ArchiveCntrlMain.Feed(ref Manager);
            ChartControlRates.Feed(ref Manager, ref RatesIndicatorMain);
            RatesIndicatorMain.Feed(ref Manager, ref ChartControlRates);
            AnalysisCntrlMain.Feed(ref Manager, ref ChartControlAnalysis);
        }
    }
}
