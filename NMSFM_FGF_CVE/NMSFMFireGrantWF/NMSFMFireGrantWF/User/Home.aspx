<%@ Page Title="Fire Grant: Admin Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="NMSFMFireGrantWF.User.Home" Async="true" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlNoAccess" runat="server">
        The grant application is not avaliable
    </asp:Panel>
    <asp:Panel ID="pnlUserHome" runat="server">
        <h2 id="hPageHeader" runat="server">User Home</h2>
        <div class="row">
            <div class="col-md-2">
                <asp:Label ID="lblFiscalYear" runat="server" Text="Select Fiscal Year" AssociatedControlID="ddlFiscalYear"></asp:Label>
            </div>
            <div clss="col-md-4">
                <telerik:RadDropDownList ID="ddlFiscalYear" runat="server" OnSelectedIndexChanged="ddlFiscalYear_SelectedIndexChanged" AutoPostBack="true"></telerik:RadDropDownList>
            </div>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-md-12">
                Please select the department application that you would like to start/edit.
            </div>
        </div>
        <div class="container" id="dvDepartments">
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnPageIndexChanged="rgDepartments_PageIndexChanged" OnItemDataBound="rgDepartments_ItemDataBound" OnItemCommand="rgDepartments_ItemCommand" AllowMultiRowSelection="false">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridButtonColumn ButtonType="LinkButton" ConfirmDialogType="RadWindow" FilterControlAltText="Filter Edit column" HeaderText="Edit" Text="Edit/Start" UniqueName="Edit" CommandName="View">
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="AddressCode" FilterControlAltText="Filter Name column" HeaderText="Department" UniqueName="Department">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="County" FilterControlAltText="Filter County column" HeaderText="County" UniqueName="County">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationNumber" FilterControlAltText="Filter Confirmation Number column" HeaderText="Application Number" UniqueName="ConfirmationNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DateSubmitted" FilterControlAltText="Filter Date Submitted column" HeaderText="Date Submitted" UniqueName="DateSubmitted">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationStatus" FilterControlAltText="Filter Application Status column" HeaderText="Application Status" UniqueName="ApplicationStatus">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastStatusChange" FilterControlAltText="Filter Status Change column" HeaderText="Last Change" UniqueName="LastStatusChange">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AddressId" FilterControlAltText="Filter AddressId column" HeaderText="User Id" UniqueName="AddressId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationId" FilterControlAltText="Filter ApplicationId column" HeaderText="ApplicationId" UniqueName="ApplicationId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
