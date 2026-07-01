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
using System.Configuration;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;

namespace NMSFMFireGrantWF.Application
{
    public partial class SignaturesDocs : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;
        private Emailer emailer;
        // private MemoryStream _stream; // legacy field, currently unused

        RadMenu _rmStep1;

        protected void Page_Init(object sender, EventArgs e)
        {
            logger = new Logging();
            var userWebModel = new UserWebModel();
accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
                this.emailer = new Emailer();
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
                        btnSave.Text = "Save";
                        if (Session["IsWebAdmin"] != null && Convert.ToBoolean(Session["IsWebAdmin"]) == true)
                        {
                            txtAppCompleteness.ReadOnly = true;
                        }
                        //txtSignatureRole.Text = "Applicant";
                    }
                    else if (Session["Role"].ToString() == "External" || Session["Role"].ToString() == "Signator")
                    {
                        dvAdmin.Visible = false;
                        //txtSignatureRole.Text = "Administrator";
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Signatures Documentation (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Signatures and Supporting Documentation";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                string appId = Session["ApplicationId"].ToString();

                if (!Page.IsPostBack)
                {
                    await LoadSettings();
                    hfApplicationId.Value = appId;
                    txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppSigsDocs docsSigs = new DetailedFGAppSigsDocs();
                        docsSigs = await fgAppService.GetFGApplicationDocsSigsAsync(appIdGuid);
                        if (docsSigs != null && docsSigs.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            await LoadDocsSigs(docsSigs);
                            await LoadDetailedAppValidation(appIdGuid);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        if (Session["DepartmentName"] != null)
                        {
                            hfDeptName.Value = Session["DepartmentName"].ToString();
                        }
                    }
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
                        if (Session["Role"].ToString() == "Signator")
                        {
                            LoadSignator();
                        }
                    } 
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
                    if (result.faCertifiationText != null && result.faCertifiationText != "")
                    {
                        hfFASigCert.Value = result.faCertifiationText;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
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
            else if (con is RadGrid)
            {
                RadGrid g = (RadGrid)con;
                g.Columns[0].Visible = false;
            }
            else if (con is DropDownList)
            {
                DropDownList ddl = (DropDownList)con;
                ddl.Enabled = false;
            }
            else if (con is RadAsyncUpload)
            {
                RadAsyncUpload upl = (RadAsyncUpload)con;
                con.Visible = false;
            }
            btnSave.Visible = false;
            dvAddDocument.Visible = false;
            dvShowModal.Visible = false;
            if (dvAdmin.Visible)
            {
                if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                {
                    txtAppCompleteness.ReadOnly = false;
                }
                txtComments.ReadOnly = false;
                btnSave.Text = "Save";
                btnSave.Visible = true;
            }
        }

        private void LoadSignator()
        {
            try
            {
                string loginToken = (Session["LoginToken"] != null) ? Session["LoginToken"].ToString() : "";
                List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                FG_App_Signatures signature = signatures.FirstOrDefault(a => a.LoginToken == loginToken);
                //string capacity = dataItem["SupportingDoc"].Text;
                if (signature != null)
                {
                    if (signature.SignatureRole == "Fire Chief")
                    {
                        hfIsFireChief.Value = "True";
                        btnSave.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = false;
                    }
                    if (signature.Signature == null || signature.Signature == "")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSignatureModal();", true);

                        hfSignatureId.Value = signature.SignatureId.ToString();
                        hfSignatureDate.Value = signature.DateEntered.ToString();
                        ddlSignatureRole.SelectedValue = signature.SignatureRole;
                        txtPrintedName.Text = signature.PrintedName;
                        txtEmail.Text = signature.EmailAddress;
                        txtSignature.Text = signature.Signature;
                        txtSignatureDate.Text = (signature.DateSigned != null) ? Convert.ToDateTime(signature.DateSigned).ToShortDateString() : DateTime.Now.ToShortDateString();
                        hfLoginToken.Value = signature.LoginToken;
                        if (signature.Signature != null && signature.Signature != "")
                        {
                            chkAgreement.Checked = true;
                        }
                        chkSelfSign.Checked = true;
                        chkSelfSign.Enabled = false;
                        chkAgreement.Enabled = true;
                        txtPrintedName.ReadOnly = false;
                        txtSignature.ReadOnly = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private async Task<bool> LoadDocsSigs(DetailedFGAppSigsDocs model)
        {
            try
            {
                string fcName = "";
                FG_App_GeneralInfo genInfo = new FG_App_GeneralInfo();
                genInfo = await fgAppService.GetFGApplicationGeneralInfoAsync(model.ApplicationId);
                if (genInfo != null)
                {
                    fcName = (genInfo.FireChiefName != null) ? genInfo.FireChiefName : "";
                }

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

                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                if (fiscalYear < 2024)
                {
                    txtAppCompleteness.Text = model.AppCompletenessGrade.ToString();
                }
                else
                {
                    if (Convert.ToBoolean(Session["IsWebAdmin"]) == true)
                    {
                        List<DetailedFGAppScores> appScores = new List<DetailedFGAppScores>();
                        appScores = await fgAppService.GetDetailedFGAppScoresAdminAsync(model.ApplicationId);
                        if (appScores.Count > 0)
                        {
                            int completenessScore = 0;
                            foreach (DetailedFGAppScores score in appScores)
                            {
                                completenessScore += score.AppCompletenessGrade;
                            }
                            lblAppCompleteness.Text = "Average Application Completeness Score: ";
                            txtAppCompleteness.Text = completenessScore.ToString();
                        }
                    }
                    else
                    {
                        if (Session["Role"].ToString() != "Signator")
                        {
                            DetailedFGAppScores appScores = new DetailedFGAppScores();
                            Guid webUserId = new Guid(Session["WebUserId"].ToString());
                            appScores = await fgAppService.GetDetailedFGAppScoresCounselorAsync(model.ApplicationId, webUserId);
                            if (appScores != null)
                            {
                                txtAppCompleteness.Text = appScores.AppCompletenessGrade.ToString();
                            }
                        }         
                    } 
                }
                
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgDocuments.DataSource = model.Documents;
                ViewState["dtDocuments"] = model.Documents;

                rgSignatures.DataSource = model.Signatures;
                ViewState["dtSignatures"] = model.Signatures;
                string username = (Session["CodepalUserName"] != null) ? Session["CodepalUserName"].ToString() : "";
                string userid = (Session["WebUserId"] != null) ? Session["WebUserId"].ToString() : "";

                foreach (FG_App_Signatures signature in model.Signatures)
                {
                    if (signature.SignatureRole == "Fire Chief")
                    {
                        if (isFireCheifLoggedIn(signature.PrintedName, fcName, username) == true)
                        {
                            hfIsFireChief.Value = "true";
                        }
                        else
                        {
                            if (signature.WebUserId != null && signature.WebUserId.ToString() != userid)
                            {
                                hfIsFireChief.Value = "true";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
            return true;
        }

        private bool isFireCheifLoggedIn(string fcSig, string genFCName, string loggedInUserName)
        {
            bool isFireCheifLoggedIn = false;
            fcSig = fcSig.ToLower();
            genFCName = genFCName.ToLower();
            loggedInUserName = loggedInUserName.ToLower();
            try
            {
                if (fcSig.Contains(loggedInUserName))
                {
                    isFireCheifLoggedIn = true;
                }
                if (genFCName.Contains(loggedInUserName))
                {
                    isFireCheifLoggedIn = true;
                }
                if (isFireCheifLoggedIn == false)
                {
                    string[] lgUserName = loggedInUserName.Split(' ');
                    switch (lgUserName.Length)
                    {
                        case 1:
                            if (fcSig.Contains(lgUserName[0]))
                            {
                                isFireCheifLoggedIn = true;
                            }
                            break;
                        case 2:
                            if (fcSig.Contains(lgUserName[0].ToString()) && fcSig.Contains(lgUserName[1].ToString()))
                            {
                                isFireCheifLoggedIn = true;
                            }
                            break;
                        case 3:
                            if (fcSig.Contains(lgUserName[0].ToString()) && fcSig.Contains(lgUserName[2].ToString()))
                            {
                                isFireCheifLoggedIn = true;
                            }
                            break;
                    }
                }
            }
            catch
            {
                return isFireCheifLoggedIn;
            }
            return isFireCheifLoggedIn;
        }

        private async Task<bool> LoadDetailedAppValidation(Guid appId, bool fromSave = false)
        {
            try
            {
                
                if (Session["SaveStatusMessage"] != null)
                {
                    try
                    {
                        fromSave = Convert.ToBoolean(Session["SaveStatusMessage"]);
                    }
                    catch { }
                }
                DetailedFGAppValidation detailedValidation = await fgAppService.GetDetailedFGApplicationValidationAsync(appId);
                //bool isFireChief = false;
                //try
                //{
                //    isFireChief = Convert.ToBoolean(hfIsFireChief.Value);
                //}
                //catch
                //{
                //    isFireChief = false;
                //}
                if (detailedValidation != null)
                {
                    if (detailedValidation.InstructionsSubmitted && detailedValidation.GeneralInfoValid && detailedValidation.BudgetInfoValid && detailedValidation.CommunityInfoValid && 
                        detailedValidation.ResponseHistoryValid && detailedValidation.WaterAvailabilityValid && detailedValidation.TrainingValid && detailedValidation.ApparatusValid && 
                        detailedValidation.CommunicationEquipValid && detailedValidation.HazardsThreatsValid && detailedValidation.PPEValid && detailedValidation.EquipmentNeedsValid && 
                        detailedValidation.GrantFundingJustificationValid && detailedValidation.ProjectBudgetValid && detailedValidation.DocsSigsValid)
                    {

                        //if ((detailedValidation.AppStatus == 6 || detailedValidation.AppStatus == 3) && isFireChief == true)
                        if ((detailedValidation.AppStatus == 6 || detailedValidation.AppStatus == 3))
                        {
                            btnSave.Visible = true;
                            btnSave.Enabled = true;
                            btnSave.Text = "Submit Application";
                        }
                        //else if (fromSave == true && (detailedValidation.AppStatus == 6 || detailedValidation.AppStatus == 3) && isFireChief == false)
                        //{
                        //    SendFireChiefEmail();
                        //    dvSubmitInstructions.Visible = true;
                        //    dvSubmitInstructionsBody.InnerHtml = "Thank you. Your application can now be submitted by the Fire Chief. An email has been sent to the Fire Chief with a link to view and submit the application.";
                        //}
                        //else if (fromSave == false && (detailedValidation.AppStatus == 6 || detailedValidation.AppStatus == 3) && isFireChief == false)
                        //{
                        //    dvSubmitInstructions.Visible = true;
                        //    dvSubmitInstructionsBody.InnerHtml = "The application is ready to be submitted by the Fire Chief. An email has been sent to the Fire Chief.";
                        //}
                    }
                }
                return true;
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
                        //Response.Redirect("~/Application/SignaturesDocs", false);
                        break;
                    default:
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                }
            }
        }


        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/ProjectBudgetSheet", false);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(SaveAndSendAsync));
        }

        private sealed class EmailSendSnapshot
        {
            public string ApplicationId { get; set; }
            public string FiscalYear { get; set; }
            public string DepartmentName { get; set; }
            public string ExternalSenderBodyLine { get; set; }
            public EmailSendContext SignatoryContext { get; set; }
            public EmailSendContext SubmittalContext { get; set; }
        }

        private EmailSendSnapshot CaptureEmailSendSnapshot()
        {
            string appId = Session["ApplicationId"] != null ? Session["ApplicationId"].ToString() : string.Empty;
            Guid? agencyId = null;
            if (Session["AgencyId"] != null && Guid.TryParse(Session["AgencyId"].ToString(), out Guid parsedAgency))
            {
                agencyId = parsedAgency;
            }

            string sentByUserId = Session["WebUserId"] != null ? Session["WebUserId"].ToString() : string.Empty;
            string sentByEmail = Session["WebUserEmail"] != null ? Session["WebUserEmail"].ToString() : string.Empty;
            string sentByLogin = Session["WebUser"] != null ? Session["WebUser"].ToString() : string.Empty;
            string sentByRole = Session["Role"] != null ? Session["Role"].ToString() : string.Empty;
            string role = sentByRole;
            string login = sentByLogin;
            string email = sentByEmail;

            return new EmailSendSnapshot
            {
                ApplicationId = appId,
                FiscalYear = Session["FiscalYear"] != null ? Session["FiscalYear"].ToString() : string.Empty,
                DepartmentName = Session["DepartmentName"] != null ? Session["DepartmentName"].ToString() : string.Empty,
                ExternalSenderBodyLine = EmailSendContextHelper.BuildExternalSenderBodyLine(role, login, email),
                SignatoryContext = EmailSendContextHelper.FromValues(
                    "SignatoryRequest", appId, sentByUserId, sentByEmail, sentByLogin, sentByRole, agencyId),
                SubmittalContext = EmailSendContextHelper.FromValues(
                    "ApplicationSubmitted", appId, sentByUserId, sentByEmail, sentByLogin, sentByRole, agencyId)
            };
        }

        private bool IsSessionIntact()
        {
            return Session["WebUserId"] != null &&
                !string.IsNullOrWhiteSpace(Convert.ToString(Session["WebUserId"]));
        }

        private async Task SaveAndSendAsync()
        {
            bool submit = btnSave.Text == "Submit Application";
            bool isAdmin = dvAdmin.Visible;
            if (await SaveForm(submit) != true)
            {
                return;
            }

            EmailSendSnapshot snapshot = CaptureEmailSendSnapshot();
            string saveMessage = "<div class='alert alert-success'>Signatures and Documents Saved</div>";
            if (isAdmin == false)
            {
                try
                {
                    if (submit == false)
                    {
                        int sentCount = await SendSignatorEmailsAsync(snapshot);
                        if (sentCount > 0)
                        {
                            saveMessage = "<div class='alert alert-success'>Signatures and Documents Saved. " +
                                sentCount + " signatory email(s) sent.</div>";
                        }
                        dvError.InnerHtml = saveMessage;
                        return;
                    }

                    await SendSubmittalEmailsAsync(snapshot);
                    if (IsSessionIntact())
                    {
                        Session["SaveMessage"] =
                            "<div class='alert alert-success'>Application submitted. Confirmation email sent.</div>";
                        Session["SaveStatusMessage"] = 1;
                        Response.Redirect("~/Application/AppConf", false);
                        return;
                    }

                    dvError.InnerHtml = "<div class='alert alert-warning'>Application was submitted and " +
                        "confirmation email was sent, but your session expired. Please log in again to confirm status.</div>";
                    return;
                }
                catch (Exception ex)
                {
                    _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + HttpUtility.HtmlEncode(ex.Message) + "</div>";
                    return;
                }
            }

            Session["SaveMessage"] = saveMessage;
            Session["SaveStatusMessage"] = 1;
            Response.Redirect("~/Application/SignaturesDocs", false);
        }

        private async Task SendSubmittalEmailsAsync(EmailSendSnapshot snapshot)
        {
            if (emailer == null || systemService == null)
            {
                throw new Exception("Email services are not available.");
            }

            string appId = snapshot.ApplicationId;
            string fy = snapshot.FiscalYear;
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new Exception("Application id is not available for email.");
            }

            string from = EmailSendContextHelper.GetDefaultSender();
            Guid appIdGuid = new Guid(appId);
            FG_App_GeneralInfo info = await fgAppService.GetFGApplicationGeneralInfoAsync(appIdGuid);
            string emailAdd = info != null ? info.EmailAddress : string.Empty;
            if (string.IsNullOrWhiteSpace(emailAdd) || !emailer.EmailIsValid(emailAdd))
            {
                throw new Exception("A valid General Information email address is required before submit confirmation can be sent.");
            }

            string department = snapshot.DepartmentName ?? string.Empty;
            string subject = "NMSFM Fire Grant Application submitted for " + department;
            string body = snapshot.ExternalSenderBodyLine ?? string.Empty;
            body += "Thank you for submitting the fire grant application for " + department + " for fiscal year " + fy + ".";
            body += "<br /><br />";
            await emailer.SendMailMessageAsync(from, emailAdd, "", "", subject, body, "", "",
                snapshot.SubmittalContext, systemService);
        }

        private async Task<int> SendSignatorEmailsAsync(EmailSendSnapshot snapshot)
        {
            if (emailer == null || systemService == null)
            {
                throw new Exception("Email services are not available.");
            }

            string appId = snapshot.ApplicationId;
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new Exception("Application id is not available for email.");
            }

            Guid appIdGuid = new Guid(appId);
            DetailedFGAppSigsDocs model = await fgAppService.GetFGApplicationDocsSigsAsync(appIdGuid);
            List<FG_App_Signatures> signatures = model != null && model.Signatures != null
                ? model.Signatures
                : new List<FG_App_Signatures>();

            List<FG_App_Signatures> pending = signatures
                .Where(sig => string.IsNullOrWhiteSpace(sig.Signature))
                .ToList();

            if (pending.Count == 0)
            {
                return 0;
            }

            string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null)
                ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
                : "https://fireservicesgrant.dhsem.nm.gov/";
            string from = EmailSendContextHelper.GetDefaultSender();
            string department = snapshot.DepartmentName ?? string.Empty;
            string externalBodyLine = snapshot.ExternalSenderBodyLine ?? string.Empty;
            var emailErrors = new List<string>();
            int sentCount = 0;

            foreach (FG_App_Signatures sig in pending)
            {
                string email = sig.EmailAddress != null ? sig.EmailAddress.Trim() : string.Empty;
                if (string.IsNullOrWhiteSpace(email) || !emailer.EmailIsValid(email))
                {
                    string roleLabel = string.IsNullOrWhiteSpace(sig.SignatureRole) ? "Signatory" : sig.SignatureRole;
                    emailErrors.Add(roleLabel + " has no valid email address.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(sig.LoginToken))
                {
                    string roleLabel = string.IsNullOrWhiteSpace(sig.SignatureRole) ? "Signatory" : sig.SignatureRole;
                    emailErrors.Add(roleLabel + " is missing a sign-in token.");
                    continue;
                }

                string decryptedLoginToken = await accountService.DecryptString(sig.LoginToken);
                string subject = "Approval Requested for NMSFM Fire Grant Application";
                string body = externalBodyLine;
                body += "Please click the link below to view and sign off on the fire grant application for " + department + ".";
                body += "<br /><br /><a href='" + url + "/LoginSignator/" + sig.ApplicationId + "/" +
                    decryptedLoginToken + "'>View Application</a>";
                await emailer.SendMailMessageAsync(from, email, "", "", subject, body, "", "",
                    snapshot.SignatoryContext, systemService);
                sentCount++;
            }

            if (sentCount == 0)
            {
                string detail = emailErrors.Count > 0
                    ? string.Join(" ", emailErrors)
                    : "No signatory emails could be sent.";
                throw new Exception(detail);
            }

            if (emailErrors.Count > 0)
            {
                throw new Exception(
                    sentCount + " signatory email(s) sent. Some could not be sent: " + string.Join(" ", emailErrors));
            }

            return sentCount;
        }

        private async Task SendFireChiefEmailAsync()
        {
            try
            {
                List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                if (signatures != null)
                {
                    foreach (FG_App_Signatures sig in signatures)
                    {
                        if (sig.SignatureRole == "Fire Chief")
                        {
                            string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "https://fireservicesgrant.dhsem.nm.gov/";
                            string from = EmailSendContextHelper.GetDefaultSender();

                            string email = sig.EmailAddress;
                            string department = (Session["DepartmentName"] != null) ? Session["DepartmentName"].ToString() : "";
                            string appId = Session["ApplicationId"].ToString();
                            string decryptedLoginToken = await accountService.DecryptString(sig.LoginToken);
                            string subject = "NMSFM Fire Grant Application Ready for Submittal";
                            string body = EmailSendContextHelper.BuildExternalSenderBodyLine();
                            body += "Please click the link below to view and submit the fire grant application for " + department + ".";
                            body += "<br /><br /><a href='" + url + "/LoginSignator/" + sig.ApplicationId + "/" + decryptedLoginToken + "'>View Application</a>";
                            var emailContext = EmailSendContextHelper.FromSession("FireChiefSubmit", appId);
                            await emailer.SendMailMessageAsync(from, email, "", "", subject, body, "", "", emailContext, systemService);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        //protected async void btnNext_Click(object sender, EventArgs e)
        //{
        //    if (await SaveForm() == true)
        //    {
        //        Response.Redirect("~/Application/Apparatus", false);
        //    }
        //}

        private async Task<bool> SaveForm(bool submit = false)
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true && Session["Role"].ToString() != "Signator" && dvAdmin.Visible != true)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;

                List<FG_AppDocListItem> documents = new List<FG_AppDocListItem>();
                if (ViewState["dtDocuments"] != null) { documents = (List<FG_AppDocListItem>)ViewState["dtDocuments"]; }

                List<FG_App_Signatures> signatures = new List<FG_App_Signatures>();
                if (ViewState["dtSignatures"] != null) { signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"]; }

                bool faSig = false;
                bool fcSig = false;
                bool cmSig = false;
                foreach (FG_App_Signatures signature in signatures)
                {
                    switch (signature.SignatureRole)
                    {
                        case "Fiscal Agent":
                            if (signature.Signature != null && signature.Signature != "")
                            {
                                faSig = true;
                            }
                            break;
                        case "Fire Chief":
                            if (signature.Signature != null && signature.Signature != "")
                            {
                                fcSig = true;
                            }
                            break;
                        case "Administrative Manager":
                            if (signature.Signature != null && signature.Signature != "")
                            {
                                cmSig = true;
                            }
                            break;
                    }
                }
                if (faSig == false)
                {
                    isValid = false;
                    errorMessage += "Fiscal Agent signature is required before submittal.<br />";
                }
                if (fcSig == false)
                {
                    isValid = false;
                    errorMessage += "Fire Chief signature is required before submittal.<br />";
                }
                if (cmSig == false)
                {
                    isValid = false;
                    errorMessage += "Administrative Manager signature is required before submittal.<br />";
                }

                Guid appIdGuid = new Guid(hfApplicationId.Value);
                FG_App_ProjectBudget projectBudget = new FG_App_ProjectBudget();
                projectBudget = await fgAppService.GetFGApplicationProjectBudgetAsync(appIdGuid);
                bool scopeOfWorkDoc = false;
                bool specDoc = false;
                bool stipendDoc = false;
                foreach (FG_AppDocListItem docs in documents)
                {
                    if (docs.DocumentType == "Scope of Project/Work")
                    {
                        scopeOfWorkDoc = true;
                    }
                    if (docs.DocumentType == "Specifications")
                    {
                        specDoc = true;
                    }
                    if (docs.DocumentType == "Stipend")
                    {
                        stipendDoc = true;
                    }
                }
                if (projectBudget != null && projectBudget.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    if (projectBudget.AmountRequested > 0)
                    {
                        if (scopeOfWorkDoc == false && specDoc == false)
                        {
                            isValid = false;
                            errorMessage += "Scope of Work document or specifications document required for grant funding.<br />";
                        }
                    }
                    if (projectBudget.StipendAmount > 0)
                    {
                        if (stipendDoc == false)
                        {
                            isValid = false;
                            errorMessage += "Stipend document document required for stipend amount requested.<br />";
                        }
                    }
                }
                    //bool fiscalAgentCommitmentSubmitted = false;
                    //if (documents != null)
                    //{
                    //    foreach (FG_App_Documents docs in documents)
                    //    {
                    //        if (docs.DocumentType == "Fiscal Agent Commitment")
                    //        {
                    //            fiscalAgentCommitmentSubmitted = true;
                    //        }
                    //    }
                    //}
                    //if (fiscalAgentCommitmentSubmitted == false)
                    //{
                    //    isValid = false;
                    //    errorMessage += "Fiscal Agent Commitment document required before submittal.<br />";
                    //}

                    if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                    
                }

                var model = new DetailedFGAppSigsDocs();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();

                model.Documents = documents;
                model.Signatures = signatures;

                model.DocumentNumber = (documents != null) ? documents.Count() : 0;
                model.SignaturesCollected = (signatures != null) ? signatures.Count() : 0;
                //model.AppCompletenessGrade = Convert.ToInt32(txtAppCompleteness.DbValue);
                model.AdminComments = txtComments.Text;

                bool retVal = await fgAppService.SaveApplicationDocsSigsAsync(model);

                if (submit)
                {
                    FGApplications app = await fgAppService.GetFGApplicationById(model.ApplicationId);
                    app.AppStatus = 5;
                    app.DateSubmitted = DateTime.Now;
                    app.LastStatusChange = DateTime.Now;
                    Guid submittedBy = new Guid(Session["CodepalUserId"].ToString());
                    app.SubmittedBy = submittedBy;

                    await fgAppService.UpdateApplication(app);
                }
                else
                {
                    await LoadDetailedAppValidation(model.ApplicationId, true);
                }

                //Added for 2024 (vwd)
                if (retVal == true && dvAdmin.Visible)
                {
                    if (Convert.ToBoolean(Session["IsWebAdmin"]) == false)
                    {
                        DetailedFGAppScores appScores = new DetailedFGAppScores();
                        appScores.ApplicationId = model.ApplicationId;
                        appScores.WebUserId = new Guid(Session["WebUserId"].ToString());
                        appScores.UserName = Session["WebUser"].ToString();
                        appScores.AppCompletenessGrade = Convert.ToInt32(txtAppCompleteness.DbValue);
                        await fgAppService.SaveCounselorScores(appScores);
                    }
                    
                }

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
        }

        protected void btnDeleteSignature_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                for (int i = 0; i < signatures.Count; i++)
                {
                    if (signatures[i].SignatureId.ToString() == hfSignatureId.Value.ToString())
                    {
                        signatures.RemoveAt(i);
                        break;
                    }
                }
                ViewState["dtSignatures"] = signatures;
                rgSignatures.DataSource = signatures;
                rgSignatures.DataBind();
                ddlSignatureRole.SelectedIndex = 0;
                chkAgreement.Checked = false;
                chkSelfSign.Checked = false;
                txtPrintedName.Text = "";
                txtSignature.Text = "";
                txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                hfSignatureId.Value = "";
                hfLoginToken.Value = "";
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected async void btnSaveSignature_ServerClick(object sender, EventArgs e)
        {
            try
            {
                lblSignatureError.Text = "";
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (chkSelfSign.Checked && chkAgreement.Checked == false)
                {
                    errorMessage += "You must agree to the application agreement.<br />";
                    isValid = false;
                }

                if (txtPrintedName.Text == "")
                {
                    errorMessage += "Signer Name / Title is Required.<br />";
                    isValid = false;
                }
                if (chkSelfSign.Checked == false)
                {
                    if (txtEmail.Text == "")
                    {
                        errorMessage += "Signer Email is Required.<br />";
                        isValid = false;
                    }
                    else
                    {
                        if (!emailer.EmailIsValid(txtEmail.Text))
                        {
                            errorMessage += "Signer Email must be a valid email address.<br />";
                            isValid = false;
                        }
                    }
                }

                if (chkSelfSign.Checked && txtSignature.Text == "")
                {
                    errorMessage += "Signature is Required.<br />";
                    isValid = false;
                }

                List<FG_App_Signatures> signatures = new List<FG_App_Signatures>();
                if (ViewState["dtSignatures"] != null)
                {
                    signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                }

                if (ddlSignatureRole.SelectedValue == "")
                {
                    errorMessage += "Signature Role is Required.<br />";
                    isValid = false;
                }
                else
                {
                    bool dupSig = false;
                    foreach (FG_App_Signatures sig in signatures)
                    {
                        if (sig.SignatureRole == ddlSignatureRole.SelectedValue && hfSignatureId.Value == "")
                        {
                            dupSig = true;
                        }
                    }
                    if (dupSig)
                    {
                        errorMessage += "Signature Role has already been entered.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }                

                FG_App_Signatures signature = new FG_App_Signatures();

                if (hfSignatureId.Value != "")
                {
                    for (int i = 0; i < signatures.Count; i++)
                    {
                        if (signatures[i].SignatureId.ToString() == hfSignatureId.Value.ToString())
                        {
                            signature = signatures[i];
                            signatures.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (signature.SignatureId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    signature.SignatureId = Guid.NewGuid();
                    signature.DateEntered = DateTime.Now;
                }
                else
                {
                    signature.DateEntered = Convert.ToDateTime(hfSignatureDate.Value.ToString());
                }
                if (signature.LoginToken == null || signature.LoginToken == "")
                {
                    string loginToken = Guid.NewGuid().ToString();
                    loginToken = accountService.EncryptString(loginToken);
                    signature.LoginToken = loginToken;
                }

                //UploadedFile file;
                //byte[] fileData = null;
                //string theextension = null;
                //string filename = null;
                //if (ruTrainingDoc.UploadedFiles.Count != 0)
                //{
                //    file = ruTrainingDoc.UploadedFiles[0];
                //    fileData = new byte[file.InputStream.Length];
                //    file.InputStream.Read(fileData, 0, Convert.ToInt32(file.InputStream.Length));
                //    theextension = file.GetExtension();
                //    filename = file.FileName;
                //}
                //else
                //{
                //    fileData = null;
                //    theextension = string.Empty;
                //    filename = null;
                //}

                Guid appId = new Guid(hfApplicationId.Value.ToString());
                signature.ApplicationId = appId;
                signature.SignatureRole = ddlSignatureRole.SelectedValue;
                signature.PrintedName = txtPrintedName.Text;
                if (!chkSelfSign.Checked)
                {
                    signature.EmailAddress = txtEmail.Text;
                    signature.Signature = "";
                    signature.DateSigned = null;
                }
                else
                {
                    signature.EmailAddress = txtEmail.Text;
                    signature.Signature = txtSignature.Text;
                    signature.DateSigned = DateTime.Now;
                    signature.WebUserId = new Guid(Session["WebUserId"].ToString());
                    if (ddlSignatureRole.SelectedValue == "Fire Chief")
                    {
                        hfIsFireChief.Value = "True";
                    }
                }
                
                signatures.Add(signature);
                ViewState["dtSignatures"] = signatures;
                rgSignatures.DataSource = signatures;
                rgSignatures.DataBind();
                ddlSignatureRole.SelectedIndex = 0;
                chkAgreement.Checked = false;
                chkSelfSign.Checked = false;
                txtPrintedName.Text = "";
                txtSignature.Text = "";
                txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                hfSignatureId.Value = "";
                hfLoginToken.Value = "";
                if (Session["Role"].ToString() == "Signator" || hfIsFireChief.Value == "True")
                {
                    await SaveForm(false);
                }
                dvError.InnerHtml = "<div class='alert alert-success'>" + signature.SignatureRole + " signature has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                //ddlSignatureRole.SelectedIndex = 0;
                //txtSignature.Text = "";
                //txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                //hfSignatureId.Value = "";
                //hfLoginToken.Value = "";
                lblSignatureError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSignatureModal();", true);
            }
        }

        protected async void lnkAddDocument_Click(object sender, EventArgs e)
        {
            try
            {
                dvDocumentError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (ddlCategory.SelectedValue == "0")
                {
                    errorMessage += "You must enter the document type.<br />";
                    isValid = false;
                }
                if (fuDocumentation.UploadedFiles.Count == 0)
                {
                    errorMessage += "You must select a file to upload.<br />";
                    isValid = false;
                }
                else
                {
                    bool isCorrectFormat = false;
                    if (fuDocumentation.UploadedFiles[0].ContentType.ToString() == "text/plain" | fuDocumentation.UploadedFiles[0].ContentType.ToString() == "image/jpeg" | fuDocumentation.UploadedFiles[0].ContentType.ToString() == "image/png" | fuDocumentation.UploadedFiles[0].ContentType.ToString() == "image/bmp" | fuDocumentation.UploadedFiles[0].ContentType.ToString() == "application/pdf" | fuDocumentation.UploadedFiles[0].ContentType.ToString().Contains("word"))
                    {
                        isCorrectFormat = true;
                    }
                    if (isCorrectFormat == false)
                    {
                        throw new Exception("Images must be in the format of .txt, .docx, .pdf, .jpg, .png or .bmp<br />");
                    }
                }
                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }

                //List<FG_App_Documents> docs = new List<FG_App_Documents>();
                List<FG_AppDocListItem> docList = new List<FG_AppDocListItem>();
                if (ViewState["dtDocuments"] != null)
                {
                    docList = (List<FG_AppDocListItem>)ViewState["dtDocuments"];
                }
                FG_App_Documents doc = new FG_App_Documents();
                FG_AppDocListItem docItem = new FG_AppDocListItem();

                if (doc.DocumentId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    doc.DocumentId = Guid.NewGuid();
                    docItem.DocumentId = Guid.NewGuid();
                }

                UploadedFile file;
                byte[] fileData = null;
                string theextension = null;
                string filename = null;
                if (fuDocumentation.UploadedFiles.Count != 0)
                {
                    file = fuDocumentation.UploadedFiles[0];
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

                Guid appId = new Guid(hfApplicationId.Value.ToString());
                doc.ApplicationId = appId;
                doc.DocumentType = ddlCategory.SelectedItem.Text;
                doc.DocumentName = filename;
                doc.Document = fileData;

                
                docList.Add(docItem);
                bool saved = await fgAppService.SaveApplicationDocumentAsync(doc);
                if (saved)
                {
                    docItem.ApplicationId = appId;
                    docItem.DocumentType = ddlCategory.SelectedItem.Text;
                    docItem.DocumentName = filename;
                    ViewState["dtDocuments"] = docList;
                    rgDocuments.DataSource = docList;
                    rgDocuments.DataBind();
                    dvDocumentError.InnerHtml = "<div class='alert alert-success'>" + filename + " has been added.</div>";
                }
                else
                {
                    dvDocumentError.InnerHtml = "<div class='alert alert-error'>An error has occured saving " + filename + "</div>";
                }
                ddlCategory.SelectedIndex = 0;
                fuDocumentation.UploadedFiles.Clear();
                dvDocumentError.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                dvDocumentError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_AppDocListItem> docs = new List<FG_AppDocListItem>();
            if (ViewState["dtDocuments"] != null)
            {
                docs = (List<FG_AppDocListItem>)ViewState["dtDocuments"];
            }
            rgDocuments.DataSource = docs;
        }

        protected void rgDocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_AppDocListItem> docs = new List<FG_AppDocListItem>();
            if (ViewState["dtDocuments"] != null)
            {
                docs = (List<FG_AppDocListItem>)ViewState["dtDocuments"];
            }
            rgDocuments.DataSource = docs;
            rgDocuments.DataBind();
        }

        protected void rgDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected async void rgDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                dvDocumentError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string pId = e.CommandArgument.ToString();
                    Guid docId = new Guid(pId);
                    if (e.CommandName == "Delete")
                    {
                        bool deleted = await fgAppService.DeleteApplicationDocumentAsync(docId);
                        if (deleted)
                        {
                            string docName = "";
                            List<FG_AppDocListItem> docs = (List<FG_AppDocListItem>)ViewState["dtDocuments"];
                            for (int i = 0; i < docs.Count; i++)
                            {
                                if (docs[i].DocumentId.ToString() == pId)
                                {
                                    docName = docs[i].DocumentName;
                                    docs.RemoveAt(i);
                                    break;
                                }
                            }
                            ViewState["dtDocuments"] = docs;
                            rgDocuments.DataSource = docs;
                            rgDocuments.DataBind();
                            dvDocumentError.InnerHtml = "<div class='alert alert-success'>" + docName + " has been removed.</div>";
                        }
                        else
                        {
                            dvDocumentError.InnerHtml = "<div class='alert alert-error'>An error occured saving the document.</div>";
                        }
                    }
                    else if (e.CommandName == "View")
                    {
                        ViewDocument(pId);
                    }
                    else if (e.CommandName == "Download")
                    {
                        DownloadDocument(pId);
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private async void ViewDocument(string docId)
        {
            try
            {
                Guid id = new Guid(docId);
                FG_App_Documents doc = await fgAppService.GetApplicationDocumentByIdAsync(id);
                if (doc != null)
                {
                    byte[] bytes;
                    string fileName, fileOnlyName, extention;
                    bytes = doc.Document;
                    //contentType = "";
                    fileName = doc.DocumentName;
                    extention = GetExtension(fileName);
                    fileOnlyName = ExtractFileNameWithoutExtention(fileName);
                    byte[] renderedBytes = bytes;
                    MemoryStream stream = new MemoryStream(bytes);
                    // RadFlow Documents
                    if (Regex.IsMatch(extention, ".docx|.rtf|.html|.txt|.pdf"))
                    {
                        if (Regex.IsMatch(extention, ".docx|.rtf|.html|.txt"))
                        {
                            IFormatProvider<RadFlowDocument> provider = null;
                            RadFlowDocument document = null;
                            switch (extention)
                            {
                                case ".docx": provider = new DocxFormatProvider(); break;
                                case ".rtf": provider = new RtfFormatProvider(); break;
                                case ".html": provider = new HtmlFormatProvider(); break;
                                case ".txt": provider = new TxtFormatProvider(); break;
                                default: provider = null; break;
                            }
                            document = provider.Import(stream);
                            Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new
                            Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                pdfProvider.Export(document, ms);
                                renderedBytes = ms.ToArray();
                            }
                        }
                        pdfView.PdfjsProcessingSettings.FileSettings.Data = Convert.ToBase64String(renderedBytes);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDocModal();", true);
                    }
                    else if (Regex.IsMatch(extention, ".jpg|.gif|.png|.jpeg"))
                    {
                        imgDocument.Src = "data:image/png;base64," + Convert.ToBase64String(bytes);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openImgModal();", true);
                    }

                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private static string GetExtension(string path)
        {
            var ret = "";
            for (; ; )
            {
                var ext = Path.GetExtension(path);
                if (String.IsNullOrEmpty(ext))
                    break;
                path = path.Substring(0, path.Length - ext.Length);
                ret = ext + ret;
            }
            return ret;
        }

        public static string ExtractFileNameWithoutExtention(string path)
        {
            string fileName = Path.GetFileName(path);
            int lastIndex = fileName.LastIndexOf(".");
            if (lastIndex != -1)
            {
                fileName = fileName.Substring(0, lastIndex);
            }
            return fileName;
        }

        private async void DownloadDocument(string docId)
        {
            try
            {
                Guid id = new Guid(docId);
                FG_App_Documents doc = await fgAppService.GetApplicationDocumentByIdAsync(id);
                if (doc != null)
                {
                    byte[] bytes;
                    string fileName, contentType;
                    bytes = doc.Document;
                    contentType = "";
                    fileName = doc.DocumentName;
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
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
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

        protected void rgSignatures_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void rgSignatures_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvError.InnerHtml = "";
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    string pId = e.CommandArgument.ToString();
                    Guid trId = new Guid(pId);
                    List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                    FG_App_Signatures signature = signatures.FirstOrDefault(a => a.SignatureId == trId);
                    //string capacity = dataItem["SupportingDoc"].Text;
                    if (e.CommandName == "View" && signature != null)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSignatureModal();", true);

                        hfSignatureId.Value = pId;
                        ddlSignatureRole.SelectedValue = signature.SignatureRole;
                        txtPrintedName.Text = signature.PrintedName;
                        txtEmail.Text = signature.EmailAddress;
                        txtSignature.Text = signature.Signature;
                        if (signature.Signature != null && signature.Signature != "")
                        {
                            chkSelfSign.Checked = true;
                        }
                        hfSignatureDate.Value = signature.DateEntered.ToString();
                        txtSignatureDate.Text = (signature.DateSigned != null) ? Convert.ToDateTime(signature.DateSigned).ToShortDateString() : DateTime.Now.ToShortDateString();
                        hfLoginToken.Value = signature.LoginToken;
                        if (signature.Signature != null && signature.Signature != "")
                        {
                            chkAgreement.Checked = true;
                        }
                        //if (training.TrainingDocument != null)
                        //{
                        //    lnkTrainingDoc.Text = training.TrainingDocumentName;
                        //    dvTrainingDocLink.Visible = true;
                        //}
                        //else
                        //{
                        //    dvTrainingDocLink.Visible = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}





