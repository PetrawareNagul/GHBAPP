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
    public class UserSecurityService : IUserSecurityService
    {
        private static readonly Lazy<IUserSecurityRepo> lazy = new Lazy<IUserSecurityRepo>(() => new UserSecurityRepo());
        public static IUserSecurityRepo IUserSecurityRepo { get { return lazy.Value; } }
        public UserSecurity PostData(UserSecurity obj)
        {
            try
            {
                obj = IUserSecurityRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserSecurity> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserSecurityRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurity UpdateData(UserSecurity obj)
        {
            try
            {
                obj = IUserSecurityRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserSecurity> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserSecurityRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurity DeleteData(Int32 Id)
        {
            UserSecurity obj = new UserSecurity();
            try
            {
                obj = IUserSecurityRepo.DeleteData(Id);
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
                result = IUserSecurityRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurity GetSingle(Int32 Id)
        {
            UserSecurity obj = new UserSecurity();
            try
            {
                obj = IUserSecurityRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserSecurity> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                objs = IUserSecurityRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserSecurityRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurity> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                objs = IUserSecurityRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserSecurityRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurity> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                objs = IUserSecurityRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserSecurityRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurity> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserSecurity> objs = new List<UserSecurity>();
            try
            {
                objs = IUserSecurityRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserSecurity> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserSecurity> UserSecuritySearch = new List<UserSecurity>();
            List<UserSecurity> UserSecuritys = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserSecuritySearch.AddRange(UserSecuritys.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserSecuritySearch.Count == 0)
                UserSecuritySearch = UserSecuritys;
            UserSecuritySearch = sortDir ? UserSecuritySearch.OrderBy(x => typeof(UserSecurity).GetProperty(sortBy).GetValue(x)).ToList() : UserSecuritySearch.OrderByDescending(x => typeof(UserSecurity).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserSecuritySearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserSecuritySearch.Count();
            totalResultsCount = UserSecuritys.Count();
            if (result == null)
            {
                return new List<UserSecurity>();
            }
            return result;
        }
    }
}
