<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicationPrint.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Reporting.ApplicationPrint" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body {
            max-width: 800px;
            margin:auto;
        }
        h1 {
            font-size:1.6em;
            font-weight:bold;
        }
        .sectionTable {
             margin-left:23px;clear: both; border: 1px solid #000000; border-right: 1px solid #000000; width: 95%; margin-top: 10px; border-collapse: collapse; text-transform: none; font-family:Arial, Helvetica, sans-serif !important; font-size:15px !important;
        }
        .sectionTitle {
            text-transform: uppercase; padding: 5px; background: #000000;border: 1px solid #000000; color: #ffffff;
        }
        .rowHeader {
            border: 1px solid #000000; padding: 5px; line-height: 140%; vertical-align: middle; width: 60%;
        }
        .rowData {
            border: 1px solid #000000;text-transform:none; padding: 5px; line-height: 140%; vertical-align: middle;
        }
        .rowDataRight {
            border: 1px solid #000000;text-transform:none; padding: 5px; line-height: 140%; vertical-align: middle; text-align:right;
        }
        .rowFundJustification {
            text-transform:none; padding: 5px; line-height: 140%; vertical-align: top;
        }
        .listHeader {
            border: 1px solid #000000; padding: 6px;background:#ccc; line-height: 150%;
        }
        .listHeaderTd {
            border: 1px solid #000000; padding: 6px;background:#ccc; line-height: 150%;
        }
        
        @media print {
            .toolbar {
                display:none !important;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="dvApplication">
            <div id="toolbar" class="toolbar">
                <asp:LinkButton ID="lnkBack" runat="server" Text="Back" OnClick="lnkBack_Click"></asp:LinkButton> | <asp:LinkButton ID="lnkPrint" runat="server" OnClientClick="window.print()" Text="Print Appliction"></asp:LinkButton> | <asp:LinkButton ID="btnSavePDF" runat="server" Text="Download PDF" OnClick="btnSavePDF_Click"></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfApplicationId" runat="server" />
            <table  style="margin-left:23px;font-size:16px; width:95%; padding:5px; font-family:Arial, Helvetica, sans-serif !important; font-size:16px !important;" cellspacing="0" cellpadding="0">
                 <tr>
                    <td style="text-align:center; font-size:16px; font-family:Arial, Helvetica, sans-serif !important; font-size:16px !important;" valign="top"><h1>APPLICATION FOR <span id="spFiscalYear" runat="server"></span> FIRE PROTECTION GRANT</h1><br />
                    Applications will be accepted from <span id="spStartDate" runat="server"></span> to <span id="spEndDate" runat="server"></span></td>
                </tr>
            </table>
            <div id="dvEligibilityRequirements" runat="server"></div>
            <div style="page-break-before:always"></div>
            <div id="dvGeneralInformation" style="page-break-before:always">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>GENERAL INFORMATION</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Grant Request Type
                        </td>
                        <td colspan="3" class="rowData" id="tdGrantRequestType" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Fire Department ID Number (using NFIRS identifier)
                        </td>
                        <td colspan="3" class="rowData" id="tdFireDeptId" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Fire Department Name
                        </td>
                        <td colspan="3" class="rowData" id="tdDepartmentName" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Fire Chief Name
                        </td>
                        <td colspan="3" class="rowData" id="tdFireCheifName" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Insurance Services Office (ISO) Rating
                        </td>
                        <td colspan="3" class="rowData" id="tdISO" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            County
                        </td>
                        <td colspan="3" class="rowData" id="tdCounty" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Department Type?
                        </td>
                        <td colspan="3" class="rowData" id="tdDepartmentType" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What kind of organization is your fire department?
                        </td>
                        <td colspan="3" class="rowData" id="tdOrganizationType" runat="server">
					        
				        </td>
                    </tr>
                    <tr id="trCountyApp" runat="server">
                        <td class="rowHeader">
				            Are all of the County departments NFIRS and Pump Test complient?
                        </td>
                        <td colspan="3" class="rowData" id="tdAllNFIRSCompliant" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader" colspan="4">
                            How many stations are in your organization?
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Main
                        </td>
                        <td colspan="3" class="rowData" id="tdMainStations" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Substations
                        </td>
                        <td colspan="3" class="rowData" id="tdSubstations" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Admin
                        </td>
                        <td colspan="3" class="rowData" id="tdAdmin" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Type of community your organization serves Based on population density
                        </td>
                        <td colspan="3" class="rowData" id="tdCommunityType" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="rowHeader" style="font-weight:bold">
                            Mailing Address
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Address
                        </td>
                        <td colspan="3" class="rowData" id="tdAddress" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            City
                        </td>
                        <td colspan="3" class="rowData" id="tdCity" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            State
                        </td>
                        <td colspan="3" class="rowData" id="tdState" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            ZipCode
                        </td>
                        <td colspan="3" class="rowData" id="tdZipCode" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Phone Number
                        </td>
                        <td colspan="3" class="rowData" id="tdPhone" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Email Address
                        </td>
                        <td colspan="3" class="rowData" id="tdEmail" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Name of Person Completing this application?
                        </td>
                        <td colspan="3" class="rowData" id="tdPersonCompletingApp" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Are you a fire department member?
                        </td>
                        <td colspan="3" class="rowData" id="tdFireDepartmentMember" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            How many firefighters?
                        </td>
                        <td colspan="3" class="rowData" id="tdFireFighters" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            How many are FF-I Certified?
                        </td>
                        <td colspan="3" class="rowData" id="tdFFI" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            How many are FF-II Certified?
                        </td>
                        <td colspan="3" class="rowData" id="tdFFII" runat="server">
					        
				        </td>
                    </tr>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvBudgetInformation">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>BUDGET INFORMATION</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What is your fire departments operating budget, including
                            personnel costs, for your current fiscal year?(in dollars)
                        </td>
                        <td colspan="3" class="rowData" id="tdOperatingBudget" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What is the current Protection Fire Fund distribution?
                        </td>
                        <td colspan="3" class="rowData" id="tdFPFDistribution" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What is the total stipend carryover?
                        </td>
                        <td colspan="3" class="rowData" id="tdStipendCarryover" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What is the approved total carryover balance, if any, of
                            Protection Fire Funds maintained by the department?
                        </td>
                        <td colspan="3" class="rowData" id="tdCarryoverBalance" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What was the purpose of the approval carryover?
                        </td>
                        <td colspan="3" class="rowData" id="tdCarryoverPurpose" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader" colspan="2">
				            What percentage of your annual operating budget is derived from:
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Taxes?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerTaxes" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Grants?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerGrants" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            State Fire Marshal Funds?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerStateFMFunds" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Donations?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerDonations" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Fund Drives?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerFundDrives" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Fee For Service?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerFeeForService" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Others?
                        </td>
                        <td colspan="3" class="rowData" id="tdPerOthers" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Please Explain (For 'Others')
                        </td>
                        <td colspan="3" class="rowData" id="tdPerOthersDesc" runat="server">
					        
				        </td>
                    </tr>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvCommunityInformation">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>COMMUNITY INFORMATION</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Name of Community Protected?
                        </td>
                        <td colspan="3" class="rowData" id="tdCommunityProtected" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Number of commercial buildings protected in fire district?
                        </td>
                        <td colspan="3" class="rowData" id="tdCommercialBuildings" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Number of homes protected in fire district?
                        </td>
                        <td colspan="3" class="rowData" id="tdHomesProtected" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            What is the permanent resident population of the community you serve?
                        </td>
                        <td colspan="3" class="rowData" id="tdPopulation" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Do you have formal automatic aid or mutual aid agreements?
                        </td>
                        <td colspan="3" class="rowData" id="tdAgreements" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="rowHeader">
				            List adjacent automatic aid fire districts (with written agreements)
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td class="listHeaderTd">
                            S.No
                        </td>
                        <td colspan="3" class="listHeaderTd">
                            Automatic Aide Fire District
                        </td>
                    </tr>
                    <asp:Literal ID="ltrAidDistricts" runat="server"></asp:Literal>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvResponseHistory">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>RESPONSE HISTORY</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="rowHeader">
				            Are you NFIRS Current?
                        </td>
                        <td colspan="2" class="rowData" id="tdNFIRSCurrent" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader" colspan="4">
				            How many reponses per category?
                        </td>
                    </tr>
                    <tr>
                        <td class="rowData">
                            Structure Fire (IT 110-118, 120-123)<br />
                            <span id="spStructureFire" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Vehicle Fire (IT 130-138)<br />
                            <span id="spVehiclefire" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Vegitation Fire (IT 140-143)<br />
                            <span id="spVegitationFire" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            EMS (IT 300-323)<br />
                            <span id="spEMS" runat="server" style="font-weight:bold"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowData">
                            Rescue (IT 331-381)<br />
                            <span id="spRescue" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Hazardous Condition (IT 400-482)<br />
                            <span id="spHazardous" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Service Calls (IT 500-571)<br />
                            <span id="spService" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Good Intent Calls (IT 600-671)<br />
                            <span id="spGoodIntent" runat="server" style="font-weight:bold"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowData">
                            False Calls (IT 700-751)<br />
                            <span id="spFalse" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td class="rowData">
                            Other<br />
                            <span id="spOther" runat="server" style="font-weight:bold"></span>
                        </td>
                        <td colspan="2" class="rowData">
                            Total Calls:<br />
                            <span id="spTotalCalls" runat="server" style="font-weight:bold"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvWaterAvailability">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>WATER AVAILABILITY</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Community hydrant system?
                        </td>
                        <td colspan="3" class="rowData" id="tdCommunityHydrant" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Total capacity of available water storage(in gallons)
                        </td>
                        <td colspan="3" class="rowData" id="tdAvailableWater" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Total capacity of water storage on wheels (in gallons)
                        </td>
                        <td colspan="3" class="rowData" id="tdWaterOnWheels" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Total capacity of water stored at station (in gallons)?
                        </td>
                        <td colspan="3" class="rowData" id="tdWaterAtStation" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Water storage tank with fire hydrant at station?
                        </td>
                        <td colspan="3" class="rowData" id="tdStorageTankAtStation" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="rowHeader">
				            Describe additional water source(s):
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td class="listHeaderTd">
                            S.No
                        </td>
                        <td colspan="2" class="listHeaderTd">
                            Source
                        </td>
                        <td class="listHeaderTd">
                            Capacity
                        </td>
                    </tr>
                    <asp:Literal ID="ltrWaterSources" runat="server"></asp:Literal>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvTraining">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>TRAINING</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Average # of training hours per Firefighter per year:
                        </td>
                        <td colspan="3" class="rowData" id="tdTrainingHours" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="rowHeader">
				            How many training opportunities has this department offered in the last calendar year?
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td colspan="2" class="listHeaderTd">
                            Training Details
                        </td>
                        <td colspan="2" class="listHeaderTd">
                            TrainingDocumentation
                        </td>
                    </tr>
                    <asp:Literal ID="ltrTrainings" runat="server"></asp:Literal>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvApparatus">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>APPARATUS</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Apparatus is part of the Project?
                        </td>
                        <td colspan="3" class="rowData" id="tdApparatusPartOfProject" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Are pump tests conducted annually on apparatus?
                        </td>
                        <td colspan="3" class="rowData" id="tdPumpTestsConducted" runat="server">
					        
				        </td>
                    </tr>
                    <tr id="trNoPumpTests" runat="server" visible="false">
                        <td class="rowHeader">
                            Explain if not tested
                        </td>
                        <td colspan="3" class="rowData" id="tdNoPumpTestsExplanation" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Has your annual hose testing been conducted?
                        </td>
                        <td colspan="3" class="rowData" id="tdHoseTestsConducted" runat="server">
					        
				        </td>
                    </tr>
                    <tr id="trNoHoseTests" runat="server" visible="false">
                        <td class="rowHeader">
                            Explain if not tested
                        </td>
                        <td colspan="3" class="rowData" id="tdNoHoseTestsExplanation" runat="server">
					        
				        </td>
                    </tr>
                </table>
                <table class="sectionTable" style="margin-top:0px !important; border-top:none">
                    <tr>
                        <td colspan="7" class="rowHeader">
				            List Pump Capable Appratus:
                        </td>
                    </tr>
                    <tr>
                        <td class="listHeaderTd">
                            Apparatus ID
                        </td>
                        <td class="listHeaderTd">
                            Vehicle Identification
                        </td>
                        <td class="listHeaderTd">
                            License Plate
                        </td>
                        <td class="listHeaderTd">
                            GPM
                        </td>
                        <td class="listHeaderTd">
                            Test Date
                        </td>
                        <td class="listHeaderTd">
                            Pass/Fail
                        </td>
                        <td class="listHeaderTd">
                            Comments
                        </td>
                    </tr>
                    <asp:Literal ID="ltrApparatusDetails" runat="server"></asp:Literal>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvCommunicationEquipment">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>COMMUNICATION EQUIPMENT</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Communication Equipment is part of the prject?
                        </td>
                        <td colspan="3" class="rowData" id="tdCommunicationEquipmentPartOfProject" runat="server">
					        
				        </td>
                    </tr>
                    <tbody id="IsPart" runat="server">
                        <tr>
                            <td colspan="4" class="rowHeader">
				                Do you have any of the following?
                            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Handheld Radios?
                            </td>
                            <td colspan="3" class="rowData" id="tdHandheldRadios" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Base Stations?
                            </td>
                            <td colspan="3" class="rowData" id="tdBaseStations" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Mobile Radios?
                            </td>
                            <td colspan="3" class="rowData" id="tdMobileRadios" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Do you have any apparatus without a mobile radio?
                            </td>
                            <td colspan="3" class="rowData" id="tdApparatusWithoutRadio" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="rowHeader">
				                List Communication Equipment by Type
                            </td>
                        </tr>
                        <tr class="listHeader">
                            <td colspan="1" class="listHeaderTd">
                                Number
                            </td>
                            <td colspan="2" class="listHeaderTd">
                                Communication Equipment
                            </td>
                            <td colspan="2" class="listHeaderTd">
                                Quantity
                            </td>
                        </tr>
                        <asp:Literal ID="ltrCommunicationEquipment" runat="server"></asp:Literal>
                        <tr>
                            <td colspan="4" class="rowHeader">
				                Do you have interoperability with any of the following agencies?
                            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Law Enforcement?
                            </td>
                            <td colspan="3" class="rowData" id="tdLawEnforcement" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Emergency Medical?
                            </td>
                            <td colspan="3" class="rowData" id="tdEmergencyMedical" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Other Fire Departments?
                            </td>
                            <td colspan="3" class="rowData" id="tdOtherDepartments" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Other (that could not be decribed above)?
                            </td>
                            <td colspan="3" class="rowData" id="tdOtherInterop" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Describe (if 'yes' on others or any additional comments)?
                            </td>
                            <td colspan="3" class="rowData" id="tdOtherInteropDesc" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                Do you have any areas in your jurisdiction which are NOT covered by a repeater?
                            </td>
                            <td colspan="3" class="rowData" id="tdOtherJurisdictions" runat="server">
					        
				            </td>
                        </tr>
                        <tr id="trOtherJurisdictionsNotCovered" runat ="server" visible="false">
                            <td class="rowHeader">
				                Describe (if 'yes' on above)?
                            </td>
                            <td colspan="3" class="rowData" id="tdOtherJurisdictionsNotCovered" runat="server">
					        
				            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvHazardsThreats">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>HAZARDS/THREATS</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="rowHeader">
				            Describe the threat to the community: (i.e., fuel storage bulk plants, railroads, high hazard occupancies, etc.)
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td colspan="1" class="listHeaderTd">
                            Hazard Type
                        </td>
                        <td colspan="3" class="listHeaderTd">
                            Hazard Detail
                        </td>
                    </tr>
                    <asp:Literal ID="ltrHazards" runat="server"></asp:Literal>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvPPE">
                <table class="sectionTable">
                    <tr>
                        <td colspan="4" class="sectionTitle">
                            <b>CURRENT PERSONAL PROTECTIVE EQUIPMENT (PPE)</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            PPE is part of the Project ?
                        </td>
                        <td colspan="3" class="rowData" id="tdPPEIsPart" runat="server">
					        
				        </td>
                    </tr>
                    <tbody id="tbPPEPart" runat="server">
                        <tr>
                            <td colspan="4" class="rowHeader">
				                Bunker Gear
                            </td>
                        </tr>
                        <tr>
                            <td class="rowHeader">
				                All PPE inspected to the most current NFPA 1851 standard?
                            </td>
                            <td colspan="3" class="rowData" id="tdPPEInspected" runat="server">
					        
				            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="rowHeader">
				                Standard Compliant PPE
                            </td>
                        </tr>
                        <tr class="listHeader">
                            <td colspan="1" class="listHeaderTd">
                                Year
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Quantity
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Age
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Condition
                            </td>
                        </tr>
                        <asp:Literal ID="ltrPPE" runat="server"></asp:Literal>
                        <tr>
                            <td colspan="4" class="rowHeader">
				                Standard Compliant SCBA
                            </td>
                        </tr>
                        <tr class="listHeader">
                            <td colspan="1" class="listHeaderTd">
                                Year
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Quantity
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Age
                            </td>
                            <td colspan="1" class="listHeaderTd">
                                Condition
                            </td>
                        </tr>
                        <asp:Literal ID="ltrSCBA" runat="server"></asp:Literal>
                    </tbody>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvEquipmentNeeds">
                <table class="sectionTable">
                    <tr>
                        <td colspan="5" class="sectionTitle">
                            <b>EQUIPMENT NEEDS</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="rowHeader">
				            List in <strong>priority order</strong>, and explain the equipment needs of your department and the total costs of fulfilling the needs.
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td colspan="1" class="listHeaderTd">
                            Priority Order #
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Priority Order Requesting From
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Equipment Needed
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Quantity
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Total Cost
                        </td>
                    </tr>
                    <asp:Literal ID="ltrEquipment" runat="server"></asp:Literal>
                </table>
                <table class="sectionTable">
                    <tr>
                        <td class="rowHeader">
				            What (specifically) will you purchase if awarded this grant?:
                        </td>
                        <td colspan="4" class="rowData" id="tdWhatPurchased" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Will fulfilling this need impact your organization's ISO rating?:
                        </td>
                        <td colspan="4" class="rowData" id="tdISOAffected" runat="server">
					        
				        </td>
                    </tr>
                    <tr id="trISOChangeExp" runat="server">
                        <td class="rowHeader">
				            Please Explain:
                        </td>
                        <td colspan="4" class="rowData" id="tdISOChangeExp" runat="server">
					        
				        </td>
                    </tr>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvGrantFundingJustification">
                <table class="sectionTable">
                    <tr>
                        <td colspan="5" class="sectionTitle">
                            <b>GRANT FUNDING JUSTIFICATION</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Project is a critical need? 
                        </td>
                        <td colspan="4" class="rowData" id="tdCriticalNeed" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowFundJustification" colspan="5">
				            <strong>Financial Need: </strong><span id="spFinancialNeed" runat="server"></span>
                        </td>
                    </tr>
                   <tr>
                        <td class="rowFundJustification" colspan="5">
				            <strong>Problem: </strong><span id="spProblem" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowFundJustification" colspan="5">
				            <strong>Benefit to the Community: </strong><span id="spBenefit" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowFundJustification" colspan="5">
				            <strong>Consequences: </strong><span id="spConsequences" runat="server"></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="page-break-before:always"></div>
            <div id="dvProjectBudgetSheet">
                <table class="sectionTable">
                    <tr>
                        <td colspan="5" class="sectionTitle">
                            <b>Project Budget Sheet</b>
                        </td>
                    </tr>
                    <tr class="listHeader">
                        <td colspan="1" class="listHeaderTd">
                            Priority Order #
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Priority Order Requesting From
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Equipment Needed
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Quantity
                        </td>
                        <td colspan="1" class="listHeaderTd">
                            Total Cost
                        </td>
                    </tr>
                    <asp:Literal ID="ltrProjectBudgetEquipment" runat="server"></asp:Literal>
                </table>
                <table class="sectionTable">
                    <tr>
                        <td class="rowHeader">
				            Total Project Cost:
                        </td>
                        <td colspan="4" class="rowDataRight" id="tdProjectCost" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Grant Amount Requested:
                        </td>
                        <td colspan="4" class="rowDataRight" id="tdAmountRequested" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Total amount the Department is responsible for:
                        </td>
                        <td colspan="4" class="rowDataRight" id="tdDepartmentResponsibility" runat="server">
					        
				        </td>
                    </tr>
                    <tr>
                        <td class="rowHeader">
				            Stipend Amount Requested:
                        </td>
                        <td colspan="4" class="rowDataRight" id="tdStipendAmountRequested" runat="server">
					        
				        </td>
                    </tr>
                </table>
            </div>
             <div style="page-break-before:always"></div>
            <div id="dvFAStatement">
                <table class="sectionTable">
                    <tr>
                        <td colspan="5">
                            <b>FISCAL AGENT COMMITMENT STATEMENT</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="padding: 5px">
                            I, as the fiscal agent for the <span id="spDepartmentName" runat="server" style="text-decoration:underline"></span> department, certify that a minimum 20% in matching funds are committed
                            to the project for which this application is submitted.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border-bottom:1px solid black; padding: 10px 5px 0px 5px" id="dvFAName" runat="server">
                            
                        </td>
                        <td colspan="1" style="border-bottom:1px solid black; padding: 10px 5px 0px 5px; text-align: right" id="dvFATitle" runat="server">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding:5px 5px 10px 5px">
                            Name of County/Municipal Fiscal Agent
                        </td>
                        <td colspan="1" style="padding: 5px 5px 10px 5px; text-align: right">
                            Title
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border-bottom:1px solid black; font-family:'Brush Script Std'; padding: 10px 5px 0px 5px" id="tdFASignature" runat="server">
                            
                        </td>
                        <td colspan="1" style="border-bottom:1px solid black; padding: 10px 5px 0px 5px; text-align:right" id="tdFADate" runat="server">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding: 5px">
                            Signature of County/Municipal Fiscal Agent
                        </td>
                        <td colspan="1" style="text-align: right;padding: 5px">
                            Date
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
