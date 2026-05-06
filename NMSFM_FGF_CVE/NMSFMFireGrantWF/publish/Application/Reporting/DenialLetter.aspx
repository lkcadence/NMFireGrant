<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DenialLetter.aspx.cs" Inherits="NMSFMFireGrantWF.Application.Reporting.DenialLetter" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="repHead" runat="server">
    <title></title>
    <%--<link href="../../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <style type="text/css">
        body {
            max-width: 800px;
            margin:auto;
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
        /*Boostrap css*/
        .row{width:100%;margin-right:0px;margin-left:0px; clear:both}
        .col-md-12,.col-md-3,.col-md-6,.col-md-8,.col-md-4{position:relative;min-height:1px;padding-right:0px;padding-left:0px}
        .col-md-12,.col-md-3,.col-md-6,.col-md-8,.col-md-4{float:left}
        .col-md-12{width:100%}.col-md-6{width:50%}.col-md-3{width:25%}.col-md-4{width:33%}.col-md-8{width:66%}
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
            <div class="row" style="margin-bottom: 2em">
                <span id="spDepartment" runat="server"></span><br />
                <span id="spAddressDesc" runat="server"></span><br />
                <span id="spCityStateZip" runat="server"></span>
            </div>
            <div class="row" style="margin-bottom: 1em; font-weight:bold">
                Reference: FY <span id="spFY" runat="server"></span> New Mexico Fire Protection Grant Council Notification
            </div>
            <div class="row" style="margin-bottom:1em">
                Dear Treasurer:
            </div>
            <div class="row"  style="margin-bottom: 1em">
                Over <span id="spGrantApps" runat="server" style="color:red"></span> grant applications were submitted, and over <span id="spGrantAmounts" runat="server"></span> million in needs were considered, while <span id="spGrantsAwarded" runat="server"></span> million were available for distribution. This process was extremely difficult this year. 
            </div>
            <div class="row" style="margin-bottom: 1em">
                The Grant Council recognizes that there is a great need for fire protection apparatus, equipment, supplies, and training throughout the fire service in New Mexico. 
                Unfortunately, your department was not awarded grant assistance for this grant cycle for the reason stated below:
            </div>
            <div id="dvDenialReason" runat="server" class="row" style="margin-bottom: 1.5em; font-weight: bold; text-align:center">

            </div>
            <div class="row" style="margin-bottom: 1em">
                Thank you for the time and effort you invested in this process. Your department is encouraged to continue working, toward improving your 
                ISO fire protection classification, and to apply for grant assistance in the future.
            </div>
            <div class="row" style="margin-bottom: 1em">
                We continue to emphasize the importance of meeting the NFIRS reporting and pump testing requirements. 
            </div>
            <div class="row" style="margin-bottom: 1em">
                If further information is required, please contact  Randy Varela, State Fire Marshal at (505)709-8150. 
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
        </div>
    </form>
</body>
</html>
