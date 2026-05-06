<%@ Page Title="Fire Grant Application: Apparatus" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="Apparatus.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Apparatus" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblApparatusPart" runat="server" Text="Apparatus is part of the project? *" AssociatedControlID="fldApparatusPart"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldApparatusPart" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbApparatusPartYes" runat="server" Text="Yes" GroupName="ApparatusPart" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbApparatusPartNo" runat="server" Text="No" GroupName="ApparatusPart" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvPumpTestsConducted">
            <div class="col-md-4">
                <asp:Label ID="lblPumpTestsConducted" runat="server" Text="Are pump tests conducted annually on apparatus? *" AssociatedControlID="fldPumpTestsConducted"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldPumpTestsConducted" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbPumpTestsConductedYes" runat="server" Text="Yes" GroupName="PumpTestsConducted" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbPumpTestsConductedNo" runat="server" Text="No" GroupName="PumpTestsConducted" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvNoPumpTestsExp">
            <div class="col-md-4">
                <asp:Label ID="lblNoPumpTestsExp" runat="server" Text="Explain if not tested *" AssociatedControlID="txtNoPumpTestsExp"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtNoPumpTestsExp" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="dvNotification" runat="server">
            <div class="col-md-12">
                <p>
                    <a data-toggle="collapse" href="#pumptestingcode" role="button" aria-expanded="false" aria-controls="collapseExample">
                        Pump Testing Statute
                    </a>
                </p>
                <div class="collapse" id="pumptestingcode">
                    <div class="card card-body alert-info">
                        <asp:Literal ID="ltrPumpTestStatute" runat="server"></asp:Literal>
                        <%--<p>All rated fire pumps shall undergo annual pump tests to ensure proper function and firefighter safety; evidence must be provided that apparatus pump tests are conducted 
                            on each apparatus with rated fire pumps by documenting results in the Pump Test Data Log below.</p>
                        <ul>
                            <li>All annual pump tests shall be in accordance with NFPA 1901 and the Insurance Service Office (ISO) requirements.</li>
                            <li>
                                A notarized Affidavit signed by the Fire Chief must be uploaded with the application. The Affidavit is to verify that three years of pump test records exist for each 
                                apparatus with a rated fire pump, are on file with the department and are available for SFMO inspection upon request. A .pdf file of the Affidavit is available on the 
                                Grant website and must be uploaded with the application. Note: Notary signature and seal must be clear and legible. <span style="font-weight:bold"><u>Falsified affidavits may result in forfeiture of funds 
                                and future grant consideration.</u></span>
                            </li>
                            <li><strong>Pump Test Affidavit should be uploaded in the ‘Signatures and Supporting documents’ tab in the Step 3</strong></li>
                        </ul>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row formRow" id="dvHoseTests">
            <div class="col-md-4">
                <asp:Label ID="lblHoseTests" runat="server" Text="Has your annual hose testing been conducted? *" AssociatedControlID="fldHoseTests"></asp:Label>
            </div>
            <div class="col-md-3">
                <fieldset id="fldHoseTests" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbHoseTestsYes" runat="server" Text="Yes" GroupName="HoseTestsConducted" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbHoseTestsNo" runat="server" Text="No" GroupName="HoseTestsConducted" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row" id="dvNoHoseTests">
            <div class="col-md-4">
                <asp:Label ID="lblNoHoseTests" runat="server" Text="Explain if hose testing not conducted *" AssociatedControlID="txtNoHoseTests"></asp:Label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtNoHoseTests" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Width="100%" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="Div1" runat="server">
            <div class="col-md-12">
                <p>
                    <a data-toggle="collapse" href="#hosetestingcode" role="button" aria-expanded="false" aria-controls="collapseExample">
                        Hose Testing Statute
                    </a>
                </p>
                <div class="collapse" id="hosetestingcode">
                    <div class="card card-body alert-info">
                        <asp:Literal ID="ltrHoseTestStatute" runat="server"></asp:Literal>
                        <%--<p style="font-weight: bold">10.25.10.10 PERIODIC REQUIREMENTS:</p>
                        <p>
                            A. Each fire department shall complete a monthly fire report utilizing the national fire incident 
                            reporting system. This report shall be filed with the state fire marshal’s office by the 10th day of each month
                            following the month for which the report is prepared, (e.g., the report for January is due by February 10th). Each
                            fire department shall identify and file with the fire marshal’s office, as a minimum, one representative responsible to
                            comply with the reporting requirements.
                        </p>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row" id="dvListApparatusHead">
            <div class="col-md-12">
                <h3>LIST PUMP CAPABLE APPARATUS</h3>
            </div>
        </div>
        <div class="row" id="dvAddApparatusButton">
            <div class="col-md-3" id="dvShowModal" runat="server">
                <button type="button" id="btnShowModal" class="btn btn-primary" onclick="clearNoteId" data-toggle="modal" data-target="#apparatusModal">
                    Add Apparatus
                </button>
            </div>
        </div>
        <div class="row" id="dvApparatusList">
            <div class="col-md-12">
                <telerik:RadGrid ID="rgApparatus" runat="server" AutoGenerateColumns="False" GroupPanelPosition="Top" Skin="Bootstrap" AllowPaging="True" PageSize="10" OnNeedDataSource="rgApparatus_NeedDataSource" OnPageIndexChanged="rgApparatus_PageIndexChanged" OnItemDataBound="rgApparatus_ItemDataBound" OnItemCommand="rgApparatus_ItemCommand">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                    <ClientSettings AllowKeyboardNavigation="True">
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter Edit column" HeaderText="Edit" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" Text="View/Edit" CommandName="View" CommandArgument='<%# Eval("ApparatusId") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Number" FilterControlAltText="Filter Number column" HeaderText="Number" UniqueName="Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ApparatusName" FilterControlAltText="Filter Apparatus ID column" HeaderText="Apparatus ID" UniqueName="ApparatusName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="VIN" FilterControlAltText="Filter Apparatus VIN column" HeaderText="Vehicle ID #" UniqueName="VIN">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="License" FilterControlAltText="Filter Apparatus License column" HeaderText="License Plate" UniqueName="License">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="GPM" FilterControlAltText="Filter GPM column" HeaderText="GPM" UniqueName="GPM">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TestDate" FilterControlAltText="Filter Test Date column" HeaderText="Test Date" UniqueName="TestDate">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Pass" FilterControlAltText="Filter Pass column" HeaderText="Pass/Fail" UniqueName="Pass">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Comments" FilterControlAltText="Filter Comments column" HeaderText="Comments" UniqueName="Comments">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="Documentation" FilterControlAltText="Filter Documentation column" HeaderText="Documentation" UniqueName="Documentation">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="ApparatusId" FilterControlAltText="Filter ApparatusId column" HeaderText="ApparatusId" UniqueName="ApparatusId" Display="False" Resizable="False">
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
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <!-- Modal -->
    <div class="modal fade" id="apparatusModal" tabindex="-1" role="dialog" data-backdrop="false" aria-labelledby="lblApparatusHeader" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <h4 class="modal-title" id="lblApparatusHeader">Add Apparatus</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:Label ID="lblApparatusError" runat="server"></asp:Label>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblNumber" runat="server" AssociatedControlID="txtNumber" Text="Apparatus Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtNumber" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Number" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblApparatusName" runat="server" AssociatedControlID="txtApparatusName" Text="Apparatus ID: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtApparatusName" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblVIN" runat="server" AssociatedControlID="txtVIN" Text="Vehicle ID Number: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtVIN" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblLicense" runat="server" AssociatedControlID="txtLicense" Text="License Plate #: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtLicense" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblGPM" runat="server" AssociatedControlID="txtGPM" Text="GPM: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadNumericTextBox ID="txtGPM" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="," EmptyMessage="0" MinValue="0" Type="Number" ClientIDMode="Static" aria-required="true"></telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblTestDate" runat="server" AssociatedControlID="txtTestDate" Text="Test Date: *"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtTestDate" runat="server" class="form-control" Width="250px" ClientIDMode="Static" TextMode="Date" aria-required="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row formRow" id="dvPassFail">
                        <div class="col-md-5">
                            <asp:Label ID="lblPassFail" runat="server" Text="Pass/Fail?" AssociatedControlID="fldPassFail"></asp:Label>
                        </div>
                        <div class="col-md-7">
                            <fieldset id="fldPassFail" runat="server">
                                <asp:RadioButton ID="rbPass" runat="server" Text="Pass" GroupName="PassFail" ClientIDMode="Static" />&nbsp;
                                <asp:RadioButton ID="rbFail" runat="server" Text="Fail" GroupName="PassFail" ClientIDMode="Static" />
                            </fieldset>
                        </div>
                    </div>
                    <div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" Text="Comments:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtComments" runat="server" class="form-control" Width="250px" ClientIDMode="Static" MaxLength="100" ></asp:TextBox>
                        </div>
                    </div>
                    <%--<div class="row formRow">
                        <div class="col-sm-5">
                            <asp:Label ID="lblDocumentation" runat="server" AssociatedControlID="fuDocumentation" Text="Pump Test Documentation:"></asp:Label>
                        </div>
                        <div class="col-sm-7">
                            <telerik:RadAsyncUpload ID="fuDocumentation" runat="server" TabIndex="0" Skin="Bootstrap" MaxFileSize="5000000" MultipleFileSelection="Disabled" MaxFileInputsCount="1">
                                <FileFilters>
                                    <telerik:FileFilter Description="Images(jpeg;jpg;gif;png;pdf)" Extensions="jpeg,jpg,gif,png,pdf" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </div>
                    </div>--%>
                    <asp:HiddenField ID="hfApparatusId" runat="server" ClientIDMode="Static" Value="" />
                </div>

                <div class="modal-footer">
                    <button id="btnDeleteApparatus" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnDeleteApparatus_ServerClick">Delete Apparatus</button>
                    <button id="btnCloseModal" type="button" class="btn btn-primary" onclick="clearNoteId()" data-dismiss="modal">Close</button>
                    <button id="btnSaveApparatus" class="btn btn-primary" runat="server" data-dismiss="modal" onserverclick="btnSaveApparatus_ServerClick">Save Apparatus</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            showApparatusDelete();
            showAddApparatus();
            showNoHoseTest();
        });

        function showAddApparatus() {
            var show = $('#rbApparatusPartYes').prop('checked');
            if (show == true) {
                $('#dvListApparatusHead').fadeIn("slow");
                $('#dvPumpTestsConducted').fadeIn("slow");
                $('#dvHoseTests').fadeIn("slow");
                $('#dvAddApparatusButton').fadeIn("slow");
                $('#dvApparatusList').fadeIn("slow");
            }
            else {
                $('#rbPumpTestsConductedYes').prop('checked', false);
                $('#rbPumpTestsConductedNo').prop('checked', false);
                $('#dvPumpTestsConducted').fadeOut("slow");
                $('#dvNoPumpTestsExp').fadeOut("slow");
                $('#dvHoseTests').fadeOut("slow");
                $('#rbHoseTestsYes').prop('checked', false);
                $('#rbHoseTestsNo').prop('checked', false);
                $('#dvNoHoseTests').fadeOut("slow");
                $('#dvListApparatusHead').fadeOut("slow");
                $('#dvAddApparatusButton').fadeOut("slow");
                $('#dvApparatusList').fadeOut("slow");
            }
            showNoPumpTest();
        }

        function showNoPumpTest() {
            var show = $('#rbPumpTestsConductedNo').prop('checked');
            if (show == true) {
                $('#dvNoPumpTestsExp').fadeIn("slow");
            }
            else {
                $("#ApplicationContent_txtNoPumpTestsExp").val("");
                $('#dvNoPumpTestsExp').fadeOut("slow");
            }
        }

        function showNoHoseTest() {
            var show = $('#rbHoseTestsNo').prop('checked');
            if (show == true) {
                $('#dvNoHoseTests').fadeIn("slow");
            }
            else {
                $("#ApplicationContent_txtNoHoseTests").val("");
                $('#dvNoHoseTests').fadeOut("slow");
            }
        }
        
        function showApparatusDelete() {
            var noteId = $('#hfTrainingId').val();
            if (noteId === "") {
                $('#ApplicationContent_btnDeleteApparatus').hide();
            }
            else {
                $('#ApplicationContent_btnDeleteApparatus').show();
            }
        }

        function openApparatusModal() {
            $('#btnShowModal').click();
            $('#modalHeader').focus();
            $('#ApplicationContent_btnDeleteApparatus').show();
        }

        function clearNoteId() {
            $('#hfApparatusId').val('');
            $('#txtNumber').val('');
            $('#txtApparatusName').val('');
            $('#txtVIN').val('');
            $('#txtLicense').val('');
            $('#txtGPM').val('');
            $('#txtTestDate').val('');
            $('#rbPass').prop('checked', false);
            $('#rbFail').prop('checked', false);
            $('#txtComments').val('');
            $('#ApplicationContent_btnDeleteApparatus').hide();
        }

        $('#rbApparatusPartYes,#rbApparatusPartNo').change(function () {
            showAddApparatus();
        });

        $('#rbPumpTestsConductedYes,#rbPumpTestsConductedNo').change(function () {
            showNoPumpTest();
        });

        $('#rbHoseTestsYes,#rbHoseTestsNo').change(function () {
            showNoHoseTest();
        });
    </script>
</asp:Content>
