using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Permit
{
	public interface IPermitService
	{
		Task<IEnumerable<v_Permits>> GetPermitsAsync();
		Task<v_Permits> GetPermitByIdAsync(Guid id);
		Task<IEnumerable<PermitType>> GetPermitTypeListAsync(Guid? agencyId);
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByPermitIdAsync(Guid id, Guid? pTypeId, Guid agency);
		Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
		Task<IEnumerable<v_FeesPermits>> GetPermitFeesByPermitIdAsync(Guid id);
		Task<IEnumerable<v_Permits>> GetIncludedPermitsAsync(Guid parentPermitId);
		Task<string> GetPermitTypeLegalTextByPermitTypeIdAsync(Guid id);
		Task<string> GetAgencyName(Guid id);
		Task<string> GetPermitTypeByIdAsync(Guid id);
		Task<bool> CreatePermitAsync(DetailedPermit model);
		Task<bool> UpdatePermitAsync(DetailedPermit model);
		Task<IEnumerable<PermitStatu>> GetPermitStatusListAsync(Guid? agencyId);
		Task<PermitSetting> GetPermitSettingAsync(Guid permitTypeId);
		Task<string> GetNextPermitNumber(string strType, DateTime? permitDate);

	}
}
