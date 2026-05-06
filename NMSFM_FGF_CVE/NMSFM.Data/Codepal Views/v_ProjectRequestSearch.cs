namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ProjectRequestSearch
    {
        [StringLength(50)]
        public string ComplaintType { get; set; }

        public DateTime? ComplaintDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid RequestId { get; set; }

        public Guid? CAgencyId { get; set; }

        public Guid? PAgencyId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? ComplaintTypeId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
