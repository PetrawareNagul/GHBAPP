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
    public partial class AddCompany : System.Web.UI.Page
    {
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
            string idString = Request.QueryString["id"];
            if (idString != null && idString != "")
            {
                int id = Convert.ToInt32(idString);
                Company company = ICompanyService.GetSingle(id);

                Id.Value = company.Id.ToString();
                Code.Value = company.Code;
                Name.Value = company.Name;
                Status.Value = company.Status.ToString();
                CreatedBy.Value = company.CreatedBy.ToString();
                CreatedDate.Value = company.CreatedDate.ToString("MM/dd/yyyy");
                UpdatedBy.Value = company.UpdatedBy.ToString();
                UpdatedDate.Value = company.UpdatedDate == null ? DateTime.Now.ToString("MM/dd/yyyy") : company.UpdatedDate.Value.ToString("MM/dd/yyyy");
            }
        }
    }
}