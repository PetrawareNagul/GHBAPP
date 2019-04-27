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
    public class BatchHeaderService : IBatchHeaderService
    {
        private static readonly Lazy<IBatchHeaderRepo> lazy = new Lazy<IBatchHeaderRepo>(() => new BatchHeaderRepo());
        public static IBatchHeaderRepo IBatchHeaderRepo { get { return lazy.Value; } }
        public BatchHeader PostData(BatchHeader obj)
        {
            try
            {
                obj = IBatchHeaderRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<BatchHeader> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBatchHeaderRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchHeader UpdateData(BatchHeader obj)
        {
            try
            {
                obj = IBatchHeaderRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<BatchHeader> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBatchHeaderRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchHeader DeleteData(Int32 Id)
        {
            BatchHeader obj = new BatchHeader();
            try
            {
                obj = IBatchHeaderRepo.DeleteData(Id);
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
                result = IBatchHeaderRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BatchHeader GetSingle(Int32 Id)
        {
            BatchHeader obj = new BatchHeader();
            try
            {
                obj = IBatchHeaderRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BatchHeader> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                objs = IBatchHeaderRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBatchHeaderRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchHeader> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                objs = IBatchHeaderRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IBatchHeaderRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchHeader> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                objs = IBatchHeaderRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBatchHeaderRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BatchHeader> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<BatchHeader> objs = new List<BatchHeader>();
            try
            {
                objs = IBatchHeaderRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<BatchHeader> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<BatchHeader> BatchHeaderSearch = new List<BatchHeader>();
            List<BatchHeader> BatchHeaders = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BatchHeaderSearch.AddRange(BatchHeaders.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BatchHeaderSearch.Count == 0)
                BatchHeaderSearch = BatchHeaders;
            BatchHeaderSearch = sortDir ? BatchHeaderSearch.OrderBy(x => typeof(BatchHeader).GetProperty(sortBy).GetValue(x)).ToList() : BatchHeaderSearch.OrderByDescending(x => typeof(BatchHeader).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BatchHeaderSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BatchHeaderSearch.Count();
            totalResultsCount = BatchHeaders.Count();
            if (result == null)
            {
                return new List<BatchHeader>();
            }
            return result;
        }
    }
}
