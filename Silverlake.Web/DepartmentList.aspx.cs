using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.CustomClasses;
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
    public partial class DepartmentList : System.Web.UI.Page
    {
        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<ICompanyService> lazyCompanyServiceObj = new Lazy<ICompanyService>(() => new CompanyService());

        public static ICompanyService ICompanyService { get { return lazyCompanyServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IBranchDepartmentService> lazyBranchDepartmentServiceServiceObj = new Lazy<IBranchDepartmentService>(() => new BranchDepartmentService());

        public static IBranchDepartmentService IBranchDepartmentService { get { return lazyBranchDepartmentServiceServiceObj.Value; } }

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
                string columnNameName = Converter.GetColumnNameByPropertyName<Department>(nameof(Silverlake.Utility.Department.Name));
                filter.Append(" and " + columnNameName + " like '%" + Search.Value + "%'");
                string columnNameCode = Converter.GetColumnNameByPropertyName<Department>(nameof(Silverlake.Utility.Department.Code));
                filter.Append(" or " + columnNameCode + " like '%" + Search.Value + "%'");
            }


            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = IDepartmentService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<Department> objs = IDepartmentService.GetDataByFilter(filter.ToString(), skip, take, true);

            StringBuilder asb = new StringBuilder();
            int index = 1;
            foreach (Department obj in objs)
            {
                asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + obj.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (obj.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td>" + obj.Code + @"</td>
                                <td>" + obj.Name + @"</td>
                                <td>" + obj.CreatedDate.ToString("dd/MM/yyyy") + @"</td>
                                <td>" + (obj.UpdatedDate == null ? "-" : obj.UpdatedDate.Value.ToString("dd/MM/yyyy")) + @"</td>
                            </tr>");
                index++;
            }
            departmentsTBody.InnerHtml = asb.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Action(Int32[] ids, String action, String rejectReason = "")
        {
            Int32 LoginUserId = 0;
            Response response = new Response();

            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                string idString = String.Join(",", ids);
                List<Department> objs = IDepartmentService.GetDataByFilter(" ID in (" + idString + ")", 0, 0, false);
                if (action == "Deactivate")
                {
                    List<string> responseMessages = new List<string>();
                    objs.ForEach(x =>
                    {
                        int branchcount = IBranchService.GetCountByFilter(" is_all=1 and status=1 ");
                        if (branchcount == 0)
                        {
                            x.UpdatedBy = LoginUserId;
                            x.UpdatedDate = DateTime.Now;
                            x.Status = 0;
                            IDepartmentService.UpdateData(x);
                            responseMessages.Add(x.Code + " Successfully Deactivated.<br/>");
                        }
                        else
                        {
                            int branchDepartmentsCount = IBranchDepartmentService.GetCountByFilter(" department_id=" + x.Id + " and status=1  ");
                            if (branchDepartmentsCount == branchcount)
                            {
                                x.UpdatedBy = LoginUserId;
                                x.UpdatedDate = DateTime.Now;
                                x.Status = 0;
                                IDepartmentService.UpdateData(x);
                                responseMessages.Add(x.Code + " Successfully Deactivated.<br/>");
                            }
                            else
                            {
                                responseMessages.Add(x.Code + " Failed to Deactivated.<br/>");
                            }
                        }
                    });
                    response.isSuccess = true;
                    response.message = String.Join(",", responseMessages.ToArray());
                    return response;
                }
                if (action == "Activate")
                {
                    objs.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 1;
                    });
                    IDepartmentService.UpdateBulkData(objs);
                }
                response.isSuccess = false;
                response.message = "Selct the Role";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("department list action: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Add(Department obj)
        {
            Int32 LoginUserId = 0;
            Response response = new Response();
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                if (obj.Id == 0)
                {
                    List<Department> codeMatches = IDepartmentService.GetDataByPropertyName(nameof(Utility.Department.Code), obj.Code, true, 0, 0, false);
                    if (codeMatches.Count > 0)
                    {
                        response.isSuccess = false;
                        response.message = "Code already exist";
                        return response;
                    }
                    List<Department> nameMatches = IDepartmentService.GetDataByPropertyName(nameof(Utility.Department.Name), obj.Name, true, 0, 0, false);
                    if (nameMatches.Count > 0)
                    {
                        response.isSuccess = false;
                        response.message = "Name already exist";
                        return response;
                    }
                    obj.CreatedBy = LoginUserId;
                    obj.CreatedDate = DateTime.Now;
                    IDepartmentService.PostData(obj);
                }
                else
                {
                    obj.UpdatedBy = LoginUserId;
                    obj.UpdatedDate = DateTime.Now;
                    IDepartmentService.UpdateData(obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("add department action: " + ex.Message);
                return false;
            }
        }
    }
}