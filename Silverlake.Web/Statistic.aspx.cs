using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web
{
    public partial class Statistic : System.Web.UI.Page
    {
        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());
        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());
        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());
        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }


        public string totalApplication = "";
        public string path = ConfigurationManager.AppSettings["SANDrive"].ToString();

        public string barChatQuery = string.Empty;
        public string pieChatQuery = string.Empty;
        public string totalDepartmentsQuery = string.Empty;

        public string startDate = ""; public string endDate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                FromDate.Value = startDate = Request.QueryString["FromDate"];

            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                ToDate.Value = endDate = Request.QueryString["ToDate"];

            divResult.InnerHtml = "";
            divResult.Visible = false;

            var branches = IBranchService.GetUtilizedBranches();
            BranchId.DataSource = branches;
            BranchId.DataTextField = "Code";
            BranchId.DataValueField = "Id";
            BranchId.DataBind();
            BranchId.Items.Insert(0, new ListItem() { Text = "All", Value = "0" });
            if (!string.IsNullOrEmpty(Request.QueryString["BranchId"]))
                BranchId.SelectedValue = Request.QueryString["BranchId"];

            if (!string.IsNullOrEmpty(Request.QueryString["IsClear"]) && Request.QueryString["IsClear"] == "1")
            {
                FromDate.Value = ToDate.Value = startDate = endDate = string.Empty;
                BranchId.SelectedValue = "0";
            }
            if (datechecking())
            {
                Querys();
                List<StatisticModel> last5DaysCount = IBatchService.GetDaysCountbyDepartment(barChatQuery, 0, 0, true);

                if (last5DaysCount.Count == 0)
                {
                    divResult.InnerHtml = @"<div class='alert alert-block alert-danger fade in'><button data-dismiss='alert' class='close close-sm' type='button'>
                                                <i class='fa fa-times'></i>
                                                </button>
                                                <strong>Oh snap!</strong> No records found for the search criteria entered.</div>";
                    divResult.Visible = true;
                }
                else
                    dataBind(last5DaysCount);

            }
        }


        public bool datechecking()
        {
            try
            {
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    DateTime dt1 = Convert.ToDateTime(startDate);
                    DateTime dt2 = Convert.ToDateTime(endDate);
                    if (dt1 > dt2)
                    {

                        divResult.InnerHtml = @"<div class='alert alert-block alert-danger fade in'><button data-dismiss='alert' class='close close-sm' type='button'>
                                                <i class='fa fa-times'></i>
                                                </button>
                                                <strong>Oh snap!</strong> From date should be less than to date.</div>";
                        divResult.Visible = true;
                        return false;
                    }
                }
            }
            catch
            {

            }
            return true;
        }

        public void dataBind(List<StatisticModel> last5DaysCount)
        {
            DriveInfo dDrive = new DriveInfo(path);
            if (dDrive.IsReady)
            {
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    double totalfilesSize = GetFilesSize(path, "*.*", startDate, endDate);
                    double freeSpace = (totalfilesSize / 1024 / 1024 / 1024);
                    if (freeSpace != 0 && freeSpace < 1)
                        freeSpace = 1;
                    lblfreeSpace.Value = freeSpace.ToString();
                }
                else
                    lblfreeSpace.Value = ((dDrive.TotalSize / 1024 / 1024 / 1024) - (dDrive.TotalFreeSpace / 1024 / 1024 / 1024)).ToString();

                lbltotalSize1.Text = (dDrive.TotalSize / 1024 / 1024 / 1024).ToString();
            }

            StringBuilder DepartmentHtml = new StringBuilder();
            // Department binding
            List<Department> objs = IDepartmentService.GetData(0, 10, false);
            DepartmentHtml.Append("<table width='220'><tr>");
            int count = 1;
            foreach (Department dep in objs)
            {
                if (count == 1)
                    DepartmentHtml.Append(@"<td><table style='font-size:smaller;color:#545454'><tbody>");

                if (count <= 4)
                    DepartmentHtml.Append(@"<tr><td class='legendColorBox'>
                                                            <div style ='border:1px solid #ccc; padding: 1px;' >
                                                                <div style = 'width:4px;height:0;border: 5px solid #" + GetColor(dep.Id) + @"; overflow: hidden'></div></div>
                                                        </td><td class='legendLabel'>&nbsp; " + dep.Code + @"</td>
                                                    </tr>");
                if (count == 4)
                {
                    DepartmentHtml.Append(@"</tbody></table></td>");
                    count = 0;
                }
                count++;
            }
            if (count != 1)
                DepartmentHtml.Append(@"</tbody></table></td>");

            DepartmentHtml.Append("</tr></table>");
            divDepartments.InnerHtml = DepartmentHtml.ToString();

            if (last5DaysCount != null && last5DaysCount.Count != 0)
            {
                StringBuilder daysHtml = new StringBuilder();
                StringBuilder todayHtml = new StringBuilder();
                List<string> departmentsCount = new List<string>();
                string date = string.Empty;
                foreach (DateTime dt in last5DaysCount.Select(x => x.CreatedOn).Distinct().ToList())
                    if (!string.IsNullOrEmpty(date))
                        date += "," + dt.Day + " " + dt.ToString("MMM");
                    else
                        date += dt.Day + " " + dt.ToString("MMM");

                lblDays.Value = date;

                //Today scan chat
                var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                todayHtml.Append("<table id=tbltoDays>");
                string todaySetCount = string.Empty;
                string todayColor = string.Empty;
                foreach (int depid in objs.Select(a => a.Id).Distinct().ToList())
                {
                    todayHtml.Append("<tr class='departmentcolor'><td><label id='departmentcolor'> " + GetColor(depid) + @"</label> </td>");
                    string depCount = "0";
                    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                        depCount = last5DaysCount.Where(a => a.DepartmentId == depid).Sum(a => a.SetCount).ToString();
                    else
                    {
                        if (last5DaysCount.Count(a => a.CreatedOn == todaysDate && a.DepartmentId == depid) != 0)
                            depCount = last5DaysCount.FirstOrDefault(a => a.CreatedOn == todaysDate && a.DepartmentId == depid).SetCount.ToString();
                    }

                    if (!string.IsNullOrEmpty(todaySetCount))
                        todaySetCount += "," + depCount;
                    else
                        todaySetCount = depCount;

                    if (!string.IsNullOrEmpty(todayColor))
                        todayColor += "," + GetColor(depid);
                    else
                        todayColor = GetColor(depid);

                    todayHtml.Append("<td><label id='lbldepartmentsetcount'>" + depCount + @"</label>  </td></tr>");
                }
                todayHtml.Append("</table>");
                divDayCount.InnerHtml = todayHtml.ToString();
                lblTodayCount.Value = todaySetCount;
                lblTodayColor.Value = todayColor;
                // end


                daysHtml.Append("<table id=tblDays>");
                foreach (int departmentId in last5DaysCount.OrderBy(a => a.DepartmentId).Select(a => a.DepartmentId).Distinct().ToList())
                {
                    daysHtml.Append("<tr class='departmentcolor'><td><label id='departmentcolor'> " + GetColor(departmentId) + @"</label> </td>");
                    string depCount = string.Empty;
                    foreach (DateTime dt in last5DaysCount.Select(x => x.CreatedOn).Distinct().ToList())
                    {
                        string depsetCount = last5DaysCount.Count(a => a.DepartmentId == departmentId && a.CreatedOn == dt) != 0 ? last5DaysCount.FirstOrDefault(a => a.DepartmentId == departmentId && a.CreatedOn == dt).SetCount.ToString() : "";
                        if (!string.IsNullOrEmpty(depCount))
                            depCount += "," + depsetCount;
                        else
                        {
                            if (!string.IsNullOrEmpty(depCount))
                                depCount += depsetCount;
                            if (!string.IsNullOrEmpty(depsetCount))
                                depCount += depsetCount;
                            else
                                depCount += ",";
                        }
                    }
                    daysHtml.Append("<td><label id='lbldepartmentsetcount'>" + depCount + @"</label>  </td></tr>");
                }
                daysHtml.Append("</table>");
                divtoDayCount.InnerHtml = daysHtml.ToString();
            }
            // end

            // last month for pie chat
            string monthColors = "";
            List<StatisticModel> monthCount = IBatchService.GetTotalCountbyDepartment(pieChatQuery, 0, 0, true);
            if (monthCount != null && monthCount.Count != 0)
            {
                StringBuilder filter = new StringBuilder();
                filter.AppendLine("<table style='position: absolute; margin-top: 5px; right: 20px; font - size:smaller; color:#545454'>");
                filter.AppendLine("<tbody>");
                foreach (StatisticModel statistic in monthCount.OrderBy(a => a.DepartmentId).ToList())
                {
                    if (!string.IsNullOrEmpty(monthColors))
                        monthColors += "," + GetColor(statistic.DepartmentId);
                    else
                        monthColors = GetColor(statistic.DepartmentId);

                    filter.AppendLine(@"<tr><td class='legendColorBox'> 
                            <div style='border:1px solid #ccc;padding:1px;'><div style ='width:4px;height:0;border:5px solid #" + GetColor(statistic.DepartmentId) + @";overflow:hidden'></div></div></td>
                            <td class='legendLabel'> " + statistic.DepartmentCode + @": <span>" + statistic.SetCount + @"</span></td></tr>");
                }
                filter.AppendLine("</tbody>");
                filter.AppendLine("</table>");
                divMonth.InnerHtml = filter.ToString();
            }
            lblMonth.Value = string.Join(",", monthCount.Select(x => x.SetCount).ToArray());
            lblMonthColors.Value = monthColors;
            //end

            // Total data for pie chat
            string totalColors = "";
            List<StatisticModel> totalStagesCount = IBatchService.GetTotalCountbyDepartment(totalDepartmentsQuery, 0, 0, true);
            if (totalStagesCount != null && totalStagesCount.Count != 0)
            {
                StringBuilder filter = new StringBuilder();
                filter.AppendLine("<table style='position: absolute; margin-top: 5px; right: 20px; font - size:smaller; color:#545454'>");
                filter.AppendLine("<tbody>");
                foreach (StatisticModel statistic in totalStagesCount.OrderBy(a => a.DepartmentId).ToList())
                {
                    if (!string.IsNullOrEmpty(totalColors))
                        totalColors += "," + GetColor(statistic.DepartmentId);
                    else
                        totalColors = GetColor(statistic.DepartmentId);

                    filter.AppendLine(@"<tr><td class='legendColorBox'> 
                            <div style='border:1px solid #ccc;padding:1px;'><div style ='width:4px;height:0;border:5px solid #" + GetColor(statistic.DepartmentId) + @";overflow:hidden'></div></div></td>
                            <td class='legendLabel'> " + statistic.DepartmentCode + @": <span>" + statistic.SetCount + @"</span></td></tr>");
                }
                filter.AppendLine("</tbody>");
                filter.AppendLine("</table>");
                divTotal.InnerHtml = filter.ToString();
            }
            lblTotalApplication.Value = totalApplication = totalStagesCount.Sum(a => a.SetCount).ToString();
            lblTotal.Value = string.Join(",", totalStagesCount.Select(x => x.SetCount).ToArray());
            lblTotalColors.Value = totalColors;
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                divResult.InnerHtml = @"<div class='alert alert-success fade in'>
                           <button data-dismiss='alert' class='close close-sm' type='button'>
                           <i class='fa fa-times'></i></button>
                           <strong>Done!</strong> Found the statistics between " + startDate + " - " + endDate + @"
                           </div>";
                divResult.Visible = true;
                BindHeaders();
            }
            // end
        }

        public string GetColor(int departmentId)
        {
            string color = string.Empty;
            switch (departmentId)
            {
                case 1:
                    color = "003f5c";
                    break;
                case 2:
                    color = "2f4b7c";
                    break;
                case 3:
                    color = "665191";
                    break;
                case 4:
                    color = "a05195";
                    break;
                case 5:
                    color = "d45087";
                    break;
                case 6:
                    color = "f95d6a";
                    break;
                case 7:
                    color = "ff7c43";
                    break;
                case 8:
                    color = "ffa600";
                    break;
                case 9:
                    color = "229954";
                    break;
            }
            return color;
        }


        public static double GetFiles(string root, string searchPattern)
        {
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.Now.AddDays(-40);
            double totalSize = 0;
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    string[] exceptions = new string[] { "System Volume Information" };
                    next = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).Where(d => !d.StartsWith("D:\\System Volume Information")).ToArray();
                    totalSize += Directory.GetFiles(path, searchPattern).Sum(a => a.Length);
                }
                catch (Exception ex)
                {
                }
                if (next != null && next.Length != 0)
                {
                    foreach (var file in next)
                    {
                        FileInfo fi = new FileInfo(file);
                        bool status = false;

                        if (fi.LastWriteTime <= dt2)
                            status = true;
                        if (fi.LastWriteTime <= dt1)
                            status = true;

                        if (fi.LastWriteTime >= dt2 && fi.LastWriteTime <= dt1)
                            totalSize += fi.Length;
                    }
                    return totalSize;
                }

                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next)
                    {
                        pending.Push(subdir);
                        FileInfo fi = new FileInfo(subdir);
                        if (fi.LastWriteTime <= dt1 && fi.LastWriteTime >= dt2)
                            totalSize += fi.Length;
                    }
                    return totalSize;
                }
                catch { }

                //string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                //double len = filesSize;
                //int order = 0;
                //while (len >= 1024 && order < sizes.Length - 1)
                //{
                //    order++;
                //    len = len / 1024;
                //}

                //// Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                //// show a single decimal place, and no space.
                //string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            }
            return totalSize;
        }


        public double GetFilesSize(string root, string searchPattern, string startDate, string endDate)
        {
            try
            {
                DateTime dt1 = Convert.ToDateTime(startDate);
                DateTime dt2 = Convert.ToDateTime(endDate);
                double totalSize = 0;
                if (!string.IsNullOrEmpty(root))
                {
                    try
                    {
                        foreach (var subdir in Directory.GetDirectories(path))
                        {
                            DirectoryInfo info = new DirectoryInfo(subdir);
                            totalSize += info.EnumerateFiles(searchPattern, SearchOption.AllDirectories).Where(a => a.LastWriteTime >= dt1 && a.LastWriteTime <= dt2).Sum(a => a.Length);
                        }
                        return totalSize;
                    }
                    catch { }
                }
                return totalSize;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void BindHeaders()
        {
            lblSANDrive.InnerText = "SAN DRIVE MEMORY";
            lblScan.InnerText = "SCAN";
            lblTransfer.InnerText ="TRANSFER";
            lblTotalApp.InnerText = "APPLICATIONS";
            lblMonthApp.InnerText = "Application Chart";
        }


        public void Querys()
        {
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                barChatQuery = " SELECT CAST(created_date AS DATE) AS CreatedOn,department_id, COUNT(department_id) as count,sum(batch_count) as SetCount " +
                                 " FROM batches where status = 1 and stage_id =" + (int)BatchesStages.Document + " and CAST(created_date AS DATE) between " +
                                 " '" + startDate + @"' and '" + endDate + @"' ";
                if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                    barChatQuery += " and branch_id=" + BranchId.SelectedItem.Value;
                barChatQuery += " GROUP BY CAST(created_date AS DATE),department_id order by 1";

                pieChatQuery = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,code as DepartmentCode  FROM batches a inner join departments b on a.department_id=b.ID  " +
                         "  where a.status = 1 and stage_id =" + (int)BatchesStages.Document + " and a.updated_date between'" + startDate + @"'  AND '" + endDate + @"' ";
                if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                    pieChatQuery += " and a.branch_id=" + BranchId.SelectedItem.Value;
                pieChatQuery += "  GROUP BY department_id,code order by 1";

                //totalDepartmentsQuery = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,b.code as DepartmentCode FROM " +
                //        "  batches a inner join departments b on a.department_id = b.ID  where a.status = 1 and stage_id =" + (int)BatchesStages.Document + "" +
                //        "  and a.updated_date between '" + startDate + @"' and '" + endDate + @"' ";
                //if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                //    totalDepartmentsQuery += " and a.branch_id=" + BranchId.SelectedItem.Value;
                //totalDepartmentsQuery += "  GROUP BY department_id,code order by 1";
            }
            else
            {
                // last 5 days data for bar chat
                barChatQuery = " SELECT CAST(created_date AS DATE) AS CreatedOn,department_id, COUNT(department_id) as count,sum(batch_count) as SetCount " +
                               " FROM batches where status = 1 and stage_id =" + (int)BatchesStages.Document + " and CAST(created_date AS DATE) between " +
                               " (select top 1 * from(select top 5 CAST(created_date AS DATE) as chartDate from batches where status = 1 and stage_id = " + (int)BatchesStages.Document + " group by CAST(created_date AS DATE) order by 1 desc) as tab order by 1) and " +
                               " (select top 1 * from(select top 5 CAST(created_date AS DATE) as chartDate from batches where status = 1 and stage_id = " + (int)BatchesStages.Document + " group by CAST(created_date AS DATE) order by 1 desc) as tab order by 1 desc) ";
                if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                    barChatQuery += " and branch_id=" + BranchId.SelectedItem.Value;
                barChatQuery += " GROUP BY CAST(created_date AS DATE),department_id order by 1 ";

                // last month for pie chat
                pieChatQuery = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,code as DepartmentCode  FROM batches a inner join departments b on a.department_id=b.ID  " +
                            " where a.status = 1 and stage_id = " + (int)BatchesStages.Document + " and " +
                            " DATEPART(m, a.created_date) = DATEPART(m, DATEADD(m, -1, getdate()))  AND " +
                            " DATEPART(yyyy, a.created_date) = DATEPART(yyyy, DATEADD(m, -1, getdate())) ";
                if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                    pieChatQuery += " and a.branch_id=" + BranchId.SelectedItem.Value;
                pieChatQuery += " GROUP BY department_id,code order by 1";

            }
            // All departments counts
            totalDepartmentsQuery = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,b.code as DepartmentCode FROM  " +
             " batches a inner join departments b on a.department_id = b.ID  where a.status =1 and stage_id =" + (int)BatchesStages.Document + "";
            if (!string.IsNullOrEmpty(BranchId.SelectedItem.Value) && BranchId.SelectedItem.Value != "0")
                totalDepartmentsQuery += " and a.branch_id=" + BranchId.SelectedItem.Value;
            totalDepartmentsQuery += " GROUP BY department_id,code order by 1";
        }
    }
}