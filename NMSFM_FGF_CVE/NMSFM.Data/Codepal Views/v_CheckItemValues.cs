namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_CheckItemValues
    {
        public Guid? CheckListId { get; set; }

        [StringLength(50)]
        public string CheckListName { get; set; }

        public Guid? InspectionTypeId { get; set; }

        public Guid? rowguid { get; set; }

        public bool? NotPrinted { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        public bool? CheckListsInactive { get; set; }

        [StringLength(50)]
        public string NFPAReport { get; set; }

        public Guid? CheckItemId { get; set; }

        public Guid? CheckItemTypeId { get; set; }

        [StringLength(1000)]
        public string CheckItem { get; set; }

        public short? SeqNum { get; set; }

        public Guid? ViolationTypeId { get; set; }

        public bool? Required { get; set; }

        public Guid? CalcTypeId { get; set; }

        public bool? CheckItemInactive { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid CheckItemValueId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InspectionId { get; set; }

        [StringLength(2000)]
        public string ResolutionText { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        public Guid? InspectionDetailId { get; set; }

        public Guid? CorrectedInspectionId { get; set; }

        [StringLength(50)]
        public string CheckItemType { get; set; }

        public bool? IsBoolean { get; set; }

        [StringLength(3000)]
        public string Value { get; set; }

        [StringLength(3)]
        public string FailValue1 { get; set; }

        [StringLength(100)]
        public string FailValue { get; set; }

        public short? CheckListOrder { get; set; }
    }
}
