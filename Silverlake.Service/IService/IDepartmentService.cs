using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Service.IService
{
    public interface IDepartmentService : IDefaultInterface<Department>
    {
        List<Department> GetDataTableData(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount);
        List<DocTypeModel> GetDocTypesByDepartmentId(int id);
    }
}
