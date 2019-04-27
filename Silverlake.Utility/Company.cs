using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("companies")]
    public class Company
    {
        public List<Department> CompanyIdDepartments { get; set; }
        public List<CompanyUser> CompanyIdCompanyUsers { get; set; }
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("code"), Display("Code")]
        public String Code { get; set; }
        [Database("name"), Display("Name")]
        public String Name { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        public List<Branch> CompanyIdBranches { get; set; }
    }
}
