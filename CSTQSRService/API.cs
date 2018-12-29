using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCSService
{
    class API
    {
        public static string domainName()
        {
            return "https://register.onec.systems";
        }

        public static string userName()
        {
            return "ocsagent";
        }

        public static string passWord()
        {
            return "KF#hsrDx@MfilSQJa&g*C2vU";
        }

        public static string Get(string Domain, string id, string username, string password, string contentType, string extra)
        {
            var client = new RestClient(Domain + "/wp-json/acf/v3/" + contentType + "/" + id + "/" + extra);
            client.Authenticator = new HttpBasicAuthenticator(userName(), passWord());
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("content-type", "application/json");
            IRestResponse response2 = client.Execute(request2);

            return response2.Content.ToString();
        }

        public static string Post(string domain, string id, string username, string password, string contentType, string content, string extra)
        {

            var content1 = content;

            var client = new RestClient(domain + "/wp-json/acf/v3/" + contentType + "/" + id + "/" + extra);
            client.Authenticator = new HttpBasicAuthenticator(userName(), passWord());
            var request2 = new RestRequest(Method.POST);
            request2.AddHeader("content-type", "application/json");
            //request2.AddJsonBody(content1); //<-- this will serialize and add the model as a JSON body.
            request2.AddParameter("application/json", content1, ParameterType.RequestBody);
            IRestResponse response2 = client.Execute(request2);

            return null;
        }




    }
}
