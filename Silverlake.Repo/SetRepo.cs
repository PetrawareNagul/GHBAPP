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
    public class SetRepo : ISetRepo
    {
        SQLDBConnect mySQLDBConnect = new SQLDBConnect();
        public Set PostData(Set obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Set>("insert") + "SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                // cmd.ExecuteNonQuery();
                obj.Id = Convert.ToInt32(cmd.ExecuteScalar());
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<Set> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Set>("insert") + "SELECT SCOPE_IDENTITY()";
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
        public Set UpdateData(Set obj)
        {
            try
            {
                string query = obj.ObjectToQuery<Set>("update");
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                cmd.ExecuteNonQuery();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("Set UpdateData " + ex.Message + " - Exception");
                throw ex;
                //Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<Set> objs)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                objs.ForEach(obj =>
                {
                    query += obj.ObjectToQuery<Set>("update");
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
        public Set DeleteData(Int32 Id)
        {
            Set obj = GetSingle(Id);
            try
            {
                string query = obj.ObjectToQuery<Set>("update");
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
        public Int32 DeleteBulkData(List<Int32> Ids)
        {
            Int32 result = 0;
            try
            {
                string query = "";
                Ids.ForEach(Id =>
                {
                    Set obj = GetSingle(Id);
                    query += obj.ObjectToQuery<Set>("update");
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
        public Set GetSingle(Int32 Id)
        {
            Set obj = new Set();
            try
            {
                string query = "select * from sets where ID = " + Id + "";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                obj = (from x in dt.AsEnumerable()
                       select new Set
                       {
                           Id = x.Field<Int32>("id"),
                           BatchId = x.Field<Int32>("batch_id"),
                           SetKey = x.Field<String>("set_key"),
                           AaNo = x.Field<String>("aa_no"),
                           AccountNo = x.Field<String>("account_no"),
                           SetXmlPath = x.Field<String>("set_xml_path"),
                           IsReleased = x.Field<Int32>("is_released"),
                           CreatedBy = x.Field<Int32>("created_by"),
                           CreatedDate = x.Field<DateTime>("created_date"),
                           UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                           UpdatedDate = x.Field<DateTime?>("updated_date"),
                           SetStatus = x.Field<Int32>("set_status"),
                           Status = x.Field<Int32>("status"),
                           Remarks = x.Field<String>("remarks"),
                       }).ToList().FirstOrDefault();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Set> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select * from sets";
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
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
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
                string query = "select count(*) from sets";
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
        public List<Set> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            string columnName = Converter.GetColumnNameByPropertyName<Set>(propertyName);
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select * from sets where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select * from sets where " + columnName + " != '" + propertyValue + "'";
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
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
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
            string columnName = Converter.GetColumnNameByPropertyName<Set>(propertyName);
            Int32 count = 0;
            try
            {
                string query = "select count(*) from sets where " + columnName + " = '" + propertyValue + "'";
                if (!isEqual)
                    query = "select count(*) from sets where " + columnName + " != '" + propertyValue + "'";
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
        public List<Set> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select * from sets where " + filter + "";
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
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
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
                string query = "select count(*) from sets where " + filter;
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
        public List<Set> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select * from sets limit " + skip + ", " + take;
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
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

        public List<Set> GetDataByFilterNew(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select s.ID,s.batch_id,s.set_key,s.aa_no,s.account_no,s.set_xml_path,s.is_released,s.created_by,s.created_date,s.updated_by,s.updated_date,s.set_status,s.status, LEFT (s.remarks, 100) as remarks from sets s " + filter + "";
                if (isOrderByDesc)
                    query += " order by s.ID desc";
                else
                    query += " order by s.ID";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY;";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }

        public Int32 GetCountByFilterNew(string filter)
        {
            Int32 count = 0;
            try
            {
                string query = "select count(*) from sets s " + filter;
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

        public void UpdateStatusForMfiles(List<Set> setList)
        {
            try
            {
                mySQLDBConnect.OpenConnection();
                foreach (var item in setList)
                {
                    string query = "update sets set is_released=" + item.IsReleased + ",status=" + item.Status + ",remarks='" + item.Remarks + "',updated_by=0,updated_date='" + item.UpdatedDate + "' where ID=" + item.Id;
                    SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                    cmd.ExecuteNonQuery();
                }
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public List<Set> GetSetsForMfiles()
        {
            List<Set> objs = new List<Set>();
            try
            {
                string query = "select b.ID,b.batch_id,b.set_key,b.aa_no,b.account_no,b.set_xml_path,b.is_released,b.created_by,b.created_date,b.updated_by,b.updated_date,b.set_status,b.status, LEFT (b.remarks, 100) as remarks from batches a inner join sets b on a.ID=b.batch_id where a.stage_id=6 and a.status=1 and b.is_released=1 and b.status=1";
                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new Set
                        {
                            Id = x.Field<Int32>("id"),
                            BatchId = x.Field<Int32>("batch_id"),
                            SetKey = x.Field<String>("set_key"),
                            AaNo = x.Field<String>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            SetXmlPath = x.Field<String>("set_xml_path"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedBy = x.Field<Int32>("created_by"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                            UpdatedBy = x.Field<Int32?>("updated_by") == null ? 0 : x.Field<Int32>("updated_by"),
                            UpdatedDate = x.Field<DateTime?>("updated_date"),
                            SetStatus = x.Field<Int32>("set_status"),
                            Status = x.Field<Int32>("status"),
                            Remarks = x.Field<String>("remarks"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("GetSetsForMfiles Repo " + ex.Message);
            }
            return objs;

        }

        public List<DocTypeSetModel> GetSetsForMfilesAccount(int department_id, string doc_type, string aano, int skip, int take)
        {
            List<DocTypeSetModel> objs = new List<DocTypeSetModel>();
            try
            {
                string query = " select b.batch_id,c.set_id,a.branch_id,a.department_id,a.batch_no,a.batch_user,c.doc_type,b.aa_no,b.account_no,c.page_count,b.is_released,b.created_date,b.updated_date from batches a inner join sets b on a.ID=b.batch_id " +
                               " inner join set_documents c on b.ID = c.set_id " +
                               " where a.status = 1";
                if (department_id != 0)
                    query += " and a.department_id=" + department_id;
                if (!string.IsNullOrEmpty(doc_type) && doc_type != "0")
                    query += " and c.doc_type='" + doc_type + "'";
                if (!string.IsNullOrEmpty(aano))
                    query += " and (aa_no like '%" + aano + "%' or account_no like '%" + aano + "%')";
                query += " group by batch_id,set_id,branch_id,department_id,batch_no,batch_user,doc_type,aa_no,account_no,page_count,is_released,b.created_date,b.updated_date ";
                query += " order by updated_date desc ";
                if (take != 0)
                    query += " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY";


                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                objs = (from x in dt.AsEnumerable()
                        select new DocTypeSetModel
                        {
                            BatchId = x.Field<Int32>("batch_id"),
                            SetId = x.Field<Int32>("set_id"),
                            BranchId = x.Field<Int32>("branch_id"),
                            DepartmentId = x.Field<Int32>("department_id"),
                            BatchNo = x.Field<string>("batch_no"),
                            BatchUser = x.Field<string>("batch_user"),
                            DocType = x.Field<string>("doc_type"),
                            AANO = x.Field<string>("aa_no"),
                            AccountNo = x.Field<String>("account_no"),
                            PageCount = x.Field<Int32>("page_count"),
                            IsReleased = x.Field<Int32>("is_released"),
                            CreatedDate = x.Field<DateTime>("created_date"),
                        }).ToList();
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("GetSetsForMfiles Repo " + ex.Message);
            }
            return objs;

        }

        public long GetSetsForMfilesAccountCount(int departmentId, string docType, string aano)
        {
            long totalCount = 0;
            try
            {
                string query = " select count(*) as setscount from batches a inner join sets b on a.ID=b.batch_id " +
                               " inner join set_documents c on b.ID = c.set_id " +
                               " where a.status = 1";
                if (departmentId != 0)
                    query += " and a.department_id=" + departmentId;
                if (!string.IsNullOrEmpty(docType) && docType != "0")
                    query += " and c.doc_type='" + docType + "'";
                if (!string.IsNullOrEmpty(aano))
                    query += " and (aa_no like '%" + aano + "%' or account_no like '%" + aano + "%')";
                query += " group by batch_id,set_id,branch_id,department_id,batch_no,batch_user,doc_type,aa_no,account_no,page_count,is_released,b.created_date ";

                SqlCommand cmd = new SqlCommand(query, mySQLDBConnect.connection);
                mySQLDBConnect.OpenConnection();
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(dt); dA.Dispose();
                totalCount = dt.Rows.Count;
                mySQLDBConnect.CloseConnection();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("GetSetsForMfiles Repo " + ex.Message);
            }
            return totalCount;

        }
    }
}
