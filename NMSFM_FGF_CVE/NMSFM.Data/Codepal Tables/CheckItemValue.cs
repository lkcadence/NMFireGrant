namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckItemValue
    {
        public Guid CheckItemValueId { get; set; }

        public Guid InspectionId { get; set; }

        public Guid CheckItemId { get; set; }

        [StringLength(3000)]
        public string TextValue { get; set; }

        public byte? BooleanValue { get; set; }

        [StringLength(2000)]
        public string ResolutionText { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        public Guid? InspectionDetailId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? CorrectedInspectionId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
