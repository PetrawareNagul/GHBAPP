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
    public class BranchDepartmentService : IBranchDepartmentService
    {
        private static readonly Lazy<IBranchDepartmentRepo> lazy = new Lazy<IBranchDepartmentRepo>(() => new BranchDepartmentRepo());
        public static IBranchDepartmentRepo IBranchDepartmentRepo { get { return lazy.Value; } }
        public BranchDepartment PostData(BranchDepartment obj)
        {
            try
            {
                obj = IBranchDepartmentRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<BranchDepartment> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchDepartmentRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchDepartment UpdateData(BranchDepartment obj)
        {
            try
            {
                obj = IBranchDepartmentRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<BranchDepartment> objs)
        {
            Int32 result = 0;
            try
            {
                result = IBranchDepartmentRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchDepartment DeleteData(Int32 Id)
        {
            BranchDepartment obj = new BranchDepartment();
            try
            {
                obj = IBranchDepartmentRepo.DeleteData(Id);
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
                result = IBranchDepartmentRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public BranchDepartment GetSingle(Int32 Id)
        {
            BranchDepartment obj = new BranchDepartment();
            try
            {
                obj = IBranchDepartmentRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<BranchDepartment> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                objs = IBranchDepartmentRepo.GetData(skip, take, isOrderByDesc);
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
                count = IBranchDepartmentRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchDepartment> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                objs = IBranchDepartmentRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IBranchDepartmentRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchDepartment> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                objs = IBranchDepartmentRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IBranchDepartmentRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<BranchDepartment> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<BranchDepartment> objs = new List<BranchDepartment>();
            try
            {
                objs = IBranchDepartmentRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<BranchDepartment> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<BranchDepartment> BranchDepartmentSearch = new List<BranchDepartment>();
            List<BranchDepartment> BranchDepartments = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //BranchDepartmentSearch.AddRange(BranchDepartments.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (BranchDepartmentSearch.Count == 0)
                BranchDepartmentSearch = BranchDepartments;
            BranchDepartmentSearch = sortDir ? BranchDepartmentSearch.OrderBy(x => typeof(BranchDepartment).GetProperty(sortBy).GetValue(x)).ToList() : BranchDepartmentSearch.OrderByDescending(x => typeof(BranchDepartment).GetProperty(sortBy).GetValue(x)).ToList();
            var result = BranchDepartmentSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = BranchDepartmentSearch.Count();
            totalResultsCount = BranchDepartments.Count();
            if (result == null)
            {
                return new List<BranchDepartment>();
            }
            return result;
        }
    }
}
