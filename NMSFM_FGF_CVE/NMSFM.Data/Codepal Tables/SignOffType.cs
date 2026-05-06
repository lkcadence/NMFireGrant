namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignOffType")]
    public partial class SignOffType
    {
        public Guid SignOffTypeId { get; set; }

        public Guid ObjectTypeId { get; set; }

        [StringLength(50)]
        public string TabText { get; set; }

        [StringLength(1000)]
        public string EmailText { get; set; }

        [StringLength(1000)]
        public string ReEmailText { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
