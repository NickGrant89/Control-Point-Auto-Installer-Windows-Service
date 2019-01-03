using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;

namespace MonitorV2
{
    class LogFile
    {
        public static string GetTempPath()
        {
            string path = @"C:\ProgramData\Onec\Logs";
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        public static void LogMessageToFile(string msg)
        {
            string date = DateTime.Now.ToString("ddMMyyyy");

            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetTempPath() + "log-" + date + ".txt");
            try
            {
                string logLine = System.DateTime.Now + ":" + " " + msg + "<br>";
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

       

     

    }
}

