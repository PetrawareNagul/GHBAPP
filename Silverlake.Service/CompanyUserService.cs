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
    public class CompanyUserService : ICompanyUserService
    {
        private static readonly Lazy<ICompanyUserRepo> lazy = new Lazy<ICompanyUserRepo>(() => new CompanyUserRepo());
        public static ICompanyUserRepo ICompanyUserRepo { get { return lazy.Value; } }
        public CompanyUser PostData(CompanyUser obj)
        {
            try
            {
                obj = ICompanyUserRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<CompanyUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = ICompanyUserRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public CompanyUser UpdateData(CompanyUser obj)
        {
            try
            {
                obj = ICompanyUserRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<CompanyUser> objs)
        {
            Int32 result = 0;
            try
            {
                result = ICompanyUserRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public CompanyUser DeleteData(Int32 Id)
        {
            CompanyUser obj = new CompanyUser();
            try
            {
                obj = ICompanyUserRepo.DeleteData(Id);
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
                result = ICompanyUserRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public CompanyUser GetSingle(Int32 Id)
        {
            CompanyUser obj = new CompanyUser();
            try
            {
                obj = ICompanyUserRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<CompanyUser> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<CompanyUser> objs = new List<CompanyUser>();
            try
            {
                objs = ICompanyUserRepo.GetData(skip, take, isOrderByDesc);
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
                count = ICompanyUserRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<CompanyUser> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<CompanyUser> objs = new List<CompanyUser>();
            try
            {
                objs = ICompanyUserRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = ICompanyUserRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<CompanyUser> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<CompanyUser> objs = new List<CompanyUser>();
            try
            {
                objs = ICompanyUserRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = ICompanyUserRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<CompanyUser> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<CompanyUser> objs = new List<CompanyUser>();
            try
            {
                objs = ICompanyUserRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<CompanyUser> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<CompanyUser> CompanyUserSearch = new List<CompanyUser>();
            List<CompanyUser> CompanyUsers = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //CompanyUserSearch.AddRange(CompanyUsers.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (CompanyUserSearch.Count == 0)
                CompanyUserSearch = CompanyUsers;
            CompanyUserSearch = sortDir ? CompanyUserSearch.OrderBy(x => typeof(CompanyUser).GetProperty(sortBy).GetValue(x)).ToList() : CompanyUserSearch.OrderByDescending(x => typeof(CompanyUser).GetProperty(sortBy).GetValue(x)).ToList();
            var result = CompanyUserSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = CompanyUserSearch.Count();
            totalResultsCount = CompanyUsers.Count();
            if (result == null)
            {
                return new List<CompanyUser>();
            }
            return result;
        }
    }
}
