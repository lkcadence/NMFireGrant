namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ComplaintPermits
    {
        [Key]
        [Column(Order = 0)]
        public Guid ComplaintPermitId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ComplaintId { get; set; }

        public Guid? PermitId { get; set; }

        public Guid? PermitTypeId { get; set; }

        [StringLength(50)]
        public string PermitNumber { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public Guid? IssuedToPartyId { get; set; }

        public bool? Complete { get; set; }

        public Guid? PermitStatusId { get; set; }

        public Guid? IssuingOfficerId { get; set; }

        [StringLength(100)]
        public string PermitType { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        [StringLength(50)]
        public string PermitStatus { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        public bool? HasFees { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
