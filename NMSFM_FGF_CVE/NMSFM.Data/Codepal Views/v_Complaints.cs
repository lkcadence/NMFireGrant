namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Complaints
    {
        [Key]
        [Column(Order = 0)]
        public Guid ComplaintId { get; set; }

        public Guid? ComplaintTypeId { get; set; }

        public DateTime? ComplaintDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public Guid? ComplainingPartyId { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(50)]
        public string ComplaintType { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string ComplaintCode { get; set; }

        [StringLength(100)]
        public string AddressType { get; set; }

        [StringLength(50)]
        public string AddressCode { get; set; }

        [StringLength(50)]
        public string AddressNumber { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string SubAddress { get; set; }

        [StringLength(15)]
        public string Suffix { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        [StringLength(50)]
        public string PropertyUseType { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public Guid? ComplaintStatusId { get; set; }

        [StringLength(50)]
        public string ComplaintStatus { get; set; }

        public Guid? ReportId { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime DateUpdated { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime DateInserted { get; set; }

        public bool? Inactive { get; set; }

        public bool? WebViewable { get; set; }

        public Guid? ApprovalStep { get; set; }

        [StringLength(50)]
        public string Direction { get; set; }

        public bool? FromWeb { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
