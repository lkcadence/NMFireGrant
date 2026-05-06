namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeVersionAgency")]
    public partial class CodeVersionAgency
    {
        [Key]
        [Column(Order = 0)]
        public Guid CodeVersionId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid AgencyId { get; set; }

        public Guid rowguid { get; set; }

        public bool Enabled { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public byte? Sequence { get; set; }
    }
}
