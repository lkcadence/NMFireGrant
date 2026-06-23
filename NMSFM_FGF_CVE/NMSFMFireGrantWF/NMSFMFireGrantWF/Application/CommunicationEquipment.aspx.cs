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

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
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
            else if (con is RadGrid)
            {
                RadGrid g = (RadGrid)con;
                g.Columns[0].Visible = false;
            }
            btnSave.Visible = false;
            dvShowModal.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
                btnSave.Visible = true;
            }
        }

        private void LoadCommunication(DetailedFGCommunication model, bool listOnly = false)
        {
            try
            {
                if (model.IsValid == false)
                {
                    if (model.InvalidText != null)
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                    }
                    else
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>No Data Saved</div>";
                    }
                }
                if (listOnly == false)
                {
                    if (model.CommunicationProject == 1) { rbCommunicationsYes.Checked = true; }
                    if (model.CommunicationProject == 2) { rbCommunicationsNo.Checked = true; }

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

                if (communicationPart == 2)
                {
                    noRadio = 0;
                    lawEnforcement = 0;
                    emeergencyMedical = 0;
                    otherFD = 0;
                    other = 0;
                    notCovered = 0;
                    txtHandheldRadios.Text = "0";
                    txtBaseStations.Text = "0";
                    txtMobileRadios.Text = "0";
                    rbAppNoRadioYes.Checked = false;
                    rbAppNoRadioNo.Checked = false;
                    rbLawEnforcementYes.Checked = false;
                    rbLawEnforcementNo.Checked = false;
                    rbEmergencyMedicalYes.Checked = false;
                    rbEmergencyMedicalNo.Checked = false;
                    rbOtherFDYes.Checked = false;
                    rbOtherFDNo.Checked = false;
                    rbOtherYes.Checked = false;
                    rbOtherNo.Checked = false;
                    rbNotCoveredYes.Checked = false;
                    rbNotCoveredNo.Checked = false;
                    txtOtherDescription.Text = "";
                    txtRepeaterDescription.Text = "";
                    communicationEquipment = new List<FG_App_CommunicationEquipment>();
                    ViewState["dtCommunicationEquipment"] = communicationEquipment;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new DetailedFGCommunication();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
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

                return retVal;
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
    }
}






