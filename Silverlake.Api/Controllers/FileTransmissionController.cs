using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Silverlake.Api.Controllers
{
    [Authorize]
    public class FileTransmissionController : ApiController
    {
        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());

        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

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

                Stream stream = request.Content.ReadAsStreamAsync().Result;
                byte[] fileBytes = new byte[stream.Length];
                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();
                string FileName = request.Headers.ToList().Where(x => x.Key == "FileName").FirstOrDefault().Value.FirstOrDefault().ToString();

                string mimeType = request.Content.Headers.ContentType.MediaType;
                string fileExtension = Extension.GetDefaultExtension(mimeType);

                if (fileExtension == null || fileExtension == "")
                {
                    fileExtension = "." + mimeType.Split('/')[1];
                }

                string FileNameWithoutExtension = FileName.Split('.')[0];
                string[] keys = FileNameWithoutExtension.Split('_');
                //string batchKey = keys[0];
                string batchKey = keys[0];
                //string batchNo = keys[1];
                string setKey = keys[1];
                string aaNo = keys[2];
                string deptCode = keys[3];
                string branchCode = keys[4];
                string accNo = keys[5];
                string docType = "";
                string pageCount = "";

                string fileNameOld = "";
                string savePath = ConfigurationManager.AppSettings["SavePath"].ToString();
                string[] filesToDelete = new string[] { };
                string[] filesToDeletePDF = new string[] { };
                LogWriter logWriter = new LogWriter("BatchStatusController: Check");
                if (fileExtension == ".pdf")
                {
                    docType = keys[6];
                    pageCount = keys[7];

                    fileNameOld = batchKey + "_" + setKey + "_" + aaNo + "_" + deptCode + "_" + branchCode + "_" + accNo + "_" + docType + "_" + pageCount;
                    filesToDeletePDF = Directory.GetFiles(savePath, fileNameOld + "*.pdf");
                }
                else if (fileExtension == ".xml")
                {
                    fileNameOld = batchKey + "_" + setKey + "_" + aaNo + "_" + deptCode + "_" + branchCode + "_" + accNo;
                    logWriter = new LogWriter("BatchStatusController: fileNameOld - " + fileNameOld);
                    filesToDelete = Directory.GetFiles(savePath, fileNameOld + "*.xml");
                    logWriter = new LogWriter("BatchStatusController: filesToDelete Count - " + filesToDelete.Count());
                }

                //if (filesToDelete.Count() > 0)
                //{
                //    foreach (string fNamePath in filesToDelete)
                //    {
                //        string fName = Path.GetFileName(fNamePath);
                //        string[] keysOld = fName.Split('_');
                //        string timestampOld = keysOld[6];
                //        logWriter = new LogWriter("BatchStatusController: timestampOld - " + timestampOld);

                //        string[] AllFilesToDelete = Directory.GetFiles(savePath, fileNameOld + "*" + timestampOld + "*");
                //        logWriter = new LogWriter("BatchStatusController: fName - " + fName);
                //        logWriter = new LogWriter("BatchStatusController: AllFilesToDelete - " + AllFilesToDelete.Count());

                //        foreach (string file in AllFilesToDelete)
                //        {
                //            File.Delete(file);
                //        }
                //    }
                //}
                //if (filesToDeletePDF.Count() > 0)
                //{
                //    foreach (string fNamePath in filesToDeletePDF)
                //    {
                //        string fName = Path.GetFileName(fNamePath);
                //        string[] keysOld = fName.Split('_');
                //        string timestampOld = keysOld[8];
                //        timestampOld = timestampOld.Remove(timestampOld.Length - 1);
                //        logWriter = new LogWriter("BatchStatusController: timestampOld - " + timestampOld);

                //        string[] AllFilesToDelete = Directory.GetFiles(savePath, fileNameOld + "*" + timestampOld + "*");
                //        logWriter = new LogWriter("BatchStatusController: fName - " + fName);
                //        logWriter = new LogWriter("BatchStatusController: AllFilesToDelete - " + AllFilesToDelete.Count());

                //        foreach (string file in AllFilesToDelete)
                //        {
                //            File.Delete(file);
                //        }
                //    }
                //}
                Branch branch = new Branch();
                Department department = new Department();
                BranchDepartment branchDepartment = new BranchDepartment();
                List<Branch> branchMatches = IBranchService.GetDataByPropertyName(nameof(Branch.Code), branchCode, true, 0, 0, true);
                List<Department> departmentMatches = IDepartmentService.GetDataByPropertyName(nameof(Department.Code), deptCode, true, 0, 0, true);

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

                    string deptId = department.Id.ToString();
                    string branchId = branch.Id.ToString();


                    string localPath = "/Content/Files/";
                    string dirFullPath = HttpContext.Current.Server.MapPath(localPath);

                    //string savePath = ConfigurationManager.AppSettings["SavePath"].ToString();
                    string filePath = savePath + FileName;
                    string fileFullPath = savePath + FileName;
                    if (File.Exists(fileFullPath))
                        File.Delete(fileFullPath);
                    using (Stream file = File.OpenWrite(fileFullPath))
                    {
                        file.Write(fileBytes, 0, fileBytes.Length);
                    }

                }
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
