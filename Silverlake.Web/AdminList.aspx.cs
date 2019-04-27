using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.CustomClasses;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class AdminList : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IUserRoleService> lazyUserRoleServiceObj = new Lazy<IUserRoleService>(() => new UserRoleService());

        public static IUserRoleService IUserRoleService { get { return lazyUserRoleServiceObj.Value; } }

        private static readonly Lazy<IUserTypeService> lazyUserTypeServiceObj = new Lazy<IUserTypeService>(() => new UserTypeService());

        public static IUserTypeService IUserTypeService { get { return lazyUserTypeServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentUserService> lazyDepartmentUserServiceObj = new Lazy<IDepartmentUserService>(() => new DepartmentUserService());

        public static IDepartmentUserService IDepartmentUserService { get { return lazyDepartmentUserServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchDepartmentService> lazyBranchDepartmentServiceObj = new Lazy<IBranchDepartmentService>(() => new BranchDepartmentService());

        public static IBranchDepartmentService IBranchDepartmentService { get { return lazyBranchDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            User user = IUserService.GetSingle(LoginUserId);
            UserRole userRole = IUserRoleService.GetSingle(user.UserRoleId);

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
                string columnNameUsername = Converter.GetColumnNameByPropertyName<User>(nameof(Silverlake.Utility.User.Username));
                filter.Append(" and " + columnNameUsername + " like '%" + Search.Value + "%'");
                string columnNameEmail = Converter.GetColumnNameByPropertyName<User>(nameof(Silverlake.Utility.User.EmailId));
                filter.Append(" or " + columnNameEmail + " like '%" + Search.Value + "%'");
                string columnNameMobile = Converter.GetColumnNameByPropertyName<User>(nameof(Silverlake.Utility.User.MobileNumber));
                filter.Append(" or " + columnNameMobile + " like '%" + Search.Value + "%'");
            }

            string userRoleId = Converter.GetColumnNameByPropertyName<User>(nameof(Silverlake.Utility.User.UserRoleId));
            List<string> adminroleids = IUserRoleService.GetDataByPropertyName(nameof(UserRole.UserTypeId), "6", true, 0, 0, false).Select(x => x.Id.ToString()).ToList();
            filter.Append(" and " + userRoleId + " in (" + String.Join(",", adminroleids.ToArray()) + ")");

            if (userRole.Name == "HQ Admin")
            {
                filter.Append(" and " + userRoleId + " > " + userRole.Id + "");
            }
            else if (userRole.Name == "Regional Admin")
            {
                filter.Append(" and " + userRoleId + " > " + userRole.Id + "");
                List<Branch> Branches = new List<Branch>();
                if (user.IsAll == 0)
                {
                    List<BranchUser> userBranches = IBranchUserService.GetDataByFilter(" user_id = '" + user.Id + "' and status='1'", 0, 0, false);
                    Branches = IBranchService.GetDataByFilter(" ID not in (" + String.Join(",", userBranches.Select(x => x.BranchId).ToArray()) + ")", 0, 0, false);
                }
                else
                {
                    Branches = IBranchService.GetDataByFilter(" status='1'", 0, 0, false);
                }
                filter.Append(" and " + userRoleId + " > " + userRole.Id + " and branch_id in (" + String.Join(",", Branches.Select(x => x.Id).ToArray()) + ")");
            }
            else if (userRole.Name == "Branch Admin")
            {
                filter.Append(" and " + userRoleId + " > " + userRole.Id + "");
                filter.Append(" and " + userRoleId + " > " + userRole.Id + " and branch_id in (" + user.BranchId + ")");
            }
            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = IUserService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<User> users = IUserService.GetDataByFilter(filter.ToString(), skip, take, true);

            StringBuilder asb = new StringBuilder();
            int index = 1;

            List<Branch> branches = IBranchService.GetData(0, 0, false);
            List<Department> departments = IDepartmentService.GetData(0, 0, false);

            foreach (User u in users)
            {
                userRole = IUserRoleService.GetSingle(u.UserRoleId);
                Branch b = IBranchService.GetSingle(u.BranchId);

                StringBuilder departmentsHTML = new StringBuilder();
                if (userRole.Name == "Branch Admin")
                {
                    List<BranchDepartment> branchDepartments = IBranchDepartmentService.GetDataByFilter(" branch_id = '" + u.BranchId + "' and status='1'", 0, 0, false);
                    if (b.IsAll == 0)
                    {
                        departments = departments.Where(x => !(branchDepartments.Select(y => y.DepartmentId).ToList().Contains(x.Id))).ToList();
                    }
                    List<DepartmentUser> userDepartments = IDepartmentUserService.GetDataByPropertyName(nameof(DepartmentUser.UserId), u.Id.ToString(), true, 0, 0, false);
                    bool isSelectAllChecked = u.IsAll == 1 ? true : false;
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
                        else if (userDepartments.Count > 0)
                        {
                            DepartmentUser bd = userDepartments.Where(x => x.DepartmentId == d.Id && x.Status == 1).FirstOrDefault();
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
                }

                StringBuilder branchesHTML = new StringBuilder();
                if (userRole.Name == "Regional Admin")
                {
                    List<BranchUser> userBranches = IBranchUserService.GetDataByFilter(" user_id = '" + u.Id + "' and status='1'", 0, 0, false);
                    bool isSelectAllChecked = u.IsAll == 1 ? true : false;
                    branchesHTML.Append(@"
                        <label class='icheck'>
                            <div class='flat-blue single-row'>
                                <div class='checkbox'>
                                    <input type='checkbox' name='checkRow' class='checkRow selectAll' value='' " + (isSelectAllChecked ? "checked" : "") + @"/> <label>Select All</label><br/>
                                </div>
                            </div>
                        </label>
                    ");
                    foreach (Branch br in branches)
                    {
                        bool isChecked = false;
                        if (isSelectAllChecked)
                        {
                            isChecked = true;
                        }
                        else if (userBranches.Count > 0)
                        {
                            BranchUser bd = userBranches.Where(x => x.BranchId == br.Id && x.Status == 1).FirstOrDefault();
                            if (bd == null)
                            {
                                isChecked = true;
                            }
                        }
                        branchesHTML.Append(@"
                        <label class='icheck'>
                            <div class='flat-green single-row'>
                                <div class='checkbox'>
                                    <input type='checkbox' name='checkRow' class='checkRow' value='" + br.Id + @"' " + (isChecked ? "checked" : "") + @"/> <label>" + br.Code + @"</label><br/>
                                </div>
                            </div>
                        </label>
                    ");
                    }
                }
                asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + u.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (u.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td>
                                    Username: <strong>" + u.Username + @"</strong><br />
                                    Email: <strong>" + u.EmailId + @"</strong><br />
                                    Mobile: <strong>" + u.MobileNumber + @"</strong><br />
                                    Role: <strong>" + userRole.Name + @"</strong><br />");

                if (userRole.Name == "Branch Admin")
                {

                    asb.Append(@"API Auth Token: <a href='javascript:;' class='btn btn-sm btn-primary view_batch_log'>View</a>
                                    <div class='div_batch_log hide draggableDiv'>
                                        <span class='log_close'>X</span>
                                        <table class='table mb-0'>
                                            <tr><td><strong>Api Auth Token</strong></td><td class='text-right'>User: <strong>" + u.Username + @"</strong></td></tr>
                                        </table>
                                        <div class='mini-stat clearfix text-left'>" + u.ApiAuthToken + @"</div>
                                    </div>");
                }
                asb.Append(@"</td>
                                <td style='width: 600px;'>
                                    " + (u.BranchId == 0 ? "<strong>Branches</strong><br />" + branchesHTML.ToString() : "<strong>Branch: " + b.Code + "</strong><br />") + @"
                                    " + (userRole.Name == "Branch Admin" ? "<strong>Departments</strong><br />" + departmentsHTML.ToString() : "") + @"
                                </td>
                            </tr>");
                index++;
            }
            adminsTbody.InnerHtml = asb.ToString();
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
                List<User> users = IUserService.GetDataByFilter(" ID in (" + idString + ")", 0, 0, false);
                if (action == "Deactivate")
                {
                    users.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedOn = DateTime.Now;
                        x.Status = 0;
                    });
                    IUserService.UpdateBulkData(users);
                }
                if (action == "Activate")
                {
                    users.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedOn = DateTime.Now;
                        x.Status = 1;
                    });
                    IUserService.UpdateBulkData(users);
                }
                if (action == "GenerateToken")
                {
                    users.ForEach(x =>
                    {
                        x.UpdatedBy = LoginUserId;
                        x.UpdatedOn = DateTime.Now;
                        x.ApiAuthToken = TokenGenerator.Get(x.Username);
                    });
                    IUserService.UpdateBulkData(users);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("User accounts action: " + ex.Message);
                return false;
            }
        }

        public static Response IsValidPhoneNumber(string phoneNumber)
        {
            Response response = new Response();
            try
            {
                //will match +61 or +66- or 0 or nothing followed by a nine digit number
                bool isValid = Regex.Match(phoneNumber,
                    @"^([\+]?66[-]?|[0])?[1-9][0-9]{8}$").Success;
                //to vary this, replace 61 with an international code of your choice 
                //or remove [\+]?61[-]? if international code isn't needed
                //{8} is the number of digits in the actual phone number less one
                if (isValid)
                {
                    response.isSuccess = true;
                    response.message = "Success";
                }
                else
                {
                    response.isSuccess = false;
                    response.message = "Invalid mobile number";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = ex.Message;
            }
            return response;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Add(User obj)
        {
            Response response = new Response();
            if (obj.UserRoleId == 0)
            {
                response.isSuccess = false;
                response.message = "Selct the Role";
                return response;
            }
            else if (obj.UserRoleId == 5 && obj.BranchId == 0)
            {
                response.isSuccess = false;
                response.message = "Select the Branch";
                return response;
            }
            if (obj.Username == "")
            {
                response.isSuccess = false;
                response.message = "Username cannot be empty";
                return response;
            }
            if (obj.EmailId == "")
            {
                response.isSuccess = false;
                response.message = "Email cannot be empty";
                return response;
            }
            if (obj.MobileNumber == "")
            {
                response.isSuccess = false;
                response.message = "Mobile number cannot be empty";
                return response;
            }
            else
            {
                response = IsValidPhoneNumber(obj.MobileNumber);
                if (!response.isSuccess)
                {
                    return response;
                }
            }
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                if (obj.Id == 0)
                {
                    if (obj.Username != "")
                    {
                        List<User> userMatches = IUserService.GetDataByPropertyName(nameof(Utility.User.Username), obj.Username, true, 0, 0, false);
                        if (userMatches.Count > 0)
                        {
                            response.isSuccess = false;
                            response.message = "Username is taken";
                            return response;
                        }
                        List<User> emailMatches = IUserService.GetDataByPropertyName(nameof(Utility.User.EmailId), obj.EmailId, true, 0, 0, false);
                        if (emailMatches.Count > 0)
                        {
                            response.isSuccess = false;
                            response.message = "Email already exist";
                            return response;
                        }
                    }
                    string defaultPassword = CustomEncryptorDecryptor.EncryptPassword("default123");
                    obj.UniqueKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    obj.Password = defaultPassword;
                    obj.TransPwd = defaultPassword;
                    obj.ApiAuthToken = TokenGenerator.Get(obj.Username);
                    obj.CreatedBy = LoginUserId;
                    obj.CreatedOn = DateTime.Now;
                    User user = obj;
                    string siteURL = ConfigurationManager.AppSettings["siteURL"];
                    if (obj.UserRoleId == 5 && user.BranchId != 0)
                    {
                        Branch b = IBranchService.GetSingle(user.BranchId);
                        user.TransPwd = b.Code;
                    }
                    else if (obj.UserRoleId == 4)
                    {
                        user.TransPwd = "Regional Admin";
                    }
                    else if (obj.UserRoleId == 3)
                    {
                        user.TransPwd = "HQ";
                    }
                    else
                    {
                        user.TransPwd = "";
                    }

                    Email email = new Email()
                    {
                        Subject = "User created - GHB",
                        Link = siteURL + "Login.aspx",
                        User = user
                    };
                    response = EmailService.SendActivationMail(email);
                    if (response.isSuccess)
                    {
                        user = IUserService.PostData(obj);
                        response.isSuccess = true;
                        response.message = "Success";
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    User user = IUserService.GetSingle(obj.Id);
                    obj.CreatedBy = user.CreatedBy;
                    obj.CreatedOn = user.CreatedOn;
                    obj.LastLoginOn = user.LastLoginOn;
                    obj.UpdatedOn = DateTime.Now;
                    obj.UpdatedBy = LoginUserId;
                    IUserService.UpdateData(obj);

                    response.isSuccess = true;
                    response.message = "Success";
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add admin action: " + ex.Message);
                response.isSuccess = false;
                response.message = ex.Message;
                return response;
            }
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
                int userId = bds.Id;
                User user = IUserService.GetSingle(userId);
                UserRole userRole = IUserRoleService.GetSingle(user.UserRoleId);
                if (bds.isSelectAll)
                {
                    user.IsAll = 1;
                    user.UpdatedOn = DateTime.Now;
                    user.UpdatedBy = LoginUserId;
                    IUserService.UpdateData(user);
                    if (userRole.Name == "Branch Admin")
                    {
                        List<DepartmentUser> userDepartments = IDepartmentUserService.GetDataByPropertyName(nameof(DepartmentUser.UserId), userId.ToString(), true, 0, 0, false);
                        userDepartments.ForEach(x =>
                        {
                            x.UpdatedBy = LoginUserId;
                            x.UpdatedDate = DateTime.Now;
                            x.Status = 0;
                        });
                        IDepartmentUserService.UpdateBulkData(userDepartments);
                    }
                    else if (userRole.Name == "Regional Admin")
                    {
                        List<BranchUser> userBranches = IBranchUserService.GetDataByPropertyName(nameof(BranchUser.UserId), userId.ToString(), true, 0, 0, false);
                        userBranches.ForEach(x =>
                        {
                            x.UpdatedBy = LoginUserId;
                            x.UpdatedDate = DateTime.Now;
                            x.Status = 0;
                        });
                        IBranchUserService.UpdateBulkData(userBranches);
                    }
                }
                else
                {
                    if (userRole.Name == "Branch Admin")
                    {

                        //List<Department> departments = IDepartmentService.GetData(0, 0, false);
                        List<DepartmentUser> userDepartments = IDepartmentUserService.GetDataByPropertyName(nameof(DepartmentUser.UserId), userId.ToString(), true, 0, 0, false);

                        foreach (DepartmentUser bd in userDepartments)
                        {
                            int? deptId = bds.DepartmentIds.Where(x => x == bd.DepartmentId).FirstOrDefault();
                            if (deptId == 0)
                            {
                                DepartmentUser bdNew = bd;
                                bd.Status = 0;
                                bd.UpdatedBy = LoginUserId;
                                bd.UpdatedDate = DateTime.Now;
                                IDepartmentUserService.UpdateData(bd);
                            }
                            else
                            {
                                DepartmentUser bdNew = bd;
                                bd.Status = 1;
                                bd.UpdatedBy = LoginUserId;
                                bd.UpdatedDate = DateTime.Now;
                                IDepartmentUserService.UpdateData(bd);
                            }
                        }

                        foreach (int departmentId in bds.DepartmentIds)
                        {
                            DepartmentUser bd = userDepartments.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
                            if (bd == null)
                            {
                                bd = new DepartmentUser
                                {
                                    UserId = userId,
                                    DepartmentId = departmentId,
                                    CreatedBy = LoginUserId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = LoginUserId,
                                    UpdatedDate = DateTime.Now,
                                    Status = 1
                                };
                                IDepartmentUserService.PostData(bd);
                            }
                            else
                            {
                                if (bd.Status == 0)
                                {
                                    bd.Status = 1;
                                    bd.UpdatedBy = LoginUserId;
                                    bd.UpdatedDate = DateTime.Now;
                                    IDepartmentUserService.UpdateData(bd);
                                }
                            }
                        }
                    }
                    else if (userRole.Name == "Regional Admin")
                    {
                        List<BranchUser> userBranches = IBranchUserService.GetDataByPropertyName(nameof(BranchUser.UserId), userId.ToString(), true, 0, 0, false);

                        foreach (BranchUser bd in userBranches)
                        {
                            int? deptId = bds.DepartmentIds.Where(x => x == bd.BranchId).FirstOrDefault();
                            if (deptId == 0)
                            {
                                BranchUser bdNew = bd;
                                bd.Status = 0;
                                bd.UpdatedBy = LoginUserId;
                                bd.UpdatedDate = DateTime.Now;
                                IBranchUserService.UpdateData(bd);
                            }
                            else
                            {
                                BranchUser bdNew = bd;
                                bd.Status = 1;
                                bd.UpdatedBy = LoginUserId;
                                bd.UpdatedDate = DateTime.Now;
                                IBranchUserService.UpdateData(bd);
                            }
                        }

                        foreach (int departmentId in bds.DepartmentIds)
                        {
                            BranchUser bd = userBranches.Where(x => x.BranchId == departmentId).FirstOrDefault();
                            if (bd == null)
                            {
                                bd = new BranchUser
                                {
                                    UserId = userId,
                                    BranchId = departmentId,
                                    CreatedBy = LoginUserId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = LoginUserId,
                                    UpdatedDate = DateTime.Now,
                                    Status = 1
                                };
                                IBranchUserService.PostData(bd);
                            }
                            else
                            {
                                if (bd.Status == 0)
                                {
                                    bd.Status = 1;
                                    bd.UpdatedBy = LoginUserId;
                                    bd.UpdatedDate = DateTime.Now;
                                    IBranchUserService.UpdateData(bd);
                                }
                            }
                        }
                    }
                    user.IsAll = 0;
                    user.UpdatedOn = DateTime.Now;
                    user.UpdatedBy = LoginUserId;
                    IUserService.UpdateData(user);
                }
            }
            return true;
        }

    }
}