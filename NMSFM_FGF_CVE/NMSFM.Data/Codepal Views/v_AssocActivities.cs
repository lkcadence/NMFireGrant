namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_AssocActivities
    {
        [Key]
        [Column(Order = 0)]
        public Guid ActivityId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid AssocActivityId { get; set; }

        public Guid? InspectionId { get; set; }

        public Guid? InspectionCauseId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? InspectorId { get; set; }

        public Guid? InspectedPartyId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public bool? Complete { get; set; }

        public Guid? ParentInspectionId { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

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

        [StringLength(204)]
        public string FullAddress { get; set; }

        public Guid? StateId { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        public int? NewViolations { get; set; }

        public int? OldViolations { get; set; }

        public int? CorrectedViolations { get; set; }

        public int? ViolationCounts { get; set; }

        public Guid? AlternatePartyId { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        public bool? PrimaryParty { get; set; }

        [StringLength(150)]
        public string AltPartyName { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        public Guid? ACAgencyId { get; set; }

        public Guid? InspAgencyId { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? RoutingSlipId { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public decimal? Hrs { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? ApprovalStep { get; set; }

        public Guid? ItemId { get; set; }

        public bool? ActInactive { get; set; }

        public bool? TypeInactive { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        public Guid? AcGroupId { get; set; }

        public Guid? SecondaryInspectorId { get; set; }

        [StringLength(50)]
        public string SecondaryInspectorName { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        public Guid? SecAddressId { get; set; }

        public Guid? ItemInspectionStatusId { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string PhoneExt { get; set; }

        [StringLength(20)]
        public string Cell { get; set; }

        [StringLength(20)]
        public string CellExt { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string ItemInspectionStatus { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public int? SubViolations { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }


        [StringLength(8000)]
        public string Comment { get; set; }
    }
}
