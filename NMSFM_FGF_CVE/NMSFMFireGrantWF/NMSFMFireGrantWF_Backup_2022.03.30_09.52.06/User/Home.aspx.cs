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

namespace NMSFMFireGrantWF.User
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
            try
            {
                if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
                {
                    Response.Redirect("~/Account/Login");
                }
                if (Session["Role"] == null || Convert.ToString(Session["Role"]) != "External")
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
                    FG_App_Help help = await fgService.GetFGHelpByPage("Home (User)");
                    if (help != null)
                    {
                        helpdiv.InnerHtml = help.HelpText;
                    }

                    FGApplicationSettings appSettings = new FGApplicationSettings();
                    appSettings = await fgService.GetFireGrantAppSettings(Convert.ToInt16(DateTime.Now.Year));
                    if (appSettings != null)
                    {
                        DateTime sDate = appSettings.StartDate;
                        DateTime eDate = appSettings.EndDate;
                        if (DateTime.Now >= sDate && DateTime.Now <= eDate)
                        {
                            pnlNoAccess.Visible = false;
                            pnlUserHome.Visible = true;
                            LoadDepartments();
                        }
                        else
                        {
                            pnlNoAccess.Visible = true;
                            pnlUserHome.Visible = false;
                        }
                    }
                    else
                    {
                        pnlNoAccess.Visible = true;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        private async void LoadDepartments()
        {
            try
            {
                var departments = new List<v_AddressParties>();
                Guid partyId = new Guid(Session["CodepalUserId"].ToString());
                departments = (await fgService.GetFGDepartmentsAsync(partyId)).OrderBy(a => a.AddressCode).ToList();
                var applications = new List<DetailedFGApplication>();

                foreach (v_AddressParties dept in departments)
                {
                    var app = new DetailedFGApplication();
                    app.AddressId = new Guid(dept.AddressId.ToString());
                    app.AddressCode = dept.AddressCode;
                    app.County = (await addressService.GetCountyListAsync()).First(c => c.CountyId == dept.CountyId).County1;

                    nm_FGApplication existingApp = new nm_FGApplication();
                    existingApp = await fgAppService.GetFGApplicationAsync(app.AddressId, Convert.ToInt16(DateTime.Now.Year));
                    if (existingApp != null)
                    {
                        app.ApplicationNumber = existingApp.ApplicationNumber;
                        app.DateSubmitted = existingApp.DateSubmitted;
                        app.LastStatusChange = existingApp.LastStatusChange;
                        app.ApplicationStatus = existingApp.ApplicationStatus;
                        app.ApplicationId = existingApp.ApplicationId;
                    }
                    applications.Add(app);
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
            LoadDepartments();
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
                    Session["ReadOnly"] = false;
                    if ((e.CommandName == "View"))
                    {
                        Session["SaveMessage"] = "";
                        Session["Department"] = itemValue;
                        Session["DepartmentName"] = departmentName;
                        Session["ApplicationId"] = appId;
                        Session["FiscalYear"] = DateTime.Now.Year.ToString();
                        Response.Redirect("~/Application/Instructions");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}