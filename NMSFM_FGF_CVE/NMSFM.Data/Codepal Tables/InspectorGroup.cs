namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InspectorGroup
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectorId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid GroupId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
