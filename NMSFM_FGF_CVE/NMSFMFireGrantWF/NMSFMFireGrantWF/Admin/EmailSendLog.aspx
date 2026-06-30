<%@ Page Title="Fire Grant: Email Send Log" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailSendLog.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.EmailSendLog" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .email-log-error {
            max-width: 420px;
            word-break: break-word;
            white-space: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row" id="dvError" runat="server"></div>
        <h2>Email Send Log</h2>
        <p>Recent outbound application emails and SMTP test tool.</p>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:CheckBox ID="chkFailedEmailLogsOnly" runat="server" Text="Show failed sends only" AutoPostBack="true" OnCheckedChanged="chkFailedEmailLogsOnly_CheckedChanged" />
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:GridView ID="gvEmailSendLog" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" EmptyDataText="No email send log entries found.">
                    <Columns>
                        <asp:BoundField DataField="DateInserted" HeaderText="Sent" DataFormatString="{0:g}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="To" HeaderText="To" />
                        <asp:BoundField DataField="Subject" HeaderText="Subject" />
                        <asp:BoundField DataField="SentByLogin" HeaderText="Sent By" />
                        <asp:BoundField DataField="SentByEmail" HeaderText="Sender Email" />
                        <asp:BoundField DataField="ContextType" HeaderText="Context" />
                        <asp:BoundField DataField="FailReason" HeaderText="Error" HtmlEncode="true" ItemStyle-CssClass="email-log-error" ItemStyle-Wrap="true" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Button ID="btnRefreshEmailLog" CssClass="btn btn-default" runat="server" Text="Refresh Log" OnClick="btnRefreshEmailLog_Click" />
            </div>
            <div class="col-md-3">
                <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#purgeEmailLogModal">Purge Logs</button>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-3">
                <asp:Label ID="lblEmailTestTo" runat="server" AssociatedControlID="txtEmailTestTo" Text="Email test To"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtEmailTestTo" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-3">
                <asp:Button ID="btnEmailTest" CssClass="btn btn-primary" runat="server" Text="Send Test Email" OnClick="btnEmailTest_Click" />
            </div>
        </div>
    </div>
    <div class="modal fade" id="purgeEmailLogModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblPurgeEmailLogHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="lblPurgeEmailLogHeader">Purge Email Send Log</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>Select how much of the email send log to delete.</p>
                    <div class="row formRow">
                        <div class="col-sm-4">
                            <asp:Label ID="lblPurgeRetention" runat="server" AssociatedControlID="ddlPurgeRetention" Text="Delete entries:"></asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlPurgeRetention" runat="server" CssClass="form-control">
                                <asp:ListItem Text="All (entire log)" Value="All" />
                                <asp:ListItem Text="Older than 10 days" Value="10" />
                                <asp:ListItem Text="Older than 20 days" Value="20" />
                                <asp:ListItem Text="Older than 30 days" Value="30" Selected="True" />
                                <asp:ListItem Text="Older than 60 days" Value="60" />
                                <asp:ListItem Text="Older than 90 days" Value="90" />
                                <asp:ListItem Text="Older than 120 days" Value="120" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnPurgeConfirm" CssClass="btn btn-warning" runat="server" Text="OK" OnClick="btnPurgeConfirm_Click" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
