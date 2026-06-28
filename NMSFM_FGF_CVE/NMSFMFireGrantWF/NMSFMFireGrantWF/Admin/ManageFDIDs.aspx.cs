using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NMSFM.Data;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using NMSFM.ViewModels;
using Telerik.Web.UI;

namespace NMSFMFireGrantWF.Admin
{
    public partial class ManageFDIDs : System.Web.UI.Page
    {
        private static readonly Guid FireDeptAddressTypeId =
            new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");

        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;


        protected void Page_Init(object sender, EventArgs e)
        {
            btnApplyFilters.Click += btnApplyFilters_Click;
            btnClearFilters.Click += btnClearFilters_Click;
            rgFDIDs.SortCommand += rgFDIDs_SortCommand;

            var userWebModel = new UserWebModel();
            logger = new Logging();
            accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null
                && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
            }
            else
            {
                this.addressService = null;
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
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            ConfigureAddressSyncVisibility();

            if (!Page.IsPostBack)
            {
                try
                {
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("FDIDs (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }

                    SetDefaultFdidSort();
                    await LoadFDIDsAsync();

                    if (IsFdidAddressSyncEnabled())
                    {
                        await BindCreateAddressDropdownsAsync();
                    }

                    if (Session["SaveMessage"] != null)
                    {
                        dvError.InnerHtml = Session["SaveMessage"].ToString();
                        Session["SaveMessage"] = "";
                    }
                }
                catch (Exception ex)
                {
                    _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
            }
        }

        private static bool IsFdidAddressSyncEnabled()
        {
            string setting = ConfigurationManager.AppSettings["EnableFdidAddressSync"];
            return string.IsNullOrEmpty(setting)
                || setting.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        private void ConfigureAddressSyncVisibility()
        {
            bool enabled = IsFdidAddressSyncEnabled();
            dvAddressSyncSection.Visible = enabled;
            dvAddressLinkPanel.Visible = enabled;
            dvAddressCreatePanel.Visible = enabled;
            dvDepartmentUdfSection.Visible = enabled;
        }

        private async System.Threading.Tasks.Task LoadFDIDsAsync()
        {
            List<FG_FDIDs> fdids = await fgService.GetFG_FDIDs();
            ViewState["dtFDIDsAll"] = fdids ?? new List<FG_FDIDs>();
            BindFDIDGrid();
        }

        private void SetDefaultFdidSort()
        {
            rgFDIDs.MasterTableView.SortExpressions.Clear();
            rgFDIDs.MasterTableView.SortExpressions.AddSortExpression(
                new GridSortExpression
                {
                    FieldName = "FDID",
                    SortOrder = GridSortOrder.Ascending
                });
        }

        private void BindFDIDGrid()
        {
            List<FG_FDIDs> display = GetFilteredSortedList();
            rgFDIDs.DataSource = display;
            rgFDIDs.DataBind();
        }

        private List<FG_FDIDs> GetFilteredSortedList()
        {
            List<FG_FDIDs> all = ViewState["dtFDIDsAll"] as List<FG_FDIDs>
                ?? new List<FG_FDIDs>();

            IEnumerable<FG_FDIDs> query = all;

            if (chkHideInactive.Checked)
            {
                query = query.Where(x => !x.Inactive);
            }

            string nerisSearch = txtSearchNerisId.Text.Trim().ToUpperInvariant();
            if (nerisSearch != "")
            {
                query = query.Where(x =>
                    (x.FDID ?? "").ToUpperInvariant().Contains(nerisSearch));
            }

            string deptSearch = txtSearchFireDepartment.Text.Trim();
            if (deptSearch != "")
            {
                query = query.Where(x =>
                    (x.FireDepartment ?? "").IndexOf(
                        deptSearch, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return ApplySort(query).ToList();
        }

        private IEnumerable<FG_FDIDs> ApplySort(IEnumerable<FG_FDIDs> query)
        {
            GridSortExpression sort = rgFDIDs.MasterTableView.SortExpressions
                .Cast<GridSortExpression>()
                .FirstOrDefault();

            if (sort == null || string.IsNullOrEmpty(sort.FieldName))
            {
                return query.OrderBy(a => a.FDID);
            }

            bool desc = sort.SortOrder == GridSortOrder.Descending;
            switch (sort.FieldName)
            {
                case "FireDepartment":
                    return desc
                        ? query.OrderByDescending(a => a.FireDepartment)
                        : query.OrderBy(a => a.FireDepartment);
                default:
                    return desc
                        ? query.OrderByDescending(a => a.FDID)
                        : query.OrderBy(a => a.FDID);
            }
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            rgFDIDs.CurrentPageIndex = 0;
            BindFDIDGrid();
        }

        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearchNerisId.Text = "";
            txtSearchFireDepartment.Text = "";
            chkHideInactive.Checked = true;
            rgFDIDs.CurrentPageIndex = 0;
            SetDefaultFdidSort();
            BindFDIDGrid();
        }

        protected void chkHideInactive_CheckedChanged(object sender, EventArgs e)
        {
            rgFDIDs.CurrentPageIndex = 0;
            BindFDIDGrid();
        }

        protected void rgFDIDs_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            BindFDIDGrid();
        }

        protected void rgFDIDs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgFDIDs.DataSource = GetFilteredSortedList();
        }

        protected void rgFDIDs_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            BindFDIDGrid();
        }

        protected void rgFDIDs_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!(e.Item is GridDataItem dataItem))
            {
                return;
            }

            FG_FDIDs row = e.Item.DataItem as FG_FDIDs;
            HyperLink editLink = dataItem.FindControl("editLink") as HyperLink;
            if (editLink == null && dataItem["Edit"].Controls.Count > 0)
            {
                editLink = dataItem["Edit"].Controls[0] as HyperLink;
            }
            if (row == null || editLink == null)
            {
                return;
            }

            string dept = row.FireDepartment ?? string.Empty;
            editLink.Text = "View/Edit " + row.FDID;
            editLink.NavigateUrl = "javascript:void(0);";
            editLink.Attributes["data-fdid"] = row.FDID ?? string.Empty;
            editLink.Attributes["data-dept"] = dept
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;");
            editLink.Attributes["data-inactive"] = row.Inactive.ToString().ToLowerInvariant();
            editLink.Attributes["onclick"] = "return fdidOpenForEdit(this);";
        }

        protected async void btnLoadAddressMatches_Click(object sender, EventArgs e)
        {
            if (!IsFdidAddressSyncEnabled() || addressService == null)
            {
                RegisterReopenModalScript("link");
                return;
            }

            ClearModalError();
            string departmentName = txtDepartmentName.Text.Trim();
            await BindAddressLinkDropdownAsync(departmentName);

            try
            {
                v_Addresses2 associated =
                    await addressService.GetAssociatedFireDepartmentAddressAsync(departmentName);
                if (associated != null)
                {
                    SelectDropDownValue(ddlAddressLink, associated.AddressId.ToString());
                    await PopulateAddressEditPanelAsync(associated);
                    SetCreateEditAddressMode();
                    RegisterReopenModalScript("create");
                    return;
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowModalError(ex.Message);
            }

            RegisterReopenModalScript(GetAddressAction());
        }

        protected async void btnLoadAddressForEdit_Click(object sender, EventArgs e)
        {
            if (!IsFdidAddressSyncEnabled() || addressService == null)
            {
                RegisterReopenModalScript(GetAddressAction());
                return;
            }

            ClearModalError();
            string selected = ddlAddressLink.SelectedValue;
            if (string.IsNullOrEmpty(selected) || selected == "__CREATE__")
            {
                hfAddressId.Value = string.Empty;
                RegisterReopenModalScript("create");
                return;
            }

            Guid addressId = new Guid(selected);
            v_Addresses2 address = await addressService.GetAddressByIdAsync(addressId);
            if (address == null)
            {
                ShowModalError("Selected address was not found.");
                RegisterReopenModalScript(GetAddressAction());
                return;
            }

            await PopulateAddressEditPanelAsync(address);
            SetCreateEditAddressMode();
            RegisterReopenModalScript("create");
        }

        protected async void btnSaveFDID_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFdidAddressSyncEnabled() && addressService != null)
                {
                    SyncAddressActionFromRadios();
                    if (GetAddressAction() == "link"
                        && ddlAddressLink.Items.Count <= 1)
                    {
                        await BindAddressLinkDropdownAsync(txtDepartmentName.Text.Trim());
                    }

                    ValidateAddressSyncInputs();
                }

                await SaveFdidAsync();

                string addressMessage = string.Empty;
                if (IsFdidAddressSyncEnabled() && addressService != null)
                {
                    addressMessage = await ProcessAddressSyncAsync();
                }

                Session["SaveMessage"] = BuildSuccessMessage(addressMessage);
                Response.Redirect("~/Admin/ManageFDIDs", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                _ = ex;
                ShowModalError(ex);
                RegisterReopenModalScript(GetAddressAction());
            }
        }

        private void ShowModalError(string message)
        {
            dvFDIDModalError.InnerHtml =
                "<div class='alert alert-danger'>" + HttpUtility.HtmlEncode(message) + "</div>";
        }

        private void ShowModalError(Exception ex)
        {
            ShowModalError(FormatExceptionForDisplay(ex));
        }

        private static string FormatExceptionForDisplay(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            SqlException sqlException = null;
            for (Exception current = ex; current != null; current = current.InnerException)
            {
                if (current is SqlException)
                {
                    sqlException = (SqlException)current;
                }
            }

            if (sqlException != null)
            {
                return sqlException.Message;
            }

            var parts = new List<string>();
            for (Exception current = ex; current != null; current = current.InnerException)
            {
                string msg = (current.Message ?? string.Empty).Trim();
                if (msg.Length == 0)
                {
                    continue;
                }

                if (!parts.Exists(p => string.Equals(p, msg, StringComparison.OrdinalIgnoreCase)))
                {
                    parts.Add(msg);
                }
            }

            Exception baseException = ex.GetBaseException();
            if (baseException != null && !ReferenceEquals(baseException, ex))
            {
                string baseMsg = (baseException.Message ?? string.Empty).Trim();
                if (baseMsg.Length > 0
                    && !parts.Exists(p => string.Equals(p, baseMsg, StringComparison.OrdinalIgnoreCase)))
                {
                    parts.Add(baseMsg);
                }
            }

            if (parts.Count == 0)
            {
                return ex.ToString();
            }

            return string.Join(" ", parts);
        }

        private void ClearModalError()
        {
            dvFDIDModalError.InnerHtml = string.Empty;
        }

        private async System.Threading.Tasks.Task SaveFdidAsync()
        {
            List<FG_FDIDs> fdidlist = ViewState["dtFDIDsAll"] as List<FG_FDIDs>;
            string nerisId = txtFDID.Text.Trim().ToUpperInvariant();
            string departmentName = txtDepartmentName.Text.Trim();
            if (nerisId == "")
            {
                throw new Exception("NERIS ID cannot be blank");
            }
            if (departmentName == "")
            {
                throw new Exception("Department Name cannot be blank");
            }

            FG_FDIDs fdidModel = new FG_FDIDs
            {
                FDID = nerisId,
                FireDepartment = departmentName,
                Inactive = chkFDIDInactive.Checked
            };

            if (hfFDID.Value == "")
            {
                if (fdidlist != null && fdidlist.Any(a => a.FDID == nerisId))
                {
                    throw new Exception("NERIS ID exists in the list");
                }

                if (!await fgService.SaveFDIDAsync(fdidModel))
                {
                    throw new Exception("Unable to save NERIS ID.");
                }
            }
            else
            {
                string originalFdid = hfFDID.Value.Trim().ToUpperInvariant();
                if (originalFdid != nerisId)
                {
                    if (fdidlist != null && fdidlist.Any(a => a.FDID == nerisId))
                    {
                        throw new Exception("NERIS ID exists in the list");
                    }

                    if (!await fgService.SaveFDIDAsync(fdidModel))
                    {
                        throw new Exception("Unable to save NERIS ID.");
                    }

                    if (!await fgService.DeleteFDIDAsync(originalFdid))
                    {
                        throw new Exception("Unable to replace the existing NERIS ID.");
                    }
                }
                else
                {
                    fdidModel.FDID = originalFdid;
                    if (!await fgService.UpdateFDIDAsync(fdidModel))
                    {
                        throw new Exception("Unable to save NERIS ID.");
                    }
                }
            }
        }

        private void ValidateAddressSyncInputs()
        {
            ValidateDepartmentUdfInputs();

            if (GetAddressAction() != "create")
            {
                return;
            }

            if (ddlCreateAddressType.SelectedValue == "")
            {
                throw new Exception("Address type is required when creating or editing an address.");
            }
            if (string.IsNullOrWhiteSpace(txtCreateCity.Text))
            {
                throw new Exception("City is required when creating or editing an address.");
            }
            if (ddlCreateState.SelectedValue == "")
            {
                throw new Exception("State is required when creating or editing an address.");
            }
            if (ddlCreateCounty.SelectedValue == "")
            {
                throw new Exception("County is required when creating or editing an address.");
            }
            if (string.IsNullOrWhiteSpace(txtCreateZip.Text))
            {
                throw new Exception("Zip is required when creating or editing an address.");
            }
        }

        private void ValidateDepartmentUdfInputs()
        {
            ParseNonNegativeUdfValue(txtDeptIso.Text, "ISO Rating");
            ParseNonNegativeUdfValue(txtMainStations.Text, "Main Stations");
            ParseNonNegativeUdfValue(txtSubStations.Text, "Substations");
            ParseNonNegativeUdfValue(txtAdminBldgs.Text, "Admin Buildings");
        }

        private static int ParseNonNegativeUdfValue(string text, string fieldName)
        {
            string value = (text ?? string.Empty).Trim();
            if (value == string.Empty)
            {
                return 0;
            }

            int parsed;
            if (!int.TryParse(value, out parsed) || parsed < 0)
            {
                throw new Exception(fieldName + " must be a non-negative whole number.");
            }

            return parsed;
        }

        private async System.Threading.Tasks.Task<string> ProcessAddressSyncAsync()
        {
            string departmentName = txtDepartmentName.Text.Trim();
            Guid? addressId = null;
            string message = string.Empty;

            if (GetAddressAction() == "create")
            {
                if (!string.IsNullOrWhiteSpace(hfAddressId.Value))
                {
                    addressId = new Guid(hfAddressId.Value);
                    message = await UpdateFireDepartmentAddressAsync(departmentName, addressId.Value);
                }
                else
                {
                    addressId = await CreateFireDepartmentAddressAsync(departmentName);
                    message = string.Format(
                        " New fire department address created (AddressId: {0}).",
                        addressId.Value);
                }
            }
            else
            {
                addressId = await LinkFireDepartmentAddressAsync(departmentName);
                if (addressId.HasValue)
                {
                    message = string.Format(
                        " Address linked (AddressId: {0}).",
                        addressId.Value);
                }
                else
                {
                    message = " Address was not linked.";
                }
            }

            if (addressId.HasValue)
            {
                await SaveDepartmentUdfsFromPanelAsync(addressId.Value);
            }

            return message;
        }

        private async System.Threading.Tasks.Task<Guid?> LinkFireDepartmentAddressAsync(string departmentName)
        {
            string selected = ddlAddressLink.SelectedValue;
            if (string.IsNullOrEmpty(selected) || selected == "__CREATE__")
            {
                return null;
            }

            Guid addressId = new Guid(selected);
            v_Addresses2 existing = await addressService.GetAddressByIdAsync(addressId);
            if (existing == null)
            {
                throw new Exception("Selected address was not found.");
            }

            string priorCode = existing.AddressCode ?? string.Empty;
            if (await addressService.ActiveFireDeptAddressCodeExistsAsync(departmentName, addressId))
            {
                throw new Exception("Another active address already uses this department name.");
            }

            existing.AddressCode = departmentName;
            await addressService.SaveAddressAsync(existing);

            logger.Info(string.Format(
                "FDID address link: AddressId={0}, prior AddressCode='{1}', new AddressCode='{2}'",
                addressId,
                priorCode,
                departmentName));

            return addressId;
        }

        private async System.Threading.Tasks.Task<Guid> CreateFireDepartmentAddressAsync(string departmentName)
        {
            if (await addressService.ActiveFireDeptAddressCodeExistsAsync(departmentName, null))
            {
                throw new Exception("An active address already uses this department name.");
            }

            v_Addresses2 model = await BuildAddressModelFromPanelAsync(
                departmentName,
                Guid.NewGuid(),
                Guid.NewGuid());

            await addressService.CreateAddressAsync(model);

            v_Addresses2 saved = await addressService.GetAddressByIdAsync(model.AddressId);
            if (saved == null)
            {
                throw new Exception(
                    "Address save failed. The new address was not found after create (AddressId: "
                    + model.AddressId + ").");
            }

            logger.Info(string.Format(
                "FDID address create: AddressId={0}, AddressCode='{1}'",
                model.AddressId,
                departmentName));

            hfAddressId.Value = model.AddressId.ToString();
            return model.AddressId;
        }

        private async System.Threading.Tasks.Task<string> UpdateFireDepartmentAddressAsync(
            string departmentName,
            Guid addressId)
        {
            v_Addresses2 existing = await addressService.GetAddressByIdAsync(addressId);
            if (existing == null)
            {
                throw new Exception("The address to update was not found.");
            }

            if (await addressService.ActiveFireDeptAddressCodeExistsAsync(departmentName, addressId))
            {
                throw new Exception("Another active address already uses this department name.");
            }

            v_Addresses2 model = await BuildAddressModelFromPanelAsync(
                departmentName,
                addressId,
                existing.rowguid);

            await addressService.SaveAddressAsync(model);

            logger.Info(string.Format(
                "FDID address update: AddressId={0}, AddressCode='{1}'",
                addressId,
                departmentName));

            return string.Format(" Fire department address updated (AddressId: {0}).", addressId);
        }

        private async System.Threading.Tasks.Task<v_Addresses2> BuildAddressModelFromPanelAsync(
            string departmentName,
            Guid addressId,
            Guid rowguid)
        {
            Guid countyId = ParseGuidOrThrow(ddlCreateCounty.SelectedValue, "County");
            Guid? zipId = await addressService.ResolveOrCreateZipIdAsync(
                txtCreateZip.Text.Trim(),
                countyId);
            if (zipId == null)
            {
                throw new Exception("Unable to resolve or create zip code '" + txtCreateZip.Text.Trim() + "'.");
            }

            Guid stateId = ParseGuidOrThrow(ddlCreateState.SelectedValue, "State");
            Guid addressTypeId = ParseGuidOrThrow(ddlCreateAddressType.SelectedValue, "Address type");
            State stateRow = (await addressService.GetStateListAsync())
                .FirstOrDefault(s => s.StateId == stateId);
            Guid? countryId = stateRow?.CountryId;
            if (!countryId.HasValue)
            {
                IEnumerable<Country> countries = await addressService.GetCountryListAsync();
                Country usa = countries.FirstOrDefault(c =>
                    c.Country1 != null
                    && c.Country1.IndexOf("United States", StringComparison.OrdinalIgnoreCase) >= 0);
                countryId = usa?.CountryId ?? countries.Select(c => c.CountryId).FirstOrDefault();
            }

            if (!countryId.HasValue || countryId.Value == Guid.Empty)
            {
                throw new Exception("Unable to determine country for the selected state.");
            }

            return new v_Addresses2
            {
                AddressId = addressId,
                rowguid = rowguid,
                AddressTypeId = addressTypeId,
                AddressCode = departmentName,
                AddressNumber = txtCreateAddressNumber.Text.Trim(),
                Direction = ddlCreateDirection.SelectedValue,
                Address = txtCreateAddress.Text.Trim(),
                Suffix = ddlCreateSuffix.SelectedValue,
                City = txtCreateCity.Text.Trim(),
                StateId = stateId,
                CountryId = countryId,
                CountyId = countyId,
                ZipId = zipId,
                Inactive = false
            };
        }

        private async System.Threading.Tasks.Task SaveDepartmentUdfsFromPanelAsync(Guid addressId)
        {
            var values = new DepartmentAddressUdfValues
            {
                IsoRating = ParseNonNegativeUdfValue(txtDeptIso.Text, "ISO Rating").ToString(),
                MainStations = ParseNonNegativeUdfValue(txtMainStations.Text, "Main Stations").ToString(),
                SubStations = ParseNonNegativeUdfValue(txtSubStations.Text, "Substations").ToString(),
                AdminBldgs = ParseNonNegativeUdfValue(txtAdminBldgs.Text, "Admin Buildings").ToString()
            };

            await addressService.SaveDepartmentAddressUdfValuesAsync(addressId, values);

            logger.Info(string.Format(
                "FDID department UDFs saved: AddressId={0}, ISO={1}, Main={2}, Sub={3}, Admin={4}",
                addressId,
                values.IsoRating,
                values.MainStations,
                values.SubStations,
                values.AdminBldgs));
        }

        private async System.Threading.Tasks.Task PopulateAddressEditPanelAsync(v_Addresses2 address)
        {
            if (address == null)
            {
                return;
            }

            hfAddressId.Value = address.AddressId.ToString();
            hfPriorAddressCode.Value = address.AddressCode ?? string.Empty;

            txtCreateAddressNumber.Text = address.AddressNumber ?? string.Empty;
            txtCreateAddress.Text = address.Address ?? string.Empty;
            txtCreateCity.Text = address.City ?? string.Empty;
            txtCreateZip.Text = await addressService.GetZipTextByZipIdAsync(address.ZipId);

            SelectDropDownValue(ddlCreateDirection, address.Direction ?? string.Empty);
            SelectDropDownValue(ddlCreateSuffix, address.Suffix ?? string.Empty);
            if (address.AddressTypeId.HasValue)
            {
                SelectDropDownValue(ddlCreateAddressType, address.AddressTypeId.Value.ToString());
            }
            if (address.StateId.HasValue)
            {
                SelectDropDownValue(ddlCreateState, address.StateId.Value.ToString());
            }
            if (address.CountyId.HasValue)
            {
                SelectDropDownValue(ddlCreateCounty, address.CountyId.Value.ToString());
            }

            DepartmentAddressUdfValues udfs =
                await addressService.GetDepartmentAddressUdfValuesAsync(address.AddressId);
            txtDeptIso.Text = udfs.IsoRating;
            txtMainStations.Text = udfs.MainStations;
            txtSubStations.Text = udfs.SubStations;
            txtAdminBldgs.Text = udfs.AdminBldgs;
        }

        private void SetCreateEditAddressMode()
        {
            rbAddressCreate.Checked = true;
            rbAddressLink.Checked = false;
            hfAddressAction.Value = "create";
        }

        private static void SelectDropDownValue(DropDownList dropdown, string value)
        {
            if (dropdown == null || string.IsNullOrEmpty(value))
            {
                return;
            }

            ListItem item = dropdown.Items.FindByValue(value);
            if (item != null)
            {
                dropdown.ClearSelection();
                item.Selected = true;
            }
        }

        private static Guid ParseGuidOrThrow(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(fieldName + " is required when creating an address.");
            }

            return new Guid(value);
        }

        private string BuildSuccessMessage(string addressPart)
        {
            return "<div class='alert alert-success'>NERIS ID saved successfully." + addressPart + "</div>";
        }

        private string GetAddressAction()
        {
            string action = hfAddressAction.Value ?? "link";
            return action.Equals("create", StringComparison.OrdinalIgnoreCase) ? "create" : "link";
        }

        private void SyncAddressActionFromRadios()
        {
            if (rbAddressCreate.Checked)
            {
                hfAddressAction.Value = "create";
            }
            else
            {
                hfAddressAction.Value = "link";
            }
        }

        private async System.Threading.Tasks.Task BindAddressLinkDropdownAsync(string departmentName)
        {
            ddlAddressLink.Items.Clear();
            ddlAddressLink.Items.Add(new ListItem("— Select an address —", ""));

            if (addressService == null)
            {
                return;
            }

            IReadOnlyList<FireDepartmentAddressMatch> matches =
                await addressService.GetFireDepartmentAddressMatchesAsync(departmentName, 20);

            foreach (FireDepartmentAddressMatch match in matches)
            {
                string displayText = string.Format(
                    "{0} — {1} (Apps: {2}, Users: {3})",
                    match.AddressCode,
                    match.FullAddress ?? string.Empty,
                    match.AppCount,
                    match.PartyLinkCount);
                ddlAddressLink.Items.Add(new ListItem(displayText, match.AddressId.ToString()));
            }

            ddlAddressLink.Items.Add(
                new ListItem("— Create / Edit Address (new) —", "__CREATE__"));
        }

        private async System.Threading.Tasks.Task BindCreateAddressDropdownsAsync()
        {
            if (addressService == null)
            {
                return;
            }

            ddlCreateDirection.Items.Clear();
            ddlCreateDirection.Items.Add(new ListItem(string.Empty, string.Empty));
            foreach (string direction in await addressService.GetDirectionListAsync())
            {
                ddlCreateDirection.Items.Add(new ListItem(direction, direction));
            }

            ddlCreateAddressType.Items.Clear();
            ddlCreateAddressType.Items.Add(new ListItem("— Select address type —", string.Empty));
            string defaultAddressTypeId = null;
            List<AddressType> addressTypes = (await addressService.GetAddressTypeListAsync()).ToList();
            foreach (AddressType addressType in addressTypes)
            {
                ddlCreateAddressType.Items.Add(
                    new ListItem(addressType.AddressType1, addressType.AddressTypeId.ToString()));
                if (defaultAddressTypeId == null
                    && addressType.AddressType1 != null
                    && addressType.AddressType1.Trim()
                        .Equals("FS Fire Department", StringComparison.OrdinalIgnoreCase))
                {
                    defaultAddressTypeId = addressType.AddressTypeId.ToString();
                }
            }
            if (defaultAddressTypeId == null)
            {
                AddressType fsFireType = addressTypes.FirstOrDefault(t =>
                    t.AddressType1 != null
                    && t.AddressType1.IndexOf(
                        "FS Fire Department",
                        StringComparison.OrdinalIgnoreCase) >= 0);
                if (fsFireType != null)
                {
                    defaultAddressTypeId = fsFireType.AddressTypeId.ToString();
                }
            }
            if (defaultAddressTypeId == null)
            {
                defaultAddressTypeId = FireDeptAddressTypeId.ToString();
            }
            if (ddlCreateAddressType.Items.FindByValue(defaultAddressTypeId) != null)
            {
                ddlCreateAddressType.SelectedValue = defaultAddressTypeId;
            }

            ddlCreateSuffix.Items.Clear();
            ddlCreateSuffix.Items.Add(new ListItem(string.Empty, string.Empty));
            foreach (string suffix in await addressService.GetSuffixListAsync())
            {
                ddlCreateSuffix.Items.Add(new ListItem(suffix, suffix));
            }

            ddlCreateState.Items.Clear();
            ddlCreateState.Items.Add(new ListItem("— Select state —", string.Empty));
            string defaultStateId = null;
            foreach (State state in await addressService.GetStateListAsync())
            {
                ddlCreateState.Items.Add(new ListItem(state.State1, state.StateId.ToString()));
                if (defaultStateId == null
                    && state.StateAbbr != null
                    && state.StateAbbr.Equals("NM", StringComparison.OrdinalIgnoreCase))
                {
                    defaultStateId = state.StateId.ToString();
                }
            }
            if (defaultStateId != null)
            {
                ddlCreateState.SelectedValue = defaultStateId;
            }

            ddlCreateCounty.Items.Clear();
            ddlCreateCounty.Items.Add(new ListItem("— Select county —", string.Empty));
            foreach (County county in await addressService.GetCountyListAsync())
            {
                ddlCreateCounty.Items.Add(
                    new ListItem(county.County1, county.CountyId.ToString()));
            }
        }

        private void RegisterReopenModalScript(string addressAction)
        {
            string action = addressAction ?? "link";
            string script = "fdidSetAddressAction('" + action + "'); fdidShowModal();";
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ReopenFDIDModal",
                script,
                true);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
