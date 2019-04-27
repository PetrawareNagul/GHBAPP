using Silverlake.Data;
using Silverlake.Data.IRepo;
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
    public class LanguageDirService : ILanguageDirService
    {
        private static readonly Lazy<ILanguageDirRepo> lazy = new Lazy<ILanguageDirRepo>(() => new LanguageDirRepo());
        public static ILanguageDirRepo ILanguageDirRepo { get { return lazy.Value; } }
        public LanguageDir PostData(LanguageDir obj)
        {
            try
            {
                obj = ILanguageDirRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<LanguageDir> objs)
        {
            Int32 result = 0;
            try
            {
                result = ILanguageDirRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir UpdateData(LanguageDir obj)
        {
            try
            {
                obj = ILanguageDirRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<LanguageDir> objs)
        {
            Int32 result = 0;
            try
            {
                result = ILanguageDirRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir DeleteData(Int32 Id)
        {
            LanguageDir obj = new LanguageDir();
            try
            {
                obj = ILanguageDirRepo.DeleteData(Id);
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
                result = ILanguageDirRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public LanguageDir GetSingle(Int32 Id)
        {
            LanguageDir obj = new LanguageDir();
            try
            {
                obj = ILanguageDirRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<LanguageDir> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                objs = ILanguageDirRepo.GetData(skip, take, isOrderByDesc);
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
                count = ILanguageDirRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                objs = ILanguageDirRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = ILanguageDirRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                objs = ILanguageDirRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = ILanguageDirRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<LanguageDir> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<LanguageDir> objs = new List<LanguageDir>();
            try
            {
                objs = ILanguageDirRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<LanguageDir> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<LanguageDir> LanguageDirSearch = new List<LanguageDir>();
            List<LanguageDir> LanguageDirs = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //LanguageDirSearch.AddRange(LanguageDirs.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (LanguageDirSearch.Count == 0)
                LanguageDirSearch = LanguageDirs;
            LanguageDirSearch = sortDir ? LanguageDirSearch.OrderBy(x => typeof(LanguageDir).GetProperty(sortBy).GetValue(x)).ToList() : LanguageDirSearch.OrderByDescending(x => typeof(LanguageDir).GetProperty(sortBy).GetValue(x)).ToList();
            var result = LanguageDirSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = LanguageDirSearch.Count();
            totalResultsCount = LanguageDirs.Count();
            if (result == null)
            {
                return new List<LanguageDir>();
            }
            return result;
        }
    }
}
