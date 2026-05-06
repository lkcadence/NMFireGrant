//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using AutoMapper;
using NMSFM.Data;
using NMSFM.Services.Audit;
using NMSFM.Services.CPSystem;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using NMSFM.Services.UDF;
using NMSFM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NMSFM.Services.Fee
{
	public class FeeService : IFeeService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;
		private IUDFService udfService;
		private ISystemService systemService;


		//public InvoiceService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		public FeeService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
			udfService = new UDFService(cwmContext, logger);
			systemService = new SystemService(cwmContext, logger);
		}

		//Task<IEnumerable<v_Fees>> GetFeesAsync()
		public async Task<IEnumerable<v_Fees>> GetFeesAsync()
		{
			IEnumerable<v_Fees> result;
			try
			{
				var fees = await cwmContext.v_Fees.ToListAsync();
				var feeTypeList = await cwmContext.FeeTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.FeeTypeId).ToListAsync();
				if (fees != null && fees.Count() > 0 & feeTypeList != null & feeTypeList.Count() > 0)
				{
					fees = fees.Where(a => feeTypeList.Contains(a.FeeTypeId == null ? Guid.Empty : a.FeeTypeId.Value)).ToList();
				}
				result = fees;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees List.", ex);
				result = new List<v_Fees>();
			}
			return result;
		}

		public async Task<List<v_Fees>> GetFeesByIdAsync(Guid recordId)
		{
			var results = new List<v_Fees>();
			try
			{
				results = await cwmContext.v_Fees.Where(a => a.RecordId == recordId && a.Inactive == false && a.WebViewable == true).AsNoTracking().ToListAsync() ?? new List<v_Fees>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees for Id:" + recordId + ".", ex);
			}
			return results;
		}

		public async Task<v_Fees> GetFeeById(Guid feeId)
		{
			v_Fees result = null;
			try
			{
				if (cwmContext.v_Fees.Select(a => a.FeeId).ToArray().Contains(feeId))
				{
					result = await cwmContext.v_Fees.SingleOrDefaultAsync(f => f.FeeId == feeId);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees List.", ex);
			}
			return result;
		}

		public async Task<FeePayment> GetFeePaymentById(Guid feePaymentId)
		{
			FeePayment result = null;
			try
			{
				//if (cwmContext.v_FeePayments.Select(a => a.FeeId).ToArray().Contains(feePaymentId))
				//{
				result = await cwmContext.FeePayments.SingleOrDefaultAsync(f => f.FeePaymentId == feePaymentId);
				//}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees List.", ex);
			}
			return result;
		}

		//Task<List<FeeType>> GetFeeTypeAsync()
		public async Task<List<FeeType>> GetFeeTypesAsync()
		{
			List<FeeType> result;
			try
			{
				var feeTypeList = await cwmContext.FeeTypes.Where(a => !a.Inactive && a.WebViewable == true).ToListAsync();
				result = feeTypeList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Types list.", ex);
				result = new List<FeeType>();
			}
			return result;
		}

		public async Task<List<FeeType>> GetFeeTypeAsync(Guid feeTypeId)
		{
			List<FeeType> result;
			try
			{
				var feeTypeList = await cwmContext.FeeTypes.Where(a => a.FeeTypeId == feeTypeId).ToListAsync();
				result = feeTypeList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Types list.", ex);
				result = new List<FeeType>();
			}
			return result;
		}

		public async Task<List<FeeTypeRR>> GetFeeTypesRRAsync(Guid id)
		{
			List<FeeTypeRR> result;
			try
			{
				var feeTypeRR = await cwmContext.FeeTypeRRs.Where(a => a.FeeTypeId == id).ToListAsync();
				result = feeTypeRR;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Types list.", ex);
				result = new List<FeeTypeRR>();
			}
			return result;
		}

		public async Task<List<FeeTypePT>> GetFeeTypePTAsync(Guid id)
		{
			List<FeeTypePT> result;
			try
			{
				var feeTypePT = await cwmContext.FeeTypePTs.Where(a => a.BaseFeeTypeId == id).ToListAsync();
				result = feeTypePT;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Type PT list.", ex);
				result = new List<FeeTypePT>();
			}
			return result;
		}

		public async Task<FeeTypePen> GetFeeTypePenAsync(Guid id)
		{
			FeeTypePen result;
			try
			{
				var feeTypePen = await cwmContext.FeeTypePens.SingleOrDefaultAsync(a => a.FeeTypeId == id);
				result = feeTypePen;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Type Penalty data.", ex);
				result = new FeeTypePen();
			}
			return result;
		}

		public async Task<List<FeesPT>> GetFeesPTsAsync(Guid id)
		{
			List<FeesPT> result;
			try
			{
				var feesPTs = await cwmContext.FeesPTs.Where(a => a.BaseFeeId == id).ToListAsync();
				result = feesPTs;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees list.", ex);
				result = new List<FeesPT>();
			}
			return result;
		}

		public async Task<List<FeesPT>> SetFeesPTsAsync(string[] fees, Guid baseFeeId)  //, Guid feeTypeId)? to get the base feetype
		{
			List<FeesPT> result;
			try
			{
				var oldFees = await cwmContext.FeesPTs.Where(f => f.BaseFeeId == baseFeeId).ToListAsync();

				foreach (FeesPT fee in oldFees)
				{
					cwmContext.FeesPTs.Remove(fee);

				}
				await ((DbContext)cwmContext).SaveChangesAsync();
				foreach (string feeId in fees)
				{
					var newPt = cwmContext.FeesPTs.Add(new FeesPT());
					newPt.rowguid = Guid.NewGuid();
					newPt.FeesPTId = Guid.NewGuid();
					newPt.DateInserted = DateTime.Now;
					newPt.DateUpdated = newPt.DateInserted;

					newPt.BaseFeeId = baseFeeId;
					newPt.FeeId = new Guid(feeId);
					await ((DbContext)cwmContext).SaveChangesAsync();
				}

				result = await GetFeesPTsAsync(baseFeeId);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fees list.", ex);
				result = new List<FeesPT>();
			}
			return result;
		}

		public async Task<List<FeeSchedule>> GetFeeSchedulesAsync(Guid id)
		{
			List<FeeSchedule> result;
			try
			{
				var feeSchedule = await cwmContext.FeeSchedules.Where(a => a.FeeTypeId == id).ToListAsync();
				result = feeSchedule;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Types list.", ex);
				result = new List<FeeSchedule>();
			}
			return result;
		}

		public async Task<List<SelectListItem>> GetFeeInvItemPLAsync(Guid invItemId)
		{
			List<SelectListItem> result = new List<SelectListItem>();

			var oRow = await cwmContext.InventoryItems.FirstOrDefaultAsync(i => i.InvItemId == invItemId);
			if (oRow != null)
			{
				if (oRow.PriceLevel1 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 1", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel2 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 2", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel3 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 3", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel4 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 4", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel5 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 5", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel6 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 6", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel7 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 7", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel8 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 8", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel9 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 9", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
				if (oRow.PriceLevel10 != null)
				{
					result.Add(new SelectListItem() { Text = "Price Level 10", Value = String.Format(CultureInfo.CurrentCulture, "{0, C}", oRow.PriceLevel1) });
				}
			}

			return result.ToList();

		}

		////Task<IEnumerable<PermitStatu>> GetPermitStatusListAsync(Guid agencyId);
		public async Task<string> GetDefaultPriceLevel(Guid InventoryItemId, Guid partyId, Guid agencyId)
		{
			int? priceLevel = 0;

			var invItemTypeid = (await cwmContext.InventoryItems.FirstOrDefaultAsync(i => i.InvItemId == InventoryItemId)).InvItemTypeId;

			if (await systemService.GetCodepalBooleanSetting("UseMultiplePL", agencyId, null))
			{
				if (invItemTypeid != null)
				{
					priceLevel = (await cwmContext.PartyPriceLevels.FirstOrDefaultAsync(p => p.PartyId == partyId && p.InvItemTypeId == invItemTypeid)).PriceLevel;
				}
			}
			if (priceLevel == null || priceLevel == 0)
			{
				priceLevel = (await cwmContext.InventoryItemTypes.FirstOrDefaultAsync(ii => ii.InvItemTypeId == invItemTypeid)).PriceLevel;
			}

			if (priceLevel == null || priceLevel == 0)
			{
				priceLevel = (await cwmContext.Parties.FirstOrDefaultAsync(p => p.PartyID == partyId)).PriceLevel;
			}

			if (priceLevel == null || priceLevel == 0)
			{
				priceLevel = 1;
			}


			return priceLevel.ToString();
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
				logger.Error("Unexpected exception caught while retrieving the inspector list.", ex);
			}
			return result;
		}

		//Task<IEnumerable<v_AddressParties>> GetPartyNameListAsync()
		public async Task<IEnumerable<v_AddressParties>> GetPartyNameListAsync()
		{
			IEnumerable<v_AddressParties> result;
			try
			{
				var addressParties = await cwmContext.v_AddressParties.Where(p => (!p.Inactive.HasValue || !p.Inactive.Value)).ToListAsync();
				var roleTypeList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
				if (addressParties != null && addressParties.Count() > 0 && roleTypeList != null && roleTypeList.Count() > 0)
				{
					addressParties = addressParties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList();
				}
				result = addressParties.GroupBy(a => a.PartyID).Select(a => a.First());
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Responsible Party list.", ex);
				result = new List<v_AddressParties>();
			}
			return result;
		}

		//Task<IEnumerable<v_FeePayments>> GetFeePaymentsAsync(Guid id);
		public async Task<IEnumerable<v_FeePayments>> GetFeePaymentsAsync(Guid id)
		{
			IEnumerable<v_FeePayments> result = new List<v_FeePayments>();
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var feePayments = await cwmContext.v_FeePayments.Where(p => p.FeeId == id).ToListAsync();

				result = feePayments;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Payments List.", ex);
			}
			return result;
		}

		public async Task<FeeSetting> GetFeeSettingAsync(Guid feeTypeId)
		{
			FeeSetting result = null;
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				result = await cwmContext.FeeSettings.SingleOrDefaultAsync(a => a.FeeTypeId == feeTypeId);
				if (result == null)
				{
					result = await cwmContext.FeeSettings.SingleOrDefaultAsync(a => a.FeeTypeId == feeTypeId);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fee Settings for Activity Id: " + feeTypeId + ".", ex);
			}
			return result;
		}

		//Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);
		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			Guid FeeTypeId = pTypeId;
			var ModuleId = new Guid();
			try
			{
				v_Fees fee = null;
				fee = await cwmContext.v_Fees.SingleOrDefaultAsync(a => a.FeeTypeId == id);
				if (fee != null)
				{
					FeeTypeId = (Guid)fee.FeeTypeId;
				}

				Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
				if (agency != null && agency != Guid.Empty)
				{
					AgencyId = agency;
				}
				Module module = null;
				module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Fee");
				ModuleId = module.ModuleId;

				var models = from cats in cwmContext.UserDefCategories
							 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = FeeTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
							 from usecat in subcat.DefaultIfEmpty()
							 where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == FeeTypeId) || (usecat.TypeId == FeeTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == FeeTypeId && cats.AllModuleTypes == true) || usecat.TypeId == FeeTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
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
				logger.Error("Unexpected exception caught while retrieving user defined values for fee '" + id.ToString() + "'.", ex);
			}

			return results;
		}

		public async Task<bool> SaveFeePayment(DetailedFeePayment model, bool isNew)
		{
			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  
				if (!isNew)
				{
					var editFeePayment = await cwmContext.FeePayments.SingleOrDefaultAsync(a => a.FeePaymentId == model.FeePaymentId);
					if (editFeePayment == null)
					{
						logger.Error("Unable to update fee payment '" + model.FeePaymentId.ToString() + "'.  The fee payment could not be located in the database.");
						return false;
					}

					var audit = new AuditModel { TableName = "FeePayments", RecordId = model.FeePaymentId, AuditAction = "RECORD UPDATED", Description = "" };
					var auditFields = new List<AuditFieldModel>();

					if (editFeePayment.PaymentDate != model.PaymentDate)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = editFeePayment.PaymentDate.ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
						editFeePayment.PaymentDate = Convert.ToDateTime(model.PaymentDate ?? DateTime.Now);
					}

					if (editFeePayment.PaymentAmt != model.PaymentAmt)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtAmount", FieldDesc = "PaymentAmt", OldId = null, OldValue = editFeePayment.PaymentAmt.ToString(), NewId = null, NewValue = model.PaymentAmt.ToString() });
						editFeePayment.PaymentAmt = model.PaymentAmt ?? 0;
					}


					if (editFeePayment.PaymentType != model.PaymentType)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cboType", FieldDesc = "PaymentType", OldId = null, OldValue = editFeePayment.PaymentType, NewId = null, NewValue = model.PaymentType });
						editFeePayment.PaymentType = model.PaymentType;
					}

					if (editFeePayment.RefNum != model.RefNum)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtRefNum", FieldDesc = "RefNum", OldId = null, OldValue = editFeePayment.RefNum, NewId = null, NewValue = model.RefNum });
						editFeePayment.RefNum = model.RefNum;
					}

					if (editFeePayment.ReceivedFrom != model.ReceivedFrom)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cboReceivedFrom", FieldDesc = "ReceivedFrom", OldId = null, OldValue = editFeePayment.ReceivedFrom, NewId = null, NewValue = model.ReceivedFrom });
						editFeePayment.ReceivedFrom = model.ReceivedFrom;
					}

					if (editFeePayment.PaymentUserId != model.PaymentUserId)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cboInspector", FieldDesc = "PaymentUserId", OldId = null, OldValue = editFeePayment.PaymentUserId.ToString(), NewId = null, NewValue = model.PaymentUserId.ToString() });
						editFeePayment.PaymentUserId = model.PaymentUserId;
					}

					if (editFeePayment.Comment != model.Comment)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = editFeePayment.Comment, NewId = null, NewValue = model.Comment });
						editFeePayment.Comment = model.Comment;
					}

					if (editFeePayment.Void != model.Void)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "chkVoid", FieldDesc = "Void", OldId = null, OldValue = Math.Abs(Convert.ToInt32(editFeePayment.Void)).ToString(), NewId = null, NewValue = Math.Abs(Convert.ToInt32(model.Void)).ToString() });
						editFeePayment.Void = model.Void;
					}

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (await UpdateFeeWithPayment(editFeePayment))
							{
								if (auditFields.Count() > 0)
								{
									await auditService.UpdateAudit(audit, auditFields);
								}
								return true;
							}
							else
							{
								cwmContext.FeePayments.Remove(editFeePayment);
								await ((DbContext)cwmContext).SaveChangesAsync();
								return false;
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update fee payment '" + model.FeePaymentId.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to update fee payment '" + model.FeePaymentId.ToString() + "', DbContext was not available.");
						return false;
					}

				}

				var newAudit = new AuditModel { TableName = "FeePayments", RecordId = model.FeePaymentId, AuditAction = "RECORD CREATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				var feePayment = cwmContext.FeePayments.Add(new FeePayment());

				feePayment.FeePaymentId = model.FeePaymentId;
				feePayment.FeeId = model.FeeId;
				feePayment.PaymentDate = DateTime.MinValue;
				feePayment.PaymentAmt = 0;
				feePayment.PaymentType = null;
				feePayment.RefNum = null;
				feePayment.ReceivedFrom = null;
				feePayment.Comment = null;
				feePayment.PaymentUserId = null;
				feePayment.InvoicePaymentId = null;
				feePayment.Void = false;
				feePayment.rowguid = Guid.NewGuid();
				feePayment.DateUpdated = DateTime.Now;
				feePayment.DateInserted = feePayment.DateUpdated;

				newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = model.FeePaymentId, NewValue = null });

				if (model.PaymentDate == null || feePayment.PaymentDate != model.PaymentDate)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = feePayment.PaymentDate.ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
					feePayment.PaymentDate = Convert.ToDateTime((model.PaymentDate ?? DateTime.Now).ToShortDateString());
				}

				if (feePayment.PaymentAmt != model.PaymentAmt)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "txtAmount", FieldDesc = "PaymentAmt", OldId = null, OldValue = feePayment.PaymentAmt.ToString(), NewId = null, NewValue = model.PaymentAmt.ToString() });
					feePayment.PaymentAmt = model.PaymentAmt ?? 0;
				}

				if (feePayment.PaymentType != model.PaymentType)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "cboType", FieldDesc = "PaymentType", OldId = null, OldValue = feePayment.PaymentType, NewId = null, NewValue = model.PaymentType });
					feePayment.PaymentType = model.PaymentType;
				}

				if (feePayment.RefNum != model.RefNum)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "txtRefNum", FieldDesc = "RefNum", OldId = null, OldValue = feePayment.RefNum, NewId = null, NewValue = model.RefNum });
					feePayment.RefNum = model.RefNum;
				}

				if (feePayment.ReceivedFrom != model.ReceivedFrom)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "cboReceivedFrom", FieldDesc = "ReceivedFrom", OldId = null, OldValue = feePayment.ReceivedFrom, NewId = null, NewValue = model.ReceivedFrom });
					feePayment.ReceivedFrom = model.ReceivedFrom;
				}

				if (feePayment.PaymentUserId != model.PaymentUserId)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "cboInspector", FieldDesc = "PaymentUserId", OldId = null, OldValue = feePayment.PaymentUserId.ToString(), NewId = null, NewValue = model.PaymentUserId.ToString() });
					feePayment.PaymentUserId = model.PaymentUserId;
				}

				if (feePayment.Comment != model.Comment)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = feePayment.Comment, NewId = null, NewValue = model.Comment });
					feePayment.Comment = model.Comment;
				}

				if (feePayment.Void != model.Void)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "chkVoid", FieldDesc = "Void", OldId = null, OldValue = Math.Abs(Convert.ToInt32(feePayment.Void)).ToString(), NewId = null, NewValue = Math.Abs(Convert.ToInt32(model.Void)).ToString() });
					feePayment.Void = model.Void;
				}
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();

						if (await UpdateFeeWithPayment(feePayment))
						{
							if (newAuditFields.Count() > 0)
							{
								await auditService.UpdateAudit(newAudit, newAuditFields);
							}
							return true;
						}
						else
						{
							cwmContext.FeePayments.Remove(feePayment);
							await ((DbContext)cwmContext).SaveChangesAsync();
							return false;
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create fee payment '" + model.FeePaymentId.ToString() + "'.", ex);
						return false;
					}
				}
				else
				{
					logger.Error("Unable to create fee payment '" + model.FeePaymentId.ToString() + "', DbContext was not available.");
					return false;
				}

			}
			return false;
		}

		public async Task<bool> SaveFee(DetailedFee model)
		{
			if (model != null)
			{
				Data.Fee fee = null;
				var auditFields = new List<AuditFieldModel>();
				AuditModel audit = null;

				fee = await cwmContext.Fees.SingleOrDefaultAsync(a => a.FeeId == model.FeeId);
				if (fee != null)
				{
					audit = new AuditModel { TableName = "Fees", RecordId = model.FeeId, AuditAction = "RECORD UPDATED", Description = "" };


					fee.rowguid = Guid.NewGuid();
					fee.DateUpdated = DateTime.Now;



					//fee.FeeTypeId = model.FeeTypeId;
					if (fee.FeeTypeId != model.FeeTypeId)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = fee.FeeTypeId, OldValue = (await cwmContext.FeeTypes.FirstOrDefaultAsync(ft => ft.FeeTypeId == fee.FeeTypeId)).FeeType1, NewId = model.FeeTypeId, NewValue = (await cwmContext.FeeTypes.FirstOrDefaultAsync(ft => ft.FeeTypeId == model.FeeTypeId)).FeeType1 });
						fee.FeeTypeId = model.FeeTypeId;
					}
					//fee.FeeAmt = model.FeeAmt;
					if (fee.FeeAmt != model.FeeAmt)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtFee", FieldDesc = "FeeAmt", OldId = null, OldValue = fee.FeeAmt.ToString(), NewId = null, NewValue = model.FeeAmt.ToString() });
						fee.FeeAmt = model.FeeAmt;
					}
					//fee.PaymentAmt = model.PaymentAmt;
					if (fee.PaymentAmt != model.PaymentAmt)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtPayment", FieldDesc = "PaymentAmt", OldId = null, OldValue = fee.PaymentAmt.ToString(), NewId = null, NewValue = model.PaymentAmt.ToString() });
						fee.PaymentAmt = model.PaymentAmt;
					}
					//fee.PaymentDate = model.PaymentDate;
					if (fee.PaymentDate != model.PaymentDate)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = (fee.PaymentDate ?? DateTime.Now).ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
						fee.PaymentDate = model.PaymentDate ?? DateTime.Now;
					}
					//fee.PaymentUserId = model.PaymentUserId;
					if (fee.PaymentUserId != model.PaymentUserId && model.PaymentUserId != Guid.Empty)
					{
						if (model.PaymentUserId != Guid.Empty)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "cboInspector", FieldDesc = "PaymentUserId", OldId = null, OldValue = fee.PaymentUserId.ToString(), NewId = null, NewValue = model.PaymentUserId.ToString() });
							fee.PaymentUserId = model.PaymentUserId;
						}
						else
						{
							fee.PaymentUserId = null;
						}

					}
					//fee.RefNum
					if (fee.RefNum != model.RefNum)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtRefNum", FieldDesc = "RefNum", OldId = null, OldValue = fee.RefNum, NewId = null, NewValue = model.RefNum });
						fee.RefNum = model.RefNum;
					}

					//fee.Comment = model.Comment;
					if (fee.Comment != model.Comment)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = fee.Comment, NewId = null, NewValue = model.Comment });
						fee.Comment = model.Comment;
					}
					//fee.FeeDate = model.FeeDate;
					if (fee.FeeDate != model.FeeDate)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = (fee.FeeDate ?? DateTime.Now).ToShortDateString(), NewId = null, NewValue = (model.FeeDate ?? DateTime.Now).ToShortDateString() });
						fee.FeeDate = model.FeeDate ?? DateTime.Now;
					}
					//fee.FeeBase = model.FeeBase;
					if (fee.FeeBase != model.FeeBase)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtFeeBase", FieldDesc = "FeeBase", OldId = null, OldValue = fee.FeeBase.ToString(), NewId = null, NewValue = model.FeeBase.ToString() });
						fee.FeeBase = model.FeeBase;
					}
					//fee.Units = model.Units;
					if (fee.Units != model.Units)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "txtCount", FieldDesc = "Units", OldId = null, OldValue = fee.Units.ToString(), NewId = null, NewValue = model.Units.ToString() });
						fee.Units = model.Units;
					}
					//fee.FeeUOM = model.FeeUOM;
					if (fee.FeeUOM != model.FeeUOM)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cboPer", FieldDesc = "FeeUOM", OldId = null, OldValue = fee.FeeUOM, NewId = null, NewValue = model.FeeUOM });
						fee.FeeUOM = model.FeeUOM;
					}
					//fee.ResponsiblePartyId = model.ResponsiblePartyId;
					if (fee.ResponsiblePartyId != model.ResponsiblePartyId && model.ResponsiblePartyId != Guid.Empty)
					{
						if (model.ResponsiblePartyId != Guid.Empty)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = fee.ResponsiblePartyId, OldValue = (await cwmContext.Parties.FirstOrDefaultAsync(p => p.PartyID == fee.ResponsiblePartyId)).PartyName, NewId = model.ResponsiblePartyId, NewValue = (await cwmContext.Parties.FirstOrDefaultAsync(p => p.PartyID == model.ResponsiblePartyId)).PartyName });
							fee.ResponsiblePartyId = model.ResponsiblePartyId;
						}
						else
						{
							fee.ResponsiblePartyId = null;
						}
					}

					//auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = fee.PaymentDate.ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
					fee.FeeDesc = GetFeeDescription(model);

					//fee.FeeStatus = model.FeeStatus;				
					if (fee.FeeStatus != model.FeeStatus)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "cboFeeStatus", FieldDesc = "FeeStatus", OldId = null, OldValue = fee.FeeStatus.ToString(), NewId = null, NewValue = model.FeeStatus.ToString() });
						fee.FeeStatus = model.FeeStatus;
					}
					//fee.ReCalcDate = fee.DateUpdated;
					if (fee.PaymentDate != model.PaymentDate)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = (fee.PaymentDate ?? DateTime.Now).ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
						fee.ReCalcDate = fee.DateUpdated;
					}

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (auditFields.Count() > 0)
							{
								await auditService.UpdateAudit(audit, auditFields);
							}
							return true;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update fee '" + model.FeeId.ToString() + "'.", ex);
							return false;
						}
					}
					else
					{
						logger.Error("Unable to update fee '" + model.FeeId.ToString() + "', DbContext was not available.");
						return false;
					}
				}

				//newFee
				fee = cwmContext.Fees.Add(new Data.Fee());
				audit = new AuditModel { TableName = "Fees", RecordId = model.FeeId, AuditAction = "RECORD CREATED", Description = "" };

				fee.DateUpdated = DateTime.Now;

				fee.DateInserted = fee.DateUpdated;
				fee.FeeId = model.FeeId;
				auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "FeeId", OldId = null, OldValue = null, NewId = fee.FeeId, NewValue = null });
				//fee.OriginalFeeDate
				//auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = fee.PaymentDate.ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
				fee.OriginalFeeDate = Convert.ToDateTime(fee.DateUpdated.ToShortDateString());

				//fee.RecordId = model.RecordId;
				if (fee.RecordId != model.RecordId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "lblNumber", FieldDesc = "RecordId", OldId = null, OldValue = null, NewId = fee.RecordId, NewValue = null });
					fee.RecordId = model.RecordId;
				}

				//fee.FeeTypeId = model.FeeTypeId;
				if (fee.FeeTypeId != model.FeeTypeId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = null, OldValue = null, NewId = model.FeeTypeId, NewValue = (await cwmContext.FeeTypes.FirstOrDefaultAsync(ft => ft.FeeTypeId == model.FeeTypeId)).FeeType1 });
					fee.FeeTypeId = model.FeeTypeId;
				}
				//fee.FeeAmt = model.FeeAmt;
				if (fee.FeeAmt != model.FeeAmt)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtFee", FieldDesc = "FeeAmt", OldId = null, OldValue = null, NewId = null, NewValue = model.FeeAmt.ToString() });
					fee.FeeAmt = model.FeeAmt;
				}
				//fee.PaymentAmt = model.PaymentAmt;
				if (fee.PaymentAmt != model.PaymentAmt)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtPayment", FieldDesc = "PaymentAmt", OldId = null, OldValue = null, NewId = null, NewValue = model.PaymentAmt.ToString() });
					fee.PaymentAmt = model.PaymentAmt;
				}
				//fee.PaymentDate = model.PaymentDate;
				if (fee.PaymentDate != model.PaymentDate)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = null, NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
					fee.PaymentDate = model.PaymentDate ?? DateTime.Now;
				}
				//fee.PaymentUserId = model.PaymentUserId;
				if (fee.PaymentUserId != model.PaymentUserId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "cboInspector", FieldDesc = "PaymentUserId", OldId = null, OldValue = null, NewId = null, NewValue = model.PaymentUserId.ToString() });
					fee.PaymentUserId = model.PaymentUserId;
				}
				//fee.RefNum
				if (fee.RefNum != model.RefNum)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtRefNum", FieldDesc = "RefNum", OldId = null, OldValue = null, NewId = null, NewValue = model.RefNum });
					fee.RefNum = model.RefNum;
				}

				//fee.Comment = model.Comment;
				if (fee.Comment != model.Comment)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtComment", FieldDesc = "Comment", OldId = null, OldValue = null, NewId = null, NewValue = model.Comment });
					fee.Comment = model.Comment;
				}
				//fee.FeeDate = model.FeeDate;
				if (fee.FeeDate != model.FeeDate)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = null, NewId = null, NewValue = (model.FeeDate ?? DateTime.Now).ToShortDateString() });
					fee.FeeDate = model.FeeDate ?? DateTime.Now;
				}
				//fee.FeeBase = model.FeeBase;
				if (fee.FeeBase != model.FeeBase)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtFeeBase", FieldDesc = "FeeBase", OldId = null, OldValue = null, NewId = null, NewValue = model.FeeBase.ToString() });
					fee.FeeBase = model.FeeBase;
				}
				//fee.Units = model.Units;
				if (fee.Units != model.Units)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "txtCount", FieldDesc = "Units", OldId = null, OldValue = null, NewId = null, NewValue = model.Units.ToString() });
					fee.Units = model.Units;
				}
				//fee.FeeUOM = model.FeeUOM;
				if (fee.FeeUOM != model.FeeUOM)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "cboPer", FieldDesc = "FeeUOM", OldId = null, OldValue = null, NewId = null, NewValue = model.FeeUOM });
					fee.FeeUOM = model.FeeUOM;
				}
				//fee.ResponsiblePartyId = model.ResponsiblePartyId;
				if (fee.ResponsiblePartyId != model.ResponsiblePartyId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = null, OldValue = null, NewId = model.ResponsiblePartyId, NewValue = (await cwmContext.Parties.FirstOrDefaultAsync(p => p.PartyID == model.ResponsiblePartyId)).PartyName });
					fee.ResponsiblePartyId = model.ResponsiblePartyId;
				}

				//auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = fee.PaymentDate.ToShortDateString(), NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
				fee.FeeDesc = GetFeeDescription(model);

				//fee.FeeStatus = model.FeeStatus;				
				if (fee.FeeStatus != model.FeeStatus)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "cboFeeStatus", FieldDesc = "FeeStatus", OldId = null, OldValue = null, NewId = null, NewValue = model.FeeStatus.ToString() });
					fee.FeeStatus = model.FeeStatus;
				}
				//fee.ReCalcDate = fee.DateUpdated;
				if (fee.PaymentDate != model.PaymentDate)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = null, NewId = null, NewValue = (model.PaymentDate ?? DateTime.Now).ToShortDateString() });
					fee.ReCalcDate = fee.DateUpdated;
				}

				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();
						if (auditFields.Count() > 0)
						{
							await auditService.UpdateAudit(audit, auditFields);
						}
						return true;
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to update fee '" + model.FeeId.ToString() + "'.", ex);
						return false;
					}
				}
				else
				{
					logger.Error("Unable to update fee '" + model.FeeId.ToString() + "', DbContext was not available.");
					return false;
				}





			}
			return false;
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
								logger.Error("Unable to save the user defined value changes for invoice '" + list[i].RecordId.ToString() + "'.", ex);
								return;
							}
						}
						else
						{
							logger.Error("Unable to update the user defined values for invoice '" + list[i].RecordId.ToString() + "', DbContext was not available.");
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

		private async Task<bool> UpdateFeeWithPayment(FeePayment model)
		{
			decimal balance = 0;
			DateTime? payDate = null;
			string refNum = null;
			Guid? paymentUserId = null;
			string inspectorName = "";

			var feePayments = cwmContext.FeePayments.Where(a => a.FeeId == model.FeeId).OrderBy(a => a.PaymentDate);
			foreach (FeePayment feePayment in feePayments)
			{
				if (!feePayment.Void)
				{
					balance += feePayment.PaymentAmt;
					if (payDate == null || feePayment.PaymentDate > payDate)
					{
						payDate = feePayment.PaymentDate;
						refNum = feePayment.RefNum;
						paymentUserId = feePayment.PaymentUserId;
						inspectorName = cwmContext.Inspectors.FirstOrDefault(a => a.InspectorId == paymentUserId).InspectorName;
					}
				}
			}

			var newAudit = new AuditModel { TableName = "Fees", RecordId = model.FeeId, AuditAction = "RECORD UPDATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			var fee = cwmContext.Fees.First(a => a.FeeId == model.FeeId);
			string oldInsp = "";
			if (fee.PaymentUserId != null)
			{
				oldInsp = cwmContext.Inspectors.FirstOrDefault(a => a.InspectorId == fee.PaymentUserId).InspectorName;
			}

			newAuditFields.Add(new AuditFieldModel { ControlName = "txtPayment", FieldDesc = "PaymentAmt", OldId = null, OldValue = fee.PaymentAmt.ToString(), NewId = null, NewValue = balance.ToString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "dtDate", FieldDesc = "PaymentDate", OldId = null, OldValue = fee.PaymentDate.ToString(), NewId = null, NewValue = payDate.Value.ToShortDateString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "txtRefNum", FieldDesc = "RefNum", OldId = null, OldValue = fee.RefNum, NewId = null, NewValue = refNum });
			newAuditFields.Add(new AuditFieldModel { ControlName = "cboInspector", FieldDesc = "PaymentUserId", OldId = fee.PaymentUserId, OldValue = oldInsp, NewId = null, NewValue = inspectorName });

			fee.PaymentAmt = balance;
			if (balance != 0)
			{
				fee.PaymentDate = payDate;
				fee.RefNum = refNum;
				fee.PaymentUserId = paymentUserId;
			}

			if (cwmContext is DbContext)
			{
				try
				{
					await ((DbContext)cwmContext).SaveChangesAsync();
					if (newAuditFields.Count() > 0)
					{
						await auditService.UpdateAudit(newAudit, newAuditFields);
					}
					return true;
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to update fee with payment '" + model.FeePaymentId.ToString() + "'.", ex);
					return false;
				}
			}
			else
			{
				logger.Error("Unable to update fee with payment '" + model.FeePaymentId.ToString() + "', DbContext was not available.");
				return false;
			}
		}

		public async Task<Guid?> InsertScannedFee(string barCode, Guid recordId)
		{
			Guid? result = Guid.Empty;
			//Dim oForm As New frmFees
			Guid feeTypeId;
			Guid? feeId = null;
			bool rate;
			Guid? invItemId;

			v_Activities inspection = await cwmContext.v_Activities.SingleOrDefaultAsync(i => i.InspectionId == recordId);
			v_Permits permit = await cwmContext.v_Permits.SingleOrDefaultAsync(i => i.PermitId == recordId);
			v_Projects project = await cwmContext.v_Projects.SingleOrDefaultAsync(i => i.ProjectId == recordId);


			var oTable = await cwmContext.FeeTypes.SingleOrDefaultAsync(f => f.FeeBarcode == barCode);

			if (oTable != null)
			{
				feeTypeId = oTable.FeeTypeId;
				rate = oTable.Rate;
				invItemId = oTable.InvItemId;
			}
			else
			{
				var iTable = await cwmContext.InventoryItems.SingleOrDefaultAsync(i => i.Barcode == barCode);
				if (iTable != null)
				{
					oTable = await cwmContext.FeeTypes.SingleOrDefaultAsync(f => f.InvItemId == iTable.InvItemId);
					if (oTable != null)
					{
						feeTypeId = oTable.FeeTypeId;
						rate = oTable.Rate;
						invItemId = oTable.InvItemId;
					}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}


			var tmpFee = cwmContext.v_Fees.FirstOrDefault(f => f.RecordId == recordId && f.FeeTypeId == feeTypeId);
			if (tmpFee != null)
			{
				feeId = tmpFee.FeeId;
			}

			if (feeId != null && feeId != Guid.Empty)
			{
				if (rate | (invItemId != null && invItemId != Guid.Empty))
				{
					if ((await cwmContext.Settings.SingleOrDefaultAsync(a => a.PropertyField == "AddOneOnScan")).ValueField == "1")
					{
						var fee = await cwmContext.Fees.SingleOrDefaultAsync(f => f.FeeId == feeId);
						if (fee.FeeId != null && fee.FeeId != Guid.Empty)
						{
							if (fee.Units == null)
							{
								fee.FeeDesc = "1 x " + String.Format("{0:$#,##0.0000}", fee.FeeBase);
								fee.Units = 1;
								fee.FeeAmt = fee.FeeBase;
							}
							else
							{
								fee.FeeDesc = (fee.Units + 1) + " x " + String.Format("{0:$#,##0.0000}", fee.FeeBase);
								fee.Units = (fee.Units + 1);
								fee.FeeAmt = fee.Units * fee.FeeBase;
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
									logger.Error("Unable to save fee '" + feeId.ToString() + "'.", ex);
									return null;
								}
							}
							else
							{
								logger.Error("Unable to update fee '" + feeId.ToString() + "', DbContext was not available.");
								return null;
							}
							if ((await cwmContext.Settings.SingleOrDefaultAsync(a => a.PropertyField == "AndOpenFee")).ValueField == "1")
							{
								result = fee.FeeId;
							}
						}

					}
				}
			}
			else
			{
				if ((await cwmContext.Settings.SingleOrDefaultAsync(a => a.PropertyField == "AddFeeOnScanNoPreExisting")).ValueField == "1")
				{
					decimal? plFeeAmt;
					Guid? plResppartyId = null;

					if ((await cwmContext.Settings.SingleOrDefaultAsync(a => a.PropertyField == "DefaultRespParty")).ValueField == "1")
					{
						if (inspection != null)
						{
							if (inspection.PrimaryParty)
							{
								plResppartyId = inspection.AlternatePartyId;
							}
							else
							{
								plResppartyId = inspection.InspectedPartyId;
							}
						}
						else if (permit != null)
						{
							plResppartyId = permit.IssuedToPartyId;
						}
					}
					string priceLevel = GetFeeUOM(invItemId, plResppartyId, out plFeeAmt);

					if (priceLevel != "0")
					{
						Data.Fee fee = GetNewDefaultedFee();
						fee.FeeAmt = plFeeAmt;
						fee.FeeDate = (Convert.ToDateTime(DateTime.Now.ToShortDateString()));
						if (inspection != null)
						{
							fee.RefNum = inspection.InspectionNumber;
						}
						else if (permit != null)
						{
							fee.RefNum = permit.PermitNumber;
						}
						else if (project != null)
						{
							fee.RefNum = project.ProjectNumber;
						}
						fee.FeeTypeId = feeTypeId;
						fee.RecordId = recordId;
						fee.FeeBase = plFeeAmt;
						fee.Units = 1;
						fee.FeeUOM = priceLevel;
						fee.ResponsiblePartyId = plResppartyId;
						fee.FeeDesc = "1 x " + String.Format("{0:$#,##0.0000}", plFeeAmt);
						fee.FeeStatus = 0;
						fee.OriginalFeeDate = fee.FeeDate;
						if (cwmContext is DbContext)
						{
							try
							{
								await ((DbContext)cwmContext).SaveChangesAsync();
							}
							catch (Exception ex)
            {
                _ = ex;
								logger.Error("Unable to save fee '" + feeId.ToString() + "'.", ex);
								return null;
							}
						}
						else
						{
							logger.Error("Unable to update fee '" + feeId.ToString() + "', DbContext was not available.");
							return null;
						}
						if ((await cwmContext.Settings.SingleOrDefaultAsync(a => a.PropertyField == "AndOpenFee")).ValueField == "1")
						{
							result = fee.FeeId;
						}
					}
					else
					{
						result = Guid.Parse("00000000-0000-0000-0000-000000000001");
					}
				}
				else
				{
					result = Guid.Parse("00000000-0000-0000-0000-000000000001");
				}

			}

			return result;
		}

		public async Task<Guid?> GetScanFeeType(string barCode)
		{
			Guid? result = null;

			var oTable = await cwmContext.FeeTypes.SingleOrDefaultAsync(f => f.FeeBarcode == barCode);

			if (oTable != null)
			{
				result = oTable.FeeTypeId;

			}
			else
			{
				var iTable = await cwmContext.InventoryItems.SingleOrDefaultAsync(i => i.Barcode == barCode);
				if (iTable != null)
				{
					oTable = await cwmContext.FeeTypes.SingleOrDefaultAsync(f => f.InvItemId == iTable.InvItemId);
					if (oTable != null)
					{
						result = oTable.FeeTypeId;

					}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}

			return result;
		}

		private string GetFeeUOM(Guid? invItemId, Guid? respPartyId, out decimal? feeAmount)
		{
			feeAmount = 0;
			string result = "0";
			if (invItemId != null)
			{
				var priceLevels = cwmContext.InventoryItems.SingleOrDefault(p => p.InvItemId == invItemId);
				if (cwmContext.Settings.SingleOrDefault(a => a.PropertyField == "UseMultiplePL").ValueField == "1")
				{
					result = GetPartyInvItemTypePriceLevel(respPartyId, invItemId: invItemId);

					if (result == "0")
					{
						result = GetInventoryItemTypeDefaultPriceLevel(invItemId: invItemId);
						if (result == "0")
						{
							result = GetPartyPriceLevel(respPartyId);       //Defaults to "1" if no value.					
						}
					}
					feeAmount = SetPriceLevelFeeAmount(priceLevels, result);
				}
				else
				{
					result = GetInventoryItemTypeDefaultPriceLevel(invItemId: invItemId);
					if (result == "0")
					{
						result = GetPartyPriceLevel(respPartyId);        //Defaults to "1" if no value.					
					}
				}
				feeAmount = SetPriceLevelFeeAmount(priceLevels, result);
			}

			return result;
		}

		private decimal? SetPriceLevelFeeAmount(InventoryItem invItem, string priceLevel)
		{
			decimal? feeAmount;
			switch (priceLevel)
			{
				case "1":
					feeAmount = invItem.PriceLevel1;
					break;
				case "2":
					feeAmount = invItem.PriceLevel2;
					break;
				case "3":
					feeAmount = invItem.PriceLevel3;
					break;
				case "4":
					feeAmount = invItem.PriceLevel4;
					break;
				case "5":
					feeAmount = invItem.PriceLevel5;
					break;
				case "6":
					feeAmount = invItem.PriceLevel6;
					break;
				case "7":
					feeAmount = invItem.PriceLevel7;
					break;
				case "8":
					feeAmount = invItem.PriceLevel8;
					break;
				case "9":
					feeAmount = invItem.PriceLevel9;
					break;
				case "10":
					feeAmount = invItem.PriceLevel10;
					break;
				default:
					feeAmount = 0;
					break;
			}
			return feeAmount;
		}

		private string GetPartyInvItemTypePriceLevel(Guid? partyId, Guid? invItemTypeId = null, Guid? invItemId = null)
		{
			string result = "0";

			if (invItemTypeId == null && (invItemId != null && invItemId != Guid.Empty))
			{
				var tmp = cwmContext.InventoryItems.SingleOrDefault(i => i.InvItemId == invItemId).InvItemTypeId;
				if (tmp != null && tmp != Guid.Empty)
				{
					invItemTypeId = tmp;
				}
			}

			var rettmp = cwmContext.PartyPriceLevels.SingleOrDefault(p => p.PartyId == partyId && p.InvItemTypeId == invItemTypeId).PriceLevel.ToString();
			if (rettmp != null && rettmp != "")
			{
				result = rettmp;
			}

			return result;
		}

		private string GetInventoryItemTypeDefaultPriceLevel(Guid? invItemTypeId = null, Guid? invItemId = null)
		{
			string result = "0";

			if (invItemTypeId == null && (invItemId != null && invItemId != Guid.Empty))
			{
				var tmp = cwmContext.InventoryItems.SingleOrDefault(i => i.InvItemId == invItemId).InvItemTypeId;
				if (tmp != null && tmp != Guid.Empty)
				{
					invItemTypeId = tmp;
				}
			}

			var rettmp = cwmContext.InventoryItemTypes.SingleOrDefault(p => p.InvItemTypeId == invItemTypeId).PriceLevel.ToString();
			if (rettmp != null && rettmp != "")
			{
				result = rettmp;
			}

			return result;
		}

		private string GetPartyPriceLevel(Guid? partyId)
		{
			string result = "1";

			var tmp = cwmContext.Parties.SingleOrDefault(p => p.PartyID == partyId).PriceLevel.ToString();
			if (tmp != null && tmp != "")
			{
				result = tmp;
			}

			return result;
		}

		private Data.Fee GetNewDefaultedFee()
		{
			Data.Fee fee = cwmContext.Fees.Add(new Data.Fee());
			fee.FeeId = Guid.NewGuid();
			fee.rowguid = Guid.NewGuid();
			fee.DateInserted = DateTime.Now;
			fee.DateUpdated = fee.DateInserted;
			fee.IsDefault = false;

			return fee;
		}

		public List<DetailedDefaultFee> GetDefaultFees(Guid? recordId, bool? forReInsp = false, string reInspLetter = "")
		{
			List<DetailedDefaultFee> results = null;

			var defaultFees = cwmContext.DefaultFees.Join(cwmContext.FeeTypes, df => df.FeeTypeId, ft => ft.FeeTypeId, (df, ft) => new { df, ft })
				.Where(ddf => ddf.ft.Inactive == false && ddf.df.RecordId == recordId && ddf.df.ForReInspection == forReInsp);
			if (forReInsp ?? false && (reInspLetter != null && reInspLetter != ""))
			{
				defaultFees = defaultFees.Where(ddf => ((ddf.df.ReInspectionLetter == reInspLetter.PadLeft(2) || ddf.df.ReInspectionLetter == null) || (ddf.df.ReInspForward == true && ((ddf.df.EndReInspectionLetter == "" || ddf.df.EndReInspectionLetter == null) && (String.Compare(ddf.df.ReInspectionLetter, reInspLetter.PadLeft(2)) <= 0 || ddf.df.ReInspectionLetter == null)) || (String.Compare(ddf.df.ReInspectionLetter, reInspLetter.PadLeft(2)) <= 0 && String.Compare(ddf.df.EndReInspectionLetter, reInspLetter.PadLeft(2)) >= 0)))).OrderByDescending(ddf => ddf.ft.Rate).ThenByDescending(ddf => ddf.ft.RatedRange).ThenByDescending(ddf => ddf.ft.TotalPercent).ThenByDescending(ddf => ddf.ft.Penalty);
			}
			else
			{
				defaultFees = defaultFees.OrderByDescending(ddf => ddf.ft.Rate).ThenByDescending(ddf => ddf.ft.RatedRange).ThenByDescending(ddf => ddf.ft.TotalPercent).ThenByDescending(ddf => ddf.ft.Penalty);
			}
			if (recordId != null && recordId != Guid.Empty)
			{
				results.AddRange(defaultFees.Select(a => Mapper.Map<DetailedDefaultFee>(a)).ToList());
			}
			return results;
		}

		public Guid DefaultRegFee(Guid? recordId, string feeAmt, Guid feeTypeId, DateTime feeDate, Guid? respPartyId)
		{
			var fee = GetNewDefaultedFee();
			var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD CREATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			fee.RecordId = recordId;
			fee.FeeAmt = Convert.ToDecimal(feeAmt);
			fee.FeeTypeId = feeTypeId;
			fee.FeeDate = Convert.ToDateTime(feeDate.ToShortDateString());
			fee.IsDefault = true;

			newAuditFields.Add(new AuditFieldModel { ControlName = "txtFee1", FieldDesc = "FeeAmt", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeAmt.ToString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeDate.Value.ToShortDateString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = null, OldValue = null, NewId = fee.FeeTypeId, NewValue = cwmContext.FeeTypes.First(ft => ft.FeeTypeId == fee.FeeTypeId).FeeType1 });

			if ((cwmContext.Settings.SingleOrDefault(a => a.PropertyField == "DefaultRespParty")).ValueField == "1")
			{
				fee.ResponsiblePartyId = respPartyId;
				newAuditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = null, OldValue = null, NewId = fee.ResponsiblePartyId, NewValue = cwmContext.Parties.First(p => p.PartyID == (respPartyId ?? Guid.Empty)).PartyName });
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					if (newAuditFields.Count() > 0)
					{
						auditService.UpdateAudit(newAudit, newAuditFields);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
					return Guid.Empty;
				}
			}
			else
			{
				logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
				return Guid.Empty;
			}

			return fee.FeeId;
		}

		public Guid DefaultRateFee(Guid? recordId, Guid? feeSchedId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId)
		{
			var fee = GetNewDefaultedFee();
			var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD CREATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			fee.RecordId = recordId;
			fee.FeeUOM = cwmContext.FeeSchedules.FirstOrDefault(fs => fs.FeeSchedId == feeSchedId).FeeItem;
			fee.FeeTypeId = feeTypeId;
			fee.FeeDate = Convert.ToDateTime(feeDate.ToShortDateString());
			fee.IsDefault = true;

			newAuditFields.Add(new AuditFieldModel { ControlName = "cboPer", FieldDesc = "FeeUOM", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeUOM });
			newAuditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeDate.Value.ToShortDateString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = null, OldValue = null, NewId = fee.FeeTypeId, NewValue = cwmContext.FeeTypes.First(ft => ft.FeeTypeId == fee.FeeTypeId).FeeType1 });


			if ((cwmContext.Settings.SingleOrDefault(a => a.PropertyField == "DefaultRespParty")).ValueField == "1")
			{
				fee.ResponsiblePartyId = respPartyId;
				newAuditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = null, OldValue = null, NewId = fee.ResponsiblePartyId, NewValue = cwmContext.Parties.First(p => p.PartyID == (respPartyId ?? Guid.Empty)).PartyName });
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					if (newAuditFields.Count() > 0)
					{
						auditService.UpdateAudit(newAudit, newAuditFields);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
					return Guid.Empty;
				}
			}
			else
			{
				logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
				return Guid.Empty;
			}

			ReCalcRatedFee(fee.FeeId, feeSchedId, true);

			return fee.FeeId;
		}

		public Guid DefaultRRFee(Guid? recordId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId)
		{
			var fee = GetNewDefaultedFee();
			var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD CREATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			fee.RecordId = recordId;
			fee.FeeTypeId = feeTypeId;
			fee.FeeDate = Convert.ToDateTime(feeDate.ToShortDateString());
			fee.IsDefault = true;


			newAuditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeDate.Value.ToShortDateString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = null, OldValue = null, NewId = fee.FeeTypeId, NewValue = cwmContext.FeeTypes.First(ft => ft.FeeTypeId == fee.FeeTypeId).FeeType1 });

			if ((cwmContext.Settings.SingleOrDefault(a => a.PropertyField == "DefaultRespParty")).ValueField == "1")
			{
				fee.ResponsiblePartyId = respPartyId;
				newAuditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = null, OldValue = null, NewId = fee.ResponsiblePartyId, NewValue = cwmContext.Parties.First(p => p.PartyID == (respPartyId ?? Guid.Empty)).PartyName });
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					if (newAuditFields.Count() > 0)
					{
						auditService.UpdateAudit(newAudit, newAuditFields);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
					return Guid.Empty;
				}
			}
			else
			{
				logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
				return Guid.Empty;
			}
			ReCalcRatedRangeFee(fee.FeeId);

			return fee.FeeId;
		}

		public Guid DefaultPOTFee(Guid? recordId, Guid feeTypeId, DateTime feeDate, Guid? respPartyId)
		{

			var fee = GetNewDefaultedFee();
			fee.RecordId = recordId;
			fee.FeeTypeId = feeTypeId;
			fee.FeeDate = Convert.ToDateTime(feeDate.ToShortDateString());

			var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD CREATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			string[,] PercoTotFeeTypes;
			string[,] PercoTotFees = null;

			var feeTypePT = cwmContext.FeeTypePTs.Where(pt => pt.BaseFeeTypeId == feeTypeId);

			if (feeTypePT.Count() == 1 && feeTypePT.First().FeeTypeId == null)
			{
				PercoTotFeeTypes = new string[1, 0];
				PercoTotFeeTypes[0, 0] = "All";
				PercoTotFeeTypes[1, 0] = feeTypePT.First().Percentage;

				var fees = cwmContext.Fees.Where(f => f.RecordId == recordId && !cwmContext.FeeTypes.Where(ft => ft.TotalPercent == true || ft.Penalty == true).Any(Item => Item.FeeTypeId == f.FeeTypeId));

				PercoTotFees = new string[2, fees.Count() - 1];

				for (int intI = 0; intI < fees.Count() - 1; intI++)
				{
					PercoTotFees[0, intI] = fees.ElementAt(intI).FeeId.ToString();
					PercoTotFees[1, intI] = fees.ElementAt(intI).FeeTypeId.ToString();
					PercoTotFees[2, intI] = fees.ElementAt(intI).FeeAmt.ToString();
				}
			}

			else if (feeTypePT.Count() > 1 || feeTypePT.First().FeeTypeId != null)
			{
				PercoTotFeeTypes = new string[1, feeTypePT.Count() - 1];
				for (int intI = 0; intI < feeTypePT.Count() - 1; intI++)
				{
					PercoTotFeeTypes[0, 0] = feeTypePT.ElementAt(intI).FeeTypeId.ToString();
					PercoTotFeeTypes[1, 0] = feeTypePT.ElementAt(intI).Percentage;
				}


				var fees = cwmContext.Fees.Where(f => f.RecordId == recordId &&
							cwmContext.FeeTypePTs.Any(pt => pt.BaseFeeTypeId == feeTypeId) &&
							!cwmContext.FeeTypes.Where(ft => ft.TotalPercent == true || ft.Penalty == true).Any(Item => Item.FeeTypeId == f.FeeTypeId));

				PercoTotFees = new string[2, fees.Count() - 1];

				for (int intI = 0; intI < fees.Count() - 1; intI++)
				{
					PercoTotFees[0, intI] = fees.ElementAt(intI).FeeId.ToString();
					PercoTotFees[1, intI] = fees.ElementAt(intI).FeeTypeId.ToString();
					PercoTotFees[2, intI] = fees.ElementAt(intI).FeeAmt.ToString();
				}
			}

			for (int intI = 0; intI < PercoTotFees.GetUpperBound(1); intI++)
			{
				if (PercoTotFees[0, intI] != null)
				{
					var feePT = cwmContext.FeesPTs.Add(new FeesPT());
					feePT.BaseFeeId = fee.FeeId;
					feePT.FeeId = Guid.Parse(PercoTotFees[0, intI]);
					feePT.FeesPTId = Guid.NewGuid();
					feePT.rowguid = Guid.NewGuid();
					feePT.DateInserted = DateTime.Now;
					feePT.DateUpdated = feePT.DateInserted;

					if (cwmContext is DbContext)
					{
						try
						{
							((DbContext)cwmContext).SaveChanges();
							if (newAuditFields.Count() > 0)
							{
								var ptAudit = new AuditModel { TableName = "FeesPT", RecordId = fee.FeeId, AuditAction = "RECORD UPDATED", Description = "" };
								var ptAuditFields = new List<AuditFieldModel>();
								ptAuditFields.Add(new AuditFieldModel { ControlName = "FeeId", FieldDesc = "Edit FeeId", OldId = null, OldValue = null, NewId = feePT.FeeId, NewValue = null });
								auditService.UpdateAudit(ptAudit, ptAuditFields);
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
							return Guid.Empty;
						}
					}
					else
					{
						logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
						return Guid.Empty;
					}
				}
			}

			newAuditFields.Add(new AuditFieldModel { ControlName = "lblNumber", FieldDesc = "RecordId", OldId = null, OldValue = null, NewId = recordId, NewValue = null });
			newAuditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = null, NewId = null, NewValue = fee.FeeDate.Value.ToShortDateString() });
			newAuditFields.Add(new AuditFieldModel { ControlName = "cmbFeeType", FieldDesc = "FeeTypeId", OldId = null, OldValue = null, NewId = fee.FeeTypeId, NewValue = cwmContext.FeeTypes.First(ft => ft.FeeTypeId == fee.FeeTypeId).FeeType1 });

			if ((cwmContext.Settings.SingleOrDefault(a => a.PropertyField == "DefaultRespParty")).ValueField == "1")
			{
				fee.ResponsiblePartyId = respPartyId;
				newAuditFields.Add(new AuditFieldModel { ControlName = "cboParty", FieldDesc = "ResponsiblePartyId", OldId = null, OldValue = null, NewId = fee.ResponsiblePartyId, NewValue = cwmContext.Parties.First(p => p.PartyID == (respPartyId ?? Guid.Empty)).PartyName });
			}

			if (cwmContext is DbContext)
			{
				try
				{
					((DbContext)cwmContext).SaveChanges();
					if (newAuditFields.Count() > 0)
					{
						auditService.UpdateAudit(newAudit, newAuditFields);
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
					return Guid.Empty;
				}
			}
			else
			{
				logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
				return Guid.Empty;
			}

			ReCalcPercoTotFee(fee.FeeId);

			return fee.FeeId;
		}

		public void ReCalcRatedFee(Guid feeId, Guid? feeSchedId = null, bool isDefault = false)
		{
			decimal feeBase = 0;
			decimal Count = 0;
			decimal feeAmt = 0;
			string feeDesc = "";

			Data.Fee oldFee;

			var fee = cwmContext.Fees.FirstOrDefault(f => f.FeeId == feeId);

			oldFee = fee;


			IQueryable<FeeSchedule> feeScheds;

			if (fee.FeeId != null && fee.FeeId != Guid.Empty)
			{
				if (feeSchedId != null && feeSchedId != Guid.Empty)
				{
					feeScheds = cwmContext.FeeSchedules.Where(fs => fs.FeeSchedId == feeSchedId);
				}
				else
				{
					feeScheds = cwmContext.FeeSchedules.Where(fs => fs.FeeTypeId == fee.FeeTypeId);
				}

				if (feeScheds.Count() > 0)
				{
					foreach (FeeSchedule feeSched in feeScheds)
					{
						if (feeSched.FeeItem.CompareTo(fee.FeeUOM) == 0)
						{
							if (feeSched.FeeRate != 0)
							{
								feeBase = feeSched.FeeRate;
							}
							else
							{
								feeBase = fee.FeeBase ?? 0;
							}

							if (feeSched.UserDefFieldId == null)
							{
								Count = fee.Units ?? 0;
							}
							else
							{
								var udfValue = cwmContext.UserDefValues.FirstOrDefault(udf => udf.RecordId == fee.RecordId && udf.UserDefFieldId == feeSched.UserDefFieldId);
								if (udfValue.UserDefValue1 != null)
								{
									Count = Convert.ToDecimal(udfValue.UserDefValue1);
								}
								else
								{
									Count = fee.Units ?? 0;
								}
							}
						}

					}
				}
			}
			if (isDefault && Count == 0)
			{
				Count = 1;
			}

			if (feeBase != 0 && Count != 0)
			{
				feeAmt = feeBase * Count;
			}

			feeDesc = Count.ToString("0") + " " + fee.FeeUOM.Trim() + " x " + feeBase.ToString().Trim();

			if (feeAmt != 0)
			{
				fee.FeeAmt = feeAmt;
				fee.FeeBase = feeBase;
				fee.Units = Count;
				fee.FeeDesc = feeDesc;

				var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD UPDATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				newAuditFields.Add(new AuditFieldModel { ControlName = "txtFee1", FieldDesc = "FeeAmt", OldId = null, OldValue = oldFee.FeeAmt.ToString(), NewId = null, NewValue = fee.FeeAmt.ToString() });
				newAuditFields.Add(new AuditFieldModel { ControlName = "txtFeeBase", FieldDesc = "FeeBase", OldId = null, OldValue = oldFee.FeeBase.ToString(), NewId = null, NewValue = fee.FeeBase.ToString() });
				newAuditFields.Add(new AuditFieldModel { ControlName = "txtCount", FieldDesc = "Units", OldId = null, OldValue = oldFee.Units.ToString(), NewId = null, NewValue = fee.Units.ToString() });

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						if (newAuditFields.Count() > 0)
						{
							auditService.UpdateAudit(newAudit, newAuditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
						return;
					}
				}
				else
				{
					logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
					return;
				}
			}

		}

		public void ReCalcRatedRangeFee(Guid feeId)
		{
			string moduleType;
			Guid recordid;
			string tempUDFValue = "";
			FeeTypeRR thisTypeRR = null;
			FeeTypeRR preTypeRR = null;
			decimal rrBase;
			decimal rrRatePer;
			decimal rrFeeAmount = 0;
			string feeDesc = "";

			Data.Fee oldFee;

			var fee = cwmContext.Fees.FirstOrDefault(f => f.FeeId == feeId);



			if (fee.FeeId != null && fee.FeeId != Guid.Empty)
			{
				oldFee = fee;
				var feeTypeRRs = cwmContext.FeeTypeRRs.Where(rr => rr.FeeTypeId == fee.FeeTypeId);
				foreach (FeeTypeRR feeTypeRR in feeTypeRRs)
				{
					if (feeTypeRR.UserDefFieldId != null && feeTypeRR.UserDefFieldId != Guid.Empty)
					{
						moduleType = udfService.GetUDFModuleType(feeTypeRR.UserDefFieldId ?? Guid.Empty);
						recordid = udfService.GetFeeUDFRecordId(moduleType, fee.RecordId ?? Guid.Empty);
						tempUDFValue = udfService.GetUDFValue(feeTypeRR.UserDefFieldId ?? Guid.Empty, recordid);
						if (tempUDFValue == null || tempUDFValue == "")
						{
							tempUDFValue = "0";
						}

						if (feeTypeRR.AmountTo == null)
						{
							if (Convert.ToDouble(tempUDFValue) >= Convert.ToDouble(feeTypeRR.AmountFrom))
							{
								thisTypeRR = feeTypeRR;
								break;
							}
						}
						else
						{
							if ((Convert.ToDouble(tempUDFValue) >= Convert.ToDouble(feeTypeRR.AmountFrom)) && (Convert.ToDouble(tempUDFValue) <= Convert.ToDouble(feeTypeRR.AmountTo)))
							{
								thisTypeRR = feeTypeRR;
								break;
							}
						}
					}
					preTypeRR = feeTypeRR;
				}

				if (thisTypeRR != null)
				{
					if (thisTypeRR.AmountTo == null)
					{
						rrBase = preTypeRR.Base + (thisTypeRR.Base * ((Convert.ToDecimal(tempUDFValue) - preTypeRR.AmountFrom) / thisTypeRR.AmountEvery ?? 1));
					}
					else
					{
						rrBase = thisTypeRR.RatePer ?? 0;
					}
					if (thisTypeRR.RatePer == null)
					{
						rrRatePer = 0;
					}
					else
					{
						rrRatePer = thisTypeRR.RatePer ?? 0;
					}
					if (thisTypeRR.AmountPer == null)
					{
						rrFeeAmount = rrBase + (rrRatePer * Convert.ToDecimal(tempUDFValue));
					}
					else
					{
						switch (thisTypeRR.RoundOption)
						{
							case 0:
								rrFeeAmount = rrBase + (rrRatePer * ((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer)));
								break;
							case 1:
								rrFeeAmount = (int)(rrBase + (rrRatePer * ((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer))));
								break;
							case 2:
								decimal tempAmount;
								if (((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer)) > (int)((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer)))
								{
									tempAmount = (int)((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer)) + 1;
								}
								else
								{
									tempAmount = (int)((Convert.ToDecimal(tempUDFValue) - Convert.ToDecimal(thisTypeRR.AmountFrom)) / Convert.ToDecimal(thisTypeRR.AmountPer));

								}

								rrFeeAmount = rrBase + (rrRatePer * tempAmount);
								break;
							default:
								break;
						}

					}
					feeDesc = thisTypeRR.Description + ": " + tempUDFValue + " - " + rrBase.ToString("c");

					if (rrRatePer != 0)
					{
						feeDesc += " + " + rrRatePer.ToString("c") + " per";

					}
				}
				var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD UPDATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				fee.FeeAmt = Convert.ToDecimal(rrFeeAmount.ToString("0.00"));
				fee.FeeBase = null;
				fee.Units = null;
				fee.FeeUOM = null;
				fee.FeeDesc = feeDesc;

				newAuditFields.Add(new AuditFieldModel { ControlName = "txtRRFeeAmount", FieldDesc = "FeeAmt", OldId = null, OldValue = oldFee.FeeAmt.ToString(), NewId = null, NewValue = fee.FeeAmt.ToString() });
				newAuditFields.Add(new AuditFieldModel { ControlName = "cboPer", FieldDesc = "FeeUOM", OldId = null, OldValue = oldFee.FeeUOM, NewId = null, NewValue = fee.FeeUOM });
				newAuditFields.Add(new AuditFieldModel { ControlName = "txtRRUDFValue", FieldDesc = "Units", OldId = null, OldValue = oldFee.Units.ToString(), NewId = null, NewValue = fee.Units.ToString() });

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						if (newAuditFields.Count() > 0)
						{
							auditService.UpdateAudit(newAudit, newAuditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
						return;
					}
				}
				else
				{
					logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
					return;
				}
			}

			return;

		}

		public void ReCalcPercoTotFee(Guid feeId)
		{

			decimal feesTotal = 0;
			decimal percentOfFees = 0;
			string feeDesc = "";
			decimal firstPercent = 0;
			bool samePercent = true;

			Data.Fee oldFee;

			var fee = cwmContext.Fees.FirstOrDefault(f => f.FeeId == feeId);
			var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD CREATED", Description = "" };
			var newAuditFields = new List<AuditFieldModel>();

			string[,] PercoTotFeeTypes = null;
			string[,] PercoTotFees = null;

			oldFee = fee;

			if (fee.FeeId != null && fee.FeeId != Guid.Empty)
			{
				var feeTypePT = cwmContext.FeeTypePTs.Where(pt => pt.BaseFeeTypeId == fee.FeeTypeId);
				if (feeTypePT.Count() == 1 && feeTypePT.First().FeeTypeId == null)
				{
					PercoTotFeeTypes = new string[1, 0];
					PercoTotFeeTypes[0, 0] = "All";
					PercoTotFeeTypes[1, 0] = feeTypePT.First().Percentage;

					var fees = cwmContext.Fees.Where(f => cwmContext.FeesPTs.Any(fp => fp.BaseFeeId == f.FeeId) && !cwmContext.FeeTypes.Where(ft => ft.TotalPercent == true || ft.Penalty == true).Any(Item => Item.FeeTypeId == f.FeeTypeId));

					PercoTotFees = new string[2, fees.Count() - 1];

					for (int intI = 0; intI < fees.Count() - 1; intI++)
					{
						PercoTotFees[0, intI] = fees.ElementAt(intI).FeeId.ToString();
						PercoTotFees[1, intI] = fees.ElementAt(intI).FeeTypeId.ToString();
						PercoTotFees[2, intI] = fees.ElementAt(intI).FeeAmt.ToString();
					}
				}

				else if (feeTypePT.Count() > 1 || feeTypePT.First().FeeTypeId != null)
				{
					PercoTotFeeTypes = new string[1, feeTypePT.Count() - 1];
					for (int intI = 0; intI < feeTypePT.Count() - 1; intI++)
					{
						PercoTotFeeTypes[0, 0] = feeTypePT.ElementAt(intI).FeeTypeId.ToString();
						PercoTotFeeTypes[1, 0] = feeTypePT.ElementAt(intI).Percentage;
						if (intI == 0)
						{
							firstPercent = Convert.ToDecimal(PercoTotFeeTypes[1, intI]);
						}
						else
						{
							if (Convert.ToDecimal(PercoTotFeeTypes[1, intI]) != firstPercent)
							{
								samePercent = false;

							}

						}
					}
					var fees = cwmContext.Fees.Where(f => cwmContext.FeesPTs.Any(pt => pt.BaseFeeId == fee.FeeId) &&
								!cwmContext.FeeTypes.Where(ft => ft.TotalPercent == true || ft.Penalty == true).Any(Item => Item.FeeTypeId == f.FeeTypeId));

					PercoTotFees = new string[2, fees.Count() - 1];

					for (int intI = 0; intI < fees.Count() - 1; intI++)
					{
						PercoTotFees[0, intI] = fees.ElementAt(intI).FeeId.ToString();
						PercoTotFees[1, intI] = fees.ElementAt(intI).FeeTypeId.ToString();
						PercoTotFees[2, intI] = fees.ElementAt(intI).FeeAmt.ToString();
					}
				}

				for (int intJ = 0; intJ < PercoTotFeeTypes.GetUpperBound(1); intJ++)
				{
					for (int intI = 0; intI < PercoTotFees.GetUpperBound(1); intI++)
					{
						if ((PercoTotFees[1, intI] == PercoTotFeeTypes[0, intJ]) || PercoTotFeeTypes[0, intJ] == "All")
						{
							if (PercoTotFees[2, intI] != "")
							{
								feesTotal += Convert.ToDecimal(PercoTotFees[2, intI]);

								percentOfFees += (Convert.ToDecimal(PercoTotFees[2, intI]) * (Convert.ToDecimal(PercoTotFeeTypes[1, intJ]) / 100));
							}

						}

					}
				}

				percentOfFees = Convert.ToDecimal(percentOfFees.ToString("0.00"));

				if (!samePercent)
				{

					feeDesc = feesTotal.ToString() + " @ variable percentage";
				}
				else
				{
					feeDesc = feesTotal.ToString() + " @ " + PercoTotFeeTypes[1, 0].ToString() + "%";
				}

				fee.FeeAmt = percentOfFees;
				fee.FeeDesc = feeDesc;
				newAuditFields.Add(new AuditFieldModel { ControlName = "txtPTFeeAmount", FieldDesc = "FeeAmt", OldId = null, OldValue = oldFee.FeeAmt.ToString(), NewId = null, NewValue = fee.FeeAmt.ToString() });
				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChanges();
						if (newAuditFields.Count() > 0)
						{
							auditService.UpdateAudit(newAudit, newAuditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
						return;
					}
				}
				else
				{
					logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
					return;
				}
			}
		}

		public void ReCalcPenaltyFee(Guid feeId)
		{
			string moduleType;
			Guid recordid = Guid.Empty;
			string tempUDFValue = "";
			string feeDesc = "";

			decimal penBase = 0;
			int penNODays = 0;
			decimal penDaily = 0;
			int penPlusDays = 0;
			decimal tempPenalty = 0;
			int daysPast = 0;
			int plusDays = 0;

			Data.Fee oldFee;

			var fee = cwmContext.Fees.FirstOrDefault(f => f.FeeId == feeId);

			oldFee = fee;

			if (fee.FeeId != null && fee.FeeId != Guid.Empty)
			{
				var feePenType = cwmContext.FeeTypePens.FirstOrDefault(ftp => ftp.FeeTypeId == fee.FeeTypeId);
				if (feePenType.FeeTypePenId != Guid.Empty)
				{
					penBase = feePenType.InitialPenalty;
					penNODays = feePenType.NumberOfDays;
					penDaily = feePenType.RatePer ?? 0;
					penPlusDays = feePenType.AmountPer ?? 0;

					if (feePenType.UserDefFieldId != null && feePenType.UserDefFieldId != Guid.Empty)
					{
						moduleType = udfService.GetUDFModuleType(feePenType.UserDefFieldId ?? Guid.Empty);

						if (moduleType == "Fee")
						{
							recordid = fee.FeeId;

						}
						if (moduleType == "Permit")
						{
							recordid = fee.RecordId ?? Guid.Empty;

						}
						tempUDFValue = udfService.GetUDFValue(feePenType.UserDefFieldId ?? Guid.Empty, recordid);
					}
				}

				if (tempUDFValue != "")
				{
					daysPast = DateTime.Now.Subtract(Convert.ToDateTime(tempUDFValue)).Days;


					if (daysPast >= penNODays)
					{
						plusDays = daysPast - (penNODays);
						tempPenalty = penBase + (penDaily * (plusDays / penPlusDays));
						feeDesc = "Total Days: " + daysPast + "  Penalty: " + penBase + " plus " + penDaily + " every " + penPlusDays + " days x " + plusDays + " days";
					}
					else
					{
						feeDesc = "No Penalty";

					}
					tempPenalty = Convert.ToDecimal(tempPenalty.ToString("0.00"));


					var newAudit = new AuditModel { TableName = "Fees", RecordId = fee.FeeId, AuditAction = "RECORD UPDATED", Description = "" };
					var newAuditFields = new List<AuditFieldModel>();

					fee.FeeAmt = Convert.ToDecimal(tempPenalty.ToString("0.00"));
					fee.FeeDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
					fee.FeeBase = null;
					fee.Units = null;
					fee.FeeUOM = null;
					fee.ReCalcDate = fee.FeeDate;
					fee.FeeDesc = feeDesc;

					if (oldFee.FeeAmt != fee.FeeAmt)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "txtPenFeeAmount", FieldDesc = "FeeAmt", OldId = null, OldValue = oldFee.FeeAmt.ToString(), NewId = null, NewValue = fee.FeeAmt.ToString() });
					}
					if (oldFee.FeeDate != fee.FeeDate)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "dtFeeDate", FieldDesc = "FeeDate", OldId = null, OldValue = oldFee.FeeDate.ToString(), NewId = null, NewValue = fee.FeeDate.ToString() });
					}

					if (cwmContext is DbContext)
					{
						try
						{
							((DbContext)cwmContext).SaveChanges();
							if (newAuditFields.Count() > 0)
							{
								auditService.UpdateAudit(newAudit, newAuditFields);
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to save fee '" + fee.FeeId.ToString() + "'.", ex);
							return;
						}
					}
					else
					{
						logger.Error("Unable to update fee '" + fee.FeeId.ToString() + "', DbContext was not available.");
						return;
					}


				}

			}
		}

		public void RecalculateFees(Guid recordId, bool parentIsComplete)
		{
			var fees = cwmContext.Fees.Join(
						cwmContext.FeeTypes, f => f.FeeTypeId, ft => ft.FeeTypeId, (f, ft) => new { f, ft }).Where(fft => fft.f.RecordId == recordId && fft.f.FeeStatus == 0).OrderByDescending(fft => fft.ft.Rate).ThenByDescending(fft => fft.ft.RatedRange).ThenByDescending(fft => fft.ft.TotalPercent);

			if (fees.Count() > 0)
			{
				foreach (var fee in fees)
				{
					if (fee.ft.Rate)
					{
						ReCalcRatedFee(fee.f.FeeId);
					}
					else if (fee.ft.RatedRange)
					{
						ReCalcRatedRangeFee(fee.f.FeeId);
					}
					else if (fee.ft.TotalPercent)
					{
						ReCalcPercoTotFee(fee.f.FeeId);
					}
					if (fee.ft.Penalty)
					{
						if (!parentIsComplete)
						{
							ReCalcPenaltyFee(fee.f.FeeId);
						}
					}

				}
			}

		}

		private string GetFeeDescription(DetailedFee fee)
		{
			string retval = null;
			var feeType = cwmContext.FeeTypes.FirstOrDefault(ft => ft.FeeTypeId == fee.FeeTypeId);
			try
			{
				switch (true)
				{
					case object _ when feeType.Rate:
						{
							if (feeType.InvItemId != null && feeType.InvItemId != Guid.Empty)
								retval = fee.Units + " x " + fee.FeeBase;
							else
								retval = fee.Units + " " + fee.FeeUOM.Trim() + " x " + fee.FeeBase.ToString().Trim();
							break;
						}
					case object _ when feeType.RatedRange:
						{
							var typeRR = cwmContext.FeeTypeRRs.FirstOrDefault(rr => rr.FeeTypeId == fee.FeeTypeId);
							string strRRUDFValue = udfService.GetUDFValue(typeRR.UserDefFieldId ?? Guid.Empty, fee.RecordId);
							retval = fee.FeeUOM.Trim() + ": " + String.Format("{0:C}", fee.Units.ToString().Trim()) + " - " + String.Format("{0:C}", typeRR.Base.ToString().Trim());
							if (typeRR.RatePer.ToString().Trim() != "" && System.Convert.ToDouble(typeRR.RatePer.ToString().Trim()) != 0)
								retval += " + " + String.Format("{0:C}", typeRR.RatePer.ToString().Trim()) + " per";
							break;
						}
					case object _ when feeType.TotalPercent:
						{
							var typePTs = cwmContext.FeeTypePTs.Where(pt => pt.FeeTypeId == fee.FeeTypeId);
							var FeesPT = cwmContext.FeesPTs.Where(fpt => fpt.BaseFeeId == fee.FeeId);
							decimal feeTotal = 0;
							foreach (var item in FeesPT)
							{
								feeTotal = feeTotal + cwmContext.Fees.First(f => f.FeeId == item.FeeId).FeeAmt ?? 0;
							}
							bool sameper = true;
							if (typePTs.Count() > 1)
							{

								string lstPer = typePTs.First().Percentage;
								foreach (var item in typePTs)
								{
									if (item.Percentage != lstPer)
									{
										sameper = false;
									}
								}
							}
							if (sameper == false)
								retval = String.Format("{0:C}", feeTotal.ToString().Trim()) + " @ variable percentage";
							else
								retval = String.Format("{0:C}", feeTotal.ToString().Trim()) + " @ " + typePTs.First().Percentage.Trim() + "%";
							break;
						}
					case object _ when feeType.Penalty:
						{
							int DaysPast;
							int PlusDays;

							// Dim strPenUDFDate As Date
							var penPT = GetFeeTypePenAsync(fee.FeeTypeId).Result;
							string strPenUDFDate = udfService.GetUDFValue(penPT.UserDefFieldId ?? Guid.Empty, fee.RecordId);
							if (!(strPenUDFDate == ""))
							{
								DaysPast = (fee.FeeDate ?? DateTime.Now).Subtract(Convert.ToDateTime(strPenUDFDate)).Days;
								// DaysPast = CDate(dtFeeDate.Value).Subtract(CDate(txtPenUDFDate.Text)).Days
								if (DaysPast >= penPT.NumberOfDays)
								{
									PlusDays = DaysPast - penPT.NumberOfDays;
									retval = "Total Days: " + DaysPast + "  Penalty: " + String.Format("{0:C}", fee.FeeBase.ToString().Trim()) + " plus " + String.Format("{0:C}", penPT.RatePer.ToString().Trim()) + " every " + penPT.AmountPer.ToString().Trim() + " days x " + PlusDays + " days";
								}
								else
									retval = "No Penalty";
							}

							break;
						}

					default:
						{
							retval = "";
							break;
						}
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unable to create fee description '" + fee.FeeId.ToString() + "'.", ex);
				return null;
			}
			return retval;
		}

	}

}

