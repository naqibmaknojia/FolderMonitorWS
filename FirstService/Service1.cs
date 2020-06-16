using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FirstService
{
    public partial class Service1 : ServiceBase
    {
        public bool RecentChannge;
        public Timer delay;
        public DateTime lastCheck;
        public DateTime checkStart;
        public int currDelay= 60000;
        public Service1()
        {
            InitializeComponent();
            delay = new Timer(currDelay) { AutoReset = true };
            delay.Elapsed += CheckerService;
        }

        private void CheckerService(object sender, ElapsedEventArgs e)
        {
            RecentChannge = false;
            checkStart = DateTime.Now;
            string p = ConfigurationManager.AppSettings["checkPath"];
            string destination = ConfigurationManager.AppSettings["updatePath"];

            foreach (string file in Directory.EnumerateFiles(p))
            {
                FileInfo a = new FileInfo(file);
                string srcFile = System.IO.Path.Combine(p, a.Name);
                string destFile = System.IO.Path.Combine(destination, a.Name);
                //Check whether file existed or has been updated after last check
                if (DateTime.Compare(a.CreationTime, lastCheck) > 0 || DateTime.Compare(a.LastWriteTime, lastCheck) > 0)
                {
                    //Copy this newly created or updated File
                    System.IO.File.Copy(srcFile, destFile, true);
                    RecentChannge = true;
                }

            }

            if (!RecentChannge && currDelay < 3600000)
            {
                //Additional 2mins
                currDelay = currDelay + 120000;
                delay.Interval = currDelay;
            }
            else
            {
                //reset interval
                delay.Interval = 60000;
            }

            lastCheck = checkStart;
        }

        protected override void OnStart(string[] args)
        {
            lastCheck = DateTime.Now;
            delay.Start();
        }

        protected override void OnStop()
        {
            
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
