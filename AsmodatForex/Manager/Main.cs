using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using Asmodat.Abbreviate;

namespace AsmodatForex
{
    public partial class Manager
    {
        public Manager(Credentials UserCredentials)
        {
            this.UserCredentials = UserCredentials;
        }

        

        //public ServiceAuthentication ForexAuthentication;
        //public ServiceConfiguration ForexConfiguration;
        //public ServiceCharting ForexCharting;
        //public ServiceRates ForexRates;
        //public TreadingAPI ForexAPI;
        //public Archive ForexArchive = new Archive();


        public ForexService Service;

        //TODO: Fix Managers
        public void Start()
        {

            if (Service == null) Service = new ForexService(ref _UserCredentials);
            else Service.UserCredentials = _UserCredentials;

            //Archive ARCH = new Archive();
            //var v3= Enums.ToEnum<Archive.TimeFrame, AsmodatForex.com.efxnow.democharting.chartingservice.TimeFrame>(Archive.TimeFrame.FIVE_MINUTE);
            //var v = Enums.ToList<Archive.TimeFrame>();

            Service.Authentication = new ServiceAuthentication(ref Service);// UserCredentials, ref ForexArchive);
            Service.Authentication.Start();
            while (Service.Authentication.Token == null) Thread.Sleep(1);

            Service.Configuration = new ServiceConfiguration(ref Service); //ForexAuthentication.ForexConfiguration = ForexConfiguration;
            Service.Charting = new ServiceCharting(ref Service);
            Service.Rates = new ServiceRates(ref Service);
            Service.API = new TreadingAPI(ref Service);
            Service.Trading = new ServiceTrading(ref Service);
            Service.Analysis = new Analysis(ref Service);
            
            UserCreditalsUpdated = false;
        }


       


    }
}
