using System;
using System.IO;
using Microsoft.VisualBasic.Devices;
using System.Management;
using RestSharp;
using RestSharp.Authenticators;
using System.Linq;
using Newtonsoft.Json;

namespace OCSService
{
    class MyDevice
    {

        //Register API Wordpress post details

        public static string WindowsVer()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();



            return name.ToString();

        }

        public static string AntiVirus()
        {

            using (var antiVirusSearch = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "Select * from AntivirusProduct"))
            {
                var getSearchResult = antiVirusSearch.Get();
                foreach (var searchResult in getSearchResult)
                {

                    string WindowsAntivirus1 = searchResult["displayName"].ToString();
                    return WindowsAntivirus1;

                }

                return null;
            }

        }

        public static string FreeHDDSpace()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;

                string WindowsHHDSpace1 = answer1.ToString() + " " + "GB";

                return WindowsHHDSpace1;
            }
            catch
            {
                return null;

            }

        }

        public static string PhysicalMemory()
        {
            try
            {
                ComputerInfo ci = new ComputerInfo();
                double mem = ci.TotalPhysicalMemory;
                long GB = 1024;

                double answer1 = mem / GB / GB;
                double after3 = Math.Round(answer1);
                string WindowsAvMemory1 = after3.ToString() + "MB";

                return WindowsAvMemory1;

            }
            catch
            {
                return null;
            }

        }

        public static void InstalledProgrames()
        {

            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mo in mos.Get())
            {
                Console.WriteLine(mo["Name"]);
            }
        }

        public static TimeSpan GetUptime()
        {
            var mo = new ManagementObject(@"\\.\root\cimv2:Win32_OperatingSystem=@");
            var lastBootUp = ManagementDateTimeConverter.ToDateTime(mo["LastBootUpTime"].ToString());
            return DateTime.Now.ToUniversalTime() - lastBootUp.ToUniversalTime();

        }

        public static void MyDevicePostInfo()
        {

            try
            {

                DarkTools.ReadPostIDXml();

                var acf_fields = new
                {

                    //Post to ACF Fields
                    my_device = new // You must have this for ACf
                    {  //<--This needs to be a proper object

                        windows_version = WindowsVer(),
                        hard_drive_space = FreeHDDSpace(),
                        available_memory = PhysicalMemory(),
                        ex_ip_address = DarkTools.getExternalIp().ToString(),
                        antivirus = AntiVirus(),
                        last_updated_md = GetUptime().ToString(),
                        last_updated_mocs = DateTime.Now.ToString(),


                    },  //END You must have this for ACf

                };

                var content1 = new
                {
                    acf_fields = new
                    {
                        my_device = acf_fields,
                    }
                };

                var client = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID);
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("content-type", "application/json");
                request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

            }
            catch
            {

            }
        }

        public static void MyDeviceClientPostInfo()
        {

            try
            {

                DarkTools.ReadPostIDXml();

                hddSpaceCha();

                var acf_fields = new
                {

                    //Post to ACF Fields
                    my_device = new // You must have this for ACf
                    {  //<--This needs to be a proper object

                        windows_version = WindowsVer(),
                        hard_drive_space = FreeHDDSpace(),
                        available_memory = PhysicalMemory(),
                        ex_ip_address = DarkTools.getExternalIp().ToString(),
                        antivirus = AntiVirus(),
                        last_updated_md = GetUptime().ToString(),
                        last_updated_mocs = DateTime.Now.ToString("HH:mm dd-MM-yyyy"),


                    },  //END You must have this for ACf

                };

                var content1 = new
                {
                    acf_fields = new
                    {
                        my_device = acf_fields,
                    }
                };

                var client = new RestClient("https://" + DarkTools.ReadSiteEndpoint + "/wp-json/acf/v3/posts/" + MonitorV1.getPostIDV1());
                client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("content-type", "application/json");
                request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

            }
            catch
            {

            }
        }

        public static void hddSpaceCha()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(@"C:");
                long totalSize = driveInfo.TotalSize;
                long FreeSpace = driveInfo.AvailableFreeSpace;
                long GB = 1024;
                long answer1 = FreeSpace / GB / GB / GB;
                long answer2 = totalSize / GB / GB / GB;
                long answer3 = answer2 - answer1;

                string totalSpace = answer2.ToString();
                string freeSpace = answer1.ToString();

                //Devices data for ACf field repeater
                string PostList = "{\"acf_fields\":{\"hard_drive_cha\":[{\"total_size_cha\":\"" + totalSpace + "\",\"available_space_cha\":\"" + freeSpace + "\",\"used_space_cha\":\"" + answer3 + "\",\"last_updated_cha\":\"" + DarkTools.timestamp.ToString() + "\"}]}}";

                var client2 = new RestClient("https://" + DarkTools.ReadSiteEndpoint + "/wp-json/acf/v3/posts/" + MonitorV1.getPostIDV1());
                client2.Authenticator = new HttpBasicAuthenticator("nick", "Bea27yee");
                var request3 = new RestRequest(Method.POST);
                request3.AddHeader("content-type", "application/json");
                //request2.AddJsonBody(nick2); //<-- this will serialize and add the model as a JSON body.
                request3.AddParameter("application/json", PostList, ParameterType.RequestBody); //This will add raw json data
                IRestResponse response3 = client2.Execute(request3);

            }
            catch
            {


            }
        }

    }
}
