<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Batches.aspx.cs" Inherits="Silverlake.Web.Batches" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="batchesForm" runat="server">
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
                                    <div class="action-section">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <button data-message="Successfully deactivated" data-status="Inactive" data-original-title="Deactivate" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Deactivate" data-isconfirm="1"><i class="fa fa-ban"></i></button>
                                                <button data-message="Successfully activated" data-status="Active" data-original-title="Activate" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Activate" data-isconfirm="0"><i class="fa fa-check"></i></button>
                                                <span class="common-divider pull-right"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can search here with Batch number</span></small>
                                </div>
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can Activate/ Deactivate on Batches</span></small>
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
                                                <th>Branch</th>
                                                <th>Department</th>
                                                <th>Batch No</th>
                                                <th>Stage</th>
                                                <th>Batch count</th>
                                                <th>Batch status</th>
                                            </tr>
                                        </thead>
                                        <tbody id="batchesTbody" runat="server">
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
    <script src="Content/mycj/common-scripts.js"></script>
</body>
</html>
