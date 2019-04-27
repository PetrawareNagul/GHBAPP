using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("branch_departments")]
    public class BranchDepartment
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("branch_id"), Display("BranchId")]
        public Int32 BranchId { get; set; }
        [Database("department_id"), Display("DepartmentId")]
        public Int32 DepartmentId { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        public Department DepartmentIdDepartment { get; set; }
        public Branch BranchIdBranch { get; set; }
    }
}
