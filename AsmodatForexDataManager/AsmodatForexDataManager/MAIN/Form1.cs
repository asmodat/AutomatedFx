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

namespace AsmodatForexDataManager
{
    public partial class Form1 : Form
    {
        public void PeacemakerUpdate()
        {
            if (Manager == null || Manager.ForexAuthentication == null) return;

            if (Manager.ForexAuthentication.Connecting)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    if (Manager.ForexAuthentication.Disconnections <= 1)
                        BtnConnect.Text = "Connecting ...";
                    else BtnConnect.Text = "Reconnecting ...";
                    BtnConnect.Enabled = false;
                }));
            }
            else if (Manager.ForexAuthentication.Connected)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    BtnConnect.Text = "DISCONNECT";
                    BtnConnect.Enabled = true;
                }));
            }
            else if (Manager.ForexAuthentication.Stopped)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    BtnConnect.Text = "RECONNECT";
                    BtnConnect.Enabled = true;
                }));
            }
            
                
        }





    }
}


//private void BtnArchiveSave_Click(object sender, EventArgs e)
//{
//    ThreadControls.Run(() => ForexManager.Service.Archive.SaveAll(true), null, true, true);
//}

//private void BtnArchiveNuke_Click(object sender, EventArgs e)
//{
//    ForexManager.Service.Archive.RemoveAll();
//}

//if (ForexManager == null || ForexManager.Service == null) return;

//if (!ForexManager.Service.Archive.Loaded)
//{
//    FormsControls.SetPropertyAllOfType<Button>(TabPageArchive, "Enabled", false, true, 2);
//}
//else
//{
//    FormsControls.SetPropertyAllOfType<Button>(TabPageArchive, "Enabled", true, true, 2);
//    FormsControls.SetProperty<Label>(LblArchiveUpdates, "Text", "Updates: " + ForexManager.Service.Archive.Updates, true);
//}

//if (ForexManager.Service.Authentication != null)
//{
//    FormsControls.SetProperty<RichTextBox>(RtbxArchiveInfoLog, "Text", ForexManager.Service.Authentication.LastException, true);
//}

////if(ForexManager == null || !ForexManager.Service.Archive.Loaded)
////{
////    if (BtnArchiveSave.Enabled || BtnArchiveNuke.Enabled)
////    {
////        BtnArchiveSave.Enabled = false;
////        BtnArchiveSave.Text = "LOADING...";

////        BtnArchiveNuke.Enabled = false;
////        BtnArchiveNuke.Text = "LOADING...";
////    }
////}
////else if (ForexManager.Service.Archive.Loaded && !BtnArchiveSave.Enabled)
////{
////    if (!BtnArchiveSave.Enabled || !BtnArchiveNuke.Enabled)
////    {
////        BtnArchiveSave.Enabled = true;
////        BtnArchiveSave.Text = "SAVE";

////        BtnArchiveNuke.Enabled = true;
////        BtnArchiveNuke.Text = "NUKE";
////    }
////}