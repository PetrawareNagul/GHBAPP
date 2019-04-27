<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LanguageDirectory.aspx.cs" Inherits="Silverlake.Web.LanguageDirectory" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="languageDirForm" runat="server">
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
                                                <button data-original-title="Add" data-trigger="hover" data-placement="bottom" type="button" class="popovers btn action-button pull-right" data-action="Add" data-url="AddLanguageDir.aspx" data-title="Add Language Directory"><i class="fa fa-plus"></i></button>
                                                <button data-original-title="Edit" data-trigger="hover" data-placement="bottom" data-content="Select one record" type="button" class="popovers btn action-button pull-right" data-action="Edit" data-url="AddLanguageDir.aspx" data-title="Edit Language Directory"><i class="fa fa-edit"></i></button>
                                                <button data-original-title="Sync" data-trigger="hover" data-placement="bottom" data-content="Syncs language JSON directory" type="button" class="popovers btn action-button pull-right" data-action="UpdateJSON" data-isconfirm="0"><i class="fa fa-refresh"></i></button>
                                                <%--<button type="button" class="btn action-button pull-right"><i class="fa fa-edit"></i></button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can search here with Page name/ Text En/ Text Th</span></small>
                                </div>
                                <div class="col-md-6">
                                    <small><strong>Note:</strong> <span>You can Sync directory to JSON/ Activate/ Deactivate on Languages</span></small>
                                </div>
                            </div>
                        </div>
                        <div class="page-container-section">
                            <div class="row">
                                <div class="col-md-12 notranslate">
                                    <table class="display responsive nowrap table table-bordered dataTable langDirList" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th>No.</th>
                                                <th>Page</th>
                                                <th>Text En</th>
                                                <th>Text Th</th>
                                                <th>Remarks</th>
                                            </tr>
                                        </thead>
                                        <tbody id="langDirTableBody" runat="server">
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
