namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeeSchedule")]
    public partial class FeeSchedule
    {
        [Key]
        public Guid FeeSchedId { get; set; }

        [Required]
        [StringLength(250)]
        public string FeeItem { get; set; }

        [Column(TypeName = "money")]
        public decimal FeeRate { get; set; }

        public Guid? FeeTypeId { get; set; }

        public Guid rowguid { get; set; }

        public Guid? UserDefFieldId { get; set; }

        public bool Inactive { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
