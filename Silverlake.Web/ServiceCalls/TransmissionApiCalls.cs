using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace Silverlake.Web.ServiceCalls
{
    public class TransmissionApiCalls
    {
        public static string PostXMLFile(string postXmlUrl)
        {
            string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
            string postXml = System.IO.File.ReadAllText(postXmlUrl);
            string destinationUrl = baseURL + "/XMLFileTransmission/PostFile";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }


        public static string PostFile(string postImageUrl)
        {
            string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
            //string postImage = System.IO.File.ReadAllText(postImageUrl);
            string destinationUrl = baseURL + "/FileTransmission/PostFile";
            byte[] bytes = System.IO.File.ReadAllBytes(postImageUrl);
            string fileExtension = Path.GetExtension(postImageUrl);
            string mimeType = MimeMapping.GetMimeMapping(postImageUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.ContentType = mimeType + "; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            request.Timeout = Timeout.Infinite;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                }
            }
            return null;
        }

        public static string PostBatchStatusXMLFile(string postXmlUrl)
        {
            string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
            string postXml = System.IO.File.ReadAllText(postXmlUrl);
            string destinationUrl = baseURL + "/BatchStatus/PostFile";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }

        //public static List<Sample> GetData()
        //{
        //    List<Sample> samples = new List<Sample>();
        //    using (var client = new HttpClient())
        //    {
        //        string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = client.GetAsync("Transmission/Get").Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseString = response.Content.ReadAsStringAsync().Result;
        //            samples = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Sample>>(responseString);
        //        }
        //        return samples;
        //    }
        //}
    }
}