namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectAddressSearch
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        public Guid? AddressId { get; set; }

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

        public Guid? StateId { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        public Guid? AddressTypeId { get; set; }

        public Guid? ZipId { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        public Guid? AgencyId { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool AlertAddress { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
