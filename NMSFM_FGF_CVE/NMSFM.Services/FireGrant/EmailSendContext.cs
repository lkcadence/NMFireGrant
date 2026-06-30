using System;

namespace NMSFM.Services.FireGrant
{
  /// <summary>
  /// Caller context for outbound application email and Settings send logging.
  /// </summary>
  public class EmailSendContext
  {
    public string ContextType { get; set; }

    public string ContextId { get; set; }

    public Guid? AgencyId { get; set; }

    public string SentByUserId { get; set; }

    public string SentByEmail { get; set; }

    public string SentByLogin { get; set; }

    public string SentByRole { get; set; }
  }
}
