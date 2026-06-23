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
    public partial class Apparatus : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

        RadMenu _rmStep1;

        private const string DocTypeApparatusDocument = "Apparatus File Upload";
        private const string ViewStateApparatusDocuments = "dtApparatusDocuments";
        private const string SectionApparatus = "APPARATUS";
        private static readonly string[] ApparatusDocumentTypes = { DocTypeApparatusDocument };
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
                    //if (Session["Role"].ToString() == "Internal")
                    //{
                    //    dvNotification.Visible = false;
                    //}
                    //else if (Session["Role"].ToString() == "External" || Session["Role"].ToString() == "Signator")
                    //{
                    //    dvNotification.Visible = true;
                    //}
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Apparatus (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Apparatus";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);
                //InitTestSources();

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
                    short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                    await LoadStatutes(fiscalYear);
                    string appId = hfApplicationId.Value;
                    if (!string.IsNullOrEmpty(appId))
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGApparatus apparatus = new DetailedFGApparatus();
                        apparatus = await fgAppService.GetFGApplicationApparatusAsync(appIdGuid);
                        if (apparatus != null && apparatus.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadApparatusInfo(apparatus);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        //added 12/23/23 (vwd) load preexisting apparatus
                        else
                        {
                            Guid addressId = new Guid(Session["Department"].ToString());
                            apparatus = await fgAppService.GetPriorFGApplicationApparatusAsync(addressId, appIdGuid);
                            if (apparatus != null && apparatus.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                            {
                                LoadApparatusInfo(apparatus, true);
                                if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                                {
                                    dvError.InnerHtml = Session["SaveMessage"].ToString();
                                    Session["SaveMessage"] = "";
                                }
                                else
                                {
                                    dvError.InnerHtml = "Information Loaded from Previous Application";
                                }
                            }
                        }
                        await LoadApplicationDocuments(appIdGuid);
                    }
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                    }
                }

            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
            
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
                .Replace("You must list Apparatus", "Apparatus list or File Upload is required")
                .Replace("Apparatus list is required", "Apparatus list or File Upload is required");
        }

        private async Task HandlePendingFileUploadAsync()
        {
            string action = hfUploadAction.Value;
            if (string.IsNullOrEmpty(action) || action != SectionApparatus)
            {
                return;
            }

            try
            {
                if (!fuApparatusDocumentation.HasFile)
                {
                    dvApparatusDocumentError.InnerHtml =
                        "<div class='alert alert-danger'>No file was received. Please try again.</div>";
                    return;
                }

                await UploadDocumentAsync(
                    fuApparatusDocumentation,
                    DocTypeApparatusDocument,
                    dvApparatusDocumentError,
                    ApparatusDocumentTypes,
                    ViewStateApparatusDocuments,
                    rgApparatusDocuments);

                dvError.InnerHtml = string.Empty;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvApparatusDocumentError.InnerHtml =
                    "<div class='alert alert-danger'>" + ex.Message + "</div>";
            }
            finally
            {
                hfUploadAction.Value = string.Empty;
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
            else if (con is FileUpload)
            {
                con.Visible = false;
            }
            else if (con is RadGrid)
            {
                RadGrid g = (RadGrid)con;
                g.Columns[0].Visible = false;
            }
            btnSave.Visible = false;
            btnUploadApparatusDocuments.Visible = false;
            dvShowModal.Visible = false;
        }

        private async Task<bool> LoadStatutes(short fYear)
        {
            try
            {
                bool loaded = true;
                FGApplicationSettings result = null;
                result = await fgService.GetFireGrantAppSettings(fYear);
                if (result != null)
                {
                    if (result.PumpTestStatute != null)
                    {
                        ltrPumpTestStatute.Text = result.PumpTestStatute;
                    }
                    else
                    {
                        ltrPumpTestStatute.Text = "Please enter Pump Test Statute in Admin area";
                    }
                    if (result.HoseTestStatute != null)
                    {
                        ltrHoseTestStatute.Text = result.HoseTestStatute;
                    }
                    else
                    {
                        ltrHoseTestStatute.Text = "Please enter Hose Test Statute in Admin area";
                    }
                }
                else
                {
                    loaded = false;
                }
                return loaded;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
        }

        private void LoadApparatusInfo(DetailedFGApparatus model, bool apparatusOnly = false)
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
                if (apparatusOnly == false)
                {
                    if (model.ApparatusPartOfProject == 1) { rbApparatusPartYes.Checked = true; }
                    if (model.ApparatusPartOfProject == 2) { rbApparatusPartNo.Checked = true; }
                    if (model.PumpTestsConducted == 1) { rbPumpTestsConductedYes.Checked = true; }
                    if (model.PumpTestsConducted == 2) { rbPumpTestsConductedNo.Checked = true; }
                    txtNoPumpTestsExp.Text = model.ExplainNoPumpTests.ToString();
                    if (model.HoseTestConducted == 1) { rbHoseTestsYes.Checked = true; }
                    if (model.HoseTestConducted == 2) { rbHoseTestsNo.Checked = true; }
                    txtNoHoseTests.Text = model.ExplainNoHostTests.ToString();
                }
                

                rgApparatus.DataSource = model.ApparatusEquipment;
                ViewState["dtApparatusEquipment"] = model.ApparatusEquipment;
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
                        //Response.Redirect("~/Application/Apparatus", false);
                        break;
                    case "Communication Equipment":
                        Response.Redirect("~/Application/CommunicationEquipment", false);
                        break;
                    case "Hazards/Threats":
                        Response.Redirect("~/Application/HazardsThreats", false);
                        break;
                    case "PPE":
                        Response.Redirect("~/Application/PPE", false);
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
            rbApparatusPartYes.Checked = true;
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("ApparatusName", typeof(string));
            cats.Columns.Add("VIN", typeof(string));
            cats.Columns.Add("License", typeof(string));
            cats.Columns.Add("GPM", typeof(string));
            cats.Columns.Add("TestDate", typeof(string));
            cats.Columns.Add("Pass", typeof(string));
            cats.Columns.Add("Comments", typeof(string));
            cats.Columns.Add("ApparatusId", typeof(string));

            for (int i = 1; i < 2; i++)
            {
                string source = "Test Apparatus " + i.ToString();
                string vin = "Test VIN Number " + i.ToString();
                string license = "Test License Number " + i.ToString();
                string gpm = (i * 1001).ToString();
                string testdate = "1/1/2021";
                string pass = "Pass";
                string comments = "Test Comments " + i.ToString();
                string apparatusId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), source, vin, license, gpm, testdate, pass, comments, apparatusId);
            }

            ViewState["dtApparatus"] = cats;
            rgApparatus.DataSource = cats;
            rgApparatus.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/Training", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Apparatus Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Apparatus Data Saved</div>";
                Response.Redirect("~/Application/Apparatus", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/CommunicationEquipment", false);
            }
        }

        private async Task<bool> SaveForm()
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                int apparatusPart = 0;
                if (rbApparatusPartYes.Checked) { apparatusPart = 1; }
                if (rbApparatusPartNo.Checked) { apparatusPart = 2; }
                if (apparatusPart == 0)
                {
                    errorMessage += "Apparatus Part of Project answer is Required.<br />";
                    isValid = false;
                }
                int pumpTests = 0;
                if (rbPumpTestsConductedYes.Checked) { pumpTests = 1; }
                if (rbPumpTestsConductedNo.Checked) { pumpTests = 2; }
                if (apparatusPart == 1 && pumpTests == 0)
                {
                    errorMessage += "Pump Test Answer is Required.<br />";
                    isValid = false;
                }
                if (apparatusPart == 1 && pumpTests == 2)
                {
                    if (txtNoPumpTestsExp.Text == "")
                    {
                        errorMessage += "If pump tests have not been conducted please explain why they have not been conducted.<br />";
                        isValid = false;
                    }
                }

                int hoseTests = 0;
                if (rbHoseTestsYes.Checked) { hoseTests = 1; }
                if (rbHoseTestsNo.Checked) { hoseTests = 2; }
                if (apparatusPart == 1 && hoseTests == 0)
                {
                    errorMessage += "Hose Test Answer is Required.<br />";
                    isValid = false;
                }
                if (apparatusPart == 1 && hoseTests == 2)
                {
                    if (txtNoHoseTests.Text == "")
                    {
                        errorMessage += "If hose tests have not been conducted please explain why they have not been conducted.<br />";
                        isValid = false;
                    }
                }

                List<FG_App_ApparatusEquipment> apparatusEquipment = new List<FG_App_ApparatusEquipment>();
                if (ViewState["dtApparatusEquipment"] != null)
                {
                    apparatusEquipment = (List<FG_App_ApparatusEquipment>)ViewState["dtApparatusEquipment"];
                }

                if (apparatusPart == 2)
                {
                    pumpTests = 0;
                    hoseTests = 0;
                    rbPumpTestsConductedYes.Checked = false;
                    rbPumpTestsConductedNo.Checked = false;
                    rbHoseTestsYes.Checked = false;
                    rbHoseTestsNo.Checked = false;
                    txtNoPumpTestsExp.Text = "";
                    txtNoHoseTests.Text = "";
                    apparatusEquipment = new List<FG_App_ApparatusEquipment>();
                    ViewState["dtApparatusEquipment"] = apparatusEquipment;
                }

                if (apparatusPart == 1)
                {
                    Guid appId = new Guid(hfApplicationId.Value);
                    List<FG_AppDocListItem> apparatusDocuments =
                        await fgAppService.GetApplicationDocumentsByTypesAsync(
                            appId, ApparatusDocumentTypes);
                    bool hasApparatusList = apparatusEquipment != null && apparatusEquipment.Count >= 1;
                    bool hasDocuments = apparatusDocuments != null && apparatusDocuments.Count >= 1;
                    if (!hasApparatusList && !hasDocuments)
                    {
                        errorMessage += "Apparatus list or File Upload is required<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    errorMessage = NormalizeInvalidText(errorMessage);
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new DetailedFGApparatus();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = NormalizeInvalidText(errorMessage);
                model.UpdatedBy = Session["WebUser"].ToString();
                model.ApparatusPartOfProject = apparatusPart;
                model.PumpTestsConducted = pumpTests;
                model.ExplainNoPumpTests = txtNoPumpTestsExp.Text;
                model.HoseTestConducted = hoseTests;
                model.ExplainNoHostTests = txtNoHoseTests.Text;
                model.ApparatusEquipment = apparatusEquipment;

                bool retVal = await fgAppService.SaveApparatusAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgApparatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_ApparatusEquipment> apparatusEquipment = (List<FG_App_ApparatusEquipment>)ViewState["dtApparatusEquipment"];
            rgApparatus.DataSource = apparatusEquipment;
        }

        protected void rgApparatus_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            //List<FG_App_ApparatusEquipment> apparatusEquipment = (List<FG_App_ApparatusEquipment>)ViewState["dtApparatusEquipment"];
            //rgApparatus.DataSource = apparatusEquipment;
            //rgApparatus.DataBind();
        }

        protected void rgApparatus_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    if (dataItem["Pass"].Text == "True")
                    {
                        dataItem["Pass"].Text = "Pass";
                    }
                    else if (dataItem["Pass"].Text == "False")
                    {
                        dataItem["Pass"].Text = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                
            }
        }

        protected void rgApparatus_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    
                    string number = dataItem["Number"].Text;
                    string name = dataItem["ApparatusName"].Text;
                    string vehicleType = dataItem["VehicleType"].Text;
                    string year = dataItem["Year"].Text;
                    string vin = dataItem["VIN"].Text;
                    //string license = dataItem["License"].Text;
                    string capacity = dataItem["Capacity"].Text;
                    string gpm = dataItem["GPM"].Text;
                    string testdate = dataItem["TestDate"].Text;
                    string pass = dataItem["Pass"].Text;
                    string comment = (dataItem["Comments"].Text == "&nbsp;") ? "" : dataItem["Comments"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openApparatusModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfApparatusId.Value = pId;
                        //txtNumber.Text = number;
                        txtApparatusName.Text = name;
                        ddlVehicleType.SelectedValue = vehicleType;
                        txtYear.Text = year;
                        txtVIN.Text = vin;
                        //txtLicense.Text = license;
                        txtCapacity.Text = capacity;
                        txtGPM.Text = gpm;
                        if (testdate != "" && testdate != "&nbsp;")
                        {
                            txtTestDate.Text = Convert.ToDateTime(testdate).ToString("yyyy-MM-dd");
                        }
                        if (pass == "&nbsp;")
                        {
                            rbNA.Checked = true;
                        }
                        else if (pass == "True" || pass == "Pass")
                        {
                            rbPass.Checked = true;
                        }
                        else
                        {
                            rbFail.Checked = true;
                        }
                        txtComments.Text = comment;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteApparatus_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_ApparatusEquipment> apparatusEquipment = (List<FG_App_ApparatusEquipment>)ViewState["dtApparatusEquipment"];
                for (int i = 0; i < apparatusEquipment.Count; i++)
                {
                    if (apparatusEquipment[i].ApparatusId.ToString() == hfApparatusId.Value.ToString())
                    {
                        apparatusEquipment.RemoveAt(i);
                        break;
                    }
                }
                int num = 1;
                foreach (FG_App_ApparatusEquipment item in apparatusEquipment)
                {
                    item.Number = num;
                    num += 1;
                }
                apparatusEquipment = apparatusEquipment.OrderBy(a => a.Number).ToList();
                ViewState["dtApparatusEquipment"] = apparatusEquipment;
                rgApparatus.DataSource = apparatusEquipment;
                rgApparatus.DataBind();
                //txtNumber.Text = "";
                txtApparatusName.Text = "";
                ddlVehicleType.SelectedIndex = 0;
                txtYear.Text = "";
                txtVIN.Text = "";
                //txtLicense.Text = "";
                txtCapacity.Text = "";
                txtGPM.Text = "";
                txtTestDate.Text = "";
                rbPass.Checked = false;
                rbFail.Checked = false;
                txtComments.Text = "";
                hfApparatusId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveApparatus_ServerClick(object sender, EventArgs e)
        {
            try 
            { 
                lblApparatusError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtApparatusName.Text == "")
                {
                    errorMessage += "Apparatus Name is Required.<br />";
                    isValid = false;
                }
                if (ddlVehicleType.SelectedIndex < 1)
                {
                    errorMessage += "Vehicle Type is Required.<br />";
                    isValid = false;
                }

                if (txtYear.Text == "")
                {
                    errorMessage += "Year is Required.<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        txtYear.Text = Convert.ToInt32(txtYear.Text).ToString();
                        if (Convert.ToInt32(txtYear.Text) < 1060)
                        {
                            errorMessage += "Year must be 1960 or greater.<br />";
                            isValid = false;
                        }
                    }
                    catch
                    {
                        errorMessage += "Year must be numeric.<br />";
                        isValid = false;
                    }
                }

                if (txtCapacity.Text == "")
                {
                    errorMessage += "Capacity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        txtCapacity.Text = Convert.ToInt32(txtCapacity.Text).ToString();
                        if (Convert.ToInt32(txtCapacity.Text) < 0)
                        {
                            errorMessage += "Capacity must be 0 or greater.<br />";
                            isValid = false;
                        }
                    }
                    catch
                    {
                        errorMessage += "Capacity must be numeric.<br />";
                        isValid = false;
                    }
                }

                if (txtGPM.Text == "")
                {
                    errorMessage += "GPM is Required.<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        txtGPM.Text = Convert.ToInt32(txtGPM.Text).ToString();
                        if (Convert.ToInt32(txtGPM.Text) < 0)
                        {
                            errorMessage += "GPM must be 0 or greater.<br />";
                            isValid = false;
                        }
                        else if (Convert.ToInt32(txtGPM.Text) > 0)
                        {
                            if (txtTestDate.Text == "")
                            {
                                errorMessage += "Test Date is Required.<br />";
                                isValid = false;
                            }
                        }
                    }
                    catch
                    {
                        errorMessage += "GPM must be numeric.<br />";
                        isValid = false;
                    }
                }
                //if (txtNumber.Text == "")
                //{
                //    errorMessage += "Apparatus Equipment Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtNumber.Text) < 1)
                //    {
                //        errorMessage += "Apparatus Equipment Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}
                

                bool? passfail = null;
                if (rbPass.Checked) { passfail = true; }
                if (rbFail.Checked) { passfail = false; }
                if (rbPass.Checked == false && rbFail.Checked == false && rbNA.Checked == false)
                {
                    errorMessage += "Pass/Fail/NA is required<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_ApparatusEquipment> apparatusEquipment = new List<FG_App_ApparatusEquipment>();
                if (ViewState["dtApparatusEquipment"] != null)
                {
                    apparatusEquipment = (List<FG_App_ApparatusEquipment>)ViewState["dtApparatusEquipment"];
                }

                FG_App_ApparatusEquipment apparatus = new FG_App_ApparatusEquipment();

                if (hfApparatusId.Value != "")
                {
                    for (int i = 0; i < apparatusEquipment.Count; i++)
                    {
                        if (apparatusEquipment[i].ApparatusId.ToString() == hfApparatusId.Value.ToString())
                        {
                            apparatus = apparatusEquipment[i];
                            apparatusEquipment.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (apparatus.ApparatusId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    apparatus.ApparatusId = Guid.NewGuid();
                }

                apparatus.Number = apparatusEquipment.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                apparatus.ApplicationId = appId;
                apparatus.ApparatusName = txtApparatusName.Text;
                apparatus.VehicleType = ddlVehicleType.SelectedValue;
                apparatus.Year = Convert.ToInt32(txtYear.Text);
                apparatus.VIN = txtVIN.Text;
                apparatus.License = "";
                apparatus.Capacity = Convert.ToInt32(txtCapacity.Text);
                apparatus.GPM = Convert.ToInt32(txtGPM.Text);
                if (txtTestDate.Text != "")
                {
                    apparatus.TestDate = Convert.ToDateTime(txtTestDate.Text);
                }
                else
                {
                    apparatus.TestDate = null;
                }    
                apparatus.Pass = passfail;
                apparatus.Comments = txtComments.Text;

                apparatusEquipment.Add(apparatus);
                apparatusEquipment = apparatusEquipment.OrderBy(a => a.Number).ToList();
                ViewState["dtApparatusEquipment"] = apparatusEquipment;
                rgApparatus.DataSource = apparatusEquipment;
                rgApparatus.DataBind();
                //txtNumber.Text = "";
                txtApparatusName.Text = "";
                ddlVehicleType.SelectedIndex = 0;
                txtYear.Text = "";
                txtVIN.Text = "";
                //txtLicense.Text = "";
                txtCapacity.Text = "";
                txtGPM.Text = "";
                txtTestDate.Text = "";
                rbPass.Checked = false;
                rbFail.Checked = false;
                txtComments.Text = "";
                hfApparatusId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + apparatus.ApparatusName + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblApparatusError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openApparatusModal();", true);
            }
        }

        private async Task LoadApplicationDocuments(Guid applicationId)
        {
            List<FG_AppDocListItem> docs =
                await fgAppService.GetApplicationDocumentsByTypesAsync(
                    applicationId, ApparatusDocumentTypes);
            ViewState[ViewStateApparatusDocuments] = docs;
            rgApparatusDocuments.DataSource = docs;
            rgApparatusDocuments.DataBind();
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

        protected void rgApparatusDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgApparatusDocuments.DataSource = GetDocumentListFromViewState(ViewStateApparatusDocuments);
        }

        protected void rgApparatusDocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            rgApparatusDocuments.DataSource = GetDocumentListFromViewState(ViewStateApparatusDocuments);
            rgApparatusDocuments.DataBind();
        }

        protected void rgApparatusDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            HideDocumentActionColumnsIfReadOnly(e);
        }

        protected async void rgApparatusDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            await HandleDocumentGridCommand(e);
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

        private async Task HandleDocumentGridCommand(GridCommandEventArgs e)
        {
            try
            {
                dvApparatusDocumentError.InnerHtml = string.Empty;
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
                        List<FG_AppDocListItem> docs =
                            GetDocumentListFromViewState(ViewStateApparatusDocuments);
                        for (int i = 0; i < docs.Count; i++)
                        {
                            if (docs[i].DocumentId.ToString() == pId)
                            {
                                docName = docs[i].DocumentName;
                                docs.RemoveAt(i);
                                break;
                            }
                        }
                        ViewState[ViewStateApparatusDocuments] = docs;
                        rgApparatusDocuments.DataSource = docs;
                        rgApparatusDocuments.DataBind();
                        dvApparatusDocumentError.InnerHtml =
                            "<div class='alert alert-success'>" + docName + " has been removed.</div>";
                    }
                    else
                    {
                        dvApparatusDocumentError.InnerHtml =
                            "<div class='alert alert-danger'>An error occurred removing the document.</div>";
                    }
                }
                else if (e.CommandName == "View")
                {
                    await ViewDocumentAsync(pId);
                }
                else if (e.CommandName == "Download")
                {
                    await DownloadDocumentAsync(pId);
                }
                else if (e.CommandName == "EditName")
                {
                    List<FG_AppDocListItem> docs =
                        GetDocumentListFromViewState(ViewStateApparatusDocuments);
                    FG_AppDocListItem docItem = docs.FirstOrDefault(d => d.DocumentId == docId);
                    if (docItem != null)
                    {
                        hfEditDocumentId.Value = docId.ToString();
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
                dvApparatusDocumentError.InnerHtml =
                    "<div class='alert alert-danger'>" + ex.Message + "</div>";
            }
        }

        protected async void btnSaveDocumentName_Click(object sender, EventArgs e)
        {
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
                    await fgAppService.GetApplicationDocumentsByTypesAsync(
                        appId, ApparatusDocumentTypes);
                ViewState[ViewStateApparatusDocuments] = docs;
                rgApparatusDocuments.DataSource = docs;
                rgApparatusDocuments.DataBind();

                dvApparatusDocumentError.InnerHtml =
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

        private async Task ViewDocumentAsync(string docId)
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
                dvApparatusDocumentError.InnerHtml =
                    "<div class='alert alert-info'>Preview is not available for this file type. " +
                    "Please use Download instead.</div>";
            }
        }

        private async Task DownloadDocumentAsync(string docId)
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






