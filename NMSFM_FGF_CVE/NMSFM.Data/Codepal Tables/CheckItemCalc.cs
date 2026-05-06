namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckItemCalc")]
    public partial class CheckItemCalc
    {
        [Key]
        public Guid CalcFieldId { get; set; }

        public Guid CalcTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Label { get; set; }

        public short Sequence { get; set; }

        public bool? Constant { get; set; }

        public decimal? ConstantVal { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? CalcCheckItemId { get; set; }
    }
}
