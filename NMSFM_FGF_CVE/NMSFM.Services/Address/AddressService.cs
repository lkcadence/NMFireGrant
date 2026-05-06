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
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Net;
using System.Xml.Linq;

namespace NMSFM.Services.Address
{
	public class AddressService : IAddressService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;
		private List<string> imageSuffixes = new List<string> { ".bmp", ".gif", ".jpeg", ".png", ".tiff", ".jpg", ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
		private string googleKey = "AIzaSyB1nTgkifPOht7SHxeK-z1nc92D55WCM64";

		public AddressService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
		}

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

		public async Task<IEnumerable<string>> GetSuffixListAsync()
		{
			IEnumerable<string> result;
			try
			{
				var addressList = await GetAddressesAsync(false);
				var suffixList = addressList.Where(a => a.Suffix != null && a.Suffix.Trim() != String.Empty).Select(a => a.Suffix).Distinct();
				result = suffixList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Suffix List.", ex);
				result = new List<string>();
			}
			return result;
		}

		public async Task<IEnumerable<Region>> GetRegionListAsync()
		{
			IEnumerable<Region> result;
			try
			{
				var regionList = await cwmContext.Regions.Where(a => !a.Inactive && a.WebViewable == true).ToListAsync();
				result = regionList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the region list.", ex);
				result = new List<Region>();
			}
			return result;
		}

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
				logger.Error("Unexpected exception caught while retrieving the region list.", ex);
				result = new List<v_AddressParties>();
			}
			return result;
		}

		public async Task<zzPartyWebAccess> GetPartyWebAccessByInfoAsync(zzPartyWebAccess user) // Unused, pending deletion
		{
			zzPartyWebAccess result = null;
			if (user != null && user.UserName != null && user.Password != null)
			{
				try
				{
					result = await cwmContext.zzPartyWebAccess.SingleOrDefaultAsync(a => a.UserName == user.UserName && a.Password == user.Password);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the User '" + user.UserName.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<Inspector> GetInspectorByInfoAsync(zzPartyWebAccess user)
		{
			Inspector result = null;
			Inspector inspector = null;
			Setting salt = null;
			if (user != null && user.UserName != null && user.Password != null)
			{
				try
				{
					inspector = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.Login == user.UserName);
					if (inspector != null && inspector.InspectorId != null)
					{
						var inspectorId = inspector.InspectorId.ToString();
						salt = await cwmContext.Settings.SingleOrDefaultAsync(a => a.UserName == inspectorId);
						if (salt != null && salt.ValueField != null)
						{
							var convertedToBytes = System.Text.UTF8Encoding.UTF8.GetBytes(user.Password + salt.ValueField);
							HashAlgorithm hashType = new SHA512Managed();
							var hashBytes = hashType.ComputeHash(convertedToBytes);
							var hashedResult = Convert.ToBase64String(hashBytes); // entered password hashed with inspector's salt

							if (inspector.Password == hashedResult)
							{
								result = inspector;
							}
						}
						else
						{
							logger.Error("No password data available for inspector: '" + user.UserName.ToString() + "'.");
						}
					}
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the User '" + user.UserName.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

		public async Task<v_Addresses2> GetAddressByIdAsync(Guid id)
		{
			v_Addresses2 result = null;
			try
			{
				result = await cwmContext.v_Addresses2.SingleOrDefaultAsync(a => a.AddressId == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<v_AddressParties>> GetAddressPartiesByAddressIdAsync(Guid addressId, Guid? partyId)
		{
			IEnumerable<v_AddressParties> result;
			try
			{
				var addressParties = await cwmContext.v_AddressParties.Where(p => p.AddressId == addressId && (!p.Inactive.HasValue || !p.Inactive.Value)).ToListAsync() ?? new List<v_AddressParties>();
				var roleTypeList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
				if (addressParties != null && addressParties.Count() > 0 && roleTypeList != null && roleTypeList.Count > 0)
				{
					addressParties = addressParties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList() ?? new List<v_AddressParties>();
				}
				if ((partyId != null && partyId != Guid.Empty) && !addressParties.Any(a => a.PartyID == partyId.Value))
				{
					var party = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyID == partyId.Value);
					addressParties.Add(new v_AddressParties() { AddressPartyId = Guid.NewGuid(), PartyID = party.PartyID, PartyName = party.PartyName });
				}
				result = addressParties;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Parties List.", ex);
				result = new List<v_AddressParties>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Items>> GetAddressItemsByAddressIdAsync(Guid id)
		{
			IEnumerable<v_Items> result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var addressItems = await cwmContext.v_Items.Where(p => p.AddressId == id).ToListAsync();

				result = addressItems;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Items List.", ex);
				result = new List<v_Items>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Permits>> GetPermitsByAddressIdAsync(Guid id)
		{
			IEnumerable<v_Permits> result;
			try
			{
				result = await cwmContext.v_Permits.Where(p => p.AddressId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Permits List.", ex);
				result = new List<v_Permits>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Complaints>> GetComplaintsByAddressIdAsync(Guid id)
		{
			IEnumerable<v_Complaints> result;
			try
			{
				result = await cwmContext.v_Complaints.Where(p => p.AddressId == id && p.Inactive == false && p.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Complaints List.", ex);
				result = new List<v_Complaints>();
			}
			return result;
		}

		public async Task<IEnumerable<v_LocationItemCount>> GetLocationsByAddressIdAsync(Guid id)
		{
			IEnumerable<v_LocationItemCount> result;
			try
			{
				result = await cwmContext.v_LocationItemCount.Where(p => p.AddressId == id && p.Inactive == false).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Locations List.", ex);
				result = new List<v_LocationItemCount>();
			}
			return result;
		}

		public async Task<IEnumerable<LocationBas>> GetLocationBasesByAddressIdAsync(Guid id)
		{
			IEnumerable<LocationBas> result;
			try
			{
				result = await cwmContext.LocationBases.Where(p => p.AddressId == id && p.Inactive == false).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Location Bases List.", ex);
				result = new List<LocationBas>();
			}
			return result;
		}

		public async Task<IEnumerable<Data.Note>> GetNotesByAddressIdAsync(Guid id)
		{
			IEnumerable<Data.Note> result;
			try
			{
				result = await cwmContext.Notes.Where(p => p.RecordId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Notes List.", ex);
				result = new List<Data.Note>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Projects>> GetProjectsByAddressIdAsync(Guid id)
		{
			IEnumerable<v_ProjectAddressSearch> projects;
			IEnumerable<v_Projects> result = Enumerable.Empty<v_Projects>();
			try
			{
				projects = await cwmContext.v_ProjectAddressSearch.Where(p => p.AddressId == id).ToListAsync();
				if (projects != null)
				{
					foreach (var row in projects)
					{
						result = result.Concat(new[] { await cwmContext.v_Projects.SingleOrDefaultAsync(p => p.ProjectId == row.ProjectId) });
					}
				}

			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Projects List.", ex);
				result = new List<v_Projects>();
			}
			return result;
		}

		public async Task<IEnumerable<AddressMap>> GetAddressMapByAddressIdAsync(Guid id)
		{
			IEnumerable<AddressMap> result;
			try
			{
				result = await cwmContext.AddressMaps.Where(p => p.AddressId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Map Data.", ex);
				result = new List<AddressMap>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Addresses2>> GetRelatedAddressesByAddressIdAsync(Guid id)
		{
			IEnumerable<v_Addresses2> result;
			try
			{
				var addressList = await GetAddressesAsync(false);
				result = addressList.Where(p => p.ParentAddressId == id && p.Inactive == false);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Related Address List.", ex);
				result = new List<v_Addresses2>();
			}
			return result;
		}

		public async Task<IEnumerable<v_Activities>> GetActivitiesByAddressIdAsync(Guid id)
		{
			IEnumerable<v_Activities> result;
			try
			{
				result = await cwmContext.v_Activities.Where(p => p.AddressId == id).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Activities List.", ex);
				result = new List<v_Activities>();
			}
			return result;
		}

		public async Task<IEnumerable<AddressType>> GetAddressTypeListAsync()
		{
			IEnumerable<AddressType> result;
			try
			{
				result = await cwmContext.AddressTypes.Where(a => !a.Inactive && a.WebViewable).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Type List.", ex);
				result = new List<AddressType>();
			}
			return result;
		}

		public async Task<IEnumerable<State>> GetStateListAsync()
		{
			IEnumerable<State> result;
			try
			{
				result = await cwmContext.States.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the State List.", ex);
				result = new List<State>();
			}
			return result;
		}

		public async Task<IEnumerable<Zip>> GetZipListAsync()
		{
			IEnumerable<Zip> result;
			try
			{
				result = await cwmContext.Zips.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Zip List.", ex);
				result = new List<Zip>();
			}
			return result;
		}

		public async Task<IEnumerable<Country>> GetCountryListAsync()
		{
			IEnumerable<Country> result;
			try
			{
				result = await cwmContext.Countries.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Country List.", ex);
				result = new List<Country>();
			}
			return result;
		}

		public async Task<IEnumerable<County>> GetCountyListAsync()
		{
			IEnumerable<County> result;
			try
			{
				result = await cwmContext.Counties.ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the County List.", ex);
				result = new List<County>();
			}
			return result;
		}

		public async Task<IEnumerable<string>> GetStreetAddressListAsync()
		{
			IEnumerable<string> result;
			try
			{
				var addressList = await GetAddressesAsync(false);
				result = addressList.Where(a => a.Address != null && a.Address.Trim() != String.Empty).Select(a => a.Address).Distinct();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Street Address List.", ex);
				result = new List<string>();
			}
			return result;
		}

		public async Task<IEnumerable<string>> GetCityListAsync()
		{
			IEnumerable<string> result;
			try
			{
				var addressList = await GetAddressesAsync(false);
				result = addressList.Where(a => a.City != null && a.City.Trim() != String.Empty).Select(a => a.City).Distinct();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the City List.", ex);
				result = new List<string>();
			}
			return result;
		}

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

		public async Task<IEnumerable<string>> GetDirectionListAsync()
		{
			IEnumerable<string> result;
			try
			{
				var addressList = await GetAddressesAsync(false);
				var directionList = addressList.Where(a => a.Direction != null && a.Direction.Trim() != String.Empty).Select(a => a.Direction).Distinct();
				result = directionList;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Direction List.", ex);
				result = new List<string>();
			}
			return result;
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
				AddressTypeId = address.AddressTypeId.HasValue ? address.AddressTypeId.Value : Guid.Empty;
				Guid AgencyId = new Guid("9808204F-D941-451E-B121-02C8A0D7E7FA");
				if (agency != null && agency != Guid.Empty)
				{
					AgencyId = agency;
				}
				Module module = null;
				module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Address");
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
								 SequenceNumber = cats.SeqNum.HasValue ? cats.SeqNum.Value : 0,
								 FieldSequenceNumber = flds.SeqNum.HasValue ? flds.SeqNum.Value : 0,
								 WebViewable = (cats.WebViewable.HasValue ? cats.WebViewable.Value : false) //&& flds.WebViewable,
							 };

				var resolutionResults = await models.ToListAsync();

				for (int i = 0; i < resolutionResults.Count(); i++)
				{
					resolutionResults[i].Resolutions = new List<Resolution>();
					if (resolutionResults[i].FieldType == new Guid("BCECC8B9-9C57-47F6-AB75-452F8A6F1488")) // Check Box
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
					else if (resolutionResults[i].FieldType == new Guid("6382BED2-B352-4D6B-8CD3-7DAD85C7CB0E")) // List
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
					logger.Error("Unexpected exception thrown while performing a search for code: " + code + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for city: " + city + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for state: " + state + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for zip: " + zip + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for region: " + region + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for county: " + county + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for occupancy type: " + occupancy + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for property use type: " + property + ".", ex);
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
					logger.Error("Unexpected exception thrown while performing a search for party: " + party + ".", ex);
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

		public async Task SaveLegalDescriptionAsync(Guid addressId, string LegalDesc)
		{
			if (addressId != null && LegalDesc != null)
			{
				var address = await cwmContext.Addresses.SingleOrDefaultAsync(a => a.AddressId == addressId);
				if (address == null)
				{
					logger.Error("Unable to update legal desc. for address '" + addressId.ToString() + "'.  The address could not be located in the database.");
					return;
				}
				var audit = new AuditModel { TableName = "Addresses", RecordId = addressId, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();
				if (address.LegalDesc != LegalDesc)
				{
					var auditField = new AuditFieldModel { ControlName = "legalText", FieldDesc = "Legal Text", OldId = null, OldValue = address.LegalDesc, NewId = null, NewValue = LegalDesc };
					auditFields.Add(auditField);
				}
				address.LegalDesc = LegalDesc;

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
						logger.Error("Unable to save the legal desc. changes for address '" + addressId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to update legal desc. for address '" + addressId.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("SaveLegalDescriptions was called with a null reference.");
			}
		}

		public async Task SaveAddressAsync(v_Addresses2 model)
		{
			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  
				var address = await cwmContext.Addresses.SingleOrDefaultAsync(a => a.AddressId == model.AddressId);
				if (address == null)
				{
					logger.Error("Unable to update address '" + model.AddressId.ToString() + "'.  The address could not be located in the database.");
					return;
				}

				var audit = new AuditModel { TableName = "Addresses", RecordId = model.AddressId, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				if (address.Address1 != model.Address)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Address", FieldDesc = "Street Name", OldId = null, OldValue = address.Address1, NewId = null, NewValue = model.Address });
					address.Address1 = model.Address;
				}

				if (address.AddressCode != model.AddressCode)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressCode", FieldDesc = "Address Code", OldId = null, OldValue = address.AddressCode, NewId = null, NewValue = model.AddressCode });
					address.AddressCode = model.AddressCode;
				}

				if (address.AddressNumber != model.AddressNumber)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressNumber", FieldDesc = "Street Number", OldId = null, OldValue = address.AddressNumber, NewId = null, NewValue = model.AddressNumber });
					address.AddressNumber = model.AddressNumber;
				}

				if (address.AddressTypeId != model.AddressTypeId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressTypeId", FieldDesc = "Address Type", OldId = address.AddressTypeId, OldValue = null, NewId = model.AddressTypeId, NewValue = null });
					address.AddressTypeId = model.AddressTypeId;
				}

				if (address.Block != model.Block)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Block", FieldDesc = "Block", OldId = null, OldValue = address.Block, NewId = null, NewValue = model.Block });
					address.Block = model.Block;
				}

				if (address.City != model.City)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "City", FieldDesc = "City", OldId = null, OldValue = address.City, NewId = null, NewValue = model.City });
					address.City = model.City;
				}

				if (address.Comment != model.Comment)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Comment", FieldDesc = "Comment", OldId = null, OldValue = address.Comment, NewId = null, NewValue = model.Comment });
					address.Comment = model.Comment;
				}

				if (address.CountryId != model.CountryId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "CountryId", FieldDesc = "Country Id", OldId = address.CountryId, OldValue = null, NewId = model.CountryId, NewValue = null });
					address.CountryId = model.CountryId;
				}

				if (address.CountyId != model.CountyId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "CountyId", FieldDesc = "County Id", OldId = address.CountyId, OldValue = null, NewId = model.CountyId, NewValue = null });
					address.CountyId = model.CountyId;
				}

				address.DateUpdated = DateTime.Now;

				if (address.Direction != model.Direction)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Direction", FieldDesc = "Direction", OldId = null, OldValue = address.Direction, NewId = null, NewValue = model.Direction });
					address.Direction = model.Direction;
				}

				if (address.Latitude != model.Latitude)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Latitude", FieldDesc = "Latitude", OldId = null, OldValue = address.Latitude, NewId = null, NewValue = model.Latitude });
					address.Latitude = model.Latitude;
				}

				if (address.Longitude != model.Longitude)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Longitude", FieldDesc = "Longitude", OldId = null, OldValue = address.Longitude, NewId = null, NewValue = model.Longitude });
					address.Longitude = model.Longitude;
				}

				if (address.Lot != model.Lot)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Lot", FieldDesc = "Lot", OldId = null, OldValue = address.Lot, NewId = null, NewValue = model.Lot });
					address.Lot = model.Lot;
				}

				if (address.Map != model.Map)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Map", FieldDesc = "Map", OldId = null, OldValue = address.Map, NewId = null, NewValue = model.Map });
					address.Map = model.Map;
				}

				if (address.OccupancyTypeId != model.OccupancyTypeId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "OccupancyTypeId", FieldDesc = "Occupancy Type Id", OldId = address.OccupancyTypeId, OldValue = null, NewId = model.OccupancyTypeId, NewValue = null });
					address.OccupancyTypeId = model.OccupancyTypeId;
				}

				if (address.POBox != model.POBox)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "POBox", FieldDesc = "POBox", OldId = null, OldValue = address.POBox.ToString(), NewId = null, NewValue = model.POBox.ToString() });
					address.POBox = model.POBox;
				}

				if (address.PropertyUseTypeId != model.PropertyUseTypeId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "PropertyUseTypeId", FieldDesc = "Property Use Type Id", OldId = address.PropertyUseTypeId, OldValue = null, NewId = model.PropertyUseTypeId, NewValue = null });
					address.PropertyUseTypeId = model.PropertyUseTypeId;
				}

				if (address.StateId != model.StateId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "StateId", FieldDesc = "StateId", OldId = address.StateId, OldValue = null, NewId = model.StateId, NewValue = null });
					address.StateId = model.StateId;
				}

				if (address.SubAddress != model.SubAddress)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "SubAddress", FieldDesc = "Sub Address", OldId = null, OldValue = address.SubAddress, NewId = null, NewValue = model.SubAddress });
					address.SubAddress = model.SubAddress;
				}

				if (address.Suffix != model.Suffix)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Suffix", FieldDesc = "Suffix", OldId = null, OldValue = address.Suffix, NewId = null, NewValue = model.Suffix });
					address.Suffix = model.Suffix;
				}

				if (address.TaxParcel != model.TaxParcel)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "TaxParcel", FieldDesc = "Tax Parcel #", OldId = null, OldValue = address.TaxParcel, NewId = null, NewValue = model.TaxParcel });
					address.TaxParcel = model.TaxParcel;
				}

				if (address.ZipId != model.ZipId)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "ZipId", FieldDesc = "Zip Id", OldId = address.ZipId, OldValue = null, NewId = model.ZipId, NewValue = null });
					address.ZipId = model.ZipId;
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
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to save the changes for address '" + model.AddressId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to update address '" + model.AddressId.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("SaveAddress was called with a null reference.");
			}
		}

		public async Task CreateAddressAsync(v_Addresses2 model)
		{
			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  
				var address = await cwmContext.Addresses.SingleOrDefaultAsync(a => a.AddressId == model.AddressId);
				if (address != null)
				{
					logger.Error("Unable to create address '" + model.AddressId.ToString() + "'.  The address is already located in the database.");
					return;
				}

				var audit = new AuditModel { TableName = "Addresses", RecordId = model.AddressId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				address = cwmContext.Addresses.Add(new Data.Address());
				address.AddressId = model.AddressId;
				address.rowguid = model.rowguid;
				address.DateInserted = DateTime.Now;
				address.DateUpdated = address.DateInserted;
				address.DefaultPass = false;
				address.Inactive = false;

				auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = address.AddressId, NewValue = null });

				if (model.Address != null && model.Address != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Address", FieldDesc = "Street Name", OldId = null, OldValue = null, NewId = null, NewValue = model.Address });
					address.Address1 = model.Address;
				}

				if (model.AddressCode != null && model.AddressCode != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressCode", FieldDesc = "Address Code", OldId = null, OldValue = null, NewId = null, NewValue = model.AddressCode });
					address.AddressCode = model.AddressCode;
				}

				if (model.AddressNumber != null && model.AddressNumber != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressNumber", FieldDesc = "Street Number", OldId = null, OldValue = null, NewId = null, NewValue = model.AddressNumber });
					address.AddressNumber = model.AddressNumber;
				}

				if (model.AddressTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "AddressTypeId", FieldDesc = "Address Type", OldId = null, OldValue = null, NewId = model.AddressTypeId, NewValue = null });
					address.AddressTypeId = model.AddressTypeId;
				}

				if (model.Block != null && model.Block != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Block", FieldDesc = "Block", OldId = null, OldValue = null, NewId = null, NewValue = model.Block });
					address.Block = model.Block;
				}

				if (model.City != null && model.City != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "City", FieldDesc = "City", OldId = null, OldValue = null, NewId = null, NewValue = model.City });
					address.City = model.City;
				}

				if (model.Comment != null && model.Comment != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Comment", FieldDesc = "Comment", OldId = null, OldValue = null, NewId = null, NewValue = model.Comment });
					address.Comment = model.Comment;
				}

				if (model.CountryId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "CountryId", FieldDesc = "Country Id", OldId = null, OldValue = null, NewId = model.CountryId, NewValue = null });
					address.CountryId = model.CountryId;
				}

				if (model.CountyId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "CountyId", FieldDesc = "County Id", OldId = null, OldValue = null, NewId = model.CountyId, NewValue = null });
					address.CountyId = model.CountyId;
				}

				address.DateUpdated = DateTime.Now;

				if (model.Direction != null && model.Direction != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Direction", FieldDesc = "Direction", OldId = null, OldValue = null, NewId = null, NewValue = model.Direction });
					address.Direction = model.Direction;
				}

				if (model.Latitude != null && model.Latitude != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Latitude", FieldDesc = "Latitude", OldId = null, OldValue = null, NewId = null, NewValue = model.Latitude });
					address.Latitude = model.Latitude;
				}

				if (model.Longitude != null && model.Longitude != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Longitude", FieldDesc = "Longitude", OldId = null, OldValue = null, NewId = null, NewValue = model.Longitude });
					address.Longitude = model.Longitude;
				}

				if (model.Lot != null && model.Lot != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Lot", FieldDesc = "Lot", OldId = null, OldValue = null, NewId = null, NewValue = model.Lot });
					address.Lot = model.Lot;
				}

				if (model.Map != null && model.Map != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Map", FieldDesc = "Map", OldId = null, OldValue = null, NewId = null, NewValue = model.Map });
					address.Map = model.Map;
				}

				if (model.OccupancyTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "OccupancyTypeId", FieldDesc = "Occupancy Type Id", OldId = null, OldValue = null, NewId = model.OccupancyTypeId, NewValue = null });
					address.OccupancyTypeId = model.OccupancyTypeId;
				}

				if (model.POBox != false)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "POBox", FieldDesc = "POBox", OldId = null, OldValue = null, NewId = null, NewValue = model.POBox.ToString() });
					address.POBox = model.POBox;
				}

				if (model.PropertyUseTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "PropertyUseTypeId", FieldDesc = "Property Use Type Id", OldId = null, OldValue = null, NewId = model.PropertyUseTypeId, NewValue = null });
					address.PropertyUseTypeId = model.PropertyUseTypeId;
				}

				if (model.StateId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "StateId", FieldDesc = "StateId", OldId = null, OldValue = null, NewId = model.StateId, NewValue = null });
					address.StateId = model.StateId;
				}

				if (model.SubAddress != null && model.SubAddress != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "SubAddress", FieldDesc = "Sub Address", OldId = null, OldValue = null, NewId = null, NewValue = model.SubAddress });
					address.SubAddress = model.SubAddress;
				}

				if (model.Suffix != null && model.Suffix != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Suffix", FieldDesc = "Suffix", OldId = null, OldValue = null, NewId = null, NewValue = model.Suffix });
					address.Suffix = model.Suffix;
				}

				if (model.TaxParcel != null && model.TaxParcel != String.Empty)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "TaxParcel", FieldDesc = "Tax Parcel #", OldId = null, OldValue = null, NewId = null, NewValue = model.TaxParcel });
					address.TaxParcel = model.TaxParcel;
				}

				if (model.ZipId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "ZipId", FieldDesc = "Zip Id", OldId = null, OldValue = null, NewId = model.ZipId, NewValue = null });
					address.ZipId = model.ZipId;
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
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create address '" + model.AddressId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create address '" + model.AddressId.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("CreateAddress was called with a null reference.");
			}
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

		public async Task<v_Parties> GetPartyWebAccessByIdAsync(Guid partyWebId)
		{
			v_Parties result = null;
			if (partyWebId != null)
			{
				try
				{
					result = await cwmContext.v_Parties.SingleOrDefaultAsync(a => a.PartyID == partyWebId);
				}
				catch (Exception ex)
            {
                _ = ex;
					logger.Error("Unexpected exception caught while retrieving the party '" + partyWebId.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

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
					logger.Error("Unexpected exception caught while retrieving the inspector '" + inspectorId.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("GetPartyWebAccessByInfoAsync was called with null arguments.");
			}
			return result;
		}

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

		public async Task<List<v_Parties>> GetPartyWebAccessListAsync()
		{
			List<v_Parties> result = null;
			try
			{
				Guid? fPFRespPartyGuid = new Guid("ba5f97d0-10d6-4fdd-8881-bc30b0e083af");
				Guid? fGFRespPartyGuid = new Guid("0068af6e-db4d-4d15-9ad3-4fc054ce32f8");
				//var roleTypeList = await cwmContext.RoleTypes.Where(a => a.RoleTypeId  == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync() ?? new List<Guid>();
				result = await cwmContext.v_Parties.Where(a => a.Inactive == false && (a.RoleTypeId == fPFRespPartyGuid || a.RoleTypeId == fGFRespPartyGuid)).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the party list.", ex);
			}
			return result;
		}

		//Added below to pull up by vAddressParties (vwd 03/14/2021)
		public async Task<List<v_AddressParties>> GetPartyWebAccessListAsync2()
		{
			List<v_AddressParties> result = null;
			try
			{
				Guid? fPFRespPartyGuid = new Guid("ba5f97d0-10d6-4fdd-8881-bc30b0e083af");
				Guid? fpAddressTypeGuid = new Guid("43856752-8b7a-4e6f-b697-bf8acd457c16");
				//var roleTypeList = await cwmContext.RoleTypes.Where(a => a.RoleTypeId  == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync() ?? new List<Guid>();
				result = await cwmContext.v_AddressParties.Where(a => a.Inactive == false && a.RoleTypeId == fPFRespPartyGuid && a.AddressTypeId == fpAddressTypeGuid).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the party list.", ex);
			}
			return result;
		}

		public async Task<zzHelp> GetHelpByInfoAsync(string id, Guid agency)
		{
			zzHelp result = null;
			try
			{
				result = await cwmContext.zzHelp.SingleOrDefaultAsync(a => a.AgencyId == agency && a.ID == id && (a.Inactive == null || a.Inactive == false));
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the help documentation.", ex);
			}
			return result;
		}

		public async Task<string> GetUserEmailAsync(Guid id)
		{
			string recipientEmail = null;
			try
			{
				var party = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyID == id);
				if (!party.Equals(null) && !string.IsNullOrEmpty(party.Email))
				{
					recipientEmail = party.Email;
				}
				else
				{
					var inspector = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.InspectorId == id);
					if (!inspector.Equals(null) && !string.IsNullOrEmpty(inspector.Email))
					{
						recipientEmail = inspector.Email;
					}
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the email for account id = " + id + ".", ex);
			}
			return recipientEmail;
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

		public async Task<List<v_Items>> GetItemListAsync(Guid addressId)
		{
			List<v_Items> result = null;
			try
			{
				result = await cwmContext.v_Items.Where(a => a.AddressId == addressId && a.Inactive == false).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Item list.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<Group>> GetGroupListAsync(Guid? id)
		{
			IEnumerable<Group> result = null;
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				result = await cwmContext.Groups.Where(a => (a.AgencyId == agencyId || a.AgencyId == null || a.GroupId == id) && a.Inactive == false && a.WebViewable == true).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Group list.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<RoleType>> GetRoleTypeListAsync(Guid? partyId)
		{
			IEnumerable<RoleType> result = null;
			try
			{
				var agencyId = (Guid)System.Web.HttpContext.Current.Session["AgencyId"];
				var selectedPartyId = partyId ?? Guid.Empty;
				var partyRoleTypes = await cwmContext.PartyRoles.Where(a => a.PartyId == selectedPartyId).Select(a => a.RoleTypeId).ToListAsync();

				result = await cwmContext.RoleTypes.Where(a => (a.AgencyId == agencyId || a.AgencyId == null) && (a.Inactive == false || partyRoleTypes.Contains(a.RoleTypeId)) && a.WebViewable == true && a.EmployeeType == false).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Role Type list.", ex);
			}
			return result;
		}

		public async Task<IEnumerable<v_InventoryItems>> GetInventoryItemListAsync(Guid id)
		{
			IEnumerable<v_InventoryItems> result = null;
			try
			{
				var item = cwmContext.Items.SingleOrDefault(a => a.ItemId == id);
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

		public async Task<Guid> GetPartyRoleTypeIdAsync(Guid partyId, Guid addressId)
		{
			Guid result = Guid.Empty;
			try
			{
				var addressParty = await cwmContext.AddressParties.Where(a => a.PartyID == partyId && a.AddressID == addressId && a.Inactive == false).FirstAsync() ?? new AddressParty();
				result = addressParty.RoleTypeId ?? Guid.Empty;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party Role Type Id for Party Id: " + partyId + " and Address Id: " + addressId + ".", ex);
			}
			return result;
		}

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
				logger.Error("Unexpected exception caught while retrieving the Project Number for Activity Id: " + activityId + ".", ex);
			}
			return result;
		}

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
				logger.Error("Unexpected exception caught while retrieving the Inspection Details for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

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
				logger.Error("Unexpected exception caught while retrieving the Fees for Activity Id:" + inspectionId + ".", ex);
			}
			return results;
		}

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
				logger.Error("Unexpected exception caught while retrieving the Fees for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

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
				logger.Error("Unexpected exception caught while retrieving the Signature for Activity Id: " + inspectionId + ".", ex);
			}
			return result;
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
				logger.Error("Unexpected exception caught while retrieving the Permits for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

		public async Task<List<v_Complaints>> GetRequestsByIdAsync(Guid inspectionId)
		{
			var results = new List<v_Complaints>();
			try
			{
				var complaintIdList = await cwmContext.ComplaintActivities.Where(a => a.ActivityId == inspectionId).Select(a => a.ComplaintId).ToListAsync();
				results = await cwmContext.v_Complaints.Where(a => complaintIdList.Contains(a.ComplaintId) && a.Inactive == false && a.WebViewable == true).ToListAsync() ?? new List<v_Complaints>();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Complaints for Activity Id: " + inspectionId + ".", ex);
			}
			return results;
		}

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
									 BooleanValue = usevalues != null ? (byte?)(usevalues.BooleanValue) : null,
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

		public async Task<SelectListItem> SaveResolutionAsync(Guid itemId, string resolutionText)
		{
			SelectListItem result = null;
			var existingResolution = cwmContext.Resolutions.Where(a => a.ResolutionType == itemId && a.Resolution1 == resolutionText);
			if (existingResolution.Count() == 0)
			{
				var newResolution = cwmContext.Resolutions.Add(new Resolution());
				newResolution.ResolutionId = Guid.NewGuid();
				newResolution.ResolutionType = itemId;
				newResolution.Resolution1 = resolutionText;
				newResolution.rowguid = Guid.NewGuid();
				newResolution.ExternalId = null;
				newResolution.DateUpdated = DateTime.Now;
				newResolution.DateInserted = DateTime.Now;
				var existingCount = cwmContext.Resolutions.Where(a => a.ResolutionType == itemId).Count();
				newResolution.Sequence = existingCount + 1;
				var audit = new AuditModel { TableName = "Resolutions", RecordId = newResolution.ResolutionId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>
				{
					new AuditFieldModel { ControlName = "ResolutionType", FieldDesc = "Resolution Type", OldId = null, OldValue = null, NewId = itemId, NewValue = null },
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
						logger.Error("Unable to save resolution for item '" + itemId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to save resolution for item '" + itemId.ToString() + "', DbContext was not available.");
				}
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

		public async Task<bool> CreateActivityAsync(v_Activities model, List<CheckItemModel> checkItems)
		{
			var result = false;
			if (model != null)
			{
				var audit = new AuditModel { TableName = "Inspections", RecordId = model.InspectionId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();

				var activity = cwmContext.Inspections.Add(new Data.Inspection());

				activity.rowguid = Guid.NewGuid();
				activity.DateInserted = DateTime.Now;
				activity.DateUpdated = activity.DateInserted;

				activity.InspectionId = model.InspectionId;
				activity.InspectionNumber = "Web Activity";
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

		public async Task<bool> SaveActivityAsync(v_Activities model, List<CheckItemModel> checkItems)
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

		public async Task<v_Permits> GetPermitByIdAsync(Guid id)
		{
			v_Permits result = null;
			try
			{
				if (cwmContext.v_Permits.Select(a => a.PermitId).ToArray().Contains(id))
				{
					result = await cwmContext.v_Permits.SingleOrDefaultAsync(a => a.PermitId == id);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the permit for id = " + id + ".", ex);
			}
			return result;
		}

		public async Task<v_Projects> GetProjectByIdAsync(Guid id)
		{
			v_Projects result = null;
			try
			{
				if (cwmContext.v_Projects.Select(a => a.ProjectId).ToArray().Contains(id))
				{
					result = await cwmContext.v_Projects.SingleOrDefaultAsync(a => a.ProjectId == id);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the project for id = " + id + ".", ex);
			}
			return result;
		}

		private void CreateCheckListItems(List<CheckItemModel> checkItems, Guid inspectionId)
		{
			//Need to Add Audits
			//bool isNew = false;
			CheckItemValue newCheckItem;

			foreach (var checkItem in checkItems)
			{
				ActivityCheckList activityCheckList = cwmContext.ActivityCheckLists.FirstOrDefault(ac => ac.ActivityId == inspectionId && ac.CheckListId == checkItem.CheckListId);
				if (activityCheckList == null || activityCheckList.ActivityId == Guid.Empty)
				{

					activityCheckList = cwmContext.ActivityCheckLists.Add(new ActivityCheckList());
					activityCheckList.ActivityId = inspectionId;
					activityCheckList.CheckListId = checkItem.CheckListId;
					activityCheckList.rowguid = Guid.NewGuid();
					activityCheckList.DateUpdated = DateTime.Now;
					activityCheckList.DateInserted = activityCheckList.DateUpdated;
					((DbContext)cwmContext).SaveChanges();
				}
			}

			foreach (var checkItem in checkItems)
			{
				if (checkItem.BooleanValue != null || checkItem.TextValue != null || checkItem.ResolutionText != null)
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
						newCheckItem = new CheckItemValue();
						newCheckItem = cwmContext.CheckItemValues.FirstOrDefault(c => c.CheckItemValueId == checkItem.CheckItemValueId);
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
				else
				{
					if (checkItem.CheckItemValueId != null && checkItem.CheckItemValueId != Guid.Empty)
					{
						newCheckItem = cwmContext.CheckItemValues.FirstOrDefault(c => c.CheckItemValueId == checkItem.CheckItemValueId);
						cwmContext.CheckItemValues.Remove(newCheckItem);
					}
				}
			}
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

		public async Task<bool> AddRelatedAddress(Guid id, Guid relId)
		{
			bool result = true;
			try
			{
				var relAdd = await cwmContext.Addresses.FirstAsync(a => a.AddressId == relId);
				relAdd.ParentAddressId = id;

				var audit = new AuditModel { TableName = "Addresses", RecordId = id, AuditAction = "RECORD UPDATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();
				auditFields.Add(new AuditFieldModel { ControlName = "Addresses", FieldDesc = "Add Related Address", OldId = null, OldValue = null, NewId = relId, NewValue = (await cwmContext.v_Addresses2.FirstAsync(a => a.AddressId == relId)).FullAddress });

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
						logger.Error("Unable to add parent address for address '" + id.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to add parent address for address '" + id.ToString() + "', DbContext was not available.");
				}

			}
			catch (Exception)
			{
				result = false;
				throw;
			}
			return result;

		}

		public async Task<AddressSetting> GetAddressSettingAsync(Guid AddressTypeId)
		{
			AddressSetting result = null;
			try
			{
				result = await cwmContext.AddressSettings.SingleOrDefaultAsync(a => a.AddressTypeId == AddressTypeId);
				if (result == null)
				{
					result = await cwmContext.AddressSettings.SingleOrDefaultAsync(a => a.AddressTypeId == AddressTypeId);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address Settings for Address Id: " + AddressTypeId + ".", ex);
			}
			return result;
		}

		public async Task<IEnumerable<v_Addresses2>> GetHydrantAddresses(Guid addressId)
		{
			bool saveAdd = false;
			string lat = null;
			string lon = null;
			List<v_Addresses2> result = new List<v_Addresses2>();
			//WEL - NMSFM - 11/20/2019 - the dist variable probably needs to come from a setting.
			double dist = 500;
			var R = 6371e3; // metres
							//WEL - NMSFM - 11/20/2019 - Change the contians to ge the proper Hydrant Address Id. This might even be a setting.
			var hydrantTypeId = (await cwmContext.AddressTypes.FirstOrDefaultAsync(at => at.AddressType1.Contains("Hydrant"))).AddressTypeId;
			var mainAddress = (await cwmContext.v_Addresses2.FirstOrDefaultAsync(add => add.AddressId == addressId));
			var hydrAdd = (await cwmContext.v_Addresses2.Where(add => add.AddressTypeId == hydrantTypeId).ToListAsync());

			if (hydrAdd.Count > 0 && (mainAddress.Latitude == null || mainAddress.Latitude == ""))
			{
				GeocodeAddress(mainAddress, out lat, out lon);

				if (lat != null && lat != "")
				{
					mainAddress.Latitude = lat;
					mainAddress.Longitude = lon;
					saveAdd = true;
				}
				else
				{
					return result;
				}

			}



			//var φ1 = lat1.toRadians();
			var φ1 = (Convert.ToDouble(mainAddress.Latitude)) * (Math.PI / 180);
			foreach (Data.v_Addresses2 addr in hydrAdd)
			{
				if (addr.AddressId != mainAddress.AddressId)
				{
					if (addr.Latitude != null && addr.Latitude != "")
					{
						//var φ2 = lat2.toRadians();
						var φ2 = (Convert.ToDouble(addr.Latitude)) * (Math.PI / 180);

						//var Δφ = (lat2 - lat1).toRadians();
						var Δφ = (Convert.ToDouble(addr.Latitude) - Convert.ToDouble(mainAddress.Latitude)) * (Math.PI / 180);
						//var Δλ = (lon2 - lon1).toRadians();
						var Δλ = (Convert.ToDouble(addr.Longitude) - Convert.ToDouble(mainAddress.Longitude)) * (Math.PI / 180);

						var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
								Math.Cos(φ1) * Math.Cos(φ2) *
								Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
						var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

						var d = R * c;
						if (d <= dist)
						{
							result.Add(addr);
						}
					}
				}
			}

			if (result.Count() > 0 && saveAdd)
			{
				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to update lat lon for address '" + mainAddress.AddressId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to update lat lon for address '" + mainAddress.AddressId.ToString() + "', DbContext was not available.");
				}
			}

			return result;
		}

		public async Task<IEnumerable<cv_CPTKHotlist>> GetHotlist()
		{
			var result = await cwmContext.cv_CPTKHotlist.ToListAsync();

			return result;
		}

		public void GeocodeAddress(v_Addresses2 address, out string lat, out string lon)
		{
			//string address = "123 something st, somewhere";

			string codedAddress = "";

			if (address.Address != null)
			{
				if (address.AddressNumber != null)
				{
					codedAddress += address.AddressNumber + " ";
				}
				if (address.Direction != null)
				{
					codedAddress += address.Direction + " ";
				}
				codedAddress += address.Address + " ";
				if (address.Suffix != null)
				{
					codedAddress += address.Suffix + ", ";
				}
				codedAddress += address.City + ", " + address.StateAbbr;

			}
			else
			{
				codedAddress = address.City + ", " + address.StateAbbr;

			}
			if (address.Zip != null)
			{
				codedAddress += " " + address.Zip;
			}

			string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(codedAddress), googleKey);

			WebRequest request = WebRequest.Create(requestUri);
			WebResponse response = request.GetResponse();
			XDocument xdoc = XDocument.Load(response.GetResponseStream());

			XElement result = xdoc.Element("GeocodeResponse").Element("result");
			if (result != null)
			{
				XElement locationElement = result.Element("geometry").Element("location");
				lat = locationElement.Element("lat").ToString().Replace("<lat>", "").Replace("</lat>", "");
				lon = locationElement.Element("lng").ToString().Replace("<lng>", "").Replace("</lng>", "");
			}
			else
			{
				lat = "";
				lon = "";
			}
		}

		public async Task<bool> UpdateAddressInactive(Guid addressId, string value)
		{
			bool result = false;

			var address = await cwmContext.Addresses.FirstAsync(a => a.AddressId == addressId);
			var audit = new AuditModel { TableName = "Addresses", RecordId = addressId, AuditAction = "RECORD UPDATED", Description = "" };
			var auditFields = new List<AuditFieldModel>();
			auditFields.Add(new AuditFieldModel { ControlName = "Inactive", FieldDesc = "Inactive", OldId = null, OldValue = address.Inactive.ToString(), NewId = null, NewValue = value});
			address.Inactive = Convert.ToBoolean(value);

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
					logger.Error("Unable to change address inactive for address '" + addressId.ToString() + "'.", ex);
				}
			}
			else
			{
				logger.Error("Unable to change address inactive for address '" + addressId.ToString() + "', DbContext was not available.");
			}
			return result;
		}

	}
}

