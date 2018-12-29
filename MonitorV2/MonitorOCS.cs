using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Xml.Linq;

namespace MonitorV2
{
    class MonitorV1
    {

        public static string getPostIDV1()
        {


            DarkTools.ReadPostIDXml();



            var client = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID + "/monitor_post_id_qsr");
            client.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("content-type", "application/json");
            //request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
            IRestResponse response2 = client.Execute(request2);

            var GetConfig = response2.Content.ToString();

            Acf post = JsonConvert.DeserializeObject<Acf>(GetConfig);

            string PostID = post.monitor_post_id_qsr;

            return PostID;

        }

        public static string regClientV1()
        {

            try
            {
                DarkTools.ReadPostIDXml();


                var acf_fields = new
                {



                    //Post to ACF Fields
                    monitor_register = new // You must have this for ACf
                    {  //<--This needs to be a proper object

                        pc_name_onec = DarkTools.pcNameV1(),
                        ip_address_onec = DarkTools.GetLocalIPAddress(),
                        mac_address_onec = DarkTools.GetMACAddress(),
                        status_onec = "Online",
                        post_id_onec = DarkTools.PostId,
                        time_stamp_onec = DarkTools.timestamp.ToString(),

                    },  //END You must have this for ACf

                };

                var content1 = new
                {
                    acf_fields = new
                    {
                        monitor_register = acf_fields,
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
            return null;

        }

        public static string updateReg()
        {
            try
            {
                DarkTools.ReadPostIDXml();


                var acf_fields = new
                {



                    //Post to ACF Fields
                    monitor_register = new // You must have this for ACf
                    {  //<--This needs to be a proper object

                        pc_name_onec = DarkTools.pcNameV1(),
                        ip_address_onec = DarkTools.GetLocalIPAddress(),
                        mac_address_onec = DarkTools.GetMACAddress(),
                        status_onec = "Online",


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

                return "dfsf";
            }
            catch
            {
                return null;
            }



        }
    }

    class MonitorQSR
    {
        //Post Id 
        public static string PostIdNew { get; set; }

        public static string MonPostID { get; set; }

        //New Monitor functions to update from site

        public static void GetPostID()
        {


            if (MonitorV1.getPostIDV1() == "null" || MonitorV1.getPostIDV1() == "" || MonitorV1.getPostIDV1() == "0" || MonitorV1.getPostIDV1() == null)
            {
                CreatePost();
                LogFile.LogMessageToFile("Client Views Created");
                UpdatePostID();
            }
            else
            {
                updateViews();
            }
        }

        public static void CreatePost()
        {

            try
            {
                var content1 = new
                {
                    title = DarkTools.Title_Onec,
                    content = "",
                    status = "publish",
                    categories = "1",
                    acf = "True"
                };

                var client = new RestClient("https://" + DarkTools.ReadSiteEndpoint + "/wp-json/wp/v2/posts/?");
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
                MonitorQSR.MonPostID = post.id.ToString();
                //Writes postid to XML

                MonitorV1.regClientV1();
                MyDevice.MyDevicePostInfo();

            }
            catch
            {

            }
        }

        public static void UpdatePostID()
        {

            DarkTools.ReadPostIDXml();

            //Update post ID //

            var acf_fields1 = new
            {

                //Post to ACF Fields
                acf_fields = new // You must have this for ACf
                {  //<--This needs to be a proper object

                    monitor_post_id_qsr = MonitorQSR.MonPostID,

                },  //END You must have this for ACf

            };

            var client2 = new RestClient(API.domainName() + "/wp-json/acf/v3/posts/" + DarkTools.ReadXMLID + "/monitor_post_id_qsr");
            client2.Authenticator = new HttpBasicAuthenticator(API.userName(), API.passWord());
            var request1 = new RestRequest(Method.POST);
            request1.AddHeader("content-type", "application/json");
            request1.AddJsonBody(acf_fields1); //<-- this will serialize and add the model as a JSON body.
            IRestResponse response2 = client2.Execute(request1);
        }

        public static void updateViews()
        {
            MonitorV1.regClientV1();
            MyDevice.MyDeviceClientPostInfo();


            MyDevice.MyDevicePostInfo();
            MonitorV1.updateReg();
            LogFile.LogMessageToFile("Views updated");
        }

        public static void Activate()
        {
            LogFile.LogMessageToFile("Running Monitor");
            GetPostID();
            SupportTicket.checkTicketRef();

        }

        public static void RunMonitorQSR()
        {
            DarkTools.UpdateConfig();
            DarkTools.WriteConfigToXml();
            DarkTools.ReadConfigToXml();

            if (DarkTools.ReadActivateMonitor == "None")
            {
                LogFile.LogMessageToFile("Config turned off");
            }
            else if (DarkTools.ReadActivateMonitor == "Activate")
            {
                Activate();

            }
        }
    }
}
