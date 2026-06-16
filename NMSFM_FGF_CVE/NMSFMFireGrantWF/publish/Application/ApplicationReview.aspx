<%@ Page Title="Fire Grant Application: Application Review" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="ApplicationReview.aspx.cs" Inherits="NMSFMFireGrantWF.Application.ApplicationReview" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row">
            <div class="col-md-12">
                <h3>Scoring</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table table-bordered" style="width:100%" id="tblSingleScores" runat="server">
                    <thead>
                        <tr>
                            <th scope="col">
                                Area
                            </th>
                            <th scope="col">
                                Score
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th scope="row">ISO Rating?</th>
                            <td id="tdISORating" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Regular and adequate training?</th>
                            <td id="tdTraining" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Financial Need Grade?</th>
                            <td id="tdFinancialNeed" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Problem Grade?</th>
                            <td id="tdProblem" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Benefit to the Community Grade?</th>
                            <td id="tdBenefit" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Consequences Grade?</th>
                            <td id="tdConsequences" runat="server">0</td>
                        </tr>
                        <tr>
                            <th scope="row">Completeness of Application?</th>
                            <td id="tdCompleteness" runat="server">0</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th scope="row" style="text-align:right">Total</th>
                            <td id="tdTotal" runat="server">0</td>
                        </tr>
                    </tfoot>
                </table>
                <asp:Literal ID="ltrMultiScores" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <%--<div class="row">
            <div class="col-md-12">
                <h3>Checklist</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblNERIS" runat="server" Text="NERIS Compliant? *" AssociatedControlID="fldNERIS"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldNERIS" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbNERISYes" runat="server" Text="Yes" GroupName="NERIS" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbNERISNo" runat="server" Text="No" GroupName="NERIS" ClientIDMode="Static" />
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
        <div class="row">
            <hr />
        </div>--%>
        <div class="row" id="dvAdmin" runat="server">
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
                <h3>Counselor Signature</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-10 card card-body" id="dvESig" runat="server">

            </div>
        </div>
        <asp:UpdatePanel ID="upnlSignature" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="row formRow" id="dvAgreement" runat="server">
                    <div class="col-md-1">
                        <asp:CheckBox ID="chkAgreement" runat="server" AutoPostBack="true" OnCheckedChanged="chkAgreement_CheckedChanged" /><br />
                    </div>
                    <div class="col-md-9">
                        <asp:Label ID="lblAgreement" runat="server" AssociatedControlID="chkAgreement">I HAVE REVIEWED THIS APPLICATION...</asp:Label>
                    </div>
                </div>
                <div class="row" id="dvSignature" runat="server">
                    <div class="col-md-3">
                        <asp:TextBox ID="txtReviewer" runat="server" ClientIDMode="Static"></asp:TextBox><br />
                        <asp:Label ID="lblReviewer" runat="server" Text="Counselor Name" ClientIDMode="Static" AssociatedControlID="txtReviewer"></asp:Label>
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkAgreement" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
        
        <div class="row" id="dvAdminSignatureTable" runat="server" visible="false">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgSignatures" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgSignatures_NeedDataSource" OnPageIndexChanged="rgSignatures_PageIndexChanged">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
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
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;
            </div>
        </div>
        <asp:HiddenField ID="hfApplicationId" runat="server" />
        <asp:HiddenField ID="hfAppReadOnly" runat="server" Value="true" />
    </div>
</asp:Content>
