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
    public partial class ApplicationReview : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
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
                    if (Session["Role"].ToString() != "Internal")
                    {
                        Response.Redirect("~/Unauthorized");
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Application Review (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Application Review";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppScores scores = new DetailedFGAppScores();
                        scores = await fgAppService.GetDetailedFGAppScoresAsync(appIdGuid);
                        if (scores != null)
                        {
                            tdTraining.InnerHtml = scores.TrainingPoints.ToString();
                            tdFinancialNeed.InnerHtml = scores.FinancialNeedGrade.ToString();
                            tdBenefit.InnerHtml = scores.BenefitGrade.ToString();
                            tdConsequences.InnerHtml = scores.ConsequencesGrade.ToString();
                            tdProblem.InnerHtml = scores.ProblemGrade.ToString();
                            tdCompleteness.InnerHtml = scores.AppCompletenessGrade.ToString();
                            tdTotal.InnerHtml = scores.TotalScore.ToString();
                        }
                        DetailedFGAppReview review = new DetailedFGAppReview();
                        review = await fgAppService.GetFGApplicationReviewAsync(appIdGuid);
                        if (review != null)
                        {
                            LoadReview(review);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Application not found.");
                    }
                    //if (Session["Role"].ToString() == "External")
                    //{
                    //    DisableControls(this);
                    //    dvAdmin.Visible = false;
                    //}
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                    }
                    
                    LoadSettings();
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }

        }

        private async void LoadSettings()
        {
            try
            {
                FGApplicationSettings result = null;
                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                result = await fgService.GetFireGrantAppSettings(fiscalYear);
                if (result != null && result.eSignatureLegalText != null)
                {
                    dvESig.InnerHtml = result.eSignatureLegalText;
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
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
            if (con is RadMaskedTextBox)
            {
                RadMaskedTextBox t = (RadMaskedTextBox)con;
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
                DropDownList t = (DropDownList)con;
                t.Enabled = false;
            }
            btnSave.Visible = false;
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

        private void LoadReview(DetailedFGAppReview model)
        {
            try
            {
                if (model.NFIRSCompliant ==1) { rbNFIRSYes.Checked = true; }
                if (model.NFIRSCompliant == 2) { rbNFIRSNo.Checked = true; }

                if (model.PumpTestCompliant == 1) { rbPumpTestsYes.Checked = true; }
                if (model.PumpTestCompliant == 2) { rbPumpTestsNo.Checked = true; }

                if (model.AckComSigs == 1) { rbSignaturesYes.Checked = true; }
                if (model.AckComSigs == 2) { rbSignaturesNo.Checked = true; }

                txtNotes.Text = model.Notes;

                if (model.ReviewerSignature != null)
                {
                    hfSignatureId.Value = model.ReviewerSignature.SignatureId.ToString();
                    chkAgreement.Checked = true;
                    txtReviewer.Text = model.ReviewerSignature.PrintedName;
                    txtSignature.Text = model.ReviewerSignature.Signature;
                    txtDate.SelectedDate = Convert.ToDateTime(model.ReviewerSignature.DateSigned);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Session["SaveMessage"] = "<div class='alert alert-success'>Application Review Saved</div>";
                Response.Redirect("~/Application/ApplicationReview", false);
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
                
                int nfirsCompliant = 0;
                if (rbNFIRSNo.Checked) { nfirsCompliant = 2; }
                if (rbNFIRSYes.Checked) { nfirsCompliant = 1; }
                if (nfirsCompliant == 0)
                {
                    errorMessage += "NFIRS Compliant selection required.<br />";
                    isValid = false;
                }

                int pumpTestCompliant = 0;
                if (rbPumpTestsNo.Checked) { pumpTestCompliant = 2; }
                if (rbPumpTestsYes.Checked) { pumpTestCompliant = 1; }
                if (pumpTestCompliant == 0)
                {
                    errorMessage += "Pump Test Compliant selection required.<br />";
                    isValid = false;
                }

                int ackSigComp = 0;
                if (rbSignaturesNo.Checked) { ackSigComp = 2; }
                if (rbSignaturesYes.Checked) { ackSigComp = 1; }
                if (ackSigComp == 0)
                {
                    errorMessage += "Acknowledgement/Commitment Signatures selection required.<br />";
                    isValid = false;
                }

                if (txtReviewer.Text.Trim() != "" || txtSignature.Text.Trim() != "" || txtDate.SelectedDate != null)
                {
                    if (txtReviewer.Text.Trim() == "")
                    {
                        throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                    }
                    if (txtSignature.Text.Trim() == "")
                    {
                        throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                    }
                    if (txtDate.SelectedDate == null)
                    {
                        throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                    }
                    else
                    {
                        try
                        {
                            DateTime sigDate = Convert.ToDateTime(txtDate.SelectedDate);
                            if (sigDate < DateTime.Now.AddDays(-7) || sigDate > DateTime.Now.AddDays(7))
                            {
                                throw new Exception("Date signed cannot be less than or greater than 7 days from current date.");
                            }
                        }
                        catch
                        {
                            throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                        }
                    }
                    if (chkAgreement.Checked == false)
                    {
                        throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                    }
                }
                else
                {
                    errorMessage += "Reviewer Signature is needed.<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new DetailedFGAppReview();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.NFIRSCompliant = nfirsCompliant;
                model.PumpTestCompliant = pumpTestCompliant;
                model.AckComSigs = ackSigComp;
                model.Notes = txtNotes.Text;

                if (txtSignature.Text.Trim() != "")
                {
                    FG_App_Signatures signature = new FG_App_Signatures();
                    if (hfSignatureId.Value != "")
                    {
                        signature.SignatureId = new Guid(hfSignatureId.Value);
                    }
                    signature.ApplicationId = model.ApplicationId;
                    signature.DateEntered = DateTime.Now;
                    signature.DateSigned = Convert.ToDateTime(txtDate.SelectedDate);
                    signature.EnteredBy = Session["WebUser"].ToString();
                    signature.FromReview = true;
                    signature.FromStatus = false;
                    signature.PrintedName = txtReviewer.Text;
                    signature.Signature = txtSignature.Text;
                    signature.SignatureRole = "Reviewer";
                    model.ReviewerSignature = signature;
                }

                bool retVal = await fgAppService.SaveApplicationReviewAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }
    }
}