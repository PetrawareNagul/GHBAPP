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
    public class UserSecurityTypeService : IUserSecurityTypeService
    {
        private static readonly Lazy<IUserSecurityTypeRepo> lazy = new Lazy<IUserSecurityTypeRepo>(() => new UserSecurityTypeRepo());
        public static IUserSecurityTypeRepo IUserSecurityTypeRepo { get { return lazy.Value; } }
        public UserSecurityType PostData(UserSecurityType obj)
        {
            try
            {
                obj = IUserSecurityTypeRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserSecurityType> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserSecurityTypeRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurityType UpdateData(UserSecurityType obj)
        {
            try
            {
                obj = IUserSecurityTypeRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserSecurityType> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserSecurityTypeRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurityType DeleteData(Int32 Id)
        {
            UserSecurityType obj = new UserSecurityType();
            try
            {
                obj = IUserSecurityTypeRepo.DeleteData(Id);
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
                result = IUserSecurityTypeRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserSecurityType GetSingle(Int32 Id)
        {
            UserSecurityType obj = new UserSecurityType();
            try
            {
                obj = IUserSecurityTypeRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserSecurityType> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurityType> objs = new List<UserSecurityType>();
            try
            {
                objs = IUserSecurityTypeRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserSecurityTypeRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurityType> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurityType> objs = new List<UserSecurityType>();
            try
            {
                objs = IUserSecurityTypeRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserSecurityTypeRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurityType> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserSecurityType> objs = new List<UserSecurityType>();
            try
            {
                objs = IUserSecurityTypeRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserSecurityTypeRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserSecurityType> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserSecurityType> objs = new List<UserSecurityType>();
            try
            {
                objs = IUserSecurityTypeRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserSecurityType> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserSecurityType> UserSecurityTypeSearch = new List<UserSecurityType>();
            List<UserSecurityType> UserSecurityTypes = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserSecurityTypeSearch.AddRange(UserSecurityTypes.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserSecurityTypeSearch.Count == 0)
                UserSecurityTypeSearch = UserSecurityTypes;
            UserSecurityTypeSearch = sortDir ? UserSecurityTypeSearch.OrderBy(x => typeof(UserSecurityType).GetProperty(sortBy).GetValue(x)).ToList() : UserSecurityTypeSearch.OrderByDescending(x => typeof(UserSecurityType).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserSecurityTypeSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserSecurityTypeSearch.Count();
            totalResultsCount = UserSecurityTypes.Count();
            if (result == null)
            {
                return new List<UserSecurityType>();
            }
            return result;
        }
    }
}
