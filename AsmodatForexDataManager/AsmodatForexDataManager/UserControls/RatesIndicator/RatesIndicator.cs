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
using Asmodat;

namespace AsmodatForexDataManager.UserControls
{

    
    public partial class RatesIndicator : ChartFeedUserControl 
    { 

        public RatesIndicator()
        {
            InitializeComponent();

            
        }

        public override void Init()
        {
            Timers.Run(() => Peacemaker(), 100, null, true, true);
            Timers.Run(() => PeacemakerChart(), 10, null, true, true);
            Chart.Main.Start();

            this.CmbxTimeFrame.MouseEnter += CmbxTimeFrame_MouseEnter;
            this.CmbxPair.MouseEnter += CmbxPair_MouseEnter;

            CmbxTimeFrame.Items.Clear();
            CmbxTimeFrame.Items.AddRange(Enums.ToString<ServiceConfiguration.TimeFrame>().ToArray());
            CmbxTimeFrame.SelectedIndex = 0;

            ClearControls(false);
        }


        public void Peacemaker()
        {
            if (Manager == null) return;

            UpdateRateControls();

            GpbxRateProperties.Invoke((MethodInvoker)(() =>
            {
                if (CmbxPair.Items.Count == 0)
                    this.UpdateItemsPair();
            }));
        }

        private void ClearControls(bool invoke)
        {
            FormsControls.SetPropertyAllOfType<TextBox>(GpbxRateProperties, "Text", "???", invoke, 1);
        }
        

        private void CmbxPair_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime = DateTime.MinValue;
            UpdateRateControls();
        }

        private void CmbxTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime = DateTime.MinValue;
            UpdateRateControls();
        }

        void CmbxPair_MouseEnter(object sender, System.EventArgs e)
        {
            this.UpdateItemsPair();
            this.UpdateItemsTimeFrame();
        }


        void CmbxTimeFrame_MouseEnter(object sender, System.EventArgs e)
        {
            this.UpdateItemsTimeFrame();
            this.UpdateItemsPair();
        }


        
        

    }
}
