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
    public class StageService : IStageService
    {
        private static readonly Lazy<IStageRepo> lazy = new Lazy<IStageRepo>(() => new StageRepo());
        public static IStageRepo IStageRepo { get { return lazy.Value; } }
        public Stage PostData(Stage obj)
        {
            try
            {
                obj = IStageRepo.PostData(obj);
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
                result = IStageRepo.PostBulkData(objs);
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
                obj = IStageRepo.UpdateData(obj);
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
                result = IStageRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Stage DeleteData(Int32 Id)
        {
            Stage obj = new Stage();
            try
            {
                obj = IStageRepo.DeleteData(Id);
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
                result = IStageRepo.DeleteBulkData(Ids);
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
                obj = IStageRepo.GetSingle(Id);
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
                objs = IStageRepo.GetData(skip, take, isOrderByDesc);
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
                count = IStageRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Stage> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Stage> objs = new List<Stage>();
            try
            {
                objs = IStageRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IStageRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
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
                objs = IStageRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IStageRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Stage> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Stage> objs = new List<Stage>();
            try
            {
                objs = IStageRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Stage> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Stage> StageSearch = new List<Stage>();
            List<Stage> Stages = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //StageSearch.AddRange(Stages.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (StageSearch.Count == 0)
                StageSearch = Stages;
            StageSearch = sortDir ? StageSearch.OrderBy(x => typeof(Stage).GetProperty(sortBy).GetValue(x)).ToList() : StageSearch.OrderByDescending(x => typeof(Stage).GetProperty(sortBy).GetValue(x)).ToList();
            var result = StageSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = StageSearch.Count();
            totalResultsCount = Stages.Count();
            if (result == null)
            {
                return new List<Stage>();
            }
            return result;
        }
    }
}
