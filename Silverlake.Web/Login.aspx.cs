using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.CustomClasses;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class Login : System.Web.UI.Page
    {

        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IUserRoleService> lazyUserRoleServiceObj = new Lazy<IUserRoleService>(() => new UserRoleService());

        public static IUserRoleService IUserRoleService { get { return lazyUserRoleServiceObj.Value; } }

        private static readonly Lazy<IUserTypeService> lazyUserTypeServiceObj = new Lazy<IUserTypeService>(() => new UserTypeService());

        public static IUserTypeService IUserTypeService { get { return lazyUserTypeServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string encryptedPassword = CustomEncryptorDecryptor.EncryptPassword(password);
                StringBuilder filter = new StringBuilder();
                filter.Append(" 1=1");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(Utility.User.Username)) + " = '" + username + "'");
                filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(Utility.User.Password)) + " = '" + encryptedPassword + "'");
                List<User> userMatches = IUserService.GetDataByFilter(filter.ToString(), 0, 0, false);
                if (userMatches.Count > 0)
                {
                    User user = userMatches.FirstOrDefault();
                    filter = new StringBuilder();
                    UserRole userRole = IUserRoleService.GetSingle(user.UserRoleId);
                    UserType userType = IUserTypeService.GetSingle(userRole.UserTypeId);
                    if (userType.Name == "Super Admin" || userType.Name == "Admin")
                    {
                        Session["UserId"] = user.Id;
                        Session["UserType"] = userType.Name;
                        Session["UserRole"] = userRole.Name;

                        user.LastLoginOn = DateTime.Now;
                        string windowsUsername = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        //string pcName = Dns.GetHostName();
                        IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
                        IPAddress[] IPaddr = ip.AddressList;
                        user.LastLoginIp = windowsUsername + ", " + String.Join(", ", IPaddr.Select(x => x.ToString()).ToArray<string>());
                        IUserService.UpdateData(user);

                        Response.Redirect("Default.aspx");
                    }
                }
            }
        }

        protected void forgotPasswordSubmit_Click(object sender, EventArgs e)
        {
            if (forgotPasswordEmail.Text != "")
            {
                string emailId = forgotPasswordEmail.Text;
                User user = IUserService.GetDataByPropertyName(nameof(Utility.User.EmailId), emailId, true, 0, 0, false).FirstOrDefault();
                if (user != null)
                {
                    string siteURL = ConfigurationManager.AppSettings["siteURL"];
                    Email email = new Email()
                    {
                        Subject = "Reset Password - GHB",
                        FromEmail = "pecmsupport@petraware.com",
                        Host = "mail.petraware.com",
                        Link = siteURL + "ResetPassword.aspx",
                        Password = "petraware123",
                        Port = 587,
                        User = user
                    };
                    EmailService.SendResetPasswordLink(email);
                    user.IsPasswordReset = 1;
                    IUserService.UpdateData(user);
                    forgotPasswordResponse.InnerHtml = "";
                }
            }
            else
            {
                forgotPasswordResponse.InnerHtml = "Please enter email";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "modal", "OpenForgotPasswordModal();", true);
            }
        }
    }
}