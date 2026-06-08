using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
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
using Telerik.Web.UI;

namespace NMSFMFireGrantWF.Admin
{
    public partial class ManageFDIDs : System.Web.UI.Page
    {
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
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
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
            // Escape only characters that break a double-quoted attribute; do not
            // HtmlAttributeEncode apostrophes (&#39;) or postback fails validation.
            editLink.Attributes["data-dept"] = dept
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;");
            editLink.Attributes["data-inactive"] = row.Inactive.ToString().ToLowerInvariant();
            editLink.Attributes["onclick"] = "return fdidOpenForEdit(this);";
        }

        protected async void btnSaveFDID_Click(object sender, EventArgs e)
        {
            try
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

                Session["SaveMessage"] = "<div class='alert alert-success'>NERIS ID saved successfully.</div>";
                Response.Redirect("~/Admin/ManageFDIDs", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ReopenFDIDModal",
                    "openFDIDModal();",
                    true);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
