using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.CustomClasses
{
    public class Email
    {
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public User User { get; set; }
        public string Link { get; set; }
    }
}
