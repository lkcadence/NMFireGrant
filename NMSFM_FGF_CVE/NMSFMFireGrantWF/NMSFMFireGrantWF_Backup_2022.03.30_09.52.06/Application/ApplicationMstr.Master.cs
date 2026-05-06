using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
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
    public partial class ApplicationMstr : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        private ILogging logger;
        private IAddressService addressService;
        private IAccountService accountService;
        private ISystemService systemService;
        private IFGService fgService;
        private IFGApplicationServices fgAppService;

        protected void Page_Init(object sender, EventArgs e)
        {

            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
            logger = new Logging();
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.fgAppService = new FGApplicationService(userContext, logger);
            }
            else
            {
                this.addressService = null;
            }
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            spVersion.InnerHtml = "1.0.0";
            spVersionDate.InnerHtml = DateTime.Now.ToLongDateString();
            if (!Page.IsPostBack)
            {
                //Check Session (vwd)
                if (Session["WebUserId"] == null)
                {
                    Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    Session.Abandon();
                }               
            }
            LoadApplicationMenu();
        }

        private void LoadApplicationMenu()
        {
            try
            {
                var app = new DetailedFGApplication();
                Guid addressId = new Guid(Session["Department"].ToString());
                short fiscalYear = Convert.ToInt16(Session["FiscalYear"]);
                nm_FGApplication existingApp = new nm_FGApplication();
                FG_App_GeneralInfo generalInfo = new FG_App_GeneralInfo();
                FG_App_BudgetInfo budgetInfo = new FG_App_BudgetInfo();
                FG_App_CommunityInfo communityInfo = new FG_App_CommunityInfo();
                FG_App_ResponseHistory responseHistory = new FG_App_ResponseHistory();
                FG_App_WaterAvailability waterAvailability = new FG_App_WaterAvailability();
                FG_App_Training training = new FG_App_Training();
                FG_App_Apparatus apparatus = new FG_App_Apparatus();
                FG_App_Communication communication = new FG_App_Communication();
                FG_App_HazardsThreats hazardsThreats = new FG_App_HazardsThreats();
                FG_App_PPE ppe = new FG_App_PPE();
                FG_App_EquipmentNeeds equipmentNeeds = new FG_App_EquipmentNeeds();
                FG_App_FundingJustification fundingJustification = new FG_App_FundingJustification();
                FG_App_ProjectBudget projectBudget = new FG_App_ProjectBudget();
                FG_App_DocsSigs docsSigs = new FG_App_DocsSigs();
                existingApp = fgAppService.GetFGApplication(addressId, fiscalYear);

                //Load Application Status Menu
                if (Session["Role"].ToString() == "Internal")
                {
                    appMenuAppStatus.Visible = true;
                    rmAppStatus.Visible = true;
                }
                else
                {
                    if (existingApp != null && existingApp.AppStatus != 6)
                    {
                        appMenuAppStatus.Visible = true;
                        rmAppStatus.Visible = true;
                        rmAppStatus.Items[1].Visible = false;
                    }
                    else
                    {
                        appMenuAppStatus.Visible = false;
                        rmAppStatus.Visible = false;
                    }
                }

                if (existingApp != null)
                {
                    if (Session["Department"] != null && Session["FiscalYear"] != null)
                    {
                        spSubTitle.InnerText = Session["DepartmentName"].ToString() + " - FY" + Session["FiscalYear"].ToString() + " (Application Status: " + existingApp.ApplicationStatus + ")"; ;
                    }
                    if (existingApp.InstructionsSubmitted)
                    {
                        rmStep1.Items[0].ImageUrl = "../Content/images/tick.png";
                    }
                    else
                    {
                        rmStep1.Items[0].ImageUrl = "../Content/images/cross.png";
                        for (int i = 1; i < rmStep1.Items.Count; i++)
                        {
                            rmStep1.Items[i].Visible = false;
                        }
                    }
                    if (existingApp.ApplicationId != null && existingApp.InstructionsSubmitted)
                    {
                        Guid appId = Guid.Parse(existingApp.ApplicationId.ToString());
                        generalInfo = fgAppService.GetFGApplicationGeneralInfo(appId);
                        if (generalInfo != null)
                        {
                            if (generalInfo.IsValid)
                            {
                                rmStep1.Items[1].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[1].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[1].ImageUrl = "../Content/images/round.png";
                        }
                        budgetInfo = fgAppService.GetFGApplicationBudgetInfo(appId);
                        if (budgetInfo != null)
                        {
                            if (budgetInfo.IsValid)
                            {
                                rmStep1.Items[2].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[2].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[2].ImageUrl = "../Content/images/round.png";
                        }
                        communityInfo = fgAppService.GetFGApplicationCommunityInfo(appId);
                        if (communityInfo != null)
                        {
                            if (communityInfo.IsValid)
                            {
                                rmStep1.Items[3].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[3].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[3].ImageUrl = "../Content/images/round.png";
                        }
                        responseHistory = fgAppService.GetFGApplicationResponseHistory(appId);
                        if (responseHistory != null)
                        {
                            if (responseHistory.IsValid)
                            {
                                rmStep1.Items[4].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[4].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[4].ImageUrl = "../Content/images/round.png";
                        }
                        waterAvailability = fgAppService.GetFGApplicationWaterAvailability(appId);
                        if (waterAvailability != null)
                        {
                            if (waterAvailability.IsValid)
                            {
                                rmStep1.Items[5].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[5].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[5].ImageUrl = "../Content/images/round.png";
                        }
                        training = fgAppService.GetFGApplicationTraining(appId);
                        if (training != null)
                        {
                            if (training.IsValid)
                            {
                                rmStep1.Items[6].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[6].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[6].ImageUrl = "../Content/images/round.png";
                        }
                        apparatus = fgAppService.GetFGApplicationApparatus(appId);
                        if (apparatus != null)
                        {
                            if (apparatus.IsValid)
                            {
                                rmStep1.Items[7].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[7].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[7].ImageUrl = "../Content/images/round.png";
                        }
                        communication = fgAppService.GetFGApplicatioCommunication(appId);
                        if (communication != null)
                        {
                            if (communication.IsValid)
                            {
                                rmStep1.Items[8].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[8].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[8].ImageUrl = "../Content/images/round.png";
                        }
                        hazardsThreats = fgAppService.GetFGApplicationHazardsThreats(appId);
                        if (hazardsThreats != null)
                        {
                            if (hazardsThreats.IsValid)
                            {
                                rmStep1.Items[9].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[9].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[9].ImageUrl = "../Content/images/round.png";
                        }
                        ppe = fgAppService.GetFGApplicationPPE(appId);
                        if (ppe != null)
                        {
                            if (ppe.IsValid)
                            {
                                rmStep1.Items[10].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[10].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[10].ImageUrl = "../Content/images/round.png";
                        }
                        equipmentNeeds = fgAppService.GetFGApplicationEquipmentNeeds(appId);
                        if (equipmentNeeds != null)
                        {
                            if (equipmentNeeds.IsValid)
                            {
                                rmStep1.Items[11].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[11].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[11].ImageUrl = "../Content/images/round.png";
                        }
                        fundingJustification = fgAppService.GetFGApplicationFundingJustification(appId);
                        if (fundingJustification != null)
                        {
                            if (fundingJustification.IsValid)
                            {
                                rmStep1.Items[13].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[13].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[13].ImageUrl = "../Content/images/round.png";
                        }
                        projectBudget = fgAppService.GetFGApplicationProjectBudget(appId);
                        if (projectBudget != null)
                        {
                            if (projectBudget.IsValid)
                            {
                                rmStep1.Items[14].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[14].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[14].ImageUrl = "../Content/images/round.png";
                        }
                        docsSigs = fgAppService.GetApplicationDocsSigs(appId);
                        if (docsSigs != null)
                        {
                            if (docsSigs.IsValid)
                            {
                                rmStep1.Items[16].ImageUrl = "../Content/images/tick.png";
                            }
                            else
                            {
                                rmStep1.Items[16].ImageUrl = "../Content/images/cross.png";
                            }
                        }
                        else
                        {
                            rmStep1.Items[16].ImageUrl = "../Content/images/round.png";
                        }
                    }
                }
                else
                {
                    rmStep1.Items[0].ImageUrl = "../Content/images/cross.png";
                    for (int i = 1; i < rmStep1.Items.Count; i++)
                    {
                        rmStep1.Items[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
        }

        protected void rmStep1_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
        {
            //IPageInterface pageInterface = Page as IPageInterface;
            //if (pageInterface != null)
            //{
            //    pageInterface.DoSomeAction();
            //}
        }
    }
}