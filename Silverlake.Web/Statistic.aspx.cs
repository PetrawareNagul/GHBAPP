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
        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        public string totalApplication = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            DirectoryInfo DirInfo = new DirectoryInfo(@"D:\");
            //var files = DirInfo.GetFiles("*",SearchOption.AllDirectories).Where(a => a.LastAccessTime < DateTime.Now.AddDays(-20) && a.LastAccessTime > DateTime.Now).Sum(a =>a.Length);
            //long totalSize = DirInfo.EnumerateFiles("*.*",SearchOption.AllDirectories).Where(a =>a.LastWriteTime >= DateTime.Now.AddDays(-40) && a.LastWriteTime <= DateTime.Now).Sum(file => file.Length);
            double filesSize = GetFiles(@"D:\", "*.*");

            var data = DirInfo.GetFiles("*").ToList();


            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = filesSize;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);


            string path = ConfigurationManager.AppSettings["MimzyCaptureOuputFiles"].ToString();
            DriveInfo dDrive = new DriveInfo(path);
            if (dDrive.IsReady)
            {
                lbltotalSize1.Text = (dDrive.TotalSize / 1024 / 1024 / 1024).ToString();
                lblfreeSpace.Value = ((dDrive.TotalSize / 1024 / 1024 / 1024) - (dDrive.TotalFreeSpace / 1024 / 1024 / 1024)).ToString();
            }


            // last 5 days data for bar chat
            string qurey = " SELECT CAST(created_date AS DATE) AS CreatedOn,department_id, COUNT(department_id) as count,sum(batch_count) as SetCount " +
                           " FROM batches where status = 1 and stage_id = 6 and CAST(created_date AS DATE) between " +
                           " (select top 1 * from(select top 5 CAST(created_date AS DATE) as chartDate from batches where status = 1 and stage_id = 6 group by CAST(created_date AS DATE) order by 1 desc) as tab order by 1) and " +
                           " (select top 1 * from(select top 5 CAST(created_date AS DATE) as chartDate from batches where status = 1 and stage_id = 6 group by CAST(created_date AS DATE) order by 1 desc) as tab order by 1 desc) " +
                           " GROUP BY CAST(created_date AS DATE),department_id order by 1 ";

            List<StatisticModel> last5DaysCount = IBatchService.GetDaysCountbyDepartment(qurey, 0, 0, true);


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
                    string depCount = string.Empty;
                    if (last5DaysCount.Count(a => a.CreatedOn == todaysDate && a.DepartmentId == depid) != 0)
                    {
                        depCount = last5DaysCount.FirstOrDefault(a => a.CreatedOn == todaysDate && a.DepartmentId == depid).SetCount.ToString();
                        if (!string.IsNullOrEmpty(todaySetCount))
                            todaySetCount += "," + depCount;
                        else
                            todaySetCount = depCount;
                        if (!string.IsNullOrEmpty(todayColor))
                            todayColor += "," + GetColor(depid);
                        else
                            todayColor = GetColor(depid);
                    }
                    else
                        depCount = "0";
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
            string monthQurey = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,code as DepartmentCode  FROM batches a inner join departments b on a.department_id=b.ID  " +
                                " where a.status = 1 and stage_id = 6 and " +
                                " DATEPART(m, a.created_date) = DATEPART(m, DATEADD(m, -1, getdate()))  AND " +
                                " DATEPART(yyyy, a.created_date) = DATEPART(yyyy, DATEADD(m, -1, getdate())) " +
                                " GROUP BY department_id,code order by 1";

            string monthColors = "";
            List<StatisticModel> monthCount = IBatchService.GetTotalCountbyDepartment(monthQurey, 0, 0, true);
            if (monthCount != null && monthCount.Count != 0)
            {
                StringBuilder filter = new StringBuilder();
                filter.AppendLine("<table style='position: absolute; top: 5px; right: 5px; font - size:smaller; color:#545454'>");
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
            string totalQurey = "SELECT department_id, COUNT(department_id) as count,sum(batch_count) as SetCount,b.code as DepartmentCode FROM  " +
                                " batches a inner join departments b on a.department_id = b.ID  where a.status =1 and stage_id =" + (int)BatchesStages.Document + "" +
                                " GROUP BY department_id,code order by 1";

            string totalColors = "";
            List<StatisticModel> totalStagesCount = IBatchService.GetTotalCountbyDepartment(totalQurey, 0, 0, true);
            if (totalStagesCount != null && totalStagesCount.Count != 0)
            {
                StringBuilder filter = new StringBuilder();
                filter.AppendLine("<table style='position: absolute; top: 5px; right: 5px; font - size:smaller; color:#545454'>");
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
            // end
        }

        public string GetColor(int departmentId)
        {
            string color = string.Empty;
            switch (departmentId)
            {
                case 1:
                    color = "E1EB5A";
                    break;
                case 2:
                    color = "EB6D5A";
                    break;
                case 3:
                    color = "9DEB5A";
                    break;
                case 4:
                    color = "98F5D3";
                    break;
                case 5:
                    color = "A786F2";
                    break;
                case 6:
                    color = "95A5A6";
                    break;
                case 7:
                    color = "E75EC0";
                    break;
                case 8:
                    color = "5D6D7E";
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
                catch(Exception ex)
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

                        if(fi.LastWriteTime >= dt2 && fi.LastWriteTime <= dt1)
                        totalSize +=  fi.Length;
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
            }
            return totalSize;
        }
    }
}