<%@ Page Title="Fire Grant: Manage FDIDs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageFDIDs.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageFDIDs" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><span id="spHeader" runat="server"></span></h2>
        <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Fire Department ID List</h3>
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgFDIDs" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="25" OnNeedDataSource="rgFDIDs_NeedDataSource" OnPageIndexChanged="rgFDIDs_PageIndexChanged" OnItemDataBound="rgFDIDs_ItemDataBound" OnItemCommand="rgFDIDs_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="View" CommandArgument='<%# Eval("FDID") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="FDID" FilterControlAltText="Filter FDID column" HeaderText="FDID" UniqueName="FDID" Resizable="False">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FireDepartment" FilterControlAltText="Filter Fire Department column" HeaderText="Fire Department" UniqueName="FireDepartment">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Inactive" FilterControlAltText="Filter Inactive column" HeaderText="Inactive" UniqueName="Inactive">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-md-2">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#fdidModal">
                    Add New FDID
                </button>
            </div>
            <div class="col-md-2">
                <%--<asp:Button ID="btnDelete" CssClass="btn btn-primary" runat="server" Text="Inactivate Category" />--%>
                <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnClose_Click" />
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="fdidModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblFDIDHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblFDIDHeader">Fire Department ID</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblFDIDError" runat="server"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblFDID" runat="server" AssociatedControlID="txtFDID" Text="FDID: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFDID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="100" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblDepartmentName" runat="server" AssociatedControlID="txtDepartmentName" Text="Department Name: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDepartmentName" runat="server" Width="250px" ClientIDMode="Static" MaxLength="50" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblFDIDInactive" runat="server" AssociatedControlID="chkFDIDInactive" Text="Inactive:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:Checkbox ID="chkFDIDInactive" runat="server" />
                        </div>
                    </div>
                    <asp:HiddenField ID="hfFDID" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <%--<button id="btnDeletePriority" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeletePriority_ServerClick">Inactivate Priority</button>--%>
                    <button id="btnSaveFDID" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveFDID_ServerClick">Save FDID</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showFDIDDelete();
        });
        
        function showFDIDDelete() {
            var noteId = $('#hfFDID').val();
            if (noteId === "") {
                $('#MainContent_btnDeleteFDID').hide();
            }
            else {
                $('#MainContent_btnDeleteFDID').show();
            }
        }

        function openFDIDModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#MainContent_btnDeleteFDID').show();
        }

        function clearNoteId() {
            $('#hfFDID').val('');
            $('#txtFDID').val('');
            $('#txtDepartmentName').val('');
            $('#MainContent_btnDeleteFDID').hide();
        }
    </script>
</asp:Content>
