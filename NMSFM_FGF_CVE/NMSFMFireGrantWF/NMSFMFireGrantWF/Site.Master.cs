using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Web.UI.HtmlControls;
using NMSFM.Services.FireGrant;
using System.Configuration;

namespace NMSFMFireGrantWF
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        private Emailer emailer;

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

            this.emailer = new Emailer();

            Page.PreLoad += master_Page_PreLoad;
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
                    //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
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
                    lnkSearchHelp.Visible = false;
                    Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    Session.Abandon();
                }
                else
                {
                    lnkSearchHelp.Visible = true;
                }
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
        }

        protected void btnSendTA_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string error = "";
                bool errBol = false;
                if (txtTAFromName.Text == "")
                {
                    errBol = true;
                    error += "From Name is Required<br />";
                }
                if (txtTAFromEmail.Text == "")
                {
                    errBol = true;
                    error += "From Email is Required<br />";
                }
                else
                {
                    if (emailer.EmailIsValid(txtTAFromEmail.Text) == false)
                    {
                        errBol = true;
                        error += "From Email must be valid email<br />";
                    }
                }
                if (txtTADetails.Text == "")
                {
                    errBol = true;
                    error += "Details Required<br />";
                }
                if (errBol)
                {
                    throw new Exception(error);
                }    

                string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "http://firegranttest.vscomptech.com";
                string from = (ConfigurationManager.AppSettings["DefaultEmailSender"] != null) ? ConfigurationManager.AppSettings["DefaultEmailSender"].ToString() : "vance@vscomptech.com";
                string to = (ConfigurationManager.AppSettings["TechnicalSupportEmail"] != null) ? ConfigurationManager.AppSettings["TechnicalSupportEmail"].ToString() : "vance@vscomptech.com";
                string subject = "Technical Support Request from NMSFM Fire Grant Application";
                string body = txtTAFromName.Text + " (" + txtTAFromEmail.Text + ") has subitted a request for Technical Assistance. <br /><br />Details: <br />" + txtTADetails.Text;
                emailer.SendMailMessage(from, to, "", "", subject, body);
                dvEmailSuccess.InnerHtml = "<div class='alert alert-danger'>Email Sent</div>"; ;
                dvEmailSuccess.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblTAError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showTAModal();", true);
            }
        }

        protected void bntSendFS_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string error = "";
                bool errBol = false;
                if (txtFSFromName.Text == "")
                {
                    errBol = true;
                    error += "From Name is Required<br />";
                }
                if (txtFSFromEmail.Text == "")
                {
                    errBol = true;
                    error += "From Email is Required<br />";
                }
                else
                {
                    if (emailer.EmailIsValid(txtFSFromEmail.Text) == false)
                    {
                        errBol = true;
                        error += "From Email must be valid email<br />";
                    }
                }
                if (txtFSDetails.Text == "")
                {
                    errBol = true;
                    error += "Details Required<br />";
                }
                if (errBol)
                {
                    throw new Exception(error);
                }
                string url = (ConfigurationManager.AppSettings["ApplicationUrl"] != null) ? ConfigurationManager.AppSettings["ApplicationUrl"].ToString() : "http://firegranttest.vscomptech.com";
                string from = (ConfigurationManager.AppSettings["DefaultEmailSender"] != null) ? ConfigurationManager.AppSettings["DefaultEmailSender"].ToString() : "vance@vscomptech.com";
                string to = (ConfigurationManager.AppSettings["AccountEmailApprovers"] != null) ? ConfigurationManager.AppSettings["AccountEmailApprovers"].ToString() : "vance@vscomptech.com";
                string subject = "Fire Services Support Request from NMSFM Fire Grant Application";
                string body = txtFSFromName.Text + " (" + txtTAFromEmail.Text + ") has subitted a request for Fire Services Support. <br /><br />Details: <br />" + txtTADetails.Text;
                emailer.SendMailMessage(from, to, "", "", subject, body);
                dvEmailSuccess.InnerHtml = "<div class='alert alert-danger'>Email Sent</div>"; ;
                dvEmailSuccess.Focus();
            }
            catch (Exception ex)
            {
                _ = ex;
                lblFSError.Text = "<div class='alert alert-danger'>" + ex.Message.ToString() + "</div>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showFSModal();", true);
            }
        }
    }

}


