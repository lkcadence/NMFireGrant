namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Phone")]
    public partial class Phone
    {
        public Guid PhoneId { get; set; }

        public Guid PhoneTypeId { get; set; }

        [Column("Phone")]
        [Required]
        [StringLength(20)]
        public string Phone1 { get; set; }

        [StringLength(20)]
        public string Extension { get; set; }

        public Guid PartyId { get; set; }

        public int? Sequence { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
