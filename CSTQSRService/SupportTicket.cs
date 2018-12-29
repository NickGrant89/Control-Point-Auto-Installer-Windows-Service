using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OCSService
{
    class SupportTicket
    {

        public static void email_Support()
        {
            DarkTools.ReadConfigToXml();

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.onec.co.uk");

                mail.From = new MailAddress("monitor@onec.co.uk", DarkTools.pcNameV1() + " - " + "Device Alert");
                mail.To.Add("support@onec.co.uk");

                mail.Subject = DarkTools.pcNameV1() + " - " + "Device Alert";
                mail.Body = "A problem has been reported with device" + " " + DarkTools.pcNameV1() + " " +", Can you call site and get some more information." +  "Client Site URL - " + DarkTools.ReadSiteEndpoint;


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("monitor@onec.co.uk", "Onec!2345!");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch
            {

               

            }
        }

        public static string getTicketRef()
        {
           DarkTools.ReadConfigToXml();

            string d = "https://" + DarkTools.ReadSiteEndpoint;

            var GetConfig = API.Get(d, MonitorV1.getPostIDV1(), API.userName(), API.passWord(), "posts", "log_t");

            Acf id = JsonConvert.DeserializeObject<Acf>(GetConfig);

            string PostID = id.log_t;

            LogFile.LogMessageToFile("Ticket Ref, Received");

            return PostID;

            
        }

        //public static void postTicket()
        //{

        //    DarkTools.ReadConfigToXml();

        //    string d = "https://" + DarkTools.ReadSiteEndpoint;

        //    var acf_fields = new
        //    {



        //        //Post to ACF Fields
        //        monitor_register = new // You must have this for ACf
        //        {  //<--This needs to be a proper object

        //            pc_name_onec = DarkTools.pcNameV1(),
        //            ip_address_onec = DarkTools.GetLocalIPAddress(),
        //            mac_address_onec = DarkTools.GetMACAddress(),
        //            status_onec = "Online",
        //            post_id_onec = DarkTools.PostId,
        //            time_stamp_onec = DarkTools.timestamp.ToString(),

        //        },  //END You must have this for ACf

        //    };

        //    var content1 = new
        //    {
        //        acf_fields = new
        //        {
        //            monitor_register = acf_fields,
        //        }
        //    };
        //    string options = "options\\option";

        //    API.Post(d, MonitorV1.getPostIDV1(), DarkTools.UserName, DarkTools.Password, options, null,  content1.ToString() );
        //}

        public static void update_Ref()
        {

            DarkTools.ReadConfigToXml();

            string d = "https://" + DarkTools.ReadSiteEndpoint;

            string content11 = "{\"acf_fields\": {\"log_t\":\"0\"}}";

            API.Post(d, MonitorV1.getPostIDV1(), API.userName(), API.passWord(), "posts", content11,  "log_t");

            LogFile.LogMessageToFile("Ticket Ref, has been updated.");
            

            
        }

        public static void checkTicketRef()
        {
            if(getTicketRef() == "1")
            {
                email_Support();
                update_Ref();
                LogFile.LogMessageToFile("Ticket Sent");
            }
            else
            {
                LogFile.LogMessageToFile("NO ticket to report");
            }
        }

    }
}
