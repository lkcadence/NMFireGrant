
namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class v_SearchInspectionDetails
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionDetailId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid InspectionId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ViolationTypeId { get; set; }

        [StringLength(3000)]
        public string Comment { get; set; }

        public DateTime? CorrectedDate { get; set; }

        public bool Severe { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid? InspectionCauseId { get; set; }

        public Guid? AddressId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public DateTime? InspectionTime { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public Guid? InspectionTypeId { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(100)]
        public string ActivityType { get; set; }

        [StringLength(50)]
        public string ActivityCategory { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(50)]
        public string InspectionStatus { get; set; }

        [StringLength(6000)]
        public string ViolationType { get; set; }

        public Guid? CategoryTypeId { get; set; }

        [StringLength(255)]
        public string RefNum { get; set; }

        [StringLength(300)]
        public string CategoryType { get; set; }

        public Guid? CodeVersionId { get; set; }

        [StringLength(100)]
        public string CodeVersion { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        [StringLength(1000)]
        public string CorrectedComments { get; set; }

        public DateTime? ExpCorrDate { get; set; }

        [StringLength(3000)]
        public string CorrectiveAction { get; set; }

        public bool RefOnly { get; set; }

        public DateTime? ViolationDate { get; set; }

        public bool? IsChkVio { get; set; }

        [StringLength(10)]
        public string CodeVersionCode { get; set; }

        [StringLength(10)]
        public string CategoryTypeCode { get; set; }

        [StringLength(10)]
        public string ViolationTypeCode { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        public Guid? ItemId { get; set; }

        public Guid? LocationBaseId { get; set; }

        public Guid? LocationId { get; set; }

        public short? Sequence { get; set; }

        public Guid? ParentInspectionId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [StringLength(50)]
        public string Map { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        [StringLength(50)]
        public string TaxParcel { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(200)]
        public string LocationDescription { get; set; }

        [StringLength(50)]
        public string LocationType { get; set; }

        [StringLength(100)]
        public string ItemDescription { get; set; }

        [StringLength(20)]
        public string ItemStatus { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        public DateTime? ItemInServiceDate { get; set; }

        public DateTime? ItemNextServiceDate { get; set; }

        [StringLength(100)]
        public string ItemServiceType { get; set; }

        [StringLength(50)]
        public string ItemLocationBase { get; set; }

        [StringLength(50)]
        public string ItemLocationType { get; set; }

        [StringLength(200)]
        public string ItemLocationDescription { get; set; }

        public Guid InspectorId { get; set; }
        
        public Guid? GroupId { get; set; }

        public Guid? ACAgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateInserted { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }

        public Guid? SecondaryInspectorId { get; set; }

        public Guid? InspAgencyId { get; set; }

        public Guid? ItemInspectionStatusId { get; set; }

        public bool? Complete { get; set; }

        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string SecondaryInspectorName { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        public bool IsItem { get; set; }

        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(30)]
        public string ProjectNumber { get; set; }
    }
}
