namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ActivityComplaints
    {
        [Key]
        public Guid ActivityId { get; set; }

        public Guid? ComplaintId { get; set; }

        public DateTime? ComplaintDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string ComplaintType { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        [StringLength(220)]
        public string FullAddress { get; set; }

        [StringLength(50)]
        public string ComplaintCode { get; set; }

        [StringLength(50)]
        public string ComplaintStatus { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateInserted { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
