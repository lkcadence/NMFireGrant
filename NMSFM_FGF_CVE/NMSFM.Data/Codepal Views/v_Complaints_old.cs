namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Complaints_old
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

        [StringLength(50)]
        public string PartyName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(220)]
        public string FullAddress { get; set; }

        public Guid? InspectionId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Complete { get; set; }

        [StringLength(50)]
        public string ComplaintCode { get; set; }
    }
}
