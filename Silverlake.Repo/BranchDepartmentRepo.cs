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
    public class BranchDepartmentRepo : IBranchDepartmentRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public BranchDepartment PostData(BranchDepartment obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BranchDepartment>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<BranchDepartment> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BranchDepartment>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public BranchDepartment UpdateData(BranchDepartment obj)
        {
            try
            {
                string query = obj.ObjectToQuery<BranchDepartment>("update");
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
        public Int32 UpdateBulkData(List<BranchDepartment> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<BranchDepartment>("update");
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
        public BranchDepartment DeleteData(Int32 Id)
        {
            BranchDepartment obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<BranchDepartment>("update");
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
                    BranchDepartment obj = GetSingle(Id);
                    query += obj.ObjectToQuery<BranchDepartment>("update");
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
        public BranchDepartment GetSingle(Int32 Id)
        {
            BranchDepartment obj = new BranchDepartment();
            try
            {
                string query = "select * from branch_departments where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                obj = (from x in dt.AsEnumerable()
                        select new BranchDepartment
                        {
                            Id = x.Field<Int32>("id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Status = x.Field<Int32>("status"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                        }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BranchDepartment> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                string query = "select * from branch_departments";
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
                        select new BranchDepartment
                        {
                            Id = x.Field<Int32>("id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Status = x.Field<Int32>("status"),
                            CreatedBy = x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from branch_departments";
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
        public List<BranchDepartment> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<BranchDepartment>(propertyName);
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                string query = "select * from branch_departments where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from branch_departments where " + columnName + " != '" + propertyValue + "'";
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
                        select new BranchDepartment
                        {
                         Id = x.Field<Int32>("id"),
                         BranchId = x.Field<Int32>("branch_id"),
                         DepartmentId = x.Field<Int32>("department_id"),
                         Status = x.Field<Int32>("status"),
                         CreatedBy = x.Field<Int32>("created_by"),
                         CreatedDate = x.Field<DateTime>("created_date"),
                         UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                         UpdatedDate = x.Field<DateTime?>("updated_date"),
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
            string columnName = Converter.GetColumnNameByPropertyName<BranchDepartment>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from branch_departments where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from branch_departments where " + columnName + " != '" + propertyValue + "'";
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
        public List<BranchDepartment> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                string query = "select * from branch_departments where " + filter +"";
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
                        select new BranchDepartment
                        {
                            Id = x.Field<Int32>("id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Status = x.Field<Int32>("status"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from branch_departments where " + filter;
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
        public List<BranchDepartment> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                string query = "select * from branch_departments limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);
                objs = (from x in dt.AsEnumerable()
                        select new BranchDepartment
                        {
                            Id = x.Field<Int32>("id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            Status = x.Field<Int32>("status"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
