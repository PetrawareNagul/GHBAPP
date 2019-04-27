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
    public partial class BranchScanDetail : System.Web.UI.Page
    {
        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());

        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchLogService> lazyBatchLogServiceObj = new Lazy<IBatchLogService>(() => new BatchLogService());

        public static IBatchLogService IBatchLogService { get { return lazyBatchLogServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

        public static String DateInterval = ConfigurationManager.AppSettings["DateInterval"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            string idString = Request.QueryString["id"];
            string stageIdString = Request.QueryString["stageid"];
            string departmentIdString = Request.QueryString["departmentid"];
            string fromDate = Request.QueryString["fromdate"];
            string toDate = Request.QueryString["todate"];
            if (!string.IsNullOrEmpty(idString))
            {
                int branchId = Convert.ToInt32(idString);
                Branch branch = IBranchService.GetSingle(branchId);
                branchName.InnerHtml = branch.Name;

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
                StringBuilder filter1 = new StringBuilder();

                filter.Append(" 1=1");
                filter.Append(" and (status='1' or status='9')");

                string branchIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BranchId));
                filter.Append(" and " + branchIdColumnName + "='" + branch.Id + "'");

                if (!string.IsNullOrEmpty(stageIdString))
                {
                    int stageId = Convert.ToInt32(stageIdString);
                    Stage stage = IStageService.GetSingle(stageId);

                    string stageIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId));
                    filter.Append(" and " + stageIdColumnName + "='" + stage.Id + "'");

                    if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                        if (stage.Id >= 5)
                    {
                        string createdDateColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.CreatedDate));
                        //filter.Append(" and DATE_FORMAT(DATE_SUB(" + createdDateColumnName + ", INTERVAL 0 YEAR),'%Y-%m-%d') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                        //SQL
                        filter.Append(" and CAST(DATEADD(year, " + DateInterval + ", " + createdDateColumnName + ") as DATE) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                        //MYSQL
                        //filter.Append(" and DATE_FORMAT(DATE_SUB(" + createdDateColumnName + ", INTERVAL " + DateInterval + " YEAR),'%Y-%m-%d') = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                }
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    filter.Append(" and created_date between '" + fromDate + "' and  '" + toDate + "' ");

                if (!string.IsNullOrEmpty(departmentIdString))
                {
                    int departmentId = Convert.ToInt32(departmentIdString);
                    Department department = IDepartmentService.GetSingle(departmentId);

                    string departmentIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.DepartmentId));
                    filter.Append(" and " + departmentIdColumnName + "='" + department.Id + "'");
                }
                // New code fro paging
                int skip = 0, take = 10;
                if (hdnCurrentPageNo.Value == "")
                {
                    skip = 0;
                    take = 10;
                    hdnNumberPerPage.Value = "10";
                    hdnCurrentPageNo.Value = "1";
                    hdnTotalRecordsCount.Value = IBatchService.GetCountByFilter(filter.ToString()).ToString();
                }
                else
                {
                    if (hdnCurrentPageNo.Value.Contains(","))
                    {
                        hdnCurrentPageNo.Value = hdnCurrentPageNo.Value.Split(',').Last();
                    }
                    skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                    take = 10;
                }

                List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), skip, take, true);
                List<BatchLog> batchLogs = new List<BatchLog>();

                List<int> batchIdList = batches.Select(a => a.Id).ToList();
                if (batchIdList != null && batchIdList.Count != 0)
                {
                    filter = new StringBuilder();
                    filter.Append(" 1=1");
                    string BatchIdColumnName = Converter.GetColumnNameByPropertyName<BatchLog>(nameof(BatchLog.BatchId));
                    List<string> branchIds = batches.Select(x => x.Id.ToString()).ToList<string>();
                    filter.Append(" and " + BatchIdColumnName + " in (" + String.Join(",", branchIds.ToArray()) + ")");
                    batchLogs = IBatchLogService.GetDataByFilter(filter.ToString(), 0, 0, false);
                }
                //End New code


                StringBuilder innerHTML = new StringBuilder();
                var groupByDepartments = batches.Where(x => x.BranchId == branch.Id).ToList().GroupBy(x => x.DepartmentId).Select(grp => grp.ToList()).ToList();
                if (groupByDepartments.Count > 0)
                    foreach (List<Batch> deptBatches in groupByDepartments)
                    {
                        Department department = IDepartmentService.GetSingle(deptBatches.FirstOrDefault().DepartmentId);
                        filter = new StringBuilder();
                        filter.Append(" 1=1");
                        //List<string> batchIds = deptBatches.Select(x => x.Id.ToString()).ToList<string>();
                        //string BatchIdColumnName = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.BatchId));
                        //filter.Append(" and " + BatchIdColumnName + " in (" + String.Join(",", batchIds.ToArray()) + ")");

                        StringBuilder batchesHTML = new StringBuilder();
                        foreach (Batch batch in deptBatches)
                        {

                            filter = new StringBuilder();
                            filter.Append(" 1=1");
                            string BatchIdColumnName = Converter.GetColumnNameByPropertyName<BatchLog>(nameof(BatchLog.BatchId));
                            filter.Append(" and " + BatchIdColumnName + " = '" + batch.Id + "'");

                            //  batchLogs = IBatchLogService.GetDataByFilter(filter.ToString(), 0, 0, false);

                            filter = new StringBuilder();
                            filter.Append(" 1=1");
                            BatchIdColumnName = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.BatchId));
                            filter.Append(" and " + BatchIdColumnName + " = '" + batch.Id + "'");
                            filter.Append(" and status in (1, 9)");

                            List<Set> sets = ISetService.GetDataByFilter(filter.ToString(), 0, 0, false);
                            StringBuilder setsHTML = new StringBuilder();
                            setsHTML.Append("<table cellspacing='0' width='100%'>");
                            setsHTML.Append("<thead><tr><th>Code No</th><th>Documents</th></tr></thead>");
                            setsHTML.Append("<tbody>");
                            int documentsCount = 0;
                            int setsCount = 0;
                            int scanPagesCount = 0;
                            if (sets.Count > 0)
                            {
                                foreach (Set set in sets)
                                {
                                    List<SetDocument> setDocuments = ISetDocumentService.GetDataByPropertyName(nameof(SetDocument.SetId), set.Id.ToString(), true, 0, 0, false);
                                    setDocuments = setDocuments.Where(x => x.Status == 1).ToList();
                                    documentsCount += setDocuments.Count;
                                    StringBuilder setDocumentsHTML = new StringBuilder();
                                    foreach (SetDocument setDocument in setDocuments)
                                    {
                                        scanPagesCount += setDocument.PageCount;
                                        //string NetworkPath = ConfigurationManager.AppSettings["NetworkPath"].ToString();
                                        //string NetworkDrive = ConfigurationManager.AppSettings["NetworkDrive"].ToString();
                                        //string mapLocalUrl = setDocument.DocumentUrl.Replace(NetworkPath, NetworkDrive);

                                        //string fileName = Path.GetFileName(setDocument.DocumentUrl);
                                        //string localUrl = Server.MapPath("/Content/Files/" + fileName);
                                        //if (File.Exists(setDocument.DocumentUrl) && !File.Exists(localUrl))
                                        //{
                                        //    File.Copy(setDocument.DocumentUrl, localUrl);
                                        //}
                                        // setDocumentsHTML.Append(@"<div><a href='/Content/Files/" + fileName + "' target='_blank' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + setDocument.DocType + @"</strong></a> ( <small> pages: <strong>" + setDocument.PageCount + @"</strong></small> )</div>");
                                        setDocumentsHTML.Append(@"<div><a href='#' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + setDocument.DocType + @"</strong></a> ( <small> pages: <strong>" + setDocument.PageCount + @"</strong></small> )</div>");

                                    }
                                    string xmlFileName = Path.GetFileName(set.SetXmlPath);
                                    string localXMLUrl = Server.MapPath("/Content/Files/" + xmlFileName);
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
                                        if (jobCode == "LN")
                                        {
                                            columnData = "AA No: " + set.AaNo;
                                            columnData += "<br/>Account No: " + set.AccountNo;
                                        }
                                        else if (jobCode == "LL")
                                        {
                                            columnData = "AA No: " + set.AaNo;
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
                                    if (!string.IsNullOrEmpty(set.SetXmlPath))
                                    {
                                        setsCount++;
                                        //if (File.Exists(set.SetXmlPath) && !File.Exists(localXMLUrl))
                                        //    File.Copy(set.SetXmlPath, localXMLUrl);
                                        //setsHTML.Append(@"
                                        //    <tr>
                                        //        <td><a href='/Content/Files/" + xmlFileName + @"' target='_blank' data-original-title='Click to view XML' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + columnData + @"</strong></a></td>
                                        //        <td>" + setDocumentsHTML.ToString() + @"</td>
                                        //    </tr>
                                        //");


                                        setsHTML.Append(@"
                                            <tr>
                                                <td><a href='SetView.aspx?setId=" + set.Id + @"' target='_blank' data-original-title='Click to view Files' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + columnData + @" <i class='fa fa-eye'></i></strong></a>
                                            </td>
                                                <td>" + setDocumentsHTML.ToString() + @"</td>
                                            </tr>
                                        ");

                                    }
                                }
                            }
                            setsHTML.Append("</tbody>");
                            setsHTML.Append("</table>");

                            StringBuilder batchLogHTML = new StringBuilder();
                            foreach (BatchLog batchLog in batchLogs.Where(a => a.BatchId == batch.Id).ToList())
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
                                                        "<span class='mini-stat-icon yellow-b'><i class='fa fa-hdd-o'></i></span>" :
                                                        "")))))) + @"
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

                            int isReleased = 0;
                            int isException = 0;
                            string mFIleStatus = "";
                            int isSetCount = sets.Count();
                            var releasedStatus2 = sets.Where(x => x.IsReleased == 2).ToList();
                            if (releasedStatus2 == null)
                            {
                                isReleased = 0;
                            }
                            else if (sets.Count != releasedStatus2.Count)
                            {
                                isReleased = 1;
                            }
                            else if (sets.Count == releasedStatus2.Count)
                            {
                                isReleased = 2;
                                if (releasedStatus2.Count(x => x.Status == 9) == 0)
                                {
                                    isException = 0;
                                }
                                else if (releasedStatus2.Count(x => x.Status == 9) != sets.Count)
                                {
                                    isException = 1;
                                }
                                else if (releasedStatus2.Count(x => x.Status == 9) == sets.Count)
                                {
                                    isException = 2;
                                }
                            }

                            batchesHTML.Append(@"
                                <tr>
                                    <td>
                                        <div class='text-center mb-5'>
                                            " + branch.Code + @" : <strong class='" + (batch.Status == 9 ? "text-danger" : "") + @"'>" + batch.BatchNo + @"</strong>
                                            <a href='javascript:;' class='text-info view_batch_log'><sup>View Log</sup></a>
                                            <div class='div_batch_log hide draggableDiv'>
                                                <span class='log_close'>X</span>
                                                <table>
                                                    <tr><td colspan='2' class='text-center'><strong>Batch log</strong></td></tr>
                                                    <tr><td>Batch No: <strong>" + batch.BatchNo + @"</strong></td><td>No. of sets: <strong>" + setsCount + @"</strong></td></tr>
                                                    <tr><td colspan='2'><small>Duration: <label class='label label-primary'>" + timeSpan.Days + @" days, " + timeSpan.Hours + @" hours, " + timeSpan.Minutes + @" minutes, " + timeSpan.Seconds + @" seconds</label></small></tr>
                                                </table>
                                                " + batchLogHTML.ToString() + @"
                                                 <div>
                                                    Total scan pages: <strong>" + scanPagesCount + @"</strong>
                                                </div>");

                            if (isSetCount != 0)
                            {

                                batchesHTML.Append(@"<div>
                                                    M - Files Status: <span clas = 'fs-14'> " + (isReleased == 0 || isReleased == 1 ? " <i class='fa fa-file text-muted'></i>" : (isReleased == 2 && isException == 0 ? "<i class='fa fa-file text-success'></i>" : (isReleased == 2 && isException == 1 ? "<i class='fa fa-file text-warning'></i>" : (isReleased == 2 && isException == 2 ? "<i class='fa fa-file text-danger'></i>" : "")))) + @"</span>
                                                </div>");
                            }
                            batchesHTML.Append(@" </div>
                                        </div>
                                    </td>
                                    <td>
                                        " + batch.BatchUser + @"
                                    </td>
                                    <td>
                                        " + batch.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") + @"
                                    </td>
                                    <td>
                                        " + setsHTML.ToString() + @"
                                        </td>
                                </tr>
                            ");
                        }

                        innerHTML.Append(@"
                        <section class='panel my-panel'>
                            <header class='panel-heading'>
                                Department: <strong>" + department.Name + @"</strong>
                                <span class='tools pull-right'>
                                    <a href='javascript:;' class='fa fa-chevron-up panel-oc'></a>
                                </span>
                            </header>
                            <div class='panel-body' style='display: none;'>
                                <table class='display responsive nowrap table table-bordered dataTable' cellspacing='0' width='100%'>
                                    <thead>
                                        <tr>
                                            <th>Batch No</th>
                                            <th>Batch user</th>
                                            <th>Batch date</th>
                                            <th>Sets</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        " + batchesHTML + @"
                                    </tbody>
                                </table>
                            </div>
                        </section>");
                    }
                panelBatches.InnerHtml = innerHTML.ToString();
            }
        }
    }
}