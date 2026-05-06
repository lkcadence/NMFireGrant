namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_LocationsCount
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
    }
}
