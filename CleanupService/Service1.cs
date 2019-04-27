using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CleanupService
{
    public partial class Service1 : ServiceBase
    {
        public string ServerNo = ConfigurationManager.AppSettings["ServerNo"].ToString();
        public string MimzyCaptureOuputELIBRARY = ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARY"].ToString();
        public string MimzyCaptureOuputETP = ConfigurationManager.AppSettings["MimzyCaptureOuputETP"].ToString();
        public string MimzyCaptureOuputLOS = ConfigurationManager.AppSettings["MimzyCaptureOuputLOS"].ToString();


        public string MimzyCaptureOuputELIBRARYLog = ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYLog"].ToString();
        public string MimzyCaptureOuputETPLog = ConfigurationManager.AppSettings["MimzyCaptureOuputETPLog"].ToString();
        public string MimzyCaptureOuputLOSLog = ConfigurationManager.AppSettings["MimzyCaptureOuputLOSLog"].ToString();

        public string LogBackUpELIBRARYLog = ConfigurationManager.AppSettings["LogBackUpELIBRARYLog"].ToString();
        public string LogBackUpETPLog = ConfigurationManager.AppSettings["LogBackUpETPLog"].ToString();
        public string LogBackUpLOSLog = ConfigurationManager.AppSettings["LogBackUpLOSLog"].ToString();


        public string TimeType = ConfigurationManager.AppSettings["TimeType"].ToString();
        public int CleanUpTime = Convert.ToInt32(ConfigurationManager.AppSettings["CleanUpTime"].ToString());

        public string FilesPath = ConfigurationManager.AppSettings["Files"].ToString();
        public string NewFilesPath = ConfigurationManager.AppSettings["LogBackUpFaliedFiles"].ToString();
   
        
        // public string WebFilesPath = ConfigurationManager.AppSettings["WebFiles"].ToString();
        // public string MfileFilesPath = ConfigurationManager.AppSettings["MfileFiles"].ToString();

        public int MonthofDayDeleteFiles = Convert.ToInt32(ConfigurationManager.AppSettings["MonthofDayDeleteFiles"].ToString());

        private System.Timers.Timer timer;
        public Service1()
        {
            this.timer = new System.Timers.Timer(3600000D);  // 3600000 milliseconds = 1 hour
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Cleanup);
            this.timer.Start();

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogWriter logWriter = new LogWriter("Sync Service Started");
        }

        protected override void OnStop()
        {
            LogWriter logWriter = new LogWriter("Sync Service Stoped");
        }
        public void Cleanup(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                String s = DateTime.Now.ToString("tt");
                LogWriter logWriter1 = new LogWriter("Cleanup runing, Date : " + DateTime.Now + " and hour " + DateTime.Now.Hour + " TimeType " + s);
                if (DateTime.Now.Hour == CleanUpTime && s == TimeType)
                {
                    // ELIBRARY Log files move to backup floder
                    filesMove(MimzyCaptureOuputELIBRARYLog, LogBackUpELIBRARYLog);
                    // LOS Log files move to backup floder
                    filesMove(MimzyCaptureOuputLOSLog, LogBackUpLOSLog);
                    // ETP Log files move to backup floder
                    filesMove(MimzyCaptureOuputETPLog, LogBackUpETPLog);
                    // Falied Files move to backup floder
                    faliedFilesMove(FilesPath, NewFilesPath);
                    // Delete files
                    //if (DateTime.Now.Day == MonthofDayDeleteFiles)
                    //{
                    //    // Delete web files files
                    //    mimzyCaptureOuputFilesDelete(WebFilesPath);
                    //    mimzyCaptureOuputFilesDelete(MfileFilesPath);
                    //    mimzyCaptureOuputFilesDelete(LogBackUpELIBRARYLog);
                    //    mimzyCaptureOuputFilesDelete(LogBackUpETPLog);
                    //    mimzyCaptureOuputFilesDelete(LogBackUpLOSLog);

                    //}
                    LogWriter logWriter = new LogWriter("Cleanup job successfully. " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("Cleanup! Exception: " + ex.Message);
            }
        }

        public void mimzyCaptureOuputFilesDelete(string path)
        {
            try
            {
                DirectoryInfo filesdi = new DirectoryInfo(path);
                foreach (FileInfo file in filesdi.GetFiles("*.xml"))
                    file.Delete();
                foreach (FileInfo file in filesdi.GetFiles("*.pdf"))
                    file.Delete();

                //foreach (string file in Directory.GetDirectories(path))
                //    Directory.Delete(file, true);
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("mimzyCaptureOuputFilesDelete! Path :" + path + " Exception: " + ex.Message);
            }

        }

        public void filesMove(string path, string newPath)
        {
            try
            {
                newPath += DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                DirectoryInfo filesdi = new DirectoryInfo(path);
                foreach (string file in Directory.GetDirectories(path))
                {
                    DirectoryInfo newDir = new DirectoryInfo(file);
                    Directory.Move(file, newPath + newDir.Name);
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("filesMove! Path :" + path + " Exception: " + ex.Message);
            }
        }

        public void faliedFilesMove(string path, string newPath)
        {
            try
            {
                newPath += DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                DirectoryInfo filesdi = new DirectoryInfo(path);
                foreach (FileInfo file in filesdi.GetFiles("*.xml"))
                    File.Move(file.FullName, newPath + file.Name);
                foreach (FileInfo file in filesdi.GetFiles("*.pdf"))
                    File.Move(file.FullName, newPath + file.Name);
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("filesMove! Path :" + path + " Exception: " + ex.Message);
            }
        }
    }
}
