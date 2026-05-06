<%@ Page Title="Fire Grant: Manage Legacy Applications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageLegacyApps.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageLegacyApps" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Manage Legacy Applications</h2>
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Department Info</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <asp:Label ID="lblDepartment" runat="server" Text="Select Department: " AssociatedControlID="rcbDepartments"></asp:Label>
            </div>
            <dov class="col-md-4">
                <telerik:RadComboBox ID="rcbDepartments" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rcbDepartments_SelectedIndexChanged" Width="100%" AccessibilityMode="True" EnableAriaSupport="True"></telerik:RadComboBox>
            </dov>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Legacy Applications</h3>
            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#legacyAppModal">
                    Add Application
                </button>
                <button id="btnHideModal" class="btn btn-primary" style="display:none" data-dismiss="modal" data-target="#legacyAppModal">Hide Modal</button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:DataList ID="dlApplications" runat="server" RepeatColumns="3" CellSpacing="2" RepeatLayout="Table" OnItemCommand="dlApplications_ItemCommand">
                    <ItemTemplate>  
                        <div class="appTableDiv">
                            <table class="apptable">  
                                <tr>  
                                    <th colspan="2">  
                                        <b>  
                                            <%# Eval("FiscalYear") %></b>  
                                    </th>  
                                </tr>  
                            
                                <tr>  
                                    <td>  
                                        File Name:  
                                    </td>  
                                    <td>  
                                        <%# Eval("FileName")%>  
                                    </td>  
                                </tr> 
                                <tr>
                                    <td colspan="2">
                                        <a href='<%# Eval("FilePath")%>' target="_blank">View Application</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:LinkButton ID="lnkViewEdit" runat="server" CommandArgument='<%# Eval("FiscalYear") %>' CommandName="Edit">Edit/Change Application</asp:LinkButton>
                                    
                                    </td>
                                </tr>
                                <%--<tr>  
                                    <td>  
                                        Designation:  
                                    </td>  
                                    <td>  
                                        <%# Eval("Designation")%>  
                                    </td>  
                                </tr>  --%>
                            </table>  
                        </div>
                        
                    </ItemTemplate>  
                </asp:DataList>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="legacyAppModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblModalHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblModalHeader">Legacy Application</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblModalError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblFiscalYear" runat="server" AssociatedControlID="txtFiscalYear" Text="Fiscal Year: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFiscalYear" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblHelpImage" runat="server" AssociatedControlID="ruAppUpload" Text="Select Application:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="ruAppUpload" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1">
                                <FileFilters>
                                    <telerik:FileFilter Description="Images(pdf;doc;docx)" Extensions="pdf,doc,docx" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" OnClientClick="return confirm('Are you sure you want to delete this application?');" OnClick="btnDeleteApp_ServerClick" Text="Delete Application" />
                    <%--<button id="btnDeleteApp" class="btn btn-primary" runat="server" data-dismiss="modal" onclick="return confirm('Are you sure you want to delete this application?');" onserverclick="btnDeleteApp_ServerClick">Delete Application</button>--%>
                    <button id="btnSaveHelp" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveHelp_ServerClick">Save Application</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

        });
        

        function openHelpModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            //$('#MainContent_btnDeleteHelp').show();
        }

        function closeHelpModal() {
            $('#btnHideModal').click();
        }

        function clearNoteId() {
            $('#txtFiscalYear').val('');
            $('#MainContent_dvHelpDocLink').hide();
            $('#ddlPage').val("");
            //$('#MainContent_btnDeleteHelp').hide();
        }

        $("#ddlPage").change(function () {
            showOther();
        });
    </script>
</asp:Content>
