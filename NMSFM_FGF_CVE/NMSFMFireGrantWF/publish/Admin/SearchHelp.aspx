<%@ Page Title="Fire Grant: Manage Help Text" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchHelp.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.SearchHelp" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Search Help Content</h2>
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Help Content</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Search Help:
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblSearchPage" runat="server" Text="Select Page" AssociatedControlID="ddlSearchPage"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlSearchPage" runat="server" Width="95%"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblSearchContent" runat="server" Text="Search Help Content" AssociatedControlID="txtSearchContent"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtSearchContent" runat="server" Width="95%"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"/>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgHelp" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="25" OnNeedDataSource="rgHelp_NeedDataSource" OnPageIndexChanged="rgHelp_PageIndexChanged" OnItemDataBound="rgHelp_ItemDataBound" OnItemCommand="rgHelp_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View Help" CommandName="View" CommandArgument='<%# Eval("HelpId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Page" FilterControlAltText="Filter Page column" HeaderText="Page" UniqueName="Page">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Section" FilterControlAltText="Filter Section column" HeaderText="Section" UniqueName="Section">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Inactive" FilterControlAltText="Filter Inactive column" HeaderText="Inactive" UniqueName="Inactive">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="HelpId" FilterControlAltText="Filter HelpId column" HeaderText="HelpId" UniqueName="HelpId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
        <div class="row" style="display:none">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#helpModal">
                    Add Help Section
                </button>
            </div>
        </div>
        <!-- Modal -->
    <div class="modal fade" id="helpModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblHelpHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content" style="width:800px">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblHelpHeader">Help Section</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblHelpError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblPage" runat="server" AssociatedControlID="ltrPage" Text="Page: *"></asp:Label>
                        </div>
                        <div class="col-sm-3" id="dvPage">
                            <asp:Literal ID="ltrPage" runat="server" ClientIDMode="Static"></asp:Literal>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblSection" runat="server" AssociatedControlID="lblSection" Text="Section:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:Literal ID="ltrSection" runat="server" ClientIDMode="Static"></asp:Literal>
                        </div>
                    </div>
                     <div class="row formRow" id="dvHelpDocLink" runat="server" visible="false">
                        <div class="col-md-3">
                            <asp:Label ID="lblHelpImageLink" runat="server" AssociatedControlID="imagexa" Text="Current Help Image:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <%--<asp:LinkButton ID="lnkHelpImage" runat="server" Text="View Help Image" OnClick="lnkHelpImage_Click"></asp:LinkButton>--%>
                            <img id="imagexa" runat="server" width="500" src="" visible="false"/>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblHelpText" runat="server" AssociatedControlID="ltrHelpText">
                                Help Text
                            </asp:Label>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-12">
                            <asp:Literal ID="ltrHelpText" runat="server" ClientIDMode="Static"></asp:Literal>
                        </div>
                    </div>
                    <div class="row">

                    </div>
                    <asp:HiddenField ID="hfHelpId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <%--<button id="btnDeletePriority" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeletePriority_ServerClick">Inactivate Priority</button>--%>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

        });
        
        //function showPriorityDelete() {
        //    var noteId = $('#hfHelpId').val();
        //    if (noteId === "") {
        //        $('#MainContent_btnDeleteHelp').hide();
        //    }
        //    else {
        //        $('#MainContent_btnDeleteHelp').show();
        //    }
        //}

        function openHelpModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            //$('#MainContent_btnDeleteHelp').show();
        }

        function clearNoteId() {
            $('#hfHelpId').val('');
            $('#txtPage').val('');
            $('#txtSection').val('');
            $('#txtHelpNumber').val('');
            $('#txtHelpText').val('');
            //$('#MainContent_btnDeleteHelp').hide();
        }

    </script>
</asp:Content>
