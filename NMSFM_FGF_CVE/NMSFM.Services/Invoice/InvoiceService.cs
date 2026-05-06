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
namespace NMSFM.Services.Invoice
{
    class InvoiceService : IInvoiceService
    {
        private ICodepalWebModel cwmContext;
        private IAuditService auditService;
        private ILogging logger;


        //public InvoiceService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        public InvoiceService(ICodepalWebModel codepalWebModel, ILogging codepalLogger)
        {
            cwmContext = codepalWebModel;
            logger = codepalLogger;
            auditService = new AuditService(logger);
        }

        //Task<IEnumerable<v_Invoices>> GetInvoicesAsync()
        public async Task<IEnumerable<v_Invoices>> GetInvoicesAsync()
        {
            IEnumerable<v_Invoices> result;
            try
            {
                var invoices = await cwmContext.v_Invoices.ToListAsync();
                var invoiceTypeList = await cwmContext.InvoiceTypes.Where(a => a.Inactive == false && a.WebViewable == true).Select(a => a.InvoiceTypeId).ToListAsync();
                if (invoices != null && invoices.Count() > 0 & invoiceTypeList != null & invoiceTypeList.Count() > 0)
                {
                    invoices = invoices.Where(a => invoiceTypeList.Contains(a.InvoiceTypeId == null ? Guid.Empty : a.InvoiceTypeId.Value)).ToList();
                }
                result = invoices;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Invoice List.", ex);
                result = new List<v_Invoices>();
            }
            return result;
        }

        //Task<List<InvoiceType>> GetInvoiceTypeAsync()
        public async Task<List<InvoiceType>> GetInvoiceTypeAsync()
        {
            List<InvoiceType> result;
            try
            {
                var invoiceTypeList = await cwmContext.InvoiceTypes.Where(a => !a.Inactive).ToListAsync();
                result = invoiceTypeList;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Invoice Types list.", ex);
                result = new List<InvoiceType>();
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
                logger.Error("Unexpected exception caught while retrieving the Invoice Notes List.", ex);
                result = new List<Data.Note>();
            }
            return result;
        }

        //Task<string> GetInvoiceTypeLegalTextByInvoiceTypeIdAsync(Guid id);
        public async Task<string> GetInvoiceTypeLegalTextByInvoiceTypeIdAsync(Guid id)
        {
            string result;
            try
            {
                /* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
                var invoiceType = await cwmContext.InvoiceTypes.SingleAsync(p => p.InvoiceTypeId == id);

                string legalDesc = invoiceType.LegalDesc;

                result = legalDesc;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Invoice Type Legal Text.", ex);
                result = "";
            }
            return result;
        }

        //Task SaveLegalDescriptionAsync(Guid invoiceId, string LegalDesc)
        public async Task SaveLegalDescriptionAsync(Guid invoiceId, string LegalDesc)
        {
            if (invoiceId != null && LegalDesc != null)
            {
                var invoice = await cwmContext.Invoices.SingleOrDefaultAsync(a => a.InvoiceId == invoiceId);
                if (invoice == null)
                {
                    logger.Error("Unable to update legal desc. for invoice '" + invoiceId.ToString() + "'.  The invoice could not be located in the database.");
                    return;
                }
                var audit = new AuditModel { TableName = "Invoices", RecordId = invoiceId, AuditAction = "RECORD UPDATED", Description = "" };
                var auditFields = new List<AuditFieldModel>();
                if (invoice.LegalDesc != LegalDesc)
                {
                    var auditField = new AuditFieldModel { ControlName = "legalText", FieldDesc = "Legal Text", OldId = null, OldValue = invoice.LegalDesc, NewId = null, NewValue = LegalDesc };
                    auditFields.Add(auditField);
                }
                invoice.LegalDesc = LegalDesc;

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
                        logger.Error("Unable to save the legal desc. changes for Invoice '" + invoiceId.ToString() + "'.", ex);
                    }
                }
                else
                {
                    logger.Error("Unable to update legal desc. for invoice '" + invoiceId.ToString() + "', DbContext was not available.");
                }
            }
            else
            {
                logger.Error("SaveLegalDescriptions was called with a null reference.");
            }
        }

		//Task<string> GetTermsByInvoiceIdAsync(Guid id);
		public async Task<string> GetTermsByInvoiceIdAsync(Guid id)
		{
			//string result;
			string result;
			try
			{
				/* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
				var invoice = await cwmContext.v_Invoices.SingleAsync(p => p.InvoiceId == id);

				var term = await cwmContext.Terms.SingleAsync(p => !p.Inactive && p.TermsId == invoice.TermsId);
				//var term = await cwmContext.Terms.SingleAsync(t => t.TermsId == invoice.TermsId).ToListAsync();


				result = term.Terms; 
			}
			catch (Exception ex)
            {
                _ = ex;
				logger.Error("Unexpected exception caught while retrieving the Invoice Term.", ex);
				result = "";
			}
			return result;
		}


		//Task<List<Term>> GetTermsAsync()
		public async Task<List<Term>> GetTermsAsync()
        {
            List<Term> result;
            try
            {
                var termList = await cwmContext.Terms.Where(a => !a.Inactive).ToListAsync();
                result = termList;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Terms list.", ex);
                result = new List<Term>();
            }
            return result;
        }

        //Task<Guid> GetBillToParty(Guid id)
        public async Task<Guid> GetBillToParty(Guid id)
        {
            Guid billtoPartyId = Guid.Empty;
            try
            {
                var party = await cwmContext.Parties.SingleOrDefaultAsync(a => a.PartyID == id);
                if (!party.Equals(null) && party.PartyID != null)
                {
                    billtoPartyId = party.PartyID;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the bill to Party for invoice id = " + id + ".", ex);
            }
            return billtoPartyId;
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

        //Task<v_Invoices> GetAddressByInvoiceIdAsync(Guid id);
        public async Task<v_Invoices> GetAddressByInvoiceIdAsync(Guid id)
        {
            v_Invoices result = null;
            try
            {
                result = await cwmContext.v_Invoices.SingleOrDefaultAsync(a => a.InvoiceId == id);
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Address '" + id.ToString() + "'.", ex);
            }
            return result;
        }

        //Task<v_Invoices> GetSecondaryAddressByInvoiceIdAsync(Guid id);
        public async Task<v_Invoices> GetSecondaryAddressByInvoiceIdAsync(Guid id)
        {
            v_Invoices result = null;
            try
            {
                result = await cwmContext.v_Invoices.SingleOrDefaultAsync(a => a.InvoiceId == id);
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Secondary Address '" + id.ToString() + "'.", ex);
            }
            return result;
        }

        //Task<Guid> GetBillToAddressId(Guid id)
        public async Task<Guid> GetBillToAddressId(Guid id)
        {
            Guid billtoAddressId = Guid.Empty;
            try
            {
                var address = await cwmContext.Addresses.SingleOrDefaultAsync(a => a.AddressId == id);
                if (!address.Equals(null) && address.AddressId != null)
                {
                    billtoAddressId = address.AddressId;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the bill to Address for invoice id = " + id + ".", ex);
            }
            return billtoAddressId;
        }

        //Task<Guid> GetServiceAddressId(Guid id)
        public async Task<Guid> GetServiceAddressId(Guid id)
        {
            Guid billtoAddressId = Guid.Empty;
            try
            {
                var address = await cwmContext.Addresses.SingleOrDefaultAsync(a => a.AddressId == id);
                if (!address.Equals(null) && address.AddressId != null)
                {
                    billtoAddressId = address.AddressId;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Service Address for invoice id = " + id + ".", ex);
            }
            return billtoAddressId;
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
                logger.Error("Unexpected exception caught while retrieving the Invoice Fees List.", ex);
                result = new List<v_Fees>();
            }
            return result;
        }


        //Task<IEnumerable<v_Fees> GetInvoiceFeesByInvoiceIdAsync(Guid id);
        public async Task<IEnumerable<v_Fees>> GetInvoiceFeesByInvoiceIdAsync(Guid id)
        {
            IEnumerable<v_Fees> result;
            try
            {
                /* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
                var invoiceFees = await cwmContext.v_Fees.Where(p => p.InvoiceId == id).ToListAsync();

                result = invoiceFees;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Invoice Fees List.", ex);
                result = new List<v_Fees>();
            }
            return result;
        }

        //Task<IEnumerable<v_InvoicePayments> GetInvoiceFeesByInvoiceIdAsync(Guid id);
        public async Task<IEnumerable<v_InvoicePayments>> GetInvoicePaymentsByInvoiceIdAsync(Guid id)
        {
            IEnumerable<v_InvoicePayments> result;
            try
            {
                /* && (!p.Inactive.HasValue || !p.Inactive.Value))*/
                var invoicePayments = await cwmContext.v_InvoicePayments.Where(p => p.InvoiceId == id).ToListAsync();

                result = invoicePayments;
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the Invoice Payments List.", ex);
                result = new List<v_InvoicePayments>();
            }
            return result;
        }

        //Task<Signature> GetSignatureByInvoiceId(Guid id);
        public async Task<Data.Signature> GetSignatureByInvoiceId(Guid id)
        {
            var result = new Data.Signature();
            try
            {
                result = await cwmContext.Signatures.SingleOrDefaultAsync(a => a.RecordId == id && a.Inactive == false) ?? new Data.Signature();
            }
            catch (Exception ex)
            {
                _ = ex;
                logger.Error("Unexpected exception caught while retrieving the InvoiceSignature for Invoice Id: " + id + ".", ex);
            }
            return result;
        }

        //Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);
        public async Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency)
        {
            IEnumerable<UserDefinedValue> results = new List<UserDefinedValue>();
            Guid InvoiceTypeId = pTypeId;
            var ModuleId = new Guid();
            try
            {
                v_Invoices invoice = null;
                invoice = await cwmContext.v_Invoices.SingleOrDefaultAsync(a => a.InvoiceTypeId == id);
                if (invoice != null)
                {
                    InvoiceTypeId = (Guid)invoice.InvoiceTypeId;
                }

                Guid AgencyId = new Guid("62a16726-f85b-4183-8556-b87154617d42");
                if (agency != null && agency != Guid.Empty)
                {
                    AgencyId = agency;
                }
                Module module = null;
                module = await cwmContext.Modules.SingleOrDefaultAsync(a => (a.AgencyId.HasValue ? a.AgencyId.Value : Guid.Empty) == AgencyId && a.ModuleDesc == "Invoice");
                ModuleId = module.ModuleId;

                var models = from cats in cwmContext.UserDefCategories
                             join catt in cwmContext.UserDefCategoryTypes on new { cd = cats.UserDefCategoryId, td = InvoiceTypeId } equals new { cd = catt.UserDefCategoryId, td = catt.TypeId } into subcat
                             from usecat in subcat.DefaultIfEmpty()
                             where (cats.ModuleId == ModuleId && (((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == InvoiceTypeId) || (usecat.TypeId == InvoiceTypeId || cats.AllModuleTypes == true) || ((cats.ModuleTypeId.Equals(null) ? Guid.Empty : cats.ModuleTypeId.Value) == InvoiceTypeId && cats.AllModuleTypes == true) || usecat.TypeId == InvoiceTypeId) && ((cats.WebViewable.Equals(null) ? false : cats.WebViewable.Value) == true && cats.Inactive == false))
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
                logger.Error("Unexpected exception caught while retrieving user defined values for invoice '" + id.ToString() + "'.", ex);
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
    }
}

