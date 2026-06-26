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
    public partial class CommunicationEquipment : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
// private IAccountService accountService; // legacy field, currently unused
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

        RadMenu _rmStep1;

        private const string DocTypeCommunicationDocument = "Communication Equipment File Upload";
        private const string ViewStateCommunicationDocuments = "dtCommunicationDocuments";
        private const string SectionCommunication = "COMMUNICATION";
        private static readonly string[] CommunicationDocumentTypes = { DocTypeCommunicationDocument };
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Communication Equipment (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Communication Equipment";
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
                        DetailedFGCommunication communication = new DetailedFGCommunication();
                        communication = await fgAppService.GetFGApplicationCommunicationAsync(appIdGuid);
                        if (communication != null && communication.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadCommunication(communication);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        //added 12/26/23 (vwd) load preexisting info
                        else
                        {
                            Guid addressId = new Guid(Session["Department"].ToString());
                            communication = await fgAppService.GetFGApplicationPriorYearCommunicationAsync(addressId, appIdGuid);
                            if (communication != null && communication.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                            {
                                PrefillChildRowRemap.RemapCommunicationEquipment(
                                    communication.CommunicationEquipment, appIdGuid);
                                LoadCommunication(communication, true);
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
                .Replace(
                    "Communication Equipment list is required",
                    "Communication Equipment list or File Upload is required")
                .Replace(
                    "You must list Communication Equipment",
                    "Communication Equipment list or File Upload is required");
        }

        private async Task HandlePendingFileUploadAsync()
        {
            string action = hfUploadAction.Value;
            if (string.IsNullOrEmpty(action) || action != SectionCommunication)
            {
                return;
            }

            try
            {
                if (!fuCommunicationDocumentation.HasFile)
                {
                    dvCommunicationDocumentError.InnerHtml =
                        "<div class='alert alert-danger'>No file was received. Please try again.</div>";
                    return;
                }

                await UploadDocumentAsync(
                    fuCommunicationDocumentation,
                    DocTypeCommunicationDocument,
                    dvCommunicationDocumentError,
                    CommunicationDocumentTypes,
                    ViewStateCommunicationDocuments,
                    rgCommunicationDocuments);

                dvError.InnerHtml = string.Empty;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvCommunicationDocumentError.InnerHtml =
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
            btnUploadCommunicationDocuments.Visible = false;
            dvShowModal.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
                btnSave.Visible = true;
            }
        }

        private void ApplyCommunicationPartOfProjectGate(DetailedFGCommunication model)
        {
            int part = model.CommunicationProject;
            if (part == 0
                && model.CommunicationEquipment != null
                && model.CommunicationEquipment.Count > 0)
            {
                part = 1;
            }
            rbCommunicationsYes.Checked = part == 1;
            rbCommunicationsNo.Checked = part == 2;
        }

        private void LoadCommunication(DetailedFGCommunication model, bool listOnly = false)
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
                ApplyCommunicationPartOfProjectGate(model);
                if (listOnly == false)
                {
                    txtHandheldRadios.Text = model.HandheldRadios.ToString();
                    txtBaseStations.Text = model.BaseStations.ToString();
                    txtMobileRadios.Text = model.MobileRadios.ToString();

                    if (model.ApparatusWoRadio == 1) { rbAppNoRadioYes.Checked = true; }
                    if (model.ApparatusWoRadio == 2) { rbAppNoRadioNo.Checked = true; }

                    if (model.LawEnforcement == 1) { rbLawEnforcementYes.Checked = true; }
                    if (model.LawEnforcement == 2) { rbLawEnforcementNo.Checked = true; }

                    if (model.EmergencyMedical == 1) { rbEmergencyMedicalYes.Checked = true; }
                    if (model.EmergencyMedical == 2) { rbEmergencyMedicalNo.Checked = true; }

                    if (model.OtherFireDepts == 1) { rbOtherFDYes.Checked = true; }
                    if (model.OtherFireDepts == 2) { rbOtherFDNo.Checked = true; }

                    if (model.Other == 1) { rbOtherYes.Checked = true; }
                    if (model.Other == 2) { rbOtherNo.Checked = true; }

                    txtOtherDescription.Text = model.OtherDescription;

                    if (model.AreasNotCovered == 1) { rbNotCoveredYes.Checked = true; }
                    if (model.AreasNotCovered == 2) { rbNotCoveredNo.Checked = true; }

                    txtRepeaterDescription.Text = model.DescribeAreasNotCovered;

                    txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";
                }
                
                rgCommunicationEquipment.DataSource = model.CommunicationEquipment;
                ViewState["dtCommunicationEquipment"] = model.CommunicationEquipment;
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
                        //Response.Redirect("~/Application/CommunicationEquipment", false);
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
            rbCommunicationsYes.Checked = true;
            DataTable cats = new DataTable();
            cats.Columns.Add("Number", typeof(string));
            cats.Columns.Add("CommunicationEquipment", typeof(string));
            cats.Columns.Add("CommunicationQty", typeof(string));
            cats.Columns.Add("CommunicationEquipmentId", typeof(string));

            for (int i = 1; i < 5; i++)
            {
                string source = "Communication Equipment " + i.ToString();
                string qty = (i * 2).ToString();
                string comId = Guid.NewGuid().ToString();
                cats.Rows.Add(i.ToString(), source, qty, comId);
            }

            ViewState["dtCommunicationEquipment"] = cats;
            rgCommunicationEquipment.DataSource = cats;
            rgCommunicationEquipment.DataBind();
        }

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/Apparatus", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Communication Equipment Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Communication Equipment Data Saved</div>";
                Response.Redirect("~/Application/CommunicationEquipment", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/PPE", false);
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
                int communicationPart = 0;
                if (rbCommunicationsYes.Checked) { communicationPart = 1; }
                if (rbCommunicationsNo.Checked) { communicationPart = 2; }
                if (communicationPart == 0)
                {
                    errorMessage += "Communications is part of the project answer is Required.<br />";
                    isValid = false;
                }

                if (communicationPart == 1 && txtHandheldRadios.Text == "")
                {
                    errorMessage += "Number of handheld radios is Required.<br />";
                    isValid = false;
                }

                if (communicationPart == 1 && txtBaseStations.Text == "")
                {
                    errorMessage += "Number of base stations is Required.<br />";
                    isValid = false;
                }

                if (communicationPart == 1 && txtMobileRadios.Text == "")
                {
                    errorMessage += "Number of mobile radios is Required.<br />";
                    isValid = false;
                }

                int noRadio = 0;
                if (rbAppNoRadioYes.Checked) { noRadio = 1; }
                if (rbAppNoRadioNo.Checked) { noRadio = 2; }
                if (communicationPart == 1 && noRadio == 0)
                {
                    errorMessage += "Do you have any apparatus without a mobile radio is required.<br />";
                    isValid = false;
                }

                int lawEnforcement = 0;
                if (rbLawEnforcementYes.Checked) { lawEnforcement = 1; }
                if (rbLawEnforcementNo.Checked) { lawEnforcement = 2; }
                if (communicationPart == 1 && lawEnforcement == 0)
                {
                    errorMessage += "Law Enforcement Interoperability answer is required.<br />";
                    isValid = false;
                }

                int emeergencyMedical = 0;
                if (rbEmergencyMedicalYes.Checked) { emeergencyMedical = 1; }
                if (rbEmergencyMedicalNo.Checked) { emeergencyMedical = 2; }
                if (communicationPart == 1 && emeergencyMedical == 0)
                {
                    errorMessage += "Emergency Medical Interoperability answer is required.<br />";
                    isValid = false;
                }

                int otherFD = 0;
                if (rbOtherFDYes.Checked) { otherFD = 1; }
                if (rbOtherFDNo.Checked) { otherFD = 2; }
                if (communicationPart == 1 && otherFD == 0)
                {
                    errorMessage += "Other Fire Department Interoperability answer is required.<br />";
                    isValid = false;
                }

                int other = 0;
                if (rbOtherYes.Checked) { other = 1; }
                if (rbOtherNo.Checked) { other = 2; }
                if (communicationPart == 1 && other == 0)
                {
                    errorMessage += "Other Agency Interoperability answer is required.<br />";
                    isValid = false;
                }
                if (communicationPart == 1 && other == 1)
                {
                    if (txtOtherDescription.Text == "")
                    {
                        errorMessage += "Other Agency Interoperability Description is Required.<br />";
                        isValid = false;
                    }
                }

                int notCovered = 0;
                if (rbNotCoveredYes.Checked) { notCovered = 1; }
                if (rbNotCoveredNo.Checked) { notCovered = 2; }
                if (communicationPart == 1 && notCovered == 0)
                {
                    errorMessage += "Emergency Medical Interoperability answer is required.<br />";
                    isValid = false;
                }
                if (communicationPart == 1 && notCovered == 1)
                {
                    if (txtRepeaterDescription.Text == "")
                    {
                        errorMessage += "Areas Not Covered By a Repeater Description is Required.<br />";
                        isValid = false;
                    }
                }

                List<FG_App_CommunicationEquipment> communicationEquipment =
                    (List<FG_App_CommunicationEquipment>)ViewState["dtCommunicationEquipment"];
                if (communicationEquipment == null)
                {
                    communicationEquipment = new List<FG_App_CommunicationEquipment>();
                }

                // Disabled: preserve section data when PartOfProject = No
                //if (communicationPart == 2)
                //{
                //    noRadio = 0;
                //    lawEnforcement = 0;
                //    emeergencyMedical = 0;
                //    otherFD = 0;
                //    other = 0;
                //    notCovered = 0;
                //    txtHandheldRadios.Text = "0";
                //    txtBaseStations.Text = "0";
                //    txtMobileRadios.Text = "0";
                //    rbAppNoRadioYes.Checked = false;
                //    rbAppNoRadioNo.Checked = false;
                //    rbLawEnforcementYes.Checked = false;
                //    rbLawEnforcementNo.Checked = false;
                //    rbEmergencyMedicalYes.Checked = false;
                //    rbEmergencyMedicalNo.Checked = false;
                //    rbOtherFDYes.Checked = false;
                //    rbOtherFDNo.Checked = false;
                //    rbOtherYes.Checked = false;
                //    rbOtherNo.Checked = false;
                //    rbNotCoveredYes.Checked = false;
                //    rbNotCoveredNo.Checked = false;
                //    txtOtherDescription.Text = "";
                //    txtRepeaterDescription.Text = "";
                //    communicationEquipment = new List<FG_App_CommunicationEquipment>();
                //    ViewState["dtCommunicationEquipment"] = communicationEquipment;
                //}
                if (communicationPart == 1)
                {
                    Guid appId = new Guid(hfApplicationId.Value);
                    List<FG_AppDocListItem> communicationDocuments =
                        await fgAppService.GetApplicationDocumentsByTypesAsync(
                            appId, CommunicationDocumentTypes);
                    bool hasEquipmentList = communicationEquipment != null && communicationEquipment.Count >= 1;
                    bool hasDocuments = communicationDocuments != null && communicationDocuments.Count >= 1;
                    if (!hasEquipmentList && !hasDocuments)
                    {
                        errorMessage += "Communication Equipment list or File Upload is required<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    errorMessage = NormalizeInvalidText(errorMessage);
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new DetailedFGCommunication();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = NormalizeInvalidText(errorMessage);
                model.UpdatedBy = Session["WebUser"].ToString();
                model.CommunicationProject = communicationPart;
                model.HandheldRadios = Convert.ToInt32(txtHandheldRadios.DbValue);
                model.BaseStations = Convert.ToInt32(txtBaseStations.DbValue);
                model.MobileRadios = Convert.ToInt32(txtMobileRadios.DbValue);
                model.ApparatusWoRadio = noRadio;
                model.LawEnforcement = lawEnforcement;
                model.EmergencyMedical = emeergencyMedical;
                model.OtherFireDepts = otherFD;
                model.Other = other;
                model.OtherDescription = txtOtherDescription.Text;
                model.AreasNotCovered = notCovered;
                model.DescribeAreasNotCovered = txtRepeaterDescription.Text;
                model.AdminComments = txtComments.Text;
                model.CommunicationEquipment = communicationEquipment;

                bool retVal = await fgAppService.SaveCommunicationAsync(model);

                // return retVal;
                return isValid && retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgCommunicationEquipment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<FG_App_CommunicationEquipment> communicationEquipment = (List<FG_App_CommunicationEquipment>)ViewState["dtCommunicationEquipment"];
            rgCommunicationEquipment.DataSource = communicationEquipment;
        }

        protected void rgCommunicationEquipment_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            List<FG_App_CommunicationEquipment> communicationEquipment = (List<FG_App_CommunicationEquipment>)ViewState["dtCommunicationEquipment"];
            rgCommunicationEquipment.DataSource = communicationEquipment;
            rgCommunicationEquipment.DataBind();
        }

        protected void rgCommunicationEquipment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgCommunicationEquipment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string name = "";
                    name = dataItem["CommunicationEquipment"].Text;
                    string number = dataItem["Number"].Text;
                    string qty = dataItem["CommunicationQty"].Text;
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openCommunicationModal();", true);
                        string pId = e.CommandArgument.ToString();
                        hfCommunicationId.Value = pId;
                        txtCommunicationEquipment.Text = name;
                        //txtCommunicationNumber.Text = number;
                        txtCommunicationQty.Text = qty;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnDeleteCommunication_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_CommunicationEquipment> communicationEquipment = (List<FG_App_CommunicationEquipment>)ViewState["dtCommunicationEquipment"];
                for (int i = 0; i < communicationEquipment.Count; i++)
                {
                    if (communicationEquipment[i].CommunicationEquipmentId.ToString() == hfCommunicationId.Value.ToString())
                    {
                        communicationEquipment.RemoveAt(i);
                        break;
                    }
                }
                int num = 1;
                foreach (FG_App_CommunicationEquipment item in communicationEquipment)
                {
                    item.Number = num;
                    num += 1;
                }
                ViewState["dtCommunicationEquipment"] = communicationEquipment;
                rgCommunicationEquipment.DataSource = communicationEquipment;
                rgCommunicationEquipment.DataBind();
                txtCommunicationEquipment.Text = "";
                //txtCommunicationNumber.Text = "";
                txtCommunicationQty.Text = "";
                hfCommunicationId.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void btnSaveCommunication_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblCommunicationError.Text = "";
                string errorMessage = "";
                bool isValid = true;
                if (txtCommunicationEquipment.Text == "")
                {
                    errorMessage += "Communication Equipment is Required.<br />";
                    isValid = false;
                }
                if (txtCommunicationQty.Text == "")
                {
                    errorMessage += "Communication Quantity is Required.<br />";
                    isValid = false;
                }
                else
                {
                    try
                    {
                        txtCommunicationQty.Text = Convert.ToInt32(txtCommunicationQty.Text).ToString();
                        if (Convert.ToInt32(txtCommunicationQty.Text) < 0)
                        {
                            errorMessage += "Communication Quantity must be greater than 0.<br />";
                            isValid = false;
                        }
                    }
                    catch
                    {
                        errorMessage += "Communication Quantity must be numeric.<br />";
                        isValid = false;
                    }
                }
                //if (txtCommunicationNumber.Text == "")
                //{
                //    errorMessage += "Communication Number is Required.<br />";
                //    isValid = false;
                //}
                //else
                //{
                //    if (Convert.ToInt32(txtCommunicationNumber.Text) < 1)
                //    {
                //        errorMessage += "Communication Number must be greater than 0.<br />";
                //        isValid = false;
                //    }
                //}

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }


                List<FG_App_CommunicationEquipment> communicationEquipment = new List<FG_App_CommunicationEquipment>();
                if (ViewState["dtCommunicationEquipment"] != null)
                {
                    communicationEquipment = (List<FG_App_CommunicationEquipment>)ViewState["dtCommunicationEquipment"];
                }

                FG_App_CommunicationEquipment communicationEquip = new FG_App_CommunicationEquipment();

                if (hfCommunicationId.Value != "")
                {
                    for (int i = 0; i < communicationEquipment.Count; i++)
                    {
                        if (communicationEquipment[i].CommunicationEquipmentId.ToString() == hfCommunicationId.Value.ToString())
                        {
                            communicationEquip = communicationEquipment[i];
                            communicationEquipment.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (communicationEquip.CommunicationEquipmentId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    communicationEquip.CommunicationEquipmentId = Guid.NewGuid();
                }

                communicationEquip.Number = communicationEquipment.Count + 1;
                Guid appId = new Guid(hfApplicationId.Value.ToString());
                communicationEquip.ApplicationId = appId;
                communicationEquip.CommunicationEquipment = txtCommunicationEquipment.Text;
                communicationEquip.CommunicationQty = Convert.ToInt32(txtCommunicationQty.Text);
                communicationEquipment.Add(communicationEquip);
                ViewState["dtCommunicationEquipment"] = communicationEquipment;
                rgCommunicationEquipment.DataSource = communicationEquipment;
                rgCommunicationEquipment.DataBind();
                txtCommunicationEquipment.Text = "";
                //txtCommunicationNumber.Text = "";
                txtCommunicationQty.Text = "";
                hfCommunicationId.Value = "";
                dvError.InnerHtml = "<div class='alert alert-success'>" + communicationEquip.CommunicationEquipment + " has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblCommunicationError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openCommunicationModal();", true);
            }
        }

        private async Task LoadApplicationDocuments(Guid applicationId)
        {
            List<FG_AppDocListItem> docs =
                await fgAppService.GetApplicationDocumentsByTypesAsync(
                    applicationId, CommunicationDocumentTypes);
            ViewState[ViewStateCommunicationDocuments] = docs;
            rgCommunicationDocuments.DataSource = docs;
            rgCommunicationDocuments.DataBind();
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

        protected void rgCommunicationDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgCommunicationDocuments.DataSource =
                GetDocumentListFromViewState(ViewStateCommunicationDocuments);
        }

        protected void rgCommunicationDocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            rgCommunicationDocuments.DataSource =
                GetDocumentListFromViewState(ViewStateCommunicationDocuments);
            rgCommunicationDocuments.DataBind();
        }

        protected void rgCommunicationDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            HideDocumentActionColumnsIfReadOnly(e);
        }

        protected async void rgCommunicationDocuments_ItemCommand(object sender, GridCommandEventArgs e)
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
                dvCommunicationDocumentError.InnerHtml = string.Empty;
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
                            GetDocumentListFromViewState(ViewStateCommunicationDocuments);
                        for (int i = 0; i < docs.Count; i++)
                        {
                            if (docs[i].DocumentId.ToString() == pId)
                            {
                                docName = docs[i].DocumentName;
                                docs.RemoveAt(i);
                                break;
                            }
                        }
                        ViewState[ViewStateCommunicationDocuments] = docs;
                        rgCommunicationDocuments.DataSource = docs;
                        rgCommunicationDocuments.DataBind();
                        dvCommunicationDocumentError.InnerHtml =
                            "<div class='alert alert-success'>" + docName + " has been removed.</div>";
                    }
                    else
                    {
                        dvCommunicationDocumentError.InnerHtml =
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
                        GetDocumentListFromViewState(ViewStateCommunicationDocuments);
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
                dvCommunicationDocumentError.InnerHtml =
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
                        appId, CommunicationDocumentTypes);
                ViewState[ViewStateCommunicationDocuments] = docs;
                rgCommunicationDocuments.DataSource = docs;
                rgCommunicationDocuments.DataBind();

                dvCommunicationDocumentError.InnerHtml =
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
                dvCommunicationDocumentError.InnerHtml =
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






