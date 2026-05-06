namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectType
    {
        public Guid ProjectTypeId { get; set; }

        [Column("ProjectType")]
        [Required]
        [StringLength(50)]
        public string ProjectType1 { get; set; }

        public Guid rowguid { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? ReportId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool Inactive { get; set; }

        public bool WebViewable { get; set; }

        public short ProjectFreq { get; set; }

        [StringLength(1)]
        public string Recurrance { get; set; }

        public bool NoAlert { get; set; }
    }
}
