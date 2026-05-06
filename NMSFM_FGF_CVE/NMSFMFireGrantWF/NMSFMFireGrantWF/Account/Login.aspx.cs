using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using NMSFMFireGrantWF.Models;
using NMSFM.Data;
using NMSFM.Services.Logging;
using NMSFM.Services.Address;
using NMSFM.Services.Account;
using NMSFM.Services.FireGrant;
using NMSFM.Services.Menu;
using NMSFM.Services.CPSystem;
using NMSFM.ViewModels;

namespace NMSFMFireGrantWF.Account
{
    public partial class Login : Page
    {
        private ILogging logger;
        private IAddressService addressService;
private IAccountService accountService;
        private IFGApplicationServices appService;
        private IFGService fgService;
        private string loginError;
        private string signatorPassword = "SignatorPassword2021!";
        private string signatorWebUserId = "36fc8581-71ce-408b-b341-f2be0ab0ae6a";

        protected void Page_Init(object sender, EventArgs e)
        {
            var userWebModel = new UserWebModel();
            logger = new Logging();
accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                this.fgService = new FGService(userContext, logger);
                this.addressService = new AddressService(userContext, logger);
            }
            else
            {
                var userConnection = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationModel"].ToString();
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                this.fgService = new FGService(userContext, logger);
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            Page.Title = "Login";
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                //RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
            if (!Page.IsPostBack)
            {
                HtmlGenericControl helpdiv = new HtmlGenericControl();
                helpdiv = (HtmlGenericControl)Master.FindControl(id: "dvPageHelp");
                FG_App_Help help = await fgService.GetFGHelpByPage("Login (Account)", "");
                if (help != null)
                {
                    helpdiv.InnerHtml = help.HelpText;
                }
                if (RouteData.Values["ApplicationId"] != null && RouteData.Values["LoginToken"] != null)
                {
                    string applicationId = RouteData.Values["ApplicationId"].ToString();
                    string loginToken = RouteData.Values["LoginToken"].ToString();
                    loginToken = accountService.EncryptString(loginToken);
                    try
                    {
                        if (await LoginByToken(loginToken, applicationId))
                        {
                            Response.Redirect("~/Application/GeneralInformation", false);
                        }
                    }
                    catch (Exception ex)
            {
                _ = ex;
                        FailureText.Text = ex.Message.ToString();
                        ErrorMessage.Visible = true;
                        ErrorMessage.Focus();
                    }
                }
            }
        }

        protected async Task<bool> LoginByToken(string loginToken, string applicationId)
        {
            bool retbol = false;
            try
            {
                FailureText.Text = "";
                ErrorMessage.Visible = false;
                
                PartyWebAccess party = new PartyWebAccess();
                party.UserName = "SignatorUser";
                party.Password = signatorPassword;
                if (await Authenticate(party) == true)
                {
                    Session["Role"] = "Signator";
                    Session["WebUserId"] = signatorWebUserId;
                    FG_App_Signatures signator = await appService.GetSignatorByToken(applicationId, loginToken);
                    if (signator != null)
                    {
                        FGApplications app = await appService.GetFGApplicationById(signator.ApplicationId);
                        if (app != null)
                        {
                            v_Addresses2 deptAddress = new v_Addresses2();
                            deptAddress = await addressService.GetAddressByIdAsync(app.AddressId);
                            if (deptAddress != null)
                            {
                                Session["Department"] = deptAddress.AddressId.ToString();
                                Session["ApplicationId"] = app.ApplicationId;
                                Session["FiscalYear"] = app.FiscalYear.ToString();
                                Session["LoginToken"] = loginToken;
                                retbol = true;
                            }
                            else
                            {
                                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                Session.Abandon();
                            }
                        }
                        else
                        {
                            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            Session.Abandon();
                        }
                    }
                    else
                    {
                        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        Session.Abandon();
                    }
                }
                else
                {
                    FailureText.Text = loginError;
                    ErrorMessage.Visible = true;
                    ErrorMessage.Focus();
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                throw ex;
            }
            return retbol;
        }

        protected async void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                FailureText.Text = "";
                ErrorMessage.Visible = false;
                //// Validate the user password
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                //// This doen't count login failures towards account lockout
                //// To enable password failures to trigger lockout, change to shouldLockout: true
                //var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                //switch (result)
                //{
                //    case SignInStatus.Success:
                //        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //        break;
                //    case SignInStatus.LockedOut:
                //        Response.Redirect("/Account/Lockout");
                //        break;
                //    case SignInStatus.RequiresVerification:
                //        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                //                                        Request.QueryString["ReturnUrl"],
                //                                        RememberMe.Checked),
                //                          true);
                //        break;
                //    case SignInStatus.Failure:
                //    default:
                //        FailureText.Text = "Invalid login attempt";
                //        ErrorMessage.Visible = true;
                //        break;
                //}
                PartyWebAccess party = new PartyWebAccess();
                party.UserName = Email.Text;
                party.Password = Password.Text;
                if (await Authenticate(party) == true)
                {
                    string role = Session["Role"].ToString();
                    switch (role) {
                        case "Internal":
                            Response.Redirect("~/Admin/Home", false);
                            break;
                        case "External":
                            Response.Redirect("~/User/Home", false);
                            break;
                        case "Manager":
                            Response.Redirect("~/Account/ManageUsers", false);
                            break;
                        default:
                            Response.Redirect("~/Default");
                            break;
                    }
                }
                else
                {
                    FailureText.Text = loginError;
                    ErrorMessage.Visible = true;
                    ErrorMessage.Focus();
                }
            }
        }

        public async Task<bool> Authenticate(PartyWebAccess viewModel) // Upon clicking 'login'.
        {
            bool retval = false;
            Session["SessionId"] = Guid.NewGuid(); // Used as the session value for Audit and User records. HttpContext.Session.SessionID is not a GUID.
            Response.Cookies["ASPCookie"].Value = "SomeValue"; // Generating -any- session and cookie value helps most browsers avoid instances where the session/cookie objects aren't loaded yet.
            if (ModelState.IsValid)
            {
                string encryptedString = await accountService.EncryptStringAsync(viewModel.Password);
                var webUser = await accountService.GetWebUserByInfoAsync(viewModel.UserName, encryptedString);

                if (webUser == null || webUser.ConnectionString == null || webUser.Inactive == true)
                {
                    loginError = "Username or Password was incorrect. Please try again.";
                    return false;
                }

                var loginRecordUpdated = await accountService.UserLoginAsync(webUser.UserId);
                if (loginRecordUpdated == false)
                {
                    loginError = "nmsfmfirefundapp has encountered a problem starting the login session. Please contact your administrator.";
                    return false;
                }

                //Encrypted Connection String Code
                string decryptedConnectionString = await accountService.DecryptString(webUser.ConnectionString.ToString());
                Session["WebUserId"] = webUser.UserId; // Used when checking Login records.
                Session["userConnection"] = decryptedConnectionString; // Used to define the connection to the user's DB.      	
                Session["userConnectionEncrypted"] = webUser.ConnectionString;
                var userContext = new CodepalWebModel(decryptedConnectionString);
                //End Encrypted Connection String Code

                //Session["WebUser"] = webUser;
                //Session["WebUserId"] = webUser.UserId; // Used when checking Login records.
                //Session["userConnection"] = webUser.ConnectionString; // Used to define the connection to the user's DB.      
                //var userContext = new CodepalWebModel(webUser.ConnectionString);
                this.addressService = new AddressService(userContext, logger);
                this.appService = new FGApplicationService(userContext, logger);

                if (webUser.IsWebAdmin == true)
                {
                    if (await MicrosoftIdentityLogin(webUser.Login, webUser.Password, "Manager"))
                    {
                        Session["WebUser"] = webUser.Login;
                        Session["Role"] = "Manager";
                        Session["IsWebAdmin"] = true;
                        //Response.Redirect("~/Account/ManageUsers");
                    }
                    else
                    {
                        loginError = "Codepal Online Viewer has encountered a problem logging in the internal user. Please contact your administrator.";
                        return false;
                    }
                }
                var inspector = await addressService.GetInspectorByIdAsync(webUser.CodepalId.Value);
                if (inspector != null) //Internal User with matching login was found
                {
                    if (inspector.Inactive == null || inspector.Inactive == false)
                    {
                        if (await MicrosoftIdentityLogin(webUser.Login, webUser.Password, "Internal"))
                        {
                            Session["AgencyId"] = inspector.AgencyId;
                            Session["CodepalUserId"] = inspector.InspectorId;
                            Session["CodepalUserName"] = inspector.InspectorName;
                            Session["CodepalUserLogin"] = inspector.Login;
                            Session["WebUser"] = webUser.Login;
                            Session["Role"] = "Internal";
                            if (webUser.NMFGC == true)
                            {
                                Session["IsWebAdmin"] = false;
                            }
                            else
                            {
                                Session["IsWebAdmin"] = true;
                            }
                            Session["ReadOnly"] = webUser.Readonly;
                            retval = true;

                            //return RedirectToAction("AllowableDistribution", "Distribution");
                            //Response.Redirect("~/Admin/Home");
                        }
                        else
                        {
                            loginError = "Codepal Online Viewer has encountered a problem logging in the internal user. Please contact your administrator.";
                            return false;
                        }
                    }
                    else
                    {
                        loginError = "This internal account is locked. Please contact your administrator.";
                        return false;
                    }
                }
                else //Search for External User
                {
                    var user = await addressService.GetPartyWebAccessByIdAsync(webUser.CodepalId.Value);
                    if (user != null)
                    {
                        if (user.Inactive != true)
                        {
                            if (await MicrosoftIdentityLogin(webUser.Login, webUser.Password, "External"))
                            {
                                Session["AgencyId"] = user.AgencyId;
                                Session["CodepalUserId"] = user.PartyID;
                                Session["CodepalUserName"] = user.PartyName;
                                Session["CodePalEmailAddress"] = user.Email;
                                Session["WebUser"] = webUser.Login;
                                Session["Role"] = "External";
                                Session["IsWebAdmin"] = false;
                                retval = true;

                                //Response.Redirect("~/User/Home");
                            }
                            else
                            {
                                loginError = "Codepal Online Viewer has encountered a problem logging in the external user. Please contact your administrator.";
                                return false;
                            }
                        }
                        else
                        {
                            loginError = "This external account is inactive. Please contact your administrator.";
                            return false;
                        }
                    }
                    else
                    {
                        if (webUser.IsWebAdmin == false)
                        {
                            loginError = "No user is linked to this username or password. Please contact your administrator.";
                            return false;
                        }
                    }
                }
            }
            //Response.Redirect("~/Account/Login");
            return retval;
        }

        private async Task<bool> MicrosoftIdentityLogin(string userName, string password, string role)
        {
            // Microsoft Identity will automatically use the "DefaultConnection" database.
            var userStore = new UserStore<IdentityUser>();
            var roleStore = new RoleStore<IdentityRole>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Internal")) // If this is the first login with Identity, create roles in DB.
            {
                var internalRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                internalRole.Name = "Internal";
                roleManager.Create(internalRole);
                var externalRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                externalRole.Name = "External";
                roleManager.Create(externalRole);
                var managerRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                managerRole.Name = "Manager";
                roleManager.Create(managerRole);
            }

            if (role == "Manager")
            {
                userName = userName + "MgrAcct00048e2966f64211b792e670cff2e442";
                password = password + "MgrAcct316ea7a765641478cf2234eb46c7db7";
            }

            var identityUser = userManager.Find(userName, password);

            if (identityUser == null)
            {
                identityUser = userManager.FindByName(userName);
                if (identityUser != null)
                {
                    var newPassword = userManager.PasswordHasher.HashPassword(password);
                    await userStore.SetPasswordHashAsync(identityUser, newPassword);
                    await userStore.UpdateAsync(identityUser);
                }
                else
                {
                    identityUser = new IdentityUser() { UserName = userName };
                    IdentityResult result = userManager.Create(identityUser, password);
                    if (result.Succeeded == false)
                    {
                        return false;
                    }
                }
            }
            if (!userManager.IsInRole(identityUser.Id, role))
            {
                await userManager.AddToRoleAsync(identityUser.Id, role);
            }
            var otherRole = "";
            if (role != "Manager")
            {
                otherRole = (role == "Internal") ? "External" : "Internal";
                if (userManager.IsInRole(identityUser.Id, otherRole))
                {
                    await userManager.RemoveFromRoleAsync(identityUser.Id, otherRole);
                }
            }

            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            var userIdentity = userManager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
            return true;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Register");
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Forgot");
        }

        //private string EncryptString(string baseString)
        //{
        //    byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
        //    byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
        //    TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        //    byte[] inputByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    var encryptString = Convert.ToBase64String(ms.ToArray());

        //    return encryptString;
        //}

        //private async Task<string> DecryptString(string encryptedString)
        //{
        //    string baseString = "";
        //    encryptedString = encryptedString.Replace(" ", "+");
        //    byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
        //    byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
        //    TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        //    byte[] inputByteArray = Convert.FromBase64String(encryptedString);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    baseString = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
        //    return baseString;
        //}
    }
}





