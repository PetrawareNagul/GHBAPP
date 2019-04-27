<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Silverlake.Web.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Content/js/bootstrap-datepicker/css/datepicker.css" />
</head>
<body>
    <form id="dashboardForm" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <div class="content">
                        <div class="page-common-section">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="search-section">
                                        <button type="button" class="button-common-label action-button" data-action="Search"><i class="fa fa-search"></i></button>
                                        <div class="search-features">
                                            <input type="text" id="Search" name="Search" runat="server" class="form-control" placeholder="Search here" />
                                            <select class="form-control" id="BranchId" runat="server" clientidmode="static">
                                                <option value="">Select</option>
                                            </select>
                                            <input type="hidden" id="IsNewSearch" name="IsNewSearch" value="0" runat="server" clientidmode="static" />
                                            <div class="search-message" id="searchMessage" runat="server"></div>
                                            <div class="input-group input-large" >
                                                  <input type="text" class="form-control form-control-inline input-medium default-date-picker" id="FromDate" runat="server" name="FromDate" placeholder="From Date" />
                                                 <%--   <span class="input-group-addon">
                                                        <i class="fa fa-calendar"></i>
                                                    </span>--%>

                                                <span class="input-group-addon">To</span>
                                                <input type="text" class="form-control dpd2" name="ToDate" id="ToDate" runat="server" placeholder="To Date" />
                                              <%--    <span class="input-group-addon">
                                                        <i class="fa fa-calendar"></i>
                                                    </span>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="action-section">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <button data-original-title="Sort Defaults" data-trigger="hover" data-placement="left" type="button" class="popovers btn btn-default action-button pull-right" data-action="ClearSort" data-isconfirm="0"><i class="fa fa-eraser"></i></button>
                                                <button data-original-title="Sort By Active" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="SortActive" data-isconfirm="0" id="sortActive" runat="server" clientidmode="static"><i class="fa fa-sort-numeric-desc"></i></button>
                                                <button data-original-title="Sort By Branch" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="SortBranch" data-isconfirm="0" id="sortBranch" runat="server" clientidmode="static"><i class="fa fa-sort-alpha-asc"></i></button>
                                                <%--<button type="button" class="btn action-button pull-right"><i class="fa fa-edit"></i></button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <small class="fs-11"><strong>Note:</strong> <span>Search with AA Number, Account Number, Project Code, Welfare Code, Date range</span></small>
                                </div>
                                <div class="col-md-4">
                                    <small class="fs-11"><strong>Note:</strong> <span>You can Sort by Active/ Branch/ Set sort defaults</span></small>
                                </div>
                            </div>
                        </div>
                        <div class="page-container-section">
                            <!--mini statistics start-->
                            <div id="sveimList" runat="server">
                                <div class="row">
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="mini-stat clearfix">
                                                    <span class="mini-stat-icon orange"><i class="fa fa-print"></i></span>
                                                    <div class="mini-stat-info">
                                                        <span id="scansCount" runat="server" clientidmode="static">0</span>
                                                        Scan
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="mini-stat clearfix">
                                                    <span class="mini-stat-icon tar"><i class="fa fa-check-square-o"></i></span>
                                                    <div class="mini-stat-info">
                                                        <span id="verificationsCount" runat="server" clientidmode="static">0</span>
                                                        Verification
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="mini-stat clearfix">
                                                    <span class="mini-stat-icon pink"><i class="fa fa-external-link"></i></span>
                                                    <div class="mini-stat-info">
                                                        <span id="exportsCount" runat="server" clientidmode="static">0</span>
                                                        Export
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="mini-stat clearfix">
                                                    <span class="mini-stat-icon green"><i class="fa fa-puzzle-piece"></i></span>
                                                    <div class="mini-stat-info">
                                                        <span id="integrationsCount" runat="server" clientidmode="static">0</span>
                                                        Integration
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="mini-stat clearfix">
                                            <span class="mini-stat-icon yellow-b"><i class="fa fa-hdd-o"></i></span>
                                            <div class="mini-stat-info">
                                                <span id="documentsCount" runat="server" clientidmode="static">100</span>
                                                Documents
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--mini statistics end-->

                            <div class="row">
                                <div class="col-md-12">
                                    <div id="divXmlString" runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="IsNewSort" runat="server" ClientIDMode="Static" Value="0" />
        <asp:HiddenField ID="SortBy" runat="server" ClientIDMode="Static" Value="SortActive" />
        <asp:HiddenField ID="SortByAsc" runat="server" ClientIDMode="Static" Value="0" />
    </form>
    <script src="Content/mycj/common-scripts.js"></script>

    <script src="Content/js/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
    <script>
        $ui(document).ready(function () {
            $ui('#FromDate, #ToDate').datepicker({
                format: 'mm/dd/yyyy',
                pickTime: false,
                autoclose: true
            });

            //$ui('#ToDate').datepicker({
            //    format: 'mm/dd/yyyy',
            //    pickTime: false,
            //    autoclose: true
            //});
        });
    </script>


    <script>
        $(document).ready(function () {

            $.ajaxSetup({ cache: false });
            var isAjaxActive = 0;
            var ajaxRequest;
            var callCount = 0;
            var refreshDashboard = function () {
                var continueFunction = $('#continueFunction').val();
                if (continueFunction == "1") {
                    if (isAjaxActive == 0) {
                        $('#IsNewSearch').val('1');
                        var data = $('#dashboardForm').serializeArray();
                        data = data.filter(function (item) {
                            return item.name.indexOf('_') === -1;
                        });
                        //console.log(data);
                        var element = {};
                        var result = data.map(function (e) {
                            element[e.name] = e.value;
                            return element;
                        });
                        var url = "Dashboard.aspx/GetNewData?" + $.param(data);
                        var tab = $('#tt').tabs('getSelected');
                        var index = $('#tt').tabs('getTabIndex', tab);
                        if (index == 0) {
                            callCount++;
                            if (callCount == 100) {
                                location.reload(true);
                            }
                            ajaxRequest = $.ajax({
                                type: "POST",
                                url: "Dashboard.aspx/GetNewData",
                                data: JSON.stringify({ "dashboardDTO": element }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                cache: false,
                                success: function (data) {
                                    $('[data-fromdate]').attr('data-fromdate', $('#FromDate').val());
                                    $('[data-todate]').attr('data-todate', $('#ToDate').val());
                                    var dashboardStatisticsTotal = data.d.dashboardStatisticsTotal;
                                    $('.all .mini-stat-info').each(function (idx, obj) {
                                        var count = 0;
                                        if (idx == 0)
                                            count = dashboardStatisticsTotal.ScanCount;
                                        else if (idx == 1)
                                            count = dashboardStatisticsTotal.IndexCount;
                                        else if (idx == 2)
                                            count = dashboardStatisticsTotal.ExportCount;
                                        else if (idx == 3)
                                            count = dashboardStatisticsTotal.IntegrateCount;
                                        else if (idx == 4)
                                            count = dashboardStatisticsTotal.ReleaseCount;
                                        else if (idx == 5)
                                            count = dashboardStatisticsTotal.DocumentCount;
                                        $(this).find('span:first').html(count);
                                    });

                                    var dashboardStatisticsBranch = data.d.dashboardStatisticsBranch;
                                    $.each(dashboardStatisticsBranch, function (idx1, dash) {
                                        $('.' + dash.BranchCode + ' .mini-stat-info').each(function (idx, obj) {
                                            var count = 0;
                                            if (idx == 0)
                                                count = dash.ScanCount;
                                            else if (idx == 1)
                                                count = dash.IndexCount;
                                            else if (idx == 2)
                                                count = dash.ExportCount;
                                            else if (idx == 3)
                                                count = dash.IntegrateCount;
                                            else if (idx == 4)
                                                count = dash.ReleaseCount;
                                            else if (idx == 5)
                                                count = dash.DocumentCount;
                                            $(this).find('span:first a').html(count);
                                        });
                                    });

                                    var dashboardStatisticsDepartment = data.d.dashboardStatisticsDepartment;
                                    $.each(dashboardStatisticsDepartment, function (idx1, dash) {
                                        var departmentExists = $('.' + dash.BranchCode + '.' + dash.DepartmentCode + ' .mini-stat-info').length;
                                        //if (departmentExists == 0) {
                                        //    var data = $('#dashboardForm').serializeArray();
                                        //    data = data.filter(function (item) {
                                        //        return item.name.indexOf('_') === -1;
                                        //    });
                                        //    var url = "Dashboard.aspx?" + $.param(data);
                                        //    $.post(url, function (content) {
                                        //        var selectedTab = $('#tt').tabs('getSelected');
                                        //        $('#tt').tabs('update', {
                                        //            tab: selectedTab,
                                        //            options: {
                                        //                content: content
                                        //            }
                                        //        });
                                        //    }, 'html');
                                        //    $('#continueFunction').val('1');
                                        //}
                                        $('.' + dash.BranchCode + '.' + dash.DepartmentCode + ' .mini-stat-info').each(function (idx, obj) {
                                            var count = 0;
                                            if (idx == 0)
                                                count = dash.ScanCount;
                                            else if (idx == 1)
                                                count = dash.IndexCount;
                                            else if (idx == 2)
                                                count = dash.ExportCount;
                                            else if (idx == 3)
                                                count = dash.IntegrateCount;
                                            else if (idx == 4)
                                                count = dash.ReleaseCount;
                                            else if (idx == 5)
                                                count = dash.DocumentCount;
                                            $(this).find('span:first a').html(count);
                                        });
                                    });

                                    isAjaxActive = 0;
                                    //Refresh
                                    setTimeout(refreshDashboard, 5000);
                                },
                                error: function (result) {
                                    console.warn(result.statusText);
                                    setTimeout(refreshDashboard, 5000);
                                }
                            });
                            //ajaxrequest = $.get(url, function (content) {
                            //    var data = $(content).find('#sveimlist').html();
                            //    $('#sveimlist').html(data);
                            //    var selectedlanguage = $('#languagecode').text().tolowercase();
                            //    $('.tabs .data-url').first().removeclass(selectedlanguage);
                            //    translatelanguage(selectedlanguage);
                            //    isajaxactive = 0;
                            //    $.getscript("content/mycj/common-scripts.js", function( data, textstatus, jqxhr ) {
                            //      console.log( data ); // data returned
                            //      console.log( textstatus ); // success
                            //      console.log( jqxhr.status ); // 200
                            //      console.log( "load was performed." );
                            //    });
                            //    refresh
                            //    settimeout(refreshdashboard, 5000);
                            //}, 'html');
                        }
                        else {
                            setTimeout(refreshDashboard, 5000);
                            callCount = 0;
                        }
                    }
                    else {
                        isAjaxActive = 1;
                        setTimeout(refreshDashboard, 5000);
                    }
                }
                else {
                    setTimeout(refreshDashboard, 5000);
                }
            }
            setTimeout(refreshDashboard, 5000);
        });
    </script>
</body>
</html>
