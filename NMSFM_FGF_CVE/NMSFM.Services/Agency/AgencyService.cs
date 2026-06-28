using NMSFM.Data;
using NMSFM.Services.Logging;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NMSFM.Services.Agency
{
  public class AgencyService : IAgencyService
  {
    private ICodepalWebModel cwmContext;
    private ILogging logger;

    public AgencyService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
    {
      cwmContext = codepalWebModel;
      logger = codepalLogger;
    }

    public async Task<Data.Agency> GetAgencyAsync(Guid agencyId)
    {
      try
      {
        return await cwmContext.Agencies.SingleOrDefaultAsync(a => a.AgencyId == agencyId);
      }
      catch (Exception ex)
      {
        logger.Error(
          "Unexpected exception caught while retrieving agency '" + agencyId + "'.",
          ex);
        return null;
      }
    }

    public async Task<bool> UpdateAgencyAsync(
      Data.Agency agency,
      byte[] reportImage,
      bool clearReportImage)
    {
      if (agency == null)
      {
        return false;
      }

      try
      {
        Data.Agency existing = await cwmContext.Agencies.SingleOrDefaultAsync(
          a => a.AgencyId == agency.AgencyId);
        if (existing == null)
        {
          logger.Error("Agency '" + agency.AgencyId + "' was not found for update.");
          return false;
        }

        existing.AgencyName = agency.AgencyName;
        existing.AgencySubName = agency.AgencySubName;
        existing.Address = agency.Address;
        existing.City = agency.City;
        existing.StateId = agency.StateId;
        existing.Zip = agency.Zip;
        existing.CountryId = agency.CountryId;
        existing.Phone = agency.Phone;
        existing.Fax = agency.Fax;
        existing.Email = agency.Email;
        existing.ExternalId = agency.ExternalId;
        existing.DateUpdated = DateTime.Now;

        if (clearReportImage)
        {
          existing.ReportImage = null;
        }
        else if (reportImage != null && reportImage.Length > 0)
        {
          existing.ReportImage = reportImage;
        }

        await ((DbContext)cwmContext).SaveChangesAsync();
        return true;
      }
      catch (Exception ex)
      {
        logger.Error(
          "Unexpected exception caught while updating agency '" + agency.AgencyId + "'.",
          ex);
        return false;
      }
    }
  }
}
