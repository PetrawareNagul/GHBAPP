<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLanguageDir.aspx.cs" Inherits="Silverlake.Web.AddLanguageDir" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="addLanguageDirForm" runat="server">
        <div id="FormId" data-value="addLanguageDirForm">
            <div class="row">
                <div class="col-md-12">
                    <div class="row mb-6">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Module:</label>
                        </div>
                        <div class="col-md-6">
                            <input type="hidden" name="ID" value="0" id="ID" runat="server" clientidmode="static" />
                            <input type="hidden" name="TextId" value="0" id="TextId" runat="server" clientidmode="static" />
                            <input type="hidden" name="Status" value="1" id="Status" runat="server" clientidmode="static" />
                            <input type="hidden" name="Remarks" value="" id="Remarks" runat="server" clientidmode="static" />
                            <select name="Module" class="form-control" id="Module" runat="server" clientidmode="static">
                                <option value="">Select</option>
                                <option value="0">Common</option>
                                <option value="1">Others</option>
                            </select>
                        </div>
                    </div>

                    <div class="row mb-6">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Page:</label>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group">
                                <select name="TextPage" class="form-control" id="TextPage" runat="server" clientidmode="static">
                                    <option value="">Select</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-6">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Text in English:</label>
                        </div>
                        <div class="col-md-9">
                            <textarea name="TextEn" class="form-control" rows="5" placeholder="Enter text in English" id="TextEn" runat="server" clientidmode="static"></textarea>
                        </div>
                    </div>

                    <div class="row mb-6">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Text in Thai:</label>
                        </div>
                        <div class="col-md-9">
                            <textarea name="TextTh" class="form-control" rows="5" placeholder="Enter text in Thai" id="TextTh" runat="server" clientidmode="static"></textarea>
                        </div>
                    </div>

                    <div class="row mb-6 hide">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Text in Malay:</label>
                        </div>
                        <div class="col-md-9">
                            <textarea name="TextMs" class="form-control" rows="5" placeholder="Enter text in Malay" id="TextMs" runat="server" clientidmode="static"></textarea>
                        </div>
                    </div>

                    <div class="row mb-6 hide">
                        <div class="col-md-3">
                            <label class="fs-14 mt-2">Text in Chinese:</label>
                        </div>
                        <div class="col-md-9">
                            <textarea name="TextZh" class="form-control" rows="5" placeholder="Enter text in Chinese" id="TextZh" runat="server" clientidmode="static"></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
