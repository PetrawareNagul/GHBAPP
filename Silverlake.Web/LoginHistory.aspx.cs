using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class LoginHistory : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        private static readonly Lazy<IUserLoginHistoryService> lazyUserLoginHistoryServiceObj = new Lazy<IUserLoginHistoryService>(() => new UserLoginHistoryService());

        public static IUserLoginHistoryService IUserLoginHistoryService { get { return lazyUserLoginHistoryServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Request.QueryString["id"].ToString();
            Int32 id = Convert.ToInt32(userId);
            User user = IUserService.GetSingle(id);

            List<UserLoginHistory> loginHistorys = IUserLoginHistoryService.GetDataByPropertyName(nameof(UserLoginHistory.UserId), userId, true , 0, 5, true);
            StringBuilder asb = new StringBuilder();

            foreach(UserLoginHistory loginHistory in loginHistorys)
            {
                List<string> loginDetails = loginHistory.LoginIp.Split(new string[] { ", " }, StringSplitOptions.None).ToList();
                List<string> ipaddress = loginDetails.Skip(1).ToList();
                asb.Append(@"<tr>
                                <td>" + loginHistory.LoginDate.ToString("dd/MM/yyyy HH:mm:ss") + @"</td>
                                <td>" + loginDetails.FirstOrDefault() + @"</td>
                                <td>" + String.Join(", ", ipaddress.ToArray()) + @"</td>
                            </tr>");
            }
            loginHistoryTbody.InnerHtml = asb.ToString();

        }
    }
}