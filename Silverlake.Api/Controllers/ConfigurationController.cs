using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Silverlake.Api.Controllers
{
    public class ConfigurationController : ApiController
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IUserRoleService> lazyUserRoleServiceObj = new Lazy<IUserRoleService>(() => new UserRoleService());

        public static IUserRoleService IUserRoleService { get { return lazyUserRoleServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IBranchDepartmentService> lazyBranchDepartmentServiceObj = new Lazy<IBranchDepartmentService>(() => new BranchDepartmentService());

        public static IBranchDepartmentService IBranchDepartmentService { get { return lazyBranchDepartmentServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IDepartmentUserService> lazyDepartmentUserServiceObj = new Lazy<IDepartmentUserService>(() => new DepartmentUserService());

        public static IDepartmentUserService IDepartmentUserService { get { return lazyDepartmentUserServiceObj.Value; } }

        private static readonly Lazy<IBranchUserService> lazyBranchUserServiceObj = new Lazy<IBranchUserService>(() => new BranchUserService());

        public static IBranchUserService IBranchUserService { get { return lazyBranchUserServiceObj.Value; } }

        // GET api/values
        public object Get(string apiAuthToken)
        {
            ConfigurationDTO configurationDTO = new ConfigurationDTO();
            List<User> userMatches = IUserService.GetDataByPropertyName(nameof(Utility.User.ApiAuthToken), apiAuthToken, true, 0, 0, false);
            User user = new Utility.User();
            if (userMatches.Count > 0)
            {
                user = userMatches.FirstOrDefault();
                if (user != null)
                {
                    if (user.Status == 0)
                    {
                        configurationDTO.isSuccess = false;
                        configurationDTO.responseMsg = "User not active";
                        configurationDTO.user = null;
                        configurationDTO.branch = null;
                        return configurationDTO;
                    }
                    else
                    {
                        UserRole userRole = IUserRoleService.GetSingle(user.UserRoleId);
                        if (user.BranchId == 0 && userRole.Name == "Super Admin")
                        {
                            configurationDTO.isSuccess = true;
                            configurationDTO.responseMsg = "SA";
                            configurationDTO.user = user;
                            configurationDTO.branch = null;
                            return configurationDTO;
                        }
                        else if (user.BranchId == 0 && userRole.Name == "HQ Admin")
                        {
                            configurationDTO.isSuccess = true;
                            configurationDTO.responseMsg = "HQ Admin";
                            configurationDTO.user = user;
                            configurationDTO.branch = null;
                            return configurationDTO;
                        }
                        else if (user.BranchId == 0 && userRole.Name == "Regional Admin")
                        {
                            configurationDTO.isSuccess = true;
                            configurationDTO.responseMsg = "Regional Admin";
                            configurationDTO.user = user;
                            configurationDTO.branch = null;
                            return configurationDTO;
                        }
                        else
                        {
                            Branch branch = IBranchService.GetSingle(user.BranchId);
                            if (branch.Status != 0)
                            {
                                List<Department> departments = new List<Department>();
                                if (branch.IsAll == 1)
                                {
                                    departments = IDepartmentService.GetData(0, 0, false);
                                }
                                else
                                {
                                    List<BranchDepartment> branchDepartments = IBranchDepartmentService.GetDataByFilter(" branch_id = '" + branch.Id + "' and status = '1'", 0, 0, false);
                                    departments = IDepartmentService.GetDataByFilter(" ID not in (" + String.Join(",", branchDepartments.Select(x => x.DepartmentId).ToArray()) + ") and status='1'", 0, 0, false);
                                    //departments.ForEach(x =>
                                    //{
                                    //    //x.Status = branchDepartments.Where(y => y.DepartmentId == x.Id).FirstOrDefault().Status;
                                    //    x.Status = 1;
                                    //});
                                }
                                List<DepartmentUser> userDepartments = IDepartmentUserService.GetDataByFilter(" user_id = '" + user.Id + "' and status='1'", 0, 0, false);
                                if (user.IsAll == 0)
                                {
                                    departments = departments.Where(x => !(userDepartments.Select(y => y.DepartmentId).ToList().Contains(x.Id))).ToList();
                                }
                                configurationDTO.isSuccess = true;
                                configurationDTO.responseMsg = "Branch";
                                configurationDTO.user = user;
                                configurationDTO.branch = branch;
                                configurationDTO.departments = departments;
                                user.LastSyncDate = DateTime.Now;
                                IUserService.UpdateData(user);
                                return configurationDTO;
                            }
                            else
                            {
                                configurationDTO.isSuccess = false;
                                configurationDTO.responseMsg = "Branch not active";
                                configurationDTO.user = null;
                                configurationDTO.branch = null;
                                return configurationDTO;
                            }
                        }
                    }
                }
                else
                {
                    configurationDTO.isSuccess = false;
                    configurationDTO.responseMsg = "User doesn't exist";
                    configurationDTO.user = null;
                    configurationDTO.branch = null;
                    return configurationDTO;
                }
            }
            else
            {
                configurationDTO.isSuccess = false;
                configurationDTO.responseMsg = "User doesn't exist";
                configurationDTO.user = null;
                configurationDTO.branch = null;
                return configurationDTO;
            }
        }
    }

    public class ConfigurationDTO
    {
        public bool isSuccess { get; set; }
        public string responseMsg { get; set; }
        public User user { get; set; }
        public Branch branch { get; set; }
        public List<Department> departments { get; set; }
    }
}
