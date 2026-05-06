<%@ Page Title="Fire Grant: Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="NMSFMFireGrantWF.Account.EditUser" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type = "text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit User</h2>
    <div class="container">
        <asp:UpdatePanel ID="upnlUserSelect" runat="server">
            <ContentTemplate>
                <div class="row" id="dvError" runat="server">
                    
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <fieldset>
                            <legend>
                                Program Admin or Fire Department Applicant
                            </legend>
                            <asp:RadioButton ID="rbInspectors" runat="server" Text="Program Admin/Fire Grant Counsel" GroupName="Users" Enabled="false"/><br />
                            <asp:RadioButton ID="rbParties" runat="server" Text="Fire Department Applicant" GroupName="Users" Enabled="false"/>
                        </fieldset>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblFirstName" runat="server" Text="First Name" AssociatedControlID="txtFirstName"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtFirstName" runat="server" CssClass="form-control" CausesValidation="true" ReadOnly="True"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblEmail" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" CausesValidation="true" ReadOnly="True"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email Address is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblPhone" runat="server" Text="Phone Number" AssociatedControlID="txtPhone"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtPhone" runat="server" CssClass="form-control" TextMode="Phone" CausesValidation="true" ReadOnly="True"></asp:Textbox>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-6">
                        <asp:CheckBox ID="chkInactive" runat="server" Text="Inactive" />
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
                                    <ClientEvents OnRowCreated="rowCreated" OnRowSelected="rowSelected"
                                    OnRowDeselected="rowDeselected" OnGridCreated="gridCreated" />
                                </ClientSettings>
                                <MasterTableView ClientDataKeyNames="AddressId">
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
                        <asp:Textbox ID="txtUsername" runat="server" CssClass="form-control" CausesValidation="true"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfUserName" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is Required"></asp:RequiredFieldValidator>

                    </div>
                </div>             
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="txtPassword"></asp:Label>
                    </div>
                    <div class="col-md-8">
                        <asp:Textbox ID="txtPassword" runat="server" CssClass="form-control"></asp:Textbox> (To Update Password Enter New Password)
                    </div>
                </div>
                <div class="row formRow" id="dvGrantAdmin" runat="server">
                    <div class="col-md-12">
                        <asp:Checkbox id="chkGrantAdmin" runat="server" Text="Grant Counsel Access Only (No Program Admin Rights)" /> 
                        <asp:Checkbox id="chkReadOnly" runat="server" Text="Read Only (User cannot update application data)" />
                    </div>
                </div>
                
                <asp:HiddenField ID="hfCodepalId" runat="server" />
                <asp:HiddenField ID="hfUserId" runat="server" />
            </ContentTemplate>   
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btnSave_Click" />&nbsp;<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnBack_Click" CausesValidation="false"/>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            sessionStorage.clear();
        });

        function rowSelected(sender, args) {
            var id = args.getDataKeyValue("AddressId");

            var selected = (JSON.parse(sessionStorage.getItem("selectedItems")) != null) ? JSON.parse(sessionStorage.getItem("selectedItems")) : {};

            if (!selected[id]) {
                selected[id] = true;

                sessionStorage.setItem("selectedItems", JSON.stringify(selected));
            }
        }

        function rowDeselected(sender, args) {
            var id = args.getDataKeyValue("AddressId");
            var selected = JSON.parse(sessionStorage.getItem("selectedItems"));

            if (selected[id]) {
                selected[id] = null;

                sessionStorage.setItem("selectedItems", JSON.stringify(selected));
            }
        }

        function rowCreated(sender, args) {
            var id = args.getDataKeyValue("AddressId");
            var selected = JSON.parse(sessionStorage.getItem("selectedItems"));

            if (selected && selected[id]) {
                args.get_gridDataItem().set_selected(true);
            }
        }

        function gridCreated(sender, eventArgs) {
            var masterTable = sender.get_masterTableView();
            var selectColumn = masterTable.getColumnByUniqueName("ClientSelect");
            var headerCheckBox = $(selectColumn.get_element()).find("[type=checkbox]")[0];

            if (headerCheckBox) {
                headerCheckBox.checked = masterTable.get_selectedItems().length ==
                    masterTable.get_dataItems().length;
            }
        }
    </script>
</asp:Content>
