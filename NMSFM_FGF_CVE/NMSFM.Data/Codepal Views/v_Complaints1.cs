namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Complaints1
    {
        public DateTime? ComplaintDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string ComplaintType { get; set; }

        [StringLength(50)]
        public string PartyName { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string ComplaintCode { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(50)]
        public string ComplaintStatus { get; set; }

        public DateTime? InspectionDate { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(8000)]
        public string Expr1 { get; set; }

        public bool? Complete { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        public int? NewViolations { get; set; }

        public int? OldViolations { get; set; }

        public int? CorrectedViolations { get; set; }

        public int? ViolationCounts { get; set; }

        [Column(TypeName = "money")]
        public decimal? Fees { get; set; }

        public DateTime? NextInspectionDate { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        public bool? PrimaryParty { get; set; }

        public Guid? ComplaintTypeId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ComplaintId { get; set; }
    }
}
