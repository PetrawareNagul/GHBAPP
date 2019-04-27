using Silverlake.Data.IRepo;
using Silverlake.Utility;
using Silverlake.Repo.MySQLDBRef;
using Silverlake.Utility.Helper;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Silverlake.Data
{
    public class LanguageDirRepo : ILanguageDirRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public LanguageDir PostData(LanguageDir obj)
        {
            try
            {
                //string query = obj.ObjectToQuery<LanguageDir>("insert") + "SELECT SCOPE_IDENTITY()";
                string query = @"INSERT INTO language_dirs(module, remarks, status, text_en, text_id, text_th, text_ms, text_page, text_zh) 
                                VALUES ('"+obj.Module+"','"+obj.Remarks+"','"+obj.Status+"','"+obj.TextEn+"','"+obj.TextId+"',N'"+obj.TextTh+"',N'"+obj.TextMs+"','"+obj.TextPage+"',N'"+obj.TextZh+"'); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                // cmd.ExecuteNonQuery();
                obj.Id = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<LanguageDir> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += @"INSERT INTO language_dirs(module, remarks, status, text_en, text_id, text_th, text_ms, text_page, text_zh) 
                                VALUES ('" + obj.Module + "','" + obj.Remarks + "','" + obj.Status + "','" + obj.TextEn + "','" + obj.TextId + "',N'" + obj.TextTh + "',N'" + obj.TextMs + "','" + obj.TextPage + "',N'" + obj.TextZh + "'); SELECT SCOPE_IDENTITY()";
                    //query += obj.ObjectToQuery<LanguageDir>("insert") + "SELECT SCOPE_IDENTITY()";
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir UpdateData(LanguageDir obj)
        {
            try
            {
                //string query = obj.ObjectToQuery<LanguageDir>("update");
                string query = "UPDATE language_dirs SET text_id = '" + obj.TextId + "', status = '" + obj.Status + "', remarks = '" + obj.Remarks + "', module = '" + obj.Module + "', text_page = '" + obj.TextPage + "', text_en = '" + obj.TextEn + "', text_th = N'" + obj.TextTh + "', text_ms = N'" + obj.TextMs + "', text_zh = N'" + obj.TextZh + "' where ID = " + obj.Id + ";";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<LanguageDir> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += "UPDATE language_dirs SET text_id = '" + obj.TextId + "', status = '" + obj.Status + "', remarks = '" + obj.Remarks + "', module = '" + obj.Module + "', text_page = '" + obj.TextPage + "', text_en = '" + obj.TextEn + "', text_th = N'" + obj.TextTh + "', text_ms = N'" + obj.TextMs + "', text_zh = N'" + obj.TextZh + "' where ID = " + obj.Id + ";";
                    //query += obj.ObjectToQuery<LanguageDir>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir DeleteData(Int32 Id)
        {
            LanguageDir obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<LanguageDir>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 DeleteBulkData(List<Int32> Ids)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                Ids.ForEach(Id =>
                {
                    LanguageDir obj = GetSingle(Id);
                    query += obj.ObjectToQuery<LanguageDir>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir GetSingle(Int32 Id)
        {
            LanguageDir obj = new LanguageDir();
            try
            {
                string query = "select * from language_dirs where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                obj = (from x in dt.AsEnumerable()
                       select new LanguageDir
                       {
                           Id = x.Field<Int32>("id"),
                           Module = x.Field<Int32>("module"),
                           Remarks = x.Field<String>("remarks"),
                           Status = x.Field<Int32>("status"),
                           TextEn = x.Field<String>("text_en"),
                           TextId = x.Field<String>("text_id"),
                           TextTh = x.Field<String>("text_th"),
                           TextMs = x.Field<String>("text_ms"),
                           TextPage = x.Field<String>("text_page"),
                           TextZh = x.Field<String>("text_zh"),
                       }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<LanguageDir> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                string query = "select * from language_dirs";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new LanguageDir
                        {
                            Id = x.Field<Int32>("id"),
                            Module = x.Field<Int32>("module"),
                            Remarks = x.Field<String>("remarks"),
                            Status = x.Field<Int32>("status"),
                            TextEn = x.Field<String>("text_en"),
                            TextId = x.Field<String>("text_id"),
                            TextTh = x.Field<String>("text_th"),
                            TextMs = x.Field<String>("text_ms"),
                            TextPage = x.Field<String>("text_page"),
                            TextZh = x.Field<String>("text_zh"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public Int32 GetCount()
        {
            Int32 count = 0;
            try
            {
                string query = "select count(*) from language_dirs";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<LanguageDir>(propertyName);
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                string query = "select * from language_dirs where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select * from language_dirs where " + columnName + " != '" + propertyValue + "'";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new LanguageDir
                        {
                            Id = x.Field<Int32>("id"),
                            Module = x.Field<Int32>("module"),
                            Remarks = x.Field<String>("remarks"),
                            Status = x.Field<Int32>("status"),
                            TextEn = x.Field<String>("text_en"),
                            TextId = x.Field<String>("text_id"),
                            TextTh = x.Field<String>("text_th"),
                            TextMs = x.Field<String>("text_ms"),
                            TextPage = x.Field<String>("text_page"),
                            TextZh = x.Field<String>("text_zh"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public Int32 GetCountByPropertyName(string propertyName, string propertyValue, bool isEqual)
        {
            string columnName = Converter.GetColumnNameByPropertyName<LanguageDir>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from language_dirs where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select count(*) from language_dirs where " + columnName + " != '" + propertyValue + "'";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                string query = "select * from language_dirs where " + filter + "";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new LanguageDir
                        {
                            Id = x.Field<Int32>("id"),
                            Module = x.Field<Int32>("module"),
                            Remarks = x.Field<String>("remarks"),
                            Status = x.Field<Int32>("status"),
                            TextEn = x.Field<String>("text_en"),
                            TextId = x.Field<String>("text_id"),
                            TextTh = x.Field<String>("text_th"),
                            TextMs = x.Field<String>("text_ms"),
                            TextPage = x.Field<String>("text_page"),
                            TextZh = x.Field<String>("text_zh"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public Int32 GetCountByFilter(string filter)
        {
            Int32 count = 0;
            try
            {
                string query = "select count(*) from language_dirs where " + filter;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                string query = "select * from language_dirs limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new LanguageDir
                        {
                            Id = x.Field<Int32>("id"),
                            Module = x.Field<Int32>("module"),
                            Remarks = x.Field<String>("remarks"),
                            Status = x.Field<Int32>("status"),
                            TextEn = x.Field<String>("text_en"),
                            TextId = x.Field<String>("text_id"),
                            TextTh = x.Field<String>("text_th"),
                            TextMs = x.Field<String>("text_ms"),
                            TextPage = x.Field<String>("text_page"),
                            TextZh = x.Field<String>("text_zh"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            filteredResultsCount = objs.Count;
            totalResultsCount = GetCount();
            return objs;
        }
    }
}
