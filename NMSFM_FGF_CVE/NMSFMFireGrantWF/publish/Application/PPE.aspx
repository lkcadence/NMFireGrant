<%@ Page Title="Fire Grant Application: PPE" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="PPE.aspx.cs" Inherits="NMSFMFireGrantWF.Application.PPE" Async="true" %>
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
            <div class="col-md-5">
                <asp:Label ID="lblPPE" runat="server" Text="PPE is part of the project? *" AssociatedControlID="fldPPE"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldPPE" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbPPEYes" runat="server" Text="Yes" GroupName="PPE" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbPPENo" runat="server" Text="No" GroupName="PPE" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div id="dvPPE" style="display:none">
            <%--<div class="row" id="dvAidAgreementsHeader">
                <div class="col-md-12">
                    <h3>Bunker Gear</h3>
                </div>
            </div>--%>
            <div class="row formRow">
                <div class="col-md-5">
                    <asp:Label ID="lblPPEInspected" runat="server" Text="All PPE inspected to the most current NFPA 1851 standard? *" AssociatedControlID="fldPPEInspected"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldPPEInspected" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbPPEInspectedYes" runat="server" Text="Yes" GroupName="PPEInspected" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbPPEInspectedNo" runat="server" Text="No" GroupName="PPEInspected" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row" id="dvStandardComplientPPEHead">
                <div class="col-md-12">
                    <h3>Standard Compliant PPE *</h3>
                </div>
            </div>
            <div class="row" id="dvShowModal" runat="server">
                <div class="col-md-12">
                    <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId()" data-toggle="modal" data-target="#standardCompliantPPEModal">
                        Add Standard Compliant PPE</button>
                    &nbsp;
                    <asp:Button ID="btnUploadPPEDocuments" runat="server" CssClass="btn btn-primary" Text="Upload PPE Documents"
                        OnClientClick="triggerPPEFileUpload(); return false;" UseSubmitBehavior="false" />
                    <%--
                    <asp:Label ID="lblPPEDocumentType" runat="server" AssociatedControlID="ddlPPEDocumentType" Text="Document Type:"></asp:Label>
                    <asp:DropDownList ID="ddlPPEDocumentType" runat="server" CssClass="form-control" ClientIDMode="Static" Width="180px">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="PPE Document" Value="PPE Document"></asp:ListItem>
                    </asp:DropDownList>
                    --%>
                    <asp:HiddenField ID="hfUploadAction" runat="server" Value="" />
                    <asp:FileUpload ID="fuPPEDocumentation" runat="server" Style="position:absolute;left:-9999px;width:1px;height:1px;opacity:0;"
                        accept=".xls,.xlsx,.csv,.pdf,.doc,.docx"
                        onchange="onPPEFileSelected();" />
                </div>
            </div>
            <div class="row" id="dvPPEDocumentError" runat="server"></div>
            <div class="row" id="dvStandardComplientPPE">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgStandardComplientPPE" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgStandardComplientPPE_NeedDataSource" OnPageIndexChanged="rgStandardComplientPPE_PageIndexChanged" OnItemDataBound="rgStandardComplientPPE_ItemDataBound" OnItemCommand="rgStandardComplientPPE_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("StandardComplientPPEId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="PPEType" FilterControlAltText="Filter PPE Type column" HeaderText="PPE Type" UniqueName="PPEType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Year" FilterControlAltText="Filter Year column" HeaderText="Year" UniqueName="Year">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="Quantity" UniqueName="Quantity">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Age" FilterControlAltText="Filter Age column" HeaderText="Age (years)" UniqueName="Age">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Condition" FilterControlAltText="Filter Condition column" HeaderText="Condition" UniqueName="Condition">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StandardComplientPPEId" FilterControlAltText="Filter StandardComplientPPEId column" HeaderText="StandardComplientPPEId" UniqueName="StandardComplientPPEId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="row" id="dvPPEDocuments" style="margin-bottom: 2em;">
                <div class="col-md-12">
                    <h4>Uploaded PPE Documents</h4>
                    <telerik:RadGrid ID="rgPPEDocuments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgPPEDocuments_NeedDataSource" OnPageIndexChanged="rgPPEDocuments_PageIndexChanged" OnItemDataBound="rgPPEDocuments_ItemDataBound" OnItemCommand="rgPPEDocuments_ItemCommand">
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
            
        </div>
        <hr style="margin: 2em 0;" />
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblSCBAQuestion" runat="server" Text="SCBA is part of the project? *" AssociatedControlID="fldSCBA"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldSCBA" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbSCBAYes" runat="server" Text="Yes" GroupName="SCBA" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbSCBANo" runat="server" Text="No" GroupName="SCBA" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div id="dvSCBA" style="display:none">
            <div class="row" id="dvStandardComplientSCBAHead">
                <div class="col-md-12">
                    <h3>Standard Compliant SCBA *</h3>
                </div>
            </div>
            <div class="row" id="dvShowModal2" runat="server">
                <div class="col-md-12">
                    <button type="button" id="btnShowModal2" class="btn btn-primary" onclick="clearNote2Id()" data-toggle="modal" data-target="#standardCompliantSCBAModal">
                        Add Standard Compliant SCBA</button>
                    &nbsp;
                    <asp:Button ID="btnUploadSCBADocuments" runat="server" CssClass="btn btn-primary" Text="Upload SCBA Documents"
                        OnClientClick="triggerSCBAFileUpload(); return false;" UseSubmitBehavior="false" />
                    <%--
                    <asp:Label ID="lblSCBADocumentType" runat="server" AssociatedControlID="ddlSCBADocumentType" Text="Document Type:"></asp:Label>
                    <asp:DropDownList ID="ddlSCBADocumentType" runat="server" CssClass="form-control" ClientIDMode="Static" Width="180px">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="SCBA Document" Value="SCBA Document"></asp:ListItem>
                    </asp:DropDownList>
                    --%>
                    <asp:FileUpload ID="fuSCBADocumentation" runat="server" Style="position:absolute;left:-9999px;width:1px;height:1px;opacity:0;"
                        accept=".xls,.xlsx,.csv,.pdf,.doc,.docx"
                        onchange="onSCBAFileSelected();" />
                </div>
            </div>
            <div class="row" id="dvSCBADocumentError" runat="server"></div>
            <div class="row" id="dvStandardComplientSCBA">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgStandardComplientSCBA" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgStandardComplientSCBA_NeedDataSource" OnPageIndexChanged="rgStandardComplientSCBA_PageIndexChanged" OnItemDataBound="rgStandardComplientSCBA_ItemDataBound" OnItemCommand="rgStandardComplientSCBA_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("StandardComplientSCBAId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="SCBAType" FilterControlAltText="Filter SCBA Type column" HeaderText="SCBA Type" UniqueName="SCBAType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Year" FilterControlAltText="Filter Year column" HeaderText="Year" UniqueName="Year">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="Quantity" UniqueName="Quantity">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Age" FilterControlAltText="Filter Age column" HeaderText="Age (years)" UniqueName="Age">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Condition" FilterControlAltText="Filter Condition column" HeaderText="Condition" UniqueName="Condition">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StandardComplientSCBAId" FilterControlAltText="Filter StandardComplientSCBAId column" HeaderText="StandardComplientSCBAId" UniqueName="StandardComplientSCBAId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <div class="row" id="dvSCBADocuments">
                <div class="col-md-12">
                    <h4>Uploaded SCBA Documents</h4>
                    <telerik:RadGrid ID="rgSCBADocuments" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgSCBADocuments_NeedDataSource" OnPageIndexChanged="rgSCBADocuments_PageIndexChanged" OnItemDataBound="rgSCBADocuments_ItemDataBound" OnItemCommand="rgSCBADocuments_ItemCommand">
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
    <!-- Modal -->
    <div class="modal fade" id="standardCompliantPPEModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblStandardCompliaintPPEHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblStandardCompliaintPPEHeader">Add Standard Compliant PPE</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblStandardCompliaintPPEError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblPPEType" runat="server" AssociatedControlID="ddlPPEType" Text="PPE Type: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlPPEType" runat="server" ClientIDMode="Static" Width="150px"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintPPEYear" runat="server" AssociatedControlID="txtStandardCompliaintPPEYear" Text="Year: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintPPEYear" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator2" errormessage="Please enter value between 1950 and 3000." forecolor="Red" controltovalidate="txtStandardCompliaintPPEYear" minimumvalue="1950" maximumvalue="3000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintPPEQuantity" runat="server" AssociatedControlID="txtStandardCompliaintPPEQuantity" Text="Quantity: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintPPEQuantity" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator5" errormessage="Please enter value between 0 and 1000." forecolor="Red" controltovalidate="txtStandardCompliaintPPEQuantity" minimumvalue="0" maximumvalue="1000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintPPEAge" runat="server" AssociatedControlID="txtStandardCompliaintPPEAge" Text="Age (years): *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintPPEAge" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator6" errormessage="Please enter value between 0 and 1000." forecolor="Red" controltovalidate="txtStandardCompliaintPPEAge" minimumvalue="0" maximumvalue="1000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintPPECondition" runat="server" AssociatedControlID="ddlStandardCompliaintPPECondition" Text="Condition: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlStandardCompliaintPPECondition" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Fair" Value="Fair"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfStandardCompliaintPPEId" runat="server" ClientIDMode="Static" Value="" />
                </div>
                
                <div class="modal-footer">
                    <button id="btnDeleteStandardCompliaintPPE" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteStandardCompliaintPPE_ServerClick">Delete</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveStandardCompliaintPPE" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveStandardCompliaintPPE_ServerClick">Save</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 2 -->
    <div class="modal fade" id="standardCompliantSCBAModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblStandardCompliaintSCBAHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader2">
                    <h4 class="modal-title" id="lblStandardCompliaintSCBAHeader">Add Standard Compliant SCBA</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblStandardCompliaintSCBAError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblSCBAType" runat="server" AssociatedControlID="ddlSCBAType" Text="SCBA Type: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlSCBAType" runat="server" ClientIDMode="Static" Width="150px"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintSCBAYear" runat="server" AssociatedControlID="txtStandardCompliaintSCBAYear" Text="Year: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintSCBAYear" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator1" errormessage="Please enter value between 1950 and 3000." forecolor="Red" controltovalidate="txtStandardCompliaintSCBAYear" minimumvalue="1950" maximumvalue="3000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintSCBAQuantity" runat="server" AssociatedControlID="txtStandardCompliaintSCBAQuantity" Text="Quantity: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintSCBAQuantity" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator3" errormessage="Please enter value between 0 and 1000." forecolor="Red" controltovalidate="txtStandardCompliaintSCBAQuantity" minimumvalue="0" maximumvalue="1000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintSCBAAge" runat="server" AssociatedControlID="txtStandardCompliaintSCBAAge" Text="Age (years): *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtStandardCompliaintSCBAAge" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator4" errormessage="Please enter value between 0 and 1000." forecolor="Red" controltovalidate="txtStandardCompliaintSCBAAge" minimumvalue="0" maximumvalue="1000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblStandardCompliaintSCBACondition" runat="server" AssociatedControlID="ddlStandardCompliaintSCBACondition" Text="Condition: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlStandardCompliaintSCBACondition" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Fair" Value="Fair"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfStandardCompliaintSCBAId" runat="server" ClientIDMode="Static" Value="" />
                </div>
                
                <div class="modal-footer">
                    <button id="btnDeleteStandardCompliaintSCBA" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteStandardCompliaintSCBA_ServerClick">Delete</button>
                    <button id="btnCloseModal2" type="button" class="btn btn-primary" onclick="clearNote2Id()" data-dismiss="modal">Close</button>
                    <button id="btnSaveStandardCompliaintSCBA" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveStandardCompliaintSCBA_ServerClick">Save</button>
                </div>
            </div>
        </div>
    </div>
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
                    <asp:HiddenField ID="hfEditDocumentSection" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSaveDocumentName" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveDocumentName_Click" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showPPE();
            showSCBA();
            showPPEDelete();
            showSCBADelete();
        });

        function clearPPEFields() {
            $('#rbPPEInspectedYes').prop('checked', false);
            $('#rbPPEInspectedNo').prop('checked', false);
            $('#fldPPEInspected').attr('aria-required', 'false');
            clearNoteId();
        }

        function showPPE() {
            var show = $('#rbPPEYes').prop('checked');
            if (show == true) {
                $('#fldPPEInspected').attr('aria-required', 'true');
                $('#dvPPE').fadeIn("slow");
            }
            else {
                $('#dvPPE').fadeOut("fast");
                // Disabled: preserve section data when PartOfProject = No
                //clearPPEFields();
            }
        }

        function showSCBA() {
            try {
                var show = $('#rbSCBAYes').prop('checked');
                if (show == true) {
                    $('#dvSCBA').fadeIn("slow");
                }
                else {
                    $('#dvSCBA').fadeOut("fast");
                    // Disabled: preserve section data when PartOfProject = No
                    //clearNote2Id();
                }
            }
            catch {

            }

        }


        function showPPEDelete() {
            var noteId = $('#hfStandardCompliaintPPEId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteStandardCompliaintPPE').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteStandardCompliaintPPE').show();
            }
        }

        function showSCBADelete() {
            var noteId = $('#hfStandardCompliaintSCBAId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteStandardCompliaintSCBA').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteStandardCompliaintSCBA').show();
            }
        }

        function openStandardCompliantPPEModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteStandardCompliaintPPE').show();
        }

        function openStandardCompliantSCBAModal() {
            $('#btnShowModal2').click();
            $('#modalHeader2').focus();
            $('#ApplicationContent_btnDeleteStandardCompliaintSCBA').show();
        }

        function clearNoteId() {
            $('#hfStandardCompliaintPPEId').val('');
            $('#txtStandardCompliaintPPEYear').val('');
            $('#txtStandardCompliaintPPEQuantity').val('');
            $('#txtStandardCompliaintPPEAge').val('');
            $('#ddlPPEType').val('');
            $('#ApplicationContent_btnDeleteStandardCompliaintPPE').hide();
        }

        function clearNote2Id() {
            $('#hfStandardCompliaintSCBAId').val('');
            $('#txtStandardCompliaintSCBAYear').val('');
            $('#txtStandardCompliaintSCBAQuantity').val('');
            $('#txtStandardCompliaintSCBAAge').val('');
            $('#ddlSCBAType').val('');
            $('#ApplicationContent_btnStandardCompliaintSCBA').hide();
        }

        $('#rbPPEYes,#rbPPENo').change(function () {
            showPPE();
        });

        $('#rbSCBAYes,#rbSCBANo').change(function () {
            showSCBA();
        });

        function triggerPPEFileUpload() {
            var fileInput = document.getElementById('<%= fuPPEDocumentation.ClientID %>');
            if (fileInput) {
                fileInput.click();
            }
            return false;
        }

        function triggerSCBAFileUpload() {
            var fileInput = document.getElementById('<%= fuSCBADocumentation.ClientID %>');
            if (fileInput) {
                fileInput.click();
            }
            return false;
        }

        function onPPEFileSelected() {
            document.getElementById('<%= hfUploadAction.ClientID %>').value = 'PPE';
            document.forms[0].submit();
        }

        function onSCBAFileSelected() {
            document.getElementById('<%= hfUploadAction.ClientID %>').value = 'SCBA';
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
