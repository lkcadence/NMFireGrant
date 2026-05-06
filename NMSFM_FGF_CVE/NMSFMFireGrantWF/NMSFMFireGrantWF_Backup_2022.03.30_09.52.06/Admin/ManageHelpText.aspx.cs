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
    public partial class ManageHelpText : System.Web.UI.Page
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
                if (Session["Role"] == null || Convert.ToString(Session["Role"]) == "External")
                {
                    Response.Redirect("~/Unauthorized");
                }
                if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                {
                    Response.Redirect("~/Unauthorized");
                }
                this.helpPages = new List<ListItem>();
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
                helpPages.Add(new ListItem("-Other-", "-Other-"));
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Manage Help Text (Admin)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                //InitTestCategories();
                ddlPage.DataSource = helpPages;
                ddlPage.DataBind();
                LoadHelpSections();
            }
        }

        private async void LoadHelpSections()
        {
            try
            {
                List<FG_App_Help> cats = await fgService.GetFGAllHelp();
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
            //try
            //{

            //    if (e.Item is GridDataItem)
            //    {
            //        GridDataItem dataItem = e.Item as GridDataItem;
            //        string page = "";
            //        page = dataItem["Page"].Text;
            //        string section = "";
            //        section = dataItem["Section"].Text;

            //        LinkButton delete = (LinkButton)dataItem["Edit"].Controls[0];
            //        delete.Text = "View/Edit " + page;
            //        if (section != "")
            //        {
            //            delete.Text += " (" + section + ")";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            //}
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
                        try
                        {
                            ddlPage.SelectedValue = help.Page;
                        }
                        catch
                        {
                            ddlPage.SelectedValue = "-Other-";
                            txtPage.Visible = true;
                            txtPage.Text = help.Page;
                        }
                        
                        txtSection.Text = help.Section;
                        txtHelpNumber.Text = help.Number.ToString();
                        txtHelpText.Content = help.HelpText;
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
                        chkHelpInactive.Checked = help.Inactive;
                        chkAdminOnly.Checked = help.AdminOnly;
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void lnkHelpImage_Click(object sender, EventArgs e)
        {
            try
            {
                string pId = hfHelpId.Value;
                Guid trId = new Guid(pId);
                List<FG_App_Help> helps = (List<FG_App_Help>)ViewState["dtHelpSections"];
                FG_App_Help help = helps.FirstOrDefault(a => a.HelpId == trId);
                if (help != null)
                {
                    byte[] bytes;
                    string fileName, contentType;
                    contentType = "image";
                    bytes = help.Image;
                    fileName = help.Page + " Image";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = contentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void btnSaveHelp_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblHelpError.Text = "";
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (ddlPage.SelectedValue == "-Other-")
                {
                    if (txtPage.Text == "")
                    {
                        errorMessage += "Page is Required.<br />";
                        isValid = false;
                    }
                }
                if (txtHelpText.Content == "")
                {
                    errorMessage += "Help Text is Required.<br />";
                    isValid = false;
                }
                if (ruHelpImage.UploadedFiles.Count != 0)
                {
                    bool isCorrectFormat = false;
                    if (ruHelpImage.UploadedFiles[0].ContentType.ToString() == "image/jpeg" | ruHelpImage.UploadedFiles[0].ContentType.ToString() == "image/png" | ruHelpImage.UploadedFiles[0].ContentType.ToString() == "image/bmp")
                    {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Images must be in the format of .jpg, .png or .bmp<br />");
                    }
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }

                FG_App_Help help = new FG_App_Help();

                
                if (hfHelpId.Value == "")
                {
                    help.HelpId = Guid.NewGuid();
                }
                else
                {
                    help.HelpId = new Guid(hfHelpId.Value);
                }

                UploadedFile file;
                byte[] fileData = null;
                string theextension = null;
                string filename = null;
                if (ruHelpImage.UploadedFiles.Count != 0)
                {
                    file = ruHelpImage.UploadedFiles[0];
                    fileData = new byte[file.InputStream.Length];
                    file.InputStream.Read(fileData, 0, Convert.ToInt32(file.InputStream.Length));
                    theextension = file.GetExtension();
                    filename = file.FileName;
                }
                else
                {
                    fileData = null;
                    theextension = string.Empty;
                    filename = null;
                }

                if (ddlPage.SelectedValue != "-Other-")
                {
                    help.Page = ddlPage.SelectedValue.ToString();
                }
                else
                {
                    help.Page = txtPage.Text;
                }
                
                help.Section = txtSection.Text;
                help.HelpText = txtHelpText.Content;
                help.Image = fileData;
                help.Number = Convert.ToInt32(txtHelpNumber.Text);
                help.Inactive = chkHelpInactive.Checked;
                help.AdminOnly = chkAdminOnly.Checked;

                bool saved = await fgService.SavHelpText(help);
                LoadHelpSections();

                txtPage.Text = "";
                txtSection.Text = "";
                txtHelpNumber.Text = "";
                txtHelpText.Content = "";
                ruHelpImage.UploadedFiles.Clear();
                hfHelpId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + txtPage.Text + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                lblHelpError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openTrainingModal();", true);

            }
        }
    }
}