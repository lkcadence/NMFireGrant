using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using NMSFM.Data;
using NMSFM.Services.Account;
using NMSFM.Services.Address;
using NMSFM.Services.CPSystem;
using NMSFM.Services.FireGrant;
using NMSFM.Services.Logging;

namespace NMSFMFireGrantWF.Admin
{
  public partial class EmailSendLog : Page
  {
    private ILogging logger;
    private IAccountService accountService;
    private ISystemService systemService;
    private Emailer emailer;

    protected void Page_Init(object sender, EventArgs e)
    {
      var userWebModel = new UserWebModel();
      logger = new Logging();
      accountService = new AccountService(userWebModel, logger);
      if (Session != null && Session["userConnection"] != null)
      {
        var userContext = new CodepalWebModel(Convert.ToString(Session["userConnection"]));
        systemService = new SystemService(userContext, logger);
        emailer = new Emailer();
      }

      if (Session["WebUserId"] == null || Convert.ToString(Session["WebUserId"]) == "")
      {
        RedirectAndEnd("~/Account/Login");
        return;
      }

      if (Session["Role"] == null || Convert.ToString(Session["Role"]) == "External")
      {
        RedirectAndEnd("~/Unauthorized");
        return;
      }

      if (Session["IsWebAdmin"] == null || Convert.ToBoolean(Session["IsWebAdmin"]) == false)
      {
        RedirectAndEnd("~/Unauthorized");
        return;
      }
    }

    protected async void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        try
        {
          HtmlGenericControl helpdiv = (HtmlGenericControl)Master.FindControl("dvPageHelp");
          if (helpdiv != null && systemService != null)
          {
            var fgService = new FGService(new CodepalWebModel(Convert.ToString(Session["userConnection"])), logger);
            FG_App_Help help = await fgService.GetFGHelpByPage("Email Send Log (Admin)", "");
            if (help != null)
            {
              helpdiv.InnerHtml = help.HelpText;
            }
          }

          await LoadEmailSendLogAsync();
        }
        catch (Exception ex)
        {
          ShowError(ex);
        }
      }
    }

    protected async void btnRefreshEmailLog_Click(object sender, EventArgs e)
    {
      try
      {
        await LoadEmailSendLogAsync();
      }
      catch (Exception ex)
      {
        _ = ex;
        ShowError(ex);
      }
    }

    protected async void chkFailedEmailLogsOnly_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        await LoadEmailSendLogAsync();
      }
      catch (Exception ex)
      {
        _ = ex;
        ShowError(ex);
      }
    }

    protected async void btnPurgeConfirm_Click(object sender, EventArgs e)
    {
      try
      {
        if (systemService == null)
        {
          throw new Exception("Email log service is not available.");
        }

        string selection = ddlPurgeRetention.SelectedValue;
        DateTime cutoff;
        string purgeDescription;

        if (string.Equals(selection, "All", StringComparison.OrdinalIgnoreCase))
        {
          cutoff = DateTime.MaxValue;
          purgeDescription = "entire log";
        }
        else if (!int.TryParse(selection, out int retentionDays) || retentionDays < 1)
        {
          throw new Exception("Select a valid purge option.");
        }
        else
        {
          cutoff = DateTime.Now.AddDays(-retentionDays);
          purgeDescription = "older than " + retentionDays + " days";
        }

        int deleted = await systemService.DeleteEmailSendLogsOlderThanAsync(cutoff);
        dvError.InnerHtml = "<div class='alert alert-success'>Purged " + deleted +
          " email log entries (" + purgeDescription + ").</div>";
        await LoadEmailSendLogAsync();
      }
      catch (Exception ex)
      {
        ShowError(ex);
      }
    }

    protected async void btnEmailTest_Click(object sender, EventArgs e)
    {
      try
      {
        if (emailer == null || systemService == null)
        {
          throw new Exception("Email services are not available.");
        }

        string to = txtEmailTestTo.Text.Trim();
        if (string.IsNullOrWhiteSpace(to) || !emailer.EmailIsValid(to))
        {
          throw new Exception("Enter a valid test recipient email address.");
        }

        string from = EmailSendContextHelper.GetDefaultSender();
        string subject = "NMSFM Fire Grant Email Test";
        string body = "Test email from NMSFM Fire Grant on " + DateTime.Now.ToLongDateString() + ".";
        var emailContext = EmailSendContextHelper.FromSession("EmailTest", string.Empty);
        await emailer.SendMailMessageAsync(
          from, to, "", "", subject, body, "", "", emailContext, systemService);
        dvError.InnerHtml = "<div class='alert alert-success'>Test email sent to " +
          HttpUtility.HtmlEncode(to) + ".</div>";
        await LoadEmailSendLogAsync();
      }
      catch (Exception ex)
      {
        ShowError(ex);
        try
        {
          await LoadEmailSendLogAsync();
        }
        catch
        {
          // Refresh is best-effort after a failed send.
        }
      }
    }

    private void ShowError(Exception ex)
    {
      string detail = ex != null && ex.InnerException != null
        ? Emailer.FormatExceptionDetail(ex)
        : (ex != null ? ex.Message : "An unknown error occurred.");
      ShowError(detail);
    }

    private void ShowError(string message)
    {
      string encoded = HttpUtility.HtmlEncode(message ?? string.Empty);
      encoded = encoded.Replace(" --> ", "<br />&nbsp;&nbsp;&rarr; ");
      dvError.InnerHtml = "<div class='alert alert-danger'>" + encoded + "</div>";
    }

    private void RedirectAndEnd(string url)
    {
      Response.Redirect(url, false);
      Context.ApplicationInstance.CompleteRequest();
    }

    private async System.Threading.Tasks.Task LoadEmailSendLogAsync()
    {
      if (systemService == null)
      {
        gvEmailSendLog.DataSource = new List<EmailSendLogEntry>();
        gvEmailSendLog.DataBind();
        return;
      }

      Guid? agencyId = null;
      if (Session["AgencyId"] != null && Guid.TryParse(Session["AgencyId"].ToString(), out Guid parsedAgency))
      {
        agencyId = parsedAgency;
      }

      var logs = await systemService.GetRecentEmailSendLogsAsync(agencyId, 100);
      if (chkFailedEmailLogsOnly.Checked)
      {
        logs = logs.Where(l => string.Equals(l.Status, "Failed", StringComparison.OrdinalIgnoreCase)).ToList();
      }

      gvEmailSendLog.DataSource = logs;
      gvEmailSendLog.DataBind();
    }
  }
}
