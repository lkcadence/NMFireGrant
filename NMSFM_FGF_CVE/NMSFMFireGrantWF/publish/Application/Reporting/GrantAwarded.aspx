<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GrantAwarded.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Reporting.GrantAwarded" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="repHead" runat="server">
    <title></title>
    <%--<link href="../../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <style type="text/css">
        body {
            max-width: 800px;
            margin:auto;
            font-size:.9em;
        }
        h1 {
            font-size:1.6em;
            font-weight:bold;
        }
         .toolbar ul {
          list-style-type: none;
          margin: 0;
          padding: 0;
          overflow: hidden;
          background-color: #333;
        }
        .toolbar li {
          float: left;
          display: inline;
          text-decoration: none;
        }
        .toolbar li a {
          display: block;
          color: white;
          text-align: center;
          padding: 14px 16px;
          text-decoration: none;
        }

        /* Change the link color to #111 (black) on hover */
        .toolbar li a:hover {
          background-color: #111;
        }
        
        @media print {
            .toolbar {
                display:none !important;
            }
        }

        table, th, td {
            border: 1px solid;
            border-collapse:collapse;
            padding:3px 3px 3px 3px;
        }

        /*Boostrap css*/
        .row{width:100%;margin-right:0px;margin-left:0px; clear:both}
        .col-md-12,.col-md-3,.col-md-6,.col-md-8,.col-md-4{position:relative;min-height:1px;padding-right:0px;padding-left:0px}
        .col-md-12,.col-md-3,.col-md-6,.col-md-8,.col-md-4{float:left}
        .col-md-12{width:100%}.col-md-6{width:50%}.col-md-3{width:25%}.col-md-4{width:33%}.col-md-8{width:66%}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
        <div id="toolbar" class="toolbar">
            <ul>
                <li>
                    <asp:LinkButton ID="lnkBack" runat="server" Text="Back" OnClick="lnkBack_Click"></asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="lnkPrint" runat="server" OnClientClick="window.print()" Text="Print Appliction"></asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="btnSavePDF" runat="server" Text="Download PDF" OnClick="btnSavePDF_Click" Visible="false"></asp:LinkButton>
                </li>
            </ul> 
        </div>
        <asp:HiddenField ID="hfApplicationId" runat="server" />
        <div id="dvLetter" runat="server" style="margin: 0px 0px 0px 0px">
             <%--Begin Header--%>
            <div class="row" style="margin-bottom: 0;padding-bottom: 0;">
                <div class="col-md-6"></div>
                <div class="col-md-6" style="text-align:right;font-family:'Times New Roman', Times, serif;font-size:medium;font-weight:normal">
                    Invoice No: <asp:Label ID="lblInvoiceNumber" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row" style="margin-bottom: 1.5em;">
                <div class="col-md-4">
                    <div class="row" style="height:7em;">&nbsp;</div>
                    <div class="row" style="font-family:Cambria; font-size:1em">
                        <span style="font-weight:bold; border-width: 0; margin-bottom:0px">
                            <asp:Label ID="lblGovernor" runat="server"></asp:Label><br /> Governor
                        </span>
                    </div>
                    <%--<div class="row" style="height:14px;">&nbsp;</div>--%>
                    <div id="dvCabinetSec" runat="server" >
                        <div class="row" style="height:10px;">&nbsp;</div>
                            <div class="row" style="font-family:Cambria; font-size:1em; text-align:left;">
                                <span style="font-weight:bold; border-width: 0; text-align:right; margin-bottom:0px">
                                    <asp:Label ID="lblCabinetSec" runat="server"></asp:Label><br />Cabinet Secretary
                                </span>
                            </div>
                    </div>
                    <div id="dvDeputyCabinetSec2" runat="server">
                            <div class="row" style="height:10px;">&nbsp;</div>
                            <div class="row" style="font-family:Cambria; font-size:1em; text-align:left;">
                                <span style="font-weight:bold; border-width: 0; text-align:right; margin-bottom:0px">
                                    <asp:Label ID="lblDeputyCabinetSec2" runat="server"></asp:Label><br />Deputy Cabinet Secretary
                                </span>
                            </div>
                        </div>
                </div>
                <div class="col-md-4" style="text-align:center">
                    <img src="/Content/Images/NM-Department-of-Homeland-Security-and-Emergency-Management-logo.png" style="height:125px;width:125px;" />
                </div>
                <div class="col-md-4">
                     <div class="row" style="height:7em;">&nbsp;</div>
                     <div class="row" style="font-family:Cambria; font-size:1em; text-align:right">
                        <span style="font-weight:bold; margin-bottom:0px">
                            <asp:Label ID="lblDeputyCabinetSec" runat="server"></asp:Label><br />State Director
                        </span>
                    </div>
                    <div class="row" style="height:10px;">&nbsp;</div>
                        <div id="dvDeputyCabinetSec" runat="server">
                            <div class="row" style="font-family:Cambria; font-size:1em; text-align:right;">
                                <span style="font-weight:bold; border-width: 0; text-align:right; margin-bottom:0px">
                                    <asp:Label ID="lblFireMarshal" runat="server"></asp:Label><br />State Fire Marshal<br />
                                </span>
                            </div>
                        </div>
                            
                        
                    
                    <div class="row" style="font-family:Cambria; font-size:1em; text-align:right;">
                        <span style="font-weight:bold; border-width: 0; text-align:right; margin-bottom:0px">
                             
                        </span>
                    </div>
                </div>
            </div>
            <!--End Header-->
            <div class="row" style="margin-bottom: 2em; padding-top:2em">
                <asp:Label ID="lblDate" runat="server"></asp:Label>
            </div>
            <div class="row">
                <div class="col-md-6" style="margin-bottom: 1em">
                    <span>Treasurer:</span><br />
                    <span id="spDepartment" runat="server"></span><br />
                    <span id="spAddressDesc" runat="server"></span><br />
                    <span id="spCityStateZip" runat="server"></span>
                </div>
                <div class="col-md-6" style="margin-bottom: 1em">
                    <span id="spRemit1" runat="server"></span>
                    <span id="spRemit2" runat="server"></span>
                    <span id="spRemit3" runat="server"></span>
                    <span id="spRemit4" runat="server"></span>
                </div>
            </div>
            <div class="row" style="margin-bottom: 8px; font-weight:bold">
                Reference: FY <span id="spFY" runat="server"></span> New Mexico Fire Protection Grant Council Notification
            </div>
            <div class="row" style="margin-bottom:1em">
                Dear Chief:
            </div>
            <div class="row" style="margin-bottom:1em">
                Congratulations! Your grant application on behalf of the <span id="spDepartment2" runat="server"></span> has been reviewed and an award has been granted.
            </div>
            <div id="dvGrantApps" runat="server" class="row"  style="margin-bottom: 1em">
                Over <span id="spGrantApps" runat="server" style="color:red"></span> grant applications were submitted and over <span id="spGrantAmounts" runat="server"></span> million in needs were considered. The Fire Department has met the 
                minimum requirements and is clearly addressing a critical need affecting the ISO fire protection classification. The equipment  purchased with this grant shall meet the requirements of the latest Editions of NFPA. 
            </div>
            <div id="dvGrantAmount" runat="server" class="row" style="margin-bottom: 1em">
                A voucher or ACH deposit, in the amount of <span id="spGrantAmount" runat="server" style="font-weight:bold"></span> for the purchase of the approved project request, will be sent to your 
                local governing body Treasurer, to include <span id="spStipendAmount" runat="server" style="font-weight:bold"></span> for Stipends after approval by this office of the submitted project 
                specifications<%--, on or near <strong><span id="spAwardDate" runat="server"></span></strong>--%>.
            </div>
            <div id="dvDeadline" runat="server" class="row" style="margin-bottom: 1em">
                The deadline to encumber the money by contract with the vendor is May 31, <span id="spEncumberYear" runat="server"></span> If the bid amount exceeds the awarded 
                amount plus the required matching amount, the additional cost shall be the responsibility of the local government. If the specified 
                equipment may be purchased for less than the grant amount, the remaining money shall be returned to the grant fund. All 
                equipment purchased with grant funds must be inspected by this office upon receipt and the attached Project Close-Out Checklist 
                completed and submitted immediately thereafter.
            </div>
            <div id="dvDeadline2" runat="server" class="row" style="margin-bottom: 1em">
                Failure to meet deadlines will result in the loss of funds. If you need additional time to complete your project, your request for an 
                extension must be made in writing, explaining the need for additional time. Grant recipients also need the Council’s written 
                permission, to make changes to their projects. Project modifications must be requested in writing, and the modification shall not 
                significantly alter the original purpose of the approved application. Extension and modification requests are reviewed on a case-by-case basis and are not automatically granted.
            </div>
            <div class="row" style="margin-bottom: 1em">
                If further information is required, please contact Randy Varela, State Fire Marshal at (505)709-8150. 
            </div>
            <div class="row">
                <div class="col-md-8">
                    Sincerely,<br />
                    <asp:Label ID="lblFireMarshal2" runat="server"></asp:Label><br />
                    State Fire Marshal<br />
                    <span style="border-bottom:1px solid black"><img src="../../Content/images/Randy_Signature.jpg" height="60"/></span>
                </div>
                <div class="col-md-4">
                    Sincerely,<br />
                    Michael Daniels<br />
                    Grant Council Chair<br />
                    <span style="border-bottom:1px solid black"><img src="../../Content/images/MDaniels_Signature.png" height="60"/></span>
                </div>
            </div>
            <div id="dvPage2" runat="server">
                <div class="row" style="page-break-after:always"></div>
                <div class="row" style="margin-bottom: 0;padding-bottom: 0;">
                    <div class="col-md-6"></div>
                    <div class="col-md-6" style="text-align:right;font-family:'Times New Roman', Times, serif;font-size:medium;font-weight:normal">
                        Invoice No: <asp:Label ID="lblInvoiceNumber2" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row" style="text-align:center">
                    FY22 NEW MEXICO FIRE PROTECTION GRANT AWARD<br />
                    PROJECT CLOSEOUT CHECKLIST<br />Part 1
                </div>
                <div class="row" style="font-weight:bold; font-size:.8em; margin-bottom:1em">
                    Upon completion of the funded project, this checklist must be submitted to the State Fire Marshal’s Office, Fire Services Support Bureau.
                </div>
                <div class="row" style="margin-bottom:1em">
                    COUNTY:&nbsp;<span id="spCounty" runat="server" style="text-decoration:underline;font-weight:bold"></span>
                </div>
                <div class="row" style="margin-bottom:1em">
                    FUNDED PROJECT:&nbsp;<span id="spProjectApparatus" runat="server" style="text-decoration:underline;font-weight:bold"></span>
                </div>
                 <%--Added 10/12/2023 to fill equipment needs--%>
                <div class="row" style="margin-bottom:1em">
                    EQUIPMENT FUNDED:&nbsp;<span id="spProjectEquipment" runat="server" style="text-decoration:underline;font-weight:bold"></span>
                </div>
                <%--End Addition--%>
                <div class="row" style="margin-bottom:1em">
                    AMOUNT AWARDED:&nbsp;<span id="spGrantAmount2" runat="server" style="text-decoration:underline;font-weight:bold"></span>
                </div>                
                <div class="row" style="font-weight:bold">
                    PROJECT CHECKLIST
                </div>
                <div class="row">
                    <table style="width:100%">
                        <thead>
                            <tr style="font-weight:bold">
                                <th scope="col" style="width:30%">Benchmark</th>
                                <th scope="col" style="width:15%">Deadline</th>
                                <th scope="col" style="width:27%">Date</th>
                                <th scope="col">Name of SFMO<br />Representative</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    Project specifications submitted to State<br />
                                    Fire Marshal’s Office for<br />
                                    Review/Approval
                                </th>
                                <td>
                                    <%--Removed 10/13/2023 (vwd)--%>
                                    <%--January 15, <span id="spCklFiscalYear1" runat="server"></span>--%>
                                </td>
                                <td style="vertical-align:top">
                                    Submittal Date
                                </td>
                                <td style="vertical-align:top">
                                    Submitted To:
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    Approval from SFMO to proceed with<br />
                                    project specifications
                                </th>
                                <td>
                                    <%--Removed 10/13/2023 (vwd)--%>
                                    <%--February 15, <span id="spCklFiscalYear2" runat="server"></span>--%>
                                </td>
                                <td style="vertical-align:top">
                                    Approval Date
                                </td>
                                <td style="vertical-align:top">
                                    Approved By:
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    Funds Encumbered by Procurement<br />
                                    Code
                                </th>
                                <td>
                                    May 31, <span id="spCklFiscalYear3" runat="server"></span>
                                </td>
                                <td style="vertical-align:top">
                                    Encumbrance Date
                                </td>
                                <td style="vertical-align:top">
                                    Encumbrance Method<br />
                                    Contract/Purchase Order #
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    Project Completed 
                                </th>
                                <td>
                                
                                </td>
                                <td style="vertical-align:top">
                                    Goods/Services<br />
                                    Received Date
                                </td>
                                <td style="vertical-align:top">
                                
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    SFMO Inspection
                                </th>
                                <td>
                                
                                </td>
                                <td style="vertical-align:top">
                                    Requested Date
                                </td>
                                <td style="vertical-align:top">
                                    Requested of whom:
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    Fire Chief
                                </th>
                                <td>
                                
                                </td>
                                <td style="vertical-align:top">
                                    Signature Date
                                </td>
                                <td style="vertical-align:top">
                                    
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    SFMO Inspection Completed
                                </th>
                                <td>
                                
                                </td>
                                <td style="vertical-align:top">
                                    Inspection Date
                                </td>
                                <td style="vertical-align:top">
                                    By Whom:
                                </td>
                            </tr>
                            <tr style="min-height:75px">
                                <th scope="row" style="text-align:left;font-weight:normal">
                                    SFMO Check of NERIS Compliance
                                </th>
                                <td>
                                
                                </td>
                                <td style="vertical-align:top">
                                
                                </td>
                                <td style="vertical-align:top">
                                
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="dvCountyTable" runat="server" visible="false" class="row" style="padding-top:1em">
                    <table style="width:100%">
                        <thead>
                            <tr style="font-weight:bold">
                                <th scope="col">Department</th>
                                <th scope="col">Project</th>
                                <th scope="col">Amount</th>
                                <th scope="col">Stipend Amount</th>
                            </tr>
                        </thead>
                        <tbody id="CountyTableBody" runat="server">

                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
