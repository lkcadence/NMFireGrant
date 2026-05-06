namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemInspectionStatu
    {
        [Key]
        public Guid ItemInspectionStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemInspectionStatus { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? ActivityCategoryId { get; set; }

        public Guid? ItemTypeId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
