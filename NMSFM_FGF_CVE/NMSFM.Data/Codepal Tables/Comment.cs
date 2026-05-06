namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public Guid CommentId { get; set; }

        public Guid RecordId { get; set; }

        [Column("Comment")]
        [StringLength(8000)]
        public string Comment1 { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool PlainText { get; set; }
    }
}
