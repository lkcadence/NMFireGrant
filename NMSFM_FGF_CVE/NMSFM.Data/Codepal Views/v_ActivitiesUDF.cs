namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ActivitiesUDF
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        [StringLength(50)]
        public string ActivityCategory { get; set; }

        [StringLength(100)]
        public string ActivityType { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public DateTime? InspectionDate { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(150)]
        public string AltPartyName { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public decimal? Hrs { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Complete { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NewViolations { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OldViolations { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CorrectedViolations { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ViolationCounts { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubViolations { get; set; }

        public int? OldSubViolations { get; set; }
    }
}
