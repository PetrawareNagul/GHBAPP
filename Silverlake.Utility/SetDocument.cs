using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("set_documents")]
    public class SetDocument
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("set_id"), Display("SetId")]
        public Int32 SetId { get; set; }
        [Database("doc_type"), Display("DocType")]
        public String DocType { get; set; }
        [Database("document_url"), Display("DocumentUrl")]
        public String DocumentUrl { get; set; }
        [Database("page_count"), Display("PageCount")]
        public Int32 PageCount { get; set; }
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
        [Database("version"), Display("Version")]
        public Int32 Version { get; set; }
        public Set SetIdSet { get; set; }
    }
}
