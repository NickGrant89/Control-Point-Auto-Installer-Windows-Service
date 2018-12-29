using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Linq;


namespace OCSService
{
    class DarkTools
    {

        //Register API Wordpress post details
   


        // For PcRegistration() //

        public static string timestamp = DateTime.Now.ToString();
        public static string ReadXMLID { get; set; }

        //String to file paths
        public static string OCCXMLConfig = @"C:\ProgramData\Onec\Config\OCC.xml";
        public static string OCCXMLPostID = @"C:\ProgramData\Onec\Config\OCCID.xml";

        // for PostInfo()
        public static string Title_Onec = pcNameV1();
        public static string PostId { get; set; }

        // for GetConfig()
        public static string PostEndPointC { get; set; }
        public static string ActivateMonitor { get; set; }

        // ReadConfigToXml() Varables

        public static string ReadSiteEndpoint { get; set; }
        public static string ReadActivateMonitor { get; set; }

        public static string pcNameV1()
        {
            string pcName = Environment.MachineName.ToString();
            return pcName;

        }

        //Get Local IP Address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");

        }

        //Gets Mac Address()
        public static string GetMACAddress()
        {



            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }

            return sMacAddress;
            //Library.WriteErrorLog("Registering");


        }

        //Gets IP external Address()
        public static string getExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();


                return externalIP;
            }
            catch { return null; }
        }

        //Creates Post for config
        public static void PostInfo()
        {
            try
            {
                var content1 = new
                {
                    title = Title_Onec,
                    content = "",
                    status = "publish",
                    categories = "1",
                    acf = "True"
                };



                var client = new RestClient(API.domainName() + "/wp-json/wp/v2/posts/?");
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("content-type", "application/json");
                request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

                //Response to Var 
                var postid = response2.Content.ToString();
                //Deserialize to object
                Rootobject post = JsonConvert.DeserializeObject<Rootobject>(postid);
                //Get postId Varable
                DarkTools.PostId = post.id.ToString();
                //Writes postid to XML
                WritePostIDToXml();

                LogFile.LogMessageToFile("Device registered add endpoint");

                PcRegistration();


            }
            catch
            {

            }
        }

        // Registers Data PC
        public static void PcRegistration()
        {
            try
            {
                DarkTools.ReadPostIDXml();


                var acf_fields = new
                {



                    //Post to ACF Fields
                    monitor_register = new // You must have this for ACf
                    {  //<--This needs to be a proper object

                        pc_name_onec = pcNameV1(),
                        ip_address_onec = GetLocalIPAddress(),
                        mac_address_onec = GetMACAddress(),
                        status_onec = "Online",
                        post_id_onec = DarkTools.PostId,
                        time_stamp_onec = timestamp.ToString(),

                    },  //END You must have this for ACf

                };

                var content1 = new
                {
                    acf_fields = new
                    {
                        monitor_register = acf_fields,
                    }
                };

                var client = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID);
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("content-type", "application/json");
                request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

                MyDevice.MyDevicePostInfo();


            }
            catch
            {

            }
        }

        public static void GetConfig()
        {
            try
            {
                DarkTools.ReadPostIDXml();

                var client = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID);
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.GET);
                request2.AddHeader("content-type", "application/json");
                //request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

                var GetConfig = response2.Content.ToString();

                //Getting JSON Values
                JObject rss = JObject.Parse(GetConfig);
                //string rssTitle = (string)rss["acf"]["monitor_register"][0]["pc_name_onec"];

                if (GetConfig.ToString() == "{\"acf\":[]}")
                {
                    DarkTools.GetConfig();
                }
                else
                {


                    try
                    {
                        //Deserializing Using LINQ Example
                        Monitor_Configuration a = new Monitor_Configuration

                        {


                            site_endpoint = (string)rss["acf"]["monitor_configuration"][0]["site_endpoint"],
                            activate_monitor = (string)rss["acf"]["monitor_configuration"][0]["activate_monitor"],


                        };

                        DarkTools.PostEndPointC = a.site_endpoint;
                        DarkTools.ActivateMonitor = a.activate_monitor;

                        if (DarkTools.PostEndPointC != "")
                        {
                            DarkTools.CreateXMLConfig();
                            WriteConfigToXml();

                        }
                        else
                        {
                            DarkTools.GetConfig();
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
        //Update Config
        public static void UpdateConfig()
        {
            try
            {


                DarkTools.ReadPostIDXml();

                var client = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID);
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.GET);
                request2.AddHeader("content-type", "application/json");
                //request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

                var GetConfig = response2.Content.ToString();

                //Getting JSON Values
                JObject rss = JObject.Parse(GetConfig);
                //string rssTitle = (string)rss["acf"]["monitor_register"][0]["pc_name_onec"];



                //Deserializing Using LINQ Example
                Monitor_Configuration a = new Monitor_Configuration

                {

                    site_endpoint = (string)rss["acf"]["monitor_configuration"][0]["site_endpoint"],
                    activate_monitor = (string)rss["acf"]["monitor_configuration"][0]["activate_monitor"],


                };

                DarkTools.PostEndPointC = a.site_endpoint;
                DarkTools.ActivateMonitor = a.activate_monitor;


            }
            catch
            {

            }
        }

        //Create Log Folder
        public static void createLogFolder()
        {
            string path = @"C:\ProgramData\Onec\Logs";

            if (Directory.Exists(path))
            {

            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
            Directory.GetCreationTime(path);
            LogFile.LogMessageToFile("Log Folders Created");
        }

        //Create folder stucture for config
        public static void CreateFolderXMLConfig()
        {


            string path = @"C:\ProgramData\Onec\Config";

            if (Directory.Exists(path))
            {

            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
            Directory.GetCreationTime(path);
            LogFile.LogMessageToFile("Folders Created");
        }

        //Creates post config / ID
        public static void CreateXMLPostID()
        {
            string path = @"C:\ProgramData\Onec\Config\OCCID.xml";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {


                }
                WritePostIDToXml();
            }
            LogFile.LogMessageToFile("OCCID XML Created");

        }
        //Writes PostID to config
        public static void WritePostIDToXml()
        {

            XElement root = new XElement("OCCID",
            new XElement("Config_ID", DarkTools.PostId)
            );
            root.Save(OCCXMLPostID);

        }
        //Reads Post ID
        public static void ReadPostIDXml()
        {


            XElement root = XElement.Load(OCCXMLPostID);
            XNamespace df = root.Name.Namespace;
            XElement status = root.Element(df + "Config_ID");
            ReadXMLID = status.Value.ToString();


        }

        //Creates post endpoint config file
        public static void CreateXMLConfig()
        {
            string path = @"C:\ProgramData\Onec\Config\OCC.xml";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {

                }

            }


        }
        //Wriets post endpoint config file
        public static void WriteConfigToXml()
        {


            XElement root = new XElement("OCC",

            new XElement("Config_SiteEndpoint", DarkTools.PostEndPointC),
            new XElement("activate_monitor", DarkTools.ActivateMonitor)


            );

            //root.ReplaceAll(OCCXMLConfig);
            root.Save(OCCXMLConfig);


        }
        //Reads post endpoint config file
        public static void ReadConfigToXml()
        {


            XElement root = XElement.Load(OCCXMLConfig);
            XNamespace df = root.Name.Namespace;
            XElement SiteEndpoint = root.Element(df + "Config_SiteEndpoint");
            XElement ActivateMonitor = root.Element(df + "activate_monitor");

            ReadSiteEndpoint = SiteEndpoint.Value.ToString();
            ReadActivateMonitor = ActivateMonitor.Value.ToString();

        }

    }
}
