<%@ Page Title="Fire Grant Application: Water Availability" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="WaterAvailability.aspx.cs" Inherits="NMSFMFireGrantWF.Application.WaterAvailability" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblCommunityHydrants" runat="server" Text="Community Hydrant System?" AssociatedControlID="fldCommunityHydrants"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldCommunityHydrants" runat="server">
                    <asp:RadioButton ID="rbCommunityHydrantsYes" runat="server" Text="Yes" GroupName="CommunityHydrants" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbCommunityHydrantsNo" runat="server" Text="No" GroupName="CommunityHydrants" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblWaterCapacity" runat="server" Text="Total capacity of available water storage (in gallons)?" AssociatedControlID="txtTotalWaterCapacity"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtTotalWaterCapacity" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblWaterCapacityWheels" runat="server" Text="Total capacity of water storage on wheels (in gallons)?" AssociatedControlID="txtWaterCapacityWheels"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtWaterCapacityWheels" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblWaterCapacityStation" runat="server" Text="Total capacity of water stored at station (in gallons)?" AssociatedControlID="txtWaterCapacityStation"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtWaterCapacityStation" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblWaterStorageTank" runat="server" Text="Water storage tank with fire hydrant at station?" AssociatedControlID="fldWaterStorageTank"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldWaterStorageTank" runat="server">
                    <asp:RadioButton ID="rbWaterStorageTankYes" runat="server" Text="Yes" GroupName="WaterStorageTank" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbWaterStorageTankNo" runat="server" Text="No" GroupName="WaterStorageTank" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvAdditionalWaterHead">
            <div class="col-md-12">
                <h3>Describe Additional Water Source(s)</h3>
            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#waterSourceModal">
                    Add Water Source
                </button>
            </div>
        </div>
        <div class="row" id="dvAdditionalWater">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgAdditionalWater" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgAdditionalWater_NeedDataSource" OnPageIndexChanged="rgAdditionalWater_PageIndexChanged" OnItemDataBound="rgAdditionalWater_ItemDataBound" OnItemCommand="rgAdditionalWater_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("WaterSourceId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="WaterSource" FilterControlAltText="Filter Water Source column" HeaderText="Water Source" UniqueName="WaterSource">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Capacity" FilterControlAltText="Filter Capacity column" HeaderText="Capacity (in gallons)" UniqueName="Capacity">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="WaterSourceId" FilterControlAltText="Filter WaterSourceId column" HeaderText="WaterSourceId" UniqueName="WaterSourceId" Display="False" Resizable="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
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
    <!-- Modal -->
    <div class="modal fade" id="waterSourceModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblWaterSourceHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblWaterSourceHeader">Add Water Source</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblWaterSourceError" runat="server"></asp:Label>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblWaterSourceNumber" runat="server" AssociatedControlID="txtWaterSourceNumber" Text="Water Source Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtWaterSourceNumber" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblWaterSource" runat="server" AssociatedControlID="txtWaterSource" Text="Water Source: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtWaterSource" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblCapacity" runat="server" AssociatedControlID="txtCapacity" Text="Capacity (in gallons): *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtCapacity" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:rangevalidator ID="Rangevalidator2" errormessage="Please enter value between 0-10000000." forecolor="Red" controltovalidate="txtCapacity" minimumvalue="0" maximumvalue="10000000" runat="server" Type="Integer">
                            </asp:rangevalidator>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfWaterSourceId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <button id="btnDeleteWaterSource" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteWaterSource_ServerClick">Delete Water Source</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveWaterSource" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveWaterSource_ServerClick">Save Water Source</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <script type="text/javascript">

        $(document).ready(function () {
            showWaterSourceDelete();
        });
        
        function showWaterSourceDelete() {
            var noteId = $('#hfWaterSourceId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteWaterSource').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteWaterSource').show();
            }
        }

        function openWaterSourceModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteWaterSource').show();
        }

        function clearNoteId() {
            $('#hfWaterSourceId').val('');
            /*$('#txtWaterSourceNumber').val('');*/
            $('#txtWaterSource').val('');
            $('#txtCapacity').val('');
            $('#ApplicationContent_btnDeleteWaterSource').hide();
        }
    </script>
</asp:Content>
