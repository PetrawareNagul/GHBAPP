using IniParser;
using IniParser.Model;
using Silverlake.WindowServerSync.Custom;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.WindowServerSync.ServiceCalls
{
    public class CustomValidator
    {
        public static string EnableValidation = ConfigurationManager.AppSettings["EnableValidation"].ToString();

        public static string ValidationApiBaseUrl = ConfigurationManager.AppSettings["ValidationApiBaseUrl"].ToString();
        public static string AAValidationApiUrl = ConfigurationManager.AppSettings["AAValidationApiUrl"].ToString();
        public static string ACValidationApiUrl = ConfigurationManager.AppSettings["ACValidationApiUrl"].ToString();
        public static string PJValidationApiUrl = ConfigurationManager.AppSettings["PJValidationApiUrl"].ToString();
        public static string WFValidationApiUrl = ConfigurationManager.AppSettings["WFValidationApiUrl"].ToString();

        public static bool CheckConnection()
        {
            AAValidateResponse ValidateResponse = new AAValidateResponse();
            try
            {
                if (EnableValidation == "0")
                {
                    ValidateResponse.Result = "AA";
                    return true;
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        string baseURL = ValidationApiBaseUrl;
                        client.BaseAddress = new Uri(baseURL);
                        client.Timeout = new TimeSpan(0, 0, 1);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        string destinationUrl = AAValidationApiUrl;
                        destinationUrl = destinationUrl.Replace("##NO##", "1");
                        var response = client.GetAsync(destinationUrl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = response.Content.ReadAsStringAsync().Result;
                            ValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AAValidateResponse>(responseString);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static AAValidateResponse isValid(string type, string no)
        {
            AAValidateResponse ValidateResponse = new AAValidateResponse();
            try
            {
                if (EnableValidation == "0")
                {
                    ValidateResponse.Result = "AA";
                    return ValidateResponse;
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        string baseURL = ValidationApiBaseUrl;
                        client.BaseAddress = new Uri(baseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        string destinationUrl = "";
                        switch (type)
                        {
                            default:
                                destinationUrl = "";
                                break;
                            case "AA":
                                destinationUrl = AAValidationApiUrl;
                                break;
                            case "AC":
                                destinationUrl = ACValidationApiUrl;
                                break;
                            case "PJ":
                                destinationUrl = PJValidationApiUrl;
                                break;
                            case "WF":
                                destinationUrl = WFValidationApiUrl;
                                break;
                        }
                        if (destinationUrl != "")
                        {
                            destinationUrl = destinationUrl.Replace("##NO##", no);
                            var response = client.GetAsync(destinationUrl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = response.Content.ReadAsStringAsync().Result;
                                ValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AAValidateResponse>(responseString);
                            }
                            else
                            {
                                ValidateResponse.Result = "Connection failure";
                            }
                        }
                        else
                        {
                            ValidateResponse.Result = "Destination unspecified";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ValidateResponse.Result = "No connection";
            }
            return ValidateResponse;
        }

        public static AAValidateResponse isValidACandAA(string type, string no, string aa)
        {
            AAValidateResponse ValidateResponse = new AAValidateResponse();
            try
            {
                if (EnableValidation == "0")
                {
                    ValidateResponse.Result = "AA";
                    return ValidateResponse;
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        string baseURL = ValidationApiBaseUrl;
                        client.BaseAddress = new Uri(baseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        string destinationUrl = "";
                        switch (type)
                        {
                            default:
                                destinationUrl = "";
                                break;
                            case "AA":
                                destinationUrl = AAValidationApiUrl;
                                break;
                            case "AC":
                                destinationUrl = ACValidationApiUrl;
                                break;
                            case "PJ":
                                destinationUrl = PJValidationApiUrl;
                                break;
                            case "WF":
                                destinationUrl = WFValidationApiUrl;
                                break;
                        }
                        if (destinationUrl != "")
                        {
                            destinationUrl = destinationUrl.Replace("##NO##", no);
                            destinationUrl = destinationUrl.Replace("##AA##", aa);
                            var response = client.GetAsync(destinationUrl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = response.Content.ReadAsStringAsync().Result;
                                ValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AAValidateResponse>(responseString);
                            }
                            else
                            {
                                ValidateResponse.Result = "Connection failure";
                            }
                        }
                        else
                        {
                            ValidateResponse.Result = "Destination unspecified";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ValidateResponse.Result = "No connection";
            }
            return ValidateResponse;
        }

        public static AAValidateResponse isValidNo(string job, string type, string no)
        {
                AAValidateResponse aAValidateResponse = new AAValidateResponse();
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
            catch(Exception ex)
            {
                aAValidateResponse.Result = ex.Message;
            }
            return aAValidateResponse;
        }

    }
}
