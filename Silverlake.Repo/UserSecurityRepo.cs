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
    public class UserSecurityRepo : IUserSecurityRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public UserSecurity PostData(UserSecurity obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserSecurity>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<UserSecurity> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserSecurity>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public UserSecurity UpdateData(UserSecurity obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserSecurity>("update");
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
        public Int32 UpdateBulkData(List<UserSecurity> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserSecurity>("update");
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
        public UserSecurity DeleteData(Int32 Id)
        {
            UserSecurity obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<UserSecurity>("update");
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
                    UserSecurity obj = GetSingle(Id);
                    query += obj.ObjectToQuery<UserSecurity>("update");
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
        public UserSecurity GetSingle(Int32 Id)
        {
            UserSecurity obj = new UserSecurity();
            try
            {
                string query = "select * from user_securities where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new UserSecurity
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            UserSecurityTypeId = x.Field<Int32>("user_security_type_id"),
                            Url = x.Field<String>("url"),
                            Value = x.Field<String>("value"),
                            VerificationPin = x.Field<Int32?>("verification_pin") == null ? 0 : x.Field<Int32>("verification_pin"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            IsVerified = x.Field<Int32>("is_verified"),
                            VerifiedDate = x.Field<DateTime?>("verified_date"),
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
        public List<UserSecurity> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                string query = "select * from user_securities";
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
                        select new UserSecurity
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            UserSecurityTypeId = x.Field<Int32>("user_security_type_id"),
                            Url = x.Field<String>("url"),
                            Value = x.Field<String>("value"),
                         VerificationPin = x.Field<Int32?>("verification_pin") == null ? 0 : x.Field<Int32>("verification_pin"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                            IsVerified = x.Field<Int32>("is_verified"),
                         VerifiedDate = x.Field<DateTime?>("verified_date"),
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
                string query = "select count(*) from user_securities";
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
        public List<UserSecurity> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<UserSecurity>(propertyName);
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                string query = "select * from user_securities where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from user_securities where " + columnName + " != '" + propertyValue + "'";
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
                        select new UserSecurity
                        {
                         Id = x.Field<Int32>("id"),
                         UserId = x.Field<Int32>("user_id"),
                         UserSecurityTypeId = x.Field<Int32>("user_security_type_id"),
                         Url = x.Field<String>("url"),
                         Value = x.Field<String>("value"),
                         VerificationPin = x.Field<Int32?>("verification_pin") == null ? 0 : x.Field<Int32>("verification_pin"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         IsVerified = x.Field<Int32>("is_verified"),
                         VerifiedDate = x.Field<DateTime?>("verified_date"),
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
            string columnName = Converter.GetColumnNameByPropertyName<UserSecurity>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from user_securities where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from user_securities where " + columnName + " != '" + propertyValue + "'";
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
        public List<UserSecurity> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                string query = "select * from user_securities where " + filter +"";
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
                        select new UserSecurity
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            UserSecurityTypeId = x.Field<Int32>("user_security_type_id"),
                            Url = x.Field<String>("url"),
                            Value = x.Field<String>("value"),
                            VerificationPin = x.Field<Int32?>("verification_pin") == null ? 0 : x.Field<Int32>("verification_pin"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            IsVerified = x.Field<Int32>("is_verified"),
                            VerifiedDate = x.Field<DateTime?>("verified_date"),
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
                string query = "select count(*) from user_securities where " + filter;
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
        public List<UserSecurity> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                string query = "select * from user_securities limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new UserSecurity
                        {
                            Id = x.Field<Int32>("id"),
                            UserId = x.Field<Int32>("user_id"),
                            UserSecurityTypeId = x.Field<Int32>("user_security_type_id"),
                            Url = x.Field<String>("url"),
                            Value = x.Field<String>("value"),
                            VerificationPin = x.Field<Int32?>("verification_pin") == null ? 0 : x.Field<Int32>("verification_pin"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            IsVerified = x.Field<Int32>("is_verified"),
                            VerifiedDate = x.Field<DateTime?>("verified_date"),
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
