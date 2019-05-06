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
    public class BranchService : IBranchService
    {
        private static readonly Lazy<IBranchRepo> lazy = new Lazy<IBranchRepo>(() => new BranchRepo());
        public static IBranchRepo IBranchRepo { get { return lazy.Value; } }
        public Branch PostData(Branch obj)
        {
            try
            {
                obj = IBranchRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<Branch> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Branch UpdateData(Branch obj)
        {
            try
            {
                obj = IBranchRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<Branch> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Branch DeleteData(Int32 Id)
        {
            Branch obj = new Branch();
            try
            {
                obj = IBranchRepo.DeleteData(Id);
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
                result = IBranchRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Branch GetSingle(Int32 Id)
        {
            Branch obj = new Branch();
            try
            {
                obj = IBranchRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                throw ex;
               // Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Branch> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                objs = IBranchRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBranchRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Branch> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                objs = IBranchRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IBranchRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Branch> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Branch> objs = new List<Branch>();
            try
            {
                objs = IBranchRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBranchRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Branch> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Branch> objs = new List<Branch>();
            try
            {
                objs = IBranchRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Branch> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Branch> BranchSearch = new List<Branch>();
            List<Branch> Branchs = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BranchSearch.AddRange(Branchs.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BranchSearch.Count == 0)
                BranchSearch = Branchs;
            BranchSearch = sortDir ? BranchSearch.OrderBy(x => typeof(Branch).GetProperty(sortBy).GetValue(x)).ToList() : BranchSearch.OrderByDescending(x => typeof(Branch).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BranchSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BranchSearch.Count();
            totalResultsCount = Branchs.Count();
            if (result == null)
            {
                return new List<Branch>();
            }
            return result;
        }

        public List<Branch> GetUtilizedBranches()
        {
            return IBranchRepo.GetUtilizedBranches();
        }
    }
}
