namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SearchAddresses
    {
        [Key]
        [Column(Order = 0)]
        public Guid AddressId { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        public Guid? AddressTypeId { get; set; }

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

        public Guid? CountryId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(10)]
        public string RegionCode { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(10)]
        public string CountyCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(10)]
        public string OccupancyTypeCode { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(10)]
        public string PropertyUseTypeCode { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        public Guid? PartyID { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Inactive { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool DefaultPass { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(2000)]
        public string LegalDesc { get; set; }

        public bool? PAInactive { get; set; }

        public Guid? RoleTypeId { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool POBox { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        [StringLength(50)]
        public string Map { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        [StringLength(50)]
        public string TaxParcel { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime DateInserted { get; set; }
    }
}
