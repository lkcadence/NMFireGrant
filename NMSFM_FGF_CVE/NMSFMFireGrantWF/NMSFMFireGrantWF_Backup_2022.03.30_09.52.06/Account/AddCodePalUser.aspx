<%@ Page Title="Fire Grant: Add CodePal User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCodePalUser.aspx.cs" Inherits="NMSFMFireGrantWF.Account.AddCodePalUser"  Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type = "text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add Existing Codepal User</h2>
    <div class="container">
        <asp:UpdatePanel ID="upnlUserSelect" runat="server">
            <ContentTemplate>
                <div class="row" id="dvError" runat="server">
                    
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <fieldset>
                            <legend>
                                Select from Existing CodePal Inspectors (admin only) or Existing Parties (applicants) that have been associated with Fire Departments
                            </legend>
                            <asp:RadioButton ID="rbInspectors" runat="server" Text="Inspectors" GroupName="Users" Checked="true" AutoPostBack="true" OnCheckedChanged="rbInspectors_CheckedChanged"/>
                            <asp:RadioButton ID="rbParties" runat="server" Text="Parties" GroupName="Users" AutoPostBack="true" OnCheckedChanged="rbInspectors_CheckedChanged"/>
                        </fieldset>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblSelectUser" runat="server" Text="Select Inspector" AssociatedControlID="ddlUsers"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="container" id="dvDepartments" runat="server" visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset aria-required="true">
                                <legend class="inlineLegend">Select Departments<span aria-hidden='true'> *</span></legend>
                                <asp:RadioButton ID="rbAssociatedDepartments" runat="server" GroupName="Depts" Text="Associated Departments" Checked="true" AutoPostBack="true" OnCheckedChanged="rbAssociatedDepartments_CheckedChanged"/>&nbsp;
                                <asp:RadioButton ID="rbAllDepartments" runat="server" GroupName="Depts" Text="All Departments" AutoPostBack="true" OnCheckedChanged="rbAssociatedDepartments_CheckedChanged"/>
                            </fieldset>
                        </div>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-md-2">
                            <asp:Label id="lblDepartment" runat="server" Text="Select Department" AssociatedControlID="ddlDepartment"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>--%>
                    <div class="row">
                        <div class="col-md-12">
                            <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnPageIndexChanged="rgDepartments_PageIndexChanged" OnItemDataBound="rgDepartments_ItemDataBound" AllowMultiRowSelection="true">
                                <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                                <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="true">
                                </ClientSettings>
                                <MasterTableView>
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelect"></telerik:GridClientSelectColumn>
                                        <telerik:GridBoundColumn DataField="AddressCode" FilterControlAltText="Filter Name column" HeaderText="Department" UniqueName="Department">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="RoleType" FilterControlAltText="Filter Role column" HeaderText="Role" UniqueName="Role">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="City" FilterControlAltText="Filter City column" HeaderText="City" UniqueName="City">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AddressId" FilterControlAltText="Filter AddressId column" HeaderText="User Id" UniqueName="AddressId" Display="False" Resizable="False">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lbllUserName" runat="server" Text="Username" AssociatedControlID="txtUsername"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtUsername" runat="server" CssClass="form-control"></asp:Textbox>
                    </div>
                </div>             
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                    </div>
                </div>
                <div class="row formRow" id="dvGrantAdmin" runat="server">
                    <div class="col-md-12">
                        <asp:Checkbox id="chkGrantAdmin" runat="server" Text="Grant Counsel Access Only (No Program Admin Rights)" /> 
                        <asp:Checkbox id="chkReadOnly" runat="server" Text="Read Only (User cannot update application data)" />
                    </div>
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnBack_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>
