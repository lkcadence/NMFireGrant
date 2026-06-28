using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NMSFM.Data;
using NMSFM.Services.Account;
using NMSFM.Services.Address;
using NMSFM.Services.Agency;
using NMSFM.Services.CPSystem;
using NMSFM.Services.FireGrant;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using NMSFM.Services.UDF;

namespace NMSFMFireGrantWF.Admin
{
  public partial class ManageAgencyInformation : Page
  {
    private static readonly Guid UdfCheckboxTypeId =
      new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488");

    private static readonly Guid UdfListTypeId =
      new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E");

    private static readonly string[] AllowedImageExtensions =
      { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };

    private ILogging logger;
    private IAddressService addressService;
    private IAccountService accountService;
    private ISystemService systemService;
    private IFGService fgService;
    private IAgencyService agencyService;
    private IUDFService udfService;
    private Emailer emailer;

    private List<UserDefinedValue> agencyUdfDefinitions = new List<UserDefinedValue>();

    protected void Page_Init(object sender, EventArgs e)
    {
      var userWebModel = new UserWebModel();
      logger = new Logging();
      emailer = new Emailer();
      accountService = new AccountService(userWebModel, logger);
      if (Session != null && Session["userConnection"] != null)
      {
        var userContext = new CodepalWebModel(Convert.ToString(Session["userConnection"]));
        systemService = new SystemService(userContext, logger);
        addressService = new AddressService(userContext, logger);
        fgService = new FGService(userContext, logger);
        agencyService = new AgencyService(userContext, logger);
        udfService = new UDFService(userContext, logger);
      }

      try
      {
        if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
        {
          Response.Redirect("~/Account/Login");
        }

        if (Session["Role"] == null || Convert.ToString(Session["Role"]) == "External")
        {
          Response.Redirect("~/Unauthorized");
        }

        if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
        {
          Response.Redirect("~/Unauthorized");
        }
      }
      catch (Exception ex)
      {
        _ = ex;
      }
    }

    protected async void Page_Load(object sender, EventArgs e)
    {
      bool reopenModal = false;

      try
      {
        if (Session["AgencyId"] == null)
        {
          ShowPageError("Agency session is missing. Please log in again.");
          return;
        }

        Guid agencyId = new Guid(Session["AgencyId"].ToString());
        hfAgencyId.Value = agencyId.ToString();

        if (!Page.IsPostBack)
        {
          HtmlGenericControl helpdiv =
            (HtmlGenericControl)Master.FindControl("dvPageHelp");
          FG_App_Help help = await fgService.GetFGHelpByPage(
            "Manage Agency Information (Admin)");
          if (help != null && helpdiv != null)
          {
            helpdiv.InnerHtml = help.HelpText;
          }
        }

        await BindLookupsAsync();

        agencyUdfDefinitions = (await udfService.GetUserDefinedValuesByAgencyIdAsync(
          agencyId)).ToList();
        RenderAdvancedUdfControls(agencyUdfDefinitions);

        if (!Page.IsPostBack)
        {
          NMSFM.Data.Agency agency = await agencyService.GetAgencyAsync(agencyId);
          if (agency == null)
          {
            ShowPageError("Agency record was not found.");
            return;
          }

          BindAgencyToForm(agency);
          await BindSupportEmailsAsync(agencyId);
          reopenModal = true;
        }
        else if (ViewState["ReopenAgencyModal"] != null
          && Convert.ToBoolean(ViewState["ReopenAgencyModal"]))
        {
          reopenModal = true;
          ViewState["ReopenAgencyModal"] = false;
        }

        if (reopenModal)
        {
          RegisterReopenModalScript();
        }
      }
      catch (Exception ex)
      {
        _ = ex;
        ShowPageError(ex.Message);
      }
    }

    protected async void btnSaveAgency_ServerClick(object sender, EventArgs e)
    {
      try
      {
        ClearModalError();
        Guid agencyId = new Guid(hfAgencyId.Value);
        NMSFM.Data.Agency agency = await agencyService.GetAgencyAsync(agencyId);
        if (agency == null)
        {
          ShowModalError("Agency record was not found.");
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        agency.AgencyName = txtAgencyName.Text.Trim();
        agency.AgencySubName = txtAgencySubName.Text.Trim();
        agency.Address = txtAddress.Text.Trim();
        agency.City = txtCity.Text.Trim();
        agency.Zip = txtZip.Text.Trim();
        agency.Phone = txtPhone.Text.Trim();
        agency.Fax = txtFax.Text.Trim();
        agency.Email = txtEmail.Text.Trim();
        agency.ExternalId = chkInactive.Checked ? "1" : "0";

        if (!string.IsNullOrEmpty(ddlState.SelectedValue))
        {
          agency.StateId = new Guid(ddlState.SelectedValue);
        }
        else
        {
          agency.StateId = null;
        }

        if (!string.IsNullOrEmpty(ddlCountry.SelectedValue))
        {
          agency.CountryId = new Guid(ddlCountry.SelectedValue);
        }
        else
        {
          agency.CountryId = null;
        }

        byte[] reportImage = null;
        bool clearReportImage = hfClearReportImage.Value == "true";
        if (fuReportImage.HasFile)
        {
          string extension = Path.GetExtension(fuReportImage.FileName);
          if (string.IsNullOrEmpty(extension)
            || !AllowedImageExtensions.Contains(extension.ToLowerInvariant()))
          {
            ShowModalError(
              "Report image must be a .bmp, .jpg, .jpeg, .gif, or .png file.");
            ViewState["ReopenAgencyModal"] = true;
            RegisterReopenModalScript();
            return;
          }

          reportImage = fuReportImage.FileBytes;
          clearReportImage = false;
        }

        string supportEmailError = ValidateSupportEmailList(
          txtTechnicalSupportEmail.Text,
          "Technical Support Email");
        if (!string.IsNullOrEmpty(supportEmailError))
        {
          ShowModalError(supportEmailError);
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        supportEmailError = ValidateSupportEmailList(
          txtFireServicesSupportEmail.Text,
          "Fire Services Support Email");
        if (!string.IsNullOrEmpty(supportEmailError))
        {
          ShowModalError(supportEmailError);
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        List<UserDefValue> udfValues = CollectUdfValuesFromForm(agencyId);
        string udfValidationError = ValidateRequiredUdfs(udfValues);
        if (!string.IsNullOrEmpty(udfValidationError))
        {
          ShowModalError(udfValidationError);
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        bool saved = await agencyService.UpdateAgencyAsync(
          agency,
          reportImage,
          clearReportImage);
        if (!saved)
        {
          ShowModalError("Unable to save agency information.");
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        if (udfValues.Count > 0)
        {
          await udfService.SaveUserDefinedValuesAsync(udfValues);
        }

        if (systemService == null
          || !await systemService.SaveSupportEmailRecipientsAsync(
            agencyId,
            txtTechnicalSupportEmail.Text.Trim(),
            txtFireServicesSupportEmail.Text.Trim()))
        {
          ShowModalError("Unable to save support email settings.");
          ViewState["ReopenAgencyModal"] = true;
          RegisterReopenModalScript();
          return;
        }

        hfClearReportImage.Value = "false";
        ShowPageSuccess("Agency information has been saved.");
        BindAgencyToForm(await agencyService.GetAgencyAsync(agencyId));
        await BindSupportEmailsAsync(agencyId);
        ViewState["ReopenAgencyModal"] = true;
        RegisterReopenModalScript();
      }
      catch (Exception ex)
      {
        _ = ex;
        ShowModalError(ex.Message);
        ViewState["ReopenAgencyModal"] = true;
        RegisterReopenModalScript();
      }
    }

    protected void btnClearReportImage_Click(object sender, EventArgs e)
    {
      imgReportPreview.Visible = false;
      imgReportPreview.ImageUrl = string.Empty;
      ViewState["ReopenAgencyModal"] = true;
      RegisterReopenModalScript();
    }

    private async Task BindLookupsAsync()
    {
      if (addressService == null)
      {
        return;
      }

      if (ddlState.Items.Count == 0)
      {
        ddlState.Items.Add(new ListItem("— Select state —", string.Empty));
        foreach (State state in await addressService.GetStateListAsync())
        {
          ddlState.Items.Add(new ListItem(state.State1, state.StateId.ToString()));
        }
      }

      if (ddlCountry.Items.Count == 0)
      {
        ddlCountry.Items.Add(new ListItem("— Select country —", string.Empty));
        foreach (Country country in await addressService.GetCountryListAsync())
        {
          ddlCountry.Items.Add(
            new ListItem(country.Country1, country.CountryId.ToString()));
        }
      }
    }

    private void BindAgencyToForm(NMSFM.Data.Agency agency)
    {
      txtAgencyName.Text = agency.AgencyName ?? string.Empty;
      txtAgencySubName.Text = agency.AgencySubName ?? string.Empty;
      txtAddress.Text = agency.Address ?? string.Empty;
      txtCity.Text = agency.City ?? string.Empty;
      txtZip.Text = agency.Zip ?? string.Empty;
      txtPhone.Text = agency.Phone ?? string.Empty;
      txtFax.Text = agency.Fax ?? string.Empty;
      txtEmail.Text = agency.Email ?? string.Empty;
      chkInactive.Checked = !string.IsNullOrEmpty(agency.ExternalId)
        && agency.ExternalId != "0";

      if (agency.StateId.HasValue)
      {
        ListItem stateItem = ddlState.Items.FindByValue(agency.StateId.Value.ToString());
        if (stateItem != null)
        {
          ddlState.ClearSelection();
          stateItem.Selected = true;
        }
      }

      if (agency.CountryId.HasValue)
      {
        ListItem countryItem = ddlCountry.Items.FindByValue(
          agency.CountryId.Value.ToString());
        if (countryItem != null)
        {
          ddlCountry.ClearSelection();
          countryItem.Selected = true;
        }
      }

      if (agency.ReportImage != null && agency.ReportImage.Length > 0)
      {
        imgReportPreview.ImageUrl = "data:image/png;base64,"
          + Convert.ToBase64String(agency.ReportImage);
        imgReportPreview.Visible = true;
      }
      else
      {
        imgReportPreview.Visible = false;
        imgReportPreview.ImageUrl = string.Empty;
      }

      lblDateInserted.Text = "Created: " + agency.DateInserted.ToString("g");
      lblDateUpdated.Text = "Last Updated: " + agency.DateUpdated.ToString("g");
    }

    private async Task BindSupportEmailsAsync(Guid agencyId)
    {
      if (systemService == null)
      {
        return;
      }

      txtTechnicalSupportEmail.Text = await systemService.GetCodepalSetting(
        FireGrantSettingKeys.TechnicalSupportEmail,
        agencyId) ?? string.Empty;
      txtFireServicesSupportEmail.Text = await systemService.GetCodepalSetting(
        FireGrantSettingKeys.FireServicesSupportEmail,
        agencyId) ?? string.Empty;
    }

    private string ValidateSupportEmailList(string raw, string fieldLabel)
    {
      if (string.IsNullOrWhiteSpace(raw))
      {
        return string.Empty;
      }

      foreach (string part in raw.Split(';'))
      {
        string email = part.Trim();
        if (email.Length == 0)
        {
          continue;
        }

        if (!emailer.EmailIsValid(email))
        {
          return fieldLabel + ": invalid email '" + email + "'.";
        }
      }

      return string.Empty;
    }

    private void RenderAdvancedUdfControls(IEnumerable<UserDefinedValue> udfValues)
    {
      phAdvancedUdf.Controls.Clear();
      List<UserDefinedValue> fields = udfValues.ToList();
      lblNoUdfFields.Visible = fields.Count == 0;

      if (fields.Count == 0)
      {
        return;
      }

      string currentCategory = null;
      foreach (UserDefinedValue field in fields.OrderBy(f => f.SequenceNumber)
        .ThenBy(f => f.Category)
        .ThenBy(f => f.FieldSequenceNumber))
      {
        if (currentCategory != field.Category)
        {
          currentCategory = field.Category;
          phAdvancedUdf.Controls.Add(new LiteralControl(
            "<div class='row formRow'><div class='col-sm-12'><h4>"
            + HttpUtility.HtmlEncode(currentCategory)
            + "</h4></div></div>"));
        }

        phAdvancedUdf.Controls.Add(new LiteralControl(
          "<div class='row formRow'><div class='col-sm-4'>"));
        Label fieldLabel = new Label
        {
          Text = field.FieldDescription,
          AssociatedControlID = GetUdfControlId(field.FieldId)
        };
        phAdvancedUdf.Controls.Add(fieldLabel);
        phAdvancedUdf.Controls.Add(new LiteralControl("</div><div class='col-sm-8'>"));

        if (field.FieldType == UdfCheckboxTypeId)
        {
          CheckBoxList checkboxList = new CheckBoxList
          {
            ID = GetUdfControlId(field.FieldId),
            RepeatLayout = RepeatLayout.Flow,
            RepeatDirection = RepeatDirection.Vertical
          };
          if (field.Resolutions != null)
          {
            for (int i = 0; i < field.Resolutions.Count; i++)
            {
              ListItem item = new ListItem(
                field.Resolutions[i].Resolution1,
                field.Resolutions[i].ResolutionId.ToString());
              if (field.boolValue != null
                && i < field.boolValue.Count
                && field.boolValue[i])
              {
                item.Selected = true;
              }

              checkboxList.Items.Add(item);
            }
          }

          phAdvancedUdf.Controls.Add(checkboxList);
        }
        else if (field.FieldType == UdfListTypeId)
        {
          DropDownList dropdown = new DropDownList
          {
            ID = GetUdfControlId(field.FieldId),
            CssClass = "form-control"
          };
          dropdown.Items.Add(new ListItem(string.Empty, string.Empty));
          if (field.Resolutions != null)
          {
            foreach (Resolution resolution in field.Resolutions)
            {
              ListItem item = new ListItem(
                resolution.Resolution1,
                resolution.Resolution1);
              if (resolution.Resolution1 == field.FieldValue)
              {
                item.Selected = true;
              }

              dropdown.Items.Add(item);
            }
          }

          phAdvancedUdf.Controls.Add(dropdown);
        }
        else
        {
          TextBox textbox = new TextBox
          {
            ID = GetUdfControlId(field.FieldId),
            CssClass = "form-control",
            Text = field.FieldValue ?? string.Empty
          };
          phAdvancedUdf.Controls.Add(textbox);
        }

        phAdvancedUdf.Controls.Add(new LiteralControl("</div></div>"));
      }
    }

    private List<UserDefValue> CollectUdfValuesFromForm(Guid agencyId)
    {
      List<UserDefValue> values = new List<UserDefValue>();
      foreach (UserDefinedValue field in agencyUdfDefinitions)
      {
        string controlId = GetUdfControlId(field.FieldId);
        Control control = phAdvancedUdf.FindControl(controlId);
        if (control == null)
        {
          continue;
        }

        string fieldValue = string.Empty;
        if (control is CheckBoxList)
        {
          CheckBoxList checkboxList = (CheckBoxList)control;
          if (field.Resolutions != null && field.Resolutions.Count > 0)
          {
            char[] bits = new char[field.Resolutions.Count];
            for (int i = 0; i < bits.Length; i++)
            {
              bits[i] = '0';
            }

            foreach (ListItem item in checkboxList.Items)
            {
              if (!item.Selected)
              {
                continue;
              }

              int index = field.Resolutions.FindIndex(
                r => r.ResolutionId.ToString() == item.Value);
              if (index >= 0)
              {
                bits[index] = '1';
              }
            }

            fieldValue = new string(bits);
          }
        }
        else if (control is DropDownList)
        {
          fieldValue = ((DropDownList)control).SelectedValue ?? string.Empty;
        }
        else if (control is TextBox)
        {
          fieldValue = ((TextBox)control).Text ?? string.Empty;
        }

        if (string.IsNullOrEmpty(fieldValue) && string.IsNullOrEmpty(field.FieldValue))
        {
          continue;
        }

        values.Add(new UserDefValue
        {
          UserDefValueId = field.ValueId,
          UserDefFieldId = field.FieldId,
          RecordId = agencyId,
          UserDefValue1 = fieldValue,
          rowguid = Guid.NewGuid()
        });
      }

      return values;
    }

    private string ValidateRequiredUdfs(List<UserDefValue> values)
    {
      foreach (UserDefinedValue field in agencyUdfDefinitions.Where(f => f.Required))
      {
        UserDefValue posted = values.FirstOrDefault(
          v => v.UserDefFieldId == field.FieldId);
        if (posted == null || string.IsNullOrWhiteSpace(posted.UserDefValue1))
        {
          return "Required field '" + field.FieldDescription + "' must be completed.";
        }
      }

      return string.Empty;
    }

    private static string GetUdfControlId(Guid fieldId)
    {
      return "udf_" + fieldId.ToString("N");
    }

    private void ShowPageError(string message)
    {
      dvError.InnerHtml = "<div class='alert alert-danger'>"
        + HttpUtility.HtmlEncode(message)
        + "</div>";
    }

    private void ShowPageSuccess(string message)
    {
      dvError.InnerHtml = "<div class='alert alert-success'>"
        + HttpUtility.HtmlEncode(message)
        + "</div>";
    }

    private void ShowModalError(string message)
    {
      dvAgencyModalError.InnerHtml = "<div class='alert alert-danger'>"
        + HttpUtility.HtmlEncode(message)
        + "</div>";
    }

    private void ClearModalError()
    {
      dvAgencyModalError.InnerHtml = string.Empty;
    }

    private void RegisterReopenModalScript()
    {
      ScriptManager.RegisterStartupScript(
        this,
        GetType(),
        "ReopenAgencyModal",
        "agencyShowModal();",
        true);
    }
  }
}
