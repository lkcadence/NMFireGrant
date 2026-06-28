using NMSFM.Data;
using NMSFM.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSFM.Services.UDF
{
	public interface IUDFService
	{
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByAddressIdAsync(Guid id, Guid agency);
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByAgencyIdAsync(Guid agencyId);
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByActivityIdAsync(Guid id, Guid agency);
		//Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByActivityTypeIdAsync(Guid actTypeId, Guid addressId, Guid agency);
		Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
		string GetUDFModuleType(Guid udfFieldId);
		Guid GetFeeUDFRecordId(string modeulType, Guid recordId);
		string GetUDFValue(Guid udfFieldId, Guid recordId);
		Task<string> GetUDFValueAsync(Guid udfFieldId, Guid recordId);

		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByFeeIdAsync(Guid id, Guid agency);
		//Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByActivityTypeIdAsync(Guid actTypeId, Guid addressId, Guid agency);
		Task<string> GetUDFCategoryNameAsync(Guid udfCategoryId);
		Task<Guid?> GetUDFCategoryIdAsync(string CategoryName, Guid? ModuleId, Guid? AgencyId, string AgencyName);		
		Task<Guid?> GetUDFFieldIdAsync(string FieldName, Guid? udfCategoryiD);
		Task<bool> UDFIsEncryptedAsync(Guid? FieldId);
		Task<string> GetUDFTypeAsync(string UserDefFieldId);
 }
}
