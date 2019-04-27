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
    public class StageRepo : IStageRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public Stage PostData(Stage obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Stage>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<Stage> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Stage>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Stage UpdateData(Stage obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Stage>("update");
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
        public Int32 UpdateBulkData(List<Stage> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Stage>("update");
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
        public Stage DeleteData(Int32 Id)
        {
            Stage obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<Stage>("update");
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
                    Stage obj = GetSingle(Id);
                    query += obj.ObjectToQuery<Stage>("update");
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
        public Stage GetSingle(Int32 Id)
        {
            Stage obj = new Stage();
            try
            {
                string query = "select * from stages where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                        select new Stage
                        {
                            Id = x.Field<Int32>("id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
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
        public List<Stage> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Stage> objs = new List<Stage>();
            try
            {
                string query = "select * from stages";
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
                        select new Stage
                        {
                            Id = x.Field<Int32>("id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
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
                string query = "select count(*) from stages";
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
        public List<Stage> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<Stage>(propertyName);
            List<Stage> objs = new List<Stage>();
            try
            {
                string query = "select * from stages where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select * from stages where " + columnName + " != '" + propertyValue + "'";
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
                        select new Stage
                        {
                         Id = x.Field<Int32>("id"),
                         Code = x.Field<String>("code"),
                         Name = x.Field<String>("name"),
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
            string columnName = Converter.GetColumnNameByPropertyName<Stage>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from stages where " + columnName + " = '" + propertyValue + "'";
                if(!isEqual)
                    query = "select count(*) from stages where " + columnName + " != '" + propertyValue + "'";
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
        public List<Stage> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Stage> objs = new List<Stage>();
            try
            {
                string query = "select * from stages where " + filter +"";
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
                        select new Stage
                        {
                            Id = x.Field<Int32>("id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
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
                string query = "select count(*) from stages where " + filter;
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
        public List<Stage> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<Stage> objs = new List<Stage>();
            try
            {
                string query = "select * from stages limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt);dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Stage
                        {
                            Id = x.Field<Int32>("id"),
                            Code = x.Field<String>("code"),
                            Name = x.Field<String>("name"),
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
