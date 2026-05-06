namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ItemInspectionStatus
    {
        [Key]
        [Column(Order = 0)]
        public Guid ItemInspectionStatusId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemInspectionStatus { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        public Guid? ActivityCategoryId { get; set; }

        public Guid? ItemTypeId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid rowguid { get; set; }
    }
}
