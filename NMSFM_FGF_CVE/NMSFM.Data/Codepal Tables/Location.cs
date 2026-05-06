namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Location
    {
        public Guid LocationId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public Guid LocationTypeId { get; set; }

        [StringLength(20)]
        public string Barcode { get; set; }

        public Guid AddressId { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool Inactive { get; set; }

        public Guid? LocationBaseId { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
