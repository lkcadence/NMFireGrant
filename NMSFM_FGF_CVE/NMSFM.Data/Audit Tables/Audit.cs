namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Audit
    {
        public Guid AuditId { get; set; }

        public Guid SessionId { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        public Guid? RecordId { get; set; }

        [StringLength(250)]
        public string AuditAction { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public DateTime? DateStamp { get; set; }

        public Guid rowguid { get; set; }

        //for future user
        //[Column(TypeName = "xml")]
        //public string RecordData { get; set; }
    }
}
