using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class AccountEnquiry : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //string SearchText = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["IsNewSearch"]))
                IsNewSearch.Value = Request.QueryString["IsNewSearch"].ToString();

            divError.Visible = false;
            divResult.Visible = false;
            int departmentId = 0;
            List<Department> departmentList = IDepartmentService.GetData(0, 0, false);
            DepartmentId.DataSource = departmentList;
            DepartmentId.DataTextField = "Name";
            DepartmentId.DataValueField = "Id";
            DepartmentId.DataBind();
            DepartmentId.Items.Insert(0, new ListItem() { Text = "Select Department", Value = "" });
            DocType.Items.Insert(0, new ListItem() { Text = "Select Doc Type", Value = "" });
            if (!string.IsNullOrEmpty(Request.QueryString["DepartmentId"]) && int.TryParse(Request.QueryString["DepartmentId"], out int n))
            {
                DepartmentId.SelectedValue = Request.QueryString["DepartmentId"];
                departmentId = Convert.ToInt32(Request.QueryString["DepartmentId"]);
                List<DocTypeModel> docTypes = IDepartmentService.GetDocTypesByDepartmentId(departmentId);
                DocType.DataSource = docTypes;
                DocType.DataTextField = "DocType";
                DocType.DataValueField = "DocType";
                DocType.DataBind();
                if (!string.IsNullOrEmpty(Request.QueryString["DocType"]))
                    DocType.SelectedValue = Request.QueryString["DocType"];
            }
            else
            {
          
            }
            //if (!string.IsNullOrEmpty(Request.QueryString["DocType"]))
            //    DocType.SelectedValue = Request.QueryString["DocType"];

            if (departmentId != 0 || !string.IsNullOrEmpty(Request.QueryString["DocType"]) || !string.IsNullOrEmpty(Request.QueryString["Search"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
                    Search.Value = Request.QueryString["Search"];

                if (!string.IsNullOrEmpty(Request.QueryString["hdnNumberPerPage"]))
                    hdnNumberPerPage.Value = Request.QueryString["hdnNumberPerPage"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["hdnCurrentPageNo"]))
                    hdnCurrentPageNo.Value = Request.QueryString["hdnCurrentPageNo"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["hdnTotalRecordsCount"]))
                    hdnTotalRecordsCount.Value = Request.QueryString["hdnTotalRecordsCount"].ToString();

                int skip = 0, take = 10;
                if (IsNewSearch.Value != "0")
                {
                    skip = 0;
                    take = 10;
                    hdnNumberPerPage.Value = "10";
                    hdnCurrentPageNo.Value = "1";
                    hdnTotalRecordsCount.Value = ISetService.GetSetsForMfilesAccountCount(departmentId, Request.QueryString["DocType"], Request.QueryString["Search"]).ToString();
                }
                else
                {
                    skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                    take = 10;
                }

                List<DocTypeSetModel> list = new List<DocTypeSetModel>();
                if (departmentId != 0 || !string.IsNullOrEmpty(Request.QueryString["DocType"]) || !string.IsNullOrEmpty(Request.QueryString["Search"]))
                    list = ISetService.GetSetsForMfilesAccount(departmentId, Request.QueryString["DocType"], Request.QueryString["Search"], skip, take);

                StringBuilder asb = new StringBuilder();
                int index = 1;
                foreach (DocTypeSetModel set in list)
                {
                    Branch branch = IBranchService.GetSingle(set.BranchId);
                    Department department = departmentList.FirstOrDefault(a => a.Id == set.DepartmentId);
                    string departmentCode = department.Code;
                    string deptCode = departmentCode.Split('-')[0];
                    string jobCode = departmentCode.Split('-')[1];
                    string columnData = "";
                    if (departmentCode == "E-LIBRARY")
                    {
                        //AA NUMBER
                        //ACCOUNT NUMBER
                        columnData = "AA No: " + set.AANO;
                        columnData += "<br/>Account No: " + set.AccountNo;
                    }
                    else if (deptCode == "ETP")
                    {
                        if (jobCode == "LN")
                        {
                            columnData = "AA No: " + set.AANO;
                            columnData += "<br/>Account No: " + set.AccountNo;
                        }
                        else if (jobCode == "LL")
                        {
                            columnData = "AA No: " + set.AANO;
                        }
                        else if (jobCode == "PR")
                        {
                            columnData = "Project Code: " + set.AANO;
                        }
                        else if (jobCode == "WF")
                        {
                            columnData = "Welfare Code: " + set.AANO;
                        }
                    }
                    else if (deptCode == "LOS")
                    {
                        columnData = "AA No: " + set.AANO;
                    }

                    asb.Append(@"<tr>
                                    <td class=''>
                                        " + index + @"
                                    </td>
                                    <td>" + branch.Code + @"</td>
                                    <td>" + department.Code + @"</td>
                                    <td class='text-center mb-5'>
                                        " + set.BatchNo + @"
                                    </td>
                                    <td>
                                        <div><strong>" + columnData + @"</strong><div>
                                    </td>
                                    <td><div style='min-width: 150px;'><a href='#' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + set.DocType + @"</strong></a> ( <small> pages: <strong>" + set.PageCount + @"</strong></small> )</div></td>
                                    <td>" + MfileStatus(set.IsReleased) + @"</td>
                                    <td><a data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info' href='SetView.aspx?setId=" + set.SetId + @"' target='_blank'><i class='fa fa-eye'></i></a></td>
                                    <td>" + set.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
                                </tr>");
                    //  }
                    index++;
                }

                string result = @"<button data-dismiss='alert' class='close close-sm' type='button'>
                                            <i class='fa fa-times'></i></button>
                                        <strong>Done!</strong> Found the " + hdnTotalRecordsCount.Value + " records with .";
                if (departmentId != 0)
                    result += DepartmentId.SelectedItem.Text + " Department &";
                if (!string.IsNullOrEmpty(Request.QueryString["DocType"]))
                    result += Request.QueryString["DocType"] + " Doc Type &";
                if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
                    result += "this " + Request.QueryString["Search"] + " text.";
                divResult.Visible = true;
                divResult.InnerHtml = result;

                setsTbody.InnerHtml = asb.ToString();
            }
            else
            {
                if(IsNewSearch.Value == "1")
                divError.Visible = true;
                divError.InnerHtml = @"<button data-dismiss='alert' class='close close-sm' type='button'>
                                            <i class='fa fa-times'></i></button>
                                        <strong>Oh snap!</strong> Please select at lest one select option.";
            }

        }
        public string MfileStatus(int id)
        {
            switch(id)
            {
                case 0:
                    return "<span class='label label-danger'>Error</span>";
                case 1:
                    return "<span class='label label-default'>In Process</span>";
                case 2:
                    return "<span class='label label-success'>Success</span>";
                case 9:
                    return "<span class='label label-danger'>Error</span>";
            }
            return "";
        }


        [System.Web.Services.WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object BindDocType(int Id)
        {
            List<DocTypeModel> docTypes = IDepartmentService.GetDocTypesByDepartmentId(Id);
            return docTypes;
        }

    }
}