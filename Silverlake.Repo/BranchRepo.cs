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
    public class BranchRepo : IBranchRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public Branch PostData(Branch obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Branch>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<Branch> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Branch>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Branch UpdateData(Branch obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Branch>("update");
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
        public Int32 UpdateBulkData(List<Branch> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Branch>("update");
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
        public Branch DeleteData(Int32 Id)
        {
            Branch obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<Branch>("update");
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
                    Branch obj = GetSingle(Id);
                    query += obj.ObjectToQuery<Branch>("update");
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
        public Branch GetSingle(Int32 Id)
        {
            Branch obj = new Branch();
            try
            {
                string query = "select * from branches where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new Branch
                        {
                            Id = x.Field<Int32>("id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            IsAll = x.Field<Int32>("is_all"),
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
        public List<Branch> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                string query = "select * from branches";
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
                        select new Branch
                        {
                            Id = x.Field<Int32>("id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            IsAll = x.Field<Int32>("is_all"),
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
                string query = "select count(*) from branches";
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
        public List<Branch> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<Branch>(propertyName);
            List<Branch> objs = new List<Branch>();
            try
            {
                string query = "select * from branches where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from branches where " + columnName + " != '" + propertyValue + "'";
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
                        select new Branch
                        {
                         Id = x.Field<Int32>("id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            Code = x.Field<String>("code"),
                         Name = x.Field<String>("name"),
                            IsAll = x.Field<Int32>("is_all"),
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
            string columnName = Converter.GetColumnNameByPropertyName<Branch>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from branches where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from branches where " + columnName + " != '" + propertyValue + "'";
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
        public List<Branch> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                string query = "select * from branches where " + filter +"";
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
                        select new Branch
                        {
                            Id = x.Field<Int32>("id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            IsAll = x.Field<Int32>("is_all"),
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
                string query = "select count(*) from branches where " + filter;
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
        public List<Branch> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                string query = "select * from branches limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Branch
                        {
                            Id = x.Field<Int32>("id"),
                            CompanyId = x.Field<Int32>("company_id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
                            IsAll = x.Field<Int32>("is_all"),
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
