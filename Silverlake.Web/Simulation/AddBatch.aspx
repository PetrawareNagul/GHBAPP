<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBatch.aspx.cs" Inherits="Silverlake.Web.Simulation.AddBatch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="addScanForm" runat="server">
        <div class="row" id="FormId" data-value="addScanForm">
            <div class="col-md-12">

                <input type="hidden" name="Id" id="Id" runat="server" value="0" clientidmode="static" />
                <input type="hidden" name="BatchKey" id="BatchKey" value="0" runat="server" clientidmode="static" />
                <input type="hidden" name="BatchNo" id="BatchNo" value="0" runat="server" clientidmode="static" />
                <input type="hidden" name="BatchStatus" id="BatchStatus" value="0" runat="server" clientidmode="static" />
                <input type="hidden" name="Status" id="Status" value="0" runat="server" clientidmode="static" />
                <input type="hidden" name="CreatedDate" id="CreatedDate" runat="server" clientidmode="static" />
                <input type="hidden" name="UpdatedDate" id="UpdatedDate" runat="server" clientidmode="static" />

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Branch:</label>
                    </div>
                    <div class="col-md-6">
                        <select name="BranchId" id="BranchId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                        </select>
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Department:</label>
                    </div>
                    <div class="col-md-6">
                        <select name="DepartmentId" id="DepartmentId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                        </select>
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Stage:</label>
                    </div>
                    <div class="col-md-6">
                        <select name="StageId" id="StageId" runat="server" clientidmode="static" class="form-control">
                            <option value="">Select</option>
                        </select>
                    </div>
                </div>

                <div class="row mb-6">
                    <div class="col-md-3">
                        <label class="fs-14 mt-2">Stage:</label>
                    </div>
                    <div class="col-md-6">
                        <input type="text" name="BatchCount" id="BatchCount" value="0" runat="server" clientidmode="static" class="form-control" />
                    </div>
                </div>

            </div>
        </div>
    </form>
</body>
</html>
