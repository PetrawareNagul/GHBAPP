using Silverlake.Service;
using Silverlake.Service.IService;
using Silverlake.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silverlake.Web.Simulation
{
    public partial class AddBatch : System.Web.UI.Page
    {
        private static readonly Lazy<IBatchService> lazyBatchServiceObj = new Lazy<IBatchService>(() => new BatchService());

        public static IBatchService IBatchService { get { return lazyBatchServiceObj.Value; } }

        private static readonly Lazy<IDepartmentService> lazyDepartmentServiceObj = new Lazy<IDepartmentService>(() => new DepartmentService());

        public static IDepartmentService IDepartmentService { get { return lazyDepartmentServiceObj.Value; } }

        private static readonly Lazy<IBranchService> lazyBranchServiceObj = new Lazy<IBranchService>(() => new BranchService());

        public static IBranchService IBranchService { get { return lazyBranchServiceObj.Value; } }

        private static readonly Lazy<IStageService> lazyStageServiceObj = new Lazy<IStageService>(() => new StageService());

        public static IStageService IStageService { get { return lazyStageServiceObj.Value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentDateString = DateTime.Now.ToString("MM/dd/yyyy");
            CreatedDate.Value = currentDateString;
            UpdatedDate.Value = currentDateString;

            List<Branch> branches = IBranchService.GetData(0, 0, false);
            List<ListItem> branchesList = new List<ListItem>();
            branches.ForEach(obj => {
                branchesList.Add(new ListItem()
                {
                    Text = obj.Code + " - " + obj.Name,
                    Value = obj.Id.ToString()
                });
            });

            BranchId.DataSource = branchesList;
            BranchId.DataTextField = "Text";
            BranchId.DataValueField = "Value";
            BranchId.DataBind();

            List<Department> departments = IDepartmentService.GetData(0, 0, false);
            List<ListItem> departmentsList = new List<ListItem>();
            departments.ForEach(obj => {
                departmentsList.Add(new ListItem()
                {
                    Text = obj.Code + " - " + obj.Name,
                    Value = obj.Id.ToString()
                });
            });

            DepartmentId.DataSource = departmentsList;
            DepartmentId.DataTextField = "Text";
            DepartmentId.DataValueField = "Value";
            DepartmentId.DataBind();

            List<Stage> stages = IStageService.GetData(0, 0, false);
            List<ListItem> stagesList = new List<ListItem>();
            stages.ForEach(obj=> {
                stagesList.Add(new ListItem() {
                    Text = obj.Code + " - " + obj.Name,
                    Value = obj.Id.ToString()
                });
            });

            StageId.DataSource = stagesList;
            StageId.DataTextField = "Text";
            StageId.DataValueField = "Value";
            StageId.DataBind();
            Status.Value = "1";
            string idString = Request.QueryString["id"];
            if (idString != null && idString != "")
            {
                int id = Convert.ToInt32(idString);
                Batch obj = IBatchService.GetSingle(id);
                Id.Value = obj.Id.ToString();
                BranchId.Value = obj.BranchId.ToString();
                StageId.Value = obj.StageId.ToString();
                BatchKey.Value = obj.BatchKey;
                BatchNo.Value = obj.BatchNo;
                BatchCount.Value = obj.BatchCount.Value.ToString();
                BatchStatus.Value = obj.BatchStatus.ToString();
                Status.Value = obj.Status.ToString();
            }
        }
    }
}