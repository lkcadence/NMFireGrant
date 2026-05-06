<%@ Page Title="Fire Grant: Manage Categories" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageCategories" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Manage Categories</h2>
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Categories</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgCategories" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="25" OnNeedDataSource="rgCategories_NeedDataSource" OnPageIndexChanged="rgCategories_PageIndexChanged" OnItemDataBound="rgCategories_ItemDataBound" OnItemCommand="rgCategories_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridButtonColumn ButtonType="LinkButton" ConfirmDialogType="RadWindow" FilterControlAltText="Filter Edit column" HeaderText="Edit" Text="Edit" UniqueName="Edit" CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter Category column" HeaderText="Category" UniqueName="CategoryName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Inactive" FilterControlAltText="Filter Inactive column" HeaderText="Inactive" UniqueName="Inactive">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CategoryId" FilterControlAltText="Filter CategoryId column" HeaderText="CategoryId" UniqueName="CategoryId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnAddCategory" CssClass="btn btn-primary" runat="server" Text="Add New Category" OnClick="btnAddCategory_Click" />
            </div>
        </div>
    </div>
</asp:Content>

