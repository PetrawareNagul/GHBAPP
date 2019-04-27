using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("language_dirs")]
    public class LanguageDir
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("module"), Display("Module")]
        public Int32 Module { get; set; }
        [Database("remarks"), Display("Remarks")]
        public String Remarks { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        [Database("text_en"), Display("TextEn")]
        public String TextEn { get; set; }
        [Database("text_id"), Display("TextId")]
        public String TextId { get; set; }
        [Database("text_th"), Display("TextTh")]
        public String TextTh { get; set; }
        [Database("text_ms"), Display("TextMs")]
        public String TextMs { get; set; }
        [Database("text_page"), Display("TextPage")]
        public String TextPage { get; set; }
        [Database("text_zh"), Display("TextZh")]
        public String TextZh { get; set; }
    }
}
