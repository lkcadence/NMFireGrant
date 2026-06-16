<%@ Page Title="Fire Grant Application: Response History (Last Year)" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="ResponseHistory.aspx.cs" Inherits="NMSFMFireGrantWF.Application.ResponseHistory" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow" id="dvHoseTests">
            <div class="col-md-4">
                <asp:Label ID="lblNERISCurrent" runat="server" Text="Are you NERIS Current? *" AssociatedControlID="fldNERISCurrent"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldNERISCurrent" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbNERISCurrentYes" runat="server" Text="Yes" GroupName="NERISCurrent" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbNERISCurrentNo" runat="server" Text="No" GroupName="NERISCurrent" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvAidAgreementsHeader">
            <div class="col-md-12">
                <h3>How many reponses per category?</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblStructure" runat="server" Text="Structure Fire (IT 110-118, 120-123)" AssociatedControlID="txtStructure"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtStructure" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," MinValue="0" EmptyMessage="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblVehicle" runat="server" Text="Vehicle Fire (IT 130-138)" AssociatedControlID="txtVehicle"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtVehicle" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblVegitation" runat="server" Text="Vegitation Fire (IT 140-143)" AssociatedControlID="txtVegitation"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtVegitation" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblEMS" runat="server" Text="EMS (IT 300-323)" AssociatedControlID="txtEMS"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtEMS" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblRescue" runat="server" Text="Rescue (IT 331-381)" AssociatedControlID="txtRescue"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtRescue" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblHazardous" runat="server" Text="Hazardous Condition (IT 400-482)" AssociatedControlID="txtHazardous"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtHazardous" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblServiceCall" runat="server" Text="Service Calls (IT 500-571)" AssociatedControlID="txtServiceCall"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtServiceCall" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblGoodIntent" runat="server" Text="Good Intent Calls (IT 600-671)" AssociatedControlID="txtGoodIntent"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtGoodIntent" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblFalseCalls" runat="server" Text="False Calls (IT 700-751)" AssociatedControlID="txtFalseCalls"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtFalseCalls" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblOther" runat="server" Text="Other" AssociatedControlID="txtOther"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtOther" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblTotal" runat="server" Text="Total Calls (must be greater than or equal to zero) *" AssociatedControlID="txtTotal"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtTotal" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ReadOnly="true" ClientIDMode="Static"></telerik:RadNumericTextBox>
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
    <script type="text/javascript">
        calcTotal();

        function calcTotal() {
            var structure = Number($("#txtStructure").val().replace(',', ''));
            var vehicle = Number($("#txtVehicle").val().replace(',', ''));
            var vegitation = Number($("#txtVegitation").val().replace(',', ''));
            var ems = Number($("#txtEMS").val().replace(',', ''));
            var rescue = Number($("#txtRescue").val().replace(',', ''));
            var hazardous = Number($("#txtHazardous").val().replace(',', ''));
            var servicecalls = Number($("#txtServiceCall").val().replace(',', ''));
            var goodintent = Number($("#txtGoodIntent").val().replace(',', ''));
            var falsecalls = Number($("#txtFalseCalls").val().replace(',', ''));
            var others = Number($("#txtOther").val().replace(',', ''));

            var total = structure + vehicle + vegitation + ems + rescue + hazardous + servicecalls + goodintent + falsecalls + others;
            $("#txtTotal").val(total);
        }

        $("#txtStructure,#txtVehicle,#txtVegitation,#txtEMS,#txtRescue,#txtHazardous,#txtServiceCall,#txtGoodIntent,#txtFalseCalls,#txtOther").keyup(function () {
            calcTotal();
        });
    </script>
</asp:Content>
