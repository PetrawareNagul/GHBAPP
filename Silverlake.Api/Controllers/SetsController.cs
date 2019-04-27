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
    public class SetsController : ApiController
    {
        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        // GET api/values
        public IEnumerable<Set> Get()
        {
            return ISetService.GetData(0, 0, true);
        }

        // POST api/values
        public Set Post(Set obj)
        {
            return ISetService.PostData(obj);
        }
    }
}
