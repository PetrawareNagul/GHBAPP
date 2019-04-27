using Silverlake.Api.Models;
using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace Silverlake.Api.Controllers
{
    [Authorize]
    public class BatchStatusController : ApiController
    {
        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IBranchDepartmentService> lazyBranchDepartmentServiceObj = new Lazy<IBranchDepartmentService>(() => new BranchDepartmentService());

        public static IBranchDepartmentService IBranchDepartmentService { get { return lazyBranchDepartmentServiceObj.Value; } }

        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }



        public void PostFile(HttpRequestMessage request)
        {
            try
            {
                string idString = User.Identity.Name;
                User user = IUserService.GetDataByPropertyName(nameof(Utility.User.ApiAuthToken), idString, true, 0, 0, false).FirstOrDefault();
                idString = user.Id.ToString();
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);
                MemoryStream xmlStream = new MemoryStream();
                xmlDoc.Save(xmlStream);
                xmlStream.Flush();//Adjust this if you want read your data 
                xmlStream.Position = 0;

                XmlSerializer deserializer = new XmlSerializer(typeof(BatchStatusModel));
                TextReader textReader = new StreamReader(xmlStream);
                BatchStatusModel batchStatusModel;
                batchStatusModel = (BatchStatusModel)deserializer.Deserialize(textReader);
                if (batchStatusModel != null)
                {
                    LogWriter logWriter = new LogWriter("API call start " + batchStatusModel.BatchKey + " ,StageId " + batchStatusModel.StageId + ",Status " + batchStatusModel.Status + ",DepartmentId " + batchStatusModel.DepartmentId + ", Count " + batchStatusModel.BatchCount + ", Date" + DateTime.Now);
                    //LogWriter logWriter = new LogWriter("API call start- " + batchStatusModel.BatchKey + " ,StageId " + batchStatusModel.StageId +",Status " + batchStatusModel.Status + ",DepartmentId " + batchStatusModel.DepartmentId + ", Date" + DateTime.Now);
                }

                Branch branch = new Branch();
                Department department = new Department();
                BranchDepartment branchDepartment = new BranchDepartment();
                List<Branch> branchMatches = IBranchService.GetDataByPropertyName(nameof(Branch.Code), batchStatusModel.BranchId, true, 0, 0, true);
                List<Department> departmentMatches = IDepartmentService.GetDataByPropertyName(nameof(Department.Code), batchStatusModel.DepartmentId, true, 0, 0, true);
                if (branchMatches.Count > 0 && departmentMatches.Count > 0)
                {
                    branch = branchMatches.FirstOrDefault();
                    department = departmentMatches.FirstOrDefault();

                    ConfigurationController configurationController = new ConfigurationController();
                    ConfigurationDTO config = (ConfigurationDTO)configurationController.Get(user.ApiAuthToken);
                    if (config.responseMsg == "Branch")
                    {
                        var dept = config.departments.Where(x => x.Id == department.Id).FirstOrDefault();
                        if (dept == null)
                        {
                            string customMessage = "Department not active!";
                            HttpResponseMessage response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                            response.ReasonPhrase = customMessage;
                            throw new HttpResponseException(response);
                        }
                    }
                }

                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                string batchKeyColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchKey));
                string batchNoColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BatchNo));
                string branchIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.BranchId));
                string departmentIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.DepartmentId));
                //string stageIdColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.StageId));
                string statusColumnName = Converter.GetColumnNameByPropertyName<Batch>(nameof(Batch.Status));
                filter.Append(" and " + batchKeyColumnName + " = '" + batchStatusModel.BatchKey + "'");
                filter.Append(" and " + batchNoColumnName + " = '" + batchStatusModel.BatchNo + "'");
                filter.Append(" and " + branchIdColumnName + " = '" + branch.Id + "'");
                filter.Append(" and " + departmentIdColumnName + " = '" + department.Id + "'");
                //filter.Append(" and " + stageIdColumnName + " = '" + batchStatusModel.StageId + "'");
                filter.Append(" and " + statusColumnName + " = '1'");

                List<Batch> batchMatches = IBatchService.GetDataByFilter(filter.ToString(), 0, 0, true);
                if (batchMatches.Count > 0)
                {
                    Batch batch = batchMatches.FirstOrDefault();

                    batch.BatchUser = batchStatusModel.UserId;
                    batch.BatchCount = batchStatusModel.BatchCount;
                    if (batchStatusModel.Status == 9)
                    {
                        batch.Status = 0;
                    }
                    else
                    {
                        batch.BatchStatus = batchStatusModel.Status;
                    }
                    if (batch.StageId != batchStatusModel.StageId)
                    {
                        batch.StageId = batchStatusModel.StageId;
                    }
                    // batch.UpdatedDate = (batchStatusModel.UpdatedDate == "" ? DateTime.Now : Convert.ToDateTime(batchStatusModel.UpdatedDate));
                    batch.UpdatedDate = DateTime.Now;
                    batch.UpdatedBy = Convert.ToInt32(idString);

                    if (batchStatusModel.StageId == 4 && batchStatusModel.Status == 1)
                    {
                        if (batch.StageId == 4 && batch.Status == 0)
                        {
                            batch.BatchStatus = 0;
                        }
                    }

                    if (!(batchStatusModel.StageId == 4 && batchStatusModel.Status == 0))
                    {
                        LogWriter logWriter2 = new LogWriter("API call update " + batchStatusModel.BatchKey + " ,StageId " + batchStatusModel.StageId + ",Status " + batchStatusModel.Status + ",DepartmentId " + batchStatusModel.DepartmentId + ", Count " + batchMatches.Count + ", Date" + DateTime.Now);
                        IBatchService.UpdateData(batch);
                    }

                    if ((batchStatusModel.StageId == 3 && batchStatusModel.Status == 1))
                    {
                        string savePath = ConfigurationManager.AppSettings["SavePath"].ToString();
                        string[] filesToDelete = Directory.GetFiles(savePath, batch.BatchKey + "*");
                        if (filesToDelete.Count() > 0)
                        {
                            foreach (string file in filesToDelete)
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    if ((batch.StageId == 4 && batch.Status == 1 && batchStatusModel.StageId == 4 && batchStatusModel.Status == 1))
                    {
                        DirectoryInfo info = new DirectoryInfo(ConfigurationManager.AppSettings["SavePath"].ToString());
                        FileInfo[] filesByTime = info.GetFiles(batch.BatchKey + "*").OrderBy(x => x.CreationTime).ToArray();

                        //string savePath = ConfigurationManager.AppSettings["SavePath"].ToString();
                        //string[] filesToDelete = Directory.GetFiles(savePath, batch.BatchKey + "*");
                        if (filesByTime.Count() > 0)
                        {
                            foreach (FileInfo fileInfo in filesByTime)
                            {
                                DateTime lastWriteTime = fileInfo.LastWriteTime;
                                DateTime currentDateTime = DateTime.Now;
                                currentDateTime = currentDateTime.AddSeconds(-7);
                                // currentDateTime = currentDateTime.AddSeconds(-1);
                                if (lastWriteTime < currentDateTime)
                                {
                                   // File.Delete(fileInfo.FullName);
                                }
                            }
                        }
                    }
                }
                else
                {
                    DateTime dateTime;
                    string buddhaDateTime = batchStatusModel.CreatedDate.ToString();
                    IFormatProvider buddhaCulture = CultureInfo.CreateSpecificCulture("th-TH");
                    bool IsBuddhaDate = DateTime.TryParse(buddhaDateTime, buddhaCulture, DateTimeStyles.None, out dateTime);
                    if (IsBuddhaDate)
                    {
                        IFormatProvider culture = CultureInfo.CreateSpecificCulture("en-US");
                        DateTime englishDate;
                        if (DateTime.TryParse(dateTime.ToString(), culture, DateTimeStyles.None, out englishDate))
                        {
                            batchStatusModel.CreatedDate = englishDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }
                    Batch batch = new Batch()
                    {
                        BranchId = branch.Id,
                        DepartmentId = department.Id,
                        StageId = batchStatusModel.StageId,
                        BatchKey = batchStatusModel.BatchKey,
                        BatchNo = batchStatusModel.BatchNo,
                        BatchUser = batchStatusModel.UserId,
                        BatchCount = batchStatusModel.BatchCount,
                        BatchStatus = batchStatusModel.Status,
                        CreatedDate =  DateTime.Now,
                        UpdatedDate =  DateTime.Now,
                        //CreatedDate = (batchStatusModel.CreatedDate == "" ? DateTime.Now : Convert.ToDateTime(batchStatusModel.CreatedDate)),
                        //UpdatedDate = (batchStatusModel.UpdatedDate == "" ? DateTime.Now : Convert.ToDateTime(batchStatusModel.UpdatedDate)),
                        CreatedBy = Convert.ToInt32(idString),
                        UpdatedBy = Convert.ToInt32(idString),
                        Status = 1,
                    };
                    LogWriter logWriter2 = new LogWriter("API call insert " + batchStatusModel.BatchKey + " ,StageId " + batchStatusModel.StageId + ",Status " + batchStatusModel.Status + ",DepartmentId " + batchStatusModel.DepartmentId +", Count "+ batchStatusModel.BatchCount + ", Date" + DateTime.Now);

                    IBatchService.PostData(batch);

                }
                    LogWriter logWriter1 = new LogWriter("API call end " + batchStatusModel.BatchKey + " ,StageId " + batchStatusModel.StageId + ",Status " + batchStatusModel.Status + ",DepartmentId " + batchStatusModel.DepartmentId +", Count "+ batchStatusModel.BatchCount + ", Date" + DateTime.Now);

                textReader.Close();
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter("Exception - BatchStatusController: " + ex.Message);

                var httpResponse = ex as HttpResponseException;
                if (httpResponse.Response.StatusCode == HttpStatusCode.Unauthorized && httpResponse.Response.ReasonPhrase == "Department not active!")
                {
                    string customMessage = "Department not active!";
                    HttpResponseMessage response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, customMessage);
                    response.ReasonPhrase = customMessage;
                    throw new HttpResponseException(response);
                }
            }
        }
    }
}
