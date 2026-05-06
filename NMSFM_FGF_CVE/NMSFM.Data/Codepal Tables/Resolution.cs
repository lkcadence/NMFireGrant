namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resolution
    {
        public Guid ResolutionId { get; set; }

        public Guid? ResolutionType { get; set; }

        [Column("Resolution")]
        [StringLength(2000)]
        public string Resolution1 { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public int Sequence { get; set; }
    }
}
