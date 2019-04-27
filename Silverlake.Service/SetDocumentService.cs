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
    public class SetDocumentService : ISetDocumentService
    {
        private static readonly Lazy<ISetDocumentRepo> lazy = new Lazy<ISetDocumentRepo>(() => new SetDocumentRepo());
        public static ISetDocumentRepo ISetDocumentRepo { get { return lazy.Value; } }
        public SetDocument PostData(SetDocument obj)
        {
            try
            {
                obj = ISetDocumentRepo.PostData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 PostBulkData(List<SetDocument> objs)
        {
            Int32 result = 0;
            try
            {
                result = ISetDocumentRepo.PostBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument UpdateData(SetDocument obj)
        {
            try
            {
                obj = ISetDocumentRepo.UpdateData(obj);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public Int32 UpdateBulkData(List<SetDocument> objs)
        {
            Int32 result = 0;
            try
            {
                result = ISetDocumentRepo.UpdateBulkData(objs);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument DeleteData(Int32 Id)
        {
            SetDocument obj = new SetDocument();
            try
            {
                obj = ISetDocumentRepo.DeleteData(Id);
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
                result = ISetDocumentRepo.DeleteBulkData(Ids);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return result;
        }
        public SetDocument GetSingle(Int32 Id)
        {
            SetDocument obj = new SetDocument();
            try
            {
                obj = ISetDocumentRepo.GetSingle(Id);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return obj;
        }
        public List<SetDocument> GetData(int skip, int take, bool isOrderByDesc)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                objs = ISetDocumentRepo.GetData(skip, take, isOrderByDesc);
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
                count = ISetDocumentRepo.GetCount();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                objs = ISetDocumentRepo.GetDataByPropertyName(propertyName, propertyValue, isEqual, skip, take, isOrderByDesc);
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
                count = ISetDocumentRepo.GetCountByPropertyName(propertyName, propertyValue, isEqual);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc)
        {
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                objs = ISetDocumentRepo.GetDataByFilter(filter, skip, take, isOrderByDesc);
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
                count = ISetDocumentRepo.GetCountByFilter(filter);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return count;
        }
        public List<SetDocument> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount)
        {
            totalResultsCount = filteredResultsCount = 0;
            List<SetDocument> objs = new List<SetDocument>();
            try
            {
                objs = ISetDocumentRepo.GetDataByPaging(take, skip, out filteredResultsCount, out totalResultsCount);
                return objs;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return objs;
        }
        public List<SetDocument> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            List<SetDocument> SetDocumentSearch = new List<SetDocument>();
            List<SetDocument> SetDocuments = GetData(0, 0, false);
            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                var searchTerms = searchBy.Split(' ').ToList().ConvertAll(x => x.ToLower());
                //SetDocumentSearch.AddRange(SetDocuments.Where(s => searchTerms.Any(srch => s.Name1.ToLower().Contains(srch))));
            }
            if (SetDocumentSearch.Count == 0)
                SetDocumentSearch = SetDocuments;
            SetDocumentSearch = sortDir ? SetDocumentSearch.OrderBy(x => typeof(SetDocument).GetProperty(sortBy).GetValue(x)).ToList() : SetDocumentSearch.OrderByDescending(x => typeof(SetDocument).GetProperty(sortBy).GetValue(x)).ToList();
            var result = SetDocumentSearch.Skip(skip).Take(take).ToList();
            filteredResultsCount = SetDocumentSearch.Count();
            totalResultsCount = SetDocuments.Count();
            if (result == null)
            {
                return new List<SetDocument>();
            }
            return result;
        }
    }
}
