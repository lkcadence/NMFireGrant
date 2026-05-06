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
    public partial class ForgotPassword : Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private ISystemService systemService;
        private IFPFService fpfService;
        private IPartyService partyService;
        private IFGService fgService;

        private Emailer emailer;

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
                FG_App_Help help = await fgService.GetFGHelpByPage("Forgot Password (Account)", "");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
            }
        }

        protected async void Forgot(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            if (IsValid)
            {
                //// Validate the user's email address
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //ApplicationUser user = manager.FindByName(Email.Text);
                //if (user == null || !manager.IsEmailConfirmed(user.Id))
                //{
                //    FailureText.Text = "The user either does not exist or is not confirmed.";
                //    ErrorMessage.Visible = true;
                //    return;
                //}
                //// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //// Send email with the code and the redirect to reset password page
                //string code = manager.GeneratePasswordResetToken(user.Id);
                //string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                //manager.SendEmail(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                try
                {
                    var webUser = await accountService.GetWebUserByEmailAsync(Email.Text.ToString());

                    if (webUser != null)
                    {
                        Guid forgotPasswordToken = Guid.NewGuid();
                        webUser.ForgotPasswordToken = forgotPasswordToken;
                        await accountService.UpdateExistingUser(webUser);

                        string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "http://firegranttest.vscomptech.com";
                        string from = (ConfigurationManager.AppSettings["DefaultEmailSender"] != null) ? ConfigurationManager.AppSettings["DefaultEmailSender"].ToString() : "vance@vscomptech.com";
                        string recoveryUrl = "<a href='" +  url + "/ResetUserPassword/" + webUser.UserId + "/" + forgotPasswordToken + "'>Reset Password</a>";
                        string subject = "Reset Password for NMSFM Fire Grant Application";
                        string body = "To reset your password for the NMSFM Fire Grant Application click the following link: <br /><br />" + recoveryUrl;
                        emailer.SendMailMessage(from, webUser.Email, "", "", subject, body);

                        loginForm.Visible = false;
                        DisplayEmail.Visible = true;
                    }
                    else
                    {
                        throw new Exception("There is no user with that email address.");
                    }
                }
                catch (Exception ex)
            {
                _ = ex;
                    ErrorMessage.Visible = true;
                    FailureText.Text = ex.Message.ToString();
                }
                
            }
        }
    }
}





