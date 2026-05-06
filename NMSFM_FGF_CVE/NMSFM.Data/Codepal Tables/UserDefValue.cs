namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDefValue
    {
        public Guid UserDefValueId { get; set; }

        public Guid UserDefFieldId { get; set; }

        public Guid RecordId { get; set; }

        [Column("UserDefValue")]
        [StringLength(3000)]
        public string UserDefValue1 { get; set; }

        public Guid rowguid { get; set; }

        public bool? VActPrint { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(2000)]
        public string CheckValues { get; set; }
    }
}
