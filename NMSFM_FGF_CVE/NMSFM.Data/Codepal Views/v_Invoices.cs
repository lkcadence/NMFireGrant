namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Invoices
    {
        [Key]
        [Column(Order = 0)]
        public Guid InvoiceId { get; set; }

        public Guid? InvoiceTypeId { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public Guid? BillToPartyId { get; set; }

        public Guid? BillToAddressId { get; set; }

        public Guid? TermsId { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Printed { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? ApprovalStep { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        public bool? Void { get; set; }

        public Guid? LinkedInvoiceId { get; set; }

        public bool? QBExported { get; set; }

        public Guid? ServiceAddressId { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(100)]
        public string QBTransactionID { get; set; }

        [StringLength(50)]
        public string QBInvoiceNumber { get; set; }

        public DateTime? QBPostedDate { get; set; }

        public decimal? InvoiceBalance { get; set; }

        [StringLength(50)]
        public string InvoiceType { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? InvoiceAge { get; set; }

        public int? PastDue { get; set; }
    }
}
