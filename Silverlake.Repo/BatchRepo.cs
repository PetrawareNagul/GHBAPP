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
    public class BatchRepo : IBatchRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public Batch PostData(Batch obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Batch>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Int32 PostBulkData(List<Batch> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Batch>("insert") + "SELECT SCOPE_IDENTITY()";
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public Batch UpdateData(Batch obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Batch>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public Int32 UpdateBulkData(List<Batch> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Batch>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public Batch DeleteData(Int32 Id)
        {
            Batch obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<Batch>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("BatchREpo DeleteData " + ex.Message);

                // Console.Write(ex.ToString());
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
                    Batch obj = GetSingle(Id);
                    query += obj.ObjectToQuery<Batch>("update");
                });
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public Batch GetSingle(Int32 Id)
        {
            Batch obj = new Batch();
            try
            {
                string query = "select * from batches where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                       select new Batch
                       {
                           Id = x.Field<Int32>("id"),
                           StageId = x.Field<Int32>("stage_id"),
                           BranchId = x.Field<Int32>("branch_id"),
                           DepartmentId = x.Field<Int32>("department_id"),
                           BatchKey = x.Field<String>("batch_key"),
                           BatchNo = x.Field<String>("batch_no"),
                           BatchUser = x.Field<String>("batch_user"),
                           BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                           IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                           BatchStatus = x.Field<Int32>("batch_status"),
                           Status = x.Field<Int32>("status"),
                           CreatedDate = x.Field<DateTime>("created_date"),
                           UpdatedDate = x.Field<DateTime?>("updated_date"),
                       }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("BatchREpo GetSingle " + ex.Message);

                // Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Batch> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Batch> objs = new List<Batch>();
            try
            {
                string query = "select * from batches";
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
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Batch
                        {
                            Id = x.Field<Int32>("id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BatchKey = x.Field<String>("batch_key"),
                            BatchNo = x.Field<String>("batch_no"),
                            BatchUser = x.Field<String>("batch_user"),
                            BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                            IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                            BatchStatus = x.Field<Int32>("batch_status"),
                            Status = x.Field<Int32>("status"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from batches";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<Batch> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<Batch>(propertyName);
            List<Batch> objs = new List<Batch>();
            try
            {
                string query = "select * from batches where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select * from batches where " + columnName + " != '" + propertyValue + "'";
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
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Batch
                        {
                            Id = x.Field<Int32>("id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BatchKey = x.Field<String>("batch_key"),
                            BatchNo = x.Field<String>("batch_no"),
                            BatchUser = x.Field<String>("batch_user"),
                            BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                            IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                            BatchStatus = x.Field<Int32>("batch_status"),
                            Status = x.Field<Int32>("status"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
            string columnName = Converter.GetColumnNameByPropertyName<Batch>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from batches where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select count(*) from batches where " + columnName + " != '" + propertyValue + "'";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<Batch> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Batch> objs = new List<Batch>();
            try
            {
                string query = "select * from batches where " + filter + "";
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
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Batch
                        {
                            Id = x.Field<Int32>("id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BatchKey = x.Field<String>("batch_key"),
                            BatchNo = x.Field<String>("batch_no"),
                            BatchUser = x.Field<String>("batch_user"),
                            BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                            IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                            BatchStatus = x.Field<Int32>("batch_status"),
                            Status = x.Field<Int32>("status"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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
                string query = "select count(*) from batches where " + filter;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
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
        public List<Batch> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<Batch> objs = new List<Batch>();
            try
            {
                string query = "select * from batches limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Batch
                        {
                            Id = x.Field<Int32>("id"),
                            StageId = x.Field<Int32>("stage_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BatchKey = x.Field<String>("batch_key"),
                            BatchNo = x.Field<String>("batch_no"),
                            BatchUser = x.Field<String>("batch_user"),
                            BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                            IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                            BatchStatus = x.Field<Int32>("batch_status"),
                            Status = x.Field<Int32>("status"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
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

        public List<BatchesStagesCount> GetBatchStagesCount(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchesStagesCount> objs = new List<BatchesStagesCount>();
            try
            {
                string query = "select stage_id,department_id,branch_id,count(*) as count from batches where " + filter + " group by stage_id,department_id,branch_id ";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new BatchesStagesCount
                        {
                            StageId = x.Field<Int32>("stage_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            count = x.Field<Int32>("count")
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public BatchesInfo GetBatchesInfo(string filter, int skip, int take, bool isOrderByDesc)
        {
            BatchesInfo batchInfo = new BatchesInfo();
            try
            {
                string query = "select * from batches a inner join batch_logs b on a.ID=b.batch_id where " + filter + "";
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
                dA.Fill(dt); dA.Dispose();
                // batches
                var Batches = (from x in dt.AsEnumerable()
                               select new Batch
                               {
                                   Id = x.Field<Int32>("batch_id"),
                                   StageId = x.Field<Int32>("stage_id"),
                                   BranchId = x.Field<Int32>("branch_id"),
                                   DepartmentId = x.Field<Int32>("department_id"),
                                   BatchKey = x.Field<String>("batch_key"),
                                   BatchNo = x.Field<String>("batch_no"),
                                   BatchUser = x.Field<String>("batch_user"),
                                   BatchCount = x.Field<Int32?>("batch_count") == null ? 0 : x.Field<Int32>("batch_count"),
                                   IsBatchCountUpdated = x.Field<Int32>("is_batch_count_updated"),
                                   BatchStatus = x.Field<Int32>("batch_status"),
                                   Status = x.Field<Int32>("status"),
                                   CreatedDate = x.Field<DateTime>("created_date"),
                                   UpdatedDate = x.Field<DateTime?>("updated_date"),
                               }).ToList();
                batchInfo.Batches = Batches.Select(x => new Batch
                {
                    Id = x.Id,
                    StageId = x.StageId,
                    BranchId = x.BranchId,
                    DepartmentId = x.DepartmentId,
                    BatchKey = x.BatchKey,
                    BatchNo = x.BatchNo,
                    BatchUser = x.BatchUser,
                    BatchCount = x.BatchCount,
                    IsBatchCountUpdated = x.IsBatchCountUpdated,
                    BatchStatus = x.BatchStatus,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                }).Distinct().ToList();
                // batch logs
                batchInfo.BatcheLogs = (from x in dt.AsEnumerable()
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
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return batchInfo;
        }

        public List<StatisticModel> GetTotalCountbyDepartment(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<StatisticModel> objs = new List<StatisticModel>();
            try
            {
                string query = filter;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new StatisticModel
                        {
                            DepartmentId = x.Field<Int32>("department_id"),
                            count = x.Field<Int32>("count"),
                            SetCount = x.Field<Int32>("SetCount"),
                            DepartmentCode = x.Field<string>("DepartmentCode")
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }

        public List<StatisticModel> GetDaysCountbyDepartment(string query, int skip, int take, bool isOrderByDesc)
        {
            List<StatisticModel> objs = new List<StatisticModel>();
            try
            {
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new StatisticModel
                        {
                            DepartmentId = x.Field<Int32>("department_id"),
                            count = x.Field<Int32>("count"),
                            SetCount = x.Field<Int32>("SetCount"),
                            CreatedOn = x.Field<DateTime>("CreatedOn")
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
    }
}
