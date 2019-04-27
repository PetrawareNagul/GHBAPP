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
    public partial class BatchHeaders : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        private static readonly Lazy<IBatchHeaderService> lazyBatchHeaderServiceObj = new Lazy<IBatchHeaderService>(() => new BatchHeaderService());

        public static IBatchHeaderService IBatchHeaderService { get { return lazyBatchHeaderServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

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

                if (userRole == "Branch Admin")
                {
                    Branch userBranch = IBranchService.GetSingle(user.BranchId);
                    filter.Append(" and branch_id='" + userBranch.Id + "'");
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
                    Search.Value = Request.QueryString["Search"].ToString();
                    string columnNameUsername = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchNo));
                    filter.Append(" and " + columnNameUsername + " like '%" + Search.Value + "%'");
                }

                int skip = 0, take = 10;
                if (hdnCurrentPageNo.Value == "")
                {
                    skip = 0;
                    take = 10;
                    hdnNumberPerPage.Value = "10";
                    hdnCurrentPageNo.Value = "1";
                    hdnTotalRecordsCount.Value = IBatchHeaderService.GetCountByFilter(filter.ToString()).ToString();
                }
                else
                {
                    skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                    take = 10;
                }

                List<BatchHeader> objs = IBatchHeaderService.GetDataByFilter(filter.ToString(), skip, take, true);

                StringBuilder asb = new StringBuilder();
                int index = 1;
                foreach (BatchHeader b in objs)
                {
                    Department department = IDepartmentService.GetSingle(b.DepartmentId);
                    asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + b.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (b.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td>" + department.Code + @"</td>
                                <td>" + b.Name + @"</td>
                                <td>" + b.Url + @"</td>
                            </tr>");
                    index++;
                }
                batcheHeadersTbody.InnerHtml = asb.ToString();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Action(Int32[] ids, String action, String rejectReason = "")
        {
            try
            {
                string idString = String.Join(",", ids);
                List<BatchHeader> objs = IBatchHeaderService.GetDataByFilter(" ID in (" + idString + ")", 0, 0, false);
                if (action == "Deactivate")
                {
                    objs.ForEach(x => {
                        x.UpdatedBy = 0;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 0;
                    });
                    IBatchHeaderService.UpdateBulkData(objs);
                }
                if (action == "Activate")
                {
                    objs.ForEach(x => {
                        x.UpdatedBy = 0;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 1;
                    });
                    IBatchHeaderService.UpdateBulkData(objs);
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