<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Silverlake.Web.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Reset Password - GHB</title>
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
    <div class="container">
        <form id="resetPasswordForm" runat="server" class="form-signin">
            <h2 class="form-signin-heading">
                <img class="img-responsive" src="Content/images/ghb_logo.png" style="height: 100px;" />
            </h2>
            <div class="login-wrap">
                <div class="user-login-info">
                    <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="New password" autofocus></asp:TextBox>
                    <asp:TextBox ID="txtConfirmPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Retype password"></asp:TextBox>
                    <div id="resetPasswordResponse" runat="server" clientidmode="static"></div>
                </div>
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-lg btn-login btn-block" Text="Submit" OnClick="btnSubmit_Click" />

                <%--<div class="registration">
                    Don't have an account yet?
                <a class="" href="registration.html">Create an account
                </a>
                </div>--%>
            </div>
        </form>
    </div>
</body>
</html>
