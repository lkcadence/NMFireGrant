<%@ Page Title="Fire Grant Application: Application Status" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="ApplicationStatus.aspx.cs" Inherits="NMSFMFireGrantWF.Application.ApplicationStatus" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblsd" runat="server" Text="Application opened on:" AssociatedControlID="lblStartDate"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblsubdt" runat="server" Text="Application submitted on:" AssociatedControlID="lblSubmittedDate"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblSubmittedDate" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblAppSt" runat="server" Text="Application status:" AssociatedControlID="lblApplicationStatus"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblApplicationStatus" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblAppNo" runat="server" Text="Application number:" AssociatedControlID="lblApplicationNumber"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblApplicationNumber" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Previous Apps</h3>
            </div>
        </div>
        <div class="container" id="dvPreviousApps">
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgPreviousApps" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" OnNeedDataSource="rgPreviousApps_NeedDataSource" OnPageIndexChanged="rgPreviousApps_PageIndexChanged" OnItemDataBound="rgPreviousApps_ItemDataBound" AllowMultiRowSelection="false">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ApplicationNumber" FilterControlAltText="Filter Confirmation Number column" HeaderText="Application Number" UniqueName="ConfirmationNumber">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DateSubmitted" FilterControlAltText="Filter Date Submitted column" HeaderText="Date Submitted" UniqueName="DateSubmitted">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationStatus" FilterControlAltText="Filter Application Status column" HeaderText="Application Status" UniqueName="ApplicationStatus">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastStatusChange" FilterControlAltText="Filter Status Change column" HeaderText="Last Change" UniqueName="LastStatusChange">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GrantedAmount" FilterControlAltText="Filter Granted Amount column" HeaderText="Granted Amount" UniqueName="GrantedAmount">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationId" FilterControlAltText="Filter ApplicationId column" HeaderText="ApplicationId" UniqueName="ApplicationId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div id="dvAdmin" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h3>Update Application Status</h3>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-2">
                    <asp:Label ID="lblGrantAmountRequested" runat="server" Text="Grant Amount Requested:" AssociatedControlID="txtGrantAmountRequested"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtGrantAmountRequested" runat="server" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" ReadOnly="true"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-2">
                    <asp:Label ID="lblGrantedAmount" runat="server" Text="Grant Amount Awarded:" AssociatedControlID="txtGrantedAmount"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtGrantedAmount" runat="server" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-2">
                    <asp:Label ID="lblStipendAmountRequested" runat="server" Text="Stipend Amount Requested:" AssociatedControlID="txtStipendAmountRequested"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtStipendAmountRequested" runat="server" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" ReadOnly="true"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-2">
                    <asp:Label ID="lblStipendAmountAwarded" runat="server" Text="Stipend Amount Awarded:" AssociatedControlID="txtStipendAmountAwarded"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtStipendAmountAwarded" runat="server" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-2">
                    <asp:Label ID="lblUpdateStatus" runat="server" Text="Update status:" AssociatedControlID="ddlUpdateStatus"></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlUpdateStatus" runat="server">
                        <asp:ListItem Value="7" Text="Awarded"></asp:ListItem>
                        <asp:ListItem Value="8" Text="Not Awarded"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Approved"></asp:ListItem>
                        <asp:ListItem Value="9" Text="Grant Approved (No Stipend)"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Rejected"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Reopen"></asp:ListItem>
                        <asp:ListItem Value="4" Text="Under Review"></asp:ListItem>
                        <asp:ListItem Value="5" Text="Submitted for Review"></asp:ListItem>
                        <asp:ListItem Value="6" Text="In Process"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-12">
                    <h3>Checklist</h3>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblNFIRS" runat="server" Text="NFIRS Compliant? *" AssociatedControlID="fldNFIRS"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldNFIRS" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbNFIRSYes" runat="server" Text="Yes" GroupName="NFIRS" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbNFIRSNo" runat="server" Text="No" GroupName="NFIRS" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblPumpTests" runat="server" Text="Pump Test Data Compliant? *" AssociatedControlID="fldPumpTests"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldPumpTests" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbPumpTestsYes" runat="server" Text="Yes" GroupName="PumpTests" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbPumpTestsNo" runat="server" Text="No" GroupName="PumpTests" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="Label1" runat="server" Text="Hose Test Data Compliant? *" AssociatedControlID="fldHoseTests"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldHoseTests" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbHoseTestsYes" runat="server" Text="Yes" GroupName="HoseTests" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbHoseTestsNo" runat="server" Text="No" GroupName="HoseTests" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblSignatures" runat="server" Text="Acknowledgement/Commitment Signatures? *" AssociatedControlID="fldSignatures"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldSignatures" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbSignaturesYes" runat="server" Text="Yes" GroupName="Signatures" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbSignaturesNo" runat="server" Text="No" GroupName="Signatures" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblSpecs" runat="server" Text="Specs Received by Deadline? *" AssociatedControlID="fldSpecs"></asp:Label>
                </div>
                <div class="col-md-3">
                    <fieldset id="fldSpecs" runat="server" aria-required="true">
                        <asp:RadioButton ID="rbSpecsYes" runat="server" Text="Yes" GroupName="Specs" ClientIDMode="Static" />&nbsp;
                        <asp:RadioButton ID="rbSpecsNo" runat="server" Text="No" GroupName="Specs" ClientIDMode="Static" />
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblNotes" runat="server" Text="Notes" AssociatedControlID="txtNotes"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-10">
                    <h3>FMO Support Reviewer Signature</h3>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-10 card card-body" id="dvESig" runat="server">

                </div>
            </div>
            <div class="row formRow" id="dvAgreement">
                <div class="col-md-1">
                    <asp:CheckBox ID="chkAgreement" runat="server" /><br />
                </div>
                <div class="col-md-9">
                    <asp:Label ID="lblAgreement" runat="server" AssociatedControlID="chkAgreement">I HAVE REVIEWED THIS APPLICATION...</asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:TextBox ID="txtReviewer" runat="server" ClientIDMode="Static"></asp:TextBox><br />
                    <asp:Label ID="lblReviewer" runat="server" Text="Reviewer Name" ClientIDMode="Static" AssociatedControlID="txtReviewer"></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtSignature" runat="server" ClientIDMode="Static"></asp:TextBox><br />
                    <asp:Label ID="lblSignature" runat="server" Text="Type Name (Signature)" ClientIDMode="Static" AssociatedControlID="txtSignature"></asp:Label>
                    <asp:HiddenField ID="hfSignatureId" runat="server" />
                </div>
                <div class="col-md-3">
                    <telerik:RadDatePicker ID="txtDate" runat="server" ClientIDMode="Static"></telerik:RadDatePicker><br />
                    <asp:Label ID="lblDate" runat="server" Text="Date" ClientIDMode="Static" AssociatedControlID="txtDate"></asp:Label>
                </div>
            </div>
            <div class="row">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;
                </div>
            </div>
        </div>
            
        <asp:HiddenField ID="hfApplicationId" runat="server" />
        <asp:HiddenField ID="hfAddressId" runat="server" />
        <asp:HiddenField ID="hfFiscalYear" runat="server" />
    </div>
</asp:Content>
