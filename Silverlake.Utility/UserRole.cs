using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("user_roles")]
    public class UserRole
    {
        public List<User> UserRoleIdUsers { get; set; }
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("name"), Display("Name")]
        public String Name { get; set; }
        [Database("user_type_id"), Display("UserTypeId")]
        public Int32 UserTypeId { get; set; }
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
        public UserType UserTypeIdUserType { get; set; }
        public List<DepartmentUser> UserRoleIdDepartmentUsers { get; set; }
        public List<CompanyUser> UserRoleIdCompanyUsers { get; set; }
        public List<BranchUser> UserRoleIdBranchUsers { get; set; }
    }
}
