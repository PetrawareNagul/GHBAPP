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
    public class BatchLogRepo : IBatchLogRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public BatchLog PostData(BatchLog obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BatchLog>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<BatchLog> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BatchLog>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public BatchLog UpdateData(BatchLog obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BatchLog>("update");
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
        public Int32 UpdateBulkData(List<BatchLog> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BatchLog>("update");
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
        public BatchLog DeleteData(Int32 Id)
        {
            BatchLog obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<BatchLog>("update");
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
                    BatchLog obj = GetSingle(Id);
                    query += obj.ObjectToQuery<BatchLog>("update");
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
        public BatchLog GetSingle(Int32 Id)
        {
            BatchLog obj = new BatchLog();
            try
            {
                string query = "select * from batch_logs where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new BatchLog
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BatchCount = x.Field<Int32>("batch_count"),
                            BatchUser = x.Field<String>("batch_user"),
                            UpdatedDate = x.Field<DateTime>("updated_date"),
                            UpdatedBy = x.Field<Int32>("updated_by"),
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
        public List<BatchLog> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                string query = "select * from batch_logs";
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
                        select new BatchLog
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BatchCount = x.Field<Int32>("batch_count"),
                            BatchUser = x.Field<String>("batch_user"),
                            UpdatedDate = x.Field<DateTime>("updated_date"),
                            UpdatedBy = x.Field<Int32>("updated_by"),
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
                string query = "select count(*) from batch_logs";
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
        public List<BatchLog> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<BatchLog>(propertyName);
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                string query = "select * from batch_logs where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from batch_logs where " + columnName + " != '" + propertyValue + "'";
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
                        select new BatchLog
                        {
                         Id = x.Field<Int32>("id"),
                         BatchId = x.Field<Int32>("batch_id"),
                         StageId = x.Field<Int32>("stage_id"),
                         BatchCount = x.Field<Int32>("batch_count"),
                            BatchUser = x.Field<String>("batch_user"),
                            UpdatedDate = x.Field<DateTime>("updated_date"),
                         UpdatedBy = x.Field<Int32>("updated_by"),
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
            string columnName = Converter.GetColumnNameByPropertyName<BatchLog>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from batch_logs where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from batch_logs where " + columnName + " != '" + propertyValue + "'";
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
        public List<BatchLog> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                string query = "select * from batch_logs where " + filter +"";
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
                        select new BatchLog
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BatchCount = x.Field<Int32>("batch_count"),
                            BatchUser = x.Field<String>("batch_user"),
                            UpdatedDate = x.Field<DateTime>("updated_date"),
                            UpdatedBy = x.Field<Int32>("updated_by"),
                            Status = x.Field<Int32>("status"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
               // return objs != null && objs.Count != 0 ? objs.GroupBy(a => a.StageId).Select(a => a.FirstOrDefault()).ToList() : objs;
        }
        public Int32 GetCountByFilter(string filter)
        {
            Int32 count = 0;
            try
            {
                string query = "select count(*) from batch_logs where " + filter;
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
        public List<BatchLog> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                string query = "select * from batch_logs limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new BatchLog
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BatchCount = x.Field<Int32>("batch_count"),
                            BatchUser = x.Field<String>("batch_user"),
                            UpdatedDate = x.Field<DateTime>("updated_date"),
                            UpdatedBy = x.Field<Int32>("updated_by"),
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
