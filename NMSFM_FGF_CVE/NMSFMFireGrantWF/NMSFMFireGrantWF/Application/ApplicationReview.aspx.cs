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
                    if (Session["Role"].ToString() != "Internal")
                    {
                        Response.Redirect("~/Unauthorized");
                    }
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
                        //Added 5/12/2023 VWD
                        Guid appIdGuid = new Guid(appId);
                        short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                        FGApplications existingApp = new FGApplications();
                        existingApp = await fgAppService.GetFGApplicationById(appIdGuid);
                        if (existingApp != null)
                        {
                            if (existingApp.AppStatus == 5 || existingApp.AppStatus == 4)
                            {
                                hfAppReadOnly.Value = "false";
                                HtmlGenericControl spReadOnly = new HtmlGenericControl();
                                spReadOnly = (HtmlGenericControl)Master.FindControl(id: "spReadOnly");
                                if (spReadOnly != null)
                                {
                                    spReadOnly.InnerHtml = "";
                                }
                            }
                            else
                            {
                                hfAppReadOnly.Value = "true";
                            }
                        }

                        if (fiscalYear < 2024)
                        {
                            tblSingleScores.Visible = true;
                            DetailedFGAppScores scores = new DetailedFGAppScores();
                            scores = await fgAppService.GetDetailedFGAppScoresAsync(appIdGuid);
                            if (scores != null)
                            {
                                tdISORating.InnerHtml = scores.ISORating.ToString();
                                tdTraining.InnerHtml = scores.TrainingPoints.ToString();
                                tdFinancialNeed.InnerHtml = scores.FinancialNeedGrade.ToString();
                                tdBenefit.InnerHtml = scores.BenefitGrade.ToString();
                                tdConsequences.InnerHtml = scores.ConsequencesGrade.ToString();
                                tdProblem.InnerHtml = scores.ProblemGrade.ToString();
                                tdCompleteness.InnerHtml = scores.AppCompletenessGrade.ToString();
                                tdTotal.InnerHtml = scores.TotalScore.ToString();
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                            {
                                tblSingleScores.Visible = true;
                                string strWebUserId = Session["WebUserId"].ToString();
                                Guid webUserId = new Guid(strWebUserId);
                                DetailedFGAppScores scores = new DetailedFGAppScores();
                                scores = await fgAppService.GetDetailedFGAppScoresCounselorAsync(appIdGuid, webUserId);
                                tdISORating.InnerHtml = scores.ISORating.ToString();
                                tdTraining.InnerHtml = scores.TrainingPoints.ToString();
                                tdFinancialNeed.InnerHtml = scores.FinancialNeedGrade.ToString();
                                tdBenefit.InnerHtml = scores.BenefitGrade.ToString();
                                tdConsequences.InnerHtml = scores.ConsequencesGrade.ToString();
                                tdProblem.InnerHtml = scores.ProblemGrade.ToString();
                                tdCompleteness.InnerHtml = scores.AppCompletenessGrade.ToString();
                                tdTotal.InnerHtml = scores.TotalScore.ToString();
                            }
                            else
                            {
                                tblSingleScores.Visible = false;
                                //ToDo Load Multiple Scores
                                List<DetailedFGAppScores> appScores = new List<DetailedFGAppScores>();
                                appScores = await fgAppService.GetDetailedFGAppScoresAdminAsync(appIdGuid);
                                if (appScores.Count > 0)
                                {
                                    string appScoreTable = "<table  class='table table-bordered' style='width: 100%'>";
                                    appScoreTable += "<tr><th scope='col'>Counselor</th><th scope='col'>Training Grade</th>";
                                    appScoreTable += "<th scope='col'>Financial Need Grade</th><th scope='col'>Problem Grade</th><th scope='col'>Benefit Grade</th>";
                                    appScoreTable += "<th scope='col'>Consequences Grade</th><th scope='col'>Completeness Grade</th><th scope='col'>Total Grade</th></tr>";
                                    double numberScores = Convert.ToDouble(appScores.Count);
                                    double trainingGrade = 0;
                                    double financialNeedGrade = 0;
                                    double problemGrade = 0;
                                    double benefitGrade = 0;
                                    double consequencesGrade = 0;
                                    double completenessGrade = 0;
                                    foreach (DetailedFGAppScores score in appScores)
                                    {
                                        trainingGrade += score.TrainingPoints;
                                        financialNeedGrade += score.FinancialNeedGrade;
                                        problemGrade += score.AppCompletenessGrade;
                                        benefitGrade += score.BenefitGrade;
                                        consequencesGrade += score.ConsequencesGrade;
                                        completenessGrade += score.AppCompletenessGrade;
                                        appScoreTable += "<tr><th scope='row'>" + score.UserName + "</th><td>" + score.TrainingPoints.ToString() + "</td>";
                                        appScoreTable += "<td>" + score.FinancialNeedGrade.ToString() + "</td>";
                                        appScoreTable += "<td>" + score.ProblemGrade.ToString() + "</td><td>" + score.BenefitGrade.ToString() + "</td>";
                                        appScoreTable += "<td>" + score.ConsequencesGrade.ToString() + "</td><td>" + score.AppCompletenessGrade.ToString() + "</td>";
                                        //Update 12/22/2023 (vwd) add total column
                                        appScoreTable += "<td>" + score.TotalScore.ToString() + "</td>";
                                        appScoreTable += "</tr>";
                                    }
                                    //Update 12/22/2023 (vwd) remove average column
                                    //trainingGrade = trainingGrade / numberScores;
                                    //financialNeedGrade = financialNeedGrade / numberScores;
                                    //problemGrade = problemGrade / numberScores;
                                    //benefitGrade = benefitGrade / numberScores;
                                    //consequencesGrade = consequencesGrade / numberScores;
                                    //completenessGrade = completenessGrade / numberScores;
                                    //appScoreTable += "<tr><th scope='row'>Average</th><td>" + appScores[0].ISORating.ToString() + "</td><td>" + trainingGrade.ToString() + "</td>";
                                    //appScoreTable += "<td>" + financialNeedGrade.ToString() + "</td><td>" + benefitGrade.ToString() + "</td>";
                                    //appScoreTable += "<td>" + problemGrade.ToString() + "</td>";
                                    //appScoreTable += "<td>" + consequencesGrade.ToString() + "</td><td>" + completenessGrade.ToString() + "</td>";
                                    //appScoreTable += "</tr>";
                                    appScoreTable += "</table>";
                                    ltrMultiScores.Text = appScoreTable;
                                }
                                
                            }
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
                        //DisableControls(this);
                    }
                    
                    await LoadSettings();
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }

        }

        private async Task<bool> LoadSettings()
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
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
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
                //if (model.NERISCompliant ==1) { rbNERISYes.Checked = true; }
                //if (model.NERISCompliant == 2) { rbNERISNo.Checked = true; }

                //if (model.PumpTestCompliant == 1) { rbPumpTestsYes.Checked = true; }
                //if (model.PumpTestCompliant == 2) { rbPumpTestsNo.Checked = true; }

                //if (model.HoseTestCompliant == 1) { rbHoseTestsYes.Checked = true; }
                //if (model.HoseTestCompliant == 2) { rbHoseTestsNo.Checked = true; }

                //if (model.AckComSigs == 1) { rbSignaturesYes.Checked = true; }
                //if (model.AckComSigs == 2) { rbSignaturesNo.Checked = true; }

                txtNotes.Text = model.Notes;


                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                string strWebUserId = Session["WebUserId"].ToString();
                Guid webUserId = new Guid(strWebUserId);
                if (fiscalYear < 2024)
                {
                    if (model.AppSignatures.Count > 0)
                    {
                        model.ReviewerSignature = model.AppSignatures.FirstOrDefault();
                        if (model.ReviewerSignature != null)
                        {
                            hfSignatureId.Value = model.ReviewerSignature.SignatureId.ToString();
                            chkAgreement.Checked = true;
                            txtReviewer.Text = model.ReviewerSignature.PrintedName;
                            txtSignature.Text = model.ReviewerSignature.Signature;
                            txtDate.SelectedDate = Convert.ToDateTime(model.ReviewerSignature.DateSigned);
                        }
                    }
                }
                else
                {
                    if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                    {
                        dvSignature.Visible = true;
                        dvAgreement.Visible = true;
                        dvAdminSignatureTable.Visible = false;
                        model.ReviewerSignature = model.AppSignatures.FirstOrDefault(a => a.WebUserId == webUserId);
                        if (model.ReviewerSignature != null)
                        {
                            hfSignatureId.Value = model.ReviewerSignature.SignatureId.ToString();
                            chkAgreement.Checked = true;
                            txtReviewer.Text = model.ReviewerSignature.PrintedName;
                            txtSignature.Text = model.ReviewerSignature.Signature;
                            txtDate.SelectedDate = Convert.ToDateTime(model.ReviewerSignature.DateSigned);
                            //added 12/21/23 (vwd)
                            chkAgreement.Enabled = false;
                            txtReviewer.ReadOnly = true;
                            txtSignature.ReadOnly = true;
                            txtDate.Enabled = false;
                            //end Add
                        }
                    }
                    else
                    {
                        dvSignature.Visible = false;
                        dvAgreement.Visible = false;
                        dvAdminSignatureTable.Visible = true;
                        rgSignatures.DataSource = model.AppSignatures;
                        ViewState["dtSignatures"] = model.AppSignatures;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
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
                //if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                //{
                //    return true;
                //}
                //ToDo Check Validation
                if (Convert.ToBoolean(hfAppReadOnly.Value) == true)
                {
                    return true;
                }
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                
                //int nfirsCompliant = 0;
                //if (rbNERISNo.Checked) { nfirsCompliant = 2; }
                //if (rbNERISYes.Checked) { nfirsCompliant = 1; }
                //if (nfirsCompliant == 0)
                //{
                //    errorMessage += "NERIS Compliant selection required.<br />";
                //    isValid = false;
                //}

                //int pumpTestCompliant = 0;
                //if (rbPumpTestsNo.Checked) { pumpTestCompliant = 2; }
                //if (rbPumpTestsYes.Checked) { pumpTestCompliant = 1; }
                //if (pumpTestCompliant == 0)
                //{
                //    errorMessage += "Pump Test Compliant selection required.<br />";
                //    isValid = false;
                //}

                //int hoseTestCompliant = 0;
                //if (rbHoseTestsNo.Checked) { hoseTestCompliant = 2; }
                //if (rbHoseTestsYes.Checked) { hoseTestCompliant = 1; }
                //if (hoseTestCompliant == 0)
                //{
                //    errorMessage += "Hose Test Compliant selection required.<br />";
                //    isValid = false;
                //}

                //int ackSigComp = 0;
                //if (rbSignaturesNo.Checked) { ackSigComp = 2; }
                //if (rbSignaturesYes.Checked) { ackSigComp = 1; }
                //if (ackSigComp == 0)
                //{
                //    errorMessage += "Acknowledgement/Commitment Signatures selection required.<br />";
                //    isValid = false;
                //}

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
                        DateTime sigDate;
                        try
                        {
                            sigDate = Convert.ToDateTime(txtDate.SelectedDate);
                        }
                        catch
                        {
                            throw new Exception("When signing please enter the reviewer name, signature, date and check the agreement checkbox.");
                        }
                        if (sigDate < DateTime.Now.AddDays(-7) || sigDate > DateTime.Now.AddDays(7))
                        {
                            throw new Exception("Date signed cannot be less than or greater than 7 days from current date.");
                        }
                    }
                    if (chkAgreement.Checked == false)
                    {
                        throw new Exception("Please enter a valid date signed.");
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
                //model.NERISCompliant = nfirsCompliant;
                //model.PumpTestCompliant = pumpTestCompliant;
                //model.HoseTestCompliant = hoseTestCompliant;
                //model.AckComSigs = ackSigComp;
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
                    signature.SignatureRole = "Counselor";
                    signature.SignedBy = Session["WebUser"].ToString();
                    signature.WebUserId = (Session["WebUser"] != null) ? new Guid(Session["WebUserId"].ToString()) : new Guid("00000000-0000-0000-0000-000000000000");
                    model.ReviewerSignature = signature;
                }

                bool retVal = await fgAppService.SaveApplicationReviewAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void rgSignatures_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
            rgSignatures.DataSource = signatures;
        }

        protected void rgSignatures_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
            rgSignatures.DataSource = signatures;
            rgSignatures.DataBind();
        }

        //Added 12/21/2023 (vwd)
        protected void chkAgreement_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAgreement.Checked)
                {
                    if (txtReviewer.Text == "") { txtReviewer.Text = Session["CodepalUserName"].ToString(); }
                    if (txtSignature.Text == "") { txtSignature.Text = Session["CodepalUserName"].ToString(); }
                    if (txtDate.SelectedDate.ToString() == "") { txtDate.SelectedDate = DateTime.Now; }
                }
                else
                {
                    txtReviewer.Text = "";
                    txtSignature.Text = "";
                    txtDate.SelectedDate = null;
                }
                chkAgreement.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }
    }
}






