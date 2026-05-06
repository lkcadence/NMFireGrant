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
    public partial class FundingJustification : System.Web.UI.Page
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

            }
        }

         async void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Funding Justification (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Funding Justification";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

                if (!Page.IsPostBack)
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        FG_App_FundingJustification fundingJustification = new FG_App_FundingJustification();
                        fundingJustification = await fgAppService.GetFGApplicationFundingJustificationAsync(appIdGuid);
                        if (fundingJustification != null && fundingJustification.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadFundingJustification(fundingJustification);
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
                    }
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
            else if (con is RadEditor)
            {
                RadEditor ed = (RadEditor)con;
                ed.Enabled = false;
            }
            btnSave.Visible = false;
            if (dvAdmin.Visible)
            {
                txtComments.ReadOnly = false;
                txtBenefitGrade.ReadOnly = false;
                txtConsequencesGrade.ReadOnly = false;
                txtFinancialNeedGrade.ReadOnly = false;
                txtProblemGrade.ReadOnly = false;
            }
        }

        private void LoadFundingJustification(FG_App_FundingJustification model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    dvError.InnerHtml =  "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                }
                if (model.CriticalNeed == 1) { rbCriticalNeedYes.Checked = true; }
                if (model.CriticalNeed == 2) { rbCriticalNeedNo.Checked = true; }
                txtFinancialNeed.Content = model.FinancialNeed.ToString();
                txtProblem.Content = model.Problem.ToString();
                txtBenefit.Content = model.BenefitToCommunity.ToString();
                txtConsequences.Content = model.Consequences.ToString();
                txtFinancialNeedGrade.Text = model.FinancialNeedGrade.ToString();
                txtProblemGrade.Text = model.ProblemGrade.ToString();
                txtBenefitGrade.Text = model.BenefitGrade.ToString();
                txtConsequencesGrade.Text = model.ConsequencesGrade.ToString();
                txtComments.Text = (model.AdminComments != null) ? model.AdminComments.ToString() : "";
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
                        //Response.Redirect("~/Application/FundingJustification", false);
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


        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/EquipmentNeeds", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Funding Justification Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Funding Justification Data Saved</div>";
                Response.Redirect("~/Application/FundingJustification", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/ProjectBudgetSheet", false);
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

                int criticalNeed = 0;
                if (rbCriticalNeedYes.Checked) { criticalNeed = 1; }
                if (rbCriticalNeedNo.Checked) { criticalNeed = 2; }
                if (criticalNeed == 0)
                {
                    errorMessage += "Critical Need response is Required.<br />";
                    isValid = false;
                }

                if (txtFinancialNeed.Content.Length < 1)
                {
                    errorMessage += "Financial need response is Required.<br />";
                    isValid = false;
                }

                if (txtProblem.Content.Length < 1)
                {
                    errorMessage += "Problem response is Required.<br />";
                    isValid = false;
                }

                if (txtBenefit.Content.Length < 1)
                {
                    errorMessage += "Benefit to Community response is Required.<br />";
                    isValid = false;
                }

                if (txtConsequences.Content.Length < 1)
                {
                    errorMessage += "Consequences response is Required.<br />";
                    isValid = false;
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new FG_App_FundingJustification();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();

                model.CriticalNeed = criticalNeed;
                model.FinancialNeed = txtFinancialNeed.Content.ToString();
                model.Problem = txtProblem.Content.ToString();
                model.BenefitToCommunity = txtBenefit.Content.ToString();
                model.Consequences = txtConsequences.Content.ToString();

                model.AdminComments = txtComments.Text;
                model.FinancialNeedGrade = Convert.ToInt32(txtFinancialNeedGrade.DbValue);
                model.ProblemGrade = Convert.ToInt32(txtProblemGrade.DbValue);
                model.BenefitGrade = Convert.ToInt32(txtBenefitGrade.DbValue);
                model.ConsequencesGrade = Convert.ToInt32(txtConsequencesGrade.DbValue);

                bool retVal = await fgAppService.SaveFundingJustificationAsync(model);

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