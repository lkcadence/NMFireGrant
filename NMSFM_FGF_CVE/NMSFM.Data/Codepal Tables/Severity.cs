namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Severity")]
    public partial class Severity
    {
        [Key]
        public Guid SeverityLevelId { get; set; }

        [Required]
        [StringLength(50)]
        public string SeverityLevel { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        public short? Age { get; set; }

        public Guid? AgencyId { get; set; }

        public bool PrintBlack { get; set; }

        public Guid rowguid { get; set; }

        public int? ARGB { get; set; }

        public bool DisableActivity { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }
    }
}
