using Silverlake.Utility.CustomClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silverlake.Utility.Helper
{
    public class EmailService
    {
        public static string SupportEmail = "";
        public static string SupportEmailHost = "";
        public static string SupportEmailPassword = "";
        public static int SupportEmailPort = 0;

        public EmailService()
        {
            SupportEmail = ConfigurationManager.AppSettings["SupportEmail"].ToString();
            SupportEmailHost = ConfigurationManager.AppSettings["SupportEmailHost"].ToString();
            SupportEmailPassword = ConfigurationManager.AppSettings["SupportEmailPassword"].ToString();
            SupportEmailPort = Convert.ToInt32(ConfigurationManager.AppSettings["SupportEmailPort"].ToString());
        }

        public static Boolean SendVerificationLink(Email email)
        {
            new EmailService();
            bool send;
            try
            {
                string FilePath = HttpContext.Current.Server.MapPath("~/EmailTemplate/AccountActivation.html");
                StreamReader str = new StreamReader(FilePath);
                string mailText = str.ReadToEnd();
                str.Close();
                using (MailMessage mm = new MailMessage(SupportEmail, email.User.EmailId))
                {
                    mm.Subject = email.Subject;
                    mailText = mailText.Replace("[url]", email.Link +"?key=" + email.User.UniqueKey);
                    mailText = mailText.Replace("[username]", email.User.Username);
                    //string body = "Hello " + email.user.Username + ",";
                    //body += "<br /><br />Please activate your MA account with Activation code sent to your mobile.";
                    //body += "<br /><a href = '" + email.link + "?key=" + email.user.UniqueKey + "'>Click here to activate your DiOTP account.</a>";
                    //body += "<br /><br />Thanks";
                    mm.Body = mailText;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = SupportEmailHost;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(SupportEmail, SupportEmailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = SupportEmailPort;
                    smtp.Send(mm);
                }
                send = true;
            }
            catch
            {
                send = false;
            }
            return send;
        }
        public static Response SendActivationMail(Email email)
        {
            Response response = new Response();
            new EmailService();
            bool send;
            try
            {
                using (MailMessage mm = new MailMessage(SupportEmail, email.User.EmailId))
                {
                    mm.Subject = email.Subject;
                    //email.User.TransPwd is Branch Code
                    string body = "Hello " + email.User.Username + ",";
                    body += "<br /><br />Your account is activated.";
                    body += "<br /><br />Username: <strong>" + email.User.Username + "</strong>";
                    body += "<br /><br />Password: <strong>" + CustomEncryptorDecryptor.DecryptPassword(email.User.Password) + "</strong>";
                    body += "<br /><br />Branch: <strong>" + email.User.TransPwd + "</strong>";
                    body += "<br /><br />API Auth Token: <strong>" + email.User.ApiAuthToken + "</strong>";
                    body += "<br /><br /><a href='" + email.Link + "'>Click here to Login.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = SupportEmailHost;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(SupportEmail, SupportEmailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = SupportEmailPort;
                    smtp.Send(mm);
                }
                send = true;
                response.isSuccess = true;
                response.message = "Success";
            }
            catch(Exception ex)
            {
                send = false;
                response.isSuccess = false;
                response.message = ex.Message;
            }
            return response;
        }
        public static Boolean SendResetPasswordLink(Email email)
        {
            new EmailService();
            bool send;
            try
            {
                using (MailMessage mm = new MailMessage(SupportEmail, email.User.EmailId))
                {
                    mm.Subject = email.Subject;
                    string body = "Hello " + email.User.Username + ",";
                    body += "<br /><br />Please click the following link to reset your account password.";
                    byte[] byteArray = Encoding.UTF8.GetBytes(email.User.UniqueKey);
                    string base64String = Convert.ToBase64String(byteArray);
                    body += "<br /><a href = '" + email.Link + "?key=" + base64String + "'>Click here to reset your password.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = SupportEmailHost;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(SupportEmail, SupportEmailPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = SupportEmailPort;
                    smtp.Send(mm);
                }
                send = true;
            }
            catch
            {
                send = false;
            }
            return send;
        }
    }
}
