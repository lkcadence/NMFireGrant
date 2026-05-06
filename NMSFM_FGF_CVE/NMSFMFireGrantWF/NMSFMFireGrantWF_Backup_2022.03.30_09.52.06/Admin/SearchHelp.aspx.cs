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
    public partial class SearchHelp : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;

        List<ListItem> helpPages;

        protected void Page_Init(object sender, EventArgs e)
        {
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
                //if (Session["Role"] == null || Convert.ToString(Session["Role"]) == "External")
                //{
                //    Response.Redirect("~/Unauthorized");
                //}
                //if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                //{
                //    Response.Redirect("~/Unauthorized");
                //}
                this.helpPages = new List<ListItem>();
                helpPages.Add(new ListItem("-All Pages-", "-All Pages-"));
                if (Session["IsWebAdmin"] != null && Convert.ToBoolean(Session["IsWebAdmin"]) == true)
                {
                    helpPages.Add(new ListItem("Add Codepal User (Account)", "Add Codepal User (Account)"));
                    helpPages.Add(new ListItem("Add New User (Account)", "Add New User (Account)"));
                    helpPages.Add(new ListItem("Edit User (Account)", "Edit User (Account)"));
                    helpPages.Add(new ListItem("Forgot Password (Account)", "Forgot Password (Account)"));
                    helpPages.Add(new ListItem("Login (Account)", "Login (Account)"));
                    helpPages.Add(new ListItem("Manage Users (Account)", "Manage Users (Account)"));
                    helpPages.Add(new ListItem("Category (Admin)", "Category (Admin)"));
                    helpPages.Add(new ListItem("Home (Admin)", "Home (Admin)"));
                    helpPages.Add(new ListItem("Manage Categories (Admin)", "Manage Categories (Admin)"));
                    helpPages.Add(new ListItem("Manage Help Text (Admin)", "Manage Help Text (Admin)"));
                    helpPages.Add(new ListItem("Manage Settings (Admin)", "Manage Settings (Admin)"));
                    helpPages.Add(new ListItem("Home (User)", "Home (User)"));
                    helpPages.Add(new ListItem("Application Review (Application)", "Application Review (Application)"));
                }
                
                helpPages.Add(new ListItem("Application Status (Application)", "Application Status (Application)"));
                helpPages.Add(new ListItem("Apparatus (Application)", "Apparatus (Application)"));
                helpPages.Add(new ListItem("Budget Info (Application)", "Budget Info (Application)"));
                helpPages.Add(new ListItem("Communication Equipment (Application)", "Communication Equipment (Application)"));
                helpPages.Add(new ListItem("Community Info (Application)", "Community Info (Application)"));
                helpPages.Add(new ListItem("Equipment Needs (Application)", "Equipment Needs (Application)"));
                helpPages.Add(new ListItem("Funding Justification (Application)", "Funding Justification (Application)"));
                helpPages.Add(new ListItem("General Information (Application)", "General Information (Application)"));
                helpPages.Add(new ListItem("Hazards Threats (Application)", "Hazards Threats (Application)"));
                helpPages.Add(new ListItem("Instructions (Application)", "Instructions (Application)"));
                helpPages.Add(new ListItem("PPE (Application)", "PPE (Application)"));
                helpPages.Add(new ListItem("Project Budget Sheet (Application)", "Project Budget Sheet (Application)"));
                helpPages.Add(new ListItem("Response History (Application)", "Response History (Application)"));
                helpPages.Add(new ListItem("Signatures Documentation (Application)", "Signatures Documentation (Application)"));
                helpPages.Add(new ListItem("Training (Application)", "Training (Application)"));
                helpPages.Add(new ListItem("Water Availability (Application)", "Water Availability (Application)"));
                
            }
            catch (Exception ex)
            {

            }
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Search Help Text");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                //InitTestCategories();
                ddlSearchPage.DataSource = helpPages;
                ddlSearchPage.DataBind();
                LoadHelpSections();
            }
        }

        private async void LoadHelpSections()
        {
            try
            {
                List<FG_App_Help> cats = await fgService.GetFGAllHelp();
                
                
                if (ddlSearchPage.SelectedIndex > 1)
                {
                    cats = cats.Where(a => a.Page == ddlSearchPage.SelectedValue.ToString()).ToList();
                }
                if (txtSearchContent.Text.Trim() != "")
                {
                    cats = cats.Where(a => a.HelpText.ToLower().Contains(txtSearchContent.Text.ToLower())).ToList();
                }

                if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                {
                    cats = cats.Where(a => a.AdminOnly == false).ToList();
                }

                ViewState["dtHelpSections"] = cats;
                rgHelp.DataSource = cats;
                rgHelp.DataBind();
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {

        }

        protected void rgHelp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_Help> help = (List<FG_App_Help>)ViewState["dtHelpSections"];
            rgHelp.DataSource = help;
        }

        protected void rgHelp_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_App_Help> help = (List<FG_App_Help>)ViewState["dtHelpSections"];
            rgHelp.DataSource = help;
            rgHelp.DataBind();
        }

        protected void rgHelp_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void rgHelp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string pId = e.CommandArgument.ToString();
                    Guid trId = new Guid(pId);
                    List<FG_App_Help> helpsections = (List<FG_App_Help>)ViewState["dtHelpSections"];
                    FG_App_Help help = helpsections.FirstOrDefault(a => a.HelpId == trId);
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View" && help != null)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openHelpModal();", true);

                        hfHelpId.Value = pId;
                        ltrPage.Text = help.Page;

                        ltrSection.Text = help.Section;
                        ltrHelpText.Text = help.HelpText;
                        if (help.Image != null)
                        {
                            //lnkHelpImage.Text = "View Image";
                            imagexa.Src = "data:image/png;base64," + Convert.ToBase64String(help.Image);
                            imagexa.Visible = true;
                            imagexa.Alt = help.Page + " Image";
                            dvHelpDocLink.Visible = true;
                        }
                        else
                        {
                            dvHelpDocLink.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadHelpSections();
        }
    }
}