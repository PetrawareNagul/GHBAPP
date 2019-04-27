using Silverlake.Window.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Window.ServiceCalls
{
    public class CustomValidator
    {
        public static AAValidateResponse isValidAccountNo(string aaNo)
        {
            AAValidateResponse ValidateResponse = new AAValidateResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string baseURL = "http://172.29.34.13/";
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync("CBSeTPD1/customize/E-Library/default.aspx?check=AA&CADAAN=" + aaNo + "&requesttype=json").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        ValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AAValidateResponse>(responseString);
                    }
                }
            }
            catch(Exception ex)
            {
                ValidateResponse.Result = "No connection";
            }
            return ValidateResponse;
        }
    }
}
