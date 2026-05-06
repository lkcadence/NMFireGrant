namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Zip
    {
        public Guid ZipId { get; set; }

        [Column("Zip")]
        [Required]
        [StringLength(15)]
        public string Zip1 { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
