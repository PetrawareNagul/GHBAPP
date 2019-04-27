using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class MapDepartments : System.Web.UI.Page
    {
        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<ICompanyService> lazyCompanyServiceObj = new Lazy<ICompanyService>(() => new CompanyService());

        public static ICompanyService ICompanyService { get { return lazyCompanyServiceObj.Value; } }

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
                string columnNameName = Converter.GetColumnNameByPropertyName<Branch>(nameof(Silverlake.Utility.Branch.Name));
                filter.Append(" and " + columnNameName + " like '%" + Search.Value + "%'");
                string columnNameCode = Converter.GetColumnNameByPropertyName<Branch>(nameof(Silverlake.Utility.Branch.Code));
                filter.Append(" or " + columnNameCode + " like '%" + Search.Value + "%'");
            }


            int skip = 0, take = 10;
            if (hdnCurrentPageNo.Value == "")
            {
                skip = 0;
                take = 10;
                hdnNumberPerPage.Value = "10";
                hdnCurrentPageNo.Value = "1";
                hdnTotalRecordsCount.Value = IBranchService.GetCountByFilter(filter.ToString()).ToString();
            }
            else
            {
                skip = (Convert.ToInt32(hdnCurrentPageNo.Value) - 1) * 10;
                take = 10;
            }

            List<Branch> objs = IBranchService.GetDataByFilter(filter.ToString(), skip, take, true);

            StringBuilder asb = new StringBuilder();
            int index = 1;
            foreach (Branch obj in objs)
            {
                Company company = ICompanyService.GetSingle(obj.CompanyId);
                asb.Append(@"<tr>
                                <td class='icheck'>
                                    <div class='square single-row'>
                                        <div class='checkbox'>
                                            <input type='checkbox' name='checkRow' class='checkRow' value='" + obj.Id + @"' /> <label>" + index + @"</label><br/>
                                        </div>
                                    </div>
                                    <span class='row-status'>" + (obj.Status == 1 ? "<span class='label label-success'>Active</span>" : "<span class='label label-danger'>Inactive</span>") + @"</span>
                                </td>
                                <td>" + obj.Code + @"</td>
                                <td>" + obj.Name + @"</td>
                                <td>" + obj.CreatedDate.ToString("dd/MM/yyyy") + @"</td>
                                <td>" + (obj.UpdatedDate == null ? "-" : obj.UpdatedDate.Value.ToString("dd/MM/yyyy")) + @"</td>
                            </tr>");
                index++;
            }
            branchesTBody.InnerHtml = asb.ToString();
        }
    }
}