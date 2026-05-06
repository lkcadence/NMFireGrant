<%@ Page Title="Fire Grant Application: Equipment Needs" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="EquipmentNeeds.aspx.cs" Inherits="NMSFMFireGrantWF.Application.EquipmentNeeds" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblWhatPurchased" runat="server" Text="What specifically will you purchase if this grant is awarded? *" AssociatedControlID="txtWhatPurchased"></asp:Label>
                <span style="color:red">Note: Only a single project can be requested.</span>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-12">
                <asp:TextBox ID="txtWhatPurchased" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="dvEquipmentEquipHead">
            <div class="col-md-12">
                <h3>Add Estimated Equipment Cost</h3>
            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#equipmentModal">
                    Add Equipment
                </button>
            </div>
        </div>
        <div class="row" id="dvEquipment">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgEquipment" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" ShowFooter="true" OnNeedDataSource="rgEquipment_NeedDataSource" OnPageIndexChanged="rgEquipment_PageIndexChanged" OnItemDataBound="rgEquipment_ItemDataBound" OnItemCommand="rgEquipment_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("EquipmentId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
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
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblEquipments" runat="server" Text="Will fullfiling this need impact your organization's ISO rating? *" AssociatedControlID="fldISORating"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldISORating" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbISORatingYes" runat="server" Text="Yes" GroupName="ISORating" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbISORatingNo" runat="server" Text="No" GroupName="ISORating" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvISOExplanation">
            <div class="col-md-2">
                <asp:Label ID="lblISOExplanation" runat="server" Text="Please Explain *" AssociatedControlID="txtISOExplanation"></asp:Label><br />
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtISOExplanation" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Width="100%" aria-required="true"></asp:TextBox>
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
                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
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
    <!-- Modal -->
    <div class="modal fade" id="equipmentModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblEquipmentHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblEquipmentHeader">Add Equipment</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblEquipmentError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblEquipmentNumber" runat="server" AssociatedControlID="txtEquipmentNumber" Text="Equipment Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtEquipmentNumber" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                        
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblPriorityCategory" runat="server" AssociatedControlID="ddlPriorityCategory" Text="Priority Category: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlPriorityCategory" CssClass="form-control" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPriorityCategory_SelectedIndexChanged">
                                
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblEquipmentNeeded" runat="server" AssociatedControlID="ddlEquipmentNeeded" Text="Equipment Needed: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlEquipmentNeeded" CssClass="form-control" runat="server" ClientIDMode="Static">
                                
                            </asp:DropDownList>
                        </div>
                    </div>
                            
                    
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblEquipmentQty" runat="server" AssociatedControlID="txtEquipmentQty" Text="Quantity: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtEquipmentQty" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblEquipmentCost" runat="server" AssociatedControlID="txtEquipmentCost" Text="Total Cost of Equipment: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <telerik:RadNumericTextBox ID="txtEquipmentCost" runat="server" ClientIDMode="Static" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfEquipmentId" runat="server" ClientIDMode="Static" Value="" />
                </div>
                <div class="modal-footer">
                    <button id="btnDeleteEquipment" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteEquipment_ServerClick">Delete Equipment</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveEquipment" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveEquipment_ServerClick">Save Equipment</button>
                </div>
            </div>
        </div>
    </div>
            
    <script type="text/javascript">

        $(document).ready(function () {
            showEquipmentDelete();
            showISOExplanation();
        });


        function showISOExplanation() {
            var show = $('#rbISORatingYes').prop('checked');
            if (show == true) {
                $('#dvISOExplanation').fadeIn("slow");
            }
            else {
                $('#dvISOExplanation').fadeOut("slow");
                $('#txtISOExplanation').val("");
            }
        }

        function showEquipmentDelete() {
            var noteId = $('#hfEquipmentId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteEquipment').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteEquipment').show();
            }
        }

        function openEquipmentModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteEquipment').show();
        }

        function openModal() {
            $('#equipmentModal').modal('show');
            $('#ddlddlPriorityCategory').focus();
        }

        function clearNoteId() {
            $('#hfEquipmentId').val('');
            $('#txtEquipmentNumber').val('');
            $('#ddlPriorityCategory').val('');
            $('#ddlEquipmentNeeded').val('');
            $('#txtEquipmentQty').val('');
            $('#txtEquipmentCost').val('');
            $('#ApplicationContent_btnDeleteEquipment').hide();
        }

        $('#rbISORatingYes,#rbISORatingNo').change(function () {
            showISOExplanation();
        });

    </script>
</asp:Content>
