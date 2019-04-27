using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimzyCaptureSyncServer.Custom
{
    class Classes
    {
    }

    public class LogList
    {
        public string FileFullPath { get; set; }
        public string ChangeType { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class AAValidateResponse
    {
        [JsonProperty("Result")]
        public string Result { get; set; }
        [JsonProperty("Account Name")]
        public string AccountName { get; set; }
        [JsonProperty("Reserved Field")]
        public string ReservedField { get; set; }
    }

}
