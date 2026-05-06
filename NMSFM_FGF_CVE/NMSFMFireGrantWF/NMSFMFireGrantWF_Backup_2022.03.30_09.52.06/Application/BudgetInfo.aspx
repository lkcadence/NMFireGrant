<%@ Page Title="Fire Grant Application: Budget Information" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="BudgetInfo.aspx.cs" Inherits="NMSFMFireGrantWF.Application.BudgetInfo" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-6">
                <asp:Label ID="lblOperatingBudget" runat="server" Text="What is your fire departments operating budget, including personnel costs, for your current fiscal year (in dollars)? *" AssociatedControlID="txtOperatingBudget"></asp:Label>
            </div>
            <div class="col-md-6">
                <telerik:RadNumericTextBox ID="txtOperatingBudget" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-6">
                <asp:Label ID="lblCurrentDistribution" runat="server" Text="What is the current Protection Fire Fund distribution? *" AssociatedControlID="txtCurrentDistribution"></asp:Label>
            </div>
            <div class="col-md-6">
                <telerik:RadNumericTextBox ID="txtCurrentDistribution" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-6">
                <asp:Label ID="Label1" runat="server" Text="What is the total stipend carryover? *" AssociatedControlID="txtStipendCarryover"></asp:Label>
            </div>
            <div class="col-md-6">
                <telerik:RadNumericTextBox ID="txtStipendCarryover" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-6">
                <asp:Label ID="lblCurrentCarryoverBal" runat="server" Text="What is the approved total carryover balance, if any, of Protection Fire Funds maintained by the department? *" AssociatedControlID="txtCurrentCarryoverBal"></asp:Label>
            </div>
            <div class="col-md-6">
                <telerik:RadNumericTextBox ID="txtCurrentCarryoverBal" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="$0.00" Type="Currency" aria-required="true" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow" id="dvCarryoverPurpose">
            <div class="col-md-6">
                <asp:Label ID="lblCarryoverPurpose" runat="server" Text="What was the purpose of the approved carryover? *" AssociatedControlID="txtCarryoverPurpose"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtCarryoverPurpose" runat="server" CssClass="form-control" class="form-control" TextMode="MultiLine" Rows="10" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <label>What percentage of your annual operating budget is derived from:</label>
                </fieldset>
                <div class="container">
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblTaxesPer" runat="server" Text="Taxes" AssociatedControlID="txtTaxesPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtTaxesPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblGrantsPer" runat="server" Text="Grants" AssociatedControlID="txtGrantsPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtGrantsPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblSFMFundsPer" runat="server" Text="State Fire Marshal Funds" AssociatedControlID="txtSFMFundsPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtSFMFundsPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblDonationsPer" runat="server" Text="Donations" AssociatedControlID="txtDonationsPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtDonationsPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblFundDrivesPer" runat="server" Text="FundDrives" AssociatedControlID="txtFundDrivesPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtFundDrivesPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblFeeForServicePer" runat="server" Text="Fee For Service" AssociatedControlID="txtFeeForServicePer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtFeeForServicePer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblOthers" runat="server" Text="Others" AssociatedControlID="txtOthers"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtOthers" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ClientIDMode="Static" Type="Percent"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvOthersExp">
                        <div class="col-md-3">
                            <asp:Label ID="lblOthersExp" runat="server" Text="Please Explain (Others)" AssociatedControlID="txtOtherExp"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtOtherExp" runat="server" CssClass="form-control" class="form-control" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-3">
                            <asp:Label ID="lblTotalPer" runat="server" Text="Total Percentage *" AssociatedControlID="txtTotalPer"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <telerik:RadNumericTextBox ID="txtTotalPer" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="2" EmptyMessage="0" ReadOnly="true" ClientIDMode="Static" Type="Percent" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
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
        ShowOthersExp();
        calcPerc();
        ShowCarryoverPurpose();

        function ShowCarryoverPurpose() {
            var sh = $('#txtCurrentCarryoverBal').val().replace('$', '').replace(',', '');
            var show = Number(sh);
            if (show > 0) {
                $('#dvCarryoverPurpose').show();
            }
            else {
                $('#dvCarryoverPurpose').hide();
                $("#ApplicationContent_txtCarryoverPurpose").val('');
            }
        }

        function ShowOthersExp() {
            var sh = $('#txtOthers').val().replace('%', '');
            var show = Number(sh);
            if (show > 0) {
                $('#dvOthersExp').show();
            }
            else {
                $("#ApplicationContent_txtOtherExp").val("");
                $('#dvOthersExp').hide();
            }
        }

        function calcPerc() {
            var taxes = Number($("#txtTaxesPer").val().replace('%', ''));
            var grants = Number($("#txtGrantsPer").val().replace('%', ''));
            var smffunds = Number($("#txtSFMFundsPer").val().replace('%', ''));
            var donations = Number($("#txtDonationsPer").val().replace('%', ''));
            var funddrives = Number($("#txtFundDrivesPer").val().replace('%', ''));
            var feeforserv = Number($("#txtFeeForServicePer").val().replace('%', ''));
            var others = Number($("#txtOthers").val().replace('%', ''));

            var total = taxes + grants + smffunds + donations + funddrives + feeforserv + others;
            $("#txtTotalPer").val(total.toFixed(2).concat('%'));
        }

        $("#txtCurrentCarryoverBal").keyup(function () {
            ShowCarryoverPurpose();
        });

        $("#txtOthers").keyup(function () {
            ShowOthersExp();
            calcPerc();
        });

        $("#txtTaxesPer,#txtGrantsPer,#txtSFMFundsPer,#txtDonationsPer,#txtFundDrivesPer,#txtFeeForServicePer").keyup(function () {
            calcPerc();
        });

    </script>
</asp:Content>
