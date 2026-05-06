namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Report
    {
        public Guid ReportId { get; set; }

        [Required]
        [StringLength(10)]
        public string ReportCode { get; set; }

        [StringLength(100)]
        public string ReportTitle { get; set; }

        [StringLength(2000)]
        public string ReportDesc { get; set; }

        [StringLength(50)]
        public string ReportFile { get; set; }

        public bool InConsole { get; set; }

        public Guid rowguid { get; set; }

        public Guid? TypeId { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        [StringLength(50)]
        public string TypeDef { get; set; }

        public bool IsSubReport { get; set; }

        public Guid? ReportGroupId { get; set; }

        public bool DefaultReport { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public bool IsWizardReport { get; set; }
    }
}
