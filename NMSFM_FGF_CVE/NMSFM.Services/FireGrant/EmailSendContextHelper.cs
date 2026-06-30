using System;
using System.Configuration;
using System.Web;

namespace NMSFM.Services.FireGrant
{
  public static class EmailSendContextHelper
  {
    public static string GetDefaultSender()
    {
      string from = ConfigurationManager.AppSettings["DefaultEmailSender"];
      if (string.IsNullOrWhiteSpace(from))
      {
        from = "donotreply@fireservicesgrant.dhsem.nm.gov";
      }

      return from;
    }

    public static bool IsSendLoggingEnabled()
    {
      string flag = ConfigurationManager.AppSettings["EnableEmailSendLogging"];
      return string.IsNullOrWhiteSpace(flag) ||
        flag.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public static EmailSendContext FromSession(string contextType, string contextId)
    {
      var session = HttpContext.Current != null ? HttpContext.Current.Session : null;
      if (session == null)
      {
        return new EmailSendContext
        {
          ContextType = contextType,
          ContextId = contextId ?? string.Empty
        };
      }

      Guid? agencyId = null;
      if (session["AgencyId"] != null && Guid.TryParse(session["AgencyId"].ToString(), out Guid parsedAgency))
      {
        agencyId = parsedAgency;
      }

      return new EmailSendContext
      {
        ContextType = contextType,
        ContextId = contextId ?? string.Empty,
        AgencyId = agencyId,
        SentByUserId = session["WebUserId"] != null ? session["WebUserId"].ToString() : string.Empty,
        SentByEmail = session["WebUserEmail"] != null ? session["WebUserEmail"].ToString() : string.Empty,
        SentByLogin = session["WebUser"] != null ? session["WebUser"].ToString() : string.Empty,
        SentByRole = session["Role"] != null ? session["Role"].ToString() : string.Empty
      };
    }

    public static string BuildExternalSenderBodyLine()
    {
      var session = HttpContext.Current != null ? HttpContext.Current.Session : null;
      if (session == null)
      {
        return string.Empty;
      }

      string role = session["Role"] != null ? session["Role"].ToString() : string.Empty;
      if (!string.Equals(role, "External", StringComparison.OrdinalIgnoreCase))
      {
        return string.Empty;
      }

      string login = session["WebUser"] != null ? session["WebUser"].ToString() : string.Empty;
      string email = session["WebUserEmail"] != null ? session["WebUserEmail"].ToString() : string.Empty;
      if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(email))
      {
        return string.Empty;
      }

      string line = "This message was sent by ";
      if (!string.IsNullOrWhiteSpace(login))
      {
        line += HttpUtility.HtmlEncode(login);
      }

      if (!string.IsNullOrWhiteSpace(email))
      {
        line += string.IsNullOrWhiteSpace(login)
          ? HttpUtility.HtmlEncode(email)
          : " (" + HttpUtility.HtmlEncode(email) + ")";
      }

      return line + ".<br /><br />";
    }
  }
}
