namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Files
    {
        [Key]
        public Guid FileID { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        [StringLength(200)]
        public string FileDesc { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        public Guid? RecordId { get; set; }

        public bool Linked { get; set; }

        public short? SeqNum { get; set; }

        public bool AdminViewOnly { get; set; }

    }
}
