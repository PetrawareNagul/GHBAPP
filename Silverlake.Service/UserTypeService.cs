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
    public class UserTypeService : IUserTypeService
    {
        private static readonly Lazy<IUserTypeRepo> lazy = new Lazy<IUserTypeRepo>(() => new UserTypeRepo());
        public static IUserTypeRepo IUserTypeRepo { get { return lazy.Value; } }
        public UserType PostData(UserType obj)
        {
            try
            {
                obj = IUserTypeRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserType> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserTypeRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserType UpdateData(UserType obj)
        {
            try
            {
                obj = IUserTypeRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserType> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserTypeRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserType DeleteData(Int32 Id)
        {
            UserType obj = new UserType();
            try
            {
                obj = IUserTypeRepo.DeleteData(Id);
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
                result = IUserTypeRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserType GetSingle(Int32 Id)
        {
            UserType obj = new UserType();
            try
            {
                obj = IUserTypeRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserType> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                objs = IUserTypeRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserTypeRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserType> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                objs = IUserTypeRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserTypeRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserType> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserType> objs = new List<UserType>();
            try
            {
                objs = IUserTypeRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserTypeRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserType> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserType> objs = new List<UserType>();
            try
            {
                objs = IUserTypeRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserType> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserType> UserTypeSearch = new List<UserType>();
            List<UserType> UserTypes = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserTypeSearch.AddRange(UserTypes.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserTypeSearch.Count == 0)
                UserTypeSearch = UserTypes;
            UserTypeSearch = sortDir ? UserTypeSearch.OrderBy(x => typeof(UserType).GetProperty(sortBy).GetValue(x)).ToList() : UserTypeSearch.OrderByDescending(x => typeof(UserType).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserTypeSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserTypeSearch.Count();
            totalResultsCount = UserTypes.Count();
            if (result == null)
            {
                return new List<UserType>();
            }
            return result;
        }
    }
}
