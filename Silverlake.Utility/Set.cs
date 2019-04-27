using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("sets")]
    public class Set
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("batch_id"), Display("BatchId")]
        public Int32 BatchId { get; set; }
        [Database("set_key"), Display("SetKey")]
        public String SetKey { get; set; }
        [Database("aa_no"), Display("AaNo")]
        public String AaNo { get; set; }
        [Database("account_no"), Display("AccountNo")]
        public String AccountNo { get; set; }
        [Database("set_xml_path"), Display("SetXmlPath")]
        public String SetXmlPath { get; set; }
        [Database("is_released"), Display("IsReleased")]
        public Int32 IsReleased { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32 CreatedBy { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("updated_by"), Display("UpdatedBy")]
        public Int32? UpdatedBy { get; set; }
        [Database("updated_date"), Display("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        [Database("set_status"), Display("SetStatus")]
        public Int32 SetStatus { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        [Database("remarks"), Display("Remarks")]
        public String Remarks { get; set; }
        public List<SetDocument> SetIdSetDocuments { get; set; }
    }
}
