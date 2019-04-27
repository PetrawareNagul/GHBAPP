<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchList.aspx.cs" Inherits="Silverlake.Web.BranchList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="branchListForm" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="content">
                        <div class="page-common-section">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="search-section">
                                        <button type="button" class="button-common-label action-button" data-action="Search"><i class="fa fa-search"></i></button>
                                        <input type="text" id="Search" name="Search" runat="server" class="form-control" placeholder="Search here" />
                                        <input type="hidden" id="IsNewSearch" name="IsNewSearch" value="0" runat="server" clientidmode="static" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="action-section">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <button data-message="Successfully deactivated" data-status="Inactive" data-original-title="Deactivate" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Deactivate" data-isconfirm="1"><i class="fa fa-ban"></i></button>
                                                <button data-message="Successfully activated" data-status="Active" data-original-title="Activate" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Activate" data-isconfirm="0"><i class="fa fa-check"></i></button>
                                                <span class="common-divider pull-right"></span>
                                                <button data-original-title="Add" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Add" data-url="AddBranch.aspx" data-title="Add Branch"><i class="fa fa-plus"></i></button>
                                                <button data-original-title="Edit" data-trigger="hover" data-placement="bottom" data-content="Select one record" type="button" class="popovers btn action-button pull-right" data-action="Edit" data-url="AddBranch.aspx" data-title="Edit Branch"><i class="fa fa-edit"></i></button>
                                                <button data-original-title="Update Departments" data-trigger="hover" data-placement="bottom" data-content="Link departments to branch" type="button" class="popovers btn action-button pull-right" data-action="UpdateDepartments" data-title="Update departments"><i class="fa fa-link"></i></button>
                                                <%--<button type="button" class="btn action-button pull-right"><i class="fa fa-edit"></i></button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can search here with Name/ Code</span></small>
                                </div>
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can Edit/ Add/ Activate/ Deactivate on Branches</span></small>
                                </div>
                            </div>
                        </div>
                        <div class="page-container-section">
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="display responsive nowrap table table-bordered dataTable langDirList" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th>No.</th>
                                                <th>Branch Code<br />
                                                    Name</th>
                                                <th>Departments</th>
                                                <th>Created date</th>
                                                <th>Updated date</th>
                                            </tr>
                                        </thead>
                                        <tbody id="branchesTBody" runat="server">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnNumberPerPage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTotalRecordsCount" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentPageNo" runat="server" ClientIDMode="Static" />
    </form>
    <script src="Content/mycj/common-scripts.js"></script>
    <script>
        $(document).ready(function () {
            $('#branchesTBody label.icheck .checkRow.selectAll').on('ifChecked', function (event) {
                var that = $(event.target);
                $(that).parents('td').find('.checkRow:not(.selectAll)').each(function () {
                    var checkbox = $(this);
                    $(checkbox).iCheck('check');
                });
            });
            $('#branchesTBody label.icheck .checkRow.selectAll').on('ifUnchecked', function (event) {
                var that = $(event.target);
                $(that).parents('td').find('.checkRow:not(.selectAll)').each(function () {
                    var checkbox = $(this);
                    $(checkbox).iCheck('uncheck');
                });
            });

            $('#branchesTBody label.icheck .checkRow:not(.selectAll)').on('ifChecked', function (event) {
                var that = $(event.target);
                var isChecked = $(that).parents('td').find('.checkRow:not(.selectAll)').map(function () {
                    var checkbox = $(this);
                    return $(checkbox).is(':checked');
                });
                var uniqueChecked = $.unique(isChecked);
                if (uniqueChecked.length == 1) {
                    var isChecked = uniqueChecked[0];
                    var checkbox = $(that).parents('td').find('.checkRow.selectAll');
                    if(isChecked)
                        $(checkbox).iCheck('check');
                    else
                        $(checkbox).iCheck('uncheck');
                }
                else {

                }
            });
            $('#branchesTBody label.icheck .checkRow:not(.selectAll)').on('ifUnchecked', function (event) {
                var that = $(event.target);
                var isChecked = $(that).parents('td').find('.checkRow:not(.selectAll)').map(function () {
                    var checkbox = $(this);
                    return $(checkbox).is(':checked');
                });
                var uniqueChecked = $.unique(isChecked);
                console.log(uniqueChecked);
                if (uniqueChecked.length != 1) {
                    var checkbox = $(that).parents('td').find('.checkRow.selectAll');
                    $(checkbox).prop('checked', false).iCheck('update');
                }
                else {

                }
            });
            //$('.flat-blue.single-row').click(function () {
            //    var that = $(this);
            //    setTimeout(function () {
            //        var checked = $(that).find('.selectAll').is(':checked');
            //        if (checked) {
            //            $(that).parents('td').find('.checkRow:not(.selectAll)').each(function () {
            //                var checkbox = $(this);
            //                $(checkbox).iCheck('check');
            //            });
            //        }
            //        else {
            //            $(that).parents('td').find('.checkRow:not(.selectAll)').each(function () {
            //                var checkbox = $(this);
            //                $(checkbox).iCheck('uncheck');
            //            });
            //        }
            //    }, 100);
            //});
        });
    </script>
</body>
</html>
