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
    public class UserDetailRepo : IUserDetailRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public UserDetail PostData(UserDetail obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserDetail>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<UserDetail> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserDetail>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public UserDetail UpdateData(UserDetail obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserDetail>("update");
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
        public Int32 UpdateBulkData(List<UserDetail> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserDetail>("update");
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
        public UserDetail DeleteData(Int32 Id)
        {
            UserDetail obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<UserDetail>("update");
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
                    UserDetail obj = GetSingle(Id);
                    query += obj.ObjectToQuery<UserDetail>("update");
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
        public UserDetail GetSingle(Int32 Id)
        {
            UserDetail obj = new UserDetail();
            try
            {
                string query = "select * from user_details where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new UserDetail
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            FirstName = x.Field<String>("first_name"),
                            LastName = x.Field<String>("last_name"),
                            Description = x.Field<String>("description"),
                            DateOfBirth = x.Field<DateTime?>("date_of_birth"),
                            ImageUrl = x.Field<String>("image_url"),
                            CreatedBy = x.Field<Int32?>("created_by") == null ? 0 : x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            ModifiedBy = x.Field<Int32?>("modified_by") == null ? 0 : x.Field<Int32>("modified_by"),
                            ModifiedDate = x.Field<DateTime?>("modified_date"),
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
        public List<UserDetail> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                string query = "select * from user_details";
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
                        select new UserDetail
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            FirstName = x.Field<String>("first_name"),
                            LastName = x.Field<String>("last_name"),
                            Description = x.Field<String>("description"),
                         DateOfBirth = x.Field<DateTime?>("date_of_birth"),
                            ImageUrl = x.Field<String>("image_url"),
                         CreatedBy = x.Field<Int32?>("created_by") == null ? 0 : x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         ModifiedBy = x.Field<Int32?>("modified_by") == null ? 0 : x.Field<Int32>("modified_by"),
                         ModifiedDate = x.Field<DateTime?>("modified_date"),
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
                string query = "select count(*) from user_details";
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
        public List<UserDetail> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<UserDetail>(propertyName);
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                string query = "select * from user_details where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from user_details where " + columnName + " != '" + propertyValue + "'";
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
                        select new UserDetail
                        {
                         Id = x.Field<Int32>("id"),
                         UserId = x.Field<Int32>("user_id"),
                         FirstName = x.Field<String>("first_name"),
                         LastName = x.Field<String>("last_name"),
                         Description = x.Field<String>("description"),
                         DateOfBirth = x.Field<DateTime?>("date_of_birth"),
                         ImageUrl = x.Field<String>("image_url"),
                         CreatedBy = x.Field<Int32?>("created_by") == null ? 0 : x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         ModifiedBy = x.Field<Int32?>("modified_by") == null ? 0 : x.Field<Int32>("modified_by"),
                         ModifiedDate = x.Field<DateTime?>("modified_date"),
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
            string columnName = Converter.GetColumnNameByPropertyName<UserDetail>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from user_details where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from user_details where " + columnName + " != '" + propertyValue + "'";
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
        public List<UserDetail> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                string query = "select * from user_details where " + filter +"";
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
                        select new UserDetail
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            FirstName = x.Field<String>("first_name"),
                            LastName = x.Field<String>("last_name"),
                            Description = x.Field<String>("description"),
                            DateOfBirth = x.Field<DateTime?>("date_of_birth"),
                            ImageUrl = x.Field<String>("image_url"),
                            CreatedBy = x.Field<Int32?>("created_by") == null ? 0 : x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            ModifiedBy = x.Field<Int32?>("modified_by") == null ? 0 : x.Field<Int32>("modified_by"),
                            ModifiedDate = x.Field<DateTime?>("modified_date"),
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
                string query = "select count(*) from user_details where " + filter;
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
        public List<UserDetail> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                string query = "select * from user_details limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new UserDetail
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            FirstName = x.Field<String>("first_name"),
                            LastName = x.Field<String>("last_name"),
                            Description = x.Field<String>("description"),
                            DateOfBirth = x.Field<DateTime?>("date_of_birth"),
                            ImageUrl = x.Field<String>("image_url"),
                            CreatedBy = x.Field<Int32?>("created_by") == null ? 0 : x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            ModifiedBy = x.Field<Int32?>("modified_by") == null ? 0 : x.Field<Int32>("modified_by"),
                            ModifiedDate = x.Field<DateTime?>("modified_date"),
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
