<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountEnquiry.aspx.cs" Inherits="Silverlake.Web.AccountEnquiry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Content/js/bootstrap-datepicker/css/datepicker.css" />
</head>
<body>
    <form id="accountEnquiryForm" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="content">
                        <div class="page-common-section">
                            <div class="row">

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DepartmentId" runat="server" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DocType" runat="server" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="search-section">
                                        <button type="button" id="btnSearch" class="button-common-label action-button" data-action="Search"><i class="fa fa-search"></i></button>
                                        <input type="text" id="Search" name="Search" runat="server" class="form-control" placeholder="Search here" />
                                        <input type="hidden" id="IsNewSearch" name="IsNewSearch" value="0" runat="server" clientidmode="static" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <small><strong>Note:</strong> <span>You can search here with AA Number, Account Number, Project Code, Welfare Code</span></small>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="page-container-section">
                            <div class="row">
                                <div class="col-md-12">

                                    <div id="divResult" runat="server" class="alert alert-success fade in"></div>

                                    <div id="divError" runat="server" class="alert alert-block alert-danger fade in"></div>


                                    <table class="display responsive nowrap table table-bordered dataTable langDirList" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Branch</th>
                                                <th>Department</th>
                                                <th>Batch No</th>
                                                <th>Code No</th>
                                                <th>PDF</th>
                                                <th>Stage</th>
                                                <th>Remarks</th>
                                                <th>Updated Date</th>
                                            </tr>
                                        </thead>
                                        <tbody id="setsTbody" runat="server">
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

    <%--<script src="Content/js/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>--%>

    <script>
        $(document).ready(function () {

            //$('#divResult').hide();
            //$('#divError').hide();

            $('#DepartmentId').change(function () {

                var departmentId = $('#DepartmentId').val();
                var dataString = "{ 'Id':'" + departmentId + "' }";

                $.ajax({
                    type: "POST",
                    url: "AccountEnquiry.aspx/BindDocType",
                    data: dataString,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#DocType').html('');

                        $.each(data.d, function (index, value) {
                            $('#DocType').append('<option value="' + value.DocType + '">' + value.DocType + '</option>');
                        });
                    },
                    error: function (result) {
                    }
                });


            });

            $('#btnSearch').click(function () {
                var url = "AccountEnquiry.aspx?";
                $.post(url, function (content) {
                    var selectedTab = $('#tt').tabs('getSelected');
                    $('#tt').tabs('update', {
                        tab: selectedTab,
                        options: {
                            content: content
                        }
                    });
                }, 'html');
            });

        });




    </script>
</body>
</html>
