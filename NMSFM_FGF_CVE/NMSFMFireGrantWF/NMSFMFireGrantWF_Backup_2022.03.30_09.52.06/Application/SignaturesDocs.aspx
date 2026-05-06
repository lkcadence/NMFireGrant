<%@ Page Title="Fire Grant Application: Signatures and Supporting Documentation" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="SignaturesDocs.aspx.cs" Inherits="NMSFMFireGrantWF.Application.SignaturesDocs" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row">
            <div class="col-md-8">
                <h3>Instructions</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <ol>
                    <li>
                        Verify the information in the application is correct.
                    </li>
                    <li>
                        Upload any required documentation.
                    </li>
                    <li>
                        Add all required signers to the application.
                    </li>
                    <li>
                        If you are the Fiscal Agent, Fire Chief or County Manager please indicate that you are signing the application and enter your information.
                    </li>
                    <li>
                        Send emails to signers to allow them access to reivew and sign the application.
                    </li>
                    <li>
                        Once all signatures have been collected submit your application for review.
                    </li>
                </ol>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
                <div class="row" id="dvDocumentError" runat="server"></div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblCategory" runat="server" AssociatedControlID="ddlCategory" Text="Categories: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Fiscal Agent Commitment" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="Other" Value="2">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblDocumentation" runat="server" AssociatedControlID="fuDocumentation" Text="Document to Upload:"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <telerik:RadAsyncUpload ID="fuDocumentation" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="OnFileSelected" OnClientFileUploadRemoved="OnFileSelected">
                                <FileFilters>
                                    <telerik:FileFilter Description="Images(jpeg;jpg;gif;png;pdf)" Extensions="jpeg,jpg,gif,png,pdf" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>
                    <div class="row formRow" id="dvAddDocumentLink" style="display:none">
                        <div class="col-md-3" id="dvAddDocument" runat="server">
                            <asp:LinkButton ID="lnkAddDocument" runat="server" Text="Upload Document" OnClick="lnkAddDocument_Click" ClientIDMode="Static"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <h3>Uploaded Files</h3>
                        </div>
                        <div class="col-md-6">
                            <telerik:RadGrid ID="rgDocuments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgDocuments_NeedDataSource" OnPageIndexChanged="rgDocuments_PageIndexChanged" OnItemDataBound="rgDocuments_ItemDataBound" OnItemCommand="rgDocuments_ItemCommand">
                                <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                                <ClientSettings AllowKeyboardNavigation="True">
                                </ClientSettings>
                                <MasterTableView>
                                    <Columns>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("DocumentId") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridBoundColumn DataField="DocumentType" FilterControlAltText="Filter Document Type column" HeaderText="DocumentType" UniqueName="DocumentType">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter Document Name column" HeaderText="DocumentName" UniqueName="DocumentName">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Document" FilterControlAltText="Filter Document Link column" HeaderText="Document" UniqueName="Document" Display="false">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridButtonColumn ButtonType="LinkButton" ConfirmDialogType="RadWindow" FilterControlAltText="Filter View column" HeaderText="View" Text="View" UniqueName="View" CommandName="View"> 
		                                </telerik:GridButtonColumn>--%>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="View" UniqueName="View">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" Text="View Document" CommandName="View" CommandArgument='<%# Eval("DocumentId") %>'>
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
         <div class="row formRow">
            <div class="col-md-3">
                <h3>Signatures</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-10 card card-body" id="dvESig" runat="server">

            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#signatureModal">
                    Add Signature
                </button>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <telerik:RadGrid ID="rgSignatures" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgSignatures_NeedDataSource" OnPageIndexChanged="rgSignatures_PageIndexChanged" OnItemDataBound="rgSignatures_ItemDataBound" OnItemCommand="rgSignatures_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="View" CommandArgument='<%# Eval("SignatureId") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="SignatureRole" FilterControlAltText="Filter Signature Role column" HeaderText="Signature Role" UniqueName="SignatureRole">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PrintedName" FilterControlAltText="Filter PrintedName column" HeaderText="Name / Title" UniqueName="PrintedName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Signature" FilterControlAltText="Filter Signature column" HeaderText="Signature" UniqueName="Signature">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DateEntered" FilterControlAltText="Filter Date Entered column" HeaderText="Entry Date" UniqueName="DateEntered">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SignatureId" FilterControlAltText="Filter SignatureId column" HeaderText="DocumentId" UniqueName="SignatureId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-md-10">
                <span class="alert alert-info" role="alert">
                    Note: Please review the Completion Checklist prior to submitting the application for review.
                </span>
            </div>
        </div>--%>
        <div id="dvAdmin" runat="server">
            <div class="row">
                <hr />
            </div>
            <div class="row formRow">
                <div class="col-md-5">
                    <asp:Label ID="lblAppCompleteness" runat="server" Text="Completeness of Application (max of 10)?" AssociatedControlID="txtAppCompleteness"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtAppCompleteness" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MaxValue="10" Type="Number"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
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
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save & Send Emails" OnClick="btnSave_Click" />&nbsp;
                <%--<asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Next" />--%>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <!-- Modal -->
    <div class="modal fade" id="signatureModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblSignatureHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblSignatureHeader">Add Signature</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblSignatureError" runat="server"></asp:Label>
                    </div>
                    
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblSignatureRole" runat="server" AssociatedControlID="ddlSignatureRole" Text="Role: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlSignatureRole" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Fiscal Agent" Value="Fiscal Agent"></asp:ListItem>
                                <asp:ListItem Text="Fire Chief" Value="Fire Chief"></asp:ListItem>
                                <asp:ListItem Text="County Manager" Value="County Manager"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow" id="dvIsSelfSign">
                        <div class="col-md-1">
                            <asp:CheckBox ID="chkSelfSign" ClientIDMode="Static" runat="server" /><br />
                        </div>
                        <div class="col-md-9">
                            <asp:Label ID="lblSelfSign" runat="server" AssociatedControlID="chkSelfSign">I am signing this application as the role indicated above. (Unchecked will send email to signator)</asp:Label>
                        </div>
                    </div>
                    <div class="row formRow" id="dvAgreement">
                        <div class="col-md-1">
                            <asp:CheckBox ID="chkAgreement" runat="server" /><br />
                        </div>
                        <div class="col-md-9">
                            <asp:Label ID="lblAgreement" runat="server" AssociatedControlID="chkAgreement">I HAVE READ AND UNDERSTAND THIS AGREEMENT, AND I ACCEPT AND AGREE TO ALL OF ITS TERMS AND CONDITIONS.</asp:Label>
                        </div>
                    </div>
                    <div class="row formRow" id="dvPrintedName">
                        <div class="col-sm-5">
                            <asp:Label ID="lblPrintedName" runat="server" AssociatedControlID="txtPrintedName" Text="Signer Name / Title: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtPrintedName" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvEmail">
                        <div class="col-sm-5">
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Signer Email Address: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvSignature">
                        <div class="col-sm-5">
                            <asp:Label ID="lblSignature" runat="server" AssociatedControlID="txtSignature" Text="Type Signature: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtSignature" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvSignatureDate">
                        <div class="col-sm-5">
                            <asp:Label ID="lblSignatureDate" runat="server" AssociatedControlID="txtSignatureDate" Text="Date Signed: *"></asp:Label>
                        </div>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtSignatureDate" runat="server" class="form-control" ClientIDMode="Static" aria-required="true" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfSignatureId" runat="server"/>
                    <asp:HiddenField ID="hfLoginToken" runat="server" />
                </div>

                <div class="modal-footer">
                    <%--<button id="btnDeleteSignature" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteSignature_ServerClick">Delete Signature</button>--%>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveSignature" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveSignature_ServerClick">Save Signature</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            //showSignatureDelete();
            SignApp();
        });
        
        function showSignatureDelete() {
            var noteId = $('#hfSignatureId').val();
            if (noteId === "") {
                $('#MainContent_btnDeleteSignature').hide();
            }
            else {
                $('#MainContent_btnDeleteSignature').show();
            }
        }

        function SignApp() {
            if ($('#chkSelfSign').is(":checked")) {
                $('#dvPrintedName').show();
                $('#dvAgreement').show();
                $('#dvEmail').hide();
                $('#dvSignature').show();
                $('#dvSignatureDate').show();
            }
            else {
                $('#dvPrintedName').show();
                $('#dvAgreement').hide();
                $('#dvEmail').show();
                $('#dvSignature').hide();
                $('#dvSignatureDate').hide();
            }
        }

        function openSignatureModal() {
            //$('#btnShowModal').click();
            $('#signatureModal').modal('show');
            $('#modalHeader').focus();
            //$('#MainContent_btnDeleteSignature').show();
        }

        function clearNoteId() {
            $('#hfSignatureId').val('');
            $('#txtSignatureDate').val('');
            $('#txtSignature').val('');
            //$('#MainContent_btnDeleteSignature').hide();
        }

        function OnFileSelected(sender, args) {

            var numFiles = sender._selectedFilesCount;
            if (numFiles > 0) {
                $('#dvAddDocumentLink').show();
            }
            else {
                $('#dvAddDocumentLink').hide();
            }
        }

        $('#chkSelfSign').change(function () {
            SignApp();
        });
        
    </script>
</asp:Content>
