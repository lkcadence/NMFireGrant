<%@ Page Title="Fire Grant Application: Communication Equipment" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="CommunicationEquipment.aspx.cs" Inherits="NMSFMFireGrantWF.Application.CommunicationEquipment" Async="true" %>
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
            <div class="col-md-6">
                <asp:Label ID="lblCommunications" runat="server" Text="Communications is part of the project? *" AssociatedControlID="fldCommunication"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldCommunication" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbCommunicationsYes" runat="server" Text="Yes" GroupName="Communications" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbCommunicationsNo" runat="server" Text="No" GroupName="Communications" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div id="dvCommunications">
            <div class="row" id="dvAidAgreementsHeader">
                <div class="col-md-12">
                    <h3>Do you have any of the following?</h3>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblHandheldRadios" runat="server" Text="Handheld Radios?" AssociatedControlID="txtHandheldRadios"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtHandheldRadios" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblBaseStations" runat="server" Text="Base Stations?" AssociatedControlID="txtBaseStations"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtBaseStations" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblMobileRadios" runat="server" Text="Mobile Radios?" AssociatedControlID="txtMobileRadios"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtMobileRadios" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-6">
                    <asp:Label ID="lblAppNoRadio" runat="server" Text="Do you have any apparatus without a mobile radio? *" AssociatedControlID="fldAppNoRadio"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldAppNoRadio" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbAppNoRadioYes" runat="server" Text="Yes" GroupName="AppNoRadio" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbAppNoRadioNo" runat="server" Text="No" GroupName="AppNoRadio" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row" id="dvCommunicationEquipHead">
                <div class="col-md-12">
                    <h3>List Existing Communication Equipment by Type</h3>
                </div>
            </div>
            <div class="row" id="dvShowModal" runat="server">
                <div class="col-md-12">
                    <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId()" data-toggle="modal" data-target="#communicationModal">
                        Add Communication Equipment</button>
                    &nbsp;
                    <asp:Button ID="btnUploadCommunicationDocuments" runat="server" CssClass="btn btn-primary"
                        Text="Upload Communication Equipment Documents"
                        OnClientClick="triggerCommunicationFileUpload(); return false;" UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hfUploadAction" runat="server" Value="" />
                    <asp:FileUpload ID="fuCommunicationDocumentation" runat="server"
                        Style="position:absolute;left:-9999px;width:1px;height:1px;opacity:0;"
                        accept=".xls,.xlsx,.csv,.pdf,.doc,.docx"
                        onchange="onCommunicationFileSelected();" />
                </div>
            </div>
            <div class="row" id="dvCommunicationDocumentError" runat="server"></div>
            <div class="row" id="dvCommunicationEquip">
                <div class="col-md-10">
                    <telerik:RadGrid ID="rgCommunicationEquipment" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgCommunicationEquipment_NeedDataSource" OnPageIndexChanged="rgCommunicationEquipment_PageIndexChanged" OnItemDataBound="rgCommunicationEquipment_ItemDataBound" OnItemCommand="rgCommunicationEquipment_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("CommunicationEquipmentId") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CommunicationEquipment" FilterControlAltText="Filter Communication Equipment column" HeaderText="Communication Equipment" UniqueName="CommunicationEquipment">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CommunicationQty" FilterControlAltText="Filter Communication Qty column" HeaderText="Communication Qty" UniqueName="CommunicationQty">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CommunicationEquipmentId" FilterControlAltText="Filter CommunicatonEquipmentId column" HeaderText="TrainingId" UniqueName="CommunicatonEquipmentId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="row" id="dvCommunicationDocuments" style="margin-bottom: 2em;">
                <div class="col-md-12">
                    <h4>Uploaded Communication Equipment Documents</h4>
                    <telerik:RadGrid ID="rgCommunicationDocuments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgCommunicationDocuments_NeedDataSource" OnPageIndexChanged="rgCommunicationDocuments_PageIndexChanged" OnItemDataBound="rgCommunicationDocuments_ItemDataBound" OnItemCommand="rgCommunicationDocuments_ItemCommand">
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
        <hr style="margin: 2em 0;" />
        <div class="row">
            <div class="col-md-12">
                <h3>Do you have interoperability with any of the following agencies?</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblLawEnforcement" runat="server" Text="Law Enforcement?" AssociatedControlID="fldLawEnforcement"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldLawEnforcement" runat="server">
                    <asp:RadioButton ID="rbLawEnforcementYes" runat="server" Text="Yes" GroupName="LawEnforcement" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbLawEnforcementNo" runat="server" Text="No" GroupName="LawEnforcement" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblEmergencyMedical" runat="server" Text="Emergency Medical?" AssociatedControlID="fldEmergencyMedical"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldEmergencyMedical" runat="server">
                    <asp:RadioButton ID="rbEmergencyMedicalYes" runat="server" Text="Yes" GroupName="EmergencyMedical" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbEmergencyMedicalNo" runat="server" Text="No" GroupName="EmergencyMedical" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblOtherFD" runat="server" Text="Other Fire Departments?" AssociatedControlID="fldOtherFD"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldOtherFD" runat="server">
                    <asp:RadioButton ID="rbOtherFDYes" runat="server" Text="Yes" GroupName="OtherFD" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbOtherFDNo" runat="server" Text="No" GroupName="OtherFD" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblOther" runat="server" Text="Other (that could not be decribed above)?" AssociatedControlID="fldOther"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldOther" runat="server">
                    <asp:RadioButton ID="rbOtherYes" runat="server" Text="Yes" GroupName="Other" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbOtherNo" runat="server" Text="No" GroupName="Other" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvOtherDescription">
            <div class="col-md-5">
                <asp:Label ID="lblOtherDesc" runat="server" Text="Describe (if 'yes' on others or any additional comments)? *" AssociatedControlID="txtOtherDescription" aria-required="true"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtOtherDescription" runat="server" CssClass="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblNotCovered" runat="server" Text="Do you have any areas in your jurisdiction which are NOT covered by a repeater?" AssociatedControlID="fldNotCovered"></asp:Label>
            </div>
            <div class="col-md-4">
                <fieldset id="fldNotCovered" runat="server">
                    <asp:RadioButton ID="rbNotCoveredYes" runat="server" Text="Yes" GroupName="NotCovered" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbNotCoveredNo" runat="server" Text="No" GroupName="NotCovered" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvRepeaterDescription">
            <div class="col-md-5">
                <asp:Label ID="lblRepeaterDescription" runat="server" Text="Describe (if 'yes' on above)? *" AssociatedControlID="txtRepeaterDescription"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtRepeaterDescription" runat="server" CssClass="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="5" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        </div>
        <div id="dvAdmin" runat="server">
            <div class="row">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="lblComments" runat="server" Text="Comments" AssociatedControlID="txtComments"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <hr />
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
    <div class="modal fade" id="communicationModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblCommunicationHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblCommunicationHeader">Add Communication Equipment</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblCommunicationError" runat="server"></asp:Label>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblCommunicationNumber" runat="server" AssociatedControlID="txtCommunicationNumber" Text="Communication Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtCommunicationNumber" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblCommunicationEquipment" runat="server" AssociatedControlID="txtCommunicationEquipment" Text="Communication Equipment: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCommunicationEquipment" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblCommunicationQty" runat="server" AssociatedControlID="txtCommunicationQty" Text="Quantity: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtCommunicationQty" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator2" errormessage="Please enter value between 0-1000." forecolor="Red" controltovalidate="txtCommunicationQty" minimumvalue="0" maximumvalue="1000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfCommunicationId" runat="server" ClientIDMode="Static" Value="" />
                </div>
                
                <div class="modal-footer">
                    <button id="btnDeleteCommunication" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteCommunication_ServerClick">Delete Communication Equipment</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveCommunication" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveCommunication_ServerClick">Save Communication Equipment</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showCommunications();
            showCommunicationDelete();
            showOtherDesc();
            showRepeaterDesc();
        });

        function clearCommunicationsFields() {
            var numericBox = $find('ApplicationContent_txtHandheldRadios');
            if (numericBox) { numericBox.set_value(0); }
            numericBox = $find('ApplicationContent_txtBaseStations');
            if (numericBox) { numericBox.set_value(0); }
            numericBox = $find('ApplicationContent_txtMobileRadios');
            if (numericBox) { numericBox.set_value(0); }
            $('#rbAppNoRadioYes').prop('checked', false);
            $('#rbAppNoRadioNo').prop('checked', false);
            $('#rbLawEnforcementYes').prop('checked', false);
            $('#rbLawEnforcementNo').prop('checked', false);
            $('#rbEmergencyMedicalYes').prop('checked', false);
            $('#rbEmergencyMedicalNo').prop('checked', false);
            $('#rbOtherFDYes').prop('checked', false);
            $('#rbOtherFDNo').prop('checked', false);
            $('#rbOtherYes').prop('checked', false);
            $('#rbOtherNo').prop('checked', false);
            $('#rbNotCoveredYes').prop('checked', false);
            $('#rbNotCoveredNo').prop('checked', false);
            $('#txtOtherDescription').val('');
            $('#txtRepeaterDescription').val('');
            showOtherDesc();
            showRepeaterDesc();
        }

        function showCommunications() {
            var show = $('#rbCommunicationsYes').prop('checked');
            if (show == true) {
                $('#dvCommunications').fadeIn("slow");
            }
            else {
                $('#dvCommunications').fadeOut("fast");
                clearCommunicationsFields();
            }
        }

        function showOtherDesc() {
            var show = $('#rbOtherYes').prop('checked');
            if (show == true) {
                $('#dvOtherDescription').fadeIn("slow");
            }
            else {
                $('#dvOtherDescription').fadeOut("slow");
                $('#txtOtherDescription').val("");
            }
        }

        function showRepeaterDesc() {
            var show = $('#rbNotCoveredYes').prop('checked');
            if (show == true) {
                $('#dvRepeaterDescription').fadeIn("slow");
            }
            else {
                $('#dvRepeaterDescription').fadeOut("slow");
                $('#txtRepeaterDescription').val("");
            }
        }
        
        function showCommunicationDelete() {
            var noteId = $('#hfCommunicationId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteCommunication').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteCommunication').show();
            }
        }

        function openCommunicationModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteCommunication').show();
        }

        function clearNoteId() {
            $('#hfCommunicationId').val('');
            /*$('#txtCommunicationNumber').val('');*/
            $('#txtCommunicationEquipment').val('');
            $('#txtCommunicationQty').val('');
            $('#ApplicationContent_btnDeleteCommunication').hide();
        }

        $('#rbCommunicationsYes,#rbCommunicationsNo').change(function () {
            showCommunications();
        });

        $('#rbOtherYes,#rbOtherNo').change(function () {
            showOtherDesc();
        });

        $('#rbNotCoveredYes,#rbNotCoveredNo').change(function () {
            showRepeaterDesc();
        });

        function triggerCommunicationFileUpload() {
            var fileInput = document.getElementById('<%= fuCommunicationDocumentation.ClientID %>');
            if (fileInput) {
                fileInput.click();
            }
            return false;
        }

        function onCommunicationFileSelected() {
            document.getElementById('<%= hfUploadAction.ClientID %>').value = 'COMMUNICATION';
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
