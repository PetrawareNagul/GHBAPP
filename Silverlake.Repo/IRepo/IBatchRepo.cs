using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using System.Data.SqlClient;

namespace Silverlake.Repo.IRepo
{
    public interface IBatchRepo : IDefaultInterface<Batch>
    {
        List<BatchesStagesCount> GetBatchStagesCount(string filter, int skip, int take, bool isOrderByDesc);
        BatchesInfo GetBatchesInfo(string filter, int skip, int take, bool isOrderByDesc);
        List<StatisticModel> GetTotalCountbyDepartment(string filter, int skip, int take, bool isOrderByDesc);
        List<StatisticModel> GetDaysCountbyDepartment(string qurey, int skip, int take, bool isOrderByDesc);
    }
}
