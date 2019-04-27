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
    public class UserLoginHistoryRepo : IUserLoginHistoryRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public UserLoginHistory PostData(UserLoginHistory obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserLoginHistory>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<UserLoginHistory> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserLoginHistory>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public UserLoginHistory UpdateData(UserLoginHistory obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserLoginHistory>("update");
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
        public Int32 UpdateBulkData(List<UserLoginHistory> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserLoginHistory>("update");
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
        public UserLoginHistory DeleteData(Int32 Id)
        {
            UserLoginHistory obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<UserLoginHistory>("update");
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
                    UserLoginHistory obj = GetSingle(Id);
                    query += obj.ObjectToQuery<UserLoginHistory>("update");
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
        public UserLoginHistory GetSingle(Int32 Id)
        {
            UserLoginHistory obj = new UserLoginHistory();
            try
            {
                string query = "select * from user_login_history where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                obj = (from x in dt.AsEnumerable()
                        select new UserLoginHistory
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            LoginDate = x.Field<DateTime>("login_date"),
                            LoginIp = x.Field<String>("login_ip"),
                        }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserLoginHistory> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                string query = "select * from user_login_history";
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
                        select new UserLoginHistory
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                         LoginDate = x.Field<DateTime>("login_date"),
                            LoginIp = x.Field<String>("login_ip"),
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
                string query = "select count(*) from user_login_history";
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
        public List<UserLoginHistory> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<UserLoginHistory>(propertyName);
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                string query = "select * from user_login_history where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from user_login_history where " + columnName + " != '" + propertyValue + "'";
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
                        select new UserLoginHistory
                        {
                         Id = x.Field<Int32>("id"),
                         UserId = x.Field<Int32>("user_id"),
                         LoginDate = x.Field<DateTime>("login_date"),
                         LoginIp = x.Field<String>("login_ip"),
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
            string columnName = Converter.GetColumnNameByPropertyName<UserLoginHistory>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from user_login_history where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from user_login_history where " + columnName + " != '" + propertyValue + "'";
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
        public List<UserLoginHistory> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                string query = "select * from user_login_history where " + filter +"";
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
                        select new UserLoginHistory
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            LoginDate = x.Field<DateTime>("login_date"),
                            LoginIp = x.Field<String>("login_ip"),
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
                string query = "select count(*) from user_login_history where " + filter;
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
        public List<UserLoginHistory> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                string query = "select * from user_login_history limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new UserLoginHistory
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            LoginDate = x.Field<DateTime>("login_date"),
                            LoginIp = x.Field<String>("login_ip"),
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
