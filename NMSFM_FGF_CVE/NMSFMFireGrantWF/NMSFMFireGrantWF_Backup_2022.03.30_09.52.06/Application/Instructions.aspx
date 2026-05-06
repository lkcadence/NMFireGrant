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
                <asp:Button ID="btnAccept" runat="server" Text="Click Here to Start Filling out the Application" OnClick="btnAccept_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Literal ID="ltrInstructions" runat="server" Text=""></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnAccept2" runat="server" Text="Click Here to Start Filling out the Application" OnClick="btnAccept_Click" />
            </div>
        </div>
        
    </div>
    <script type="text/javascript">
        function disableLogin() {
            $("#btnAccept2").prop('disabled', true);
            $("#btnAccept").prop('disabled', true);
        }
        window.onbeforeunload = disableLogin;
    </script>
</asp:Content>
