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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private static readonly Lazy<IUserRoleService> lazyUserRoleServiceObj = new Lazy<IUserRoleService>(() => new UserRoleService());

        public static IUserRoleService IUserRoleService { get { return lazyUserRoleServiceObj.Value; } }

        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        public static String DateInterval = ConfigurationManager.AppSettings["DateInterval"].ToString();

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
                string fromDate = Request.QueryString["FromDate"];
                string toDate = Request.QueryString["ToDate"];
                //
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

                BranchId.DataSource = branchList;
                BranchId.DataTextField = "Name";
                BranchId.DataValueField = "Id";
                BranchId.DataBind();

                if (userRole == "Branch Admin" && user.BranchId != 0)
                {
                    BranchId.Disabled = true;
                    BranchId.Value = user.BranchId.ToString();
                }

                BranchId.Items.Insert(0, new ListItem() { Text = "Select All", Value = "" });

                if (!string.IsNullOrEmpty(Request.QueryString["IsNewSearch"]))
                    IsNewSearch.Value = Request.QueryString["IsNewSearch"].ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["IsNewSort"]))
                    IsNewSort.Value = Request.QueryString["IsNewSort"].ToString();

                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1 ");

                if (userRole == "Regional Admin")
                {
                    filter.Append(" and branch_id in (" + String.Join(",", branchList.Select(x => x.Id).ToArray()) + ") ");
                }
                else if (userRole == "Branch Admin")
                {
                    filter.Append(" and branch_id in (" + user.BranchId + ") ");
                }
                List<Branch> BranchListSearchResult = new List<Branch>();
                List<Department> DepartmentListSearchResult = new List<Department>();
                if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
                {
                    Search.Value = Request.QueryString["Search"].ToString();
                    bool isNumber = long.TryParse(Search.Value, out long number);
                    if (isNumber)
                        Search.Value = number.ToString();
                    StringBuilder subFilter = new StringBuilder();
                    subFilter.Append(" 1=1 ");
                    string columnNameAaNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AaNo));
                    subFilter.Append(" and " + columnNameAaNo + " like '%" + Search.Value + "%'");
                    string columnNameAcNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AccountNo));
                    subFilter.Append(" or " + columnNameAcNo + " like '%" + Search.Value + "%'");

                    List<Set> setMatches = ISetService.GetDataByFilter(subFilter.ToString(), 0, 0, false);

                    List<string> batchIds = setMatches.Select(x => x.BatchId.ToString()).ToList<string>();
                    subFilter = new StringBuilder();
                    subFilter.Append(" 1=1 ");
                    string columnNameBatchId = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Id));
                    subFilter.Append(" and " + columnNameBatchId + " in (" + String.Join(",", batchIds.ToArray()) + ")");
                    List<Batch> batchMatches = IBatchService.GetDataByFilter(subFilter.ToString(), 0, 0, false);

                    List<string> branchIds = batchMatches.Select(x => x.BranchId.ToString()).ToList<string>();

                    subFilter = new StringBuilder();
                    subFilter.Append(" 1=1 ");
                    string columnNameBranchId = Converter.GetColumnNameByPropertyName<Branch>(nameof(Branch.Id));
                    subFilter.Append(" and " + columnNameBranchId + " in (" + String.Join(",", branchIds.ToArray()) + ")");
                    BranchListSearchResult = IBranchService.GetDataByFilter(subFilter.ToString(), 0, 0, false);
                    if (BranchListSearchResult.Count == 0)
                        searchMessage.InnerHtml = "<small class='text-danger'>No records</small>";
                    else
                    {
                        foreach(Branch bc in BranchListSearchResult)
                            foreach(Batch bt in batchMatches)
                                if(bc.Id == bt.BranchId)
                                {
                                    if (bc.BranchIdBatches == null)
                                        bc.BranchIdBatches = new List<Batch>();
                                    bc.BranchIdBatches.Add(bt);
                                }
                        searchMessage.InnerHtml = "<small class='text-success'>" + batchMatches.Count + " batch(es) in " + BranchListSearchResult.Count + " branch(es) found.</small>";
                    }
                }
                if (Request.QueryString["BranchId"] != "" && Request.QueryString["BranchId"] != null)
                {
                    BranchId.Value = Request.QueryString["BranchId"].ToString();
                    if (BranchListSearchResult.Count(a => a.Id == Convert.ToInt32(BranchId.Value)) == 0)
                    {
                        Branch selectedBranch = IBranchService.GetSingle(Convert.ToInt32(BranchId.Value));
                        BranchListSearchResult.Add(selectedBranch);
                    }
                }
                if (Request.QueryString["SortBy"] != "" && Request.QueryString["SortBy"] != null)
                {
                    SortBy.Value = Request.QueryString["SortBy"].ToString();
                }
                string stageIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId));
                filter.Append(" and " + stageIdColumnName + " < 5 ");
                filter.Append(" and status = '1' ");
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    filter.Append(" and created_date between '" + fromDate + "' and  '" + toDate + "' ");
                // old code
                //  List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);
                // New method
                List<BatchesStagesCount> batchesCount = new List<BatchesStagesCount>();
                batchesCount = IBatchService.GetBatchStagesCount(filter.ToString(), 0, 0, true);
                filter = new StringBuilder();
                filter.Append(" 1=1 ");
                filter.Append(" and (" + stageIdColumnName + " = 5 or " + stageIdColumnName + " = 6) ");
                string createdDateColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.CreatedDate));

                //SQL
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    filter.Append(" and created_date between '" + fromDate + "' and  '" + toDate + "' ");
                    FromDate.Value = fromDate;
                    ToDate.Value = toDate;
                }
                else
                    filter.Append(" and CAST(DATEADD(year, " + DateInterval + ", " + createdDateColumnName + ") as DATE) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                filter.Append(" and (status = '1' or status = '9') ");
                //filter.Append(" and (status = '1' ) ");


                List<BatchesStagesCount> batchesserverstageCount = new List<BatchesStagesCount>();
                if (BranchListSearchResult.Count != 0 && BranchListSearchResult.FirstOrDefault().BranchIdBatches != null && BranchListSearchResult.FirstOrDefault().BranchIdBatches.Count != 0)
                    foreach (Branch bt in BranchListSearchResult)
                    {
                        foreach (Batch bc in bt.BranchIdBatches)
                        {
                            BatchesStagesCount bsc = new BatchesStagesCount();
                            bsc.BranchId = bc.BranchId;
                            bsc.DepartmentId = bc.DepartmentId;
                            bsc.SetCount = 1;
                            bsc.count = 1;
                            bsc.StageId = bc.StageId;
                            batchesserverstageCount.Add(bsc);
                        }
                    }
                else
                    batchesserverstageCount = IBatchService.GetBatchStagesCount(filter.ToString(), 0, 0, true);

                //  List<Batch> batchesCompletedToday = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);

                //old code
                //int stage1Count = batches.Where(x => x.StageId == 1).ToList().Count;
                //int stage2Count = batches.Where(x => x.StageId == 2).ToList().Count;
                //int stage3Count = batches.Where(x => x.StageId == 3).ToList().Count;
                //int stage4Count = batches.Where(x => x.StageId == 4).ToList().Count;
                //int stage5Count = batchesCompletedToday.Where(x => x.StageId == 5).ToList().Count;
                //int stage6Count = batchesCompletedToday.Where(x => x.StageId == 6).ToList().Count;
                // New code
                int stage1Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Scan) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Scan).Sum(a => a.count) : 0;
                int stage2Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Index) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Index).Sum(a => a.count) : 0;
                int stage3Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Export) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Export).Sum(a => a.count) : 0;
                int stage4Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Integrate) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Integrate).Sum(a => a.count) : 0;
                int stage5Count = batchesserverstageCount.Count(a => a.StageId == (int)BatchesStages.Release) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Release).Sum(a => a.count) : 0;
                int stage6Count = batchesserverstageCount.Count(a => a.StageId == (int)BatchesStages.Document) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Document).Sum(a => a.count) : 0;
                StringBuilder stringBuilder = new StringBuilder();

                if (BranchListSearchResult.Count == 0)
                {
                    stringBuilder.Append(@"
                        <h5><strong>All</strong></h5>
                            <div class='row all'>
                                <div class='col-md-8 branchSection'>
                                    <div class='row'>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon orange'><i class='fa fa-print'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage1Count + @"</span>
                                                    <span>Scan</span> <br/><small></small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon tar'><i class='fa fa-check-square-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage2Count + @"</span>
                                                    <span>Index</span> <br/><small></small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon pink'><i class='fa fa-external-link'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage3Count + @"</span>
                                                    <span>Export</span> <br/><small>To Server</small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon green'><i class='fa fa-puzzle-piece'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage4Count + @"</span>
                                                    <span>Integrate</span> <br/><small>Sync Control</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class='col-md-4'>
                                    <div class='row'>
                                        <div class='col-md-6'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon yellow-b'><i class='fa fa-files-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage5Count + @"</span>
                                                    <span>Release</span> <br/><small>To Mimzy</small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-6'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon yellow-b'><i class='fa fa-hdd-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span>" + stage6Count + @"</span>
                                                    <span>Document</span> <br/><small>By Mimzy</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>");
                    BranchListSearchResult = branchList;
                }
                //stringBuilder.Append("<hr class='my-hr' />");
                sortActive.Attributes.Remove("class");
                sortActive.Attributes.Add("class", "popovers btn action-button pull-right");
                sortBranch.Attributes.Remove("class");
                sortBranch.Attributes.Add("class", "popovers btn btn-inactive action-button pull-right");
                if (IsNewSort.Value == "1")
                {
                    SortByAsc.Value = Request.QueryString["SortByAsc"].ToString();
                    if (SortBy.Value == "SortActive")
                    {
                        sortActive.Attributes.Remove("class");
                        sortActive.Attributes.Add("class", "popovers btn action-button pull-right");
                        sortBranch.Attributes.Remove("class");
                        sortBranch.Attributes.Add("class", "popovers btn btn-inactive action-button pull-right");
                        //var groupByBranches = batches.Where(x => x.StageId >= 5).ToList().GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                        var groupByBranches = batchesCount.Where(x => x.StageId >= 5).ToList().GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                        if (SortByAsc.Value == "1")
                        {
                            sortActive.InnerHtml = "<i class='fa fa-sort-numeric-asc'></i>";
                            groupByBranches = groupByBranches.OrderBy(x => x.Count).ToList();
                        }
                        else
                        {
                            sortActive.InnerHtml = "<i class='fa fa-sort-numeric-desc'></i>";
                            groupByBranches = groupByBranches.OrderByDescending(x => x.Count).ToList();
                        }
                        var branchOrderList = groupByBranches.Select(x => x.FirstOrDefault().BranchId).ToList();
                        int newIndex = 0;
                        foreach (Int32 branchid in branchOrderList)
                        {
                            Branch branch = BranchListSearchResult.Where(x => x.Id == branchid).FirstOrDefault();
                            BranchListSearchResult.Remove(branch);
                            BranchListSearchResult.Insert(newIndex, branch);
                            newIndex++;
                        }
                    }
                    if (SortBy.Value == "SortBranch")
                    {
                        sortActive.Attributes.Remove("class");
                        sortActive.Attributes.Add("class", "popovers btn btn-inactive action-button pull-right");
                        sortBranch.Attributes.Remove("class");
                        sortBranch.Attributes.Add("class", "popovers btn action-button pull-right");
                        if (SortByAsc.Value == "1")
                        {
                            sortBranch.InnerHtml = "<i class='fa fa-sort-alpha-asc'></i>";
                            BranchListSearchResult = BranchListSearchResult.OrderBy(x => x.Code).ToList();
                        }
                        else
                        {
                            sortBranch.InnerHtml = "<i class='fa fa-sort-alpha-desc'></i>";
                            BranchListSearchResult = BranchListSearchResult.OrderByDescending(x => x.Code).ToList();
                        }
                    }
                }
                else
                {

                    var groupByBranches = batchesCount.GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                    //var groupByBranches = batches.GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                    groupByBranches = groupByBranches.OrderByDescending(x => x.Count).ToList();
                    var branchOrderList = groupByBranches.Select(x => x.FirstOrDefault().BranchId).ToList();
                    int newIndex = 0;
                    foreach (Int32 branchid in branchOrderList)
                    {
                        Branch branch = BranchListSearchResult.Where(x => x.Id == branchid).FirstOrDefault();
                        if (branch != null)
                        {
                            BranchListSearchResult.Remove(branch);
                            BranchListSearchResult.Insert(newIndex, branch);
                            newIndex++;
                        }
                    }
                }
                foreach (Branch branch in BranchListSearchResult)
                {

                    //stage1Count = batches.Where(x => x.StageId == 1 && x.BranchId == branch.Id).ToList().Count;
                    //stage2Count = batches.Where(x => x.StageId == 2 && x.BranchId == branch.Id).ToList().Count;
                    //stage3Count = batches.Where(x => x.StageId == 3 && x.BranchId == branch.Id).ToList().Count;
                    //stage4Count = batches.Where(x => x.StageId == 4 && x.BranchId == branch.Id).ToList().Count;
                    //stage5Count = batchesCompletedToday.Where(x => x.StageId == 5 && x.BranchId == branch.Id).ToList().Count;
                    //stage6Count = batchesCompletedToday.Where(x => x.StageId == 6 && x.BranchId == branch.Id).ToList().Count;
                    stage1Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Scan && x.BranchId == branch.Id) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Scan && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stage2Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Index && x.BranchId == branch.Id) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Index && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stage3Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Export && x.BranchId == branch.Id) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Export && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stage4Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Integrate && x.BranchId == branch.Id) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Integrate && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stage5Count = batchesserverstageCount.Count(x => x.StageId == (int)BatchesStages.Release && x.BranchId == branch.Id) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Release && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stage6Count = batchesserverstageCount.Count(x => x.StageId == (int)BatchesStages.Document && x.BranchId == branch.Id) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Document && a.BranchId == branch.Id).Sum(a => a.count) : 0;
                    stringBuilder.Append(@"
                        <h5><strong>" + branch.Code + @"</strong></h5>
                        <div class='row " + branch.Code + @"'>
                            <div class='col-md-8 branchSection'>
                                <div class='row'>
                                    <div class='col-md-3'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon orange'><i class='fa fa-print'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='1' data-id='" + branch.Id + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-title='View Batches' data-action='View'>" + stage1Count + @"</a></span>
                                                <span>Scan</span> <br/><small></small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-md-3'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon tar'><i class='fa fa-check-square-o'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='2' data-id='" + branch.Id + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-title='View Batches' data-action='View'>" + stage2Count + @"</a></span>
                                                <span>Index</span> <br/><small></small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-md-3'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon pink'><i class='fa fa-external-link'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='3' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage3Count + @"</a></span>
                                                <span>Export</span> <br/><small>To Server</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-md-3'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon green'><i class='fa fa-puzzle-piece'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='4' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage4Count + @"</a></span>
                                                <span>Integrate</span> <br/><small>Sync Control</small>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-4'>
                                <div class='row'>
                                    <div class='col-md-6'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon yellow-b'><i class='fa fa-files-o'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='5' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage5Count + @"</a></span>
                                                <span>Release</span> <br/><small>To Mimzy</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-md-6'>
                                        <div class='mini-stat clearfix'>
                                            <span class='mini-stat-icon yellow-b'><i class='fa fa-hdd-o'></i></span>
                                            <div class='mini-stat-info'>
                                                <span><a href='javascript:;' data-stageId='6' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage6Count + @"</a></span>
                                                <span>Document</span> <br/><small>By Mimzy</small>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>");

                    List<BatchesStagesCount> totalBatchesCount = new List<BatchesStagesCount>();
                    if (batchesCount != null && batchesCount.Count != 0)
                        totalBatchesCount.AddRange(batchesCount);
                    if (batchesserverstageCount != null && batchesserverstageCount.Count != 0)
                        totalBatchesCount.AddRange(batchesserverstageCount);


                    var groupByDepartments = totalBatchesCount
              .Where(x => x.BranchId == branch.Id)
              .ToList()
              .GroupBy(x => x.DepartmentId)
              .Select(grp => grp.ToList())
              .ToList()
              .OrderBy(o => o.FirstOrDefault().DepartmentId)
              .ToList();

                    //var groupByDepartments = batchesCount
                    //  .Where(x => x.BranchId == branch.Id)
                    //  .ToList()
                    //  .GroupBy(x => x.DepartmentId)
                    //  .Select(grp => grp.ToList())
                    //  .ToList()
                    //  .OrderBy(o => o.FirstOrDefault().DepartmentId)
                    //  .ToList();


                    //var groupByDepartments = batches
                    //    .Where(x => x.BranchId == branch.Id)
                    //    .ToList()
                    //    .GroupBy(x => x.DepartmentId)
                    //    .Select(grp => grp.ToList())
                    //    .ToList()
                    //    .OrderBy(o => o.FirstOrDefault().DepartmentId)
                    //    .ToList();
                    if (BranchId.Value != "")
                    {
                        stringBuilder.Append("<hr class='my-hr' />");
                        if (groupByDepartments.Count == 0)
                        {
                            stringBuilder.Append("<div class='text-center'><h5>No records found</h5></div>");
                        }
                    }
                    if (groupByDepartments.Count > 0 && BranchId.Value != "")
                        foreach (List<BatchesStagesCount> deptBatch in groupByDepartments)
                        {
                            Department department = IDepartmentService.GetSingle(deptBatch.FirstOrDefault().DepartmentId);
                            //stage1Count = batches.Where(x => x.StageId == 1 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            //stage2Count = batches.Where(x => x.StageId == 2 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            //stage3Count = batches.Where(x => x.StageId == 3 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            //stage4Count = batches.Where(x => x.StageId == 4 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            //stage5Count = batchesCompletedToday.Where(x => x.StageId == 5 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            //stage6Count = batchesCompletedToday.Where(x => x.StageId == 6 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                            stage1Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Scan && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stage2Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Index && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stage3Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Export && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stage4Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Integrate && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stage5Count = batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Release && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stage6Count = batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Document && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                            stringBuilder.Append(@"
                        <div class='department department-" + department.Code + @"'>
                            <h5><strong>" + department.Name + @"</strong></h5>
                            <div class='row " + branch.Code + @" " + department.Code + @"'>
                                <div class='col-md-8 branchSection'>
                                    <div class='row'>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon orange'><i class='fa fa-print'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='1' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage1Count + @"</a></span>
                                                    <span>Scan</span> <br/><small></small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon tar'><i class='fa fa-check-square-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='2' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage2Count + @"</a></span>
                                                    <span>Index</span> <br/><small></small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon pink'><i class='fa fa-external-link'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='3' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage3Count + @"</a></span>
                                                    <span>Export</span> <br/><small>To Server</small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-3'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon green'><i class='fa fa-puzzle-piece'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='4' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage4Count + @"</a></span>
                                                    <span>Integrate</span> <br/><small>Sync Control</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class='col-md-4'>
                                    <div class='row'>
                                        <div class='col-md-6'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon yellow-b'><i class='fa fa-files-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='5' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage5Count + @"</a></span>
                                                    <span>Release</span> <br/><small>To Mimzy</small>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-md-6'>
                                            <div class='mini-stat clearfix'>
                                                <span class='mini-stat-icon yellow-b'><i class='fa fa-hdd-o'></i></span>
                                                <div class='mini-stat-info'>
                                                    <span><a href='javascript:;' data-departmentId='" + department.Id + @"' data-stageId='6' data-id='" + branch.Id + @"' data-fromdate='" + fromDate + @"' data-todate='" + toDate + @"' data-original-title='View Batch' data-trigger='hover' data-placement='bottom' class='popovers action-button' data-url='BranchScanDetail.aspx' data-title='View Batches' data-action='View'>" + stage6Count + @"</a></span>
                                                    <span>Document</span> <br/><small>By Mimzy</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>");

                        }

                }
                sveimList.InnerHtml = stringBuilder.ToString();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static object GetNewData(DashboardDTO dashboardDTO)
        {
            DashboardStatistics dashboardStatisticsTotal = new DashboardStatistics();
            List<DashboardStatistics> dashboardStatisticsBranch = new List<DashboardStatistics>();
            List<DashboardStatistics> dashboardStatisticsDepartment = new List<DashboardStatistics>();

            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1 ");
            List<Branch> BranchListSearchResult = new List<Branch>();
            List<Department> DepartmentListSearchResult = new List<Department>();
            if (!string.IsNullOrEmpty(dashboardDTO.Search))
            {
                bool isNumber = long.TryParse(dashboardDTO.Search, out long number);
                if (isNumber)
                    dashboardDTO.Search = number.ToString();

                StringBuilder subFilter = new StringBuilder();
                subFilter.Append(" 1=1 ");
                string columnNameAaNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AaNo));
                subFilter.Append(" and " + columnNameAaNo + " like '%" + dashboardDTO.Search + "%'");
                string columnNameAcNo = Converter.GetColumnNameByPropertyName<Set>(nameof(Set.AccountNo));
                subFilter.Append(" or " + columnNameAcNo + " like '%" + dashboardDTO.Search + "%'");

                List<Set> setMatches = ISetService.GetDataByFilter(subFilter.ToString(), 0, 0, false);
                List<string> batchIds = setMatches.Select(x => x.BatchId.ToString()).ToList<string>();
                subFilter = new StringBuilder();
                subFilter.Append(" 1=1 ");
                string columnNameBatchId = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Id));
                subFilter.Append(" and " + columnNameBatchId + " in (" + String.Join(",", batchIds.ToArray()) + ")");
                List<Batch> batchMatches = IBatchService.GetDataByFilter(subFilter.ToString(), 0, 0, false);

                List<string> branchIds = batchMatches.Select(x => x.BranchId.ToString()).ToList<string>();

                subFilter = new StringBuilder();
                subFilter.Append(" 1=1 ");
                string columnNameBranchId = Converter.GetColumnNameByPropertyName<Branch>(nameof(Branch.Id));
                subFilter.Append(" and " + columnNameBranchId + " in (" + String.Join(",", branchIds.ToArray()) + ")");
                BranchListSearchResult = IBranchService.GetDataByFilter(subFilter.ToString(), 0, 0, false);

                if (BranchListSearchResult != null && BranchListSearchResult.Count != 0)
                    foreach (Branch bc in BranchListSearchResult)
                        foreach (Batch bt in batchMatches)
                            if (bc.Id == bt.BranchId)
                            {
                                if (bc.BranchIdBatches == null)
                                    bc.BranchIdBatches = new List<Batch>();
                                bc.BranchIdBatches.Add(bt);
                            }

            }
            if (!string.IsNullOrEmpty(dashboardDTO.BranchId))
            {
                Branch selectedBranch = IBranchService.GetSingle(Convert.ToInt32(dashboardDTO.BranchId));
                BranchListSearchResult.Add(selectedBranch);
            }

            string stageIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId));
            filter.Append(" and status = '1' ");
            if (!string.IsNullOrEmpty(dashboardDTO.FromDate) && !string.IsNullOrEmpty(dashboardDTO.ToDate))
                filter.Append(" and created_date between '" + dashboardDTO.FromDate + "' and  '" + dashboardDTO.ToDate + "' ");

            // old data
            //  List<Batch> batches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);
            // New method
            List<BatchesStagesCount> batchesCount = IBatchService.GetBatchStagesCount(filter.ToString(), 0, 0, true);

            filter = new StringBuilder();
            filter.Append(" 1=1 ");
            filter.Append(" and (" + stageIdColumnName + " = 5 or " + stageIdColumnName + " = 6) ");

            if (!string.IsNullOrEmpty(dashboardDTO.FromDate) && !string.IsNullOrEmpty(dashboardDTO.ToDate))
                filter.Append(" and created_date between '" + dashboardDTO.FromDate + "' and  '" + dashboardDTO.ToDate + "' ");

            string createdDateColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.CreatedDate));

            //SQL
            filter.Append(" and CAST(DATEADD(year, " + DateInterval + ", " + createdDateColumnName + ") as DATE) = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ");
            //filter.Append(" and status = '1' ");
            filter.Append(" and (status = '1' or status = '9') ");

            List<BatchesStagesCount> batchesserverstageCount = new List<BatchesStagesCount>();
            if (!string.IsNullOrEmpty(dashboardDTO.FromDate) && !string.IsNullOrEmpty(dashboardDTO.ToDate))
                batchesserverstageCount = batchesCount.Where(a => a.StageId == (int)BatchesStages.Release || a.StageId == (int)BatchesStages.Document).ToList();
            else
                batchesserverstageCount = IBatchService.GetBatchStagesCount(filter.ToString(), 0, 0, true);

            // List<Batch> batchesCompletedToday = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);
            // New code
            int stage1Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Scan) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Scan).Sum(a => a.count) : 0;
            int stage2Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Index) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Index).Sum(a => a.count) : 0;
            int stage3Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Export) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Export).Sum(a => a.count) : 0;
            int stage4Count = batchesCount.Count(a => a.StageId == (int)BatchesStages.Integrate) != 0 ? batchesCount.Where(a => a.StageId == (int)BatchesStages.Integrate).Sum(a => a.count) : 0;
            int stage5Count = batchesserverstageCount.Count(a => a.StageId == (int)BatchesStages.Release) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Release).Sum(a => a.count) : 0;
            int stage6Count = batchesserverstageCount.Count(a => a.StageId == (int)BatchesStages.Document) != 0 ? batchesserverstageCount.Where(a => a.StageId == (int)BatchesStages.Document).Sum(a => a.count) : 0;
            StringBuilder stringBuilder = new StringBuilder();

            if (BranchListSearchResult.Count == 0)
            {
                dashboardStatisticsTotal.ScanCount = stage1Count.ToString();
                dashboardStatisticsTotal.IndexCount = stage2Count.ToString();
                dashboardStatisticsTotal.ExportCount = stage3Count.ToString();
                dashboardStatisticsTotal.IntegrateCount = stage4Count.ToString();
                dashboardStatisticsTotal.ReleaseCount = stage5Count.ToString();
                dashboardStatisticsTotal.DocumentCount = stage6Count.ToString();

                List<Branch> branchList = IBranchService.GetData(0, 0, false);
                BranchListSearchResult = branchList;
            }
            //stringBuilder.Append("<hr class='my-hr' />");
            if (dashboardDTO.IsNewSort == "1")
            {
                if (dashboardDTO.SortBy == "SortActive")
                {

                    var groupByBranches = batchesCount.Where(x => x.StageId >= 5).ToList().GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                    //var groupByBranches = batches.Where(x => x.StageId >= 5).ToList().GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                    if (dashboardDTO.SortByAsc == "1")
                        groupByBranches = groupByBranches.OrderBy(x => x.Count).ToList();
                    else
                        groupByBranches = groupByBranches.OrderByDescending(x => x.Count).ToList();

                    var branchOrderList = groupByBranches.Select(x => x.FirstOrDefault().BranchId).ToList();
                    int newIndex = 0;
                    foreach (Int32 branchid in branchOrderList)
                    {
                        Branch branch = BranchListSearchResult.Where(x => x.Id == branchid).FirstOrDefault();
                        BranchListSearchResult.Remove(branch);
                        BranchListSearchResult.Insert(newIndex, branch);
                        newIndex++;
                    }
                }
                if (dashboardDTO.SortBy == "SortBranch")
                {
                    if (dashboardDTO.SortByAsc == "1")
                        BranchListSearchResult = BranchListSearchResult.OrderBy(x => x.Code).ToList();
                    else
                        BranchListSearchResult = BranchListSearchResult.OrderByDescending(x => x.Code).ToList();
                }
            }
            else
            {
                var groupByBranches = batchesCount.GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                //var groupByBranches = batches.GroupBy(grp => grp.BranchId).Select(x => x.ToList()).ToList();
                groupByBranches = groupByBranches.OrderByDescending(x => x.Count).ToList();
                var branchOrderList = groupByBranches.Select(x => x.FirstOrDefault().BranchId).ToList();
                int newIndex = 0;
                foreach (Int32 branchid in branchOrderList)
                {
                    Branch branch = BranchListSearchResult.Where(x => x.Id == branchid).FirstOrDefault();
                    if (branch != null)
                    {
                        BranchListSearchResult.Remove(branch);
                        BranchListSearchResult.Insert(newIndex, branch);
                        newIndex++;
                    }
                }
            }
            foreach (Branch branch in BranchListSearchResult)
            {
                //stage1Count = batches.Where(x => x.StageId == 1 && x.BranchId == branch.Id).ToList().Count;
                //stage2Count = batches.Where(x => x.StageId == 2 && x.BranchId == branch.Id).ToList().Count;
                //stage3Count = batches.Where(x => x.StageId == 3 && x.BranchId == branch.Id).ToList().Count;
                //stage4Count = batches.Where(x => x.StageId == 4 && x.BranchId == branch.Id).ToList().Count;
                stage1Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Scan && x.BranchId == branch.Id) != 0 ? batchesCount.Where(x => x.StageId == (int)BatchesStages.Scan && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                stage2Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Index && x.BranchId == branch.Id) != 0 ? batchesCount.Where(x => x.StageId == (int)BatchesStages.Index && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                stage3Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Export && x.BranchId == branch.Id) != 0 ? batchesCount.Where(x => x.StageId == (int)BatchesStages.Export && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                stage4Count = batchesCount.Count(x => x.StageId == (int)BatchesStages.Integrate && x.BranchId == branch.Id) != 0 ? batchesCount.Where(x => x.StageId == (int)BatchesStages.Integrate && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                stage5Count = batchesserverstageCount.Count(x => x.StageId == (int)BatchesStages.Release && x.BranchId == branch.Id) != 0 ? batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Release && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                stage6Count = batchesserverstageCount.Count(x => x.StageId == (int)BatchesStages.Document && x.BranchId == branch.Id) != 0 ? batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Document && x.BranchId == branch.Id).Sum(a => a.count) : 0;
                dashboardStatisticsBranch.Add(new DashboardStatistics
                {
                    BranchCode = branch.Code,
                    ScanCount = stage1Count.ToString(),
                    IndexCount = stage2Count.ToString(),
                    ExportCount = stage3Count.ToString(),
                    IntegrateCount = stage4Count.ToString(),
                    ReleaseCount = stage5Count.ToString(),
                    DocumentCount = stage6Count.ToString()
                });
                var groupByDepartments = batchesCount
                   .Where(x => x.BranchId == branch.Id)
                   .ToList()
                   .GroupBy(x => x.DepartmentId)
                   .Select(grp => grp.ToList())
                   .ToList()
                   .OrderBy(o => o.FirstOrDefault().DepartmentId)
                   .ToList();
                //var groupByDepartments = batches
                //    .Where(x => x.BranchId == branch.Id)
                //    .ToList()
                //    .GroupBy(x => x.DepartmentId)
                //    .Select(grp => grp.ToList())
                //    .ToList()
                //    .OrderBy(o => o.FirstOrDefault().DepartmentId)
                //    .ToList();
                if (dashboardDTO.BranchId != "")
                {
                    stringBuilder.Append("<hr class='my-hr' />");
                    if (groupByDepartments.Count == 0)
                    {
                        stringBuilder.Append("<div class='text-center'><h5>No records found</h5></div>");
                    }
                }
                if (groupByDepartments.Count > 0 && dashboardDTO.BranchId != "")
                    foreach (List<BatchesStagesCount> deptBatch in groupByDepartments)
                    {
                        Department department = IDepartmentService.GetSingle(deptBatch.FirstOrDefault().DepartmentId);
                        stage1Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Scan && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        stage2Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Index && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        stage3Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Export && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        stage4Count = batchesCount.Where(x => x.StageId == (int)BatchesStages.Integrate && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        stage5Count = batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Release && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        stage6Count = batchesserverstageCount.Where(x => x.StageId == (int)BatchesStages.Document && x.BranchId == branch.Id && x.DepartmentId == department.Id).Sum(a => a.count);
                        //stage1Count = batches.Where(x => x.StageId == 1 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                        //stage2Count = batches.Where(x => x.StageId == 2 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                        //stage3Count = batches.Where(x => x.StageId == 3 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                        //stage4Count = batches.Where(x => x.StageId == 4 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                        //stage5Count = batchesCompletedToday.Where(x => x.StageId == 5 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;
                        //stage6Count = batchesCompletedToday.Where(x => x.StageId == 6 && x.BranchId == branch.Id && x.DepartmentId == department.Id).ToList().Count;

                        dashboardStatisticsDepartment.Add(new DashboardStatistics
                        {
                            BranchCode = branch.Code,
                            DepartmentCode = department.Code,
                            ScanCount = stage1Count.ToString(),
                            IndexCount = stage2Count.ToString(),
                            ExportCount = stage3Count.ToString(),
                            IntegrateCount = stage4Count.ToString(),
                            ReleaseCount = stage5Count.ToString(),
                            DocumentCount = stage6Count.ToString()
                        });
                    }

            }

            return new
            {
                dashboardStatisticsTotal = dashboardStatisticsTotal,
                dashboardStatisticsBranch = dashboardStatisticsBranch,
                dashboardStatisticsDepartment = dashboardStatisticsDepartment
            };
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Test()
        {
            return "Success";
        }
    }

    public class DashboardDTO
    {
        public string Search { get; set; }
        public string BranchId { get; set; }
        public string IsNewSearch { get; set; }
        public string IsNewSort { get; set; }
        public string SortBy { get; set; }
        public string SortByAsc { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class DashboardStatistics
    {
        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string ScanCount { get; set; }
        public string IndexCount { get; set; }
        public string ExportCount { get; set; }
        public string IntegrateCount { get; set; }
        public string ReleaseCount { get; set; }
        public string DocumentCount { get; set; }
    }

}