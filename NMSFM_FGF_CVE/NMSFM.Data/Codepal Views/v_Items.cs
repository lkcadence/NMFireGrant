namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Items
    {
        [Key]
        [Column(Order = 0)]
        public Guid ItemId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Description { get; set; }

        public Guid? ItemTypeId { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? StatusId { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        public decimal? Cost { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        public DateTime? InServiceDate { get; set; }

        public DateTime? NextServiceDate { get; set; }

        public Guid? ServiceTypeId { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Inactive { get; set; }

        public Guid? ActivityTypeId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(50)]
        public string LocationType { get; set; }

        public Guid? ItemCategoryId { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        public Guid? InvItemId { get; set; }

        public Guid? LocationBaseId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
