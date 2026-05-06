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
    public partial class ApplicationStatus : System.Web.UI.Page
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
                //else
                //{
                //    if (Session["Role"].ToString() != "Internal")
                //    {
                //        Response.Redirect("~/Unauthorized");
                //    }
                //}
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
                FG_App_Help help = await fgService.GetFGHelpByPage("Application Status (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Application Status";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    LoadSettings();
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        FGApplications app = new FGApplications();
                        app = await fgAppService.GetFGApplicationById(appIdGuid);
                        if (app != null)
                        {
                            LoadAppInfo(app);
                            if (dvError.InnerHtml == "" && Session["SaveMessage"] != null)
                            {
                                dvError.InnerHtml = Session["SaveMessage"].ToString();
                                Session["SaveMessage"] = "";
                            }
                        }
                        else
                        {
                            throw new Exception("Application not found.");
                        }
                    }
                    else
                    {
                        throw new Exception("Application not found.");
                    }
                    if (Session["Role"].ToString() == "External")
                    {
                        DisableControls(this);
                        dvAdmin.Visible = false;
                    }
                    else if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        DisableControls(this);
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

        private async void LoadAppInfo(FGApplications model)
        {
            try
            {
                lblStartDate.Text = model.DateStarted.ToShortDateString();
                lblSubmittedDate.Text = (model.DateSubmitted != null) ? Convert.ToDateTime(model.DateSubmitted).ToShortDateString() : "";
                string applicationStatus = "";
                switch (model.AppStatus)
                {
                    case 1:
                        applicationStatus = "Approved";
                        break;
                    case 2:
                        applicationStatus = "Rejected";
                        break;
                    case 3:
                        applicationStatus = "Reopen";
                        break;
                    case 4:
                        applicationStatus = "Under Review";
                        break;
                    case 5:
                        applicationStatus = "Submitted for Review";
                        break;
                    case 6:
                        applicationStatus = "In Process";
                        break;
                    case 7:
                        applicationStatus = "Awarded";
                        break;
                    case 8:
                        applicationStatus = "Not Awarded";
                        break;
                    default:
                        applicationStatus = "Error";
                            break;
                }
                lblApplicationStatus.Text = applicationStatus;
                ddlUpdateStatus.SelectedValue = model.AppStatus.ToString();
                lblApplicationNumber.Text = model.ApplicationNumber;
                txtGrantedAmount.Text = model.GrantedAmount.ToString();
                txtNotes.Text = model.ApplicationNotes;
                hfAddressId.Value = model.AddressId.ToString();
                hfFiscalYear.Value = model.FiscalYear.ToString();

                List<nm_FGApplication> previousApps = await fgAppService.GetAllFGApplicationByAddressAsync(model.AddressId);
                previousApps = previousApps.Where(a => a.FiscalYear != model.FiscalYear).ToList();
                rgPreviousApps.DataSource = previousApps;
                rgPreviousApps.DataBind();

                FG_App_Signatures signature = await fgAppService.GetCounselorSignature(model.ApplicationId.ToString());
                if (signature != null)
                {
                    hfSignatureId.Value = signature.SignatureId.ToString();
                    chkAgreement.Checked = true;
                    txtReviewer.Text = signature.PrintedName;
                    txtSignature.Text = signature.Signature;
                    txtDate.SelectedDate = Convert.ToDateTime(signature.DateSigned);
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
                        Response.Redirect("~/Application/SignaturesDocs", false);
                        break;
                    default:
                        Response.Redirect("~/Application/Instructions", false);
                        break;
                }
            }
        }

        protected void rgPreviousApps_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }

        protected void rgPreviousApps_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            //LoadApps();
        }

        protected void rgPreviousApps_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Application Status Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Application Status Saved</div>";
                Response.Redirect("~/Application/ApplicatonStatus", false);
            }
        }

        private async Task<bool> SaveForm()
        {
            try
            {
                if(Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                {
                    return true;
                }
                //ToDo Check Validation
                dvError.InnerText = "";
                string errorMessage = "";
                bool isValid = true;

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
                    errorMessage += "Counselor Signature is needed.<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new FGApplications();
                model.ApplicationId = new Guid(hfApplicationId.Value);
                model.AddressId = new Guid(hfAddressId.Value);
                model.FiscalYear = Convert.ToInt16(hfFiscalYear.Value);
                model.AppStatus = Convert.ToInt16(ddlUpdateStatus.SelectedValue);
                model.GrantedAmount = Convert.ToDecimal(txtGrantedAmount.DbValue);
                model.ApplicationNotes = txtNotes.Text;
                model.LastStatusChange = DateTime.Now;

                bool retVal = await fgAppService.UpdateApplication(model);

                if (retVal == true)
                {
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
                        signature.FromReview = false;
                        signature.FromStatus = true;
                        signature.PrintedName = txtReviewer.Text;
                        signature.Signature = txtSignature.Text;
                        signature.SignatureRole = "Counselor";
                        await fgAppService.SaveApplicationSignatures(signature);
                    }
                }

                return retVal;
            }
            catch (Exception ex)

            {
                throw ex;
            }
        }
    }
}