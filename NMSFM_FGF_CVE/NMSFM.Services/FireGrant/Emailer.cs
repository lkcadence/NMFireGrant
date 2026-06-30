using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NMSFM.Services.CPSystem;

namespace NMSFM.Services.FireGrant
{
  public class Emailer
  {
    /// <summary>
    /// Sends an email message (synchronous wrapper).
    /// </summary>
    public void SendMailMessage(
      string from,
      string recepient,
      string bcc,
      string cc,
      string subject,
      string body,
      string att = "",
      string replyTo = "",
      EmailSendContext context = null,
      ISystemService systemService = null)
    {
      SendMailMessageAsync(
        from, recepient, bcc, cc, subject, body, att, replyTo, context, systemService)
        .ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends an email message asynchronously with optional send logging.
    /// </summary>
    public async Task SendMailMessageAsync(
      string from,
      string recepient,
      string bcc,
      string cc,
      string subject,
      string body,
      string att = "",
      string replyTo = "",
      EmailSendContext context = null,
      ISystemService systemService = null)
    {
      Guid messageId = Guid.NewGuid();
      EmailSendLogPayload logPayload = BuildInitialPayload(from, recepient, subject, context, replyTo);
      bool loggingEnabled = EmailSendContextHelper.IsSendLoggingEnabled() && systemService != null;

      if (loggingEnabled)
      {
        logPayload.status = "Queued";
        await systemService.InsertEmailSendLogAsync(
          messageId, logPayload, context != null ? context.AgencyId : null).ConfigureAwait(false);
      }

      bool emailsent = false;
      string exception = string.Empty;
      using (MailMessage mMailMessage = BuildMailMessage(
        from, recepient, bcc, cc, subject, body, att, replyTo, context))
      using (SmtpClient mSmtpClient = new SmtpClient())
      {
        mSmtpClient.Timeout = 15000;
        try
        {
          await mSmtpClient.SendMailAsync(mMailMessage).ConfigureAwait(false);
          emailsent = true;
        }
        catch (Exception ex)
        {
          exception = FormatExceptionDetail(ex);
        }
      }

      if (loggingEnabled)
      {
        logPayload.status = emailsent ? "Sent" : "Failed";
        logPayload.fail = emailsent ? string.Empty : exception;
        await systemService.UpdateEmailSendLogAsync(messageId, logPayload).ConfigureAwait(false);
      }

      if (!emailsent)
      {
        throw new Exception(
          "The system was unable to send the email. If this problem persists please contact the system administrator. Error: " +
          exception);
      }
    }

    /// <summary>
    /// Builds a detailed, admin-friendly description of an email send failure.
    /// </summary>
    public static string FormatExceptionDetail(Exception ex)
    {
      if (ex == null)
      {
        return string.Empty;
      }

      if (ex is AggregateException aggregateException)
      {
        ex = aggregateException.GetBaseException();
      }

      var parts = new List<string>();
      Exception current = ex;
      int depth = 0;
      while (current != null && depth < 10)
      {
        string part = current.GetType().Name + ": " + (current.Message ?? string.Empty);

        SmtpFailedRecipientException failedRecipient = current as SmtpFailedRecipientException;
        if (failedRecipient != null)
        {
          part += " [Failed recipient: " + failedRecipient.FailedRecipient + "]";
        }

        SmtpException smtpException = current as SmtpException;
        if (smtpException != null)
        {
          part += " [SMTP StatusCode: " + smtpException.StatusCode + "]";
        }

        parts.Add(part);
        current = current.InnerException;
        depth++;
      }

      return string.Join(" --> ", parts);
    }

    public bool EmailIsValid(string email)
    {
      string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

      if (Regex.IsMatch(email, expression))
      {
        if (Regex.Replace(email, expression, string.Empty).Length == 0)
        {
          return true;
        }
      }

      return false;
    }

    private static MailMessage BuildMailMessage(
      string from,
      string recepient,
      string bcc,
      string cc,
      string subject,
      string body,
      string att,
      string replyTo,
      EmailSendContext context)
    {
      string effectiveReplyTo = ResolveReplyTo(replyTo, context);
      MailMessage mMailMessage = new MailMessage();
      mMailMessage.From = BuildFromAddress(from, context);
      if (!string.IsNullOrWhiteSpace(effectiveReplyTo))
      {
        mMailMessage.ReplyToList.Add(new MailAddress(effectiveReplyTo));
      }

      if (recepient.Contains(";"))
      {
        string[] recipientlist = recepient.Split(';');
        for (int i = 0; i < recipientlist.Length; i++)
        {
          MailAddress mmaddress = new MailAddress(recipientlist[i].ToString().Trim());
          mMailMessage.To.Add(mmaddress);
        }
      }
      else
      {
        mMailMessage.To.Add(new MailAddress(recepient));
      }

      if (att != "")
      {
        mMailMessage.Attachments.Add(new Attachment(att));
      }

      if (bcc != null & bcc != string.Empty)
      {
        mMailMessage.Bcc.Add(new MailAddress(bcc));
      }

      if (cc != null & cc != string.Empty)
      {
        mMailMessage.CC.Add(new MailAddress(cc));
      }

      mMailMessage.Subject = subject;
      mMailMessage.Body = body;
      mMailMessage.IsBodyHtml = true;
      mMailMessage.Priority = MailPriority.Normal;
      return mMailMessage;
    }

    private static MailAddress BuildFromAddress(string from, EmailSendContext context)
    {
      if (context != null &&
          string.Equals(context.SentByRole, "External", StringComparison.OrdinalIgnoreCase) &&
          !string.IsNullOrWhiteSpace(context.SentByLogin))
      {
        return new MailAddress(from, context.SentByLogin + " (NMSFM Fire Grant)");
      }

      return new MailAddress(from);
    }

    private static string ResolveReplyTo(string replyTo, EmailSendContext context)
    {
      if (!string.IsNullOrWhiteSpace(replyTo))
      {
        return replyTo;
      }

      if (context != null &&
          string.Equals(context.SentByRole, "External", StringComparison.OrdinalIgnoreCase) &&
          !string.IsNullOrWhiteSpace(context.SentByEmail))
      {
        return context.SentByEmail;
      }

      return string.Empty;
    }

    private static EmailSendLogPayload BuildInitialPayload(
      string from,
      string recepient,
      string subject,
      EmailSendContext context,
      string replyTo)
    {
      string truncatedSubject = subject;
      if (!string.IsNullOrEmpty(truncatedSubject) && truncatedSubject.Length > 200)
      {
        truncatedSubject = truncatedSubject.Substring(0, 200);
      }

      var payload = new EmailSendLogPayload
      {
        from = from ?? string.Empty,
        to = recepient ?? string.Empty,
        subject = truncatedSubject ?? string.Empty,
        replyTo = ResolveReplyTo(replyTo, context),
        fail = string.Empty
      };

      if (context != null)
      {
        payload.ctx = context.ContextType ?? string.Empty;
        payload.ctxId = context.ContextId ?? string.Empty;
        payload.sentByUserId = context.SentByUserId ?? string.Empty;
        payload.sentByEmail = context.SentByEmail ?? string.Empty;
        payload.sentByLogin = context.SentByLogin ?? string.Empty;
        payload.sentByRole = context.SentByRole ?? string.Empty;
      }

      return payload;
    }
  }
}
