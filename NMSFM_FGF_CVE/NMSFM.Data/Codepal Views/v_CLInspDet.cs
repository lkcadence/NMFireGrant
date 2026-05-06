namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_CLInspDet
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionDetailId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InspectionId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ViolationTypeId { get; set; }

        [StringLength(3000)]
        public string Comment { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CorrectedDate { get; set; }

        public bool? Severe { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime LastUpdated { get; set; }

        [StringLength(6000)]
        public string ViolationType { get; set; }

        public Guid? CategoryTypeId { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        public Guid? CorrectedInspectionId { get; set; }

        [StringLength(1000)]
        public string CorrectedComments { get; set; }

        public Guid? SeverityLevelId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpCorrDate { get; set; }

        [StringLength(3000)]
        public string CorrectiveAction { get; set; }

        public bool? RefOnly { get; set; }

        public DateTime? ViolationDate { get; set; }

        public DateTime? InspectionDate { get; set; }
    }
}
