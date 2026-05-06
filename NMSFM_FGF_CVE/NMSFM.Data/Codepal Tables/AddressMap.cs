namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AddressMap")]
    public partial class AddressMap
    {
        [Key]
        public Guid AddressId { get; set; }

        [Column(TypeName = "image")]
        public byte[] MapData { get; set; }

        public int? Zoom { get; set; }

        [StringLength(20)]
        public string Style { get; set; }

        public bool? LatLon { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
