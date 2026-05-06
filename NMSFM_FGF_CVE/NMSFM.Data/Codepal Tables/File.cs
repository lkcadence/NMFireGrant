namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class File
    {
        public Guid FileId { get; set; }

        [StringLength(150)]
        public string FileName { get; set; }

        [StringLength(200)]
        public string FileDesc { get; set; }

        [StringLength(400)]
        public string FilePath { get; set; }

        [Column(TypeName = "image")]
        public byte[] FileData { get; set; }

        public bool Linked { get; set; }

        public Guid? RecordId { get; set; }

        public Guid rowguid { get; set; }

        public short? SeqNum { get; set; }

        public bool AdminViewOnly { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
