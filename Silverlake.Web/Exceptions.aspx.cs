using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class Exceptions : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["hdnNumberPerPage"] != "" && Request.QueryString["hdnNumberPerPage"] != null)
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
            filter.Append(" 1=1 ");

            string columnNameIsReleased = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.IsReleased));
            filter.Append(" and " + columnNameIsReleased + " in (0, 2)");
            string columnNameRemarks = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.Remarks));
            filter.Append(" and " + columnNameRemarks + " != ''");

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
                Search.Value = Request.QueryString["Search"].ToString();
                string columnNameAaNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AaNo));
                filter.Append(" and (" + columnNameAaNo + " like '%" + Search.Value + "%'");
                string columnNameAcNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AccountNo));
                filter.Append(" or " + columnNameAcNo + " like '%" + Search.Value + "%')");
            }
            string fromdate = ""; string toDate = "";
            if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
            {
                fromdate = Request.QueryString["FromDate"];
                FromDate.Value = fromdate;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
            {
                toDate = Request.QueryString["ToDate"];
                ToDate.Value = toDate;
            }
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
                filter.Append("  and created_date between '" + fromdate + "' and  '" + toDate + "'");

            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = ISetService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<Set> objs = ISetService.GetDataByFilter(filter.ToString(), skip, take, true);

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
                                                    "<span>Export <small>To Server</small></span>" :
                                                    (batchLog.StageId == 4 ?
                                                    "<span>Integrate <small>Sync Control</small></span>" :
                                                    (batchLog.StageId == 5 ?
                                                    "<span>Release <small>To Mimzy</small></span>" :
                                                    (batchLog.StageId == 6 ?
                                                    "<span>Document <small>By Mimzy</small></span>" : "")))))) + @"
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
                foreach (SetDocument setDocument in setDocuments)
                {
                    scanPagesCount += setDocument.PageCount;
                    string fileName = Path.GetFileName(setDocument.DocumentUrl);
                    string localUrl = Server.MapPath("/Content/Files/" + fileName);
                    //if (!File.Exists(localUrl))
                    //    File.Copy(setDocument.DocumentUrl, localUrl);
                    setDocumentsHTML.Append(@"<div style='min-width: 150px;'><a href='/Content/Files/" + fileName + "' target='_blank' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + setDocument.DocType + @"</strong></a> ( <small> pages: <strong>" + setDocument.PageCount + @"</strong></small> )</div>");
                }
                string xmlFileName = Path.GetFileName(set.SetXmlPath);
                string localXMLUrl = Server.MapPath("/Content/Files/" + xmlFileName);
                if (set.SetXmlPath != null && set.SetXmlPath != "")
                {
                    //if (!File.Exists(localXMLUrl))
                    //    File.Copy(set.SetXmlPath, localXMLUrl);
                    string departmentCode = department.Code;
                    string deptCode = departmentCode.Split('-')[0];
                    string jobCode = departmentCode.Split('-')[1];
                    string columnData = "";
                    if (departmentCode == "E-LIBRARY")
                    {
                        //AA NUMBER
                        //ACCOUNT NUMBER
                        columnData = "AA No: " + set.AaNo;
                        columnData += "<br/>Account No: " + set.AccountNo;
                    }
                    else if (deptCode == "ETP")
                    {
                        if (jobCode == "LL")
                        {
                            columnData = "AA No: " + set.AaNo;
                        }
                        else if (jobCode == "LN")
                        {
                            columnData = "AA No: " + set.AaNo;
                            columnData += "<br/>Account No: " + set.AccountNo;
                        }
                        else if (jobCode == "PR")
                        {
                            columnData = "Project Code: " + set.AaNo;
                        }
                        else if (jobCode == "WF")
                        {
                            columnData = "Welfare Code: " + set.AaNo;
                        }
                    }
                    else if (deptCode == "LOS")
                    {
                        columnData = "AA No: " + set.AaNo;
                    }

                    asb.Append(@"<tr>
                                     <td class='icheck'>
                                        <div class='square single-row'>
                                            <div class='checkbox'>
                                                <input type='checkbox' name='checkRow' class='checkRow' value='" + b.Id + @"' /> <label>" + index + @"</label><br/>
                                            </div>
                                        </div>
                                    </td>
                                    <td>" + branch.Code + @"</td>
                                    <td>" + department.Code + @"</td>
                                    <td class='text-center mb-5'>
                                        <span>" + b.BatchNo + @" 
                                        <a href='javascript:;' class='text-info view_batch_log'><sup>View Log</sup></a>
                                        <div class='div_batch_log hide draggableDiv'>
                                            <span class='log_close'>X</span>
                                            <table>
                                                <tr><td>Batch No: <strong>" + b.BatchNo + @"</strong></td><td>No. of sets: <strong>" + setsCount + @"</strong></td></tr>
                                                <tr><td colspan='2'><small>Duration: <label class='label label-primary'>" + timeSpan.Days + @" days, " + timeSpan.Hours + @" hours, " + timeSpan.Minutes + @" minutes, " + timeSpan.Seconds + @" seconds</label></small></tr>
                                            </table>
                                            " + batchLogHTML.ToString() + @"
                                        </div>
                                    <span> " + b.UpdatedDate + @"</span>
                                    </td>
                                    <td><a href='/Content/Files/" + xmlFileName + @"' target='_blank' data-original-title='Click to view XML' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + columnData + @"</strong></a></td>
                                    <td>" + setDocumentsHTML.ToString() + @"</td>
                                    <td>" + set.Remarks + @"</td>
                                </tr>");
                }
                index++;
            }
            exceptionSetsTbody.InnerHtml = asb.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Action(Int32[] ids, String action, String rejectReason = "")
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                string idString = String.Join(",", ids);
                List<Batch> batches = IBatchService.GetDataByFilter(" ID in (" + idString + ")", 0, 0, false);
                if (action == "Repost")
                {
                    batches.ForEach(x =>
                    {
                        x.StageId = 5;
                        x.Status = 1;
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedDate = DateTime.Now;
                    });
                    IBatchService.UpdateBulkData(batches);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("User accounts action: " + ex.Message);
                return false;
            }
        }
    }
}