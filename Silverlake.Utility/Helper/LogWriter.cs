using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public class LogWriter
    {
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            try
            {
                string logPath = ConfigurationManager.AppSettings["logPath"].ToString();
                string logFilename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(logPath + "\\" + logFilename))
                    File.Create(logPath + "\\" + logFilename).Dispose();
                // old line
                // File.Create(logPath + "\\" + logFilename);
                using (StreamWriter w = File.AppendText(logPath + "\\" + logFilename))
                {
                    Log(logMessage, w);
                    w.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                txtWriter.WriteLine(logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
