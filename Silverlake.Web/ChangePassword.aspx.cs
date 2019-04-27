using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            string isSubmit = Request.QueryString["isSubmit"];
            if (isSubmit != null)
                if (isSubmit.ToString() == "1")
                {
                    string oldPassword = Request.QueryString["txtOldPassword"].ToString();
                    txtOldPassword.Text = oldPassword;
                    string newPassword = Request.QueryString["txtNewPassword"].ToString();
                    txtNewPassword.Text = newPassword;
                    string confirmPassword = Request.QueryString["txtConfirmPassword"].ToString();
                    txtConfirmPassword.Text = confirmPassword;
                    if (txtOldPassword.Text != "" && txtNewPassword.Text != "" && txtConfirmPassword.Text != "")
                    {
                        Int32 UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                        User user = IUserService.GetSingle(UserId);
                        if (user != null)
                        {
                            string encryptOldPassword = CustomEncryptorDecryptor.EncryptPassword(txtOldPassword.Text);
                            if (encryptOldPassword == user.Password)
                            {
                                if (txtNewPassword.Text == txtConfirmPassword.Text)
                                {
                                    string encryptPassword = CustomEncryptorDecryptor.EncryptPassword(txtNewPassword.Text);
                                    if (encryptPassword != user.Password)
                                    {
                                        user.Password = encryptPassword;
                                        user.TransPwd = encryptPassword;
                                        user.UniqueKey = CustomGenerator.GenerateUniqueKeyForUser(user.Username);
                                        user.UpdatedOn = DateTime.Now;
                                        user.UpdatedBy = LoginUserId;
                                        IUserService.UpdateData(user);
                                        resetPasswordResponse.InnerHtml = "Updated successfully!";
                                    }
                                    else
                                    {
                                        resetPasswordResponse.InnerHtml = "Please enter new password!";
                                    }
                                }
                                else
                                {
                                    resetPasswordResponse.InnerHtml = "Passwords doesn't match!";
                                }
                            }
                            else
                            {
                                resetPasswordResponse.InnerHtml = "Please enter correct password!";
                            }
                        }
                        else
                        {
                            resetPasswordResponse.InnerHtml = "No user!";
                        }
                    }
                }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Int32 LoginUserId = 0;
            if (HttpContext.Current.Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
            }
            if (Session["UserId"] == null)
            {
            }
            else
            {
                Int32 UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = IUserService.GetSingle(UserId);
                if (user != null)
                {
                    if (txtOldPassword.Text != "" && txtNewPassword.Text != "" && txtConfirmPassword.Text != "")
                    {
                        string encryptOldPassword = CustomEncryptorDecryptor.EncryptPassword(txtOldPassword.Text);
                        if (encryptOldPassword == user.Password)
                        {
                            if (txtNewPassword.Text == txtConfirmPassword.Text)
                            {
                                string encryptPassword = CustomEncryptorDecryptor.EncryptPassword(txtNewPassword.Text);
                                if (encryptPassword != user.Password)
                                {
                                    user.Password = encryptPassword;
                                    user.TransPwd = encryptPassword;
                                    user.UniqueKey = CustomGenerator.GenerateUniqueKeyForUser(user.Username);
                                    user.UpdatedOn = DateTime.Now;
                                    user.UpdatedBy = LoginUserId;
                                    IUserService.UpdateData(user);
                                    resetPasswordResponse.InnerHtml = "Updated successfully!";
                                }
                                else
                                {
                                    resetPasswordResponse.InnerHtml = "Please enter new password!";
                                }
                            }
                            else
                            {
                                resetPasswordResponse.InnerHtml = "Passwords doesn't match!";
                            }
                        }
                        else
                        {
                            resetPasswordResponse.InnerHtml = "Please enter correct password!";
                        }
                    }
                    else
                    {
                        resetPasswordResponse.InnerHtml = "Please enter all fields!";
                    }
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object SubmitClick(string txtOldPassword, string txtNewPassword, string txtConfirmPassword)
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                return null;
            }
            else
            {
                Int32 UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                User user = IUserService.GetSingle(UserId);
                if (user != null)
                {
                    if (txtOldPassword != "" && txtNewPassword != "" && txtConfirmPassword != "")
                    {
                        string encryptOldPassword = CustomEncryptorDecryptor.EncryptPassword(txtOldPassword);
                        if (encryptOldPassword == user.Password)
                        {
                            if (txtNewPassword == txtConfirmPassword)
                            {
                                string encryptPassword = CustomEncryptorDecryptor.EncryptPassword(txtNewPassword);
                                if (encryptPassword != user.Password)
                                {
                                    user.Password = encryptPassword;
                                    user.TransPwd = encryptPassword;
                                    user.UniqueKey = CustomGenerator.GenerateUniqueKeyForUser(user.Username);
                                    user.UpdatedOn = DateTime.Now;
                                    user.UpdatedBy = UserId;
                                    IUserService.UpdateData(user);
                                    return "Updated successfully!";
                                }
                                else
                                {
                                    return "Please enter new password!";
                                }
                            }
                            else
                            {
                                return "Passwords doesn't match!";
                            }
                        }
                        else
                        {
                            return "Please enter correct password!";
                        }
                    }
                    else
                    {
                        return "Please enter all fields!";
                    }
                }
                else
                {
                    return "No user!";
                }
            }
        }
    }
}