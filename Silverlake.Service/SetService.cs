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
    public class SetService : ISetService
    {
        private static readonly Lazy<ISetRepo> lazy = new Lazy<ISetRepo>(() => new SetRepo());
        public static ISetRepo ISetRepo { get { return lazy.Value; } }
        public Set PostData(Set obj)
        {
            try
            {
                obj = ISetRepo.PostData(obj);
            }
            catch(Exception ex)
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
                result = ISetRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Set UpdateData(Set obj)
        {
            try
            {
                obj = ISetRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<Set> objs)
        {
            Int32 result = 0;
            try
            {
                result = ISetRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Set DeleteData(Int32 Id)
        {
            Set obj = new Set();
            try
            {
                obj = ISetRepo.DeleteData(Id);
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
                result = ISetRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
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
                obj = ISetRepo.GetSingle(Id);
            }
            catch(Exception ex)
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
                objs = ISetRepo.GetData(skip, take, isOrderByDesc);
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
                count = ISetRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Set> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Set> objs = new List<Set>();
            try
            {
                objs = ISetRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = ISetRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
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
                objs = ISetRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Set> GetDataByFilterNew(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Set> objs = new List<Set>();
            try
            {
                objs = ISetRepo.GetDataByFilterNew(filter, skip, take, isOrderByDesc);
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
                count = ISetRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public Int32 GetCountByFilterNew(string filter)
        {
            Int32 count = 0;
            try
            {
                count = ISetRepo.GetCountByFilterNew(filter);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Set> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Set> objs = new List<Set>();
            try
            {
                objs = ISetRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Set> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Set> SetSearch = new List<Set>();
            List<Set> Sets = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //SetSearch.AddRange(Sets.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (SetSearch.Count == 0)
                SetSearch = Sets;
            SetSearch = sortDir ? SetSearch.OrderBy(x => typeof(Set).GetProperty(sortBy).GetValue(x)).ToList() : SetSearch.OrderByDescending(x => typeof(Set).GetProperty(sortBy).GetValue(x)).ToList();
            var result = SetSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = SetSearch.Count();
            totalResultsCount = Sets.Count();
            if (result == null)
            {
                return new List<Set>();
            }
            return result;
        }

        public void UpdateStatusForMfiles(List<Set> setList)
        {
            ISetRepo.UpdateStatusForMfiles(setList);
        }

        public List<Set> GetSetsForMfiles()
        {
            return ISetRepo.GetSetsForMfiles();
        }

        public List<DocTypeSetModel> GetSetsForMfilesAccount(int departmentId, string docType, string aano, int skip, int take)
        {
            return ISetRepo.GetSetsForMfilesAccount(departmentId,docType,aano,skip,take);
        }

        public long GetSetsForMfilesAccountCount(int departmentId, string docType, string aano)
        {
            return ISetRepo.GetSetsForMfilesAccountCount(departmentId,docType,aano);
        }
    }
}
