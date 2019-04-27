<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdmin.aspx.cs" Inherits="Silverlake.Web.AddAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div id="addAdminForm" runat="server">
        <div class="row" id="FormId" data-value="addAdminForm">
            <div class="col-md-12">

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Role:</label>
                    </div>
                    <div class="col-md-6">
                        <select id="UserRoleId" name="UserRoleId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                        </select>
                    </div>
                </div>
                <input type="hidden" name="CompanyId" value="1" id="CompanyId" runat="server" clientidmode="static" />

                <div class="row mb-6 add-admin-branch-input-row">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Branch:</label>
                    </div>
                    <div class="col-md-6">
                        <select id="BranchId" name="BranchId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                        </select>
                    </div>
                </div>

                <input type="hidden" name="DepartmentId" value="0" id="DepartmentId" runat="server" clientidmode="static" />

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Username:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="hidden" name="Id" value="0" id="Id" runat="server" clientidmode="static" />
                        <input type="hidden" name="IsAll" value="1" id="IsAll" runat="server" clientidmode="static" />
                        <input type="hidden" name="Status" value="1" id="Status" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedBy" value="0" id="CreatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedDate" value="0" id="CreatedDate" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedBy" value="0" id="UpdatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedDate" value="0" id="UpdatedDate" runat="server" clientidmode="static" />

                        <input type="hidden" name="Password" id="Password" runat="server" clientidmode="static" class="form-control" placeholder="Enter Name" />
                        <input type="hidden" name="TransPwd" id="TransPwd" runat="server" clientidmode="static" class="form-control" placeholder="Enter Name" />
                        <input type="hidden" name="UniqueKey" id="UniqueKey" runat="server" clientidmode="static" class="form-control" placeholder="Enter Name" />
                        <input type="hidden" name="IsOnline" value="0" id="IsOnline" runat="server" clientidmode="static" />
                        <input type="hidden" name="IsActive" value="0" id="IsActive" runat="server" clientidmode="static" />
                        <input type="hidden" name="IsPrimary" value="0" id="IsPrimary" runat="server" clientidmode="static" />
                        <input type="hidden" name="RegisterIp" value="0" id="RegisterIp" runat="server" clientidmode="static" />
                        <input type="hidden" name="LastLoginOn" value="" id="LastLoginOn" runat="server" clientidmode="static" />
                        <input type="hidden" name="LastLoginIp" value="0" id="LastLoginIp" runat="server" clientidmode="static" />

                        <input type="text" name="Username" id="Username" runat="server" clientidmode="static" class="form-control" placeholder="Enter Username" />
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Email:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="text" name="EmailId" id="EmailId" runat="server" clientidmode="static" class="form-control" placeholder="Enter Email" />
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Mobile number:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="text" name="MobileNumber" id="MobileNumber" runat="server" onkeypress="return isNumber(event)" clientidmode="static" class="form-control" placeholder="Enter Mobile number" />
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">API Auth Token:</label>
                    </div>
                    <div class="col-md-9">
                        <input type="text" name="ApiAuthToken" id="ApiAuthToken" runat="server" clientidmode="static" class="form-control" placeholder="System generates" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#UserRoleId').change(function () {
                var value = $(this).val();
                if (value == "3" || value == "4") {
                    $('.add-admin-branch-input-row').addClass('hide');
                    $('#BranchId').val("0");
                    $('#BranchId').attr('disabled', 'disabled');
                }
                else {
                    $('.add-admin-branch-input-row').removeClass('hide');
                    $('#BranchId').removeAttr('disabled');
                }
            });
            $('#UserRoleId').change();
        });


        function isNumber(evt) {
            debugger;
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if ($('#MobileNumber').val().length >= 14) { return false; }
            return true;
        }

    </script>
</body>
</html>