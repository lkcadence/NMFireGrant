namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ViolationSearch
    {
        [Key]
        [Column(Order = 0)]
        public Guid ViolationTypeId { get; set; }

        [StringLength(6000)]
        public string ViolationType { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CategoryTypeId { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool VioInactive { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Locked { get; set; }

        [StringLength(100)]
        public string VioExtId { get; set; }

        [StringLength(10)]
        public string VioCode { get; set; }

        [StringLength(300)]
        public string CategoryType { get; set; }

        [StringLength(10)]
        public string CatCode { get; set; }

        public Guid? CodeVersionId { get; set; }

        public bool? CatInactive { get; set; }

        [StringLength(100)]
        public string CatExtId { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        public bool? Filter { get; set; }

        public bool? BookInactive { get; set; }

        public bool? NonPurchasedCode { get; set; }

        [StringLength(10)]
        public string BookCode { get; set; }

        [StringLength(100)]
        public string BookExtId { get; set; }

        public byte? Sequence { get; set; }
    }
}
