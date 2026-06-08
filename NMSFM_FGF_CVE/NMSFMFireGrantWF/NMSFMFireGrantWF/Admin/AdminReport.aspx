<%@ Page Title="Fire Grant: Admin Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminReport.aspx.cs" Inherits="NMSFMFireGrantWF.Admin.AdminReport" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row" id="dvError" runat="server"></div>
        <h2>Admin Report</h2>
        <%--<div class='alert alert-error'>Page Under Construction: This page is currently undergoing construction. Not all functionality and data will be available. Stay tuned...</div>--%>
        <div class="row"><hr /></div>
        <div id="dvSearch">
            <h3>Search</h3>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblFiscalYear" runat="server" Text="Select Fiscal Year" AssociatedControlID="ddlFiscalYear"></asp:Label>
                </div>
                <div class="col-md-2">
                    <telerik:RadDropDownList ID="ddlFiscalYear" runat="server" OnSelectedIndexChanged="ddlFiscalYear_SelectedIndexChanged" AutoPostBack="true"></telerik:RadDropDownList>
                </div>
                <div class="col-md-2">
                    <label>Date Range:</label>
                </div>
                <div class="col-md-2">
                    <telerik:RadDatePicker ID="rdpStartDate" runat="server"></telerik:RadDatePicker>
                    <span style="display:none"><asp:Label ID="lblStartDate" runat="server" AssociatedControlID="rdpStartDate" Text="Start Date"></asp:Label></span>
                </div>
                <div class="col-md-1">
                    To
                </div>
                <div class="col-md-2">
                    <%--<asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>--%>
                    <telerik:RadDatePicker ID="rdpEndDate" runat="server"></telerik:RadDatePicker>
                    <span style="display:none"><asp:Label ID="lblEndDate" runat="server" AssociatedControlID="rdpEndDate" Text="End Date"></asp:Label></span>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click"/>
                </div>
            </div>
            <div class="row"><hr /></div>
            <div class="row">
                <div class="col-md-5">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="ddlReportColumns" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="450" Label="Select Report Columns">
                        <Items>
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Default Fields" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Number" Value="FGApplicationIdentity" Checked="true" />
                            <telerik:RadComboBoxItem Text="Fiscal Year" Value="FiscalYear" Checked="true" />
                            <telerik:RadComboBoxItem Text="Department Name" Value="DepartmentName" Checked="true" />
                            <telerik:RadComboBoxItem Text="Application Number" Value="ApplicationNumber" Checked="true" />
                            <telerik:RadComboBoxItem Text="Date Started" Value="DateStarted" Checked="true" />
                            <telerik:RadComboBoxItem Text="Mailing Address" Value="MailingAddress" Checked="true" />
                            <telerik:RadComboBoxItem Text="County" Value="County" Checked="true" />
                            <telerik:RadComboBoxItem Text="Fire Chief" Value="FireChiefName" Checked="true" />
                            <telerik:RadComboBoxItem Text="Total Project Cost" Value="TotalProjectCost" Checked="true" />
                            <telerik:RadComboBoxItem Text="Amount Requested" Value="AmountRequested" Checked="true" />
                            <telerik:RadComboBoxItem Text="Stipend Amount" Value="StipendAmount" Checked="true" />
                            <telerik:RadComboBoxItem Text="Granted Amount" Value="GrantedAmount" Checked="true" />
                            <telerik:RadComboBoxItem Text="Stipend Carryover" Value="StipendCarryover" Checked="true" />
                            <telerik:RadComboBoxItem Text="Status" Value="Status" Checked="true" />
                            <telerik:RadComboBoxItem Text="Last Status Change" Value="LastStatusChange" Checked="true" />
                            <telerik:RadComboBoxItem Text="Approved Date" Value="ApprovedDate" Checked="true" />
                            <telerik:RadComboBoxItem Text="Approved By" Value="ApprovedByName" Checked="true" />
                            <telerik:RadComboBoxItem Text="Date Submitted" Value="DateSubmitted" Checked="true" />
                            <telerik:RadComboBoxItem Text="Submitted By" Value="SubmittedByName" Checked="true" />
                            <telerik:RadComboBoxItem Text="NERIS ID" Value="NERISID" Checked="true" />
                            <telerik:RadComboBoxItem Text="Training Points" Value="TrainingPoints" Checked="true" />
                            <telerik:RadComboBoxItem Text="Financial Need Grade" Value="FinancialNeedGrade" Checked="true" />
                            <telerik:RadComboBoxItem Text="Problem Grade" Value="ProblemGrade" Checked="true" />
                            <telerik:RadComboBoxItem Text="Benefit Grade" Value="BenefitGrade" Checked="true" />
                            <telerik:RadComboBoxItem Text="Consequences Grade" Value="ConsequencesGrade" Checked="true" />
                            <telerik:RadComboBoxItem Text="App Completeness Grade" Value="AppCompletenessGrade" Checked="true" />
                            <telerik:RadComboBoxItem Text="Total Score" Value="TotalScore" Checked="true" />
                            <telerik:RadComboBoxItem Text="Last Received Grant" Value="LastReceivedGrant" Checked="true" />
                            <telerik:RadComboBoxItem Text="Priority Categories" Value="PriorityCategories" Checked="true" />
                            <telerik:RadComboBoxItem Text="ISORating" Value="ISORating" Checked="true" />
                            <telerik:RadComboBoxItem Text="Person Complete App" Value="PersonCompleteApp" Checked="true" />
                            <telerik:RadComboBoxItem Text="NERIS Compliant" Value="NERISCompliant" Checked="true" />
                            <telerik:RadComboBoxItem Text="Pump Test Compliant" Value="PumpTestCompliant" Checked="true" />   
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="General Information" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Individual Dept" Value="IndividualDept" />
                            <telerik:RadComboBoxItem Text="Phone" Value="Phone" />
                            <telerik:RadComboBoxItem Text="Email Address" Value="EmailAddress" />
                            <telerik:RadComboBoxItem Text="Is City Muni" Value="IsCityMuni" />
                            <telerik:RadComboBoxItem Text="Dept Type" Value="DeptType" />
                            <telerik:RadComboBoxItem Text="Is Admin Dept" Value="IsAdminDept" />
                            <telerik:RadComboBoxItem Text="County Depts Compliant" Value="CountyDeptsCompliant" />
                            <telerik:RadComboBoxItem Text="Main Stations" Value="MainStations" />
                            <telerik:RadComboBoxItem Text="Sub Stations" Value="SubStations" />
                            <telerik:RadComboBoxItem Text="Admin Bldgs" Value="AdminBldgs" />
                            <telerik:RadComboBoxItem Text="Community" Value="Community" />
                            <telerik:RadComboBoxItem Text="Number Of Firefighters" Value="NumberOfFirefighters" />
                            <telerik:RadComboBoxItem Text="FFI Firefighters" Value="FFI_Firefighters" />
                            <telerik:RadComboBoxItem Text="FFII Firefighters" Value="FFII_Firefighters" />
                            <telerik:RadComboBoxItem Text="Mailing Address" Value="MailingAddress" />
                            <telerik:RadComboBoxItem Text="Mailing City" Value="MailingCity" />
                            <telerik:RadComboBoxItem Text="Mailing State" Value="MailingState" />
                            <telerik:RadComboBoxItem Text="Mailing Zip" Value="MailingZip" />
                            <telerik:RadComboBoxItem Text="Fire Dept Member" Value="FireDeptMember" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Budget Information" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Operating Budget" Value="OperatingBudget" />
                            <telerik:RadComboBoxItem Text="FPF Distribution" Value="FPFDistribution" />
                            <telerik:RadComboBoxItem Text="Carryover Balance" Value="CarryoverBalance" />
                            <telerik:RadComboBoxItem Text="Carryover Purpose" Value="CarryoverPurpose" />
                            <telerik:RadComboBoxItem Text="Taxes %" Value="PerTaxes" />
                            <telerik:RadComboBoxItem Text="Grants %" Value="" />
                            <telerik:RadComboBoxItem Text="State FM Funds %" Value="PerStateFMFunds" />
                            <telerik:RadComboBoxItem Text="Donations %" Value="PerDonations" />
                            <telerik:RadComboBoxItem Text="Fund Drives %" Value="PerFundDrives" />
                            <telerik:RadComboBoxItem Text="Fee For Service %" Value="PerFeeForService" />
                            <telerik:RadComboBoxItem Text="Others %" Value="PerOthers" />
                            <telerik:RadComboBoxItem Text="Others Desc" Value="OthersDesc" />
                            <telerik:RadComboBoxItem Text="Total %" Value="PerTotal" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Community Information" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Community Name" Value="CommunityName" />
                            <telerik:RadComboBoxItem Text="Number Homes" Value="NumberOfHomes" />
                            <telerik:RadComboBoxItem Text="Number Comm" Value="NumberOfComm" />
                            <telerik:RadComboBoxItem Text="Resident Pop" Value="ResidentPopulation" />
                            <telerik:RadComboBoxItem Text="Aid Agreements" Value="AidAgreements" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Response History" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="NERIS Current" Value="NERISCurrent" />
                            <telerik:RadComboBoxItem Text="Response Structure" Value="ResponseStructure" />
                            <telerik:RadComboBoxItem Text="Response Vehicle" Value="ResponseVehicle" />
                            <telerik:RadComboBoxItem Text="Response Vegitation" Value="ResponseVegitation" />
                            <telerik:RadComboBoxItem Text="Response EMS" Value="ResponseEMS" />
                            <telerik:RadComboBoxItem Text="Response Rescue" Value="ResponseRescue" />
                            <telerik:RadComboBoxItem Text="Response Hazardous" Value="ResponseHazardous" />
                            <telerik:RadComboBoxItem Text="Response Service" Value="ResponseService" />
                            <telerik:RadComboBoxItem Text="Response Good Intent" Value="ResponseGoodIntent" />
                            <telerik:RadComboBoxItem Text="Response False" Value="ResponseFalse" />
                            <telerik:RadComboBoxItem Text="Response Other" Value="ResponseOther" />
                            <telerik:RadComboBoxItem Text="Response Total" Value="ResponseTotal" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Water Availability" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Com Hydrant Sys" Value="ComHydrantSys" />
                            <telerik:RadComboBoxItem Text="Avail. Water Cap" Value="AvailableWaterCapacity" />
                            <telerik:RadComboBoxItem Text="Water On Wheels Cap" Value="WaterOnWheelsCapacity" />
                            <telerik:RadComboBoxItem Text="Station Water Cap" Value="StationWaterCapacity" />
                            <telerik:RadComboBoxItem Text="Tank At Station" Value="TankAtStation" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Training" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Yearly Training Hours" Value="YearlyTrainingHours" />
                            <telerik:RadComboBoxItem Text="Number Of Listed Trainings" Value="NumberOfListedTrainings" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Apparatus" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Apparatus Part Of Project" Value="ApparatusPartOfProject" />
                            <telerik:RadComboBoxItem Text="Pump Tests Conducted" Value="PumpTestsConducted" />
                            <telerik:RadComboBoxItem Text="Explain No Pump Tests" Value="ExplainNoPumpTests" />
                            <telerik:RadComboBoxItem Text="Hose Test Conducted" Value="HoseTestConducted" />
                            <telerik:RadComboBoxItem Text="Explain No Host Tests" Value="ExplainNoHostTests" />
                            <telerik:RadComboBoxItem Text="Number Of Listed Apparatus" Value="NumberOfListedApparatus" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Communication" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Communication Project" Value="CommunicationProject" />
                            <telerik:RadComboBoxItem Text="Handheld Radios" Value="HandheldRadios" />
                            <telerik:RadComboBoxItem Text="Base Stations" Value="BaseStations" />
                            <telerik:RadComboBoxItem Text="Mobile Radios" Value="MobileRadios" />
                            <telerik:RadComboBoxItem Text="Apparatus W/O Radio" Value="ApparatusWoRadio" />
                            <telerik:RadComboBoxItem Text="Law Enforcement" Value="LawEnforcement" />
                            <telerik:RadComboBoxItem Text="Emergency Medical" Value="EmergencyMedical" />
                            <telerik:RadComboBoxItem Text="Other Fire Depts" Value="OtherFireDepts" />
                            <telerik:RadComboBoxItem Text="Other" Value="Other" />
                            <telerik:RadComboBoxItem Text="Other Description" Value="OtherDescription" />
                            <telerik:RadComboBoxItem Text="Areas Not Covered" Value="AreasNotCovered" />
                            <telerik:RadComboBoxItem Text="Describe Areas Not Covered" Value="DescribeAreasNotCovered" />
                            <telerik:RadComboBoxItem Text="# Communication Devices Listed" Value="NumberOfCommunicationDevicesListed" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Hazards/Threats" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="# Hazards/Threats Listed" Value="NumberOfHazardsThreatsListed" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="PPE" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="PPE Part Of Project" Value="PPEPartOfProject" />
                            <telerik:RadComboBoxItem Text="PPE Inspected" Value="PPEInspected" />
                            <telerik:RadComboBoxItem Text="# PPE Listed" Value="NumberOfPPEListed" />
                            <telerik:RadComboBoxItem Text="# SCBA Listed" Value="NumberOfSCBAListed" />
                            <telerik:RadComboBoxItem runat="server" IsSeparator="True" Text="Equipment Needed" Enabled="false"/>
                            <telerik:RadComboBoxItem Text="Specific Needs" Value="SpecificNeeds" />
                            <telerik:RadComboBoxItem Text="ISO Impacted" Value="ISOImpacted" />
                            <telerik:RadComboBoxItem Text="ISO Impact Explanation" Value="ISOImpactExplanation" />
                            <telerik:RadComboBoxItem Text="# Equipmen tNeeded" Value="NumberOfEquipmentNeeded" />
                            <telerik:RadComboBoxItem Text="Amount Equipment Needed" Value="AmountOfEquipmentNeeded" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
                <div class="col-md-3">
                    <asp:LinkButton ID="lnkShowColumns" runat="server" Text="Update Report Columns" OnClick="lnkShowColumns_Click"></asp:LinkButton>
                </div>
            </div>
            <div class="row"><hr /></div>
            <div class="row">
                <div class="col-md-2">
                    <label>Report Exports</label>
                </div>
                <div class="col-md-3">
                    <asp:ImageButton id="ibtnExportExcel" runat="server" ImageUrl="~/Content/images/ms-excel.png" Height="30px" AlternateText="Export To Excel" OnClick="ibtnExportExcel_Click"/><br />
                    <asp:linkbutton id="lnkExcelExport" runat="server" Text="Export to Excel" OnClick="lnkExcelExport_Click"></asp:linkbutton>
                </div>
                <div class="col-md-3">
                    <asp:ImageButton ID="ibtnExportPdf" runat="server" ImageUrl="~/Content/images/pdf--v1.png" Height="30px" AlternateText="Export To PDF" OnClick="ibtnExportPdf_Click" /><br />  
                    <asp:linkbutton id="lnkExportPdf" runat="server" Text="Export to PDF" OnClick="lnkExportPdf_Click"></asp:linkbutton>
                </div>
            </div>
            
        </div>
        <div class="row"><hr /></div>
        <div class="row">
                <div class="col-md-12">
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="rgDepartments">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgDepartments" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadGrid ID="rgDepartments" runat="server" AutoGenerateColumns="False" Skin="Bootstrap" ShowGroupPanel="True" AllowPaging="True" OnNeedDataSource="rgDepartments_NeedDataSource" OnItemCreated="rgDepartments_ItemCreated"
                        RenderMode="Lightweight" AllowFilteringByColumn="True" AllowSorting="True" Width="100%" ShowFooter="True" OnItemDataBound="rgDepartments_ItemDataBound">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups" CaseSensitive="false" ShowUnGroupButton="true"></GroupingSettings>
                        <ClientSettings AllowKeyboardNavigation="True" Selecting-AllowRowSelect="false" Scrolling-AllowScroll="true" AllowDragToGroup="True">
                        <Scrolling AllowScroll="True" ScrollHeight="" UseStaticHeaders="true"></Scrolling>
                        </ClientSettings>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                            <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="15mm" PageFooterMargin="15mm" PageLeftMargin="15mm" PageRightMargin="15mm"  
                                BorderStyle="Medium" BorderColor="#666666">
                            </Pdf>
                        </ExportSettings>
                        <MasterTableView>
                            <RowIndicatorColumn ShowNoSortIcon="False"></RowIndicatorColumn>
                            <ExpandCollapseColumn ShowNoSortIcon="False"></ExpandCollapseColumn>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Application Details" Name="ApplicationDetails">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="General Information" Name="GeneralInformation">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Budget Information" Name="BudgetInformation">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Communitiy Information" Name="CommunityInformation">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Response History" Name="ResponseHistory">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Water Availability" Name="WaterAvailability">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Training" Name="Training">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Apparatus" Name="Apparatus">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Communication" Name="Communication">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Hazards/Threats" Name="HazardsThreats">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="PPE" Name="PPE">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Equipment Needed" Name="EquipmentNeeded">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Project Budget" Name="ProjectBudget">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Application Review" Name="ApplicationReview">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="ApplicationScores" Name="ApplicationScores">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridBoundColumn DataField="" FilterControlAltText="Filter  column" HeaderText="" UniqueName="" ColumnGroupName="" Visible="false">
                                </telerik:GridBoundColumn>
                                <%--Application Data--%>
                                <telerik:GridBoundColumn DataField="FGApplicationIdentity" FilterControlAltText="Filter FGApplicationIdentity column" HeaderText="No." UniqueName="FGApplicationIdentity" ColumnGroupName="ApplicationDetails" FooterText="Total Apps: " Aggregate="Count" >
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FiscalYear" FilterControlAltText="Filter FiscalYear column" HeaderText="Year" UniqueName="FiscalYear" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>   
                                <telerik:GridBoundColumn DataField="ApplicationNumber" FilterControlAltText="Filter Application Number column" HeaderText="Application #" UniqueName="ApplicationNumber" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DateStarted" FilterControlAltText="Filter Date Started column" DataFormatString="{0:d}" HeaderText="Date Started" UniqueName="DateStarted" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DateSubmitted" FilterControlAltText="Filter Date Submitted column" DataFormatString="{0:d}" HeaderText="Date Submitted" UniqueName="DateSubmitted" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SubmittedByName" FilterControlAltText="Filter Submitted By column" HeaderText="Submitted By" UniqueName="SubmittedByName" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column" HeaderText="Status" UniqueName="Status" ColumnGroupName="ApplicationDetails">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbStatus" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Status").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="StatusChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Approved" Value="Approved" />
                                                <telerik:RadComboBoxItem Text="Grant Approved (No Stipend)" Value="Grant Approved (No Stipend)" />
                                                <telerik:RadComboBoxItem Text="Rejected" Value="Rejected" />
                                                <telerik:RadComboBoxItem Text="Reopen" Value="Reopen" />
                                                <telerik:RadComboBoxItem Text="Under Review" Value="Under Review" />
                                                <telerik:RadComboBoxItem Text="Submitted for Review" Value="Submitted for Review" />
                                                <telerik:RadComboBoxItem Text="In Process" Value="In Process" />
                                                <telerik:RadComboBoxItem Text="Awarded" Value="Awarded" />
                                                <telerik:RadComboBoxItem Text="Not Awarded" Value="Not Awarded" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                            <script type="text/javascript">
                                                function StatusChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("Status", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastStatusChange" FilterControlAltText="Filter Last Status Change column" DataFormatString="{0:d}" HeaderText="Last Status Change" UniqueName="LastStatusChange" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApprovedDate" FilterControlAltText="Filter Approved Date column" DataFormatString="{0:d}" HeaderText="Approved Date" UniqueName="ApprovedDate" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApprovedByName" FilterControlAltText="Filter Approved By column" HeaderText="Approved By" UniqueName="ApprovedByName" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="GrantedAmount" FilterControlAltText="Filter Granted Amount column" DataFormatString="{0:C}" HeaderText="Granted Amount" UniqueName="GrantedAmount" FooterText="Total Granted: " Aggregate="Sum" FooterAggregateFormatString="{0:C}" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastReceivedGrant" FilterControlAltText="Filter Last Received Grant column" HeaderText="Last Received Grant" UniqueName="LastReceivedGrant" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PriorityCategories" FilterControlAltText="Filter Priority Categories column" HeaderText="Priority Categories" UniqueName="PriorityCategories" ColumnGroupName="ApplicationDetails">
                                </telerik:GridBoundColumn>
                                <%--General Information--%>
                                <telerik:GridBoundColumn DataField="strIndividualDept" FilterControlAltText="Filter Individual Dept column" HeaderText="Individual Dept" UniqueName="IndividualDept" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbIndividualDept" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("IndividualDept").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="IndividualDeptChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Individual Department" Value="Individual Department" />
                                                <telerik:RadComboBoxItem Text="County" Value="County" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                            <script type="text/javascript">
                                                function IndividualDeptChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("IndividualDept", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NERISID" FilterControlAltText="Filter NERIS ID column" HeaderText="NERIS ID" UniqueName="NERISID" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter Department Name column" HeaderText="Department" UniqueName="DepartmentName" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FireChiefName" FilterControlAltText="Filter Fire Chief Name column" HeaderText="Fire Chief Name" UniqueName="FireChiefName" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Phone" FilterControlAltText="Filter Phone column" HeaderText="Phone" UniqueName="Phone" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter Email Address column" HeaderText="Email Address" UniqueName="EmailAddress" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ISORating" FilterControlAltText="Filter ISO Rating column" HeaderText="ISO Rating" UniqueName="ISORating" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="County" FilterControlAltText="Filter County column" HeaderText="County" UniqueName="County" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strCityMuni" FilterControlAltText="Filter Municipality column" HeaderText="Municipality" UniqueName="IsCityMuni" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbIsCityMuni" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("IsCityMuni").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="IsCityMuniChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="City/Municipality" Value="City/Municipality" />
                                                <telerik:RadComboBoxItem Text="County" Value="County" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                            <script type="text/javascript">
                                                function IsCityMuniChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("IsCityMuni", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strDeptType" FilterControlAltText="Filter Dept Type column" HeaderText="Dept Type" UniqueName="DeptType" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbDeptType" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("DeptType").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="DeptTypeChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Career" Value="Career" />
                                                <telerik:RadComboBoxItem Text="Volunteer" Value="Volunteer" />
                                                <telerik:RadComboBoxItem Text="Combined" Value="Combined Career & Volunteer" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                                            <script type="text/javascript">
                                                function DeptTypeChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("DeptType", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsAdminDept" FilterControlAltText="Filter Admin column" HeaderText="Admin" UniqueName="IsAdminDept" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strCountyDeptsCompliant" FilterControlAltText="Filter County Depts Compliant column" HeaderText="County Depts Compliant" UniqueName="CountyDeptsCompliant" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbCountyDeptsCompliant" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("CountyDeptsCompliant").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="CountyDeptsCompliantChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock5" runat="server">
                                            <script type="text/javascript">
                                                function CountyDeptsCompliantChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("CountyDeptsCompliant", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MainStations" FilterControlAltText="Filter Main Stations column" HeaderText="Main Stations" UniqueName="MainStations" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SubStations" FilterControlAltText="Filter Sub Stations column" HeaderText="Sub Stations" UniqueName="SubStations" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AdminBldgs" FilterControlAltText="Filter Admin Bldgs column" HeaderText="Admin Bldgs" UniqueName="AdminBldgs" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strCommunity" FilterControlAltText="Filter Community column" HeaderText="Community" UniqueName="Community" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcCommunity" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Community").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="CommunityChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Urban" Value="Urban" />
                                                <telerik:RadComboBoxItem Text="Rural" Value="Rural" />
                                                <telerik:RadComboBoxItem Text="Sub-Urban" Value="Sub-Urban" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock6" runat="server">
                                            <script type="text/javascript">
                                                function CommunityChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("Community", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfFirefighters" FilterControlAltText="Filter # Firefighters column" HeaderText="# Firefighters" UniqueName="NumberOfFirefighters" ColumnGroupName="GeneralInformation" Visible="false" FooterText="Total FF: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FFI_Firefighters" FilterControlAltText="Filter # FFI Firefighters column" HeaderText="# FFI Firefighters" UniqueName="FFI_Firefighters" ColumnGroupName="GeneralInformation" Visible="false" FooterText="Total FF-I: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FFII_Firefighters" FilterControlAltText="Filter # FFII Firefighters column" HeaderText="# FFII Firefighters" UniqueName="FFII_Firefighters" ColumnGroupName="GeneralInformation" Visible="false" FooterText="Total FF-II: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MailingAddress" FilterControlAltText="Filter Address column" HeaderText="Address" UniqueName="MailingAddress" ColumnGroupName="GeneralInformation">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MailingCity" FilterControlAltText="Filter City column" HeaderText="City" UniqueName="MailingCity" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MailingState" FilterControlAltText="Filter State column" HeaderText="State" UniqueName="MailingState" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MailingZip" FilterControlAltText="Filter Zip column" HeaderText="Zip" UniqueName="MailingZip" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PersonCompleteApp" FilterControlAltText="Filter Person Completing App column" HeaderText="Person Completing App" UniqueName="PersonCompleteApp" ColumnGroupName="GeneralInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strFireDeptMember" FilterControlAltText="Filter Fire Dept Member column" HeaderText="Fire Dept Member" UniqueName="FireDeptMember" ColumnGroupName="GeneralInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbFireDeptMember" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FireDeptMember").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="FireDeptMemberChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock7" runat="server">
                                            <script type="text/javascript">
                                                function FireDeptMemberChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("FireDeptMember", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <%--Budget Information--%>
                                <telerik:GridBoundColumn DataField="OperatingBudget" FilterControlAltText="Filter Operating Budget column" DataFormatString="{0:C}" HeaderText="Operating Budget" UniqueName="OperatingBudget" ColumnGroupName="BudgetInformation" Visible="false" FooterText="Total OB.: " FooterAggregateFormatString="{0:C}" Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FPFDistribution" FilterControlAltText="Filter FPF Distribution column" DataFormatString="{0:C}" HeaderText="FPF Distribution" UniqueName="FPFDistribution" ColumnGroupName="BudgetInformation" Visible="false" FooterText="Total Dist.: " FooterAggregateFormatString="{0:C}" Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StipendCarryover" FilterControlAltText="Filter Stipend Carryover column" DataFormatString="{0:C}" HeaderText="Stipend Carryover" UniqueName="StipendCarryover" ColumnGroupName="BudgetInformation" Visible="false" FooterText="Total Carryover.: " FooterAggregateFormatString="{0:C}" Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CarryoverBalance" FilterControlAltText="Filter Carryover Balance column" DataFormatString="{0:C}" HeaderText="Carryover Balance" UniqueName="CarryoverBalance" ColumnGroupName="BudgetInformation" FooterText="Total CB.: " FooterAggregateFormatString="{0:C}" Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CarryoverPurpose" FilterControlAltText="Filter Carryover Purpose column" HeaderText="Carryover Purpose" UniqueName="CarryoverPurpose" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerTaxes" FilterControlAltText="Filter Taxes % column" HeaderText="Taxes %" DataFormatString="{0:p}" UniqueName="PerTaxes" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerGrants" FilterControlAltText="Filter Grants % column" HeaderText="Grants %" DataFormatString="{0:p}" UniqueName="PerGrants" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerStateFMFunds" FilterControlAltText="Filter State FMF % column" HeaderText="State FMF %" DataFormatString="{0:p}" UniqueName="PerStateFMFunds" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerDonations" FilterControlAltText="Filter Donatons % column" HeaderText="Donations %" DataFormatString="{0:p}" UniqueName="PerDonations" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerFundDrives" FilterControlAltText="Filter Fund Drives % column" HeaderText="Fund Drives %" DataFormatString="{0:p}" UniqueName="PerFundDrives" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerFeeForService" FilterControlAltText="Filter Fee For Service % column" HeaderText="Fee For Service %" DataFormatString="{0:p}" UniqueName="PerFeeForService" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerOthers" FilterControlAltText="Filter Others % column" HeaderText="Others %" DataFormatString="{0:p}" UniqueName="PerOthers" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OthersDesc" FilterControlAltText="Filter Others Desc column" HeaderText="Others Desc" UniqueName="OthersDesc" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PerTotal" FilterControlAltText="Filter Total % column" HeaderText="Total %" DataFormatString="{0:p}" UniqueName="PerTotal" ColumnGroupName="BudgetInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <%--Community Information--%>
                                <telerik:GridBoundColumn DataField="CommunityName" FilterControlAltText="Filter Community Name column" HeaderText="Community Name" UniqueName="CommunityName" ColumnGroupName="CommunityInformation" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfHomes" FilterControlAltText="Filter # Homes column" HeaderText="# Homes" UniqueName="NumberOfHomes" ColumnGroupName="CommunityInformation" Visible="false" FooterText="Total Homes: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfComm" FilterControlAltText="Filter # Commercial column" HeaderText="# Commercial" UniqueName="NumberOfComm" ColumnGroupName="CommunityInformation" Visible="false" FooterText="Total Comm.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResidentPopulation" FilterControlAltText="Filter Resident Population column" HeaderText="Resident Population" UniqueName="ResidentPopulation" ColumnGroupName="CommunityInformation" Visible="false" FooterText="Total Pop.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strAidAgreements" FilterControlAltText="Filter Aid Agreements column" HeaderText="Aid Agreements" UniqueName="AidAgreements" ColumnGroupName="CommunityInformation" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbAidAgreements" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("AidAgreements").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="AidAgreementsChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock8" runat="server">
                                            <script type="text/javascript">
                                                function AidAgreementsChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("AidAgreements", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <%--Response History--%>
                                <telerik:GridBoundColumn DataField="strNERISCurrent" FilterControlAltText="Filter NERIS Current column" HeaderText="NERIS Current" UniqueName="NERISCurrent" ColumnGroupName="ResponseHistory" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbNERISCurrent" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("NERISCurrent").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="NERISCurrentChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock9" runat="server">
                                            <script type="text/javascript">
                                                function NERISCurrentChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("NERISCurrent", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseStructure" FilterControlAltText="Filter Structure column" HeaderText="Structure" UniqueName="ResponseStructure" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Structure: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseVehicle" FilterControlAltText="Filter Vehicle column" HeaderText="Vehicle" UniqueName="ResponseVehicle" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Vehicle: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseVegitation" FilterControlAltText="Filter Vegitation column" HeaderText="Vegitation" UniqueName="ResponseVegitation" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Veg.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseEMS" FilterControlAltText="Filter EMS column" HeaderText="EMS" UniqueName="ResponseEMS" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total EMS: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseRescue" FilterControlAltText="Filter Rescue column" HeaderText="Rescue" UniqueName="ResponseRescue" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Rescue: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseHazardous" FilterControlAltText="Filter Hazardous column" HeaderText="Hazardous" UniqueName="ResponseHazardous" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Haz.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseService" FilterControlAltText="Filter Service column" HeaderText="Service" UniqueName="ResponseService" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Service: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseGoodIntent" FilterControlAltText="Filter Good Intent column" HeaderText="Good Intent" UniqueName="ResponseGoodIntent" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total GI.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseFalse" FilterControlAltText="Filter False column" HeaderText="False" UniqueName="ResponseFalse" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total False: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseOther" FilterControlAltText="Filter Other column" HeaderText="Other" UniqueName="ResponseOther" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Other: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ResponseTotal" FilterControlAltText="Filter Total column" HeaderText="Total" UniqueName="ResponseTotal" ColumnGroupName="ResponseHistory" Visible="false" FooterText="Total Responses: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--Water Availability--%>
                                <telerik:GridBoundColumn DataField="strComHydrantSys" FilterControlAltText="Filter Commercial Hydrant column" HeaderText="Commercial Hydrant" UniqueName="ComHydrantSys" ColumnGroupName="WaterAvailability" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbComHydrantSys" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("ComHydrantSys").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="ComHydrantSysChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock10" runat="server">
                                            <script type="text/javascript">
                                                function ComHydrantSysChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("ComHydrantSys", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AvailableWaterCapacity" FilterControlAltText="Filter Avail Water column" HeaderText="Avail Water Cap" UniqueName="AvailableWaterCapacity" ColumnGroupName="WaterAvailability" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="WaterOnWheelsCapacity" FilterControlAltText="Filter Water On Wheels column" HeaderText="Water On Wheels" UniqueName="WaterOnWheelsCapacity" ColumnGroupName="WaterAvailability" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StationWaterCapacity" FilterControlAltText="Filter Station Water column" HeaderText="Station Water" UniqueName="StationWaterCapacity" ColumnGroupName="WaterAvailability" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strTankAtStation" FilterControlAltText="Filter Tank At Station column" HeaderText="Tank At Station" UniqueName="TankAtStation" ColumnGroupName="WaterAvailability" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbTankAtStation" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("TankAtStation").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="TankAtStationChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock11" runat="server">
                                            <script type="text/javascript">
                                                function TankAtStationChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("TankAtStation", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <%--Training--%>
                                <telerik:GridBoundColumn DataField="YearlyTrainingHours" FilterControlAltText="Filter Yearly Training column" HeaderText="Yearly Training" UniqueName="YearlyTrainingHours" ColumnGroupName="Training" Visible="false" FooterText="Total Training Hrs.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfListedTrainings" FilterControlAltText="Filter # Tranings column" HeaderText="# Tranings" UniqueName="NumberOfListedTrainings" ColumnGroupName="Training" Visible="false" FooterText="Total Trainings: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--Apparatus--%>
                                <telerik:GridBoundColumn DataField="strApparatusPartOfProject" FilterControlAltText="Filter Part of Project column" HeaderText="Part of Project" UniqueName="ApparatusPartOfProject" ColumnGroupName="Apparatus" Visible="false">
                                        <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbApparatusPartOfProject" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("ApparatusPartOfProject").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="ApparatusPartOfProjectChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                            <script type="text/javascript">
                                                function ApparatusPartOfProjectChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("ApparatusPartOfProject", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strPumpTestsConducted" FilterControlAltText="Filter Pump Tests Conducted column" HeaderText="Pump Tests Conducted" UniqueName="PumpTestsConducted" ColumnGroupName="Apparatus" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbPumpTestsConducted" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("PumpTestsConducted").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="PumpTestsConductedChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock13" runat="server">
                                            <script type="text/javascript">
                                                function PumpTestsConductedChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("PumpTestsConducted", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ExplainNoPumpTests" FilterControlAltText="Filter No Pump Tests Exp column" HeaderText="No Pump Tests Exp" UniqueName="ExplainNoPumpTests" ColumnGroupName="Apparatus" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strHoseTestsConducted" FilterControlAltText="Filter Hose Test Conducted column" HeaderText="Hose Test Conducted" UniqueName="HoseTestConducted" ColumnGroupName="Apparatus" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbHoseTestConducted" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("HoseTestConducted").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="HoseTestConductedChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock14" runat="server">
                                            <script type="text/javascript">
                                                function HoseTestConductedChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("HoseTestConducted", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ExplainNoHostTests" FilterControlAltText="Filter No Hose Tests Exp column" HeaderText="No Hose Tests Exp" UniqueName="ExplainNoHostTests" ColumnGroupName="Apparatus" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfListedApparatus" FilterControlAltText="Filter # Apparatus column" HeaderText="# Apparatus" UniqueName="NumberOfListedApparatus" ColumnGroupName="Apparatus" Visible="false" FooterText="Total Apparatus: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--Communication--%>
                                <telerik:GridBoundColumn DataField="strCommunicationProject" FilterControlAltText="Filter Part of Proj column" HeaderText="Part of Proj" UniqueName="CommunicationProject" ColumnGroupName="Communication" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbCommunicationProject" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("CommunicationProject").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="CommunicationProjectChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock15" runat="server">
                                            <script type="text/javascript">
                                                function CommunicationProjectChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("CommunicationProject", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HandheldRadios" FilterControlAltText="Filter Hand Radio column" HeaderText="Hand Radio" UniqueName="HandheldRadios" ColumnGroupName="Communication" Visible="false" FooterText="Total Radios: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BaseStations" FilterControlAltText="Filter Base Stations column" HeaderText="Base Stations" UniqueName="BaseStations" ColumnGroupName="Communication" Visible="false" FooterText="Total Base St.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MobileRadios" FilterControlAltText="Filter Mobile Radios column" HeaderText="Mobile Radios" UniqueName="MobileRadios" ColumnGroupName="Communication" Visible="false" FooterText="Total Mob. Rad.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApparatusWoRadio" FilterControlAltText="Filter App W/O Radio column" HeaderText="App W/O Radio" UniqueName="ApparatusWoRadio" ColumnGroupName="Communication" Visible="false" FooterText="Total No Rad.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LawEnforcement" FilterControlAltText="Filter Law Enforcement column" HeaderText="Law Enforcement" UniqueName="LawEnforcement" ColumnGroupName="Communication" Visible="false" FooterText="Total LE: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EmergencyMedical" FilterControlAltText="Filter Emergency Medical column" HeaderText="Emergency Medical" UniqueName="EmergencyMedical" ColumnGroupName="Communication" Visible="false" FooterText="Total EM: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OtherFireDepts" FilterControlAltText="Filter Other Fire Depts column" HeaderText="Other Fire Depts" UniqueName="OtherFireDepts" ColumnGroupName="Communication" Visible="false" FooterText="Total FD: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Other" FilterControlAltText="Filter Other column" HeaderText="Other" UniqueName="Other" ColumnGroupName="Communication" Visible="false" FooterText="Total Other: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OtherDescription" FilterControlAltText="Filter Other Description column" HeaderText="Other Description" UniqueName="OtherDescription" ColumnGroupName="Communication" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AreasNotCovered" FilterControlAltText="Filter Areas Not Covered column" HeaderText="Areas Not Covered" UniqueName="AreasNotCovered" ColumnGroupName="Communication" Visible="false" FooterText="Total NC: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DescribeAreasNotCovered" FilterControlAltText="Filter Describe Not Covered column" HeaderText="Describe Not Covered" UniqueName="DescribeAreasNotCovered" ColumnGroupName="Communication" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfCommunicationDevicesListed" FilterControlAltText="Filter # Comm Dev column" HeaderText="# Comm Dev" UniqueName="NumberOfCommunicationDevicesListed" ColumnGroupName="Communication" Visible="false" FooterText="Total Comm.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--Hazards/Threads--%>
                                <telerik:GridBoundColumn DataField="NumberOfHazardsThreatsListed" FilterControlAltText="Filter # Haz/Threats column" HeaderText="# Haz/Threats" UniqueName="NumberOfHazardsThreatsListed" ColumnGroupName="HazardsThreats" Visible="false" FooterText="Total Haz/Threat: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--PPE--%>
                                <telerik:GridBoundColumn DataField="strPPEPartOfProject" FilterControlAltText="Filter PPE Part Of Project column" HeaderText="Part Of Proj" UniqueName="PPEPartOfProject" ColumnGroupName="PPE" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbPPEPartOfProject" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("PPEPartOfProject").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="PPEPartOfProjectChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock16" runat="server">
                                            <script type="text/javascript">
                                                function PPEPartOfProjectChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("PPEPartOfProject", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strPPEInspected" FilterControlAltText="Filter PPE Inspected column" HeaderText="PPE Inspected" UniqueName="PPEInspected" ColumnGroupName="PPE" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbPPEInspected" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("PPEInspected").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="PPEInspectedChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock17" runat="server">
                                            <script type="text/javascript">
                                                function PPEInspectedChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("PPEInspected", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfPPEListed" FilterControlAltText="Filter # PPE column" HeaderText="# PPE" UniqueName="NumberOfPPEListed" ColumnGroupName="PPE" Visible="false" FooterText="Total PPE: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfSCBAListed" FilterControlAltText="Filter # SCBA column" HeaderText="# SCBA" UniqueName="NumberOfSCBAListed" ColumnGroupName="PPE" Visible="false" FooterText="Total SCBA: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--EquipmentNeeded--%>
                                <telerik:GridBoundColumn DataField="SpecificNeeds" FilterControlAltText="Filter Specific Needs column" HeaderText="Specific Needs" UniqueName="SpecificNeeds" ColumnGroupName="EquipmentNeeded" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strISOImpacted" FilterControlAltText="Filter ISO Impacted column" HeaderText="ISO Impacted" UniqueName="ISOImpacted" ColumnGroupName="EquipmentNeeded" Visible="false">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbISOImpacted" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("ISOImpacted").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="ISOImpactedChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock18" runat="server">
                                            <script type="text/javascript">
                                                function ISOImpactedChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("ISOImpacted", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ISOImpactExplanation" FilterControlAltText="Filter ISO Explanation column" HeaderText="ISO Explanation" UniqueName="ISOImpactExplanation" ColumnGroupName="EquipmentNeeded" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NumberOfEquipmentNeeded" FilterControlAltText="Filter # Equipment column" HeaderText="# Equipment" UniqueName="NumberOfEquipmentNeeded" ColumnGroupName="EquipmentNeeded" Visible="false" FooterText="Total Equip.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AmountOfEquipmentNeeded" FilterControlAltText="Filter Equipment Cost column" DataFormatString="{0:C}" HeaderText="Equipment Cost" UniqueName="AmountOfEquipmentNeeded" ColumnGroupName="EquipmentNeeded" Visible="false" FooterText="Total Equip $: " FooterAggregateFormatString="{0:C}" Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <%--Project Budget--%>
                                <telerik:GridBoundColumn DataField="TotalProjectCost" FilterControlAltText="Filter Total Project Cost column" DataFormatString="{0:C}" HeaderText="Total Project Cost" UniqueName="TotalProjectCost" FooterText="Total Proj.: " Aggregate="Sum" FooterAggregateFormatString="{0:C}" ColumnGroupName="ProjectBudget">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AmountRequested" FilterControlAltText="Filter Amount Requested column" DataFormatString="{0:C}" HeaderText="Amount Requested" UniqueName="AmountRequested" FooterText="Total Req.: " Aggregate="Sum" FooterAggregateFormatString="{0:C}" ColumnGroupName="ProjectBudget">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StipendAmount" FilterControlAltText="Filter Stipend Amount column" DataFormatString="{0:C}" HeaderText="Stipend Amount" UniqueName="StipendAmount" FooterText="Total Stip.: " Aggregate="Sum" FooterAggregateFormatString="{0:C}" ColumnGroupName="ProjectBudget">
                                </telerik:GridBoundColumn>
                                <%--Application Review--%>
                                <telerik:GridBoundColumn DataField="strNERISCompliant" FilterControlAltText="Filter NERIS Compliant column" HeaderText="NERIS Compliant" UniqueName="NERISCompliant" ColumnGroupName="ApplicationReview">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbNERISCompliant" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("NERISCompliant").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="NERISCompliantChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock19" runat="server">
                                            <script type="text/javascript">
                                                function NERISCompliantChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("NERISCompliant", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="strPumpTestCompliant" FilterControlAltText="Filter Pump Test Compliant column" HeaderText="Pump Test Compliant" UniqueName="PumpTestCompliant" ColumnGroupName="ApplicationReview">
                                    <FilterTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbPumpTestCompliant" Width="150px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("PumpTestCompliant").CurrentFilterValue %>'
                                            runat="server" OnClientSelectedIndexChanged="PumpTestCompliantChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" />
                                                <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                <telerik:RadComboBoxItem Text="No" Value="No" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock ID="RadScriptBlock20" runat="server">
                                            <script type="text/javascript">
                                                function PumpTestCompliantChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("PumpTestCompliant", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                </telerik:GridBoundColumn>                            
                                <%--Application Scores--%>
                                <telerik:GridBoundColumn DataField="TrainingPoints" FilterControlAltText="Filter Training Points column" HeaderText="Training Points" UniqueName="TrainingPoints" ColumnGroupName="ApplicationScores" FooterText="Total Training: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FinancialNeedGrade" FilterControlAltText="Filter Financial Need Grade column" HeaderText="Financial Need Grade" UniqueName="FinancialNeedGrade" ColumnGroupName="ApplicationScores" FooterText="Total Fin. Need: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProblemGrade" FilterControlAltText="Filter Problem Grade column" HeaderText="Problem Grade" UniqueName="ProblemGrade" ColumnGroupName="ApplicationScores" FooterText="Total Problem: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BenefitGrade" FilterControlAltText="Filter Benefit Grade column" HeaderText="Benefit Grade" UniqueName="BenefitGrade" ColumnGroupName="ApplicationScores" FooterText="Total Benefit: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ConsequencesGrade" FilterControlAltText="Filter Consequences Grade column" HeaderText="Consequences Grade" UniqueName="ConsequencesGrade" ColumnGroupName="ApplicationScores" FooterText="Total Consequences: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AppCompletenessGrade" FilterControlAltText="Filter App Completeness Grade column" HeaderText="App Completeness Grade" UniqueName="AppCompletenessGrade" ColumnGroupName="ApplicationScores" FooterText="Total App Comp.: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TotalScore" FilterControlAltText="Filter Total Score column" HeaderText="Total Score" UniqueName="TotalScore" ColumnGroupName="ApplicationScores" FooterText="Total Score: " Aggregate="Sum">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ApplicationId" FilterControlAltText="Filter ApplicationId column" HeaderText="ApplicationId" UniqueName="ApplicationId" Display="False" Resizable="False">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings>
                            <EditColumn ShowNoSortIcon="False"></EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <FilterMenu RenderMode="Lightweight"></FilterMenu>
                        <HeaderContextMenu RenderMode="Lightweight"></HeaderContextMenu>
                    </telerik:RadGrid>
                </div>
            </div>
    </div>
</asp:Content>
