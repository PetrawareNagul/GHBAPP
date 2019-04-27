<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCompany.aspx.cs" Inherits="Silverlake.Web.AddCompany" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div id="addCompanyForm" runat="server">
        <div class="row" id="FormId" data-value="addCompanyForm">
            <div class="col-md-12">
                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Code:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="hidden" name="Id" value="0" id="Id" runat="server" clientidmode="static" />
                        <input type="hidden" name="Status" value="1" id="Status" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedBy" value="0" id="CreatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedDate" value="0" id="CreatedDate" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedBy" value="0" id="UpdatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedDate" value="0" id="UpdatedDate" runat="server" clientidmode="static" />

                        <input type="text" name="Code" id="Code" runat="server" clientidmode="static" class="form-control" placeholder="Enter Code" />
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Name:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="text" name="Name" id="Name" runat="server" clientidmode="static" class="form-control" placeholder="Enter Name" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
