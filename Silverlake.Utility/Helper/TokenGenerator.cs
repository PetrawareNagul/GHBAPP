using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility.Helper
{
    public class TokenGenerator
    {
        public static string Get(string username)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));
            string token = username + builder.ToString() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            return authToken;
        }
    }
}
