using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class AddBranch : System.Web.UI.Page
    {
        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<ICompanyService> lazyCompanyServiceObj = new Lazy<ICompanyService>(() => new CompanyService());

        public static ICompanyService ICompanyService { get { return lazyCompanyServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }

            string currentDateString = DateTime.Now.ToString("MM/dd/yyyy");
            CreatedBy.Value = "0";
            UpdatedBy.Value = "0";
            CreatedDate.Value = currentDateString;
            UpdatedDate.Value = currentDateString;

            List<Company> companies = ICompanyService.GetData(0, 0, false);
            CompanyId.DataSource = companies;
            CompanyId.DataTextField = "Name";
            CompanyId.DataValueField = "Id";
            CompanyId.DataBind();

            string idString = Request.QueryString["id"];
            if (idString != null && idString != "")
            {
                int id = Convert.ToInt32(idString);
                Branch obj = IBranchService.GetSingle(id);

                CompanyId.Value = obj.CompanyId.ToString();
                Id.Value = obj.Id.ToString();
                Code.Value = obj.Code;
                Name.Value = obj.Name;
                Status.Value = obj.Status.ToString();
                CreatedBy.Value = obj.CreatedBy.ToString();
                CreatedDate.Value = obj.CreatedDate.ToString("MM/dd/yyyy");
                UpdatedBy.Value = obj.UpdatedBy.ToString();
                UpdatedDate.Value = obj.UpdatedDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : obj.UpdatedDate.Value.ToString("MM/dd/yyyy");
                IsAll.Value = obj.IsAll.ToString();
            }
        }
    }
}