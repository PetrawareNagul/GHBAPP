<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Silverlake.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Silverlake</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE-edge,chrome=1" />
    <meta name="description" content="" />
    <link rel="shortcut icon" href="/Content/images/favicon.png" />

    <link href="Content/js/jquery-ui/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
    <link href="Content/js/responsive-datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="Content/js/responsive-datatables/dataTables.responsive.min.css" rel="stylesheet" />
    <link href="Content/dynamic-tabs/easyui.css" rel="stylesheet" />
    <link href="Content/dynamic-tabs/icon.css" rel="stylesheet" />

    <link href="Content/js/googlecss.css" rel="stylesheet" />
    <%--<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet" />--%>
    <link href="Content/js/bootstrap-datepicker/css/datepicker.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Content/js/bootstrap-fileupload/bootstrap-fileupload.css" />
    <link href="Content/js/bootstrap-colorpicker/css/colorpicker.css" rel="stylesheet" />
    <!--icheck-->
    <link href="Content/js/iCheck/skins/square/square.css" rel="stylesheet" />
    <link href="Content/js/iCheck/skins/flat/green.css" rel="stylesheet" />
    <link href="Content/js/iCheck/skins/flat/blue.css" rel="stylesheet" />
    <link href="Content/bs3/css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="Content/css/bootstrap-reset.css" rel="stylesheet" />--%>
    <link href="Content/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/mycj/mystyles.css" rel="stylesheet" />
    <link href="Content/mycj/My-StyleSheet.css" rel="stylesheet" />
    <link href="Content/mycj/ghb-styles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="commonModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="commonModalTitle"></h4>
                    </div>
                    <div class="modal-body" id="commonModalContent">
                    </div>
                    <div class="modal-footer">
                        <button data-dismiss="modal" class="btn btn-default" type="button">Cancel</button>
                        <button class="btn btn-success" type="button" id="btnCommonModalSubmit">Save</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="loadingDiv hide">
            <div class="lds-ellipsis">
                <div></div>
                <div></div>
                <div></div>
                <div></div>
            </div>
        </div>
        <section id="container">
            <aside>
                <div id="sidebar" class="nav-collapse">
                    <div class="leftside-navigation">
                        <div class="logoo">
                            <a>
                                <img src="Content/images/ghb_logo.png" class="img-responsive" /></a>
                        </div>
                        <ul class="sidebar-menu" id="navAccordion" runat="server" clientidmode="static">

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Dashboard.aspx" data-title="Dashboard" data-closable="false">
                                    <span>Dashbosard</span>
                                </a>
                            </li>

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Statistic.aspx" data-title="Statistic">
                                    <span>Statistics</span>
                                </a>
                            </li>

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Searcher.aspx" data-title="Search">
                                    <span>Search</span>
                                </a>
                            </li>


                            <li class="sub-menu">
                                <a href="javascript:;">
                                    <i class="fa fa-building"></i>
                                    <span>User</span>
                                </a>
                                <ul class="sub">
                                    <li>
                                        <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="AdminList.aspx" data-title="Admin List">
                                            <span>Admin list</span>
                                        </a>
                                    </li>
                                </ul>
                            </li>

                            <li class="sub-menu">
                                <a href="javascript:;">
                                    <i class="fa fa-building"></i>
                                    <span>Organisation</span>
                                </a>
                                <ul class="sub">
                                    <li>
                                        <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="CompanyList.aspx" data-title="Company List">
                                            <span>Company List</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="DepartmentList.aspx" data-title="Department List">
                                            <span>Department List</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="BranchList.aspx" data-title="Branch List">
                                            <span>Branch List</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="DocumentSeparators.aspx" data-title="Document Separators">
                                            <span>Document Separators</span>
                                        </a>
                                    </li>
                                </ul>
                            </li>

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Batches.aspx" data-title="Batches">
                                    <span>Batches</span>
                                </a>
                            </li>

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Sets.aspx" data-title="Sets">
                                    <span>Sets</span>
                                </a>
                            </li>

                            <li>
                                <a href="javascript:;" class="easyui-linkbutton tab-menu-item" data-url="Simulation/Simulation.aspx" data-title="Simulation">
                                    <span>Simulation</span>
                                </a>
                            </li>

                        </ul>
                    </div>
                </div>
            </aside>

            <%--     <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
            <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
            <script src="Content/js/jquery-1.12.4.js"></script>
            <script src="Content/js/jquery-ui.js"></script>
            <script src="Content/js/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
            <script>
                $.browser = {};
                (function () {
                    $.browser.msie = false;
                    $.browser.version = 0;
                    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                        $.browser.msie = true;
                        $.browser.version = RegExp.$1;
                    }
                })();
            </script>
            <script type="text/javascript" src="Content/js/bootstrap-fileupload/bootstrap-fileupload.js"></script>
            <script type="text/javascript" src="Content/js/bootstrap-colorpicker/js/bootstrap-colorpicker.js"></script>
            
            <script>
                var $ui = $.noConflict(true);
            </script>
            
            <script src="Content/js/jquery.js"></script>
            <script src="Content/bs3/js/bootstrap.min.js"></script>
            <script src="Content/dynamic-tabs/jquery.easyui.min.js"></script>
            <script src="Content/js/jquery.dcjqaccordion.2.7.js"></script>
            <script src="Content/js/skycons/skycons.js"></script>
            <script src="Content/js/jquery.scrollTo.min.js"></script>
            <script src="Content/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.js"></script>
            <script src="Content/js/jquery.nicescroll.js"></script>
            <script src="Content/js/jquery.scrollTo/jquery.scrollTo.js"></script>
            <script src="Content/js/responsive-datatables/jquery.dataTables.min.js.js"></script>
            <script src="Content/js/responsive-datatables/dataTables.responsive.min.js"></script>
            
            <%--<script src="https://cdn.datatables.net/fixedcolumns/3.2.6/js/dataTables.fixedColumns.min.js"></script>--%>
            <script src="Content/js/iCheck/jquery.icheck.js"></script>
            <script class="include" type="text/javascript" src="http://bucketadmin.themebucket.net/js/jquery.dcjqaccordion.2.7.js"></script>
            <script src="http://bucketadmin.themebucket.net/js/chart-js/Chart.js"></script>

            <section id="main-content">
                <div id="snackbar">Some text some message..</div>
                <header class="header clearfix">
                    <div class="top-nav clearfix">
                        <ul class="nav pull-right top-menu">
                            <!-- user login dropdown start-->
                            <li class="dropdown">
                                <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                                    <img alt="" src="Content/images/avatar1_small.jpg" />
                                    <span class="username"><span id="userName" runat="server"></span></span>
                                    <b class="caret"></b>
                                </a>
                                <ul class="dropdown-menu extended logout">
                                    <li>
                                        <a href="javascript:;" class="tab-menu-item" data-url="ChangePassword.aspx" data-title="Change Password">
                                            <i class="fa fa-cogs"></i><span>Change Password</span>
                                        </a>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnLogout" runat="server" data-toggle="tooltip" data-placement="bottom" title="Logout" OnClick="btnLogout_Click">
                                        <i class="fa fa-power-off"></i> <span>Logout</span>
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <div class="language-selected">
                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                        <span id="languageCode" runat="server" class="uppercase text-uppercase">EN</span> <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu" id="languagesList" runat="server" clientidmode="static">
                                        <li data-toggle="tooltip" data-title="English" data-placement="left"><a href="javascript:;">EN</a></li>
                                        <li data-toggle="tooltip" data-title="Thai" data-placement="left"><a href="javascript:;">TH</a></li>
                                    </ul>
                                </div>
                            </li>
                        </ul>
                        <!--search & user info end-->
                    </div>
                </header>
                <section class="wrapper">
                    <div id="tt" class="easyui-tabs">
                    </div>
                </section>
            </section>
        </section>
        <input type="hidden" id="continueFunction" value="1" />
        <script src="Content/mycj/myscripts.js"></script>
        <script src="Content/js/scripts.js"></script>
        <script src="Content/js/js.cookie.min.js"></script>
        <%--<script src="https://cdn.jsdelivr.net/npm/js-cookie@2/src/js.cookie.min.js"></script>--%>
        <script>
                var isLanguageExec = false;
                function TranslateLanguage(selectedLanguage) {

                    console.log("Translate: " + new Date());
                    $('.tabs .tabs-selected .data-url').each(function (idx, obj) {
                        var pageName = $(obj).text();
                        if (!$(obj).hasClass(selectedLanguage)) {
                            $(obj).removeClass("en");
                            $(obj).removeClass("th");
                            $.getJSON("/language/langDir.json", function (data) {
                                console.log("Language file: " + new Date());
                                var langDir = data;
                                $("*:not(.notranslate *)").each(function (idx, tag) {
                                    $.each(langDir, function (idx, obj) {
                                        //console.log(langDir.length);
                                        //console.log("loop: " + new Date());
                                        if (obj.TextId == "0" || obj.TextPage == pageName) {
                                            //console.log("inner loop 1: " + new Date());
                                            var tagType = $(tag).attr('type');
                                            if (tagType == undefined || tagType == "button") {
                                                if ($(tag).html().indexOf('<') == -1) {
                                                    //if (!$(tag).hasClass('text-uppercase'))
                                                    //    $(tag).addClass('text-capitalize');
                                                    if ($(tag).text().toLowerCase() == obj.TextEn.trim().toLowerCase() || $(tag).text().toLowerCase() == obj.TextTh.trim().toLowerCase()) {
                                                        if (selectedLanguage == 'en')
                                                            $(tag).text(obj.TextEn);
                                                        if (selectedLanguage == 'th')
                                                            $(tag).text(obj.TextTh);
                                                        if (selectedLanguage == 'ms')
                                                            $(tag).text(obj.TextMs);
                                                        if (selectedLanguage == 'zh')
                                                            $(tag).text(obj.TextZh);
                                                    }
                                                }
                                            }
                                            if (tagType == "submit") {
                                                if ($(tag).val().toLowerCase() == obj.TextEn.trim().toLowerCase() || $(tag).val().toLowerCase() == obj.TextTh.trim().toLowerCase()) {
                                                    if (selectedLanguage == 'en')
                                                        $(tag).val(obj.TextEn);
                                                    if (selectedLanguage == 'th')
                                                        $(tag).val(obj.TextTh);
                                                }
                                            }
                                        }
                                    });

                                    //if (obj.TextPage == pageName) {
                                    //    $("*:not(.notranslate *)").each(function (idx, tag) {
                                    //        console.log("inner loop 2: " + new Date());
                                    //        var tagType = $(tag).attr('type');
                                    //        if (tagType == undefined || tagType == "button") {
                                    //            if ($(tag).html().indexOf('<') == -1) {
                                    //                //if (!$(tag).hasClass('text-uppercase'))
                                    //                //    $(tag).addClass('text-capitalize');
                                    //                if ($(tag).text().toLowerCase() == obj.TextEn.trim().toLowerCase() || $(tag).text().toLowerCase() == obj.TextTh.trim().toLowerCase()) {
                                    //                    if (selectedLanguage == 'en')
                                    //                        $(tag).text(obj.TextEn);
                                    //                    if (selectedLanguage == 'th')
                                    //                        $(tag).text(obj.TextTh);
                                    //                }
                                    //            }
                                    //        }
                                    //        if (tagType == "submit") {
                                    //            if ($(tag).val().toLowerCase() == obj.TextEn.trim().toLowerCase() || $(tag).val().toLowerCase() == obj.TextTh.trim().toLowerCase()) {
                                    //                if (selectedLanguage == 'en')
                                    //                    $(tag).val(obj.TextEn);
                                    //                if (selectedLanguage == 'th')
                                    //                    $(tag).val(obj.TextTh);
                                    //            }
                                    //        }
                                    //    });
                                    //}
                                });
                                $('.loadingDiv').addClass('hide');
                                console.log("Language file end: " + new Date());
                                isLanguageExec = false;
                            });
                            $(obj).addClass(selectedLanguage);
                        }
                        else {
                            $('.loadingDiv').addClass('hide');
                        }
                    });
                }
                var initLanguage = $('#languageCode').text().toLowerCase();
                if (Cookies.get("selectedLanguage") == undefined) {
                    Cookies.set("selectedLanguage", initLanguage);
                }
                else {
                    var selectedLanguage = Cookies.get("selectedLanguage");
                    $('#languageCode').html(selectedLanguage);
                    if (selectedLanguage != "en") {
                        $('.loadingDiv').removeClass('hide');
                        TranslateLanguage(selectedLanguage);
                    }
                }
                $(document).ready(function () {
                    $('#languagesList a').click(function () {
                        var langCode = $(this).html();
                        $('#languagesList a').removeClass('active');
                        $(this).addClass('active');
                        $('#languageCode').html(langCode);
                        var selectedLanguage = $('#languageCode').text().toLowerCase();
                        Cookies.set("selectedLanguage", selectedLanguage);
                        $('.loadingDiv').removeClass('hide');
                        TranslateLanguage(selectedLanguage);
                    });
                    $(document).on('hidden.bs.modal', function (event) {
                        if ($('.modal:visible').length) {
                            $('body').addClass('modal-open');
                        }
                    });
                    $('#tt').tabs({
                        onBeforeClose: function (title, index) {
                            return true;
                        },
                        onClose: function (title, index) {
                            setTimeout(function () {
                                $('.dataTable').resize();
                            }, 100);
                        }
                    });
                    $('body').on('click', '.tab-menu-item', function () {
                        var title = $(this).attr('data-title');
                        var url = $(this).attr('data-url');
                        var closable = $(this).attr('data-closable');
                        var showModal = $(this).attr('data-showModal');
                        if (showModal != '1') {
                            if ($('#tt').tabs('exists', '<span>' + title + '</span><span class="data-url hide">' + url + '</span>')) {
                                $('#tt').tabs('select', '<span>' + title + '</span><span class="data-url hide">' + url + '</span>');
                            } else {
                                if (closable == "false")
                                    closable = false;
                                else
                                    closable = true;
                                $.get(url, function (content) {
                                    var notranslate = "";
                                    if (url == "LanguageDirectory.aspx") {
                                        notranslate = "notranslate"
                                    }
                                    $('#tt').tabs('add', {
                                        title: '<span>' + title + '</span><span class="data-url hide ' + notranslate + '">' + url + '</span>',
                                        content: content,
                                        closable: closable
                                    });
                                }, 'html');
                            }
                        }
                        else {
                            var modalSize = $(this).attr('data-modalSize');
                            var dataId = $(this).attr('data-id');
                            var isModal = $(this).attr('data-isModal');
                            if (isModal != '1') {
                                $('#largeModalContent').html("");
                                if (modalSize == 'L') {
                                    $('#largeModal').modal('toggle');
                                    $('#largeModalTitle').html(title);
                                    $('#largeModalContent').load(url + "?id=" + dataId);
                                }
                            }
                            else {
                                $('#innerLargeModalContent').html("");
                                if (modalSize == 'L') {
                                    $('#innerLargeModal').modal('toggle');
                                    $('#innerLargeModalTitle').html(title);
                                    $('#innerLargeModalContent').load(url + "?id=" + dataId);
                                }
                                if (modalSize == 'M') {
                                    var dataInnerId = $(this).attr('data-innerId');
                                    $('#innerMediumModal').modal('toggle');
                                    $('#innerMediumModalTitle').html(title);
                                    $('#innerMediumModalContent').load(url + "?id=" + dataId + "&innerId=" + dataInnerId);
                                }
                            }
                        }
                    });
                    $('body').on('click', '.tabs-first', function () {
                        //$.get("Dashboard.aspx", function (content) {
                        //    var selectedTab = $('#tt').tabs("getTab", 0);
                        //    $('#tt').tabs('update', {
                        //        tab: selectedTab,
                        //        options: {
                        //            content: content
                        //        }
                        //    });
                        //}, 'html');
                    });
                    //var isAjaxActive = 0;
                    //var ajaxRequest;
                    //var refreshDashboard = function () {
                    //    var continueFunction = $('#continueFunction').val();
                    //    if (continueFunction == "1") {
                    //        if (isAjaxActive == 0) {
                    //            $('#IsNewSearch').val('1');
                    //            var data = $('#dashboardForm').serializeArray();
                    //            data = data.filter(function (item) {
                    //                return item.name.indexOf('_') === -1;
                    //            });
                    //            var url = "Dashboard.aspx?" + $.param(data);
                    //            var tab = $('#tt').tabs('getSelected');
                    //            var index = $('#tt').tabs('getTabIndex', tab);
                    //            if (index == 0) {
                    //                ajaxRequest = $.post(url, function (content) {
                    //                    isAjaxActive = 0;
                    //                    var selectedTab = $('#tt').tabs("getTab", 0);
                    //                    $('#tt').tabs('update', {
                    //                        tab: selectedTab,
                    //                        options: {
                    //                            content: content
                    //                        }
                    //                    });
                    //                    setTimeout(refreshDashboard, 2000);
                    //                }, 'html');
                    //            }
                    //            else {
                    //                setTimeout(refreshDashboard, 3000);
                    //            }
                    //        }
                    //        else {
                    //            isAjaxActive = 1;
                    //            setTimeout(refreshDashboard, 2000);
                    //        }
                    //    }
                    //    else {
                    //        setTimeout(refreshDashboard, 2000);
                    //    }
                    //}
                    //setTimeout(refreshDashboard, 2000);

                    //var refreshLanguage = function () {
                    //    if (!isLanguageExec) {
                    //        var selectedLanguage = $('#languageCode').text().toLowerCase();
                    //        isLanguageExec = true;
                    //        TranslateLanguage(selectedLanguage);
                    //        setTimeout(refreshLanguage, 3000);
                    //    }
                    //}
                    //setTimeout(refreshLanguage, 3000);

                });

            //$(document).ready(function () {
            //    $.ajaxSetup({ cache: false });
            //    var isAjaxActive = 0;
            //    var ajaxRequest;
            //    var refreshDashboard = function () {
            //        var continueFunction = $('#continueFunction').val();
            //        if (continueFunction == "1") {
            //            if (isAjaxActive == 0) {
            //                $('#IsNewSearch').val('1');
            //                var data = $('#dashboardForm').serializeArray();
            //                data = data.filter(function (item) {
            //                    return item.name.indexOf('_') === -1;
            //                });
            //                var url = "Dashboard.aspx/GetNewData?" + $.param(data);
            //                var tab = $('#tt').tabs('getSelected');
            //                var index = $('#tt').tabs('getTabIndex', tab);
            //                if (index == 0) {
            //                    ajaxRequest = $.get(url, function (content) {
            //                        var data = $(content).find('#sveimList').html();
            //                        $('#sveimList').html(data);
            //                        var selectedLanguage = $('#languageCode').text().toLowerCase();
            //                        $('.tabs .data-url').first().removeClass(selectedLanguage);
            //                        TranslateLanguage(selectedLanguage);
            //                        isAjaxActive = 0;
            //                        $.getScript("Content/mycj/common-scripts.js", function (data, textStatus, jqxhr) {
            //                            //console.log( data ); // Data returned
            //                            //console.log( textStatus ); // Success
            //                            //console.log( jqxhr.status ); // 200
            //                            //console.log( "Load was performed." );
            //                        });
            //                        //Refresh
            //                        setTimeout(refreshDashboard, 5000);
            //                    }, 'html');
            //                }
            //                else {
            //                    setTimeout(refreshDashboard, 5000);
            //                }
            //            }
            //            else {
            //                isAjaxActive = 1;
            //                setTimeout(refreshDashboard, 5000);
            //            }
            //        }
            //        else {
            //            setTimeout(refreshDashboard, 5000);
            //        }
            //    }
            //    setTimeout(refreshDashboard, 5000);
            //});
        </script>
        <script>
                function showSnackBar(message) {
                    var x = $("#snackbar");
                    x.html(message);
                    x.addClass('show');
                    setTimeout(function () {
                        x.className = x.removeClass('show');
                    }, 3000);
                }
        </script>

    </form>
</body>
</html>
