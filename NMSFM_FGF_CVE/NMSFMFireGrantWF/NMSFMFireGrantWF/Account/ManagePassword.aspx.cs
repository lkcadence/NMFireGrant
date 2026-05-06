using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using Telerik.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace NMSFMFireGrantWF.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        private ILogging logger;
private IAccountService accountService;
        private ISystemService systemService;

        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        private void Page_Init(object sender, EventArgs e)
        {
            var userWebModel = new UserWebModel();
            logger = new Logging();
accountService = new AccountService(userWebModel, logger);
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userConnection"] != null)
            {
                var userConnection = System.Web.HttpContext.Current.Session["userConnection"];
                var userContext = new CodepalWebModel(Convert.ToString(userConnection));
                systemService = new SystemService(userContext, logger);
            }
            if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
            {
                Response.Redirect("~/Account/Login");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    changePasswordHolder.Visible = true;
                }
                else
                {
                    setPassword.Visible = true;
                    changePasswordHolder.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");
                }
            }
        }

        protected async void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Guid webUserId = new Guid(Session["WebUserId"].ToString());
                NMSFM.Data.User user = await accountService.GetWebUserByIdAsync(webUserId);
                if (user == null)
                {
                    dvError.InnerHtml = "<div class='alert alert-danger'>Account Not Found</div>";
                    return;
                }
                else
                {
                    if (user.Password != EncryptString(CurrentPassword.Text.Trim()))
                    {
                        dvError.InnerHtml = "<div class='alert alert-danger'>Current Password is incorrect</div>";
                        return;
                    }
                    else
                    {
                        if (NewPassword.Text.Trim() != "")
                        {
                            bool passwordValid = true;
                            if (NewPassword.Text.Length < 6)
                            {
                                passwordValid = false;
                            }
                            if (HasUpper(NewPassword.Text.Trim()) == false)
                            {
                                passwordValid = false;
                            }
                            if (HasNumber(NewPassword.Text.Trim()) == false)
                            {
                                passwordValid = false;
                            }
                            if (passwordValid == false)
                            {
                                dvError.InnerHtml = "<div class='alert alert-danger'>Password must be a minimum of 6 characters, have at least 1 uppercase letter and at least 1 number.</div>";
                            }
                        }
                        else
                        {
                            throw new Exception("Password is Required");
                        }
                        user.Password = EncryptString(NewPassword.Text.Trim());
                        if (await accountService.UpdateExistingUser(user) == true)
                        {
                            Response.Redirect("~/Account/ResetPasswordConfirmation");
                        }
                    }
                }
                
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                //IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
                //if (result.Succeeded)
                //{
                //    var user = manager.FindById(User.Identity.GetUserId());
                //    signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                //    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                //}
                //else
                //{
                //    AddErrors(result);
                //}
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

        private string EncryptString(string baseString)
        {
            byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
            byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            byte[] inputByteArray = System.Text.UTF8Encoding.UTF8.GetBytes(baseString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var encryptString = Convert.ToBase64String(ms.ToArray());

            return encryptString;
        }

        private async Task<string> DecryptString(string encryptedString)
        {
            string baseString = "";
            encryptedString = encryptedString.Replace(" ", "+");
            byte[] byKey = { 7, 2, 0, 1, 5, 6, 7, 8, 9, 1, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 24 };
            byte[] IV = { 1, 0, 2, 4, 1, 9, 7, 5 };
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(encryptedString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            baseString = System.Text.UTF8Encoding.UTF8.GetString(ms.ToArray());
            return baseString;
        }

        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}



