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
    public class BatchService : IBatchService
    {
        private static readonly Lazy<IBatchRepo> lazy = new Lazy<IBatchRepo>(() => new BatchRepo());
        public static IBatchRepo IBatchRepo { get { return lazy.Value; } }
        public Batch PostData(Batch obj)
        {
            try
            {
                obj = IBatchRepo.PostData(obj);
            }
            catch(Exception ex)
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
                result = IBatchRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Batch UpdateData(Batch obj)
        {
            try
            {
                obj = IBatchRepo.UpdateData(obj);
            }
            catch(Exception ex)
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
                result = IBatchRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Batch DeleteData(Int32 Id)
        {
            Batch obj = new Batch();
            try
            {
                obj = IBatchRepo.DeleteData(Id);
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
                result = IBatchRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
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
                obj = IBatchRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Batch> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Batch> objs = new List<Batch>();
            try
            {
                objs = IBatchRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBatchRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Batch> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Batch> objs = new List<Batch>();
            try
            {
                objs = IBatchRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IBatchRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
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
                objs = IBatchRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBatchRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Batch> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Batch> objs = new List<Batch>();
            try
            {
                objs = IBatchRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Batch> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Batch> BatchSearch = new List<Batch>();
            List<Batch> Batchs = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BatchSearch.AddRange(Batchs.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BatchSearch.Count == 0)
                BatchSearch = Batchs;
            BatchSearch = sortDir ? BatchSearch.OrderBy(x => typeof(Batch).GetProperty(sortBy).GetValue(x)).ToList() : BatchSearch.OrderByDescending(x => typeof(Batch).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BatchSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BatchSearch.Count();
            totalResultsCount = Batchs.Count();
            if (result == null)
            {
                return new List<Batch>();
            }
            return result;
        }

        public List<BatchesStagesCount> GetBatchStagesCount(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BatchesStagesCount> batchCounts = IBatchRepo.GetBatchStagesCount(filter,skip,take,isOrderByDesc);
            return batchCounts;
        }

        public BatchesInfo GetBatchesInfo(string filter, int skip, int take, bool isOrderByDesc)
        {
            return IBatchRepo.GetBatchesInfo(filter, skip, take, isOrderByDesc);
        }

        public List<StatisticModel> GetTotalCountbyDepartment(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<StatisticModel> batchCounts = IBatchRepo.GetTotalCountbyDepartment(filter, skip, take, isOrderByDesc);
            return batchCounts;
        }

        public List<StatisticModel> GetDaysCountbyDepartment(string qurey, int skip, int take, bool isOrderByDesc)
        {
            List<StatisticModel> batchCounts = IBatchRepo.GetDaysCountbyDepartment(qurey, skip, take, isOrderByDesc);
            return batchCounts;
        }
    }
}
