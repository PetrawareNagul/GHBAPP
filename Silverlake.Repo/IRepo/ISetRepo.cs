using Silverlake.Utility.Helper;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using System.Data.SqlClient;

namespace Silverlake.Repo.IRepo
{
    public interface ISetRepo : IDefaultInterface<Set>
    {
        List<Set> GetDataByFilterNew(string filter, int skip, int take, bool isOrderByDesc);
        Int32 GetCountByFilterNew(string filter);
        void UpdateStatusForMfiles(List<Set> setList);
        List<Set> GetSetsForMfiles();
        List<DocTypeSetModel> GetSetsForMfilesAccount(int departmentId, string docType, string aano, int skip, int take);
        long GetSetsForMfilesAccountCount(int departmentId, string docType, string aano);
    }
}
