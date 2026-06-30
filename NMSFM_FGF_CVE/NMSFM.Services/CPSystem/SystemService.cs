using NMSFM.Data;
using NMSFM.Services.Audit;
using NMSFM.Services.FireGrant;
using NMSFM.Services.Logging;
using NMSFM.Services.Models;
using NMSFM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace NMSFM.Services.CPSystem
{
	public class SystemService : ISystemService
	{
		private ICodepalWebModel cwmContext;
		private IAuditService auditService;
		private ILogging logger;

		public SystemService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
		{
			cwmContext = codepalWebModel;
			logger = codepalLogger;
			auditService = new AuditService(logger);
		}

		public async Task<string> GetCodepalSetting(string propertyField, Guid? agencyId, string userName = "")
		{
			string retval = "";
			Setting setting;
			try
			{
				if (userName != null && userName != "")
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField && s.UserName == userName);
				}
				else if (agencyId != null && agencyId != Guid.Empty)
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField && s.AgencyId == agencyId);
				}
				else
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField);
				}
				if (setting != null)
				{
					retval = setting.ValueField;
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving setting '" + propertyField + "' for Agency '" + agencyId + ".", ex);
			}
			return retval;
		}

		public async Task<bool> SaveCodepalSetting(string propertyField, string value, Guid? agencyId)
		{
			try
			{
				Setting setting = null;
				if (agencyId != null && agencyId != Guid.Empty)
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(
						s => s.PropertyField == propertyField && s.AgencyId == agencyId);
				}
				else
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(
						s => s.PropertyField == propertyField);
				}

				if (setting == null)
				{
					setting = new Setting
					{
						SettingsId = Guid.NewGuid(),
						PropertyField = propertyField,
						ValueField = value ?? string.Empty,
						AgencyId = agencyId,
						rowguid = Guid.NewGuid(),
						DateInserted = DateTime.Now,
						DateUpdated = DateTime.Now
					};
					cwmContext.Settings.Add(setting);
				}
				else
				{
					setting.ValueField = value ?? string.Empty;
					setting.DateUpdated = DateTime.Now;
				}

				if (cwmContext is DbContext)
				{
					await ((DbContext)cwmContext).SaveChangesAsync();
					return true;
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				logger.Error(
					"Unexpected exception caught while saving setting '"
					+ propertyField + "' for Agency '" + agencyId + "'.",
					ex);
			}

			return false;
		}

		public async Task<SupportEmailRecipients> GetSupportEmailRecipientsAsync(Guid agencyId)
		{
			string technical = await GetSupportSettingValueAsync(
				FireGrantSettingKeys.TechnicalSupportEmail,
				agencyId,
				"TechnicalSupportEmail");

			string fireServices = await GetSupportSettingValueAsync(
				FireGrantSettingKeys.FireServicesSupportEmail,
				agencyId,
				"FireServicesSupportEmail");

			return new SupportEmailRecipients
			{
				TechnicalSupport = technical ?? string.Empty,
				FireServicesSupport = fireServices ?? string.Empty
			};
		}

		private async Task<string> GetSupportSettingValueAsync(
			string propertyField,
			Guid agencyId,
			string webConfigKey)
		{
			string value = await GetCodepalSetting(propertyField, agencyId);
			if (string.IsNullOrWhiteSpace(value))
			{
				value = await cwmContext.Settings
					.Where(s => s.PropertyField == propertyField && s.ValueField != null && s.ValueField != "")
					.Select(s => s.ValueField)
					.FirstOrDefaultAsync();
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				value = ConfigurationManager.AppSettings[webConfigKey] ?? string.Empty;
			}

			return value;
		}

		public async Task<bool> SaveSupportEmailRecipientsAsync(
			Guid agencyId,
			string technicalSupport,
			string fireServicesSupport)
		{
			bool technicalSaved = await SaveCodepalSetting(
				FireGrantSettingKeys.TechnicalSupportEmail,
				technicalSupport ?? string.Empty,
				agencyId);
			if (!technicalSaved)
			{
				return false;
			}

			return await SaveCodepalSetting(
				FireGrantSettingKeys.FireServicesSupportEmail,
				fireServicesSupport ?? string.Empty,
				agencyId);
		}

		public async Task<bool> GetCodepalBooleanSettingAsync(string propertyField, Guid? agencyId, string userName = "")
		{
			bool retval = false;
			Setting setting;
			try
			{
				if (userName != null && userName != "")
				{
					setting = await cwmContext.Settings.FirstOrDefaultAsync(s => s.PropertyField == propertyField && s.UserName == userName);
				}
				else if (agencyId != null && agencyId != Guid.Empty)
				{
					setting = await cwmContext.Settings.FirstOrDefaultAsync(s => s.PropertyField == propertyField && s.AgencyId == agencyId);
				}
				else
				{
					setting = await cwmContext.Settings.FirstOrDefaultAsync(s => s.PropertyField == propertyField);
				}
				if (setting != null)
				{
					retval = Convert.ToBoolean(Convert.ToInt32(setting.ValueField));
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving setting '" + propertyField + "' for Agency '" + agencyId + ".", ex);
			}
			return retval;
		}

		public async Task<bool> GetCodepalBooleanSetting(string propertyField, Guid? agencyId, string userName = "")
		{
			bool retval = false;
			Setting setting;
			try
			{
				if (userName != null && userName != "")
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField && s.UserName == userName);
				}
				else if (agencyId != null && agencyId != Guid.Empty)
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField && s.AgencyId == agencyId);
				}
				else
				{
					setting = await cwmContext.Settings.SingleOrDefaultAsync(s => s.PropertyField == propertyField);
				}
				if (setting != null)
				{
					retval = Convert.ToBoolean(setting.ValueField);
				}
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving setting '" + propertyField + "' for Agency '" + agencyId + ".", ex);
			}
			return retval;
		}

		public async Task<AgencyAliases> GetAliases(Guid? agencyId)
		{
			AgencyAliases result = new AgencyAliases();
			var aliases = await cwmContext.v_ModuleAliases.Where(m => m.AgencyId == (agencyId ?? Guid.Empty)).ToListAsync();

			foreach (v_ModuleAliases moduleAlias in aliases)
			{
				switch (moduleAlias.ModuleDesc)
				{
					case "Address":

						result.AddressAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.AddressAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Complaint":
					case "Request":
						result.ComplaintAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.ComplaintAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Fee":
						result.FeeAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.FeeAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Inspection":
					case "Activity":
						result.ActivityAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.ActivityAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Inspection Detail":
						result.InspectionDetailAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.InspectionDetailAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Memorandum":
						result.MemorandumAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.MemorandumAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Mileage":
						result.MileageAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.MileageAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Party":
						result.PartyAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.PartyAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Permit":
						result.PermitAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.PermitAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Agency":
						result.AgencyAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.AgencyAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Item":
						result.ItemAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.ItemAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Location":
						result.LocationAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.LocationAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Invoice":
						result.InvoiceAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.InvoiceAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Certifications":
						result.CertificationAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.CertificationAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					case "Project":
						result.ProjectAlias = moduleAlias.ModuleAlias.Singularize(false);
						result.ProjectAliasP = moduleAlias.ModuleAlias.Pluralize(false);
						break;

					default:
						break;
				}
			}
			return result;
		}

		public async Task<Guid?> GetModuleId(Guid? AgencyId, string ModuleName)
		{
			return (await cwmContext.Modules.FirstOrDefaultAsync(m => m.AgencyId == AgencyId && m.ModuleDesc == ModuleName)).ModuleId;
		}
		
		public async Task<Inspector> GetUserAsync(Guid? userId)
		{
			return await cwmContext.Inspectors.FirstOrDefaultAsync(u => u.InspectorId == userId);
		}

		public async Task<string> GetUserNameAsync(Guid? userId)
		{
			return (await cwmContext.Inspectors.FirstOrDefaultAsync(u => u.InspectorId == userId)).InspectorName;
		}

		public async Task<Guid?> GetAgencyIdFromNameAsync(string agencyName)
		{
			return (await cwmContext.Agencies.FirstOrDefaultAsync(a => a.AgencyName == agencyName)).AgencyId;
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

		public async Task CreateInspector(DetailedInspector model)
        {
			if (model != null)
			{
				// The user is saving a new party
				var inspector = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.InspectorName == model.InspectorName);

				if (inspector != null)
				{
					logger.Error("Unable to create party '" + model.InspectorName.ToString() + "'.  The inspector name is already in the database.");
					return;
				}

				var newAudit = new AuditModel { TableName = "Inspectors", RecordId = model.InspectorId, AuditAction = "RECORD CREATED", Description = "Inspector Created From Fire Grant Application" };
				var newAuditFields = new List<AuditFieldModel>();

				inspector = cwmContext.Inspectors.Add(new Data.Inspector());
				inspector.rowguid = Guid.NewGuid();
				inspector.Code = null;
				inspector.InspectorName = null;
				inspector.Login = null;
				inspector.Password = null;
				inspector.Admin = false;
				inspector.AgencyId = null;
				inspector.InspectorPhone = null;
				inspector.Signature = null;
				inspector.LoggedIn = null;
				inspector.Madmin = false;
				inspector.GroupId = null;
				inspector.Inactive = false;
				inspector.ExternalId = null;
				inspector.Email = null;
				inspector.CodeExempt = false;
				inspector.GlobalUser = false;
				inspector.RCLevel = null;
				inspector.ActiveModules = "000000000000";
				inspector.DateUpdated = DateTime.Now; 
				inspector.DateInserted = DateTime.Now;
				inspector.DisablePWChange = true;
				inspector.SecQOne = null;
				inspector.SecAOne = null;
				inspector.SecQTwo = null;
				inspector.SecATwo = null;
				inspector.Title = null;


				newAuditFields.Add(new AuditFieldModel { ControlName = "", FieldDesc = "", OldId = null, OldValue = null, NewId = model.InspectorId, NewValue = null });
				inspector.InspectorId = model.InspectorId;

				if (model.InspectorName != null && model.InspectorName != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "InspectorName", FieldDesc = "Inspector Name", OldId = null, OldValue = null, NewId = null, NewValue = model.InspectorName });
					inspector.InspectorName = model.InspectorName;

					newAuditFields.Add(new AuditFieldModel { ControlName = "Code", FieldDesc = "Code", OldId = null, OldValue = null, NewId = null, NewValue = model.Code });
					inspector.Code = "fgs_" + model.InspectorName.Substring(0, 5);
				}

				if (model.Email != null && model.Email != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = null, NewId = null, NewValue = model.Email });
					inspector.Email = model.Email;
				}

				if (model.InspectorPhone != null && model.InspectorPhone != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "InspectorPhone", FieldDesc = "InspectorPhone", OldId = null, OldValue = null, NewId = null, NewValue = model.InspectorPhone });
					inspector.InspectorPhone = model.InspectorPhone;
				}

				if (model.Login != null && model.Login != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Login", FieldDesc = "Login", OldId = null, OldValue = null, NewId = null, NewValue = model.Login });
					inspector.Login = model.Login;
				}

				if (model.Password != null && model.Password != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Password", FieldDesc = "Password", OldId = null, OldValue = null, NewId = null, NewValue = model.Password });
					inspector.Password = model.Password;
				}

				if (model.AgencyId != null && model.AgencyId.ToString() != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "AgencyId", FieldDesc = "AgencyId", OldId = null, OldValue = null, NewId = null, NewValue = model.Password });
					inspector.AgencyId = model.AgencyId;
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
						logger.Error("Unable to create inspector '" + model.InspectorName.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to create inspector '" + model.InspectorName.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("CreateParty was called with a null reference.");
			}
		}

		public async Task UpdateInspector(DetailedInspector model)
		{
			if (model != null)
			{
				// The user is saving a new party
				var inspector = await cwmContext.Inspectors.SingleOrDefaultAsync(a => a.InspectorId == model.InspectorId);

				if (inspector == null)
				{
					logger.Error("Unable to update inspector '" + model.InspectorName.ToString() + "'.  The inspector name is not in the database.");
					return;
				}

				var newAudit = new AuditModel { TableName = "Inspectors", RecordId = model.InspectorId, AuditAction = "RECORD UPDATED", Description = "Inspector Updated From Fire Grant Application" };
				var newAuditFields = new List<AuditFieldModel>();


				if (model.InspectorName != null && model.InspectorName != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "InspectorName", FieldDesc = "Inspector Name", OldId = null, OldValue = null, NewId = null, NewValue = model.InspectorName });
					inspector.InspectorName = model.InspectorName;
				}

				if (model.Email != null && model.Email != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Email", FieldDesc = "Email", OldId = null, OldValue = null, NewId = null, NewValue = model.Email });
					inspector.Email = model.Email;
				}

				if (model.InspectorPhone != null && model.InspectorPhone != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "InspectorPhone", FieldDesc = "InspectorPhone", OldId = null, OldValue = null, NewId = null, NewValue = model.InspectorPhone });
					inspector.InspectorPhone = model.InspectorPhone;
				}

				if (model.Login != null && model.Login != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "Login", FieldDesc = "Login", OldId = null, OldValue = null, NewId = null, NewValue = model.Login });
					inspector.Login = model.Login;
				}

				//if (model.Password != null && model.Password != String.Empty)
				//{
				//	newAuditFields.Add(new AuditFieldModel { ControlName = "Password", FieldDesc = "Password", OldId = null, OldValue = null, NewId = null, NewValue = model.Password });
				//	inspector.Password = model.Password;
				//}

				if (model.AgencyId != null && model.AgencyId.ToString() != String.Empty)
				{
					newAuditFields.Add(new AuditFieldModel { ControlName = "AgencyId", FieldDesc = "AgencyId", OldId = null, OldValue = null, NewId = null, NewValue = model.Password });
					inspector.AgencyId = model.AgencyId;
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
						logger.Error("Unable to update inspector '" + model.InspectorName.ToString() + "'.", ex);
					}
				}
				else
				{
					logger.Error("Unable to update inspector '" + model.InspectorName.ToString() + "', DbContext was not available.");
				}
			}
			else
			{
				logger.Error("Update Inspector was called with a null reference.");
			}
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

		public async Task<string> DecryptField(string Value)
		{
			string result = "";
			TripleDES Decryptor = new TripleDES();

			result = Decryptor.Decrypt(Value);

			return await Task.FromResult(result);
		}

		public async Task<string> EncryptField(string Value)
		{
			string result = "";
			TripleDES Encryptor = new TripleDES();

			result = Encryptor.Encrypt(Value);

			return await Task.FromResult(result);
		}

		public async Task InsertEmailSendLogAsync(Guid messageId, EmailSendLogPayload payload, Guid? agencyId)
		{
			if (messageId == Guid.Empty || payload == null)
			{
				return;
			}

			try
			{
				var setting = new Setting
				{
					SettingsId = Guid.NewGuid(),
					PropertyField = FireGrantSettingKeys.EmailLogPrefix + messageId,
					ValueField = TruncateValueField(EmailSendLogJson.Serialize(payload)),
					AgencyId = agencyId,
					rowguid = Guid.NewGuid(),
					DateInserted = DateTime.Now,
					DateUpdated = DateTime.Now
				};
				cwmContext.Settings.Add(setting);

				if (cwmContext is DbContext)
				{
					await ((DbContext)cwmContext).SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				logger.Error("Failed to insert email send log for message '" + messageId + "'.", ex);
			}
		}

		public async Task UpdateEmailSendLogAsync(Guid messageId, EmailSendLogPayload payload)
		{
			if (messageId == Guid.Empty || payload == null)
			{
				return;
			}

			try
			{
				string propertyField = FireGrantSettingKeys.EmailLogPrefix + messageId;
				var setting = await cwmContext.Settings.SingleOrDefaultAsync(
					s => s.PropertyField == propertyField);
				if (setting == null)
				{
					return;
				}

				setting.ValueField = TruncateValueField(EmailSendLogJson.Serialize(payload));
				setting.DateUpdated = DateTime.Now;

				if (cwmContext is DbContext)
				{
					await ((DbContext)cwmContext).SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				logger.Error("Failed to update email send log for message '" + messageId + "'.", ex);
			}
		}

		public async Task<IReadOnlyList<EmailSendLogEntry>> GetRecentEmailSendLogsAsync(
			Guid? agencyId,
			int take)
		{
			var results = new List<EmailSendLogEntry>();
			if (take <= 0)
			{
				return results;
			}

			try
			{
				string prefix = FireGrantSettingKeys.EmailLogPrefix;
				IQueryable<Setting> query = cwmContext.Settings.Where(
					s => s.PropertyField.StartsWith(prefix));
				if (agencyId != null && agencyId != Guid.Empty)
				{
					query = query.Where(s => s.AgencyId == agencyId);
				}

				var rows = await query
					.OrderByDescending(s => s.DateInserted)
					.Take(take)
					.ToListAsync();

				foreach (Setting row in rows)
				{
					EmailSendLogPayload payload = EmailSendLogJson.Deserialize(row.ValueField);
					Guid messageId = Guid.Empty;
					if (row.PropertyField != null &&
						row.PropertyField.Length > prefix.Length)
					{
						Guid.TryParse(row.PropertyField.Substring(prefix.Length), out messageId);
					}

					results.Add(new EmailSendLogEntry
					{
						MessageId = messageId,
						DateInserted = row.DateInserted,
						DateUpdated = row.DateUpdated,
						Status = payload.status,
						From = payload.from,
						ReplyTo = payload.replyTo,
						To = payload.to,
						Subject = payload.subject,
						ContextType = payload.ctx,
						ContextId = payload.ctxId,
						SentByLogin = payload.sentByLogin,
						SentByEmail = payload.sentByEmail,
						SentByRole = payload.sentByRole,
						FailReason = payload.fail
					});
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				logger.Error("Failed to read recent email send logs.", ex);
			}

			return results;
		}

		public async Task<int> DeleteEmailSendLogsOlderThanAsync(DateTime cutoffUtc)
		{
			int deleted = 0;
			try
			{
				string prefix = FireGrantSettingKeys.EmailLogPrefix;
				var rows = await cwmContext.Settings.Where(
					s => s.PropertyField.StartsWith(prefix) && s.DateInserted < cutoffUtc).ToListAsync();
				if (rows.Count == 0)
				{
					return 0;
				}

				foreach (Setting row in rows)
				{
					cwmContext.Settings.Remove(row);
					deleted++;
				}

				if (cwmContext is DbContext)
				{
					await ((DbContext)cwmContext).SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_ = ex;
				logger.Error("Failed to purge old email send logs.", ex);
			}

			return deleted;
		}

		private static string TruncateValueField(string value)
		{
			if (string.IsNullOrEmpty(value) || value.Length <= 3000)
			{
				return value ?? string.Empty;
			}

			return value.Substring(0, 3000);
		}


	}
}

