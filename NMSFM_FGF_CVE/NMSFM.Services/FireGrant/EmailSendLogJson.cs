using System.Web.Script.Serialization;

namespace NMSFM.Services.FireGrant
{
  internal static class EmailSendLogJson
  {
    private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

    public static string Serialize(EmailSendLogPayload payload)
    {
      if (payload == null)
      {
        return "{}";
      }

      return Serializer.Serialize(payload);
    }

    public static EmailSendLogPayload Deserialize(string json)
    {
      if (string.IsNullOrWhiteSpace(json))
      {
        return new EmailSendLogPayload();
      }

      return Serializer.Deserialize<EmailSendLogPayload>(json) ?? new EmailSendLogPayload();
    }
  }
}
