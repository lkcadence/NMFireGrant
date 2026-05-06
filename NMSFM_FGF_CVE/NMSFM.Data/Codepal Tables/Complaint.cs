namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Complaint
    {
        public Guid ComplaintId { get; set; }

        public Guid? ComplaintTypeId { get; set; }

        public Guid? ComplainingPartyId { get; set; }

        public DateTime? ComplaintDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public Guid? AddressId { get; set; }

        public Guid rowguid { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public bool? FromWeb { get; set; }

        public Guid? ReportId { get; set; }

        [StringLength(50)]
        public string ComplaintCode { get; set; }

        public Guid? ComplaintStatusId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? ApprovalStep { get; set; }
    }
}
