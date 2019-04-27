using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Service.IService
{
    public interface IBatchService : IDefaultInterface<Batch>
    {
        List<Batch> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount);
        List<BatchesStagesCount> GetBatchStagesCount(string filter, int skip, int take, bool isOrderByDesc);
        BatchesInfo GetBatchesInfo(string filter, int skip, int take, bool v);
        List<StatisticModel> GetTotalCountbyDepartment(string filter, int skip, int take, bool isOrderByDesc);
        List<StatisticModel> GetDaysCountbyDepartment(string qurey, int skip, int take, bool isOrderByDesc);
    }
}
