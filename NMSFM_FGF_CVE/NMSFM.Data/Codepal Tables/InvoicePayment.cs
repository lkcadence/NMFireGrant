namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoicePayment
    {
        [Key]
        public Guid PaymentId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public Guid InvoiceId { get; set; }

        [Required]
        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? PaymentUserId { get; set; }

        [StringLength(2)]
        public string QBImportExport { get; set; }

        [StringLength(100)]
        public string ReceivedFrom { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string QBTransactionId { get; set; }

        [StringLength(50)]
        public string QBInvoiceNumber { get; set; }

        public DateTime? QBPostedDate { get; set; }

        public Guid rowguid { get; set; }

        public bool? Void { get; set; }

        public bool? Returned { get; set; }

        public Guid? ReturnedBy { get; set; }

        [StringLength(100)]
        public string ReturnedTo { get; set; }

        public DateTime? ReturnedDate { get; set; }

        [StringLength(50)]
        public string ReturnedNumber { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
