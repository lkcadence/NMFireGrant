namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Preplans
    {
        [Key]
        [Column(Order = 0)]
        public Guid AddressId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public Guid? ParentAddressId { get; set; }

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

        [StringLength(200)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Inactive { get; set; }

        [StringLength(2000)]
        public string LegalDesc { get; set; }
    }
}
