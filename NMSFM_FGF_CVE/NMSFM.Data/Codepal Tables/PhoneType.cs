namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PhoneType
    {
        public Guid PhoneTypeId { get; set; }

        [Column("PhoneType")]
        [Required]
        [StringLength(25)]
        public string PhoneType1 { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public short Sequence { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
