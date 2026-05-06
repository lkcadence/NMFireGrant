namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InspectionDetail
    {
        public Guid InspectionDetailId { get; set; }

        public Guid InspectionId { get; set; }

        public Guid ViolationTypeId { get; set; }

        [StringLength(3000)]
        public string Comment { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CorrectedDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool? Severe { get; set; }

        public Guid rowguid { get; set; }

        public Guid? CorrectedInspectionId { get; set; }

        [StringLength(1000)]
        public string CorrectedComments { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? SeverityLevelId { get; set; }

        public bool IsItem { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpCorrDate { get; set; }

        [StringLength(3000)]
        public string CorrectiveAction { get; set; }

        public bool? RefOnly { get; set; }

        public DateTime? ViolationDate { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? LocationBaseId { get; set; }

        public Guid? LocationId { get; set; }

        public short? Sequence { get; set; }
    }
}
