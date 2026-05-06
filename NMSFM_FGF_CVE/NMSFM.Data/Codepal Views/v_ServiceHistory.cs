namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ServiceHistory
    {
        [Key]
        [Column(Order = 0)]
        public Guid ServiceHistoryId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ItemId { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime ServiceDate { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid ServiceTypeId { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime DateInserted { get; set; }
    }
}
