namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Certifications
    {
        [Key]
        [Column(Order = 0)]
        public Guid CertificationId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid RecordId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid CertificationTypeId { get; set; }

        [StringLength(100)]
        public string CertificationType { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime CertificationDate { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }

        public DateTime? CertificationEndDate { get; set; }

        public Guid? IssueingPartyId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string CertNumber { get; set; }

        public bool? UserCertification { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
