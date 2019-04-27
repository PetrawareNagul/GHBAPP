<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetView.aspx.cs" Inherits="Silverlake.Web.SetView" %>

<!DOCTYPE html>

<%--<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
                                                <th>Details</th>
                                                <th>Data</th>
                                                <th>View</th>
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
</body>
</html>--%>



<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
        #left_col {
            font-size: 12px;
            background: #c3c3c3;
            color: black;
        }

        #right_col1 {
            font-size: 12px;
            background: #FFF;
            border-color: #000;
            border: 1px solid #000;
        }

        #props {
            background: #c3c3c3;
            color: #333;
            padding: 10px;
            height: 100vh;
        }

        #ResultHolder .pdf-document {
            background: #94ddf5;
            margin: 5px 0;
            padding: 5px 5px;
        }
        #ResultHolder .pdf-document.active {
            background: #91b9e2;
        }

        .rotate90 {
            -webkit-transform: rotate(-90deg);
            -moz-transform: rotate(-90deg);
            -o-transform: rotate(-90deg);
            -ms-transform: rotate(-90deg);
            transform: rotate(-90deg);
        }

        #metaTag {
            position: absolute;
            margin-left: -55px;
            margin-top: 60px;
        }

        #preView {
            position: absolute;
            margin-left: -50.5px;
            margin-top: 140px;
        }

        .col-no-padding {
            padding-left: 0 !important;
            padding-right: 0 !important;
        }

        #FileHolder iframe {
            height: calc(100vh - 40px);
        }
        .fa-file-pdf-o{
            color: #ff0000;
        }
    </style>
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
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/mycj/mystyles.css" rel="stylesheet" />
    <link href="Content/mycj/My-StyleSheet.css" rel="stylesheet" />
    <link href="Content/mycj/ghb-styles.css" rel="stylesheet" />
</head>
<body>
    <section class="container-fluid">
        <div class="row">
            <div class="col-md-2 col-no-padding">
                <div id="props">
                    <div id="PropsHolder" runat="server">
                    </div>
                </div>

            </div>

            <div class="col-md-5 col-no-padding">
                <div>
                    <div class="col-md-12">
                        <h5><strong>Name</strong></h5>
                    <div id="ResultHolder" runat="server">
                    </div>
                    </div>
                </div>
            </div>
            <div class="col-md-5" id="FileHolder" runat="server">
            </div>
        </div>
    </section>

    <form runat="server">
        <input type="hidden" id="IsNewSearch" name="IsNewSearch" value="0" runat="server" clientidmode="static" />
        <div id="setsTbody" runat="server" visible="false"></div>
        <asp:HiddenField ID="hdnNumberPerPage" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTotalRecordsCount" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentPageNo" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>
<script src="Content/js/jquery-1.12.4.js"></script>
<script src="Content/js/jquery-ui.js"></script>
<script src="Content/mycj/common-scripts.js"></script>

<script>
    $(document).ready(function () {
        $('#metaTag').click(function () {
            $('.pdfView').hide();
            $('#tblView').show();
        });
        $('#preView').click(function () {

            if ($('div.active').find('a').length == 0) {
                $('.pdfView').first().show()
            }
            else {
                var id = $('div.active').find('a').attr('id');
                $('.pdfView').hide();
                $('#pdf-' + id).show();
            }
            $('#tblView').hide();
           // $('.pdfView').show();
        });
    });

    function pdfview(ths) {
        var id = ths.id;
        $('#tblView').hide();
        $('.pdfView').hide();
        $('#pdf-' + id).show();
        $('.pdf-document').removeClass('active');
        $(ths).parent('div').addClass('active');
        $('div.active').find('a').length
    }
</script>

