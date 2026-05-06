<%@ Page Title="Fire Grant: View Legacy Applications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewLegacyApps.aspx.cs" Inherits="NMSFMFireGrantWF.User.ViewLegacyApps" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>View Legacy Applications</h2>
        <div class="row" id="dvError" runat="server">
                    
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Department Info</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <asp:Label ID="lblDepartment" runat="server" Text="Select Department: " AssociatedControlID="rcbDepartments"></asp:Label>
            </div>
            <dov class="col-md-4">
                <telerik:RadComboBox ID="rcbDepartments" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rcbDepartments_SelectedIndexChanged" Width="100%" AccessibilityMode="True" EnableAriaSupport="True"></telerik:RadComboBox>
            </dov>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3>Legacy Applications</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:DataList ID="dlApplications" runat="server" RepeatColumns="3" CellSpacing="2" RepeatLayout="Table">
                    <ItemTemplate>  
                        <div class="appTableDiv">
                            <table class="apptable">  
                                <tr>  
                                    <th colspan="2">  
                                        <b>  
                                            <%# Eval("FiscalYear") %></b>  
                                    </th>  
                                </tr>  
                            
                                <tr>  
                                    <td>  
                                        File Name:  
                                    </td>  
                                    <td>  
                                        <%# Eval("FileName")%>  
                                    </td>  
                                </tr> 
                                <tr>
                                    <td colspan="2">
                                        <a href='<%# Eval("FilePath")%>' target="_blank">View Application</a>
                                    </td>
                                </tr>
                            </table>  
                        </div>      
                    </ItemTemplate>  
                </asp:DataList>
            </div>
        </div>
    </div>
</asp:Content>
