namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ItemList
    {
        [Key]
        [Column(Order = 0)]
        public Guid ItemId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

        public Guid? StatusId { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid ItemTypeId { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(75)]
        public string ItemType { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(100)]
        public string ItemNumber { get; set; }

        public DateTime? InServiceDate { get; set; }

        public DateTime? NextServiceDate { get; set; }

        public Guid? ServiceTypeId { get; set; }

        [StringLength(100)]
        public string ServiceType { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Inactive { get; set; }

        public Guid? ActivityTypeId { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        [StringLength(50)]
        public string LocationBase { get; set; }

        [StringLength(50)]
        public string LocationType { get; set; }

        [StringLength(200)]
        public string LocationDescription { get; set; }

        [StringLength(20)]
        public string LocationBarcode { get; set; }

        [StringLength(323)]
        public string Location { get; set; }

        public Guid? ItemCategoryId { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string AgencyName { get; set; }

        public Guid? StateId { get; set; }

        public bool? IsSub { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string AgencySubName { get; set; }


    }
}
