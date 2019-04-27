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
    public class DepartmentUserService : IDepartmentUserService
    {
        private static readonly Lazy<IDepartmentUserRepo> lazy = new Lazy<IDepartmentUserRepo>(() => new DepartmentUserRepo());
        public static IDepartmentUserRepo IDepartmentUserRepo { get { return lazy.Value; } }
        public DepartmentUser PostData(DepartmentUser obj)
        {
            try
            {
                obj = IDepartmentUserRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<DepartmentUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = IDepartmentUserRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public DepartmentUser UpdateData(DepartmentUser obj)
        {
            try
            {
                obj = IDepartmentUserRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<DepartmentUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = IDepartmentUserRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public DepartmentUser DeleteData(Int32 Id)
        {
            DepartmentUser obj = new DepartmentUser();
            try
            {
                obj = IDepartmentUserRepo.DeleteData(Id);
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
                result = IDepartmentUserRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public DepartmentUser GetSingle(Int32 Id)
        {
            DepartmentUser obj = new DepartmentUser();
            try
            {
                obj = IDepartmentUserRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<DepartmentUser> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                objs = IDepartmentUserRepo.GetData(skip, take, isOrderByDesc);
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
                count = IDepartmentUserRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<DepartmentUser> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                objs = IDepartmentUserRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IDepartmentUserRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<DepartmentUser> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                objs = IDepartmentUserRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IDepartmentUserRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<DepartmentUser> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<DepartmentUser> objs = new List<DepartmentUser>();
            try
            {
                objs = IDepartmentUserRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<DepartmentUser> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<DepartmentUser> DepartmentUserSearch = new List<DepartmentUser>();
            List<DepartmentUser> DepartmentUsers = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //DepartmentUserSearch.AddRange(DepartmentUsers.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (DepartmentUserSearch.Count == 0)
                DepartmentUserSearch = DepartmentUsers;
            DepartmentUserSearch = sortDir ? DepartmentUserSearch.OrderBy(x => typeof(DepartmentUser).GetProperty(sortBy).GetValue(x)).ToList() : DepartmentUserSearch.OrderByDescending(x => typeof(DepartmentUser).GetProperty(sortBy).GetValue(x)).ToList();
            var result = DepartmentUserSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = DepartmentUserSearch.Count();
            totalResultsCount = DepartmentUsers.Count();
            if (result == null)
            {
                return new List<DepartmentUser>();
            }
            return result;
        }
    }
}
