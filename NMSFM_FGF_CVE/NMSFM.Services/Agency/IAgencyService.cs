using NMSFM.Data;
using System;
using System.Threading.Tasks;

namespace NMSFM.Services.Agency
{
  public interface IAgencyService
  {
    Task<Data.Agency> GetAgencyAsync(Guid agencyId);

    Task<bool> UpdateAgencyAsync(
      Data.Agency agency,
      byte[] reportImage,
      bool clearReportImage);
  }
}
