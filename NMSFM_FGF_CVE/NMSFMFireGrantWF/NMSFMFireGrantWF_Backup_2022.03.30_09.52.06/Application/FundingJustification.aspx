<%@ Page Title="Fire Grant Application: Funding Justification" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="FundingJustification.aspx.cs" Inherits="NMSFMFireGrantWF.Application.FundingJustification" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblCriticalNeed" runat="server" Text="Project is a critical need? *" AssociatedControlID="fldCriticalNeed"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldCriticalNeed" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbCriticalNeedYes" runat="server" Text="Yes" GroupName="CriticalNeed" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbCriticalNeedNo" runat="server" Text="No" GroupName="CriticalNeed" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblFinancialNeed" runat="server" AssociatedControlID="txtFinancialNeed">
                    Financial Need: *<span style="font-weight:normal"> In this section, describe the department’s current funding issues. Does the department currently have debt? If so, describe. 
                    Does the department have Fire Protection carry-over funds? If so, for what purpose and are any of the carryover funds being used to assist in the proposed grant 
                    project/purchase(s)? How will the department satisfy the amount in excess of the funds awarded to complete the project?</span>
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-10">
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="txtFinancialNeed" SkinID="DefaultSetOfTools" Width="100%" Height="350px" ToolsFile="~/Content/NMSFMBasicTools.xml" aria-required="true" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-10">
                
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblProblem" runat="server" AssociatedControlID="txtProblem">
                    Problem: *<span style="font-weight:normal"> Describe in detail, the problem the department is addressing with this grant request and the impact on effective service delivery.</span> 
                </asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="txtProblem" SkinID="DefaultSetOfTools" Width="100%" Height="350px" ToolsFile="~/Content/NMSFMBasicTools.xml" aria-required="true" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblBenefit" runat="server" AssociatedControlID="txtBenefit">
                   Benefit to the Community: *<span style="font-weight:normal"> Describe in detail, how the community served will be impacted by this award. </span>
                </asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="txtBenefit" SkinID="DefaultSetOfTools" Width="100%" Height="350px" ToolsFile="~/Content/NMSFMBasicTools.xml" aria-required="true" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>  
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblConsequences" runat="server" AssociatedControlID="txtConsequences">
                    Consequences: *<span style="font-weight:normal"> Describe how the department will address the problem described above if this request is not funded. </span>
                </asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="txtConsequences" SkinID="DefaultSetOfTools" Width="100%" Height="350px" ToolsFile="~/Content/NMSFMBasicTools.xml" aria-required="true" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                </telerik:RadEditor>
            </div>
        </div>
        <div id="dvAdmin" runat="server">
            <div class="row">
                <hr />
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblFinancialNeedGrade" runat="server" Text="Financial Need Grade?" AssociatedControlID="txtFinancialNeedGrade"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtFinancialNeedGrade" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MaxValue="100"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblProblemGrade" runat="server" Text="Problem Grade?" AssociatedControlID="txtProblemGrade"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtProblemGrade" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MaxValue="100"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblBenefitGrade" runat="server" Text="Benefit to the Community Grade?" AssociatedControlID="txtBenefitGrade"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtBenefitGrade" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MaxValue="100"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row formRow">
                <div class="col-md-3">
                    <asp:Label ID="lblConsequencesGrade" runat="server" Text="Consequences Grade?" AssociatedControlID="txtConsequencesGrade"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtConsequencesGrade" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MaxValue="100"></telerik:RadNumericTextBox>
                </div>
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
        <asp:HiddenField ID="hfApplicationId" runat="server" />
    </div>
</asp:Content>
