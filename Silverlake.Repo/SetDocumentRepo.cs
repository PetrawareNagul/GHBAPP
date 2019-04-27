using Silverlake.Repo.IRepo;
using Silverlake.Utility;
using Silverlake.Repo.MySQLDBRef;
using Silverlake.Utility.Helper;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using System.Data.SqlClient;

namespace Silverlake.Repo
{
    public class SetDocumentRepo : ISetDocumentRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public SetDocument PostData(SetDocument obj)
        {
            try
            {
                string query = obj.ObjectToQuery<SetDocument>("insert") + "SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
               // cmd.ExecuteNonQuery();
                obj.Id = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<SetDocument> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<SetDocument>("insert") + "SELECT SCOPE_IDENTITY()";
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument UpdateData(SetDocument obj)
        {
            try
            {
                string query = obj.ObjectToQuery<SetDocument>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<SetDocument> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<SetDocument>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument DeleteData(Int32 Id)
        {
            SetDocument obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<SetDocument>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
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
                    SetDocument obj = GetSingle(Id);
                    query += obj.ObjectToQuery<SetDocument>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                result = cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument GetSingle(Int32 Id)
        {
            SetDocument obj = new SetDocument();
            try
            {
                string query = "select * from set_documents where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new SetDocument
                        {
                            Id = x.Field<Int32>("id"),
                            SetId = x.Field<Int32>("set_id"),
                            DocType = x.Field<String>("doc_type"),
                            DocumentUrl = x.Field<String>("document_url"),
                            PageCount = x.Field<Int32>("page_count"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            Status = x.Field<Int32>("status"),
                            Version = x.Field<Int32>("version"),
                        }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<SetDocument> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                string query = "select * from set_documents";
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
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new SetDocument
                        {
                            Id = x.Field<Int32>("id"),
                            SetId = x.Field<Int32>("set_id"),
                            DocType = x.Field<String>("doc_type"),
                            DocumentUrl = x.Field<String>("document_url"),
                            PageCount = x.Field<Int32>("page_count"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
                            Status = x.Field<Int32>("status"),
                            Version = x.Field<Int32>("version"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
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
                string query = "select count(*) from set_documents";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<SetDocument>(propertyName);
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                string query = "select * from set_documents where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from set_documents where " + columnName + " != '" + propertyValue + "'";
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
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new SetDocument
                        {
                         Id = x.Field<Int32>("id"),
                         SetId = x.Field<Int32>("set_id"),
                         DocType = x.Field<String>("doc_type"),
                         DocumentUrl = x.Field<String>("document_url"),
                            PageCount = x.Field<Int32>("page_count"),
                         CreatedBy = x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
                         Status = x.Field<Int32>("status"),
                            Version = x.Field<Int32>("version"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public Int32 GetCountByPropertyName(string propertyName, string propertyValue, bool isEqual)
        {
            string columnName = Converter.GetColumnNameByPropertyName<SetDocument>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from set_documents where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from set_documents where " + columnName + " != '" + propertyValue + "'";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                string query = "select * from set_documents where " + filter +"";
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
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new SetDocument
                        {
                            Id = x.Field<Int32>("id"),
                            SetId = x.Field<Int32>("set_id"),
                            DocType = x.Field<String>("doc_type"),
                            DocumentUrl = x.Field<String>("document_url"),
                            PageCount = x.Field<Int32>("page_count"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            Status = x.Field<Int32>("status"),
                            Version = x.Field<Int32>("version"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
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
                string query = "select count(*) from set_documents where " + filter;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                string query = "select * from set_documents limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new SetDocument
                        {
                            Id = x.Field<Int32>("id"),
                            SetId = x.Field<Int32>("set_id"),
                            DocType = x.Field<String>("doc_type"),
                            DocumentUrl = x.Field<String>("document_url"),
                            PageCount = x.Field<Int32>("page_count"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            Status = x.Field<Int32>("status"),
                            Version = x.Field<Int32>("version"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            filteredResultsCount = objs.Count;
            totalResultsCount = GetCount();
            return objs;
        }
    }
}
