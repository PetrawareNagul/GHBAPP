using Silverlake.Utility;
using Silverlake.Utility.Helper;
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

namespace Silverlake.Window.ServiceCalls
{
    public class ApiCalls
    {
        public static List<Branch> GetBranches()
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
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
            List<Department> objs = new List<Department>();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
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
            List<Batch> objs = new List<Batch>();
            using (var client = new HttpClient())
            {
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
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

        public static List<Set> GetSets(String filter)
        {
            List<Set> objs = new List<Set>();
            using (var client = new HttpClient())
            {
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("Sets/Get?filter=" + filter).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Set>>(responseString);
                }
                return objs;
            }
        }

        public static List<Set> GetSetDocuments(String filter)
        {
            List<Set> objs = new List<Set>();
            using (var client = new HttpClient())
            {
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("Sets/Get?filter=" + filter).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Set>>(responseString);
                }
                return objs;
            }
        }


        public static List<Stage> GetStages()
        {
            List<Stage> objs = new List<Stage>();

            using (var client = new HttpClient())
            {
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("Stages/Get").Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    objs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stage>>(responseString);
                }
                return objs;
            }
        }
    }
}
