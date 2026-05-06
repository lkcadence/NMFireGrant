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
using NMSFM.Services.CPSystem;
using NMSFM.Services.UDF;
using NMSFM.Services.FireGrant;
using Telerik.Web.UI;
using System.Configuration;

namespace NMSFMFireGrantWF.Account
{
    public partial class ResetPassword : Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IPartyService partyService;
        private IFGService fgService;

        private Emailer emailer;
        protected string StatusMessage
        {
            get;
            private set;
        }

        private string webUserId;

        protected void Page_Init(object sender, EventArgs e)
        {
            var userWebModel = new UserWebModel();
            logger = new Logging();
accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.partyService = new PartyService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.emailer = new Emailer();
            }
            else
            {
                var userConnection = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationModel"].ToString();
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
                this.fpfService = new FPFService(userContext, logger);
                this.partyService = new PartyService(userContext, logger);
                this.fgService = new FGService(userContext, logger);
                this.emailer = new Emailer();
            }
            try
            {
                Session["SessionId"] = Guid.NewGuid(); // Used as the session value for Audit and User records. HttpContext.Session.SessionID is not a GUID.
                Response.Cookies["ASPCookie"].Value = "SomeValue"; // Generating -any- session and cookie value helps most browsers avoid instances where the session/cookie objects aren't loaded yet.
                Session["CodepalUserId"] = "3c15fe68-b359-4c33-b138-90b95d9caea0";
                Session["CodepalUserName"] = "Anonymous Web Registration";
            }
            catch (Exception ex)
            {
                _ = ex;

            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Register New Account", "");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
                if (RouteData.Values["UserId"] != null && RouteData.Values["ForgotPasswordToken"] != null)
                {
                    string userId = RouteData.Values["UserId"].ToString();
                    string forgotPasswordToken = RouteData.Values["ForgotPasswordToken"].ToString();
                    Guid gToken = new Guid(forgotPasswordToken);

                    Guid gdUserId = new Guid(userId);
                    try
                    {
                        var webUser = await accountService.GetWebUserByIdAsync(gdUserId);
                        if (webUser != null && webUser.ForgotPasswordToken == gToken)
                        {
                            hfUserId.Value = userId;
                            hfOldPassword.Value = webUser.Password;
                            webUserId = userId;
                            Email.Text = webUser.Email;
                        }
                        else
                        {
                            throw new Exception("The login link has expired");
                        }

                    }
                    catch (Exception ex)
            {
                _ = ex;
                        ErrorMessage.Text = ex.Message.ToString();
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Login");
                }
            }
        }

        protected async void Reset_Click(object sender, EventArgs e)
        {
            try
            {
                if (Password.Text.Trim() != "")
                {
                    bool passwordValid = true;
                    if (Password.Text.Length < 6)
                    {
                        passwordValid = false;
                    }
                    if (HasUpper(Password.Text.Trim()) == false)
                    {
                        passwordValid = false;
                    }
                    if (HasNumber(Password.Text.Trim()) == false)
                    {
                        passwordValid = false;
                    }
                    if (passwordValid == false)
                    {
                        throw new Exception("Password must be a minimum of 6 characters, have at least 1 uppercase letter and at least 1 number.");
                    }
                }
                else
                {
                    throw new Exception("Password is Required");
                }
                string userId = hfUserId.Value;
                string password = hfOldPassword.Value;
                Guid gdUserId = new Guid(userId);
                var webUser = await accountService.GetWebUserByIdAsync(gdUserId);
                if (webUser != null && webUser.Password == password)
                {
                    string newPassword = Password.Text.Trim();
                    newPassword = await accountService.EncryptStringAsync(newPassword);
                    webUser.Password = newPassword;
                    webUser.ForgotPasswordToken = null;
                    bool isUpdated = await accountService.UpdateExistingUser(webUser);
                    if (isUpdated)
                    {
                        Response.Redirect("~/Account/ResetPasswordConfirmation");
                    }
                }
                else
                {
                    throw new Exception("Web User Not Found");
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                ErrorMessage.Text = ex.Message.ToString();
            }
        }

        private bool HasUpper(string password)
        {
            bool hasUpper = false;
            foreach (char p in password)
            {
                if (Char.IsUpper(p))
                {
                    hasUpper = true;
                }
            }
            return hasUpper;
        }

        private bool HasNumber(string password)
        {
            bool hasNumber = false;
            foreach (char p in password)
            {
                if (Char.IsNumber(p))
                {
                    hasNumber = true;
                }
            }
            return hasNumber;
        }
    }
}





