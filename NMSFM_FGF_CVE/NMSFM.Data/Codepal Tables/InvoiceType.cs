namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceType
    {
        public Guid InvoiceTypeId { get; set; }

        [Column("InvoiceType")]
        [StringLength(50)]
        public string InvoiceType1 { get; set; }

        [StringLength(1000)]
        public string Disclaimer { get; set; }

        public Guid? TermsId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        [StringLength(2000)]
        public string LegalFooter { get; set; }

        [StringLength(200)]
        public string MailToMethod { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool? IsQuote { get; set; }

        public Guid? ReportId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
