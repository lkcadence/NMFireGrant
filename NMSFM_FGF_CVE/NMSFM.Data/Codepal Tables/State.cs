namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class State
    {
        public Guid StateId { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [Column("State")]
        [Required]
        [StringLength(50)]
        public string State1 { get; set; }

        public Guid? CountryId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
