namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Signature
    {
        [Key]
        public Guid SignatureId { get; set; }

        [Column(TypeName = "image")]
        public byte[] FileData { get; set; }

        public Guid? RecordId { get; set; }

        [StringLength(100)]
        public string PrintedName { get; set; }

        public int? Sequence { get; set; }

        public Guid? SignatureTypeId { get; set; }

        [StringLength(150)]
        public string SignatureType { get; set; }

        public bool? PreserveOnReopen { get; set; }

        public bool? Inactive { get; set; }

        [StringLength(40)]
        public string ModuleId { get; set; }

        [StringLength(6000)]
        public string SignatureLegalText { get; set; }

        public Guid? AgencyId { get; set; }

        public DateTime? DateSigned { get; set; }
    }
}
