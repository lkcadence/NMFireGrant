namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_FeePayments
    {
        [Key]
        [Column(Order = 0)]
        public Guid FeeId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid FeePaymentId { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime PaymentDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal PaymentAmt { get; set; }

        [StringLength(15)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string RefNum { get; set; }

        [StringLength(100)]
        public string ReceivedFrom { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Void { get; set; }
    }
}
