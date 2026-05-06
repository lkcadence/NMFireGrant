namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDefField
    {
        public Guid UserDefFieldId { get; set; }

        public Guid UserDefCategoryId { get; set; }

        [Required]
        [StringLength(150)]
        public string FieldDesc { get; set; }

        public short? SeqNum { get; set; }

        public Guid UserDefTypeId { get; set; }

        public Guid? GlobalId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool FieldEncrypted { get; set; }

        public bool StaticCombo { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }

        public bool Required { get; set; }

        public bool ViewGrid { get; set; }

        [StringLength(3000)]
        public string DefaultValue { get; set; }
    }
}
