using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public class DocTypeModel
    {
        public int DepartmentId { get; set; }
        public string DocType { get; set; }
    }


    public class DocTypeSetModel
    {
        public int BatchId { get; set; }
        public int SetId { get; set; }
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }
        public string BatchNo { get; set; }
        public string BatchUser { get; set; }
        public string DocType { get; set; }
        public string AANO { get; set; }
        public string AccountNo { get; set; }
        public int PageCount { get; set; }
        public int IsReleased { get; set; }
        public DateTime CreatedDate { get; set; }
    }


}
