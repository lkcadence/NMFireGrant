using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.Services.FYDist

{
    public interface IFPFService
    {
        Task<FYAllowableDist> GetAllowableDistAsync(short year);
        Task<IEnumerable<nm_FYTotalDistribution>> GetTotalDistAsync(short year);
        Task<nm_FYTotalDistribution> GetTotalDistAsync(short year, Guid addressId);
        Task<IEnumerable<nm_FYTotalDistributionCalc>> GetTotalDistCalcsAsync(short year, bool showInactive = false);
        Task<List<FYStatuteDist>> GetStatuteDistsAsync(short year);
        Task<decimal> GetStatuteDistAsync(short year, int isoClass, bool sub = false);
        Task<List<FYTotalDistCalc>> GetFYTotalDistCalcsAsync(short year, bool refresh = false);
        //Task<FYTotal> GetTotalsAsync(short year);
        Task<List<string>> GetYearListAsync();
        Task<v_AddressParties> GetFPFApplicationAsync(Guid partyId, Guid addressId);
        v_AddressParties GetFGFApplicationAddress(Guid partyId, Guid addressId);
        Task<List<v_AddressParties>> GetFPFApplicationsAsync(Guid partyId);
        Task<List<v_Addresses2>> GetFPFApplicationsAllAsync();
        Task<List<v_AddressParties>> GetTreasurerAddresses();
        Task<v_Addresses2> GetTreasurerAddress(bool isCity, string cityCounty);
        Task<v_AddressParties> GetFPFApplicationAddressAsync(Guid addressId);
        Task<List<v_Addresses2>> GetStationListAsync(string StationType, Guid parentAddressId);
        Task<List<FYAppStation>> GetExistingStationListAsync(string stationType, Guid departAddressId, short year);
        Task<bool> SaveAllowableDistAsync(DetailedFYAllowableDist model);
        Task<bool> SaveDistributionCalculationsAsync(DetailedFYCalculatedDist model);
        Task<bool> SaveTotalDistributionsAsync(DetailedFYCalculatedDist model);
        Task<bool> SaveTotalDistributionCalcs(string col, Guid addressId, string value);
        Task<bool> SaveStatuteDistAsync(DetailedFYAllowableDist model);
        Task<bool> FinalizeAsync(DetailedFYCalculatedDist model);
        Task<bool> UnFinalizeAsync(DetailedFYCalculatedDist model);
        Task<bool> RecalcFYStatuteDistributionAsync(List<FYStatuteDist> statuteDists = null, short year = 0);
        Task<DetailedApplication> SaveApplication(DetailedApplication model);
        Task<decimal> RecalcNMFATotal(short year);
        Task<nm_FYDetailedApplication> LoadExistingApplicationAsync(Guid partyId, Guid addressId, short year);
        Task<List<nm_FYDetailedApplication>> LoadExistingApplicationsAsync(Guid partyId);
        Task<List<nm_FYDetailedApplication>> LoadExistingApplicationsAsync(Guid partyId, short year);
        Task<List<nm_FYDetailedApplication>> LoadExistingApplicationByAddressAsync(Guid addressId, short year);
        Task<List<nm_FYDetailedApplication>> LoadSubmittedApplications(short year);
        Task<nm_FYDetailedApplication> LoadSubmittedApplicationAsync(Guid applicationId);
        Task<nm_FYDistributionInvoice> LoadExistingInvoiceAsync(Guid addressId, short year, short quarter);
        Task<FYInvoices> LoadExistingInvoiceTableAsync(Guid addressId, short year, short quarter);
        Task<List<FYInvoices>> LoadSavedInvoices(short year, short quarter);
        Task<DetailedFYInvoice> SaveInvoice(DetailedFYInvoice model);
    }
}