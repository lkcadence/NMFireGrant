namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SearchInvoices
    {
        [Key]
        [Column(Order = 0)]
        public Guid InvoiceId { get; set; }

        public Guid? InvoiceTypeId { get; set; }

        [StringLength(50)]
        public string InvoiceType { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? TermsId { get; set; }

        [StringLength(50)]
        public string Terms { get; set; }

        public Guid? RecordId { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(1000)]
        public string Disclaimer { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Printed { get; set; }

        public Guid? ApprovalStep { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        [StringLength(2000)]
        public string LegalFooter { get; set; }

        [StringLength(7000)]
        public string OriginalLegalDesc { get; set; }

        [StringLength(200)]
        public string MailToMethod { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public bool? IsQuote { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        public bool? QBExported { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "money")]
        public decimal FeeSum { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal PaymentSum { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal BalanceDue { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(15)]
        public string Paid { get; set; }

        [Key]
        [Column(Order = 7)]
        public decimal QBPaymentSum { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(15)]
        public string QBPaid { get; set; }

        public DateTime? QBPostedDate { get; set; }

        [StringLength(50)]
        public string QBInvoiceNumber { get; set; }

        [StringLength(100)]
        public string QBTransactionID { get; set; }

        public decimal? InvoiceBalance { get; set; }

        public Guid? BillToAddressId { get; set; }

        public bool? Void { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public int? InvoiceAge { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int? PastDue { get; set; }
    }
}
