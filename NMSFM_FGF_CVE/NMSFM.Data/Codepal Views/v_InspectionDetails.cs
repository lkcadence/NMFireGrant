namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_InspectionDetails
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

        public Guid? InspectionCauseId { get; set; }

        public Guid? AddressId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public DateTime? InspectionTime { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(6000)]
        public string ViolationType { get; set; }

        public Guid? CategoryTypeId { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [StringLength(300)]
        public string CategoryType { get; set; }

        public Guid? CodeVersionId { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        public Guid? RecordId { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

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

        [StringLength(36)]
        public string InspectionTypeIdstr { get; set; }

        [StringLength(36)]
        public string CheckItemValueIdstr { get; set; }

        public bool? IsChkVio { get; set; }

        [StringLength(10)]
        public string CodeVersionCode { get; set; }

        [StringLength(10)]
        public string CategoryTypeCode { get; set; }

        [StringLength(10)]
        public string ViolationTypeCode { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        public Guid? ItemId { get; set; }

        public Guid? LocationBaseId { get; set; }

        public Guid? LocationId { get; set; }

        public short? Sequence { get; set; }

        public Guid? ParentInspectionId { get; set; }
    }
}
