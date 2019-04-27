using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncClient
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
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile("SyncClientConfig.ini");
                KeyData keyLogFile = data.Global.GetKeyData("LogFile");
                string logPath = keyLogFile.Value;

                string logFilename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(logPath + "\\" + logFilename))
                    File.Create(logPath + "\\" + logFilename);
                using (StreamWriter w = File.AppendText(logPath + "\\" + logFilename))
                {
                    Log(logMessage, w);
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
                txtWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                txtWriter.WriteLine(logMessage);
                txtWriter.WriteLine("------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
