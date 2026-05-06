<%@ Page Title="Fire Grant: Manage Help Text" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageHelpText.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.ManageHelpText" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Manage Help Sections</h2>
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Help Sections</h3>
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
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("HelpId") %>'>
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
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#helpModal">
                    Add Help Section
                </button>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="helpModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblHelpHeader" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin:200px auto !important">
            <div class="modal-content">
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
                            <asp:Label ID="lblPage" runat="server" AssociatedControlID="ddlPage" Text="Page: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlPage" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            
                        </div>
                        <div class="col-sm-3" id="dvPage" style="display:none">
                            <asp:TextBox ID="txtPage" runat="server" ClientIDMode="Static" MaxLength="50" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblSection" runat="server" AssociatedControlID="txtSection">Section: <span style='font-size:smaller;'><i>(Leave blank for page help)</i></span></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtSection" runat="server" Width="250px" ClientIDMode="Static" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblHelpNumber" runat="server" AssociatedControlID="txtHelpNumber" Text="Help Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtHelpNumber" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblHelpImage" runat="server" AssociatedControlID="ruHelpImage" Text="Help Image:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <telerik:RadAsyncUpload ID="ruHelpImage" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1">
                                <FileFilters>
                                    <telerik:FileFilter Description="Images(jpeg;jpg;gif;png)" Extensions="jpeg,jpg,gif,png" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>
                     <div class="row formRow" id="dvHelpDocLink" runat="server" visible="false">
                        <div class="col-md-3">
                            <asp:Label ID="lblHelpImageLink" runat="server" AssociatedControlID="imagexa" Text="Current Help Image:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <%--<asp:LinkButton ID="lnkHelpImage" runat="server" Text="View Help Image" OnClick="lnkHelpImage_Click"></asp:LinkButton>--%>
                            <img id="imagexa" runat="server" width="250" src="" visible="false"/>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblHelpText" runat="server" AssociatedControlID="txtHelpText">
                                Enter Help Text
                            </asp:Label>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-md-12">
                            <telerik:RadEditor RenderMode="Lightweight" runat="server" ID="txtHelpText" SkinID="DefaultSetOfTools" Width="100%" Height="250px" ToolsFile="~/Content/NMSFMBasicTools.xml" aria-required="true" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd">
                            </telerik:RadEditor>  
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="lblHelpInactive" runat="server" AssociatedControlID="chkHelpInactive" Text="Inactive:"></asp:Label>
                        </div>
                        <div class="col-sm-1">
                            <asp:Checkbox ID="chkHelpInactive" runat="server" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Label ID="lblAdminOnly" runat="server" AssociatedControlID="chkAdminOnly" Text="Admin Only:"></asp:Label>
                        </div>
                        <div class="col-sm-1">
                            <asp:Checkbox ID="chkAdminOnly" runat="server" />
                        </div>
                    </div>
                    <asp:HiddenField ID="hfHelpId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <%--<button id="btnDeletePriority" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeletePriority_ServerClick">Inactivate Priority</button>--%>
                    <button id="btnSaveHelp" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveHelp_ServerClick">Save Help Section</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            //showPriorityDelete();
            showOther();
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

        function showOther() {
            var page = $("#ddlPage").val();
            if (page === '-Other-') {
                $("#dvPage").show();
            }
            else {
                $("#dvPage").hide();
            }
        }

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
            var editor = $find("<%=txtHelpText.ClientID%>");
            editor.set_html("");
            $('#MainContent_imagexa').attr('src', '');
            $('#MainContent_dvHelpDocLink').hide();
            $('#ddlPage').val("");
            //$('#MainContent_btnDeleteHelp').hide();
        }

        $("#ddlPage").change(function () {
            showOther();
        });
    </script>
</asp:Content>
