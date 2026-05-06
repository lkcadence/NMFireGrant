namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignOffValue")]
    public partial class SignOffValue
    {
        public Guid SignOffValueId { get; set; }

        public Guid RecordId { get; set; }

        public Guid SignOffItemId { get; set; }

        public Guid SignOffUserId { get; set; }

        [Column("SignOffValue")]
        [StringLength(3000)]
        public string SignOffValue1 { get; set; }

        [StringLength(100)]
        public string SignOffCBValues { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
