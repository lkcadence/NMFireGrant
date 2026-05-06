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
// private IAccountService accountService; // legacy field, currently unused
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
                if (RouteData.Values["PriorApps"] != null)
                {
                    Session["FiscalYear"] = (DateTime.Now.Month > 4) ? (DateTime.Now.Year - 1).ToString() : (DateTime.Now.Year).ToString();
                    hPageHeader.InnerHtml = "Prior Apps";
                }
                else
                {
                    Session["FiscalYear"] = (DateTime.Now.Month < 5) ? (DateTime.Now.Year).ToString() : (DateTime.Now.Year + 1).ToString();
                    hPageHeader.InnerHtml = "User Home";
                }
            }
            catch (Exception ex)
            {
                _ = ex;

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

                    LoadFiscalYears();
                }
                
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        private async void LoadFiscalYears()
        {
            ddlFiscalYear.Items.Clear();
            short fiscalyear = 0;
            fiscalyear = Convert.ToInt16(DateTime.Now.Year + 1);

            for (int y = 2022; y <= fiscalyear; y++)
            {
                DropDownListItem li = new DropDownListItem();
                li.Text = y.ToString();
                li.Value = y.ToString();
                ddlFiscalYear.Items.Add(li);
            }

            if (Session["FiscalYear"] != null)
            {
                try
                {
                    short sfy = Convert.ToInt16(Session["FiscalYear"]);
                    if (sfy == fiscalyear)
                    {
                        ddlFiscalYear.SelectedValue = fiscalyear.ToString();
                        ddlFiscalYear.Enabled = false;
                        FGApplicationSettings appSettings = new FGApplicationSettings();
                        appSettings = await fgService.GetFireGrantAppSettings(fiscalyear);
                        if (appSettings != null)
                        {
                            DateTime sDate = appSettings.StartDate;
                            DateTime eDate = appSettings.EndDate;
                            eDate = eDate.AddHours(23);
                            eDate = eDate.AddMinutes(59);
                            eDate = eDate.AddSeconds(59);
                            if (Session["WebUser"].ToString() == "tuser@test.com")
                            {
                                if  (DateTime.Now > Convert.ToDateTime("6/22/2024") && DateTime.Now < Convert.ToDateTime("6/26/2024"))
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
                        }
                        else
                        {
                            pnlNoAccess.Visible = true;
                        }
                    }
                    else
                    {
                        ddlFiscalYear.Items.Remove(ddlFiscalYear.Items[ddlFiscalYear.Items.Count() - 1]);
                        ddlFiscalYear.SelectedValue = (fiscalyear - 1).ToString();
                        ddlFiscalYear.Enabled = true;
                    }
                }
                catch
                {

                }
            }
            
            LoadDepartments();
            ddlFiscalYear.Focus();
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
                    bool isApp = false;
                    var app = new DetailedFGApplication();
                    app.AddressId = new Guid(dept.AddressId.ToString());
                    app.AddressCode = dept.AddressCode;
                    app.County = (await addressService.GetCountyListAsync()).First(c => c.CountyId == dept.CountyId).County1;

                    nm_FGApplication existingApp = new nm_FGApplication();
                    short fy = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                    
                    existingApp = await fgAppService.GetFGApplicationAsync(app.AddressId, fy);
                    if (existingApp != null)
                    {
                        app.ApplicationNumber = existingApp.ApplicationNumber;
                        app.DateSubmitted = existingApp.DateSubmitted;
                        app.LastStatusChange = existingApp.LastStatusChange;
                        app.ApplicationStatus = existingApp.ApplicationStatus;
                        app.ApplicationId = existingApp.ApplicationId;
                        isApp = true;
                    }
                    if (ddlFiscalYear.Enabled == false)
                    {
                        applications.Add(app);
                    }
                    else
                    {
                        if (isApp)
                        {
                            applications.Add(app);
                        }
                    }
                    
                }
                rgDepartments.DataSource = applications;
                rgDepartments.DataBind();
            }
            catch (Exception ex)
            {
                _ = ex;

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

        protected async void rgDepartments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    short fy = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                    FGApplicationSettings appSettings = new FGApplicationSettings();
                    appSettings = await fgService.GetFireGrantAppSettings(fy);
                    DateTime sDate = Convert.ToDateTime("1/1/2000");
                    DateTime eDate = Convert.ToDateTime("1/1/2000");
                    if (appSettings != null)
                    {
                        sDate = (appSettings.StartDate != null) ? appSettings.StartDate : sDate;
                        eDate = (appSettings.EndDate != null) ? appSettings.EndDate : eDate;
                        eDate = eDate.AddHours(23);
                        eDate = eDate.AddMinutes(59);
                        eDate = eDate.AddSeconds(59);
                    }
                    Session["FiscalYear"] = fy.ToString();
                    var ditem = e.Item as GridDataItem;
                    string itemValue = ditem["AddressId"].Text.ToString();
                    string appId = ditem["ApplicationId"].Text.ToString();
                    string departmentName = ditem["Department"].Text.ToString();
                    string status = ditem["ApplicationStatus"].Text.ToString();
                    Session["ReadOnly"] = false;
                    string eDateStr = eDate.ToString();

                    
                    if (DateTime.Now < sDate || DateTime.Now > eDate)
                    {
                        Session["ReadOnly"] = true;
                    }
                    else
                    {
                        if (status != "In Process" && status != "Reopen" && status != "&nbsp;")
                        {
                            Session["ReadOnly"] = true;
                        }
                    }
                    if (Session["WebUser"].ToString() == "tuser@test.com")
                    {
                        if (DateTime.Now > Convert.ToDateTime("6/22/2024") && DateTime.Now < Convert.ToDateTime("6/26/2024"))
                        {
                            Session["ReadOnly"] = false;
                        }
                    }
                    if ((e.CommandName == "View"))
                    {
                        Session["SaveMessage"] = "";
                        Session["Department"] = itemValue;
                        Session["DepartmentName"] = departmentName;
                        Session["ApplicationId"] = appId;
                        Session["FiscalYear"] = fy;
                        Response.Redirect("~/Application/Instructions");
                    }
                }
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        protected async void ddlFiscalYear_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                short fiscalyear = Convert.ToInt16(ddlFiscalYear.SelectedValue);
                FGApplicationSettings appSettings = new FGApplicationSettings();
                appSettings = await fgService.GetFireGrantAppSettings(fiscalyear);
                //if (appSettings != null)
                //{
                //    DateTime sDate = appSettings.StartDate;
                //    DateTime eDate = appSettings.EndDate;
                //    if (DateTime.Now >= sDate && DateTime.Now <= eDate)
                //    {
                //        pnlNoAccess.Visible = false;
                //        pnlUserHome.Visible = true;
                //        LoadDepartments();
                //    }
                //    else
                //    {
                //        pnlNoAccess.Visible = true;
                //        pnlUserHome.Visible = false;
                //    }
                //}
                //else
                //{
                //    pnlNoAccess.Visible = true;
                //}
                LoadDepartments();
                ddlFiscalYear.Focus();
            }
            catch
            {

            }
        }
    }
}






