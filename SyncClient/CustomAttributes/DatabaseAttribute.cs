using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncClient.CustomAttributes
{
    public class DatabaseAttribute : Attribute
    {
        public string dbName = "";
        public DatabaseAttribute(String dbname)
        {
            dbName = dbname;
        }
        public string GetValue()
        {
            return dbName;
        }
    }
}
