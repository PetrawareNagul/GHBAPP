using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("user_login_history")]
    public class UserLoginHistory
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("user_id"), Display("UserId")]
        public Int32 UserId { get; set; }
        [Database("login_date"), Display("LoginDate")]
        public DateTime LoginDate { get; set; }
        [Database("login_ip"), Display("LoginIp")]
        public String LoginIp { get; set; }
    }
}
