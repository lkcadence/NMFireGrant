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

namespace NMSFM.Services.Activity
{
	public interface IActivityService
	{
		Task<IEnumerable<Inspection>> GetActivitiesAsync();
		Task<v_Activities> GetActivityByIdAsync(Guid id);
		Task<IEnumerable<ActivityType>> GetActivityCategoryListAsync(Guid? agencyId);
		Task<IEnumerable<InspectionType>> GetActivityTypeListAsync(Guid categoryId);
		Task<List<InspectionType>> GetInspectionTypesAsync();
		Task<InspectionType> GetInspectionTypeByIdAsync(Guid inspectionTypeId);
		Task<List<InspectionCaus>> GetInspectionCausesAsync();
		Task<IEnumerable<InspectionCaus>> GetInspectionCauseTypeListAsync(Guid inspectionTypeId);
		Task<v_Activities> GetAddressByInspectionIdAsync(Guid id);
		Task<v_Activities> GetSecondaryAddressByInspectionIdAsync(Guid id);
		Task<IEnumerable<v_Activities>> GetInspectionItemsByInspectionIdAsync(Guid id);
		Task<Inspector> GetInspectorByIdAsync(Guid inspectorid);
		Task<List<Inspector>> GetInspectorListAsync();
		Task<Inspector> GetSecondaryInspectorByIdAsync(Guid inspectorid);
		Task<List<Inspector>> GetSecondaryInspectorListAsync();
		Task<Guid> GetPartyRoleTypeIdAsync(Guid partyId, Guid addressId);
		Task<IEnumerable<ItemInspectionStatu>> GetItemInspectionStatusListAsync();
		Task<ActivitySetting> GetActivitySettingAsync(Guid activityTypeId);
		Task<string> GetActivityProjectNumberAsync(Guid activityId);
		//Task<v_Activities> GetActivitiesByAddressIdAsync(Guid id);
		Task<List<v_InspectionDetails>> GetInspectionDetailsByIdAsync(Guid inspectionId);
		Task<List<v_Fees>> GetFeesByIdAsync(Guid inspectionId);
		Task<List<v_Permits>> GetPermitsByActivityId(Guid inspectionId);
		Task<List<v_Activities>> GetAssociatedActivitiesById(Guid inspectionId);
		Task<List<v_Complaints>> GetRequestsByActivityIdAsync(Guid activityId);
		Task<IEnumerable<Data.Note>> GetNotesByInspectionIdAsync(Guid id);
		Task<List<v_Activities>> GetChildInspectionsByIdAsync(Guid inspectionId);
		Task<Data.Signature> GetSignatureByActivityId(Guid inspectionId);
		Task<List<CheckItemModel>> GetCheckListsByIdAsync(Guid inspectionId);
		Task<List<CheckItemModel>> GetCheckListsByTypeIdAsync(Guid? activityTypeId, Guid activityId);
		Task<SelectListItem> SaveResolutionAsync(Guid checkItemId, string text);
		//Task<List<SearchActivities>> PerformActivitySearchAsync(string addressTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party);
		Task<List<SearchAddress>> PerformSearchAsync(string addressTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party);
		Task<IEnumerable<v_Addresses2>> GetAddressesAsync(bool showInactive);
		Task<IEnumerable<OccupancyType>> GetOccupancyTypeListAsync();
		Task<IEnumerable<PropertyUseType>> GetPropertyUseTypeListAsync();
		Task<bool> CreateActivityAsync(DetailedActivity model, List<CheckItemModel> checkItems);
		Task<bool> SaveActivityAsync(DetailedActivity model, List<CheckItemModel> checkItems);
		Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
		Task<bool> SaveActivityCLAsync(List<CheckItemModel> checkItems, Guid inspectionId);
		Task<IEnumerable<v_Activities>> GetActivitiesByInspectorIdAsync(Guid id);
		Task<IEnumerable<v_Activities>> GetActivitiesByAHJIdAsync(Guid id);
		Task<IEnumerable<v_Files>> GetFilesByActivityIdAsync(Guid id);
		Task<string> GetCheckListName(Guid checklistId);
		Task<Guid?> GetCheckListIdAsync(string checklistName, Guid? inspectionTypeId = null);
		Task<string> GetNextActivityNumber(string strType, DateTime? activityDate, string permitNumber = "", Guid? groupdId = null, Guid? inspectorId = null);





	}
}
