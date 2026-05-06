namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CategoryType
    {
        public Guid CategoryTypeId { get; set; }

        [Column("CategoryType")]
        [StringLength(300)]
        public string CategoryType1 { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public Guid? CodeVersionId { get; set; }

        public Guid rowguid { get; set; }

        public bool Inactive { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool WebViewable { get; set; }

        public short? Sequence { get; set; }
    }
}
