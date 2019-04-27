using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.ValidationAPI
{
    public class CustomValidator
    {
        public static AAValidateResponse IsValidNo(string job, string type, string no)
        {
            AAValidateResponse aAValidateResponse = new AAValidateResponse();
            if (job != "" && type != "" && no != "")
            {
                try
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile("ValidationAPISettings.ini");
                    string apiUrl = data.Global.GetKeyData(job + type).Value;
                    var uri = new Uri(apiUrl);
                    var baseUri = uri.GetLeftPart(System.UriPartial.Authority);
                    var destinationUri = apiUrl.Replace(baseUri, "");
                    using (var client = new HttpClient())
                    {
                        string baseURL = baseUri;
                        client.BaseAddress = new Uri(baseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        string destinationUrl = "";

                        if (destinationUri != "")
                        {
                            destinationUrl = destinationUri.Replace("##NO##", no);
                            var response = client.GetAsync(destinationUrl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = response.Content.ReadAsStringAsync().Result;
                                aAValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AAValidateResponse>(responseString);
                            }
                            else
                            {
                                aAValidateResponse.Result = "Connection failure";
                            }
                        }
                        else
                        {
                            aAValidateResponse.Result = "Destination unspecified";
                        }
                    }
                }
                catch (Exception ex)
                {
                    aAValidateResponse.Result = ex.Message;
                }
            }
            else {
                aAValidateResponse.ReservedField = "No Job/Type/No";
                aAValidateResponse.Result = "AA";
            }
            return aAValidateResponse;
        }
    }

    public class AAValidateResponse
    {
        [JsonProperty("Result")]
        public string Result { get; set; }
        [JsonProperty("Account Name")]
        public string AccountName { get; set; }
        [JsonProperty("Reserved Field")]
        public string ReservedField { get; set; }
    }
}
