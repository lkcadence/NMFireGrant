<%@ Page Title="Fire Grant: Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NMSFMFireGrantWF._Default" async="true"%>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" id="dvDefaultHeader" runat="server">
        <h1>FY <span id="spFY" runat="server"></span> - Online Fire Protection Grant Application ( OFPGA )</h1>
        <p class="lead">*For questions and technical support regarding the application, departments should initially contact their SFMO Fire Support Inspector.</p>
    </div>
    <div class="jumbotron" id="dvDefaultHeaderApplication" runat="server">

    </div>
    <div class="row">
        <div class="col-md-12" id="dvDefaultContent" runat="server">
            <h2>Welcome to the Online New Mexico Fire Protection Grant Application</h2>
            <p>
                This web-app is in response to feedback from many New Mexico departments for a more user-friendly process. Your continued patience and understanding is appreciated as we 
                work to improve the process and serve you better.
            </p>
            <p>
                To begin, enter your department’s five digit NFIRS FDID number in both the NFIRS FDID number field and the Password field. Upon logon, in the General Information page, 
                you will be prompted to provide an email address and may change the password. Please note: Only one application per department will be accepted; therefore only one email 
                address and password per department will be recognized.
            </p>
            <p style="text-decoration:underline">
                Please read the eligibility requirements on the Welcome Page carefully before completing the application.
            </p>
            <p>
                To assist in tracking completion of the application, the status is shown in then gray shaded area to the right of the application. A green checkmark <img src="Content/images/tick.png" /> tick indicates the 
                section has been opened and started. Due to the varied responses, however, It does not necessarily indicate that the section is complete. A red cross <img src="Content/images/cross.png" /> indicates that there 
                is required information that has not been completed. A circle <img src="Content/images/round.png" /> with an empty in the center indicates the section has not yet been started.
            </p>
            <p>
                Should you have technical questions or experience problems navigating through the application, click on the Technical Support link located in the gray shaded banner at the 
                top of each page, describe the problem, and click SEND. Your question will be answered within 2 business days.
            </p>
            <p>
                Should you have questions specific to the content requirements of the application, click on the SFMO Fire Services Support Team link located in the gray shaded banner at the top of each page, describe the question, 
                and click SEND. Your question will be answered within 2 business days.
            </p>
            <p>
                You are now ready to begin entering the FY<%: DateTime.Now.Year.ToString() %> NM Fire Protection Grant application.
            </p>
            <p>
                <a class="btn btn-default" href="https://www.nmdhsem.org/state-firemarshal/fire-grant-council/#grant" target="_blank">Login or Register to begin...</a>
            </p>
            <p>
                <a class="btn btn-default" href="https://www.nmdhsem.org/state-firemarshal/fire-grant-council/#grant" target="_blank">New Mexico State Fire Marshal Fire Grant Council &raquo;</a>
            </p>
        </div>
        <div class="col-md-12" id="dvApplicationDefaultContent" runat="server" visible="false">
        </div>
    </div>


</asp:Content>
