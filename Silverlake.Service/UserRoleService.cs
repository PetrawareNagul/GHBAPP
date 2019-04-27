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
    public class UserRoleService : IUserRoleService
    {
        private static readonly Lazy<IUserRoleRepo> lazy = new Lazy<IUserRoleRepo>(() => new UserRoleRepo());
        public static IUserRoleRepo IUserRoleRepo { get { return lazy.Value; } }
        public UserRole PostData(UserRole obj)
        {
            try
            {
                obj = IUserRoleRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserRole> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserRoleRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserRole UpdateData(UserRole obj)
        {
            try
            {
                obj = IUserRoleRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserRole> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserRoleRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserRole DeleteData(Int32 Id)
        {
            UserRole obj = new UserRole();
            try
            {
                obj = IUserRoleRepo.DeleteData(Id);
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
                result = IUserRoleRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserRole GetSingle(Int32 Id)
        {
            UserRole obj = new UserRole();
            try
            {
                obj = IUserRoleRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserRole> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserRole> objs = new List<UserRole>();
            try
            {
                objs = IUserRoleRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserRoleRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserRole> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserRole> objs = new List<UserRole>();
            try
            {
                objs = IUserRoleRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserRoleRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserRole> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserRole> objs = new List<UserRole>();
            try
            {
                objs = IUserRoleRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserRoleRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserRole> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserRole> objs = new List<UserRole>();
            try
            {
                objs = IUserRoleRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserRole> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserRole> UserRoleSearch = new List<UserRole>();
            List<UserRole> UserRoles = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserRoleSearch.AddRange(UserRoles.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserRoleSearch.Count == 0)
                UserRoleSearch = UserRoles;
            UserRoleSearch = sortDir ? UserRoleSearch.OrderBy(x => typeof(UserRole).GetProperty(sortBy).GetValue(x)).ToList() : UserRoleSearch.OrderByDescending(x => typeof(UserRole).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserRoleSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserRoleSearch.Count();
            totalResultsCount = UserRoles.Count();
            if (result == null)
            {
                return new List<UserRole>();
            }
            return result;
        }
    }
}
