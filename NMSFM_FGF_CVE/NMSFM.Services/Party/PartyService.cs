using NMSFM.Data;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NMSFM.Services.Models;
using NMSFM.Services.Audit;
using log4net;
using NMSFM.Services.Logging;
using NMSFM.ViewModels;

namespace NMSFM.Services.Party
{
	public class PartyService : IPartyService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;

		public PartyService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
		}

		public async Task<IEnumerable<v_Parties>> GetPartiesAsync()
		{
			IEnumerable<v_Parties> result;
			try
			{
				var parties = await cwmContext.v_Parties.Where(a => a.Inactive == false).ToListAsync();
				var roleTypeList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
				if (parties != null && parties.Count() > 0 & roleTypeList != null & roleTypeList.Count() > 0)
				{
					parties = parties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList();
				}
				result = parties;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address List.", ex);
				result = new List<v_Parties>();
			}
			return result;
		}

		public async Task<IEnumerable<AddressParty>> GetAddressPartiesAsync()
		{
			IEnumerable<AddressParty> result;
			try
			{
				var addressParties = await cwmContext.AddressParties.Where(p => !p.Inactive).ToListAsync();
				var roleTypeList = await cwmContext.RoleTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.RoleTypeId).ToListAsync();
				if (addressParties != null && addressParties.Count() > 0 & roleTypeList != null & roleTypeList.Count() > 0)
				{
					addressParties = addressParties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList();
				}
				result = addressParties;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unxpected exception caught while retrieving the Address Party List.", ex);
				result = new List<AddressParty>();
			}
			return result;
		}
		public async Task<v_AddressParties> GetAddressPartyByNameAsync(String name)
		{
			v_AddressParties result = null;
			try
			{
				result = await cwmContext.v_AddressParties.SingleOrDefaultAsync(a => a.PartyName == name);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party '" + name.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<v_AddressParties> GetAddressPartyByIdAsync(Guid partyId, Guid addressId)
		{
			v_AddressParties result = null;
			try
			{
				result = await cwmContext.v_AddressParties.SingleOrDefaultAsync(a => a.PartyID == partyId && a.AddressId == addressId);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party '" + partyId.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<v_AddressParties> GetAddressPartyRoleByIdAsync(Guid partyId, Guid addressId, Guid roleTypeId)
		{
			v_AddressParties result = null;
			try
			{
				result = await cwmContext.v_AddressParties.SingleOrDefaultAsync(a => a.PartyID == partyId && a.AddressId == addressId && a.RoleTypeId == roleTypeId);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party '" + partyId.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<v_AddressParties> GetAddressPartyByAddressPartyRoleAsync(Guid addressId, Guid roleTypeId)
		{
			v_AddressParties result = null;
			try
			{
				result = await cwmContext.v_AddressParties.FirstOrDefaultAsync(a => a.AddressId == addressId && a.RoleTypeId == roleTypeId && a.Inactive == false);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Fire Department Party for '" + addressId.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<v_Parties> GetPartyByIdAsync(Guid id)
		{
			v_Parties result = null;
			try
			{
				result = await cwmContext.v_Parties.SingleOrDefaultAsync(a => a.PartyID == id);
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Party '" + id.ToString() + "'.", ex);
			}
			return result;
		}

		public async Task<List<Phone>> GetPhoneListForPartyAsync(Guid id)
		{
			var results = new List<Phone>();
			try
			{
				var phoneTypes = (await GetPhoneTypeListAsync()).Select(a => a.PhoneTypeId);
				results = await cwmContext.Phones.Where(a => a.PartyId == id && phoneTypes.Contains(a.PhoneTypeId)).OrderBy(a => a.Sequence).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Phone list for Party '" + id.ToString() + "'.", ex);
			}
			return results;
		}

		public async Task<IEnumerable<RoleType>> GetRoleTypeListAsync()
		{
			IEnumerable<RoleType> result;
			try
			{
				result = await cwmContext.RoleTypes.Where(a => !a.Inactive && a.WebViewable).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Role Type List.", ex);
				result = new List<RoleType>();
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

		public async Task<IEnumerable<PhoneType>> GetPhoneTypeListAsync()
		{
			IEnumerable<PhoneType> results = null;
			try
			{
				results = await cwmContext.PhoneTypes.Where(a => a.Inactive == false && a.WebViewable == true).OrderBy(a => a.Sequence).ToListAsync();
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Phone Type list.", ex);
			}
			return results;
		}

		public async Task CreatePhonesAsync(List<Phone> phoneList)
		{
			for (int i = 0; i < phoneList.Count; i++)
			{
				var phone = phoneList[i];
				var audit = new AuditModel { TableName = "Phones", RecordId = phone.PhoneId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();
				auditFields.Add(new AuditFieldModel { ControlName = "PhoneId", FieldDesc = "", OldId = null, OldValue = null, NewId = phone.PhoneId, NewValue = null });

				if (phone.PhoneTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "PhoneType", FieldDesc = "Phone Type", OldId = null, OldValue = null, NewId = phone.PhoneTypeId, NewValue = null });
				}
				if (phone.Phone1 != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Phone", FieldDesc = "Phone", OldId = null, OldValue = null, NewId = null, NewValue = phone.Phone1 });
				}
				if (phone.Extension != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Extension", FieldDesc = "Extension", OldId = null, OldValue = null, NewId = null, NewValue = phone.Extension });
				}
				if (phone.Sequence != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "Sequence", FieldDesc = "Sequence", OldId = null, OldValue = null, NewId = null, NewValue = phone.Sequence.ToString() });
				}
				if (phone.PartyId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "PartyId", FieldDesc = "Party Id", OldId = null, OldValue = null, NewId = phone.PartyId, NewValue = null });
				}

				cwmContext.Phones.Add(phone);
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
						logger.Error("Unable to create phone '" + phone.PhoneId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create phone '" + phone.PhoneId.ToString() + "', DbContext was not available.");
				}
			}
		}

		public async Task UpdatePhonesAsync(List<Phone> phoneList)
		{
			for (int i = 0; i < phoneList.Count(); i++)
			{
				var phoneId = phoneList[i].PhoneId;
				var phone = cwmContext.Phones.SingleOrDefault(a => a.PhoneId == phoneId);
				if (phone != null)
				{
					var audit = new AuditModel { TableName = "Phone", RecordId = phone.PhoneId, AuditAction = "RECORD UPDATED", Description = "" };
					var auditFields = new List<AuditFieldModel>();
					if (phoneList[i].PhoneTypeId != phone.PhoneTypeId)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "PhoneType", FieldDesc = "Phone Type", OldId = phone.PhoneTypeId, OldValue = null, NewId = phoneList[i].PhoneTypeId, NewValue = null });
						phone.PhoneTypeId = phoneList[i].PhoneTypeId;
						phone.DateUpdated = DateTime.Now;
					}
					if (phoneList[i].Phone1 != phone.Phone1)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "Phone1", FieldDesc = "Phone", OldId = null, OldValue = phone.Phone1, NewId = null, NewValue = phoneList[i].Phone1 });
						phone.Phone1 = phoneList[i].Phone1;
						phone.DateUpdated = DateTime.Now;
					}
					if (phoneList[i].Extension != phone.Extension)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "Extension", FieldDesc = "Extension", OldId = null, OldValue = phone.Extension, NewId = null, NewValue = phoneList[i].Extension });
						phone.Extension = phoneList[i].Extension;
						phone.DateUpdated = DateTime.Now;
					}
					if (phoneList[i].Sequence != phone.Sequence)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "Sequence", FieldDesc = "Sequence", OldId = null, OldValue = phone.Sequence.ToString(), NewId = null, NewValue = phoneList[i].Sequence.ToString() });
						phone.Sequence = phoneList[i].Sequence;
						phone.DateUpdated = DateTime.Now;
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
							logger.Error("Unable to update phone '" + phone.PhoneId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to update phone '" + phone.PhoneId.ToString() + "', DbContext was not available.");
					}
				}
			}
		}

		public async Task DeletePhonesAsync(Guid partyId, List<Guid> phoneIdList)
		{
			var recordsToDelete = cwmContext.Phones.Where(a => !phoneIdList.Contains(a.PhoneId) && a.PartyId == partyId).ToList();
			if (recordsToDelete != null && recordsToDelete.Count > 0)
			{
				for (int i = 0; i < recordsToDelete.Count(); i++)
				{
					var audit = new AuditModel { TableName = "Phones", RecordId = recordsToDelete[i].PhoneId, AuditAction = "RECORD DELETED", Description = "" };
					var auditFields = new List<AuditFieldModel>();
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = recordsToDelete[i].PhoneId, OldValue = null, NewId = null, NewValue = null });
					cwmContext.Phones.Remove(recordsToDelete[i]);
					if (auditFields.Count() > 0)
					{
						await auditService.UpdateAudit(audit, auditFields);
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
						logger.Error("Unable to remove phones.", ex);
					}
				}
				else
				{
					logger.Error("Unable to remove phones, DbContext was not available.");
				}
			}
		}

		public async Task RemoveAddressParty(v_AddressParties model)
		{
			if (model != null)
			{
				var result = cwmContext.AddressParties.SingleOrDefault(a => a.AddressPartyId == model.AddressPartyId);
				if (result != null)
				{
					var audit = new AuditModel { TableName = "AddressParties", RecordId = model.AddressPartyId.Value, AuditAction = "RECORD DELETED", Description = "" };
					var auditFields = new List<AuditFieldModel>();
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = model.AddressPartyId, OldValue = null, NewId = null, NewValue = null });
					cwmContext.AddressParties.Remove(result);
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
							logger.Error("Unable to remove party address '" + model.AddressPartyId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to remove party address '" + model.AddressPartyId.ToString() + "', DbContext was not available.");
					}
				}
			}
			else
			{
				logger.Error("RemoveAddressParty was called with a null reference.");
			}
		}

		public async Task<Guid> AttachExistingParty(v_AddressParties model)
		{
			if (model != null)
			{
				IEnumerable<AddressParty> result = await cwmContext.AddressParties.Where(a => a.AddressID == model.AddressId && a.PartyID == model.PartyID).ToListAsync();
				if (result.Count() > 0)
				{
					//logger.Error("Unable to create  address party '" + model.PartyName.ToString() + "'.  The used name is already in the database for this address.");
					return Guid.Empty;
				}

				var addressParty = new Data.AddressParty();
				cwmContext.AddressParties.Add(addressParty);
				addressParty.AddressPartyId = Guid.NewGuid();

				var audit = new AuditModel { TableName = "AddressParties", RecordId = addressParty.AddressPartyId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();
				auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = addressParty.AddressPartyId, NewValue = null });

				if (model.PartyID != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Party ID", OldId = null, OldValue = null, NewId = model.PartyID, NewValue = null });
					addressParty.PartyID = model.PartyID;
				}
				if (model.AddressId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Address Id", OldId = null, OldValue = null, NewId = model.AddressId, NewValue = null });
					addressParty.AddressID = model.AddressId.Value;
				}
				if (model.RoleTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId, NewValue = null });
					addressParty.RoleTypeId = model.RoleTypeId;
				}

				addressParty.rowguid = Guid.NewGuid();
				addressParty.Inactive = false;
				addressParty.ExternalId = null;
				addressParty.ExternalValue = null;
				addressParty.DateUpdated = DateTime.Now;
				addressParty.DateInserted = DateTime.Now;

				if (cwmContext is DbContext)
				{
					try
					{
						await ((DbContext)cwmContext).SaveChangesAsync();
						if (auditFields.Count() > 0)
						{
							await auditService.UpdateAudit(audit, auditFields);
							return addressParty.AddressPartyId;
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "', DbContext was not available.");
				}

			}
			else
			{
				logger.Error("AttachExistingParty was called with a null reference.");
			}
			return Guid.Empty;
		}

		public async Task AttachExistingParty(DetailedAddressParty model)
		{
			if (model != null)
			{
				IEnumerable<AddressParty> result = await cwmContext.AddressParties.Where(a => a.AddressID == model.AddressId && a.PartyID == model.PartyId).ToListAsync();
				if (result.Count() > 0)
				{
					//logger.Error("Unable to create  address party '" + model.PartyName.ToString() + "'.  The used name is already in the database for this address.");
					return;
				}

				var addressParty = new Data.AddressParty();
				cwmContext.AddressParties.Add(addressParty);
				addressParty.AddressPartyId = Guid.NewGuid();

				var audit = new AuditModel { TableName = "AddressParties", RecordId = addressParty.AddressPartyId, AuditAction = "RECORD CREATED", Description = "" };
				var auditFields = new List<AuditFieldModel>();
				auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = addressParty.AddressPartyId, NewValue = null });

				if (model.PartyId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Party ID", OldId = null, OldValue = null, NewId = model.PartyId, NewValue = null });
					addressParty.PartyID = model.PartyId.Value;
				}
				if (model.AddressId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Address Id", OldId = null, OldValue = null, NewId = model.AddressId, NewValue = null });
					addressParty.AddressID = model.AddressId.Value;
				}
				if (model.RoleTypeId != null)
				{
					auditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId, NewValue = null });
					addressParty.RoleTypeId = model.RoleTypeId;
				}

				addressParty.rowguid = Guid.NewGuid();
				addressParty.Inactive = false;
				addressParty.ExternalId = null;
				addressParty.ExternalValue = null;
				addressParty.DateUpdated = DateTime.Now;
				addressParty.DateInserted = DateTime.Now;

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
						logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "', DbContext was not available.");
				}

			}
			else
			{
				logger.Error("AttachExistingParty was called with a null reference.");
			}
		}

		//public async Task CreatePartyAsync(v_AddressParties model) // Needs Audit
		public async Task CreatePartyAsync(AttachAddressParty model) // Needs Audit
		{
			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  
				if (model.AddressPartyId != null && model.AddressPartyId != Guid.Empty) // The user is editing an existing party
				{
					var editParty = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyID == model.PartyId);
					if (editParty == null)
					{
						logger.Error("Unable to update party '" + model.PartyId.ToString() + "'.  The party could not be located in the database.");
						return;
					}

					var audit = new AuditModel { TableName = "Parties", RecordId = model.PartyId, AuditAction = "RECORD UPDATED", Description = "" };
					var auditFields = new List<AuditFieldModel>();

					if (editParty.PartyName != model.PartyName)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "PartyName", FieldDesc = "Party Name", OldId = null, OldValue = editParty.PartyName, NewId = null, NewValue = model.PartyName });
						editParty.PartyName = model.PartyName;
					}

					if (editParty.Email != model.Email)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = editParty.Email, NewId = null, NewValue = model.Email });
						editParty.Email = model.Email;
					}

					if (editParty.Comment != model.Comment)
					{
						auditFields.Add(new AuditFieldModel { ControlName = "Comment", FieldDesc = "Comment", OldId = null, OldValue = editParty.Comment, NewId = null, NewValue = model.Comment });
						editParty.Comment = model.Comment;
					}

					if (model.PartyType == "ind")
					{
						if (editParty.Salutation != model.Salutation)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "Salutation", FieldDesc = "Salutation", OldId = null, OldValue = editParty.Salutation, NewId = null, NewValue = model.Salutation });
							editParty.Salutation = model.Salutation;
						}

						if (editParty.FirstName != model.FirstName)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "FirstName", FieldDesc = "FirstName", OldId = null, OldValue = editParty.FirstName, NewId = null, NewValue = model.FirstName });
							editParty.FirstName = model.FirstName;
						}

						if (editParty.MiddleInitial != model.MiddleInitial)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "MiddleInitial", FieldDesc = "MiddleInitial", OldId = null, OldValue = editParty.MiddleInitial, NewId = null, NewValue = model.MiddleInitial });
							editParty.MiddleInitial = model.MiddleInitial;
						}

						if (editParty.LastName != model.LastName)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "LastName", FieldDesc = "LastName", OldId = null, OldValue = editParty.LastName, NewId = null, NewValue = model.LastName });
							editParty.LastName = model.LastName;
						}

						if (editParty.Suffix != model.Suffix)
						{
							auditFields.Add(new AuditFieldModel { ControlName = "Suffix", FieldDesc = "Suffix", OldId = null, OldValue = editParty.Suffix, NewId = null, NewValue = model.Suffix });
							editParty.Suffix = model.Suffix;
						}
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
							logger.Error("Unable to update party '" + model.PartyId.ToString() + "'.", ex);
							return;
						}
					}
					else
					{
						logger.Error("Unable to update party '" + model.PartyId.ToString() + "', DbContext was not available.");
						return;
					}

					var editAddressParty = await cwmContext.AddressParties.SingleOrDefaultAsync(a => a.AddressPartyId == model.AddressPartyId);
					if (editAddressParty == null)
					{
						logger.Error("Unable to update address party '" + model.AddressPartyId.ToString() + "'.  The  address party could not be located in the database.");
						return;
					}

					audit = new AuditModel { TableName = "AddressParties", RecordId = model.AddressPartyId, AuditAction = "RECORD UPDATED", Description = "" };
					auditFields = new List<AuditFieldModel>();
					var partyRoleAudit = new AuditModel { TableName = "PartyRoles", RecordId = model.AddressPartyId, AuditAction = "RECORD CREATED", Description = "" };
					var partyRoleAuditFields = new List<AuditFieldModel>();

					if ((editAddressParty.RoleTypeId ?? Guid.Empty) != (model.RoleTypeId ?? Guid.Empty))
					{
						auditFields.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = editAddressParty.RoleTypeId.Value, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
						if ((model.RoleTypeId ?? Guid.Empty) != Guid.Empty)
						{
							var partyRole = cwmContext.PartyRoles.Where(a => a.PartyId == model.PartyId).First();
							if (partyRole == null)
							{
								partyRole = cwmContext.PartyRoles.Add(new Data.PartyRole() { PartyRoleId = Guid.NewGuid(), PartyId = model.PartyId, RoleTypeId = model.RoleTypeId.Value, rowguid = Guid.NewGuid(), ExternalId = null, DateUpdated = DateTime.Now, DateInserted = DateTime.Now });
								partyRoleAuditFields.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
							}
						}
					}

					editAddressParty.RoleTypeId = model.RoleTypeId;

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (auditFields.Count() > 0)
							{
								await auditService.UpdateAudit(audit, auditFields);
							}
							if (partyRoleAuditFields.Count() > 0)
							{
								await auditService.UpdateAudit(partyRoleAudit, partyRoleAuditFields);
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to update address party '" + model.AddressPartyId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to update address party '" + model.AddressPartyId.ToString() + "', DbContext was not available.");
					}
					return;
				}

				// The user is saving a new party
				var party = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyName == model.PartyName);

				if (party != null)
				{
					logger.Error("Unable to create party '" + model.PartyName.ToString() + "'.  The used name is already in the database.");
					return;
				}

				var newAudit = new AuditModel { TableName = "Parties", RecordId = model.PartyId, AuditAction = "RECORD CREATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				party = cwmContext.Parties.Add(new Data.Party());
				party.rowguid = Guid.NewGuid();
				party.ExternalId = null;
				party.Inactive = false;
				party.WebUserId = null;
				party.QBCustomerListID = null;
				party.InspectorId = null;
				party.PartyImage = null;
				party.Signature = null;
				party.DateUpdated = DateTime.Now;
				party.DateInserted = DateTime.Now;
				party.PriceLevel = null;
				party.Salutation = null;
				party.FirstName = null;
				party.MiddleInitial = null;
				party.Suffix = null;

				newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = model.PartyId, NewValue = null });
				party.PartyID = model.PartyId;

				if (model.PartyName != null && model.PartyName != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "PartyName", FieldDesc = "Party Name", OldId = null, OldValue = null, NewId = null, NewValue = model.PartyName });
					party.PartyName = model.PartyName;
				}

				if (party.Email != model.Email && model.Email != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = null, NewId = null, NewValue = model.Email });
					party.Email = model.Email;
				}

				if (party.Comment != model.Comment && model.Comment != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Comment", FieldDesc = "Comment", OldId = null, OldValue = null, NewId = null, NewValue = model.Comment });
					party.Comment = model.Comment;
				}

				if (model.PartyType == "ind")
				{
					if (party.Salutation != model.Salutation)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "Salutation", FieldDesc = "Salutation", OldId = null, OldValue = null, NewId = null, NewValue = model.Salutation });
						party.Salutation = model.Salutation;
					}

					if (party.FirstName != model.FirstName)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "FirstName", FieldDesc = "FirstName", OldId = null, OldValue = null, NewId = null, NewValue = model.FirstName });
						party.FirstName = model.FirstName;
					}

					if (party.MiddleInitial != model.MiddleInitial)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "MiddleInitial", FieldDesc = "MiddleInitial", OldId = null, OldValue = null, NewId = null, NewValue = model.MiddleInitial });
						party.MiddleInitial = model.MiddleInitial;
					}

					if (party.LastName != model.LastName)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "LastName", FieldDesc = "LastName", OldId = null, OldValue = null, NewId = null, NewValue = model.LastName });
						party.LastName = model.LastName;
					}

					if (party.Suffix != model.Suffix)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "Suffix", FieldDesc = "Suffix", OldId = null, OldValue = null, NewId = null, NewValue = model.Suffix });
						party.Suffix = model.Suffix;
					}
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
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create party '" + model.PartyName.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create party '" + model.PartyName.ToString() + "', DbContext was not available.");
				}

				if  (model.AddressId != null && model.AddressId.ToString() != "")
                {
					var addressParty = cwmContext.AddressParties.Add(new Data.AddressParty());
					addressParty.AddressPartyId = Guid.NewGuid();

					newAudit = new AuditModel { TableName = "AddressParties", RecordId = addressParty.AddressPartyId, AuditAction = "RECORD CREATED", Description = "" };
					newAuditFields = new List<AuditFieldModel>();
					newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = addressParty.AddressPartyId, NewValue = null });

					if (model.PartyId != null && model.PartyId != Guid.NewGuid())
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "PartyID", FieldDesc = "Party ID", OldId = null, OldValue = null, NewId = model.PartyId, NewValue = null });
						addressParty.PartyID = model.PartyId;
					}

					if (model.AddressId != null && model.AddressId != Guid.NewGuid())
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Address Id", OldId = null, OldValue = null, NewId = model.AddressId, NewValue = null });
						addressParty.AddressID = model.AddressId;
					}

					var partyRoleAudit2 = new AuditModel { TableName = "PartyRoles", RecordId = model.PartyId, AuditAction = "RECORD CREATED", Description = "" };
					var partyRoleAuditFields2 = new List<AuditFieldModel>();
					if (model.RoleTypeId != null)
					{
						newAuditFields.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
						partyRoleAuditFields2.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
						addressParty.RoleTypeId = model.RoleTypeId;
						var partyRole = cwmContext.PartyRoles.Add(new Data.PartyRole() { PartyRoleId = Guid.NewGuid(), PartyId = model.PartyId, RoleTypeId = model.RoleTypeId.Value, rowguid = Guid.NewGuid(), ExternalId = null, DateUpdated = DateTime.Now, DateInserted = DateTime.Now });
					}

					addressParty.rowguid = Guid.NewGuid();
					addressParty.Inactive = false;
					addressParty.ExternalId = null;
					addressParty.ExternalValue = null;
					addressParty.DateUpdated = DateTime.Now;
					addressParty.DateInserted = DateTime.Now;

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();
							if (newAuditFields.Count() > 0)
							{
								await auditService.UpdateAudit(newAudit, newAuditFields);
							}
							if (partyRoleAuditFields2.Count() > 0)
							{
								await auditService.UpdateAudit(partyRoleAudit2, partyRoleAuditFields2);
							}
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "', DbContext was not available.");
					}
				}
			}
			else
			{
				logger.Error("CreateParty was called with a null reference.");
			}
		}

		public async Task<Guid> CreatePartyAsync(DetailedAddressParty model) // Needs Audit
		{
			Guid result = Guid.Empty;

			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  

				// The user is saving a new party								
				var party = cwmContext.Parties.Add(new Data.Party());
				party.PartyID = new Guid(model.PartyId.ToString());
				var newAudit = new AuditModel { TableName = "Parties", RecordId = party.PartyID, AuditAction = "RECORD CREATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				party.rowguid = Guid.NewGuid();
				party.ExternalId = null;
				party.Inactive = false;
				party.WebUserId = null;
				party.QBCustomerListID = null;
				party.InspectorId = null;
				party.PartyImage = null;
				party.Signature = null;
				party.DateUpdated = DateTime.Now;
				party.DateInserted = DateTime.Now;
				party.PriceLevel = null;
				party.Salutation = null;
				party.FirstName = null;
				party.MiddleInitial = null;
				party.Suffix = null;
				party.FromWeb = model.FromWeb;

				newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = model.PartyId, NewValue = null });

				if (model.PartyName != null && model.PartyName != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "PartyName", FieldDesc = "Party Name", OldId = null, OldValue = null, NewId = null, NewValue = model.PartyName });
					party.PartyName = model.PartyName;
				}

				if (party.Email != model.Email && model.Email != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = null, NewId = null, NewValue = model.Email });
					party.Email = model.Email;
				}

				if (model.Phone != null && model.Phone != "")
				{
					//Add Phone.
					var phone = cwmContext.Phones.Add(new Phone());
					phone.PhoneId = Guid.NewGuid();
					phone.PhoneTypeId = new Guid("32b3745f-2d04-4839-9afe-2b138cae74a1");
					phone.Phone1 = model.Phone;
					phone.PartyId = party.PartyID;
					phone.Sequence = await cwmContext.Phones.Where(p => p.PartyId == party.PartyID).CountAsync() + 1;
					phone.rowguid = Guid.NewGuid();
					phone.DateUpdated = DateTime.Now;
					phone.DateInserted = DateTime.Now;
				}

				if (cwmContext is DbContext)
				{
					try
					{
						((DbContext)cwmContext).SaveChangesAsync().Wait();						
						if (newAuditFields.Count() > 0)
						{
							await auditService.UpdateAudit(newAudit, newAuditFields);
						}
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to create party '" + model.PartyName.ToString() + "'.", ex);
					}


					var addressParty = cwmContext.AddressParties.Add(new Data.AddressParty());
					addressParty.AddressPartyId = Guid.NewGuid();

					var newAudit2 = new AuditModel { TableName = "AddressParties", RecordId = addressParty.AddressPartyId, AuditAction = "RECORD CREATED", Description = "" };
					var newAuditFields2 = new List<AuditFieldModel>();
					newAuditFields2.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = addressParty.AddressPartyId, NewValue = null });

					if (party.PartyID != null && party.PartyID != Guid.NewGuid())
					{
						newAuditFields2.Add(new AuditFieldModel { ControlName = "PartyID", FieldDesc = "Party ID", OldId = null, OldValue = null, NewId = model.PartyId, NewValue = null });
						addressParty.PartyID = party.PartyID;
					}

					if (model.AddressId != null && model.AddressId != Guid.NewGuid())
					{
						newAuditFields2.Add(new AuditFieldModel { ControlName = "", FieldDesc = "Address Id", OldId = null, OldValue = null, NewId = model.AddressId, NewValue = null });
						addressParty.AddressID = model.AddressId.Value;
					}

					var partyRoleAudit2 = new AuditModel { TableName = "PartyRoles", RecordId = party.PartyID, AuditAction = "RECORD CREATED", Description = "" };
					var partyRoleAuditFields2 = new List<AuditFieldModel>();
					if (model.RoleTypeId != null)
					{
						newAuditFields2.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
						partyRoleAuditFields2.Add(new AuditFieldModel { ControlName = "RoleTypeId", FieldDesc = "Role Type Id", OldId = null, OldValue = null, NewId = model.RoleTypeId.Value, NewValue = null });
						addressParty.RoleTypeId = model.RoleTypeId;
						var partyRole = cwmContext.PartyRoles.Add(new Data.PartyRole() { PartyRoleId = Guid.NewGuid(), PartyId = party.PartyID, RoleTypeId = model.RoleTypeId.Value, rowguid = Guid.NewGuid(), ExternalId = null, DateUpdated = DateTime.Now, DateInserted = DateTime.Now });
					}

					addressParty.rowguid = Guid.NewGuid();
					addressParty.Inactive = false;
					addressParty.ExternalId = null;
					addressParty.ExternalValue = null;
					addressParty.DateUpdated = DateTime.Now;
					addressParty.DateInserted = DateTime.Now;

					if (cwmContext is DbContext)
					{
						try
						{
							await ((DbContext)cwmContext).SaveChangesAsync();

							if (newAuditFields.Count() > 0)
							{
								await auditService.UpdateAudit(newAudit, newAuditFields);
							}
							if (newAuditFields2.Count() > 0)
							{
								await auditService.UpdateAudit(newAudit2, newAuditFields2);
							}
							if (partyRoleAuditFields2.Count() > 0)
							{
								await auditService.UpdateAudit(partyRoleAudit2, partyRoleAuditFields2);
							}

							return party.PartyID;
						}
						catch (Exception ex)
            {
                _ = ex;
							logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "'.", ex);
						}
					}
					else
					{
						logger.Error("Unable to create party address '" + addressParty.AddressPartyId.ToString() + "', DbContext was not available.");
					}
				}
				else
				{
					logger.Error("Unable to create party '" + model.PartyName.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("CreateParty was called with a null reference.");
			}
			return result;
		}

		public async Task UpdateParty(Data.Party model) // Needs Audit
		{
			Guid result = Guid.Empty;

			if (model != null)
			{
				// The DbContext is not tracking changes from AutoMapper.  

				// The user is updating a new party							
				var party = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyID == model.PartyID);
				if (party == null)
				{
					logger.Error("Unable to update party '" + model.PartyName.ToString() + "'.  The party name is not in the database.");
					return;
				}

				party.PartyID = new Guid(model.PartyID.ToString());
				var newAudit = new AuditModel { TableName = "Parties", RecordId = party.PartyID, AuditAction = "RECORD UPDATED", Description = "" };
				var newAuditFields = new List<AuditFieldModel>();

				newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = model.PartyID, NewValue = null });

				if (model.PartyName != null && model.PartyName != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "PartyName", FieldDesc = "Party Name", OldId = null, OldValue = null, NewId = null, NewValue = model.PartyName });
					party.PartyName = model.PartyName;
				}

				if (party.Email != model.Email && model.Email != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = null, NewId = null, NewValue = model.Email });
					party.Email = model.Email;
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
					}
					catch (Exception ex)
            {
                _ = ex;
						logger.Error("Unable to update party '" + model.PartyName.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to update party '" + model.PartyName.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("UpdateParty was called with a null reference.");
			}
			return;
		}


		public async Task<IEnumerable<v_Parties>> PerformSearchAsync(Guid roleTypeId, string partyName, bool hideInactive)
		{
			IEnumerable<v_Parties> result;
			try
			{

				var parties = await cwmContext.v_Parties.ToListAsync();

				if (hideInactive)
				{
					parties = parties.Where(p => p.Inactive == false).ToList();
				}

				if (partyName != null && partyName != "")
				{
					parties = parties.Where(p => p.PartyName != null && p.PartyName.ToUpper().Contains(partyName.ToUpper())).ToList();
				}
				var roleTypes = await cwmContext.RoleTypes.Where(a => a.Inactive == !hideInactive && a.WebViewable == true).ToListAsync();

				if (roleTypeId.ToString() != "12345678-1234-1234-1234-123456789012")
				{
					roleTypes = roleTypes.Where(r => r.RoleTypeId == roleTypeId).ToList();
				}

				var roleTypeList = roleTypes.Select(a => a.RoleTypeId);

				if (parties != null && parties.Count() > 0 & roleTypeList != null & roleTypeList.Count() > 0)
				{
					parties = parties.Where(a => roleTypeList.Contains(a.RoleTypeId == null ? Guid.Empty : a.RoleTypeId.Value)).ToList();
				}
				result = parties;
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Address List.", ex);
				result = new List<v_Parties>();
			}
			return result;
		}
	}
}

