using System;
using NMSFM.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.Services.CPSystem
{
	public interface ISystemService
	{
		Task<string> GetCodepalSetting(string propertyField, Guid? agencyId, string userName = "");
		Task<bool> GetCodepalBooleanSettingAsync(string propertyField, Guid? agencyId, string userName = "");

		Task<bool> GetCodepalBooleanSetting(string propertyField, Guid? agencyId, string userName = "");

		Task<AgencyAliases> GetAliases(Guid? agencyId);

		Task<Guid?> GetModuleId(Guid? AgencyId, string ModuleName);
		Task<Inspector> GetUserAsync(Guid? userId);
		Task<string> GetUserNameAsync(Guid? userId);
		Task<Guid?> GetAgencyIdFromNameAsync(string agencyName);

		Task<List<Inspector>> GetInspectorListAsync();
		Task CreateInspector(DetailedInspector model);
		Task UpdateInspector(DetailedInspector model);

		Task<IEnumerable<Group>> GetGroupListAsync(Guid? id);

		Task<string> DecryptField(string Value);

		Task<string> EncryptField(string Value);
	}
}
