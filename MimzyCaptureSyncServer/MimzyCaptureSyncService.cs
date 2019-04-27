using MimzyCaptureSyncServer.Custom;
using MimzyCaptureSyncServer.ServiceCalls;
using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MimzyCaptureSyncServer
{
    public partial class MimzyCaptureSyncService : ServiceBase
    {

        public string ServerNo = ConfigurationManager.AppSettings["ServerNo"].ToString();

        public string MimzyCaptureOuputELIBRARY = ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARY"].ToString();
        public string MimzyCaptureOuputETP = ConfigurationManager.AppSettings["MimzyCaptureOuputETP"].ToString();
        public string MimzyCaptureOuputLOS = ConfigurationManager.AppSettings["MimzyCaptureOuputLOS"].ToString();

        public string MimzyCaptureOuputELIBRARYEx = ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYEx"].ToString();
        public string MimzyCaptureOuputETPEx = ConfigurationManager.AppSettings["MimzyCaptureOuputETPEx"].ToString();
        public string MimzyCaptureOuputLOSEx = ConfigurationManager.AppSettings["MimzyCaptureOuputLOSEx"].ToString();

        public string DocumentsPath = ConfigurationManager.AppSettings["Documents"].ToString();
        public string DocumentsArchivePath = ConfigurationManager.AppSettings["DocumentsArchive"].ToString();
        public string DocumentsExceptionPath = ConfigurationManager.AppSettings["DocumentsException"].ToString();

        public string FilesPath = ConfigurationManager.AppSettings["Files"].ToString();
        public string FilesArchivePath = ConfigurationManager.AppSettings["FilesArchive"].ToString();

        public string NetworkPath = ConfigurationManager.AppSettings["NetworkPath"].ToString();

        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());

        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchLogService> lazyBatchLogServiceObj = new Lazy<IBatchLogService>(() => new BatchLogService());

        public static IBatchLogService IBatchLogService { get { return lazyBatchLogServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        public bool isProcessingGenerate;
        public bool isProcessingSync;

        private BackgroundWorker backgroundWorkerProcessSyncFilesToDB;
        private BackgroundWorker backgroundWorkerProcessGenerateDocumentXML;

        public MimzyCaptureSyncService()
        {
            isProcessingGenerate = false;
            isProcessingSync = false;
            LogWriter logWriter = new LogWriter("Sync Service Initialized");
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogWriter logWriter = new LogWriter("Sync Service Started");
            this.timer = new System.Timers.Timer(3600000D);  // 3600000 milliseconds = 1 hour
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.CheckAndVoidBatches);
            this.timer.Start();

            this.mFiletimer = new System.Timers.Timer(90000);  // 150000 milliseconds = 2.5 min
            this.mFiletimer.AutoReset = true;
            this.mFiletimer.Elapsed += new System.Timers.ElapsedEventHandler(this.MFileXmlToSplit);
            this.mFiletimer.Start();

            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerProcessSyncFilesToDB = new BackgroundWorker();
            backgroundWorkerProcessSyncFilesToDB.DoWork += new DoWorkEventHandler(backgroundWorkerProcessSyncFilesToDB_DoWork);
            backgroundWorkerProcessSyncFilesToDB.WorkerReportsProgress = false;
            backgroundWorkerProcessSyncFilesToDB.WorkerSupportsCancellation = true;
            backgroundWorkerProcessSyncFilesToDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProcessSyncFilesToDB_RunWorkerCompleted);
            backgroundWorkerProcessSyncFilesToDB.RunWorkerAsync();

            backgroundWorkerProcessGenerateDocumentXML = new BackgroundWorker();
            backgroundWorkerProcessGenerateDocumentXML.DoWork += new DoWorkEventHandler(backgroundWorkerProcessGenerateDocumentXML_DoWork);
            backgroundWorkerProcessGenerateDocumentXML.WorkerReportsProgress = false;
            backgroundWorkerProcessGenerateDocumentXML.WorkerSupportsCancellation = true;
            backgroundWorkerProcessGenerateDocumentXML.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProcessGenerateDocumentXML_RunWorkerCompleted);
            //backgroundWorkerProcessGenerateDocumentXML.RunWorkerAsync();
        }

        private void backgroundWorkerProcessSyncFilesToDB_DoWork(object sender, DoWorkEventArgs e)
        {
            isProcessingSync = true;
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = SyncFilesToDB(worker, e);
        }
        private void backgroundWorkerProcessSyncFilesToDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
            }
            else
            {
                isProcessingSync = false;
                if (!backgroundWorkerProcessGenerateDocumentXML.IsBusy)
                    backgroundWorkerProcessGenerateDocumentXML.RunWorkerAsync();
            }
        }

        private void backgroundWorkerProcessGenerateDocumentXML_DoWork(object sender, DoWorkEventArgs e)
        {
            isProcessingGenerate = true;
            BackgroundWorker worker = sender as BackgroundWorker;
            e.Result = GenerateDocumentsXML(worker, e);
        }
        private void backgroundWorkerProcessGenerateDocumentXML_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
            }
            else
            {
                isProcessingGenerate = false;
                if (!backgroundWorkerProcessSyncFilesToDB.IsBusy)
                    backgroundWorkerProcessSyncFilesToDB.RunWorkerAsync();
            }
        }

        public object GenerateDocumentsXML(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " = '5'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '1'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
                List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, false);
                if (batches.Count > 0)
                {
                    bool checkConnection = CustomValidator.CheckConnection();
                    if (checkConnection)
                    {
                        foreach (Batch batch in batches)
                        {
                            if (batch.IsBatchCountUpdated == Convert.ToInt32(ServerNo))//Service1: IsBatchCountUpdated = 1, Service2: IsBatchCountUpdated = 2
                            {
                                filter = new StringBuilder();
                                filter.Append(" 1=1");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<BatchLog>(nameof(BatchLog.BatchId)) + " = '" + batch.Id + "'");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<BatchLog>(nameof(BatchLog.Status)) + " = '1'");

                                List<BatchLog> batchLogs = IBatchLogService.GetDataByFilter(filter.ToString(), 0, 0, false);

                                BatchLog scanLog = batchLogs.Where(x => x.StageId == 1).FirstOrDefault();
                                BatchLog verificationLog = batchLogs.Where(x => x.StageId == 2).FirstOrDefault();

                                Branch branch = IBranchService.GetSingle(batch.BranchId);
                                Department department = IDepartmentService.GetSingle(batch.DepartmentId);
                                string departmentCode = department.Code;
                                string deptCode = departmentCode.Split('-')[0];
                                string jobCode = departmentCode.Split('-')[1];

                                filter = new StringBuilder();
                                filter.Append(" 1=1");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Set>(nameof(Set.BatchId)) + " = '" + batch.Id + "'");
                                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Set>(nameof(Set.Status)) + " = '1'");

                                List<Set> sets = ISetService.GetDataByFilter(filter.ToString(), 0, 0, false);
                                List<Set> updatedSets = new List<Set>();
                                foreach (Set set in sets)
                                {
                                    bool ValidateAA = false;
                                    bool ValidateAC = false;
                                    bool ValidatePJ = false;
                                    bool ValidateWF = false;
                                    string className = "";
                                    if (departmentCode == "E-LIBRARY")
                                    {
                                        //AA NUMBER
                                        ValidateAA = true;
                                        //ACCOUNT NUMBER
                                        ValidateAC = true;
                                        className = "e-Library";
                                    }
                                    else if (deptCode == "ETP")
                                    {
                                        if (jobCode == "LN")
                                        {
                                            //AA NUMBER
                                            ValidateAA = true;
                                            //ACCOUNT NUMBER
                                            ValidateAC = true;
                                            className = "Legal";
                                        }
                                        else if (jobCode == "LL")
                                        {
                                            //AA NUMBER
                                            ValidateAA = true;
                                            className = "eTPLoan";
                                        }
                                        else if (jobCode == "PR")
                                        {
                                            //PROJECT CODE
                                            ValidatePJ = true;
                                            className = "Project";
                                        }
                                        else if (jobCode == "WF")
                                        {
                                            //WELFARE CODE
                                            ValidateWF = true;
                                            className = "Welfare";
                                        }
                                    }
                                    else if (deptCode == "LOS")
                                    {
                                        //AA NUMBER
                                        ValidateAA = true;
                                        className = "LOS";
                                    }
                                    //Validate AA
                                    bool isValidAA = false;
                                    string AARejectReason = "";
                                    if (ValidateAA)
                                    {
                                        AAValidateResponse ValidateResponseAA = CustomValidator.isValid("AA", set.AaNo);
                                        if (ValidateResponseAA.Result == "AA")
                                        {
                                            isValidAA = true;
                                        }
                                        else
                                        {
                                            isValidAA = false;
                                            AARejectReason = ValidateResponseAA.Result;
                                        }
                                    }
                                    //Validate AC
                                    bool isValidAC = false;
                                    string ACRejectReason = "";
                                    if (ValidateAC)
                                    {
                                        AAValidateResponse ValidateResponseAC = CustomValidator.isValidACandAA("AC", set.AccountNo, set.AaNo);
                                        if (ValidateResponseAC.Result == "AA")
                                        {
                                            isValidAC = true;
                                        }
                                        else
                                        {
                                            isValidAC = false;
                                            ACRejectReason = ValidateResponseAC.Result;
                                        }
                                    }
                                    //Validate PR
                                    bool isValidPJ = false;
                                    string PJRejectReason = "";
                                    if (ValidatePJ)
                                    {
                                        AAValidateResponse ValidateResponsePJ = CustomValidator.isValid("PJ", set.AaNo);
                                        if (ValidateResponsePJ.Result == "AA")
                                        {
                                            isValidPJ = true;
                                        }
                                        else
                                        {
                                            isValidPJ = false;
                                            PJRejectReason = ValidateResponsePJ.Result;
                                        }
                                    }
                                    //Validate WF
                                    bool isValidWF = false;
                                    string WFRejectReason = "";
                                    if (ValidateWF)
                                    {
                                        AAValidateResponse ValidateResponseWF = CustomValidator.isValid("WF", set.AaNo);
                                        if (ValidateResponseWF.Result == "AA")
                                        {
                                            isValidWF = true;
                                        }
                                        else
                                        {
                                            isValidWF = false;
                                            WFRejectReason = ValidateResponseWF.Result;
                                        }
                                    }
                                    bool isValid = false;
                                    if (ValidateAA)
                                    {
                                        if (isValidAA)
                                            isValid = true;
                                        else
                                        {
                                            isValid = false;
                                            set.Remarks = AARejectReason;
                                        }
                                    }
                                    if (ValidateAA && ValidateAC)
                                    {
                                        if (isValidAA && isValidAC)
                                            isValid = true;
                                        else
                                        {
                                            isValid = false;
                                            set.Remarks = AARejectReason + "," + ACRejectReason;
                                        }
                                    }
                                    if (ValidatePJ)
                                    {
                                        if (isValidPJ)
                                            isValid = true;
                                        else
                                        {
                                            isValid = false;
                                            set.Remarks = PJRejectReason;
                                        }
                                    }
                                    if (ValidateWF)
                                    {
                                        if (isValidWF)
                                            isValid = true;
                                        else
                                        {
                                            isValid = false;
                                            set.Remarks = WFRejectReason;
                                        }
                                    }

                                    if (isValid)
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                                        doc.AppendChild(docNode);
                                        XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("Documents"));
                                        filter = new StringBuilder();
                                        filter.Append(" 1=1");
                                        filter.Append(" and " + Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.SetId)) + " = '" + set.Id + "'");
                                        filter.Append(" and " + Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.Status)) + " = '1'");

                                        List<SetDocument> setDocuments = ISetDocumentService.GetDataByFilter(filter.ToString(), 0, 0, false);
                                        if (setDocuments.Count > 0)
                                        {
                                            foreach (SetDocument setDocument in setDocuments)
                                            {
                                                XmlElement docElement = (XmlElement)root.AppendChild(doc.CreateElement("Document"));
                                                string docURL = setDocument.DocumentUrl;
                                                string docName = Path.GetFileName(docURL);
                                                string docArchiveURL = "";
                                                //File.Copy(docURL, docArchiveURL);
                                                if (departmentCode == "E-LIBRARY")
                                                {
                                                    docArchiveURL = MimzyCaptureOuputELIBRARY + docName;
                                                    if (File.Exists(docArchiveURL))
                                                        File.Delete(docArchiveURL);
                                                    File.Copy(docURL, docArchiveURL);
                                                }
                                                else
                                                {
                                                    if (deptCode == "ETP")
                                                    {
                                                        docArchiveURL = MimzyCaptureOuputETP + docName;
                                                        if (File.Exists(docArchiveURL))
                                                            File.Delete(docArchiveURL);
                                                        File.Copy(docURL, docArchiveURL);
                                                    }
                                                    else if (deptCode == "LOS")
                                                    {
                                                        docArchiveURL = MimzyCaptureOuputLOS + docName;
                                                        if (File.Exists(docArchiveURL))
                                                            File.Delete(docArchiveURL);
                                                        File.Copy(docURL, docArchiveURL);
                                                    }
                                                }
                                                docElement.AppendChild(doc.CreateElement("SourceFile")).InnerText = docArchiveURL;
                                                docElement.AppendChild(doc.CreateElement("ScanDate")).InnerText = scanLog == null ? "" : scanLog.UpdatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                docElement.AppendChild(doc.CreateElement("IndexDate")).InnerText = verificationLog == null ? "" : verificationLog.UpdatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                docElement.AppendChild(doc.CreateElement("QCDate")).InnerText = "";
                                                docElement.AppendChild(doc.CreateElement("StationID")).InnerText = branch.Code;
                                                docElement.AppendChild(doc.CreateElement("DocumentSetID")).InnerText = set.Id.ToString();//NEW
                                                docElement.AppendChild(doc.CreateElement("DocumentID")).InnerText = setDocument.Id.ToString();//NEW
                                                docElement.AppendChild(doc.CreateElement("PageNumber")).InnerText = setDocument.PageCount.ToString();
                                                docElement.AppendChild(doc.CreateElement("Verified")).InnerText = "N";//NEW
                                                docElement.AppendChild(doc.CreateElement("ScanUser")).InnerText = scanLog == null ? "" : scanLog.BatchUser;
                                                docElement.AppendChild(doc.CreateElement("IndexUser")).InnerText = verificationLog == null ? "" : verificationLog.BatchUser;
                                                docElement.AppendChild(doc.CreateElement("QCUser")).InnerText = "";
                                                docElement.AppendChild(doc.CreateElement("BatchID")).InnerText = batch.BatchKey;//NEW
                                                docElement.AppendChild(doc.CreateElement("DocumentCode")).InnerText = setDocument.DocType;
                                                docElement.AppendChild(doc.CreateElement("DocumentTypeName")).InnerText = "";//NEW

                                                if (departmentCode == "E-LIBRARY")
                                                {
                                                    docElement.AppendChild(doc.CreateElement("AANumber")).InnerText = set.AaNo;
                                                   // docElement.AppendChild(doc.CreateElement("AccountNumber")).InnerText = set.AccountNo;//NEW
                                                    docElement.AppendChild(doc.CreateElement("AccountNumber")).InnerText = set.AccountNo.PadLeft(12, '0');//NEW
                                                    if (jobCode == "LIBRARY")
                                                    {
                                                        docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;//NEW
                                                    }
                                                }
                                                else if (deptCode == "ETP")
                                                {
                                                    if (jobCode == "LN")
                                                    {
                                                        docElement.AppendChild(doc.CreateElement("AANumber")).InnerText = set.AaNo;
                                                       // docElement.AppendChild(doc.CreateElement("AccountNumber")).InnerText = set.AccountNo;//NEW
                                                        docElement.AppendChild(doc.CreateElement("AccountNumber")).InnerText = set.AccountNo.PadLeft(12, '0');//NEW
                                                        docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;//NEW
                                                    }
                                                    else if (jobCode == "LL")
                                                    {
                                                        docElement.AppendChild(doc.CreateElement("AANumber")).InnerText = set.AaNo;
                                                        docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;//NEW
                                                    }
                                                    else if (jobCode == "PR")
                                                    {
                                                        docElement.AppendChild(doc.CreateElement("ProjectCode")).InnerText = set.AaNo;//NEW
                                                        docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;//NEW
                                                    }
                                                    else if (jobCode == "WF")
                                                    {
                                                        docElement.AppendChild(doc.CreateElement("WelfareCode")).InnerText = set.AaNo;//NEW
                                                        docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;//NEW
                                                    }
                                                }
                                                else if (deptCode == "LOS")
                                                {
                                                    docElement.AppendChild(doc.CreateElement("AANumber")).InnerText = set.AaNo;
                                                    docElement.AppendChild(doc.CreateElement("Class")).InnerText = className;
                                                }
                                            }
                                            //doc.Save(DocumentsPath + set.AccountNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml");
                                            string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                            currentDateString = set.Id.ToString();

                                            if (File.Exists(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml"))
                                                File.Delete(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml");
                                            doc.Save(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml");
                                            //if (File.Exists(DocumentsPath + set.AaNo + "_" + currentDateString + ".xml"))
                                            //    File.Delete(DocumentsPath + set.AaNo + "_" + currentDateString + ".xml");
                                            //File.Copy(DocumentsArchivePath + set.AccountNo + "_" + currentDateString + ".xml", DocumentsPath + set.AccountNo + "_" + currentDateString + ".xml");


                                            if (departmentCode == "E-LIBRARY")
                                            {
                                                if (File.Exists(MimzyCaptureOuputELIBRARY + set.AaNo + "_" + currentDateString + ".xml"))
                                                    File.Delete(MimzyCaptureOuputELIBRARY + set.AaNo + "_" + currentDateString + ".xml");
                                                File.Copy(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml", MimzyCaptureOuputELIBRARY + set.AaNo + "_" + currentDateString + ".xml");
                                            }
                                            else
                                            {
                                                if (deptCode == "ETP")
                                                {
                                                    if (File.Exists(MimzyCaptureOuputETP + set.AaNo + "_" + currentDateString + ".xml"))
                                                        File.Delete(MimzyCaptureOuputETP + set.AaNo + "_" + currentDateString + ".xml");
                                                    File.Copy(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml", MimzyCaptureOuputETP + set.AaNo + "_" + currentDateString + ".xml");
                                                }
                                                else if (deptCode == "LOS")
                                                {
                                                    if (File.Exists(MimzyCaptureOuputLOS + set.AaNo + "_" + currentDateString + ".xml"))
                                                        File.Delete(MimzyCaptureOuputLOS + set.AaNo + "_" + currentDateString + ".xml");
                                                    File.Copy(DocumentsArchivePath + set.AaNo + "_" + currentDateString + ".xml", MimzyCaptureOuputLOS + set.AaNo + "_" + currentDateString + ".xml");
                                                }
                                            }
                                            set.IsReleased = 1;
                                            set.Remarks = "";
                                        }
                                        else
                                        {
                                            set.IsReleased = 0;
                                            set.Remarks = "No Documents";
                                        }
                                    }
                                    else
                                    {
                                        set.IsReleased = 0;
                                    }
                                    set.UpdatedBy = 0;
                                    set.UpdatedDate = DateTime.Now;
                                    set.SetXmlPath = set.SetXmlPath.Replace("\\", "\\\\");
                                    updatedSets.Add(set);
                                    ISetService.UpdateData(set);
                                }
                                if (sets.Count > 0)
                                {
                                    Batch batchUpdate = batch;
                                    if (updatedSets.Where(x => x.IsReleased == 0).Count() == 0)
                                    {
                                        batchUpdate.StageId = 6;
                                    }
                                    else
                                    {
                                        batchUpdate.StageId = 5;
                                        batchUpdate.Status = 9;
                                    }
                                    batchUpdate.UpdatedBy = 0;
                                    batchUpdate.UpdatedDate = DateTime.Now;
                                    IBatchService.UpdateData(batchUpdate);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("GenerateDocumentsXML " + ex.Message + " - Exception");
            }
            return "";
        }

        public object SyncFilesToDB(BackgroundWorker worker, DoWorkEventArgs e)
        {
            //CheckAndVoidBatches();
            CheckFilesBeforeSync();
            CheckBatchesAndServer();
            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " = '4'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '0'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
            List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, false);
            try
            {
                foreach (Batch batch in batches)
                {
                    DateTime lastUpdatedTime = batch.UpdatedDate.Value;
                    DateTime currentDateTime2 = DateTime.Now;
                    currentDateTime2 = currentDateTime2.AddSeconds(-1);
                    if (lastUpdatedTime < currentDateTime2)
                    {
                        Batch updatedBatch = IBatchService.GetSingle(batch.Id);
                        if (updatedBatch.IsBatchCountUpdated == 0)
                        {
                            updatedBatch.IsBatchCountUpdated = Convert.ToInt32(ServerNo);//Service1: IsBatchCountUpdated = 1, Service2: IsBatchCountUpdated = 2
                            updatedBatch.UpdatedBy = 0;
                            updatedBatch.UpdatedDate = DateTime.Now;
                            IBatchService.UpdateData(updatedBatch);
                        }
                        if (batch.IsBatchCountUpdated == Convert.ToInt32(ServerNo))//Service1: IsBatchCountUpdated = 1, Service2: IsBatchCountUpdated = 2
                        {
                            bool isSetXMLUpdated = true;
                            DirectoryInfo info = new DirectoryInfo(FilesPath);
                            FileInfo[] filesByTime = info.GetFiles(batch.BatchKey + "*.xml").OrderBy(x => x.CreationTime).ToArray();
                            if (filesByTime.Count() > 0)
                            {
                                string[] files = Directory.GetFiles(FilesPath, batch.BatchKey + "*.xml");
                                string CurrentDateFolder = DateTime.Now.ToString("yyyyMMdd");
                                string CurrentDateFolderPath = FilesPath + DateTime.Now.ToString("yyyyMMdd") + "\\\\";
                                if (!Directory.Exists(FilesPath + CurrentDateFolder))
                                {
                                    Directory.CreateDirectory(FilesPath + CurrentDateFolder);
                                }
                                foreach (FileInfo fileInfo in filesByTime)
                                {

                                    DateTime lastWriteTime = fileInfo.LastWriteTime;
                                    DateTime currentDateTime = DateTime.Now;
                                    currentDateTime = currentDateTime.AddSeconds(-2);
                                    if (lastWriteTime < currentDateTime)
                                    {
                                        string xmlfile = fileInfo.FullName;
                                        List<string> allSetFiles = new List<string>();
                                        if (!IsFileLocked(fileInfo))
                                        {
                                            string FileName = Path.GetFileName(xmlfile);
                                            string fileExtension = Path.GetExtension(xmlfile);
                                            string FileNameWithoutExtension = FileName.Split('.')[0];
                                            string[] keys = FileNameWithoutExtension.Split('_');
                                            //string batchKey = keys[0];
                                            string batchKey = keys[0];
                                            string batchPath = CurrentDateFolderPath + batchKey + "\\\\";
                                            if (!Directory.Exists(CurrentDateFolderPath + batchKey))
                                            {
                                                Directory.CreateDirectory(CurrentDateFolderPath + batchKey);
                                            }
                                            FileInfo[] documentsByTime = info.GetFiles(keys[0] + "_" + keys[1] + "_" + keys[2] + "_" + keys[3] + "_" + keys[4] + "_" + keys[5] + "*.pdf").OrderBy(x => x.CreationTime).ToArray();

                                            documentsByTime.ToList().ForEach(x =>
                                            {
                                                LogWriter logWriter0 = new LogWriter("documentsByTime: current: " + currentDateTime + " last: " + x.LastWriteTime.ToString());
                                            });

                                            var docList = documentsByTime.Where(x => x.LastWriteTime > currentDateTime).FirstOrDefault();
                                            if (docList == null)
                                            {
                                                LogWriter logWriter = new LogWriter(" in docList null: ");
                                                string[] documents = Directory.GetFiles(FilesPath, keys[0] + "_" + keys[1] + "_" + keys[2] + "_" + keys[3] + "_" + keys[4] + "_" + keys[5] + "*.pdf");
                                                //  string documentsCountString = keys[keys.Length - 1];
                                                string documentsCountString = "";
                                                documentsCountString = keys[keys.Length - 1];
                                                //if (documentsCountString.Length <= 3)
                                                //    documentsCountString = keys[keys.Length - 1];
                                                //else
                                                //    documentsCountString = keys[keys.Length - 2];
                                                if (!string.IsNullOrEmpty(documentsCountString))
                                                {
                                                    Int32 documentsCount = Convert.ToInt32(documentsCountString);
                                                    if (documentsCount == documents.Count())
                                                    {
                                                        allSetFiles.Add(xmlfile);
                                                        allSetFiles.AddRange(documents);
                                                    }
                                                }
                                            }
                                            string InsertedXML = "";
                                            foreach (string file in allSetFiles)
                                            {
                                                fileExtension = Path.GetExtension(file);
                                                FileName = Path.GetFileName(file);

                                                FileNameWithoutExtension = FileName.Split('.')[0];
                                                keys = FileNameWithoutExtension.Split('_');
                                                string batchFilePath = CurrentDateFolderPath + batchKey + "\\\\" + FileName;
                                                if (fileExtension == ".xml")
                                                {
                                                    InsertedXML = Path.GetFileName(batchFilePath);
                                                }
                                                //string batchNo = keys[1];
                                                string setKey = keys[1];
                                                string aaNo = keys[2];
                                                string deptCode = keys[3];
                                                string branchCode = keys[4];
                                                string accNo = keys[5];
                                                string docType = "";
                                                string pageCount = "";
                                                if (accNo == "NNN")
                                                    accNo = "";
                                                // Remove the zero's front of account number and accNo must be 12 char (New change 01-03-2019)
                                                if (!string.IsNullOrEmpty(accNo))
                                                {
                                                    accNo = accNo.TrimStart('0');
                                                    accNo = accNo.PadLeft(12, '0');
                                                }
                                                if (fileExtension == ".pdf")
                                                {
                                                    docType = keys[6];
                                                    pageCount = keys[7];
                                                }
                                                Branch branch = new Branch();
                                                Department department = new Department();
                                                List<Branch> branchMatches = IBranchService.GetDataByPropertyName(nameof(Branch.Code), branchCode, true, 0, 0, true);
                                                List<Department> departmentMatches = IDepartmentService.GetDataByPropertyName(nameof(Department.Code), deptCode, true, 0, 0, true);
                                                if (branchMatches.Count > 0 && departmentMatches.Count > 0)
                                                {
                                                    branch = branchMatches.FirstOrDefault();
                                                    department = departmentMatches.FirstOrDefault();
                                                    string deptId = department.Id.ToString();
                                                    string branchId = branch.Id.ToString();

                                                    filter = new StringBuilder();
                                                    filter.Append(" 1=1");

                                                    string batchKeyColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey));
                                                    //string batchNoColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchNo));
                                                    string branchIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BranchId));
                                                    string departmentIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.DepartmentId));
                                                    string statusColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status));
                                                    filter.Append(" and " + batchKeyColumnName + " = '" + batchKey + "'");
                                                    //filter.Append(" and " + batchNoColumnName + " = '" + batchNo + "'");
                                                    filter.Append(" and " + branchIdColumnName + " = '" + branchId + "'");
                                                    filter.Append(" and " + departmentIdColumnName + " = '" + deptId + "'");
                                                    filter.Append(" and " + statusColumnName + " = '1'");

                                                    List<Batch> batchMatches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);


                                                    if (batchMatches.Count > 0)
                                                    {
                                                        filter = new StringBuilder();
                                                        filter.Append(" 1=1");
                                                        string batchIdColumnName = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.BatchId));
                                                        string aaNoColumnName = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AaNo));
                                                        string setKeyColumnName = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.SetKey));
                                                        string setUrl = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.SetXmlPath));

                                                        filter.Append(" and " + batchIdColumnName + " = '" + batch.Id + "'");
                                                        filter.Append(" and " + aaNoColumnName + " = '" + aaNo + "'");
                                                        //filter.Append(" and " + setKeyColumnName + " = '" + setKey + "'");
                                                        filter.Append(" and " + setUrl + " like '%" + InsertedXML + "'");
                                                        filter.Append(" and status = '1'");

                                                        List<Set> setMatches = ISetService.GetDataByFilter(filter.ToString(), 0, 0, true);
                                                        if (setMatches.Count > 0)
                                                        {
                                                            Set set = setMatches.FirstOrDefault();
                                                            set.UpdatedDate = DateTime.Now;
                                                            set.UpdatedBy = 0;
                                                            if (fileExtension == ".xml")
                                                                set.SetXmlPath = batchFilePath;
                                                            if (fileExtension == ".pdf")
                                                            {
                                                                //set.SetXmlPath = set.SetXmlPath.Replace("\\", "\\\\");
                                                                filter = new StringBuilder();
                                                                filter.Append(" 1=1");
                                                                string docFileName = Path.GetFileName(batchFilePath);
                                                                string setIdColumnName = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.SetId));
                                                                string docTypeColumnName = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.DocType));
                                                                string documentUrl = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.DocumentUrl));
                                                                filter.Append(" and " + setIdColumnName + " = '" + set.Id + "'");
                                                                filter.Append(" and " + docTypeColumnName + " = '" + docType + "'");
                                                                filter.Append(" and " + documentUrl + " like '%" + docFileName + "'");
                                                                filter.Append(" and status = '1'");
                                                                List<SetDocument> setDocumentMatches = ISetDocumentService.GetDataByFilter(filter.ToString(), 0, 0, true);
                                                                if (setDocumentMatches.Count > 0)
                                                                {
                                                                    SetDocument setDocument = setDocumentMatches.FirstOrDefault();
                                                                    setDocument.DocumentUrl = batchFilePath;
                                                                    setDocument.PageCount = Convert.ToInt32(pageCount);
                                                                    setDocument.UpdatedBy = 0;
                                                                    setDocument.UpdatedDate = DateTime.Now;
                                                                    ISetDocumentService.UpdateData(setDocument);
                                                                }
                                                                else
                                                                {
                                                                    SetDocument setDocument = new SetDocument()
                                                                    {
                                                                        SetId = set.Id,
                                                                        DocType = docType,
                                                                        DocumentUrl = batchFilePath,
                                                                        PageCount = Convert.ToInt32(pageCount),
                                                                        CreatedBy = 0,
                                                                        CreatedDate = DateTime.Now,
                                                                        UpdatedBy = 0,
                                                                        UpdatedDate = DateTime.Now,
                                                                        Status = 1
                                                                    };
                                                                    setDocument = ISetDocumentService.PostData(setDocument);
                                                                    if (setDocument.Id == 0)
                                                                    {
                                                                        throw new Exception("Db not updated");
                                                                    }
                                                                }

                                                            }
                                                            if (set.SetXmlPath == "")
                                                                isSetXMLUpdated = false;
                                                            else
                                                                ISetService.UpdateData(set);
                                                        }
                                                        else
                                                        {
                                                            Set set = new Set()
                                                            {
                                                                BatchId = batch.Id,
                                                                SetKey = setKey,
                                                                AaNo = aaNo,
                                                                AccountNo = accNo,
                                                                SetStatus = 1,
                                                                CreatedBy = 0,
                                                                CreatedDate = DateTime.Now,
                                                                UpdatedBy = 0,
                                                                UpdatedDate = DateTime.Now,
                                                                Status = 1,
                                                            };
                                                            if (fileExtension == ".xml")
                                                                set.SetXmlPath = batchFilePath;
                                                            if (fileExtension == ".pdf")
                                                            {
                                                                filter = new StringBuilder();
                                                                filter.Append(" 1=1");
                                                                string docFileName = Path.GetFileName(batchFilePath);
                                                                string setIdColumnName = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.SetId));
                                                                string docTypeColumnName = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.DocType));
                                                                string documentUrl = Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.DocumentUrl));
                                                                filter.Append(" and " + setIdColumnName + " = '" + set.Id + "'");
                                                                filter.Append(" and " + docTypeColumnName + " = '" + docType + "'");
                                                                filter.Append(" and " + documentUrl + " like '%" + docFileName + "'");
                                                                filter.Append(" and status = '1'");
                                                                List<SetDocument> setDocumentMatches = ISetDocumentService.GetDataByFilter(filter.ToString(), 0, 0, true);
                                                                if (setDocumentMatches.Count > 0)
                                                                {
                                                                    SetDocument setDocument = setDocumentMatches.FirstOrDefault();
                                                                    setDocument.DocumentUrl = batchFilePath;
                                                                    setDocument.PageCount = Convert.ToInt32(pageCount);
                                                                    setDocument.UpdatedBy = 0;
                                                                    setDocument.UpdatedDate = DateTime.Now;
                                                                    ISetDocumentService.UpdateData(setDocument);
                                                                }
                                                                else
                                                                {
                                                                    SetDocument setDocument = new SetDocument()
                                                                    {
                                                                        SetId = set.Id,
                                                                        DocType = docType,
                                                                        DocumentUrl = batchFilePath,
                                                                        PageCount = Convert.ToInt32(pageCount),
                                                                        CreatedBy = 0,
                                                                        CreatedDate = DateTime.Now,
                                                                        UpdatedBy = 0,
                                                                        UpdatedDate = DateTime.Now,
                                                                        Status = 1
                                                                    };
                                                                    setDocument = ISetDocumentService.PostData(setDocument);
                                                                    if (setDocument.Id == 0)
                                                                    {
                                                                        throw new Exception("Db not updated");
                                                                    }
                                                                }
                                                            }
                                                            if (set.SetXmlPath == "")
                                                                isSetXMLUpdated = false;
                                                            else
                                                            {
                                                                set = ISetService.PostData(set);
                                                                if (set.Id == 0)
                                                                {
                                                                    throw new Exception("Db not updated");
                                                                }
                                                            }
                                                        }
                                                        if (File.Exists(batchFilePath))
                                                            File.Delete(batchFilePath);
                                                        File.Move(file, batchFilePath);

                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            ////Now Create all of the directories
                            //foreach (string dirPath in Directory.GetDirectories(NetworkPath, "*",
                            //    SearchOption.AllDirectories))
                            //    Directory.CreateDirectory(dirPath.Replace(NetworkPath, FilesPath));

                            ////Copy all the files & Replaces any files with the same name
                            //foreach (string newPath in Directory.GetFiles(NetworkPath, "*.*",
                            //    SearchOption.AllDirectories))
                            //    File.Copy(newPath, newPath.Replace(NetworkPath, FilesPath), true);

                            filter = new StringBuilder();
                            filter.Append(" 1=1");
                            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Set>(nameof(Set.BatchId)) + " = '" + batch.Id + "'");
                            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Set>(nameof(Set.Status)) + " = '1'");
                            List<Set> newSetMatches = ISetService.GetDataByFilter(filter.ToString(), 0, 0, true);

                            if (isSetXMLUpdated && newSetMatches.Count == batch.BatchCount)
                            {
                                batch.StageId = 5;
                                batch.BatchStatus = 1;
                                batch.UpdatedBy = 0;
                                batch.UpdatedDate = DateTime.Now;
                                IBatchService.UpdateData(batch);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    StackFrame stackFrame = new StackFrame(1,true);
                    //var st = new StackTrace(ex, true);

                    //// Get the top stack frame
                    //var frame = st.GetFrame(st.GetFrames().Count() - 1);
                    //// Get the line number from the stack frame
                    //var line = frame.GetFileLineNumber();

                    LogWriter logWriter = new LogWriter("SyncFilesToDB " + ex.Message + " - Exception");
                    logWriter = new LogWriter("Exception: " + ex.StackTrace);
                    logWriter = new LogWriter("Frame: " + stackFrame.GetFileName());
                    logWriter = new LogWriter("Line: " + stackFrame.GetFileLineNumber() + " and column number" + stackFrame.GetFileColumnNumber());
                }
                catch (Exception exinner)
                {
                    LogWriter logWriter = new LogWriter("SyncFilesToDB " + exinner.Message + " - Exception");
                }
            }

            return "";
        }

        public void CheckFilesBeforeSync()
        {
            try
            {
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " = '4'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '1'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
                List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, false);
                foreach (Batch batch in batches)
                {
                    DateTime lastUpdatedTime = batch.UpdatedDate.Value;
                    DateTime currentDateTime2 = DateTime.Now;
                    currentDateTime2 = currentDateTime2.AddSeconds(-2);
                    if (lastUpdatedTime < currentDateTime2)
                    {
                        int batchCount = batch.BatchCount.Value;
                        string[] files = Directory.GetFiles(FilesPath, batch.BatchKey + "*.xml");
                        if (batchCount == files.Count())
                        {
                            Batch batchNew = batch;
                            batchNew.BatchStatus = 0;
                            batchNew.UpdatedBy = 0;
                            try
                            {
                                //NEW
                                int setCount = 0;
                                foreach (string xmlfile in files)
                                {
                                    List<string> allSetFiles = new List<string>();
                                    FileInfo fileInfo = new FileInfo(xmlfile);
                                    if (!IsFileLocked(fileInfo))
                                    {
                                        string FileName = Path.GetFileName(xmlfile);
                                        string fileExtension = Path.GetExtension(xmlfile);
                                        string FileNameWithoutExtension = FileName.Split('.')[0];
                                        string[] keys = FileNameWithoutExtension.Split('_');
                                        //string batchKey = keys[0];
                                        string batchKey = keys[0];
                                        string[] documents = Directory.GetFiles(FilesPath, keys[0] + "_" + keys[1] + "_" + keys[2] + "_" + keys[3] + "_" + keys[4] + "_" + keys[5] + "*.pdf");
                                        string documentsCountString = keys[keys.Length - 1];
                                        if (!string.IsNullOrEmpty(documentsCountString))
                                        {
                                            Int32 documentsCount = Convert.ToInt32(documentsCountString);
                                            if (documentsCount == documents.Count())
                                            {
                                                allSetFiles.Add(xmlfile);
                                                allSetFiles.AddRange(documents);
                                                setCount++;
                                            }
                                        }
                                    }
                                }
                                //NEW

                                if (setCount == batchNew.BatchCount)
                                {
                                    IBatchService.UpdateData(batchNew);

                                    List<Set> existingSets = ISetService.GetDataByFilter(" batch_id=" + batch.Id, 0, 0, false);
                                    List<SetDocument> existingSetDocuments = new List<SetDocument>();
                                    existingSets.ForEach(set =>
                                    {
                                        set.UpdatedDate = DateTime.Now;
                                        set.UpdatedBy = 0;
                                        set.Status = 0;
                                        List<SetDocument> setDocumentsIn = ISetDocumentService.GetDataByFilter(" set_id=" + set.Id, 0, 0, false);
                                        existingSetDocuments.AddRange(setDocumentsIn);
                                    });
                                    ISetService.UpdateBulkData(existingSets);
                                    existingSetDocuments.ForEach(setDoc =>
                                    {
                                        setDoc.UpdatedDate = DateTime.Now;
                                        setDoc.UpdatedBy = 0;
                                        setDoc.Status = 0;
                                    });
                                    ISetDocumentService.UpdateBulkData(existingSetDocuments);
                                }
                            }
                            catch (Exception ex)
                            {
                                LogWriter logWriter = new LogWriter("Exception: " + ex.Message + " - CheckFilesBeforeSync Inner");
                            }
                        }
                        else
                        {
                            // new code
                            if(files.Count() < batchCount)
                            {
                                foreach(string file in files)
                                {
                                    FileInfo fileInfo = new FileInfo(file);
                                    DateTime datetime = DateTime.Now.AddSeconds(-30);
                                    if(fileInfo.LastWriteTime < datetime)
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("Exception: " + ex.Message + " - CheckFilesBeforeSync");
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
            catch (Exception)
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

        public object MoveFilesToNetwork(BackgroundWorker worker, DoWorkEventArgs e)
        {
            string[] files = Directory.GetFiles(FilesPath);
            if (files.Count() > 0)
            {
            }
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (!File.Exists(NetworkPath + fileName))
                {
                    File.Copy(file, NetworkPath + fileName);
                    File.Delete(file);
                }
            }
            return "";
        }

        private System.Timers.Timer timer;
        private System.Timers.Timer mFiletimer;


        public void CheckAndVoidBatches(object sender, System.Timers.ElapsedEventArgs e)
        {
            LogWriter logWriter = new LogWriter("CheckAndVoidBatches: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            try
            {
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " = '1'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '1'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
                List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, false);

                foreach (Batch b in batches)
                {
                    DateTime dateTime;
                    string buddhaDateTime = b.CreatedDate.ToString();
                    IFormatProvider buddhaCulture = CultureInfo.CreateSpecificCulture("th-TH");
                    DateTime.TryParse(buddhaDateTime, buddhaCulture, DateTimeStyles.None, out dateTime);
                    IFormatProvider culture = CultureInfo.CreateSpecificCulture("en-US");
                    DateTime englishDate;
                    if (DateTime.TryParse(dateTime.ToString(), culture, DateTimeStyles.None, out englishDate))
                    {
                        DateTime now;
                        DateTime.TryParse(DateTime.Now.ToString(), culture, DateTimeStyles.None, out now);
                        double hours = (now - englishDate).TotalHours;
                        if (hours > 3)
                        {
                            Batch updateBatch = b;
                            updateBatch.Status = 0;
                            IBatchService.UpdateData(updateBatch);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logWriter = new LogWriter("CheckAndVoidBatches " + ex.Message + " - Exception");
            }
        }

        public void CheckBatchesAndServer()
        {
            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId)) + " in (4, 5)");
            //filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.IsBatchCountUpdated)) + " = '0'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchStatus)) + " = '0'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status)) + " = '1'");
            List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, false);
            batches.ForEach(batch => {
                DateTime currentDatetime = DateTime.Now;
                currentDatetime = currentDatetime.AddSeconds(-10);
                if (batch.UpdatedDate.Value <= currentDatetime && batch.IsBatchCountUpdated != Convert.ToInt32(ServerNo))
                {
                    batch.IsBatchCountUpdated = Convert.ToInt32(ServerNo);
                    batch.UpdatedDate = DateTime.Now;
                    batch.UpdatedBy = 0;
                    IBatchService.UpdateData(batch);
                }
            });

        }

        protected override void OnStop()
        {
            LogWriter logWriter = new LogWriter("Sync Service Stopped");
        }

        public void MFileXmlToSplit(object sender, System.Timers.ElapsedEventArgs e)
        {
            //logWriter = new LogWriter("Post Batch To Split: " + batchXMLPath);
            try
            {
               // List<Set> sets = ISetService.GetDataByFilter(" is_released=1 and status=1", 0, 0, false);
                // Get the Set list for batch_status=6 and is_released=1
                List<Set> sets = ISetService.GetSetsForMfiles();
                foreach (var set in sets)
                {
                    Set UpdateSet = set;
                    Batch batch = IBatchService.GetSingle(set.BatchId);
                    if (batch != null && batch.Id != 0)
                    {
                        //string DeptDirectory = MimzyCaptureOuputELIBRARY;
                        //string DeptExceptionDirectory = MimzyCaptureOuputELIBRARYEx;

                        Department department = IDepartmentService.GetSingle(batch.DepartmentId);
                        if (department != null && department.Id != 0)
                        {
                            string departmentCode = department.Code;
                            string deptCode = "";
                           // logWriter = new LogWriter("MFileXmlToSplit department Code " + department.Code);

                            if (!string.IsNullOrEmpty(department.Code))
                            {
                                deptCode = departmentCode.Split('-')[0];
                                string jobCode = departmentCode.Split('-')[1];
                            }
                            string DeptDirectory = "";
                            string DeptExceptionDirectory = "";
                            if (departmentCode == "E-LIBRARY")
                            {
                                DeptDirectory = MimzyCaptureOuputELIBRARY;
                                DeptExceptionDirectory = MimzyCaptureOuputELIBRARYEx;
                            }
                            else
                            {
                                if (deptCode == "ETP")
                                {
                                    DeptDirectory = MimzyCaptureOuputETP;
                                    DeptExceptionDirectory = MimzyCaptureOuputELIBRARYEx;
                                }
                                else if (deptCode == "LOS")
                                {
                                    DeptDirectory = MimzyCaptureOuputLOS;
                                    DeptExceptionDirectory = MimzyCaptureOuputLOSEx;
                                }
                            }

                            if (!File.Exists(DeptDirectory + set.AaNo + "_" + set.Id + ".xml"))
                            {
                                UpdateSet.IsReleased = 2;
                                DirectoryInfo infoExceptionDir = new DirectoryInfo(DeptExceptionDirectory);
                                FileInfo[] exceptionFiles = infoExceptionDir.GetFiles(set.AaNo + "_" + set.Id + "*ERRORS.xml", SearchOption.AllDirectories);

                                if (exceptionFiles.Count() > 0)
                                {
                                    foreach (FileInfo file in exceptionFiles)
                                    {
                                        string fileName = file.Name;

                                        if (!string.IsNullOrEmpty(fileName) && fileName.Split('_').Count() > 2)
                                        {
                                            string isError = fileName.Split('_')[2];

                                            if (!string.IsNullOrEmpty(isError) && isError == "ERRORS.xml")
                                            {
                                                var xmlDoc = new XmlDocument();
                                                xmlDoc.Load(file.FullName);
                                                MemoryStream xmlStream = new MemoryStream();
                                                xmlDoc.Save(xmlStream);
                                                xmlStream.Flush();//Adjust this if you want read your data 
                                                xmlStream.Position = 0;
                                                XmlSerializer deserializer = new XmlSerializer(typeof(MFileDocuments));
                                                TextReader textReader = new StreamReader(xmlStream);
                                                MFileDocuments mFileStatusModel;
                                                mFileStatusModel = (MFileDocuments)deserializer.Deserialize(textReader);
                                                if (mFileStatusModel != null && mFileStatusModel.Document != null && mFileStatusModel.Document.Count != 0)
                                                {
                                                    List<Set> setList = new List<Set>();
                                                    foreach (var item in mFileStatusModel.Document)
                                                    {
                                                        if (item.MFILES_IMPORTER_ERROR != null && !string.IsNullOrEmpty(item.MFILES_IMPORTER_ERROR.Text))
                                                        {
                                                            UpdateSet.Remarks = item.MFILES_IMPORTER_ERROR.Text.Replace("'", " ");
                                                            UpdateSet.UpdatedBy = 0;
                                                            UpdateSet.UpdatedDate = DateTime.Now;
                                                            UpdateSet.Status = 9;
                                                            ISetService.UpdateData(UpdateSet);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    UpdateSet.Status = 9;

                                }
                                else
                                {
                                    UpdateSet.Remarks = "";
                                }
                            }
                            UpdateSet.UpdatedBy = 0;
                            UpdateSet.UpdatedDate = DateTime.Now;
                            ISetService.UpdateData(UpdateSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("MFileXmlToSplit " + ex.Message + " - Exception");
            }
        }
    }
}
