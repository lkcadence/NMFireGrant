<%@ Page Title="Fire Grant: Add New User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewUser.aspx.cs" Inherits="NMSFMFireGrantWF.Account.AddNewUser" EnableEventValidation="false" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type = "text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New User</h2>
    <div class="container">
        <asp:UpdatePanel ID="upnlUserSelect" runat="server">
            <ContentTemplate>
                <div class="row" id="dvError" runat="server">
                    
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <fieldset>
                            <legend>
                                Add New Program Admin or Fire Department Applicant
                            </legend>
                            <asp:RadioButton ID="rbInspectors" runat="server" Text="Program Admin/Fire Grant Counsel" GroupName="Users" Checked="true" AutoPostBack="true" OnCheckedChanged="rbInspectors_CheckedChanged"/><br />
                            <asp:RadioButton ID="rbParties" runat="server" Text="Fire Department Applicant" GroupName="Users" AutoPostBack="true" OnCheckedChanged="rbInspectors_CheckedChanged"/>
                        </fieldset>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblFirstName" runat="server" Text="First Name" AssociatedControlID="txtFirstName"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtFirstName" runat="server" CssClass="form-control" CausesValidation="true"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblEmail" runat="server" Text="Email" AssociatedControlID="txtEmail"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" CausesValidation="true"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email Address is Required"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row formRow">
                    <div class="col-md-2">
                        <asp:Label id="lblPhone" runat="server" Text="Phone Number" AssociatedControlID="txtPhone"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Textbox ID="txtPhone" runat="server" CssClass="form-control" TextMode="Phone" CausesValidation="true"></asp:Textbox>
                    </div>
                </div>
                
                <div class="container" id="dvDepartments" runat="server" visible="false">
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
                    <div class="row">
                        <div class="col-md-12">
                            <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnPageIndexChanged="rgDepartments_PageIndexChanged" OnItemDataBound="rgDepartments_ItemDataBound" OnItemCommand="rgDepartments_ItemCommand">
                                <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                                <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false">
                                </ClientSettings>
                                <MasterTableView ClientDataKeyNames="AddressId">
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
                    <div class="col-md-4">
                        <asp:Textbox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:Textbox>
                        <asp:RequiredFieldValidator ID="rfPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is Required"></asp:RequiredFieldValidator>
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
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"/>&nbsp;<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnBack_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
    
</asp:Content>
