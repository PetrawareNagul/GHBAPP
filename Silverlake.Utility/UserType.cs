using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("user_types")]
    public class UserType
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("name"), Display("Name")]
        public String Name { get; set; }
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
        public List<UserRole> UserTypeIdUserRoles { get; set; }
    }
}
