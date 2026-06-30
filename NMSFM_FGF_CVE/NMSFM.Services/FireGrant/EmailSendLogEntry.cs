using System;

namespace NMSFM.Services.FireGrant
{
  /// <summary>
  /// Deserialized email send log row for admin display.
  /// </summary>
  public class EmailSendLogEntry
  {
    public Guid MessageId { get; set; }

    public DateTime DateInserted { get; set; }

    public DateTime DateUpdated { get; set; }

    public string Status { get; set; }

    public string From { get; set; }

    public string ReplyTo { get; set; }

    public string To { get; set; }

    public string Subject { get; set; }

    public string ContextType { get; set; }

    public string ContextId { get; set; }

    public string SentByLogin { get; set; }

    public string SentByEmail { get; set; }

    public string SentByRole { get; set; }

    public string FailReason { get; set; }
  }
}
