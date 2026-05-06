//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using System.Web.Mvc;
using NMSFM.Services.Models;
using NMSFM.ViewModels;

namespace NMSFM.Services.Fee
{
	public interface IFeeService
	{
		Task<IEnumerable<v_Fees>> GetFeesAsync();
		Task<List<v_Fees>> GetFeesByIdAsync(Guid recordId);
		Task<v_Fees> GetFeeById(Guid feeId);
		Task<List<FeeType>> GetFeeTypesAsync();
		Task<List<FeeTypeRR>> GetFeeTypesRRAsync(Guid id);
		Task<List<FeeTypePT>> GetFeeTypePTAsync(Guid id);
		Task<FeeTypePen> GetFeeTypePenAsync(Guid id);
		Task<List<FeesPT>> GetFeesPTsAsync(Guid id);
		Task<List<FeesPT>> SetFeesPTsAsync(string[] fees, Guid baseFeeId);
		Task<List<FeeSchedule>> GetFeeSchedulesAsync(Guid id);
		Task<List<SelectListItem>> GetFeeInvItemPLAsync(Guid invITemId);
		Task<string> GetDefaultPriceLevel(Guid InventoryItemId, Guid partyId, Guid agencyId);
		Task<List<Inspector>> GetInspectorListAsync();
		Task<FeeSetting> GetFeeSettingAsync(Guid id);
		Task<IEnumerable<v_AddressParties>> GetPartyNameListAsync();
		Task<IEnumerable<v_FeePayments>> GetFeePaymentsAsync(Guid id);
		Task<FeePayment> GetFeePaymentById(Guid feePaymentId);
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);
		Task<bool> SaveFeePayment(DetailedFeePayment model, bool isNew);
		Task<bool> SaveFee(DetailedFee model);
		Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
		Task<Guid?> InsertScannedFee(string barCode, Guid recordId);
		Task<Guid?> GetScanFeeType(string barCode);
		List<DetailedDefaultFee> GetDefaultFees(Guid? recordId, bool? forReInsp, string reInspLetter);
		Guid DefaultRegFee(Guid? recordId, string feeAmt, Guid feeTypeId, DateTime feeDate, Guid? respPartyId);
		Guid DefaultRateFee(Guid? recordId, Guid? feeSchedId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId);
		Guid DefaultRRFee(Guid? recordId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId);
		Guid DefaultPOTFee(Guid? recordId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId);
		void ReCalcRatedFee(Guid feeId, Guid? feeSchedId = null, bool isDefault = false);
		void ReCalcRatedRangeFee(Guid feeId);
		void ReCalcPercoTotFee(Guid feeId);
		void ReCalcPenaltyFee(Guid feeId);
		void RecalculateFees(Guid recordId, bool parentIsComplete);

	}
}
