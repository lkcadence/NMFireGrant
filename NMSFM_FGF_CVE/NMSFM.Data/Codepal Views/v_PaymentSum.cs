namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_PaymentSum
    {
        public Guid? RecordId { get; set; }

        [Key]
        [Column(Order = 0)]
        public decimal PaymentSum { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "money")]
        public decimal ReleviedAmt { get; set; }
    }
}
