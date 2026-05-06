namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fee
    {
        public Guid FeeId { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? FeeTypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeAmt { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmt { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string RefNum { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public DateTime? FeeDate { get; set; }

        public Guid rowguid { get; set; }

        [Column(TypeName = "money")]
        public decimal? FeeBase { get; set; }

        public decimal? Units { get; set; }

        [StringLength(250)]
        public string FeeUOM { get; set; }

        public Guid? InvoiceId { get; set; }

        public Guid? ResponsiblePartyId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(250)]
        public string FeeDesc { get; set; }

        public int? FeeStatus { get; set; }

        public Guid? PaymentUserId { get; set; }

        public bool IsDefault { get; set; }

        public DateTime? ReCalcDate { get; set; }

        public DateTime? OriginalFeeDate { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
