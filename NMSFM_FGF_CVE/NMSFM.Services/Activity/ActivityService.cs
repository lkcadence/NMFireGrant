//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using AutoMapper;
using NMSFM.Data;
using NMSFM.Services.Audit;
using NMSFM.Services.CPSystem;
using NMSFM.Services.Fee;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using NMSFM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NMSFM.Services.Activity
{
	public class ActivityService : IActivityService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;
		private IFeeService feeService;

		public ActivityService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
			feeService = new FeeService(cwmContext, logger);
		}

		public ActivityService()
		{
			//var userConnection = System.Web.HttpContext.Current.Session["userConnection"].ToString();
			//cwmContext = new CodepalWebModel(userConnection);
			cwmContext = new CodepalWebModel();
			logger = new Logging.Logging();
			auditService = new AuditService(logger);
			feeService = new FeeService(cwmContext, logger);
		}

		//Task<List<ActivityType>> GetActivityCategoryListAsync(Guid? agencyId);
		public async Task<IEnumerable<ActivityType>> GetActivityCategoryListAsync(Guid? agencyId)
		{
			IEnumerable<ActivityType> result = null;
			try
			{
				if (agencyId != null && agencyId != Guid.Empty)
				{
					result = await cwmContext.ActivityTypes.Where(a => a.Inactive == false && a.WebViewable == true && (a.AgencyId == agencyId || (a.AgencyId == null && cwmContext.AgencyActivityTypes.Where(at => at.ActivityTypeId == a.ActivityTypeId).Select(aa => aa.AgencyId).Contains(agencyId)))).ToListAsync();
				}
				else
				{
					result = await cwmContext.ActivityTypes.Where(a => a.Inactive == false && a.WebViewable == true).ToListAsync();
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Category list.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<InspectionType>> GetActivityTypeListAsync(Guid categoryId)
		{
			IEnumerable<InspectionType> result = null;
			try
			{
				result = await cwmContext.InspectionTypes.Where(a => a.ActivityTypeId == categoryId && a.Inactive == false && a.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Type list.", ex);
			}
			return result;
		}

		//Task<IEnumerable<Inspection>> GetActivityAsync();
		public async Task<IEnumerable<Inspection>> GetActivitiesAsync()
		{
			IEnumerable<Inspection> result;
			try
			{
				var activityList = await cwmContext.Inspections.Where(a => a.InspectionId != null).ToListAsync();
				var activityTypeList = await cwmContext.ActivityTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.ActivityTypeId).ToListAsync();
				if (activityList != null && activityList.Count() > 0)
				{
					for (int i = activityList.Count() - 1; i > -1; i--)
					{
						var activityType = activityList[i].ActivityTypeId == null ? Guid.Empty : activityList[i].ActivityTypeId.Value;
						if (!activityTypeList.Contains(activityType))
						{
							activityList.RemoveAt(i);
						}
					}
				}
				result = activityList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity list.", ex);
				result = new List<Inspection>();
			}
			return result;
		}

		public async Task<v_Activities> GetActivityByIdAsync(Guid id)
		{
			v_Activities result = null;
			try
			{
				if (cwmContext.v_Activities.Select(a => a.InspectionId).ToArray().Contains(id))
				{
					result = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the activity for id = " + id + ".", ex);
			}
			return result;
		}

		//Task<List<InspectionType>> GetInspectionTypeAsync();
		public async Task<List<InspectionType>> GetInspectionTypesAsync()
		{
			List<InspectionType> result;
			try
			{
				var inspectionTypeList = await cwmContext.InspectionTypes.Where(a => !a.Inactive).ToListAsync();
				result = inspectionTypeList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Inspection Types list.", ex);
				result = new List<InspectionType>();
			}
			return result;
		}

		public async Task<InspectionType> GetInspectionTypeByIdAsync(Guid inspectionTypeId)
		{
			InspectionType result;
			try
			{
				result = await cwmContext.InspectionTypes.Where(a => a.InspectionTypeId == inspectionTypeId).FirstAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Inspection Types list.", ex);
				result = new InspectionType();
			}
			return result;
		}

		//Task<List<InspectionCause>> GetInspectionCausesAsync();
		public async Task<List<InspectionCaus>> GetInspectionCausesAsync()
		{
			List<InspectionCaus> result;
			try
			{
				var inspectionCauseList = await cwmContext.InspectionCauses.Where(a => !a.Inactive).ToListAsync();
				result = inspectionCauseList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Inspection Causes list.", ex);
				result = new List<InspectionCaus>();
			}
			return result;
		}

		public async Task<IEnumerable<InspectionCaus>> GetInspectionCauseTypeListAsync(Guid inspectionTypeId)
		{
			IEnumerable<InspectionCaus> result = null;
			try
			{
				var inspectionCauseIdList = await cwmContext.ActivityTypeCauses.Where(a => a.ActivityTypeId == inspectionTypeId).Select(a => a.InspectionCauseId).ToListAsync();
				result = await cwmContext.InspectionCauses.Where(a => inspectionCauseIdList.Contains(a.InspectionCauseId) && a.Inactive == false && a.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Inspection Cause list.", ex);
			}
			return result;
		}

		//Task<v_Activities> GetAddressByInspectionIdAsync(Guid id);
		public async Task<v_Activities> GetAddressByInspectionIdAsync(Guid id)
		{
			v_Activities result = null;
			try
			{
				result = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Address '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		//Task<v_Activities> GetSecondaryAddressByInspectionIdAsync(Guid id);
		public async Task<v_Activities> GetSecondaryAddressByInspectionIdAsync(Guid id)
		{
			v_Activities result = null;
			try
			{
				result = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Secondary Address '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		//Task<IEnumerable<v_Activities>> GetInspectionItemsByInspectionIdAsync(Guid id);
		public async Task<IEnumerable<v_Activities>> GetInspectionItemsByInspectionIdAsync(Guid id)
		{
			IEnumerable<v_Activities> result;
			try
			{
				var activityItems = await cwmContext.v_Activities.Where(p => p.InspectionId == id).ToListAsync();
				result = activityItems;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Items List.", ex);
				result = new List<v_Activities>();
			}
			return result;
		}

		//Task<Inspector> GetInspectorByIdAsync(Guid inspectorid);
		public async Task<Inspector> GetInspectorByIdAsync(Guid inspectorId)
		{
			Inspector result = null;
			if (inspectorId != null)
			{
				try
				{
					result = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.InspectorId == inspectorId);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the Activity inspector '" + inspectorId.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		//Task<List<Inspector>> GetInspectorListAsync();
		public async Task<List<Inspector>> GetInspectorListAsync()
		{
			List<Inspector> result = null;
			try
			{
				result = await cwmContext.Inspectors.OrderBy(a => a.InspectorName).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity inspector list.", ex);
			}
			return result;
		}

		//Task<Inspector> GetSecondaryInspectorByIdAsync(Guid secondaryinspectorId);
		public async Task<Inspector> GetSecondaryInspectorByIdAsync(Guid secondaryinspectorId)
		{
			Inspector result = null;
			if (secondaryinspectorId != null)
			{
				try
				{
					result = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.InspectorId == secondaryinspectorId);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the Activity secondary inspector '" + secondaryinspectorId.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		//Task<List<Inspector>> GetSecondaryInspectorListAsync();
		public async Task<List<Inspector>> GetSecondaryInspectorListAsync()
		{
			List<Inspector> result = null;
			try
			{
				result = await cwmContext.Inspectors.OrderBy(a => a.InspectorName).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the secondary inspector list.", ex);
			}
			return result;
		}

		//Task<Guid> GetPartyRoleTypeIdAsync(Guid partyId, Guid addressId);
		public async Task<Guid> GetPartyRoleTypeIdAsync(Guid partyId, Guid addressId)
		{
			Guid result = Guid.Empty;
			try
			{
				var addressParty = await cwmContext.AddressParties.FirstAsync(a => a.PartyID == partyId && a.AddressID == addressId && a.Inactive == false) ?? new AddressParty();
				result = addressParty.RoleTypeId ?? Guid.Empty;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Party Role Type Id for Party Id: " + partyId + " and Address Id: " + addressId + ".", ex);
			}
			return result;
		}

		public async Task<IEnumerable<ItemInspectionStatu>> GetItemInspectionStatusListAsync()
		{
			IEnumerable<ItemInspectionStatu> result = null;
			try
			{
				result = await cwmContext.ItemInspectionStatus.Where(a => a.Inactive == false && a.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item Inspection Status list.", ex);
			}
			return result;
		}

		//Task<ActivitySetting> GetActivitySettingAsync(Guid activityTypeId);
		public async Task<ActivitySetting> GetActivitySettingAsync(Guid activityTypeId)
		{
			ActivitySetting result = null;
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				result = await cwmContext.ActivitySettings.SingleOrDefaultAsync(a => a.ActivityTypeId == activityTypeId && a.AgencyId == agencyId);
				if (result == null)
				{
					result = await cwmContext.ActivitySettings.SingleOrDefaultAsync(a => a.ActivityTypeId == activityTypeId && a.AgencyId == null);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Settings for Activity Id: " + activityTypeId + ".", ex);
			}
			return result;
		}

		//Task<string> GetActivityProjectNumberAsync(Guid activityId);
		public async Task<string> GetActivityProjectNumberAsync(Guid activityId)
		{
			var result = "";
			try
			{
				var projectActivity = await cwmContext.v_ProjectActivitySearch.SingleOrDefaultAsync(a => a.InspectionId == activityId) ?? new v_ProjectActivitySearch();
				result = projectActivity.ProjectNumber;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Project Number for Activity Id: " + activityId + ".", ex);
			}
			return result;
		}

		////Task<v_Activities> GetActivitiesByAddressIdAsync(Guid id);
		//public async Task<v_Activities> GetActivitiesByAddressIdAsync(Guid id)
		//{
		//    v_Activities result = null;
		//    try
		//    {
		//        result = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
		//    }
		//    catch (Exception ex)
		//    {
		//        logger.Error("Unexpected exception caught while retrieving the Address '" + id.ToString() + "'.", ex);
		//    }
		//    return result;
		//}

		//Task<List<v_InspectionDetails>> GetInspectionDetailsByIdAsync(Guid inspectionId);
		public async Task<List<v_InspectionDetails>> GetInspectionDetailsByIdAsync(Guid inspectionId)
		{
			var results = new List<v_InspectionDetails>();
			try
			{
				results = await cwmContext.v_InspectionDetails.Where(a => a.InspectionId == inspectionId).ToListAsync() ?? new List<v_InspectionDetails>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Actvity Inspection Details for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

		//Task<List<v_Fees>> GetFeesByIdAsync(Guid inspectionId);
		public async Task<List<v_Fees>> GetFeesByIdAsync(Guid inspectionId)
		{
			var results = new List<v_Fees>();
			try
			{
				results = await cwmContext.v_Fees.Where(a => a.RecordId == inspectionId && a.Inactive == false && a.WebViewable == true).ToListAsync() ?? new List<v_Fees>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Fees for Activity Id:" + inspectionId + ".", ex);
			}
			return results;
		}

		public async Task<List<v_Permits>> GetPermitsByActivityId(Guid inspectionId)
		{
			var results = new List<v_Permits>();
			try
			{
				var permitIdList = await cwmContext.ActivityPermits.Where(a => a.ActivityId == inspectionId).Select(a => a.PermitId).ToListAsync();
				results = await cwmContext.v_Permits.Where(a => permitIdList.Contains(a.PermitId)).ToListAsync() ?? new List<v_Permits>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permits for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

		//Task<List<v_Activities>> GetAssociatedActivitiesById(Guid inspectionId);
		public async Task<List<v_Activities>> GetAssociatedActivitiesById(Guid inspectionId)
		{
			var results = new List<v_Activities>();
			try
			{
				var activityIdList = await cwmContext.AssociatedActivities.Where(a => a.ActivityId == inspectionId).Select(a => a.AssocActivityId).ToListAsync();
				results = await cwmContext.v_Activities.Where(a => activityIdList.Contains(a.InspectionId) && a.Inactive == false).ToListAsync() ?? new List<v_Activities>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Permits for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

		//Task<List<v_Complaints>> GetRequestsByActivityIdAsync(Guid activityId);
		public async Task<List<v_Complaints>> GetRequestsByActivityIdAsync(Guid activityId)
		{
			var results = new List<v_Complaints>();
			try
			{
				var complaintIdList = await cwmContext.ComplaintActivities.Where(a => a.ActivityId == activityId).Select(a => a.ComplaintId).ToListAsync();
				results = await cwmContext.v_Complaints.Where(a => complaintIdList.Contains(a.ComplaintId) && a.Inactive == false && a.WebViewable == true).ToListAsync() ?? new List<v_Complaints>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Requests for Activity Id: " + activityId + ".", ex);
			}
			return results;
		}

		//Task<IEnumerable<Data.Note>> GetNotesByInspectionIdAsync(Guid id);
		public async Task<IEnumerable<Data.Note>> GetNotesByInspectionIdAsync(Guid id)
		{
			IEnumerable<Data.Note> result;
			try
			{
				result = await cwmContext.Notes.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Notes List.", ex);
				result = new List<Data.Note>();
			}
			return result;
		}

		//Task<List<v_Activities>> GetChildInspectionsByIdAsync(Guid inspectionId);
		public async Task<List<v_Activities>> GetChildInspectionsByIdAsync(Guid inspectionId)
		{
			var results = new List<v_Activities>();
			try
			{
				results = await cwmContext.v_Activities.Where(a => a.ParentInspectionId == inspectionId && a.Inactive == false).ToListAsync() ?? new List<v_Activities>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Child Activities for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

		//Task<Signature> GetSignatureByActivityId(Guid inspectionId);
		public async Task<Data.Signature> GetSignatureByActivityId(Guid inspectionId)
		{
			var result = new Data.Signature();
			try
			{
				result = await cwmContext.Signatures.SingleOrDefaultAsync(a => a.RecordId == inspectionId && a.Inactive == false) ?? new Data.Signature();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Signature for Activity Id: " + inspectionId + ".", ex);
			}
			return result;
		}

		//Task<List<CheckItemModel>> GetCheckListsByIdAsync(Guid inspectionId);
		public async Task<List<CheckItemModel>> GetCheckListsByIdAsync(Guid inspectionId)
		{
			var results = new List<CheckItemModel>();

			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				string userLogin = (string)System.Web.HttpContext.Current.Session["CodepalUserLogin"];
				bool defaultIndCLQ = Convert.ToBoolean(Convert.ToInt32(cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "DefaultIndChecklstQ" && set.AgencyId == agencyId).ValueField));
				bool userAllowDefaults = Convert.ToBoolean(Convert.ToInt32(cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "ALLOW_CHECKLIST_DEFAULTS" && set.UserName == userLogin).ValueField));



				var checkLists = from actChecks in cwmContext.ActivityCheckLists
								 join checks in cwmContext.CheckLists on new { ad = actChecks.CheckListId } equals new { ad = checks.CheckListId } into subchecks
								 from usechecks in subchecks.DefaultIfEmpty()
								 where (actChecks.ActivityId == inspectionId && usechecks.Inactive == false && usechecks.WebViewable == true)
								 join items in cwmContext.CheckItems on usechecks.CheckListId equals items.CheckListId
								 join values in cwmContext.CheckItemValues on new { id = items.CheckItemId, ud = inspectionId } equals new { id = values.CheckItemId, ud = values.InspectionId } into subvalues
								 from usevalues in subvalues.DefaultIfEmpty()
								 where (items.Inactive == false)
								 select new CheckItemModel
								 {
									 ActivityId = inspectionId,
									 CheckListId = usechecks.CheckListId,
									 CheckListName = usechecks.CheckListName,
									 CheckListOrder = usechecks.CheckListOrder ?? 0,
									 CheckListTypeId = usechecks.CheckListTypeId ?? Guid.Empty,
									 CheckItemId = items.CheckItemId,
									 CheckItem = items.CheckItem1,
									 CheckItemTypeId = items.CheckItemTypeId,
									 CheckItemOrder = items.SeqNum ?? 0,
									 CheckItemValueId = usevalues != null ? usevalues.CheckItemValueId : Guid.Empty,
									 TextValue = usevalues != null ? usevalues.TextValue : "",
									 BooleanValue = usevalues != null ? usevalues.BooleanValue : null,
									 ResolutionText = usevalues != null ? usevalues.ResolutionText : "",
									 Corrected = usevalues != null ? usevalues.Corrected : null,
									 DefaultValue = items.DefaultValue,
									 FailValue = items.FailValue,
									 Required = items.Required,
									 FailsCheckList = items.FailsCheckList,
									 HideNA = items.HideNA,
									 HideNO = items.HideNO,
									 StaticList = items.StaticList,
									 HideAddRef = items.HideAddRef,
									 DefaultLastValues = usechecks.DefaultValues,
								 };

				checkLists = checkLists.OrderBy(cl => cl.CheckListOrder).ThenBy(cl => cl.CheckListId).ThenBy(cl => cl.CheckItemOrder).ThenBy(cl => cl.CheckItem);


				if (checkLists != null && checkLists.Count() > 0)
				{
					bool prevValues = checkLists.Count(cl => cl.CheckItemValueId != null && cl.CheckItemValueId != Guid.Empty) > 0;
					results = checkLists.OrderBy(cl => cl.CheckListOrder).ThenBy(cl => cl.CheckItemOrder).ToList();

					string thisInfo = "";
					Guid curChklstId = Guid.Empty;

					for (int i = 0; i < results.Count(); i++)
					{
						if (curChklstId != results[i].CheckListId)
						{
							curChklstId = results[i].CheckListId;
							thisInfo = GetInfoLineText(inspectionId, results[i].CheckListId);
						}
						results[i].InfoLine = thisInfo;


						var checkItemId = results[i].CheckItemId;

						if (userAllowDefaults)
						{
							bool didPrev = false;


							PrevCheckItem prevCheckItem = GetLastValues(results[i]);

							if ((!prevValues || defaultIndCLQ) && results[i].DefaultLastValues && (prevCheckItem != null && (prevCheckItem.BooleanValue != null || (prevCheckItem.TextValue != null && prevCheckItem.TextValue != "") || (prevCheckItem.ResolutionText != null && prevCheckItem.ResolutionText != ""))))
							{
								if (results[i].CheckItemValueId == null || results[i].CheckItemValueId == Guid.Empty)
								{
									switch (results[i].CheckItemTypeId.ToString().ToUpper())
									{
										case "760BD541-04DA-403B-8A9E-D1A705C57BD0":  //  Calculation
										case "2C33CAB4-45A9-48CF-A794-CA633CA6507A":  //  Check Box											
										case "D290E005-205B-48C9-A689-90C68C911C24":  //  Text
										case "DF88CE10-9F66-47A2-AB3A-99CA24BE99F4":  //  Long Text
										case "F6D38607-9589-49DD-975D-609437106650":  //  Date										
										case "380714E6-4A48-4D89-B01C-445BC8C864F3":  //  Numeric										
										case "33648CDD-1B80-497F-BF65-B6797FC28D34":  //  Time
											results[i].TextValue = prevCheckItem.TextValue;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
										case "CE0D1281-6ADF-4ADC-AEA5-F3750F99FCCB":  //  List
											results[i].TextValue = prevCheckItem.ResolutionText;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
										case "0B90628B-C6E3-406E-8EEE-92968E6AE835":  //  In/Out																				
										case "C92B6951-407E-4C75-9CC3-5DFA1B63DA4A":  //  OK/Service
										case "71E2DDDC-22E2-4661-92CC-F16FE7E66AE8":  //  Pass/Fail
										case "34E5AB9F-66BF-4D4E-A5E5-3DBE179133E6":  //  Satisfactory/Unsatisfactory										
										case "BFE3D7C3-5EE1-4BF4-AC8D-CE4F33AE63C4":  //  True/False
										case "301F3909-CA52-482E-8481-E219AC39CFC1":  //  Yes/No
										default:
											results[i].BooleanValue = prevCheckItem.BooleanValue;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
									}
									didPrev = true;
								}
							}


							if ((!prevValues && !didPrev) && (((results[i].TextValue == null || results[i].TextValue == "") && results[i].BooleanValue == null) && (results[i].DefaultValue != null && results[i].DefaultValue != "")))
							{
								results[i].BooleanValue = Convert.ToByte(results[i].DefaultValue);
								switch (results[i].CheckItemTypeId.ToString().ToUpper())
								{
									case "CE0D1281-6ADF-4ADC-AEA5-F3750F99FCCB":  //  List
										results[i].TextValue = results[i].DefaultValue;
										results[i].ResolutionText = results[i].DefaultValue;
										break;
									case "0B90628B-C6E3-406E-8EEE-92968E6AE835":  //  In/Out																				
									case "C92B6951-407E-4C75-9CC3-5DFA1B63DA4A":  //  OK/Service
									case "71E2DDDC-22E2-4661-92CC-F16FE7E66AE8":  //  Pass/Fail
									case "34E5AB9F-66BF-4D4E-A5E5-3DBE179133E6":  //  Satisfactory/Unsatisfactory										
									case "BFE3D7C3-5EE1-4BF4-AC8D-CE4F33AE63C4":  //  True/False
									case "301F3909-CA52-482E-8481-E219AC39CFC1":  //  Yes/No
										results[i].BooleanValue = Convert.ToByte(results[i].DefaultValue);
										results[i].ResolutionText = results[i].ResolutionText;
										break;
									case "760BD541-04DA-403B-8A9E-D1A705C57BD0":  //  Calculation
									case "2C33CAB4-45A9-48CF-A794-CA633CA6507A":  //  Check Box
									case "D290E005-205B-48C9-A689-90C68C911C24":  //  Text
									case "DF88CE10-9F66-47A2-AB3A-99CA24BE99F4":  //  Long Text
									case "F6D38607-9589-49DD-975D-609437106650":  //  Date										
									case "380714E6-4A48-4D89-B01C-445BC8C864F3":  //  Numeric										
									case "33648CDD-1B80-497F-BF65-B6797FC28D34":  //  Time
									default:
										results[i].TextValue = results[i].DefaultValue;
										results[i].ResolutionText = results[i].ResolutionText;
										break;
								}
							}

						}

						results[i].Resolutions = (await cwmContext.Resolutions.Where(a => a.ResolutionType == checkItemId && !string.IsNullOrEmpty(a.Resolution1)).OrderBy(r => r.Sequence).ThenBy(r => r.Resolution1).Select(a => new SelectListItem() { Text = a.Resolution1, Value = a.ResolutionId.ToString() }).ToListAsync()) ?? new List<SelectListItem>();
						if (results[i].Resolutions == null)
						{
							results[i].Resolutions = new List<SelectListItem>();
						}
						if (results[i].CheckItemTypeId == new Guid("2C33CAB4-45A9-48CF-A794-CA633CA6507A")) // Check Box
						{
							if (results[i].TextValue != String.Empty && results[i].TextValue.Length == results[i].Resolutions.Count())
							{
								results[i].CheckBoxValues = new List<bool>();
								for (int j = 0; j < results[i].Resolutions.Count(); j++)
								{
									results[i].CheckBoxValues.Add(results[i].TextValue.ElementAt(j) == '1' ? true : false);
								}
							}
							else
							{
								results[i].CheckBoxValues = new List<bool>(results[i].Resolutions.Count());
								for (int j = 0; j < results[i].CheckBoxValues.Capacity; j++)
								{
									results[i].CheckBoxValues.Add(false);
								}
							}
						}
					}
				}
				else
				{
					Inspection inspection = cwmContext.Inspections.FirstOrDefault(ins => ins.InspectionId == inspectionId);
					Guid? agencyId2 = cwmContext.AgencyActivityTypes.FirstOrDefault(aat => aat.ActivityTypeId == inspection.ActivityTypeId).AgencyId ?? null;
					results = checkLists.ToList();
					string retval = "";
					retval += "Q:0 ";
					retval += "A:0 ";
					retval += "O:0 ";
					retval += "F:0 ";
					var setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNA" && set.AgencyId == agencyId2).ValueField;
					if (setting != null && setting != "")
					{
						retval += "NA:0 ";
					}
					setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNO" && set.AgencyId == agencyId2).ValueField;
					if (setting != null && setting != "")
					{
						retval += "NO:0 ";
					}

					retval += "V:0";
					for (int i = 0; i < results.Count(); i++)
					{

						results[i].InfoLine = retval;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to get checklists by Id - '" + inspectionId + "'.", ex);
				//throw;
			}

			return results;
		}

		//Task<List<CheckItemModel>> GetCheckListsByTypeIdAsync(Guid? activityTypeId);
		public async Task<List<CheckItemModel>> GetCheckListsByTypeIdAsync(Guid? activityTypeId, Guid activityId)
		{
			var results = new List<CheckItemModel>();
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				string userLogin = (string)System.Web.HttpContext.Current.Session["CodepalUserLogin"];
				bool defaultIndCLQ = Convert.ToBoolean(Convert.ToInt32(cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "DefaultIndChecklstQ" && set.AgencyId == agencyId).ValueField));
				bool userAllowDefaults = Convert.ToBoolean(Convert.ToInt32(cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "ALLOW_CHECKLIST_DEFAULTS" && set.UserName == userLogin).ValueField));

				var checkLists = from actChecks in cwmContext.CheckListActivityTypes
								 join checks in cwmContext.CheckLists on new { ad = actChecks.CheckListId } equals new { ad = checks.CheckListId } into subchecks
								 from usechecks in subchecks.DefaultIfEmpty()
								 where (usechecks.Inactive == false && usechecks.WebViewable == true)
								 join items in cwmContext.CheckItems on usechecks.CheckListId equals items.CheckListId
								 where (items.Inactive == false && usechecks.Inactive == false && actChecks.ActivityTypeId == activityTypeId)
								 select new CheckItemModel
								 {
									 CheckListId = usechecks.CheckListId,
									 CheckListName = usechecks.CheckListName,
									 CheckListOrder = usechecks.CheckListOrder ?? 0,
									 CheckListTypeId = usechecks.CheckListTypeId,
									 CheckItemId = items.CheckItemId,
									 CheckItem = items.CheckItem1,
									 CheckItemTypeId = items.CheckItemTypeId,
									 CheckItemOrder = items.SeqNum ?? 0,
									 BooleanValue = null,
									 ResolutionText = "",
									 Corrected = null,
									 DefaultValue = items.DefaultValue,
									 FailValue = items.FailValue,
									 Required = items.Required,
									 FailsCheckList = items.FailsCheckList,
									 HideNA = items.HideNA,
									 HideNO = items.HideNO,
									 StaticList = items.StaticList,
									 HideAddRef = items.HideAddRef,
									 DefaultLastValues = usechecks.DefaultValues,
								 };

				if (checkLists != null && checkLists.Count() > 0)
				{
					bool prevValues = false; // checkLists.Count(cl => cl.CheckItemValueId != null && cl.CheckItemValueId != Guid.Empty) > 0;

					results = checkLists.OrderBy(cl => cl.CheckListOrder).ThenBy(cl => cl.CheckItemOrder).ToList();
					string thisInfo = "";
					Guid curChklstId = Guid.Empty;

					for (int i = 0; i < results.Count(); i++)
					{
						results[i].ActivityId = activityId;

						if (curChklstId != results[i].CheckListId)
						{
							curChklstId = results[i].CheckListId;
							thisInfo = GetInfoLineText(activityId, results[i].CheckListId);
						}
						results[i].InfoLine = thisInfo;

						var checkItemId = results[i].CheckItemId;

						if (userAllowDefaults)
						{
							bool didPrev = false;


							PrevCheckItem prevCheckItem = GetLastValues(results[i]);

							if ((!prevValues || defaultIndCLQ) && results[i].DefaultLastValues && (prevCheckItem != null && (prevCheckItem.BooleanValue != null || (prevCheckItem.TextValue != null && prevCheckItem.TextValue != "") || (prevCheckItem.ResolutionText != null && prevCheckItem.ResolutionText != ""))))
							{
								if (results[i].CheckItemValueId == null || results[i].CheckItemValueId == Guid.Empty)
								{
									switch (results[i].CheckItemTypeId.ToString().ToUpper())
									{
										case "760BD541-04DA-403B-8A9E-D1A705C57BD0":  //  Calculation
										case "2C33CAB4-45A9-48CF-A794-CA633CA6507A":  //  Check Box											
										case "D290E005-205B-48C9-A689-90C68C911C24":  //  Text
										case "DF88CE10-9F66-47A2-AB3A-99CA24BE99F4":  //  Long Text
										case "F6D38607-9589-49DD-975D-609437106650":  //  Date										
										case "380714E6-4A48-4D89-B01C-445BC8C864F3":  //  Numeric										
										case "33648CDD-1B80-497F-BF65-B6797FC28D34":  //  Time
											results[i].TextValue = prevCheckItem.TextValue;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
										case "CE0D1281-6ADF-4ADC-AEA5-F3750F99FCCB":  //  List
											results[i].TextValue = prevCheckItem.ResolutionText;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
										case "0B90628B-C6E3-406E-8EEE-92968E6AE835":  //  In/Out																				
										case "C92B6951-407E-4C75-9CC3-5DFA1B63DA4A":  //  OK/Service
										case "71E2DDDC-22E2-4661-92CC-F16FE7E66AE8":  //  Pass/Fail
										case "34E5AB9F-66BF-4D4E-A5E5-3DBE179133E6":  //  Satisfactory/Unsatisfactory										
										case "BFE3D7C3-5EE1-4BF4-AC8D-CE4F33AE63C4":  //  True/False
										case "301F3909-CA52-482E-8481-E219AC39CFC1":  //  Yes/No
										default:
											results[i].BooleanValue = prevCheckItem.BooleanValue;
											results[i].ResolutionText = prevCheckItem.ResolutionText;
											break;
									}
									didPrev = true;
								}
							}


							if ((!prevValues && !didPrev) && (((results[i].TextValue == null || results[i].TextValue == "") && results[i].BooleanValue == null) && (results[i].DefaultValue != null && results[i].DefaultValue != "")))
							{
								results[i].BooleanValue = Convert.ToByte(results[i].DefaultValue);
								switch (results[i].CheckItemTypeId.ToString().ToUpper())
								{
									case "CE0D1281-6ADF-4ADC-AEA5-F3750F99FCCB":  //  List
										results[i].TextValue = results[i].DefaultValue;
										results[i].ResolutionText = results[i].DefaultValue;
										break;
									case "0B90628B-C6E3-406E-8EEE-92968E6AE835":  //  In/Out																				
									case "C92B6951-407E-4C75-9CC3-5DFA1B63DA4A":  //  OK/Service
									case "71E2DDDC-22E2-4661-92CC-F16FE7E66AE8":  //  Pass/Fail
									case "34E5AB9F-66BF-4D4E-A5E5-3DBE179133E6":  //  Satisfactory/Unsatisfactory										
									case "BFE3D7C3-5EE1-4BF4-AC8D-CE4F33AE63C4":  //  True/False
									case "301F3909-CA52-482E-8481-E219AC39CFC1":  //  Yes/No
										results[i].BooleanValue = Convert.ToByte(results[i].DefaultValue);
										results[i].ResolutionText = results[i].ResolutionText;
										break;
									case "760BD541-04DA-403B-8A9E-D1A705C57BD0":  //  Calculation
									case "2C33CAB4-45A9-48CF-A794-CA633CA6507A":  //  Check Box
									case "D290E005-205B-48C9-A689-90C68C911C24":  //  Text
									case "DF88CE10-9F66-47A2-AB3A-99CA24BE99F4":  //  Long Text
									case "F6D38607-9589-49DD-975D-609437106650":  //  Date										
									case "380714E6-4A48-4D89-B01C-445BC8C864F3":  //  Numeric										
									case "33648CDD-1B80-497F-BF65-B6797FC28D34":  //  Time
									default:
										results[i].TextValue = results[i].DefaultValue;
										results[i].ResolutionText = results[i].ResolutionText;
										break;
								}
							}

						}

						results[i].Resolutions = (await cwmContext.Resolutions.Where(a => a.ResolutionType == checkItemId && !string.IsNullOrEmpty(a.Resolution1)).OrderBy(r => r.Sequence).ThenBy(r => r.Resolution1).Select(a => new SelectListItem() { Text = a.Resolution1, Value = a.ResolutionId.ToString() }).ToListAsync()) ?? new List<SelectListItem>();
						if (results[i].Resolutions == null)
						{
							results[i].Resolutions = new List<SelectListItem>();
						}

						if (results[i].CheckItemTypeId == new Guid("2C33CAB4-45A9-48CF-A794-CA633CA6507A")) // Check Box
						{
							if (results[i].TextValue != String.Empty && results[i].TextValue.Length == results[i].Resolutions.Count())
							{
								results[i].CheckBoxValues = new List<bool>();
								for (int j = 0; j < results[i].Resolutions.Count(); j++)
								{
									results[i].CheckBoxValues.Add(results[i].TextValue.ElementAt(j) == '1' ? true : false);
								}
							}
							else
							{
								results[i].CheckBoxValues = new List<bool>(results[i].Resolutions.Count()) { false };
							}
						}
					}
				}
				else
				{
					Inspection inspection = cwmContext.Inspections.FirstOrDefault(ins => ins.InspectionId == activityId);
					Guid? agencyId2 = cwmContext.AgencyActivityTypes.FirstOrDefault(aat => aat.ActivityTypeId == inspection.ActivityTypeId).AgencyId ?? null;
					results = checkLists.ToList();
					string retval = "";
					retval += "Q:0 ";
					retval += "A:0 ";
					retval += "O:0 ";
					retval += "F:0 ";
					var setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNA" && set.AgencyId == agencyId2).ValueField;
					if (setting != null && setting != "")
					{
						retval += "NA:0 ";
					}
					setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNO" && set.AgencyId == agencyId2).ValueField;
					if (setting != null && setting != "")
					{
						retval += "NO:0 ";
					}

					retval += "V:0";
					for (int i = 0; i < results.Count(); i++)
					{

						results[i].InfoLine = retval;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to get checklists by Id - '" + activityId + "'.", ex);
				//throw;
			}
			return results;
		}

		public async Task<string> GetCheckListName(Guid checklistId)
		{
			string result = "";
			try
			{

				var checklist = await cwmContext.CheckLists.FirstOrDefaultAsync(c => c.CheckListId == checklistId);

				if (checklist != null)
				{
					result = checklist.CheckListName;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving Checlist Name for ChecklistId '" + checklistId.ToString() + "'.", ex);
			}

			return result;
		}
		//Task<SelectListItem> SaveResolutionAsync(Guid checkItemId, string resolutionText);
		public async Task<SelectListItem> SaveResolutionAsync(Guid checkItemId, string resolutionText)
		{
			SelectListItem result = null;
			var existingResolution = cwmContext.Resolutions.Where(a => a.ResolutionType == checkItemId && a.Resolution1 == resolutionText);
			if (existingResolution.Count() == 0)
			{
				var newResolution = cwmContext.Resolutions.Add(new Resolution());
				newResolution.ResolutionId = Guid.NewGuid();
				newResolution.ResolutionType = checkItemId;
				newResolution.Resolution1 = resolutionText;
				newResolution.rowguid = Guid.NewGuid();
				newResolution.ExternalId = null;
				newResolution.DateUpdated = DateTime.Now;
				newResolution.DateInserted = DateTime.Now;
				var existingCount = cwmContext.Resolutions.Where(a => a.ResolutionType == checkItemId).Count();
				newResolution.Sequence = existingCount + 1;
				var audit = new AuditModel { TableName = "Resolutions", RecordId = newResolution.ResolutionId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>
				{
					new AuditFieldModel { ControlName = "ResolutionType", FieldDesc = "Resolution Type", OldId = null, OldValue = null, NewId = checkItemId, NewValue = null },
					new AuditFieldModel { ControlName = "Resolution", FieldDesc = "Resolution", OldId = null, OldValue = null, NewId = null, NewValue = resolutionText }
				};
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();
						await auditService.UpdateAudit(audit, auditFields);
						result = new SelectListItem() { Value = newResolution.ResolutionId.ToString(), Text = resolutionText };
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save resolution for check item '" + checkItemId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to save resolution for check item '" + checkItemId.ToString() + "', DbContext was not available.");
				}
			}
			return result;
		}

		//Task<List<SearchActivities>> PerformActivitySearchAsync(string addressTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party);
		//public async Task<List<SearchActivities>> PerformActivitySearchAsync(string activityTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party)
		//{
		//    List<SearchActivities> results = new List<SearchActivities>();
		//    IEnumerable<v_Activities> activities = null;
		//    if (activities != null && activities.Count() > 0)
		//    {

		//    }
		//    if (activityTypeSearch != null && activityTypeSearch != Guid.Empty.ToString() && activityTypeSearch != "12345678-1234-1234-1234-123456789101" && activities != null && activities.Count() > 0)
		//    {
		//        var activityTypeId = Guid.Parse(activityTypeSearch);
		//    results = results.Where(a => a.ActivityTypeId != null && a.ActivityTypeId == activityTypeId).ToList();
		//    }

		//    return results;
		//}


		//Task<List<SearchAddress>> PerformSearchAsync(string addressTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party);
		public async Task<List<SearchAddress>> PerformSearchAsync(string addressTypeSearch, string searchType, string beginRange, string endRange, string direction, string streetAddress, string subStreetAddress, string suffix, bool hideInactive, string code, string city, string state, string zip, string region, string county, string occupancy, string property, string party)
		{
			List<SearchAddress> results = new List<SearchAddress>();
			IEnumerable<v_Addresses2> addresses = null;

			if (searchType == "Range")
			{
				beginRange = beginRange ?? String.Empty;
				endRange = endRange ?? String.Empty;
				direction = direction ?? String.Empty;
				streetAddress = streetAddress ?? String.Empty;
				subStreetAddress = subStreetAddress ?? String.Empty;
				suffix = suffix ?? String.Empty;

				int beginNumber, endNumber;
				var bothNumbersAvailable = (Int32.TryParse(beginRange, out beginNumber)) & (Int32.TryParse(endRange, out endNumber));

				try
				{
					if (bothNumbersAvailable)                                                                // Two numbers available, do a value compare
					{
						addresses = await (from a in cwmContext.v_Addresses2
										   where (direction == String.Empty || a.Direction == direction)
											  && (streetAddress == String.Empty || a.Address.Contains(streetAddress))
											  && (subStreetAddress == String.Empty || a.SubAddress.Contains(subStreetAddress))
											  && (suffix == String.Empty || a.Suffix == suffix)
											  && (!hideInactive || !a.Inactive)
										   select a).ToListAsync();

						addresses = addresses.Where(a => !String.IsNullOrWhiteSpace(a.AddressNumber) && CompareRange(beginNumber, endNumber, a.AddressNumber)).ToList();
					}
					else if (String.IsNullOrWhiteSpace(beginRange) != String.IsNullOrWhiteSpace(endRange))                 // One value available, string compare, everything that starts with that string
					{
						var searchTerm = (beginRange + endRange).Trim();
						addresses = await (from a in cwmContext.v_Addresses2
										   where a.AddressNumber.StartsWith(searchTerm)
											  && (direction == String.Empty || a.Direction == direction)
											  && (streetAddress == String.Empty || a.Address.Contains(streetAddress))
											  && (subStreetAddress == String.Empty || a.SubAddress.Contains(subStreetAddress))
											  && (suffix == String.Empty || a.Suffix == suffix)
											  && (!hideInactive || !a.Inactive)
										   select a).ToListAsync();
					}
					else                                                                                                   // If no earlier case, just do a simple string compare over selected range
					{
						addresses = await (from a in cwmContext.v_Addresses2
										   where (beginRange == String.Empty || beginRange.CompareTo(a.AddressNumber) <= 0)
											  && (endRange == String.Empty || endRange.CompareTo(a.AddressNumber) >= 0)
											  && (direction == String.Empty || a.Direction == direction)
											  && (streetAddress == String.Empty || a.Address.Contains(streetAddress))
											  && (subStreetAddress == String.Empty || a.SubAddress.Contains(subStreetAddress))
											  && (suffix == String.Empty || a.Suffix == suffix)
											  && (!hideInactive || !a.Inactive)
										   select a).ToListAsync();
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					var searchParams = ((!String.IsNullOrWhiteSpace(beginRange) ? "Begin Range = " + beginRange : String.Empty) +
										(!String.IsNullOrWhiteSpace(endRange) ? "   End Range = " + endRange : String.Empty) +
										(!String.IsNullOrWhiteSpace(direction) ? "   Direction = " + direction : String.Empty) +
										(!String.IsNullOrWhiteSpace(streetAddress) ? "   Street Address = " + streetAddress : String.Empty) +
										(!String.IsNullOrWhiteSpace(subStreetAddress) ? "   Sub-Street Address = " + subStreetAddress : String.Empty) +
										(!String.IsNullOrWhiteSpace(suffix) ? "   Suffix = " + suffix : String.Empty)).TrimStart();
					logger.Error("Unexpected exception thrown while performing a search, " + searchParams + ".", ex);
				}
				if (addresses != null && addresses.Count() > 0)
				{
					var addressTypeList = await cwmContext.AddressTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.AddressTypeId).ToListAsync();
					var addressList = addresses.ToList();
					if (addressList != null && addressList.Count() > 0 && addressTypeList != null && addressTypeList.Count() > 0)
					{
						addressList = addressList.Where(a => addressTypeList.Contains(a.AddressTypeId == null ? Guid.Empty : a.AddressTypeId.Value)).ToList();
					}
					addresses = addressList.AsEnumerable();
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
			}
			else if (searchType == "Code")
			{
				code = code ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (code != String.Empty)
					{
						var lowercase = code.ToLower();
						addresses = addresses.Where(a => a.AddressCode != null && a.AddressCode.ToLower().Contains(lowercase));
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address code: " + code + ".", ex);
				}
			}
			else if (searchType == "City")
			{
				city = city ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (city != String.Empty)
					{
						var lowercase = city.ToLower();
						addresses = addresses.Where(a => a.City != null && a.City.ToLower().Contains(lowercase));
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address city: " + city + ".", ex);
				}
			}
			else if (searchType == "State")
			{
				state = state ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (state != Guid.Empty.ToString())
					{
						var stateId = Guid.Parse(state);
						addresses = addresses.Where(a => a.StateId != null && a.StateId == stateId);
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address state: " + state + ".", ex);
				}
			}
			else if (searchType == "Zip")
			{
				zip = zip ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (zip != Guid.Empty.ToString())
					{
						var zipId = Guid.Parse(zip);
						var zipObject = await cwmContext.Zips.SingleOrDefaultAsync(a => a.ZipId == zipId);
						addresses = addresses.Where(a => a.Zip != null && a.Zip == zipObject.Zip1);
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address zip: " + zip + ".", ex);
				}
			}
			else if (searchType == "Region")
			{
				region = region ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (region != Guid.Empty.ToString())
					{
						var regionId = Guid.Parse(region);
						var regionObject = await cwmContext.Regions.SingleOrDefaultAsync(a => a.RegionId == regionId);
						addresses = addresses.Where(a => a.RegionId != null && a.RegionId == regionObject.RegionId);
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address region: " + region + ".", ex);
				}
			}
			else if (searchType == "County")
			{
				county = county ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (county != Guid.Empty.ToString())
					{
						var countyId = Guid.Parse(county);
						var countyObject = await cwmContext.Counties.SingleOrDefaultAsync(a => a.CountyId == countyId);
						addresses = addresses.Where(a => a.CountyId != null && a.CountyId == countyObject.CountyId);
					}
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address county: " + county + ".", ex);
				}
			}
			else if (searchType == "Occupancy")
			{
				occupancy = occupancy ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (occupancy != Guid.Empty.ToString())
					{
						var occupancyId = Guid.Parse(occupancy);
						addresses = addresses.Where(a => a.OccupancyTypeId != null && a.OccupancyTypeId == occupancyId);
					}
					else
					{
						var occupancyTypes = await GetOccupancyTypeListAsync();
						var occupancyGuids = occupancyTypes.Select(a => a.OccupancyTypeId).ToList();
						addresses = addresses.Where(a => a.OccupancyTypeId != null && occupancyGuids.Contains(a.OccupancyTypeId == null ? Guid.Empty : a.OccupancyTypeId.Value));
					}
					addresses = addresses.OrderBy(a => a.OccupancyType);
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address occupancy type: " + occupancy + ".", ex);
				}
			}
			else if (searchType == "Property")
			{
				property = property ?? String.Empty;
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					if (property != Guid.Empty.ToString())
					{
						var propertyId = Guid.Parse(property);
						addresses = addresses.Where(a => a.PropertyUseTypeId != null && a.PropertyUseTypeId == propertyId);
					}
					else
					{
						var propertyUses = await GetPropertyUseTypeListAsync();
						var propertyGuids = propertyUses.Select(a => a.PropertyUseTypeId).ToList();
						addresses = addresses.Where(a => a.PropertyUseTypeId != null && propertyGuids.Contains(a.PropertyUseTypeId == null ? Guid.Empty : a.PropertyUseTypeId.Value));
					}
					addresses = addresses.OrderBy(a => a.PropertyUseType);
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address property use type: " + property + ".", ex);
				}
			}
			else if (searchType == "Party")
			{
				var partyName = party ?? String.Empty;
				try
				{
					var addressParties = new List<v_AddressParties>();
					if (partyName != String.Empty)
					{
						partyName = partyName.ToLower();
						addressParties = await cwmContext.v_AddressParties.Where(a => a.PartyName.ToLower().Contains(partyName) && a.AddressId != null && a.Inactive != null && a.Inactive == false && a.RoleTypeId != null).ToListAsync();
					}
					else
					{
						addressParties = await cwmContext.v_AddressParties.Where(a => a.AddressId != null && a.Inactive != null && a.Inactive == false && a.RoleTypeId != null).ToListAsync();
					}
					var roleList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
					if (addressParties != null && addressParties.Count() > 0)
					{
						addressParties = addressParties.Where(a => roleList.Contains(a.RoleTypeId.Value)).ToList();
						if (addressParties != null && addressParties.Count() > 0)
						{
							addresses = await GetAddressesAsync(!hideInactive);
							var addressIdList = addresses.Select(a => a.AddressId).ToList();
							for (int i = addressParties.Count() - 1; i > -1; i--)
							{
								if (addressIdList.Contains(addressParties[i].AddressId == null ? Guid.Empty : addressParties[i].AddressId.Value))
								{
									var searchRow = new SearchAddress
									{
										AddressId = addressParties[i].AddressId == null ? Guid.Empty : addressParties[i].AddressId.Value,
										Inactive = addressParties[i].Inactive == null ? false : addressParties[i].Inactive.Value,
										AddressType = addressParties[i].AddressType,
										AddressTypeId = addressParties[i].AddressTypeId == null ? Guid.Empty : addressParties[i].AddressTypeId.Value,
										AddressCode = addressParties[i].AddressCode,
										AddressNumber = addressParties[i].AddressNumber,
										Direction = addressParties[i].Direction,
										Address = addressParties[i].Address,
										SubAddress = addressParties[i].SubAddress,
										City = addressParties[i].City,
										Suffix = addressParties[i].Suffix,
										StateAbbr = addressParties[i].StateAbbr,
										Zip = addressParties[i].Zip,
										Comment = addressParties[i].Comment,
										Party = addressParties[i].PartyName
									};
									results.Add(searchRow);
								}
							}
							results = results.OrderBy(a => a.Party).ToList();
						}
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for a Address party: " + party + ".", ex);
				}
			}
			else if (searchType == "All")
			{
				try
				{
					addresses = await GetAddressesAsync(!hideInactive);
					results.AddRange(addresses.Select(a => Mapper.Map<SearchAddress>(a)).ToList());
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception thrown while performing a search for all addresses.", ex);
				}
			}
			if (addressTypeSearch != null && addressTypeSearch != Guid.Empty.ToString() && addressTypeSearch != "12345678-1234-1234-1234-123456789101" && addresses != null && addresses.Count() > 0)
			{
				var addressTypeId = Guid.Parse(addressTypeSearch);
				results = results.Where(a => a.AddressTypeId != null && a.AddressTypeId == addressTypeId).ToList();
			}
			return results;
		}

		//Task<IEnumerable<v_Addresses2>> GetAddressesAsync(bool showInactive);
		public async Task<IEnumerable<v_Addresses2>> GetAddressesAsync(bool showInactive)
		{
			IEnumerable<v_Addresses2> result;
			try
			{
				var addressList = new List<v_Addresses2>();
				if (showInactive == false)
				{
					addressList = await cwmContext.v_Addresses2.Where(a => a.Inactive == false).ToListAsync();
				}
				else
				{
					addressList = await cwmContext.v_Addresses2.ToListAsync();
				}
				var addressTypeList = await cwmContext.AddressTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.AddressTypeId).ToListAsync();
				if (addressList != null && addressList.Count() > 0)
				{
					for (int i = addressList.Count() - 1; i > -1; i--)
					{
						var addressType = addressList[i].AddressTypeId == null ? Guid.Empty : addressList[i].AddressTypeId.Value;
						if (!addressTypeList.Contains(addressType))
						{
							addressList.RemoveAt(i);
						}
					}
				}
				result = addressList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address List.", ex);
				result = new List<v_Addresses2>();
			}
			return result;
		}

		//Task<IEnumerable<OccupancyType>> GetOccupancyTypeListAsync();
		public async Task<IEnumerable<OccupancyType>> GetOccupancyTypeListAsync()
		{
			IEnumerable<OccupancyType> result;
			try
			{
				result = await cwmContext.OccupancyTypes.Where(o => !o.Inactive && o.WebViewable).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Occupancy Type List.", ex);
				result = new List<OccupancyType>();
			}
			return result;
		}

		//Task<IEnumerable<PropertyUseType>> GetPropertyUseTypeListAsync();
		public async Task<IEnumerable<PropertyUseType>> GetPropertyUseTypeListAsync()
		{
			IEnumerable<PropertyUseType> result;
			try
			{
				result = await cwmContext.PropertyUseTypes.Where(o => !o.Inactive && o.WebViewable).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Property Use Type List.", ex);
				result = new List<PropertyUseType>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Activities>> GetActivitiesByInspectorIdAsync(Guid id)
		{
			IEnumerable<v_Activities> result;
			try
			{
				result = await cwmContext.v_Activities.Where(p => p.InspectorId == id).ToListAsync();
				foreach (v_Activities item in result)
				{
					if (item.Comment != null && item.Comment != "" && item.Comment.Contains(@"\rtf"))
					{
						System.Windows.Forms.RichTextBox richTextbox = new System.Windows.Forms.RichTextBox();
						richTextbox.Rtf = item.Comment;
						item.Comment = richTextbox.Text;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activities List.", ex);
				result = new List<v_Activities>();
			}
			return result;
		}

		//GetActivitiesByAHJIdAsync
		public async Task<IEnumerable<v_Activities>> GetActivitiesByAHJIdAsync(Guid id)
		{
			IEnumerable<v_Activities> result;
			try
			{
				var allIds = cwmContext.Inspectors.Where(i => i.AgencyId == id).Select(i => i.InspectorId);
				var searchIds = allIds.ToList();

				result = await cwmContext.v_Activities.Where(p => searchIds.Contains(p.InspectorId ?? Guid.Empty)).ToListAsync();
				foreach (v_Activities item in result)
				{
					if (item.Comment != null && item.Comment != "" && item.Comment.Contains(@"\rtf"))
					{
						System.Windows.Forms.RichTextBox richTextbox = new System.Windows.Forms.RichTextBox();
						richTextbox.Rtf = item.Comment;
						item.Comment = richTextbox.Text;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activities List.", ex);
				result = new List<v_Activities>();
			}
			return result;
		}

		//Task<bool> CreateActivityAsync(v_Activities model, List<CheckItemModel> checkItems);
		public async Task<bool> CreateActivityAsync(DetailedActivity model, List<CheckItemModel> checkItems)
		{
			var result = false;

			if (model != null)
			{
				var audit = new AuditModel { TableName = "Inspections", RecordId = model.InspectionId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				var activity = cwmContext.Inspections.Add(new Data.Inspection());
				var actType = (await cwmContext.ActivityTypes.SingleOrDefaultAsync(at => at.ActivityTypeId == (model.ActivityTypeId ?? Guid.Empty))).ActivityType1;
				activity.rowguid = Guid.NewGuid();
				activity.DateInserted = DateTime.Now;
				activity.DateUpdated = activity.DateInserted;
				activity.PrimaryParty = false;

				var violationCounts = CalculatViolationCounts();
				if (violationCounts.Length == 0)
				{
					activity.NewViolations = 0;
					activity.OldViolations = 0;
					activity.CorrectedViolations = 0;
					activity.ViolationCounts = 0;
					activity.SubViolations = 0;
					activity.OldSubViolations = 0;
				}
				else
				{
					activity.NewViolations = violationCounts[0];
					activity.OldViolations = violationCounts[1];
					activity.CorrectedViolations = violationCounts[2];
					activity.ViolationCounts = violationCounts[3];
					activity.SubViolations = violationCounts[4];
					activity.OldSubViolations = violationCounts[5];
				}
				activity.dummyAgreement = false;
				activity.FollowUp = false;
				activity.SignOffComplete = false;
				activity.InspectionId = model.InspectionId;
				activity.InspectionNumber = (await GetNextActivityNumber(GetNumberType(actType), model.InspectionDate, "", null, model.InspectorId));
				activity.InspectionCauseId = model.InspectionCauseId;
				activity.AddressId = model.AddressId;
				activity.InspectorId = model.InspectorId;
				activity.InspectedPartyId = model.InspectedPartyId;
				activity.InspectedPartyRoleTypeId = model.InspectedPartyRoleTypeId;
				activity.InspectionDate = model.InspectionDate;
				activity.InspectionTypeId = model.InspectionTypeId;
				activity.Hrs = model.Hrs;
				activity.Complete = model.Complete;
				activity.AlternatePartyId = model.AlternatePartyId;
				activity.AlternatePartyRoleTypeId = model.AlternatePartyRoleTypeId;
				activity.ItemId = model.ItemId;
				activity.ItemInspectionStatusId = model.ItemInspectionStatusId;
				activity.ActivityTypeId = model.ActivityTypeId;
				activity.EndDate = model.EndDate;
				activity.StartDate = model.StartDate;
				activity.SecondaryInspectorId = model.SecondaryInspectorId;
				activity.SecAddressId = model.SecAddressId;

				if (model.Comment != null && model.Comment != "")
				{
					var actComment = cwmContext.Comments.Add(new Data.Comment());
					actComment.rowguid = Guid.NewGuid();
					actComment.RecordId = model.InspectionId;
					actComment.DateInserted = DateTime.Now;
					actComment.DateUpdated = activity.DateInserted;
					actComment.CommentId = Guid.NewGuid();
					actComment.PlainText = true;
					actComment.Comment1 = model.Comment;
				}

				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();

						CreateCheckListItems(checkItems, model.InspectionId);

						await ((DbContext)cwmContext).SaveChangesAsync();

						CreateDefaultFees(model.InspectionId);

						await ((DbContext)cwmContext).SaveChangesAsync();

						if (auditFields.Count() > 0)
						{
							await auditService.UpdateAudit(audit, auditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create activity '" + model.InspectionId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create activity '" + model.InspectionId.ToString() + "', DbContext was not available.");
				}
			}
			return result;
		}

		public async Task<bool> SaveActivityAsync(DetailedActivity model, List<CheckItemModel> checkItems)
		{
			//Change this for saveing not creating

			var result = false;
			if (model != null)
			{
				var audit = new AuditModel { TableName = "Inspections", RecordId = model.InspectionId, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				var activity = cwmContext.Inspections.First(i => i.InspectionId == model.InspectionId);

				activity.InspectionCauseId = model.InspectionCauseId;
				activity.AddressId = model.AddressId;
				activity.InspectorId = model.InspectorId;
				activity.InspectedPartyId = model.InspectedPartyId;
				activity.InspectedPartyRoleTypeId = model.InspectedPartyRoleTypeId;
				activity.InspectionDate = model.InspectionDate;
				activity.InspectionTypeId = model.InspectionTypeId;
				activity.Hrs = model.Hrs;
				activity.Complete = model.Complete;
				activity.AlternatePartyId = model.AlternatePartyId;
				activity.AlternatePartyRoleTypeId = model.AlternatePartyRoleTypeId;
				activity.ItemId = model.ItemId;
				activity.ItemInspectionStatusId = model.ItemInspectionStatusId;
				activity.ActivityTypeId = model.ActivityTypeId;
				activity.EndDate = model.EndDate;
				activity.StartDate = model.StartDate;
				activity.SecondaryInspectorId = model.SecondaryInspectorId;
				activity.SecAddressId = model.SecAddressId;
				activity.DateUpdated = DateTime.Now;

				if (model.Comment != null && model.Comment != "")
				{
					var actComment = cwmContext.Comments.FirstOrDefault(c => c.RecordId == model.InspectionId);
					if (actComment == null || actComment.CommentId == Guid.Empty)
					{
						actComment = cwmContext.Comments.Add(new Data.Comment());
						actComment.rowguid = Guid.NewGuid();
						actComment.RecordId = model.InspectionId;
						actComment.DateInserted = DateTime.Now;
						actComment.DateUpdated = actComment.DateInserted;
						actComment.CommentId = Guid.NewGuid();
					}

					actComment.PlainText = true;
					actComment.Comment1 = model.Comment;
				}
				//CreateCheckListItems(checkItems, model.InspectionId);

				if (cwmContext is DbContext)
				{
					try
					{
						DbChangeTracker dBChangeTracker = ((DbContext)cwmContext).ChangeTracker;
						foreach (var entry in dBChangeTracker.Entries())
						{
							if (entry.Entity.GetType().FullName.Contains("v_"))
							{
								entry.State = EntityState.Unchanged;
							}
						}
						await ((DbContext)cwmContext).SaveChangesAsync();

						CreateCheckListItems(checkItems, model.InspectionId);

						await ((DbContext)cwmContext).SaveChangesAsync();

						if (auditFields.Count() > 0)
						{
							await auditService.UpdateAudit(audit, auditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create activity '" + model.InspectionId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create activity '" + model.InspectionId.ToString() + "', DbContext was not available.");
				}


			}
			return result;
		}

		public async Task<bool> SaveActivityCLAsync(List<CheckItemModel> checkItems, Guid inspectionId)
		{
			//Change this for saveing not creating
			//Guid inspectionId = checkItems[0].ActivityId;
			var result = false;
			if (checkItems != null)
			{
				var audit = new AuditModel { TableName = "Inspections", RecordId = inspectionId, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				try
				{
					DbChangeTracker dBChangeTracker = ((DbContext)cwmContext).ChangeTracker;
					foreach (var entry in dBChangeTracker.Entries())
					{
						if (entry.Entity.GetType().FullName.Contains("v_"))
						{
							entry.State = EntityState.Unchanged;
						}
					}
					await ((DbContext)cwmContext).SaveChangesAsync();

					CreateCheckListItems(checkItems, inspectionId);

					await ((DbContext)cwmContext).SaveChangesAsync();

					if (auditFields.Count() > 0)
					{
						await auditService.UpdateAudit(audit, auditFields);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save checklist values for activity '" + inspectionId.ToString() + "'.", ex);
				}

			}
			return result;
		}

		public async Task SaveUserDefinedValuesAsync(List<UserDefValue> list)
		{
			if (list != null && list.Count() > 0)
			{
				try
				{
					for (int i = 0; i < list.Count(); i++)
					{
						var audit = new AuditModel { TableName = "UserDefValues", Description = "" };
						var auditFields = new List<AuditFieldModel>();
						var auditField = new AuditFieldModel { ControlName = "UserValues[i].FieldValue", };
						var userDefinedValue = new UserDefValue();

						if (list[i].UserDefValueId != null && list[i].UserDefValueId != Guid.Empty)
						{
							Guid tempGuid = list[i].UserDefValueId;
							userDefinedValue = await cwmContext.UserDefValues.SingleOrDefaultAsync(a => a.UserDefValueId == tempGuid);
							auditField.OldId = userDefinedValue.UserDefValueId;
							auditField.OldValue = userDefinedValue.UserDefValue1;
							audit.AuditAction = "RECORD UPDATED";
						}
						else
						{
							userDefinedValue = cwmContext.UserDefValues.Add(new Data.UserDefValue());
							userDefinedValue.UserDefValueId = Guid.NewGuid();
							userDefinedValue.UserDefFieldId = list[i].UserDefFieldId;
							userDefinedValue.DateInserted = DateTime.Now;
							userDefinedValue.RecordId = list[i].RecordId;
							userDefinedValue.rowguid = list[i].rowguid != Guid.Empty ? list[i].rowguid : Guid.NewGuid();
							userDefinedValue.VActPrint = false;
							userDefinedValue.ExternalId = null;
							auditField.OldId = null;
							auditField.OldValue = null;
							audit.AuditAction = "RECORD CREATED";
						}
						userDefinedValue.UserDefValue1 = list[i].UserDefValue1;
						userDefinedValue.DateUpdated = DateTime.Now;
						auditField.NewId = userDefinedValue.UserDefValueId;
						auditField.NewValue = userDefinedValue.UserDefValue1;
						auditField.FieldDesc = cwmContext.UserDefFields.FirstOrDefault(a => a.UserDefFieldId == userDefinedValue.UserDefFieldId).FieldDesc;
						audit.RecordId = userDefinedValue.UserDefValueId;

						if (cwmContext is DbContext)
						{
							try
							{
								await ((DbContext)cwmContext).SaveChangesAsync();
								var idCheck = (auditField.OldId ?? Guid.Empty) != (auditField.NewId ?? Guid.Empty);
								var valCheck = (auditField.OldValue != null ? auditField.OldValue : String.Empty) != (auditField.NewValue != null ? auditField.NewValue : String.Empty);
								if (idCheck || valCheck)
								{
									auditFields.Add(auditField);
									await auditService.UpdateAudit(audit, auditFields);
								}
							}
							catch (Exception ex)
            {
                _ = ex;
								logger.Error("Unable to save the user defined value changes for '" + list[i].RecordId.ToString() + "'.", ex);
								return;
							}
						}
						else
						{
							logger.Error("Unable to update the user defined values for '" + list[i].RecordId.ToString() + "', DbContext was not available.");
							return;
						}
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to complete function: SaveUserDefinedValuesAsync. " + ex);
				}
			}
			else
			{
				logger.Error("SaveUserDefinedValuesAsync was called with a null reference.");
			}
		}


		private void CreateCheckListItems(List<CheckItemModel> checkItems, Guid inspectionId)
		{
			//Need to Add Audits			
			CheckItemValue newCheckItem;

			foreach (var checkItem in checkItems)
			{
				if (checkItem.CheckItemValueId == null || checkItem.CheckItemValueId == Guid.Empty)
				{
					newCheckItem = cwmContext.CheckItemValues.Add(new Data.CheckItemValue());
					newCheckItem.rowguid = Guid.NewGuid();
					newCheckItem.CheckItemValueId = Guid.NewGuid();
					newCheckItem.DateUpdated = DateTime.Now;
					newCheckItem.DateInserted = newCheckItem.DateUpdated;
				}
				else
				{
					newCheckItem = cwmContext.CheckItemValues.First(c => c.CheckItemValueId == checkItem.CheckItemValueId);
					newCheckItem.DateUpdated = DateTime.Now;
				}

				newCheckItem.InspectionId = inspectionId;
				newCheckItem.CheckItemId = checkItem.CheckItemId;
				newCheckItem.TextValue = checkItem.TextValue;
				newCheckItem.BooleanValue = checkItem.BooleanValue;
				newCheckItem.ResolutionText = checkItem.ResolutionText;
				if (checkItem.CorrectedInspectionId == null && checkItem.Corrected != null)
				{
					newCheckItem.Corrected = checkItem.Corrected;
					newCheckItem.CorrectedInspectionId = inspectionId;
				}
			}
		}

		private void CreateDefaultFees(Guid InspectionId)
		{
			var activity = cwmContext.v_Activities.Single(a => a.InspectionId == InspectionId);
			bool isReinspection = activity.InspectionCause.ToUpper() == "Reinspection".ToUpper();
			Guid currentDefaultFeeId;
			if (isReinspection)
			{

				string reInspLetter;

				if (activity.InspectionNumber.Contains(".S0"))
				{
					int First = activity.InspectionNumber.LastIndexOf(".S");

					int Second = activity.InspectionNumber.LastIndexOf(".", First - 1);

					reInspLetter = activity.InspectionNumber.Substring(Second + 1, First - (Second + 1));
				}
				else
				{
					reInspLetter = activity.InspectionNumber.Substring(activity.InspectionNumber.LastIndexOf(".") + 1, activity.InspectionNumber.Length - (activity.InspectionNumber.LastIndexOf(".") + 1));
				}


				var defaultReFees = feeService.GetDefaultFees(activity.InspectionTypeId, true, reInspLetter);


				foreach (DetailedDefaultFee oFee in defaultReFees)
				{

					if (oFee.FeeAmount != null && oFee.FeeAmount != "")
					{
						currentDefaultFeeId = feeService.DefaultRegFee(InspectionId, oFee.FeeAmount, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
					}
					else
					{
						if (oFee.FeeTypeId != null && oFee.FeeTypeId != Guid.Empty)
						{
							if (oFee.FeeSchedId != null && oFee.FeeSchedId != Guid.Empty)
							{
								currentDefaultFeeId = feeService.DefaultRateFee(InspectionId, oFee.FeeSchedId, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
							}
							else
							{
								currentDefaultFeeId = feeService.DefaultRRFee(InspectionId, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);


							}

						}
					}

				}
			}
			else
			{
				var defaultFees = feeService.GetDefaultFees(activity.InspectionTypeId, false, "");

				foreach (DetailedDefaultFee oFee in defaultFees)
				{
					if ((oFee.FeeAmount != null && oFee.FeeAmount != "") && oFee.FeeAmount == "0")
					{
						double feeAmt = 0;

						if (feeAmt == 0)
						{
							currentDefaultFeeId = feeService.DefaultRegFee(InspectionId, oFee.FeeAmount, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
						}
						else
						{
							currentDefaultFeeId = feeService.DefaultRegFee(InspectionId, feeAmt.ToString(), oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
						}
					}
					else if (oFee.FeeAmount != null && oFee.FeeAmount != "")
					{
						currentDefaultFeeId = feeService.DefaultRegFee(InspectionId, oFee.FeeAmount, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
					}

					else
					{
						if (oFee.FeeTypeId != null)
						{
							if (oFee.FeeSchedId != null && oFee.FeeSchedId != Guid.Empty)
							{
								currentDefaultFeeId = feeService.DefaultRateFee(InspectionId, oFee.FeeSchedId, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
							}
							else
							{
								if (oFee.TotalPercent)
								{
									currentDefaultFeeId = feeService.DefaultPOTFee(InspectionId, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
								}
								else
								{
									currentDefaultFeeId = feeService.DefaultRRFee(InspectionId, oFee.FeeTypeId, activity.InspectionDate ?? DateTime.Now, activity.InspectedPartyId);
								}
							}
						}
					}

				}
			}
		}

		private bool CompareRange(int beginNumber, int endNumber, string addressNumber)
		{
			var result = false;
			int addressValue;
			if (Int32.TryParse(addressNumber, out addressValue))           // Current data shows nearly all address numbers are integer values
			{
				result = (beginNumber <= addressValue) && (addressValue <= endNumber);
			}
			else                                                           // Very few should require this more expensive operation
			{                                                              // Just extracting the first number from the string, any additional numbers are ignored.
				addressNumber = addressNumber.TrimStart();
				int index = 0;
				while (index < addressNumber.Length && Char.IsDigit(addressNumber[index]))
					++index;
				addressNumber = addressNumber.Substring(0, index);
				result = Int32.TryParse(addressNumber, out addressValue) ? (beginNumber <= addressValue) && (addressValue <= endNumber) : false;
			}
			return result;
		}

		//Task<IEnumerable<v_Files>> GetFilesByActivityIdAsync(Guid id);
		public async Task<IEnumerable<v_Files>> GetFilesByActivityIdAsync(Guid id)
		{
			IEnumerable<v_Files> result;
			try
			{
				result = await cwmContext.v_Files.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity Files List.", ex);
				result = new List<v_Files>();
			}
			return result;
		}

		public async Task<Guid?> GetCheckListIdAsync(string checklistName, Guid? inspectionTypeId = null)
		{
			Guid? result = null;

			string oSQL = "";

			oSQL = "Select chl.CheckListId From CheckLists chl LEFT OUTER JOIN ChecklistActivityTypes ";

			oSQL += "on chl.CheckListId = CheckListActivityTypes.CheckListId Where CheckListName='" + checklistName + "'";

			if (inspectionTypeId != null)
			{
				oSQL += " AND InspectionTypeId='" + inspectionTypeId + "'";
			}

			var checklist = cwmContext.CheckLists.SqlQuery(oSQL, null);

			if (checklist != null)
			{
				result = (await checklist.FirstOrDefaultAsync()).CheckListId;
			}

			return result;
		}

		public async Task<string> GetNextActivityNumber(string strType, DateTime? activityDate, string permitNumber = "", Guid? groupdId = null, Guid? inspectorId = null)
		{
			string strNextNumber = "";
			ISystemService systemService = new SystemService(this.cwmContext, this.logger);

			string retval = "";

			bool OneUp = false;

			System.Text.StringBuilder newFrontSchema = new System.Text.StringBuilder();
			System.Text.StringBuilder newBackSchema = new System.Text.StringBuilder();
			string Schema;
			string[] SchemaSub;
			int Num = 0;
			string BlankString;

			string[] TableArray = new string[4];


			string groupCode = null;


			//Guid? agencyId = new Guid("9808204f-d941-451e-b121-02c8a0d7e7fa");
			//Guid logId = new Guid("1f67d542-70e9-4580-be79-007b39276161");
			//string loginCode = (await cwmContext.Inspectors.FirstAsync(i => i.InspectorId == logId)).Code;

			Guid? agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
			Guid currentUserId = new Guid(HttpContext.Current.Session["CodepalUserId"].ToString());

			string loginCode = (await cwmContext.Inspectors.SingleOrDefaultAsync(i => i.InspectorId == currentUserId)).Code;


			string loginCodeOther = (await cwmContext.Inspectors.SingleOrDefaultAsync(i => i.InspectorId == (inspectorId ?? Guid.Empty))).Code;

			var group = (await cwmContext.Groups.SingleOrDefaultAsync(i => i.GroupId == (groupdId ?? Guid.Empty)));
			if (group != null) groupCode = group.Code;

			string sepChar = await systemService.GetCodepalSetting("NumberSchemaAcSep", agencyId);

			Guid moduleId = (await cwmContext.Modules.SingleOrDefaultAsync(m => m.ModuleDesc == "Activity" && m.AgencyId == agencyId)).ModuleId;


			bool doForPermit = await systemService.GetCodepalBooleanSetting("NumberSchemaANumFromPNum", agencyId);


			try
			{
				if (permitNumber != "" && doForPermit)
				{
					newFrontSchema.Append(permitNumber + "-A");
					strNextNumber = "01";
					Num = 2;
					OneUp = true;
				}
				else
				{
					if (activityDate == null)
					{
						activityDate = DateTime.Now;
					}
					if (strType == null || strType == "")
					{
						strType = "I";
					}
					//if(sepChar == "")
					//{
					//	sepChar = "-";
					//}

					Schema = await systemService.GetCodepalSetting("NumberSchemaActivity", agencyId);
					if (Schema == "")
					{
						Schema = "0*|1*|4*|94";
					}
					SchemaSub = Schema.Split('|');
					newBackSchema.Append(sepChar);

					for (int intI = 0; intI < SchemaSub.Length; intI++)
					{
						switch (SchemaSub[intI].Substring(0, 1).Replace("*", ""))
						{
							case "0":
								if (OneUp)
									newBackSchema.Append(strType);
								else
									newFrontSchema.Append(strType);

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = strType;

								break;
							case "1":
								if (OneUp)
									newBackSchema.Append(loginCode);
								else
									newFrontSchema.Append(loginCode);

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = loginCode;

								break;
							case "2":
								if (OneUp)
									newBackSchema.Append(loginCodeOther);
								else
									newFrontSchema.Append(loginCodeOther);

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = loginCodeOther;

								break;
							case "3":
								if (OneUp)
									newBackSchema.Append(groupCode);
								else
									newFrontSchema.Append(groupCode);

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = groupCode;
								break;
							case "4":
								if (OneUp)
									newBackSchema.Append(activityDate.Value.ToString("yy"));
								else
									newFrontSchema.Append(activityDate.Value.ToString("yy"));

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = activityDate.Value.ToString("yy");
								break;

							case "5":
								if (OneUp)
									newBackSchema.Append(activityDate.Value.ToString("yyyy"));
								else
									newFrontSchema.Append(activityDate.Value.ToString("yyyy"));

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = activityDate.Value.ToString("yyyy");
								break;
							case "6":
								if (OneUp)
									newBackSchema.Append(activityDate.Value.ToString("MM"));
								else
									newFrontSchema.Append(activityDate.Value.ToString("MM"));

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = activityDate.Value.ToString("MM");
								break;
							case "7":
								if (OneUp)
									newBackSchema.Append(Math.Abs(activityDate.Value.Month / 3) + 1);

								else
									newFrontSchema.Append(Math.Abs(activityDate.Value.Month / 3) + 1);

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = (Math.Abs(activityDate.Value.Month / 3) + 1).ToString();
								break;
							case "8":
								if (OneUp)
									newBackSchema.Append(SchemaSub[intI].Replace("*", "").Remove(0, 1));
								else
									newFrontSchema.Append(SchemaSub[intI].Replace("*", "").Remove(0, 1));

								if (SchemaSub[intI].IndexOf('*') >= 0)
									TableArray[intI] = SchemaSub[intI].Replace("*", "").Remove(0, 1);
								break;
							case "9":
								OneUp = true;
								Num = Convert.ToInt32(SchemaSub[intI].Remove(0, 1));
								break;
							default:
								break;
						}


						if (OneUp && SchemaSub[intI].Substring(0, 1) != "7")
						{
							newFrontSchema.Append(sepChar);
							newBackSchema.Append(sepChar);
						}
						else
							newFrontSchema.Append(sepChar);

					}

					while (newFrontSchema.ToString().Substring(newFrontSchema.Length - 1, 1) == sepChar)
					{
						newFrontSchema.Remove(newFrontSchema.Length - 1, 1);
					}

					newFrontSchema.Append(sepChar);

					while (newBackSchema.ToString().Length > 0 && (newBackSchema.ToString().Substring(newBackSchema.Length - 1, 1) == sepChar && sepChar.Length > 0))
					{
						newBackSchema.Remove(newBackSchema.Length - 1, 1);

					}
					while (newBackSchema.ToString().Length > 0 && (newBackSchema.ToString().Substring(0, 1) == sepChar && sepChar.Length > 0))
					{
						newBackSchema.Remove(0, 1);
					}

					if (newBackSchema.ToString().Length > 0)
						newBackSchema.Insert(0, sepChar, 1);

					if (Num >= 2)
					{
						for (int intj = 2; intj <= Num; intj++)
						{
							strNextNumber += "0";
						}
						strNextNumber += "1";
					}
					else
						strNextNumber = "0001";
				}

				if (OneUp)
				{
					BlankString = "";
					for (int i = 0; i < Num; i++)
					{
						BlankString += "_";
					}

					if (doForPermit)
					{
						//var CurNum = (await cwmContext.Inspections.FirstOrDefaultAsync(i=>i.));
						strNextNumber = ((from i in cwmContext.Inspections where SqlMethods.Like(i.InspectionNumber, newFrontSchema.ToString() + "%") && (SqlFunctions.IsNumeric(i.InspectionNumber.Replace(newFrontSchema.ToString(), "")) == 1) select Convert.ToInt32(i.InspectionNumber.Replace(newFrontSchema.ToString(), ""))).Max() + 1).ToString();
						while (strNextNumber.Length < Num)
						{
							strNextNumber = "0" + strNextNumber;
						}
					}
					else
					{
						string part1 = TableArray.Length > 0 ? TableArray[0] : null;
						string part2 = TableArray.Length > 1 ? TableArray[1] : null;
						string part3 = TableArray.Length > 2 ? TableArray[2] : null;
						string part4 = TableArray.Length > 3 ? TableArray[3] : null;
						string part5 = TableArray.Length > 4 ? TableArray[4] : null;
						//string part6 = TableArray[5];
						var curNums = await cwmContext.NumberSchemas.Where(n => n.ModuleId == moduleId && n.Part1 == part1).ToListAsync();

						NumberSchema curNum = null;

						if (part2 != null && part2 != "") curNums = curNums.Where(n => n.Part2 == part2).ToList();
						if (part3 != null && part3 != "") curNums = curNums.Where(n => n.Part3 == part3).ToList();
						if (part4 != null && part4 != "") curNums = curNums.Where(n => n.Part4 == part4).ToList();
						if (part5 != null && part5 != "") curNums = curNums.Where(n => n.Part5 == part5).ToList();
						//if (part6 != null && part6 != "") curNums = curNums.Where(n => n.Part1 == part6).ToList();
						if (curNums.Count() == 1) curNum = curNums.First();

						if (curNum != null)
						{
							strNextNumber = (Convert.ToInt32(curNum.CurrentNumber) + 1).ToString();
							curNum.CurrentNumber = strNextNumber;
							if (part1 != null && part1 != "") curNum.Part1 = part1;
							if (part2 != null && part2 != "") curNum.Part2 = part2;
							if (part3 != null && part3 != "") curNum.Part3 = part3;
							if (part4 != null && part4 != "") curNum.Part4 = part4;
							if (part5 != null && part5 != "") curNum.Part5 = part5;
							//if (part6 != null && part6 != "") curNum.Part6 = part6;
						}
						else
						{
							NumberSchema insNum = cwmContext.NumberSchemas.Add(new NumberSchema());
							insNum.NumberSchemaId = Guid.NewGuid();
							insNum.rowguid = Guid.NewGuid();
							insNum.ModuleId = moduleId;
							insNum.CurrentNumber = strNextNumber;
							insNum.Part1 = part1;
							if (part2 != null && part2 != "") insNum.Part2 = part2;
							if (part3 != null && part3 != "") insNum.Part3 = part3;
							if (part4 != null && part4 != "") insNum.Part4 = part4;
							if (part5 != null && part5 != "") insNum.Part5 = part5;
							//if (part6 != null && part6 != "") insNum.Part6 = part6;
						}
					}
					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save Permit NumberSchema for ModuleId '" + moduleId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to save Permit NumberSchema for ModuleId '" + moduleId.ToString() + "', DbContext was not available.");
					}

					while (strNextNumber.Length < Num)
					{
						strNextNumber = "0" + strNextNumber;
					}

					if (newBackSchema.Length == 0)
					{
						while (newFrontSchema.ToString().Substring(newFrontSchema.Length - 2, 1) == sepChar)
						{
							newFrontSchema.Remove(newFrontSchema.Length - 1, 1);
						}
						retval = newFrontSchema.ToString() + strNextNumber;
					}
					else
						retval = newFrontSchema.ToString() + strNextNumber + newBackSchema.ToString();
				}
				else
				{
					if (newBackSchema.Length == 0)
						retval = newFrontSchema.ToString().Remove(newFrontSchema.ToString().Length - 1, 1);
					else
						retval = newFrontSchema.ToString() + newBackSchema.ToString().Substring(0, newBackSchema.ToString().Length - 1);
				}
			}
			catch (Exception)
			{
				//HandleError(ex, "modCodepal", "GetNextPermitNumber")
			}
			return retval;
		}

		private string GetNumberType(string ActivityType)
		{
			string strType = "";
			int start = 0;
			try
			{

				if (ActivityType != "")
				{
					string newstart = "";
					while (start > -1)
					{
						strType += ActivityType.Substring(start, 1);
						start = ActivityType.IndexOf(" ", start + 1);
						if (start > -1)
							start = start + 1;
						try
						{
							newstart = ActivityType.Substring(start, 1);
						}
						catch (Exception)
						{
							start = -1;
						}
					}
				}
			}

			catch (Exception)
			{

				throw;
			}

			return strType;
		}

		private string GetInfoLineText(Guid inspectionId, Guid checkListId)
		{
			string retval = "";
			int m_QCount;
			int m_AQCount = 0;
			int m_OQCOunt = 0;
			int m_FQCount = 0;
			int m_NAQCount = 0;
			int m_NOQCount = 0;
			int m_VioCount = 0;
			string oSQL;
			Guid? agencyId = null;

			try
			{

				Inspection inspection = cwmContext.Inspections.FirstOrDefault(ins => ins.InspectionId == inspectionId);
				if (inspection != null)
				{
					agencyId = cwmContext.AgencyActivityTypes.FirstOrDefault(aat => aat.ActivityTypeId == inspection.ActivityTypeId).AgencyId ?? null;
				}
				m_QCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), CheckItemId)) AS Expr1 FROM CheckItems WHERE (CheckListId = '" + checkListId.ToString() + "') AND (Inactive = 0)").FirstOrDefault();

				if (agencyId != null)
				{
					oSQL = "SELECT COUNT(CONVERT(varchar(50), CheckItemValueId)) AS Expr1 FROM CheckItems CHKITM LEFT OUTER JOIN CheckItemValues CHKVAL ON CHKITM.CheckItemId = CHKVAL.CheckItemId LEFT OUTER JOIN Inspections INSP ON INSP.InspectionId = CHKVAL.InspectionId WHERE CHKVAL.InspectionId <> '" + inspectionId + "' AND INSP.AddressId = '" + inspection.AddressId.ToString() + "' AND INSP.InspectionTypeId = '" + inspection.InspectionTypeId.ToString() + "' AND INSP.Complete = 1 AND INSP.InspectionDate < '" + inspection.InspectionDate + "' ";

					if (inspection.ItemId != null && inspection.ItemId.ToString() != "")
					{
						oSQL += "AND INSP.ItemId='" + inspection.ItemId.ToString() + "' ";
					}

					oSQL += "AND CHKVAL.BooleanValue = CHKITM.FailValue AND CHKITM.CheckListId='" + checkListId + "' AND (CHKITM.Inactive=0 or CHKITM.Inactive IS NULL) AND CHKVAL.Corrected IS NULL ";

					m_OQCOunt = ((DbContext)cwmContext).Database.SqlQuery<int>(oSQL).FirstOrDefault();


					m_AQCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), CheckItemValueId)) AS Expr1 FROM CheckItemValues WHERE (InspectionId = '" + inspectionId + "') AND  (CheckItemId IN (SELECT CheckItemId FROM CheckItems AS CheckItems_3 WHERE (CheckListId = '" + checkListId + "')))").FirstOrDefault();

					m_FQCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), CheckItemValueId)) AS Expr1 FROM CheckItemValues WHERE (InspectionId = '" + inspectionId + "') AND (CheckItemId IN (SELECT CheckItemId FROM CheckItems AS CheckItems_1 WHERE (CheckListId = '" + checkListId + "') AND (CheckItemValues.BooleanValue = 0)))").FirstOrDefault();

					m_NAQCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), CheckItemValueId)) AS Expr1 FROM CheckItemValues WHERE (InspectionId = '" + inspectionId + "') AND (CheckItemId IN (SELECT CheckItemId FROM CheckItems AS CheckItems_1 WHERE (CheckListId = '" + checkListId + "') AND (CheckItemValues.BooleanValue = 2)))").FirstOrDefault();

					m_NOQCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), CheckItemValueId)) AS Expr1 FROM CheckItemValues WHERE (InspectionId = '" + inspectionId + "') AND (CheckItemId IN (SELECT CheckItemId FROM CheckItems AS CheckItems_1 WHERE (CheckListId = '" + checkListId + "') AND (CheckItemValues.BooleanValue = 3)))").FirstOrDefault();

					m_VioCount = ((DbContext)cwmContext).Database.SqlQuery<int>("SELECT COUNT(CONVERT(varchar(50), InspectionDetailId)) AS Expr1 FROM CheckItemValueInspectionDetails WHERE (CheckItemValueId IN (SELECT CheckItemValueId FROM CheckItemValues AS CheckItemValues_2 WHERE (InspectionId ='" + inspectionId + "') AND (CheckItemId IN (SELECT CheckItemId FROM CheckItems AS CheckItems_2 WHERE (CheckListId = '" + checkListId + "'))))) ").FirstOrDefault();
				}
				retval += "Q:" + m_QCount + " ";

				//AnsweredQuestionCount

				retval += "A:" + m_AQCount + " ";

				//'OutstandingQuestionCount

				retval += "O:" + m_OQCOunt + " ";

				//'FailedQuestionCount

				retval += "F:" + m_FQCount + " ";

				//'NAQuestionCount
				var setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNA" && set.AgencyId == agencyId).ValueField;
				if (setting != null && setting != "")
				{
					retval += "NA:" + m_NAQCount + " ";
				}

				//'NOQuestionCount
				setting = cwmContext.Settings.FirstOrDefault(set => set.PropertyField == "NoNO" && set.AgencyId == agencyId).ValueField;
				if (setting != null && setting != "")
				{
					retval += "NO:" + m_NOQCount + " ";
				}

				//'ViolationCount

				retval += "V:" + m_VioCount;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to create infoline '" + inspectionId + "'.", ex);
				//throw;
			}
			return retval;
		}

		public PrevCheckItem GetLastValues(CheckItemModel checkItem)
		{
			string oSQL = "";
			object[] parameters = { };
			DateTime? maxDate;
			Guid activityId = checkItem.ActivityId;
			Inspection inspection;
			PrevCheckItem retval = null;
			try
			{
				ICodepalWebModel resoContext = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
				inspection = resoContext.Inspections.First(i => i.InspectionId == activityId);

				oSQL = "(SELECT MAX(INSP.InspectionDate) " +
					"FROM CheckLists CHKLST JOIN CheckItems CHKITM ON CHKLST.CheckListId = CHKITM.CheckListId " +
					"JOIN CheckItemTypes ITMTYPE ON CHKITM.CheckItemTypeId = ITMTYPE.CheckItemTypeId " +
					"LEFT OUTER JOIN CheckItemValues CHKVAL ON CHKITM.CheckItemId = CHKVAL.CheckItemId AND CHKVAL.InspectionId<> '" + checkItem.ActivityId.ToString() + "'" +
					"LEFT OUTER JOIN Inspections INSP On INSP.InspectionId = CHKVAL.InspectionId " +
					"WHERE CHKLST.CheckListId = '" + checkItem.CheckListId.ToString() + "' " +
					"AND INSP.AddressId = '" + inspection.AddressId.ToString() + "'" +
					"AND(CHKVAL.InspectionId <> '" + checkItem.ActivityId.ToString() + "' AND CHKVAL.InspectionId IS NOT NULL) " +
					"AND(INSP.InspectionTypeId In(Select ActivityTypeId From CheckListActivityTypes Where CheckListId = '" + checkItem.CheckListId.ToString() + "')) ";
				if (inspection.ItemId != null && inspection.ItemId != Guid.Empty)
				{
					oSQL += "AND INSP.ItemId = '" + inspection.ItemId.ToString() + "'";
				}

				oSQL += "AND INSP.Complete = 1)";

				maxDate = ((DbContext)resoContext).Database.SqlQuery<DateTime?>(oSQL, parameters).FirstOrDefault();

				if (maxDate != null)
				{
					oSQL = "SELECT Top 1 TextValue, BooleanValue, CHKVAL.ResolutionText ";
					oSQL += "FROM dbo.CheckItems CHKITM ";
					oSQL += "LEFT OUTER JOIN dbo.CheckItemValues CHKVAL ON CHKITM.CheckItemId = CHKVAL.CheckItemId ";
					oSQL += "LEFT OUTER JOIN dbo.Inspections INSP On INSP.InspectionId = CHKVAL.InspectionId ";
					oSQL += "WHERE CHKITM.CheckItemId = '" + checkItem.CheckItemId.ToString() + "' ";
					oSQL += "AND INSP.AddressId = '" + inspection.AddressId.ToString() + "' ";
					oSQL += "AND CHKVAL.InspectionId <> '" + checkItem.ActivityId.ToString() + "' ";
					oSQL += "AND(INSP.InspectionTypeId In(Select ActivityTypeId From CheckListActivityTypes Where CheckListId = '" + checkItem.CheckListId.ToString() + "')) ";
					if (maxDate != null && maxDate.ToString() != "" && maxDate != DateTime.MinValue)
					{
						oSQL += "AND INSP.InspectionDate = '" + maxDate.ToString() + "' ";
					}
					else
					{
						oSQL += "AND INSP.InspectionDate IS NULL ";
					}

					if (inspection.ItemId != null && inspection.ItemId != Guid.Empty)
					{
						oSQL += "AND INSP.ItemId = '" + inspection.ItemId.ToString() + "' ";
					}
					oSQL += "AND INSP.Complete = 1 ";
					oSQL += "ORDER BY INSP.InspectionDate Desc";

					retval = ((DbContext)resoContext).Database.SqlQuery<PrevCheckItem>(oSQL, parameters).FirstOrDefault();
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to get last values CheckItem '" + checkItem.CheckItemId + "'.", ex);
				throw;
			}
			return retval;
		}

		private int[] CalculatViolationCounts()
		{
			int[] results = Array.Empty<int>();

			return results;
		}

	}
}

