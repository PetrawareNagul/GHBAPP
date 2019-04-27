using MetroFramework.Forms;
using Silverlake.Web.ServiceCalls;
using Silverlake.Window.Custom;
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
        private FileSystemWatcher watcher;
        private bool isWatchingFromADC;
        private bool isWatchingSplitFromADC;
        private bool isDirtyFromADC;
        private bool isDirtySplitFromADC;

        public string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
        public string FromADCArchive = ConfigurationManager.AppSettings["FromADCArchive"].ToString();
        public string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
        public string SplitFromADCArchive = ConfigurationManager.AppSettings["SplitFromADCArchive"].ToString();
        public string BranchId = ConfigurationManager.AppSettings["BranchId"].ToString();
        public string DepartmentId = ConfigurationManager.AppSettings["DepartmentId"].ToString();

        public FileWatcher()
        {
            InitializeComponent();
            logListFromADC = new List<LogList>();
            logListSplitFromADC = new List<LogList>();
            stringBuilder = new StringBuilder();
            isWatchingFromADC = false;
            isWatchingSplitFromADC = false;
            isDirtyFromADC = false;
            isDirtySplitFromADC = false;
            fromADCLogListView.Columns.Add("Log Message", -2, HorizontalAlignment.Left);
            splitFromADCLogListView.Columns.Add("Log Message", -2, HorizontalAlignment.Left);
            //logListView.Columns.Add("Change Type", 100, HorizontalAlignment.Left);
            //logListView.Columns.Add("File Name", 398, HorizontalAlignment.Left);
            WatchFromADCFolder();
            WatchSplitFromADCFolder();
        }

        public void WatchFromADCFolder()
        {
            if (isWatchingFromADC)
            {
                isWatchingFromADC = false;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            else
            {

                isWatchingFromADC = true;
                watcher = new FileSystemWatcher();
                string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
                watcher.Path = FromADC;

                watcher.Filter = "*.*";

                watcher.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.DirectoryName |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Security |
                                        NotifyFilters.Size;

                watcher.IncludeSubdirectories = true;

                watcher.Changed += new FileSystemEventHandler(FromADCOnChanged);
                watcher.Created += new FileSystemEventHandler(FromADCOnChanged);
                watcher.Deleted += new FileSystemEventHandler(FromADCOnChanged);
                watcher.Renamed += new RenamedEventHandler(FromADCOnRenamed);

                //watcher.EnableRaisingEvents = true;
                watcher.EnableRaisingEvents = true;
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
            ProcessFromADCFiles();
            logListFromADC.Add(new LogList
            {
                FileFullPath = e.FullPath,
                ChangeType = e.ChangeType.ToString(),
                UpdatedDate = DateTime.Now
            });
            //if (!isDirtyFromADC)
            //{
            //    if (e.ChangeType.ToString() == "Created")
            //    {

            //    }
            //    else
            //    {
            //    }


            //    isDirtyFromADC = true;
            //}
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
                string batchNo = fileName.Split('_')[2];
                if (fileExtension.ToLower() == ".xml")
                {
                    XDocument xdoc = XDocument.Load(file);
                    foreach (var element in xdoc.Root.Elements())
                    {
                        string setAccNo = element.Attribute("account_no").Value;
                        string filePath = SplitFromADC + currentDateString + "_" + setAccNo + "_" + batchNo + "_" + DepartmentId + "_" + BranchId + fileExtension;
                        element.Save(filePath);
                    }
                }
                index++;
                File.Move(file, FromADCArchive + fileNameWithExtension);
            }
        }

        public void FromADCOnRenamed(object sender, RenamedEventArgs e)
        {
            if (!isDirtyFromADC)
            {
                logListFromADC.Add(new LogList
                {
                    FileFullPath = e.FullPath,
                    ChangeType = e.ChangeType.ToString(),
                    UpdatedDate = DateTime.Now
                });
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
                            listViewItem.SubItems.Insert(0, new ListViewItem.ListViewSubItem { Text = x.ChangeType + " : " + Path.GetFileName(x.FileFullPath) });
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
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            else
            {

                isWatchingSplitFromADC = true;
                watcher = new FileSystemWatcher();
                string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
                watcher.Path = SplitFromADC;

                watcher.Filter = "*.*";

                watcher.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.DirectoryName |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Security |
                                        NotifyFilters.Size;

                watcher.IncludeSubdirectories = true;

                watcher.Changed += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcher.Created += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcher.Deleted += new FileSystemEventHandler(SplitFromADCOnChanged);
                watcher.Renamed += new RenamedEventHandler(SplitFromADCOnRenamed);

                //watcher.EnableRaisingEvents = true;
                watcher.EnableRaisingEvents = true;
            }
        }

        public void SplitFromADCOnChanged(object sender, FileSystemEventArgs e)
        {
            ProcessSplitFromADCFiles();
            logListSplitFromADC.Add(new LogList
            {
                FileFullPath = e.FullPath,
                ChangeType = e.ChangeType.ToString(),
                UpdatedDate = DateTime.Now
            });
        }

        public void ProcessSplitFromADCFiles()
        {
            string[] files = Directory.GetFiles(SplitFromADC);
            foreach (string file in files)
            {
                //string fileExtension = Path.GetExtension(file);
                string fileNameWithExtension = Path.GetFileName(file);
                //string fileName = Path.GetFileNameWithoutExtension(file);
                TransmissionApiCalls.PostFile(file);
                File.Move(file, SplitFromADCArchive + fileNameWithExtension);
            }
        }

        public void SplitFromADCOnRenamed(object sender, RenamedEventArgs e)
        {
            if (!isDirtySplitFromADC)
            {
                logListSplitFromADC.Add(new LogList
                {
                    FileFullPath = e.FullPath,
                    ChangeType = e.ChangeType.ToString(),
                    UpdatedDate = DateTime.Now
                });
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
                            listViewItem.SubItems.Insert(0, new ListViewItem.ListViewSubItem { Text = x.ChangeType + " : " + Path.GetFileName(x.FileFullPath) });
                            //listViewItem.SubItems.Insert(1, new ListViewItem.ListViewSubItem { Text = Path.GetFileName(x.FileFullPath) });
                            return listViewItem;
                        })
                        .ToList<ListViewItem>()
                        .ToArray()
                );
                splitFromADCLogListView.EndUpdate();
                isDirtySplitFromADC = false;
            }
        }
    }
}
