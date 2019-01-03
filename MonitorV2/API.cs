using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorV2
{
    class API
    {

        public static string domainName()
        {
            return "https://portal.onec.systems";
        }

        public static string userName()
        {
            return "ocsagent";
        }

        public static string passWord()
        {
            return "KF#hsrDx@MfilSQJa&g*C2vU";
        }

        public static string getAuth()
        {

            var client = new RestClient(API.domainName() + "/api/v1/auth/login");
            var request2 = new RestRequest(Method.POST);
            request2.AddHeader("content-type", "application/json");
            request2.AddParameter("undefined", "{\n\t\"email\":\"nickgrant1989@live.co.uk\",\n\t\"password\":\"Bea27yee\"\n}", ParameterType.RequestBody);
            IRestResponse response2 = client.Execute(request2);

            if (response2.StatusCode.ToString() == "0")
            {
                return "";
            }
            else
            {
                //Response to Var 
                Console.WriteLine(response2.Content.ToString());
                //Deserialize to object
                DeviceModel.Auth auth = JsonConvert.DeserializeObject<DeviceModel.Auth>(response2.Content);

                return auth.token.ToString();
            }
        }

        public static void findID()
        {
            var client = new RestClient(API.domainName() + "/api/v1/devices/find/" + MyDevice.getMACAddress());
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("content-type", "application/json");
            request2.AddHeader("Authorization", "bearer " + API.getAuth());
            IRestResponse response2 = client.Execute(request2);

            if(response2.Content == "Not Found!")
            {
                FunctionsV2.deviceCheckIn();
               
            }
            else
            {
                //Deserialize to object
                DeviceModel.RootObject device = JsonConvert.DeserializeObject<DeviceModel.RootObject>(response2.Content);
                FunctionsV2.writeTxtFile(@"C:\ProgramData\Onec\Config\id.txt", device._id);
            }
        }





    }
}
