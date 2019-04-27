<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Silverlake.Web.ChangePassword" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Change Password - GHB</title>
    <link rel="shortcut icon" href="/Content/images/favicon.png" />
    <!--Core CSS -->
    <link href="Content/bs3/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/bootstrap-reset.css" rel="stylesheet" />
    <link href="Content/font-awesome/css/font-awesome.css" rel="stylesheet" />

    <!-- Custom styles for this template -->
    <link href="Content/css/style.css" rel="stylesheet" />
    <link href="Content/css/style-responsive.css" rel="stylesheet" />
    <link href="Content/mycj/ghb-styles.css" rel="stylesheet" />
    <link href="Content/mycj/My-StyleSheet.css" rel="stylesheet" />
</head>
<body class="login-body">
    <div class="container-fluid">
        <form id="changePasswordForm" runat="server" class="form-signin" clientidmode="static">
            <div class="page-container-section">
            <div class="login-wrap">
                <div class="user-login-info">
                    <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Old password" autofocus></asp:TextBox>
                    <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="New password"></asp:TextBox>
                    <asp:TextBox ID="txtConfirmPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Retype password"></asp:TextBox>
                    <div id="resetPasswordResponse" runat="server" clientidmode="static"></div>
                </div>
                <%--<asp:Button ID="btnFormSubmit" runat="server" CssClass="btn btn-lg btn-login btn-block" Text="Submit" />--%>
                <a href="javascript:;" class="btn btn-lg btn-login btn-block" id="btnChangePasswordSubmit">Submit</a>
            </div>
            </div>
            <script>
                $(document).ready(function () {
                    $('#btnChangePasswordSubmit').click(function () {
                        var data = $('#changePasswordForm').serializeArray();
                        data = data.filter(function (item) {
                            return item.name.indexOf('_') === -1;
                        });
                        var url = "/ChangePassword.aspx?" + $.param(data) + "&isSubmit=1";
                        $.post(url, function (content) {
                            var selectedTab = $('#tt').tabs('getSelected');
                            $('#tt').tabs('update', {
                                tab: selectedTab,
                                options: {
                                    content: content
                                }
                            });
                            show
                        }, 'html');
                    });
                });
            </script>
        </form>
    </div>
</body>
</html>
