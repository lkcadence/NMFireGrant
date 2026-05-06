namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Note
    {
        public Guid NoteId { get; set; }

        public Guid RecordId { get; set; }

        public DateTime NoteDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Column("Note")]
        [StringLength(5000)]
        public string Note1 { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(150)]
        public string ObjectRef { get; set; }

        public bool? WebViewable { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public short? Pinned { get; set; }
    }
}
