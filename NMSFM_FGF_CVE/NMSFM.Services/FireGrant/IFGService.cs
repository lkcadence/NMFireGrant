using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.ViewModels;

namespace NMSFM.Services.FireGrant
{
    public interface IFGService
    {
        Task<FGApplicationSettings> GetFireGrantAppSettings(short year);
        Task<List<FG_Categories>> GetFGCategories();
        Task<FG_Categories> GetFGCategory(int categoryId);
        Task<List<FG_FDIDs>> GetFG_FDIDs();
        Task<FG_FDIDs> GetFG_FDID(int fdid);
        Task<bool> SaveFDIDAsync(FG_FDIDs model);
        Task<bool> UpdateFDIDAsync(FG_FDIDs model);
        Task<FG_FDIDs> IsFDIDValid(int fdid);
        Task<bool> SaveCategoryAsync(DetailedFGCategory model);
        Task<bool> UpdateCategoryAsync(DetailedFGCategory model);
        Task<List<FG_Priorities>> GetFGPriorities(int categoryId);
        Task<List<v_AddressParties>> GetFGDepartmentsAsync(Guid partyId);
        Task<v_AddressParties> GetFGDepartmentByPartyAddAsync(Guid addressId, Guid partyId);
        Task<v_AddressParties> GetFGDepartmentByIdAsync(Guid addressId);
        Task<List<v_Addresses2>> GetFGDepartmentsAllAsync();
        Task<bool> UpdateFireGrantMainSettings(FGApplicationSettings fgSettings);
        Task<List<FG_App_Help>> GetFGAllHelp();
        Task<FG_App_Help> GetFGHelpByPage(string page, string section = "");
        Task<FG_App_Help> GetFGHelpById(Guid Id);
        Task<bool> SavHelpText(FG_App_Help model);
        String GetDefaultEligibilityRequirements();
    }
}
