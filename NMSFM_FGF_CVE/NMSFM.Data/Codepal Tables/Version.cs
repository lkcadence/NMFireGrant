namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Version")]
    public partial class Version
    {
        [Key]
        public Guid AppId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public Guid? ProductCode { get; set; }

        public short? Major { get; set; }

        public short? Minor { get; set; }

        public short? Revision { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
