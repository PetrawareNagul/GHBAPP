using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Service.IService
{
    public interface IBranchUserService : IDefaultInterface<BranchUser>
    {
        List<BranchUser> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount);
    }
}
