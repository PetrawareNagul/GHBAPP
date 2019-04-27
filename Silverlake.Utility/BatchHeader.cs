using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("batch_headers")]
    public class BatchHeader
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("department_id"), Display("DepartmentId")]
        public Int32 DepartmentId { get; set; }
        [Database("code"), Display("Code")]
        public String Code { get; set; }
        [Database("name"), Display("Name")]
        public String Name { get; set; }
        [Database("url"), Display("Url")]
        public String Url { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
    }
}
