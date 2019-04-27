using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Service.IService
{
    public interface ISetService : IDefaultInterface<Set>
    {
        List<Set> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount);
        List<Set> GetDataByFilterNew(string filter, int skip, int take, bool isOrderByDesc);
        Int32 GetCountByFilterNew(string filter);
        void UpdateStatusForMfiles(List<Set> setList);
        List<Set> GetSetsForMfiles();
        long GetSetsForMfilesAccountCount(int departmentId, string docType, string aano);
        List<DocTypeSetModel> GetSetsForMfilesAccount(int departmentId, string docType, string aano, int skip,int take);
    }
}
