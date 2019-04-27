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
    public class UsersController : ApiController
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        // GET api/values
        public IEnumerable<User> Get(string filter)
        {
            if (filter == "")
                return IUserService.GetData(0, 0, true);
            else
                return IUserService.GetDataByFilter(filter, 0, 0, false);
        }

        // POST api/values
        public User Post(User obj)
        {
            return IUserService.PostData(obj);
        }
    }
}
