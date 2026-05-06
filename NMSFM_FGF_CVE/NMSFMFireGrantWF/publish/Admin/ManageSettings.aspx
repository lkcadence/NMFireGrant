<%@ Page Title="Fire Grant: Manage Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageSettings.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageSettings" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row" id="dvError" runat="server"></div>
        <h2>Manage Settings</h2>
        
        <div class="row">
            <div class="col-md-12">
                <h3>Program Settings</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblFiscalYear" runat="server" AssociatedControlID="ddlFiscalYear" Text="Fiscal Year *"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlFiscalYear" runat="server" aria-require="true" AutoPostBack="true" OnSelectedIndexChanged="ddlFiscalYear_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="txtStartDate" Text="Start Date *"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtStartDate" CssClass="datepicker" runat="server" TextMode="Date" aria-require="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblEndDate" runat="server" AssociatedControlID="txtEndDate" Text="End Date *"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtEndDate" CssClass="datepicker" runat="server" TextMode="Date" aria-require="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblMaxGrant" runat="server" AssociatedControlID="txtMaxGrant" Text="Maximum Grant Amount *"></asp:Label>
            </div>
            <div class="col-md-4">
                <telerik:RadNumericTextBox ID="txtMaxGrant" runat="server" MaxValue="100000000" MinValue="0" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," NumberFormat-DecimalSeparator="." aria-require="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblEsigText" runat="server" AssociatedControlID="txtEsigText" Text="eSignature Legal Text *"></asp:Label>
            </div>
            <div class="col-md-7">
                <asp:TextBox ID="txtEsigText" runat="server" TextMode="MultiLine" Rows="4" Width="100%" aria-require="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblFACertText" runat="server" AssociatedControlID="txtFACertification" Text="Fiscal Agent Certification Text *"></asp:Label>
            </div>
            <div class="col-md-7">
                <asp:Label ID="lblFACertification" runat="server" AssociatedControlID="txtFACertification" Text="I, as the fiscal agent for the {Department Name} department, certify..."></asp:Label><br />
                <asp:TextBox ID="txtFACertification" runat="server" TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save Program Settings" OnClick="btnSave_Click"/>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Eligibility Requirement Document</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Label ID="lblReqDoc" runat="server" AssociatedControlID="fuReqDoc" Text="Select Requirements Document"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:FileUpload ID="fuReqDoc" runat="server" />
            </div>
            <div class="col-md-3">
                <%--<asp:LinkButton ID="lnkReqDoc" runat="server" OnClick="lnkReqDoc_Click" Visible="false"></asp:LinkButton>--%>
                <asp:HyperLink ID="hlnkDocument" runat="server" Text="" Visible="false"></asp:HyperLink>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSaveReqDoc" CssClass="btn btn-primary" runat="server" Text="Save Requirements Document" OnClick="btnSaveReqDoc_Click" />
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Initial Page Content</h3>
                <p>Provide initial page content that the FD applicant will see in the welcome screen before entering into the application.</p>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblPageContent" runat="server" AssociatedControlID="rtbPageContent" Text="Initial Page Content *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbPageContent" SkinID="DefaultSetOfTools" Width="100%" Height="450px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSavePageContent" CssClass="btn btn-primary" runat="server" Text="Save Initial Page Content" OnClick="btnSavePageContent_Click"/>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Default Page Information</h3>
                <p>Provide default (home) page Information.</p>
            </div>
        </div>
         <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblDefaultPageHeader" runat="server" AssociatedControlID="rtbDefaultPageHeader" Text="Default Page Header *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbDefaultPageHeader" SkinID="DefaultSetOfTools" Width="100%" Height="200px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblDefaultPageContent" runat="server" AssociatedControlID="rtbDefaultPageContent" Text="Default Page Content *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbDefaultPageContent" SkinID="DefaultSetOfTools" Width="100%" Height="450px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSaveDefaultPageContent" CssClass="btn btn-primary" runat="server" Text="Save Default Page Content" OnClick="btnSaveDefaultPageContent_Click"/>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Eligibility Requirements</h3>
                <p>Edit the Eligibility Requirements</p>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblEligibilityRequirements" runat="server" AssociatedControlID="rtbEligibilityRequirements" Text="Eligibility Requirements *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbEligibilityRequirements" SkinID="DefaultSetOfTools" Width="100%" Height="450px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSaveEligibilityRequirements" CssClass="btn btn-primary" runat="server" Text="Save Eligibility Requirements" OnClick="btnSaveEligibilityRequirements_Click"/>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Pump and Hose Test Statutes</h3>
                <p>Provide the pump test statute for the selected application year</p>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblPumpTestStatute" runat="server" AssociatedControlID="rtbPumpTestStatute" Text="Pump Test Requirements Text *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbPumpTestStatute" SkinID="DefaultSetOfTools" Width="75%" Height="200px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:Label ID="lblHoseTestStatute" runat="server" AssociatedControlID="rtbHoseTestStatute" Text="Hose Test Requirements Text *"></asp:Label><br />
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="rtbHoseTestStatute" SkinID="DefaultSetOfTools" Width="75%" Height="200px" ContentAreaCssFile="~/Content/EditorStyles.css" StripFormattingOptions="MSWordRemoveAll" ToolsFile="~/Content/NMSFMBasicTools.xml" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnSaveTestStatutes" CssClass="btn btn-primary" runat="server" Text="Save Pump/Hose Test Statutes" OnClick="btnSaveTestStatutes_Click"/>
            </div>
        </div>
    </div>
    
    <asp:HiddenField ID="hfProgramSettings" runat="server" Value="" />
</asp:Content>
