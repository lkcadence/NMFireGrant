<%@ Page Title="Fire Grant: Register Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="NMSFMFireGrantWF.Account.Register" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type = "text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <h2>Register New Account</h2>
    <div class="container">
        <asp:UpdatePanel ID="upnlUserSelect" runat="server">
            <ContentTemplate>
                <div class="row" id="dvError" runat="server">
                    
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h3>Enter User Information<h3>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblFirstName" runat="server" Text="First Name *" AssociatedControlID="txtFirstName"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtFirstName" runat="server" CssClass="form-control" CausesValidation="true" aria-required="true"></asp:Textbox>
                        
                    </div>
                    <div class="col-md-4">
                        <asp:RequiredFieldValidator ID="rfFirstName" runat="server" ControlToValidate="txtFirstName" ForeColor="Red" ErrorMessage="First Name is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblLastName" runat="server" Text="Last Name *" AssociatedControlID="txtLastName"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtLastName" runat="server" CssClass="form-control" CausesValidation="true" aria-required="true"></asp:Textbox>
                        
                    </div>
                    <div class="col-md-4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastName" ForeColor="Red" ErrorMessage="Last Name is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblEmail" runat="server" Text="Email *" AssociatedControlID="txtEmail"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" CausesValidation="true" aria-required="true"></asp:Textbox>
                    </div>
                    <div class="col-md-4">
                        <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Email Address is Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rfEmail2" runat="server" ControlToValidate="txtEmail"
                            ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            Display="Dynamic" ErrorMessage = "Invalid email address"/>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblPhone" runat="server" Text="Phone Number" AssociatedControlID="txtPhone"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <telerik:RadMaskedTextBox ID="txtPhone" runat="server" Mask="(###) ###-####"></telerik:RadMaskedTextBox>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblFDID" runat="server" Text="NFIRS FDID # *" AssociatedControlID="txtFDID"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <telerik:RadNumericTextBox ID="txtFDID" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Type="Number" MaxLength="5" aria-required="true" ClientIDMode="Static" CausesValidation="true"></telerik:RadNumericTextBox>
                        
                    </div>
                    <div class="col-md-4">
                        <asp:RequiredFieldValidator ID="rfFDID" runat="server" ControlToValidate="txtFDID" ForeColor="Red" ErrorMessage="NFIRS FDID # is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h3>Select Associated Departments *</h3>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-3">
                        <asp:Label ID="lblSearchDept" runat="server" Text="Search by Department Name" AssociatedControlID="rcDepartments"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <telerik:RadComboBox ID="rcDepartments" runat="server" AccessibilityMode="true" Filter="Contains" DataTextField="AddressCode" DataValueField="AddressId" Width="300px"></telerik:RadComboBox>
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="lnkSearch" runat="server" Text="Add Selected Department" CausesValidation="false" OnClick="lnkSearch_Click"></asp:LinkButton>
                    </div>
                </div>
                <div class="container" id="dvDepartments" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnPageIndexChanged="rgDepartments_PageIndexChanged" OnItemDataBound="rgDepartments_ItemDataBound" OnPageSizeChanged="rgDepartments_PageSizeChanged" OnItemCommand="rgDepartments_ItemCommand">
                                <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                                <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false">
<%--                                    <ClientEvents OnRowCreated="rowCreated" OnRowSelected="rowSelected"
                                    OnRowDeselected="rowDeselected" OnGridCreated="gridCreated" />--%>
                                </ClientSettings>
                                <MasterTableView ClientDataKeyNames="addressId">
                                    <Columns>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Remove column" HeaderText="Remove" UniqueName="Remove">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" Text="Remove" CausesValidation="false" CommandName="Delete" CommandArgument='<%# Eval("addressId") %>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter Name column" HeaderText="Department" UniqueName="Department">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="RoleType" FilterControlAltText="Filter Role column" HeaderText="Role" UniqueName="Role">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="addressId" FilterControlAltText="Filter AddressId column" HeaderText="User Id" UniqueName="AddressId" Display="False" Resizable="False">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <h3>Enter Account Information<h3>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lbllUserName" runat="server" Text="Username *" AssociatedControlID="txtUsername"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtUsername" runat="server" CssClass="form-control" CausesValidation="true" aria-required="true"></asp:Textbox>
                    </div>
                    <div class="col-md-4">
                        <asp:RequiredFieldValidator ID="rfUserName" runat="server" ControlToValidate="txtUsername" ForeColor="Red" ErrorMessage="Username is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>             
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lblPassword" runat="server" Text="Password *" AssociatedControlID="txtPassword"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" aria-required="true"></asp:Textbox>
                    </div>
                    <div class="col-md-4">
                        
                        <asp:RequiredFieldValidator ID="rfPassword" runat="server" ControlToValidate="txtPassword" ForeColor="Red" ErrorMessage="Password is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password *" AssociatedControlID="txtConfirmPassword"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" CausesValidation="true" aria-required="true"></asp:Textbox>
                        
                    </div>
                    <div class="col-md-6">
                        <asp:RequiredFieldValidator ID="rfConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ForeColor="Red" ErrorMessage="Confirm Password is Required"></asp:RequiredFieldValidator>
                        <asp:CompareValidator runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"
                            ForeColor="Red" ErrorMessage="The password and confirmation password do not match." />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Register" OnClick="btnSave_Click"/>&nbsp;<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnBack_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //$(document).ready(function () {
        //    //sessionStorage.clear();
        //});

        //$(window).unload(function () {
        //    sessionStorage.clear();
        //});

        //function rowSelected(sender, args) {
        //    var id = args.getDataKeyValue("AddressId");

        //    var selected = (JSON.parse(sessionStorage.getItem("selectedItems")) != null) ? JSON.parse(sessionStorage.getItem("selectedItems")) : {};

        //    if (!selected[id]) {
        //        selected[id] = true;

        //        sessionStorage.setItem("selectedItems", JSON.stringify(selected));
        //    }
        //}

        //function rowDeselected(sender, args) {
        //    var id = args.getDataKeyValue("AddressId");
        //    var selected = JSON.parse(sessionStorage.getItem("selectedItems"));

        //    if (selected[id]) {
        //        selected[id] = null;

        //        sessionStorage.setItem("selectedItems", JSON.stringify(selected));
        //    }
        //}

        //function rowCreated(sender, args) {
        //    var id = args.getDataKeyValue("AddressId");
        //    var selected = JSON.parse(sessionStorage.getItem("selectedItems"));

        //    if (selected && selected[id]) {
        //        args.get_gridDataItem().set_selected(true);
        //    }
        //}

        //function gridCreated(sender, eventArgs) {
        //    var masterTable = sender.get_masterTableView();
        //    var selectColumn = masterTable.getColumnByUniqueName("ClientSelect");
        //    var headerCheckBox = $(selectColumn.get_element()).find("[type=checkbox]")[0];

        //    if (headerCheckBox) {
        //        headerCheckBox.checked = masterTable.get_selectedItems().length ==
        //            masterTable.get_dataItems().length;
        //    }
        //}
    </script>
</asp:Content>
