using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Window.ServiceCalls
{
    public class AuthenticationApiCalls
    {
        public static User AuthenticateUser(string filter)
        {
            User user = new User();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync("Users/Get?filter=" + filter).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        List<User> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(responseString);
                        if (users.Count > 0)
                            user = users.FirstOrDefault();
                    }
                }
            }
            catch(Exception ex)
            {
                user.IsOnline = 0;
                user.UniqueKey = "API error!";
            }
            return user;
        }
    }
}
