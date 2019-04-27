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
    public class StagesController : ApiController
    {
        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

        // GET api/values
        public IEnumerable<Stage> Get()
        {
            return IStageService.GetData(0, 0, false);
        }
    }
}
