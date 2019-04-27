<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginHistory.aspx.cs" Inherits="Silverlake.Web.LoginHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login History - GHB</title>
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
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <form id="loginHistoryForm" runat="server" class="form-signin login-history">

                    <h2 class="form-signin-heading">
                        <img class="img-responsive" src="Content/images/ghb_logo.png" style="height: 100px;" />
                    </h2>
                    <div class="login-wrap">
                        <table class="display responsive nowrap table table-bordered dataTable table-striped" cellspacing="0" width="100%">
                            <thead>
                                <tr>
                                    <th colspan="3" class="text-center">Recent logins <small>(Showing latest 5)</small></th>
                                </tr>
                                <tr>
                                    <th>Date</th>
                                    <th>PC user</th>
                                    <th>IP address</th>
                                </tr>
                            </thead>
                            <tbody id="loginHistoryTbody" runat="server">
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="3">
                                        <a href="/Login.aspx" class="btn btn-lg btn-login btn-block">Proceed to Login</a>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
