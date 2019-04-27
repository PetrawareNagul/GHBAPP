using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public class Extension
    {
        public static string GetDefaultExtension(string mimeType)
        {
            string defaultExt;
            RegistryKey key;
            object value;
            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" +mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            defaultExt = value != null ? value.ToString() : string.Empty;
            return defaultExt;
        }
    }
}
