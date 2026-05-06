namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ItemGrid
    {
        [Key]
        [Column(Order = 0)]
        public Guid InspectionId { get; set; }

        public Guid? ItemId { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Complete { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime? InServiceDate { get; set; }

        public Guid? ParentItemId { get; set; }

        public decimal? Cost { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        public DateTime? NextServiceDate { get; set; }

        public Guid? ItemTypeId { get; set; }

        public Guid? StatusId { get; set; }

        public Guid? InspectionCauseId { get; set; }

        public Guid? InspectorId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(220)]
        public string Address { get; set; }

        public Guid? InspectedPartyId { get; set; }

        public DateTime? InspectionDate { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public decimal? Hrs { get; set; }

        public Guid? AlternatePartyId { get; set; }

        public Guid? ParentInspectionId { get; set; }

        public Guid? ItemInspectionStatusId { get; set; }

        [StringLength(50)]
        public string ActivityCategory { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(220)]
        public string SecAddress { get; set; }

        public Guid? AlternatePartyRoleTypeId { get; set; }

        public Guid? InspectedPartyRoleTypeId { get; set; }

        public Guid? GroupId { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? SecondaryInspectorId { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(100)]
        public string ActivityType { get; set; }

        public Guid? InvItemId { get; set; }

        [Column("Suggested Replacement")]
        public Guid? Suggested_Replacement { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? LocationBaseId { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }
    }
}
