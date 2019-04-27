using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class MfileLogs : System.Web.UI.Page
    {
        public string MimzyCaptureOuputELIBRARYEx = ConfigurationManager.AppSettings["MimzyCaptureOuputELIBRARYEx"].ToString();
        public string MimzyCaptureOuputETPEx = ConfigurationManager.AppSettings["MimzyCaptureOuputETPEx"].ToString();
        public string MimzyCaptureOuputLOSEx = ConfigurationManager.AppSettings["MimzyCaptureOuputLOSEx"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

       // public void delete()
    }
}