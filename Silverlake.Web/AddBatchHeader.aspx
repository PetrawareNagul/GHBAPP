<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBatchHeader.aspx.cs" Inherits="Silverlake.Web.AddBatchHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="addBatchHeaderForm" runat="server">
        <div class="row" id="FormId" data-value="addBatchHeaderForm">
            <div class="col-md-12">

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Department:</label>
                    </div>
                    <div class="col-md-6">
                        
                        <select id="DepartmentId" name="DepartmentId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                            <option value="1">E-LIBRARY</option>
                            <option value="2">ETP-LL</option>
                            <option value="3">ETP-LN</option>
                            <option value="4">ETP-PR</option>
                            <option value="5">ETP-WF</option>
                            <option value="6">LOS-AD</option>
                            <option value="7">LOS-CL</option>
                            <option value="8">LOS-LA</option>
                        </select>
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Code:</label>
                    </div>
                    <div class="col-md-6">
                    <%--    <input type="hidden" name="Id" value="0" id="Id" runat="server" clientidmode="static" />
                        <input type="hidden" name="Status" value="1" id="Status" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedBy" value="0" id="CreatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="CreatedDate" value="0" id="CreatedDate" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedBy" value="0" id="UpdatedBy" runat="server" clientidmode="static" />
                        <input type="hidden" name="UpdatedDate" value="0" id="UpdatedDate" runat="server" clientidmode="static" />
                        <input type="hidden" name="Url" id="Url" runat="server" clientidmode="static" />--%>

                        <input type="text" name="Code" id="Code" runat="server" clientidmode="static" class="form-control" placeholder="Enter Code" />
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Name:</label>
                    </div>
                    <div class="col-md-6">
                        <input id="fileUpload" type="file" />
                        <%--<input type="text" name="Name" id="Name" runat="server" clientidmode="static" class="form-control" placeholder="Enter Name" />--%>
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
