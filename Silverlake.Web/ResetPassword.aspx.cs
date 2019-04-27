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
    public partial class ResetPassword : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        public string key = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string base64String = Request.QueryString["key"].ToString();
            byte[] byteArray = Convert.FromBase64String(base64String);
            key = Encoding.UTF8.GetString(byteArray);
            User user = IUserService.GetDataByPropertyName(nameof(Utility.User.UniqueKey), key, true, 0, 0, false).FirstOrDefault();
            if(user != null)
            {

            }
            else
            {
                resetPasswordResponse.InnerHtml = "Link not valid!";
                btnSubmit.Enabled = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            User user = IUserService.GetDataByPropertyName(nameof(Utility.User.UniqueKey), key, true, 0, 0, false).FirstOrDefault();
            if(user != null)
            {
                if(txtNewPassword.Text != "" && txtConfirmPassword.Text != "" && txtNewPassword.Text == txtConfirmPassword.Text)
                {
                    string encryptPassword = CustomEncryptorDecryptor.EncryptPassword(txtNewPassword.Text);
                    if (encryptPassword != user.Password)
                    {
                        user.Password = encryptPassword;
                        user.TransPwd = encryptPassword;
                        user.UniqueKey = CustomGenerator.GenerateUniqueKeyForUser(user.Username);
                        user.UpdatedOn = DateTime.Now;
                        user.UpdatedBy = user.Id;
                        user.IsPasswordReset = 0;
                        IUserService.UpdateData(user);
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        resetPasswordResponse.InnerHtml = "Please enter new password!";
                    }
                }
            }
        }
    }
}