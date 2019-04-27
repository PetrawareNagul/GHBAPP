using MetroFramework.Forms;
using Silverlake.Window.Custom;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.ListView;

namespace Silverlake.Window
{
    public partial class FileWatcher : MetroForm
    {
        private StringBuilder stringBuilder;
        public List<LogList> logListFromADC;
        public List<LogList> logListSplitFromADC;
        public List<LogList> logListADCStatus;
        private FileSystemWatcher watcherFromADC;
        private FileSystemWatcher watcherSplitFromADC;
        private FileSystemWatcher watcherADCStatus;
        private bool isWatchingFromADC;
        private bool isWatchingSplitFromADC;
        private bool isWatchingADCStatus;
        private bool isDirtyFromADC;
        private bool isDirtySplitFromADC;
        private bool isDirtyADCStatus;

        public string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
        public string FromADCArchive = ConfigurationManager.AppSettings["FromADCArchive"].ToString();
        public string FromADCException = ConfigurationManager.AppSettings["FromADCException"].ToString();
        public string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
        public string SplitFromADCArchive = ConfigurationManager.AppSettings["SplitFromADCArchive"].ToString();
        public string SplitFromADCException = ConfigurationManager.AppSettings["SplitFromADCException"].ToString();
        public string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
        public string ADCStatusArchive = ConfigurationManager.AppSettings["ADCStatusArchive"].ToString();
        public string ADCStatusException = ConfigurationManager.AppSettings["ADCStatusException"].ToString();
        public string BranchId = ConfigurationManager.AppSettings["BranchId"].ToString();
        public string DepartmentId = ConfigurationManager.AppSettings["DepartmentId"].ToString();

        public FileWatcher()
        {
            InitializeComponent();
            logListFromADC = new List<LogList>();
            logListSplitFromADC = new List<LogList>();
            logListADCStatus = new List<LogList>();
            stringBuilder = new StringBuilder();
            isWatchingFromADC = false;
            isWatchingSplitFromADC = false;
            isWatchingADCStatus = false;
            isDirtyFromADC = false;
            isDirtySplitFromADC = false;
            isDirtyADCStatus = false;
            fromADCLogListView.Columns.Add("Log Message", -2, HorizontalAlignment.Left);
            splitFromADCLogListView.Columns.Add("Log Message", -2, HorizontalAlignment.Left);
            //logListView.Columns.Add("Change Type", 100, HorizontalAlignment.Left);
            //logListView.Columns.Add("File Name", 398, HorizontalAlignment.Left);

            //Check the files if exists and export first
            ProcessFromADCFiles();
            ProcessSplitFromADCFiles();
            ProcessADCStatusFiles();

            //Add watch for folders to process
            WatchFromADCFolder();
            WatchSplitFromADCFolder();
            WatchADCStatusFolder();
        }

        public void WatchFromADCFolder()
        {
            if (isWatchingFromADC)
            {
                isWatchingFromADC = false;
                watcherFromADC.EnableRaisingEvents = false;
                watcherFromADC.Dispose();
            }
            else
            {

                isWatchingFromADC = true;
                watcherFromADC = new FileSystemWatcher();
                string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
                watcherFromADC.Path = FromADC;

                watcherFromADC.Filter = "*.*";

                watcherFromADC.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.DirectoryName |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Security |
                                        NotifyFilters.Size;

                watcherFromADC.IncludeSubdirectories = false;

                watcherFromADC.Changed += new FileSystemEventHandler(FromADCOnChanged);
                watcherFromADC.Created += new FileSystemEventHandler(FromADCOnChanged);
                watcherFromADC.Deleted += new FileSystemEventHandler(FromADCOnChanged);
                watcherFromADC.Renamed += new RenamedEventHandler(FromADCOnRenamed);

                //watcher.EnableRaisingEvents = true;
                watcherFromADC.EnableRaisingEvents = true;
            }
        }

        public int index = 1;

        private bool IsFileReady(string path)
        {
            //One exception per file rather than several like in the polling pattern
            try
            {
                //If we can't open the file, it's still copying
                using (var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        public void FromADCOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.ToString() == "Created" || e.ChangeType.ToString() == "Deleted")
            {
                logListFromADC.Add(new LogList
                {
                    FileFullPath = e.FullPath,
                    ChangeType = e.ChangeType.ToString(),
                    UpdatedDate = DateTime.Now
                });
            }
            if (!isDirtyFromADC)
            {
                if (e.ChangeType.ToString() == "Created")
                {
                    Task.Delay(1000).ContinueWith(t => ProcessFromADCFiles());
                }
                else
                {
                }
                isDirtyFromADC = true;
            }
        }

        public void ProcessFromADCFiles()
        {
            string[] files = Directory.GetFiles(FromADC);
            int index = 1;
            foreach (string file in files)
            {
                string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index;
                string fileExtension = Path.GetExtension(file);
                string fileNameWithExtension = Path.GetFileName(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileExtension.ToLower() == ".xml")
                {
                    string batchKey = fileName.Split('_')[1];
                    DepartmentId = fileName.Split('_')[2];
                    BranchId = fileName.Split('_')[3];
                    List<string> appendicesUrls = new List<string>();
                    XDocument xdoc = XDocument.Load(file);
                    if (xdoc.Root.Name == "records" && xdoc.Root.Elements().First().Name == "batches")
                    {
                        var batchElement = xdoc.Root.Elements().First();
                        foreach (var setElement in batchElement.Elements())
                        {
                            var firstFieldElement = setElement.Elements("forms").First().Elements("field").First();
                            string setAccNo = firstFieldElement.Attribute("name").Value == "AA_NUMBER" ? firstFieldElement.Value : "";

                            string CommonFileName = currentDateString + "_" + batchKey + "_" + setAccNo + "_" + DepartmentId + "_" + BranchId;

                            string XMLFilePath = SplitFromADC + CommonFileName + fileExtension;

                            foreach (var formElemement in setElement.Elements().Skip(1))
                            {
                                var docType = formElemement.Elements("field").Where(x => x.Attribute("name").Value == "DOC_TYPE").First();
                                var appendices = formElemement.Elements("field").Where(x => x.Attribute("name").Value == "APPENDICES").First();
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

                                //using (MagickImageCollection collection = new MagickImageCollection())
                                //{
                                //    foreach (string subURL in subUrls)
                                //    {
                                //        string urlFileExtension = Path.GetExtension(subURL);
                                //        string urlFileNameWithExtension = Path.GetFileName(subURL);
                                //        if (urlFileExtension.ToLower() == ".tif")
                                //        {
                                //            var magickImage = new MagickImage(subURL);
                                //            collection.Add(magickImage);
                                //        }
                                //    }
                                //    string PDFFilePath = SplitFromADC + CommonFileName + "_" + type + ".pdf";
                                //    collection.Write(PDFFilePath);
                                //}
                                appendicesUrls.AddRange(subUrls);
                            }


                            //element.SetAttributeValue("batch_guid", xdoc.Root.Elements().First().Attribute("guid"));
                            setElement.Save(XMLFilePath);
                        }
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
                        if (File.Exists(FromADCArchive + urlFileNameWithExtension))
                            File.Delete(FromADCArchive + urlFileNameWithExtension);
                        File.Move(url, FromADCArchive + urlFileNameWithExtension);
                    }

                }
                index++;
            }
        }

        public void FromADCOnRenamed(object sender, RenamedEventArgs e)
        {
            //logListFromADC.Add(new LogList
            //{
            //    FileFullPath = e.FullPath,
            //    ChangeType = e.ChangeType.ToString(),
            //    UpdatedDate = DateTime.Now
            //});
            if (!isDirtyFromADC)
            {

                isDirtyFromADC = true;
            }
        }

        private void writeFromADCLog_Tick(object sender, EventArgs e)
        {
            if (isDirtyFromADC)
            {
                fromADCLogListView.BeginUpdate();
                fromADCLogListView.Items.Clear();
                fromADCLogListView.Items.AddRange(
                    logListFromADC
                        .Select(x =>
                        {
                            ListViewItem listViewItem = new ListViewItem();
                            listViewItem.SubItems.Clear();
                            listViewItem.SubItems.Insert(0, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            //listViewItem.SubItems.Insert(1, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            return listViewItem;
                        })
                        .ToList<ListViewItem>()
                        .ToArray()
                );
                fromADCLogListView.EndUpdate();
                isDirtyFromADC = false;
            }
        }

        public void WatchSplitFromADCFolder()
        {
            if (isWatchingSplitFromADC)
            {
                isWatchingSplitFromADC = false;
                watcherSplitFromADC.EnableRaisingEvents = false;
                watcherSplitFromADC.Dispose();
            }
            else
            {

                isWatchingSplitFromADC = true;
                watcherSplitFromADC = new FileSystemWatcher();
                string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
                watcherSplitFromADC.Path = SplitFromADC;

                watcherSplitFromADC.Filter = "*.*";

                watcherSplitFromADC.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.DirectoryName |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Security |
                                        NotifyFilters.Size;

                watcherSplitFromADC.IncludeSubdirectories = false;

                watcherSplitFromADC.Changed += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcherSplitFromADC.Created += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcherSplitFromADC.Deleted += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcherSplitFromADC.Renamed += new RenamedEventHandler(SplitFromADCOnRenamed);

                //watcher.EnableRaisingEvents = true;
                watcherSplitFromADC.EnableRaisingEvents = true;
            }
        }

        public void SplitFromADCOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.ToString() == "Created" || e.ChangeType.ToString() == "Deleted")
            {
                logListSplitFromADC.Add(new LogList
                {
                    FileFullPath = e.FullPath,
                    ChangeType = e.ChangeType.ToString(),
                    UpdatedDate = DateTime.Now
                });
            }
            if (!isDirtySplitFromADC)
            {
                if (e.ChangeType.ToString() == "Created")
                {
                    Task.Delay(1000).ContinueWith(t => ProcessSplitFromADCFiles());
                }
                else
                {
                }
                isDirtySplitFromADC = true;
            }
        }

        public void ProcessSplitFromADCFiles()
        {
            string[] files = Directory.GetFiles(SplitFromADC);
            foreach (string file in files)
            {
                //string fileExtension = Path.GetExtension(file);
                string fileNameWithExtension = Path.GetFileName(file);
                //string fileName = Path.GetFileNameWithoutExtension(file);
                try
                {
                    TransmissionApiCalls.PostFile(file);
                    if (File.Exists(SplitFromADCArchive + fileNameWithExtension))
                        File.Delete(SplitFromADCArchive + fileNameWithExtension);
                    File.Move(file, SplitFromADCArchive + fileNameWithExtension);
                }
                catch (Exception ex)
                {
                    if (File.Exists(SplitFromADCException + fileNameWithExtension))
                        File.Delete(SplitFromADCException + fileNameWithExtension);
                    try
                    {
                        File.Move(file, SplitFromADCException + fileNameWithExtension);
                    }
                    catch(Exception exSub)
                    {

                    }
                }
            }
        }

        public void SplitFromADCOnRenamed(object sender, RenamedEventArgs e)
        {
            //logListSplitFromADC.Add(new LogList
            //{
            //    FileFullPath = e.FullPath,
            //    ChangeType = e.ChangeType.ToString(),
            //    UpdatedDate = DateTime.Now
            //});
            if (!isDirtySplitFromADC)
            {

                isDirtySplitFromADC = true;
            }
        }

        private void writeSplitFromADCLog_Tick(object sender, EventArgs e)
        {
            if (isDirtySplitFromADC)
            {
                splitFromADCLogListView.BeginUpdate();
                splitFromADCLogListView.Items.Clear();
                splitFromADCLogListView.Items.AddRange(
                    logListSplitFromADC
                        .Select(x =>
                        {
                            ListViewItem listViewItem = new ListViewItem();
                            listViewItem.SubItems.Clear();
                            listViewItem.SubItems.Insert(0, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            //listViewItem.SubItems.Insert(1, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            return listViewItem;
                        })
                        .ToList<ListViewItem>()
                        .ToArray()
                );
                splitFromADCLogListView.EndUpdate();
                //Task.Delay(1000).ContinueWith(t => ProcessSplitFromADCFiles());
                isDirtySplitFromADC = false;
            }
        }

        public void WatchADCStatusFolder()
        {
            if (isWatchingADCStatus)
            {
                isWatchingADCStatus = false;
                watcherADCStatus.EnableRaisingEvents = false;
                watcherADCStatus.Dispose();
            }
            else
            {

                isWatchingADCStatus = true;
                watcherADCStatus = new FileSystemWatcher();
                string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
                watcherADCStatus.Path = ADCStatus;

                watcherADCStatus.Filter = "*.*";

                watcherADCStatus.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.DirectoryName |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Security |
                                        NotifyFilters.Size;

                watcherADCStatus.IncludeSubdirectories = false;

                watcherADCStatus.Changed += new FileSystemEventHandler(ADCStatusOnChanged);
                watcherADCStatus.Created += new FileSystemEventHandler(ADCStatusOnChanged);
                watcherADCStatus.Deleted += new FileSystemEventHandler(ADCStatusOnChanged);
                watcherADCStatus.Renamed += new RenamedEventHandler(ADCStatusOnRenamed);

                //watcher.EnableRaisingEvents = true;
                watcherADCStatus.EnableRaisingEvents = true;
            }
        }

        public void ADCStatusOnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.ToString() == "Created" || e.ChangeType.ToString() == "Deleted")
            {
                logListADCStatus.Add(new LogList
                {
                    FileFullPath = e.FullPath,
                    ChangeType = e.ChangeType.ToString(),
                    UpdatedDate = DateTime.Now
                });
            }

            if (!isDirtyADCStatus)
            {
                if (e.ChangeType.ToString() == "Created")
                {
                    Task.Delay(1000).ContinueWith(t => ProcessADCStatusFiles());
                }
                else
                {
                }
                isDirtyADCStatus = true;
            }
        }

        public void ProcessADCStatusFiles()
        {
            string[] files = Directory.GetFiles(ADCStatus);
            foreach (string file in files)
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

        public void ADCStatusOnRenamed(object sender, RenamedEventArgs e)
        {
            //logListADCStatus.Add(new LogList
            //{
            //    FileFullPath = e.FullPath,
            //    ChangeType = e.ChangeType.ToString(),
            //    UpdatedDate = DateTime.Now
            //});
            if (!isDirtyADCStatus)
            {

                isDirtyADCStatus = true;
            }
        }

        private void writeADCStatusLog_Tick(object sender, EventArgs e)
        {
            if (isDirtyADCStatus)
            {
                ADCStatusLogListView.BeginUpdate();
                ADCStatusLogListView.Items.Clear();
                ADCStatusLogListView.Items.AddRange(
                    logListADCStatus
                        .Select(x =>
                        {
                            ListViewItem listViewItem = new ListViewItem();
                            listViewItem.SubItems.Clear();
                            listViewItem.SubItems.Insert(0, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            //listViewItem.SubItems.Insert(1, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            return listViewItem;
                        })
                        .ToList<ListViewItem>()
                        .ToArray()
                );
                ADCStatusLogListView.EndUpdate();
                
                isDirtyADCStatus = false;
            }
        }
    }
}
