using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class AddLanguageDir : System.Web.UI.Page
    {
        private static readonly Lazy<ILanguageDirService> lazyLanguageDirService = new Lazy<ILanguageDirService>(() => new LanguageDirService());

        public static ILanguageDirService ILanguageDirService { get { return lazyLanguageDirService.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo currentDir = new DirectoryInfo(path);
            DirectoryInfo parentDir = new DirectoryInfo(currentDir.Parent.FullName + "\\Silverlake.Web\\");
            FileInfo[] Files = parentDir.GetFiles("*.aspx");
            TextPage.DataSource = Files;
            TextPage.DataTextField = "Name";
            TextPage.DataValueField = "Name";
            TextPage.DataBind();

            //TextPage.Items.Insert(0, new ListItem()
            //{
            //    Text = "Select",
            //    Value = ""
            //});

            string idString = Request.QueryString["id"];
            if (idString != null && idString != "")
            {
                int id = Convert.ToInt32(idString);
                LanguageDir languageDir = ILanguageDirService.GetSingle(id);
                ID.Value = languageDir.Id.ToString();
                Module.Value = languageDir.Module.ToString();
                TextId.Value = languageDir.TextId;
                TextPage.Value = languageDir.TextPage.ToString();
                TextEn.Value = languageDir.TextEn.ToString();
                TextMs.Value = languageDir.TextMs.ToString();
                TextZh.Value = languageDir.TextZh.ToString();
                TextTh.Value = languageDir.TextTh.ToString();
                Status.Value = languageDir.Status.ToString();
                Remarks.Value = languageDir.Remarks;
            }
        }
    }
}