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
    public class UserLoginHistoryService : IUserLoginHistoryService
    {
        private static readonly Lazy<IUserLoginHistoryRepo> lazy = new Lazy<IUserLoginHistoryRepo>(() => new UserLoginHistoryRepo());
        public static IUserLoginHistoryRepo IUserLoginHistoryRepo { get { return lazy.Value; } }
        public UserLoginHistory PostData(UserLoginHistory obj)
        {
            try
            {
                obj = IUserLoginHistoryRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserLoginHistory> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserLoginHistoryRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserLoginHistory UpdateData(UserLoginHistory obj)
        {
            try
            {
                obj = IUserLoginHistoryRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserLoginHistory> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserLoginHistoryRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserLoginHistory DeleteData(Int32 Id)
        {
            UserLoginHistory obj = new UserLoginHistory();
            try
            {
                obj = IUserLoginHistoryRepo.DeleteData(Id);
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
                result = IUserLoginHistoryRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserLoginHistory GetSingle(Int32 Id)
        {
            UserLoginHistory obj = new UserLoginHistory();
            try
            {
                obj = IUserLoginHistoryRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserLoginHistory> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                objs = IUserLoginHistoryRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserLoginHistoryRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserLoginHistory> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                objs = IUserLoginHistoryRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserLoginHistoryRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserLoginHistory> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                objs = IUserLoginHistoryRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserLoginHistoryRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserLoginHistory> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserLoginHistory> objs = new List<UserLoginHistory>();
            try
            {
                objs = IUserLoginHistoryRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserLoginHistory> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserLoginHistory> UserLoginHistorySearch = new List<UserLoginHistory>();
            List<UserLoginHistory> UserLoginHistorys = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserLoginHistorySearch.AddRange(UserLoginHistorys.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserLoginHistorySearch.Count == 0)
                UserLoginHistorySearch = UserLoginHistorys;
            UserLoginHistorySearch = sortDir ? UserLoginHistorySearch.OrderBy(x => typeof(UserLoginHistory).GetProperty(sortBy).GetValue(x)).ToList() : UserLoginHistorySearch.OrderByDescending(x => typeof(UserLoginHistory).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserLoginHistorySearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserLoginHistorySearch.Count();
            totalResultsCount = UserLoginHistorys.Count();
            if (result == null)
            {
                return new List<UserLoginHistory>();
            }
            return result;
        }
    }
}
