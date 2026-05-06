namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mileage")]
    public partial class Mileage
    {
        public Guid MileageId { get; set; }

        public Guid PartyId { get; set; }

        public DateTime MileageDate { get; set; }

        public short Miles { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
