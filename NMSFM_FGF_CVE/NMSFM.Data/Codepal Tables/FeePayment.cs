namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeePayment
    {
        public Guid FeePaymentId { get; set; }

        public Guid FeeId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmt { get; set; }

        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string RefNum { get; set; }

        [StringLength(100)]
        public string ReceivedFrom { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public Guid? PaymentUserId { get; set; }

        public Guid? InvoicePaymentId { get; set; }

        public bool Void { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
