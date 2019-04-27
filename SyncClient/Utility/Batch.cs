using SyncClient.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncClient.Utility
{
    [Database("batches")]
    public class Batch
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("branch_id"), Display("BranchId")]
        public Int32 BranchId { get; set; }
        [Database("department_id"), Display("DepartmentId")]
        public Int32 DepartmentId { get; set; }
        [Database("stage_id"), Display("StageId")]
        public Int32 StageId { get; set; }
        [Database("batch_key"), Display("BatchKey")]
        public String BatchKey { get; set; }
        [Database("batch_no"), Display("BatchNo")]
        public String BatchNo { get; set; }
        [Database("batch_count"), Display("BatchCount")]
        public Int32? BatchCount { get; set; }
        [Database("is_batch_count_updated"), Display("IsBatchCountUpdated")]
        public Int32 IsBatchCountUpdated { get; set; }
        [Database("batch_user"), Display("BatchUser")]
        public String BatchUser { get; set; }
        [Database("batch_status"), Display("BatchStatus")]
        public Int32 BatchStatus { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
    }
}
