namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReportDataDefinition")]
    public partial class ReportDataDefinition
    {
        [Key]
        public Guid ReportDataDefId { get; set; }

        public Guid ReportId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ReportDataDef { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
