<%@ Page Title="Fire Grant Application: Project Budget Sheet" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="ProjectBudgetSheet.aspx.cs" Inherits="NMSFMFireGrantWF.Application.ProjectBudgetSheet" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row" id="dvEquipment">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgEquipment" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" ShowFooter="true" OnNeedDataSource="rgEquipment_NeedDataSource" OnPageIndexChanged="rgEquipment_PageIndexChanged">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PriorityCategory" FilterControlAltText="Filter Priority Category column" HeaderText="Priority Category" UniqueName="PriorityCategory">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EquipmentNeeded" FilterControlAltText="Filter Equipment Needed column" HeaderText="Equipment Needed" UniqueName="EquipmentNeeded">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column" HeaderText="Quantity" UniqueName="Quantity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Cost" FilterControlAltText="Filter Cost column" HeaderText="Cost" UniqueName="Cost" DataFormatString="{0:C}" Aggregate="Sum" FooterAggregateFormatString="{0:C}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EquipmentId" FilterControlAltText="Filter EquipmentId column" HeaderText="EquipmentId" UniqueName="EquipmentId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblWhatPurchased" runat="server" Text="What specifically will you purchase if awarded this grant?" AssociatedControlID="txtWhatPurchased"></asp:Label>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:TextBox ID="txtWhatPurchased" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Width="100%" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblEquipments" runat="server" Text="Will fullfiling this need impact your organization's ISO rating?" AssociatedControlID="fldISORating"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldISORating" runat="server">
                    <asp:RadioButton ID="rbISORatingYes" runat="server" Text="Yes" GroupName="ISORating" ClientIDMode="Static" Enabled="false" />&nbsp;
                    <asp:RadioButton ID="rbISORatingNo" runat="server" Text="No" GroupName="ISORating" ClientIDMode="Static" Enabled="false" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvISOExplanation">
            <div class="col-md-2">
                <asp:Label ID="lblISOExplanation" runat="server" Text="Please Explain" AssociatedControlID="txtISOExplanation"></asp:Label><br />
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtISOExplanation" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Width="100%" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Provide information on your estimated budget</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="container container-fluid" style="width:auto;">
                    <div class="row formRow">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblTotalAmount" runat="server" AssociatedControlID="txtTotalAmount" Text="Total Project Cost: *"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number"></telerik:RadNumericTextBox>
                            <asp:HiddenField ID="hfEquipmentCost" runat="server" />
                        </div>
                    </div>
                    <%--<div class="row">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblMatchingAmount" runat="server" AssociatedControlID="txtMatchingAmount" Text=" Less matching amount:"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtMatchingAmount" runat="server" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblSubTotal" runat="server" AssociatedControlID="txtSubTotal" Text="SubTotal:"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtSubTotal" runat="server" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number"></telerik:RadNumericTextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblAmountRequested" runat="server" AssociatedControlID="txtAmountRequested" Text="Grant Amount Requested: *"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtAmountRequested" runat="server" CssClass="form-control" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <%--<div class="row">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblDeptResponsibilityAbove" runat="server" AssociatedControlID="txtDeptResponsibilityAbove" Text="Department is responsible for funding needs exceeding $400,000.00:"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtDeptResponsibilityAbove" runat="server" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number"></telerik:RadNumericTextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblTotalDeptResp" runat="server" AssociatedControlID="txtTotalDeptResp" Text="Total amount the Department is responsible for:"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtTotalDeptResp" runat="server" CssClass="form-control" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" ReadOnly="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row">
                        <hr />
                    </div>
                    <div class="row">
                        <div class="col-md-9" style="text-align:right">
                            <asp:Label ID="lblStipendAmountRequested" runat="server" AssociatedControlID="txtStipendAmountRequested" Text="Stipend Amount Requested: *"></asp:Label>
                        </div>
                        <div class="col-md-3" style="text-align:right">
                            <telerik:RadNumericTextBox ID="txtStipendAmountRequested" runat="server" CssClass="form-control" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-md-8">
                <h3>Note: You are almost complete and few steps left to submit the application for review:</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <ol>
                    <li>
                        Save project budget sheet.
                    </li>
                    <li>
                        Verify all the information entered in this application is complete.
                    </li>
                    <li>
                        Obtain commitment statement signed by the Fiscal Agent.
                    </li>
                    <li>
                        Scan the signed application and upload in the next step - “Fiscal Agent”.
                    </li>
                </ol>
            </div>
        </div>--%>
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
        <asp:HiddenField ID="hfApplicationId" runat="server" />
    </div>
    <script type="text/javascript">
        calcBudget();

        function calcBudget() {
            var projectCost = Number($("#txtTotalAmount").val().replace('$', '').replace(',',''));
            var grantRequest = Number($("#txtAmountRequested").val().replace('$', '').replace(',', ''));
            var departmentResponsibility = projectCost - grantRequest;
            departmentResponsibility = departmentResponsibility.toFixed(2);
            var deptRespVal = numberWithCommas(departmentResponsibility);
            $("#txtTotalDeptResp").val(deptRespVal);
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }

        $("#txtTotalDeptResp").keyup(function () {
            calcBudget()
        });
    </script>
</asp:Content>
