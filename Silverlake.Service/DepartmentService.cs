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
    public class DepartmentService : IDepartmentService
    {
        private static readonly Lazy<IDepartmentRepo> lazy = new Lazy<IDepartmentRepo>(() => new DepartmentRepo());
        public static IDepartmentRepo IDepartmentRepo { get { return lazy.Value; } }
        public Department PostData(Department obj)
        {
            try
            {
                obj = IDepartmentRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<Department> objs)
        {
            Int32 result = 0;
            try
            {
                result = IDepartmentRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Department UpdateData(Department obj)
        {
            try
            {
                obj = IDepartmentRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<Department> objs)
        {
            Int32 result = 0;
            try
            {
                result = IDepartmentRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Department DeleteData(Int32 Id)
        {
            Department obj = new Department();
            try
            {
                obj = IDepartmentRepo.DeleteData(Id);
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
                result = IDepartmentRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public Department GetSingle(Int32 Id)
        {
            Department obj = new Department();
            try
            {
                obj = IDepartmentRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<Department> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<Department> objs = new List<Department>();
            try
            {
                objs = IDepartmentRepo.GetData(skip, take, isOrderByDesc);
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
                count = IDepartmentRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Department> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<Department> objs = new List<Department>();
            try
            {
                objs = IDepartmentRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = IDepartmentRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Department> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<Department> objs = new List<Department>();
            try
            {
                objs = IDepartmentRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = IDepartmentRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<Department> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<Department> objs = new List<Department>();
            try
            {
                objs = IDepartmentRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<Department> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<Department> DepartmentSearch = new List<Department>();
            List<Department> Departments = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //DepartmentSearch.AddRange(Departments.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (DepartmentSearch.Count == 0)
                DepartmentSearch = Departments;
            DepartmentSearch = sortDir ? DepartmentSearch.OrderBy(x => typeof(Department).GetProperty(sortBy).GetValue(x)).ToList() : DepartmentSearch.OrderByDescending(x => typeof(Department).GetProperty(sortBy).GetValue(x)).ToList();
            var result = DepartmentSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = DepartmentSearch.Count();
            totalResultsCount = Departments.Count();
            if (result == null)
            {
                return new List<Department>();
            }
            return result;
        }

        public List<DocTypeModel> GetDocTypesByDepartmentId(int id)
        {
            return IDepartmentRepo.GetDocTypesByDepartmentId(id);

        }
    }
}
