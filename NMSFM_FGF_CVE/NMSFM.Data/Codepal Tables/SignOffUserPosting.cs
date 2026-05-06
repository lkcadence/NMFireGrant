namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignOffUserPosting")]
    public partial class SignOffUserPosting
    {
        public Guid SignOffUserPostingId { get; set; }

        public Guid SignOffUserId { get; set; }

        public Guid RecordId { get; set; }

        public bool Complete { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
