using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.Services.FYDist

{
    public interface IFYDistService
    {
        Task<FYAllowableDist> GetAllowableDistAsync(short year);
        Task<IEnumerable<nm_FYTotalDistribution>> GetTotalDistAsync(short year);
        Task<IEnumerable<nm_FYTotalDistributionCalc>> GetTotalDistCalcsAsync(short year, bool showInactive = false);
        Task<List<FYStatuteDist>> GetStatuteDistsAsync(short year);
        Task<List<FYTotalDistCalc>> GetFYTotalDistCalcsAsync(short year, bool refresh = false);
        //Task<FYTotal> GetTotalsAsync(short year);
        Task<List<string>> GetYearListAsync();
        Task<v_AddressParties> GetFPFApplicationAsync(Guid PartyId);
        Task<List<v_Addresses2>> GetStationListAsync(string StationType, Guid parentAddressId);
        Task<bool> SaveAllowableDistAsync(DetailedFYAllowableDist model);
        Task<bool> SaveDistributionCalculationsAsync(DetailedFYCalculatedDist model);
        Task<bool> SaveTotalDistributionsAsync(DetailedFYCalculatedDist model);
        Task<bool> SaveTotalDistributionCalcs(string col, Guid addressId, string value);
        Task<bool> SaveStatuteDistAsync(DetailedFYAllowableDist model);
        Task<bool> FinalizeAsync(DetailedFYCalculatedDist model);
        Task<bool> UnFinalizeAsync(DetailedFYCalculatedDist model);
        Task<bool> RecalcFYStatuteDistributionAsync(List<FYStatuteDist> statuteDists = null, short year = 0);
    }
}