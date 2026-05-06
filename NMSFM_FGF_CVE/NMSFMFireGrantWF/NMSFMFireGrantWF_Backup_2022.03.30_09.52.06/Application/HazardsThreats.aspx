<%@ Page Title="Fire Grant Application: Hazards/Threats" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="HazardsThreats.aspx.cs" Inherits="NMSFMFireGrantWF.Application.HazardsThreats" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row" id="dvCommunicationEquipHead">
                <div class="col-md-12">
                    <h3>Describe the threat to the community:</h3>
                    <p> (i.e., fuel storage bulk plants, railroads, high hazard occupancies, etc.)</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3" id="dvShowModal" runat="server">
                    <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#hazardModal">
                        Add Hazard/Threat
                    </button>
                </div>
            </div>
            <div class="row" id="dvCommunicationEquip">
                <div class="col-md-12">
                    <telerik:RadGrid ID="rgHazards" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgHazards_NeedDataSource" OnPageIndexChanged="rgHazards_PageIndexChanged" OnItemDataBound="rgHazards_ItemDataBound" OnItemCommand="rgHazards_ItemCommand">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True">
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("HazardId") %>'>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HazardType" FilterControlAltText="Filter Hazard Type column" HeaderText="Hazard Type" UniqueName="HazardType">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HazardDetail" FilterControlAltText="Filter Hazard Detail column" HeaderText="Hazard Detail" UniqueName="HazardDetail">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HazardId" FilterControlAltText="Filter HazardId column" HeaderText="HazardId" UniqueName="HazardId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
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
    <!-- Modal -->
    <div class="modal fade" id="hazardModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblHazardHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblHazardHeader">Add Hazard/Threat</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblCommunicationError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblHazardNumber" runat="server" AssociatedControlID="txtHazardNumber" Text="Hazard Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtHazardNumber" runat="server" class="form-control" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblHazardType" runat="server" AssociatedControlID="txtHazardType" Text="Hazard Type: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtHazardType" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblHazardDetail" runat="server" AssociatedControlID="txtHazardDetail" Text="Hazard Detail: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtHazardDetail" runat="server" class="form-control" ClientIDMode="Static" TextMode="MultiLine" Rows="5" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfHazardnId" runat="server" ClientIDMode="Static" Value="" />
                </div>
                
                <div class="modal-footer">
                    <button id="btnDeleteHazard" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteHazard_ServerClick">Delete Hazard/Threat</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveHazard" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveHazard_ServerClick">Save Hazard/Threat</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showHazardDelete();
        });
        
        function showHazardDelete() {
            var noteId = $('#hfHazardId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteHazard').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteHazard').show();
            }
        }

        function openHazardModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteCommunication').show();
        }

        function clearNoteId() {
            $('#hfHazardId').val('');
            $('#txtHazardNumber').val('');
            $('#txtHazardType').val('');
            $('#txtHazardDetail').val('');
            $('#ApplicationContent_btnDeleteHazard').hide();
        }

    </script>
</asp:Content>
