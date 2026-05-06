<%@ Page Title="Fire Grant: Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="NMSFMFireGrantWF.Account.ManageUsers" Async="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Users</h2>
    <div class="container">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row">
            <div class="col-md-12">
                <h3>Manage Existing Users</h3>
            </div>
        </div>
        <div class="row" id="dvSuccess" runat="server">
            
        </div>
        <div class="row">
            <div class="col-md-12">
                <h4>Search Users/Departments</h4>
            </div>
        </div>
        <div class="row" id="dvSearch">
            <div class="col-md-3">
                <telerik:RadTextbox ID="txtSearchDepartment" runat="server" EmptyMessage="Department Name"></telerik:RadTextbox>
            </div>
            <div class="col-md-3">
                <telerik:RadTextbox ID="txtSearchUser" runat="server" EmptyMessage="User Name"></telerik:RadTextbox>
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" CausesValidation="false" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgExistingUsers" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgExistingUsers_NeedDataSource" OnPageIndexChanged="rgExistingUsers_PageIndexChanged" OnItemDataBound="rgExistingUsers_ItemDataBound" OnItemCommand="rgExistingUsers_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridButtonColumn ButtonType="LinkButton" ConfirmDialogType="RadWindow" FilterControlAltText="Filter Edit column" HeaderText="Edit" Text="Edit" UniqueName="Edit" CommandName="View">
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column" HeaderText="Name" UniqueName="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Login" FilterControlAltText="Filter Login column" HeaderText="Login" UniqueName="Login">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Role" FilterControlAltText="Filter Role column" HeaderText="Role" UniqueName="Role">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column" HeaderText="Department" UniqueName="Department">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Inactive" FilterControlAltText="Filter Inactive column" HeaderText="Inactive" UniqueName="Inactive">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserId" FilterControlAltText="Filter UserId column" HeaderText="User Id" UniqueName="UserId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:LinkButton ID="lnkAddCodePalUser" runat="server" CssClass="btn btn-primary form-control" Text="Add CodePal User" OnClick="lnkAddCodePalUser_Click"></asp:LinkButton> <br /><br />
                <asp:LinkButton ID="lnkAddNewUser" runat="server" CssClass="btn btn-primary form-control" Text="Add New User" OnClick="lnkAddNewUser_Click"></asp:LinkButton>
            </div>
        </div>
        
    </div>
</asp:Content>
