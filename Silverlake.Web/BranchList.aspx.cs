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
    public partial class BranchList : System.Web.UI.Page
    {
        private static readonly Lazy<IBranchDepartmentService> lazyBranchDepartmentServiceObj = new Lazy<IBranchDepartmentService>(() => new BranchDepartmentService());

        public static IBranchDepartmentService IBranchDepartmentService { get { return lazyBranchDepartmentServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<ICompanyService> lazyCompanyServiceObj = new Lazy<ICompanyService>(() => new CompanyService());

        public static ICompanyService ICompanyService { get { return lazyCompanyServiceObj.Value; } }

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
                string columnNameName = Converter.GetColumnNameByPropertyName<Branch>(nameof(Silverlake.Utility.Branch.Name));
                filter.Append(" and " + columnNameName + " like '%" + Search.Value + "%'");
                string columnNameCode = Converter.GetColumnNameByPropertyName<Branch>(nameof(Silverlake.Utility.Branch.Code));
                filter.Append(" or " + columnNameCode + " like '%" + Search.Value + "%'");
            }


            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = IBranchService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<Branch> objs = IBranchService.GetDataByFilter(filter.ToString(), skip, take, false);

            List<Department> departments = IDepartmentService.GetData(0, 0, false);

            StringBuilder asb = new StringBuilder();
            int index = 1;
            foreach (Branch obj in objs)
            {
                StringBuilder departmentsHTML = new StringBuilder();
                List<BranchDepartment> branchDepartments = IBranchDepartmentService.GetDataByPropertyName(nameof(BranchDepartment.BranchId), obj.Id.ToString(), true, 0, 0, false);
                bool isSelectAllChecked = obj.IsAll == 1 ? true : false;
                departmentsHTML.Append(@"
                    <label class='icheck'>
                        <div class='flat-blue single-row'>
                            <div class='checkbox'>
                                <input type='checkbox' name='checkRow' class='checkRow selectAll' value='' " + (isSelectAllChecked ? "checked" : "") + @"/> <label>Select All</label><br/>
                            </div>
                        </div>
                    </label>
                ");
                foreach (Department d in departments)
                {
                    bool isChecked = false;
                    if (isSelectAllChecked)
                    {
                        isChecked = true;
                    }
                    else if (branchDepartments.Count > 0)
                    {
                        BranchDepartment bd = branchDepartments.Where(x => x.DepartmentId == d.Id && x.Status == 1).FirstOrDefault();
                        if (bd == null)
                        {
                            isChecked = true;
                        }
                    }
                    departmentsHTML.Append(@"
                        <label class='icheck'>
                            <div class='flat-green single-row'>
                                <div class='checkbox'>
                                    <input type='checkbox' name='checkRow' class='checkRow' value='" + d.Id + @"' " + (isChecked ? "checked" : "") + @"/> <label>" + d.Code + @"</label><br/>
                                </div>
                            </div>
                        </label>
                    ");
                }
                Company company = ICompanyService.GetSingle(obj.CompanyId);
                asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + obj.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (obj.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td><strong>" + obj.Code + "</strong><br/>" + obj.Name + @"</td>
                                <td style='width: 300px;'>
                                    " + departmentsHTML.ToString() + @"
                                </td>
                                <td>" + obj.CreatedDate.ToString("dd/MM/yyyy") + @"</td>
                                <td>" + (obj.UpdatedDate == null ? "-" : obj.UpdatedDate.Value.ToString("dd/MM/yyyy")) + @"</td>
                            </tr>");
                index++;
            }
            branchesTBody.InnerHtml = asb.ToString();
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
                List<Branch> objs = IBranchService.GetDataByFilter(" ID in (" + idString + ")", 0, 0, false);
                if (action == "Deactivate")
                {
                    objs.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 0;
                    });
                    IBranchService.UpdateBulkData(objs);
                }
                if (action == "Activate")
                {
                    objs.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 1;
                    });
                    IBranchService.UpdateBulkData(objs);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("branch list action: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Add(Branch obj)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            Response response = new Response();
            try
            {
                if (obj.Id == 0)
                {
                    if (IBranchService.GetCountByPropertyName(nameof(Utility.Branch.Code), obj.Code, true) != 0)
                    {
                        response.isSuccess = false;
                        response.message = "Code already exist";
                        return response;
                    }
                    if (IBranchService.GetCountByPropertyName(nameof(Utility.Branch.Name), obj.Name, true) != 0)
                    {
                        response.isSuccess = false;
                        response.message = "Name already exist";
                        return response;
                    }
                    obj.CreatedBy = LoginUserId;
                    obj.CreatedDate = DateTime.Now;
                    IBranchService.PostData(obj);
                    response.message = "Successfully added";
                    response.isSuccess = true;

                }
                else
                {
                    Branch branch = IBranchService.GetSingle(obj.Id);
                    if (branch.Name == obj.Name && branch.Code == obj.Code)
                    {
                        response.message = "No changes";
                        response.isSuccess = false;
                    }
                    else
                    {
                        obj.UpdatedBy = LoginUserId;
                        obj.UpdatedDate = DateTime.Now;
                        IBranchService.UpdateData(obj);
                        response.message = "Successfully updated";
                        response.isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.message = ex.ToString();
                response.isSuccess = false;
            }
            return response;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object UpdateDepartments(List<IdDepartments> allObjs)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            foreach (IdDepartments bds in allObjs)
            {
                int branchId = bds.Id;
                Branch branch = IBranchService.GetSingle(branchId);
                if (bds.isSelectAll)
                {
                    branch.IsAll = 1;
                    branch.UpdatedDate = DateTime.Now;
                    branch.UpdatedBy = LoginUserId;
                    IBranchService.UpdateData(branch);

                    List<BranchDepartment> branchDepartments = IBranchDepartmentService.GetDataByPropertyName(nameof(BranchDepartment.BranchId), branchId.ToString(), true, 0, 0, false);
                    branchDepartments.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedDate = DateTime.Now;
                        x.Status = 0;
                    });
                    IBranchDepartmentService.UpdateBulkData(branchDepartments);
                }
                else
                {
                    //List<Department> departments = IDepartmentService.GetData(0, 0, false);
                    List<BranchDepartment> branchDepartments = IBranchDepartmentService.GetDataByPropertyName(nameof(BranchDepartment.BranchId), branchId.ToString(), true, 0, 0, false);

                    foreach (BranchDepartment bd in branchDepartments)
                    {
                        int? deptId = bds.DepartmentIds.Where(x => x == bd.DepartmentId).FirstOrDefault();
                        if (deptId == 0)
                        {
                            BranchDepartment bdNew = bd;
                            bd.Status = 0;
                            bd.UpdatedBy = LoginUserId;
                            bd.UpdatedDate = DateTime.Now;
                            IBranchDepartmentService.UpdateData(bd);
                        }
                        else
                        {
                            BranchDepartment bdNew = bd;
                            bd.Status = 1;
                            bd.UpdatedBy = LoginUserId;
                            bd.UpdatedDate = DateTime.Now;
                            IBranchDepartmentService.UpdateData(bd);
                        }
                    }

                    foreach (int departmentId in bds.DepartmentIds)
                    {
                        BranchDepartment bd = branchDepartments.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
                        if (bd == null)
                        {
                            bd = new BranchDepartment
                            {
                                BranchId = branchId,
                                DepartmentId = departmentId,
                                CreatedBy = LoginUserId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = LoginUserId,
                                UpdatedDate = DateTime.Now,
                                Status = 1
                            };
                            IBranchDepartmentService.PostData(bd);
                        }
                        else
                        {
                            if (bd.Status == 0)
                            {
                                bd.Status = 1;
                                bd.UpdatedBy = LoginUserId;
                                bd.UpdatedDate = DateTime.Now;
                                IBranchDepartmentService.UpdateData(bd);
                            }
                        }
                    }

                    branch.IsAll = 0;
                    branch.UpdatedDate = DateTime.Now;
                    branch.UpdatedBy = LoginUserId;
                    IBranchService.UpdateData(branch);
                }
            }
            return true;
        }


    }
}