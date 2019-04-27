using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("user_securities")]
    public class UserSecurity
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("user_id"), Display("UserId")]
        public Int32 UserId { get; set; }
        [Database("user_security_type_id"), Display("UserSecurityTypeId")]
        public Int32 UserSecurityTypeId { get; set; }
        [Database("url"), Display("Url")]
        public String Url { get; set; }
        [Database("value"), Display("Value")]
        public String Value { get; set; }
        [Database("verification_pin"), Display("VerificationPin")]
        public Int32? VerificationPin { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("is_verified"), Display("IsVerified")]
        public Int32 IsVerified { get; set; }
        [Database("verified_date"), Display("VerifiedDate")]
        public DateTime? VerifiedDate { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        public UserSecurityType UserSecurityTypeIdUserSecurityType { get; set; }
        public User UserIdUser { get; set; }
    }
}
