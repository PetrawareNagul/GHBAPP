using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncClient.CustomAttributes
{
    public class DisplayAttribute : Attribute
    {
        public string displayName = "";
        public DisplayAttribute(String displayname)
        {
            displayName = displayname;
        }
        public string GetValue()
        {
            return displayName;
        }
    }
}
