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

namespace NMSFMFireGrantWF.Admin
{
    public partial class Home : System.Web.UI.Page
    {
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IFGService fgService;
        private IPartyService partyService;
        private IFGApplicationServices fgAppService;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
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
                    this.partyService = new PartyService(userContext, logger);
                    this.fgAppService = new FGApplicationService(userContext, logger);
                }
                else
                {
                    this.addressService = null;
                }

                if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
                {
                    Response.Redirect("~/Account/Login");
                }
                if (Session["Role"] == null || Convert.ToString(Session["Role"]) != "Internal")
                {
                    Response.Redirect("~/Unauthorized");
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
                if (!Page.IsPostBack)
                {
                    HtmlGenericControl helpdiv = new HtmlGenericControl();
                    helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                    FG_App_Help help = await fgService.GetFGHelpByPage("Home (Admin)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }

                    spFiscalYear.InnerHtml = DateTime.Now.Year.ToString();
                    FGApplicationSettings appSettings = new FGApplicationSettings();
                    appSettings = await fgService.GetFireGrantAppSettings(Convert.ToInt16(DateTime.Now.Year));
                    if (appSettings != null)
                    {
                        DateTime sDate = appSettings.StartDate;
                        DateTime eDate = appSettings.EndDate;
                        rdpStartDate.SelectedDate = sDate;
                        rdpEndDate.SelectedDate = eDate;
                    }
                    LoadApplications();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async void LoadApplications()
        {
            try
            {
                var applications = new List<nm_FGApplication>();
                short fy = Convert.ToInt16(DateTime.Now.Year);
                applications = await fgAppService.GetAllFGApplicationByFYAsync(fy);

                if (txtConfNumber.Text != "")
                {
                    applications = applications.Where(a => a.ApplicationNumber.ToLower().Contains(txtConfNumber.Text.ToLower())).ToList();
                }
                if (txtDepartment.Text != "")
                {
                    applications = applications.Where(a => a.AddressCode.ToLower().Contains(txtDepartment.Text.ToLower())).ToList();
                }
                if (txtCounty.Text != "")
                {
                    applications = applications.Where(a => a.County.ToLower().Contains(txtCounty.Text.ToLower())).ToList();
                }
                if (rdpStartDate.SelectedDate != null)
                {
                    applications = applications.Where(a => a.DateSubmitted >= rdpStartDate.SelectedDate || a.DateSubmitted == null).ToList();
                }
                if (rdpEndDate.SelectedDate != null)
                {
                    applications = applications.Where(a => a.DateSubmitted <= rdpEndDate.SelectedDate || a.DateSubmitted == null).ToList();
                }
                rgDepartments.DataSource = applications;
                rgDepartments.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void rgDepartments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }

        protected void rgDepartments_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            LoadApplications();
        }

        protected void rgDepartments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
        }

        protected void rgDepartments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    var ditem = e.Item as GridDataItem;
                    string itemValue = ditem["AddressId"].Text.ToString();
                    string appId = ditem["ApplicationId"].Text.ToString();
                    string departmentName = ditem["Department"].Text.ToString();
                    if ((e.CommandName == "View"))
                    {
                        Session["SaveMessage"] = "";
                        Session["Department"] = itemValue;
                        Session["DepartmentName"] = departmentName;
                        Session["ApplicationId"] = appId;
                        Session["FiscalYear"] = DateTime.Now.Year.ToString();
                        Response.Redirect("~/Application/GeneralInformation");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadApplications();
        }
    }
}