using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Party
{
	public interface IPartyService
	{
		Task<IEnumerable<v_Parties>> GetPartiesAsync();
		Task<v_AddressParties> GetAddressPartyByNameAsync(String name);
		Task<v_AddressParties> GetAddressPartyByIdAsync(Guid partyId, Guid addressId);
		Task<v_AddressParties> GetAddressPartyRoleByIdAsync(Guid partyId, Guid addressId, Guid roleTypeId);
		Task<v_AddressParties> GetAddressPartyByAddressPartyRoleAsync(Guid addressId, Guid roleTypeId);
		Task<v_Parties> GetPartyByIdAsync(Guid id);
		Task<List<Phone>> GetPhoneListForPartyAsync(Guid id);
		Task<IEnumerable<RoleType>> GetRoleTypeListAsync();
		Task<IEnumerable<RoleType>> GetRoleTypeListAsync(Guid? partyId);
		Task<IEnumerable<PhoneType>> GetPhoneTypeListAsync();
		Task CreatePhonesAsync(List<Phone> phoneList);
		Task UpdatePhonesAsync(List<Phone> phoneList);
		Task DeletePhonesAsync(Guid partyId, List<Guid> phoneIdList);
		Task RemoveAddressParty(v_AddressParties model);
		Task AttachExistingParty(DetailedAddressParty model);
		Task<Guid> AttachExistingParty(v_AddressParties model);
		//Task CreatePartyAsync(v_AddressParties model);
		Task CreatePartyAsync(AttachAddressParty model);
		Task<Guid> CreatePartyAsync(DetailedAddressParty model);
		Task UpdateParty(Data.Party model);
		Task<IEnumerable<v_Parties>> PerformSearchAsync(Guid roleTypeId, string partyName, bool hideInactive);
	}
}
