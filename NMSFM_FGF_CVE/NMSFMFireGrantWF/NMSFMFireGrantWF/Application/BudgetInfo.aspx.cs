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
    public partial class BudgetInfo : System.Web.UI.Page
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
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl helpdiv = new HtmlGenericControl();
            helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
            FG_App_Help help = await fgService.GetFGHelpByPage("Budget Info (Application)");
            if (help != null)
            {
                helpdiv.InnerHtml = help.HelpText;
            }

            Label lblTheTitle;
            lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
            lblTheTitle.Text = "Budget Information";
            _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
            _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);

            if (!Page.IsPostBack)
            {
                try
                {
                    string appId = Session["ApplicationId"].ToString();
                    hfApplicationId.Value = appId;
                    if (appId != null)
                    {
                        Guid appIdGuid = new Guid(appId);
                        FG_App_BudgetInfo budgetInfo = new FG_App_BudgetInfo();
                        budgetInfo = await fgAppService.GetFGApplicationBudgetInfoAsync(appIdGuid);
                        if (budgetInfo != null && budgetInfo.Id.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            LoadBudgetInfoData(budgetInfo);
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
                catch (Exception ex)
            {
                _ = ex;
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                }
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
            btnSave.Visible = false;
        }


        private void LoadBudgetInfoData(FG_App_BudgetInfo model)
        {
            try
            {
                if (model.IsValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + model.InvalidText + "</div>";
                }
                txtOperatingBudget.Text = model.OperatingBudget.ToString();
                txtCurrentDistribution.Text = model.FPFDistribution.ToString();
                txtStipendCarryover.Text = model.StipendCarryover.ToString();
                txtCurrentCarryoverBal.Text = model.CarryoverBalance.ToString();
                txtCarryoverPurpose.Text = model.CarryoverPurpose.ToString();
                txtTaxesPer.Text = model.PerTaxes.ToString();
                txtGrantsPer.Text = model.PerGrants.ToString();
                txtSFMFundsPer.Text = model.PerStateFMFunds.ToString();
                txtDonationsPer.Text = model.PerDonations.ToString();
                txtFundDrivesPer.Text = model.PerFundDrives.ToString();
                txtFeeForServicePer.Text = model.PerFeeForService.ToString();
                txtOthers.Text = model.PerOthers.ToString();
                txtOtherExp.Text = model.OthersDesc.ToString();
                txtTotalPer.Text = model.PerTotal.ToString();
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
                        //Response.Redirect("~/Application/BudgetInfo", false);
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

        protected async void btnBack_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/GeneralInformation", false);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                //if (dvError.InnerText == "")
                //{
                //    dvError.InnerHtml = "<div class='alert alert-success'>Budget Information Data Saved</div>";
                //}
                Session["SaveMessage"] = "<div class='alert alert-success'>Budget Information Saved</div>";
                Response.Redirect("~/Application/BudgetInfo", false);
            }
        }

        protected async void btnNext_Click(object sender, EventArgs e)
        {
            if (await SaveForm() == true)
            {
                Response.Redirect("~/Application/CommunityInfo", false);
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
                if (txtOperatingBudget.Text == "" || Convert.ToDecimal(txtOperatingBudget.DbValue) < 1)
                {
                    errorMessage += "Fire Department Operations Budget amount is Required.<br />";
                    isValid = false;
                }
                if (txtCurrentDistribution.Text == "" || Convert.ToDecimal(txtCurrentDistribution.DbValue) < 1)
                {
                    errorMessage += "Current Fire Protection Fund amount is Required.<br />";
                    isValid = false;
                }
                if (txtStipendCarryover.Text == "" || Convert.ToDecimal(txtStipendCarryover.DbValue) < 0)
                {
                    errorMessage += "Stipent Carryover amount is Required.<br />";
                    isValid = false;
                }
                else
                {
                    //if (Convert.ToDecimal(txtStipendCarryover.DbValue) > 25000)
                    //{
                    //    errorMessage += "Stipent Carryover amount cannot exceed $25,000<br />";
                    //    isValid = false;
                    //}
                }
                if (txtCurrentCarryoverBal.Text == "")
                {
                    errorMessage += "Current Carryover Balance amount is Required.<br />";
                    isValid = false;
                }
                if (Convert.ToDecimal(txtCurrentCarryoverBal.DbValue) >  0)
                {
                    if (txtCarryoverPurpose.Text == "")
                    {
                        errorMessage += "Carryover Purpose is Required.<br />";
                        isValid = false;
                    }
                }
                
                if (Convert.ToDecimal(txtOthers.DbValue) > 0)
                {
                    if (txtOtherExp.Text == "")
                    {
                        errorMessage += "Others Budget Description is Required.<br />";
                        isValid = false;
                    }
                }
                decimal totalPer = 0;
                try
                {
                    totalPer = Convert.ToDecimal(txtTaxesPer.DbValue) + Convert.ToDecimal(txtDonationsPer.DbValue) + Convert.ToDecimal(txtFeeForServicePer.DbValue) + Convert.ToDecimal(txtFundDrivesPer.DbValue) + Convert.ToDecimal(txtGrantsPer.DbValue) + Convert.ToDecimal(txtSFMFundsPer.DbValue) + Convert.ToDecimal(txtOthers.Text);
                }
                catch
                {

                }
                if (totalPer == 0)
                {
                    errorMessage += "Annual Budget percentages are Required.<br />";
                    isValid = false;
                }
                else
                {
                    if (totalPer != 100)
                    {
                        errorMessage += "Annual Budget percentages must total up to 100%.<br />";
                        isValid = false;
                    }
                }

                if (isValid == false)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>" + errorMessage + "</div>";
                }

                var model = new FG_App_BudgetInfo();

                model.ApplicationId = new Guid(hfApplicationId.Value);

                model.IsValid = isValid;
                model.InvalidText = errorMessage;
                model.UpdatedBy = Session["WebUser"].ToString();
                model.OperatingBudget = Convert.ToDecimal(txtOperatingBudget.DbValue);
                model.FPFDistribution = Convert.ToDecimal(txtCurrentDistribution.DbValue);
                model.StipendCarryover = Convert.ToDecimal(txtStipendCarryover.DbValue);
                model.CarryoverBalance = Convert.ToDecimal(txtCurrentCarryoverBal.DbValue);
                model.CarryoverPurpose = txtCarryoverPurpose.Text;
                model.PerTaxes = Convert.ToDecimal(txtTaxesPer.DbValue);
                model.PerGrants = Convert.ToDecimal(txtGrantsPer.DbValue);
                model.PerStateFMFunds = Convert.ToDecimal(txtSFMFundsPer.DbValue);
                model.PerDonations = Convert.ToDecimal(txtDonationsPer.DbValue);
                model.PerFundDrives = Convert.ToDecimal(txtFundDrivesPer.DbValue);
                model.PerFeeForService = Convert.ToDecimal(txtFeeForServicePer.DbValue);
                model.PerOthers = Convert.ToDecimal(txtOthers.DbValue);
                model.OthersDesc = txtOtherExp.Text;
                model.PerTotal = totalPer;

                bool retVal = await fgAppService.SaveBudgetInformationAsync(model);

                return retVal;
            }
            catch (Exception ex)
            {
                _ = ex;
                dvError.InnerHtml = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                return false;
            }
            
        }
    }
}






