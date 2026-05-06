namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CodeVersion
    {
        public Guid CodeVersionId { get; set; }

        [Column("CodeVersion")]
        [StringLength(100)]
        public string CodeVersion1 { get; set; }

        public Guid rowguid { get; set; }

        public bool Filter { get; set; }

        public bool InActive { get; set; }

        public bool? NonPurchasedCode { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
