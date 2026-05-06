namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Alerts
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        public Guid? InspectionTypeId { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        public Guid? AddressId { get; set; }

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

        public Guid? ZipId { get; set; }

        public Guid? StateId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        public Guid? InspectedPartyId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        public DateTime? InspectionDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime NextInspectionDate { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public Guid? ParentInspectionId { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(150)]
        public string AltPartyName { get; set; }

        public Guid? AlternatePartyId { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool PrimaryParty { get; set; }

        public Guid? ActivityId { get; set; }

        public Guid? ItemId { get; set; }

        public Guid? AgencyId { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(50)]
        public string InspectionCause { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? InspectionCauseId { get; set; }

        public Guid? GroupId { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }
    }
}
