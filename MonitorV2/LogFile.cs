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

        //Register API Wordpress post details

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

        public static string postRequest(string domainName, string postId)
        {

            var acf_fields = new
            {

                //Post to ACF Fields
                acf_fields = new // You must have this for ACf
                {  //<--This needs to be a proper object


                    service_log_file_ocs = LogRead()


                },  //END You must have this for ACf

            };

            var client = new RestClient("https://" + domainName + "/wp-json/acf/v3/posts/" + postId);
            client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
            var request2 = new RestRequest(Method.POST);
            request2.AddHeader("content-type", "application/json");
            request2.AddJsonBody(acf_fields); //<-- this will serialize and add the model as a JSON body.
            IRestResponse response2 = client.Execute(request2);

            return response2.ResponseStatus.ToString();

        }

        public static string LogRead()
        {
            string date = DateTime.Now.ToString("ddMMyyyy");

            string readText = File.ReadAllText(@"C:\ProgramData\Onec\Logs\log-" + date + ".txt");

            return readText;
        }

    }
}

