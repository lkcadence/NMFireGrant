<%@ Page Title="Fire Grant Application: Community Information" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="CommunityInfo.aspx.cs" Inherits="NMSFMFireGrantWF.Application.CommunityInfo" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblCommunityName" runat="server" Text="Name of Community Protected? *" AssociatedControlID="txtCommunityProtected"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtCommunityProtected" runat="server" class="form-control" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblHomesProtected" runat="server" Text="Number of homes protected in fire district? *" AssociatedControlID="txtHomesProtected"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtHomesProtected" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MinValue="0" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblCommercial" runat="server" Text="Number of commercial buildings in fire district? *" AssociatedControlID="txtCommercial"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtCommercial" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MinValue="0" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblPopulation" runat="server" Text="What is the permanent resident population of the community you serve? *" AssociatedControlID="txtPopulation"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtPopulation" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" MinValue="0" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblAidAgreements" runat="server" Text="Do you have formal automatic aid or mutual aid agreements? *" AssociatedControlID="fldAidAgreements"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldAidAgreements" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbAidAgreementsYes" runat="server" Text="Yes" GroupName="AidAgreements" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbAidAgreementsNo" runat="server" Text="No" GroupName="AidAgreements" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvAidAgreementsHeader">
            <div class="col-md-12">
                <h3>List adjacent automatic aid fire districts (with written agreements)<span id="spAidDistrictsRequired"></span></h3>
            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#districtModal">
                    Add Aid District
                </button>
            </div>
        </div>
        <div class="row" id="dvAidAgreements">
            <div class="col-md-10">
                <telerik:RadGrid ID="rgAidDistricts" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgAidDistricts_NeedDataSource" OnPageIndexChanged="rgAidDistricts_PageIndexChanged" OnItemDataBound="rgAidDistricts_ItemDataBound" OnItemCommand="rgAidDistricts_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("AidDistrictId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AidDistrict" FilterControlAltText="Filter Aid District column" HeaderText="Automatic Aid Fire Districts" UniqueName="AidDistrict">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AidDistrictId" FilterControlAltText="Filter AidDistrictId column" HeaderText="AidDistrictId" UniqueName="AidDistrictId" Display="False" Resizable="False">
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
                <%--<asp:HiddenField ID="hfAidDistrictCount" runat="server" ClientIDMode="Static" Value="0" />--%>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="districtModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblDistrictHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblDistrictHeader">Aid Fire District</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblDistrictError" runat="server"></asp:Label>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblDistrictNumber" runat="server" AssociatedControlID="txtDistrictNumber" Text="District Number:"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDistrictNumber" runat="server" class="form-control" Width="50px" ClientIDMode="Static" ReadOnly="true" aria-required="true"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="row formRow">
                        <div class="col-sm-3">
                            <asp:Label ID="lblDistrict" runat="server" AssociatedControlID="txtDistrict" Text="Aid Fire District: *"></asp:Label>
                        </div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDistrict" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>

                    <asp:HiddenField ID="hfDistrictId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <button id="btnDeleteDistrict" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteDistrict_ServerClick">Delete District</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveDistrict" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveDistrict_ServerClick">Save District</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <script type="text/javascript">

        $(document).ready(function () {
            showAidDistrictsReq();
            showDistrictDelete();
        });
        
        function showDistrictDelete() {
            var noteId = $('#hfDistrictId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteDistrict').hide();
            }
            else {
                $('#ApplicationApplicationContent_btnDeleteDistrict').show();
            }
        }

        function showAidDistrictsReq() {
            var show = $('#rbAidAgreementsYes').prop('checked');
            if (show == true) {
                $('#spAidDistrictsRequired').html("*");
            }
            else {
                $('#spAidDistrictsRequired').html("");
            }
        }

        $('#rbAidAgreementsYes,#rbAidAgreementsNo').change(function () {
            showAidDistrictsReq();
        });

        function openDistrictModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteDistrict').show();
        }

        function clearNoteId() {
            $('#hfDistrictId').val('');
            $('#txtDistrict').val('');
            $('#ApplicationContent_btnDeleteDistrict').hide();
        }
    </script>
</asp:Content>
