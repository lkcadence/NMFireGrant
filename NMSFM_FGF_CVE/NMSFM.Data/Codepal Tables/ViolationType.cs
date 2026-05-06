namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ViolationType
    {
        public Guid ViolationTypeId { get; set; }

        [Column("ViolationType")]
        [StringLength(6000)]
        public string ViolationType1 { get; set; }

        public Guid CategoryTypeId { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        public Guid rowguid { get; set; }

        public bool Inactive { get; set; }

        public bool Locked { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public short? Sequence { get; set; }
    }
}
