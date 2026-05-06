namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReportDefinition
    {
        [Key]
        public Guid ReportDefId { get; set; }

        public Guid? ReportId { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportName { get; set; }

        [StringLength(100)]
        public string BaseReport { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ReportLayout { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
