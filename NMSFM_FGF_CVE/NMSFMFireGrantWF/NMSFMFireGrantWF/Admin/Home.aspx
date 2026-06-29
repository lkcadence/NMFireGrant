<%@ Page Title="Fire Grant: Admin Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.Home" async="true"%>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function updateDeleteButtonState() {
            var btn = document.getElementById('<%= btnDelete.ClientID %>');
            if (!btn) return;
            var anyChecked = false;
            var boxes = document.querySelectorAll('.app-delete-chk input[type=checkbox]');
            for (var i = 0; i < boxes.length; i++) {
                if (boxes[i].checked) {
                    anyChecked = true;
                    break;
                }
            }
            btn.disabled = !anyChecked;
        }

        function wireDeleteConfirm() {
            var btn = document.getElementById('<%= btnDelete.ClientID %>');
            if (!btn) return;

            var existingOnclick = btn.getAttribute('onclick') || '';
            if (existingOnclick.indexOf('confirm(') >= 0) {
                return;
            }

            btn.setAttribute('onclick',
                "if (this.disabled) return false; if (!confirm('Are you sure you want to permanently delete the selected application(s) and all associated data?')) return false; " +
                existingOnclick);
        }

        function initDeleteCheckboxes() {
            wireDeleteConfirm();
            var boxes = document.querySelectorAll('.app-delete-chk input[type=checkbox]');
            for (var i = 0; i < boxes.length; i++) {
                boxes[i].removeEventListener('change', updateDeleteButtonState);
                boxes[i].addEventListener('change', updateDeleteButtonState);
            }
            updateDeleteButtonState();
        }

        if (typeof Sys !== 'undefined' && Sys.Application) {
            Sys.Application.add_load(initDeleteCheckboxes);
        } else {
            document.addEventListener('DOMContentLoaded', initDeleteCheckboxes);
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Applications for Review / Approval for FY<span id="spFiscalYear" runat="server"></span></h2>
        <div class="container" id="dvDepartments">
            <div class="row"><hr /></div>
            <div id="dvSearch">
                <h3>Search</h3>
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
                    <div class="col-md-3">
                        <telerik:RadTextBox ID="txtConfNumber" runat="server" EmptyMessage="Confirmation Number" Width="75%"></telerik:RadTextBox>
                        <span style="display:none"><asp:Label ID="lblConfNumber" runat="server" AssociatedControlID="txtConfNumber" Text="Confirmation Number"></asp:Label></span>
                    </div>
                    <div class="col-md-3">
                        <telerik:RadTextBox ID="txtDepartment" runat="server" EmptyMessage="Department Name" Width="75%"></telerik:RadTextBox>
                        <span style="display:none"><asp:Label ID="lblDepartment" runat="server" AssociatedControlID="txtDepartment" Text="Department"></asp:Label></span>
                    </div>
                    <div class="col-md-3">
                        <telerik:RadTextBox ID="txtCounty" runat="server" EmptyMessage="County" Width="75%"></telerik:RadTextBox>
                        <span style="display:none"><asp:Label ID="lblCounty" runat="server" AssociatedControlID="txtCounty" Text="County"></asp:Label></span>
                    </div>
                </div>
                <div class="row">&nbsp;</div>
                <div class="row">
                    <div class="col-md-3">
                        Date Range:
                    </div>
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="rdpStartDate" runat="server"></telerik:RadDatePicker>
                        <span style="display:none"><asp:Label ID="lblStartDate" runat="server" AssociatedControlID="rdpStartDate" Text="Start Date"></asp:Label></span>
                    </div>
                    <div class="col-md-1">
                        To
                    </div>
                    <div class="col-md-3">
                        <%--<asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>--%>
                        <telerik:RadDatePicker ID="rdpEndDate" runat="server"></telerik:RadDatePicker>
                        <span style="display:none"><asp:Label ID="lblEndDate" runat="server" AssociatedControlID="rdpEndDate" Text="End Date"></asp:Label></span>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click"/>
                        <asp:Button ID="btnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to permanently delete the selected application(s) and all associated data?');" />
                    </div>
                </div>
            </div>
            <div class="row"><hr /></div>
            <div class="row">
                <div class="col-md-12">
                    Please select the department application that you would like to view.
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" id="dvMessage" runat="server"></div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnPageIndexChanged="rgDepartments_PageIndexChanged" OnPageSizeChanged="rgDepartments_PageSizeChanged" OnItemDataBound="rgDepartments_ItemDataBound" OnItemCommand="rgDepartments_ItemCommand" AllowMultiRowSelection="false">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridButtonColumn ButtonType="LinkButton" ConfirmDialogType="RadWindow" FilterControlAltText="Filter Edit column" HeaderText="Edit" Text="View/Edit" UniqueName="Edit" CommandName="View">
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
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Delete column" HeaderText="Delete" UniqueName="Delete">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete" runat="server" CssClass="app-delete-chk" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
</asp:Content>
