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
    public class BranchUserService : IBranchUserService
    {
        private static readonly Lazy<IBranchUserRepo> lazy = new Lazy<IBranchUserRepo>(() => new BranchUserRepo());
        public static IBranchUserRepo IBranchUserRepo { get { return lazy.Value; } }
        public BranchUser PostData(BranchUser obj)
        {
            try
            {
                obj = IBranchUserRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<BranchUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchUserRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchUser UpdateData(BranchUser obj)
        {
            try
            {
                obj = IBranchUserRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<BranchUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchUserRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchUser DeleteData(Int32 Id)
        {
            BranchUser obj = new BranchUser();
            try
            {
                obj = IBranchUserRepo.DeleteData(Id);
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
                result = IBranchUserRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchUser GetSingle(Int32 Id)
        {
            BranchUser obj = new BranchUser();
            try
            {
                obj = IBranchUserRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BranchUser> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BranchUser> objs = new List<BranchUser>();
            try
            {
                objs = IBranchUserRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBranchUserRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchUser> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<BranchUser> objs = new List<BranchUser>();
            try
            {
                objs = IBranchUserRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IBranchUserRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchUser> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BranchUser> objs = new List<BranchUser>();
            try
            {
                objs = IBranchUserRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBranchUserRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchUser> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<BranchUser> objs = new List<BranchUser>();
            try
            {
                objs = IBranchUserRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<BranchUser> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<BranchUser> BranchUserSearch = new List<BranchUser>();
            List<BranchUser> BranchUsers = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BranchUserSearch.AddRange(BranchUsers.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BranchUserSearch.Count == 0)
                BranchUserSearch = BranchUsers;
            BranchUserSearch = sortDir ? BranchUserSearch.OrderBy(x => typeof(BranchUser).GetProperty(sortBy).GetValue(x)).ToList() : BranchUserSearch.OrderByDescending(x => typeof(BranchUser).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BranchUserSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BranchUserSearch.Count();
            totalResultsCount = BranchUsers.Count();
            if (result == null)
            {
                return new List<BranchUser>();
            }
            return result;
        }
    }
}
