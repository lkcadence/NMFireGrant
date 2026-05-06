namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Signature
    {
        public Guid SignatureId { get; set; }

        [Column(TypeName = "image")]
        public byte[] FileData { get; set; }

        public Guid? RecordId { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string PrintedName { get; set; }

        public int? Sequence { get; set; }

        public Guid? SignatureTypeId { get; set; }

        public DateTime? DateSigned { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        [Column(TypeName = "xml")]
        public string SignatureData { get; set; }
    }
}
