namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SearchComplaintUDFs
    {
        [Key]
        [Column(Order = 0)]
        public Guid ComplaintId { get; set; }

        [StringLength(50)]
        public string ComplaintType { get; set; }

        public DateTime? ComplaintDate { get; set; }

        [StringLength(50)]
        public string PartyName { get; set; }

        public bool? Complete { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public Guid? ComplainingPartyId { get; set; }
    }
}
