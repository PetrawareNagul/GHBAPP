<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sets.aspx.cs" Inherits="Silverlake.Web.Sets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Content/js/bootstrap-datepicker/css/datepicker.css" />
</head>
<body>
    <form id="setsForm" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="content">
                        <div class="page-common-section">
                            <div class="row">

                                <div class="col-md-6">
                                <div class="form-group">
                                                <div class="col-md-12">
                                                    <div class="input-group input-large" data-date="" data-date-format="mm/dd/yyyy">
                                                        <input type="text" class="form-control dpd1" id="FromDate" runat="server" name="FromDate" />
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <span class="input-group-addon">To</span>
                                                        <input type="text" class="form-control dpd2" name="ToDate" id="ToDate" runat="server" />
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                    <span class="help-block">Select date range</span>
                                                </div>
                                            </div>
                                    </div>
                                <div class="col-md-6">
                                    <div class="search-section">
                                        <button type="button" class="button-common-label action-button" data-action="Search"><i class="fa fa-search"></i></button>
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

    <script src="Content/js/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
    <script>
        $ui(document).ready(function () {

            $ui('.dpd1, .dpd2').datepicker({
                format: 'mm/dd/yyyy',
                pickTime: false,
                autoclose: true
            });

            //$ui('.dpd1').datepicker({
            //    format: 'mm/dd/yyyy',
            //    pickTime: false,
            //    autoclose: true
            //});
        });
    </script>

    <script>
        $(document).ready(function () {
            $('#setsForm .popovers').popover();

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
