namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckItem
    {
        public Guid CheckItemId { get; set; }

        public Guid CheckItemTypeId { get; set; }

        [Column("CheckItem")]
        [Required]
        [StringLength(1000)]
        public string CheckItem1 { get; set; }

        public short? SeqNum { get; set; }

        public Guid CheckListId { get; set; }

        public Guid? ViolationTypeId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Required { get; set; }

        public Guid? CalcTypeId { get; set; }

        public bool Inactive { get; set; }

        public bool FailsCheckList { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(3000)]
        public string DefaultValue { get; set; }

        [StringLength(100)]
        public string FailValue { get; set; }

        public bool HideNA { get; set; }

        public bool HideNO { get; set; }

        public bool StaticList { get; set; }

        public bool HideAddRef { get; set; }
    }
}
