using NMSFM.Data;
using NMSFM.Services.Audit;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using NMSFM.Services.CPSystem;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace NMSFM.Services.UDF
{
	public enum CPUDFModules
	{
		Activities,
		Addresses,
		Agency,
		Fees,
		InspectionDetails,
		Invoices,
		Items,
		Mileage,
		Party,
		Permits,
		Projects,
		Requests
	}
	public class UDFService : IUDFService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;
		private ISystemService systemService;

		public UDFService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
			systemService = new SystemService(codepalWebModel, codepalLogger);
		}

		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByAddressIdAsync(Guid id, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			var AddressTypeId = new Guid();
			var ModuleId = new Guid();
			try
			{
				v_Addresses2 address = null;
				address = await cwmContext.v_Addresses2.SingleOrDefaultAsync(a => a.AddressId == id);
				AddressTypeId = address.AddressTypeId ?? Guid.Empty;
				Guid AgencyId = new Guid("9808204F-D941-451E-B121-02C8A0D7E7FA");
				if (agency != null && agency != Guid.Empty)
				{
					AgencyId = agency;
				}
				Module module = null;
				module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId ?? Guid.Empty) == AgencyId && a.ModuleDesc == "Address");
				ModuleId = module.ModuleId;

				var models = from cats in cwmContext.UserDefCategories
							 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = AddressTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
							 from usecat in subcat.DefaultIfEmpty()
							 where ((((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == AddressTypeId) || ((cats.ModuleId == null && cats.AllAgency == "add") && (usecat.TypeId == AddressTypeId || cats.AllModuleTypes == true)) || ((cats.ModuleId.Equals(null) ? Guid.Empty : cats.ModuleId.Value) == ModuleId && cats.AllModuleTypes == true) || usecat.TypeId == AddressTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
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
								 SequenceNumber = cats.SeqNum ?? 0,
								 FieldSequenceNumber = flds.SeqNum ?? 0,
								 WebViewable = (cats.WebViewable ?? false) //&& flds.WebViewable,
							 };

				var resolutionResults = await models.ToListAsync();

				for (int i = 0; i < resolutionResults.Count(); i++)
				{
					resolutionResults[i].Resolutions = new List<Resolution>();
					if (resolutionResults[i].FieldType == new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488")) // Check Box
					{
						var fieldId = resolutionResults[i].FieldId;
						resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
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
					else if (resolutionResults[i].FieldType == new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E")) // List
					{
						var fieldId = resolutionResults[i].FieldId;
						resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
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

		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByActivityIdAsync(Guid id, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			var activityTypeId = new Guid();
			var ModuleId = new Guid();
			//int intI = 0;
			try
			{
				v_Activities activity = null;
				activity = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
				if (activity != null)
				{
					activityTypeId = activity.InspectionTypeId ?? Guid.Empty;
					Guid AgencyId = new Guid("9808204F-D941-451E-B121-02C8A0D7E7FA");
					//intI = 1;

					if (agency != null && agency != Guid.Empty)
					{
						AgencyId = agency;
					}
					Module module = null;
					module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId ?? Guid.Empty) == AgencyId && a.ModuleDesc == "Activity");
					ModuleId = module.ModuleId;
					//intI = 2;
					var models = from cats in cwmContext.UserDefCategories
								 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = activityTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
								 from usecat in subcat.DefaultIfEmpty()
								 where ((((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == activityTypeId) || ((cats.ModuleId == null && cats.AllAgency == "act") && (usecat.TypeId == activityTypeId || cats.AllModuleTypes == true)) || ((cats.ModuleId.Equals(null) ? Guid.Empty : cats.ModuleId.Value) == ModuleId && cats.AllModuleTypes == true) || usecat.TypeId == activityTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
								 join flds in cwmContext.UserDefFields on cats.UserDefCategoryId equals flds.UserDefCategoryId
								 join vals in cwmContext.UserDefValues on new { id = flds.UserDefFieldId, ad = id } equals new { id = vals.UserDefFieldId, ad = vals.RecordId } into subvals
								 from usevals in subvals.DefaultIfEmpty()
								 where flds.Inactive == false
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
									 SequenceNumber = cats.SeqNum ?? 0,
									 FieldSequenceNumber = flds.SeqNum ?? 0,
									 WebViewable = (cats.WebViewable ?? false), //&& flds.WebViewable,
									 StaticCombo = flds.StaticCombo,
									 Required = flds.Required,
									 DefaultValue = flds.DefaultValue,
									 Persistent = flds.GlobalId != null,
								 };
					//intI = 3;
					var resolutionResults = await models.ToListAsync();
					//intI = 4;

					bool noValues = resolutionResults.Count(rr => rr.ValueId != null && rr.ValueId != Guid.Empty) == 0;

					for (int i = 0; i < resolutionResults.Count(); i++)
					{
						if (noValues)
						{
							if (resolutionResults[i].Persistent)
							{
								string oSQL = "";
								Guid PreviousObjectId;
								object[] parameters = { };
								//intI = 5;
								oSQL = GetPersistentValue(CPUDFModules.Activities, id, resolutionResults[i].CategoryId, resolutionResults[i].FieldId, activity.AddressId ?? Guid.Empty);
								//intI = 6;

								ICodepalWebModel resoContext1 = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
								PreviousObjectId = await ((DbContext)resoContext1).Database.SqlQuery<Guid>(oSQL, parameters).FirstOrDefaultAsync();

								if (PreviousObjectId != null && PreviousObjectId != Guid.Empty)
								{
									//intI = 7;
									//var tmpUserDef = await cwmContext.UserDefValues.Where(udf => udf.RecordId == PreviousObjectId && udf.UserDefFieldId == resolutionResults[i].FieldId).FirstAsync();'

									IEnumerable<UserDefValue> userDefinedValues = await resoContext1.UserDefValues.Where(udf => udf.RecordId == PreviousObjectId).ToListAsync();

									UserDefValue userDefined = userDefinedValues.Where(udf => udf.UserDefFieldId == resolutionResults[i].FieldId).FirstOrDefault();

									if (userDefined != null)
									{
										if (userDefined.UserDefValue1 == null)
										{
											resolutionResults[i].PersistentValue = "";
										}
										else
										{
											resolutionResults[i].PersistentValue = userDefined.UserDefValue1;


											if (resolutionResults[i].Persistent && (resolutionResults[i].PersistentValue != null && resolutionResults[i].PersistentValue != ""))
											{
												resolutionResults[i].FieldValue = resolutionResults[i].PersistentValue;
											}
										}
									}
								}
							}
							if ((resolutionResults[i].FieldValue == null || resolutionResults[i].FieldValue == "") && (resolutionResults[i].DefaultValue != null && resolutionResults[i].DefaultValue != ""))
							{
								resolutionResults[i].FieldValue = resolutionResults[i].DefaultValue;
							}
						}
						//intI = 8;
						resolutionResults[i].Resolutions = new List<Resolution>();
						//intI = 9;
						if (resolutionResults[i].FieldType == new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488")) // Check Box
						{

							var fieldId = resolutionResults[i].FieldId;
							//intI = 10;
							ICodepalWebModel resoContext = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
							resolutionResults[i].Resolutions = await resoContext.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
							if (resolutionResults[i].Resolutions != null && resolutionResults[i].Resolutions.Count() > 0)
							{
								//intI = 11;
								resolutionResults[i].boolValue = new List<bool>();
								for (int j = 0; j < resolutionResults[i].Resolutions.Count(); j++)
								{
									//intI = 12;
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
						else if (resolutionResults[i].FieldType == new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E")) // List
						{
							//intI = 13;
							//Debug.Assert(i != 57);
							var fieldId = resolutionResults[i].FieldId;
							//intI = 12;
							ICodepalWebModel resoContext2 = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
							var resoluts = await resoContext2.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
							//intI = 14;
							resolutionResults[i].Resolutions = resoluts;
							//intI = 15;
							var linkedResolution = resoluts.Find(a => a.Resolution1 == resolutionResults[i].FieldValue);
							if (linkedResolution != null)
							{
								resolutionResults[i].ResolutionId = linkedResolution.ResolutionId;
							}
						}


					}
					//intI = 16;
					results = resolutionResults;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				//MessageBox.Show(intI.ToString());
				logger.Error("Unexpected exception caught while retrieving user defined values for activity '" + id.ToString() + "'.", ex);
			}

			return results;
		}

		public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByFeeIdAsync(Guid id, Guid agency)
		{
			IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
			var feeTypeId = new Guid();
			var ModuleId = new Guid();
			int intI = 0;
			try
			{
				v_Fees fee = null;
				fee = await cwmContext.v_Fees.SingleOrDefaultAsync(a => a.FeeId == id);
				if (fee != null)
				{
					feeTypeId = fee.FeeTypeId ?? Guid.Empty;
					Guid AgencyId = new Guid("9808204F-D941-451E-B121-02C8A0D7E7FA");
					intI = 1;

					if (agency != null && agency != Guid.Empty)
					{
						AgencyId = agency;
					}
					Module module = null;
					module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId ?? Guid.Empty) == AgencyId && a.ModuleDesc == "Fee");
					ModuleId = module.ModuleId;
					intI = 2;
					var models = from cats in cwmContext.UserDefCategories
								 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = feeTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
								 from usecat in subcat.DefaultIfEmpty()
								 where ((((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == feeTypeId) || ((cats.ModuleId == null && cats.AllAgency == "fee") && (usecat.TypeId == feeTypeId || cats.AllModuleTypes == true)) || ((cats.ModuleId.Equals(null) ? Guid.Empty : cats.ModuleId.Value) == ModuleId && cats.AllModuleTypes == true) || usecat.TypeId == feeTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
								 join flds in cwmContext.UserDefFields on cats.UserDefCategoryId equals flds.UserDefCategoryId
								 join vals in cwmContext.UserDefValues on new { id = flds.UserDefFieldId, ad = id } equals new { id = vals.UserDefFieldId, ad = vals.RecordId } into subvals
								 from usevals in subvals.DefaultIfEmpty()
								 where flds.Inactive == false
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
									 SequenceNumber = cats.SeqNum ?? 0,
									 FieldSequenceNumber = flds.SeqNum ?? 0,
									 WebViewable = (cats.WebViewable ?? false), //&& flds.WebViewable,
									 StaticCombo = flds.StaticCombo,
									 Required = flds.Required,
									 DefaultValue = flds.DefaultValue,
									 Persistent = flds.GlobalId != null,
								 };
					intI = 3;
					var resolutionResults = await models.ToListAsync();
					intI = 4;

					bool noValues = resolutionResults.Count(rr => rr.ValueId != null && rr.ValueId != Guid.Empty) == 0;

					for (int i = 0; i < resolutionResults.Count(); i++)
					{
						if (noValues)
						{
							if (resolutionResults[i].Persistent)
							{
								string oSQL = "";
								Guid PreviousObjectId;
								object[] parameters = { };
								intI = 5;
								oSQL = GetPersistentValue(CPUDFModules.Activities, id, resolutionResults[i].CategoryId, resolutionResults[i].FieldId, fee.AddressId ?? Guid.Empty);
								intI = 6;

								ICodepalWebModel resoContext1 = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
								PreviousObjectId = await ((DbContext)resoContext1).Database.SqlQuery<Guid>(oSQL, parameters).FirstOrDefaultAsync();

								if (PreviousObjectId != null && PreviousObjectId != Guid.Empty)
								{
									intI = 7;
									//var tmpUserDef = await cwmContext.UserDefValues.Where(udf => udf.RecordId == PreviousObjectId && udf.UserDefFieldId == resolutionResults[i].FieldId).FirstAsync();'

									IEnumerable<UserDefValue> userDefinedValues = await resoContext1.UserDefValues.Where(udf => udf.RecordId == PreviousObjectId).ToListAsync();

									UserDefValue userDefined = userDefinedValues.Where(udf => udf.UserDefFieldId == resolutionResults[i].FieldId).FirstOrDefault();

									if (userDefined != null)
									{
										if (userDefined.UserDefValue1 == null)
										{
											resolutionResults[i].PersistentValue = "";
										}
										else
										{
											resolutionResults[i].PersistentValue = userDefined.UserDefValue1;


											if (resolutionResults[i].Persistent && (resolutionResults[i].PersistentValue != null && resolutionResults[i].PersistentValue != ""))
											{
												resolutionResults[i].FieldValue = resolutionResults[i].PersistentValue;
											}
										}
									}
								}
							}
							if ((resolutionResults[i].FieldValue == null || resolutionResults[i].FieldValue == "") && (resolutionResults[i].DefaultValue != null && resolutionResults[i].DefaultValue != ""))
							{
								resolutionResults[i].FieldValue = resolutionResults[i].DefaultValue;
							}
						}
						intI = 8;
						resolutionResults[i].Resolutions = new List<Resolution>();
						intI = 9;
						if (resolutionResults[i].FieldType == new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488")) // Check Box
						{

							var fieldId = resolutionResults[i].FieldId;
							intI = 10;
							ICodepalWebModel resoContext = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
							resolutionResults[i].Resolutions = await resoContext.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
							if (resolutionResults[i].Resolutions != null && resolutionResults[i].Resolutions.Count() > 0)
							{
								intI = 11;
								resolutionResults[i].boolValue = new List<bool>();
								for (int j = 0; j < resolutionResults[i].Resolutions.Count(); j++)
								{
									intI = 12;
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
						else if (resolutionResults[i].FieldType == new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E")) // List
						{
							intI = 13;
							//Debug.Assert(i != 57);
							var fieldId = resolutionResults[i].FieldId;
							intI = 12;
							ICodepalWebModel resoContext2 = new CodepalWebModel(HttpContext.Current.Session["userConnection"].ToString());
							var resoluts = await resoContext2.Resolutions.Where(a => (a.ResolutionType ?? Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
							intI = 14;
							resolutionResults[i].Resolutions = resoluts;
							intI = 15;
							var linkedResolution = resoluts.Find(a => a.Resolution1 == resolutionResults[i].FieldValue);
							if (linkedResolution != null)
							{
								resolutionResults[i].ResolutionId = linkedResolution.ResolutionId;
							}
						}


					}
					intI = 16;
					results = resolutionResults;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				MessageBox.Show(intI.ToString());
				logger.Error("Unexpected exception caught while retrieving user defined values for activity '" + id.ToString() + "'.", ex);
			}

			return results;
		}

		//public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByActivityTypeIdAsync(Guid actTypeId, Guid addressId, Guid agency)
		//{
		//	IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
		//	var activityTypeId = new Guid();
		//	var ModuleId = new Guid();
		//	try
		//	{
		//		//v_Activities activity = null;
		//		//activity = await cwmContext.v_Activities.SingleOrDefaultAsync(a => a.InspectionId == id);
		//		//activityTypeId = activity.InspectionTypeId.HasValue ? activity.InspectionTypeId.Value : Guid.Empty;

		//		activityTypeId = actTypeId;

		//		Guid AgencyId = new Guid("9808204F-D941-451E-B121-02C8A0D7E7FA");

		//		if (agency != null && agency != Guid.Empty)
		//		{
		//			AgencyId = agency;
		//		}
		//		Module module = null;
		//		module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Activity");
		//		ModuleId = module.ModuleId;

		//		var models = from cats in cwmContext.UserDefCategories
		//					 join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = activityTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
		//					 from usecat in subcat.DefaultIfEmpty()
		//					 where ((((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == activityTypeId) || ((cats.ModuleId == null && cats.AllAgency == "act") && (usecat.TypeId == activityTypeId || cats.AllModuleTypes == true)) || ((cats.ModuleId.Equals(null) ? Guid.Empty : cats.ModuleId.Value) == ModuleId && cats.AllModuleTypes == true) || usecat.TypeId == activityTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
		//					 join flds in cwmContext.UserDefFields on cats.UserDefCategoryId equals flds.UserDefCategoryId into subvals							 
		//					 from usevals in subvals.DefaultIfEmpty()
		//					 select new UserDefinedValue
		//					 {
		//						 Category = cats.Category,
		//						 CategoryId = cats.UserDefCategoryId,
		//						 FieldDescription = usevals.FieldDesc,								 
		//						 FieldId = usevals.UserDefFieldId,
		//						 FieldType = usevals.UserDefTypeId,
		//						 SequenceNumber = cats.SeqNum.HasValue ? cats.SeqNum.Value : 0,
		//						 FieldSequenceNumber = usevals.SeqNum.HasValue ? usevals.SeqNum.Value : 0,
		//						 WebViewable = (cats.WebViewable.HasValue ? cats.WebViewable.Value : false), //&& flds.WebViewable,
		//						 StaticCombo = usevals.StaticCombo,
		//						 Required = usevals.Required,
		//						 DefaultValue = usevals.DefaultValue,
		//						 Persistent = usevals.GlobalId != null,
		//					 };

		//		var resolutionResults = await models.ToListAsync();

		//		for (int i = 0; i < resolutionResults.Count(); i++)
		//		{
		//			if (resolutionResults[i].Persistent)
		//			{
		//				resolutionResults[i].PersistentValue = GetPersistentValue(CPUDFModules.Activities, id, resolutionResults[i].CategoryId, resolutionResults[i].FieldId, activity.AddressId ?? Guid.Empty);
		//			}
		//			resolutionResults[i].Resolutions = new List<Resolution>();
		//			if (resolutionResults[i].FieldType == new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488")) // Check Box
		//			{
		//				var fieldId = resolutionResults[i].FieldId;
		//				resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId).OrderBy(a => a.Sequence).ToListAsync();
		//				if (resolutionResults[i].Resolutions != null && resolutionResults[i].Resolutions.Count() > 0)
		//				{
		//					resolutionResults[i].boolValue = new List<bool>();
		//					for (int j = 0; j < resolutionResults[i].Resolutions.Count(); j++)
		//					{
		//						if (resolutionResults[i].FieldValue != String.Empty && resolutionResults[i].FieldValue.Length == resolutionResults[i].Resolutions.Count())
		//						{
		//							if (resolutionResults[i].FieldValue.ElementAt(j) == '1')
		//							{
		//								resolutionResults[i].boolValue.Add(true);
		//							}
		//							else
		//							{
		//								resolutionResults[i].boolValue.Add(false);
		//							}
		//						}
		//						else
		//						{
		//							resolutionResults[i].boolValue.Add(false);
		//						}
		//					}
		//				}
		//			}
		//			else if (resolutionResults[i].FieldType == new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E")) // List
		//			{
		//				var fieldId = resolutionResults[i].FieldId;
		//				resolutionResults[i].Resolutions = await cwmContext.Resolutions.Where(a => (a.ResolutionType.HasValue ? a.ResolutionType.Value : Guid.Empty) == fieldId || a.ResolutionType == null).OrderBy(a => a.Sequence).ToListAsync();
		//				var linkedResolution = resolutionResults[i].Resolutions.Find(a => a.Resolution1 == resolutionResults[i].FieldValue);
		//				if (linkedResolution != null)
		//				{
		//					resolutionResults[i].ResolutionId = linkedResolution.ResolutionId;
		//				}
		//			}
		//		}
		//		results = resolutionResults;
		//	}
		//	catch (Exception ex)
		//	{
		//		logger.Error("Unexpected exception caught while retrieving user defined values for activity '" + id.ToString() + "'.", ex);
		//	}

		//	return results;
		//}

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
							if (list[i].UserDefValue1 == null || list[i].UserDefValue1 == "")
							{
								audit.AuditAction = "RECORD DELETED";
							}
							else
							{
								audit.AuditAction = "RECORD UPDATED";
							}

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

						auditField.FieldDesc = cwmContext.UserDefFields.FirstOrDefault(a => a.UserDefFieldId == userDefinedValue.UserDefFieldId).FieldDesc;
						audit.RecordId = userDefinedValue.UserDefValueId;
						if (list[i].UserDefValue1 == null && list[i].UserDefValue1 == "")
						{
							cwmContext.UserDefValues.Remove(userDefinedValue);
						}
						else
						{
							userDefinedValue.UserDefValue1 = list[i].UserDefValue1;
							userDefinedValue.DateUpdated = DateTime.Now;
							auditField.NewId = userDefinedValue.UserDefValueId;
							auditField.NewValue = userDefinedValue.UserDefValue1;
						}



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

		private string GetPersistentValue(CPUDFModules m_CPModule, Guid m_RecordId, Guid m_UDFCategoryId, Guid m_UDFFieldId, Guid m_AddressId, CPUDFModules m_FeeModule = CPUDFModules.Activities)
		{
			string oSQL = "";
			string retval = "";
			//int intI = 0;
			try
			{
				//intI = 1;
				if (systemService.GetCodepalSetting("AllUDFTypes", (Guid?)HttpContext.Current.Session["AgencyId"], null).ToString() == "1" || systemService.GetCodepalSetting("AllUDFTypes", (Guid?)HttpContext.Current.Session["AgencyId"], null).ToString() == "true")
				{
					switch (m_CPModule)
					{
						case CPUDFModules.Activities:
							oSQL = "SELECT TOP 1 InspectionId FROM Inspections WHERE InspectionTypeId IN";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Inspections WHERE InspectionId='" + m_RecordId + "') ";
							oSQL += "AND InspectionId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Addresses:
							oSQL = "SELECT TOP 1 AddressId FROM Addresses WHERE AddressTypeId IN";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND AddressId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Fees: //Pull AddressId From Object
							oSQL = "SELECT Recordid FROM Fees WHERE FeeId='" + m_RecordId + "'";
							oSQL = "SELECT TOP 1 FeeId FROM Fees WHERE FeeTypeId IN ";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							switch (m_FeeModule)
							{
								case CPUDFModules.Activities:
									oSQL += "AND RecordId IN (SELECT InspectionId FROM Inspections WHERE AddressId='" + m_AddressId + "')";
									break;
								case CPUDFModules.Permits:
									oSQL += "AND RecordId IN (SELECT PermitId FROM Permits WHERE AddressId='" + m_AddressId + "')";
									break;
							}
							oSQL += "AND FeeId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Items:
							oSQL = "SELECT TOP 1 ItemId FROM Items WHERE ItemTypeId IN";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Items WHERE ItemId='" + m_RecordId + "') ";
							oSQL += "AND ItemId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Permits:
							oSQL = "SELECT TOP 1 PermitId FROM Permits WHERE PermitTypeId IN ";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Permits WHERE PermitId='" + m_RecordId + "') ";
							oSQL += "AND PermitId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Projects:
							oSQL = "SELECT TOP 1 ProjectId FROM Projects WHERE ProjectTypeId IN";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND ProjectId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Requests:
							oSQL = "SELECT TOP 1 ComplaintId FROM Complaints WHERE ComplaintTypeId IN";
							oSQL += "(SELECT TypeId FROM UserDefCategoryTypes WHERE UserDefCategoryId='" + m_UDFCategoryId + "') ";
							oSQL += "AND AddressId=(Select AddressId FROM Complaints WHERE ComplaintId='" + m_RecordId + "') ";
							oSQL += "AND ComplaintId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
					}
				}
				else
				{
					switch (m_CPModule)
					{
						case CPUDFModules.Activities:
							oSQL = "SELECT TOP 1 InspectionId FROM Inspections WHERE InspectionTypeId=";
							oSQL += "(SELECT InspectionTypeId FROM Inspections WHERE InspectionId='" + m_RecordId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Inspections WHERE InspectionId='" + m_RecordId + "') ";
							oSQL += "AND InspectionId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Addresses:
							oSQL = "SELECT TOP 1 AddressId FROM Addresses WHERE AddressTypeId=";
							oSQL += "(SELECT AddressTypeId FROM Addresses WHERE AddressId='" + m_RecordId + "') ";
							oSQL += "AND AddressId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Agency:
							oSQL = "SELECT TOP 1 AgencyId FROM Agency WHERE AgencyId=";
							oSQL += "AND AgencyId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Fees:
							oSQL = "SELECT Recordid FROM Fees WHERE FeeId='" + m_RecordId + "'";
							oSQL = "SELECT TOP 1 FeeId FROM Fees WHERE FeeTypeId=";
							oSQL += "(SELECT FeeTypeId FROM Fees WHERE FeeId='" + m_RecordId + "') ";
							switch (m_FeeModule)
							{
								case CPUDFModules.Activities:
									oSQL += "AND RecordId IN (SELECT InspectionId FROM Inspections WHERE AddressId='" + m_AddressId + "')";
									break;
								case CPUDFModules.Permits:
									oSQL += "AND RecordId IN (SELECT PermitId FROM Permits WHERE AddressId='" + m_AddressId + "')";
									break;
							}
							oSQL += "AND FeeId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Items:
							oSQL = "SELECT TOP 1 ItemId FROM Items WHERE ItemTypeId=";
							oSQL += "(SELECT ItemTypeId FROM Items WHERE ItemId='" + m_RecordId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Items WHERE ItemId='" + m_RecordId + "') ";
							oSQL += "AND ItemId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Permits:
							oSQL = "SELECT TOP 1 PermitId FROM Permits WHERE PermitTypeId=";
							oSQL += "(SELECT PermitTypeId FROM Permits WHERE PermitId='" + m_RecordId + "') ";
							oSQL += "AND AddressId=(SELECT AddressId FROM Permits WHERE PermitId='" + m_RecordId + "') ";
							oSQL += "AND PermitId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Projects:
							oSQL = "SELECT TOP 1 ProjectId FROM Projects WHERE ProjectTypeId=";
							oSQL += "(SELECT ProjectTypeId FROM Projects WHERE ProjectId='" + m_RecordId + "') ";
							oSQL += "AND ProjectId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
						case CPUDFModules.Requests:
							oSQL = "SELECT TOP 1 ComplaintId FROM Complaints WHERE ComplaintTypeId=";
							oSQL += "(SELECT ComplaintTypeId FROM Complaints WHERE ComplaintId='" + m_RecordId + "') ";
							oSQL += "AND AddressId=(Select AddressId FROM Complaints WHERE ComplaintId='" + m_RecordId + "') ";
							oSQL += "AND ComplaintId<>'" + m_RecordId + "' ";
							oSQL += "ORDER BY DateInserted DESC";
							break;
					}
				}
				//intI = 2;
				//object[] parameters =  { };
				//PreviousObjectId = await ((DbContext)cwmContext).Database.SqlQuery<Guid>(oSQL, parameters).FirstOrDefaultAsync();

				//intI = 3;
				//if (PreviousObjectId != null && PreviousObjectId != Guid.Empty)
				//{
				//	intI = 4;

				//	UserDefValue userDefined = await cwmContext.UserDefValues.Where(udf => udf.RecordId == PreviousObjectId && udf.UserDefFieldId == m_UDFFieldId).FirstOrDefaultAsync();

				//	retval = userDefined.UserDefValue1; 

				//	if (retval == null)
				//	{
				//		retval = "";
				//	}
				//}
				retval = oSQL;
			}
			catch (Exception ex)
            {
                _ = ex;
				//MessageBox.Show("GPV " + intI.ToString());
				logger.Error("Unexpected exception caught while retrieving user defined values for activity '" + m_RecordId.ToString() + "'.", ex);
			}
			return retval;
		}

		public string GetUDFModuleType(Guid udfFieldId)
		{
			string result = "";

			if (udfFieldId != null)
			{
				Guid udfCategoryId = cwmContext.UserDefFields.FirstOrDefault(f => f.UserDefFieldId == udfFieldId).UserDefCategoryId;
				if (udfCategoryId != Guid.Empty)
				{
					UserDefCategory udfCategory = cwmContext.UserDefCategories.FirstOrDefault(m => m.UserDefCategoryId == udfCategoryId);

					if (udfCategory.ModuleId != null)
					{
						result = cwmContext.Modules.FirstOrDefault(m => m.ModuleId == udfCategory.ModuleId).ModuleDesc;
					}
					else
					{
						result = udfCategory.AllAgency;
					}
				}
			}
			if (result == null)
			{
				result = "";
			}

			return result;
		}

		public Guid GetFeeUDFRecordId(string modeulType, Guid recordId)
		{
			Guid result = Guid.Empty;
			Guid? permitId = null;
			Guid? inspectionId = null;

			if (cwmContext.Inspections.Where(i => i.InspectionId == recordId).Count() == 0)
			{
				permitId = recordId;
			}
			else
			{
				inspectionId = recordId;
			}

			switch (modeulType)
			{
				case "Activity":
				case "Act":
				case "act":
					result = inspectionId ?? Guid.Empty;
					break;
				case "Address":
				case "Add":
				case "add":
					if (inspectionId != null)
					{
						result = cwmContext.Inspections.FirstOrDefault(i => i.InspectionId == inspectionId).AddressId ?? Guid.Empty;
					}
					else if (permitId != null)
					{
						result = cwmContext.Permits.FirstOrDefault(i => i.PermitId == permitId).AddressId ?? Guid.Empty;
					}
					break;
				case "Party":
				case "Par":
				case "par":
					if (inspectionId != null)
					{
						result = cwmContext.Inspections.FirstOrDefault(i => i.InspectionId == inspectionId).InspectedPartyId ?? Guid.Empty;
					}
					else if (permitId != null)
					{
						result = cwmContext.Permits.FirstOrDefault(i => i.PermitId == permitId).IssuedToPartyId ?? Guid.Empty;
					}
					break;
				case "Permit":
				case "Per":
				case "per":
					result = permitId ?? Guid.Empty;
					break;
				case "Item":
				case "Ite":
				case "ite":
					if (inspectionId != null)
					{
						result = cwmContext.Inspections.FirstOrDefault(i => i.InspectionId == inspectionId).ItemId ?? Guid.Empty;
					}
					else if (permitId != null)
					{
						result = cwmContext.Permits.FirstOrDefault(i => i.PermitId == permitId).ItemId ?? Guid.Empty;
					}
					break;
				default:
					break;
			}

			return result;
		}

		public string GetUDFValue(Guid udfFieldId, Guid recordId)
		{
			return cwmContext.UserDefValues.FirstOrDefault(udf => udf.UserDefFieldId == udfFieldId && udf.RecordId == recordId).UserDefValue1.ToString();
		}

		public async Task<string> GetUDFValueAsync(Guid udfFieldId, Guid recordId)
		{
			string result = "";
			var udfVal = await cwmContext.UserDefValues.FirstOrDefaultAsync(udf => udf.UserDefFieldId == udfFieldId && udf.RecordId == recordId);
			if (udfVal != null)
			{
				result = udfVal.UserDefValue1.ToString();
			}			
			return result;
		}

		public async Task<string> GetUDFCategoryNameAsync(Guid udfCategoryId)
		{
			string result = "";
			try
			{

				var udfCat = await cwmContext.UserDefCategories.FirstOrDefaultAsync(u => u.UserDefCategoryId == udfCategoryId);

				if (udfCat != null)
				{
					result = udfCat.Category;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving user defined category for CategoryId '" + udfCategoryId.ToString() + "'.", ex);
			}

			return result;
		}

		public async Task<Guid?> GetUDFCategoryIdAsync(string CategoryName, Guid? ModuleId, Guid? AgencyId, string AgencyName)
		{
			string oSQL;
			Guid? result = null;
			try
			{

				if (AgencyName == null)
					AgencyName = "";

				oSQL = "Select UserDefCategoryId From UserDefCategories Where Category='" + CategoryName + "' ";

				if (ModuleId != null && AgencyName.Length != 3)
					oSQL += "AND ModuleId='" + ModuleId + "' ";
				else
					oSQL += "AND ModuleId IS NULL ";

				if (AgencyName.Length == 3)
					oSQL += "AND AllAgency='" + AgencyName + "' ";
				else
					oSQL += "AND AgencyId='" + AgencyId + "' ";

				var thisCat = await cwmContext.UserDefCategories.SqlQuery(oSQL, null).FirstOrDefaultAsync();

				if (thisCat != null)
					result = thisCat.UserDefCategoryId;
			}
			catch (Exception)
			{
			}
			return result;
		}

		public async Task<Guid?> GetUDFFieldIdAsync(string FieldName, Guid? udfCategoryiD)
		{
			return (await cwmContext.UserDefFields.FirstOrDefaultAsync(u => u.FieldDesc == FieldName && u.UserDefCategoryId == udfCategoryiD)).UserDefFieldId;
		}

		public async Task<bool> UDFIsEncryptedAsync(Guid? FieldId)
		{
			return (await cwmContext.UserDefFields.FirstOrDefaultAsync(u => u.UserDefFieldId == FieldId)).FieldEncrypted;
		}

		public async Task<string> GetUDFTypeAsync(string UserDefFieldId)
		{
			string oSQL = "";
			string result = "";

			oSQL = "Select UserDefType From UserDefTypes LEFT OUTER JOIN UserDefFields ON UserDefFields.UserDefTypeid=UserDefTypes.UserDefTypeId Where UserDefFieldId='" + UserDefFieldId + "' ";


			var udfType = cwmContext.UserDefTypes.SqlQuery(oSQL);

			if (udfType.First() != null)
			{
				result = (await udfType.FirstAsync()).UserDefType1;
			}

			return result;

		}

	}
}

