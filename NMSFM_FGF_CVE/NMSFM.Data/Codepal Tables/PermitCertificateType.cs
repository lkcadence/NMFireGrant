namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PermitCertificateType
    {
        public Guid PermitCertificateTypeId { get; set; }

        [Column("PermitCertificateType")]
        [Required]
        [StringLength(50)]
        public string PermitCertificateType1 { get; set; }

        [Required]
        [StringLength(7000)]
        public string LegalDesc { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(1000)]
        public string FooterText { get; set; }

        public Guid? ReportId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
