<%@ Page Title="Fire Grant: Manage Agency Information" Language="C#"
  MasterPageFile="~/Site.Master" AutoEventWireup="true"
  CodeBehind="ManageAgencyInformation.aspx.cs"
  Inherits="NMSFMFireGrantWF.Admin.ManageAgencyInformation" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2>Manage Agency Information</h2>
    <div class="row" id="dvError" runat="server"></div>
    <asp:HiddenField ID="hfAgencyId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfClearReportImage" runat="server" Value="false"
      ClientIDMode="Static" />
    <button id="btnHideModal" type="button" class="btn btn-primary"
      style="display:none" data-dismiss="modal" data-target="#agencyInfoModal">
      Hide Modal
    </button>
  </div>

  <div class="modal fade" id="agencyInfoModal" tabindex="-1" role="dialog"
    data-backdrop="false" aria-labelledby="lblAgencyModalHeader" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document" style="margin:80px auto !important">
      <div class="modal-content">
        <div class="modal-header">
          <h4 class="modal-title" id="lblAgencyModalHeader">Agency Information</h4>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <div class="row" id="dvAgencyModalError" runat="server"></div>
          <div class="row formRow">
            <div class="col-md-12 text-right">
              <asp:CheckBox ID="chkInactive" runat="server" Text="Inactive"
                ClientIDMode="Static" />
            </div>
          </div>

          <ul class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active">
              <a href="#tabGeneral" aria-controls="tabGeneral" role="tab"
                data-toggle="tab">General</a>
            </li>
            <li role="presentation">
              <a href="#tabAdvanced" aria-controls="tabAdvanced" role="tab"
                data-toggle="tab">Advanced</a>
            </li>
          </ul>

          <div class="tab-content" style="padding-top:15px;">
            <div role="tabpanel" class="tab-pane active" id="tabGeneral">
              <div class="row formRow">
                <div class="col-md-7">
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblAgencyName" runat="server"
                        AssociatedControlID="txtAgencyName" Text="Name"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtAgencyName" runat="server"
                        CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblAgencySubName" runat="server"
                        AssociatedControlID="txtAgencySubName" Text="Sub Name"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtAgencySubName" runat="server"
                        CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblAddress" runat="server"
                        AssociatedControlID="txtAddress" Text="Address"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtAddress" runat="server"
                        CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblCity" runat="server"
                        AssociatedControlID="txtCity" Text="City"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtCity" runat="server"
                        CssClass="form-control" MaxLength="50" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblStateZip" runat="server" Text="State/Zip"></asp:Label>
                    </div>
                    <div class="col-sm-5">
                      <asp:DropDownList ID="ddlState" runat="server"
                        CssClass="form-control" ClientIDMode="Static" />
                    </div>
                    <div class="col-sm-4">
                      <asp:TextBox ID="txtZip" runat="server"
                        CssClass="form-control" MaxLength="20" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblCountry" runat="server"
                        AssociatedControlID="ddlCountry" Text="Country"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:DropDownList ID="ddlCountry" runat="server"
                        CssClass="form-control" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblPhone" runat="server"
                        AssociatedControlID="txtPhone" Text="Phone"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtPhone" runat="server"
                        CssClass="form-control" MaxLength="25" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblFax" runat="server"
                        AssociatedControlID="txtFax" Text="Fax"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtFax" runat="server"
                        CssClass="form-control" MaxLength="25" ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-3">
                      <asp:Label ID="lblEmail" runat="server"
                        AssociatedControlID="txtEmail" Text="E-mail"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                      <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control" MaxLength="100" TextMode="Email"
                        ClientIDMode="Static" />
                    </div>
                  </div>
                </div>
                <div class="col-md-5">
                  <div class="row formRow">
                    <div class="col-sm-12">
                      <asp:Label ID="lblReportImage" runat="server"
                        AssociatedControlID="imgReportPreview" Text="Report Image"></asp:Label>
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-12">
                      <asp:Image ID="imgReportPreview" runat="server"
                        CssClass="img-thumbnail" Width="150" Height="150"
                        Visible="false" AlternateText="Report image preview" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-12">
                      <asp:FileUpload ID="fuReportImage" runat="server"
                        ClientIDMode="Static" />
                    </div>
                  </div>
                  <div class="row formRow">
                    <div class="col-sm-12">
                      <asp:Button ID="btnClearReportImage" runat="server"
                        CssClass="btn btn-default" Text="Clear Report Image"
                        OnClientClick="document.getElementById('hfClearReportImage').value='true'; return true;"
                        OnClick="btnClearReportImage_Click" />
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div role="tabpanel" class="tab-pane" id="tabAdvanced">
              <asp:PlaceHolder ID="phAdvancedUdf" runat="server" />
              <asp:Label ID="lblNoUdfFields" runat="server" Visible="false"
                Text="There are no additional fields defined for this record." />
            </div>
          </div>

          <div class="row formRow" style="margin-top:10px;">
            <div class="col-sm-6">
              <asp:Label ID="lblDateInserted" runat="server" />
            </div>
            <div class="col-sm-6 text-right">
              <asp:Label ID="lblDateUpdated" runat="server" />
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button id="btnSaveAgency" class="btn btn-primary" runat="server"
            onserverclick="btnSaveAgency_ServerClick">Save</button>
          <button id="btnCloseAgencyModal" type="button" class="btn btn-primary"
            onclick="window.location.href='/Admin/Home';">Close</button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    function agencyShowModal() {
      $('#agencyInfoModal').modal('show');
      $('body').addClass('modal-open');
    }
  </script>
</asp:Content>
