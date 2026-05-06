namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ArchiveDB
    {
        [Key]
        public Guid ArchiveId { get; set; }

        [Required]
        [StringLength(50)]
        public string ArchiveName { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? Build { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
