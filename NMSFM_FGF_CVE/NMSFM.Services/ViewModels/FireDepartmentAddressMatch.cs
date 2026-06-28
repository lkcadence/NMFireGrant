using System;

namespace NMSFM.ViewModels
{
  public class FireDepartmentAddressMatch
  {
    public Guid AddressId { get; set; }
    public string AddressCode { get; set; }
    public string FullAddress { get; set; }
    public string City { get; set; }
    public int AppCount { get; set; }
    public int PartyLinkCount { get; set; }
    public int MatchRank { get; set; }
  }
}
