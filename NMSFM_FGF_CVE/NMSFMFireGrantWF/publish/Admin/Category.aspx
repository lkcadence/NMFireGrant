<%@ Page Title="Fire Grant: Edit Category" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.EditCategory" Async="true" %>

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
                <h3>Category Information</h3>
            </div>
        </div>
        <%--<div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblCategoryNumber" runat="server" AssociatedControlID="txtCategoryNumber" Text="Category Number *"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtCategoryNumber" CssClass="formContral" runat="server" TextMode="Number" aria-require="true"></asp:TextBox>
            </div>
        </div>--%>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblCategoryName" runat="server" AssociatedControlID="txtCategoryName" Text="Category Name *"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtCategoryName" CssClass="formContral" runat="server" aria-require="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="lblInactive" runat="server" AssociatedControlID="chkInactive" Text="Inactive:"></asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:Checkbox ID="chkInactive" runat="server" />
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Category Priorities</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgPriorities" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="25" OnNeedDataSource="rgPriorities_NeedDataSource" OnPageIndexChanged="rgPriorities_PageIndexChanged" OnItemDataBound="rgPriorities_ItemDataBound" OnItemCommand="rgPriorities_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="View" CommandArgument='<%# Eval("PriorityId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="PriorityName" FilterControlAltText="Filter Priority column" HeaderText="Priority" UniqueName="PriorityName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Inactive" FilterControlAltText="Filter Inactive column" HeaderText="Inactive" UniqueName="Inactive">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PriorityId" FilterControlAltText="Filter PriorityId column" HeaderText="PriorityId" UniqueName="PriorityId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-md-2">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#priorityModal">
                    Add New Priority
                </button>
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" onclick="btnSave_Click" />
            </div>
            <div class="col-md-2">
                <%--<asp:Button ID="btnDelete" CssClass="btn btn-primary" runat="server" Text="Inactivate Category" />--%>
                <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnClose_Click" />
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="priorityModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblPriorityHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblPriorityHeader">Priority</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblPriorityError" runat="server"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblPriority" runat="server" AssociatedControlID="txtPriority" Text="Priority: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtPriority" runat="server" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblPriorityInactive" runat="server" AssociatedControlID="chkPriorityInactive" Text="Inactive:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:Checkbox ID="chkPriorityInactive" runat="server" />
                        </div>
                    </div>
                    <asp:HiddenField ID="hfPriorityId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <%--<button id="btnDeletePriority" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeletePriority_ServerClick">Inactivate Priority</button>--%>
                    <button id="btnSavePriority" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSavePriority_ServerClick">Save Priority</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showPriorityDelete();
        });
        
        function showPriorityDelete() {
            var noteId = $('#hfPriorityId').val();
            if (noteId === "") {
                $('#MainContent_btnDeletePriority').hide();
            }
            else {
                $('#MainContent_btnDeletePriority').show();
            }
        }

        function openPriorityModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#MainContent_btnDeletePriority').show();
        }

        function clearNoteId() {
            $('#hfPriorityId').val('');
            $('#txtPriority').val('');
            $('#MainContent_btnDeletePriority').hide();
        }
    </script>
</asp:Content>
