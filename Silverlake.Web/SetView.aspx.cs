using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class SetView : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());

        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IBatchLogService> lazyBatchLogServiceObj = new Lazy<IBatchLogService>(() => new BatchLogService());

        public static IBatchLogService IBatchLogService { get { return lazyBatchLogServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                Int32 UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = IUserService.GetSingle(UserId);
                string userType = Session["UserType"].ToString();
                string userRole = Session["UserRole"].ToString();
                string setId = "";

                if (!string.IsNullOrEmpty(Request.QueryString["hdnNumberPerPage"]))
                {
                    hdnNumberPerPage.Value = Request.QueryString["hdnNumberPerPage"].ToString();
                }
                if (Request.QueryString["hdnCurrentPageNo"] != "" && Request.QueryString["hdnCurrentPageNo"] != null)
                {
                    hdnCurrentPageNo.Value = Request.QueryString["hdnCurrentPageNo"].ToString();
                }
                if (Request.QueryString["hdnTotalRecordsCount"] != "" && Request.QueryString["hdnTotalRecordsCount"] != null)
                {
                    hdnTotalRecordsCount.Value = Request.QueryString["hdnTotalRecordsCount"].ToString();
                }

                StringBuilder filter = new StringBuilder();
                filter.Append(" join batches b on b.id = s.batch_id ");
                // filter.Append(" and s.status in (1,9) ");
                if (!string.IsNullOrEmpty(Request.QueryString["setId"]))
                    filter.Append(" and s.ID=" + Request.QueryString["setId"] + " ");

                List<Branch> branchList = IBranchService.GetDataByFilter(" status='1'", 0, 0, false);

                if (userRole == "Regional Admin")
                {
                    List<Branch> Branches = new List<Branch>();
                    if (user.IsAll == 0)
                    {
                        List<BranchUser> userBranches = IBranchUserService.GetDataByFilter(" user_id = '" + user.Id + "' and status='1'", 0, 0, false);
                        branchList = IBranchService.GetDataByFilter(" ID not in (" + String.Join(",", userBranches.Select(x => x.BranchId).ToArray()) + ")", 0, 0, false);
                    }
                }
                if (userRole == "Branch Admin")
                {
                    if (user.BranchId != 0)
                    {
                        Branch branch = IBranchService.GetSingle(user.BranchId);
                        List<Branch> branches = new List<Branch>();
                        branches.Add(branch);
                        branchList = branches;
                    }
                }

                if (userRole == "Regional Admin")
                {
                    filter.Append(" and b.branch_id in (" + String.Join(",", branchList.Select(x => x.Id).ToArray()) + ") ");
                }
                else if (userRole == "Branch Admin")
                {
                    filter.Append(" and b.branch_id in (" + user.BranchId + ") ");
                }

                if (Request.QueryString["IsNewSearch"] != "" && Request.QueryString["IsNewSearch"] != null)
                {
                    IsNewSearch.Value = Request.QueryString["IsNewSearch"].ToString();
                }
                if (IsNewSearch.Value == "1")
                {
                    hdnCurrentPageNo.Value = "";
                }
                if (Request.QueryString["Search"] != "" && Request.QueryString["Search"] != null)
                {
                    //Search.Value = Request.QueryString["Search"].ToString();
                    //bool isNumber = long.TryParse(Search.Value, out long number);
                    //if (isNumber)
                    //    Search.Value = number.ToString();
                    //string columnNameAaNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AaNo));
                    //filter.Append(" and (s." + columnNameAaNo + " like '%" + Search.Value + "%'");
                    //string columnNameAcNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AccountNo));
                    //filter.Append(" or s." + columnNameAcNo + " like '%" + Search.Value + "%'");
                    //string columnNameBatchNo = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchNo));
                    //filter.Append(" or b." + columnNameBatchNo + " like '%" + Search.Value + "%')");
                }


                int skip = 0, take = 10;
                if (hdnCurrentPageNo.Value == "")
                {
                    skip = 0;
                    take = 10;
                    hdnNumberPerPage.Value = "10";
                    hdnCurrentPageNo.Value = "1";
                    hdnTotalRecordsCount.Value = ISetService.GetCountByFilterNew(filter.ToString()).ToString();
                }
                else
                {
                    skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                    take = 10;
                }

                List<Set> objs = ISetService.GetDataByFilterNew(filter.ToString(), skip, take, true);

                StringBuilder asb = new StringBuilder();
                int index = 1;
                foreach (Set set in objs)
                {
                    Batch b = IBatchService.GetSingle(set.BatchId);

                    int setsCount = ISetService.GetCountByFilter(" batch_id='" + b.Id + "'");
                    int scanPagesCount = 0;

                    filter = new StringBuilder();
                    filter.Append(" 1=1");
                    string BatchIdColumnName = Converter.GetColumnNameByPropertyName<BatchLog>(nameof(BatchLog.BatchId));
                    filter.Append(" and " + BatchIdColumnName + " = '" + b.Id + "'");

                    List<BatchLog> batchLogs = IBatchLogService.GetDataByFilter(filter.ToString(), 0, 0, false);
                    StringBuilder batchLogHTML = new StringBuilder();
                    foreach (BatchLog batchLog in batchLogs)
                    {
                        batchLogHTML.Append(@"
                                                    <div class='mini-stat clearfix text-left'>
                                                        " + (batchLog.StageId == 1 ?
                                                @"<span class='mini-stat-icon orange'><i class='fa fa-print'></i></span>" :
                                                (batchLog.StageId == 2 ?
                                                "<span class='mini-stat-icon tar'><i class='fa fa-check-square-o'></i></span>" :
                                                (batchLog.StageId == 3 ?
                                                "<span class='mini-stat-icon pink'><i class='fa fa-external-link'></i></span>" :
                                                (batchLog.StageId == 4 ?
                                                "<span class='mini-stat-icon green'><i class='fa fa-puzzle-piece'></i></span>" :
                                                (batchLog.StageId == 5 ?
                                                "<span class='mini-stat-icon yellow-b'><i class='fa fa-files-o'></i></span>" :
                                                (batchLog.StageId == 6 ?
                                                "<span class='mini-stat-icon yellow-b'><i class='fa fa-hdd-o'></i></span>" : "")))))) + @"
                                                        <div class='mini-stat-info'>
                                                            " + (batchLog.StageId == 1 ?
                                                        "<span>Scan</span>" :
                                                        (batchLog.StageId == 2 ?
                                                        "<span>Index</span>" :
                                                        (batchLog.StageId == 3 ?
                                                        "<span><span>Export</span> <small>To Server</small></span>" :
                                                        (batchLog.StageId == 4 ?
                                                        "<span><span>Integrate</span> <small>Sync Control</small></span>" :
                                                        (batchLog.StageId == 5 ?
                                                        "<span><span>Release</span> <small>To Mimzy</small></span>" :
                                                        (batchLog.StageId == 6 ?
                                                        "<span><span>Document</span> <small>By Mimzy</small></span>" : "")))))) + @"
                                                            " + batchLog.UpdatedDate.ToString("dd/MM/yyyy HH:mm:ss") + @" - <strong>" + batchLog.BatchUser + @"</strong>
                                                        </div>
                                                    </div>
                                ");
                    }
                    BatchLog firstBatchLog = batchLogs.FirstOrDefault();
                    BatchLog lastBatchLog = batchLogs.LastOrDefault();
                    TimeSpan timeSpan = lastBatchLog.UpdatedDate.Subtract(firstBatchLog.UpdatedDate);

                    Branch branch = IBranchService.GetSingle(b.BranchId);
                    Department department = IDepartmentService.GetSingle(b.DepartmentId);
                    Stage stage = IStageService.GetSingle(b.StageId);
                    filter = new StringBuilder();
                    filter.Append(" 1=1");
                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.SetId)) + "='" + set.Id + "'");
                    filter.Append(" and " + Converter.GetColumnNameByPropertyName<SetDocument>(nameof(SetDocument.Status)) + "='1'");
                    List<SetDocument> setDocuments = ISetDocumentService.GetDataByFilter(filter.ToString(), 0, 0, false);
                    StringBuilder setDocumentsHTML = new StringBuilder();
                    StringBuilder setDocumentsPdf = new StringBuilder();



                    string departmentCode = department.Code;
                    string deptCode = departmentCode.Split('-')[0];
                    string jobCode = departmentCode.Split('-')[1];
                    string columnData = "";
                    string accountNumber = "";
                    if (departmentCode == "E-LIBRARY")
                    {
                        //AA NUMBER
                        //ACCOUNT NUMBER
                        columnData = "AA No: " + set.AaNo;
                        columnData += "<br/>Account No: " + set.AccountNo;
                        accountNumber += " <tr><td> AA Number </td><td> " + set.AaNo + " </td></tr>";
                        accountNumber += " <tr><td> Acount Number </td><td> " + set.AccountNo + " </td></tr>";
                    }
                    else if (deptCode == "ETP")
                    {
                        if (jobCode == "LN")
                        {
                            columnData = "AA No: " + set.AaNo;
                            columnData += "<br/>Account No: " + set.AccountNo;
                            accountNumber += " <tr><td> AA Number </td><td> " + set.AaNo + " </td></tr>";
                            accountNumber += " <tr><td> Acount Number </td><td> " + set.AccountNo + " </td></tr>";
                        }
                        else if (jobCode == "LL")
                        {
                            columnData = "AA No: " + set.AaNo;
                            accountNumber += " <tr><td> AA Number </td><td> " + set.AaNo + " </td></tr>";
                        }
                        else if (jobCode == "PR")
                        {
                            columnData = "Project Code: " + set.AaNo;
                            accountNumber += " <tr><td> Project Code </td><td> " + set.AaNo + " </td></tr>";
                        }
                        else if (jobCode == "WF")
                        {
                            columnData = "Welfare Code: " + set.AaNo;
                            accountNumber += " <tr><td> Welfare Code </td><td> " + set.AaNo + " </td></tr>";
                        }
                    }
                    else if (deptCode == "LOS")
                    {
                        columnData = "AA No: " + set.AaNo;
                        accountNumber += " <tr><td> AA Number </td><td> " + set.AaNo + " </td></tr>";
                    }


                    setDocumentsPdf.Append(@"<button class='rotate90' id='metaTag'>Meta Tag</button>
            <button class='rotate90' id='preView'>Preview</button>
            <h5><i class='fa fa-file-pdf-o'></i> <strong>" + department.Code + @" - " + set.AaNo + @"</strong></h5>
            <table id='tblView' class='table table-condensed table-striped'>" + accountNumber + @"
                           <tr><td> Branch </td><td>" + branch.Code + @"</td></tr>
                           <tr><td> Department </td><td>" + department.Code + @"</ td ></ tr >    
                                <tr><td> Date </td><td> " + set.CreatedDate + @" </td></tr>
                                <tr><td> M - File Status </td><td> " + (set.IsReleased == 1 ? "<i class='fa fa-file text-muted'></i>Pending " : (set.IsReleased == 2 && set.Status == 1 ? "<i class='fa fa-file text-success'></i>Mfile update has been Success" : "<i class='fa fa-file text-danger'></i>Mfile update has been Failed")) + @" </td></tr>
                                       <tr><td> Remarks </td><td>" + set.Remarks + @"</td></tr>
                                   </table> ");
                    foreach (SetDocument setDocument in setDocuments)
                    {
                        scanPagesCount += setDocument.PageCount;
                        string fileName = Path.GetFileName(setDocument.DocumentUrl);
                        string localUrl = Server.MapPath("/Content/Files/" + fileName);

                        if (File.Exists(setDocument.DocumentUrl) && !File.Exists(localUrl))
                            File.Copy(setDocument.DocumentUrl, localUrl);
                        setDocumentsHTML.Append(@" <div class='pdf-document'>
                            <a href='javascript:;' id='" + setDocument.DocType + @"' onclick='pdfview(this)'><i class='fa fa-file-pdf-o'></i> " + setDocument.DocType + @" Shown Documents from " + setDocument.UpdatedDate + @"</p>
                        </a> </div>");
                        // setDocumentsHTML.Append(@"<div style='min-width: 150px;'><a href='/Content/Files/" + fileName + "' target='_blank' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + setDocument.DocType + @"</strong></a> ( <small> pages: <strong>" + setDocument.PageCount + @"</strong></small> )</div>");
                        setDocumentsPdf.Append(@"<div class='pdfView' id='pdf-" + setDocument.DocType + @"' style='display: none'> <iframe width='100%' height='100%' src='/Content/Files/" + fileName + @"'> </iframe></div>");
                    }
                    ResultHolder.InnerHtml = setDocumentsHTML.ToString();
                    FileHolder.InnerHtml = setDocumentsPdf.ToString();
                    string xmlFileName = Path.GetFileName(set.SetXmlPath);
                    string localXMLUrl = Server.MapPath("/Content/Files/" + xmlFileName);
                    if (set.SetXmlPath != null && set.SetXmlPath != "")
                    {
                        if (File.Exists(set.SetXmlPath) && !File.Exists(localXMLUrl))
                            File.Copy(set.SetXmlPath, localXMLUrl);

                        asb.Append(@" <div>
                        " + columnData + @"
                       <br /> Branch : " + branch.Code + @" 
                       <br /> Department : " + department.Code + @"
                       <br /> Batch No : " + b.BatchNo + @"
                    </div>");
                        //  PropsHolder.InnerHtml = asb.ToString();
                        //                        asb.Append(@"<tr>
                        //                                    <td class='" + (set.IsReleased == 0 ? "" : "text-success") + @"'>
                        //                                        " + index + @"
                        //                                    </td>
                        //                                    <td>branch Code " + branch.Code + @"<br />Department - " + department.Code + @"<br />User Batch No " + b.BatchNo + @"<br />
                        //AA No " + b.BatchNo + @"<br />
                        //Acount Number " + b.BatchNo + @"<br /></td>
                        //                                    <td><a href='/Content/Files/" + xmlFileName + @"' target='_blank' data-original-title='Click to view XML' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + columnData + @"</strong></a></td>
                        //                                    <td style='text-align:left'>
                        //Created On " + (set.UpdatedDate == null ? set.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") : set.UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")) + @"<br />
                        //                        "+ setDocumentsPdf.ToString() + @" </td>
                        //                                </tr>");
                    }
                    index++;
                }
                PropsHolder.InnerHtml = asb.ToString();
            }
        }
    }
}