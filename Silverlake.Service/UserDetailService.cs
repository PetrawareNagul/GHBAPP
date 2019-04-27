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
    public class UserDetailService : IUserDetailService
    {
        private static readonly Lazy<IUserDetailRepo> lazy = new Lazy<IUserDetailRepo>(() => new UserDetailRepo());
        public static IUserDetailRepo IUserDetailRepo { get { return lazy.Value; } }
        public UserDetail PostData(UserDetail obj)
        {
            try
            {
                obj = IUserDetailRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<UserDetail> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserDetailRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserDetail UpdateData(UserDetail obj)
        {
            try
            {
                obj = IUserDetailRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<UserDetail> objs)
        {
            Int32 result = 0;
            try
            {
                result = IUserDetailRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserDetail DeleteData(Int32 Id)
        {
            UserDetail obj = new UserDetail();
            try
            {
                obj = IUserDetailRepo.DeleteData(Id);
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
                result = IUserDetailRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public UserDetail GetSingle(Int32 Id)
        {
            UserDetail obj = new UserDetail();
            try
            {
                obj = IUserDetailRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<UserDetail> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                objs = IUserDetailRepo.GetData(skip, take, isOrderByDesc);
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
                count = IUserDetailRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserDetail> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                objs = IUserDetailRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IUserDetailRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserDetail> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                objs = IUserDetailRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IUserDetailRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<UserDetail> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<UserDetail> objs = new List<UserDetail>();
            try
            {
                objs = IUserDetailRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<UserDetail> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<UserDetail> UserDetailSearch = new List<UserDetail>();
            List<UserDetail> UserDetails = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //UserDetailSearch.AddRange(UserDetails.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (UserDetailSearch.Count == 0)
                UserDetailSearch = UserDetails;
            UserDetailSearch = sortDir ? UserDetailSearch.OrderBy(x => typeof(UserDetail).GetProperty(sortBy).GetValue(x)).ToList() : UserDetailSearch.OrderByDescending(x => typeof(UserDetail).GetProperty(sortBy).GetValue(x)).ToList();
            var result = UserDetailSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = UserDetailSearch.Count();
            totalResultsCount = UserDetails.Count();
            if (result == null)
            {
                return new List<UserDetail>();
            }
            return result;
        }
    }
}
