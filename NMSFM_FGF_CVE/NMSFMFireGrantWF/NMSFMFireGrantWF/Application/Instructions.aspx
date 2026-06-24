<%@ Page Title="Fire Grant Application: Instructions" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="Instructions.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Instructions" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row">
            <div class="col-md-12">
                <asp:Literal ID="ltrInstructions" runat="server" Mode="PassThrough" Text=""></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row formRow" id="dvAcknowledgment" runat="server">
            <div class="col-md-1">
                <asp:CheckBox ID="chkInstructionsRead" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-md-9">
                <asp:Label ID="lblInstructionsAck" runat="server" ClientIDMode="Static"
                    AssociatedControlID="chkInstructionsRead"
                    Text="I have read and understand the application instructions above." />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnContinue" runat="server" ClientIDMode="Static"
                    CssClass="btn btn-primary" Text="Continue to Application"
                    OnClick="btnContinue_Click" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function updateContinueButton() {
            if ($('#dvAcknowledgment').is(':visible')) {
                $('#btnContinue').prop('disabled', !$('#chkInstructionsRead').is(':checked'));
            }
        }

        function disableContinue() {
            $('#btnContinue').prop('disabled', true);
        }

        $(document).ready(function () {
            updateContinueButton();
            $('#chkInstructionsRead').change(updateContinueButton);
        });

        window.onbeforeunload = disableContinue;
    </script>
</asp:Content>
