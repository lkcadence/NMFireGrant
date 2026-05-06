<%@ Page Title="Fire Grant Application: Training" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="Training.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Training" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-5">
                <asp:Label ID="lblTrainingHours" runat="server" Text="Average number of training hours per Firefighter per year? *" AssociatedControlID="txtTrainingHours"></asp:Label>
            </div>
            <div class="col-md-3">
                <telerik:RadNumericTextBox ID="txtTrainingHours" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" Type="Number" aria-required="true"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="row" id="dvTrainingsHead">
            <div class="col-md-12">
                <h3>How many training opportunities has this department offered in the last calendar year?</h3>
            </div>
        </div>
        <div class="row" id="dvShowModal" runat="server">
            <div class="col-md-3">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#trainingModal">
                    Add Training
                </button>
            </div>
        </div>
        <div class="row" id="dvTranings">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgTrainings" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgTrainings_NeedDataSource" OnPageIndexChanged="rgTrainings_PageIndexChanged" OnItemDataBound="rgTrainings_ItemDataBound" OnItemCommand="rgTrainings_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("TrainingId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TrainingDetail" FilterControlAltText="Filter Training Detail column" HeaderText="Training Detail" UniqueName="TrainingDetail">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TrainingDocumentName" FilterControlAltText="Filter Supporting Doc column" HeaderText="Supporting Documentation" UniqueName="TrainingDocumentName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TrainingId" FilterControlAltText="Filter TrainingId column" HeaderText="TrainingId" UniqueName="TrainingId" Display="False" Resizable="False">
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
            <div class="row formRow">
                <div class="col-md-5">
                    <asp:Label ID="lblTrainingPoints" runat="server" Text="Regular and adequate training points (max of 10)?" AssociatedControlID="txtTrainingPoints"></asp:Label>
                </div>
                <div class="col-md-3">
                    <telerik:RadNumericTextBox ID="txtTrainingPoints" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MaxValue="10" Type="Number"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
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
    <div class="modal fade" id="trainingModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblTrainingHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblTrainingHeader">Add Training</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblTrainingError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblTrainingNumber" runat="server" AssociatedControlID="txtTrainingNumber" Text="Training Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTrainingNumber" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblTrainingDetails" runat="server" AssociatedControlID="txtTrainingDetails" Text="Training Details: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTrainingDetails" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblTrainingDoc" runat="server" AssociatedControlID="ruTrainingDoc" Text="Supporting Documentation: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadAsyncUpload ID="ruTrainingDoc" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1" aria-required="true">
                                <FileFilters>
                                    <telerik:FileFilter Description="Documents(jpeg;jpg;gif;png;pdf;xls;xlsx;doc;docx;txt)" Extensions="jpeg,jpg,gif,png,pdf,xls,xlsx,doc,docx,txt" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>
                    <div class="row formRow" id="dvTrainingDocLink" runat="server" visible="false">
                        <div class="col-md-5">
                            <asp:Label ID="lblTrainingDocLink" runat="server" AssociatedControlID="lnkTrainingDoc" Text="Current Document Link:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:LinkButton ID="lnkTrainingDoc" runat="server" Text="Download Required Documentation" OnClick="lnkTrainingDoc_Click"></asp:LinkButton>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfTrainingId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <button id="btnDeleteTraining" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteTraining_ServerClick">Delete Training</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveTraining" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveTraining_ServerClick">Save Training</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showTrainingDelete();
        });
        
        function showTrainingDelete() {
            var noteId = $('#hfTrainingId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteTraining').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteTraining').show();
            }
        }

        function openTrainingModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteTraining').show();
        }

        function clearNoteId() {
            $('#hfTrainingId').val('');
            $('#txtTrainingNumber').val('');
            $('#txtTrainingDetails').val('');
            $('#ruTrainingDoc').val('');
            $('#ApplicationContent_btnDeleteTraining').hide();
        }
    </script>
</asp:Content>
