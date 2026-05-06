namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReportSQL")]
    public partial class ReportSQL
    {
        public Guid ReportSQLId { get; set; }

        public Guid ReportId { get; set; }

        [Required]
        [StringLength(50)]
        public string TableName { get; set; }

        [Required]
        [StringLength(5000)]
        public string SQLString { get; set; }

        [StringLength(100)]
        public string KeyField { get; set; }

        public Guid? UDFCategoryId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
