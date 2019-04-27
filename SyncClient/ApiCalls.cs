using IniParser;
using IniParser.Model;
using SyncClient;
using SyncClient.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace SynClient
{
    public class ApiCalls
    {
        public static string ApiBaseURL = "";
        public static string ApiReportStatus = "";
        public static string ApiFileTransmission = "";

        public ApiCalls()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("SyncClientConfig.ini");

            KeyData keyApiBaseURL = data.Global.GetKeyData("ApiBaseURL");
            ApiBaseURL = keyApiBaseURL.Value;
            KeyData keyApiReportStatus = data.Global.GetKeyData("ApiReportStatus");
            ApiReportStatus = keyApiReportStatus.Value;
            KeyData keyApiFileTransmission = data.Global.GetKeyData("ApiFileTransmission");
            ApiFileTransmission = keyApiFileTransmission.Value;
        }

        public static List<Branch> GetBranches()
        {
            new ApiCalls();
            List<Branch> objs = new List<Branch>();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = ApiBaseURL;
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync("Branches/Get").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Branch>>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter(ex.Message);
            }
            return objs;
        }

        public static List<Department> GetDepartments()
        {
            new ApiCalls();
            List<Department> objs = new List<Department>();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = ApiBaseURL;
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync("Departments/Get").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Department>>(responseString);
                    }
                }
            }
            catch(Exception ex)
            {
                LogWriter logWriter = new LogWriter(ex.Message);
            }
            return objs;
        }

        public static List<Batch> GetBatches(String filter)
        {
            new ApiCalls();
            List<Batch> objs = new List<Batch>();
            using (var client = new HttpClient())
            {
                string baseURL = ApiBaseURL;
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("Batches/Get?filter=" + filter).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Batch>>(responseString);
                }
                return objs;
            }
        }

    }
}
