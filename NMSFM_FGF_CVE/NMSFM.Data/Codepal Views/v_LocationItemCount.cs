namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_LocationItemCount
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

        public int? ItemCount { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        [StringLength(20)]
        public string Barcode { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Inactive { get; set; }

        public Guid? LocationBaseId { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
