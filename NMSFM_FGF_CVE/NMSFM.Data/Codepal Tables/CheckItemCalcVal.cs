namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckItemCalcVal")]
    public partial class CheckItemCalcVal
    {
        public Guid CheckItemCalcValId { get; set; }

        public Guid CheckItemId { get; set; }

        public Guid CheckItemValueId { get; set; }

        public Guid CalcTypeId { get; set; }

        public Guid CalcFieldId { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
