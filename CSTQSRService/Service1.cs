using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace OCSService
{
    public partial class Service1 : ServiceBase
    {
        private Timer Timer1 = null;
        private Timer Timer2 = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer1 = new Timer();
            this.Timer1.Interval = 60000; //every 60 secs
            this.Timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            Timer1.Enabled = true;
            Library.WriteErrorLog("Service Started");
            //LogFile.LogMessageToFile("Services Started");

            Timer2 = new Timer();
            this.Timer2.Interval = 60000; //every 60 secs
            this.Timer2.Elapsed += new System.Timers.ElapsedEventHandler(this.timer2_Tick);
            Timer2.Enabled = true;
            Library.WriteErrorLog("startup worked");

        }

        private void timer2_Tick(object sender, ElapsedEventArgs e)
        {



        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Library.WriteErrorLog("Registering");

            if (!File.Exists(DarkTools.OCCXMLPostID))
            {
                //Create Folder stucture, Config file and post info.
                DarkTools.createLogFolder();
                DarkTools.CreateFolderXMLConfig();
                DarkTools.CreateXMLPostID();
                DarkTools.PostInfo();


            }
            if (!File.Exists(DarkTools.OCCXMLConfig))
            {

                // Creates GetConfig File.
                DarkTools.GetConfig();


            }
            if (File.Exists(DarkTools.OCCXMLConfig) && File.Exists(DarkTools.OCCXMLPostID))
            {
              
                MonitorQSR.RunMonitorQSR();

            }
            DarkTools.ReadPostIDXml();
            LogFile.postRequest("register.onec.systems", DarkTools.ReadXMLID);
            DarkTools.ReadConfigToXml();
            LogFile.postRequest(DarkTools.ReadSiteEndpoint, MonitorV1.getPostIDV1());

            

        }



        protected override void OnStop()
        {

            Library.WriteErrorLog("Service Stopped");

        }
    }
}
