namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_FeeBalanceSummary
    {
        public Guid? RecordId { get; set; }

        [Key]
        [Column(Order = 0, TypeName = "money")]
        public decimal FeeSum { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal PaymentSum { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal ReleviedAmt { get; set; }

        public decimal? BalanceDue { get; set; }

        [StringLength(15)]
        public string Paid { get; set; }
    }
}
