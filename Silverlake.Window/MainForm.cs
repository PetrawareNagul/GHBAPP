using MetroFramework.Forms;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
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
using System.Xml;
using System.Xml.Linq;

namespace Silverlake.Window
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();

            List<Branch> branches = ApiCalls.GetBranches();
            branches.Insert(0, new Branch { Name = "Select Branch", Id = 0, Code = "0" });
            ddlBranches.DataSource = branches;
            ddlBranches.DisplayMember = "Name";
            ddlBranches.ValueMember = "Code";
            ddlBranches.SelectedValue = "0";

            List<Department> departments = ApiCalls.GetDepartments();
            departments.Insert(0, new Department { Name = "Select Department", Id = 0, Code = "0" });
            ddlDepartments.DataSource = departments;
            ddlDepartments.DisplayMember = "Name";
            ddlDepartments.ValueMember = "Code";
            ddlDepartments.SelectedValue = "0";

            List<Batch> batches = new List<Batch>();
            batches.Insert(0, new Batch { BatchNo = "Select Batch", Id = 0 });
            ddlBatches.DataSource = batches;
            ddlBatches.DisplayMember = "BatchNo";
            ddlBatches.ValueMember = "Id";
            ddlBatches.SelectedValue = 0;

            List<Stage> stages = ApiCalls.GetStages();
            stages.Insert(0, new Stage { Name = "Select Stage", Id = 0 });
            stages.Add(new Stage { Name = "Delete", Id = 9 });
            ddlStages.DataSource = stages;
            ddlStages.DisplayMember = "Name";
            ddlStages.ValueMember = "Id";
            ddlStages.SelectedValue = 0;
        }

        private void btnPostFiles_Click(object sender, EventArgs e)
        {
            string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
            string[] files = Directory.GetFiles(FromADC);
            foreach (string file in files)
            {
                string postUrl = file;
                TransmissionApiCalls.PostFile(postUrl);
            }
        }

        private void btnNewBatch_Click(object sender, EventArgs e)
        {
            string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
            //User user = APIUser.GetUser();
            if (ddlBranches.SelectedValue.ToString() != "0" && ddlDepartments.SelectedValue.ToString() != "0" && txtBatchCount.Text != "")
            {
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
                string branchId = (ddlBranches.SelectedValue.ToString());
                string deptId = (ddlDepartments.SelectedValue.ToString());
                Int32 batchCount = Convert.ToInt32(txtBatchCount.Text);
                DateTime currentDate = DateTime.Now;
                string key = currentDate.ToString("yyyyMMddHHmmssfff");
                string currentDateString = currentDate.ToString("yyyy-MM-ddTHH:mm:ss");
                el.AppendChild(doc.CreateElement("BranchId")).InnerText = branchId.ToString();
                el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = deptId.ToString();
                el.AppendChild(doc.CreateElement("StageId")).InnerText = "1";
                el.AppendChild(doc.CreateElement("BatchKey")).InnerText = key;
                el.AppendChild(doc.CreateElement("UserId")).InnerText = "sachin";
                el.AppendChild(doc.CreateElement("BatchNo")).InnerText = "batch" + key;
                el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batchCount.ToString();
                el.AppendChild(doc.CreateElement("Status")).InnerText = "1";
                el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = currentDateString;
                el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = currentDateString;
                //el.AppendChild(doc.CreateElement("CreatedBy")).InnerText = user.Id.ToString();
                //el.AppendChild(doc.CreateElement("UpdatedBy")).InnerText = user.Id.ToString();
                doc.Save(ADCStatus + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_"+ key + "_" + deptId + "_" + branchId + ".xml");
                //TransmissionApiCalls.PostNewBatchXML(doc);
            }
            else
            {
                // Please select
            }
        }

        private void ddlDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranches.SelectedValue.ToString() != "0" && ddlDepartments.SelectedValue.ToString() != "0")
            {
                List<Branch> branches = ApiCalls.GetBranches();
                List<Department> departments = ApiCalls.GetDepartments();

                Int32 branchId = branches.Where(x=>x.Code == ddlBranches.SelectedValue.ToString()).FirstOrDefault().Id;
                Int32 departmentId = departments.Where(x => x.Code == ddlDepartments.SelectedValue.ToString()).FirstOrDefault().Id;

                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BranchId)) + " = '" + branchId + "'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.DepartmentId)) + " = '" + departmentId + "'");

                List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                batches.Insert(0, new Batch { BatchNo = "Select Batch", Id = 0 });
                ddlBatches.DataSource = batches;
                ddlBatches.DisplayMember = "BatchNo";
                ddlBatches.ValueMember = "Id";
                ddlBatches.SelectedValue = 0;
            }
        }

        private void ddlBatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBatches.Items.Count > 1)
            {
                if (ddlBatches.SelectedValue.ToString() != "0")
                {
                    StringBuilder filter = new StringBuilder();
                    filter.Append(" 1=1");
                    filter.Append(" and ID = '" + ddlBatches.SelectedValue + "'");
                    List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                    Batch batch = batches.FirstOrDefault();
                    ddlStages.SelectedValue = batch.StageId;
                }
            }
        }

        private void btnUpdateBatchStatus_Click(object sender, EventArgs e)
        {
            List<Branch> branches = ApiCalls.GetBranches();
            List<Department> departments = ApiCalls.GetDepartments();

            string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
            string ADCStatus = ConfigurationManager.AppSettings["ADCStatus"].ToString();
            //User user = APIUser.GetUser();
            if (ddlBatches.SelectedValue.ToString() != "0" && ddlStages.SelectedValue.ToString() != "0")
            {
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and ID = '" + ddlBatches.SelectedValue + "'");
                List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                Batch batch = batches.FirstOrDefault();

                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
                string branchCode = branches.Where(x=>x.Id == batch.BranchId).FirstOrDefault().Code;
                string departmentCode = departments.Where(x => x.Id == batch.DepartmentId).FirstOrDefault().Code;
                Int32 batchCount = batch.BatchCount.Value;
                DateTime createdDate = batch.CreatedDate;
                DateTime updatedDate = DateTime.Now;
                string key = batch.BatchKey;
                string no = batch.BatchNo;
                string createdDateString = createdDate.ToString("yyyy-MM-ddTHH:mm:ss");
                string updatedDateString = updatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                el.AppendChild(doc.CreateElement("BranchId")).InnerText = branchCode;
                el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = departmentCode;
                el.AppendChild(doc.CreateElement("StageId")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? batch.StageId.ToString() : ddlStages.SelectedValue.ToString();
                el.AppendChild(doc.CreateElement("BatchKey")).InnerText = key;
                el.AppendChild(doc.CreateElement("UserId")).InnerText = "sachin";
                el.AppendChild(doc.CreateElement("BatchNo")).InnerText = no;
                el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batchCount.ToString();
                el.AppendChild(doc.CreateElement("Status")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? "9" : "1";
                el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = createdDateString;
                el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = updatedDateString;
                doc.Save(ADCStatus + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_"+ key + "_" + departmentCode + "_" + branchCode + ".xml");
                if(ddlStages.SelectedValue.ToString() == "4")
                {
                    string filepath = "D:\\SL\\Gordon\\GHB\\ADC-Export-XML\\181018022921625_cf602fa3-d11a-4825-b455-148ba0ff9c23_DOCSCAN_SC01.xml";
                    string fileName = Path.GetFileName(filepath);

                    File.Copy(filepath, FromADC + key + "_" + departmentCode + "_" + branchCode + ".xml");
                }
                //TransmissionApiCalls.PostNewBatchXML(doc);
            }
            else if(ddlBranches.SelectedValue.ToString() != "0" && ddlDepartments.SelectedValue.ToString() != "0")
            {
                Int32 branchId = branches.Where(x => x.Code == ddlBranches.SelectedValue.ToString()).FirstOrDefault().Id;
                Int32 departmentId = departments.Where(x => x.Code == ddlDepartments.SelectedValue.ToString()).FirstOrDefault().Id;

                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BranchId)) + " = '" + branchId + "'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.DepartmentId)) + " = '" + departmentId + "'");

                List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                foreach(Batch batch in batches)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);
                    XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
                    string branchCode = branches.Where(x => x.Id == batch.BranchId).FirstOrDefault().Code;
                    string departmentCode = departments.Where(x => x.Id == batch.DepartmentId).FirstOrDefault().Code;
                    Int32 batchCount = batch.BatchCount.Value;
                    DateTime createdDate = batch.CreatedDate;
                    DateTime updatedDate = DateTime.Now;
                    string key = batch.BatchKey;
                    string no = batch.BatchNo;
                    string createdDateString = createdDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    string updatedDateString = updatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    el.AppendChild(doc.CreateElement("BranchId")).InnerText = branchCode;
                    el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = departmentCode;
                    el.AppendChild(doc.CreateElement("StageId")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? batch.StageId.ToString() : ddlStages.SelectedValue.ToString();
                    el.AppendChild(doc.CreateElement("BatchKey")).InnerText = key;
                    el.AppendChild(doc.CreateElement("UserId")).InnerText = "sachin";
                    el.AppendChild(doc.CreateElement("BatchNo")).InnerText = no;
                    el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batchCount.ToString();
                    el.AppendChild(doc.CreateElement("Status")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? "9" : "1";
                    el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = createdDateString;
                    el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = updatedDateString;
                    doc.Save(ADCStatus + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + key + "_" + departmentCode + "_" + branchCode + ".xml");
                    if (ddlStages.SelectedValue.ToString() == "4")
                    {
                        string filepath = "D:\\SL\\Gordon\\GHB\\ADC-Export-XML\\181018022921625_cf602fa3-d11a-4825-b455-148ba0ff9c23_DOCSCAN_SC01.xml";
                        string fileName = Path.GetFileName(filepath);

                        File.Copy(filepath, FromADC + key + "_" + departmentCode + "_" + branchCode + ".xml");
                    }
                }
            }
            else
            {
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");

                List<Batch> batches = ApiCalls.GetBatches(filter.ToString());
                foreach (Batch batch in batches)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);
                    XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("BatchStatusModel"));
                    string branchCode = branches.Where(x => x.Id == batch.BranchId).FirstOrDefault().Code;
                    string departmentCode = departments.Where(x => x.Id == batch.DepartmentId).FirstOrDefault().Code;
                    Int32 batchCount = batch.BatchCount.Value;
                    DateTime createdDate = batch.CreatedDate;
                    DateTime updatedDate = DateTime.Now;
                    string key = batch.BatchKey;
                    string no = batch.BatchNo;
                    string createdDateString = createdDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    string updatedDateString = updatedDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    el.AppendChild(doc.CreateElement("BranchId")).InnerText = branchCode;
                    el.AppendChild(doc.CreateElement("DepartmentId")).InnerText = departmentCode;
                    el.AppendChild(doc.CreateElement("StageId")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? batch.StageId.ToString() : ddlStages.SelectedValue.ToString();
                    el.AppendChild(doc.CreateElement("BatchKey")).InnerText = key;
                    el.AppendChild(doc.CreateElement("UserId")).InnerText = "sachin";
                    el.AppendChild(doc.CreateElement("BatchNo")).InnerText = no;
                    el.AppendChild(doc.CreateElement("BatchCount")).InnerText = batchCount.ToString();
                    el.AppendChild(doc.CreateElement("Status")).InnerText = ddlStages.SelectedValue.ToString() == "9" ? "9" : "1";
                    el.AppendChild(doc.CreateElement("CreatedDate")).InnerText = createdDateString;
                    el.AppendChild(doc.CreateElement("UpdatedDate")).InnerText = updatedDateString;
                    doc.Save(ADCStatus + "rpt-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + key + "_" + departmentCode + "_" + branchCode + ".xml");
                    if (ddlStages.SelectedValue.ToString() == "4")
                    {
                        string filepath = "D:\\SL\\Gordon\\GHB\\ADC-Export-XML\\181018022921625_cf602fa3-d11a-4825-b455-148ba0ff9c23_DOCSCAN_SC01.xml";
                        string fileName = Path.GetFileName(filepath);

                        File.Copy(filepath, FromADC + key + "_" + departmentCode + "_" + branchCode + ".xml");
                    }
                }
            }
        }

        private void btnSplitAndPost_Click(object sender, EventArgs e)
        {
            string SplitFromADC = ConfigurationManager.AppSettings["SplitFromADC"].ToString();
            string FromADC = ConfigurationManager.AppSettings["FromADC"].ToString();
            string[] files = Directory.GetFiles(FromADC);
            int index = 1;
            foreach (string file in files)
            {
                string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index;
                string fileExtension = Path.GetExtension(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                string batchNo = fileName.Split('_')[2];
                if (fileExtension == ".xml")
                {
                    XDocument xdoc = XDocument.Load(file);
                    foreach (var element in xdoc.Root.Elements())
                    {
                        string setAccNo = element.Attribute("account_no").Value;
                        string filePath = SplitFromADC + currentDateString + "_" + setAccNo + "_" + batchNo + "_" + ddlDepartments.SelectedValue.ToString() + "_" + ddlBranches.SelectedValue.ToString() + fileExtension;
                        element.Save(filePath);
                    }
                }
                index++;
            }
        }

        private void btnConvertTIFFsToPDF_Click(object sender, EventArgs e)
        {
            lblMessage.Text = DateTime.Now.ToString("HH : mm : ss");
            string TIFFFilesPath = ConfigurationManager.AppSettings["TIFFFilesPath"].ToString();
            string[] files = Directory.GetFiles(TIFFFilesPath);
            //using (MagickImageCollection collection = new MagickImageCollection())
            //{
            //    foreach (string file in files)
            //    {
            //        string fileExtension = Path.GetExtension(file);
            //        if (fileExtension.ToLower() == ".tif")
            //        {
            //            var magickImage = new MagickImage(file);
            //            collection.Add(magickImage);
            //        }
            //    }
            //    collection.Write(TIFFFilesPath + "TIFFsToPDF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");
            //}
            lblMessage.Text += " - " + DateTime.Now.ToString("HH : mm : ss");
        }

        private void btnOpenFolderWatcher_Click(object sender, EventArgs e)
        {
            var folderWatcher = new FolderWatcher();
            folderWatcher.Show();
        }
    }
}
