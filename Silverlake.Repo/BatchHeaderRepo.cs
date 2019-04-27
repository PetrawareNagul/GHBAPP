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
    public class BatchHeaderRepo : IBatchHeaderRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public BatchHeader PostData(BatchHeader obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BatchHeader>("insert") + "SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                //cmd.ExecuteNonQuery();
                obj.Id = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<BatchHeader> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BatchHeader>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public BatchHeader UpdateData(BatchHeader obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BatchHeader>("update");
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
        public Int32 UpdateBulkData(List<BatchHeader> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BatchHeader>("update");
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
        public BatchHeader DeleteData(Int32 Id)
        {
            BatchHeader obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<BatchHeader>("update");
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
                    BatchHeader obj = GetSingle(Id);
                    query += obj.ObjectToQuery<BatchHeader>("update");
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
        public BatchHeader GetSingle(Int32 Id)
        {
            BatchHeader obj = new BatchHeader();
            try
            {
                string query = "select * from batch_headers where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                obj = (from x in dt.AsEnumerable()
                        select new BatchHeader
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            Url = x.Field<String>("url"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            Status = x.Field<Int32>("status"),
                        }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BatchHeader> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                string query = "select * from batch_headers";
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
                        select new BatchHeader
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            Url = x.Field<String>("url"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                            CreatedBy = x.Field<Int32>("created_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            Status = x.Field<Int32>("status"),
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
                string query = "select count(*) from batch_headers";
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
        public List<BatchHeader> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<BatchHeader>(propertyName);
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                string query = "select * from batch_headers where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from batch_headers where " + columnName + " != '" + propertyValue + "'";
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
                        select new BatchHeader
                        {
                         Id = x.Field<Int32>("id"),
                         DepartmentId = x.Field<Int32>("department_id"),
                         Code = x.Field<String>("code"),
                         Name = x.Field<String>("name"),
                         Url = x.Field<String>("url"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         CreatedBy = x.Field<Int32>("created_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         Status = x.Field<Int32>("status"),
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
            string columnName = Converter.GetColumnNameByPropertyName<BatchHeader>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from batch_headers where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from batch_headers where " + columnName + " != '" + propertyValue + "'";
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
        public List<BatchHeader> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                string query = "select * from batch_headers where " + filter +"";
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
                        select new BatchHeader
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            Url = x.Field<String>("url"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            Status = x.Field<Int32>("status"),
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
                string query = "select count(*) from batch_headers where " + filter;
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
        public List<BatchHeader> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                string query = "select * from batch_headers limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new BatchHeader
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            Url = x.Field<String>("url"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            Status = x.Field<Int32>("status"),
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
