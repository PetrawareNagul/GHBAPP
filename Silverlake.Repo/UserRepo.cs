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
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Silverlake.Repo
{
    public class UserRepo : IUserRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();

        public User PostData(User obj)
        {
            try
            {
                string query = obj.ObjectToQuery<User>("insert") + "SELECT SCOPE_IDENTITY()";
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                //cmd.ExecuteNonQuery();
                obj.Id = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<User> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<User>("insert") + "SELECT SCOPE_IDENTITY()";
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public User UpdateData(User obj)
        {
            try
            {
                string query = obj.ObjectToQuery<User>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public Int32 UpdateBulkData(List<User> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<User>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public User DeleteData(Int32 Id)
        {
            User obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<User>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
                    User obj = GetSingle(Id);
                    query += obj.ObjectToQuery<User>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public User GetSingle(Int32 Id)
        {
            User obj = new User();
            try
            {
                string query = "select * from users where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                //SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                       select new User
                       {
                           Id = x.Field<Int32>("id"),
                           UserRoleId = x.Field<Int32>("user_role_id"),
                           Username = x.Field<String>("username"),
                           EmailId = x.Field<String>("email_id"),
                           MobileNumber = x.Field<String>("mobile_number"),
                           Password = x.Field<String>("password"),
                           TransPwd = x.Field<String>("trans_pwd"),
                           UniqueKey = x.Field<String>("unique_key"),
                           IsOnline = (x.Field<Int32>("is_online")),
                           IsActive = (x.Field<Int32>("is_active")),
                           IsPrimary = (x.Field<Int32>("is_primary")),
                           RegisterIp = x.Field<String>("register_ip"),
                           LastLoginOn = x.Field<DateTime?>("last_login_on"),
                           LastLoginIp = x.Field<String>("last_login_ip"),
                           CreatedBy = x.Field<Int32>("created_by"),
                           CreatedOn = x.Field<DateTime>("created_on"),
                           UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                           UpdatedOn = x.Field<DateTime?>("updated_on"),
                           Status = x.Field<Int32>("status"),
                           ApiAuthToken = x.Field<String>("api_auth_token"),
                           BranchId = x.Field<Int32>("branch_id"),
                           DepartmentId = x.Field<Int32>("department_id"),
                           CompanyId = x.Field<Int32>("company_id"),
                           IsPasswordReset = x.Field<Int32>("is_password_reset"),
                           LastSyncDate = x.Field<DateTime?>("last_sync_date"),
                            IsAll = x.Field<Int32>("is_all"),
                       }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<User> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<User> objs = new List<User>();
            try
            {
                string query = "select * from users";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                //SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new User
                        {
                            Id = x.Field<Int32>("id"),
                            UserRoleId = x.Field<Int32>("user_role_id"),
                            Username = x.Field<String>("username"),
                            EmailId = x.Field<String>("email_id"),
                            MobileNumber = x.Field<String>("mobile_number"),
                            Password = x.Field<String>("password"),
                            TransPwd = x.Field<String>("trans_pwd"),
                            UniqueKey = x.Field<String>("unique_key"),
                            IsOnline = (x.Field<Int32>("is_online")),
                            IsActive = (x.Field<Int32>("is_active")),
                            IsPrimary = (x.Field<Int32>("is_primary")),
                            RegisterIp = x.Field<String>("register_ip"),
                            LastLoginOn = x.Field<DateTime?>("last_login_on"),
                            LastLoginIp = x.Field<String>("last_login_ip"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
                            Status = x.Field<Int32>("status"),
                            ApiAuthToken = x.Field<String>("api_auth_token"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            IsPasswordReset = x.Field<Int32>("is_password_reset"),
                            LastSyncDate = x.Field<DateTime?>("last_sync_date"),
                            IsAll = x.Field<Int32>("is_all"),
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
                string query = "select count(*) from users";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<User> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<User>(propertyName);
            List<User> objs = new List<User>();
            try
            {
                string query = "select * from users where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select * from users where " + columnName + " != '" + propertyValue + "'";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                //SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new User
                        {
                            Id = x.Field<Int32>("id"),
                            UserRoleId = x.Field<Int32>("user_role_id"),
                            Username = x.Field<String>("username"),
                            EmailId = x.Field<String>("email_id"),
                            MobileNumber = x.Field<String>("mobile_number"),
                            Password = x.Field<String>("password"),
                            TransPwd = x.Field<String>("trans_pwd"),
                            UniqueKey = x.Field<String>("unique_key"),
                            IsOnline = (x.Field<Int32>("is_online")),
                            IsActive = (x.Field<Int32>("is_active")),
                            IsPrimary = (x.Field<Int32>("is_primary")),
                            RegisterIp = x.Field<String>("register_ip"),
                            LastLoginOn = x.Field<DateTime?>("last_login_on"),
                            LastLoginIp = x.Field<String>("last_login_ip"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
                            Status = x.Field<Int32>("status"),
                            ApiAuthToken = x.Field<String>("api_auth_token"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            IsPasswordReset = x.Field<Int32>("is_password_reset"),
                            LastSyncDate = x.Field<DateTime?>("last_sync_date"),
                            IsAll = x.Field<Int32>("is_all"),
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
            string columnName = Converter.GetColumnNameByPropertyName<User>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from users where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select count(*) from users where " + columnName + " != '" + propertyValue + "'";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<User> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<User> objs = new List<User>();
            try
            {
                string query = "select * from users where " + filter + "";
                if (isOrderByDesc)
                    query += " order by ID desc";
                else
                    query += " order by ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                //SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new User
                        {
                            Id = x.Field<Int32>("id"),
                            UserRoleId = x.Field<Int32>("user_role_id"),
                            Username = x.Field<String>("username"),
                            EmailId = x.Field<String>("email_id"),
                            MobileNumber = x.Field<String>("mobile_number"),
                            Password = x.Field<String>("password"),
                            TransPwd = x.Field<String>("trans_pwd"),
                            UniqueKey = x.Field<String>("unique_key"),
                            IsOnline = (x.Field<Int32>("is_online")),
                            IsActive = (x.Field<Int32>("is_active")),
                            IsPrimary = (x.Field<Int32>("is_primary")),
                            RegisterIp = x.Field<String>("register_ip"),
                            LastLoginOn = x.Field<DateTime?>("last_login_on"),
                            LastLoginIp = x.Field<String>("last_login_ip"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
                            Status = x.Field<Int32>("status"),
                            ApiAuthToken = x.Field<String>("api_auth_token"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            IsPasswordReset = x.Field<Int32>("is_password_reset"),
                            LastSyncDate = x.Field<DateTime?>("last_sync_date"),
                            IsAll = x.Field<Int32>("is_all"),
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
                string query = "select count(*) from users where " + filter;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<User> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<User> objs = new List<User>();
            try
            {
                string query = "select * from users limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                //SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                //SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new User
                        {
                            Id = x.Field<Int32>("id"),
                            UserRoleId = x.Field<Int32>("user_role_id"),
                            Username = x.Field<String>("username"),
                            EmailId = x.Field<String>("email_id"),
                            MobileNumber = x.Field<String>("mobile_number"),
                            Password = x.Field<String>("password"),
                            TransPwd = x.Field<String>("trans_pwd"),
                            UniqueKey = x.Field<String>("unique_key"),
                            IsOnline = (x.Field<Int32>("is_online")),
                            IsActive = (x.Field<Int32>("is_active")),
                            IsPrimary = (x.Field<Int32>("is_primary")),
                            RegisterIp = x.Field<String>("register_ip"),
                            LastLoginOn = x.Field<DateTime?>("last_login_on"),
                            LastLoginIp = x.Field<String>("last_login_ip"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
                            Status = x.Field<Int32>("status"),
                            ApiAuthToken = x.Field<String>("api_auth_token"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            IsPasswordReset = x.Field<Int32>("is_password_reset"),
                            LastSyncDate = x.Field<DateTime?>("last_sync_date"),
                            IsAll = x.Field<Int32>("is_all"),
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
