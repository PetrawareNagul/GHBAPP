using Silverlake.Repo;
using Silverlake.Repo.IRepo;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Service
{
    public class BatchLogService : IBatchLogService
    {
        private static readonly Lazy<IBatchLogRepo> lazy = new Lazy<IBatchLogRepo>(() => new BatchLogRepo());
        public static IBatchLogRepo IBatchLogRepo { get { return lazy.Value; } }
        public BatchLog PostData(BatchLog obj)
        {
            try
            {
                obj = IBatchLogRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<BatchLog> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBatchLogRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchLog UpdateData(BatchLog obj)
        {
            try
            {
                obj = IBatchLogRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<BatchLog> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBatchLogRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchLog DeleteData(Int32 Id)
        {
            BatchLog obj = new BatchLog();
            try
            {
                obj = IBatchLogRepo.DeleteData(Id);
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
                result = IBatchLogRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchLog GetSingle(Int32 Id)
        {
            BatchLog obj = new BatchLog();
            try
            {
                obj = IBatchLogRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BatchLog> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                objs = IBatchLogRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBatchLogRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchLog> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                objs = IBatchLogRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public Int32 GetCountByPropertyName(string propertyName, string propertyValue, bool isEqual)
        {
            Int32 count = 0;
            try
            {
                count = IBatchLogRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchLog> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                objs = IBatchLogRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBatchLogRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchLog> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<BatchLog> objs = new List<BatchLog>();
            try
            {
                objs = IBatchLogRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<BatchLog> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            string sortBy = "";
            bool sortDir = true;
            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }
            List<BatchLog> BatchLogSearch = new List<BatchLog>();
            List<BatchLog> BatchLogs = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BatchLogSearch.AddRange(BatchLogs.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BatchLogSearch.Count == 0)
                BatchLogSearch = BatchLogs;
            BatchLogSearch = sortDir ? BatchLogSearch.OrderBy(x => typeof(BatchLog).GetProperty(sortBy).GetValue(x)).ToList() : BatchLogSearch.OrderByDescending(x => typeof(BatchLog).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BatchLogSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BatchLogSearch.Count();
            totalResultsCount = BatchLogs.Count();
            if (result == null)
            {
                return new List<BatchLog>();
            }
            return result;
        }
    }
}
