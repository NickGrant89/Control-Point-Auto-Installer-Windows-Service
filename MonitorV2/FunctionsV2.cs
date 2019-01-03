using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorV2
{
    class FunctionsV2
    {
        //Create Folder
        public static void createFolder(string path)
        {
            

            if (Directory.Exists(path))
            {

            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
            Directory.GetCreationTime(path);
            LogFile.LogMessageToFile("");
        }
        //Creates post config / ID
        public static void createTxtFile(string path)
        {
            //string path = @"C:\ProgramData\Onec\Config\OCCID.xml";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {


                }
     
            }
            LogFile.LogMessageToFile("");

        }
        //Writes PostID to config
        public static void writeTxtFile(string path, string text)
        {
     

            System.IO.File.WriteAllText(path, text);

        }
        //Reads Post ID
        public static string readTxtFile(string filePath)
        {
            //@"C:\ProgramData\Onec\Config\id.txt

            // Read the file as one string.
            string text = System.IO.File.ReadAllText(filePath);

            // Display the file contents to the console. Variable text is a string.
            System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

            return text;

        }
        //Creates Device
        public static void deviceCheckIn()
        {
            try
            {
                var device = new
                {
                    pcname = MyDevice.getPcName(),
                    ipaddress = MyDevice.getLocalIPAddress(),
                    macaddress = MyDevice.getMACAddress(),
                    status = "Disabled",
                    timestamp = DateTime.Now.ToString(),
                };

                var client = new RestClient(API.domainName() +"/api/v1/devices/checkin");
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("content-type", "application/json");
                request2.AddHeader("Authorization", "bearer " + API.getAuth());
                request2.AddJsonBody(device); //<-- this will serialize and add the model as a JSON body.
                IRestResponse response2 = client.Execute(request2);

                //Response to Var 
                Console.WriteLine(response2.Content.ToString());
                //Deserialize to object
                DeviceModel.RootObject account = JsonConvert.DeserializeObject<DeviceModel.RootObject>(response2.Content);

                writeTxtFile(@"C:\ProgramData\Onec\Config\id.txt", account._id);


                LogFile.LogMessageToFile("Device registered add endpoint");


            }
            catch
            {

            }
        }

    }
}
