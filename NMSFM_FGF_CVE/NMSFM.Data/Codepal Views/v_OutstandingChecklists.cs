namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_OutstandingChecklists
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CheckListName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1000)]
        public string CheckItem { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid CheckItemId { get; set; }

        [StringLength(50)]
        public string CheckItemType { get; set; }

        public short? CheckListOrder { get; set; }

        public short? SeqNum { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        [StringLength(200)]
        public string ResolutionText { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        public bool? RefOnly { get; set; }

        [StringLength(6000)]
        public string ViolationType { get; set; }

        public int? Sequence { get; set; }

        public Guid? InspectionDetailId { get; set; }

        public bool? Severe { get; set; }

        [StringLength(3000)]
        public string Comment { get; set; }

        public Guid? CorrectedInspectionId { get; set; }

        [StringLength(1000)]
        public string CorrectedComments { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExpCorrDate { get; set; }

        [StringLength(3000)]
        public string CorrectiveAction { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CorrectedDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Inactive { get; set; }

        public Guid? InspectionId { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(50)]
        public string SeverityLevel { get; set; }

        [StringLength(500)]
        public string Message { get; set; }
    }
}
