namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Address
    {
        public Guid AddressId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        [Column("Address")]
        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? StateId { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? RegionId { get; set; }

        public Guid? CountyId { get; set; }

        public Guid? ParentAddressId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public Guid? AddressTypeId { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        public bool Inactive { get; set; }

        public bool DefaultPass { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [StringLength(2000)]
        public string LegalDesc { get; set; }

        public bool? FromWeb { get; set; }

        public bool? WebAccepted { get; set; }

        public bool? WebRejected { get; set; }

        public Guid? ReportId { get; set; }

        public bool POBox { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        [StringLength(50)]
        public string Map { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        [StringLength(50)]
        public string TaxParcel { get; set; }

        [StringLength(350)]
        public string Schedule { get; set; }

        [NotMapped]
        public DbGeometry SpatialPoint { get; set; }
    }
}
