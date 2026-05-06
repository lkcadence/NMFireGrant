namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationBases")]
    public partial class LocationBas
    {
        [Key]
        public Guid LocationBaseId { get; set; }

        [Required]
        [StringLength(50)]
        public string LocationBase { get; set; }

        public Guid? AddressId { get; set; }

        public bool? Inactive { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
