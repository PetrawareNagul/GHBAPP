using iTextSharp.text;
using iTextSharp.text.pdf;
using MetroFramework.Forms;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using Silverlake.Window.Custom;
using Silverlake.Window.Properties;
using Silverlake.Window.ServiceCalls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Silverlake.Window
{
    public partial class FolderWatcher : MetroForm
    {

        public bool isProcessingADCStatus = false;
        public bool isProcessingFromADC = false;
        public bool isProcessingSplitFromADC = false;

        public string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
        public string FromADCArchive = ConfigurationManager.AppSettings["FromADCArchive"].ToString();
        public string FromADCException = ConfigurationManager.AppSettings["FromADCException"].ToString();
        public string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
        public string SplitFromADCProcessing = ConfigurationManager.AppSettings["SplitFromADCProcessing"].ToString();
        public string SplitFromADCArchive = ConfigurationManager.AppSettings["SplitFromADCArchive"].ToString();
        public string SplitFromADCException = ConfigurationManager.AppSettings["SplitFromADCException"].ToString();
        public string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
        public string ADCStatusArchive = ConfigurationManager.AppSettings["ADCStatusArchive"].ToString();
        public string ADCStatusException = ConfigurationManager.AppSettings["ADCStatusException"].ToString();
        public string BranchId = ConfigurationManager.AppSettings["BranchId"].ToString();
        public string DepartmentId = ConfigurationManager.AppSettings["DepartmentId"].ToString();

        public FolderWatcher()
        {
            InitializeComponent();
            notifyIcon = new NotifyIcon()
            {
                Icon = Icon.FromHandle(Resources.favicon.GetHicon()),
                BalloonTipText = "For Branch",
                BalloonTipTitle = "Sync control",
                BalloonTipIcon = ToolTipIcon.Info,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Open", new EventHandler(open_Click)),
                    new MenuItem("Show notification", new EventHandler(showNotification_Click)),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };
            notifyIcon.BalloonTipClosed += new EventHandler(notifyIcon_BalloonTipClosed);
            notifyIcon.MouseDoubleClick += new MouseEventHandler(notifyIcon_MouseDoubleClick);
            this.ShowInTaskbar = false;
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            this.Resize += new EventHandler(FolderWatcher_Resize);
        }

        private void showNotification_Click(object sender, EventArgs e)
        {
            notifyIcon.ShowBalloonTip(1000);
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipText = "For Branch";
            notifyIcon.BalloonTipTitle = "Sync control";
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            notifyIcon.Visible = false;
            Application.Exit();
        }
        private void open_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
            this.Visible = true;
        }

        private BackgroundWorker backgroundWorkerProcessADCStatusFiles;
        private BackgroundWorker backgroundWorkerProcessFromADCFiles;
        private BackgroundWorker backgroundWorkerProcessSplitFromADCFiles;
        private void InitializeBackgroundWorkerADCStatusFiles()
        {
            backgroundWorkerProcessADCStatusFiles = new BackgroundWorker();

            backgroundWorkerProcessADCStatusFiles.DoWork += new DoWorkEventHandler(backgroundWorkerProcessADCStatusFiles_DoWork);
            backgroundWorkerProcessADCStatusFiles.WorkerReportsProgress = false;
            backgroundWorkerProcessADCStatusFiles.WorkerSupportsCancellation = true;
            //backgroundWorkerProcessADCStatusFiles.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProcessADCStatusFiles_RunWorkerCompleted);
            backgroundWorkerProcessADCStatusFiles.RunWorkerAsync();
        }

        private void InitializeBackgroundWorkerFromADCFiles()
        {
            backgroundWorkerProcessFromADCFiles = new BackgroundWorker();

            backgroundWorkerProcessFromADCFiles.DoWork += new DoWorkEventHandler(backgroundWorkerProcessFromADCFiles_DoWork);
            backgroundWorkerProcessFromADCFiles.WorkerReportsProgress = false;
            backgroundWorkerProcessFromADCFiles.WorkerSupportsCancellation = true;
            //backgroundWorkerProcessFromADCFiles.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProcessFromADCFiles_RunWorkerCompleted);
            backgroundWorkerProcessFromADCFiles.RunWorkerAsync();
        }

        private void InitializeBackgroundWorkerSplitFromADCFiles()
        {
            backgroundWorkerProcessSplitFromADCFiles = new BackgroundWorker();

            backgroundWorkerProcessSplitFromADCFiles.DoWork += new DoWorkEventHandler(backgroundWorkerProcessSplitFromADCFiles_DoWork);
            backgroundWorkerProcessSplitFromADCFiles.WorkerReportsProgress = false;
            backgroundWorkerProcessSplitFromADCFiles.WorkerSupportsCancellation = true;
            backgroundWorkerProcessSplitFromADCFiles.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProcessSplitFromADCFiles_RunWorkerCompleted);
            backgroundWorkerProcessSplitFromADCFiles.RunWorkerAsync();
        }

        private bool IsClosePending;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (backgroundWorkerProcessADCStatusFiles.IsBusy || backgroundWorkerProcessFromADCFiles.IsBusy || backgroundWorkerProcessSplitFromADCFiles.IsBusy)
            {
                IsClosePending = true;
                backgroundWorkerProcessADCStatusFiles.CancelAsync();
                backgroundWorkerProcessFromADCFiles.CancelAsync();
                backgroundWorkerProcessSplitFromADCFiles.CancelAsync();
                return;
            }
            notifyIcon.Visible = false;
            base.OnFormClosing(e);
        }


        private void backgroundWorkerProcessADCStatusFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            isProcessingADCStatus = true;
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = ProcessADCStatusFiles(worker, e);
        }
        private void backgroundWorkerProcessADCStatusFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                //resultLabel.Text = "Canceled";
            }
            else
            {
                isProcessingADCStatus = false;
                if (!IsClosePending)
                {
                    //if (!backgroundWorkerProcessADCStatusFiles.IsBusy)
                    //{
                    //    backgroundWorkerProcessADCStatusFiles.RunWorkerAsync();
                    //}
                    InitializeBackgroundWorkerFromADCFiles();
                }
                //resultLabel.Text = e.Result.ToString();
            }
        }

        private void backgroundWorkerProcessFromADCFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!isProcessingADCStatus)
            {
                isProcessingFromADC = true;
                BackgroundWorker worker = sender as BackgroundWorker;
                e.Result = ProcessFromADCFiles(worker, e);
            }
        }
        private void backgroundWorkerProcessFromADCFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //CheckIsCompletedAndClose();
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                //resultLabel.Text = "Canceled";
            }
            else
            {
                isProcessingFromADC = false;
                if (!IsClosePending)
                {
                    if (!backgroundWorkerProcessFromADCFiles.IsBusy)
                    {
                        InitializeBackgroundWorkerSplitFromADCFiles();
                    }
                }
                //resultLabel.Text = e.Result.ToString();
            }
        }

        private void backgroundWorkerProcessSplitFromADCFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!isProcessingSplitFromADC)
            {
                isProcessingSplitFromADC = true;
                BackgroundWorker worker = sender as BackgroundWorker;
                e.Result = ProcessSplitFromADCFiles(worker, e);
            }
        }
        private void backgroundWorkerProcessSplitFromADCFiles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isProcessingSplitFromADC = false;
            //CheckIsCompletedAndClose();
            //if (e.Error != null)
            //{
            //    MessageBox.Show(e.Error.Message);
            //}
            //else if (e.Cancelled)
            //{
            //    //resultLabel.Text = "Canceled";
            //}
            //else
            //{
            //    isProcessingSplitFromADC = false;
            //    if (!IsClosePending)
            //    {
            //        if (!backgroundWorkerProcessSplitFromADCFiles.IsBusy)
            //        {

            //            backgroundWorkerProcessSplitFromADCFiles.RunWorkerAsync();
            //        }
            //    }
            //    //resultLabel.Text = e.Result.ToString();
            //}
        }

        public void CheckIsCompletedAndClose()
        {
            if (!backgroundWorkerProcessADCStatusFiles.CancellationPending && !backgroundWorkerProcessFromADCFiles.CancellationPending && !backgroundWorkerProcessSplitFromADCFiles.CancellationPending)
                if (!backgroundWorkerProcessADCStatusFiles.IsBusy && !backgroundWorkerProcessFromADCFiles.IsBusy && !backgroundWorkerProcessSplitFromADCFiles.IsBusy)
                {
                    IsClosePending = false;
                    this.Close();
                }
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public object ProcessADCStatusFiles(BackgroundWorker worker, DoWorkEventArgs e)
        {
            //-----------------------------------
            DirectoryInfo info = new DirectoryInfo(ADCStatus);
            FileInfo[] filesByTime = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            if (filesByTime.ToList().Count == 0)
                isProcessingADCStatus = false;
            foreach (FileInfo fileInfo in filesByTime)
            {
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                DateTime currentTime = DateTime.Now;
                currentTime = currentTime.AddSeconds(-2);
                if (lastWriteTime <= currentTime)
                {
                    string file = fileInfo.FullName;
                    if (!IsFileLocked(fileInfo))
                    {
                        string fileNameWithExtension = Path.GetFileName(file);
                        try
                        {
                            TransmissionApiCalls.PostBatchStatusXMLFile(file);
                            if (File.Exists(ADCStatusArchive + fileNameWithExtension))
                                File.Delete(ADCStatusArchive + fileNameWithExtension);
                            File.Move(file, ADCStatusArchive + fileNameWithExtension);
                        }
                        catch (Exception ex)
                        {
                            if (File.Exists(ADCStatusException + fileNameWithExtension))
                                File.Delete(ADCStatusException + fileNameWithExtension);
                            File.Move(file, ADCStatusException + fileNameWithExtension);
                        }
                    }
                }
            }
            //-----------------------------------
            //var filesByTime = Directory.GetFiles(ADCStatus).OrderBy(d => new FileInfo(d).CreationTime);
            //string[] files = Directory.GetFiles(ADCStatus);
            //if (files.ToList().Count == 0)
            //    isProcessingADCStatus = false;
            //foreach (string file in files)
            //{
            //    FileInfo fileInfo = new FileInfo(file);
            //    DateTime lastWriteTime = fileInfo.LastWriteTime;
            //    DateTime currentTime = DateTime.Now;
            //    currentTime = currentTime.AddSeconds(-2);
            //    if (lastWriteTime <= currentTime)
            //    {

            //    }
            //}
            return "";
        }

        public object ProcessFromADCFiles(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (!isProcessingADCStatus)
            {
                DirectoryInfo info = new DirectoryInfo(FromADC);
                FileInfo[] filesByTime = info.GetFiles("*.xml").OrderBy(p => p.CreationTime).ToArray();
                if (filesByTime.ToList().Count == 0)
                    isProcessingFromADC = false;
                int index = 1;
                foreach (FileInfo fileInfo in filesByTime)
                {
                    DateTime lastWriteTime = fileInfo.LastWriteTime;
                    DateTime currentTime = DateTime.Now;
                    currentTime = currentTime.AddSeconds(-3);
                    if (lastWriteTime <= currentTime)
                    {
                        string file = fileInfo.FullName;
                        if (!IsFileLocked(fileInfo))
                        {
                            string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index;
                            string fileExtension = Path.GetExtension(file);
                            string fileNameWithExtension = Path.GetFileName(file);
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            if (fileExtension.ToLower() == ".xml")
                            {
                                string batchKey = fileName.Split('_')[0];
                                DepartmentId = fileName.Split('_')[1];
                                BranchId = fileName.Split('_')[2];

                                StringBuilder filter = new StringBuilder();
                                filter.Append(" 1=1");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey)) + "='" + batchKey + "'");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + "='4'");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + "='1'");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + "='1'");
                                List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                                if (batches.Count > 0)
                                {
                                    List<string> appendicesUrls = new List<string>();
                                    XDocument xdoc = XDocument.Load(file);
                                    if (xdoc.Root.Name != "records")
                                    {
                                        if (File.Exists(FromADCException + fileNameWithExtension))
                                            File.Delete(FromADCException + fileNameWithExtension);
                                        File.Move(file, FromADCException + fileNameWithExtension);
                                    }
                                    else if (xdoc.Root.Elements().ToList().Count == 0)
                                    {
                                        if (File.Exists(FromADCException + fileNameWithExtension))
                                            File.Delete(FromADCException + fileNameWithExtension);
                                        File.Move(file, FromADCException + fileNameWithExtension);
                                    }
                                    else if (xdoc.Root.Elements().First().Name != "batches")
                                    {
                                        if (File.Exists(FromADCException + fileNameWithExtension))
                                            File.Delete(FromADCException + fileNameWithExtension);
                                        File.Move(file, FromADCException + fileNameWithExtension);
                                    }
                                    else if (xdoc.Root.Elements().First().Elements().ToList().Count == 0)
                                    {
                                        if (File.Exists(FromADCException + fileNameWithExtension))
                                            File.Delete(FromADCException + fileNameWithExtension);
                                        File.Move(file, FromADCException + fileNameWithExtension);
                                    }
                                    else
                                    if (xdoc.Root.Name == "records" && xdoc.Root.Elements().First().Name == "batches")
                                    {
                                        var batchElement = xdoc.Root.Elements().First();
                                        int batchCount = 0;
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
                                            AAValidateResponse ValidateResponse = new AAValidateResponse();//CustomValidator.isValidAccountNo(setAccNo);
                                            ValidateResponse.Result = "AA";
                                            if (ValidateResponse.Result == "AA")
                                                batchCount++;
                                            string CommonFileName = batchKey + "_" + setKey + "_" + setAaNo + "_" + DepartmentId + "_" + BranchId + "_" + accNo;

                                            string XMLFilePath = SplitFromADC + CommonFileName + "_" + currentDateString;
                                            string XMLFilePathException = FromADCException + CommonFileName + "_" + currentDateString;

                                            List<string> pdfs = new List<string>();
                                            if (ValidateResponse.Result == "AA")
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
                                                        if (ValidateResponse.Result == "AA")
                                                        {
                                                            Document doc = new Document();
                                                            doc.SetPageSize(PageSize.A4);
                                                            string PDFFilePath = SplitFromADC + CommonFileName + "_" + type + "_" + subUrls.Count + "_" + currentDateString + ".pdf";
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
                                                        }
                                                        appendicesUrls.AddRange(subUrls);
                                                    }
                                                }

                                            if (ValidateResponse.Result == "AA")
                                            {
                                                setElement.Save(XMLFilePath + "_" + pdfs.Count + fileExtension);
                                            }
                                            else
                                            {
                                                string exceptionMessage = ValidateResponse.Result.Replace(":", "-");
                                                exceptionMessage = exceptionMessage.Trim('.');
                                                setElement.Save(XMLFilePathException + "_" + exceptionMessage + "_" + pdfs.Count + fileExtension);
                                                notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                                                notifyIcon.BalloonTipTitle = "Exception files found";
                                                notifyIcon.BalloonTipText = "Please watch folder" + FromADCException;
                                                notifyIcon.ShowBalloonTip(3000);
                                            }

                                        }

                                        UpdateBatchCount(batchKey, batchCount);

                                        if (File.Exists(FromADCArchive + fileNameWithExtension))
                                            File.Delete(FromADCArchive + fileNameWithExtension);
                                        File.Move(file, FromADCArchive + fileNameWithExtension);
                                    }
                                    else
                                    {
                                        if (File.Exists(FromADCException + fileNameWithExtension))
                                            File.Delete(FromADCException + fileNameWithExtension);
                                        File.Move(file, FromADCException + fileNameWithExtension);
                                    }
                                    foreach (string url in appendicesUrls)
                                    {
                                        string urlFileNameWithExtension = Path.GetFileName(url);
                                        //if (File.Exists(FromADCArchive + urlFileNameWithExtension))
                                        //    File.Delete(FromADCArchive + urlFileNameWithExtension);
                                        //File.Move(url, FromADCArchive + urlFileNameWithExtension);
                                    }
                                    //ProcessBatchFiles(batchKey);
                                }
                            }
                            index++;
                        }
                    }
                }
                //string[] files = Directory.GetFiles(FromADC, "*.xml");
                //int index = 1;
                //if (files.ToList().Count == 0)
                //    isProcessingFromADC = false;
                //foreach (string file in files)
                //{
                //    FileInfo fileInfo = new FileInfo(file);
                //    DateTime lastWriteTime = fileInfo.LastWriteTime;
                //    DateTime currentTime = DateTime.Now;
                //    currentTime = currentTime.AddSeconds(-4);
                //    if (lastWriteTime <= currentTime)
                //    {

                //    }
                //}
            }
            return "";
        }

        public bool isBatchCountUpdated = false;

        public void UpdateBatchCount(string batchKey, int batchCount)
        {
            isBatchCountUpdated = false;
            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey)) + "='" + batchKey + "'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + "='4'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + "='1'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + "='1'");
            List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
            while (batches.Count == 0)
            {
                batches = ApiCalls.GetBatches(filter.ToString());
            }
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
                //el.AppendChild(doc.CreateElement("CreatedBy")).InnerText = user.Id.ToString();
                //el.AppendChild(doc.CreateElement("UpdatedBy")).InnerText = user.Id.ToString();
                string statusFileName = ADCStatusArchive + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + key + "_" + department.Code + "_" + branch.Code + ".xml";
                doc.Save(statusFileName);

                string response = TransmissionApiCalls.PostBatchStatusXMLFile(statusFileName);
                while (response == null)
                {
                    response = TransmissionApiCalls.PostBatchStatusXMLFile(statusFileName);
                }
                //if (response != null)
                //{
                //    string[] files = Directory.GetFiles(SplitFromADC, batchKey + "*");

                //    foreach (string file in files)
                //    {
                //        string fileName = Path.GetFileName(file);
                //        string destFile = Path.Combine(SplitFromADC, fileName);
                //        File.Move(file, destFile);
                //    }
                //}
            }

            isBatchCountUpdated = true;
        }

        public object ProcessSplitFromADCFiles(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                if (
                    (!isProcessingADCStatus && !isProcessingFromADC) ||
                    (Directory.GetFiles(FromADC).ToList().Count == 0 && Directory.GetFiles(ADCStatus).ToList().Count == 0))
                {
                    LogWriter logWriter = new LogWriter("In SplitFromADC");
                    List<Task> TaskList = new List<Task>();
                    DirectoryInfo info = new DirectoryInfo(SplitFromADC);
                    FileInfo[] filesByTime = info.GetFiles("*.xml").OrderBy(p => p.CreationTime).ToArray();
                    if (filesByTime.ToList().Count == 0)
                        isProcessingSplitFromADC = false;
                    foreach (FileInfo fileInfo in filesByTime)
                    {
                        DateTime lastWriteTime = fileInfo.LastWriteTime;
                        DateTime currentTime = DateTime.Now;
                        currentTime = currentTime.AddSeconds(-3);
                        if (lastWriteTime <= currentTime)
                        {
                            string file = fileInfo.FullName;
                            string fileName = Path.GetFileName(file);
                            try
                            {
                                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                                string[] splitFileName = fileNameWithoutExtension.Split('_');

                                string fileCount = splitFileName.Last();

                                string commonFileName = String.Join("_", splitFileName.ToList<string>().Take(4).ToArray());
                                logWriter = new LogWriter("commonFileName: " + commonFileName);
                                string[] matchedFiles = Directory.GetFiles(SplitFromADC, commonFileName + "*");
                                logWriter = new LogWriter("matchedFiles: " + matchedFiles.ToList().Count);
                                int tryCount = 0;
                                while (matchedFiles.ToList().Count - 1 != Convert.ToInt32(fileCount) && tryCount <= 10)
                                {
                                    matchedFiles = Directory.GetFiles(SplitFromADC, commonFileName + "*");
                                    tryCount++;
                                }
                                if (tryCount >= 10)
                                {
                                    File.Move(file, SplitFromADCException + fileName);
                                }
                                if (matchedFiles.ToList().Count - 1 == Convert.ToInt32(fileCount))
                                {
                                    FileInfo fileInfo1 = new FileInfo(file);
                                    DateTime lastWriteTime1 = fileInfo.LastWriteTime;
                                    logWriter = new LogWriter("lastWriteTime: " + lastWriteTime1);
                                    DateTime currentTime1 = DateTime.Now;
                                    currentTime1.AddSeconds(-3);
                                    if (lastWriteTime1 <= currentTime1)
                                        if (!IsFileLocked(fileInfo1))
                                        {
                                            Task<string> task = TransmissionApiCalls.PostAllFiles(file);
                                            TaskList.Add(task);
                                        }
                                }
                            }
                            catch (Exception ex)
                            {
                                File.Move(file, SplitFromADCException + fileName);
                                MessageBox.Show(ex.Message);
                                logWriter = new LogWriter("File exception");
                                logWriter = new LogWriter(ex.Message);
                                logWriter = new LogWriter("---------------------------------------------------------------------------------------");
                                logWriter = new LogWriter(ex.StackTrace);
                            }
                        }
                    }
                    //string[] files = Directory.GetFiles(SplitFromADC, "*.xml");
                    //logWriter = new LogWriter("files: " + files.ToList().Count);
                    //foreach (string file in files)
                    //{
                        
                    //}
                    if (TaskList.Count > 0)
                    {
                        //Wait for batch
                        Task.WaitAll(TaskList.ToArray());
                        string batchKey = "";
                        foreach (Task<string> task in TaskList)
                        {
                            if (task.Result != null)
                                if (batchKey != task.Result)
                                {
                                    batchKey = task.Result;
                                    StringBuilder filter = new StringBuilder();
                                    filter.Append(" 1=1");
                                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey)) + " = '" + batchKey + "'");
                                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " = '4'");
                                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '1'");
                                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");

                                    logWriter = new LogWriter("ProcessSplitFromADCFiles WaitAll: " + filter.ToString());
                                    List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                                    if (batches.Count > 0)
                                    {
                                        Batch batch = batches.FirstOrDefault();
                                        if (batch != null)
                                        {
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
                                            string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
                                            el.AppendChild(doc.CreateElement("BranchId")).InnerText = branch.Code;
                                            el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = department.Code;
                                            el.AppendChild(doc.CreateElement("StageId")).InnerText = "4";
                                            el.AppendChild(doc.CreateElement("BatchKey")).InnerText = batch.BatchKey;
                                            el.AppendChild(doc.CreateElement("UserId")).InnerText = "system";
                                            el.AppendChild(doc.CreateElement("BatchNo")).InnerText = batch.BatchNo;
                                            el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batch.BatchCount.ToString();
                                            el.AppendChild(doc.CreateElement("Status")).InnerText = "0";
                                            el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = batch.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                                            el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = currentDateString;
                                            //el.AppendChild(doc.CreateElement("CreatedBy")).InnerText = user.Id.ToString();
                                            //el.AppendChild(doc.CreateElement("UpdatedBy")).InnerText = user.Id.ToString();
                                            string statusFileName = ADCStatusArchive + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + batch.BatchKey + "_" + department.Code + "_" + branch.Code + ".xml";
                                            doc.Save(statusFileName);
                                            string response = TransmissionApiCalls.PostBatchStatusXMLFile(statusFileName);
                                        }
                                    }
                                }
                        }
                        TaskList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogWriter logWriter = new LogWriter(ex.Message);
                logWriter = new LogWriter("---------------------------------------------------------------------------------------");
                logWriter = new LogWriter(ex.StackTrace);
            }
            return "";
        }

        private void FolderWatcher_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
            this.Visible = true;
        }

        private void backgroundWorkerTimer_Tick(object sender, EventArgs e)
        {
            if (!IsClosePending)
            {
                if (backgroundWorkerProcessADCStatusFiles == null)
                {
                    InitializeBackgroundWorkerADCStatusFiles();
                }
                else if (!backgroundWorkerProcessADCStatusFiles.IsBusy)
                {
                    InitializeBackgroundWorkerADCStatusFiles();
                }
                if (backgroundWorkerProcessFromADCFiles == null)
                {
                    InitializeBackgroundWorkerFromADCFiles();
                }
                else if (!backgroundWorkerProcessFromADCFiles.IsBusy)
                {
                    InitializeBackgroundWorkerFromADCFiles();
                }
                if (backgroundWorkerProcessSplitFromADCFiles == null)
                {
                    InitializeBackgroundWorkerSplitFromADCFiles();
                }
                else if (!backgroundWorkerProcessSplitFromADCFiles.IsBusy)
                {
                    InitializeBackgroundWorkerSplitFromADCFiles();
                }
            }
        }

        //public object ProcessBatchFiles(string batchKey)
        //{
        //    List<Task> TaskList = new List<Task>();
        //    string[] files = Directory.GetFiles(SplitFromADC, batchKey + "*");
        //    foreach (string file in files)
        //    {
        //        FileInfo fileInfo = new FileInfo(file);
        //        if (!IsFileLocked(fileInfo))
        //        {
        //            string fileExtension = Path.GetExtension(file);
        //            string fileNameWithExtension = Path.GetFileName(file);
        //            //string fileName = Path.GetFileNameWithoutExtension(file);
        //            try
        //            {
        //                if (fileExtension.ToLower() == ".xml")
        //                {
        //                    string fileName = Path.GetFileNameWithoutExtension(file);
        //                    string[] splitFileName = fileName.Split('_');
        //                    string commonFileName = String.Join("_", splitFileName.ToList<string>().Take(3).ToArray());
        //                    string[] matchedFiles = Directory.GetFiles(SplitFromADC, commonFileName + "*");
        //                    foreach (string matchFile in matchedFiles)
        //                    {
        //                        fileNameWithExtension = Path.GetFileName(matchFile);
        //                        Task<string> task = TransmissionApiCalls.PostFile(matchFile);
        //                        TaskList.Add(task);
        //                        if (File.Exists(SplitFromADCArchive + fileNameWithExtension))
        //                            File.Delete(SplitFromADCArchive + fileNameWithExtension);
        //                        File.Move(matchFile, SplitFromADCArchive + fileNameWithExtension);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                if (File.Exists(SplitFromADCException + fileNameWithExtension))
        //                    File.Delete(SplitFromADCException + fileNameWithExtension);
        //                try
        //                {
        //                    File.Move(file, SplitFromADCException + fileNameWithExtension);
        //                }
        //                catch (Exception exSub)
        //                {

        //                }
        //            }
        //        }
        //    }

        //    Task.WaitAll(TaskList.ToArray());
        //    string resultBatchKey = "";
        //    foreach (Task<string> task in TaskList)
        //    {
        //        if (resultBatchKey != task.Result)
        //        {
        //            resultBatchKey = task.Result;
        //            StringBuilder filter = new StringBuilder();
        //            filter.Append(" 1=1");
        //            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey)) + " = '" + batchKey + "'");
        //            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
        //            List<Batch> batches = ApiCalls.GetBatches(filter.ToString());

        //            if (batches.Count > 0)
        //            {
        //                Batch batch = batches.FirstOrDefault();
        //                if (batch != null)
        //                {
        //                    if (batch.StageId == 4)
        //                    {
        //                        List<Branch> branches = ApiCalls.GetBranches();
        //                        List<Department> departments = ApiCalls.GetDepartments();

        //                        Branch branch = branches.Where(x => x.Id == batch.BranchId).FirstOrDefault();
        //                        Department department = departments.Where(x => x.Id == batch.DepartmentId).FirstOrDefault();

        //                        XmlDocument doc = new XmlDocument();
        //                        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //                        doc.AppendChild(docNode);
        //                        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
        //                        DateTime currentDate = DateTime.Now;
        //                        string key = currentDate.ToString("yyyyMMddHHmmssfff");
        //                        string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
        //                        el.AppendChild(doc.CreateElement("BranchId")).InnerText = branch.Code;
        //                        el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = department.Code;
        //                        el.AppendChild(doc.CreateElement("StageId")).InnerText = "4";
        //                        el.AppendChild(doc.CreateElement("BatchKey")).InnerText = batch.BatchKey;
        //                        el.AppendChild(doc.CreateElement("UserId")).InnerText = "system";
        //                        el.AppendChild(doc.CreateElement("BatchNo")).InnerText = batch.BatchNo;
        //                        el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batch.BatchCount.ToString();
        //                        el.AppendChild(doc.CreateElement("Status")).InnerText = "0";
        //                        el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = batch.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
        //                        el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = currentDateString;
        //                        //el.AppendChild(doc.CreateElement("CreatedBy")).InnerText = user.Id.ToString();
        //                        //el.AppendChild(doc.CreateElement("UpdatedBy")).InnerText = user.Id.ToString();
        //                        doc.Save(ADCStatus + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + key + "_" + department.Code + "_" + branch.Code + ".xml");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return "";
        //}
    }
}
