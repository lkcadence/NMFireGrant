namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Memorandums")]
    public partial class Memorandum
    {
        public Guid MemorandumId { get; set; }

        [Column("Memorandum")]
        [StringLength(150)]
        public string Memorandum1 { get; set; }

        public string Description { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
