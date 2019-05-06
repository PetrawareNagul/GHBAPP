using OfficeOpenXml;
using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class GetExcel : System.Web.UI.Page
    {

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());
        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());
        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());
        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());
        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());
        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

        public string totalApplication = "";
        public string path = ConfigurationManager.AppSettings["SANDrive"].ToString();

        public string barChatQuery = string.Empty;
        public string pieChatQuery = string.Empty;
        public string totalDepartmentsQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //string SearchText = string.Empty;
            int departmentId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["DepartmentId"]) && int.TryParse(Request.QueryString["DepartmentId"], out int n))
                departmentId = Convert.ToInt32(Request.QueryString["DepartmentId"]);
            //if (!string.IsNullOrEmpty(Request.QueryString["DocType"]))
            //    DocType.SelectedValue = Request.QueryString["DocType"];

            if (departmentId != 0 || !string.IsNullOrEmpty(Request.QueryString["DocType"]) || !string.IsNullOrEmpty(Request.QueryString["Search"]))
            {
                List<DocTypeSetModel> list = ISetService.GetSetsForMfilesAccount(departmentId, Request.QueryString["DocType"].ToString(), Request.QueryString["Search"].ToString(), 0, 0);
                if (list != null && list.Count != 0)
                {
                    StringBuilder asb = new StringBuilder();
                    int index = 1;
                    ExcelPackage pck = new ExcelPackage();

                    var ws = pck.Workbook.Worksheets.Add("Sample1");

                    ws.Cells["A" + index].Value = "S.No";
                    ws.Cells["B" + index].Value = "Branch";
                    ws.Cells["C" + index].Value = "Batch No";
                    ws.Cells["D" + index].Value = "Department";
                    ws.Cells["E" + index].Value = "Document Type";
                    ws.Cells["F" + index].Value = "AA NO/Project Code/Welfare Code";
                    ws.Cells["G" + index].Value = "Acount NO";
                    ws.Cells["H" + index].Value = "Total Pages";
                    ws.Cells["I" + index].Value = "Status";
                    ws.Cells["J" + index].Value = "User";
                    ws.Cells["K" + index].Value = "Created On";
                    ws.Cells["L" + index].Value = "Updated On";
                    index++;
                    int i = 1;
                    List<Department> departmentList = IDepartmentService.GetData(0, 0, false);
                    List<Branch> branchList = IBranchService.GetData(0, 0, false);
                    foreach (DocTypeSetModel set in list)
                    {
                        Department department = departmentList.FirstOrDefault(a => a.Id == set.DepartmentId);
                        ws.Cells["A" + index].Value = i;
                        ws.Cells["B" + index].Value = branchList.Count(a => a.Id == set.BatchId) != 0 ? branchList.FirstOrDefault(a => a.Id == set.BatchId).Code : "";
                        ws.Cells["C" + index].Value = set.BatchNo;
                        ws.Cells["D" + index].Value = departmentList.Count(a => a.Id == set.DepartmentId) != 0 ? departmentList.FirstOrDefault(a => a.Id == set.DepartmentId).Code : "";
                        ws.Cells["E" + index].Value = set.DocType;
                        ws.Cells["F" + index].Value = set.AANO;
                        ws.Cells["G" + index].Value = set.AccountNo;
                        ws.Cells["H" + index].Value = set.PageCount;
                        ws.Cells["I" + index].Value = MfileStatus(set.IsReleased);
                        ws.Cells["J" + index].Value = set.BatchUser;
                        ws.Cells["K" + index].Value = set.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                        ws.Cells["L" + index].Value = set.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                        i++;
                        index++;
                    }
                    pck.SaveAs(Response.OutputStream);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Sample1.xlsx");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);

                    Response.End();
                }
            }

        }
        public string MfileStatus(int id)
        {
            switch (id)
            {
                case 0:
                    return "Error";
                case 1:
                    return "Pending";
                case 2:
                    return "Success";
                case 9:
                    return "Error";
            }
            return "";
        }

    }
}