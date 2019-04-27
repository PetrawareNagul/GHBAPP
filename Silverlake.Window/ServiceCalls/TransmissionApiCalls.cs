using Silverlake.Utility;
using Silverlake.Utility.Helper;
using Silverlake.Window.Custom;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Silverlake.Window.ServiceCalls
{
    public class TransmissionApiCalls
    {
        public static string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
        public static string FromADCArchive = ConfigurationManager.AppSettings["FromADCArchive"].ToString();
        public static string FromADCException = ConfigurationManager.AppSettings["FromADCException"].ToString();
        public static string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
        public static string SplitFromADCProcessing = ConfigurationManager.AppSettings["SplitFromADCProcessing"].ToString();
        public static string SplitFromADCArchive = ConfigurationManager.AppSettings["SplitFromADCArchive"].ToString();
        public static string SplitFromADCException = ConfigurationManager.AppSettings["SplitFromADCException"].ToString();
        public static string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
        public static string ADCStatusArchive = ConfigurationManager.AppSettings["ADCStatusArchive"].ToString();
        public static string ADCStatusException = ConfigurationManager.AppSettings["ADCStatusException"].ToString();
        public static string BranchId = ConfigurationManager.AppSettings["BranchId"].ToString();
        public static string DepartmentId = ConfigurationManager.AppSettings["DepartmentId"].ToString();

        public static string PostXMLFile(string postXmlUrl)
        {
            string FileName = Path.GetFileName(postXmlUrl);
            string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
            string postXml = System.IO.File.ReadAllText(postXmlUrl);
            string destinationUrl = baseURL + "/FileTransmission/PostFile";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            request.Headers.Add("FileName", FileName);
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

        //public async static Task<int> PostFileAsync(string postUrl, HttpClient httpClient)
        //{
        //    MultipartFormDataContent form = new MultipartFormDataContent();
        //    FileStream fs = File.OpenRead(postUrl);
        //    var streamContent = new StreamContent(fs);

        //    var imageContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
        //    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        //    form.Add(imageContent, "image", Path.GetFileName(postUrl));
        //    var response = httpClient.PostAsync(url, form).Result;

        //    return 0;
        //}
        public async static Task<string> PostAllFiles(string file)
        {
            LogWriter logWriter = new LogWriter("Post All Files: " + file);
            User user = APIUser.GetUser();
            string fileExtension = Path.GetExtension(file);
            string fileNameWithExtension = Path.GetFileName(file);
            //string fileName = Path.GetFileNameWithoutExtension(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            string[] splitFileName = fileName.Split('_');
            int filesSuccessfullCount = 0;
            List<string> filesSuccessfull = new List<string>();
            int filesCount = 0;
            try
            {
                if (fileExtension.ToLower() == ".xml")
                {
                    string commonFileName = String.Join("_", splitFileName.ToList<string>().Take(3).ToArray());
                    string[] matchedFiles = Directory.GetFiles(SplitFromADC, commonFileName + "*");
                    if (matchedFiles.ToList().Count - 1 == Convert.ToInt32(splitFileName.ToList().Last().ToString()))
                    {
                        filesCount = matchedFiles.Count();
                        foreach (string matchFile in matchedFiles)
                        {
                            //fileNameWithExtension = Path.GetFileName(matchFile);
                            logWriter = new LogWriter("Post All Files matchFile: " + matchFile);
                            string FileName = Path.GetFileName(matchFile);
                            bool isSuccess = false;
                            while (!isSuccess)
                            {
                                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                                //string postImage = System.IO.File.ReadAllText(postImageUrl);
                                string destinationUrl = baseURL + "/FileTransmission/PostFile";
                                byte[] bytes = System.IO.File.ReadAllBytes(matchFile);
                                //string fileExtension = Path.GetExtension(matchFile);
                                string mimeType = MimeMapping.MimeUtility.GetMimeMapping(matchFile);
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                                request.Headers.Add("FileName", FileName);
                                request.ContentType = mimeType + "; encoding='utf-8'";
                                request.ContentLength = bytes.Length;
                                request.Method = "POST";
                                request.Timeout = Timeout.Infinite;
                                request.Headers.Add("X-ApiKey", user.ApiAuthToken);
                                String username = user.Username;
                                String password = CustomEncryptorDecryptor.DecryptPassword(user.Password);
                                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                                request.Headers.Add("Authorization", "Basic " + encoded);
                                request.UseDefaultCredentials = true;

                                request.PreAuthenticate = true;

                                request.Credentials = CredentialCache.DefaultCredentials;
                                //var task = request.GetRequestStreamAsync();
                                logWriter = new LogWriter("Post All Files While: " + file);
                                try
                                {
                                    using (var requestStream = await request.GetRequestStreamAsync())
                                    {
                                        requestStream.Write(bytes, 0, bytes.Length);
                                    }

                                    using (var response = request.GetResponse())
                                    {
                                        using (var responseStream = response.GetResponseStream())
                                        {
                                            using (var sr = new StreamReader(responseStream))
                                            {
                                                string responseStr = sr.ReadToEnd();
                                                //if (File.Exists(SplitFromADCArchive + fileName))
                                                //    File.Delete(SplitFromADCArchive + fileName);
                                                //File.Move(matchFile, SplitFromADCArchive + fileName);
                                                filesSuccessfullCount++;
                                                filesSuccessfull.Add(matchFile);
                                                isSuccess = true;
                                                //return splitFileName[0];
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    isSuccess = false;
                                    logWriter = new LogWriter("Post All Files While Exception: " + ex.Message);
                                }
                            }
                            //Task<string> task = TransmissionApiCalls.PostFile(matchFile);
                            //TaskList.Add(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Postc All Files File: "+ file);
                logWriter = new LogWriter("Post All Files Exception: "+ ex.Message);
                return null;
            }

            if (filesSuccessfullCount == filesCount)
            {
                foreach (string successFile in filesSuccessfull)
                {
                    string successFileName = Path.GetFileName(successFile);
                    if (File.Exists(SplitFromADCArchive + successFile))
                        File.Delete(SplitFromADCArchive + successFile);
                    File.Move(successFile, SplitFromADCArchive + successFileName);
                }
                return splitFileName[0];
            }
            //else
            //{
            //    foreach(string successFile in filesSuccessfull)
            //    {
            //        if (File.Exists(SplitFromADCArchive + fileName))
            //            File.Delete(SplitFromADCArchive + fileName);
            //        File.Move(successFile, SplitFromADCArchive + fileName);
            //    }
            //}

            return null;
        }

        public async static Task<string> PostFile(string postUrl)
        {
            LogWriter logWriter = new LogWriter(postUrl);
            User user = APIUser.GetUser();
            try
            {
                string FileName = Path.GetFileName(postUrl);
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                //string postImage = System.IO.File.ReadAllText(postImageUrl);
                string destinationUrl = baseURL + "/FileTransmission/PostFile";
                byte[] bytes = System.IO.File.ReadAllBytes(postUrl);
                string fileExtension = Path.GetExtension(postUrl);
                string mimeType = MimeMapping.MimeUtility.GetMimeMapping(postUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                request.Headers.Add("FileName", FileName);
                request.ContentType = mimeType + "; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Timeout = Timeout.Infinite;
                request.Headers.Add("X-ApiKey", user.ApiAuthToken);
                String username = user.Username;
                String password = CustomEncryptorDecryptor.DecryptPassword(user.Password);
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);
                request.UseDefaultCredentials = true;

                request.PreAuthenticate = true;

                request.Credentials = CredentialCache.DefaultCredentials;
                logWriter = new LogWriter("User: " + username + " : " + password);
                logWriter = new LogWriter("Authorization: " + "Basic " + encoded);
                //var task = request.GetRequestStreamAsync();
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    string responseStr = sr.ReadToEnd();
                    string[] splitFileName = FileName.Split('_');

                    string fileName = Path.GetFileName(postUrl);

                    if (File.Exists(SplitFromADCArchive + fileName))
                        File.Delete(SplitFromADCArchive + fileName);
                    File.Move(postUrl, SplitFromADCArchive + fileName);

                    return splitFileName[0];
                }
            }
            catch (WebException webex)
            {
                logWriter = new LogWriter("Exception: " + postUrl + " : " + webex.Message);
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Exception: " + postUrl + " : " + ex.Message);
            }
            return null;
        }

        public static string PostBatchStatusXMLFile(string postXmlUrl)
        {
            LogWriter logWriter = new LogWriter("ADC Status: " + postXmlUrl);
            User user = APIUser.GetUser();
            try
            {
                string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
                string postXml = File.ReadAllText(postXmlUrl);
                string destinationUrl = baseURL + "/BatchStatus/PostFile";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Timeout = Timeout.Infinite;
                request.Headers.Add("X-ApiKey", user.ApiAuthToken);
                String username = user.Username;
                String password = CustomEncryptorDecryptor.DecryptPassword(user.Password);
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream))
                        {
                            string responseStr = sr.ReadToEnd();
                            return responseStr;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Exception: " + ex.Message);
            }
            return null;
        }

        public static string PostNewBatchXML(XmlDocument doc)
        {
            User user = APIUser.GetUser();
            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            Byte[] bytes = xmlStream.ToArray();

            string baseURL = ConfigurationManager.AppSettings["apiBaseURL"].ToString();
            string destinationUrl = baseURL + "/BatchStatus/PostFile";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            request.Timeout = Timeout.Infinite;
            request.Headers.Add("X-ApiKey", user.ApiAuthToken);
            String username = user.Username;
            String password = CustomEncryptorDecryptor.DecryptPassword(user.Password);
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string responseStr = sr.ReadToEnd();
                return responseStr;
            }
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