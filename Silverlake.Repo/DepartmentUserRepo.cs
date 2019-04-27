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
    public class DepartmentUserRepo : IDepartmentUserRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public DepartmentUser PostData(DepartmentUser obj)
        {
            try
            {
                string query = obj.ObjectToQuery<DepartmentUser>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<DepartmentUser> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<DepartmentUser>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public DepartmentUser UpdateData(DepartmentUser obj)
        {
            try
            {
                string query = obj.ObjectToQuery<DepartmentUser>("update");
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
        public Int32 UpdateBulkData(List<DepartmentUser> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<DepartmentUser>("update");
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
        public DepartmentUser DeleteData(Int32 Id)
        {
            DepartmentUser obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<DepartmentUser>("update");
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
                    DepartmentUser obj = GetSingle(Id);
                    query += obj.ObjectToQuery<DepartmentUser>("update");
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
        public DepartmentUser GetSingle(Int32 Id)
        {
            DepartmentUser obj = new DepartmentUser();
            try
            {
                string query = "select * from department_users where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new DepartmentUser
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            UserId = x.Field<Int32>("user_id"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
        public List<DepartmentUser> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                string query = "select * from department_users";
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
                        select new DepartmentUser
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            UserId = x.Field<Int32>("user_id"),
                            CreatedBy = x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from department_users";
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
        public List<DepartmentUser> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<DepartmentUser>(propertyName);
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                string query = "select * from department_users where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from department_users where " + columnName + " != '" + propertyValue + "'";
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
                        select new DepartmentUser
                        {
                         Id = x.Field<Int32>("id"),
                         DepartmentId = x.Field<Int32>("department_id"),
                         UserId = x.Field<Int32>("user_id"),
                         CreatedBy = x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
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
            string columnName = Converter.GetColumnNameByPropertyName<DepartmentUser>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from department_users where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from department_users where " + columnName + " != '" + propertyValue + "'";
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
        public List<DepartmentUser> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                string query = "select * from department_users where " + filter +"";
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
                        select new DepartmentUser
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            UserId = x.Field<Int32>("user_id"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from department_users where " + filter;
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
        public List<DepartmentUser> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                string query = "select * from department_users limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new DepartmentUser
                        {
                            Id = x.Field<Int32>("id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            UserId = x.Field<Int32>("user_id"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
