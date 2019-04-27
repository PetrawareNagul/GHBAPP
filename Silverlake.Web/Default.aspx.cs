using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using Silverlake.Web.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Silverlake.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly Lazy<IUserService> lazyUserServiceObj = new Lazy<IUserService>(() => new UserService());

        public static IUserService IUserService { get { return lazyUserServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                Int32 UserId = Convert.ToInt32(Session["UserId"].ToString());
                User user = IUserService.GetSingle(UserId);
                string userType = Session["UserType"].ToString();
                string userRole = Session["UserRole"].ToString();
                userName.InnerHtml = user.Username + "<small class='text-uppercase'> - " + userRole + "</small>";
                StringBuilder menuHTML = new StringBuilder();
                switch (userRole)
                {
                    case "Super Admin":
                        menuHTML.Append(@"
                                            <li>
                                                <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Dashboard.aspx' data-title='Dashboard' data-closable='false'>
                                                    <span>Dashboard</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Statistic.aspx' data-title='Statistics'>
                                                    <span>Statistics</span>
                                                </a>
                                            </li>
                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Monitoring/Reporting</span>
                                                </a>
                                                <ul class='sub'>
                                                   <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='AccountEnquiry.aspx' data-title='AccountEnquiry'>
                                                            <span>Account Enquiry</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Sets.aspx' data-title='Enquiry'>
                                                            <span>Enquiry</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Exceptions.aspx' data-title='Exceptions'>
                                                            <span>Exceptions</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Configuration</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='AdminList.aspx' data-title='Users'>
                                                            <span>Users</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='DepartmentList.aspx' data-title='Department List'>
                                                            <span>Department List</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='BranchList.aspx' data-title='Branch List'>
                                                            <span>Branch List</span>
                                                        </a>
                                                    </li>
                                                    <!--<li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='DocumentSeparators.aspx' data-title='Document Separators'>
                                                            <span>Document Separators</span>
                                                        </a>
                                                    </li>-->
                                                </ul>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Content Management</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='LanguageDirectory.aspx' data-title='Language Directory'>
                                                            <span>Language Directory</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>
                        ");
                        break;
                    case "HQ Admin":
                        menuHTML.Append(@"
                                           <li>
                                                <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Dashboard.aspx' data-title='Dashboard' data-closable='false'>
                                                    <span>Dashboard</span>
                                                </a>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Monitoring/Reporting</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Sets.aspx' data-title='Enquiry'>
                                                            <span>Enquiry</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Exceptions.aspx' data-title='Exceptions'>
                                                            <span>Exceptions</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Configuration</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='AdminList.aspx' data-title='Admin List'>
                                                            <span>Users</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='DepartmentList.aspx' data-title='Department List'>
                                                            <span>Department List</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='BranchList.aspx' data-title='Branch List'>
                                                            <span>Branch List</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Content Management</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='LanguageDirectory.aspx' data-title='Language Directory'>
                                                            <span>Language Directory</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>
                        ");
                        break;
                    case "Regional Admin":
                        menuHTML.Append(@"
                                            <li>
                                                <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Dashboard.aspx' data-title='Dashboard' data-closable='false'>
                                                    <span>Dashboard</span>
                                                </a>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <i class='fa fa-building'></i>
                                                    <span>Monitoring/Reporting</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Sets.aspx' data-title='Enquiry'>
                                                            <span>Enquiry</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Exceptions.aspx' data-title='Exceptions'>
                                                            <span>Exceptions</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <span>Configuration</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='AdminList.aspx' data-title='Admin List'>
                                                            <span>Users</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>

                        ");
                        break;
                    case "Branch Admin":
                        menuHTML.Append(@"
                                            <li>
                                                <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Dashboard.aspx' data-title='Dashboard' data-closable='false'>
                                                    <span>Dashboard</span>
                                                </a>
                                            </li>

                                            <li class='sub-menu'>
                                                <a href='javascript:;'>
                                                    <i class='fa fa-building'></i>
                                                    <span>Monitoring/Reporting</span>
                                                </a>
                                                <ul class='sub'>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Sets.aspx' data-title='Enquiry'>
                                                            <span>Enquiry</span>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href='javascript:;' class='easyui-linkbutton tab-menu-item' data-url='Exceptions.aspx' data-title='Exceptions'>
                                                            <span>Exceptions</span>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </li>
                        ");
                        break;
                }
                navAccordion.InnerHtml = menuHTML.ToString();
            }


            //List<Sample> samples = TransmissionApiCalls.GetData();
            //int index = 1;
            //foreach (Sample sample in samples)
            //{
            //    XmlDocument xmlDoc = new XmlDocument();
            //    String XmlString = Encoding.UTF8.GetString(sample.XmlString);

            //    StringWriter sw = new StringWriter();
            //    XmlTextWriter xw = new XmlTextWriter(sw);
            //    xmlDoc.WriteTo(xw);
            //    xmlDoc.LoadXml(XmlString);

            //    string savePath = "D:\\SavePath\\";
            //    xmlDoc.Save(savePath + "sample" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + index + ".xml");
            //    index++;
            //}

            //string postXMLUrl = "C:\\Users\\PetraLap3\\Desktop\\sample.xml";
            //for (int i = 0; i < 100; i++)
            //{
            //    TransmissionApiCalls.PostXMLData(postXMLUrl);
            //}
            //string imageFilesPath = ConfigurationManager.AppSettings["imageFilesPath"].ToString();
            //string[] files = Directory.GetFiles(imageFilesPath);
            //foreach (string file in files)
            //{
            //    string postImageUrl = file;
            //    TransmissionApiCalls.PostFile(postImageUrl);
            //}

            //string batchStatusXMLFilesPath = ConfigurationManager.AppSettings["batchStatusPath"].ToString();
            //string[] files = Directory.GetFiles(batchStatusXMLFilesPath);
            //foreach (string file in files)
            //{
            //    string postXMLUrl = file;
            //    TransmissionApiCalls.PostBatchStatusXMLFile(postXMLUrl);
            //}
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Int32 UserId = Convert.ToInt32(Session["UserId"].ToString());
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LoginHistory.aspx?id=" + UserId);
        }
    }
}