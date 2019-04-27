using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Silverlake.Web
{
    public class Global : HttpApplication
    {
        System.Timers.Timer timer = new System.Timers.Timer();

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //NetworkDrive nd = new NetworkDrive();
            //nd.MapNetworkDrive(@"\\192.168.0.21\Sachin_Shared", "Z:", "Administrator", "petraware@90");
            //double inter = Convert.ToDouble(60 * 60 * 1000);
            //timer.Interval = inter;
            //timer.AutoReset = true;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //timer.Enabled = true;
            // filesDelete();
           // filesMove(System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYEx"].ToString(), System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYLog"].ToString());
        }
        public void filesDelete()
        {
            string str = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["Files"].ToString();
            DirectoryInfo filesdi = new DirectoryInfo(str);
            foreach (FileInfo file in filesdi.GetFiles())
                file.Delete();
        }

        public void mimzyCaptureOuputFilesDelete()
        {
            string str =  System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputFiles"].ToString();
            DirectoryInfo filesdi = new DirectoryInfo(str);
            foreach (FileInfo file in filesdi.GetFiles("*.xml"))
                file.Delete();
            foreach (FileInfo file in filesdi.GetFiles("*.pdf"))
                file.Delete();
        }

        public void filesMove(string path, string newPath)
        {
            try
            {
                newPath +=  DateTime.Now.ToString("yyyyMMdd") + "\\";
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                DirectoryInfo filesdi = new DirectoryInfo(path);
                foreach (FileInfo file in filesdi.GetFiles())
                    File.Move(file.FullName, newPath + file.Name);
            }
            catch (Exception ex)
            {

            }
        }
        public void timer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            String s = DateTime.Now.ToString("tt");
            if (DateTime.Now.Hour == 12 && s == "PM")
            {
                // ELIBRARY
                filesMove(System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYEx"].ToString(), System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYLog"].ToString());
                // LOS
                filesMove(System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputLOSEx"].ToString(), System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputLOSLog"].ToString());
                // ETP
                filesMove(System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputETPEx"].ToString(), System.Configuration.ConfigurationManager.AppSettings["MimzyCaptureOuputETPLog"].ToString());
                // Files Delete
                filesDelete();
                LogWriter logWriter1 = new LogWriter("time api runing, Date success" + DateTime.Now);
            }
            else
            {
                LogWriter logWriter1 = new LogWriter("time api runing, Date else" + DateTime.Now);
            }
        }

    }
}