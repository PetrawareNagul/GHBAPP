using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("stages")]
    public class Stage
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("code"), Display("Code")]
        public String Code { get; set; }
        [Database("name"), Display("Name")]
        public String Name { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        public List<Batch> StageIdBatches { get; set; }
    }
}
