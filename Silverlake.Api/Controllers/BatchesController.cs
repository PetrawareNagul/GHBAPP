using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Silverlake.Api.Controllers
{
    public class BatchesController : ApiController
    {
        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        // GET api/values
        public IEnumerable<Batch> Get(string filter)
        {
            List<Batch> batches = new List<Batch>();
            if (filter == "" || filter == null)
            {
                batches = IBatchService.GetData(0, 0, true);
            }
            else
            {
                batches = IBatchService.GetDataByFilter(filter, 0, 0, true);
            }

            try
            {
                string[] keys = filter.Split(new string[] { " and " }, StringSplitOptions.None);
                string batchKeyString = keys.Where(x => x.Contains("batch_key")).FirstOrDefault();
                string[] batchKeys = batchKeyString.Split('=');
                string batchKey = batchKeys[1].Replace('\'', ' ');
                List<Batch> batchesNew = IBatchService.GetDataByFilter(" batch_key='" + batchKey.Trim() + "' ", 0, 0, true);

                if (batchesNew.Count > 0)
                {
                    Batch batch = batchesNew.FirstOrDefault();
                    if (batch.StageId == 4 && batch.BatchStatus == 1)
                    {
                        batch.StageId = 3;
                        batch.BatchStatus = 1;
                        string savePath = ConfigurationManager.AppSettings["SavePath"].ToString();
                        string[] filesToDelete = Directory.GetFiles(savePath, batch.BatchKey + "_*");
                        if (filesToDelete.Count() > 0)
                        {
                            foreach (string fNamePath in filesToDelete)
                            {
                                File.Delete(fNamePath);
                            }
                        }
                        IBatchService.UpdateData(batch);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return batches;
        }

        // POST api/values
        public Batch Post(Batch obj)
        {
            return IBatchService.PostData(obj);
        }
    }
}
