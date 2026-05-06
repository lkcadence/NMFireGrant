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
                    LoadSettings();
                    hfApplicationId.Value = appId;
                    txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        DetailedFGAppSigsDocs docsSigs = new DetailedFGAppSigsDocs();
                        docsSigs = await fgAppService.GetFGApplicationDocsSigsAsync(appIdGuid);
                        if (docsSigs != null && docsSigs.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadDocsSigs(docsSigs);
                            LoadDetailedAppValidation(appIdGuid);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
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
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSignatureModal();", true);

                    hfSignatureId.Value = signature.SignatureId.ToString();
                    ddlSignatureRole.SelectedValue = signature.SignatureRole;
                    txtPrintedName.Text = signature.PrintedName;
                    txtEmail.Text = signature.EmailAddress;
                    txtSignature.Text = signature.Signature;
                    txtSignatureDate.Text = signature.DateEntered.ToShortDateString();
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
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void LoadDocsSigs(DetailedFGAppSigsDocs model)
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

                txtAppCompleteness.Text = model.AppCompletenessGrade.ToString();
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";

                rgDocuments.DataSource = model.Documents;
                ViewState["dtDocuments"] = model.Documents;

                rgSignatures.DataSource = model.Signatures;
                ViewState["dtSignatures"] = model.Signatures;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void LoadDetailedAppValidation(Guid appId)
        {
            try
            {
                DetailedFGAppValidation detailedValidation = await fgAppService.GetDetailedFGApplicationValidationAsync(appId);
                if (detailedValidation != null)
                {
                    if (detailedValidation.InstructionsSubmitted && detailedValidation.GeneralInfoValid && detailedValidation.BudgetInfoValid && detailedValidation.CommunityInfoValid && 
                        detailedValidation.ResponseHistoryValid && detailedValidation.WaterAvailabilityValid && detailedValidation.TrainingValid && detailedValidation.ApparatusValid && 
                        detailedValidation.CommunicationEquipValid && detailedValidation.HazardsThreatsValid && detailedValidation.PPEValid && detailedValidation.EquipmentNeedsValid && 
                        detailedValidation.GrantFundingJustificationValid && detailedValidation.ProjectBudgetValid && detailedValidation.DocsSigsValid)
                    {
                        if (detailedValidation.AppStatus == 6)
                        {
                            btnSave.Text = "Submit Application";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            bool submit = false;
            if (btnSave.Text == "Submit Application")
            {
                submit = true;
            }
            if (await SaveForm(submit) == true)
            {
                if (submit == false)
                {
                    SendEmails();
                }
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Signatures and Documents Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Signatures and Documents Saved</div>";
                Response.Redirect("~/Application/SignaturesDocs", false);
            }
        }

        private async void SendEmails()
        {
            try
            {
                List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];
                if (signatures != null)
                {
                    foreach (FG_App_Signatures sig in signatures)
                    {
                        if (sig.Signature == null || sig.Signature == "")
                        {
                            string email = sig.EmailAddress;
                            string department = Session["Department"].ToString();
                            string appId = Session["ApplicationId"].ToString();
                            string decryptedLoginToken = await accountService.DecryptString(sig.LoginToken);
                            string subject = "Approval Requested for NMSFM Fire Grant Application";
                            string body = "Please click the link below to view and sign off on the fire grant application for " + department + ".";
                            body += "<br /><br /><a href='http://firegranttest.vscomptech.com/LoginSignator/" + sig.ApplicationId + "/" + decryptedLoginToken + "'>View Application</a>";
                            emailer.SendMailMessage("vance@vscomptech.com", email, "", "", subject, body);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true && Session["Role"].ToString() != "Signator")
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;

                List<FG_App_Documents> documents = (List<FG_App_Documents>)ViewState["dtDocuments"];
                List<FG_App_Signatures> signatures = (List<FG_App_Signatures>)ViewState["dtSignatures"];

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
                        case "County Manager":
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
                    errorMessage += "County Manager signature is required before submittal.<br />";
                }

                bool fiscalAgentCommitmentSubmitted = false;
                if (documents != null)
                {
                    foreach (FG_App_Documents docs in documents)
                    {
                        if (docs.DocumentType == "Fiscal Agent Commitment")
                        {
                            fiscalAgentCommitmentSubmitted = true;
                        }
                    }
                }
                if (fiscalAgentCommitmentSubmitted == false)
                {
                    isValid = false;
                    errorMessage += "Fiscal Agent Commitment document required before submittal.<br />";
                }

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
                model.AppCompletenessGrade = Convert.ToInt32(txtAppCompleteness.DbValue);
                model.AdminComments = txtComments.Text;

                bool retVal = await fgAppService.SaveApplicationDocsSigsAsync(model);

                if (submit)
                {
                    FGApplications app = await fgAppService.GetFGApplicationById(model.ApplicationId);
                    app.AppStatus = 5;
                    app.DateSubmitted = DateTime.Now;
                    app.LastStatusChange = DateTime.Now;

                    await fgAppService.UpdateApplication(app);
                    Response.Redirect("../User/Home");
                }
                else
                {
                    LoadDetailedAppValidation(model.ApplicationId);
                }

                return retVal;
            }
            catch (Exception ex)
            {
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
                    signature.Signature = null;
                }
                else
                {
                    signature.EmailAddress = null;
                    signature.Signature = txtSignature.Text;
                    signature.DateSigned = DateTime.Now;
                }
                signature.DateEntered = DateTime.Now;
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
                if (Session["Role"].ToString() == "Signator")
                {
                    await SaveForm(false);
                }
                dvError.InnerHtml = "<div class='alert alert-success'>" + signature.SignatureRole + " signature has been added.</div>";
                dvError.Focus();
            }
            catch (Exception ex)
            {
                //ddlSignatureRole.SelectedIndex = 0;
                //txtSignature.Text = "";
                //txtSignatureDate.Text = DateTime.Now.ToShortDateString();
                //hfSignatureId.Value = "";
                //hfLoginToken.Value = "";
                lblSignatureError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSignatureModal();", true);
            }
        }

        protected void lnkAddDocument_Click(object sender, EventArgs e)
        {
            try
            {
                dvDocumentError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;
                if (ddlCategory.SelectedIndex == 0)
                {
                    errorMessage += "You must select a file category.<br />";
                    isValid = false;
                }
                if (fuDocumentation.UploadedFiles.Count == 0)
                {
                    errorMessage += "You must select a file to upload.<br />";
                    isValid = false;
                }
                if (isValid == false)
                {
                    throw new Exception(errorMessage);
                }

                List<FG_App_Documents> docs = new List<FG_App_Documents>();
                if (ViewState["dtDocuments"] != null)
                {
                    docs = (List<FG_App_Documents>)ViewState["dtDocuments"];
                }
                FG_App_Documents doc = new FG_App_Documents();

                if (doc.DocumentId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    doc.DocumentId = Guid.NewGuid();
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
                docs.Add(doc);
                ViewState["dtDocuments"] = docs;
                rgDocuments.DataSource = docs;
                rgDocuments.DataBind();
                ddlCategory.SelectedIndex = 0;
                fuDocumentation.UploadedFiles.Clear();
                dvDocumentError.InnerHtml = "<div class='alert alert-success'>" + filename + " has been added.</div>";
                dvDocumentError.Focus();
            }
            catch (Exception ex)
            {
                dvDocumentError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        protected void rgDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<FG_App_Documents> docs = new List<FG_App_Documents>();
            if (ViewState["dtDocuments"] != null)
            {
                docs = (List<FG_App_Documents>)ViewState["dtDocuments"];
            }
            rgDocuments.DataSource = docs;
        }

        protected void rgDocuments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            List<FG_App_Documents> docs = new List<FG_App_Documents>();
            if (ViewState["dtDocuments"] != null)
            {
                docs = (List<FG_App_Documents>)ViewState["dtDocuments"];
            }
            rgDocuments.DataSource = docs;
            rgDocuments.DataBind();
        }

        protected void rgDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void rgDocuments_ItemCommand(object sender, GridCommandEventArgs e)
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
                        string docName = "";
                        List<FG_App_Documents> docs = (List<FG_App_Documents>)ViewState["dtDocuments"];
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
                    else if (e.CommandName == "View")
                    {
                        ViewDocument(pId);
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }

        private void ViewDocument(string docId)
        {
            try
            {
                string pId = docId;
                Guid trId = new Guid(pId);
                List<FG_App_Documents> docs = (List<FG_App_Documents>)ViewState["dtDocuments"];
                FG_App_Documents doc = docs.FirstOrDefault(a => a.DocumentId == trId);
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
                        txtSignatureDate.Text = signature.DateEntered.ToShortDateString();
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
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
            }
        }
    }
}