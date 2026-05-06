namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvNarrativeText")]
    public partial class InvNarrativeText
    {
        [Key]
        public Guid InvNarrativeId { get; set; }

        [Column("InvNarrativeText")]
        [StringLength(8000)]
        public string InvNarrativeText1 { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
