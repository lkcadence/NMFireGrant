namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Locations
    {
        [Key]
        [Column(Order = 0)]
        public Guid LocationId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid AddressId { get; set; }

        public Guid? LocationTypeId { get; set; }

        [StringLength(50)]
        public string LocationType { get; set; }

        [StringLength(20)]
        public string Barcode { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Inactive { get; set; }

        public Guid? LocationBaseId { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime DateInserted { get; set; }
    }
}
