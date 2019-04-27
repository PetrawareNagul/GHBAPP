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
    public class UserTypeRepo : IUserTypeRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public UserType PostData(UserType obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserType>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<UserType> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserType>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public UserType UpdateData(UserType obj)
        {
            try
            {
                string query = obj.ObjectToQuery<UserType>("update");
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
        public Int32 UpdateBulkData(List<UserType> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<UserType>("update");
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
        public UserType DeleteData(Int32 Id)
        {
            UserType obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<UserType>("update");
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
                    UserType obj = GetSingle(Id);
                    query += obj.ObjectToQuery<UserType>("update");
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
        public UserType GetSingle(Int32 Id)
        {
            UserType obj = new UserType();
            try
            {
                string query = "select * from user_types where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new UserType
                        {
                            Id = x.Field<Int32>("id"),
                            Name = x.Field<String>("name"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
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
        public List<UserType> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                string query = "select * from user_types";
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
                        select new UserType
                        {
                            Id = x.Field<Int32>("id"),
                            Name = x.Field<String>("name"),
                            CreatedBy = x.Field<Int32>("created_by"),
                         CreatedOn = x.Field<DateTime>("created_on"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedOn = x.Field<DateTime?>("updated_on"),
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
                string query = "select count(*) from user_types";
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
        public List<UserType> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<UserType>(propertyName);
            List<UserType> objs = new List<UserType>();
            try
            {
                string query = "select * from user_types where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from user_types where " + columnName + " != '" + propertyValue + "'";
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
                        select new UserType
                        {
                         Id = x.Field<Int32>("id"),
                         Name = x.Field<String>("name"),
                         CreatedBy = x.Field<Int32>("created_by"),
                         CreatedOn = x.Field<DateTime>("created_on"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedOn = x.Field<DateTime?>("updated_on"),
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
            string columnName = Converter.GetColumnNameByPropertyName<UserType>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from user_types where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from user_types where " + columnName + " != '" + propertyValue + "'";
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
        public List<UserType> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                string query = "select * from user_types where " + filter +"";
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
                        select new UserType
                        {
                            Id = x.Field<Int32>("id"),
                            Name = x.Field<String>("name"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
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
                string query = "select count(*) from user_types where " + filter;
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
        public List<UserType> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                string query = "select * from user_types limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new UserType
                        {
                            Id = x.Field<Int32>("id"),
                            Name = x.Field<String>("name"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedOn = x.Field<DateTime>("created_on"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedOn = x.Field<DateTime?>("updated_on"),
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
