using IniParser;
using IniParser.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Silverlake.SynClient
{
    public class Services
    {
        public static string ApiBaseURL = "";
        public static string ApiReportStatus = "";
        public static string ApiFileTransmission = "";

        public static string ADCStatusPath = "";
        public static string FromADCPath = "";
        public static string SplitFromADCPath = "";

        public static string LogFile = "";

        public static string ApiAuthToken = "";

        public static int index = 0;

        public static LogWriter logWriter = new LogWriter("Log Started");

        public Services()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("SyncClientConfig.ini");

            KeyData keyApiBaseURL = data.Global.GetKeyData("ApiBaseURL");
            ApiBaseURL = keyApiBaseURL.Value;
            KeyData keyApiReportStatus = data.Global.GetKeyData("ApiReportStatus");
            ApiReportStatus = keyApiReportStatus.Value;
            KeyData keyApiFileTransmission = data.Global.GetKeyData("ApiFileTransmission");
            ApiFileTransmission = keyApiFileTransmission.Value;

            KeyData keyADCStatusPath = data.Global.GetKeyData("ADCStatusPath");
            ADCStatusPath = keyADCStatusPath.Value;
            KeyData keyFromADCPath = data.Global.GetKeyData("FromADCPath");
            FromADCPath = keyFromADCPath.Value;
            KeyData keySplitFromADCPath = data.Global.GetKeyData("SplitFromADCPath");
            SplitFromADCPath = keySplitFromADCPath.Value;

            KeyData keyLogFile = data.Global.GetKeyData("LogFile");
            LogFile = keyLogFile.Value;

            KeyData keyApiAuthToken = data.Global.GetKeyData("ApiAuthToken");
            ApiAuthToken = keyApiAuthToken.Value;
        }

        public static bool PostBatchStatusXMLFile(string batchStatusXMLPath)
        {
            try
            {
                logWriter = new LogWriter("Post Status: " + batchStatusXMLPath);
                string baseURL = ApiBaseURL;
                string postXml = File.ReadAllText(batchStatusXMLPath);
                string destinationUrl = baseURL + ApiReportStatus;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Timeout = Timeout.Infinite;
                request.Headers.Add("X-ApiKey", ApiAuthToken);
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ApiAuthToken));
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
                            logWriter = new LogWriter("Post Status: " + batchStatusXMLPath + " - Successfull");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Post Status: " + batchStatusXMLPath + " - Failure");
            }
            return false;
        }

        public static List<string> PostBatchXmlToSplit(string batchXMLPath)
        {
            List<string> setXMLPaths = new List<string>();
            logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath);
            try
            {
                string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index;
                string fileExtension = Path.GetExtension(batchXMLPath);
                string fileNameWithExtension = Path.GetFileName(batchXMLPath);
                string fileName = Path.GetFileNameWithoutExtension(batchXMLPath);
                if (fileExtension.ToLower() == ".xml")
                {
                    string batchKey = fileName.Split('_')[0];
                    string DepartmentId = fileName.Split('_')[1];
                    string BranchId = fileName.Split('_')[2];

                    List<string> appendicesUrls = new List<string>();
                    XDocument xdoc = XDocument.Load(batchXMLPath);
                    if (xdoc.Root.Name != "records")
                    {
                        if (File.Exists(FromADCPath + "Exception\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Exception\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Exception\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
                    }
                    else if (xdoc.Root.Elements().ToList().Count == 0)
                    {
                        if (File.Exists(FromADCPath + "Exception\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Exception\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Exception\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
                    }
                    else if (xdoc.Root.Elements().First().Name != "batches")
                    {
                        if (File.Exists(FromADCPath + "Exception\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Exception\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Exception\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
                    }
                    else if (xdoc.Root.Elements().First().Elements().ToList().Count == 0)
                    {
                        if (File.Exists(FromADCPath + "Exception\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Exception\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Exception\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
                    }
                    else
                        if (xdoc.Root.Name == "records" && xdoc.Root.Elements().First().Name == "batches")
                    {
                        var batchElement = xdoc.Root.Elements().First();
                        foreach (var setElement in batchElement.Elements())
                        {
                            string setKey = setElement.Attribute("GUID").Value;

                            var firstFieldElement = setElement.Elements("forms").First().Elements("field").First();
                            string setAaNo =
                                firstFieldElement.Attribute("name").Value == "AA_NUMBER" ?
                                    firstFieldElement.Value :
                                firstFieldElement.Attribute("name").Value == "Welfare Code" ?
                                    firstFieldElement.Value :
                                firstFieldElement.Attribute("name").Value == "Project Code" ?
                                    firstFieldElement.Value : "";
                            //Acc_Number
                            string accNo = "NNN";
                            var accFieldElement = setElement.Elements("forms").First().Elements("field").Where(x => x.Attribute("name").Value == "Acc_Number").FirstOrDefault();
                            if (accFieldElement != null)
                            {
                                accNo = accFieldElement.Value;
                            }
                            string CommonFileName = batchKey + "_" + setKey + "_" + setAaNo + "_" + DepartmentId + "_" + BranchId + "_" + accNo;

                            string XMLFilePath = SplitFromADCPath + CommonFileName + "_" + currentDateString;
                            string XMLFilePathException = FromADCPath + "Exception\\" + CommonFileName + "_" + currentDateString;

                            List<string> pdfs = new List<string>();
                            foreach (var formElemement in setElement.Elements().Skip(1))
                            {
                                var docType = formElemement.Elements("field").Where(x => x.Attribute("name").Value == "DOC_TYPE").FirstOrDefault();
                                var appendices = formElemement.Elements("field").Where(x => x.Attribute("name").Value == "APPENDICES").FirstOrDefault();
                                if (appendices != null)
                                {
                                    List<string> subUrls = new List<string>();
                                    string url = appendices.Value;
                                    string type = docType.Value;
                                    if (url.Contains("@@"))
                                    {
                                        string[] subs = url.Split(new string[] { "@@" }, StringSplitOptions.None);
                                        string firstFileUrl = subs[0];
                                        string firstFileName = Path.GetFileName(subs[0]);
                                        string commonFilePath = firstFileUrl.Replace(firstFileName, "");
                                        int idx = 0;
                                        foreach (string sub in subs)
                                        {
                                            if (idx == 0)
                                                subUrls.Add(sub);
                                            else
                                                subUrls.Add(commonFilePath + sub);
                                            idx++;
                                        }
                                    }
                                    else
                                    {
                                        subUrls.Add(url);
                                    }
                                    Document doc = new Document();
                                    doc.SetPageSize(PageSize.A4);
                                    string PDFFilePath = SplitFromADCPath + CommonFileName + "_" + type + "_" + subUrls.Count + "_" + currentDateString + ".pdf";
                                    PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(PDFFilePath, FileMode.Create));
                                    doc.Open();
                                    foreach (string subURL in subUrls)
                                    {
                                        string urlFileExtension = Path.GetExtension(subURL);
                                        string urlFileNameWithExtension = Path.GetFileName(subURL);
                                        if (urlFileExtension.ToLower() == ".tif")
                                        {
                                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(subURL);
                                            image.SetAbsolutePosition(0, 0);
                                            image.ScaleAbsoluteHeight(doc.PageSize.Height);
                                            image.ScaleAbsoluteWidth(doc.PageSize.Width);
                                            doc.NewPage();
                                            doc.Add(image);
                                        }
                                    }
                                    pdfs.Add(PDFFilePath);
                                    doc.Close();
                                    doc.Dispose();
                                    appendicesUrls.AddRange(subUrls);
                                }
                            }
                            string setXML = XMLFilePath + "_" + pdfs.Count + fileExtension;
                            setElement.Save(setXML);
                            setXMLPaths.Add(setXML);
                        }
                        if (File.Exists(FromADCPath + "Archive\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Archive\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Archive\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Successfull");
                    }
                    else
                    {
                        if (File.Exists(FromADCPath + "Exception\\" + fileNameWithExtension))
                            File.Delete(FromADCPath + "Exception\\" + fileNameWithExtension);
                        File.Move(batchXMLPath, FromADCPath + "Exception\\" + fileNameWithExtension);
                        logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
                    }
                    UpdateBatchCount(batchKey, setXMLPaths.Count);
                }
                index++;
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath + " - Failure");
            }
            return setXMLPaths;
        }

        public static void UpdateBatchCount(string batchKey, int batchCount)
        {
            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey)) + "='" + batchKey + "'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + "='4'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + "='1'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + "='1'");
            List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
            if (batches.Count > 0)
            {
                Batch batch = batches.FirstOrDefault();
                List<Branch> branches = ApiCalls.GetBranches();
                List<Department> departments = ApiCalls.GetDepartments();
                while (branches.Count == 0)
                {
                    branches = ApiCalls.GetBranches();
                }
                while (departments.Count == 0)
                {
                    departments = ApiCalls.GetDepartments();
                }
                Branch branch = branches.Where(x => x.Id == batch.BranchId).FirstOrDefault();
                Department department = departments.Where(x => x.Id == batch.DepartmentId).FirstOrDefault();
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
                DateTime currentDate = DateTime.Now;
                string key = currentDate.ToString("yyyyMMddHHmmssfff");
                string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
                el.AppendChild(doc.CreateElement("BranchId")).InnerText = branch.Code;
                el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = department.Code;
                el.AppendChild(doc.CreateElement("StageId")).InnerText = batch.StageId.ToString();
                el.AppendChild(doc.CreateElement("BatchKey")).InnerText = batch.BatchKey;
                el.AppendChild(doc.CreateElement("UserId")).InnerText = batch.BatchUser;
                el.AppendChild(doc.CreateElement("BatchNo")).InnerText = batch.BatchNo;
                el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batchCount.ToString();
                el.AppendChild(doc.CreateElement("Status")).InnerText = batch.BatchStatus.ToString();
                el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = batch.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = currentDateString;
                string statusFileName = ADCStatusPath + "Archive\\" + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + key + "_" + department.Code + "_" + branch.Code + ".xml";
                doc.Save(statusFileName);
                PostBatchStatusXMLFile(statusFileName);
            }
        }

        public static bool PostSetFile(string setFilePathfile)
        {
            string file = setFilePathfile;
            logWriter = new LogWriter("Post Set: " + setFilePathfile);
            string fileExtension = Path.GetExtension(file);
            string fileNameWithExtension = Path.GetFileName(file);
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
                    string[] matchedFiles = Directory.GetFiles(SplitFromADCPath, commonFileName + "*");
                    if (matchedFiles.ToList().Count - 1 == Convert.ToInt32(splitFileName.ToList().Last().ToString()))
                    {
                        filesCount = matchedFiles.Count();
                        foreach (string matchFile in matchedFiles)
                        {
                            string FileName = Path.GetFileName(matchFile);
                            string baseURL = ApiBaseURL;
                            string destinationUrl = baseURL + ApiFileTransmission;
                            byte[] bytes = System.IO.File.ReadAllBytes(matchFile);
                            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(matchFile);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                            request.Headers.Add("FileName", FileName);
                            request.ContentType = mimeType + "; encoding='utf-8'";
                            request.ContentLength = bytes.Length;
                            request.Method = "POST";
                            request.Timeout = Timeout.Infinite;
                            request.Headers.Add("X-ApiKey", ApiAuthToken);
                            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ApiAuthToken));
                            request.Headers.Add("Authorization", "Basic " + encoded);
                            request.UseDefaultCredentials = true;
                            request.PreAuthenticate = true;
                            request.Credentials = CredentialCache.DefaultCredentials;
                            try
                            {
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
                                            filesSuccessfullCount++;
                                            logWriter = new LogWriter("Post Set file: " + matchFile + " - Successfull");
                                            filesSuccessfull.Add(matchFile);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logWriter = new LogWriter("Post Set file: " + setFilePathfile + " - Failure");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("Post Set: " + setFilePathfile + " - Failure");
                return false;
            }

            logWriter = new LogWriter("Post Set filesSuccessfullCount: " + filesSuccessfullCount + " - filesCount:" + filesCount);
            if (filesSuccessfullCount == filesCount)
            {
                foreach (string successFile in filesSuccessfull)
                {
                    string successFileName = Path.GetFileName(successFile);
                    if (File.Exists(SplitFromADCPath + "Archive\\" + successFile))
                        File.Delete(SplitFromADCPath + "Archive\\" + successFile);
                    File.Move(successFile, SplitFromADCPath + "Archive\\" + successFileName);
                }
                return true;
            }
            return false;
        }
    }
}
