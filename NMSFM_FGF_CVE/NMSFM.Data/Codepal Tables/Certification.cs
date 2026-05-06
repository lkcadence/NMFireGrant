namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Certification
    {
        public Guid CertificationId { get; set; }

        public Guid RecordId { get; set; }

        public Guid CertificationTypeId { get; set; }

        public DateTime CertificationDate { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }

        public DateTime? CertificationEndDate { get; set; }

        public Guid? IssueingPartyId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string CertNumber { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
