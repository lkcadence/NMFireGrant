namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectAddress")]
    public partial class ProjectAddress
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid AddressId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool AlertAddress { get; set; }
    }
}
