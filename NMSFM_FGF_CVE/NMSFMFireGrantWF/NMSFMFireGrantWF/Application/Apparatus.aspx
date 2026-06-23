<%@ Page Title="Fire Grant Application: Apparatus" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="Apparatus.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Apparatus" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <script type="text/javascript">
        window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
    </script>
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblApparatusPart" runat="server" Text="Apparatus is part of the project? *" AssociatedControlID="fldApparatusPart"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldApparatusPart" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbApparatusPartYes" runat="server" Text="Yes" GroupName="ApparatusPart" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbApparatusPartNo" runat="server" Text="No" GroupName="ApparatusPart" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div id="dvApparatusDetails" style="display:none">
        <div class="row formRow" id="dvPumpTestsConducted">
            <div class="col-md-4">
                <asp:Label ID="lblPumpTestsConducted" runat="server" Text="Are pump tests conducted annually on apparatus? *" AssociatedControlID="fldPumpTestsConducted"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldPumpTestsConducted" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbPumpTestsConductedYes" runat="server" Text="Yes" GroupName="PumpTestsConducted" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbPumpTestsConductedNo" runat="server" Text="No" GroupName="PumpTestsConducted" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvNoPumpTestsExp">
            <div class="col-md-4">
                <asp:Label ID="lblNoPumpTestsExp" runat="server" Text="Explain if not tested *" AssociatedControlID="txtNoPumpTestsExp"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtNoPumpTestsExp" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <%--<div class="row" id="dvNotification" runat="server">
            <div class="col-md-12">
                <p>
                    <a data-toggle="collapse" href="#pumptestingcode" role="button" aria-expanded="false" aria-controls="collapseExample">
                        Pump Testing Statute
                    </a>
                </p>
                <div class="collapse" id="pumptestingcode">
                    <div class="card card-body alert-info">
                        <asp:Literal ID="ltrPumpTestStatute" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="row" runat="server">
            <div class="col-md-12">
                <h4>Pump Testing Requirements</h4>
                <asp:Literal ID="ltrPumpTestStatute" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row formRow" id="dvHoseTests">
            <div class="col-md-4">
                <asp:Label ID="lblHoseTests" runat="server" Text="Has your annual hose testing been conducted? *" AssociatedControlID="fldHoseTests"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldHoseTests" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbHoseTestsYes" runat="server" Text="Yes" GroupName="HoseTestsConducted" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbHoseTestsNo" runat="server" Text="No" GroupName="HoseTestsConducted" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvNoHoseTests">
            <div class="col-md-4">
                <asp:Label ID="lblNoHoseTests" runat="server" Text="Explain if hose testing not conducted *" AssociatedControlID="txtNoHoseTests"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtNoHoseTests" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <%--<div class="row" id="Div1" runat="server">
            <div class="col-md-12">
                <p>
                    <a data-toggle="collapse" href="#hosetestingcode" role="button" aria-expanded="false" aria-controls="collapseExample">
                        Hose Testing Statute
                    </a>
                </p>
                <div class="collapse" id="hosetestingcode">
                    <div class="card card-body alert-info">
                        <asp:Literal ID="ltrHoseTestStatute" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="row" runat="server">
            <div class="col-md-12">
                <h4>Hose Testing Requirements</h4>
                <asp:Literal ID="ltrHoseTestStatute" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div id="dvApparatus">
            <div class="row" id="dvListApparatusHead">
                <div class="col-md-12">
                    <h3>LIST ALL APPARATUS *</h3>
                </div>
            </div>
            <div class="row" id="dvAddApparatusButton">
                <div class="col-md-12" id="dvShowModal" runat="server">
                    <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId()" data-toggle="modal" data-target="#apparatusModal">
                        Add Apparatus</button>
                    &nbsp;
                    <asp:Button ID="btnUploadApparatusDocuments" runat="server" CssClass="btn btn-primary"
                        Text="Upload Apparatus Documents"
                        OnClientClick="triggerApparatusFileUpload(); return false;" UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hfUploadAction" runat="server" Value="" />
                    <asp:FileUpload ID="fuApparatusDocumentation" runat="server"
                        Style="position:absolute;left:-9999px;width:1px;height:1px;opacity:0;"
                        accept=".xls,.xlsx,.csv,.pdf,.doc,.docx"
                        onchange="onApparatusFileSelected();" />
                </div>
            </div>
            <div class="row" id="dvApparatusDocumentError" runat="server"></div>
            <div class="row" id="dvApparatusList">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgApparatus" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgApparatus_NeedDataSource" OnPageIndexChanged="rgApparatus_PageIndexChanged" OnItemDataBound="rgApparatus_ItemDataBound" OnItemCommand="rgApparatus_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("ApparatusId") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApparatusName" FilterControlAltText="Filter Apparatus ID column" HeaderText="Apparatus ID" UniqueName="ApparatusName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VehicleType" FilterControlAltText="Filter Vehicle Type column" HeaderText="Vehicle Type" UniqueName="VehicleType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Year" FilterControlAltText="Filter Year column" HeaderText="Year" UniqueName="Year">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VIN" FilterControlAltText="Filter Apparatus VIN column" HeaderText="Vehicle ID #" UniqueName="VIN">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="License" FilterControlAltText="Filter Apparatus License column" HeaderText="License Plate" UniqueName="License">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="Capacity" FilterControlAltText="Filter Capacity column" HeaderText="Capacity (gal)" UniqueName="Capacity">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GPM" FilterControlAltText="Filter GPM column" HeaderText="GPM" UniqueName="GPM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TestDate" FilterControlAltText="Filter Test Date column" DataFormatString="{0:d}" HeaderText="Test Date" UniqueName="TestDate">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Pass" FilterControlAltText="Filter Pass column" HeaderText="Pass/Fail" UniqueName="Pass">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Comments" FilterControlAltText="Filter Comments column" HeaderText="Comments" UniqueName="Comments">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="Documentation" FilterControlAltText="Filter Documentation column" HeaderText="Documentation" UniqueName="Documentation">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="ApparatusId" FilterControlAltText="Filter ApparatusId column" HeaderText="ApparatusId" UniqueName="ApparatusId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="row" id="dvApparatusDocuments" style="margin-bottom: 2em;">
                <div class="col-md-12">
                    <h4>Uploaded Apparatus Documents</h4>
                    <telerik:RadGrid ID="rgApparatusDocuments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgApparatusDocuments_NeedDataSource" OnPageIndexChanged="rgApparatusDocuments_PageIndexChanged" OnItemDataBound="rgApparatusDocuments_ItemDataBound" OnItemCommand="rgApparatusDocuments_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView DataKeyNames="DocumentId">
                            <Columns>
                                <telerik:GridBoundColumn DataField="DocumentType" FilterControlAltText="Filter Document Type column" HeaderText="Document Type" UniqueName="DocumentType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter Document Name column" HeaderText="Document Name" UniqueName="DocumentName">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit Name column" HeaderText="Edit Name" UniqueName="EditName">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditName" runat="server" Text="Edit Name" CommandName="EditName" CommandArgument='<%# Eval("DocumentId") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter View column" HeaderText="View" UniqueName="View">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnView" runat="server" Text="View Document" CommandName="View" CommandArgument='<%# Eval("DocumentId") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Download column" HeaderText="Download" UniqueName="Download">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDownload" runat="server" Text="Download Doc" CommandName="Download" CommandArgument='<%# Eval("DocumentId") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Remove column" HeaderText="Remove" UniqueName="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" CommandName="Delete" CommandArgument='<%# Eval("DocumentId") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="DocumentId" FilterControlAltText="Filter DocumentId column" HeaderText="DocumentId" UniqueName="DocumentId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="row">
                <hr />
            </div>
        </div>
        </div>
            
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Previous" OnClick="btnBack_Click" CausesValidation="false"/>&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;
                <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Next" OnClick="btnNext_Click" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <!-- Document View Modal -->
    <div class="modal fade" id="docModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblDocModalHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="width:fit-content; height:fit-content">
            <div class="modal-content">
                <div class="modal-header" id="modalHeaderDoc">
                    <h4 class="modal-title" id="lblDocModalHeader">View Document</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body" style="background-color:lightgray">
                    <telerik:RadPdfViewer ID="pdfView" runat="server" Width="750px" Height="900px" MaxSerializerLength="20485760">
                    </telerik:RadPdfViewer>
                </div>
            </div>
        </div>
    </div>
    <!-- Edit Document Name Modal -->
    <div class="modal fade" id="editDocumentNameModal" tabindex="-1" role="dialog" data-backdrop="false" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Edit Document Name</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblEditDocumentNameError" runat="server"></asp:Label>
                    <asp:TextBox ID="txtEditDocumentName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    <asp:HiddenField ID="hfEditDocumentId" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveDocumentName" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveDocumentName_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="apparatusModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblApparatusHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblApparatusHeader">Add Apparatus</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblApparatusError" runat="server"></asp:Label>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblNumber" runat="server" AssociatedControlID="txtNumber" Text="Apparatus Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtNumber" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblApparatusName" runat="server" AssociatedControlID="txtApparatusName" Text="Apparatus ID: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtApparatusName" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblVehicleType" runat="server" AssociatedControlID="ddlVehicleType" Text="Vehicle Type: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlVehicleType" runat="server" class="form-control">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Text="Tender" Value="Tender"></asp:ListItem>
                                <asp:ListItem Text="Pumper" Value="Pumper"></asp:ListItem>
                                <asp:ListItem Text="Ladder" Value="Ladder"></asp:ListItem>
                                <asp:ListItem Text="Command" Value="Command"></asp:ListItem>
                                <asp:ListItem Text="Rescue" Value="Rescue"></asp:ListItem>
                                <asp:ListItem Text="Service" Value="Service"></asp:ListItem>
                                <asp:ListItem Text="Wildland" Value="Wildland"></asp:ListItem>
                                <asp:ListItem Text="Hazmat" Value="Hazmat"></asp:ListItem>
                                <asp:ListItem Text="Rehab" Value="Rehab"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblYear" runat="server" AssociatedControlID="txtYear" Text="Vehicle Year *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadNumericTextBox ID="txtYear" runat="server" NumberFormat-DecimalDigits="0" EmptyMessage="0" NumberFormat-GroupSeparator="" MinValue="1950" Type="Number" ClientIDMode="Static" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblVIN" runat="server" AssociatedControlID="txtVIN" Text="Vehicle ID Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtVIN" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblLicense" runat="server" AssociatedControlID="txtLicense" Text="License Plate #: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtLicense" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblCapacity" runat="server" AssociatedControlID="txtCapacity" Text="Capacity (gallons) [0 = N/A] *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadNumericTextBox ID="txtCapacity" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblGPM" runat="server" AssociatedControlID="txtGPM" Text="GPM: (0 = N/A) *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadNumericTextBox ID="txtGPM" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblTestDate" runat="server" ClientIDMode="Static" AssociatedControlID="txtTestDate" Text="Test Date:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTestDate" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Date" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvPassFail">
                        <div class="col-md-5">
                            <asp:Label ID="lblPassFail" runat="server" Text="Pass/Fail?" AssociatedControlID="fldPassFail"></asp:Label>
                        </div>
                        <div class="col-md-7">
                            <fieldset id="fldPassFail" runat="server">
                                <asp:RadioButton ID="rbPass" runat="server" Text="Pass" GroupName="PassFail" ClientIDMode="Static" />&nbsp;
                                <asp:RadioButton ID="rbFail" runat="server" Text="Fail" GroupName="PassFail" ClientIDMode="Static" />&nbsp;
                                <asp:RadioButton ID="rbNA" runat="server" Text="N/A" GroupName="PassFail" ClientIDMode="Static" />
                            </fieldset>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" Text="Comments:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtComments" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" ></asp:TextBox>
                        </div>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblDocumentation" runat="server" AssociatedControlID="fuDocumentation" Text="Pump Test Documentation:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadAsyncUpload ID="fuDocumentation" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1">
                                <FileFilters>
                                    <telerik:FileFilter Description="Images(jpeg;jpg;gif;png;pdf)" Extensions="jpeg,jpg,gif,png,pdf" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>--%>
                    <asp:HiddenField ID="hfApparatusId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <button id="btnDeleteApparatus" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteApparatus_ServerClick">Delete Apparatus</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveApparatus" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveApparatus_ServerClick">Save Apparatus</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showApparatusDelete();
            showApparatusDetails();
            showNoHoseTest();
            showNoPumpTest();
            isTestRequired();
        });

        function clearApparatusDetailFields() {
            $('#rbPumpTestsConductedYes').prop('checked', false);
            $('#rbPumpTestsConductedNo').prop('checked', false);
            $('#rbHoseTestsYes').prop('checked', false);
            $('#rbHoseTestsNo').prop('checked', false);
            $('#ApplicationContent_txtNoPumpTestsExp').val('');
            $('#ApplicationContent_txtNoHoseTests').val('');
            $('#dvNoPumpTestsExp').hide();
            $('#dvNoHoseTests').hide();
            clearNoteId();
        }

        function showApparatusDetails() {
            var show = $('#rbApparatusPartYes').prop('checked');
            if (show == true) {
                $('#dvApparatusDetails').fadeIn("slow");
            }
            else {
                $('#dvApparatusDetails').fadeOut("fast");
                clearApparatusDetailFields();
            }
        }

        function showNoPumpTest() {
            var show = $('#rbPumpTestsConductedNo').prop('checked');
            if (show == true) {
                $('#dvNoPumpTestsExp').fadeIn("slow");
            }
            else {
                $("#ApplicationContent_txtNoPumpTestsExp").val("");
                $('#dvNoPumpTestsExp').fadeOut("slow");
            }
        }

        function showNoHoseTest() {
            var show = $('#rbHoseTestsNo').prop('checked');
            if (show == true) {
                $('#dvNoHoseTests').fadeIn("slow");
            }
            else {
                $("#ApplicationContent_txtNoHoseTests").val("");
                $('#dvNoHoseTests').fadeOut("slow");
            }
        }
        
        function showApparatusDelete() {
            var noteId = $('#hfApparatusId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteApparatus').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteApparatus').show();
            }
        }

        function openApparatusModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteApparatus').show();
        }

        function isTestRequired() {
            var gpm = $('#txtGPM').val();
            var isRequired = true;
            if (gpm === "0" || gpm === "") {
                isRequired = false;
            }
            if (isRequired) {
                $('#lblTestDate').text("Test Date: *");
                $('#txtTestDate').attr("aria-required", "true");
            }
            else {
                $('#lblTestDate').text("Test Date:");
                $('#txtTestDate').attr("aria-required", "false");
            }
        }

        function clearNoteId() {
            $('#hfApparatusId').val('');
            /*$('#txtNumber').val('');*/
            $('#txtApparatusName').val('');
            $('#txtVIN').val('');
            $('#txtLicense').val('');
            $('#txtGPM').val('');
            $('#txtTestDate').val('');
            $('#rbPass').prop('checked', false);
            $('#rbFail').prop('checked', false);
            $('#txtComments').val('');
            $('#ApplicationContent_btnDeleteApparatus').hide();
        }

        $('#rbApparatusPartYes,#rbApparatusPartNo').change(function () {
            showApparatusDetails();
        });

        $('#rbPumpTestsConductedYes,#rbPumpTestsConductedNo').change(function () {
            showNoPumpTest();
        });

        $('#txtGPM').change(function () {
            isTestRequired();
        });

        $('#rbHoseTestsYes,#rbHoseTestsNo').change(function () {
            showNoHoseTest();
        });

        function triggerApparatusFileUpload() {
            var fileInput = document.getElementById('<%= fuApparatusDocumentation.ClientID %>');
            if (fileInput) {
                fileInput.click();
            }
            return false;
        }

        function onApparatusFileSelected() {
            document.getElementById('<%= hfUploadAction.ClientID %>').value = 'APPARATUS';
            document.forms[0].submit();
        }

        function openDocModal() {
            $('#docModal').modal('show');
            $('#modalHeaderDoc').focus();
        }

        function openEditDocumentNameModal() {
            $('#editDocumentNameModal').modal('show');
        }
    </script>
</asp:Content>
