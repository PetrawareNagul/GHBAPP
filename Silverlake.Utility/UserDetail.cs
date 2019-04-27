using Silverlake.Utility.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silverlake.Utility
{
    [Database("user_details")]
    public class UserDetail
    {
        [Database("id"), Display("Id")]
        public Int32 Id { get; set; }
        [Database("user_id"), Display("UserId")]
        public Int32 UserId { get; set; }
        [Database("first_name"), Display("FirstName")]
        public String FirstName { get; set; }
        [Database("last_name"), Display("LastName")]
        public String LastName { get; set; }
        [Database("description"), Display("Description")]
        public String Description { get; set; }
        [Database("date_of_birth"), Display("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
        [Database("image_url"), Display("ImageUrl")]
        public String ImageUrl { get; set; }
        [Database("created_by"), Display("CreatedBy")]
        public Int32? CreatedBy { get; set; }
        [Database("created_date"), Display("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Database("modified_by"), Display("ModifiedBy")]
        public Int32? ModifiedBy { get; set; }
        [Database("modified_date"), Display("ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
        [Database("status"), Display("Status")]
        public Int32 Status { get; set; }
        public User ModifiedByUser { get; set; }
        public User CreatedByUser { get; set; }
        public User UserIdUser { get; set; }
    }
}
