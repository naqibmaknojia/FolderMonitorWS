using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecondService
{
    public partial class Service1 : ServiceBase
    {
        public Timer delay;
        public DateTime lastCheck;
        public DateTime checkStart;
        public Service1()
        {
            InitializeComponent();
            delay = new Timer(900000) { AutoReset = true };
            delay.Elapsed += MailAgent;
        }

        private void MailAgent(object sender, ElapsedEventArgs e)
        {
            checkStart = DateTime.Now;
            string Msgbody = "";
            string p = ConfigurationManager.AppSettings["Path"];

            foreach (string file in Directory.EnumerateFiles(p))
            {
                FileInfo a = new FileInfo(file);
                //Check whether file exist before last check
                if(DateTime.Compare(a.CreationTime, lastCheck) > 0)
                {
                    Msgbody = Msgbody + "File: " + a.Name + " Has been created of size " + a.Length + " bytes.";
                }
                
                //Check wether the file has been updated 
                else if(DateTime.Compare(a.LastWriteTime, lastCheck) > 0)
                {
                    Msgbody = Msgbody + "File: " + a.Name + " Has been Updated Current size is " + a.Length + " bytes.";
                }
            }

            if (!string.Equals(Msgbody,""))
            {
                SmtpClient sc = new SmtpClient();
                MailMessage mail = new MailMessage();
                mail.To.Add("anymail@gmail.com");
                mail.Subject = "Folder Updates";
                mail.Body = Msgbody;
                sc.Send(mail);
            }

            lastCheck = checkStart;
        }

        protected override void OnStart(string[] args)
        {
            lastCheck = DateTime.Now;
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "ServiceStarted.txt");
            delay.Start();
        }

        protected override void OnStop()
        {
            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "ServiceStopped.txt");
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
