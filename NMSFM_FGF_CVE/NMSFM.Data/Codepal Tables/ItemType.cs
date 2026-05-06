namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemType
    {
        public Guid ItemTypeId { get; set; }

        [Column("ItemType")]
        [Required]
        [StringLength(75)]
        public string ItemType1 { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid? ReportId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
