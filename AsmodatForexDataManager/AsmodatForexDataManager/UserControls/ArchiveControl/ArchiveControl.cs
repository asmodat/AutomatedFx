using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AsmodatForex;

using Asmodat.Abbreviate;

namespace AsmodatForexDataManager.UserControls
{
    public partial class ArchiveControl : UserControl
    {
        public ArchiveControl()
        {
            InitializeComponent();

            BtnSaveAll.Click += BtnSaveAll_Click;
            BtnNuke.Click += BtnNuke_Click;
            BtnRepair.Click += BtnRepair_Click;

            BtnSaveAll.Enabled = false;
            BtnNuke.Enabled = false;
            BtnRepair.Enabled = false;
        }

        

        


        private Manager Manager;
        private ThreadedTimers Timers = new ThreadedTimers();
        private ThreadedMethod Methods = new ThreadedMethod();

        public void Feed(ref Manager Manager)
        {
            this.Manager = Manager;
            Timers.Run(() => Peacemaker(), 1000, null, true, true, true);
        }

        public void Peacemaker()
        {
            if (Manager == null || Manager.ForexArchive == null) return;

            if (Manager.ForexArchive.Saving || Manager.ForexArchive.Loading || Manager.ForexArchive.Repairing)
            {
                FormsControls.SetProperty(BtnNuke, "Enabled", false, true);
                FormsControls.SetProperty(BtnSaveAll, "Enabled", false, true);
                FormsControls.SetProperty(BtnRepair, "Enabled", false, true);
            }
            else
            {
                FormsControls.SetProperty(BtnNuke, "Enabled", true, true);
                FormsControls.SetProperty(BtnSaveAll, "Enabled", true, true);
                FormsControls.SetProperty(BtnRepair, "Enabled", true, true);
            }

            if (Manager.ForexArchive.Saving)
                FormsControls.SetProperty(TbxStatus, "Text", "Saving Records ... ", true);
            else if (Manager.ForexArchive.Loading)
                FormsControls.SetProperty(TbxStatus, "Text", "Loading Records ... ", true);
            else if (Manager.ForexArchive.Repairing)
                FormsControls.SetProperty(TbxStatus, "Text", "Reapiring Records ... ", true);
            else
                FormsControls.SetProperty(TbxStatus, "Text", " Archive Ready ! ", true);
            


            FormsControls.SetProperty(TbxNewUpdates, "Text",  Manager.ForexArchive.Updates.ToString(), true);
            FormsControls.SetProperty(TbxTotalRecords, "Text", Manager.ForexArchive.Records.ToString(), true);
        }

        void BtnNuke_Click(object sender, EventArgs e)
        {
            DialogResult DResult = MessageBox.Show("Do you want to wipe out whole data base ?\nAll stored records will be removed.", "Unsafe operation warning !", MessageBoxButtons.YesNo);

            if (DResult == DialogResult.Yes)
            {
                BtnNuke.Enabled = false;
                Methods.Run(() => Manager.ForexArchive.RemoveAll(), null, true, true);
            }
            else return;
        }

        void BtnSaveAll_Click(object sender, EventArgs e)
        {
            Methods.Run(() => Manager.ForexArchive.SaveAll(true), null, true, true);
            BtnSaveAll.Enabled = false;
        }

        void BtnRepair_Click(object sender, EventArgs e)
        {
            Methods.Run(() => Manager.ForexArchive.RepairAll(ref Manager), null, true, true);
            BtnRepair.Enabled = false;
        }
    }
}
