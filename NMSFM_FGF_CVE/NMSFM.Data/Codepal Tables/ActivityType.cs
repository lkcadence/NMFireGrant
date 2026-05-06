namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityType
    {
        public Guid ActivityTypeId { get; set; }

        [Column("ActivityType")]
        [Required]
        [StringLength(50)]
        public string ActivityType1 { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string ViolationAlias { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Inactive { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool WebViewable { get; set; }
    }
}
