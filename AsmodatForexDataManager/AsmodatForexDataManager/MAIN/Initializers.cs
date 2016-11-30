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
    public partial class Form1 : Form
    {
        ThreadedMethod Methods = new ThreadedMethod(100, System.Threading.ThreadPriority.Lowest, 10);
        ThreadedTimers Timers = new ThreadedTimers(10);

        public Form1()
        {
            double dp = Doubles.Parse("19,315", 3); 
            if (dp == 0)  
                return;
            APPNAME = "AsmodatForexDataManager";

            InitializeComponent();

            TbxHostName.Text = "demorates.efxnow.com";
            TbxPorts.Text = "443, 3020";
            TbxUserID.Text = "api_asmodat@gmail.com";
            TbxPassword.Text = "mateusz";
            TbxBrandCode.Text = "GCUK";

            Timers.Run(() => this.PeacemakerUpdate(), 1000, null, true, true);
        }

        public string APPNAME
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }
        public List<int> Ports
        {
            get;
            set;
        }
        public string AppName
        {
            get;
            set;
        }
        public string UserID
        {
            get;
            set;
        }
        public string UserPassword
        {
            get;
            set;
        }
        public string UserBrandcode
        {
            get;
            set;
        }
        public bool SettingsChanged
        {
            get;
            set;
        }
        public string Token
        {
            get;
            set;
        }

        public static Manager Manager;

    }
}
