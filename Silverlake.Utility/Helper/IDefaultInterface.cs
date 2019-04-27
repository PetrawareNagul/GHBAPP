using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public interface IDefaultInterface<T>
    {
        T PostData(T obj);
        Int32 PostBulkData(List<T> objs);
        T UpdateData(T obj);
        Int32 UpdateBulkData(List<T> objs);
        T DeleteData(Int32 Id);
        Int32 DeleteBulkData(List<Int32> Ids);
        T GetSingle(Int32 Id);
        List<T> GetData(int skip, int take, bool isOrderByDesc);
        Int32 GetCount();
        List<T> GetDataByPropertyName(string propertyName, string propertyValue, bool isEqual, int skip, int take, bool isOrderByDesc);
        Int32 GetCountByPropertyName(string propertyName, string propertyValue, bool isEqual);
        List<T> GetDataByFilter(string filter, int skip, int take, bool isOrderByDesc);
        Int32 GetCountByFilter(string filter);
        List<T> GetDataByPaging(int take, int skip, out int filteredResultsCount, out int totalResultsCount);
    }
}
