using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class Searcher : System.Web.UI.Page
    {
        private static readonly Lazy<ISetDocumentService> lazySetDocumentServiceObj = new Lazy<ISetDocumentService>(() => new SetDocumentService());

        public static ISetDocumentService ISetDocumentService { get { return lazySetDocumentServiceObj.Value; } }

        private static readonly Lazy<ISetService> lazySetServiceObj = new Lazy<ISetService>(() => new SetService());

        public static ISetService ISetService { get { return lazySetServiceObj.Value; } }

        private static readonly Lazy<IBatchLogService> lazyBatchLogServiceObj = new Lazy<IBatchLogService>(() => new BatchLogService());

        public static IBatchLogService IBatchLogService { get { return lazyBatchLogServiceObj.Value; } }

        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["hdnNumberPerPage"] != "" && Request.QueryString["hdnNumberPerPage"] != null)
            {
                hdnNumberPerPage.Value = Request.QueryString["hdnNumberPerPage"].ToString();
            }
            if (Request.QueryString["hdnCurrentPageNo"] != "" && Request.QueryString["hdnCurrentPageNo"] != null)
            {
                hdnCurrentPageNo.Value = Request.QueryString["hdnCurrentPageNo"].ToString();
            }
            if (Request.QueryString["hdnTotalRecordsCount"] != "" && Request.QueryString["hdnTotalRecordsCount"] != null)
            {
                hdnTotalRecordsCount.Value = Request.QueryString["hdnTotalRecordsCount"].ToString();
            }

            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1 ");

            if (Request.QueryString["IsNewSearch"] != "" && Request.QueryString["IsNewSearch"] != null)
            {
                IsNewSearch.Value = Request.QueryString["IsNewSearch"].ToString();
            }
            if (IsNewSearch.Value == "1")
            {
                hdnCurrentPageNo.Value = "";
            }
            if (Request.QueryString["Search"] != "" && Request.QueryString["Search"] != null)
            {
                Search.Value = Request.QueryString["Search"].ToString();
                string columnNameName = Converter.GetColumnNameByPropertyName<Company>(nameof(Silverlake.Utility.Company.Name));
                filter.Append(" and " + columnNameName + " like '%" + Search.Value + "%'");
            }

            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = ISetService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<Set> objs = ISetService.GetDataByFilter(filter.ToString(), skip, take, true);

            StringBuilder asb = new StringBuilder();
            int index = 1;
            StringBuilder setsHTML = new StringBuilder();
            foreach (Set obj in objs)
            {
                List<SetDocument> setDocuments = ISetDocumentService.GetDataByPropertyName(nameof(SetDocument.SetId), obj.Id.ToString(), true, 0, 0, false);
                StringBuilder setDocumentsHTML = new StringBuilder();
                foreach (SetDocument setDocument in setDocuments)
                {
                    string fileName = Path.GetFileName(setDocument.DocumentUrl);
                    string localUrl = Server.MapPath("/Content/Files/" + fileName);
                    if (!File.Exists(localUrl))
                        File.Copy(setDocument.DocumentUrl, localUrl);
                    setDocumentsHTML.Append(@"<div><a href='/Content/Files/" + fileName + "' target='_blank' data-original-title='Click to view' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + setDocument.DocType + @"</strong></a> ( <small> pages: <strong>" + setDocument.PageCount + @"</strong></small> )</div>");
                }
                string xmlFileName = Path.GetFileName(obj.SetXmlPath);
                string localXMLUrl = Server.MapPath("/Content/Files/" + xmlFileName);
                if (!File.Exists(localXMLUrl))
                    File.Copy(obj.SetXmlPath, localXMLUrl);
                setsHTML.Append(@"<tr>
                                    <td>"+ index + @"</td>
                                    <td><a href='/Content/Files/" + xmlFileName + @"' target='_blank' data-original-title='Click to view XML' data-trigger='hover' data-placement='bottom' class='popovers text-info'><strong>" + obj.AccountNo + @"</strong></a></td>
                                    <td>" + setDocumentsHTML.ToString() + @"</td>
                                </tr>
                            ");
                index++;
            }
            setsTBody.InnerHtml = setsHTML.ToString();
        }
    }
}