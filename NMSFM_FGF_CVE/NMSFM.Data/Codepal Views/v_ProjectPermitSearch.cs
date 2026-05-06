namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectPermitSearch
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid PermitId { get; set; }

        public Guid? PermitTypeId { get; set; }

        [StringLength(50)]
        public string PermitNumber { get; set; }

        [StringLength(100)]
        public string PermitType { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? PAgencyId { get; set; }

        public Guid? PerAgencyId { get; set; }

        [StringLength(2500)]
        public string Comment { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
