using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("batch_logs")]
    public class BatchLog
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("batch_id"), Display("BatchId")]
        public Int32 BatchId { get; set; }
        [Database("stage_id"), Display("StageId")]
        public Int32 StageId { get; set; }
        [Database("batch_user"), Display("BatchUser")]
        public String BatchUser { get; set; }
        [Database("batch_count"), Display("BatchCount")]
        public Int32 BatchCount { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32 UpdatedBy { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
    }
}
