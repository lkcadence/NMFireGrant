namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_InvoicePayments
    {
        [Key]
        [Column(Order = 0)]
        public Guid PaymentId { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime PaymentDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal PaymentAmount { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid InvoiceId { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

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

        public bool? Void { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime DateInserted { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }
    }
}
