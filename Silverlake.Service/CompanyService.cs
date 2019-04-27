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
    public class CompanyService : ICompanyService
    {
        private static readonly Lazy<ICompanyRepo> lazy = new Lazy<ICompanyRepo>(() => new CompanyRepo());
        public static ICompanyRepo ICompanyRepo { get { return lazy.Value; } }
        public Company PostData(Company obj)
        {
            try
            {
                obj = ICompanyRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<Company> objs)
        {
            Int32 result = 0;
            try
            {
                result = ICompanyRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Company UpdateData(Company obj)
        {
            try
            {
                obj = ICompanyRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<Company> objs)
        {
            Int32 result = 0;
            try
            {
                result = ICompanyRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Company DeleteData(Int32 Id)
        {
            Company obj = new Company();
            try
            {
                obj = ICompanyRepo.DeleteData(Id);
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
                result = ICompanyRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Company GetSingle(Int32 Id)
        {
            Company obj = new Company();
            try
            {
                obj = ICompanyRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Company> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Company> objs = new List<Company>();
            try
            {
                objs = ICompanyRepo.GetData(skip, take, isOrderByDesc);
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
                count = ICompanyRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Company> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Company> objs = new List<Company>();
            try
            {
                objs = ICompanyRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = ICompanyRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Company> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Company> objs = new List<Company>();
            try
            {
                objs = ICompanyRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = ICompanyRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Company> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Company> objs = new List<Company>();
            try
            {
                objs = ICompanyRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Company> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Company> CompanySearch = new List<Company>();
            List<Company> Companys = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //CompanySearch.AddRange(Companys.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (CompanySearch.Count == 0)
                CompanySearch = Companys;
            CompanySearch = sortDir ? CompanySearch.OrderBy(x => typeof(Company).GetProperty(sortBy).GetValue(x)).ToList() : CompanySearch.OrderByDescending(x => typeof(Company).GetProperty(sortBy).GetValue(x)).ToList();
            var result = CompanySearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = CompanySearch.Count();
            totalResultsCount = Companys.Count();
            if (result == null)
            {
                return new List<Company>();
            }
            return result;
        }
    }
}
