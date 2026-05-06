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

namespace NMSFM.Services.Invoice
{
    public interface IInvoiceService
    {
        Task<IEnumerable<v_Invoices>> GetInvoicesAsync();
        Task<List<InvoiceType>> GetInvoiceTypeAsync();
        Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id);
        Task<string> GetInvoiceTypeLegalTextByInvoiceTypeIdAsync(Guid id);
        Task SaveLegalDescriptionAsync(Guid invoiceId, string LegalDesc);
        Task<string> GetTermsByInvoiceIdAsync(Guid id);
        Task<List<Term>> GetTermsAsync();
        Task<Guid> GetBillToParty(Guid id);
        Task<IEnumerable<v_Addresses2>> GetAddressesAsync(bool showInactive);
        Task<v_Invoices> GetAddressByInvoiceIdAsync(Guid id);
        Task<v_Invoices> GetSecondaryAddressByInvoiceIdAsync(Guid id);
        Task<Guid> GetBillToAddressId(Guid id);
        Task<Guid> GetServiceAddressId(Guid id);
        Task<IEnumerable<v_Fees>> GetFeesAsync();
        Task<IEnumerable<v_Fees>> GetInvoiceFeesByInvoiceIdAsync(Guid id);
        Task<IEnumerable<v_InvoicePayments>> GetInvoicePaymentsByInvoiceIdAsync(Guid id);
        Task<Data.Signature> GetSignatureByInvoiceId(Guid id);
        Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);
        Task SaveUserDefinedValuesAsync(List<UserDefValue> list);

    }

}
