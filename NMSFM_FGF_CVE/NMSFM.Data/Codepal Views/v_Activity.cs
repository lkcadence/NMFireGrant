namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Activity
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        public Guid? RecordId { get; set; }

        public Guid? InspectionCauseId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? InspectorId { get; set; }

        public Guid? InspectedPartyId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public DateTime? InspectionTime { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(8000)]
        public string Comment { get; set; }

        public decimal? Hrs { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Complete { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        [StringLength(7000)]
        public string LegalDesc { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? StateId { get; set; }

        public Guid? ZipId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(41)]
        public string PartyPhone { get; set; }

        [Column(TypeName = "money")]
        public decimal? Fees { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ViolationCounts { get; set; }

        public int? TotalViolations { get; set; }

        [Column(TypeName = "image")]
        public byte[] SignatureFileData { get; set; }

        [StringLength(5)]
        public string State { get; set; }

        public Guid? AlternatePartyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(100)]
        public string AddressExtId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string InspectorPhone { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool PrimaryParty { get; set; }

        [StringLength(2000)]
        public string LegalFooter { get; set; }

        public bool? AlwaysPrintProj { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? InvNarrativeId { get; set; }

        public Guid? SecAddressId { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubViolations { get; set; }

        public int? OldSubViolations { get; set; }
    }
}
