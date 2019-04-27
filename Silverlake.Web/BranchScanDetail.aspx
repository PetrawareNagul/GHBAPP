<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchScanDetail.aspx.cs" Inherits="Silverlake.Web.BranchScanDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="branchScanDetailForm" runat="server">
        <h5>Branch: <strong id="branchName" runat="server"></strong></h5>
        <div class="row">
            <div class="col-md-12" id="panelBatches" runat="server">
                <!--pagination start-->
                <section class="panel">
                    <header class="panel-heading">
                        Initail Collaps bar
                            <span class="tools pull-right">
                                <a href="javascript:;" class="fa fa-chevron-up"></a>
                            </span>
                    </header>
                    <div class="panel-body" style="display: none;">
                        contents goes here
                    </div>
                </section>
                <!--pagination end-->
            </div>
            <div class="text-center paginationDiv"></div>
            <div class="text-center infoDiv"></div>
        </div>
        <asp:HiddenField ID="hdnNumberPerPage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTotalRecordsCount" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentPageNo" runat="server" ClientIDMode="Static" />
    </form>
    <script src="Content/mycj/common-scripts.js"></script>
    <script>
        $(document).ready(function () {
            $('.panel .tools .fa.panel-oc').click(function () {
                var el = $(this).parents(".panel").children(".panel-body");
                if ($(this).hasClass("fa-chevron-down")) {
                    $(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
                    el.slideUp(200);
                } else {
                    $(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
                    el.slideDown(200);
                }
            });
            $('.panel:first .tools .fa:first').click();
            $('#branchScanDetailForm .popovers').popover();

            $('.view_batch_log').click(function () {
                var divLog = $(this).next();
                $('.div_batch_log').each(function (idx, obj) {
                    if (!$(obj).hasClass('hide'))
                        $(obj).addClass('hide');
                });
                $(divLog).removeClass('hide');
            });

            $('.draggableDiv').draggable();

            $('.log_close').click(function () {
                $(this).parent('.div_batch_log').addClass('hide');
            });

        });
    </script>
</body>
</html>
