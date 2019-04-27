using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class AddAdmin : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IUserRoleService> lazyUserRoleServiceObj = new Lazy<IUserRoleService>(() => new UserRoleService());

        public static IUserRoleService IUserRoleService { get { return lazyUserRoleServiceObj.Value; } }

        private static readonly Lazy<IUserTypeService> lazyUserTypeServiceObj = new Lazy<IUserTypeService>(() => new UserTypeService());

        public static IUserTypeService IUserTypeService { get { return lazyUserTypeServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<ICompanyService> lazyCompanyServiceObj = new Lazy<ICompanyService>(() => new CompanyService());

        public static ICompanyService ICompanyService { get { return lazyCompanyServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] != null)
            {
                Int32 UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = IUserService.GetSingle(UserId);
                UserRole userRole = IUserRoleService.GetSingle(user.UserRoleId);
                if (user.BranchId != 0)
                {
                    BranchId.Disabled = true;
                    BranchId.Value = user.BranchId.ToString();
                }
                List<Branch> branches = IBranchService.GetDataByPropertyName(nameof(Branch.CompanyId), "1", true, 0, 0, false);
                List<UserRole> adminRoles = IUserRoleService.GetDataByPropertyName(nameof(UserRole.UserTypeId), "6", true, 0, 0, false);
                if (userRole.Name == "HQ Admin")
                {
                    adminRoles = adminRoles.Where(x => x.Id > userRole.Id).ToList();
                }
                else if (userRole.Name == "Regional Admin")
                {
                    if (user.IsAll == 0)
                    {
                        List<BranchUser> userBranches = IBranchUserService.GetDataByFilter(" user_id = '" + user.Id + "' and status='1'", 0, 0, false);
                        branches = IBranchService.GetDataByFilter(" ID not in (" + String.Join(",", userBranches.Select(x => x.BranchId).ToArray()) + ")", 0, 0, false);
                    }
                    adminRoles = adminRoles.Where(x => x.Id > userRole.Id).ToList();
                }
                else if (userRole.Name == "Branch Admin")
                {
                    Branch branch = IBranchService.GetSingle(user.BranchId);
                    List<Branch> AdminBranches = new List<Branch>();
                    AdminBranches.Add(branch);
                    branches = AdminBranches;
                    adminRoles = adminRoles.Where(x => x.Id > userRole.Id).ToList();
                }
                UserRoleId.DataSource = adminRoles;
                UserRoleId.DataTextField = "Name";
                UserRoleId.DataValueField = "Id";
                UserRoleId.DataBind();
                UserRoleId.Items.Insert(0, new ListItem { Value = "", Text = "Select" });


                BranchId.DataSource = branches;
                BranchId.DataTextField = "Code";
                BranchId.DataValueField = "Id";
                BranchId.DataBind();
                BranchId.Items.Insert(0, new ListItem { Value = "0", Text = "Select" });

            }

            string currentDateString = DateTime.Now.ToString("MM/dd/yyyy");
            CreatedBy.Value = "0";
            UpdatedBy.Value = "0";
            CreatedDate.Value = currentDateString;
            UpdatedDate.Value = currentDateString;

            

            ApiAuthToken.Attributes.Add("readonly", "readonly");

            string idString = Request.QueryString["id"];
            if (idString != null && idString != "")
            {
                int id = Convert.ToInt32(idString);
                User obj = IUserService.GetSingle(id);

                UserRoleId.Value = obj.UserRoleId.ToString();
                Username.Value = obj.Username;
                EmailId.Value = obj.EmailId;
                MobileNumber.Value = obj.MobileNumber;

                Password.Value = obj.Password;
                TransPwd.Value = obj.TransPwd;
                UniqueKey.Value = obj.UniqueKey;
                IsOnline.Value = obj.IsOnline.ToString();
                IsActive.Value = obj.IsActive.ToString();
                IsPrimary.Value = obj.IsPrimary.ToString();
                RegisterIp.Value = obj.RegisterIp.ToString();
                LastLoginOn.Value = obj.LastLoginOn == null ? DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") : obj.LastLoginOn.Value.ToString("MM/dd/yyyy HH:mm:ss");
                LastLoginIp.Value = obj.LastLoginIp.ToString();
                ApiAuthToken.Value = obj.ApiAuthToken;

                Id.Value = obj.Id.ToString();
                Status.Value = obj.Status.ToString();
                CreatedBy.Value = obj.CreatedBy.ToString();
                CreatedDate.Value = obj.CreatedOn.ToString("MM/dd/yyyy HH:mm:ss");
                UpdatedBy.Value = obj.UpdatedBy.ToString();
                UpdatedDate.Value = obj.UpdatedOn == null ? DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") : obj.UpdatedOn.Value.ToString("MM/dd/yyyy HH:mm:ss");

                CompanyId.Value = "1";
                BranchId.Value = obj.BranchId == 0 ? "0" : obj.BranchId.ToString();
                DepartmentId.Value = obj.DepartmentId == 0 ? "0" : obj.DepartmentId.ToString();
                IsAll.Value = obj.IsAll.ToString();

            }
        }
    }
}