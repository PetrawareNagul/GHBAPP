using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public static class Converter
    {
        public static List<T> DataTableToList<T>(this DataTable table)
        {
            try
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                var columnNames = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
                var objectProperties = typeof(T).GetProperties(flags).Where(y => y.PropertyType.Namespace == "System" && y.GetCustomAttribute(typeof(DatabaseAttribute), true) != null);
                List<T> targetList = table.AsEnumerable().Select(dataRow =>
                {
                    var instanceOfT = Activator.CreateInstance<T>();
                    foreach (var properties in
                        objectProperties
                            .Where(
                                properties =>
                                columnNames
                                    .Contains(
                                        (
                                            (properties
                                            .GetCustomAttributes(typeof(DatabaseAttribute), true)
                                            .FirstOrDefault() as DatabaseAttribute)
                                            .GetValue()
                                        )
                                    )
                                    &&
                                    dataRow[(properties
                                        .GetCustomAttributes(typeof(DatabaseAttribute), true)
                                        .FirstOrDefault() as DatabaseAttribute)
                                        .GetValue()] != DBNull.Value
                            )
                    )
                    {
                        var columnName = (properties
                                           .GetCustomAttributes(typeof(DatabaseAttribute), true)
                                           .FirstOrDefault() as DatabaseAttribute)
                                           .GetValue();
                        var dataValue = dataRow[columnName];
                        var columnDataType = dataRow.Table.Columns[columnName].DataType.Name;
                        properties.SetValue(instanceOfT, columnDataType == "Boolean" ? Convert.ToInt32(dataValue) : dataValue, null);

                    }
                    return instanceOfT;
                }).ToList<T>();
                return targetList;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }
        }
        public static string ObjectToQuery<T>(this T obj, string type)
        {
            DatabaseAttribute dynamicType = typeof(T).GetCustomAttributes(typeof(DatabaseAttribute), true).FirstOrDefault() as DatabaseAttribute;
            string tableName = dynamicType.GetValue();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var objectProperties = typeof(T).GetProperties(flags).Where(y => y.PropertyType.Namespace == "System").Select(y => { return (y.Name != "Id" ? (y.GetCustomAttributes(typeof(DatabaseAttribute), true).FirstOrDefault() as DatabaseAttribute).GetValue() : null); }).Where(x => x != null).ToList();
            var objectPropertyId = obj.GetType().GetProperties(flags).Where(y => y.PropertyType.Namespace == "System").Select(y => { return (y.Name == "Id" ? y.GetValue(obj) : null); }).Where(x => x != null).FirstOrDefault();
            List<string> objectPropertyValues = new List<string>();
            objectProperties.ToList().ForEach(p => {
                var value = obj
                            .GetType()
                            .GetProperties(flags)
                            .Where(y => y.PropertyType.Namespace == "System")
                            .Select(y => {
                                return ((y.GetCustomAttributes(typeof(DatabaseAttribute), true).FirstOrDefault() as DatabaseAttribute).GetValue() == p.ToString() ? 
                                    (y.PropertyType == typeof(DateTime) || y.PropertyType == typeof(DateTime?) ?
                                        "convert(datetime,'" + Convert.ToDateTime(y.GetValue(obj)).ToString("MM/dd/yyyy hh:mm:ss tt") + "')" :
                                        y.PropertyType == typeof(Byte[]) ? "" + y.GetValue(obj) + "" : "'" + y.GetValue(obj) + "'") :
                                    null);
                            })
                            .Where(x => x != null)
                            .FirstOrDefault();
                if(value.ToString() == "convert(datetime,'01/01/0001 12:00:00 AM')")
                {
                    value = "''";
                }
                objectPropertyValues.Add(value.ToString());
            });
            List<string> columnNames = objectProperties.ToList();
            StringBuilder sCommand = new StringBuilder();
            List<string> Rows = new List<string>();
            switch (type)
            {
                case "insert":
                    sCommand = new StringBuilder("INSERT INTO " + tableName + "(" + String.Join(", ", columnNames.ToArray()) + ") VALUES ");
                    Rows.Add(string.Format("({0})", String.Join(",", objectPropertyValues.ToArray())));
                    sCommand.Append(string.Join(", ", Rows));
                    sCommand.Append("; ");
                    break;
                case "update":
                    string columnsAndParams = String.Join(", ", columnNames.Select(x => x + " = ###" + x + "###").ToArray());
                    int index = 0;
                    foreach (string val in objectPropertyValues)
                    {
                        string columnName = columnNames[index];
                        columnsAndParams = columnsAndParams.Replace("###" + columnName + "###", val);
                        index++;
                    }
                    sCommand = new StringBuilder("UPDATE " + tableName + " SET " + columnsAndParams);
                    sCommand.Append(" where ID = " + objectPropertyId);
                    sCommand.Append(";");
                    break;
            }
            return sCommand.ToString();
        }
        public static string GetColumnNameByPropertyName<T>(string propertyName)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            string columnName = typeof(T)
                .GetProperties(flags)
                .Where(y => y.Name == propertyName)
                .Select(y =>
                    (y
                        .GetCustomAttributes(typeof(DatabaseAttribute), true)
                        .FirstOrDefault() as DatabaseAttribute
                    )
                    .GetValue()
                ).FirstOrDefault().ToString();
            return columnName;
        }
    }
}
