using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("users")]
    public class User
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("user_role_id"), Display("UserRoleId")]
        public Int32 UserRoleId { get; set; }
        [Database("username"), Display("Username")]
        public String Username { get; set; }
        [Database("email_id"), Display("EmailId")]
        public String EmailId { get; set; }
        [Database("mobile_number"), Display("MobileNumber")]
        public String MobileNumber { get; set; }
        [Database("password"), Display("Password")]
        public String Password { get; set; }
        [Database("trans_pwd"), Display("TransPwd")]
        public String TransPwd { get; set; }
        [Database("unique_key"), Display("UniqueKey")]
        public String UniqueKey { get; set; }
        [Database("is_online"), Display("IsOnline")]
        public Int32 IsOnline { get; set; }
        [Database("is_active"), Display("IsActive")]
        public Int32 IsActive { get; set; }
        [Database("is_primary"), Display("IsPrimary")]
        public Int32 IsPrimary { get; set; }
        [Database("register_ip"), Display("RegisterIp")]
        public String RegisterIp { get; set; }
        [Database("last_login_on"), Display("LastLoginOn")]
        public DateTime? LastLoginOn { get; set; }
        [Database("last_login_ip"), Display("LastLoginIp")]
        public String LastLoginIp { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("created_on"), Display("CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
        [Database("updated_on"), Display("UpdatedOn")]
        public DateTime? UpdatedOn { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        [Database("api_auth_token"), Display("ApiAuthToken")]
        public String ApiAuthToken { get; set; }
        [Database("branch_id"), Display("BranchId")]
        public Int32 BranchId { get; set; }
        [Database("department_id"), Display("DepartmentId")]
        public Int32 DepartmentId { get; set; }
        [Database("company_id"), Display("CompanyId")]
        public Int32 CompanyId { get; set; }
        [Database("is_password_reset"), Display("IsPasswordReset")]
        public Int32 IsPasswordReset { get; set; }
        [Database("last_sync_date"), Display("LastSyncDate")]
        public DateTime? LastSyncDate { get; set; }
        [Database("is_all"), Display("IsAll")]
        public Int32 IsAll { get; set; }
        public User UpdatedByUser { get; set; }
        public User CreatedByUser { get; set; }
        public UserRole UserRoleIdUserRole { get; set; }
        public List<UserSecurity> UserIdUserSecurities { get; set; }
        public List<UserDetail> ModifiedByUserDetails { get; set; }
        public List<UserDetail> CreatedByUserDetails { get; set; }
        public List<UserDetail> UserIdUserDetails { get; set; }
        public List<DepartmentUser> UserIdDepartmentUsers { get; set; }
        public List<CompanyUser> UserIdCompanyUsers { get; set; }
        public List<BranchUser> UserIdBranchUsers { get; set; }
    }
}
