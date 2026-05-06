namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckItemValueInspectionDetail
    {
        [Key]
        [Column(Order = 0)]
        public Guid CheckItemValueId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InspectionDetailId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public int? Sequence { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
