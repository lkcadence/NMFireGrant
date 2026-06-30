namespace NMSFM.Services.FireGrant
{
  /// <summary>
  /// JSON payload stored in Settings.ValueField for one outbound email log row.
  /// </summary>
  public class EmailSendLogPayload
  {
    public string status { get; set; }

    public string from { get; set; }

    public string replyTo { get; set; }

    public string to { get; set; }

    public string subject { get; set; }

    public string ctx { get; set; }

    public string ctxId { get; set; }

    public string sentByUserId { get; set; }

    public string sentByEmail { get; set; }

    public string sentByLogin { get; set; }

    public string sentByRole { get; set; }

    public string fail { get; set; }
  }
}
