<%@ Page Title="Fire Grant Application: General Information" Language="C#" MasterPageFile="~/Application/ApplicationMstr.Master" AutoEventWireup="true" CodeBehind="GeneralInformation.aspx.cs" Inherits="NMSFMFireGrantWF.Application.GeneralInformation" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ApplicationContent" runat="server">
    <div class="wrapper">
        <div class="row" id="dvError" runat="server"></div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblGrantType" runat="server" Text="Grant Source *" AssociatedControlID="fldGrantType"></asp:Label>
            </div>
            <div class="col-md-8">
                <fieldset id="fldGrantType" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbIndividual" runat="server" Text="Individual Department" GroupName="GrantSource" ClientIDMode="Static" />&nbsp;
                    <asp:RadioButton ID="rbCountyWide" runat="server" Text="County Wide Project" GroupName="GrantSource" ClientIDMode="Static" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFDID" runat="server" AssociatedControlID="txtFDID" Text="Fire Department ID Number (using NFIRS Identifier) *"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtFDID" runat="server" class="form-control" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="txtDepartment" Text="Fire Department Name *"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtDepartment" runat="server" class="form-control" ReadOnly="true" ToolTip="Fire Department Name loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFireCheif" runat="server" AssociatedControlID="txtFireCheif" Text="Fire Chief Name *"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtFireCheif" runat="server" class="form-control" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="Phone Number *"></asp:Label>
            </div>
            <div class="col-md-8">
                <%--<asp:TextBox ID="txtPhone" runat="server" class="form-control" TextMode="Phone"></asp:TextBox>--%>
                <telerik:RadMaskedTextBox RenderMode="Lightweight" CssClass="form-control" ID="txtPhone" runat="server" Mask="(###)###-####" aria-required="true">
                </telerik:RadMaskedTextBox>
                <asp:RequiredFieldValidator Display="Dynamic" ID="mtvPhone" ForeColor="Red"
                    runat="server" ErrorMessage="Please enter a phone number" ControlToValidate="txtPhone"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator Display="Dynamic" ID="mtvPhone2" ForeColor="Red"
                    runat="server" ErrorMessage="Format is (###)###-####" ControlToValidate="txtPhone"
                    ValidationExpression="\(\d{3}\)\d{3}-\d{4}"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email Address *"></asp:Label>
            </div>
            <div class="col-md-8">
                <%--<asp:TextBox ID="txtEmail" runat="server" class="form-control" TextMode="Email"></asp:TextBox>--%>
                <telerik:RadTextBox RenderMode="Lightweight" CssClass="form-control" ID="txtEmail" runat="server" aria-required="true"></telerik:RadTextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" Display="Dynamic" ForeColor="Red"
                    ErrorMessage="Please enter valid e-mail address" ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                    ControlToValidate="txtEmail">
                </asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblISO" runat="server" AssociatedControlID="txtISO" Text="Insurance Services Office (ISO) Rating"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtISO" runat="server" class="form-control" ReadOnly="true" ToolTip="ISO info loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblCounty" runat="server" AssociatedControlID="txtCounty" Text="County"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtCounty" runat="server" class="form-control" ReadOnly="true" ToolTip="County Name loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFsType" runat="server" AssociatedControlID="fsType" Text="Department Type *"></asp:Label>
            </div>
            <div class="col-md-8">
                <fieldset id="fsType" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbCityMuni" runat="server" Text="City/Municipality" GroupName="Type" />&nbsp;
                    <asp:RadioButton ID="rbCounty" runat="server" Text="County" GroupName="Type" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblDeptType" runat="server" AssociatedControlID="fsDeptType" Text="Is your department... *"></asp:Label>
            </div>
            <div class="col-md-4">
                <fieldset id="fsDeptType" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbCareer" runat="server" Text="Career" GroupName="DeptType" /><br />
                    <asp:RadioButton ID="rbVolunteer" runat="server" Text="Volunteer" GroupName="DeptType" /><br />
                    <asp:RadioButton ID="rbCombined" runat="server" Text="Combined Career & Volunteer" GroupName="DeptType" /><hr />
                    <asp:Checkbox ID="chkAdmin" runat="server" Text="Administration" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow" id="dvCountyDepts">
            <div class="col-md-4">
                <asp:Label ID="lblCountyDepts" runat="server" Text="Are all of the County departments NFIRS and Pump Test complient? *" AssociatedControlID="fsCountyDepts"></asp:Label>
            </div>
            <div class="col-md-8">
                <fieldset id="fsCountyDepts" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbCountyDeptsYes" runat="server" Text="Yes" GroupName="CountyDepts" />&nbsp;
                    <asp:RadioButton ID="rbCountyDeptsNo" runat="server" Text="No" GroupName="CountyDepts" />
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>How many stations in your organization</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblMainStations" runat="server" AssociatedControlID="txtMainStations" Text="Main Stations"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtMainStations" runat="server" class="form-control" ReadOnly="true" ToolTip="Main Station info loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblSubStations" runat="server" AssociatedControlID="txtSubStations" Text="Substations"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtSubStations" runat="server" class="form-control" ReadOnly="true" ToolTip="Sub Station info loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblAdmin" runat="server" AssociatedControlID="txtAdmin" Text="Admin Buildings"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtAdmin" runat="server" class="form-control" ReadOnly="true" ToolTip="Admin Building info loaded from department information"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblCommunity" runat="server" AssociatedControlID="fsCommunity" Text="Type of community your organization services (based on population density)"></asp:Label>
            </div>
            <div class="col-md-8">
                <fieldset id="fsCommunity" runat="server">
                    <asp:RadioButton ID="chkUrban" runat="server" Text="Urban" GroupName="Community"/><br />
                    <asp:RadioButton ID="chkRural" runat="server" Text="Rural" GroupName="Community" /><br />
                    <asp:RadioButton ID="chkSubUrban" runat="server" Text="Sub-Urban" GroupName="Community" />
                </fieldset>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFirefighters" runat="server" AssociatedControlID="txtFirefighters" Text="Number of firefighters"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFirefighters" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFF1" runat="server" AssociatedControlID="txtlblFF1" Text="Number of FF-I Certified firefighters"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtlblFF1" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFF2" runat="server" AssociatedControlID="txtFF2" Text="Number of FF-II Certified firefighters"></asp:Label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFF2" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Mailing Address</h3>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="Address"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtAddress" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="City"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtCity" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblState" runat="server" AssociatedControlID="txtState" Text="State"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtState" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblZip" runat="server" AssociatedControlID="txtZip" Text="Zip Code"></asp:Label>
            </div>
            <div class="col-md-2">
                <%--<asp:TextBox ID="txtZip" runat="server" class="form-control" TextMode="Number"></asp:TextBox>--%>
                <telerik:RadMaskedTextBox ID="txtZip" RenderMode="Lightweight" CssClass="form-control" runat="server" Mask="#####" aria-required="true"></telerik:RadMaskedTextBox>
                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" ForeColor="Red"
                    runat="server" ErrorMessage="Please enter a zip code" ControlToValidate="txtZip"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1" ForeColor="Red"
                    runat="server" ErrorMessage="Format is #####" ControlToValidate="txtZip"
                    ValidationExpression="\d{5}"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblApplicationName" runat="server" AssociatedControlID="txtApplicationName" Text="Name of Person Completing this Application *"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:TextBox ID="txtApplicationName" runat="server" class="form-control" aria-required="true"></asp:TextBox>
            </div>
        </div>
        <div class="row formRow">
            <div class="col-md-4">
                <asp:Label ID="lblFDMember" runat="server" Text="Are you a fire department member *" AssociatedControlID="fsFDMember"></asp:Label>
            </div>
            <div class="col-md-8">
                <fieldset id="fsFDMember" runat="server" aria-required="true">
                    <asp:RadioButton ID="rbFDMemberYes" runat="server" Text="Yes" GroupName="FDMember" />&nbsp;
                    <asp:RadioButton ID="rbFDMemberNo" runat="server" Text="No" GroupName="FDMember" />
                </fieldset>
            </div>
        </div>
        <div class="row">
            <hr />
        </div>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnBack_Click" CausesValidation="false"/>&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" CausesValidation="false" />&nbsp;
                <asp:Button ID="btnNext" runat="server" CssClass="btn btn-primary" Text="Next" OnClick="btnNext_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfApplicationId" runat="server" />
    <script type="text/javascript">
        ShowCountyDepts();

        function ShowCountyDepts() {
            var show = $('#rbCountyWide').prop('checked');
            if (show === true) {
                $('#dvCountyDepts').show();
            }
            else {
                $('#dvCountyDepts').hide();
            }
        }

        $("#rbIndividual,#rbCountyWide").change(function () {
            ShowCountyDepts();
        });
    </script>
</asp:Content>
