namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuditField
    {
        public Guid AuditFieldId { get; set; }

        public Guid AuditId { get; set; }

        [StringLength(100)]
        public string ControlName { get; set; }

        [StringLength(100)]
        public string FieldDesc { get; set; }

        [StringLength(36)]
        public string OldId { get; set; }

        [StringLength(3000)]
        public string OldValue { get; set; }

        [StringLength(36)]
        public string NewId { get; set; }

        [StringLength(3000)]
        public string NewValue { get; set; }

        public Guid rowguid { get; set; }
    }
}
