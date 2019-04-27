using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class LanguageDirectory : System.Web.UI.Page
    {
        private static readonly Lazy<ILanguageDirService> lazyLanguageDirService = new Lazy<ILanguageDirService>(() => new LanguageDirService());

        public static ILanguageDirService ILanguageDirService { get { return lazyLanguageDirService.Value; } }

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
            if(IsNewSearch.Value == "1")
            {
                hdnCurrentPageNo.Value = "";
            }
            if (Request.QueryString["Search"] != "" && Request.QueryString["Search"] != null)
            {
                Search.Value = Request.QueryString["Search"].ToString();
                string columnNameTextPage = Converter.GetColumnNameByPropertyName<LanguageDir>(nameof(LanguageDir.TextPage));
                filter.Append(" and " + columnNameTextPage + " like '%" + Search.Value + "%'");
                string columnNameTextEn = Converter.GetColumnNameByPropertyName<LanguageDir>(nameof(LanguageDir.TextEn));
                filter.Append(" or " + columnNameTextEn + " like '%" + Search.Value + "%'");
                string columnNameTextTh = Converter.GetColumnNameByPropertyName<LanguageDir>(nameof(LanguageDir.TextTh));
                filter.Append(" or " + columnNameTextTh + " like '%" + Search.Value + "%'");
            }
            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = ILanguageDirService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<LanguageDir> languageDirectory = ILanguageDirService.GetDataByFilter(filter.ToString(), skip, take, true);

            StringBuilder asb = new StringBuilder();
            int index = 1;
            foreach (LanguageDir l in languageDirectory)
            {
                asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + l.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (l.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td>" + l.TextPage + @"</td>
                                <td>" + l.TextEn + @"</td>
                                <td>" + l.TextTh + @"</td>
                                <td>" + l.Remarks + @"</td>
                            </tr>");
                index++;
            }
            langDirTableBody.InnerHtml = asb.ToString();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Action(Int32[] ids, String action)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                string idString = String.Join(",", ids);
                List<LanguageDir> languageDirs = ILanguageDirService.GetDataByFilter(" ID in ("+ idString + ")", 0, 0, false);
                if (action == "Deactivate")
                {
                    languageDirs.ForEach(x=> {
                        x.Status = 0;
                    });
                    ILanguageDirService.UpdateBulkData(languageDirs);
                }
                if (action == "Activate")
                {
                    languageDirs.ForEach(x => {
                        x.Status = 1;
                    });
                    ILanguageDirService.UpdateBulkData(languageDirs);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("User accounts action: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Add(LanguageDir obj)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            try
            {
                if (obj.Id == 0)
                {
                    ILanguageDirService.PostData(obj);
                }
                else
                {
                    ILanguageDirService.UpdateData(obj);

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add language dir action: " + ex.Message);
                return false;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object UpdateLanguageDirJSON()
        {
            List<LanguageDir> languageDirs = ILanguageDirService.GetData(0, 0, false);
            string json = JsonConvert.SerializeObject(languageDirs);
            string langDirJsonPath = HttpContext.Current.Server.MapPath("/language/langDir.json");

            File.WriteAllText(langDirJsonPath, json);

            return true;
        }
    }
}