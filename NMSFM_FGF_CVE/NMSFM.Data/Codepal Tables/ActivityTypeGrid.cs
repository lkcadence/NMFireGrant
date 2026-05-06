namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityTypeGrid
    {
        public Guid ActivityTypeGridId { get; set; }

        public Guid ActivityTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string ColumnName { get; set; }

        public int ColumnSequence { get; set; }

        [Required]
        [StringLength(10)]
        public string ColumnType { get; set; }

        public Guid? ColumnKey { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string Externalid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
