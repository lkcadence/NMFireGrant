namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignOffUser")]
    public partial class SignOffUser
    {
        public Guid SignOffUserId { get; set; }

        public Guid SignOffTypeId { get; set; }

        public Guid UserId { get; set; }

        public bool LockRecord { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
