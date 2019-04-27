using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using System.Data.SqlClient;

namespace Silverlake.Repo.IRepo
{
    public interface IDepartmentRepo : IDefaultInterface<Department>
    {
        List<DocTypeModel> GetDocTypesByDepartmentId(int id);
    }
}
