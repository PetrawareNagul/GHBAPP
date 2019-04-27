<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Silverlake.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - GHB</title>
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
        <form id="loginForm" runat="server" class="form-signin">
            <h2 class="form-signin-heading">
                <img class="img-responsive" src="Content/images/ghb_logo.png" style="height: 100px;" />
            </h2>
            <div class="login-wrap">
                <div class="user-login-info">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username" autofocus></asp:TextBox>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Password"></asp:TextBox>
                </div>
                <label class="checkbox">
                    <%--<input type="checkbox" value="remember-me" />
                    Remember me--%>
                    <span class="pull-right mb-15">
                        <a data-toggle="modal" href="#forgotPasswordModal">Forgot Password?</a>
                    </span>
                </label>
                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-login btn-block" Text="Sign in" OnClick="btnLogin_Click" />

                <%--<div class="registration">
                    Don't have an account yet?
                <a class="" href="registration.html">Create an account
                </a>
                </div>--%>
            </div>

            <!-- Modal -->
            <div aria-hidden="true" aria-labelledby="forgotPasswordModalLabel" role="dialog" tabindex="-1" id="forgotPasswordModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">Forgot Password ?</h4>
                        </div>
                        <div class="modal-body">
                            <p>Enter your e-mail address below to reset your password.</p>
                            <asp:TextBox ID="forgotPasswordEmail" runat="server" placeholder="Email" CssClass="form-control placeholder-no-fix" autocomplete="off"></asp:TextBox>
                            <div id="forgotPasswordResponse" runat="server" clientidmode="static"></div>
                        </div>
                        <div class="modal-footer">
                            <button data-dismiss="modal" class="btn btn-default" type="button">Cancel</button>
                            <asp:Button ID="forgotPasswordSubmit" runat="server" CssClass="btn btn-success" Text="Submit" OnClick="forgotPasswordSubmit_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- modal -->
            <!--Core js-->
            <script src="Content/js/jquery.js"></script>
            <script src="Content/bs3/js/bootstrap.min.js"></script>
            <script>
                function OpenForgotPasswordModal() {
                    $('[href="#forgotPasswordModal"]').click();
                }
            </script>
        </form>
    </div>


</body>
</html>
