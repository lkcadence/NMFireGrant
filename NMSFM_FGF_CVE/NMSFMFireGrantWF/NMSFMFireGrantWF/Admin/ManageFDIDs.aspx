<%@ Page Title="Fire Grant: Manage NERIS IDs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageFDIDs.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageFDIDs" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">

        #fdidModal {

            z-index: 10050 !important;

        }

        #fdidModal.fdid-modal-visible {

            display: block !important;

            opacity: 1;

        }

        #fdidModal.fdid-modal-visible .modal-dialog {

            transform: translate(0, 0);

        }

    </style>

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



        <div class="row formRow">

            <div class="col-md-12">

                <h4>Search and Filter</h4>

            </div>

        </div>

        <div class="row formRow" id="dvFdidFilters">

            <div class="col-md-2">

                <asp:Label ID="lblSearchNerisId" runat="server" Text="Search NERIS ID" AssociatedControlID="txtSearchNerisId"></asp:Label>

            </div>

            <div class="col-md-3">

                <asp:TextBox ID="txtSearchNerisId" runat="server" CssClass="form-control" MaxLength="20" placeholder="NERIS ID"></asp:TextBox>

            </div>

            <div class="col-md-2">

                <asp:Label ID="lblSearchFireDepartment" runat="server" Text="Search Fire Department" AssociatedControlID="txtSearchFireDepartment"></asp:Label>

            </div>

            <div class="col-md-3">

                <asp:TextBox ID="txtSearchFireDepartment" runat="server" CssClass="form-control" MaxLength="50" placeholder="Fire Department"></asp:TextBox>

            </div>

        </div>

        <div class="row formRow">

            <div class="col-md-4">

                <asp:CheckBox ID="chkHideInactive" runat="server" Text="Hide inactive departments" Checked="true" />

            </div>

            <div class="col-md-2">

                <asp:Button ID="btnApplyFilters" CssClass="btn btn-primary" runat="server" Text="Apply" CausesValidation="false" />

            </div>

            <div class="col-md-2">

                <asp:Button ID="btnClearFilters" CssClass="btn btn-default" runat="server" Text="Clear" CausesValidation="false" />

            </div>

        </div>

        <div class="row">&nbsp;</div>



        <div class="row">

            <div class="col-md-12">

                <telerik:RadGrid ID="rgFDIDs" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" AllowSorting="True" PageSize="25" OnNeedDataSource="rgFDIDs_NeedDataSource" OnPageIndexChanged="rgFDIDs_PageIndexChanged" OnItemDataBound="rgFDIDs_ItemDataBound">

                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>

                    <ClientSettings AllowKeyboardNavigation="True">

                    </ClientSettings>

                    <MasterTableView>

                        <Columns>

                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">

                                <ItemTemplate>

                                    <asp:HyperLink ID="editLink" runat="server" NavigateUrl="javascript:void(0);" Text="View/Edit" CssClass="fdid-edit-link" />

                                </ItemTemplate>

                            </telerik:GridTemplateColumn>

                            <telerik:GridBoundColumn DataField="FDID" FilterControlAltText="Filter NERIS ID column" HeaderText="NERIS ID" UniqueName="FDID" SortExpression="FDID" Resizable="False">

                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="FireDepartment" FilterControlAltText="Filter Fire Department column" HeaderText="Fire Department" UniqueName="FireDepartment" SortExpression="FireDepartment">

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

                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="fdidClearForm(); fdidShowModal(); return false;">

                    Add New NERIS ID

                </button>

            </div>

            <div class="col-md-2">

                <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnClose_Click" />

            </div>

        </div>

    </div>



    <div class="modal fade" id="fdidModal" tabindex="-1" role="dialog" aria-labelledby="lblFDIDHeader" aria-hidden="true">

        <div class="modal-dialog" role="document">

            <div class="modal-content">

                <div class="modal-header" id="fdidModalHeader">

                    <h4 class="modal-title" id="lblFDIDHeader">Fire Department ID</h4>

                    <button type="button" class="close" onclick="fdidHideModal(); return false;" aria-label="Close">

                        <span aria-hidden="true">&times;</span>

                    </button>

                </div>

                <div class="modal-body">

                    <div class="row">

                        <asp:Label ID="lblFDIDError" runat="server"></asp:Label>

                    </div>

                    <div class="row">

                        <div class="col-sm-3">

                            <asp:Label ID="lblFDID" runat="server" AssociatedControlID="txtFDID" Text="NERIS ID: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <%-- Legacy (pre-NERIS 20-char): MaxLength="5", Width="100px"
                            <asp:TextBox ID="txtFDID" runat="server" Width="100px" ClientIDMode="Static" MaxLength="5" aria-required="true"></asp:TextBox>
                            --%>
                            <asp:TextBox ID="txtFDID" runat="server" Width="180px" ClientIDMode="Static" MaxLength="20" aria-required="true"></asp:TextBox>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-sm-3">

                            <asp:Label ID="lblDepartmentName" runat="server" AssociatedControlID="txtDepartmentName" Text="Department Name: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:TextBox ID="txtDepartmentName" runat="server" Width="250px" ClientIDMode="Static" MaxLength="50" aria-required="true" ValidateRequestMode="Disabled"></asp:TextBox>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-sm-3">

                            <asp:Label ID="lblFDIDInactive" runat="server" AssociatedControlID="chkFDIDInactive" Text="Inactive:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:Checkbox ID="chkFDIDInactive" runat="server" ClientIDMode="Static" />

                        </div>

                    </div>

                    <asp:HiddenField ID="hfFDID" runat="server" ClientIDMode="Static" Value="" />

                </div>

                <div class="modal-footer">

                    <button id="btnSaveFDID" type="button" class="btn btn-primary" runat="server" onserverclick="btnSaveFDID_Click">Save NERIS ID</button>

                    <button type="button" class="btn btn-primary" onclick="fdidHideModal(); fdidClearForm(); return false;">Close</button>

                </div>

            </div>

        </div>

    </div>



    <script type="text/javascript">

        function fdidShowModal() {

            var modal = document.getElementById('fdidModal');

            if (!modal) {

                return;

            }

            modal.style.display = 'block';

            modal.classList.add('in', 'fdid-modal-visible');

            modal.setAttribute('aria-hidden', 'false');

            document.body.classList.add('modal-open');

        }



        function fdidHideModal() {

            var modal = document.getElementById('fdidModal');

            if (!modal) {

                return;

            }

            modal.style.display = 'none';

            modal.classList.remove('in', 'fdid-modal-visible');

            modal.setAttribute('aria-hidden', 'true');

            document.body.classList.remove('modal-open');

        }



        function fdidNormalizeNerisId(value) {

            return (value || '').trim().toUpperCase();

        }



        function fdidApplyNerisIdNormalization() {

            var field = document.getElementById('txtFDID');

            if (!field) {

                return;

            }

            field.value = fdidNormalizeNerisId(field.value);

        }



        function fdidClearForm() {

            document.getElementById('hfFDID').value = '';

            document.getElementById('txtFDID').value = '';

            document.getElementById('txtDepartmentName').value = '';

            document.getElementById('chkFDIDInactive').checked = false;

        }



        function fdidDecodeAttribute(value) {

            if (!value) {

                return '';

            }

            var el = document.createElement('textarea');

            el.innerHTML = value;

            return el.value;

        }



        function fdidOpenForEdit(link) {

            if (!link) {

                return false;

            }

            document.getElementById('hfFDID').value =
                fdidNormalizeNerisId(link.getAttribute('data-fdid') || '');

            document.getElementById('txtFDID').value =
                fdidNormalizeNerisId(link.getAttribute('data-fdid') || '');

            document.getElementById('txtDepartmentName').value =
                fdidDecodeAttribute(link.getAttribute('data-dept') || '');

            document.getElementById('chkFDIDInactive').checked =

                (link.getAttribute('data-inactive') || '').toLowerCase() === 'true';

            fdidShowModal();

            return false;

        }



        function openFDIDModal() {

            fdidShowModal();

        }



        (function () {

            var field = document.getElementById('txtFDID');

            if (!field) {

                return;

            }

            field.addEventListener('input', fdidApplyNerisIdNormalization);

            field.addEventListener('blur', fdidApplyNerisIdNormalization);

        })();

    </script>

</asp:Content>

