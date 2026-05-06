namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OccupancyType
    {
        public Guid OccupancyTypeId { get; set; }

        [Column("OccupancyType")]
        [StringLength(50)]
        public string OccupancyType1 { get; set; }

        [StringLength(10)]
        public string OccupancyTypeCode { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
