namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SubCheckItemValues
    {
        [Key]
        public Guid ActivityId { get; set; }

        public Guid? CheckListId { get; set; }

        [StringLength(50)]
        public string CheckListName { get; set; }

        public short? CheckListOrder { get; set; }

        public bool? NotPrinted { get; set; }

        public bool? Inactive { get; set; }

        public Guid? CheckItemId { get; set; }

        [StringLength(50)]
        public string CheckItemType { get; set; }

        [StringLength(1000)]
        public string CheckItem { get; set; }

        public short? SeqNum { get; set; }

        public Guid? CheckItemValueId { get; set; }

        [StringLength(2000)]
        public string ResolutionText { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Corrected { get; set; }

        public Guid? InspectionDetailId { get; set; }

        [StringLength(3000)]
        public string Value { get; set; }

        [StringLength(3)]
        public string FailValue1 { get; set; }

        public bool? ChkItemInactive { get; set; }
    }
}
