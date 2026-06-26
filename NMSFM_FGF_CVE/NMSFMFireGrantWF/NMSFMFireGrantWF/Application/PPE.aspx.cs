using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using NMSFM.Data;
using NMSFM.ViewModels;
using NMSFM.Services.Logging;
using NMSFM.Services.Images;
using NMSFM.Services.Party;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.Menu;
using NMSFM.Services.FYDist;
using NMSFM.Services.FireGrant;
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;

namespace NMSFMFireGrantWF.Application
{
    public partial class PPE : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

        RadMenu _rmStep1;

        private const string DocTypePpeDocument = "PPE File Upload";
        private const string DocTypeScbaDocument = "SCBA File Upload";
        private const string ViewStatePpeDocuments = "dtPPEDocuments";
        private const string ViewStateScbaDocuments = "dtSCBADocuments";
        private const string SectionPpe = "PPE";
        private const string SectionScba = "SCBA";

        private static readonly string[] PpeDocumentTypes = { DocTypePpeDocument };
        private static readonly string[] ScbaDocumentTypes = { DocTypeScbaDocument };
        private static readonly string[] AllowedExtensions =
            { ".xls", ".xlsx", ".csv", ".pdf", ".doc", ".docx" };
        private const int MaxDocumentNameLength = 255;

        protected void Page_Init(object sender, EventArgs e)
        {
            logger = new Logging();
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
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
                if (Session["Role"] == null)
                {
                    Response.Redirect("~/Unauthorized");
                }
                if (Session["Department"] == null)
                {
                    if (Session["Role"].ToString() == "Internal")
                    {
                        Response.Redirect("~/Admin/Home");
                    }
                    else if (Session["Role"].ToString() == "External")
                    {
                        Response.Redirect("~/User/Home");
                    }
                    else
                    {
                        Response.Redirect("~/Unauthorized");
                    }
                }
                else
                {
                    if (Session["Role"].ToString() == "Internal")
                    {
                        dvAdmin.Visible = true;
                    }
                    else if (Session["Role"].ToString() == "External" || Session["Role"].ToString() == "Signator")
                    {
                        dvAdmin.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("PPE (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Current Personal Protective Equipment (PPE)";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (Session["ApplicationId"] != null)
                {
                    hfApplicationId.Value = Session["ApplicationId"].ToString();
                }

                if (Page.IsPostBack)
                {
                    await HandlePendingFileUploadAsync();
                }

                if (!Page.IsPostBack)
                {
                    string appId = hfApplicationId.Value;
                    if (!string.IsNullOrEmpty(appId))
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppPPE ppe = new DetailedFGAppPPE();
                        ppe = await fgAppService.GetFGApplicationPPEAsync(appIdGuid);
                        if (ppe != null && ppe.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadPPE(ppe);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        await LoadApplicationDocuments(appIdGuid);
                    }
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                    }
                    await LoadPPETypes();
                    await LoadSCBATypes();
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }

            //InitTestSources();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.Form != null)
            {
                Page.Form.Enctype = "multipart/form-data";
            }
        }

        private static string NormalizeInvalidText(string invalidText)
        {
            if (string.IsNullOrEmpty(invalidText))
            {
                return invalidText;
            }

            return invalidText
                .Replace("PPE list is required", "PPE list or File Upload is required")
                .Replace("SCBA list is required", "SCBA list or File Upload is required");
        }

        private async Task HandlePendingFileUploadAsync()
        {
            string action = hfUploadAction.Value;
            if (string.IsNullOrEmpty(action))
            {
                return;
            }

            try
            {
                if (action == SectionPpe)
                {
                    if (!fuPPEDocumentation.HasFile)
                    {
                        dvPPEDocumentError.InnerHtml =
                            "<div class='alert alert-danger'>No file was received. Please try again.</div>";
                        return;
                    }

                    await UploadDocumentAsync(
                        fuPPEDocumentation,
                        DocTypePpeDocument,
                        dvPPEDocumentError,
                        PpeDocumentTypes,
                        ViewStatePpeDocuments,
                        rgPPEDocuments);
                }
                else if (action == SectionScba)
                {
                    if (!fuSCBADocumentation.HasFile)
                    {
                        dvSCBADocumentError.InnerHtml =
                            "<div class='alert alert-danger'>No file was received. Please try again.</div>";
                        return;
                    }

                    await UploadDocumentAsync(
                        fuSCBADocumentation,
                        DocTypeScbaDocument,
                        dvSCBADocumentError,
                        ScbaDocumentTypes,
                        ViewStateScbaDocuments,
                        rgSCBADocuments);
                }

                dvError.InnerHtml = string.Empty;
            }
            catch (Exception ex)
            {
                _ = ex;
                HtmlGenericControl errorDiv = action == SectionScba
                    ? dvSCBADocumentError
                    : dvPPEDocumentError;
                errorDiv.InnerHtml =
                    "<div class='alert alert-danger'>" + ex.Message + "</div>";
            }
            finally
            {
                hfUploadAction.Value = string.Empty;
            }
        }

        private async Task<bool> LoadPPETypes()
        {
            //ToDo Get Equipment From Database Once connected
            try
            {
                ddlPPEType.Items.Clear();
                List<FG_Priorities> priorities = await fgService.GetFGPriorities(4);
                foreach (FG_Priorities priority in priorities)
                {
                    ListItem li = new ListItem();
                    li.Text = priority.PriorityName;
                    li.Value = priority.PriorityName;
                    ddlPPEType.Items.Add(li);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> LoadSCBATypes()
        {
            //ToDo Get Equipment From Database Once connected
            try
            {
                ddlSCBAType.Items.Clear();
                List<FG_Priorities> priorities = await fgService.GetFGPriorities(5);
                foreach (FG_Priorities priority in priorities)
                {
                    ListItem li = new ListItem();
                    li.Text = priority.PriorityName;
                    li.Value = priority.PriorityName;
                    ddlSCBAType.Items.Add(li);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            if (con is TextBox)
            {
                TextBox t = (TextBox)con;
                t.ReadOnly = true;
            }
            if (con is RadTextBox)
            {
                RadTextBox t = (RadTextBox)con;
                t.ReadOnly = true;
            }
            if (con is RadNumericTextBox)
            {
                RadNumericTextBox t = (RadNumericTextBox)con;
                t.ReadOnly = true;
            }
            else if (con is CheckBox)
            {
                CheckBox t = (CheckBox)con;
                t.Enabled = false;
            }
            else if (con is RadioButton)
            {
                RadioButton t = (RadioButton)con;
                t.Enabled = false;
            }
            else if (con is DropDownList)
            {
                DropDownList d = (DropDownList)con;
                d.Enabled = false;
            }
            else if (con is FileUpload)
            {
                con.Visible = false;
            }
            else if (con is RadAsyncUpload)
            {
                con.Visible = false;
            }
            else if (con is RadGrid)
            {
                RadGrid g = (RadGrid)con;
                g.Columns[0].Visible = false;
            }
            btnSave.Visible = false;
            btnUploadPPEDocuments.Visible = false;
            btnUploadSCBADocuments.Visible = false;
            dvShowModal.Visible = false;
            dvShowModal2.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
            }
        }

        private void ApplyPPEPartOfProjectGates(DetailedFGAppPPE model)
        {
            int ppePart = model.PPEPartOfProject;
            if (ppePart == 0
                && model.StandardPPE != null
                && model.StandardPPE.Count > 0)
            {
                ppePart = 1;
            }
            rbPPEYes.Checked = ppePart == 1;
            rbPPENo.Checked = ppePart == 2;

            int scbaPart = model.SCBAPartOfProject;
            if (scbaPart == 0
                && model.StandardSCBA != null
                && model.StandardSCBA.Count > 0)
            {
                scbaPart = 1;
            }
            rbSCBAYes.Checked = scbaPart == 1;
            rbSCBANo.Checked = scbaPart == 2;
        }

        private void LoadPPE(DetailedFGAppPPE model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    if (model.InvalidText != null)
                    {
                        string invalidText = NormalizeInvalidText(model.InvalidText);
                        dvError.InnerHtml = "<div class='alert alert-danger'>" + invalidText + "</div>";
                    }
                    else
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    }
                }
                ApplyPPEPartOfProjectGates(model);

                if (model.PPEInspected == 1) { rbPPEInspectedYes.Checked = true; }
                if (model.PPEInspected == 2) { rbPPEInspectedNo.Checked = true; }

                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgStandardComplientPPE.DataSource = model.StandardPPE;
                ViewState["dtPPE"] = model.StandardPPE;

                rgStandardComplientSCBA.DataSource = model.StandardSCBA;
                ViewState["dtSCBA"] = model.StandardSCBA;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        protected async void rmStep1_Click(object sender, Telerik.Web.UI.RadMenuEventArgs e)
        {
            if (await SaveForm() == true)
            {
                switch (_rmStep1.SelectedItem.Text)
                {
                    case "Instructions":
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                    case "General Information":
                        Response.Redirect("~/Application/GeneralInformation", false);
                        break;
                    case "Budget Information":
                        Response.Redirect("~/Application/BudgetInfo", false);
                        break;
                    case "Community Information":
                        Response.Redirect("~/Application/CommunityInfo", false);
                        break;
                    case "Response History":
                        Response.Redirect("~/Application/ResponseHistory", false);
                        break;
                    case "Water Availability":
                        Response.Redirect("~/Application/WaterAvailability", false);
                        break;
                    case "Training":
                        Response.Redirect("~/Application/Training", false);
                        break;
                    case "Apparatus":
                        Response.Redirect("~/Application/Apparatus", false);
                        break;
                    case "Communication Equipment":
                        Response.Redirect("~/Application/CommunicationEquipment", false);
                        break;
                    case "Hazards/Threats":
                        Response.Redirect("~/Application/HazardsThreats", false);
                        break;
                    case "PPE":
                        //Response.Redirect("~/Application/PPE", false);
                        break;
                    case "Equipment Needs":
                        Response.Redirect("~/Application/EquipmentNeeds", false);
                        break;
                    case "Grant Funding Justification":
                        Response.Redirect("~/Application/FundingJustification", false);
                        break;
                    case "Project Budget Sheet":
                        Response.Redirect("~/Application/ProjectBudgetSheet", false);
                        break;
                    case "Signatures and Supporting Docs":
                        Response.Redirect("~/Application/SignaturesDocs", false);
                        break;
                    default:
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                }
            }
        }

        private void InitTestSources()
        {
            rbPPEYes.Checked = true;
            DataTable ppe = new DataTable();
            ppe.Columns.Add("Year", typeof(string));
            ppe.Columns.Add("Quantity", typeof(string));
            ppe.Columns.Add("Age", typeof(string));
            ppe.Columns.Add("Condition", typeof(string));
            ppe.Columns.Add("StandardComplientPPEId", typeof(string));

            DataTable scba = new DataTable();
            scba.Columns.Add("Year", typeof(string));
            scba.Columns.Add("Quantity", typeof(string));
            scba.Columns.Add("Age", typeof(string));
            scba.Columns.Add("Condition", typeof(string));
            scba.Columns.Add("StandardComplientSCBAId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string year = (2000 + i).ToString();
                string qty = (i * 2).ToString();
                string age = (5 + 1).ToString();
                string condition = "Good";
                string ppeId = Guid.NewGuid().ToString();
                ppe.Rows.Add(year, qty, age, condition, ppeId);
                scba.Rows.Add(year, qty, age, condition, ppeId);
            }

            ViewState["dtPPE"] = ppe;
            ViewState["dtSCBA"] = scba;
            rgStandardComplientPPE.DataSource = ppe;
            rgStandardComplientPPE.DataBind();
            rgStandardComplientSCBA.DataSource = scba;
            rgStandardComplientSCBA.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/CommunicationEquipment", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>PPE Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>PPE Data Saved</div>";
                Response.Redirect("~/Application/PPE", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/HazardsThreats", false);
            }
        }

        private async Task<bool> SaveForm()
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true && dvAdmin.Visible == false)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                List<FG_App_StandardPPE> standardPPE = (List<FG_App_StandardPPE>)ViewState["dtPPE"];
                List<FG_App_StandardSCBA> standardSCBA = (List<FG_App_StandardSCBA>)ViewState["dtSCBA"];

                int ppePartOfProject = 0;
                if (rbPPEYes.Checked) { ppePartOfProject = 1; }
                if (rbPPENo.Checked) { ppePartOfProject = 2; }
                if (ppePartOfProject == 0)
                {
                    errorMessage += "Is PPE Part of Project reponse is required<br />";
                    isValid = false;
                }
                int ppeInspected = 0;
                // Disabled: preserve section data when PartOfProject = No
                //if (ppePartOfProject == 2)
                //{
                //    ppeInspected = 0;
                //    rbPPEInspectedYes.Checked = false;
                //    rbPPEInspectedNo.Checked = false;
                //    standardPPE = new List<FG_App_StandardPPE>();
                //    ViewState["dtPPE"] = standardPPE;
                //}
                if (ppePartOfProject == 1)
                {
                    if (rbPPEInspectedYes.Checked) { ppeInspected = 1; }
                    if (rbPPEInspectedNo.Checked) { ppeInspected = 2; }
                    if (ppeInspected == 0)
                    {
                        errorMessage += "Is PPE Inspected reponse is required<br />";
                        isValid = false;
                    }
                    Guid appId = new Guid(hfApplicationId.Value);
                    List<FG_AppDocListItem> ppeDocuments =
                        await fgAppService.GetApplicationDocumentsByTypesAsync(
                            appId, PpeDocumentTypes);
                    bool hasPpeList = standardPPE != null && standardPPE.Count >= 1;
                    bool hasPpeDocuments = ppeDocuments != null && ppeDocuments.Count >= 1;
                    if (!hasPpeList && !hasPpeDocuments)
                    {
                        errorMessage += "PPE list or File Upload is required<br />";
                        isValid = false;
                    }
                }

                int scbaPartOfProject = 0;
                if (rbSCBAYes.Checked) { scbaPartOfProject = 1; }
                if (rbSCBANo.Checked) { scbaPartOfProject = 2; }
                if (scbaPartOfProject == 0)
                {
                    errorMessage += "Is SCBA Part of Project reponse is required<br />";
                    isValid = false;
                }
                // Disabled: preserve section data when PartOfProject = No
                //if (scbaPartOfProject == 2)
                //{
                //    standardSCBA = new List<FG_App_StandardSCBA>();
                //    ViewState["dtSCBA"] = standardSCBA;
                //}
                if (scbaPartOfProject == 1)
                {
                    Guid appId = new Guid(hfApplicationId.Value);
                    List<FG_AppDocListItem> scbaDocuments =
                        await fgAppService.GetApplicationDocumentsByTypesAsync(
                            appId, ScbaDocumentTypes);
                    bool hasScbaList = standardSCBA != null && standardSCBA.Count >= 1;
                    bool hasScbaDocuments = scbaDocuments != null && scbaDocuments.Count >= 1;
                    if (!hasScbaList && !hasScbaDocuments)
                    {
                        errorMessage += "SCBA list or File Upload is required<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    errorMessage = NormalizeInvalidText(errorMessage);
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new DetailedFGAppPPE();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = NormalizeInvalidText(errorMessage);
                model.UpdatedBy = Session["WebUser"].ToString();

                model.PPEPartOfProject = ppePartOfProject;
                model.SCBAPartOfProject = scbaPartOfProject;
                model.PPEInspected = ppeInspected;
                model.AdminComments = txtComments.Text;
                model.StandardPPE = standardPPE;
                model.StandardSCBA = standardSCBA;

                bool retVal = await fgAppService.SavePPEAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void btnDeleteStandardCompliaintPPE_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_StandardPPE> standardPPE = (List<FG_App_StandardPPE>)ViewState["dtPPE"];
                for (int i = 0; i < standardPPE.Count; i++)
                {
                    if (standardPPE[i].StandardComplientPPEId.ToString() == hfStandardCompliaintPPEId.Value.ToString())
                    {
                        standardPPE.RemoveAt(i);
                        break;
                    }
                }
                ViewState["dtPPE"] = standardPPE;
                rgStandardComplientPPE.DataSource = standardPPE;
                rgStandardComplientPPE.DataBind();
                txtStandardCompliaintPPEAge.Text = "";
                txtStandardCompliaintPPEQuantity.Text = "";
                txtStandardCompliaintPPEYear.Text = "";
                ddlPPEType.SelectedIndex = 0;
                ddlStandardCompliaintPPECondition.SelectedIndex = 0;
                hfStandardCompliaintPPEId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgStandardComplientPPE_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_StandardPPE> standardPPE = (List<FG_App_StandardPPE>)ViewState["dtPPE"];
            rgStandardComplientPPE.DataSource = standardPPE;
            
        }

        protected void rgStandardComplientPPE_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_StandardPPE> standardPPE = (List<FG_App_StandardPPE>)ViewState["dtPPE"];
            rgStandardComplientPPE.DataSource = standardPPE;
            rgStandardComplientPPE.DataBind();
        }

        protected void rgStandardComplientPPE_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgStandardComplientPPE_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string year = "";
                    year = dataItem["Year"].Text;
                    string age = dataItem["Age"].Text;
                    string qty = dataItem["Quantity"].Text;
                    string condition = dataItem["Condition"].Text;
                    string ppeType = dataItem["PPEType"].Text;
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openStandardCompliantPPEModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfStandardCompliaintPPEId.Value = pId;
                        ddlPPEType.SelectedValue = ppeType;
                        txtStandardCompliaintPPEYear.Text = year;
                        txtStandardCompliaintPPEQuantity.Text = qty;
                        txtStandardCompliaintPPEAge.Text = age;
                        ddlStandardCompliaintPPECondition.SelectedValue = condition;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgStandardComplientSCBA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_StandardSCBA> standardSCBA = (List<FG_App_StandardSCBA>)ViewState["dtSCBA"];
            rgStandardComplientSCBA.DataSource = standardSCBA;
        }

        protected void rgStandardComplientSCBA_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_App_StandardSCBA> standardSCBA = (List<FG_App_StandardSCBA>)ViewState["dtSCBA"];
            rgStandardComplientSCBA.DataSource = standardSCBA;
            rgStandardComplientSCBA.DataBind();
        }

        protected void rgStandardComplientSCBA_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void rgStandardComplientSCBA_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string year = "";
                    year = dataItem["Year"].Text;
                    string age = dataItem["Age"].Text;
                    string qty = dataItem["Quantity"].Text;
                    string condition = dataItem["Condition"].Text;
                    string scbaType = dataItem["SCBAType"].Text;
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openStandardCompliantSCBAModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfStandardCompliaintSCBAId.Value = pId;
                        ddlSCBAType.SelectedValue = scbaType;
                        txtStandardCompliaintSCBAYear.Text = year;
                        txtStandardCompliaintSCBAQuantity.Text = qty;
                        txtStandardCompliaintSCBAAge.Text = age;
                        ddlStandardCompliaintSCBACondition.SelectedValue = condition;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveStandardCompliaintPPE_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblStandardCompliaintPPEError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtStandardCompliaintPPEAge.Text == "")
                {
                    errorMessage += "PPE Age is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintPPEAge.Text) < 1)
                    {
                        errorMessage += "PPE Age must be greater than 0.<br />";
                        isValid = false;
                    }
                }
                if (txtStandardCompliaintPPEQuantity.Text == "")
                {
                    errorMessage += "PPE Quantity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintPPEQuantity.Text) < 1)
                    {
                        errorMessage += "PPE Quantity must be greater than 0.<br />";
                        isValid = false;
                    }
                }

                if (txtStandardCompliaintPPEYear.Text == "")
                {
                    errorMessage += "PPE Year is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintPPEYear.Text) < 1)
                    {
                        errorMessage += "PPE Year must be greater than 0.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_StandardPPE> standardPPEs = new List<FG_App_StandardPPE>();
                if (ViewState["dtPPE"] != null)
                {
                    standardPPEs = (List<FG_App_StandardPPE>)ViewState["dtPPE"];
                }

                FG_App_StandardPPE standardPPE = new FG_App_StandardPPE();

                if (hfStandardCompliaintPPEId.Value != "")
                {
                    for (int i = 0; i < standardPPEs.Count; i++)
                    {
                        if (standardPPEs[i].StandardComplientPPEId.ToString() == hfStandardCompliaintPPEId.Value.ToString())
                        {
                            standardPPE = standardPPEs[i];
                            standardPPEs.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (standardPPE.StandardComplientPPEId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    standardPPE.StandardComplientPPEId = Guid.NewGuid();
                }

                standardPPE.Age = Convert.ToInt32(txtStandardCompliaintPPEAge.Text);
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                standardPPE.ApplicationId = appId;
                standardPPE.PPEType = ddlPPEType.Text;
                standardPPE.Quantity = Convert.ToInt32(txtStandardCompliaintPPEQuantity.Text);
                standardPPE.Year = Convert.ToInt32(txtStandardCompliaintPPEYear.Text);
                standardPPE.Condition = ddlStandardCompliaintPPECondition.SelectedValue;
                standardPPEs.Add(standardPPE);
                ViewState["dtPPE"] = standardPPEs;
                rgStandardComplientPPE.DataSource = standardPPEs;
                rgStandardComplientPPE.DataBind();
                txtStandardCompliaintPPEAge.Text = "";
                txtStandardCompliaintPPEYear.Text = "";
                txtStandardCompliaintPPEQuantity.Text = "";
                ddlStandardCompliaintPPECondition.SelectedIndex = 0;
                hfStandardCompliaintPPEId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + standardPPE.Condition + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblStandardCompliaintPPEError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openStandardCompliantPPAModal();", true);
            }
        }

        protected void btnDeleteStandardCompliaintSCBA_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_StandardSCBA> standardSCBA = (List<FG_App_StandardSCBA>)ViewState["dtSCBA"];
                for (int i = 0; i < standardSCBA.Count; i++)
                {
                    if (standardSCBA[i].StandardComplientSCBAId.ToString() == hfStandardCompliaintSCBAId.Value.ToString())
                    {
                        standardSCBA.RemoveAt(i);
                        break;
                    }
                }
                ViewState["dtSCBA"] = standardSCBA;
                rgStandardComplientSCBA.DataSource = standardSCBA;
                rgStandardComplientSCBA.DataBind();
                txtStandardCompliaintSCBAAge.Text = "";
                txtStandardCompliaintSCBAQuantity.Text = "";
                txtStandardCompliaintSCBAYear.Text = "";
                ddlSCBAType.SelectedIndex = 0;
                ddlStandardCompliaintSCBACondition.SelectedIndex = 0;
                hfStandardCompliaintSCBAId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveStandardCompliaintSCBA_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblStandardCompliaintSCBAError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtStandardCompliaintSCBAAge.Text == "")
                {
                    errorMessage += "SCBA Age is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintSCBAAge.Text) < 1)
                    {
                        errorMessage += "SCBA Age must be greater than 0.<br />";
                        isValid = false;
                    }
                }
                if (txtStandardCompliaintSCBAQuantity.Text == "")
                {
                    errorMessage += "SCBA Quantity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintSCBAQuantity.Text) < 1)
                    {
                        errorMessage += "SCBA Quantity must be greater than 0.<br />";
                        isValid = false;
                    }
                }

                if (txtStandardCompliaintSCBAYear.Text == "")
                {
                    errorMessage += "SCBA Year is Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (Convert.ToInt32(txtStandardCompliaintSCBAYear.Text) < 1)
                    {
                        errorMessage += "SCBA Year must be greater than 0.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_StandardSCBA> standardSCBAs = new List<FG_App_StandardSCBA>();
                if (ViewState["dtSCBA"] != null)
                {
                    standardSCBAs = (List<FG_App_StandardSCBA>)ViewState["dtSCBA"];
                }

                FG_App_StandardSCBA standardSCBA = new FG_App_StandardSCBA();

                if (hfStandardCompliaintSCBAId.Value != "")
                {
                    for (int i = 0; i < standardSCBAs.Count; i++)
                    {
                        if (standardSCBAs[i].StandardComplientSCBAId.ToString() == hfStandardCompliaintSCBAId.Value.ToString())
                        {
                            standardSCBA = standardSCBAs[i];
                            standardSCBAs.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (standardSCBA.StandardComplientSCBAId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    standardSCBA.StandardComplientSCBAId = Guid.NewGuid();
                }

                standardSCBA.Age = Convert.ToInt32(txtStandardCompliaintSCBAAge.Text);
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                standardSCBA.ApplicationId = appId;
                standardSCBA.SCBAType = ddlSCBAType.Text;
                standardSCBA.Quantity = Convert.ToInt32(txtStandardCompliaintSCBAQuantity.Text);
                standardSCBA.Year = Convert.ToInt32(txtStandardCompliaintSCBAYear.Text);
                standardSCBA.Condition = ddlStandardCompliaintSCBACondition.SelectedValue;
                standardSCBAs.Add(standardSCBA);
                ViewState["dtSCBA"] = standardSCBAs;
                rgStandardComplientSCBA.DataSource = standardSCBAs;
                rgStandardComplientSCBA.DataBind();
                txtStandardCompliaintSCBAAge.Text = "";
                txtStandardCompliaintSCBAYear.Text = "";
                txtStandardCompliaintSCBAQuantity.Text = "";
                ddlStandardCompliaintSCBACondition.SelectedIndex = 0;
                hfStandardCompliaintSCBAId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + standardSCBA.Condition + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblStandardCompliaintSCBAError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openStandardCompliantSCBAModal();", true);
            }
        }

        private async Task LoadApplicationDocuments(Guid applicationId)
        {
            string[] allTypes = PpeDocumentTypes.Concat(ScbaDocumentTypes).ToArray();
            List<FG_AppDocListItem> allDocs =
                await fgAppService.GetApplicationDocumentsByTypesAsync(applicationId, allTypes);

            List<FG_AppDocListItem> ppeDocs = allDocs
                .Where(d => PpeDocumentTypes.Contains(d.DocumentType))
                .ToList();
            List<FG_AppDocListItem> scbaDocs = allDocs
                .Where(d => ScbaDocumentTypes.Contains(d.DocumentType))
                .ToList();

            ViewState[ViewStatePpeDocuments] = ppeDocs;
            ViewState[ViewStateScbaDocuments] = scbaDocs;
            rgPPEDocuments.DataSource = ppeDocs;
            rgPPEDocuments.DataBind();
            rgSCBADocuments.DataSource = scbaDocs;
            rgSCBADocuments.DataBind();
        }

        private List<FG_AppDocListItem> GetDocumentListFromViewState(string viewStateKey)
        {
            if (ViewState[viewStateKey] != null)
            {
                return (List<FG_AppDocListItem>)ViewState[viewStateKey];
            }
            return new List<FG_AppDocListItem>();
        }

        private async Task UploadDocumentAsync(
            FileUpload upload,
            string documentType,
            HtmlGenericControl errorDiv,
            string[] sectionDocumentTypes,
            string viewStateKey,
            RadGrid grid)
        {
            errorDiv.InnerHtml = string.Empty;

            if (string.IsNullOrWhiteSpace(documentType))
            {
                throw new Exception("Document Type is required.<br />");
            }
            if (!upload.HasFile)
            {
                throw new Exception("You must select a file to upload.<br />");
            }

            HttpPostedFile file = upload.PostedFile;
            string fileName = Path.GetFileName(file.FileName);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new Exception("You must select a file to upload.<br />");
            }
            if (fileName.Length > MaxDocumentNameLength)
            {
                throw new Exception(
                    "File name must be " + MaxDocumentNameLength + " characters or less. " +
                    "Please rename the file and try again.<br />");
            }

            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new Exception(
                    "File type not allowed. Allowed types: .xls, .xlsx, .csv, .pdf, .doc, .docx<br />");
            }

            if (file.ContentLength > 10000000)
            {
                throw new Exception("File size must be 10MB or less.<br />");
            }

            byte[] fileData = new byte[file.ContentLength];
            int bytesRead = file.InputStream.Read(fileData, 0, file.ContentLength);
            if (bytesRead != file.ContentLength)
            {
                throw new Exception("Unable to read the uploaded file.<br />");
            }

            Guid appId = new Guid(hfApplicationId.Value);
            FG_App_Documents doc = new FG_App_Documents();
            doc.DocumentId = Guid.NewGuid();
            doc.ApplicationId = appId;
            doc.DocumentType = documentType;
            doc.DocumentName = fileName;
            doc.Document = fileData;
            doc.DocType = extension;

            bool saved = await fgAppService.SaveApplicationDocumentAsync(doc);
            if (!saved)
            {
                throw new Exception(
                    "An error occurred saving " + fileName +
                    ". Please try again or use a shorter file name.<br />");
            }

            List<FG_AppDocListItem> docs =
                await fgAppService.GetApplicationDocumentsByTypesAsync(appId, sectionDocumentTypes);
            ViewState[viewStateKey] = docs;
            grid.DataSource = docs;
            grid.DataBind();

            errorDiv.InnerHtml =
                "<div class='alert alert-success'>" + doc.DocumentName + " has been added.</div>";
        }

        protected void rgPPEDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgPPEDocuments.DataSource = GetDocumentListFromViewState(ViewStatePpeDocuments);
        }

        protected void rgPPEDocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            rgPPEDocuments.DataSource = GetDocumentListFromViewState(ViewStatePpeDocuments);
            rgPPEDocuments.DataBind();
        }

        protected void rgPPEDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            HideDocumentActionColumnsIfReadOnly(e);
        }

        protected async void rgPPEDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            await HandleDocumentGridCommand(
                e, ViewStatePpeDocuments, rgPPEDocuments, dvPPEDocumentError, SectionPpe);
        }

        protected void rgSCBADocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgSCBADocuments.DataSource = GetDocumentListFromViewState(ViewStateScbaDocuments);
        }

        protected void rgSCBADocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            rgSCBADocuments.DataSource = GetDocumentListFromViewState(ViewStateScbaDocuments);
            rgSCBADocuments.DataBind();
        }

        protected void rgSCBADocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            HideDocumentActionColumnsIfReadOnly(e);
        }

        protected async void rgSCBADocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            await HandleDocumentGridCommand(
                e, ViewStateScbaDocuments, rgSCBADocuments, dvSCBADocumentError, SectionScba);
        }

        private void HideDocumentActionColumnsIfReadOnly(GridItemEventArgs e)
        {
            if (Session["ReadOnly"] == null || !Convert.ToBoolean(Session["ReadOnly"]))
            {
                return;
            }
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton btnEditName = item.FindControl("btnEditName") as LinkButton;
                LinkButton btnRemove = item.FindControl("btnRemove") as LinkButton;
                if (btnEditName != null) { btnEditName.Visible = false; }
                if (btnRemove != null) { btnRemove.Visible = false; }
            }
        }

        private async Task HandleDocumentGridCommand(
            GridCommandEventArgs e,
            string viewStateKey,
            RadGrid grid,
            HtmlGenericControl errorDiv,
            string section)
        {
            try
            {
                errorDiv.InnerHtml = string.Empty;
                if (!(e.Item is GridDataItem))
                {
                    return;
                }

                string pId = e.CommandArgument.ToString();
                Guid docId = new Guid(pId);

                if (e.CommandName == "Delete")
                {
                    bool deleted = await fgAppService.DeleteApplicationDocumentAsync(docId);
                    if (deleted)
                    {
                        string docName = "";
                        List<FG_AppDocListItem> docs = GetDocumentListFromViewState(viewStateKey);
                        for (int i = 0; i < docs.Count; i++)
                        {
                            if (docs[i].DocumentId.ToString() == pId)
                            {
                                docName = docs[i].DocumentName;
                                docs.RemoveAt(i);
                                break;
                            }
                        }
                        ViewState[viewStateKey] = docs;
                        grid.DataSource = docs;
                        grid.DataBind();
                        errorDiv.InnerHtml =
                            "<div class='alert alert-success'>" + docName + " has been removed.</div>";
                    }
                    else
                    {
                        errorDiv.InnerHtml =
                            "<div class='alert alert-danger'>An error occurred removing the document.</div>";
                    }
                }
                else if (e.CommandName == "View")
                {
                    await ViewDocumentAsync(pId, errorDiv);
                }
                else if (e.CommandName == "Download")
                {
                    await DownloadDocumentAsync(pId, errorDiv);
                }
                else if (e.CommandName == "EditName")
                {
                    List<FG_AppDocListItem> docs = GetDocumentListFromViewState(viewStateKey);
                    FG_AppDocListItem docItem = docs.FirstOrDefault(d => d.DocumentId == docId);
                    if (docItem != null)
                    {
                        hfEditDocumentId.Value = docId.ToString();
                        hfEditDocumentSection.Value = section;
                        txtEditDocumentName.Text = docItem.DocumentName;
                        lblEditDocumentNameError.Text = "";
                        System.Web.UI.ScriptManager.RegisterStartupScript(
                            this, this.GetType(), "EditDocName", "openEditDocumentNameModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                errorDiv.InnerHtml = "<div class='alert alert-danger'>" + ex.Message + "</div>";
            }
        }

        protected async void btnSaveDocumentName_Click(object sender, EventArgs e)
        {
            HtmlGenericControl errorDiv = dvPPEDocumentError;
            string viewStateKey = ViewStatePpeDocuments;
            RadGrid grid = rgPPEDocuments;
            string[] sectionTypes = PpeDocumentTypes;

            if (hfEditDocumentSection.Value == SectionScba)
            {
                errorDiv = dvSCBADocumentError;
                viewStateKey = ViewStateScbaDocuments;
                grid = rgSCBADocuments;
                sectionTypes = ScbaDocumentTypes;
            }

            try
            {
                lblEditDocumentNameError.Text = "";
                if (string.IsNullOrWhiteSpace(txtEditDocumentName.Text))
                {
                    throw new Exception("Document Name is required.");
                }

                Guid docId = new Guid(hfEditDocumentId.Value);
                FG_App_Documents doc = await fgAppService.GetApplicationDocumentByIdAsync(docId);
                if (doc == null)
                {
                    throw new Exception("Document not found.");
                }

                doc.DocumentName = txtEditDocumentName.Text.Trim();
                bool saved = await fgAppService.SaveApplicationDocumentAsync(doc);
                if (!saved)
                {
                    throw new Exception("An error occurred saving the document name.");
                }

                Guid appId = new Guid(hfApplicationId.Value);
                List<FG_AppDocListItem> docs =
                    await fgAppService.GetApplicationDocumentsByTypesAsync(appId, sectionTypes);
                ViewState[viewStateKey] = docs;
                grid.DataSource = docs;
                grid.DataBind();

                errorDiv.InnerHtml =
                    "<div class='alert alert-success'>Document name has been updated.</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "CloseEditDoc", "$('#editDocumentNameModal').modal('hide');", true);
            }
            catch (Exception ex)
            {
                _ = ex;
                lblEditDocumentNameError.Text =
                    "<div class='alert alert-danger'>" + ex.Message + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "EditDocName", "openEditDocumentNameModal();", true);
            }
        }

        private async Task ViewDocumentAsync(string docId, HtmlGenericControl errorDiv)
        {
            Guid id = new Guid(docId);
            FG_App_Documents doc = await fgAppService.GetApplicationDocumentByIdAsync(id);
            if (doc == null)
            {
                throw new Exception("Document not found.");
            }

            byte[] bytes = doc.Document;
            string fileName = doc.DocumentName;
            string extension = GetExtension(fileName).ToLowerInvariant();
            byte[] renderedBytes = bytes;

            if (Regex.IsMatch(extension, @"\.(docx|rtf|html|txt|pdf)$"))
            {
                if (Regex.IsMatch(extension, @"\.(docx|rtf|html|txt)$"))
                {
                    IFormatProvider<RadFlowDocument> provider = null;
                    MemoryStream stream = new MemoryStream(bytes);
                    switch (extension)
                    {
                        case ".docx": provider = new DocxFormatProvider(); break;
                        case ".rtf": provider = new RtfFormatProvider(); break;
                        case ".html": provider = new HtmlFormatProvider(); break;
                        case ".txt": provider = new TxtFormatProvider(); break;
                    }
                    if (provider != null)
                    {
                        RadFlowDocument document = provider.Import(stream);
                        Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider =
                            new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            pdfProvider.Export(document, ms);
                            renderedBytes = ms.ToArray();
                        }
                    }
                }
                pdfView.PdfjsProcessingSettings.FileSettings.Data =
                    Convert.ToBase64String(renderedBytes);
                System.Web.UI.ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Pop", "openDocModal();", true);
            }
            else
            {
                errorDiv.InnerHtml =
                    "<div class='alert alert-info'>Preview is not available for this file type. " +
                    "Please use Download instead.</div>";
            }
        }

        private async Task DownloadDocumentAsync(string docId, HtmlGenericControl errorDiv)
        {
            Guid id = new Guid(docId);
            FG_App_Documents doc = await fgAppService.GetApplicationDocumentByIdAsync(id);
            if (doc == null)
            {
                throw new Exception("Document not found.");
            }

            byte[] bytes = doc.Document;
            string fileName = doc.DocumentName;
            string extension = GetExtension(fileName).ToLowerInvariant();
            string contentType = GetContentType(extension);

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

        private static string GetContentType(string extension)
        {
            switch (extension)
            {
                case ".doc":
                    return "application/vnd.ms-word";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".csv":
                    return "text/csv";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        private static string GetExtension(string path)
        {
            var ret = "";
            for (; ; )
            {
                var ext = Path.GetExtension(path);
                if (String.IsNullOrEmpty(ext))
                {
                    break;
                }
                path = path.Substring(0, path.Length - ext.Length);
                ret = ext + ret;
            }
            return ret;
        }
    }
}






