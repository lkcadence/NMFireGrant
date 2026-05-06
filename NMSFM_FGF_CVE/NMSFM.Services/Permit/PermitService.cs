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
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NMSFM.Services.Permit
{
	public class PermitService : IPermitService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;
		private IFeeService feeService;

		public PermitService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		//public PermitService(ICodepalWebModel codepalWebModel)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
			feeService = new FeeService(cwmContext, logger);
		}

		public PermitService()
		{
			//var userConnection = System.Web.HttpContext.Current.Session["userConnection"].ToString();
			//cwmContext = new CodepalWebModel(userConnection);
			cwmContext = new CodepalWebModel();
			logger = new Logging.Logging();
			auditService = new AuditService(logger);
			feeService = new FeeService(cwmContext, logger);
		}
		//Task<IEnumerable<v_Permits>> GetPermitsAsync();
		public async Task<IEnumerable<v_Permits>> GetPermitsAsync()
		{
			IEnumerable<v_Permits> result;
			try
			{
				var permits = await cwmContext.v_Permits.ToListAsync();
				var permitTypeList = await cwmContext.PermitTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.PermitTypeId).ToListAsync();
				if (permits != null && permits.Count() > 0 & permitTypeList != null & permitTypeList.Count() > 0)
				{
					permits = permits.Where(a => permitTypeList.Contains(a.PermitTypeId == null ? Guid.Empty : a.PermitTypeId.Value)).ToList();
				}
				result = permits;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permit List.", ex);
				result = new List<v_Permits>();
			}
			return result;
		}
		//Task<v_Permits> GetPermitByIdAsync(Guid id);
		public async Task<v_Permits> GetPermitByIdAsync(Guid id)
		{
			v_Permits result = null;
			try
			{
				result = await cwmContext.v_Permits.SingleOrDefaultAsync(a => a.PermitId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the permit '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		//Task<IEnumerable<PermitType>> GetPermitTypeListAsync(Guid agencyId);
		public async Task<IEnumerable<PermitType>> GetPermitTypeListAsync(Guid? agencyId)
		{
			IEnumerable<PermitType> result;
			try
			{
				result = await cwmContext.PermitTypes.Where(a => !a.Inactive && a.WebViewable && (a.AgencyId == agencyId || a.AgencyId == null)).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permit Type List.", ex);
				result = new List<PermitType>();
			}
			return result;
		}

		//Task<IEnumerable<PermitStatu>> GetPermitStatusListAsync(Guid? agencyId);
		public async Task<IEnumerable<PermitStatu>> GetPermitStatusListAsync(Guid? agencyId)
		{
			IEnumerable<PermitStatu> result;
			try
			{
				result = await cwmContext.PermitStatus.Where(a => !a.Inactive && a.WebViewable && (a.AgencyId == agencyId || a.AgencyId == null)).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permit Type List.", ex);
				result = new List<PermitStatu>();
			}
			return result;
		}

		//Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByPermitIdAsync(Guid id, Guid agency);
		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByPermitIdAsync(Guid id, Guid? pTypeId, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			Guid PermitTypeId = pTypeId ?? Guid.Empty;
			var ModuleId = new Guid();
			try
			{
				v_Permits permit = null;
				permit = await cwmContext.v_Permits.SingleOrDefaultAsync(a => a.PermitId == id);
				if (permit != null)
				{
					PermitTypeId = (Guid)permit.PermitTypeId;
				}

				Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
				if (agency != null && agency != Guid.Empty)
				{
					AgencyId = agency;
				}
				Module module = null;
				module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Permit");
				ModuleId = module.ModuleId;

				var models = from cats in cwmContext.UserDefCategories
							 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = PermitTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
							 from usecat in subcat.DefaultIfEmpty()
							 where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == PermitTypeId) || (usecat.TypeId == PermitTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == PermitTypeId && cats.AllModuleTypes == true) || usecat.TypeId == PermitTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
							 join flds in cwmContext.UserDefFields on cats.UserDefCategoryId equals flds.UserDefCategoryId
							 join vals in cwmContext.UserDefValues on new { id = flds.UserDefFieldId, ad = id } equals new { id = vals.UserDefFieldId, ad = vals.RecordId } into subvals
							 from usevals in subvals.DefaultIfEmpty()
							 select new UserDefinedValue
							 {
								 Category = cats.Category,
								 CategoryId = cats.UserDefCategoryId,
								 FieldDescription = flds.FieldDesc,
								 FieldValue = usevals.UserDefValue1.Equals(null) ? String.Empty : usevals.UserDefValue1,
								 FieldOldValue = usevals.UserDefValue1.Equals(null) ? String.Empty : usevals.UserDefValue1,
								 ValueId = usevals.UserDefValueId.Equals(null) ? Guid.Empty : usevals.UserDefValueId,
								 FieldId = flds.UserDefFieldId,
								 FieldType = flds.UserDefTypeId,
								 SequenceNumber = cats.SeqNum.HasValue ? cats.SeqNum.Value : 0,
								 FieldSequenceNumber = flds.SeqNum.HasValue ? flds.SeqNum.Value : 0,
								 WebViewable = (cats.WebViewable.HasValue ? cats.WebViewable.Value : false) //&& flds.WebViewable,
							 };

				var resolutionResults = await models.ToListAsync();

				for (int i = 0; i < resolutionResults.Count(); i++)
				{
					resolutionResults[i].Resolutions = new List<Resolution>();
					Guid cbId = new Guid(cwmContext.UserDefTypes.Single(t => t.UserDefType1 == "check box").UserDefTypeId.ToString());
					Guid lstId = new Guid(cwmContext.UserDefTypes.Single(t => t.UserDefType1 == "list").UserDefTypeId.ToString());
					if (resolutionResults[i].FieldType == cbId) // Check Box
					{
						var fieldId = resolutionResults[i].FieldId;
						resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
						if (resolutionResults[i].Resolutions != null && resolutionResults[i].Resolutions.Count() > 0)
						{
							resolutionResults[i].boolValue = new List<bool>();
							for (int j = 0; j < resolutionResults[i].Resolutions.Count(); j++)
							{
								if (resolutionResults[i].FieldValue != String.Empty && resolutionResults[i].FieldValue.Length == resolutionResults[i].Resolutions.Count())
								{
									if (resolutionResults[i].FieldValue.ElementAt(j) == '1')
									{
										resolutionResults[i].boolValue.Add(true);
									}
									else
									{
										resolutionResults[i].boolValue.Add(false);
									}
								}
								else
								{
									resolutionResults[i].boolValue.Add(false);
								}
							}
						}
					}
					else if (resolutionResults[i].FieldType == lstId) // List
					{
						var fieldId = resolutionResults[i].FieldId;
						resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
						var linkedResolution = resolutionResults[i].Resolutions.Find(a => a.Resolution1 == resolutionResults[i].FieldValue);
						if (linkedResolution != null)
						{
							resolutionResults[i].ResolutionId = linkedResolution.ResolutionId;
						}
					}
				}
				results = resolutionResults;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving user defined values for address '" + id.ToString() + "'.", ex);
			}

			return results;
		}

		//Task SaveUserDefinedValuesAsync(List<UserDefValue> list);
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
							userDefinedValue = cwmContext.UserDefValues.Add(new UserDefValue());
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
								bool idCheck = (auditField.OldId ?? Guid.Empty) != (auditField.NewId ?? Guid.Empty);
								bool valCheck = (auditField.OldValue ?? String.Empty) != (auditField.NewValue ?? String.Empty);
								if (idCheck || valCheck)
								{
									auditFields.Add(auditField);
									await auditService.UpdateAudit(audit, auditFields);
								}
							}
							catch (Exception ex)
            {
                _ = ex;
								logger.Error("Unable to save the user defined value changes for address '" + list[i].RecordId.ToString() + "'.", ex);
								return;
							}
						}
						else
						{
							logger.Error("Unable to update the user defined values for address '" + list[i].RecordId.ToString() + "', DbContext was not available.");
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

		//Task<IEnumerable<v_FeesPermits>> GetPermitFeesByPermitIdAsync(Guid id);
		public async Task<IEnumerable<v_FeesPermits>> GetPermitFeesByPermitIdAsync(Guid id)
		{
			IEnumerable<v_FeesPermits> result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var permitFees = await cwmContext.v_FeesPermits.Where(p => p.PermitId == id).ToListAsync();

				result = permitFees;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Items List.", ex);
				result = new List<v_FeesPermits>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Permits>> GetIncludedPermitsAsync(Guid parentPermitId)
		{
			IEnumerable<v_Permits> result;
			try
			{
				var permits = await cwmContext.v_Permits.Where(p => p.ParentPermitId == parentPermitId).ToListAsync();
				var permitTypeList = await cwmContext.PermitTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.PermitTypeId).ToListAsync();
				if (permits != null && permits.Count() > 0 & permitTypeList != null & permitTypeList.Count() > 0)
				{
					permits = permits.Where(a => permitTypeList.Contains(a.PermitTypeId == null ? Guid.Empty : a.PermitTypeId.Value)).ToList();
				}
				result = permits;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permit List.", ex);
				result = new List<v_Permits>();
			}
			return result;
		}

		public async Task<string> GetPermitTypeLegalTextByPermitTypeIdAsync(Guid id)
		{
			string result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var permitType = await cwmContext.PermitTypes.SingleAsync(p => p.PermitTypeId == id);

				string legalDesc = permitType.LegalDesc;

				result = legalDesc;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Items List.", ex);
				result = "";
			}
			return result;
		}

		public async Task<string> GetAgencyName(Guid id)
		{
			string result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var agency = await cwmContext.Agencies.SingleAsync(a => a.AgencyId == id);

				if (agency != null)
				{
					if (agency.AgencySubName != null && agency.AgencySubName != "")
					{
						result = agency.AgencyName + " " + agency.AgencySubName;
					}
					else
					{
						result = agency.AgencyName;
					}
				}
				else
				{
					result = "";
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Items List.", ex);
				result = "";
			}
			return result;
		}

		public async Task<string> GetPermitTypeByIdAsync(Guid id)
		{
			string result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var permitType = await cwmContext.PermitTypes.SingleAsync(pt => pt.PermitTypeId == id);

				if (permitType != null)
				{

					result = permitType.PermitType1;
				}
				else
				{
					result = "";
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Items List.", ex);
				result = "";
			}
			return result;
		}

		public async Task<bool> CreatePermitAsync(DetailedPermit model)
		{
			try
			{

				if (model != null)
				{
					var permit = await cwmContext.Permits.SingleOrDefaultAsync(a => a.PermitId == model.PermitId);

					if (permit != null)
					{
						return false;
					}
					var audit = new AuditModel { TableName = "Permits", RecordId = model.PermitId, AuditAction = "RECORD CREATED", Description = "" };
					var auditFields = new List<AuditFieldModel>();

					permit = cwmContext.Permits.Add(new NMSFM.Data.Permit());
					var permitType = cwmContext.PermitTypes.First(pt => pt.PermitTypeId == model.PermitTypeId);
					permit.rowguid = Guid.NewGuid();
					permit.DateInserted = DateTime.Now;
					permit.DateUpdated = permit.DateInserted;
					permit.PermitId = model.PermitId;
					permit.PermitNumber = await GetNextPermitNumber(GetNumberType(model.PermitType), model.BeginDate);

					//permit.AddressId = model.AddressId;
					permit.Comment = model.Comment;
					permit.PermitTypeId = model.PermitTypeId;

					//permit.IssuedToPartyId = model.IssuedToPartyId;
					//permit.IssuedToRoleTypeId = cwmContext.PartyRoles.First(p => p.PartyId == model.IssuedToPartyId).RoleTypeId;
					//permit.Complete = false;				
					//permit.SubmittalDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
					//permit.PermitStatusId = permitType.DefaultStatusId;

					//AddressId				
					auditFields.Add(new AuditFieldModel { ControlName = "lblAddress", FieldDesc = "AddressId", OldId = null, OldValue = null, NewId = model.AddressId, NewValue = model.AddressDisplay.FullAddress });
					permit.AddressId = model.AddressId;
					//IssuedToPartyId
					var newPartyName = (await cwmContext.Parties.SingleOrDefaultAsync(p => p.PartyID == model.IssuedToPartyId)).PartyName;
					auditFields.Add(new AuditFieldModel { ControlName = "cboOccupantParty", FieldDesc = "IssuedToPartyId", OldId = null, OldValue = null, NewId = model.IssuedToPartyId, NewValue = newPartyName });
					permit.IssuedToPartyId = model.IssuedToPartyId;

					//IssuedToRoleTypeId				
					var newPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == model.IssuedToRoleTypeId)).RoleType1;
					auditFields.Add(new AuditFieldModel { ControlName = "cboIssuedToRoleType", FieldDesc = "IssuedToRoleTypeId", OldId = null, OldValue = null, NewId = model.IssuedToRoleTypeId, NewValue = newPartyRole });
					permit.IssuedToRoleTypeId = model.IssuedToRoleTypeId;

					//PermitTypeId
					var newPermitType = (await cwmContext.PermitTypes.SingleOrDefaultAsync(p => p.PermitTypeId == model.PermitTypeId)).PermitType1;
					auditFields.Add(new AuditFieldModel { ControlName = "cboType", FieldDesc = "PermitTypeId", OldId = null, OldValue = null, NewId = model.PermitTypeId, NewValue = newPermitType });
					permit.PermitTypeId = model.PermitTypeId;

					if (model.ContactId != null && model.ContactId != Guid.Empty)
					{
						//ContactId			
						newPartyName = "";
						newPartyName = (await cwmContext.Parties.SingleOrDefaultAsync(p => p.PartyID == model.ContactId)).PartyName;
						auditFields.Add(new AuditFieldModel { ControlName = "cboContact", FieldDesc = "ContactId", OldId = null, OldValue = null, NewId = model.ContactId, NewValue = newPartyName });
						permit.ContactId = model.ContactId;
					}
					if (model.ContactRoleTypeId != null && model.ContactRoleTypeId != Guid.Empty)
					{
						//ContactRoleTypeId				
						newPartyRole = "";
						newPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == model.ContactRoleTypeId)).RoleType1;
						auditFields.Add(new AuditFieldModel { ControlName = "cboContactRoleType", FieldDesc = "ContactRoleTypeId", OldId = null, OldValue = null, NewId = model.ContactRoleTypeId, NewValue = newPartyRole });
						permit.ContactRoleTypeId = model.ContactRoleTypeId;
					}



					var thisDate = "";
					if (model.SubmittalDate != null)
					{
						thisDate = Convert.ToDateTime(model.SubmittalDate).ToShortDateString();

						auditFields.Add(new AuditFieldModel { ControlName = "dtSubmittal", FieldDesc = "SubmittalDate", OldId = null, OldValue = null, NewId = null, NewValue = thisDate });
						permit.SubmittalDate = model.SubmittalDate;
					}
					//BeginDate
					thisDate = "";
					if (model.BeginDate != null)
					{
						thisDate = Convert.ToDateTime(model.BeginDate).ToShortDateString();

						auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "BeginDate", OldId = null, OldValue = null, NewId = null, NewValue = thisDate });
						permit.BeginDate = model.BeginDate;
					}

					thisDate = "";
					if (model.EndDate != null)
					{
						thisDate = Convert.ToDateTime(model.EndDate).ToShortDateString();

						auditFields.Add(new AuditFieldModel { ControlName = "dtEndDate", FieldDesc = "EndDate", OldId = null, OldValue = null, NewId = null, NewValue = thisDate });
						permit.EndDate = model.EndDate;

						//PermitStatusId				
						var newPermitStatus = (await cwmContext.PermitStatus.SingleOrDefaultAsync(p => p.PermitStatusId == model.PermitStatusId)).PermitStatus;
						auditFields.Add(new AuditFieldModel { ControlName = "cboStatus", FieldDesc = "PermitStatusId", OldId = null, OldValue = null, NewId = model.PermitStatusId, NewValue = newPermitStatus });
						permit.PermitStatusId = model.PermitStatusId;
					}
					if (model.ItemId != null && model.ItemId != Guid.Empty)
					{
						//ItemId								
						auditFields.Add(new AuditFieldModel { ControlName = "txtItem", FieldDesc = "ItemId", OldId = null, OldValue = null, NewId = model.ItemId, NewValue = model.Item });
						permit.ItemId = model.ItemId;
					}
					if (model.IssuingOfficerId != null && model.IssuingOfficerId != Guid.Empty)
					{
						//IssuingOfficerId								
						var newinspector = (await cwmContext.Inspectors.SingleOrDefaultAsync(p => p.InspectorId == model.IssuingOfficerId)).InspectorName;
						auditFields.Add(new AuditFieldModel { ControlName = "cboIssuingOff", FieldDesc = "IssuingOfficerId", OldId = null, OldValue = null, NewId = model.IssuingOfficerId, NewValue = newinspector });
						permit.IssuingOfficerId = model.IssuingOfficerId;
					}
					if (model.Comment != null && model.Comment != "")
					{
						//Comment				
						auditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = null, NewId = null, NewValue = model.Comment });
						permit.Comment = model.Comment;
					}

					//Complete
					auditFields.Add(new AuditFieldModel { ControlName = "chkComplete", FieldDesc = "Complete", OldId = null, OldValue = null, NewId = null, NewValue = model.Complete.ToString() });
					permit.Complete = model.Complete;


					//StopAlerts
					auditFields.Add(new AuditFieldModel { ControlName = "chkStopAlerts", FieldDesc = "StopAlerts", OldId = null, OldValue = null, NewId = null, NewValue = model.StopAlerts.ToString() });
					permit.StopAlerts = model.StopAlerts;

					if (model.AddressDisplay.OccupancyTypeId != null && model.AddressDisplay.OccupancyTypeId != Guid.Empty)
					{
						//OccupancyTypeId
						var occType = (await cwmContext.OccupancyTypes.SingleOrDefaultAsync(o => o.OccupancyTypeId == model.AddressDisplay.OccupancyTypeId)).OccupancyType1;
						auditFields.Add(new AuditFieldModel { ControlName = "cboOccType", FieldDesc = "OccupancyTypeId", OldId = null, OldValue = null, NewId = model.AddressDisplay.OccupancyTypeId, NewValue = occType });
						permit.OccupancyTypeId = model.AddressDisplay.OccupancyTypeId;
					}
					if (model.AddressDisplay.PropertyUseTypeId != null && model.AddressDisplay.PropertyUseTypeId != Guid.Empty)
					{
						//PropertyUseTypeId
						var propType = (await cwmContext.PropertyUseTypes.SingleAsync(o => o.PropertyUseTypeId == model.AddressDisplay.PropertyUseTypeId)).PropertyUseType1;
						auditFields.Add(new AuditFieldModel { ControlName = "cboPropUse", FieldDesc = "PropertyUseTypeId", OldId = null, OldValue = null, NewId = model.AddressDisplay.PropertyUseTypeId, NewValue = propType });
						permit.PropertyUseTypeId = model.AddressDisplay.PropertyUseTypeId;
					}
					permit.FromWeb = true;

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();

							CreateDefaultFees(model.PermitId);

							await ((DbContext)cwmContext).SaveChangesAsync();

							if (auditFields.Count() > 0)
							{
								await auditService.UpdateAudit(audit, auditFields);
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to create permit '" + model.PermitId.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to create permit '" + model.PermitId.ToString() + "', DbContext was not available.");
						return false;
					}

					return true;
				}

			}
			catch (Exception)
			{

				//throw;
			}
			return false;
		}

		public async Task<bool> UpdatePermitAsync(DetailedPermit model)
		{

			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  
				var permit = await cwmContext.Permits.SingleOrDefaultAsync(a => a.PermitId == model.PermitId);
				var permitVals = await cwmContext.v_Permits.SingleOrDefaultAsync(a => a.PermitId == model.PermitId);
				if (permit == null)
				{
					logger.Error("Unable to update permit '" + model.PermitId.ToString() + "'.  The permit could not be located in the database.");
					return false;
				}

				var audit = new AuditModel { TableName = "Permits", RecordId = model.PermitId, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				//AddressId
				if (permit.AddressId != model.AddressId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "lblAddress", FieldDesc = "AddressId", OldId = permit.AddressId, OldValue = (await cwmContext.v_AddressesReport.SingleOrDefaultAsync(a => a.AddressId == permit.AddressId)).FullAddress, NewId = model.AddressId, NewValue = model.AddressDisplay.FullAddress });
					permit.AddressId = model.AddressId;
				}
				//IssuedToPartyId
				if (permit.IssuedToPartyId != model.IssuedToPartyId)
				{
					var oldPartyName = permitVals.PartyName;
					var newPartyName = (await cwmContext.Parties.SingleOrDefaultAsync(p => p.PartyID == model.IssuedToPartyId)).PartyName;
					auditFields.Add(new AuditFieldModel { ControlName = "cboOccupantParty", FieldDesc = "IssuedToPartyId", OldId = permit.IssuedToPartyId, OldValue = oldPartyName, NewId = model.IssuedToPartyId, NewValue = newPartyName });
					permit.IssuedToPartyId = model.IssuedToPartyId;
				}
				//IssuedToRoleTypeId
				if (permit.IssuedToRoleTypeId != model.IssuedToRoleTypeId)
				{
					var oldPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == permit.IssuedToRoleTypeId)).RoleType1;
					var newPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == model.IssuedToRoleTypeId)).RoleType1;
					auditFields.Add(new AuditFieldModel { ControlName = "cboIssuedToRoleType", FieldDesc = "IssuedToRoleTypeId", OldId = permit.IssuedToRoleTypeId, OldValue = oldPartyRole, NewId = model.IssuedToRoleTypeId, NewValue = newPartyRole });
					permit.IssuedToRoleTypeId = model.IssuedToRoleTypeId;
				}
				//ContactId					
				if (permit.ContactId != model.ContactId)
				{
					var oldPartyName = "";
					var newPartyName = "";
					if (permit.ContactId != null)
					{
						oldPartyName = (await cwmContext.Parties.SingleOrDefaultAsync(p => p.PartyID == permit.ContactId)).PartyName;
					}
					if (permit.ContactId != null)
					{
						newPartyName = (await cwmContext.Parties.SingleOrDefaultAsync(p => p.PartyID == model.ContactId)).PartyName;
					}

					auditFields.Add(new AuditFieldModel { ControlName = "cboContact", FieldDesc = "ContactId", OldId = permit.ContactId, OldValue = oldPartyName, NewId = model.ContactId, NewValue = newPartyName });
					permit.ContactId = model.ContactId;
				}
				//ContactRoleTypeId
				if (permit.ContactRoleTypeId != model.ContactRoleTypeId)
				{
					var oldPartyRole = "";
					var newPartyRole = "";
					if (permit.ContactRoleTypeId != null)
					{
						oldPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == permit.ContactRoleTypeId)).RoleType1;
					}
					if (model.ContactRoleTypeId != null)
					{
						newPartyRole = (await cwmContext.RoleTypes.SingleOrDefaultAsync(p => p.RoleTypeId == model.ContactRoleTypeId)).RoleType1;
					}

					auditFields.Add(new AuditFieldModel { ControlName = "cboContactRoleType", FieldDesc = "ContactRoleTypeId", OldId = permit.ContactRoleTypeId, OldValue = oldPartyRole, NewId = model.ContactRoleTypeId, NewValue = newPartyRole });
					permit.ContactRoleTypeId = model.ContactRoleTypeId;
				}
				//PermitTypeId
				if (permit.PermitTypeId != model.PermitTypeId)
				{
					var oldPermitType = permitVals.PermitType;
					var newPermitType = (await cwmContext.PermitTypes.SingleOrDefaultAsync(p => p.PermitTypeId == model.PermitTypeId)).PermitType1;
					auditFields.Add(new AuditFieldModel { ControlName = "cboType", FieldDesc = "PermitTypeId", OldId = permit.PermitTypeId, OldValue = oldPermitType, NewId = model.PermitTypeId, NewValue = newPermitType });
					permit.PermitTypeId = model.PermitTypeId;
				}
				//If dtSubmittal.Text <> "" Then
				//	 SubmittalDate = '" & GetDatabaseDateString(dtSubmittal.Value, DateFormat.ShortDate) & "'"
				//Else
				//	 SubmittalDate = NULL"
				//End If
				if ((permit.SubmittalDate != model.SubmittalDate))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtSubmittal", FieldDesc = "SubmittalDate", OldId = null, OldValue = permit.SubmittalDate.ToString(), NewId = null, NewValue = model.SubmittalDate.ToString() });
					permit.SubmittalDate = model.SubmittalDate;
				}
				//BeginDate
				if ((permit.BeginDate != model.BeginDate))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "BeginDate", OldId = null, OldValue = permit.BeginDate.ToString(), NewId = null, NewValue = model.BeginDate.ToString() });
					permit.BeginDate = model.BeginDate;
				}
				//If dtEndDate.Text <> "" Then
				//	EndDate = '" & GetDatabaseDateString(dtEndDate.Value, DateFormat.ShortDate) & "'"
				//Else
				//	EndDate = NULL"
				//End If
				if ((permit.EndDate != model.EndDate))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtEndDate", FieldDesc = "EndDate", OldId = null, OldValue = permit.EndDate.ToString(), NewId = null, NewValue = model.EndDate.ToString() });
					permit.EndDate = model.EndDate;
				}
				//PermitStatusId
				if (permit.PermitStatusId != model.PermitStatusId)
				{
					var oldPermitStatus = permitVals.PermitStatus;
					var newPermitStatus = "";
					if (model.PermitStatusId != null)
					{
						newPermitStatus = (await cwmContext.PermitStatus.SingleOrDefaultAsync(p => p.PermitStatusId == model.PermitStatusId)).PermitStatus;
					}
					auditFields.Add(new AuditFieldModel { ControlName = "cboStatus", FieldDesc = "PermitStatusId", OldId = permit.PermitStatusId, OldValue = oldPermitStatus, NewId = model.PermitStatusId, NewValue = newPermitStatus });
					permit.PermitStatusId = model.PermitStatusId;
				}
				//ItemId
				if (permit.ItemId != model.ItemId)
				{
					var oldItemDesc = "";
					var newItemDesc = model.Item;
					if (permit.ItemId != null)
					{
						oldItemDesc = (await cwmContext.Items.SingleOrDefaultAsync(p => p.ItemId == permit.ItemId)).Description;
					}

					auditFields.Add(new AuditFieldModel { ControlName = "txtItem", FieldDesc = "ItemId", OldId = permit.ItemId, OldValue = oldItemDesc, NewId = model.ItemId, NewValue = newItemDesc });
					permit.ItemId = model.ItemId;
				}

				//IssuingOfficerId
				if (permit.IssuingOfficerId != model.IssuingOfficerId)
				{
					var oldInspector = "";
					var newinspector = "";
					if (permit.IssuingOfficerId != null)
					{
						oldInspector = (await cwmContext.Inspectors.SingleOrDefaultAsync(p => p.InspectorId == permit.IssuingOfficerId)).InspectorName;
					}
					if (model.IssuingOfficerId != null)
					{
						newinspector = (await cwmContext.Inspectors.SingleOrDefaultAsync(p => p.InspectorId == model.IssuingOfficerId)).InspectorName;
					}
					auditFields.Add(new AuditFieldModel { ControlName = "cboIssuingOff", FieldDesc = "IssuingOfficerId", OldId = permit.IssuingOfficerId, OldValue = oldInspector, NewId = model.IssuingOfficerId, NewValue = newinspector });
					permit.IssuingOfficerId = model.IssuingOfficerId;
				}
				//Comment
				if ((permit.Comment != model.Comment))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = permit.Comment, NewId = null, NewValue = model.Comment });
					permit.Comment = model.Comment;
				}
				//Complete
				if ((permit.Complete != model.Complete))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "chkComplete", FieldDesc = "Complete", OldId = null, OldValue = permit.Complete.ToString(), NewId = null, NewValue = model.Complete.ToString() });
					permit.Complete = model.Complete;
				}
				//StopAlerts
				if ((permit.StopAlerts != model.StopAlerts))
				{
					auditFields.Add(new AuditFieldModel { ControlName = "chkStopAlerts", FieldDesc = "StopAlerts", OldId = null, OldValue = permit.StopAlerts.ToString(), NewId = null, NewValue = model.StopAlerts.ToString() });
					permit.StopAlerts = model.StopAlerts;
				}
				//RecordId - Not needed at this time This used to hold the Itemid befor it got it's own field.
				//DateUpdated
				permit.DateUpdated = DateTime.Now;
				/*
				Activity Listing-
				If Not utxtPropConst.Tag Is Nothing AndAlso utxtPropConst.Tag.ToString <> "" Then
					PropConst=" & CheckStringNull(utxtPropConst.Tag.ToString)
				Else
					PropConst=" & CheckStringNull(utxtPropConst.Text)
				End If
				OwnerId
				ContractorId
				-Activity Listing

				If OccupancyTypeId Is Nothing AndAlso Not gbSuperAdmin Then
					 OccupancyTypeId=" & CheckStringNull(GetAddressOccupancyTypeId(AddressId))
				ElseIf Not OccupancyTypeId Is Nothing Then
					 OccupancyTypeId=" & CheckStringNull(OccupancyTypeId)
				Else
					 OccupancyTypeId=NULL "
				End If
				If PropertyUseTypeId Is Nothing AndAlso Not gbSuperAdmin Then
					 PropertyUseTypeId=" & CheckStringNull(GetAddressPropertyUseTypeId(AddressId))
				ElseIf Not PropertyUseTypeId Is Nothing Then
					 PropertyUseTypeId=" & CheckStringNull(PropertyUseTypeId)
				Else
					 PropertyUseTypeId=NULL "
				End If
				If (TestSetting("CPOnlinePurchased") AndAlso CBool(cSettings("CPOnlinePurchased"))) Then
					 ApprovalStep=" & CheckStringNull(GetApprovalStepId("Approved"))
				End If
				*/
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();


						if (auditFields.Count() > 0)
						{
							await auditService.UpdateAudit(audit, auditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create permit '" + model.PermitId.ToString() + "'.", ex);
						return false;
					}
				}
				else
				{
					logger.Error("Unable to create permit '" + model.PermitId.ToString() + "', DbContext was not available.");
					return false;
				}
				return true;
			}
			return false;
		}

		public string GetNextPermitNumber()
		{

			return "OnlinePermit";
		}

		/// <summary>
		/// This function gets the Next Permit number.
		/// </summary>
		/// <param name="strType">       (Optional) The string value of the Permit Type.</param>
		/// <param name="PermitDate">    (Optional) The Permit Date.</param>
		/// <returns>                    Returns the next Permit Number.</returns>
		/// <remarks></remarks>
		public async Task<string> GetNextPermitNumber(string strType, DateTime? permitDate)
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

			string[] TableArray = new string[4];

			Guid? agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];

			Guid currentUserId = new Guid(HttpContext.Current.Session["CodepalUserId"].ToString());

			string loginCode = (await cwmContext.Inspectors.SingleOrDefaultAsync(i => i.InspectorId == currentUserId)).Code;

			string sepChar = await systemService.GetCodepalSetting("NumberSchemaPeSep", agencyId);

			Guid moduleId = (await cwmContext.Modules.FirstAsync(m => m.ModuleDesc == "Permit" && m.AgencyId == agencyId)).ModuleId;


			try
			{
				if (permitDate == null)
				{
					permitDate = DateTime.Now;
				}
				if (strType == null || strType == "")
				{
					strType = "P";
				}
				//if(sepChar == "")
				//{
				//	sepChar = "-";
				//}

				Schema = await systemService.GetCodepalSetting("NumberSchemaPermit", agencyId);
				if (Schema == "")
				{
					Schema = "6P*|1*|3*|74";
				}
				SchemaSub = Schema.Split('|');

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
							break;
						case "3":
							if (OneUp)
								newBackSchema.Append(permitDate.Value.ToString("yy"));
							else
								newFrontSchema.Append(permitDate.Value.ToString("yy"));

							if (SchemaSub[intI].IndexOf('*') >= 0)
								TableArray[intI] = permitDate.Value.ToString("yy");
							break;
						case "4":
							if (OneUp)
								newBackSchema.Append(permitDate.Value.ToString("yyyy"));
							else
								newFrontSchema.Append(permitDate.Value.ToString("yyyy"));

							if (SchemaSub[intI].IndexOf('*') >= 0)
								TableArray[intI] = permitDate.Value.ToString("yyyy");
							break;
						case "5":
							if (OneUp)
								newBackSchema.Append(permitDate.Value.ToString("MM"));
							else
								newFrontSchema.Append(permitDate.Value.ToString("MM"));

							if (SchemaSub[intI].IndexOf('*') >= 0)
								TableArray[intI] = permitDate.Value.ToString("MM");
							break;
						case "6":
							if (OneUp)
								newBackSchema.Append(SchemaSub[intI].Replace("*", "").Remove(0, 1));
							else
								newFrontSchema.Append(SchemaSub[intI].Replace("*", "").Remove(0, 1));

							if (SchemaSub[intI].IndexOf('*') >= 0)
								TableArray[intI] = SchemaSub[intI].Replace("*", "").Remove(0, 1);
							break;
						case "7":
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

				while (newBackSchema.ToString().Substring(newBackSchema.Length - 1, 1) == sepChar && sepChar.Length > 0)
				{
					newBackSchema.Remove(newBackSchema.Length - 1, 1);

				}
				while (newBackSchema.ToString().Substring(0, 1) == sepChar && sepChar.Length > 0)
				{
					newBackSchema.Remove(0, 1);
				}
				if (newBackSchema.ToString().Length > 0)
					newBackSchema.Insert(0, sepChar, 1);

				if (Num >= 2)
				{
					for (int intj = 2; intj < Num; intj++)
					{
						strNextNumber += "0";
					}
					strNextNumber += "1";
				}
				else
					strNextNumber = "0001";

				if (OneUp)
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
						curNum.Part1 = part1;
						if (part2 != null && part2 != "") curNum.Part2 = part2;
						if (part3 != null && part3 != "") curNum.Part3 = part3;
						if (part4 != null && part4 != "") curNum.Part4 = part4;
						if (part5 != null && part5 != "") curNum.Part5 = part5;
						//if (part6 != null && part6 != "") curNum.Part6 = part6;
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
						retval = newFrontSchema.ToString() + strNextNumber;
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

		private string GetNumberType(string permitType)
		{
			string strType = "";
			int start = 1;
			try
			{

				if (permitType != "")
				{
					string newstart = "";
					while (start > 0)
					{
						strType += permitType.Substring(start + 1, 1);
						start = permitType.IndexOf(" ", start + 2);
						if (start > -1)
							start = start + 2;
						try
						{
							newstart = permitType.Substring(start + 1, 1);
						}
						catch (Exception)
						{
							start = 0;
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

		private void CreateDefaultFees(Guid permitId)
		{
			var permit = cwmContext.v_Permits.Single(a => a.PermitId == permitId);
			Guid currentDefaultFeeId;

			var defaultFees = feeService.GetDefaultFees(permit.PermitTypeId, false, "");

			foreach (DetailedDefaultFee oFee in defaultFees)
			{
				if (oFee.FeeAmount != null && oFee.FeeAmount != "")
				{
					currentDefaultFeeId = feeService.DefaultRegFee(permitId, oFee.FeeAmount, oFee.FeeTypeId, permit.BeginDate ?? DateTime.Now, permit.IssuedToPartyId);
				}
				else
				{
					if (oFee.FeeTypeId != null)
					{
						if (oFee.FeeSchedId != null && oFee.FeeSchedId != Guid.Empty)
						{
							currentDefaultFeeId = feeService.DefaultRateFee(permitId, oFee.FeeSchedId, oFee.FeeTypeId, permit.BeginDate ?? DateTime.Now, permit.IssuedToPartyId);
						}
						else
						{
							if (oFee.TotalPercent)
							{
								currentDefaultFeeId = feeService.DefaultPOTFee(permitId, oFee.FeeTypeId, permit.BeginDate ?? DateTime.Now, permit.IssuedToPartyId);
							}
							else
							{
								currentDefaultFeeId = feeService.DefaultRRFee(permitId, oFee.FeeTypeId, permit.BeginDate ?? DateTime.Now, permit.IssuedToPartyId);
							}
						}
					}
				}

			}
		}

		//Task<PermitSetting> GetPermitSettingAsync(Guid permitTypeId);
		public async Task<PermitSetting> GetPermitSettingAsync(Guid permitTypeId)
		{
			PermitSetting result = null;
			try
			{
				result = await cwmContext.PermitSettings.SingleOrDefaultAsync(a => a.PermitTypeId == permitTypeId);
				if (result == null)
				{
					result = await cwmContext.PermitSettings.SingleOrDefaultAsync(a => a.PermitTypeId == permitTypeId);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permit Settings for Permit Id: " + permitTypeId + ".", ex);
			}
			return result;
		}
	}
}

