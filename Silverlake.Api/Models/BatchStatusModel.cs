using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silverlake.Api.Models
{
    public class BatchStatusModel
    {
        public String BranchId { get; set; }
        public String DepartmentId { get; set; }
        public Int32 StageId { get; set; }
        public String BatchKey { get; set; }
        public String UserId { get; set; }
        public String BatchNo { get; set; }
        public Int32 BatchCount { get; set; }
        public Int32 Status { get; set; }
        public String CreatedDate { get; set; }
        public String UpdatedDate { get; set; }
    }
}