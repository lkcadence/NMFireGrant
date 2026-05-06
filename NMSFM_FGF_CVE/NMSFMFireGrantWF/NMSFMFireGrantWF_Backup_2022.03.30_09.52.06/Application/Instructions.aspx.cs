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
    public partial class Instructions : System.Web.UI.Page
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
            }
            catch (Exception ex)
            {

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            
            
            try
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Instructions (Application)");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }

                Label lblTheTitle;
                lblTheTitle = (Label)Master.FindControl(id: "lblTitle");
                lblTheTitle.Text = "Application Instructions";
                _rmStep1 = (RadMenu)Master.FindControl(id: "rmStep1");
                _rmStep1.ItemClick += new RadMenuEventHandler(rmStep1_Click);
                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                if (!Page.IsPostBack)
                {
                    LoadInstructions(fiscalYear);
                    LoadAppInfo();
                    if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                    {
                        btnAccept.Visible = false;
                        btnAccept2.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                dvError.InnerHtml = "<div class='col-md-10'><span class='alert alert-error'>" + ex.Message.ToString() + "</span></div>";
                dvError.Focus();
            }
        }

        private async void LoadInstructions(short fYear)
        {
            try
            {
                FGApplicationSettings result = null;
                result = await fgService.GetFireGrantAppSettings(fYear);
                if (result != null)
                {
                    if (result.ApplicationInstructions != null)
                    {
                        ltrInstructions.Text = result.ApplicationInstructions;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void LoadAppInfo()
        {
            try
            {
                if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
                {
                    return;
                }
                var app = new DetailedFGApplication();
                Guid addressId = new Guid(Session["Department"].ToString());
                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                nm_FGApplication existingApp = new nm_FGApplication();
                existingApp = await fgAppService.GetFGApplicationAsync(addressId, fiscalYear);
                if (existingApp != null)
                {
                    if (existingApp.InstructionsSubmitted)
                    {
                        btnAccept.Text = "Go To Application";
                        btnAccept2.Text = "Go To Application";
                    }
                    if (Session["Role"].ToString() == "External")
                    {
                        if (existingApp.ApplicationStatus == "In Process")
                        {
                            Session["ReadOnly"] = false;
                        }
                        else
                        {
                            Session["ReadOnly"] = true;
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
            switch (_rmStep1.SelectedItem.Text)
            {
                case "Instructions":
                    //Response.Redirect("~/Application/Instructions", false);
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

        protected async void btnAccept_Click(object sender, EventArgs e)
        {
            if (btnAccept.Text == "Go To Application")
            {
                Response.Redirect("~/Application/GeneralInformation", false);
            }
            else
            {
                //ToDo Create the Application
                try
                {
                    if (await SaveForm() == true)
                    {
                        Response.Redirect("~/Application/GeneralInformation", false);
                    }
                    else
                    {
                        dvError.InnerHtml = "<div class='col-md-10'><span class='alert alert-error'>Error Creating New Application</span></div>";
                        dvError.Focus();
                    }
                }
                catch (Exception ex)
                {
                    dvError.InnerHtml = "<div class='col-md-10'><span class='alert alert-error'>" + ex.Message.ToString() + "</span></div>";
                    dvError.Focus();
                }
            }
        }

        private async Task<bool> SaveForm()
        {
            if (Session["ReadOnly"] != null && Convert.ToBoolean(Session["ReadOnly"]) == true)
            {
                return true;
            }
            bool retbol = false;
            string appId = "";
            try
            {
                Guid addressId = new Guid(Session["Department"].ToString());
                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                nm_FGApplication existingApp = new nm_FGApplication();
                existingApp = await fgAppService.GetFGApplicationAsync(addressId, fiscalYear);
                if (existingApp == null)
                {
                    var app = new FGApplications();
                    app.ApplicationId = Guid.NewGuid();
                    app.AddressId = new Guid(Session["Department"].ToString());
                    app.FiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                    app.InstructionsSubmitted = true;
                    retbol = await fgAppService.CreateNewApplication(app);

                    if (retbol)
                    {
                        nm_FGApplication newApp = fgAppService.GetFGApplication(app.AddressId, app.FiscalYear);
                        if (newApp != null)
                        {
                            appId = newApp.ApplicationId.ToString();
                            Session["ApplicationId"] = appId;
                        }
                        else
                        {
                            retbol = false;
                        }
                    }
                    
                }
                else
                {
                    retbol = true;
                }
            }
            catch
            {
                retbol = false;
            }
            return retbol;
        }
    }
}