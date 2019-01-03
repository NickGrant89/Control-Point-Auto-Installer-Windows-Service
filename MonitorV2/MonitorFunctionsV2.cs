using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorV2
{
    class MonitorFunctionsV2
    {

        public static string getDeviceStatus()
        {
            var client = new RestClient(API.domainName() + "/api/v1/devices/" + FunctionsV2.readTxtFile(@"C:\ProgramData\Onec\Config\id.txt"));
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("content-type", "application/json");
            request2.AddHeader("Authorization", "bearer " + API.getAuth());
            IRestResponse response2 = client.Execute(request2);

            return response2.Content;
        }

        public static void checkIfActive()
        {
            
            if(getDeviceStatus() == "The device with the given ID cannot be found!")
            {
                FunctionsV2.deviceCheckIn();
            }
            //Deserialize to object
            DeviceModel.RootObject device = JsonConvert.DeserializeObject<DeviceModel.RootObject>(getDeviceStatus());

            if (device.status != "Active")
            {
                return;
            }
            else
            {
                runMonitor();
            }

            LogFile.LogMessageToFile(" ");
        }

        public static void runMonitor()
        {
            try
            {
                var device = new
                {

                    pcname = MyDevice.getPcName(),
                    ipaddress = MyDevice.getLocalIPAddress(),
                    macaddress = MyDevice.getMACAddress(),
                    status = "Active",
                    timestamp = DateTime.Now.ToString(),

                    deviceinfo = new
                    {
                        windowsversion = MyDevice.WindowsVer(),
                        cpu = MyDevice.getProcessor(),
                        availablememory = MyDevice.PhysicalMemory(),
                        exipaddress = MyDevice.getExternalIp(),
                        antivirus = MyDevice.AntiVirus(),
                        deviceuptime = MyDevice.GetUptime().ToString(),
                        lastupdated = "",

                    },
                    devicestatus = new
                    {
                        cpu = "13",
                        memory = "47",
                        network = "5",
                    },
                    harddrivespace = new
                    {
                        totalspace = MyDevice.TotalSpace(),
                        freespace = MyDevice.freeSpace(),
                        usedspace = MyDevice.usedSpace(),
                    },
                     

                    ocslogfile = FunctionsV2.readTxtFile(@"C:\ProgramData\Onec\Logs\log-" + DateTime.Now.ToString("ddMMyyyy") + ".txt"),

                };

                var client = new RestClient(API.domainName() + "/api/v1/devices/" + FunctionsV2.readTxtFile(@"C:\ProgramData\Onec\Config\id.txt"));
                var request2 = new RestRequest(Method.PUT);
                request2.AddHeader("content-type", "application/json");
                request2.AddHeader("Authorization", "bearer " + API.getAuth());
                request2.AddJsonBody(device); //<-- this will serialize and add the model as a JSON body.
                //request2.AddJsonBody(Devicestatus);
                IRestResponse response2 = client.Execute(request2);

                LogFile.LogMessageToFile(" ");


            }
            catch
            {

            }
        }
    }
}
