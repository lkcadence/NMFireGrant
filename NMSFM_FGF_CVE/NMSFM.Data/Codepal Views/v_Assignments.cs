namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Assignments
    {
        [Key]
        [Column(Order = 0)]
        public Guid AddressId { get; set; }

        public Guid? ParentAddressId { get; set; }

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

        public Guid? CountryId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid InspectionTypeId { get; set; }

        [StringLength(100)]
        public string InspectionType { get; set; }

        public Guid? InspectorId { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public Guid? ActivityTypeId { get; set; }

        public Guid? AgencyId { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Inactive { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? GroupId { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }
    }
}
