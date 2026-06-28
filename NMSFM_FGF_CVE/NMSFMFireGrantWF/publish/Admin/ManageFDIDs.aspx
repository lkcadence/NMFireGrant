<%@ Page Title="Fire Grant: Manage FDIDs / Related Addresses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageFDIDs.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageFDIDs" Async="true" %>

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

            margin: 30px auto;

        }

        #fdidModalHeader {

            cursor: move;

            user-select: none;

        }

        #fdidModalHeader .close {

            cursor: pointer;

        }

        #dvFDIDModalError:empty {

            display: none;

        }

        #dvFDIDModalError .alert {

            margin-bottom: 15px;

        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <h2><span id="spHeader" runat="server">Manage FDIDs / Related Addresses</span></h2>

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

                    <div id="dvFDIDModalError" runat="server" ClientIDMode="Static" class="row"></div>

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

                    <div class="row" id="dvAddressSyncSection" runat="server">

                        <div class="col-sm-12">

                            <h5>Fire department address</h5>

                            <p class="help-block">
                                Link an existing Codepal fire department address or create a new one.
                                Use Full Address to distinguish departments with the same name.
                            </p>

                        </div>

                        <div class="col-sm-12">

                            <asp:RadioButton ID="rbAddressLink" runat="server" ClientIDMode="Static"
                                GroupName="AddressAction" Text="Link existing address" Checked="true"
                                onclick="fdidSetAddressAction('link'); return true;" />

                            <asp:RadioButton ID="rbAddressCreate" runat="server" ClientIDMode="Static"
                                GroupName="AddressAction" Text="Create / Edit Address"
                                onclick="fdidSetAddressAction('create'); return true;" />

                        </div>

                    </div>

                    <div id="dvAddressLinkPanel" class="row formRow" runat="server" ClientIDMode="Static">

                        <div class="col-sm-3">

                            <asp:Label ID="lblAddressLink" runat="server"
                                AssociatedControlID="ddlAddressLink" Text="Link to address:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlAddressLink" runat="server" ClientIDMode="Static"
                                CssClass="form-control" onchange="fdidAddressLinkChanged(this);" />

                        </div>

                    </div>

                    <div id="dvAddressCreatePanel" class="row formRow" style="display:none;" runat="server" ClientIDMode="Static">

                        <div class="col-sm-12"><strong>Address (create or edit — required for invoices and legal documents)</strong></div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateAddressType" runat="server"
                                AssociatedControlID="ddlCreateAddressType" Text="Address type: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlCreateAddressType" runat="server" ClientIDMode="Static"
                                CssClass="form-control" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateAddressNumber" runat="server"
                                AssociatedControlID="txtCreateAddressNumber" Text="Street number:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:TextBox ID="txtCreateAddressNumber" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="50" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateDirection" runat="server"
                                AssociatedControlID="ddlCreateDirection" Text="Direction:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlCreateDirection" runat="server" CssClass="form-control" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateAddress" runat="server"
                                AssociatedControlID="txtCreateAddress" Text="Street name:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:TextBox ID="txtCreateAddress" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="50" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateSuffix" runat="server"
                                AssociatedControlID="ddlCreateSuffix" Text="Suffix:"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlCreateSuffix" runat="server" CssClass="form-control" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateCity" runat="server"
                                AssociatedControlID="txtCreateCity" Text="City: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:TextBox ID="txtCreateCity" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="50" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateState" runat="server"
                                AssociatedControlID="ddlCreateState" Text="State: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlCreateState" runat="server" CssClass="form-control" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateCounty" runat="server"
                                AssociatedControlID="ddlCreateCounty" Text="County: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:DropDownList ID="ddlCreateCounty" runat="server" CssClass="form-control" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblCreateZip" runat="server"
                                AssociatedControlID="txtCreateZip" Text="Zip: *"></asp:Label>

                        </div>

                        <div class="col-sm-9">

                            <asp:TextBox ID="txtCreateZip" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="15" placeholder="e.g. 88101" />

                        </div>

                    </div>

                    <div id="dvDepartmentUdfSection" class="row formRow" runat="server" ClientIDMode="Static">

                        <div class="col-sm-12"><strong>Department information (Codepal UDFs)</strong></div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblDeptIso" runat="server"
                                AssociatedControlID="txtDeptIso" Text="ISO Rating:"></asp:Label>

                        </div>

                        <div class="col-sm-3">

                            <asp:TextBox ID="txtDeptIso" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="10" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblMainStations" runat="server"
                                AssociatedControlID="txtMainStations" Text="Main Stations:"></asp:Label>

                        </div>

                        <div class="col-sm-3">

                            <asp:TextBox ID="txtMainStations" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="10" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblSubStations" runat="server"
                                AssociatedControlID="txtSubStations" Text="Substations:"></asp:Label>

                        </div>

                        <div class="col-sm-3">

                            <asp:TextBox ID="txtSubStations" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="10" />

                        </div>

                        <div class="col-sm-3">

                            <asp:Label ID="lblAdminBldgs" runat="server"
                                AssociatedControlID="txtAdminBldgs" Text="Admin Buildings:"></asp:Label>

                        </div>

                        <div class="col-sm-3">

                            <asp:TextBox ID="txtAdminBldgs" runat="server" ClientIDMode="Static"
                                CssClass="form-control" MaxLength="10" />

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

                    <asp:HiddenField ID="hfAddressAction" runat="server" ClientIDMode="Static" Value="link" />

                    <asp:HiddenField ID="hfAddressId" runat="server" ClientIDMode="Static" Value="" />

                    <asp:HiddenField ID="hfPriorAddressCode" runat="server" ClientIDMode="Static" Value="" />

                    <asp:HiddenField ID="hfFDID" runat="server" ClientIDMode="Static" Value="" />

                    <asp:Button ID="btnLoadAddressMatches" runat="server" Text="Load Matches"
                        OnClick="btnLoadAddressMatches_Click" CausesValidation="false"
                        Style="display:none;" />

                    <asp:Button ID="btnLoadAddressForEdit" runat="server" Text="Load Address For Edit"
                        OnClick="btnLoadAddressForEdit_Click" CausesValidation="false"
                        Style="display:none;" />

                </div>

                <div class="modal-footer">

                    <button id="btnSaveFDID" type="button" class="btn btn-primary" runat="server" onserverclick="btnSaveFDID_Click">Save NERIS ID</button>

                    <button type="button" class="btn btn-primary" onclick="fdidHideModal(); fdidClearForm(); return false;">Close</button>

                </div>

            </div>

        </div>

    </div>



    <script type="text/javascript">

        function fdidResetModalPosition() {

            var modal = document.getElementById('fdidModal');

            var dialog = modal ? modal.querySelector('.modal-dialog') : null;

            if (!dialog) {

                return;

            }

            dialog.style.position = '';

            dialog.style.margin = '';

            dialog.style.left = '';

            dialog.style.top = '';

            dialog.style.transform = '';

        }



        function fdidClearModalError() {

            var errorDiv = document.getElementById('dvFDIDModalError');

            if (errorDiv) {

                errorDiv.innerHTML = '';

            }

        }



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

            fdidResetModalPosition();

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



        function fdidSetAddressAction(action) {

            var hfAction = document.getElementById('hfAddressAction');

            if (hfAction) {

                hfAction.value = action || 'link';

            }

            var linkPanel = document.getElementById('dvAddressLinkPanel');

            var createPanel = document.getElementById('dvAddressCreatePanel');

            if (!linkPanel || !createPanel) {

                return;

            }

            var isCreate = action === 'create';

            linkPanel.style.display = isCreate ? 'none' : '';

            createPanel.style.display = isCreate ? '' : 'none';

        }



        function fdidAddressLinkChanged(select) {

            if (select && select.value === '__CREATE__') {

                fdidSetAddressAction('create');

                var rbCreate = document.getElementById('rbAddressCreate');

                if (rbCreate) {

                    rbCreate.checked = true;

                }

                var hfAddressId = document.getElementById('hfAddressId');

                if (hfAddressId) {

                    hfAddressId.value = '';

                }

                var physicalFields = [
                    'txtCreateAddressNumber',
                    'txtCreateAddress',
                    'txtCreateCity',
                    'txtCreateZip'
                ];

                for (var i = 0; i < physicalFields.length; i++) {

                    var field = document.getElementById(physicalFields[i]);

                    if (field) {

                        field.value = '';

                    }

                }

                return;

            }

            if (select && select.value && select.value !== '') {

                __doPostBack('<%= btnLoadAddressForEdit.UniqueID %>', '');

            }

        }



        function fdidClearAddressFields() {

            fdidSetAddressAction('link');

            var rbLink = document.getElementById('rbAddressLink');

            if (rbLink) {

                rbLink.checked = true;

            }

            var ddl = document.getElementById('ddlAddressLink');

            if (ddl) {

                ddl.selectedIndex = 0;

            }

            var hfAddressId = document.getElementById('hfAddressId');

            if (hfAddressId) {

                hfAddressId.value = '';

            }

            var hfPrior = document.getElementById('hfPriorAddressCode');

            if (hfPrior) {

                hfPrior.value = '';

            }

            var createFields = [
                'txtCreateAddressNumber',
                'txtCreateAddress',
                'txtCreateCity',
                'txtCreateZip',
                'txtDeptIso',
                'txtMainStations',
                'txtSubStations',
                'txtAdminBldgs'
            ];

            for (var i = 0; i < createFields.length; i++) {

                var field = document.getElementById(createFields[i]);

                if (field) {

                    field.value = '';

                }

            }

            var ddlIds = ['ddlCreateAddressType', 'ddlCreateDirection', 'ddlCreateSuffix', 'ddlCreateState', 'ddlCreateCounty'];

            for (var j = 0; j < ddlIds.length; j++) {

                var ddlField = document.getElementById(ddlIds[j]);

                if (ddlField) {

                    ddlField.selectedIndex = 0;

                }

            }

            var zipField = document.getElementById('txtCreateZip');

            if (zipField) {

                zipField.value = '';

            }

        }



        function fdidClearForm() {

            document.getElementById('hfFDID').value = '';

            document.getElementById('txtFDID').value = '';

            document.getElementById('txtDepartmentName').value = '';

            document.getElementById('chkFDIDInactive').checked = false;

            fdidClearAddressFields();

            fdidClearModalError();

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

            fdidClearAddressFields();

            __doPostBack('<%= btnLoadAddressMatches.UniqueID %>', '');

            return false;

        }



        function openFDIDModal() {

            fdidShowModal();

        }



        (function () {

            var field = document.getElementById('txtFDID');

            if (field) {

                field.addEventListener('input', fdidApplyNerisIdNormalization);

                field.addEventListener('blur', fdidApplyNerisIdNormalization);

            }

            fdidInitModalDrag();

        })();



        function fdidInitModalDrag() {

            var modal = document.getElementById('fdidModal');

            var header = document.getElementById('fdidModalHeader');

            if (!modal || !header) {

                return;

            }

            var dialog = modal.querySelector('.modal-dialog');

            if (!dialog) {

                return;

            }

            var dragging = false;

            var startX = 0;

            var startY = 0;

            var startLeft = 0;

            var startTop = 0;



            header.addEventListener('mousedown', function (e) {

                if (e.button !== 0) {

                    return;

                }

                if (e.target && e.target.closest && e.target.closest('.close')) {

                    return;

                }

                dragging = true;

                var rect = dialog.getBoundingClientRect();

                startX = e.clientX;

                startY = e.clientY;

                startLeft = rect.left;

                startTop = rect.top;

                dialog.style.position = 'fixed';

                dialog.style.margin = '0';

                dialog.style.left = startLeft + 'px';

                dialog.style.top = startTop + 'px';

                dialog.style.transform = 'none';

                e.preventDefault();

            });



            document.addEventListener('mousemove', function (e) {

                if (!dragging) {

                    return;

                }

                dialog.style.left = (startLeft + e.clientX - startX) + 'px';

                dialog.style.top = (startTop + e.clientY - startY) + 'px';

            });



            document.addEventListener('mouseup', function () {

                dragging = false;

            });

        }

    </script>

</asp:Content>

