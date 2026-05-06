<%@ Page Title="Fire Grant: Registration Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationConfirmation.aspx.cs" Inherits="NMSFMFireGrantWF.Account.RegistrationConfirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <div>
        <p>Your registration has been successfully submitted. Please wait for a confirmation email activating your account before logging in.</p>
    </div>
</asp:Content>
