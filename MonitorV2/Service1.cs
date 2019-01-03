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


namespace MonitorV2
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

            Timer2 = new Timer();
            this.Timer2.Interval = 60000; //every 60 secs
            this.Timer2.Elapsed += new System.Timers.ElapsedEventHandler(this.timer2_Tick);
            Timer2.Enabled = true;
      

        }

        private void timer2_Tick(object sender, ElapsedEventArgs e)
        {



        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            if (!File.Exists(@"C:\ProgramData\Onec\Config\id.txt"))
            {
                //Create Folder stucture, Config file and post info.

                FunctionsV2.createFolder(@"C:\ProgramData\Onec\Logs");
                FunctionsV2.createFolder(@"C:\ProgramData\Onec\Config");
                FunctionsV2.createTxtFile(@"C:\ProgramData\Onec\Config\id.txt");
                API.findID();



            }
            MonitorFunctionsV2.checkIfActive();

        }



        protected override void OnStop()
        {

  

        }
    }
}
