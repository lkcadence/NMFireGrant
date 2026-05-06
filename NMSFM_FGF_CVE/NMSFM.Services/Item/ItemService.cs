//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using NMSFM.Data;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NMSFM.Services.Models;
using NMSFM.Services.Audit;
using log4net;
using NMSFM.Services.Logging;
using System.Security.Cryptography;
using NMSFM.ViewModels;
using AutoMapper;

namespace NMSFM.Services.Item
{
	public class ItemService: IItemService 
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;


		//public PermitService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		public ItemService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
		}

		public async Task<IEnumerable<v_Items>> GetItemsAsync()
		{
			IEnumerable<v_Items> result;
			try
			{
				var items = await cwmContext.v_Items.ToListAsync();
				var itemTypeList = await cwmContext.ItemTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.ItemTypeId).ToListAsync();
				if (items != null && items.Count() > 0 & itemTypeList != null & itemTypeList.Count() > 0)
				{
					items = items.Where(a => itemTypeList.Contains(a.ItemTypeId == null ? Guid.Empty : a.ItemTypeId.Value)).ToList();
				}
				result = items;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item List.", ex);
				result = new List<v_Items>();
			}
			return result;
		}


		public async Task<v_Items> GetItemAsync(Guid? itemId)
		{
			v_Items result;
			try
			{
				result = await cwmContext.v_Items.Where(i=>i.ItemId==itemId).FirstOrDefaultAsync();				
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item List.", ex);
				result = new v_Items();
			}
			return result;
		}

		//Task<List<InspectionType>> GetItemTypeAsync();
		public async Task<List<ItemType>> GetItemTypesAsync()
		{
			List<ItemType> result;
			try
			{
				var itemTypeList = await cwmContext.ItemTypes.Where(a => !a.Inactive).ToListAsync();
				result = itemTypeList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item Types list.", ex);
				result = new List<ItemType>();
			}
			return result;
		}

		//Task<List<v_Items>> GetChildItemsByIdAsync(Guid itemId);
		public async Task<List<v_Items>> GetChildItemsByIdAsync(Guid itemId)
		{
			var results = new List<v_Items>();
			try
			{
				results = await cwmContext.v_Items.Where(a => a.ItemCategoryId == itemId && a.Inactive == false).ToListAsync() ?? new List<v_Items>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Child Items for Item Id: " + itemId + ".", ex);
			}
			return results;
		}


		//Task<IEnumerable<ItemsStatu>> GetItemStatusListAsync(Guid agencyId);
		public async Task<IEnumerable<ItemsStatu>> GetItemStatusListAsync(Guid agencyId)
		{
			IEnumerable<ItemsStatu> result;
			try
			{
				result = await cwmContext.ItemsStatus.Where(a => !a.Inactive && a.WebViewable).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item Status List.", ex);
				result = new List<ItemsStatu>();
			}
			return result;
		}

		//Task<IEnumerable<v_InventoryItems>> GetInvItemByIdAsync()
		public async Task<List<v_InventoryItems>> GetInvItemByIdAsync(Guid itemId)
		{
			//IEnumerable<v_InventoryItems> result;
			var result = new List<v_InventoryItems>();
			try
			{
				var invItemId = await cwmContext.v_Items.Where(a => a.ItemId == itemId && a.Inactive == false).Select(a => a.InvItemId).ToListAsync();
				//var invItems = await cwmContext.v_InventoryItems.Where(p => (p.InvItemId = invItemId)).Select(p => p.InventoryItem).ToListAsync();
				//var roleTypeList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
				//if (addressParties != null && addressParties.Count() > 0 && roleTypeList != null && roleTypeList.Count() > 0)
				//{
				//    addressParties = addressParties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList();
				//}
				//result = invItemId;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party list.", ex);
				result = new List<v_InventoryItems>();
			}
			return result;
		}

		public async Task<IEnumerable<v_InventoryItems>> GetInventoryItemListAsync(Guid itemId)
		{
			IEnumerable<v_InventoryItems> result = null;
			try
			{
				var item = cwmContext.Items.SingleOrDefault(a => a.ItemId == itemId);
				var inventoryItemId = item == null ? Guid.Empty : item.InvItemId;
				var inventoryItem = cwmContext.InventoryItems.SingleOrDefault(a => a.InvItemId == inventoryItemId);
				var inventoryItemTypeId = inventoryItem == null ? Guid.Empty : inventoryItem.InvItemTypeId;
				result = await cwmContext.v_InventoryItems.Where(a => a.InvItemTypeId == inventoryItemTypeId && a.Inactive == false && a.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Inventory Item list.", ex);
			}
			return result;
		}
		//Task<v_Activities> GetActivitiesByItemIdAsync(Guid id);
		public async Task<v_Activities> GetActivitiesByItemIdAsync(Guid id)
		{
			v_Activities result = null;
			try
			{
				result = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.ItemId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activity '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		//Task<IEnumerable<v_Permits>> GetPermitsByItemIdAsync(Guid id)
		public async Task<IEnumerable<v_Permits>> GetPermitsByItemIdAsync(Guid id)
		{
			IEnumerable<v_Permits> result;
			try
			{
				result = await cwmContext.v_Permits.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permits List.", ex);
				result = new List<v_Permits>();
			}
			return result;
		}
		
		//Task<IEnumerable<v_Files>> GetFilesByItemIdAsync(Guid id);
		public async Task<IEnumerable<v_Files>> GetFilesByItemIdAsync(Guid id)
		{
			IEnumerable<v_Files> result;
			try
			{
				result = await cwmContext.v_Files.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the File List.", ex);
				result = new List<v_Files>();
			}
			return result;
		}
		
		//Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id);
		public async Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id)
		{
			IEnumerable<Data.Note> result;
			try
			{
				result = await cwmContext.Notes.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item Notes List.", ex);
				result = new List<Data.Note>();
			}
			return result;
		}



		//Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid agency);
		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			Guid ItemTypeId = pTypeId;
			var ModuleId = new Guid();
			try
			{
				v_Items item = null;
				item = await cwmContext.v_Items.SingleOrDefaultAsync(a => a.ItemId == id);
				if (item != null)
				{
					ItemTypeId = (Guid)item.ItemTypeId;
				}

				Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
				if (agency != null && agency != Guid.Empty)
				{
					AgencyId = agency;
				}
				Module module = null;
				module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Item");
				ModuleId = module.ModuleId;

				var models = from cats in cwmContext.UserDefCategories
							 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = ItemTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
							 from usecat in subcat.DefaultIfEmpty()
							 where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ItemTypeId) || (usecat.TypeId == ItemTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == ItemTypeId && cats.AllModuleTypes == true) || usecat.TypeId == ItemTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
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
						//var audit = new AuditModel { TableName = "UserDefValues", Description = "" };
						//var auditFields = new List<AuditFieldModel>();
						//var auditField = new AuditFieldModel { ControlName = "UserValues[i].FieldValue", };
						var userDefinedValue = new UserDefValue();

						if (list[i].UserDefValueId != null && list[i].UserDefValueId != Guid.Empty)
						{
							Guid tempGuid = list[i].UserDefValueId;
							userDefinedValue = await cwmContext.UserDefValues.SingleOrDefaultAsync(a => a.UserDefValueId == tempGuid);
							//auditField.OldId = userDefinedValue.UserDefValueId;
							//auditField.OldValue = userDefinedValue.UserDefValue1;
							//audit.AuditAction = "RECORD UPDATED";
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
							//auditField.OldId = null;
							//auditField.OldValue = null;
							//audit.AuditAction = "RECORD CREATED";
						}
						userDefinedValue.UserDefValue1 = list[i].UserDefValue1;
						userDefinedValue.DateUpdated = DateTime.Now;
						//auditField.NewId = userDefinedValue.UserDefValueId;
						//auditField.NewValue = userDefinedValue.UserDefValue1;
						//auditField.FieldDesc = cwmContext.UserDefFields.FirstOrDefault(a => a.UserDefFieldId == userDefinedValue.UserDefFieldId).FieldDesc;
						//audit.RecordId = userDefinedValue.UserDefValueId;

						if (cwmContext is DbContext)
						{
							try
							{
								await ((DbContext)cwmContext).SaveChangesAsync();
								//bool idCheck = (auditField.OldId ?? Guid.Empty) != (auditField.NewId ?? Guid.Empty);
								//bool valCheck = (auditField.OldValue ?? String.Empty) != (auditField.NewValue ?? String.Empty);
								//if (idCheck || valCheck)
								//{
								//	auditFields.Add(auditField);
								//	await auditService.UpdateAudit(audit, auditFields);
								//}
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

		public async Task<v_Locations> GetLocationAsync(Guid? locationId)
		{
			v_Locations result;
			try
			{
				result = await cwmContext.v_Locations.Where(l=>l.LocationId == locationId).FirstOrDefaultAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item List.", ex);
				result = new v_Locations();
			}
			return result;
		}
	}
}

