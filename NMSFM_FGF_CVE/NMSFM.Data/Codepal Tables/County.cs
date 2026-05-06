namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class County
    {
        public Guid CountyId { get; set; }

        [Column("County")]
        [Required]
        [StringLength(50)]
        public string County1 { get; set; }

        [StringLength(10)]
        public string CountyCode { get; set; }

        public Guid StateId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
