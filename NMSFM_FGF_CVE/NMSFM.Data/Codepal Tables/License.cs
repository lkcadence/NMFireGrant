namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("License")]
    public partial class License
    {
        public Guid LicenseId { get; set; }

        [StringLength(50)]
        public string Licensee { get; set; }

        [StringLength(500)]
        public string LicenseKey { get; set; }

        public DateTime? LicenseDate { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
