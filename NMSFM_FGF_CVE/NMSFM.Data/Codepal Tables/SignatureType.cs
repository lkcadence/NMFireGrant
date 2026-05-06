namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SignatureType
    {
        public Guid SignatureTypeId { get; set; }

        [Column("SignatureType")]
        [Required]
        [StringLength(150)]
        public string SignatureType1 { get; set; }

        public bool PreserveOnReopen { get; set; }

        public bool Inactive { get; set; }

        [StringLength(40)]
        public string ModuleId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(6000)]
        public string SignatureLegalText { get; set; }

        public Guid? AgencyId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool WebViewable { get; set; }
    }
}
